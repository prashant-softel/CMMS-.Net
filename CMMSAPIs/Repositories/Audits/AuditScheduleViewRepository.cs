using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Audits
{
    public class AuditScheduleViewRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        private JobRepository _jobRepo;
        public AuditScheduleViewRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            _jobRepo = new JobRepository(sqlDBHelper);
        }

        internal async Task<List<CMAuditScheduleList>> GetAuditScheduleViewList(CMAuditListFilter request)
        {
            /*
             * Primary Table - AuditSchedule
             * Supporting tables - Users, Facility
             * Check the CMAuditScheduleList models properties and return 
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMAuditScheduleDetail> GetAuditScheduleDetail(int audit_id)
        {
            /*
             * Primary Table - AuditSchedule, AuditExecution
             * Supporting tables - Users, Facility, history, job
             * Check the CMAuditScheduleDetail models properties and return
             * Your Code goes here
            */
            return null;
        }

        internal async Task<List<CMDefaultResponse>> ExecuteAuditSchedule(CMPMExecutionDetail request, int userID)
        {
            List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();
            string statusQry = $"SELECT status FROM pm_task WHERE id = {request.task_id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status != CMMS.CMMS_Status.AUDIT_START)
            {
                responseList.Add(new CMDefaultResponse(request.task_id, CMMS.RETRUNSTATUS.FAILURE,
                    "Task Execution must be rejected or in progress to modify execution details"));
                return responseList;
            }

            CMPMScheduleObservation schedule = new CMPMScheduleObservation();
            schedule = request.schedules[0];


            string executeQuery1 = $"SELECT id FROM pm_schedule WHERE id = {schedule.schedule_id} and task_id = {request.task_id};";

            DataTable dtSch = await Context.FetchData(executeQuery1).ConfigureAwait(false);

            if (dtSch.Rows.Count == 0)
                throw new MissingMemberException($"PM Schedule PMSCH{schedule.schedule_id} associated with PM Task PMTASK{request.task_id} not found");

            string executeQuery = $"SELECT id FROM pm_execution WHERE PM_Schedule_Id = {schedule.schedule_id} and task_id = {request.task_id};";

            DataTable dt = await Context.FetchData(executeQuery).ConfigureAwait(false);

            List<int> executeIds = dt.GetColumn<int>("id");

            foreach (var schedule_detail in schedule.add_observations)
            {
                CMDefaultResponse response;
                if (executeIds.Contains(schedule_detail.execution_id))
                {
                    int changeFlag = 0;
                    string myQuery1 = "SELECT id as execution_id, PM_Schedule_Observation as observation, job_created as job_create " +
                                        $"FROM pm_execution WHERE id = {schedule_detail.execution_id};";
                    List<AddObservation> execution_details = await Context.GetData<AddObservation>(myQuery1).ConfigureAwait(false);

                    string myQuery2 = $"SELECT checkpoint.type from pm_execution left join checkpoint on pm_execution.check_point_id = checkpoint.id WHERE pm_execution.id = {schedule_detail.execution_id};";

                    DataTable dtType = await Context.FetchData(myQuery2).ConfigureAwait(false);
                    object valueFromDatabase = dtType.Rows[0][0];
                    string CPtypeValue = "";
                    /*    if (Convert.ToInt32(dtType.Rows[0][0]) == 0)
                     {
                         CPtypeValue = $" , `text` = '{schedule_detail.text}' ";
                     }
                     else if (Convert.ToInt32(dtType.Rows[0][0]) == 1)
                     {
                         CPtypeValue = $" , `text` = {schedule_detail.text} ";
                     }
                     else if (Convert.ToInt32(dtType.Rows[0][0]) == 2)
                     {
                         CPtypeValue = $" , `text` = {schedule_detail.text} ";
                     }*/
                    if (valueFromDatabase != DBNull.Value)
                    {
                        int typeValue = Convert.ToInt32(valueFromDatabase);

                        // Now use typeValue in your logic
                        if (typeValue == 0)
                        {
                            CPtypeValue = $" , `text` = '{schedule_detail.text}' ";
                        }
                        else if (typeValue == 1 || typeValue == 2)
                        {
                            CPtypeValue = $" , `text` = {schedule_detail.text} ";
                        }
                    }
                    CPtypeValue = CPtypeValue + $" , is_ok = {schedule_detail.cp_ok} ";
                    if (schedule_detail.observation != null || !schedule_detail.observation.Equals(execution_details[0].observation))
                    {
                        string updateQry = "UPDATE pm_execution SET ";
                        string message;
                        if (execution_details[0].observation == null || execution_details[0].observation == "")
                        {
                            updateQry += $"PM_Schedule_Observation_added_by = {userID}, " +
                                        $"PM_Schedule_Observation_add_date = '{UtilsRepository.GetUTCTime()}', " +
                                        $"PM_Schedule_Observation = '{schedule_detail.observation}' {CPtypeValue} ";
                            message = "Observation Added";
                            response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "Observation Added Successfully");
                        }
                        else
                        {
                            updateQry += $"PM_Schedule_Observation = '{schedule_detail.observation}', ";
                            updateQry += $"PM_Schedule_Observation_update_by = {userID}, " +
                                        $"PM_Schedule_Observation_update_date = '{UtilsRepository.GetUTCTime()}' {CPtypeValue} ";
                            message = "Observation Updated";
                            response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "Observation Updated Successfully");
                        }
                        updateQry += $"WHERE id = {schedule_detail.execution_id};";
                        await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, message, CMMS.CMMS_Status.PM_UPDATED, userID);
                        responseList.Add(response);
                        changeFlag++;
                    }
                    if (schedule_detail.job_create == 1 && execution_details[0].job_create == 0)
                    {

                        string facilityQry = $"SELECT pm_task.facility_id as block, CASE WHEN facilities.parentId=0 THEN facilities.id ELSE facilities.parentId END AS parent " +
                                            $"FROM pm_schedule Left join pm_task on pm_task.id = pm_schedule.task_id LEFT JOIN facilities ON pm_task.facility_id=facilities.id " +
                                            $"WHERE pm_schedule.id = {schedule.schedule_id}  group by pm_task.id ;";
                        DataTable dtFacility = await Context.FetchData(facilityQry).ConfigureAwait(false);
                        string titleDescQry = $"SELECT CONCAT('PMSCH{schedule.schedule_id}', Check_Point_Name, ': ', Check_Point_Requirement) as title, " +
                                                $"PM_Schedule_Observation as description FROM pm_execution WHERE id = {schedule_detail.execution_id};";
                        DataTable dtTitleDesc = await Context.FetchData(titleDescQry).ConfigureAwait(false);
                        string assetsPMQry = $"SELECT Asset_id FROM pm_schedule WHERE id = {schedule.schedule_id};";
                        DataTable dtPMAssets = await Context.FetchData(assetsPMQry).ConfigureAwait(false);
                        CMCreateJob newJob = new CMCreateJob()
                        {
                            title = dtTitleDesc.GetColumn<string>("title")[0],
                            description = dtTitleDesc.GetColumn<string>("description")[0],
                            facility_id = dtFacility.GetColumn<int>("parent")[0],
                            block_id = dtFacility.GetColumn<int>("block")[0],
                            breakdown_time = DateTime.UtcNow,
                            AssetsIds = dtPMAssets.GetColumn<int>("Asset_id"),
                            jobType = CMMS.CMMS_JobType.PreventiveMaintenance
                        };
                        CMDefaultResponse jobResp = await _jobRepo.CreateNewJob(newJob, userID);
                        string updateQry = $"UPDATE pm_execution SET job_created = 1, linked_job_id = {jobResp.id[0]} WHERE id = {schedule_detail.execution_id};";
                        await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, $"Job {jobResp.id[0]} Created for PM", CMMS.CMMS_Status.PM_UPDATED, userID);
                        response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, $"Job {jobResp.id[0]} Created for PM Successfully");
                        responseList.Add(response);
                        changeFlag++;
                    }
                    if (schedule_detail.pm_files != null)
                    {
                        if (schedule_detail.pm_files.Count != 0)
                        {
                            foreach (var file in schedule_detail.pm_files)
                            {
                                string checkEventFiles = $"SELECT * FROM pm_schedule_files " +
                                                            $"WHERE PM_Schedule_id = {schedule.schedule_id} " +
                                                            $"AND PM_Execution_id = {schedule_detail.execution_id} " +
                                                            $"AND PM_Event = {(int)file.pm_event}";
                                DataTable dt2 = await Context.FetchData(checkEventFiles).ConfigureAwait(false);

                                if (dt2.Rows.Count > 0)
                                {
                                    string deleteQry = "DELETE FROM pm_schedule_files " +
                                                            $"WHERE PM_Schedule_id = {schedule.schedule_id} " +
                                                            $"AND PM_Execution_id = {schedule_detail.execution_id} " +
                                                            $"AND PM_Event = {(int)file.pm_event}";
                                    await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
                                }
                                string insertFile = "INSERT INTO pm_schedule_files(PM_Schedule_id, PM_Schedule_Code, PM_Execution_id, File_id, PM_Event, " +
                                                    "File_Discription, File_added_by, File_added_date, File_Server_id, File_Server_Path) VALUES " +
                                                    $"({schedule.schedule_id}, 'PMSCH{schedule.schedule_id}', {schedule_detail.execution_id}, {file.file_id}, " +
                                                    $"{(int)file.pm_event}, '{file.file_desc}', {userID}, '{UtilsRepository.GetUTCTime()}', 1, " +
                                                    $"'http://cms_test.com/' ); SELECT LAST_INSERT_ID();";
                                DataTable dt3 = await Context.FetchData(insertFile).ConfigureAwait(false);
                                int id = Convert.ToInt32(dt3.Rows[0][0]);
                                string otherDetailsQry = "UPDATE pm_schedule_files as pmf " +
                                                            "JOIN uploadedfiles as f ON pmf.File_id = f.id " +
                                                            "JOIN pm_schedule as pms ON pms.id = pmf.PM_Schedule_id " +
                                                            "JOIN pm_execution as pme ON pme.id = pmf.PM_Execution_id " +
                                                         "SET " +
                                                            "pmf.PM_Schedule_Title = pms.PM_Schedule_Name, " +
                                                            "pmf.Check_Point_id = pme.Check_Point_id, " +
                                                            "pmf.Check_Point_Code = pme.Check_Point_Code, " +
                                                            "pmf.Check_Point_Name = pme.Check_Point_Name, " +
                                                            "pmf.File_Name = SUBSTRING_INDEX(f.file_path, '\\\\', -1), " +
                                                            "pmf.File_Path = f.file_path, " +
                                                            "pmf.File_Type_name = f.file_type, " +
                                                            "pmf.File_Size = f.file_size, " +
                                                            "pmf.File_Size_Units = f.file_size_units, " +
                                                            "pmf.File_Size_bytes = f.file_size_bytes, " +
                                                            "pmf.File_Type_ext = SUBSTRING_INDEX(f.file_path, '.', -1) " +
                                                         $"WHERE pmf.id = {id};";
                                await Context.ExecuteNonQry<int>(otherDetailsQry).ConfigureAwait(false);
                            }
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, schedule.schedule_id, CMMS.CMMS_Modules.PM_EXECUTION, schedule_detail.execution_id, $"{schedule_detail.pm_files.Count} file(s) attached to PMSCH{schedule.schedule_id}", CMMS.CMMS_Status.PM_UPDATED, userID);
                            response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, $"{schedule_detail.pm_files.Count} file(s) attached to PM Successfully");
                            responseList.Add(response);
                            changeFlag++;
                        }
                    }
                    if (changeFlag == 0)
                    {
                        response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.SUCCESS, "No changes");
                        responseList.Add(response);
                    }
                    else
                    {
                        string pmScheduleUpdate = $"UPDATE pm_schedule as pms " +
                                                  $"SET pms.PM_Schedule_Completed_by_id = {userID}, " +
                                                  $"pms.PM_Schedule_Completed_date = '{UtilsRepository.GetUTCTime()}' " +
                                                  $"WHERE pms.id = {schedule.schedule_id};";
                        await Context.ExecuteNonQry<int>(pmScheduleUpdate).ConfigureAwait(false);

                        CMDefaultResponse responseSchedule = new CMDefaultResponse(schedule.schedule_id, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Execution Updated Successfully.");

                        responseList.Add(responseSchedule);
                    }
                }
                else
                {
                    response = new CMDefaultResponse(schedule_detail.execution_id, CMMS.RETRUNSTATUS.INVALID_ARG, $"Execution ID {schedule_detail.execution_id} is not associated with PMSCH{schedule.schedule_id}");
                    responseList.Add(response);
                }
            }

            //string taskQry = $"update pm_task set updated_by = {userID},updated_at = '{UtilsRepository.GetUTCTime()}', update_remarks = '{request.comment}' WHERE id = {request.task_id};";
            //int val = await Context.ExecuteNonQry<int>(taskQry).ConfigureAwait(false);
            //CMDefaultResponse responseSchedule = new CMDefaultResponse(schedule.schedule_id, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Execution Updated Successfully.");

            //responseList.Add(responseSchedule);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, request.task_id, 0, 0, $"PM Task {request.task_id} updated.", CMMS.CMMS_Status.PM_UPDATED, userID);
            return responseList;
        }

        internal async Task<CMDefaultResponse> ApproveAuditSchedule(CMApproval request)
        {
            /*
             * Primary Table - AuditSchedule
             * Read the reques and Update the to primary table
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> RejectAuditSchedule(CMApproval request)
        {
            /*
             * Primary Table - AuditSchedule
             * Read the reques and Update the to primary table
             * Your Code goes here
            */
            return null;
        }
    }
}
