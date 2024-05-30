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
                fid = request.Facility_Id;
                string AttenQuery = $"INSERT INTO employee_attendance ( attendance_id, facility_id, employee_id, present, in_time, out_time,Date, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES " +
               $"({request.Attendance_Id}, {fid}, '{request.Employee_Id}', {request.present}, '{request.InTime}','{request.OutTime}','{date}','{UtilsRepository.GetUTCTime()}',{userID},'{UtilsRepository.GetUTCTime()}',{userID}); " +
               $"SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(AttenQuery).ConfigureAwait(false);
                employeeValue = Convert.ToInt32(dt2.Rows[0][0]);
            }
            response = new CMDefaultResponse(employeeValue, CMMS.RETRUNSTATUS.SUCCESS, "Attendence Mark successfully.");
            //Contractor Ateendence

            CMGetCotractor requst = requests.contractAttendance;


            requst.Contractor_Id = 0;
            string contQuery = $"INSERT INTO contractor_attendnace (facility_id, Date, contractor_id, age_lessthan_35, age_Between_35_50 , age_Greater_50, purpose) VALUES " +
               $"( {fid},'{date}', {requst.Contractor_Id}, {requst.lessThan35}, {requst.between35to50} ,{requst.greaterThan50},'{requst.purpose}'); " +
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
                Facility_Id = ea.Facility_Id,
                Attendance_Id = ea.Attendance_Id,
                Employee_Id = ea.Employee_Id,
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
            string queryemp = $"Delete From employee_attendance where Date={requests.date} ;";
            await Context.ExecuteNonQry<int>(queryemp).ConfigureAwait(false);

            string querycon = $"Delete From contractor_attendnace where Date={requests.date} ;";
            await Context.ExecuteNonQry<int>(querycon).ConfigureAwait(false);
            int fid = 0;
            int employeeValue = 0;
            foreach (CMGetAttendence request in requests.hfeAttendance)
            {
                fid = request.Facility_Id;
                string instimp = $"INSERT INTO employee_attendance ( attendance_id, facility_id, employee_id, present, in_time, out_time,Date, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES " +
               $"({request.Attendance_Id}, {fid}, '{request.Employee_Id}', {request.present}, '{request.InTime}','{request.OutTime}','{requests.date}','{UtilsRepository.GetUTCTime()}',{userID},'{UtilsRepository.GetUTCTime()}',{userID}); " +
               $"SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(instimp).ConfigureAwait(false);
                employeeValue = Convert.ToInt32(dt2.Rows[0][0]);
            }
            response = new CMDefaultResponse(employeeValue, CMMS.RETRUNSTATUS.SUCCESS, "Attendence Updated successfully.");

            //Contractor Ateendence
            CMGetCotractor requst = requests.contractAttendance;
            requst.Contractor_Id = 0;
            string contupdt = $"INSERT INTO contractor_attendnace (facility_id, Date, contractor_id, age_lessthan_35, age_Between_35_50 , age_Greater_50, purpose) VALUES " +
               $"( {fid},'{requests.date}', {requst.Contractor_Id}, {requst.lessThan35}, {requst.between35to50}  , {requst.greaterThan50} ,' {requst.purpose}'); " +
               $"SELECT LAST_INSERT_ID();";
            DataTable dt3 = await Context.FetchData(contupdt).ConfigureAwait(false);
            int contractValue = Convert.ToInt32(dt3.Rows[0][0]);

            return response;
        }

    }
}
