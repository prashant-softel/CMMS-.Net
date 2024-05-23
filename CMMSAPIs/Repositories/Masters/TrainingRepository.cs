using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Masters
{
    public class TrainingRepository : GenericRepository

    {


        private UtilsRepository _utilsRepo;
        public static Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        private MYSQLDBHelper getDB;
        private ErrorLog m_errorLog;

        public const string MA_Actual = "MA_Actual";
        public const string MA_Contractual = "MA_Contractual";
        public const string Internal_Grid = "Internal_Grid";
        public const string External_Grid = "External_Grid";
        private Dictionary<CMMS.CMMS_Modules, int> module_dict = new Dictionary<CMMS.CMMS_Modules, int>()
        {
            { CMMS.CMMS_Modules.DASHBOARD, 1 },
            { CMMS.CMMS_Modules.JOB, 2 },
            { CMMS.CMMS_Modules.PTW, 3 },
            { CMMS.CMMS_Modules.JOBCARD, 4 },
            { CMMS.CMMS_Modules.CHECKLIST_NUMBER, 5 },
            { CMMS.CMMS_Modules.CHECKPOINTS, 6 },
            { CMMS.CMMS_Modules.CHECKLIST_MAPPING, 7 },
            { CMMS.CMMS_Modules.PM_SCHEDULE, 8 },
            { CMMS.CMMS_Modules.PM_SCEHDULE_VIEW, 9 },
            { CMMS.CMMS_Modules.PM_EXECUTION, 10 },
            { CMMS.CMMS_Modules.PM_SCHEDULE_REPORT, 11 },
            { CMMS.CMMS_Modules.PM_SUMMARY, 12 },
            { CMMS.CMMS_Modules.SM_MASTER, 13 },
            { CMMS.CMMS_Modules.SM_GO, 14 },
            { CMMS.CMMS_Modules.SM_MRS, 15 },
            { CMMS.CMMS_Modules.SM_MRS_RETURN, 16 },
            { CMMS.CMMS_Modules.SM_S2S, 17 },
            { CMMS.CMMS_Modules.AUDIT_PLAN, 18 },
            { CMMS.CMMS_Modules.AUDIT_SCHEDULE, 19 },
            { CMMS.CMMS_Modules.AUDIT_SCEHDULE_VIEW, 20 },
            { CMMS.CMMS_Modules.AUDIT_EXECUTION, 21 },
            { CMMS.CMMS_Modules.AUDIT_SUMMARY, 22 },
            { CMMS.CMMS_Modules.HOTO_PLAN, 23 },
            { CMMS.CMMS_Modules.HOTO_SCHEDULE, 24 },
            { CMMS.CMMS_Modules.HOTO_SCEHDULE_VIEW, 25 },
            { CMMS.CMMS_Modules.HOTO_EXECUTION, 26 },
            { CMMS.CMMS_Modules.HOTO_SUMMARY, 27 },
            { CMMS.CMMS_Modules.INVENTORY, 28 },
            { CMMS.CMMS_Modules.WARRANTY_CLAIM, 30 },
            { CMMS.CMMS_Modules.CALIBRATION, 31 },
           // { CMMS.CMMS_Modules.MODULE_CLEANING, 32 },
            { CMMS.CMMS_Modules.VEGETATION, 33 }
        };


        public TrainingRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment _webHost = null) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(_webHost);
            getDB = sqlDBHelper;
        }

        internal async Task<CMDefaultResponse> CreateTrainingCourse(CMTrainingCourse request, int userID)
        {
            string ctq = $"INSERT INTO course (Topic,facility_id, Description, Traning_category_id, No_Of_Days, Max_capacity, Targated_group_id, Duration_in_Minutes,Status,CreatedAt,CreatedBy) Values" +
                       $"('{request.name}',{request.facility_id},'{request.description}',{request.category_id},{request.number_of_days},{request.max_cap},{request.group_id},{request.duration},1,'{UtilsRepository.GetUTCTime()}',{userID}) ; " +
                       $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(ctq).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Course Added Successfully.");
        }

        internal async Task<List<CMTrainingCourse>> GetCourseList(int facility_id)
        {
            string gcl = $" SELECT id as id, facility_id as facility_id,Topic as name, Description as description ,Traning_category_id as category_id , No_Of_Days as number_of_days, Max_capacity as max_cap, Targated_group_id as group_id, Duration_in_Minutes as duration ,Status as status, CreatedAt  , CreatedBy  , UpdateAt  , UpdateBy from course where facility_id={facility_id}";
            List<CMTrainingCourse> result = await Context.GetData<CMTrainingCourse>(gcl).ConfigureAwait(false);
            return result;
        }

        internal async Task<CMDefaultResponse> UpdateCourseList(CMTrainingCourse request, int userID)
        {
            string ctq = $"UPDATE course SET " +
             $"Topic = '{request.name}', " +
             $"facility_id = {request.facility_id}, " +
             $"Description = '{request.description}', " +
             $"Training_category_id = {request.category_id}, " +
             $"No_Of_Days = {request.number_of_days}, " +
             $"Max_capacity = {request.max_cap}, " +
             $"Targeted_group_id = {request.group_id}, " +
             $"Duration_in_Minutes = {request.duration}, " +
             $"Status = 1, " +
             $"UpdatedAt = {UtilsRepository.GetUTCTime()}, " +
             $"UreatedBy = {userID} " +
             $"WHERE  id={request.id};";
            DataTable dt = await Context.FetchData(ctq).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Course Updated Successfully.");
        }
        internal async Task<CMDefaultResponse> DeleteCourseList(int id, int userID)
        {
            string deleteQry = $"UPDATE course " +
               $" SET Status = 0 , updatedBy = {userID} , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Course Deleted Successfully.");
        }


        internal async Task<CMDefaultResponse> CreateScheduleCourse()
        {
            return null;
        }


        internal async Task<CMDefaultResponse> GetScheduleCourse()
        {
            return null;
        }

        internal async Task<CMDefaultResponse> ExecuteScheduleCourse()
        {
            return null;
        }

    }

}


