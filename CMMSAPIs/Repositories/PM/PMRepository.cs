using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Models.Notifications;
using System.Numerics;
using CMMSAPIs.Models.WC;

namespace CMMSAPIs.Repositories.PM
{
    public class PMRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public PMRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;
            retValue = "";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PM_PLAN_DRAFT:
                    retValue = "Draft"; break;
                case CMMS.CMMS_Status.PM_PLAN_CREATED:
                    retValue = "Waiting For Approval"; break;
                case CMMS.CMMS_Status.PM_PLAN_REJECTED:
                    retValue = "Rejected"; break;
                case CMMS.CMMS_Status.PM_PLAN_UPDATED:
                    retValue = "Updated"; break;
                case CMMS.CMMS_Status.PM_PLAN_DELETED:
                    retValue = "Deleted"; break;
                case CMMS.CMMS_Status.PM_PLAN_APPROVED:
                    retValue = "Approved"; break;
                default:
                    break;
            }
            return retValue;

        }

        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMPMPlanDetail PlanObj)
        {
            string retValue = " ";


            switch (notificationID)
            {
                case CMMS.CMMS_Status.PM_PLAN_DRAFT:
                    retValue = String.Format("PM Plan in Draft by {0}", PlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_CREATED:
                    retValue = String.Format("PM Plan submitted by {0}", PlanObj.created_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_REJECTED:
                    retValue = String.Format("PM Plan Rejected by {0}", PlanObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_APPROVED:
                    retValue = String.Format("PM Plan Approved by {0}", PlanObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.PM_PLAN_DELETED:
                    //retValue = String.Format("Warranty Claim Dispachted by {0} at {1}", WCObj.dispatched_by, WCObj.dispatched_at);
                    retValue = String.Format("PM Plan Deleted by {0} ", PlanObj.created_by_name);
                    break;
               
                default:
                    break;
            }
            return retValue;

        }
        internal async Task<CMDefaultResponse> CreatePMPlan(CMPMPlanDetail pm_plan, int userID)
        {
            int status = pm_plan.isDraft > 0 ? (int)CMMS.CMMS_Status.PM_PLAN_DRAFT : (int)CMMS.CMMS_Status.PM_PLAN_CREATED;

            string checklistIDsQry = $"SELECT id FROM checklist_number WHERE facility_id = {pm_plan.facility_id} " +
                                        $"AND asset_category_id = {pm_plan.category_id} AND frequency_id = {pm_plan.plan_freq_id} " +
                                        $"AND checklist_type = 1; ";
            DataTable dt1 = await Context.FetchData(checklistIDsQry).ConfigureAwait(false);
            List<int> checklistIDs = dt1.GetColumn<int>("id");
            string assetIDsQry = $"SELECT id FROM assets WHERE facilityId = {pm_plan.facility_id} AND categoryId = {pm_plan.category_id}; ";
            DataTable dt2 = await Context.FetchData(assetIDsQry).ConfigureAwait(false);
            List<int> assetIDs = dt2.GetColumn<int>("id");
            List<int> invalidChecklists = new List<int>();
            List<int> invalidAssets = new List<int>();
            foreach (var map in pm_plan.mapAssetChecklist)
            {
                if(!checklistIDs.Contains(map.checklist_id))
                    invalidChecklists.Add(map.checklist_id);
                if(!assetIDs.Contains(map.asset_id))
                    invalidAssets.Add(map.asset_id);
            }
            if (invalidChecklists.Count > 0 || invalidAssets.Count > 0)
                return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.INVALID_ARG,
                    $"{invalidChecklists.Count} invalid checklists [{string.Join(',', invalidChecklists)}] " +
                    $"and {invalidAssets.Count} invalid assets [{string.Join(',', invalidAssets)}] linked");

            string addPlanQry = $"INSERT INTO pm_plan(plan_name, facility_id, category_id, frequency_id, " +
                                $"status, plan_date,assigned_to, created_by, created_at, updated_by, updated_at) VALUES " +
                                $"('{pm_plan.plan_name}', {pm_plan.facility_id}, {pm_plan.category_id}, {pm_plan.plan_freq_id}, " +
                                $"{status}, '{pm_plan.plan_date.ToString("yyyy-MM-dd")}',{pm_plan.assign_to_id}, " +
                                $"{userID}, '{UtilsRepository.GetUTCTime()}', {userID}, '{UtilsRepository.GetUTCTime()}'); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt3 = await Context.FetchData(addPlanQry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt3.Rows[0][0]);

            string mapChecklistQry = "INSERT INTO pmplanassetchecklist(planId, assetId, checklistId) VALUES ";
            foreach(var map in pm_plan.mapAssetChecklist)
            {
                mapChecklistQry += $"({id}, {map.asset_id}, {map.checklist_id}), ";
            }
            mapChecklistQry = mapChecklistQry.Substring(0, mapChecklistQry.Length - 2) + ";";
            await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, id, 0, 0, "PM Plan added", CMMS.CMMS_Status.PM_PLAN_CREATED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Plan added successfully");
            return response;
        }
        
        internal async Task<List<CMPMPlanList>> GetPMPlanList(int facility_id, int category_id, int frequency_id, DateTime? start_date, DateTime? end_date)
        {
            if (facility_id <= 0)
                throw new ArgumentException("Invalid Facility ID");
            string planListQry = $"SELECT plan.id as plan_id, plan.plan_name, plan.status as status_id, statuses.statusName as status_short, plan.plan_date, " +
                                    $"facilities.id as facility_id, facilities.name as facility_name, category.id as category_id, category.name as category_name, " +
                                    $"frequency.id as plan_freq_id, frequency.name as plan_freq_name, createdBy.id as created_by_id, " +
                                    $"CONCAT(createdBy.firstName, ' ', createdBy.lastName) as created_by_name, plan.created_at,CONCAT(assignedTo.firstName, ' ', assignedTo.lastName) as assigned_to_name, " +
                                    $"updatedBy.id as updated_by_id, CONCAT(updatedBy.firstName, ' ', updatedBy.lastName) as updated_by_name, plan.updated_at " +
                                    $"FROM pm_plan as plan " +
                                    $"LEFT JOIN statuses ON plan.status = statuses.softwareId " +
                                    $"JOIN facilities ON plan.facility_id = facilities.id " +
                                    $"LEFT JOIN assetcategories as category ON plan.category_id = category.id " +
                                    $"LEFT JOIN frequency ON plan.frequency_id = frequency.id " +
                                    $"LEFT JOIN users as createdBy ON createdBy.id = plan.created_by " +
                                    $"LEFT JOIN users as updatedBy ON updatedBy.id = plan.updated_by " +
                                    $"LEFT JOIN users as assignedTo ON assignedTo.id = plan.assigned_to " +
                                    $"WHERE facilities.id = {facility_id} ";
            if (category_id > 0)
                planListQry += $"AND category.id = {category_id} ";
            if (frequency_id > 0)
                planListQry += $"AND frequency.id = {frequency_id} ";
            if (start_date != null)
                planListQry += $"AND plan.plan_date >= {((DateTime)start_date).ToString("yyyy-MM-dd")} ";
            if (end_date != null)
                planListQry += $"AND plan.plan_date <= {((DateTime)start_date).ToString("yyyy-MM-dd")} ";
            planListQry += $";";

            List<CMPMPlanList> plan_list = await Context.GetData<CMPMPlanList>(planListQry).ConfigureAwait(false);

            foreach (var plan in plan_list)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(plan.status_id);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.PM_PLAN, _Status);
                plan.status_short = _shortStatus;
            }

            return plan_list;
        }

        internal async Task<CMPMPlanDetail> GetPMPlanDetail(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Facility ID");
            string planListQry = $"SELECT plan.id as plan_id, plan.plan_name, plan.status as status_id, statuses.statusName as status_short, plan.plan_date, " +
                                    $"facilities.id as facility_id, facilities.name as facility_name, category.id as category_id, category.name as category_name, " +
                                    $"frequency.id as plan_freq_id, frequency.name as plan_freq_name, createdBy.id as created_by_id, " +
                                    $"CONCAT(createdBy.firstName, ' ', createdBy.lastName) as created_by_name, plan.created_at,approvedBy.id as approved_by_id, CONCAT(approvedBy.firstName, ' ', approvedBy.lastName) as approved_by_name, plan.approved_at, rejectedBy.id as rejected_by_id, CONCAT(rejectedBy.firstName, ' ', rejectedBy.lastName) as rejected_by_name, plan.rejected_at,CONCAT(assignedTo.firstName, ' ', assignedTo.lastName) as assigned_to_name, " +
                                    $"updatedBy.id as updated_by_id, CONCAT(updatedBy.firstName, ' ', updatedBy.lastName) as updated_by_name, plan.updated_at " +
                                    $"FROM pm_plan as plan " +
                                    $"LEFT JOIN statuses ON plan.status = statuses.softwareId " +
                                    $"JOIN facilities ON plan.facility_id = facilities.id " +
                                    $"LEFT JOIN assetcategories as category ON plan.category_id = category.id " +
                                    $"LEFT JOIN frequency ON plan.frequency_id = frequency.id " +
                                    $"LEFT JOIN users as createdBy ON createdBy.id = plan.created_by " +
                                    $"LEFT JOIN users as updatedBy ON updatedBy.id = plan.updated_by " +
                                    $"LEFT JOIN users as approvedBy ON approvedBy.id = plan.approved_by " +
                                    $"LEFT JOIN users as rejectedBy ON rejectedBy.id = plan.rejected_by " +
                                    $"LEFT JOIN users as assignedTo ON assignedTo.id = plan.assigned_to " +

                                    $"WHERE plan.id = {id} ";
            List<CMPMPlanDetail>  planDetails = await Context.GetData<CMPMPlanDetail>(planListQry).ConfigureAwait(false);

            if (planDetails.Count == 0)
                return null;

            string assetChecklistsQry = $"SELECT assets.id as asset_id, assets.name as asset_name, parent.id as parent_id, parent.name as parent_name, assets.moduleQuantity as module_qty, checklist.id as checklist_id, checklist.checklist_number as checklist_name " +
                                        $"FROM pmplanassetchecklist as planmap " +
                                        $"LEFT JOIN assets ON assets.id = planmap.assetId " +
                                        $"LEFT JOIN assets as parent ON assets.parentId = parent.id " +
                                        $"LEFT JOIN checklist_number as checklist ON checklist.id = planmap.checklistId " +
                                        $"WHERE planmap.planId = {id};";
            List<AssetCheckList> assetCheckLists = await Context.GetData<AssetCheckList>(assetChecklistsQry).ConfigureAwait(false);
            planDetails[0].mapAssetChecklist = assetCheckLists;

            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(planDetails[0].status_id);
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.PM_PLAN, _Status);
            planDetails[0].status_short = _shortStatus;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(planDetails[0].status_id);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.WARRANTY_CLAIM, _Status_long, planDetails[0]);
            planDetails[0].status_long = _longStatus;

            return planDetails[0];
        }
        internal async Task<List<CMScheduleData>> GetScheduleData(int facility_id, int category_id)
        {
            /*
             * Primary Table - PMSchedule
             * Read All properties mention in model and return list
             * Code goes here
            */

            string myQuery = "SELECT assets.id as asset_id, assets.name as asset_name, category.id as category_id, category.name as category_name, block.id as block_id, block.name as block_name " +
                                "FROM assets " +
                                "LEFT JOIN assetcategories as category ON assets.categoryId = category.id " +
                                "LEFT JOIN facilities as block ON assets.blockId = block.id ";
            if (facility_id > 0)
            {
                myQuery += " WHERE assets.facilityId = " + facility_id;
                if (category_id > 0)
                {
                    myQuery += " AND category.id = " + category_id;
                }
                else
                {
                    throw new ArgumentException("Invalid Category ID");
                }
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            myQuery += " ORDER BY assets.id ASC;";
            List<CMScheduleData> _scheduleList = await Context.GetData<CMScheduleData>(myQuery).ConfigureAwait(false);
            foreach(CMScheduleData schedule in _scheduleList)
            {
                string query2 = "SELECT a.id as schedule_id, frequency.id as frequency_id, frequency.name as frequency_name, " +
                                    "a.PM_Schedule_date as schedule_date FROM pm_schedule as a " +
                                    "RIGHT JOIN frequency ON a.PM_Frequecy_id = frequency.id " +
                                    "WHERE a.PM_Schedule_date = (SELECT CASE WHEN MAX(b.PM_Schedule_date) IS NOT NULL THEN MAX(b.PM_Schedule_date) ELSE NULL END AS schdate " +
                                    "FROM pm_schedule as b WHERE a.Asset_id = b.Asset_id AND a.PM_Frequecy_id = b.PM_Frequecy_id AND b.status NOT IN " +
                                    $"({(int)CMMS.CMMS_Status.PM_CANCELLED}, {(int)CMMS.CMMS_Status.PM_APPROVED}) AND " +
                                    $"PM_Rescheduled = 0) AND a.Asset_id = {schedule.asset_id} GROUP BY frequency.id ORDER BY frequency.id;";
                List<ScheduleFrequencyData> _freqData = await Context.GetData<ScheduleFrequencyData>(query2).ConfigureAwait(false);
                schedule.frequency_dates = _freqData;
            }
            return _scheduleList;
        }

        internal async Task<List<CMDefaultResponse>> SetScheduleData(CMSetScheduleData request, int userID)
        {
            /*
             * Primary Table - PMSchedule
             * Set All properties mention in model and return list
             * Code goes here
            */
            List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();
            string myQuery1 = $"SELECT id, CONCAT(firstName,' ',lastName) as full_name, loginId as user_name, mobileNumber as contact_no " +
                                        $"FROM users WHERE id = {userID};";
            List<CMUser> user = await Context.GetData<CMUser>(myQuery1).ConfigureAwait(false);
            string myQuery2 = $"SELECT id, name, address, city, state, country, zipcode as pin FROM facilities WHERE id = {request.facility_id};";
            List<CMFacility> facility = await Context.GetData<CMFacility>(myQuery2).ConfigureAwait(false);
            foreach(var asset_schedule in request.asset_schedules)
            {
                CMDefaultResponse response = null;
                string myQuery3 = $"SELECT id, facilityId, categoryId, name FROM assets WHERE id = {asset_schedule.asset_id};";
                List<CMAsset> asset = await Context.GetData<CMAsset>(myQuery3).ConfigureAwait(false);
                if (asset[0].facilityId != request.facility_id)
                {
                    response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.INVALID_ARG, $"Facility ID {request.facility_id} does not include Asset ID {asset_schedule.asset_id}");
                    responseList.Add(response);
                    continue;
                }
                string myQuery4 = $"SELECT serialNumber, blockId FROM assets WHERE id = {asset_schedule.asset_id};";
                DataTable dt = await Context.FetchData(myQuery4).ConfigureAwait(false);
                string serialNumber = Convert.ToString(dt.Rows[0]["serialNumber"]);
                string blockId = Convert.ToString(dt.Rows[0]["blockId"]);
                string myQuery5 = $"SELECT id, name, description FROM assetcategories WHERE id = {asset[0].categoryId};";
                List<CMAssetCategory> category = await Context.GetData<CMAssetCategory>(myQuery5).ConfigureAwait(false);
                if (category.Count == 0)
                {
                    CMAssetCategory cat = new CMAssetCategory();
                    cat.id = 0;
                    cat.name = "Others";
                    cat.description = "Others";
                    category.Add(cat);
                }
                foreach(var frequency_schedule in asset_schedule.frequency_dates)
                {
                    string myQuery6 = $"SELECT * FROM frequency WHERE id = {frequency_schedule.frequency_id};";
                    List<CMFrequency> frequency = await Context.GetData<CMFrequency>(myQuery6).ConfigureAwait(false);
                    string myQuery7 = "SELECT id as schedule_id, PM_Schedule_date as schedule_date, Facility_id as facility_id, Asset_id as asset_id, PM_Frequecy_id as frequency_id " +
                                        $"FROM pm_schedule WHERE Asset_id = {asset_schedule.asset_id} AND PM_Frequecy_id = {frequency_schedule.frequency_id} " +
                                        $"AND status NOT IN ({(int)CMMS.CMMS_Status.PM_CANCELLED}) AND PM_Rescheduled = 0;";
                    List<ScheduleIDData> scheduleData = await Context.GetData<ScheduleIDData>(myQuery7).ConfigureAwait(false);
                    if(scheduleData.Count > 0)
                    {
                        if(frequency_schedule.schedule_date != null)
                        {
                            string updateQry = $"UPDATE pm_schedule SET PM_Schedule_date = '{((DateTime)frequency_schedule.schedule_date).ToString("yyyy'-'MM'-'dd")}', " +
                                                $"PM_Schedule_updated_by = {userID}, PM_Schedule_updated_date = '{UtilsRepository.GetUTCTime()}' " + 
                                                $"WHERE id = {scheduleData[0].schedule_id};";
                            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, scheduleData[0].schedule_id, 0, 0, "PM Schedule Details Updated", CMMS.CMMS_Status.PM_UPDATED, userID);
                            response = new CMDefaultResponse(scheduleData[0].schedule_id, CMMS.RETRUNSTATUS.SUCCESS, "Schedule date updated successfully");
                            responseList.Add(response);
                        }
                        else
                        {
                            string deleteQry = $"DELETE FROM pm_schedule WHERE id = {scheduleData[0].schedule_id};";
                            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, scheduleData[0].schedule_id, 0, 0, "PM Schedule Deleted", CMMS.CMMS_Status.PM_DELETED, userID);
                            response = new CMDefaultResponse(scheduleData[0].schedule_id, CMMS.RETRUNSTATUS.SUCCESS, "Schedule data deleted due to empty date");
                            responseList.Add(response);
                        }
                    }
                    else
                    {
                        if(frequency_schedule.schedule_date != null)
                        {
                            string mainQuery = $"INSERT INTO pm_schedule(PM_Schedule_Date, PM_Frequecy_Name, PM_Frequecy_id, PM_Frequecy_Code, " +
                               $"Facility_id, Facility_Name, Facility_Code, Block_Id, Block_Code, Asset_Category_id, Asset_Category_Code, Asset_Category_name, " +
                               $"Asset_id, Asset_Code, Asset_Name, PM_Schedule_User_id, PM_Schedule_User_Name, PM_Schedule_Emp_id, PM_Schedule_Emp_name, " +
                               $"PM_Schedule_created_date, Asset_Sno, status, status_updated_at) VALUES " +
                               $"('{((DateTime)frequency_schedule.schedule_date).ToString("yyyy'-'MM'-'dd")}', '{frequency[0].name}', {frequency[0].id}, 'FRC{frequency[0].id}', " +
                               $"{facility[0].id}, '{facility[0].name}', 'FAC{facility[0].id + 1000}', {blockId}, 'BLOCK{blockId}', {category[0].id}, 'AC{category[0].id + 1000}', '{category[0].name}', " +
                               $"{asset[0].id}, 'INV{asset[0].id}', '{asset[0].name}', {user[0].id}, '{user[0].full_name}', {user[0].id}, '{user[0].full_name}', " +
                               $"'{UtilsRepository.GetUTCTime()}', '{serialNumber}', {(int)CMMS.CMMS_Status.PM_SUBMIT}, '{UtilsRepository.GetUTCTime()}'); SELECT LAST_INSERT_ID();";
                            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
                            int id = Convert.ToInt32(dt2.Rows[0][0]);
                            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_SCHEDULE, id, 0, 0, "PM Schedule Created", CMMS.CMMS_Status.PM_SUBMIT, userID);
                            response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "PM Schedule data inserted successfully");
                            responseList.Add(response);
                        }
                        else
                        {
                            response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.INVALID_ARG, "Date cannot be null");
                            responseList.Add(response);
                        }
                    }
                }
            }

            string setCodeNameQuery = "UPDATE pm_schedule " +
                                        "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                        "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                        "PM_Schedule_Number = CONCAT('SCH',id), " +
                                        "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
            await Context.ExecuteNonQry<int>(setCodeNameQuery);
            return responseList;
        }

        internal async Task<CMDefaultResponse> ApprovePMPlan(CMApproval request, int userId)
        {
          
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }
            string approveQuery = $"Update pm_plan set status = {(int)CMMS.CMMS_Status.PM_PLAN_APPROVED}, approved_at = '{UtilsRepository.GetUTCTime()}', " +
                $" remarks = '{request.comment}',  " +
                $" approved_by = {userId}" +
                $" where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            string mainQuery = $"INSERT INTO pm_task(plan_id,facility_id,category_id,frequency_id,plan_Date,assigned_to,status)  " +
                               $"select id as plan_id,facility_id,category_id,frequency_id,plan_Date,assigned_to,{(int)CMMS.CMMS_Status.PM_SCHEDULED} as status from pm_plan where id = {request.id}; " +
                               $"SELECT LAST_INSERT_ID(); ";
            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt3.Rows[0][0]);

            string scheduleQry = $"INSERT INTO pm_schedule(task_id,plan_id,Asset_id,checklist_id,status) " +
                                $"select {id} as task_id,planId as plan_id, assetId as Asset_id, checklistId as checklist_id,{(int)CMMS.CMMS_Status.PM_SCHEDULED} as status from pmplanassetchecklist where planId = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);
            
            string setCodeNameQuery = "UPDATE pm_schedule " +
                                        "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                        "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                        "PM_Schedule_Number = CONCAT('SCH',id), " +
                                        "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
            await Context.ExecuteNonQry<int>(setCodeNameQuery);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, request.id, 0, 0, "PM Plan Approved ", CMMS.CMMS_Status.PM_PLAN_APPROVED);

            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.APPROVED, new[] { _WCList[0].created_by }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "PM Plan Approved Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectPMPlan(CMApproval request, int userId)
        {
           
            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string approveQuery = $"Update pm_plan set status = {(int)CMMS.CMMS_Status.PM_PLAN_REJECTED} , " +
                $"remarks = '{request.comment}',  " +
                $"rejected_by = {userId}, rejected_at = '{UtilsRepository.GetUTCTime()}' " +
                $" where id = {request.id} ";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, request.id, 0, 0, "PM Plan Rejected ", CMMS.CMMS_Status.PM_PLAN_REJECTED);

            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.REJECTED, new[] { _WCList[0].created_by }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "PM Plan Rejected Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> DeletePMPlan(int planId, int userID)
        {
            string approveQuery = $"Delete from pm_plan where id = {planId}; Delete from pmplanassetchecklist where planId = {planId}; ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, planId, 0, 0, $"PM Plan Deleted by user {userID}", CMMS.CMMS_Status.PM_PLAN_DELETED);

            CMDefaultResponse response = new CMDefaultResponse(planId, CMMS.RETRUNSTATUS.SUCCESS, $" PM Plan Deleted");
            return response;
        }
    }
}
