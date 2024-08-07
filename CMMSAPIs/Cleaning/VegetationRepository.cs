using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CMMSAPIs.Helper.CMMS;

namespace CMMSAPIs.Repositories.CleaningRepository
{

    public class VegetationRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public int moduleType;
        public static string measure = "moduleQuantity";
        public VegetationRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            moduleType = (int)cleaningType.Vegetation;
            measure = "area";
        }

        private Dictionary<int, string> StatusDictionary = new Dictionary<int, string>()
        {
            { (int)CMMS.CMMS_Status.MC_PLAN_DRAFT, "Draft" },
            { (int)CMMS.CMMS_Status.MC_PLAN_SUBMITTED, "Waiting for Approval" },
            { (int)CMMS.CMMS_Status.MC_PLAN_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.MC_PLAN_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.MC_PLAN_DELETED, "Deleted" },
            { (int)CMMS.CMMS_Status.MC_TASK_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.MC_TASK_STARTED, "In Progress" },
            { (int)CMMS.CMMS_Status.MC_TASK_COMPLETED, "Completed" },
            { (int)CMMS.CMMS_Status.MC_TASK_ABANDONED, "Abandoned" },
            { (int)CMMS.CMMS_Status.MC_TASK_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.MC_TASK_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW,"PTW_Linked" },
            { (int)CMMS.CMMS_Status.MC_TASK_END_APPROVED,"Approved" },
            { (int)CMMS.CMMS_Status.MC_TASK_END_REJECTED,"Reject" },
            { (int)CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED,"Approved" },
            { (int)CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT,"Reject" },
            { (int)CMMS.CMMS_Status.MC_TASK_RESCHEDULED,"Reschedule" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_DRAFT, "Draft" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_SUBMITTED, "Waiting for Approval" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_DELETED, "Deleted" },
            { (int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.VEG_TASK_STARTED, "In Progress" },
            { (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED, "Completed" },
            { (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED, "Abandoned" },
            { (int)CMMS.CMMS_Status.VEG_TASK_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.VEG_TASK_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.VEGETATION_LINKED_TO_PTW, "PTW Linked" },
            { (int)CMMS.CMMS_Status.VEG_TASK_END_APPROVED, "Closed Approved" },
            { (int)CMMS.CMMS_Status.VEG_TASK_END_REJECTED, "Closed Rejected" },
            { (int)CMMS.CMMS_Status.VEG_TASK_UPDATED, " Task updated" },
            { (int)CMMS.CMMS_Status.VEG_TASK_ASSIGNED , "Task Reassign" },
            { (int)CMMS.CMMS_Status.EQUIP_CLEANED, "Cleaned" },
            { (int)CMMS.CMMS_Status.EQUIP_ABANDONED, "Abandoned" },
            { (int)CMMS.CMMS_Status.EQUIP_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED, "TASK ABANDONED REJECTED" },
            { (int)CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED, "TASK ABANDONED APPROVED" },
            { (int)CMMS.CMMS_Status.RESCHEDULED_TASK, "TASK RESCHEDULE" },
            { (int)CMMS.CMMS_Status.MC_ASSIGNED, "TASK REASSING" },

        };
        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMMCPlan planObj, CMMCExecution executionObj)
        {
            string retValue = "";

            switch (notificationID)
            {
                case CMMS.CMMS_Status.MC_PLAN_DRAFT:
                    retValue = String.Format("Module Cleaning Plan <{0}> Draft by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_SUBMITTED:
                    retValue = String.Format("Module Cleaning Plan <{0}> Waiting for Approval ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_REJECTED:
                    retValue = String.Format("Module Cleaning Plan <{0}> Rejected by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_APPROVED:
                    retValue = String.Format("Module Cleaning Plan <{0}> Approved by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.MC_PLAN_DELETED:
                    retValue = String.Format("Module Cleaning Plan <{0}> Deleted by {1} ", planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULED:
                    retValue = String.Format("Module Cleaning Task <{0}> Scheduled ", executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue = String.Format("Module Cleaning Task <{0}> Execution started by {1} ", executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue = String.Format("Module Cleaning Task <{0}> Execution Completed - Waiting for Approval", executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue = String.Format("Module Cleaning Task <{0}>  Execution Abandoned ", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED:
                    retValue = String.Format("Module Cleaning Task <{0}>  Task Abandoned Rejected", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED:
                    retValue = String.Format("Module Cleaning Task <{0}>  Task Abandoned Approved", executionObj.executionId, executionObj.approvedById);
                    break;

                case CMMS.CMMS_Status.MC_TASK_REJECTED:
                    retValue = String.Format("Module Cleaning Task <{0}> Rejected ", executionObj.executionId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_APPROVED:
                    retValue = String.Format("Module Cleaning Task <{0}> Approved ", executionObj.executionId);
                    break;
                case CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW:
                    retValue = String.Format("Module Cleaning Task <{0}> PTW_Linked ", executionObj.executionId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_APPROVED:
                    retValue = String.Format("Module Cleaning Task <{0}> Approved ", executionObj.executionId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_END_REJECTED:
                    retValue = String.Format("Module Cleaning Task <{0}> Rejected ", executionObj.executionId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT:
                    retValue = String.Format("Module Cleaning Task <{0}> Rejected ", executionObj.executionId);
                    break;
                case CMMS.CMMS_Status.MC_TASK_RESCHEDULED:
                    retValue = String.Format("Module Cleaning Task <{0}> Reschdule ", executionObj.executionId);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_DRAFT:
                    retValue = String.Format("Vegetation Plan <{0}> Draft by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_SUBMITTED:
                    retValue = String.Format("Vegetation Plan <{0}> Submitted by {1} ", planObj.planId, planObj.createdBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_REJECTED:
                    retValue = String.Format("Vegetation Plan <{0}> Rejected by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_APPROVED:
                    retValue = String.Format("Vegetation Plan <{0}> Approved by {1} ", planObj.planId, planObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.VEG_PLAN_DELETED:
                    retValue = String.Format("Vegetation Plan <{0}> Deleted ", planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                    retValue = String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue = String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue = String.Format("Vegetation Cleaning Task <{0}> Execution Completed by {1} ", executionObj.executionId, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue = String.Format("Vegetation Task <{0}>  Execution Abandoned by {1} ", executionObj.executionId, executionObj.abandonedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_REJECTED:
                    retValue = String.Format("Vegetation Task <{0}> Rejected ", executionObj.executionId);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_APPROVED:
                    retValue = String.Format("Vegetation Task <{0}> Approved ", executionObj.executionId);
                    break;

                default:
                    break;
            }
            return retValue;

        }
        public static string Status_PTW(int statusID)
        {
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)statusID;
            string statusName = "";
            switch (status)
            {
                case CMMS.CMMS_Status.PTW_CREATED:
                    statusName = "Waiting for Approval";
                    break;
                case CMMS.CMMS_Status.PTW_ISSUED:
                    statusName = "Issued";
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    statusName = "Rejected By Issuer";
                    break;
                case CMMS.CMMS_Status.PTW_APPROVED:
                    statusName = "Approved";
                    break;
                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER:
                    statusName = "Rejected By Approver";
                    break;
                case CMMS.CMMS_Status.PTW_CLOSED:
                    statusName = "Closed";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    statusName = "Cancelled BY Issuer";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    statusName = "Cancelled By HSE";
                    break;
                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    statusName = "Cancelled By Approver";
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED:
                    statusName = "Cancelled";
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    statusName = "Cancel Request Rejected";
                    break;
                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED:
                    statusName = "Cancelled";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    statusName = "Requested for Extension";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE:
                    statusName = "Approved Extension";
                    break;
                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    statusName = "Rejected Extension";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    statusName = "Linked to Job";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    statusName = "Linked to PM";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    statusName = "Linked to Audit";
                    break;
                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    statusName = "Linked to HOTO";
                    break;
                case CMMS.CMMS_Status.PTW_EXPIRED:
                    statusName = "Expired";
                    break;
                case CMMS.CMMS_Status.PTW_UPDATED:
                    statusName = "Updated";
                    break;
                default:
                    statusName = "Invalid";
                    break;
            }
            return statusName;
        }
        internal async Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId)
        {
            int planIds = 0;
            int status;
            int cleaningType;
            status = (int)CMMS.CMMS_Status.VEG_PLAN_SUBMITTED;


            foreach (CMMCPlan plan in request)
            {
                cleaningType = plan.cleaningType;
                string startDate = "NULL";

                /* if (plan.startDate != null && plan.startDate != Convert.ToDateTime("01-01-0001 00:00:00"))
                 {
                     startDate = " '" + plan.startDate.ToString("yyyy-MM-dd hh:MM:ss")+"' ";

                 }*/
                string qry = "insert into `cleaning_plan` (`moduleType`,`facilityId`,`title`,`description`, `durationDays`,`frequencyId`,cleaningType,`startDate`,`assignedTo`,`status`,`createdById`,`createdAt`) VALUES " +
                            $"({moduleType},'{plan.facilityId}','{plan.title}','{plan.description}','{plan.noOfCleaningDays}','{plan.frequencyId}',{plan.cleaningType},'{plan.startDate}','{plan.assignedToId}',{status},'{userId}','{UtilsRepository.GetUTCTime()}');" +
                             "SELECT LAST_INSERT_ID() as id ;";

                List<CMMCPlan> planQry = await Context.GetData<CMMCPlan>(qry).ConfigureAwait(false);

                var planId = Convert.ToInt16(planQry[0].id.ToString());

                string scheduleQry = " ";
                string equipmentQry = $"insert into `cleaning_plan_items` (`planId`,`moduleType`,`scheduleId`,`assetId`,`plannedDay`,`createdById`,`createdAt`) VALUES ";

                if (plan.schedules.Count > 0)
                {

                    foreach (var schedule in plan.schedules)
                    {
                        //cleaningType = schedule.cleaningType;

                        //if (moduleType == 2)
                        //    cleaningType = 0;

                        scheduleQry = "insert into `cleaning_plan_schedules` (`planId`,`moduleType`,cleaningType,`plannedDay`,`createdById`,`createdAt`) VALUES ";
                        scheduleQry += $"({planId},{moduleType},{cleaningType},{schedule.cleaningDay},'{userId}','{UtilsRepository.GetUTCTime()}');" +
                                           $"SELECT LAST_INSERT_ID() as id ;";

                        List<CMMCSchedule> schedule_ = await Context.GetData<CMMCSchedule>(scheduleQry).ConfigureAwait(false);
                        var scheduleId = Convert.ToInt16(schedule_[0].id.ToString());

                        if (schedule.equipments.Count > 0)
                        {
                            foreach (var equipment in schedule.equipments)
                            {
                                equipmentQry += $"({planId},{moduleType},{scheduleId},{equipment.id},{schedule.cleaningDay},'{userId}','{UtilsRepository.GetUTCTime()}'),";
                            }

                        }

                    }

                    if (plan.schedules[0].equipments.Count > 0)
                    {
                        equipmentQry = equipmentQry.Substring(0, equipmentQry.Length - 1);

                        equipmentQry += $"; update cleaning_plan_items left join assets on cleaning_plan_items.assetId = assets.id set cleaning_plan_items.{measure} = assets.{measure} where planId ={planId};";

                        await Context.GetData<CMMCPlan>(equipmentQry).ConfigureAwait(false);
                    }
                }

                planIds = planId;
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_PLAN, planId, 0, 0, "Plan Created", (CMMS.CMMS_Status)status, userId);

            }

            CMDefaultResponse response = new CMDefaultResponse(planIds, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Created Successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userID)
        {
            string approveQuery = $"Update cleaning_plan set isApproved = 1,status= {(int)CMMS.CMMS_Status.VEG_PLAN_APPROVED} ,approvedById={userID}, ApprovalReason='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}' where planId = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            string mainQuery = $"INSERT INTO cleaning_execution(planId,moduleType,facilityId,frequencyId,noOfDays,startDate,assignedTo,status)  " +
                              $"select planId as planId,{moduleType} as moduleType,facilityId,frequencyId,durationDays as noOfDays ,startDate,assignedTo," +
                              $"{(int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED} as status " +
                              $"from cleaning_plan where planId = {request.id}; " +
                              $"SELECT LAST_INSERT_ID(); ";

            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int taskid = Convert.ToInt32(dt3.Rows[0][0]);

            string scheduleQry = $"INSERT INTO cleaning_execution_schedules(executionId,planId,actualDay,cleaningType,status) " +
                                $"select {taskid} as executionId,planId as planId, plannedDay,cleaningType,{(int)CMMS.CMMS_Status.MC_TASK_SCHEDULED} as status from cleaning_plan_schedules where planId = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);

            string equipmentQry = $"INSERT INTO cleaning_execution_items (`executionId`,`moduleType`,`scheduleId`,`assetId`,`{measure}`,`plannedDate`,`plannedDay`,`createdById`,`createdAt`,`status`) SELECT '{taskid}' as executionId,item.moduleType,schedule.scheduleId,item.assetId,{measure},DATE_ADD('{DateTime.Now.ToString("yyyy-MM-dd")}',interval item.plannedDay - 1  DAY) as plannedDate,item.plannedDay,schedule.createdById,schedule.createdAt,{(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} as status from cleaning_plan_items as item join cleaning_execution_schedules as schedule on item.planId = schedule.planId and item.plannedDay = schedule.actualDay where schedule.executionId = {taskid}";

            var retVal = await Context.ExecuteNonQry<int>(equipmentQry).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.VEG_PLAN_APPROVED, userID);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, taskid, 0, 0, "Execution scheduled", CMMS.CMMS_Status.VEG_TASK_APPROVED, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Approved");
            return response;

        }

        internal async Task<CMDefaultResponse> UpdatePlan(List<CMMCPlan> requests, int userId)
        {
            int planId = 0;

            foreach (CMMCPlan request in requests)
            {

                string Query = "UPDATE cleaning_plan SET ";
                if (request.title != null && request.title != "")
                    Query += $"title = '{request.title}', ";
                if (request.assignedToId > 0)
                    Query += $"assignedTo = '{request.assignedToId}', ";
                if (request.description != null && request.description != "")
                    Query += $"description = '{request.description}', ";
                if (request.frequencyId > 0)
                    Query += $"frequencyId = {request.frequencyId}, ";
                if (request.cleaningType > 0)
                    Query += $"cleaningType = {request.cleaningType}, ";

                Query += $"startDate = '{request.startDate}', ";
                if (request.noOfCleaningDays > 0)
                    Query += $"durationDays = {request.noOfCleaningDays}, ";

                Query += $"updatedAt = '{UtilsRepository.GetUTCTime()}', updatedById = {userId} WHERE planId = {request.planId};";

                await Context.ExecuteNonQry<int>(Query).ConfigureAwait(false);

                string myQuery = "";

                if (request.schedules.Count > 0)
                {

                    foreach (var schedule in request.schedules)
                    {

                        if (moduleType == 2)
                        {
                            //(CASE WHEN { schedule.cleaningType} = 'Wet' then 1 else WHEN { schedule.cleaningType} = 'Dry' then 2 end)
                            // if (schedule.cleaningType != 0)
                            // cleaningType = { schedule.cleaningType},
                            myQuery += $"UPDATE cleaning_plan_schedules SET updatedAt = '{UtilsRepository.GetUTCTime()}', updatedById = {userId} where plannedDay ={schedule.cleaningDay} and planId={request.planId};";
                        }

                        myQuery += $"Delete from cleaning_plan_items where scheduleId = {schedule.scheduleId};";

                        if (schedule.equipments.Count > 0)
                        {
                            myQuery += $"insert into `cleaning_plan_items` (`planId`,`scheduleId`,`assetId`,`{measure}`,`plannedDay`,`updatedById`,`updatedAt`,`createdById`,`createdAt`) VALUES ";


                            foreach (var equipment in schedule.equipments)
                            {
                                myQuery += $"({request.planId},{schedule.scheduleId},{equipment.id},(select {measure} from assets where id={equipment.id}),{schedule.cleaningDay},{userId},'{UtilsRepository.GetUTCTime()}',(select createdById from cleaning_plan where planId={request.planId}),(select createdAt from cleaning_plan where planId={request.planId})),";
                                // if (equipment.noOfPlanDay > 0)
                                //myQuery += $"UPDATE cleaning_plan_items SET plannedDay ={equipment.noOfPlanDay},updatedAt = '{UtilsRepository.GetUTCTime()}', updatedBy = {userId} where assetId = {equipment.id} and scheduleId={schedule.scheduleId};";
                            }
                            myQuery = myQuery.Substring(0, myQuery.Length - 1);
                            myQuery += ";";
                            planId = request.planId;
                        }

                    }
                    await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
                }

                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION, request.planId, 0, 0, "Plan Updated", CMMS.CMMS_Status.VEG_PLAN_DRAFT, userId);
            }

            CMDefaultResponse response = new CMDefaultResponse(planId, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Updated Successfully ");
            return response;
        }
        internal async Task<CMMCPlan> GetPlanDetails(int planId, string facilitytimeZone)
        {
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN plan.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string planQuery = $"select plan.planId,plan.title,plan.startDate ,plan.frequencyId,plan.assignedTo as assignedToId ,plan.approvedById,plan.createdById,plan.facilityId,f.name as siteName, CONCAT(createdBy.firstName, createdBy.lastName) as createdBy , plan.createdAt,freq.name as frequency, " +
                $" plan.durationDays as noOfCleaningDays, CONCAT(approvedBy.firstName, approvedBy.lastName) as approvedBy , plan.approvedAt as approvedAt,CONCAT(assignedTo.firstName, ' ', assignedTo.lastName) as assignedTo,plan.status,{statusOut} as status_short from cleaning_plan as plan  LEFT JOIN Frequency as freq on freq.id = plan.frequencyId LEFT JOIN users as createdBy ON createdBy.id = plan.createdById LEFT JOIN users as approvedBy ON approvedBy.id = plan.approvedById LEFT JOIN facilities as f  on f.id=plan.facilityId    " +
              $"LEFT JOIN users as assignedTo ON assignedTo.id = plan.assignedTo where plan.planId = {planId}  ;";

            List<CMMCPlan> _ViewMCPlan = await Context.GetData<CMMCPlan>(planQuery).ConfigureAwait(false);

            string measures = ",count(distinct assets.parentId ) as Invs,count(item.assetId) as smbs ";

            if (moduleType == 2)
            {
                measures += ", sum(assets.area) as area ";
            }
            else
            {
                measures += ",sum(assets.moduleQuantity) as scheduledModules,cp.cleaningType as cleaningType ,CASE cp.cleaningType WHEN 1 then 'Wet' When 2 then 'Dry' when 3 then 'Robotic' else 'Wet 'end as cleaningTypeName";
            }

            string scheduleQuery = $"select schedule.scheduleId, schedule.plannedDay as cleaningDay {measures} from cleaning_plan_schedules as schedule  LEFT JOIN cleaning_plan_items as item on schedule.scheduleId = item.scheduleId   LEFT JOIN cleaning_plan as cp on schedule.planId = cp.planId LEFT   JOIN assets on assets.id = item.assetId where schedule.planId={planId} group by schedule.scheduleId ORDER BY schedule.scheduleId ASC";


            List<CMMCSchedule> _Schedules = await Context.GetData<CMMCSchedule>(scheduleQuery).ConfigureAwait(false);

            string equipmentQuery = $"select assets.id,assets.parentId as parentId,parent.name as parentName, assets.name as equipmentName,assets.moduleQuantity as moduleQuantity,assets.area ,item.plannedDay as noOfPlanDay  from cleaning_plan_items as item  LEFT JOIN assets  on assets.id = item.assetId left join assets as parent on assets.parentId = parent.id where item.planId={planId} ORDER BY item.plannedDay ASC ";

            List<CMMCEquipmentDetails> _Equipments = await Context.GetData<CMMCEquipmentDetails>(equipmentQuery).ConfigureAwait(false);

            foreach (CMMCSchedule Schedules in _Schedules)
            {
                Schedules.equipments = new List<CMMCEquipmentDetails>();
                foreach (CMMCEquipmentDetails equip in _Equipments)
                {
                    if (Schedules.cleaningDay == equip.noOfPlanDay)
                    {
                        Schedules.equipments.Add(equip);
                    }

                }
            }

            _ViewMCPlan[0].schedules = _Schedules;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_ViewMCPlan[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.VEGETATION, _Status_long, _ViewMCPlan[0], null);
            _ViewMCPlan[0].status_long = _longStatus;
            foreach (var list in _ViewMCPlan)
            {
                if (list != null && list.approvedAt != null)
                    list.approvedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.approvedAt);

                if (list != null && list.createdAt != null)
                    list.createdAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.createdAt);
                if (list != null && list.startDate != null)
                    list.startDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.startDate);

            }

            return _ViewMCPlan[0];
        }

        internal virtual async Task<List<CMMCEquipmentList>> GetEquipmentList(int facility_Id)
        {
            string filter = "";
            List<CMMCEquipmentList> invs = new List<CMMCEquipmentList>();

            if (facility_Id > 0)
            {
                string invQuery = $"SELECT assets.id AS invId, assets.name AS invName, assets.moduleQuantity FROM assets JOIN assetcategories ON assets.categoryId = assetcategories.id WHERE assetcategories.name=\"Inverter\" and facilityId={facility_Id}  ";


                invs = await Context.GetData<CMMCEquipmentList>(invQuery).ConfigureAwait(false);


                string smbQuery = $"SELECT assets.id as smbId, assets.name as smbName , assets.parentId, assets.moduleQuantity ,assets.area from assets  join assetcategories on  assets.categoryId =assetcategories.id where assetcategories.name = \"SMB\" and facilityId={facility_Id}  ";

                List<CMPlanSMB> smbs = await Context.GetData<CMPlanSMB>(smbQuery).ConfigureAwait(false);

                //List<CMSMB> invSmbs = new List<CMSMB>;

                foreach (CMMCEquipmentList inv in invs)
                {
                    inv.moduleQuantity = 0;
                    inv.area = 0;

                    foreach (CMPlanSMB smb in smbs)
                    {
                        if (inv.invId == smb.parentId)
                        {
                            inv.moduleQuantity += smb.moduleQuantity;
                            inv.area += smb.area;
                            inv?.smbs.Add(smb);
                        }
                    }
                }
            }
            return invs;
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

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, executionId, 0, 0, "Execution Started", (CMMS.CMMS_Status)status, userId);


            CMDefaultResponse response = new CMDefaultResponse(executionId, CMMS.RETRUNSTATUS.SUCCESS, $"Task Execution started");
            return response;
        }
        internal async Task<CMDefaultResponse> StartScheduleExecutionVegetation(int scheduleId, int userId)
        {
            int status;


            status = (int)CMMS.CMMS_Status.VEG_TASK_STARTED;


            string scheduleQuery = $"Update cleaning_execution_schedules set status = {status},startedById={userId},startedAt='{UtilsRepository.GetUTCTime()}' where scheduleId = {scheduleId}; ";


            await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_EXECUTION, scheduleId, 0, 0, "schedule Started", (CMMS.CMMS_Status)status, userId);

            CMDefaultResponse response = new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule started Successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateScheduleExecutionVegetation(CMMCGetScheduleExecution request, int userId)
        {
            int status1 = (int)CMMS.CMMS_Status.EQUIP_CLEANED;
            int abandonStatus1 = (int)CMMS.CMMS_Status.EQUIP_ABANDONED;

            string field = $"waterUsed ={request.waterUsed},";


            int status = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;
            int abandonStatus = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED;

            field = "";


            string scheduleQuery = $"Update cleaning_execution_schedules set {field} updatedById={userId},remark_of_schedule='{request.remark}',updatedAt='{UtilsRepository.GetUTCTime()}' where scheduleId = {request.scheduleId};";
            //  $" Update cleaning_execution_items set status = {(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} where scheduleId = {request.scheduleId} ;";

            int val = await Context.ExecuteNonQry<int>(scheduleQuery).ConfigureAwait(false);

            if (request?.cleanedEquipmentIds?.Length > 0)
            {
                string equipIds = (request?.cleanedEquipmentIds?.Length > 0 ? " " + string.Join(" , ", request.cleanedEquipmentIds) + " " : string.Empty);

                string cleanedQuery = $"Update cleaning_execution_items set status = {status1},executionDay={request.cleaningDay},cleanedById={userId},cleanedAt= '{UtilsRepository.GetUTCTime()}' where executionId = {request.executionId} and assetId IN ({equipIds}); ";

                int val2 = await Context.ExecuteNonQry<int>(cleanedQuery).ConfigureAwait(false);
            }

            if (request.abandonedEquipmentIds.Length > 0)
            {
                string equipIds2 = string.Join(" , ", request.abandonedEquipmentIds);

                string abandoneQuery = $"Update cleaning_execution_items set status = {abandonStatus1},executionDay={request.cleaningDay},abandonedById={userId},abandonedAt= '{UtilsRepository.GetUTCTime()}' where executionId = {request.executionId} and assetId IN ({equipIds2}); ";
                int val3 = await Context.ExecuteNonQry<int>(abandoneQuery).ConfigureAwait(false);
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_EXECUTION, request.scheduleId, 0, 0, "schedule Updated", CMMS.CMMS_Status.VEG_TASK_UPDATED, userId);

            CMDefaultResponse response = new CMDefaultResponse(request.scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Updated Successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> AbandonScheduleVegetation(CMApproval request, int userId)
        {
            int status = (int)CMMS.CMMS_Status.MC_TASK_ABANDONED;
            int equipstatus = (int)CMMS.CMMS_Status.EQUIP_ABANDONED;
            int notStatus = (int)CMMS.CMMS_Status.MC_TASK_COMPLETED;
            if (moduleType == 2)
            {
                status = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED;
                notStatus = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;
            }

            string Query = $"Update cleaning_execution_schedules set status = {status}, abandonedById={userId},abandonedAt='{UtilsRepository.GetUTCTime()}' ,remark = '{request.comment}' where scheduleId = {request.id} ;" +
                 $"Update cleaning_execution_items set status = {equipstatus}, abandonedById={userId},abandonedAt='{UtilsRepository.GetUTCTime()}'  where scheduleId = {request.id} and  status NOT IN ( {notStatus} );";


            int val = await Context.ExecuteNonQry<int>(Query).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MC_TASK, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            return response;
        }
        internal async Task<CMDefaultResponse> EndScheduleExecutionVegetation(int scheduleId, int userId)
        {
            int status;
            status = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;
            string scheduleQuery = $"Update cleaning_execution_schedules set status = {status},endedById={userId},endedAt='{UtilsRepository.GetUTCTime()}' where scheduleId = {scheduleId}; ";
            //$"Update cleaning_execution_items set status = {status} where scheduleId = {scheduleId}";

            await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_EXECUTION, scheduleId, 0, 0, "schedule Execution Completed", (CMMS.CMMS_Status)status, userId);

            CMDefaultResponse response = new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule Execution Completed");
            return response;
        }
        internal async Task<CMDefaultResponse> ApproveScheduleExecutionVegetation(ApproveMC request, int userID)
        {
            int status;
            status = (int)CMMS.CMMS_Status.VEG_TASK_END_APPROVED;
            string approveQuery = $"Update cleaning_execution_schedules set status= {status} ,approvedById={userID}, remark='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}' where  scheduleId= {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_EXECUTION, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution Schedule Approved");
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
        internal async Task<CMDefaultResponse> ApproveEndExecutionVegetation(ApproveMC request, int userID)
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

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_EXECUTION, request.id, 0, 0, request.comment, CMMS.CMMS_Status.VEG_PLAN_APPROVED, userID);

            CMDefaultResponse response = new CMRescheduleApprovalResponse(taskid, request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution End Approved To Vegetation Task {taskid}");

            return response;

        }
        internal async Task<CMDefaultResponse> ReAssignTaskVegetation(int task_id, int assign_to, int userID)
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

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, task_id, 0, 0, $" Task Assigned to user_Id {assign_to}", CMMS.CMMS_Status.VEG_TASK_ASSIGNED, userID);
            return response;
        }
        internal async Task<CMDefaultResponse> AbandonExecutionVegetation(CMApproval request, int userId)
        {
            int status;
            int notStatus;

            status = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED;

            string Query = $"Update cleaning_execution set status = {status},abandonedById={userId},abandonedAt='{UtilsRepository.GetUTCTime()}' ,reasonForAbandon = '{request.comment}' where id = {request.id};";

            await Context.GetData<CMMCExecutionSchedule>(Query).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            return response;
        }
        internal async Task<CMDefaultResponse> RejectAbandonExecutionVegetation(CMApproval request, int userId)
        {
            int status = (int)CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED;
            int notStatus = (int)CMMS.CMMS_Status.MC_TASK_COMPLETED;
            if (moduleType == 2)
            {
                status = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED_REJECTED;
                notStatus = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;
            }

            string Query = $"Update cleaning_execution set status = {status},abandonedById={userId},abandonedAt='{UtilsRepository.GetUTCTime()}' ,reasonForAbandon = '{request.comment}' where id = {request.id};";

            await Context.GetData<CMMCExecutionSchedule>(Query).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MC_TASK, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            return response;
        }

        internal async Task<CMDefaultResponse> ApproveAbandonExecutionVegetation(CMApproval request, int userId)
        {

            int status = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED_APPROVED;
            int notStatus = (int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED;



            string Query = $"Update cleaning_execution set status = {status},abandonedById={userId},abandonedAt='{UtilsRepository.GetUTCTime()}' ,reasonForAbandon = '{request.comment}' where id = {request.id};";
            Query += $" Update cleaning_execution_schedules set status = {status} where executionId = {request.id} and  status  IN ( {notStatus} ) ;";
            await Context.GetData<CMMCExecutionSchedule>(Query).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            return response;
        }
        internal async Task<CMDefaultResponse> LinkPermitToVegetation(int scheduleId, int permit_id, int userId)
        {
            /*
            * Primary Table - PMSchedule
            * Set the required fields in primary table for linling permit to MC
            * Code goes here
           */
            CMDefaultResponse response;
            string statusQry = $"SELECT  status,ifnull(ptw_id,0) ptw_id FROM cleaning_execution_schedules WHERE  scheduleId= {scheduleId}";
            DataTable dt1 = await Context.FetchData(statusQry).ConfigureAwait(false);
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)Convert.ToInt32(dt1.Rows[0][0]);
            int ptw_id = Convert.ToInt32(dt1.Rows[0][1]);


            if (ptw_id > 0)
            {
                return new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.FAILURE, "Permit is already Linked to scheduled");
            }
            string permitQuery = "SELECT ptw.id as ptw_id, ptw.code as ptw_code, ptw.title as ptw_title, ptw.status as ptw_status " +
                                    "FROM " +
                                        "permits as ptw " +
                                    $"WHERE ptw.id = {permit_id} ;";
            List<ScheduleLinkedPermit> permit = await Context.GetData<ScheduleLinkedPermit>(permitQuery).ConfigureAwait(false);
            if (permit.Count == 0)
                return new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.FAILURE, $"Permit {permit_id} does not exist.");
            string myQuery = "UPDATE cleaning_execution_schedules  SET " +
                                $"ptw_id = {permit[0].ptw_id}, " +
                                $"status = {(int)CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW}, " +
                                $"updatedAt = '{UtilsRepository.GetUTCTime()}', " +
                                $"updatedById = '{userId}' " +
                                $"WHERE scheduleId = {scheduleId};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_EXECUTION, scheduleId, CMMS.CMMS_Modules.PTW, permit_id, "PTW linked to MC", CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW, userId);

            response = new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Permit {permit_id} linked to MC Schedule {scheduleId} Successfully");

            return response;
        }

        internal async Task<CMDefaultResponse> CompleteExecutionVegetation(CMMCExecution request, int userId)
        {
            return null;
        }

        internal async Task<CMDefaultResponse> RejectEndExecutionVegetation(ApproveMC request, int userID)
        {

            int status;

            status = (int)CMMS.CMMS_Status.VEG_TASK_END_REJECTED;
            string approveQuery = $"Update cleaning_execution set status= {status},rejectedById={userID},executionRejectedRemarks='{request.comment}', rejectedAt='{UtilsRepository.GetUTCTime()}', executionRejectedAt = '{UtilsRepository.GetUTCTime()}' where id = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, request.execution_id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.execution_id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution End Rejected");
            return response;

        }

        internal async Task<CMDefaultResponse> RejectExecutionVegetation(CMApproval request, int userID)
        {

            int status = (int)CMMS.CMMS_Status.MC_TASK_REJECTED;

            if (moduleType == 2)
            {
                status = (int)CMMS.CMMS_Status.VEG_TASK_REJECTED;
            }

            string approveQuery = $"Update cleaning_execution set status= {status},rejectedById={userID},remark='{request.comment}', rejectedAt='{UtilsRepository.GetUTCTime()}' where id = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution Rejected");
            return response;
        }

        internal async Task<CMDefaultResponse> ApproveExecutionVegetation(CMApproval request, int userID)
        {

            int status;
            int status2;


            status = (int)CMMS.CMMS_Status.VEG_TASK_APPROVED;
            status2 = (int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED;



            string approveQuery = $"Update cleaning_execution set status= {status} ,approvedById={userID},rescheduled = 1, remark='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            string mainQuery = $"INSERT INTO cleaning_execution(planId,moduleType,facilityId,frequencyId,noOfDays,startDate,assignedTo,status,prevTaskId,prevTaskDoneDate)  " +
                              $"select planId as planId,{moduleType} as moduleType,facilityId,frequencyId, noOfDays ,DATE_ADD(startDate, INTERVAL freq.days DAY),assignedTo," +
                              $"{status2} as status, {request.id} as prevTaskId, '{UtilsRepository.GetUTCTime()}' as prevTaskDoneDate " +
                              $" from cleaning_execution left join frequency as freq on cleaning_execution.frequencyId = freq.id where  cleaning_execution.id = {request.id}; " +
                              $"SELECT LAST_INSERT_ID(); ";

            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int taskid = Convert.ToInt32(dt3.Rows[0][0]);

            string scheduleQry = $"INSERT INTO cleaning_execution_schedules(executionId,planId,actualDay,cleaningType,status) " +
                                $"select {taskid} as executionId,planId as planId, actualDay,cleaningType,{status2} as status from cleaning_execution_schedules where executionId = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);

            string equipmentQry = $"INSERT INTO cleaning_execution_items (`executionId`,`moduleType`,`scheduleId`,`assetId`,`{measure}`,`plannedDate`,`plannedDay`,`createdById`,`createdAt`,status) SELECT '{taskid}' as executionId,item.moduleType,schedule.scheduleId,item.assetId,{measure},DATE_ADD('{DateTime.Now.ToString("yyyy-MM-dd")}',interval schedule.actualDay DAY) as plannedDate,schedule.actualDay,schedule.createdById,schedule.createdAt ,{(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} as status from cleaning_execution_items as item join cleaning_execution_schedules as schedule on item.executionId = schedule.executionId and item.plannedDay = schedule.actualDay where schedule.executionId = {taskid}";

            var retVal = await Context.ExecuteNonQry<int>(equipmentQry).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, request.id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userID);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, taskid, 0, 0, "Execution scheduled", CMMS.CMMS_Status.VEG_TASK_SCHEDULED, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution Approved");
            return response;


        }

        internal async Task<CMDefaultResponse> RejectScheduleExecutionVegetation(ApproveMC request, int userId)
        {

            int status;

            status = (int)CMMS.CMMS_Status.VEG_TASK_END_REJECTED;



            string approveQuery = $"Update cleaning_execution_schedules set status= {status},rejectedById={userId},remark='{request.comment}', rejectedAt='{UtilsRepository.GetUTCTime()}' where scheduleId = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.VEGETATION_TASK, request.schedule_id, 0, 0, request.comment, (CMMS.CMMS_Status)status, userId);

            CMDefaultResponse response = new CMDefaultResponse(request.schedule_id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution Schedule Rejected");
            return response;

        }

        internal async Task<List<CMMCPlan>> GetPlanList(int facilityId, string facilitytimeZone)
        {
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN mc.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string myQuery1 = $"select mc.planId,mc.status, mc.frequencyId,mc.assignedTo as assignedToId,mc.startDate,mc.durationDays as noOfCleaningDays, " +
                $"mc.facilityId,mc.title,CONCAT(createdBy.firstName, createdBy.lastName) as createdBy , " +
                $"mc.createdAt,CONCAT(approvedBy.firstName, approvedBy.lastName) as approvedBy,mc.approvedAt,freq.name as" +
                $" frequency,CONCAT(assignedTo.firstName, ' ', assignedTo.lastName) as assignedTo,mc.durationDays,{statusOut} as status_short" +
                $" from cleaning_plan as mc LEFT JOIN Frequency as freq on freq.id = mc.frequencyId " +
             $"LEFT JOIN users as assignedTo ON assignedTo.id = mc.assignedTo " +
            $"LEFT JOIN users as createdBy ON createdBy.id = mc.createdById " +
            $"LEFT JOIN users as approvedBy ON approvedBy.id = mc.approvedById where moduleType={moduleType} ";

            if (facilityId > 0)
            {
                myQuery1 += $" and facilityId={facilityId} ";
            }

            List<CMMCPlan> _ViewMCPlanList = await Context.GetData<CMMCPlan>(myQuery1).ConfigureAwait(false);
            foreach (var mclist in _ViewMCPlanList)
            {
                if (mclist != null && mclist.approvedAt != null)
                    mclist.approvedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, mclist.approvedAt);
                if (mclist != null && mclist.createdAt != null)
                    mclist.createdAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, mclist.createdAt);
                if (mclist != null && mclist.startDate != null)
                    mclist.startDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, mclist.startDate);
            }
            return _ViewMCPlanList;

        }
        internal async Task<List<CMMCTaskList>> GetTaskList(int facilityId, string facilitytimeZone)
        {
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN mc.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string myQuery1 = $"select mc.id as executionId ,mp.title,mc.planId,mc.status, CONCAT(createdBy.firstName, createdBy.lastName) as responsibility , mc.startDate, mc.endedAt as doneDate,mc.prevTaskDoneDate as lastDoneDate,freq.name as frequency,mc.noOfDays, {statusOut} as status_short from cleaning_execution as mc left join cleaning_plan as mp on mp.planId = mc.planId LEFT JOIN Frequency as freq on freq.id = mp.frequencyId LEFT JOIN users as createdBy ON createdBy.id = mc.assignedTo LEFT JOIN users as approvedBy ON approvedBy.id = mc.approvedByID where mc.moduleType={moduleType} ";

            if (facilityId > 0)
            {
                myQuery1 += $" and mc.facilityId = {facilityId} ";
            }
            List<CMMCTaskList> _ViewMCTaskList = await Context.GetData<CMMCTaskList>(myQuery1).ConfigureAwait(false);
            foreach (var ViewMCTaskList in _ViewMCTaskList)
            {
                if (ViewMCTaskList != null && ViewMCTaskList.doneDate != null)
                    ViewMCTaskList.doneDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, ViewMCTaskList.doneDate);
                if (ViewMCTaskList != null && ViewMCTaskList.startDate != null)
                    ViewMCTaskList.startDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, ViewMCTaskList.startDate);
                if (ViewMCTaskList != null && ViewMCTaskList.lastDoneDate != null)
                    ViewMCTaskList.lastDoneDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, ViewMCTaskList.lastDoneDate);

            }
            return _ViewMCTaskList;
        }
        internal virtual async Task<List<CMMCTaskEquipmentList>> GetTaskEquipmentList(int taskId, string facilitytimeZone)
        {

            string status = "";
            status = $" case WHEN task.status = {(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} THEN 1 ELSE 0 END as isPending , " +
                $"case WHEN task.status = {(int)CMMS.CMMS_Status.EQUIP_CLEANED} THEN 1 ELSE 0 END as isCleaned," +
                $"case WHEN task.status = {(int)CMMS.CMMS_Status.EQUIP_ABANDONED} THEN 1 ELSE 0 END as isAbandoned , ";

            string smbQuery = $"select task.assetId as smbId, assets.name as smbName , assets.parentId as parentId , task.{measure}, {status} " +
                " task.plannedDate AS scheduledAt, task.cleanedAt AS cleanedAt, task.abandonedAt AS abandonedAt" +
                $", plannedDay as scheduledDay, executionDay as executedDay from cleaning_execution_items as task left join assets on assets.id = task.assetId where task.executionId ={taskId}";

            List<CMSMB> smbs = await Context.GetData<CMSMB>(smbQuery).ConfigureAwait(false);
            string invQuery = $"Select id as invId , name as invName from assets where id in (Select  parent.id as id from cleaning_execution_items as task left join  assets on assets.id = task.assetId left join assets as parent on parent.id = assets.parentId where task.executionId ={taskId} group by assets.parentId)";
            List<CMMCTaskEquipmentList> invs = await Context.GetData<CMMCTaskEquipmentList>(invQuery).ConfigureAwait(false);
            foreach (CMMCTaskEquipmentList inv in invs)
            {
                inv.moduleQuantity = 0;
                inv.area = 0;
                foreach (CMSMB smb in smbs)
                {
                    if (inv.invId == smb.parentId)
                    {
                        inv.moduleQuantity += smb.moduleQuantity;
                        inv.area += smb.area;
                        inv?.smbs.Add(smb);
                    }
                }

            }
            foreach (var smb in smbs)
            {
                if (smb != null && smb.abandonedAt != null)
                    smb.abandonedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, smb.abandonedAt);
                if (smb != null && smb.cleanedAt != null)
                    smb.cleanedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, smb.cleanedAt);
                if (smb != null && smb.scheduledAt != null)
                    smb.scheduledAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, smb.scheduledAt);
            }
            return invs;
        }
        internal async Task<CMMCExecution> GetExecutionDetails(int exectionId, string facilitytimeZone)
        {

            string statusEx = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusEx += $"WHEN ex.status = {status.Key} THEN '{status.Value}' ";
            }
            statusEx += $"ELSE 'Invalid Status' END";

            string statusSc = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusSc += $"WHEN schedule.status = {status.Key} THEN '{status.Value}' ";
            }
            statusSc += $"ELSE 'Invalid Status' END";

            string statusEquip = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusEquip += $"WHEN item.status = {status.Key} THEN '{status.Value}' ";
            }
            statusEquip += $"ELSE 'Invalid Status' END";

            string executionQuery = $"select ex.id as executionId, ex.status,ex.planId ,ex.startDate,CONCAT(assignedTo.firstName, assignedTo.lastName) as assignedTo , " +
                $"plan.title, CONCAT(createdBy.firstName, createdBy.lastName) as plannedBy ,plan.createdAt as plannedAt,freq.name as frequency, CONCAT(startedBy.firstName, startedBy.lastName) as startedBy ," +
                $" ex.executionStartedAt as startedAt , CONCAT(rejectedById.firstName, rejectedById.lastName) as rejectedById,ex.rejectedAt,CONCAT(approvedById.firstName, approvedById.lastName) as approvedById,ex.approvedAt,  {statusEx} as status_short " +
                $"  from cleaning_execution as ex JOIN cleaning_plan as plan on ex.planId = plan.planId " +
                $" LEFT JOIN Frequency as freq on freq.id = plan.frequencyId " +
                $" LEFT JOIN users as createdBy ON createdBy.id = plan.createdById " +
                $" LEFT JOIN users as rejectedById ON rejectedById.id = ex.rejectedById " +
                $" LEFT JOIN users as approvedById ON approvedById.id = ex.approvedById " +
                $" LEFT JOIN users as startedBy ON startedBy.id = ex.executedById " +
                $" LEFT JOIN users as assignedTo ON assignedTo.id = ex.assignedTo where ex.id={exectionId};";


            List<CMMCExecution> _ViewExecution = await Context.GetData<CMMCExecution>(executionQuery).ConfigureAwait(false);

            string scheduleQuery = $"select schedule.scheduleId as scheduleId ,schedule.status ,schedule.executionId, schedule.actualDay as cleaningDay ,schedule.startedAt as start_date,schedule.endedAt as end_date  , " +
                                   $" cp.cleaningType ,CASE cp.cleaningType WHEN 1 then 'Wet' When 2 then 'Dry' when 3 then 'Robotic'  else 'Wet '  end as cleaningTypeName, SUM({measure}) as scheduled , " +
                                   $" permit.id as permit_id,permit.code as  permit_code , CONCAT(rejectedById.firstName, rejectedById.lastName) as rejectedById,schedule.rejectedAt,CONCAT(approvedById.firstName, approvedById.lastName) as approvedById,schedule.approvedAt, " +
                                   $" Case when permit.TBT_Done_By is null or permit.TBT_Done_By = 0 then 0 else 1 end ptw_tbt_done,permit.status as ptw_status ," +
                                   $" SUM(CASE WHEN item.status = {(int)CMMS.CMMS_Status.EQUIP_CLEANED} THEN {measure} ELSE 0 END) as cleaned , SUM(CASE WHEN item.status = {(int)CMMS.CMMS_Status.EQUIP_ABANDONED} THEN {measure} ELSE 0 END) as abandoned , " +
                                   $" SUM(CASE WHEN item.status = {(int)CMMS.CMMS_Status.EQUIP_SCHEDULED} THEN {measure} ELSE 0 END) as pending ,schedule.remark_of_schedule as remark_of_schedule ,schedule.waterUsed, schedule.remark as remark ,{statusSc} as status_short from cleaning_execution_schedules as schedule " +
                                   $" left join cleaning_execution_items as item on schedule.scheduleId = item.scheduleId " +
                                   $" left join permits as permit on permit.id = schedule.ptw_id " +
                                   $" LEFT JOIN users as rejectedById ON rejectedById.id = schedule.rejectedById " +
                                   $" LEFT JOIN users as approvedById ON approvedById.id = schedule.approvedById " +
                                   $" left join cleaning_plan as cp on schedule.planId= cp.planId " +
                                   $" where schedule.executionId = {exectionId} group by schedule.scheduleId;";

            List<CMMCExecutionSchedule> _ViewSchedule = await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);

            //string equipmentQuery = $"select item.assetId as id ,assets.parentId, parent.name as parentName,DATE_ADD(execution.startDate,interval item.plannedDay - 1  DAY) as scheduledAt ,item.cleanedAt,item.abandonedAt,item.status,assets.name as equipmentName,item.abandonedAt, item.moduleQuantity, item.plannedDay as cleaningDay ,{statusEquip} as short_status from cleaning_execution_items as item left join cleaning_execution_schedules as schedule on schedule.executionId = item.executionId left join cleaning_execution as execution on execution.id = item.executionId left join assets on item.assetId = assets.id left join assets as parent on assets.parentId = parent.id  where schedule.executionId = {exectionId} group by item.assetId ;";

            //List<CMMCExecutionEquipment> _ViewEquipment = await Context.GetData<CMMCExecutionEquipment>(equipmentQuery).ConfigureAwait(false);
            //foreach(var equiment in _ViewEquipment)
            //{
            //    if(equiment!=null && equiment.abandonedAt!=null)
            //    equiment.abandonedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, equiment.abandonedAt);
            //    if (equiment != null && equiment.cleanedAt != null)
            //        equiment.cleanedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, equiment.cleanedAt);
            //    if (equiment != null && equiment.scheduledAt != null)
            //        equiment.scheduledAt= await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, equiment.scheduledAt);

            //}

            //foreach(var schedule in _ViewSchedule)
            //{
            //    schedule.equipments = new List<CMMCExecutionEquipment>();

            //    foreach (var equipment in _ViewEquipment)
            //    {
            //        if(schedule.cleaningDay == equipment.cleaningDay)
            //            schedule.equipments.Add(equipment);
            //    }
            //}

            _ViewExecution[0].noOfDays = _ViewSchedule.Count;
            _ViewExecution[0].schedules = _ViewSchedule;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_ViewExecution[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.MC_TASK, _Status_long, null, _ViewExecution[0]);
            _ViewExecution[0].status_long = _longStatus;
            foreach (var item in _ViewSchedule)
            {
                CMMS.CMMS_Status ptw_status1 = (CMMS.CMMS_Status)(item.ptw_status);
                item.status_short_ptw = Status_PTW((int)ptw_status1);

            }

            foreach (var view in _ViewExecution)
            {
                if (view != null && view.abandonedAt != null)
                    view.abandonedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, view.abandonedAt);
                if (view != null && view.plannedAt != null)
                    view.plannedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, view.plannedAt);
                if (view != null && view.startDate != null)
                    view.startDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, view.startDate);
                if (view != null && view.startedAt != null)
                    view.startedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, view.startedAt);

            }
            return _ViewExecution[0];
        }


        internal async Task<CMDefaultResponse> RejectPlan(CMApproval request, int userID)
        {
            string approveQuery = $"Update cleaning_plan set isApproved = 2,status= {(int)CMMS.CMMS_Status.MC_PLAN_REJECTED},approvedById={userID},ApprovalReason='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}' where planId = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MC_PLAN, request.id, 0, 0, request.comment, CMMS.CMMS_Status.MC_PLAN_REJECTED, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Rejected");
            return response;
        }

        internal async Task<CMDefaultResponse> DeletePlan(int planid, int userID)
        {
            string approveQuery = $"Delete from cleaning_plan where planId ={planid};Delete from cleaning_plan_schedules where planId ={planid};Delete from cleaning_plan_items where planId ={planid}"; ;
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MC_PLAN, planid, 0, 0, "Plan Deleted", CMMS.CMMS_Status.MC_PLAN_DELETED, userID);

            CMDefaultResponse response = new CMDefaultResponse(planid, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Deleted");
            return response;
        }
    }
}
