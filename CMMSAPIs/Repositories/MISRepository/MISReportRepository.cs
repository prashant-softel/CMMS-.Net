using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System.Collections.Generic;
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
            string facility = $"Select f.name as sitename, f.state as state,f.city as district,bus1.name as contractor_name,bus.name AS contractor_site_incahrge_name,spv1.name as spv_name from facilities as f  left join business as bus on bus.id=f.ownerId left join business as bus1 on bus.id=f.operatorId left join spv as spv1 on spv1.id=f.spvId where f.id={facility_id} group by f.id;";
            List<ProjectDetails> data = await Context.GetData<ProjectDetails>(facility).ConfigureAwait(false);

            string hfedat = $"SELECT  CONCAT(YEAR(e.Date), '-', LPAD(MONTH(e.Date), 2, '0')) AS YearMonth,      COUNT( e.employee_id) AS AvgHFEEmployee,      COUNT(e.employee_id) AS ManDaysHFEEmployee,      COUNT(DISTINCT e.Date) AS ManHoursWorkedHFEEmployee,     COUNT(e.employee_id) * 8 AS ManHoursWorkedHFEEmployee FROM      employee_attendance AS e LEFT JOIN      facilities AS f ON f.id = e.facility_id LEFT JOIN      (SELECT DISTINCT Date FROM contractor_attendnace) AS cd ON cd.Date = e.Date     where e.present=1 and facility_id={facility_id} GROUP BY   f.id,  YEAR(e.Date), MONTH(e.Date);";
            List<ManPowerData> hfedata = await Context.GetData<ManPowerData>(hfedat).ConfigureAwait(false);

            string occupationaldata = $"select NoOfHealthExamsOfNewJoiner,PeriodicTests,OccupationaIllnesses from mis_occupationalhealthdata where facility_id={facility_id}  group by month_id; ";
            List<OccupationalHealthData> occupational_Helath = await Context.GetData<OccupationalHealthData>(occupationaldata).ConfigureAwait(false);

            string visitnotice = $"SELECT GovtAuthVisits,NoOfFineByThirdParty,NoOfShowCauseNoticesByThirdParty,NoticesToContractor,NoticesToContractor, AmountOfPenaltiesToContractors,AnyOther FROM mis_visitsandnotices where facility_id={facility_id}  group by month_id;";
            List<VisitsAndNotices> Regulatory_visit_notice = await Context.GetData<VisitsAndNotices>(visitnotice).ConfigureAwait(false);

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
               $" where  waste_data.plantId = {facility_id} and DATE_FORMAT(Date,'%Y') = '{year}' and mw.status=1 " +
               $" group by MONTH(month_id) , waste_data.wasteTypeId  " +
               $"  ;";

            List<CMWaterDataMonthWise> ListResult_waste_data = await Context.GetData<CMWaterDataMonthWise>(SelectQ_waste_data).ConfigureAwait(false);


            return new List<MISSUMMARY>();
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
