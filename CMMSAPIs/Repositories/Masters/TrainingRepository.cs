﻿using CMMSAPIs.Helper;
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

            if (request.uploadfile_ids != null && request.uploadfile_ids.Count > 0)
            {
                foreach (int data in request.uploadfile_ids)
                {

                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facility_id},description='{request.description}', module_type={(int)CMMS.CMMS_Modules.TRAINING_COURSE},module_ref_id={id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Course Added Successfully.");
        }

        internal async Task<List<CMTrainingCourse>> GetCourseList(int facility_id, DateTime from_date, DateTime to_date)
        {
            string From = Convert.ToDateTime(from_date).ToString("yyyy-MM-dd");
            string to = Convert.ToDateTime(to_date).ToString("yyyy-MM-dd");
            string gcl = $" SELECT c.id as id, c.facility_id as facility_id,c.Topic as name, c.Description as description ,c.Traning_category_id as category_id,cs.name as Traning_category , c.No_Of_Days as number_of_days, c.Max_capacity as max_cap ," +
                $" c.Targated_group_id as group_id ,tg.name as Targated_group, " +
                $" c.Duration_in_Minutes as duration ,c.Status as status,c.CreatedAt, c.CreatedBy  , c.UpdatedAt  , c.UpdatedBy from course as c " +
                $" Left join course_category as cs on cs.id=c.Traning_category_id " +
                $" Left join targeted_group  as tg on tg.id=c.Targated_group_id " +
                $" where c.facility_id={facility_id} and c.Status=1 or CreatedAt='{From}' or CreatedAt='{to}'  ";
            List<CMTrainingCourse> result = await Context.GetData<CMTrainingCourse>(gcl).ConfigureAwait(false);



            return result;
        }
        internal async Task<List<CMTrainingCourse>> GetCourseDetailById(int ids)
        {
            string gcl = $" SELECT c.id as Id, c.facility_id as facility_id,c.Topic as name, c.Description as description ," +
                         $" c.Traning_category_id as category_id ,cs.name as Traning_category , c.No_Of_Days as number_of_days, c.Max_capacity as max_cap, " +
                         $" c.Targated_group_id as group_id,tg.name as Targated_group, c.Duration_in_Minutes as duration ,c.Status as status, c.CreatedAt  , c.CreatedBy  , c.UpdatedAt  , c.UpdatedBy from course as c " +
                         $" Left join course_category as cs on cs.id=c.Traning_category_id " +
                         $" Left join targeted_group  as tg on tg.id=c.Targated_group_id " +
                         $" where c.id={ids} and c.Status=1 ";
            List<CMTrainingCourse> result = await Context.GetData<CMTrainingCourse>(gcl).ConfigureAwait(false);

            string myQuery17 = "SELECT cor.id as id, file_path as fileName,U.description FROM uploadedfiles AS U " +
                             "Left JOIN course as cor on cor.id= U.module_ref_id" +
                             " where module_ref_id =" + ids + " and U.module_type = " + (int)CMMS.CMMS_Modules.TRAINING_COURSE + ";";
            List<CMTRAININGFILE> _fileUpload = await Context.GetData<CMTRAININGFILE>(myQuery17).ConfigureAwait(false);
            result[0].ImageDetails = _fileUpload;
            return result;
        }

        internal async Task<CMDefaultResponse> UpdateCourseList(CMTrainingCourse request, int userID)
        {
            string ctq = $"UPDATE course SET " +
             $"Topic = '{request.name}', " +
             $"facility_id = {request.facility_id}, " +
             $"Description = '{request.description}', " +
             $"Traning_category_id= {request.category_id}, " +
             $"No_Of_Days = {request.number_of_days}, " +
             $"Max_capacity = {request.max_cap}, " +
             $"Targated_group_id = {request.group_id}, " +
             $"Duration_in_Minutes = {request.duration}, " +
             $"Status = 1, " +
             $"UpdatedAt = '{UtilsRepository.GetUTCTime()}', " +
             $"UpdatedBy = {userID} " +
             $"WHERE  id={request.Id};";
            await Context.ExecuteNonQry<int>(ctq).ConfigureAwait(false);
            if (request.uploadfile_ids != null && request.uploadfile_ids.Count > 0)
            {
                foreach (int data in request.uploadfile_ids)
                {

                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facility_id}, module_type={(int)CMMS.CMMS_Modules.TRAINING_COURSE},module_ref_id={request.Id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            return new CMDefaultResponse(request.Id, CMMS.RETRUNSTATUS.SUCCESS, "Course Updated Successfully.");
        }
        internal async Task<CMDefaultResponse> DeleteCourseList(int id, int userID)
        {
            string deleteQry = $"UPDATE course " +
               $" SET Status = 0 , UpdatedBy = {userID} , UpdatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Course Deleted Successfully.");
        }


        internal async Task<CMDefaultResponse> CreateScheduleCourse(TrainingSchedule request, int userID)
        {
            string crtqry = $"INSERT INTO training_schedule (CourseId,Course_name, ScheduleDate, TraingCompany, Trainer, Online,Comment,CreatedAt,CreatedBy) values " +
                            $"('{request.CourseId}',{request.CourseName},'{request.DateOfTraining}',{request.TrainingAgencyId},{request.TrainerName},{request.online},{request.Comment},'{UtilsRepository.GetUTCTime()}',{userID}) ; " +
                            $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(crtqry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Schedule Created Successfully.");
        }


        internal async Task<CMDefaultResponse> GetScheduleCourse()
        {
            return null;
        }

        internal async Task<CMDefaultResponse> ExecuteScheduleCourse()
        {
            return null;
        }
        //Master OF Trainig Category
        internal async Task<List<CMTRAININGCATE>> GetTrainingCategorty()
        {
            string myQuery = "SELECT id, name, description as description,status as status FROM course_category where status=1 ";
            List<CMTRAININGCATE> Data = await Context.GetData<CMTRAININGCATE>(myQuery).ConfigureAwait(false);
            return Data;
        }
        internal async Task<CMDefaultResponse> CreateTrainingCategorty(CMTRAININGCATE request)
        {

            String myqry = $"INSERT INTO course_category(name, description, status) VALUES " +
                                $"('{request.name}','{request.description}',1); " +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myqry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Training Category Added");
        }
        internal async Task<CMDefaultResponse> UpdateTrainingCategorty(CMTRAININGCATE request)
        {
            string updateQry = "UPDATE course_category  SET ";
            updateQry += $"name = '{request.name}', " +
                 $"description = '{request.description}', " +
                 $"status = 1 " +
                $"WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Training Category Updated");

        }
        internal async Task<CMDefaultResponse> DeleteTrainingCategorty(int id)
        {
            string delqry = $"UPDATE course_category  SET status=0  where id={id};";
            await Context.ExecuteNonQry<int>(delqry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Training Category deleted");
        }

        internal async Task<List<CMTRAININGCATE>> GetTargetedGroupList()
        {
            string myQuery1 = "SELECT id, name, description as  description, status FROM targeted_group where status=1 ";
            List<CMTRAININGCATE> Data = await Context.GetData<CMTRAININGCATE>(myQuery1).ConfigureAwait(false);
            return Data;
        }

        internal async Task<CMDefaultResponse> CreateTargetedGroup(CMTRAININGCATE request)
        {
            String myqry1 = $"INSERT INTO targeted_group(name, description, status) VALUES " +
                               $"('{request.name}','{request.description}',1); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myqry1).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Target Group  Add Successfully");
        }

        internal async Task<CMDefaultResponse> UpdateTargetedGroup(CMTRAININGCATE request)
        {
            string updateQry1 = "UPDATE targeted_group  SET ";
            updateQry1 += $"name = '{request.name}', " +
                 $"description = '{request.description}', " +
                 $"status = 1 " +
                $"WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry1).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Target Group Update Successfully");
        }

        internal async Task<CMDefaultResponse> DeleteTargetedGroup(int id)
        {
            string delqry1 = $"UPDATE targeted_group  SET status=0  where id={id};";
            await Context.ExecuteNonQry<int>(delqry1).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Target Group delete Successfully");
        }
    }

}


