using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Masters
{
    public class AttendenceRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public static Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        private MYSQLDBHelper getDB;
        private ErrorLog m_errorLog;
        public AttendenceRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment _webHost = null) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(_webHost);
            getDB = sqlDBHelper;
        }
        internal async Task<CMDefaultResponse> CreateAttendance(CMCreateAttendence requests, int userID)
        {
            int employeeValue = 0;
            int fid = 0;
            string date = requests.date.ToString("yyyy-MM-dd");
            CMDefaultResponse response = new CMDefaultResponse();
            //Employee ATTENDENCE


            foreach (CMGetAttendence request in requests.hfeAttendance)
            {


                string AttenQuery = $"INSERT INTO employee_attendance ( attendance_id, facility_id, employee_id, present, in_time, out_time,Date, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES " +
               $"({request.Attendance_Id}, {requests.facility_id}, '{request.employee_id}', {request.present}, '{request.InTime}','{request.OutTime}','{date}','{UtilsRepository.GetUTCTime()}',{userID},'{UtilsRepository.GetUTCTime()}',{userID}); " +
               $"SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(AttenQuery).ConfigureAwait(false);
                employeeValue = Convert.ToInt32(dt2.Rows[0][0]);
            }
            response = new CMDefaultResponse(employeeValue, CMMS.RETRUNSTATUS.SUCCESS, "Attendence Mark successfully.");
            //Contractor Ateendence

            CMGetCotractor requst = requests.contractAttendance;


            requst.contractor_id = 0;
            string contQuery = $"INSERT INTO contractor_attendnace (facility_id, Date, contractor_id, age_lessthan_35, age_Between_35_50 , age_Greater_50, purpose) VALUES " +
               $"( {requests.facility_id},'{date}', {requst.contractor_id}, {requst.lessThan35}, {requst.between35to50} ,{requst.greaterThan50},'{requst.purpose}'); " +
               $"SELECT LAST_INSERT_ID();";
            DataTable dt3 = await Context.FetchData(contQuery).ConfigureAwait(false);
            int contractValue = Convert.ToInt32(dt3.Rows[0][0]);

            return response;
        }

        internal async Task<object> GetAttendanceByDetails(int facility_id, DateTime date)
        {
            DateTime Dates = date;
            string employeeFilter = " WHERE (DATE(ea.Date) >= '" + date.ToString("yyyy-MM-dd") + "')";
            employeeFilter += " AND ea.facility_id = " + facility_id;

            string getEmployeeAttendance = $"SELECT ea.id AS Id, ea.attendance_id AS Attendance_Id, ea.facility_id AS Facility_Id, ea.employee_id AS Employee_Id, ea.present AS Present, ea.in_time AS In_Time, ea.out_time AS Out_Time,(DATE_FORMAT(ea.Date ,'%Y-%m-%d')) as Dates " +
                                           $"FROM employee_attendance ea {employeeFilter};";


            List<CMGetAttendence> employeeAttendanceList = await Context.GetData<CMGetAttendence>(getEmployeeAttendance).ConfigureAwait(false);

            foreach (CMGetAttendence cmg in employeeAttendanceList)
            {
                Dates = Convert.ToDateTime(cmg.Dates);

                if (cmg.present == 1)
                {
                    cmg.present = true;
                }
                else
                {
                    cmg.present = false;
                }

            }

            var hfeAttendance = employeeAttendanceList.Select(ea => new
            {
                id = ea.id,
                Facility_Id = ea.facility_id,
                Attendance_Id = ea.Attendance_Id,
                Employee_Id = ea.employee_id,
                present = ea.present,
                inTime = ea.InTime,
                outTime = ea.OutTime
            }).ToList();

            string contractorFilter = " WHERE (DATE(contractor_attendnace.Date) >= '" + date.ToString("yyyy-MM-dd") + "')";
            contractorFilter += " AND contractor_attendnace.facility_id = " + facility_id;

            string getContractorAttendance = $"SELECT id AS Id, facility_id AS Facility_Id,(DATE_FORMAT(Date ,'%Y-%m-%d')) as Date, contractor_id AS Contractor_Id, age_lessthan_35 AS Age_Less_Than35, age_Between_35_50 AS Age_Between_35And50, age_Greater_50 AS Age_Greater50, Purpose as Purpose " +
                                             $"FROM contractor_attendnace {contractorFilter};";

            var contractAttendances = await Context.GetData<CMGetCotractor>(getContractorAttendance).ConfigureAwait(false);


            var response = new
            {
                Date = Dates.ToString("yyyy-MM-dd"),
                hfeAttendance,
                contractAttendances
            };

            return response;
        }


        internal async Task<CMDefaultResponse> UpdateAttendance(CMCreateAttendence requests, int userID)
        {
            CMDefaultResponse response = new CMDefaultResponse();


            int employeeValue = 0;
            foreach (CMGetAttendence request in requests.hfeAttendance)
            {

                string instimp = $"Update  employee_attendance SET attendance_id={request.Attendance_Id}, facility_id={request.facility_id}, employee_id={request.employee_id}, present={request.present}, in_time= '{request.InTime}', out_time,Date=,'{request.OutTime}', CreatedAt='{UtilsRepository.GetUTCTime()}', CreatedBy={userID}, UpdatedAt='{UtilsRepository.GetUTCTime()}', UpdatedBy={userID} where date={request.Dates} ;";
                await Context.ExecuteNonQuery(instimp).ConfigureAwait(false);

            }
            //Contractor Ateendence
            CMGetCotractor requst = requests.contractAttendance;
            string contupdt = $"Update  contractor_attendnace SET facility_id={requests.facility_id},contractor_id={requst.contractor_id}, age_lessthan_35={requst.lessThan35}, age_Between_35_50={requst.between35to50}, age_Greater_50={requst.greaterThan50} , purpose='{requst.purpose}' where Date={requests.date} ;";
            await Context.ExecuteNonQuery(contupdt).ConfigureAwait(false);
            response = new CMDefaultResponse(employeeValue, CMMS.RETRUNSTATUS.SUCCESS, "Attendence Updated successfully.");
            return response;
        }

        internal async Task<List<CMGetAttendenceList>> GetAttendanceList(int facility_id, int year)
        {
            string getattendence = "SELECT  cc.Date AS date,cc.facility_id,DAY(cc.Date) AS day_id,MONTH(cc.Date) AS month_id,MONTHNAME(cc.Date) AS month_name, " +
                " YEAR(cc.Date) AS years FROM employee_attendance AS cc " +
                $" WHERE  YEAR(cc.Date) ={year} and facility_id={facility_id} GROUP BY  cc.Date, cc.facility_id ORDER BY  cc.Date;";
            List<CMGetAttendenceList> employeeAttendanceList = await Context.GetData<CMGetAttendenceList>(getattendence).ConfigureAwait(false);
            string getmonth = "SELECT cc.Date as date, COALESCE(cs.age_Between_35_50) AS age_Between_35_50, COALESCE(cs.age_lessthan_35) AS age_lessthan_35, " +
                $"COALESCE(cs.age_Greater_50) AS age_Greater_50, COUNT(cc.employee_id) AS hfe_employees FROM employee_attendance AS cc " +
                $" LEFT JOIN ( SELECT Date, SUM(age_Between_35_50) AS age_Between_35_50,SUM(age_lessthan_35) AS age_lessthan_35,SUM(age_Greater_50) AS age_Greater_50 " +
                $" FROM contractor_attendnace   GROUP BY  Date) AS cs ON  cc.Date = cs.Date  WHERE  YEAR(cc.Date) ={year} " +
                $" and facility_id={facility_id} GROUP BY  cc.Date, cc.facility_id ORDER BY  cc.Date;";
            List<MonthData> MONTHList = await Context.GetData<MonthData>(getmonth).ConfigureAwait(false);

            var groupedResult = employeeAttendanceList
         .GroupBy(x => new { x.dates, x.facility_id, x.month_id, x.month_name, x.years })
         .Select(g => new CMGetAttendenceList
         {
             dates = g.Key.dates,
             facility_id = g.Key.facility_id,
             month_id = g.Key.month_id,
             month_name = g.Key.month_name,
             years = g.Key.years,
             month_data = MONTHList.Where(m => m.date.Month == g.Key.month_id && m.date.Year == g.Key.years).Select(g => new MonthData
             {
                 date = g.date,
                 hfe_employees = g.hfe_employees,
                 age_Between_35_50 = g.age_Between_35_50,
                 age_lessthan_35 = g.age_lessthan_35,
                 age_Greater_50 = g.age_Greater_50,
             }).ToList()
         }).ToList();
            return groupedResult;
            /*
                        List<CMGetAttendenceList> groupedResult = employeeAttendanceList.GroupBy(x => new { x.dates, x.facility_id, x.month_id, x.month_name, x.years }).Select(group => new CMGetAttendenceList
                        {
                            facility_id = group.facility_id,
                            month_name = group.month_name,
                            month_id = (int)group.month_id,
                            years = (int)group.years,

                            month_data = MONTHList.Select(g => new MonthData
                            {
                                date = g.date,
                                hfe_employees = g.hfe_employees,
                                age_Between_35_50 = g.age_Between_35_50,
                                age_lessthan_35 = g.age_lessthan_35,
                                age_Greater_50 = g.age_Greater_50,

                            }).ToList()
                        }).ToList();
                        return groupedResult;
                        /*
                        return new Dictionary<string, object>
                {
                    { "facility_id", groupedData.facility_id },
                    { "month_id", groupedData.month_id },
                    { "month_name", groupedData.month_name },
                    { "years", groupedData.years },
                    { "month_data", groupedData.month_data }
                };*/
        }
    }
}
