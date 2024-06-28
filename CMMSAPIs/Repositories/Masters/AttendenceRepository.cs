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
            string delete = $"DELETE FROM employee_attendance WHERE Date='{date}';";
            await Context.ExecuteNonQry<int>(delete).ConfigureAwait(false);
            string deletecontractor = $"DELETE FROM contractor_attendnace WHERE Date='{date}';";
            await Context.ExecuteNonQry<int>(deletecontractor).ConfigureAwait(false);
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

        internal async Task<object> GetAttendanceByDetails(int facility_id, DateTime dates)
        {
            DateTime Dates = dates;
            string detail = $"SELECT  facility_id AS Facility_Id " +
                                           $" FROM employee_attendance where facility_id ={facility_id};";
            List<CMGETAttendenceDETAIL> Detail = await Context.GetData<CMGETAttendenceDETAIL>(detail).ConfigureAwait(false);

            string getEmployeeAttendance = $"SELECT employee_attendance.id AS Id, attendance_id AS Attendance_Id, facility_id AS Facility_Id, employee_id AS Employee_Id,concat(u.firstName,u.lastName) as name, present AS Present, in_time AS In_Time, out_time AS Out_Time,(DATE_FORMAT(Date ,'%Y-%m-%d')) as Dates " +
                                           $" FROM employee_attendance left join users as u on u.id=employee_attendance.employee_id  WHERE (DATE(Date) = '{dates.ToString("yyyy-MM-dd")} ') AND facility_id ={facility_id};";
            List<CMGetAttendence1> employeeAttendanceList = await Context.GetData<CMGetAttendence1>(getEmployeeAttendance).ConfigureAwait(false);

            string getContractorAttendance = $"SELECT id AS Id, facility_id AS Facility_Id,(DATE_FORMAT(Date ,'%Y-%m-%d')) as Date, contractor_id AS Contractor_Id, age_lessthan_35 AS Age_Less_Than35, age_Between_35_50 AS Age_Between_35And50, age_Greater_50 AS Age_Greater50, Purpose as Purpose " +
                                            $" FROM contractor_attendnace WHERE (DATE(contractor_attendnace.Date) = '{dates.ToString("yyyy-MM-dd")} ') and contractor_attendnace.facility_id ={facility_id};";
            var contractAttendances = await Context.GetDataFirst<CMGetCotractor1>(getContractorAttendance).ConfigureAwait(false);

            var hfeAttendance = employeeAttendanceList.Select(a => new CMGetAttendence1
            {
                // id = a.id,
                employee_id = a.employee_id,
                name = a.name,
                present = a.present == 1 ? true : false,
                In_Time = a.In_Time,
                Out_Time = a.Out_Time,
            }).ToList();
            var response = new
            {
                date = dates.ToString("yyyy-MM-dd"),
                facility_id = facility_id,
                hfeAttendance,
                contractAttendances
            };
            return response;
        }

        internal async Task<List<CMGetAttendenceList>> GetAttendanceList(int facility_id, int year)
        {
            string getattendence = "SELECT cc.Date  AS dates,cc.facility_id,DAY(cc.Date) AS day_id,MONTH(cc.Date) AS month_id,MONTHNAME(cc.Date) AS month_name, " +
                " YEAR(cc.Date) AS years FROM employee_attendance AS cc " +
                $" WHERE  YEAR(cc.Date) ={year} and facility_id={facility_id} GROUP BY  cc.Date, cc.facility_id ORDER BY  cc.Date;";
            List<CMGetAttendenceList> employeeAttendanceList = await Context.GetData<CMGetAttendenceList>(getattendence).ConfigureAwait(false);
            string getmonth = "SELECT  cc.Date as date, COALESCE(cs.age_Between_35_50) AS age_Between_35_50, COALESCE(cs.age_lessthan_35) AS age_lessthan_35, " +
                $"COALESCE(cs.age_Greater_50) AS age_Greater_50, COUNT(cc.employee_id) AS hfe_employees FROM employee_attendance AS cc " +
                $" LEFT JOIN ( SELECT Date, SUM(age_Between_35_50) AS age_Between_35_50,SUM(age_lessthan_35) AS age_lessthan_35,SUM(age_Greater_50) AS age_Greater_50 " +
                $" FROM contractor_attendnace   GROUP BY  Date) AS cs ON  cc.Date = cs.Date  WHERE  YEAR(cc.Date) ={year} " +
                $" and facility_id={facility_id} GROUP BY  cc.Date, cc.facility_id ORDER BY  cc.Date;";
            List<MonthData> MONTHList = await Context.GetData<MonthData>(getmonth).ConfigureAwait(false);

            var groupedResult = employeeAttendanceList
         .GroupBy(x => new { x.facility_id, x.month_id, x.month_name, x.years })
         .Select(g => new CMGetAttendenceList
         {
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

        }

        //        internal async Task<object> GetAttendanceByDetailsByMonth(int facility_id, DateTime from_date, DateTime to_date)
        //        {
        //            string querybymonth = $"select facility_id ,f.name as facility_name from employee_attendance left join facilities f on f.id=employee_attendance.facility_id;";
        //            var employeeAttendanceList = await Context.GetDataFirst<CMGETAttendenceMONTH>(querybymonth).ConfigureAwait(false);

        //            string queryByMonthEmp = "SELECT ea.employee_id as employeeId, CONCAT(u.firstName, u.lastname) AS employeeName, u.joiningDate as dateOfJoining, " +
        //                                     " ea.Date AS date, ea.present AS status, u.dateofExit as DateofExit, ea.in_time  as inTime , " +
        //                                     "ea.out_time AS outTime , CASE WHEN u.dateofExit < NOW() THEN 'Active' ELSE 'Inactive' END AS workingStatus " +
        //                                     $"FROM employee_attendance AS ea LEFT JOIN users AS u ON u.id = ea.employee_id " +
        //                                     $"WHERE ea.facility_id = {facility_id}  or ea.Date between'{from_date}' and ea.Date = '{to_date}' order by ea.Date or employee_id ;";

        //            List<EmployeeMonth> empDetails = await Context.GetData<EmployeeMonth>(queryByMonthEmp).ConfigureAwait(false);

        //            string queryByEmp = $"SELECT ea.employee_id as emp_id, ea.Date AS date, ea.present AS status, ea.in_time as inTime ," +
        //                                $" ea.out_time AS outTime " +
        //                                $" FROM employee_attendance AS ea LEFT JOIN users AS u ON u.id = ea.employee_id " +
        //                                $" WHERE ea.facility_id ={facility_id} or ea.Date between'{from_date}' and ea.Date = '{to_date}' order by ea.Date;";

        //            List<DetailsOFMonth> EmpAteendence = await Context.GetData<DetailsOFMonth>(queryByEmp).ConfigureAwait(false);


        //            var result = empDetails.Select(a => new EmployeeMonth
        //            {
        //                employeeId = a.employeeId,
        //                employeeName = a.employeeName,
        //                dateOfJoining = a.dateOfJoining,
        //                DateofExit = a.DateofExit,
        //                workingStatus = a.workingStatus,

        //                details = EmpAteendence.Where(x => x.emp_id == a.employeeId).Select(b => new DetailsOFMonth
        //                {
        //                    emp_id = b.emp_id,
        //                    date = b.date,
        //                    inTime = b.inTime,
        //                    outTime = b.outTime,
        //                    status = b.status
        //                }).ToList()
        //            }).ToList();

        //            var response = new
        //            {
        //                facility_id = facility_id,
        //                facility_name = employeeAttendanceList.facility_name,
        //                result

        //            };
        //            return response;

        //        }
        public async Task<object> GetAttendanceByDetailsByMonth(int facility_id, DateTime from_date, DateTime to_date)
        {
            string querybymonth = "SELECT DISTINCT facility_id, f.name as facility_name FROM employee_attendance LEFT JOIN facilities f ON f.id = employee_attendance.facility_id;";
            var employeeAttendanceList = await Context.GetDataFirst<CMGETAttendenceMONTH>(querybymonth).ConfigureAwait(false);

            string queryByMonthEmp = "SELECT DISTINCT ea.employee_id as employeeId, CONCAT(u.firstName, u.lastname) AS employeeName, u.joiningDate as dateOfJoining, " +
                                     "u.dateofExit as DateofExit, CASE WHEN u.dateofExit < NOW() THEN 'Active' ELSE 'Inactive' END AS workingStatus " +
                                     $"FROM employee_attendance AS ea LEFT JOIN users AS u ON u.id = ea.employee_id " +
                                     $"WHERE ea.facility_id = {facility_id} OR (ea.Date BETWEEN '{from_date}' AND '{to_date}') ORDER BY ea.employee_id;";

            List<EmployeeMonth> empDetails = await Context.GetData<EmployeeMonth>(queryByMonthEmp).ConfigureAwait(false);

            string queryByEmp = $"SELECT ea.employee_id as emp_id, ea.Date AS date, ea.present AS status, ea.in_time as inTime, ea.out_time AS outTime " +
                                $"FROM employee_attendance AS ea LEFT JOIN users AS u ON u.id = ea.employee_id " +
                                $"WHERE ea.facility_id = {facility_id} OR (ea.Date BETWEEN '{from_date}' AND '{to_date}') ORDER BY ea.Date;";

            List<DetailsOFMonth> EmpAteendence = await Context.GetData<DetailsOFMonth>(queryByEmp).ConfigureAwait(false);

            var result = empDetails.Select(a => new EmployeeMonth
            {
                employeeId = a.employeeId,
                employeeName = a.employeeName,
                dateOfJoining = a.dateOfJoining,
                DateofExit = a.DateofExit,
                workingStatus = a.workingStatus,
                details = EmpAteendence.Where(x => x.emp_id == a.employeeId).ToList()
            }).ToList();

            var response = new
            {
                facility_id = facility_id,
                facility_name = employeeAttendanceList.facility_name,
                result = result
            };

            return response;
        }

    }
}
