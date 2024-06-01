using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.PM
{
    public class PMRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;
        private MYSQLDBHelper sqlDBHelper;

        public PMRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(iwebhostobj);

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

            string checklistIDsQry = $"SELECT id FROM checklist_number WHERE facility_id IN ({pm_plan.facility_id},0) " +
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
                if (!checklistIDs.Contains(map.checklist_id))
                    invalidChecklists.Add(map.checklist_id);
                if (!assetIDs.Contains(map.asset_id))
                    invalidAssets.Add(map.asset_id);
            }
            if (invalidChecklists.Count > 0 || invalidAssets.Count > 0)
                return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.INVALID_ARG,
                    $"{invalidChecklists.Count} invalid checklists [{string.Join(',', invalidChecklists)}] " +
                    $"and {invalidAssets.Count} invalid assets [{string.Join(',', invalidAssets)}] linked");

            string addPlanQry = $"INSERT INTO pm_plan(plan_name, facility_id, category_id, frequency_id, " +
                                $"status, plan_date,assigned_to, created_by, created_at, updated_by, updated_at,next_schedule_date) VALUES " +
                                $"('{pm_plan.plan_name}', {pm_plan.facility_id}, {pm_plan.category_id}, {pm_plan.plan_freq_id}, " +
                                $"{status}, '{pm_plan.plan_date.ToString("yyyy-MM-dd")}',{pm_plan.assigned_to_id}, " +
                                $"{userID}, '{UtilsRepository.GetUTCTime()}', {userID}, '{UtilsRepository.GetUTCTime()}','{pm_plan.plan_date.ToString("yyyy-MM-dd HH:mm")}'); " +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt3 = await Context.FetchData(addPlanQry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt3.Rows[0][0]);

            string mapChecklistQry = "INSERT INTO pmplanassetchecklist(planId, assetId, checklistId) VALUES ";
            foreach (var map in pm_plan.mapAssetChecklist)
            {
                mapChecklistQry += $"({id}, {map.asset_id}, {map.checklist_id}), ";
            }
            mapChecklistQry = mapChecklistQry.Substring(0, mapChecklistQry.Length - 2) + ";";
            await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, id, 0, 0, "PM Plan added", CMMS.CMMS_Status.PM_PLAN_CREATED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Plan added successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdatePMPlan(CMPMPlanDetail request, int userID)
        {

            string myQuery = "UPDATE pm_plan SET ";
            if (request.plan_name != null && request.plan_name != "")
                myQuery += $"plan_name = '{request.plan_name}', ";
            if (request.plan_date != null)
                myQuery += $"plan_date = '{request.plan_date.ToString("yyyy-MM-dd")}', ";
            if (request.plan_freq_id > 0)
                myQuery += $"frequency_id = {request.plan_freq_id}, ";
            if (request.facility_id > 0)
                myQuery += $"facility_id = {request.facility_id}, ";
            if (request.category_id > 0)
                myQuery += $"category_id = {request.category_id}, ";

            myQuery += $"status_id=1 ,";
            if (request.assigned_to_id > 0)
                myQuery += $"assigned_to = {request.assigned_to_id}, ";

            myQuery += $"updated_at = '{UtilsRepository.GetUTCTime()}', updated_by = {userID} WHERE id = {request.plan_id} and facility_id = {request.facility_id};";

            string myQuery2 = $"Delete from pmplanassetchecklist where planId = {request.plan_id};";
            await Context.ExecuteNonQry<int>(myQuery2).ConfigureAwait(false);
            if (request.mapAssetChecklist != null)
            {
                string mapChecklistQry = "INSERT INTO pmplanassetchecklist(planId, assetId, checklistId) VALUES ";
                foreach (var map in request.mapAssetChecklist)
                {
                    mapChecklistQry += $"({request.plan_id}, {map.asset_id}, {map.checklist_id}), ";
                }
                mapChecklistQry = mapChecklistQry.Substring(0, mapChecklistQry.Length - 2) + ";";
                await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);
            }

            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, request.plan_id, 0, 0, "PM Plan Updated ", CMMS.CMMS_Status.PM_PLAN_UPDATED);

            CMDefaultResponse response = new CMDefaultResponse(request.plan_id, CMMS.RETRUNSTATUS.SUCCESS, $"Plan Updated Successfully ");
            return response;
        }

        internal async Task<List<CMPMPlanList>> GetPMPlanList(int facility_id, string category_id, string frequency_id, DateTime? start_date, DateTime? end_date, string facilitytimeZone)
        {
            if (facility_id <= 0)
                throw new ArgumentException("Invalid Facility ID");
            string planListQry = $"SELECT plan.id as plan_id, plan.plan_name, plan.status as status_id, statuses.statusName as status_short, plan.plan_date, " +
                                    $"facilities.id as facility_id, facilities.name as facility_name, category.id as category_id, category.name as category_name, " +
                                    $"frequency.id as plan_freq_id, frequency.name as plan_freq_name, createdBy.id as created_by_id, " +
                                    $"CONCAT(createdBy.firstName, ' ', createdBy.lastName) as created_by_name, plan.created_at,CONCAT(assignedTo.firstName, ' ', assignedTo.lastName) as assigned_to_name, " +
                                    $"updatedBy.id as updated_by_id, CONCAT(updatedBy.firstName, ' ', updatedBy.lastName) as updated_by_name, plan.updated_at," +
                                    $" (select task.plan_date from pm_task task where task.id = (select max(id) from pm_task where pm_task.plan_id = plan.id  ) ) as next_schedule_date,  " +
                                    $" (select task1.id from pm_task task1 where task1.id = (select max(id) from pm_task where pm_task.plan_id = plan.id  ) ) as next_task_id " +
                                    $" FROM pm_plan as plan " +
                                    $"LEFT JOIN statuses ON plan.status = statuses.softwareId " +
                                    $"JOIN facilities ON plan.facility_id = facilities.id " +
                                    $"LEFT JOIN assetcategories as category ON plan.category_id = category.id " +
                                    $"LEFT JOIN frequency ON plan.frequency_id = frequency.id " +
                                    $"LEFT JOIN users as createdBy ON createdBy.id = plan.created_by " +
                                    $"LEFT JOIN users as updatedBy ON updatedBy.id = plan.updated_by " +
                                    $"LEFT JOIN users as assignedTo ON assignedTo.id = plan.assigned_to " +
                                    $"WHERE facilities.id = {facility_id} and status_id = 1 ";

            if (category_id != null && category_id != "")
                planListQry += $"AND category.id IN ( {category_id} )";
            if (frequency_id != null && category_id != "")
                planListQry += $"AND frequency.id IN ( {frequency_id} )";
            if (start_date != null)
                planListQry += $"AND plan.plan_date >= '{((DateTime)start_date).ToString("yyyy-MM-dd")}' ";
            if (end_date != null)
                planListQry += $"AND plan.plan_date <= '{((DateTime)end_date).ToString("yyyy-MM-dd")}' ";
            planListQry += $";";

            List<CMPMPlanList> plan_list = await Context.GetData<CMPMPlanList>(planListQry).ConfigureAwait(false);


            foreach (var plan in plan_list)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(plan.status_id);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.PM_PLAN, _Status);
                plan.status_short = _shortStatus;
            }
            foreach (var detail in plan_list)
            {
                detail.approved_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.approved_at);
                detail.created_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.created_at);
                detail.plan_date = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.plan_date);
                detail.rejected_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.rejected_at);
                detail.updated_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.updated_at);
            }

            return plan_list;
        }

        internal async Task<CMPMPlanDetail> GetPMPlanDetail(int id, string facilitytimeZone)
        {


            if (id <= 0)
                throw new ArgumentException("Invalid Plan ID");
            string planListQry = $"SELECT plan.id as plan_id, plan.plan_name, plan.status as status_id, statuses.statusName as status_short, plan.plan_date, " +
                                    $"facilities.id as facility_id, facilities.name as facility_name, category.id as category_id, category.name as category_name, " +
                                    $"frequency.id as plan_freq_id, frequency.name as plan_freq_name, createdBy.id as created_by_id, " +
                                    $"CONCAT(createdBy.firstName, ' ', createdBy.lastName) as created_by_name, plan.created_at,approvedBy.id as approved_by_id, CONCAT(approvedBy.firstName, ' ', approvedBy.lastName) as approved_by_name, plan.approved_at, rejectedBy.id as rejected_by_id, CONCAT(rejectedBy.firstName, ' ', rejectedBy.lastName) as rejected_by_name, plan.rejected_at,CONCAT(assignedTo.firstName, ' ', assignedTo.lastName) as assigned_to_name, " +
                                    $"updatedBy.id as updated_by_id, CONCAT(updatedBy.firstName, ' ', updatedBy.lastName) as updated_by_name,plan.updated_at " +
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

            List<CMPMPlanDetail> planDetails = await Context.GetData<CMPMPlanDetail>(planListQry).ConfigureAwait(false);

            if (planDetails.Count == 0)
                return null;
            string assetChecklistsQry = $"SELECT distinct assets.id as asset_id, assets.name as asset_name, parent.id as parent_id, parent.name as parent_name, assets.moduleQuantity as module_qty, checklist.id as checklist_id, checklist.checklist_number as checklist_name " +
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
            foreach (var detail in planDetails)
            {
                detail.updated_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.updated_at);
                detail.created_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.created_at);
                detail.approved_at = (DateTime)await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, detail.approved_at);
            }

            return planDetails[0];
        }
        internal async Task<List<CMScheduleData>> GetScheduleData(int facility_id, int category_id, string facilitytimeZone)
        {
            /*
             * Primary Table - PMSchedule
             * Read All properties mention in model and return 
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
            foreach (CMScheduleData schedule in _scheduleList)
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
                foreach (var detail in _freqData)
                {
                    detail.schedule_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.schedule_date);

                }


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
            foreach (var asset_schedule in request.asset_schedules)
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
                foreach (var frequency_schedule in asset_schedule.frequency_dates)
                {
                    string myQuery6 = $"SELECT * FROM frequency WHERE id = {frequency_schedule.frequency_id};";
                    List<CMFrequency> frequency = await Context.GetData<CMFrequency>(myQuery6).ConfigureAwait(false);
                    string myQuery7 = "SELECT id as schedule_id, PM_Schedule_date as schedule_date, Facility_id as facility_id, Asset_id as asset_id, PM_Frequecy_id as frequency_id " +
                                        $"FROM pm_schedule WHERE Asset_id = {asset_schedule.asset_id} AND PM_Frequecy_id = {frequency_schedule.frequency_id} " +
                                        $"AND status NOT IN ({(int)CMMS.CMMS_Status.PM_CANCELLED}) AND PM_Rescheduled = 0;";
                    List<ScheduleIDData> scheduleData = await Context.GetData<ScheduleIDData>(myQuery7).ConfigureAwait(false);
                    if (scheduleData.Count > 0)
                    {
                        if (frequency_schedule.schedule_date != null)
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
                        if (frequency_schedule.schedule_date != null)
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
                               $"select id as plan_id,facility_id,category_id,frequency_id,plan_Date,assigned_to," +
                               $"CASE WHEN assigned_to = '' or assigned_to IS NULL THEN {(int)CMMS.CMMS_Status.PM_SCHEDULED} " +
                               $"ELSE {(int)CMMS.CMMS_Status.PM_ASSIGNED} END as status " +
                               $"from pm_plan where id = {request.id}; " +
                               $"SELECT LAST_INSERT_ID(); ";

            DataTable dt3 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt3.Rows[0][0]);

            string scheduleQry = $"INSERT INTO pm_schedule(task_id,plan_id,Asset_id,checklist_id,PM_Schedule_date,status) " +
                                $"select {id} as task_id,planId as plan_id, assetId as Asset_id, checklistId as checklist_id,PP.plan_date  as PM_Schedule_date,{(int)CMMS.CMMS_Status.PM_SCHEDULED} as status from pmplanassetchecklist  P inner join pm_plan PP on PP.Id = P.planId where planId = {request.id}";
            await Context.ExecuteNonQry<int>(scheduleQry);

            string setCodeNameQuery = "UPDATE pm_schedule " +
                                        "SET PM_Schedule_Code = CONCAT(id,Facility_Code,Asset_Category_Code,Asset_Code,PM_Frequecy_Code), " +
                                        "PM_Schedule_Name = CONCAT(id,' ',Facility_Name,' ',Asset_Category_name,' ',Asset_Name), " +
                                        "PM_Schedule_Number = CONCAT('SCH',id), " +
                                        "PM_Maintenance_Order_Number = CONCAT('PMSCH',id);";
            await Context.ExecuteNonQry<int>(setCodeNameQuery);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Plan Approved " : request.comment, CMMS.CMMS_Status.PM_PLAN_APPROVED);

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

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, request.id, 0, 0, string.IsNullOrEmpty(request.comment) ? "PM Plan Rejected " : request.comment, CMMS.CMMS_Status.PM_PLAN_REJECTED);

            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.REJECTED, new[] { _WCList[0].created_by }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "PM Plan Rejected Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> DeletePMPlan(int planId, int userID)
        {
            string approveQuery = $"update pm_plan set status_id = 0 where id = {planId}; ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_PLAN, planId, 0, 0, $"PM Plan Deleted by user {userID}", CMMS.CMMS_Status.PM_PLAN_DELETED);

            CMDefaultResponse response = new CMDefaultResponse(planId, CMMS.RETRUNSTATUS.SUCCESS, $" PM Plan Deleted");
            return response;
        }

        internal async Task<CMImportFileResponse> ImportPMPlanFile(int file_id, int Facility, int userID)
        {
            int FID = Facility;
            int facilityid = 0;
            int equpifac = 0;
            string queryplan;
            CMImportFileResponse response = new CMImportFileResponse();
            DataTable dt2 = new DataTable();
            DataTable dtplan = new DataTable();
            DataTable dt3 = new DataTable();
            Dictionary<string, int> plan = new Dictionary<string, int>();



            string queryfacilities = "SELECT id, UPPER(name) as name FROM facilities GROUP BY name ORDER BY id ASC;";
            DataTable dtqueryfacilities = await Context.FetchData(queryfacilities).ConfigureAwait(false);
            List<string> fac_Names = dtqueryfacilities.GetColumn<string>("name");
            List<int> fac_IDs = dtqueryfacilities.GetColumn<int>("id");
            Dictionary<string, int> facilities = new Dictionary<string, int>();
            facilities.Merge(fac_Names, fac_IDs);

            string queryCat = "SELECT id, UPPER(name) as name FROM assetcategories GROUP BY name ORDER BY id ASC;";
            DataTable dtqueryCat = await Context.FetchData(queryCat).ConfigureAwait(false);
            List<string> Cat_Names = dtqueryCat.GetColumn<string>("name");
            List<int> Cat_IDs = dtqueryCat.GetColumn<int>("id");
            Dictionary<string, int> assetcategories = new Dictionary<string, int>();
            assetcategories.Merge(Cat_Names, Cat_IDs);

            string queryfrequency = "SELECT id, UPPER(name) as name FROM frequency GROUP BY name ORDER BY id ASC;";
            DataTable dtqueryfrequency = await Context.FetchData(queryfrequency).ConfigureAwait(false);
            List<string> frequency_Names = dtqueryfrequency.GetColumn<string>("name");
            List<int> frequency_IDs = dtqueryfrequency.GetColumn<int>("id");
            Dictionary<string, int> frequency = new Dictionary<string, int>();
            frequency.Merge(frequency_Names, frequency_IDs);

            string queryusers = "SELECT id, UPPER(CONCAT(firstName, ' ', lastName)) as name FROM users GROUP BY name ORDER BY id ASC;";
            DataTable dtqueryusers = await Context.FetchData(queryusers).ConfigureAwait(false);
            List<string> users_Names = dtqueryusers.GetColumn<string>("name");
            List<int> users_IDs = dtqueryusers.GetColumn<int>("id");
            Dictionary<string, int> users = new Dictionary<string, int>();
            users.Merge(users_Names, users_IDs);



            string queryasset = "SELECT id, UPPER(name) as name FROM assets GROUP BY name ORDER BY id ASC;";
            DataTable dtasset = await Context.FetchData(queryasset).ConfigureAwait(false);
            List<string> asset_name = dtasset.GetColumn<string>("name");
            List<int> asset_id = dtasset.GetColumn<int>("id");
            Dictionary<string, int> asset = new Dictionary<string, int>();
            asset.Merge(asset_name, asset_id);



            List<int> idList = new List<int>();
            List<int> updatedIdList = new List<int>();


            Dictionary<string, Tuple<string, Type>> columnNames = new Dictionary<string, Tuple<string, Type>>()
            {
                { "Plant Name", new Tuple<string, Type>("PlantName", typeof(string)) },
                { "Plan Name", new Tuple<string, Type>("PlanName", typeof(string)) },
                { "Equipment Category", new Tuple<string, Type>("EquipmentCategory", typeof(string)) },
                //{ "Eq_Location", new Tuple<string, Type>("Eq_Location", typeof(string)) },
                //{ "EquipmentName", new Tuple<string, Type>("EquipmentName", typeof(string)) },
                { "Start date", new Tuple<string, Type>("StartDate", typeof(DateTime)) },
                { "Frequency", new Tuple<string, Type>("Frequency", typeof(string)) },
                { "AssignedTo", new Tuple<string, Type>("AssignedTo", typeof(string)) },
                //{ "CheckList", new Tuple<string, Type>("CheckList", typeof(string)) }
                { "Approval", new Tuple<string, Type>("Approval", typeof(string)) },


            };

            string query1 = $"SELECT file_path FROM uploadedfiles WHERE id = {file_id};";
            DataTable dt1 = await Context.FetchData(query1).ConfigureAwait(false);
            string path = Convert.ToString(dt1.Rows[0][0]);
            string dir = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);



            if (!Directory.Exists(dir))
                return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"Directory '{dir}' cannot be found");
            else if (!File.Exists(path))
                return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"File '{filename}' cannot be found in directory '{dir}'");

            else
            {
                FileInfo info = new FileInfo(path);
                if (info.Extension == ".xlsx")
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excel = new ExcelPackage(path);
                    var sheet = excel.Workbook.Worksheets["PMPlan"];
                    var sheet2 = excel.Workbook.Worksheets["PlanEquipments"];


                    if (sheet == null)
                        return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, "The file must contain PMPlan sheet");
                    else if (sheet2 == null)
                        return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, "The file must contain PlanEquipment sheet");
                    else
                    {

                        foreach (var header in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                        {
                            try
                            {
                                dt2.Columns.Add(columnNames[header.Text].Item1, columnNames[header.Text].Item2);
                            }
                            catch (KeyNotFoundException)
                            {
                                dt2.Columns.Add(header.Text);
                            }
                        }
                        List<string> headers = dt2.GetColumnNames();
                        foreach (var item in columnNames.Values)
                        {
                            if (!headers.Contains(item.Item1))
                            {
                                dt2.Columns.Add(item.Item1, item.Item2);
                            }
                        }

                        dt2.Columns.Add("row_no", typeof(int));
                        dt2.Columns.Add("plantID", typeof(int));
                        dt2.Columns.Add("categoryID", typeof(int));
                        dt2.Columns.Add("frequencyID", typeof(int));
                        dt2.Columns.Add("assignedToID", typeof(int));
                        dt2.Columns.Add("planID", typeof(int));

                        //dt2.Columns.Add("checklistID", typeof(int));
                        //dt2.Columns.Add("equipmentID", typeof(int));

                        List<CMApproval> approval = new List<CMApproval>();
                        int updateCount = 0;

                        for (int rN = 2; rN <= sheet.Dimension.End.Row; rN++)
                        {
                            ExcelRange row = sheet.Cells[rN, 1, rN, sheet.Dimension.End.Column];
                            DataRow newR = dt2.NewRow();
                            foreach (var cell in row)
                            {
                                try
                                {
                                    if (cell.Text == null || cell.Text == "")
                                        continue;
                                    newR[cell.Start.Column - 1] = Convert.ChangeType(cell.Text, dt2.Columns[cell.Start.Column - 1].DataType);
                                }
                                catch (Exception ex)
                                {
                                    ex.GetType();
                                    //+ ex.ToString();
                                    //status = status.Substring(0, (status.IndexOf("Exception") + 8));
                                    // m_ErrorLog.SetError("," + status);
                                }
                            }
                            if (newR.IsEmpty())
                            {

                                continue;
                            }
                            newR["PlantName"] = newR[0];
                            newR["PlanName"] = newR[1];
                            newR["EquipmentCategory"] = newR[2];
                            //newR["Eq_Location"] = newR[3];
                            //newR["EquipmentName"] = newR[4];
                            newR["StartDate"] = newR[3];
                            newR["Frequency"] = newR[4];
                            newR["AssignedTo"] = newR[5];
                            newR["Approval"] = newR[6];
                            string fqr = $"select id From facilities where name='{newR["PlantName"]}';";
                            DataTable dt = await Context.FetchData(fqr).ConfigureAwait(false);
                            int id = Convert.ToInt32(dt.Rows[0][0]);
                            equpifac = id;
                            if (FID != id)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Invalid Plant Name '{newR["PlantName"]}'.");
                                continue;
                            }
                            try
                            {
                                newR["plantID"] = facilities[Convert.ToString(newR["PlantName"]).ToUpper()];

                                //  Plan named  Power Transformer Halfyearly Check does not exist. Row not Inserted.
                                if (facilityid == 0 && newR["plantID"].ToInt() > 0)
                                {

                                    facilityid = Convert.ToInt32(newR["plantID"]);
                                    queryplan = $"SELECT id, UPPER(plan_name) as name FROM pm_plan WHERE facility_id = {facilityid} GROUP BY name ORDER BY id ASC;";
                                    dtplan = await Context.FetchData(queryplan).ConfigureAwait(false);
                                    List<string> plan_name = dtplan.GetColumn<string>("name");
                                    List<int> plan_id = dtplan.GetColumn<int>("id");
                                    plan = new Dictionary<string, int>();
                                    plan.Merge(plan_name, plan_id);
                                }
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Invalid Plant Name '{newR["PlantName"]}'. Plan '{newR["PlanName"]}' not Inserted.");
                                newR.Delete();
                                continue;
                                // return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Plant named '{newR[0]}' does not exist.");
                            }

                            if (Convert.ToString(newR["PlanName"]) == null || Convert.ToString(newR["PlanName"]) == "")
                            {
                                m_errorLog.SetError($"[Row: {rN}] Plan Name cannot be null. Row not Inserted");
                                newR.Delete();
                                continue;
                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Plan N cannot be null.");
                            }
                            if (newR["StartDate"] == DBNull.Value)
                            {
                                continue;
                            }
                            DateTime startDate = (DateTime)newR["StartDate"];
                            DateTime currentDate = DateTime.Now;

                            if (startDate.Date < currentDate.Date)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Start Date '{newR["StartDate"]}' is less than Current Date . Plan '{newR["PlanName"]}' not Inserted.");
                                newR.Delete();
                                continue;
                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Invalid Start date {newR["StartDate"]}.");

                            }

                            try
                            {
                                newR["categoryID"] = assetcategories[Convert.ToString(newR["EquipmentCategory"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Equipment category named '{newR["EquipmentCategory"]}' does not exist. Plan '{newR["PlanName"]}' not Inserted.");
                                newR.Delete();
                                continue;
                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] equipment category named '{newR[2]}' does not exist.");
                            }
                            try
                            {
                                newR["frequencyID"] = frequency[Convert.ToString(newR["Frequency"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Frequency named '{newR["Frequency"]}' does not exist. Plan '{newR["PlanName"]}' not Inserted.");
                                newR.Delete();
                                continue;
                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] frequency named '{newR[6]}' does not exist.");
                            }
                            try
                            {
                                newR["assignedToID"] = users[Convert.ToString(newR["AssignedTo"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Assigned to named user '{newR["AssignedTo"]}' does not exist. Plan '{newR["PlanName"]}' not Inserted.");
                                newR.Delete();
                                continue;
                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] assigned to named '{newR[7]}' does not exist.");
                            }

                            //   if (plan.ContainsKey(Convert.ToString(newR["PlanName"])))
                            //   {
                            //       m_errorLog.SetError($"[Row: {rN}] PlanName Is Already Present");
                            //        newR.Delete();
                            //        continue;
                            //    }
                            try
                            {


                                newR["planID"] = plan[Convert.ToString(newR["PlanName"]).ToUpper()];

                                CMPMPlanDetail updatePlan = new CMPMPlanDetail();

                                updatePlan.plan_id = Convert.ToInt32(newR["planID"]);
                                updatePlan.plan_name = Convert.ToString(newR["PlanName"]);
                                updatePlan.plan_date = startDate;
                                updatePlan.plan_freq_id = Convert.ToInt32(newR["frequencyID"]);
                                updatePlan.facility_id = Convert.ToInt32(newR["plantID"]);
                                updatePlan.category_id = Convert.ToInt32(newR["categoryID"]);
                                updatePlan.assigned_to_id = Convert.ToInt32(newR["assignedToID"]);
                                var resPlan = await UpdatePMPlan(updatePlan, userID);
                                updateCount++;
                                string myQuery2 = $"Update pm_plan set status = {(int)CMMS.CMMS_Status.PM_PLAN_CREATED},approved_by = null where id = {updatePlan.plan_id};";
                                await Context.ExecuteNonQry<int>(myQuery2).ConfigureAwait(false);
                                int app = Convert.ToInt32(newR["Approval"]);
                                if (app == 1)
                                {
                                    CMApproval planApproval = new CMApproval
                                    {
                                        id = Convert.ToInt32(newR["planID"]),
                                        comment = "Approved"
                                    };

                                    approval.Add(planApproval);
                                }
                                //m_errorLog.SetWarning($"[Row: {rN}] Updated Plan '{newR["PlanName"]}'. Plan ID :{newR["planID"]}. ");
                                updatedIdList.Add(updatePlan.plan_id);
                                newR.Delete();
                                continue;

                            }
                            catch (KeyNotFoundException)
                            {

                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Equipment named '{newR["EquipmentName"]}' does not exist.");
                            }


                            //try
                            //{
                            //    newR["checklistID"] = checklist[Convert.ToString(newR[8]).ToUpper()];
                            //}
                            //catch (KeyNotFoundException)
                            //{
                            //    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Checklist named '{newR[8]}' does not exist.");
                            //}

                            //try
                            //{
                            //    newR["equipmentID"] = asset[Convert.ToString(newR[4]).ToUpper()];
                            //}
                            //catch (KeyNotFoundException)
                            //{
                            //    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Equipment named '{newR[4]}' does not exist.");
                            //}



                            newR["row_no"] = rN;

                            //if (Convert.ToString(newR["PlantName"]) == null || Convert.ToString(newR["PlantName"]) == "")
                            //{
                            //    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] plant name cannot be null.");
                            //}

                            //if (Convert.ToString(newR["EquipmentCategory"]) == null || Convert.ToString(newR["EquipmentCategory"]) == "")
                            //{
                            //    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] equipment category cannot be null.");
                            //}
                            //if (Convert.ToString(newR["Frequency"]) == null || Convert.ToString(newR["Frequency"]) == "")
                            //{
                            //    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] frequency cannot be null.");
                            //}
                            //if (Convert.ToString(newR["Frequency"]) == null || Convert.ToString(newR["Frequency"]) == "")
                            //{
                            //    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] frequency cannot be null.");
                            //}
                            //if (Convert.ToString(newR["AssignedTo"]) == null || Convert.ToString(newR["AssignedTo"]) == "")
                            //{
                            //    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] assigned to cannot be null.");
                            //}

                            dt2.Rows.Add(newR);
                        }
                        if (dt2.Rows.Count == 0 && updateCount == 0)
                        {
                            string logPath1 = m_errorLog.SaveAsText($"ImportLog\\ImportPMPlan_File{file_id}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}");
                            string logQry1 = $"UPDATE uploadedfiles SET logfile = '{logPath1}' WHERE id = {file_id}";
                            await Context.ExecuteNonQry<int>(logQry1).ConfigureAwait(false);
                            logPath1 = logPath1.Replace("\\\\", "\\");

                            m_errorLog.SetImportInformation("File has invalid rows.");
                            // response.error_log_file_path = logPath1;
                            // response.import_log = m_errorLog.errorLog();
                            return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, logPath1, m_errorLog.errorLog(), "Import failed, file has empty rows.");
                        }
                        /* string insertQuery = "INSERT INTO pm_plan " +
                             "(plan_name, facility_id, category_id, frequency_id, " +
                             " plan_date, assigned_to, status, created_by, created_at, " +
                             " approved_by, approved_at,  " +
                             " remarks,status_id)";*/
                        //foreach (DataRow row in dt2.Rows)
                        //{
                        //    insertQuery = insertQuery + $"Select '{row.ItemArray[1]}',{row.ItemArray[12]}, {row.ItemArray[13]}, {row.ItemArray[14]}," +
                        //        $" '{Convert.ToDateTime(row.ItemArray[5]).ToString("yyyy-MM-dd HH:mm")}', {row.ItemArray[15]}, {(int)CMMS.CMMS_Status.PM_PLAN_APPROVED}, {userID}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
                        //        $" {userID},'{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}', 'Approved', 1 UNION ALL ";
                        //}

                        String insertQuery = "";
                        foreach (DataRow row in dt2.Rows)
                        {
                            string plan_name1 = Convert.ToString(row["PlanName"]);
                            int facility_id = Convert.ToInt32(row["plantID"]);
                            int category_id = Convert.ToInt32(row["categoryID"]);
                            int frequency_id = Convert.ToInt32(row["frequencyID"]);
                            int assigned_to = Convert.ToInt32(row["assignedToID"]);

                            insertQuery = "INSERT INTO pm_plan " +
                                        "(plan_name, facility_id, category_id, frequency_id, " +
                                        " plan_date, assigned_to, status, created_by, created_at, " +
                                        " status_id)";
                            insertQuery = insertQuery + $"Select '{plan_name1}',{facility_id}, {category_id}, {frequency_id}," +
                                $" '{Convert.ToDateTime(row["StartDate"]).ToString("yyyy-MM-dd HH:mm")}', {assigned_to}, {(int)CMMS.CMMS_Status.PM_PLAN_CREATED}, {userID}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
                                $"  1 ; SELECT LAST_INSERT_ID(); ";
                            DataTable dt_insert = await Context.FetchData(insertQuery).ConfigureAwait(false);

                            int id = Convert.ToInt32(dt_insert.Rows[0][0]);
                            plan.Add(plan_name1.ToUpper(), id);
                            idList.Add(id);
                            int approvalFlag = Convert.ToInt32(row["Approval"]);
                            if (approvalFlag == 1)
                            {
                                CMApproval planApproval = new CMApproval
                                {
                                    id = id,
                                    comment = "Approved"
                                };

                                approval.Add(planApproval);
                            }

                            //string mapChecklistQry = "INSERT INTO pmplanassetchecklist(planId, assetId, checklistId) VALUES ";
                            ////foreach (var map in pm_plan.mapAssetChecklist)
                            ////{
                            //mapChecklistQry += $"({id}, {row.ItemArray[17]}, {row.ItemArray[16]}), ";
                            ////}
                            //mapChecklistQry = mapChecklistQry.Substring(0, mapChecklistQry.Length - 2) + ";";
                            //await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);
                        }


                        //int lastIndex = insertQuery.LastIndexOf("UNION ALL ");
                        //if (lastIndex > 0)
                        //{
                        //    insertQuery = insertQuery.Remove(lastIndex, "UNION ALL ".Length);
                        //    var insertedResult = await Context.ExecuteNonQry<int>(insertQuery).ConfigureAwait(false);
                        //}

                        //if (!insertQuery.Contains("Select"))
                        //{
                        //    return new p=(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, "Import failed, file has empty rows.");
                        //}
                        string querychecklist = $"SELECT id, UPPER(checklist_number) as name FROM checklist_number where facility_id ={facilityid} GROUP BY name ORDER BY id ASC;";
                        DataTable dtchecklist = await Context.FetchData(querychecklist).ConfigureAwait(false);
                        List<string> checklist_name = dtchecklist.GetColumn<string>("name");
                        List<int> checklist_id = dtchecklist.GetColumn<int>("id");
                        Dictionary<string, int> checklist = new Dictionary<string, int>();
                        checklist.Merge(checklist_name, checklist_id);
                        Dictionary<string, Tuple<string, Type>> equipColumnNames = new Dictionary<string, Tuple<string, Type>>()
                        {
                           { "Plan Name", new Tuple<string, Type>("PlanName", typeof(string)) },
                           { "Equipment Name", new Tuple<string, Type>("EquipmentName", typeof(string)) },
                           { "CheckList", new Tuple<string, Type>("CheckList", typeof(string)) }

                        };

                        foreach (var header in sheet2.Cells[1, 1, 1, sheet2.Dimension.End.Column])
                        {
                            try
                            {
                                dt3.Columns.Add(equipColumnNames[header.Text].Item1, equipColumnNames[header.Text].Item2);
                            }
                            catch (KeyNotFoundException)
                            {
                                dt3.Columns.Add(header.Text);
                            }
                        }
                        List<string> headers1 = dt3.GetColumnNames();
                        foreach (var item in equipColumnNames.Values)
                        {
                            if (!headers1.Contains(item.Item1))
                            {
                                dt3.Columns.Add(item.Item1, item.Item2);
                            }
                        }

                        dt3.Columns.Add("row_no", typeof(int));
                        dt3.Columns.Add("planID", typeof(int));
                        dt3.Columns.Add("equipmentID", typeof(int));
                        dt3.Columns.Add("checklistID", typeof(int));
                        int p_id = 0;

                        for (int rN = 2; rN <= sheet2.Dimension.End.Row; rN++)
                        {
                            ExcelRange row = sheet2.Cells[rN, 1, rN, sheet2.Dimension.End.Column];
                            DataRow newR = dt3.NewRow();
                            foreach (var cell in row)
                            {
                                try
                                {
                                    if (cell.Text == null || cell.Text == "")
                                        continue;
                                    newR[cell.Start.Column - 1] = Convert.ChangeType(cell.Text, dt3.Columns[cell.Start.Column - 1].DataType);
                                }
                                catch (Exception ex)
                                {
                                    ex.GetType();
                                    //+ ex.ToString();
                                    //status = status.Substring(0, (status.IndexOf("Exception") + 8));
                                    // m_ErrorLog.SetError("," + status);
                                }
                            }
                            if (newR.IsEmpty())
                            {

                                continue;
                            }

                            newR["PlanName"] = newR[0];
                            newR["EquipmentName"] = newR[1];
                            newR["CheckList"] = newR[2];


                            try
                            {
                                newR["planID"] = plan[Convert.ToString(newR["PlanName"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Plan  named '{newR["PlanName"]}' does not exist. Row not Inserted.");
                                newR.Delete();
                                continue;
                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Equipment named '{newR["EquipmentName"]}' does not exist.");
                            }

                            try
                            {

                                {
                                    newR["equipmentID"] = asset[Convert.ToString(newR["EquipmentName"]).ToUpper()];
                                }

                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Equipment  named '{newR["EquipmentName"]}' does not exist.Plan '{newR["PlanName"]}' not imported.");
                                string qur = $"select id from pm_plan where plan_name= '{newR["PlanName"]}' and facility_id={equpifac} ;";
                                DataTable dt = await Context.FetchData(qur).ConfigureAwait(false);
                                string blockId = Convert.ToString(dt.Rows[0][0]);

                                string qurq = $"DELETE FROM pm_plan WHERE id ={blockId};";

                                await Context.ExecuteNonQry<int>(qurq).ConfigureAwait(false);
                                newR.Delete();
                                continue;


                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Equipment named '{newR["EquipmentName"]}' does not exist.");
                            }
                            try
                            {
                                newR["checklistID"] = checklist[Convert.ToString(newR["CheckList"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] CheckList named'{newR["CheckList"]}' does not exist. Row not Inserted.");
                                newR.Delete();
                                continue;
                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Checklist named '{newR[""]}' does not exist.");
                            }
                            dt3.Rows.Add(newR);
                        }

                        if (dt3.Rows.Count == 0)
                        {
                            string logPath1 = m_errorLog.SaveAsText($"ImportLog\\ImportPMPlan_File{file_id}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}");
                            string logQry1 = $"UPDATE uploadedfiles SET logfile = '{logPath1}' WHERE id = {file_id}";
                            await Context.ExecuteNonQry<int>(logQry1).ConfigureAwait(false);
                            logPath1 = logPath1.Replace("\\\\", "\\");

                            m_errorLog.SetImportInformation("File sheet[PlanEquipments] has invalid rows.");
                            // response.error_log_file_path = logPath1;
                            // response.import_log = m_errorLog.errorLog();
                            return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, logPath1, m_errorLog.errorLog(), "Import failed, file sheet[PlanEquipments] no rows to Insert.");
                        }

                        string mapChecklistQry = "INSERT INTO pmplanassetchecklist(planId, assetId, checklistId,facility_id) VALUES ";

                        foreach (DataRow row in dt3.Rows)
                        {

                            int planID = Convert.ToInt32(row["planID"]);
                            int equipmentID = Convert.ToInt32(row["equipmentID"]);
                            int checklistID = Convert.ToInt32(row["checklistID"]);


                            mapChecklistQry += $"({planID}, {equipmentID}, {checklistID},{Facility}), ";

                        }
                        mapChecklistQry = mapChecklistQry.Substring(0, mapChecklistQry.Length - 2) + ";";

                        await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);


                        foreach (var plans in approval)
                        {
                            var approvePlan = await ApprovePMPlan(plans, userID);
                        }
                    }
                }
                else //
                {

                    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, "File is not an excel file");

                }
            }
            string logPath = m_errorLog.SaveAsText($"ImportLog\\ImportPMPlan_File{file_id}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}");
            string logQry = $"UPDATE uploadedfiles SET logfile = '{logPath}' WHERE id = {file_id}";
            await Context.ExecuteNonQry<int>(logQry).ConfigureAwait(false);
            logPath = logPath.Replace("\\\\", "\\");

            m_errorLog.SetImportInformation($"Total Errors <{m_errorLog.GetErrorCount()}>");
            return new CMImportFileResponse(file_id, idList, updatedIdList, CMMS.RETRUNSTATUS.SUCCESS, logPath, m_errorLog.errorLog(), $"{idList.Count} Plan(s) created, {updatedIdList.Count} Plan(s) updated.");
            //return response;*/
        }

        internal async Task<CMDefaultResponse> DeletePMTask(CMApproval request, int userID)
        {
            CMDefaultResponse response = null;
            string mrsQ = "select status,id from smmrs where whereUsedRefID = " + request.id + ";";
            DataTable dt_insert = await Context.FetchData(mrsQ).ConfigureAwait(false);

            string taskQ = "select ptw_id from pm_task where id = " + request.id + ";";
            DataTable dt_task = await Context.FetchData(taskQ).ConfigureAwait(false);

            int mrs_status = 0;
            int mrs_id = 0;
            int permit_id = 0;
            if (dt_insert.Rows.Count > 0)
            {
                mrs_status = Convert.ToInt32(dt_insert.Rows[0][0]);
                mrs_id = Convert.ToInt32(dt_insert.Rows[0][1]);
            }

            if (dt_task.Rows.Count > 0)
            {
                if (Convert.ToString(dt_task.Rows[0][0]) != "")
                {
                    permit_id = Convert.ToInt32(dt_insert.Rows[0][0]);
                }

            }

            if ((CMMS.CMMS_Status)mrs_status == CMMS.CMMS_Status.MRS_REQUEST_ISSUED)
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, $" PM Plan With MRS " + mrs_id + " Is Issued. Please Return MRS To Proceed Further.");
                return response;
            }

            string approveQuery = $"update pm_task set status = {(int)CMMS.CMMS_Status.PM_TASK_DELETED}, update_remarks = '{request.comment}' where id = {request.id}; ";
            approveQuery = approveQuery + $" update smmrs set status = {(int)CMMS.CMMS_Status.MRS_REQUEST_REJECTED}, issue_rejected_comment = '{request.comment}', issue_rejected_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}' where id = {mrs_id}; ";
            approveQuery = approveQuery + $" update permits set rejectReason = '{request.comment}', rejectStatus = 1, status = {(int)CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER}, status_updated_at = '{UtilsRepository.GetUTCTime()}', rejectedDate ='{UtilsRepository.GetUTCTime()}', rejectedById = {userID}  where id = {permit_id};";
            //string approveQuery = $"update pm_task set status = {(int)CMMS.CMMS_Status.PM_TASK_DELETED} where id = {request.id}; ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.PM_TASK, request.id, 0, 0, request.comment, CMMS.CMMS_Status.PM_TASK_DELETED);

            response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $" PM Task Deleted With MRS : " + mrs_id + "");
            return response;
        }
    }
}
