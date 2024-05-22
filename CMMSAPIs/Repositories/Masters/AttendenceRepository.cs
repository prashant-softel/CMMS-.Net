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
            CMDefaultResponse response = new CMDefaultResponse();
            //Employee ATTENDENCE
            foreach (CMGetAttendence request in requests.hfeAttendance)
            {
                fid = request.Facility_Id;
                string AttenQuery = $"INSERT INTO employee_attendance ( attendance_id, facility_id, employee_id, present, in_time, out_time,Date, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy) VALUES " +
               $"({request.Attendance_Id}, {fid}, '{request.Employee_Id}', {request.Present}, '{request.In_Time}','{request.Out_Time}','{requests.Date.ToString("yyyy - MM - dd")}','{UtilsRepository.GetUTCTime()}',{userID},'{UtilsRepository.GetUTCTime()}',{userID}); " +
               $"SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(AttenQuery).ConfigureAwait(false);
                employeeValue = Convert.ToInt32(dt2.Rows[0][0]);
            }
            response = new CMDefaultResponse(employeeValue, CMMS.RETRUNSTATUS.SUCCESS, "Attendence Mark successfully.");
            //Contractor Ateendence

            CMGetCotractor requst = requests.contractAttendance;


            requst.Contractor_Id = 0;
            string contQuery = $"INSERT INTO contractor_attendnace (facility_id, Date, contractor_id, age_lessthan_35, age_Between_35_50 , age_Greater_50, purpose) VALUES " +
               $"( {fid},'{requests.Date.ToString("yyyy - MM - dd")}', {requst.Contractor_Id}, {requst.Age_Less_Than35}, {requst.Age_Between_35And50} ,{requst.Age_Greater50},'{requst.Purpose}'); " +
               $"SELECT LAST_INSERT_ID();";
            DataTable dt3 = await Context.FetchData(contQuery).ConfigureAwait(false);
            int contractValue = Convert.ToInt32(dt3.Rows[0][0]);

            return response;
        }

        internal async Task<object> GetAttendanceList(int facility_id, DateTime from_date, DateTime to_date)
        {
            string employeeFilter = " WHERE (DATE(ea.Date) >= '" + from_date.ToString("yyyy-MM-dd") + "' AND DATE(ea.Date) <= '" + to_date.ToString("yyyy-MM-dd") + "')";
            employeeFilter += " AND ea.facility_id = " + facility_id;

            string getEmployeeAttendance = $"SELECT ea.id AS Id, ea.attendance_id AS Attendance_Id, ea.facility_id AS Facility_Id, ea.employee_id AS Employee_Id, ea.present AS Present, ea.in_time AS In_Time, ea.out_time AS Out_Time, ea.Date " +
                                           $"FROM employee_attendance ea {employeeFilter};";


            List<CMGetAttendence> employeeAttendanceList = await Context.GetData<CMGetAttendence>(getEmployeeAttendance).ConfigureAwait(false);

            foreach (CMGetAttendence cmg in employeeAttendanceList)
            {
                if (cmg.Present == 1)
                {
                    cmg.Present = true;
                }
                else
                {
                    cmg.Present = false;
                }

            }

            var hfeAttendance = employeeAttendanceList.Select(ea => new
            {
                id = ea.Id,
                Facility_Id = ea.Facility_Id,
                Attendance_Id = ea.Attendance_Id,
                Employee_Id = ea.Employee_Id,
                present = ea.Present,
                inTime = ea.In_Time,
                outTime = ea.Out_Time
            }).ToList();

            string contractorFilter = " WHERE (DATE(contractor_attendnace.Date) >= '" + from_date.ToString("yyyy-MM-dd") + "' AND DATE(contractor_attendnace.Date) <= '" + to_date.ToString("yyyy-MM-dd") + "')";
            contractorFilter += " AND contractor_attendnace.facility_id = " + facility_id;

            string getContractorAttendance = $"SELECT id AS Id, facility_id AS Facility_Id, Date, contractor_id AS Contractor_Id, age_lessthan_35 AS Age_Less_Than35, age_Between_35_50 AS Age_Between_35And50, age_Greater_50 AS Age_Greater50, Purpose as Purpose " +
                                             $"FROM contractor_attendnace {contractorFilter};";

            var contractAttendances = await Context.GetData<CMGetCotractor>(getContractorAttendance).ConfigureAwait(false);

            var response = new
            {
                date = from_date,
                hfeAttendance,
                contractAttendances
            };

            return response;
        }


        internal async Task<CMDefaultResponse> UpdateAttendance(CMCreateAttendence request)
        {
            return null;
        }

    }
}
