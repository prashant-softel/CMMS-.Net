using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.NewFolder
{
    public class MISReportRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public static Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        private MYSQLDBHelper getDB;
        private ErrorLog m_errorLog;
        public MISReportRepository(MYSQLDBHelper sqlDbHelper) : base(sqlDbHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDbHelper);
            getDB = sqlDbHelper;

        }
        internal async Task<List<MISSUMMARY>> GetMisSummary(string year, int facility_id)
        {
            string facility = $"Select f.name as sitename, f.state as state,f.city as district,f.city as  talukMandal,bus1.name as contractorName,bus2.name as hfeSiteInChargeName, bus.name AS contractorSiteInChargeName,spv1.name as spv_name from facilities as f  left join business as bus on bus.id=f.ownerId  left join business as bus1 on bus1.id=f.operatorId  left join business as bus2 on bus2.id=f.customerId   left join spv as spv1 on spv1.id=f.spvId where f.id={facility_id} group by f.id;";
            ProjectDetails data = await Context.GetDataFirst<ProjectDetails>(facility).ConfigureAwait(false);

            string hfedat = $"SELECT   YEAR(e.Date) AS year, month( e.Date) as month_id, monthname(e.Date) as month,  COUNT(distinct e.employee_id) AS AvgHFEEmployee,  COUNT(distinct e.employee_id) AS ManDaysHFEEmployee,COUNT( distinct e.employee_id) * 8 AS ManHoursWorkedHFEEmployee,cd1.age_lessthan_35 + cd1.age_Between_35_50+ cd1.age_Greater_50 AS avgContractorWorkers, (cd1.age_lessthan_35 + cd1.age_Between_35_50+ cd1.age_Greater_50 )*8  as   manHoursWorked,       (COUNT(distinct e.employee_id) * 8) + ((cd1.age_lessthan_35 + cd1.age_Between_35_50 + cd1.age_Greater_50) * 8) AS totalManHours   FROM      employee_attendance AS e LEFT JOIN      facilities AS f ON f.id = e.facility_id LEFT JOIN      (SELECT DISTINCT Date FROM contractor_attendnace) AS cd ON cd.Date = e.Date  LEFT JOIN contractor_attendnace AS cd1 ON cd1.Date = e.Date    where e.present=1 and e.facility_id={facility_id} GROUP BY   f.id, MONTH(e.Date);";
            List<ManPowerData> hfedata = await Context.GetData<ManPowerData>(hfedat).ConfigureAwait(false);

            string occupationaldata = $"select   year as  year,  month_id as month_id,NoOfHealthExamsOfNewJoiner,PeriodicTests,OccupationaIllnesses from mis_occupationalhealthdata where facility_id={facility_id}  group by month_id; ";
            List<OccupationalHealthData> occupational_Helath = await Context.GetData<OccupationalHealthData>(occupationaldata).ConfigureAwait(false);

            string visitnotice = $"SELECT  year year,  month_id as month_id, GovtAuthVisits,NoOfFineByThirdParty,NoOfShowCauseNoticesByThirdParty,NoticesToContractor,NoticesToContractor, AmountOfPenaltiesToContractors,AnyOther FROM mis_visitsandnotices where facility_id={facility_id}  group by month_id;";
            List<VisitsAndNotices> Regulatory_visit_notice = await Context.GetData<VisitsAndNotices>(visitnotice).ConfigureAwait(false);

            //string incident = $"SELECT FatalIncidents, LostTimeInjuries, MedicalTreatmentInjuries, FirstAidIncidents, FireIncidents,NearMisses,ManDaysLost,ManDaysLost_After_incident,CostOfAccidents FROM incidents WHERE facility_id={facility_id};";
            string incident = $"SELECT IFNULL(YEAR(ins.incident_datetime), 0) AS year, IFNULL(MONTH(ins.incident_datetime), 0) AS month_id, " +
                              $"IFNULL(MONTHNAME(ins.incident_datetime), 0) AS month, SUM( instype.incidenttype = 'FATAL') AS FatalIncidents, " +
                              $"SUM(instype.incidenttype = 'LTI' ) AS LostTimeInjuries, SUM( instype.incidenttype = 'MTI' ) AS MedicalTreatmentInjuries, " +
                              $"SUM( instype.incidenttype = 'First Aid') AS FirstAidIncidents, SUM( instype.incidenttype = 'Fire' ) AS FireIncidents, " +
                              $"SUM( instype.incidenttype = 'Near Miss') AS NearMisses FROM incidenttype AS instype " +
                              $"LEFT JOIN incidents AS ins ON ins.risk_type = instype.id " +
                              $" GROUP BY     YEAR(ins.incident_datetime),    " +
                              $" MONTH(ins.incident_datetime) ORDER BY  year, month_id;";
            List<IncidentAccidentData> incidentAccidentData = await Context.GetData<IncidentAccidentData>(incident).ConfigureAwait(false);

            // Fetch HSE Training Data
            string trainingdata = $"SELECT totalTrainings, trainingManHours, mockDrillsConducted, specialTrainingsConducted FROM mis_trainingdata WHERE facility_id={facility_id};";
            List<HseTrainingData> hseTrainingData = await Context.GetData<HseTrainingData>(trainingdata).ConfigureAwait(false);
            /*
            // Fetch HSE Inspection and Audit Data
            string auditdata = ""; //$"SELECT observationsRaised, observationsClosed, majorObservationsRaised, majorObservationsClosed FROM mis_audit WHERE facility_id={facility_id};";
            List<HseInspectionAuditData> hseInspectionAuditData = await Context.GetData<HseInspectionAuditData>(auditdata).ConfigureAwait(false);

            // Fetch Report Checklist Data
            string checklistdata = "";//$"SELECT reportsToBeInspected, reportsInspectedInMonth, reportsNotInspected FROM mis_checklists WHERE facility_id={facility_id};";
            List<ReportChecklistData> reportChecklistData = await Context.GetData<ReportChecklistData>(checklistdata).ConfigureAwait(false);

            // Fetch Grievance Data
            string grievancedata = "";// $"SELECT totalGrievancesRaised, grievancesResolved, workforceGrievancesPending, localCommunityGrievancesResolved FROM mis_grievances WHERE facility_id={facility_id};";
            List<GrievanceData> grievanceData = await Context.GetData<GrievanceData>(grievancedata).ConfigureAwait(false);
            */


            string SelectQ = $" select distinct mis_waterdata.id,plantId as facility_id,fc.name facility_name,MONTHNAME(date) as month_name,Month(date) as month_id,YEAR(date) as year, " +
            $"sum(creditQty) as procured_qty, sum(debitQty) as consumed_qty, mw.name as water_type,mw.show_opening" +
            $" from mis_waterdata" +
            $" LEFT JOIN facilities fc ON fc.id = mis_waterdata.plantId" +
            $" LEFT JOIN mis_watertype mw on mw.id = mis_waterdata.waterTypeId" +
            $" where  mis_waterdata.plantId = {facility_id} and DATE_FORMAT(Date,'%Y') = '{year}'" +
            $" group by MONTH(month_id) , mis_waterdata.waterTypeId  " +
            $"  ;";
            List<CMWaterDataMonthWise> ListResult = await Context.GetData<CMWaterDataMonthWise>(SelectQ).ConfigureAwait(false);

            // waste data
            string SelectQ_waste_data = $" select distinct waste_data.id, facilityId as facility_id,fc.name facility_name,MONTHNAME(date) as month_name,Month(date) as month_id,YEAR(date) as year, " +
            $" sum(creditQty) as procured_qty, sum(debitQty) as consumed_qty, mw.name as water_type, show_opening " +
            $" from waste_data" +
            $" LEFT JOIN facilities fc ON fc.id = waste_data.facilityId" +
            $" LEFT JOIN mis_wastetype mw on mw.id = waste_data.wasteTypeId" +
            $" where  waste_data.facilityId = {facility_id} and DATE_FORMAT(Date,'%Y') = '{year}' and mw.status=1 " +
            $" group by MONTH(month_id) , waste_data.wasteTypeId  " +
            $"  ;";
            List<CMWaterDataMonthWise> ListResult_waste_data = await Context.GetData<CMWaterDataMonthWise>(SelectQ_waste_data).ConfigureAwait(false);
            var allMonths = Enumerable.Range(1, 12).ToList();
            var monthlyData = allMonths.Select(month => new MapAuditlist
            {
                year = year,
                month_id = month,
                month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                manPowerData = hfedata.Where(h => h.month_id == month).ToList(),
                waterData = ListResult.Where(w => w.month_id == month).ToList(),
                WasteData = ListResult_waste_data.Where(w => w.month_id == month).ToList(),
                incidentAccidentData = incidentAccidentData.Where(i => i.month_id == month).ToList(),
                /*hseTrainingData = hseTrainingData.Where(t => t.month_id == month).ToList(), 
                hseInspectionAuditData = hseInspectionAuditData.Where(a => a.month_id == month).ToList(),
                reportChecklistData = reportChecklistData.Where(c => c.month_id == month).ToList(),
                grievanceData = grievanceData.Where(g => g.month_id == month).ToList(), */
                healthDatas = occupational_Helath.Where(o => o.month_id == month).ToList(),
                visitsAndNotices = Regulatory_visit_notice.Where(v => v.month_id == month).ToList()
            })
            .OrderBy(md => md.month_id) // Order by month_id to ensure the data is in correct sequence
            .ToList();
            var summary = new List<MISSUMMARY>
            {
                new MISSUMMARY
                {
                    year = year,
                    projectdetails = data,
                    MonthlyData = monthlyData
                }
            };

            return summary;
        }

        internal async Task<List<EnviromentalSummary>> GeEnvironmentalSummary(string year, int facility_id)
        {
            string facility = $"Select name as facilty_name from facilities where  id={facility_id};";
            List<EnviromentalSummary> data = await Context.GetData<EnviromentalSummary>(facility).ConfigureAwait(false);
            string occupationaldata = $"select NoOfHealthExamsOfNewJoiner,PeriodicTests,OccupationaIllnesses from mis_occupationalhealthdata where facility_id={facility_id}  group by month_id; ";
            List<OccupationalHealthData> occupational_Helath = await Context.GetData<OccupationalHealthData>(occupationaldata).ConfigureAwait(false);

            string visitnotice = $"SELECT GovtAuthVisits,NoOfFineByThirdParty,NoOfShowCauseNoticesByThirdParty,NoticesToContractor,NoticesToContractor, AmountOfPenaltiesToContractors,AnyOther FROM mis_visitsandnotices where facility_id={facility_id} group by month_id;";
            List<VisitsAndNotices> Regulatory_visit_notice = await Context.GetData<VisitsAndNotices>(visitnotice).ConfigureAwait(false);

            List<EnviromentalSummary> projectDetailsList = new List<EnviromentalSummary>();

            foreach (var item in data)
            {
                EnviromentalSummary projectDetail = new EnviromentalSummary
                {
                    healthDatas = occupational_Helath,
                    visitsAndNotices = Regulatory_visit_notice
                };
                projectDetailsList.Add(projectDetail);
            }
            return projectDetailsList;
        }

    }
}
