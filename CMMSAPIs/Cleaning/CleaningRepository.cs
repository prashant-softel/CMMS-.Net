using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CMMSAPIs.BS.Facility;
using CMMSAPIs.Models.SM;

namespace CMMSAPIs.Repositories.CleaningRepository
{

    public class CleaningRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public int moduleType ;
        public static string measure = "moduleQuantity";
        public CleaningRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);

            if (moduleType == (int)CMMS.cleaningType.Vegetation)
            {
                measure = "area";
            }
        }  

    private Dictionary<int, string> StatusDictionary = new Dictionary<int, string>()
        {
            { (int)CMMS.CMMS_Status.MC_PLAN_DRAFT, "Draft" },          
            { (int)CMMS.CMMS_Status.MC_PLAN_SUBMITTED, "Submitted" },
            { (int)CMMS.CMMS_Status.MC_PLAN_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.MC_PLAN_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.MC_PLAN_DELETED, "Deleted" },
            { (int)CMMS.CMMS_Status.MC_TASK_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.MC_TASK_STARTED, "In Progress" },
            { (int)CMMS.CMMS_Status.MC_TASK_COMPLETED, "Completed" },
            { (int)CMMS.CMMS_Status.MC_TASK_ABANDONED, "Abandoned" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_DRAFT, "Draft" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_SUBMITTED, "Submitted" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_DELETED, "Deleted" },
            { (int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.VEG_TASK_STARTED, "In Progress" },
            { (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED, "Completed" },
            { (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED, "Abandoned" },
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
                    retValue = String.Format("Module Cleaning Plan <{0}> Submitted by {1} ", planObj.planId, planObj.createdBy);
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
                    retValue = String.Format("Module Cleaning Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_STARTED:
                    retValue = String.Format("Module Cleaning Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_COMPLETED:
                    retValue = String.Format("Module Cleaning Task <{0}> Execution Completed by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.MC_TASK_ABANDONED:
                    retValue = String.Format("Module Cleaning Task <{0}>  Execution Abandoned by {1} ", executionObj.id, executionObj.abandonedBy);
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
                    retValue = String.Format("Vegetation Plan <{0}> Deleted by {1} ", planObj.planId, planObj.deletedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_SCHEDULED:
                    retValue = String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_STARTED:
                    retValue = String.Format("Vegetation Task <{0}> Execution started by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_COMPLETED:
                    retValue = String.Format("Vegetation Cleaning Task <{0}> Execution Completed by {1} ", executionObj.id, executionObj.startedBy);
                    break;
                case CMMS.CMMS_Status.VEG_TASK_ABANDONED:
                    retValue = String.Format("Vegetation Task <{0}>  Execution Abandoned by {1} ", executionObj.id, executionObj.abandonedBy);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        internal async Task<List<CMMCPlan>> GetPlanList(int facilityId)
        {
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN mc.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string myQuery1 = $"select mc.planId,mc.title,CONCAT(createdBy.firstName, createdBy.lastName) as createdBy , mc.createdAt,CONCAT(approvedBy.firstName, approvedBy.lastName) as approvedBy,mc.approvedAt,freq.name as frequency,mc.durationDays,{statusOut} as status_short from cleaning_plan as mc LEFT JOIN Frequency as freq on freq.id = mc.frequencyId LEFT JOIN users as createdBy ON createdBy.id = mc.createdById LEFT JOIN users as approvedBy ON approvedBy.id = mc.approvedById where moduleType={moduleType} ";

            if (facilityId > 0)
            {
                myQuery1 += $" and facilityId={facilityId} ";
            }

            List<CMMCPlan> _ViewMCPlanList = await Context.GetData<CMMCPlan>(myQuery1).ConfigureAwait(false);
            return _ViewMCPlanList;
        }
        
        internal async Task<CMDefaultResponse> CreatePlan(List<CMMCPlan> request, int userId)
        {                                     
            int planIds = 0;
            int status = (int)CMMS.CMMS_Status.MC_PLAN_DRAFT;
            int cleaningType;

            if (moduleType == 2)
            {
                status = (int)CMMS.CMMS_Status.VEG_PLAN_DRAFT;
            }

            foreach(CMMCPlan plan in request)
            {
                string qry = "insert into `cleaning_plan` (`moduleType`,`facilityId`,`title`,`description`, `durationDays`,`frequencyId`,`status`,`createdById`,`createdAt`) VALUES " +
                            $"({moduleType},'{plan.facilityId}','{plan.title}','{plan.description}','{plan.noOfCleaningDays}','{plan.frequencyId}',{status},'{userId}','{UtilsRepository.GetUTCTime()}');" +
                             "SELECT LAST_INSERT_ID() as id ;";

                List<CMMCPlan> planQry = await Context.GetData<CMMCPlan>(qry).ConfigureAwait(false);

                var planId = Convert.ToInt16(planQry[0].id.ToString());

                string scheduleQry = " ";
                string equipmentQry = $"insert into `cleaning_plan_items` (`planId`,`moduleType`,`scheduleId`,`assetId`,`plannedDay`,`createdById`,`createdAt`) VALUES ";

                foreach (var schedule in plan.schedules)
                {
                    cleaningType = schedule.cleaningType;

                    if (moduleType == 2)                  
                        cleaningType = 0;
                    
                    scheduleQry = "insert into `cleaning_plan_schedules` (`planId`,`moduleType`,`plannedDay`,`cleaningType`,`createdById`,`createdAt`) VALUES ";
                    scheduleQry += $"({planId},{moduleType},{schedule.cleaningDay},{cleaningType},'{userId}','{UtilsRepository.GetUTCTime()}');" +
                                       $"SELECT LAST_INSERT_ID() as id ;";

                    List<CMMCSchedule> schedule_ = await Context.GetData<CMMCSchedule>(scheduleQry).ConfigureAwait(false);
                    var scheduleId = Convert.ToInt16(schedule_[0].id.ToString());

                    foreach (var equipment in schedule.equipments)
                    {
                        equipmentQry += $"({planId},{moduleType},{scheduleId},{equipment.id},{schedule.cleaningDay},'{userId}','{UtilsRepository.GetUTCTime()}'),";                        
                    }

                }

                equipmentQry = equipmentQry.Substring(0, equipmentQry.Length - 1);

                equipmentQry += $"; update cleaning_plan_items left join assets on cleaning_plan_items.assetId = assets.id set cleaning_plan_items.{measure} = assets.{measure} where planId ={planId};";

                await Context.GetData<CMMCPlan>(equipmentQry).ConfigureAwait(false);
                planIds = planId;
            }

            CMDefaultResponse response = new CMDefaultResponse(planIds, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Created Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdatePlan(CMMCPlan request, int userId)
        {
           
            string myQuery = "UPDATE cleaning_plan SET ";
                if (request.title != null && request.title != "")
                    myQuery += $"title = '{request.title}', ";
                if (request.description != null && request.description != "")
                    myQuery += $"description = '{request.description}', ";
                if (request.frequencyId > 0)
                    myQuery += $"frequencyId = {request.frequencyId}, ";
                if (request.noOfCleaningDays > 0)
                    myQuery += $"durationDays = {request.noOfCleaningDays}, ";

                myQuery += $"updatedAt = '{UtilsRepository.GetUTCTime()}', updatedById = {userId} WHERE planId = {request.planId};";

                foreach(var schedule in request.schedules)
               {

                   if (moduleType == 1)
                   {
                    //(CASE WHEN { schedule.cleaningType} = 'Wet' then 1 else WHEN { schedule.cleaningType} = 'Dry' then 2 end)
                    if (schedule.cleaningType != 0)
                        myQuery += $"UPDATE cleaning_plan_schedules SET cleaningType = {schedule.cleaningType},updatedAt = '{UtilsRepository.GetUTCTime()}', updatedById = {userId} where plannedDay ={schedule.cleaningDay} and planId={request.planId};";
                   }

                myQuery += $"Delete from cleaning_plan_items where scheduleId = {schedule.scheduleId};";

                myQuery += $"insert into `cleaning_plan_items` (`planId`,`scheduleId`,`assetId`,`{measure}`,`plannedDay`,`updatedById`,`updatedAt`,`createdById`,`createdAt`) VALUES ";
               
                foreach (var equipment in schedule.equipments)
                  {
                    myQuery += $"({request.planId},{schedule.scheduleId},{equipment.id},7864,{schedule.cleaningDay},{userId},'{UtilsRepository.GetUTCTime()}',(select createdById from cleaning_plan where planId={request.planId}),(select createdAt from cleaning_plan where planId={request.planId})),";
                   // if (equipment.noOfPlanDay > 0)
                   //myQuery += $"UPDATE cleaning_plan_items SET plannedDay ={equipment.noOfPlanDay},updatedAt = '{UtilsRepository.GetUTCTime()}', updatedBy = {userId} where assetId = {equipment.id} and scheduleId={schedule.scheduleId};";
                 }
                myQuery = myQuery.Substring(0, myQuery.Length - 1);
                myQuery += ";";

            }
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            

            CMDefaultResponse response = new CMDefaultResponse(request.planId, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Updated Successfully ");
            return response;
        }
        internal async Task<CMMCPlan> GetPlanDetails(int planId)
        {
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN plan.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string planQuery = $"select plan.planId, CONCAT(createdBy.firstName, createdBy.lastName) as createdBy , plan.createdAt,freq.name as frequency,plan.durationDays as noOfCleaningDays, CONCAT(approvedBy.firstName, approvedBy.lastName) as approvedBy , plan.approvedAt ,plan.status,{statusOut} as short_status from cleaning_plan as plan  LEFT JOIN Frequency as freq on freq.id = plan.frequencyId LEFT JOIN users as createdBy ON createdBy.id = plan.createdById LEFT JOIN users as approvedBy ON approvedBy.id = plan.approvedById where plan.planId={planId}; ";
          
            List<CMMCPlan> _ViewMCPlan = await Context.GetData<CMMCPlan>(planQuery).ConfigureAwait(false);

            string measures = ",count(distinct assets.parentId ) as Invs,count(item.assetId) as smbs ,sum(assets.moduleQuantity) as scheduledModules ,CASE schedule.cleaningType WHEN 1 then 'Wet' When 2 then 'Dry' else 'Wet 'end as cleaningTypeName";

            if (moduleType == 2)
            {
                measures = ", count(distinct assets.blockId) as blocks, count(assets.id) as Invs, sum(assets.area) as scheduledArea ";
            }

            string scheduleQuery = $"select schedule.scheduleId, schedule.plannedDay as cleaningDay {measures} from cleaning_plan_schedules as schedule  LEFT JOIN cleaning_plan_items as item on schedule.scheduleId = item.scheduleId LEFT JOIN assets on assets.id = item.assetId where schedule.planId={planId} group by schedule.scheduleId ORDER BY schedule.scheduleId ASC";


            List<CMMCSchedule> _Schedules = await Context.GetData<CMMCSchedule>(scheduleQuery).ConfigureAwait(false);

            string equipmentQuery = $"select assets.id,assets.parentId as parentId, assets.name as equipmentName,assets.moduleQuantity as moduleQuantity,assets.area ,item.plannedDay as noOfPlanDay  from cleaning_plan_items as item  LEFT JOIN assets  on assets.id = item.assetId where item.planId={planId} ORDER BY item.plannedDay ASC ";

            List<CMMCEquipment> _Equipments = await Context.GetData<CMMCEquipment>(equipmentQuery).ConfigureAwait(false);

            foreach(CMMCSchedule Schedules in _Schedules)
            {
                Schedules.equipments = new List<CMMCEquipment>();
                foreach (CMMCEquipment equip in _Equipments)
                { 
                    if(Schedules.cleaningDay == equip.noOfPlanDay)
                    {
                        Schedules.equipments.Add(equip);
                    }
                    
                }
            }
        
            _ViewMCPlan[0].schedules = _Schedules;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_ViewMCPlan[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.INCIDENT_REPORT, _Status_long, _ViewMCPlan[0],null);
            _ViewMCPlan[0].status_long = _longStatus;


            return _ViewMCPlan[0];
        }
        internal async Task<CMDefaultResponse> ApprovePlan(CMApproval request, int userID)
        {
            string approveQuery = $"Update cleaning_plan set isApproved = 1,status=343 ,approvedById={userID}, ApprovalReason='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}' where planId = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Approved");
            return response;

        }
        internal async Task<CMDefaultResponse> RejectPlan(CMApproval request, int userID)
        {
            string approveQuery = $"Update cleaning_plan set isApproved = 2,status=342 ,approvedById={userID}, ApprovalReason='{request.comment}', approvedAt='{UtilsRepository.GetUTCTime()}' where planId = {request.id}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Rejected");
            return response;
        }

        internal async Task<CMDefaultResponse> DeletePlan(int planid, int userID)
        {
            string approveQuery = $"Delete from cleaning_plan where planId ={planid}";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse(planid, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Deleted");
            return response;
        }
        internal async Task<List<CMMCTaskList>> GetTaskList(int facilityId)
        {
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN mc.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string myQuery1 = $"select mc.id as id ,mc.planId, CONCAT(createdBy.firstName, createdBy.lastName) as responsibility , mc.executionStartedAt as startDate,freq.name as frequency,mc.noOfDays, {statusOut} as status_short from cleaning_execution as mc left join cleaning_plan as mp on mp.planId = mc.planId LEFT JOIN Frequency as freq on freq.id = mp.frequencyId LEFT JOIN users as createdBy ON createdBy.id = mc.executedById LEFT JOIN users as approvedBy ON approvedBy.id = mc.approvedByID where mc.moduleType={moduleType}";

            if (facilityId > 0)
            {
                myQuery1 += $" and facilityId={facilityId} ";
            }
            List<CMMCTaskList> _ViewMCTaskList = await Context.GetData<CMMCTaskList>(myQuery1).ConfigureAwait(false);
            return _ViewMCTaskList;
        }
        internal async Task<CMDefaultResponse> StartExecution(int planId, int userId)
        {
            string days = $"SELECT durationDays as noOfDays from cleaning_plan where planId = {planId}";
            List<CMMCExecution> _days = await Context.GetData<CMMCExecution>(days).ConfigureAwait(false);

            string qry = "INSERT INTO `cleaning_execution` (`planId`,`moduleType`,`noOfDays`,`executedById`,`executionStartedAt`,`status`) VALUES " +
                          $"('{planId}',{moduleType},{_days[0].noOfDays},'{userId}','{UtilsRepository.GetUTCTime()}',{(int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED});" +
                          $"SELECT LAST_INSERT_ID() as id ; ";

            List<CMMCExecution> ExecutionQry = await Context.GetData<CMMCExecution>(qry).ConfigureAwait(false);

            var executionId = Convert.ToInt16(ExecutionQry[0].id.ToString());

            string scheduleQry = $"INSERT INTO cleaning_execution_schedules (`planId`,`moduleType`,`executionId`,`actualDay`,`status`,`cleaningType`,`createdById`,`createdAt`) SELECT execution.planId,'{moduleType}' as moduleType,'{executionId}' as executionId ,schedule.plannedDay,'{(int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED}' as status,schedule.cleaningType,execution.executedById,execution.executionStartedAt from cleaning_plan_schedules as schedule left join cleaning_execution as execution on schedule.planId = execution.planId where execution.id={executionId}";

            await Context.ExecuteNonQry<int>(scheduleQry).ConfigureAwait(false);

            string equipmentQry = $"INSERT INTO cleaning_execution_items (`executionId`,`moduleType`,`scheduleId`,`assetId`,`{measure}`,`plannedDate`,`plannedDay`,`createdById`,`createdAt`) SELECT '{executionId}' as executionId,item.moduleType,schedule.scheduleId,item.assetId,{measure},DATE_ADD('{DateTime.Now.ToString("yyyy-MM-dd")}',interval item.plannedDay DAY) as plannedDate,item.plannedDay,schedule.createdById,schedule.createdAt from cleaning_plan_items as item join cleaning_execution_schedules as schedule on item.planId = schedule.planId and item.plannedDay = schedule.actualDay where schedule.executionId = {executionId}";

            var retVal = await Context.ExecuteNonQry<int>(equipmentQry).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (retVal > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS; 
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MODULE_CLEANING, executionId, 0, 0, "Execution Started", CMMS.CMMS_Status.MC_TASK_SCHEDULED);


            CMDefaultResponse response = new CMDefaultResponse(executionId, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Execution started");
            return response;
        }

        internal async Task<CMMCExecution> GetExecutionDetails(int exectionId)
        {

            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN ex.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string executionQuery = $"select plan.title, CONCAT(createdBy.firstName, createdBy.lastName) as plannedBy , plan.createdAt as plannedAt,freq.name as frequency, CONCAT(startedBy.firstName, startedBy.lastName) as startedBy , ex.executionStartedAt as startedAt , {statusOut} as status_short from cleaning_execution as ex JOIN cleaning_plan as plan on ex.planId = plan.planId LEFT JOIN Frequency as freq on freq.id = plan.frequencyId LEFT JOIN users as createdBy ON createdBy.id = plan.createdById LEFT JOIN users as startedBy ON startedBy.id = ex.executedById where ex.id={exectionId};";

            List<CMMCExecution> _ViewExecution = await Context.GetData<CMMCExecution>(executionQuery).ConfigureAwait(false);

            string scheduleQuery = $"select schedule.scheduleId as id ,schedule.actualDay as cleaningDay ,count(as scheduledModules, as cleanedModules , as abandonedModules , as pendingModules ,schedule.waterUsed, schedule.remark ,{statusOut} as status from cleaning_execution_schedules as schedule join cleaning_execution_items as item on schedule.exectionId = item.exectionId where schedule.exectionId = {exectionId} groub by items.scheduleId;";

            List<CMMCExecutionSchedule> _ViewSchedule = await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);

            string equipmentQuery = $"select equipment.id ,schedule.actualDay as cleaningDay ,count(as scheduledModules, as cleanedModules , as abandonedModules , as pendingModules ,schedule.waterUsed, schedule.remark ,{statusOut} as status from cleaning_execution_schedules as schedule join cleaning_execution_items as item on ///schedule.exectionId = item.exectionId where schedule.exectionId = {exectionId} groub by items.scheduleId;";

            List<CMMCEquipment> _ViewEquipment = await Context.GetData<CMMCEquipment>(equipmentQuery).ConfigureAwait(false);

            _ViewExecution[0].schedules= _ViewSchedule;

            foreach(var schedule in _ViewSchedule)
            {
                foreach(var equipment in _ViewEquipment)
                {
                    if(schedule.cleaningDay == equipment.noOfPlanDay)
                        schedule.equipments.Add(equipment);
                }
            }

            return _ViewExecution[0];
        }
        internal async Task<CMDefaultResponse> StartScheduleExecution(int scheduleId, int userId)
        {
            int status = (int)CMMS.CMMS_Status.MC_TASK_STARTED;

            if (moduleType == 2)
            {
                status = (int)CMMS.CMMS_Status.VEG_TASK_STARTED;
            }

            string scheduleQuery = $"Update cleaning_execution_schedules set status = {status},startedById={userId},startedAt='{UtilsRepository.GetUTCTime()}' where scheduleId = {scheduleId}; " +
                 $"Update cleaning_execution_items set status = {status} where scheduleId = {scheduleId}";

            await Context.GetData<CMMCExecutionSchedule>(scheduleQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MODULE_CLEANING, scheduleId, 0, 0, "Mc schedule Started", CMMS.CMMS_Status.MC_TASK_STARTED);

            CMDefaultResponse response = new CMDefaultResponse(scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"Schedule started");
            return response;
        }

        internal async Task<CMDefaultResponse> EndScheduleExecution(CMMCExecutionSchedule request, int userId)
        {
            int status = (int)CMMS.CMMS_Status.MC_TASK_COMPLETED;

            string field = $"waterUsed ={request.waterUsed},";

            if (moduleType == 2)
            {
                status = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;
                field = "";
            }

            string equipIds = "";

            string Query = $"Update cleaning_execution_schedules set status = {status},{field} remark='{request.remark}',endedById={userId},endedAt='{UtilsRepository.GetUTCTime()}' where scheduleId = {request.scheduleId}; ";

            foreach (CMMCEquipment equipment in request.equipments)
            {
                equipIds += $"{equipment.id},";
            }

            equipIds = equipIds.Substring(0, equipIds.Length - 1);

            Query += $"Update cleaning_execution_items set status = {status},executionDay={request.cleaningDay},cleanedById={userId},cleanedAt= '{UtilsRepository.GetUTCTime()}' where executionId = {request.executionId} and assetId IN ({equipIds}); ";

            //Query += $"Update cleaning_execution as execution left join cleaning_execution_items as items on execution.id = items.executionId set execution.status = {status} where moduleType={moduleType} and count(CASE WHEN items.status == {status} or (items.status == {(int)CMMS.CMMS_Status.MC_TASK_COMPLETED} or items.status == {(int)CMMS.CMMS_Status.VEG_TASK_COMPLETED} THEN items.status END) > 0 group by execution.id ";

            await Context.GetData<CMMCExecutionSchedule>(Query).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MODULE_CLEANING, request.scheduleId, 0, 0, "Mc schedule Completed", CMMS.CMMS_Status.MC_TASK_COMPLETED);

            CMDefaultResponse response = new CMDefaultResponse(request.scheduleId, CMMS.RETRUNSTATUS.SUCCESS, $"schedule Ended");
            return response;
        }

        internal async Task<CMDefaultResponse> AbandonExecution(CMApproval request, int userId)
        {
            int status = (int)CMMS.CMMS_Status.MC_TASK_ABANDONED;
            int notStatus = (int)CMMS.CMMS_Status.MC_TASK_COMPLETED;
            if (moduleType == 2)
            {
                status = (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED;
                notStatus = (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED;
            }

            string Query = $"Update cleaning_execution set status = {status},abandonedById={userId},abandonedAt='{UtilsRepository.GetUTCTime()}' ,reasonForAbandon = '{request.comment}' where id = {request.id};" +
                 $"Update cleaning_execution_schedules set status = {status} where executionId = {request.id} and NOT status = {notStatus};" +
                 $"Update cleaning_execution_items set status = {status} where executionId = {request.id} and NOT status = {notStatus};";


            await Context.GetData<CMMCExecutionSchedule>(Query).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.MODULE_CLEANING, request.id, 0, 0, "Mc schedule Completed", CMMS.CMMS_Status.MC_TASK_ABANDONED);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Execution abonded");
            return response;
        }

        internal virtual async Task<List<CMMCEquipmentList>> GetEquipmentList(int facilityId)
        {
            string filter = "";

            if (facilityId > 0)
            {
                filter += $" and facilityId={facilityId} ";
            }

            string invQuery = $"select id as invId, name as invName from assets where categoryId = 2 {filter}";

            List<CMMCEquipmentList> invs = await Context.GetData<CMMCEquipmentList>(invQuery).ConfigureAwait(false);


            string smbQuery = $"select id as smbId, name as smbName , parentId, moduleQuantity from assets where categoryId = 4 {filter}";
           
            List<CMSMB> smbs = await Context.GetData<CMSMB>(smbQuery).ConfigureAwait(false);

            //List<CMSMB> invSmbs = new List<CMSMB>;

            foreach (CMMCEquipmentList inv in invs)
            {   
                foreach (CMSMB smb in smbs)
                {
                    if(inv.invId == smb.parentId)
                    {
                        inv.moduleQuantity += smb.moduleQuantity;
                        inv?.smbs.Add(smb);
                    }
                }

            }
            return invs;
        }

        internal async Task<List<CMVegEquipmentList>> GetVegEquipmentList(int facilityId)
        {
            string filter = "";
            string Query = $"select id as blockId,  name as blockName from facilities  where isBlock = 1";

            if (facilityId > 0)
            {
                filter += $" and facilityId={facilityId} ";
            }

            List<CMVegEquipmentList> blocks = await Context.GetData<CMVegEquipmentList>(Query).ConfigureAwait(false);


            string InvQuery = $"select id as invId, name as invName , blockId ,area from assets where categoryId = 2 {filter} ";

            List<CMInv> Invs = await Context.GetData<CMInv>(InvQuery).ConfigureAwait(false);


            foreach (CMVegEquipmentList block in blocks)
            {
                foreach (CMInv inv in Invs)
                {
                    if (block.blockId == inv.blockId)
                    {
                        block.area += inv.area;
                        block.invs.Add(inv);
                    }

                }
            }
            return blocks;
        }


    }
}
