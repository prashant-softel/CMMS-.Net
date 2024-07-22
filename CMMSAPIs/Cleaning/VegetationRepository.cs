using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Repositories.CleaningRepository
{

    public class VegetationRepository : CleaningRepository
    {
        private UtilsRepository _utilsRepo;
        public VegetationRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            moduleType = (int)cleaningType.Vegetation;
            measure = "area";
        }

        internal async Task<List<CMVegEquipmentList>> GetEquipmentList(int facilityId)
        {
            string filter = "";
            string Query = $"select id as blockId,  name as blockName from facilities ";

            if (facilityId > 0)
            {
                Query += $" where facilityId={facilityId} ";
                filter += $" and facilityId={facilityId} ";
            }

            List<CMVegEquipmentList> blocks = await Context.GetData<CMVegEquipmentList>(Query).ConfigureAwait(false);

            //string blockId = "";
            //foreach (var block in blocks)
            //{
            //    blockId += $"{block.blockId},";
            //}

            //blockId = blockId.Substring(0, blockId.Length - 1);

            string InvQuery = $"select id as invId, name as invName , blockId ,area from assets where categoryId = 2 {filter} ";
            List<CMInv> Invs = await Context.GetData<CMInv>(Query).ConfigureAwait(false);


            foreach (CMVegEquipmentList block in blocks)
            {
                foreach (CMInv inv in Invs)
                {
                    if (block.blockId == inv.blockId)
                    {
                        block.invs.Add(inv);
                    }

                }
            }
            return blocks;
        }
        internal async Task<CMDefaultResponse> StartExecutionVegetation(int executionId, int userId)
        {
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            int status;


            status = (int)CMMS.CMMS_Status.VEG_TASK_STARTED;

            string Query = $"Update cleaning_execution set status = {status},executedById={userId},executionStartedAt='{UtilsRepository.GetUTCTime()}' where id = {executionId}; ";

            int retVal = await Context.ExecuteNonQry<int>(Query).ConfigureAwait(false);


            if (retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION, executionId, 0, 0, "Execution Started", (CMMS.CMMS_Status)status, userId);


            CMDefaultResponse response = new CMDefaultResponse(executionId, CMMS.RETRUNSTATUS.SUCCESS, $"Task Execution started");
            return response;
        }
        internal async Task<CMDefaultResponse> StartScheduleExecutionVegetation(int scheduleId, int userId)
        {
            int status;


            status = (int)CMMS.CMMS_Status.VEG_TASK_STARTED;


            string scheduleQuery = $"Update cleaning_execution_schedules set status = {status},startedById={userId},startedAt='{UtilsRepository.GetUTCTime()}' where scheduleId = {scheduleId}; ";


            await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION, scheduleId, 0, 0, "schedule Started", (CMMS.CMMS_Status)status, userId);

            CMDefaultResponse response = new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule started Successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateScheduleExecutionVegetation(CMMCGetScheduleExecution request, int userId)
        {
            int status = (int)CMMS.CMMS_Status.EQUIP_CLEANED;
            int abandonStatus = (int)CMMS.CMMS_Status.EQUIP_ABANDONED;

            string field = $"waterUsed ={request.waterUsed},";


            status = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;
            abandonStatus = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED;

            field = "";


            string scheduleQuery = $"Update cleaning_execution_schedules set {field} updatedById={userId},remark_of_schedule='{request.remark}',updatedAt='{UtilsRepository.GetUTCTime()}' where scheduleId = {request.scheduleId}; Update cleaning_execution_items set status = {(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} where scheduleId = {request.scheduleId} ;";

            int val = await Context.ExecuteNonQry<int>(scheduleQuery).ConfigureAwait(false);

            if (request?.cleanedEquipmentIds?.Length > 0)
            {
                string equipIds = (request?.cleanedEquipmentIds?.Length > 0 ? " " + string.Join(" , ", request.cleanedEquipmentIds) + " " : string.Empty);

                string cleanedQuery = $"Update cleaning_execution_items set status = {status},executionDay={request.cleaningDay},cleanedById={userId},cleanedAt= '{UtilsRepository.GetUTCTime()}' where executionId = {request.executionId} and assetId IN ({equipIds}); ";

                int val2 = await Context.ExecuteNonQry<int>(cleanedQuery).ConfigureAwait(false);
            }

            if (request.abandonedEquipmentIds.Length > 0)
            {
                string equipIds2 = string.Join(" , ", request.abandonedEquipmentIds);

                string abandoneQuery = $"Update cleaning_execution_items set status = {abandonStatus},executionDay={request.cleaningDay},abandonedById={userId},abandonedAt= '{UtilsRepository.GetUTCTime()}' where executionId = {request.executionId} and assetId IN ({equipIds2}); ";

                int val3 = await Context.ExecuteNonQry<int>(abandoneQuery).ConfigureAwait(false);
            }


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION, request.scheduleId, 0, 0, "schedule Updated", CMMS.CMMS_Status.VEG_TASK_UPDATED, userId);

            CMDefaultResponse response = new CMDefaultResponse(request.scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Updated Successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> EndScheduleExecutionVegetation(int scheduleId, int userId)
        {
            int status;
            status = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;
            string scheduleQuery = $"Update cleaning_execution_schedules set status = {status},endedById={userId},endedAt='{UtilsRepository.GetUTCTime()}' where scheduleId = {scheduleId}; ";
            //$"Update cleaning_execution_items set status = {status} where scheduleId = {scheduleId}";

            await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION, scheduleId, 0, 0, "schedule Execution Completed", (CMMS.CMMS_Status)status, userId);

            CMDefaultResponse response = new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Execution Completed");
            return response;
        }
        internal async Task<CMDefaultResponse> ApproveScheduleExecutionVegetation(ApproveMC request, int userID)
        {
            int status;
            status = (int)CMMS.CMMS_Status.VEG_TASK_END_APPROVED;
            string approveQuery = $"Update cleaning_execution_schedules set status= {status} ,approvedById={userID}, remark='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}' where  scheduleId= {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION, request.schedule_id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.schedule_id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution Schedule Approved");
            return response;

        }
        internal async Task<CMDefaultResponse> EndExecutionVegetation(int executionId, int userId)
        {
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            int status;




            status = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;



            string Query = $"Update cleaning_execution set status = {status},endedById={userId},endedAt='{UtilsRepository.GetUTCTime()}' where id = {executionId}; ";
            //$"Update cleaning_execution_schedules set status = {status} where scheduleId = {scheduleId}";

            int retVal = await Context.ExecuteNonQry<int>(Query).ConfigureAwait(false);



            if (retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION, executionId, 0, 0, "Execution Completed", (CMMS.CMMS_Status)status, userId);


            CMDefaultResponse response = new CMDefaultResponse(executionId, CMMS.RETRUNSTATUS.SUCCESS, $"Task Execution Completed");
            return response;
        }
        internal async Task<CMDefaultResponse> ApproveEndExecution(ApproveMC request, int userID)
        {
            //comment remark='{request.comment}'

            int status = (int)CMMS.CMMS_Status.VEG_TASK_END_APPROVED;

            string approveQuery = $"Update cleaning_execution set status= {status} ,approvedById={userID}, " +
                $" approvedAt='{UtilsRepository.GetUTCTime()}',rescheduled = 1 where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            string statusQry = $"SELECT status FROM cleaning_execution  WHERE id = {request.id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status1 = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            if (status1 != CMMS.CMMS_Status.VEG_TASK_END_APPROVED)
                return new CMRescheduleApprovalResponse(0, request.id, CMMS.RETRUNSTATUS.FAILURE, "Only a End Vegetation Task can be Approved ");

            string mainQuery = $"INSERT INTO cleaning_execution(planId,moduleType,facilityId,frequencyId,noOfDays,startDate,assignedTo,status,prevTaskId,prevTaskDoneDate)  " +
                              $"select planId as planId,{moduleType} as moduleType,facilityId,frequencyId,noOfDays as noOfDays , " +
                              $"Case WHEN cleaning_execution.frequencyId in(4,5,6) THEN DATE_ADD(startDate,INTERVAL freq.months MONTH) WHEN  cleaning_execution.frequencyId=7 THEN DATE_ADD(startDate,INTERVAL 1 YEAR)  else DATE_ADD(startDate, INTERVAL freq.days DAY) end as startDate,assignedTo , " +
                              $"{(int)CMMS.CMMS_Status.RESCHEDULED_TASK} as status,{request.id} as prevTaskId, '{UtilsRepository.GetUTCTime()}' as prevTaskDoneDate " +
                              $" from cleaning_execution left join frequency as freq on cleaning_execution.frequencyId= freq.id " +
                              $" where cleaning_execution.id = {request.id}; " +
                              $"SELECT LAST_INSERT_ID(); ";

            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int taskid = Convert.ToInt32(dt3.Rows[0][0]);

            string scheduleQry = $"INSERT INTO cleaning_execution_schedules(executionId,planId,actualDay,cleaningType,status) " +
                                $"select {taskid} as executionId,planId as planId, actualDay,cleaningType,{(int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED} as status from cleaning_execution_schedules where cleaning_execution_schedules.executionId = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);


            string equipmentQry = $"INSERT INTO cleaning_execution_items (`executionId`,`moduleType`,`scheduleId`,`assetId`,`{measure}`,`plannedDate`,`plannedDay`,`createdById`,`createdAt`,`status`) " +
                $"SELECT '{taskid}' as executionId,item.moduleType,item.scheduleId, item.assetId,{measure},DATE_ADD('{DateTime.Now.ToString("yyyy-MM-dd")}',interval item.plannedDay - 1  DAY) as plannedDate, " +
                $"item.plannedDay,item.createdById,item.createdAt,  " +
                $"{(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} as status from cleaning_execution_items as item  " +
                $"join cleaning_execution as schedule on item.executionId = schedule.id and item.plannedDay = schedule.noOfDays where item.executionId= {request.id}";

            var retVal = await Context.ExecuteNonQry<int>(equipmentQry).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION, request.id, 0, 0, request.comment, CMMS.CMMS_Status.VEG_PLAN_APPROVED, userID);

            CMDefaultResponse response = new CMRescheduleApprovalResponse(taskid, request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution End Approved To Vegetation Task {taskid}");

            return response;

        }
        internal async Task<CMDefaultResponse> ReAssignMcTaskVegetation(int task_id, int assign_to, int userID)
        {

            string statusQry = $"SELECT status FROM cleaning_execution WHERE id = {task_id};";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);

            if (status != CMMS.CMMS_Status.RESCHEDULED_TASK && status != CMMS.CMMS_Status.VEG_PLAN_APPROVED && status != CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW)
            {
                return new CMDefaultResponse(task_id, CMMS.RETRUNSTATUS.FAILURE, "Only Scheduled Tasks can be assigned ");
            }

            string myQuery = "UPDATE cleaning_execution SET " +
                                $"assignedTo = {assign_to}, " +
                                $"status = {(int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED}, " +
                                $"updatedAt = '{UtilsRepository.GetUTCTime()}', " +
                                $"updatedById = {userID}  " +
                                $"WHERE id = {task_id} ;";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;

            CMDefaultResponse response = new CMDefaultResponse();


            if (status == CMMS.CMMS_Status.VEG_TASK_SCHEDULED)
            {
                response = new CMDefaultResponse(task_id, retCode, $"Vegetation Task Assigned To user Id {assign_to}");
            }
            else
            {
                response = new CMDefaultResponse(task_id, retCode, $"Vegetation Task Reassigned To user Id {assign_to}");
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION, task_id, 0, 0, $" Task Assigned to user_Id {assign_to}", CMMS.CMMS_Status.VEG_TASK_ASSIGNED, userID);
            return response;
        }

    }
}
