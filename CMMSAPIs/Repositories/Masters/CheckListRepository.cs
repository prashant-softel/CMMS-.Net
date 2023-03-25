using CMMSAPIs.BS.Masters;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Masters
{
    public class CheckListRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public CheckListRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        #region checklist
        internal async Task<List<CMCheckList>> GetCheckList(int facility_id, string type)
        {
            /* Table - CheckList_Number
             * supporting table - AssetCategory - to get Category Name, Frequency - To get Frequency Name
             * Read All properties from above table and return the list
             * Code goes here
            */
            string myQuery = "SELECT "+
                                "checklist_number.id , checklist_number.checklist_number , checklist_number.checklist_type as type, checklist_number.status, checklist_number.created_by as createdById, CONCAT(created_user.firstName, ' ', created_user.lastName) as createdByName, checklist_number.created_at as createdAt, checklist_number.updated_by as updatedById, CONCAT(updated_user.firstName, ' ', updated_user.lastName) as updatedByName, checklist_number.updated_at as updatedAt, checklist_number.asset_category_id as category_id, asset_cat.name as category_name, checklist_number.frequency_id, frequency.name as frequency_name, checklist_number.manpower as manPower, checklist_number.duration, checklist_number.facility_id as facility_id, facilities.name as facility_name " + 
                             "FROM "+
                                "checklist_number "+
                             "LEFT JOIN "+
                                "facilities on facilities.id=checklist_number.facility_id "+
                             "LEFT JOIN "+
                                "assetcategories as asset_cat ON asset_cat.id=checklist_number.asset_category_id "+
                             "LEFT JOIN "+
                                "frequency ON frequency.id=checklist_number.frequency_id "+
                             "LEFT JOIN "+
                                "users as created_user ON created_user.id=checklist_number.created_by " +
                             "LEFT JOIN "+
                                "users as updated_user ON updated_user.id=checklist_number.updated_by ";
            if (facility_id > 0)
            {
                myQuery += $" WHERE checklist_number.facility_id= { facility_id } ";
                if (type != null)
                    myQuery += $" and  checklist_number.checklist_type in ({ type }) ";
                else
                {
                    throw new ArgumentException("Type cannot be empty");
                }
            }
            else
            {
                throw new ArgumentException("Facility ID cannot be empty or zero");
            }
            myQuery += " ORDER BY checklist_number.id DESC ";
            List<CMCheckList> _checkList = await Context.GetData<CMCheckList>(myQuery).ConfigureAwait(false);
            return _checkList;
        }

        internal async Task<CMDefaultResponse> CreateChecklist(List<CMCreateCheckList> request_list, int userID)
        {
            /*
             * Table - CheckList_Number
             * Insert all properties in CMCreateCheckList model to CheckList_Number
             * Code goes here
            */
            List<int> id_list = new List<int>();
            foreach (CMCreateCheckList request in request_list)
            {
                string query = "INSERT INTO checklist_number(checklist_number, checklist_type, facility_id, status ,created_by ," +
                    " created_at , asset_category_id ,frequency_id ,manpower , duration,updated_at )VALUES" +
                            $"('{request.checklist_number}', {request.type}, {request.facility_id},{request.status},  {userID}," +
                            $"'{UtilsRepository.GetUTCTime()}',{request.category_id},{request.frequency_id}, {request.manPower},{request.duration},'{UtilsRepository.GetUTCTime()}'); select LAST_INSERT_ID();";

                DataTable dt = await Context.FetchData(query).ConfigureAwait(false);

                int id = Convert.ToInt32(dt.Rows[0][0]);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKLIST_NUMBER, id, 0, 0, "Check List Created", CMMS.CMMS_Status.CREATED, userID);
                id_list.Add(id);
            }
            CMDefaultResponse response = new CMDefaultResponse(id_list, CMMS.RETRUNSTATUS.SUCCESS, "Check List Created Successfully");

            return response;
            
        }

        internal async Task<CMDefaultResponse> UpdateCheckList(CMCreateCheckList request, int userID)
        {
            /*
             * Update the changed value in CheckList_Number for requested id
             * Code goes here
            */
            string updateQry = $"UPDATE  softel_cmms.checklist_number SET ";
            if (request.checklist_number != null && request.checklist_number != "")
                updateQry += $" checklist_number = '{request.checklist_number}', ";
            if (request.type > 0)
                updateQry += $" checklist_type = {request.type}, ";
            if (request.facility_id > 0)
                updateQry += $" facility_id = {request.facility_id}, ";
            if (request.status > 0)
                updateQry += $" status = {request.status}, ";
            if (request.category_id > 0)
                updateQry += $" asset_category_id = {request.category_id}, ";
            if (request.frequency_id > 0)
                updateQry += $" frequency_id = {request.frequency_id}, ";
            if (request.manPower > 0)
                updateQry += $" manpower = {request.manPower}, ";
            if (request.duration != null)
                updateQry += $" duration = {request.duration}, ";
            updateQry += $" updated_by = {userID}, updated_at = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id}; ";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKLIST_NUMBER, request.id, 0, 0, "Check List Updated", CMMS.CMMS_Status.UPDATED, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Check List Updated Successfully");

            return response;
        }

        internal async Task<CMDefaultResponse> DeleteChecklist(int id, int userID)
        {
            /* 
             * Set Status to 0 in CheckList_Number table for requested id
             * Code goes here
            */
            string deleteQry = $"DELETE FROM  softel_cmms.checklist_number " +
               $"WHERE  id  = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKLIST_NUMBER, id, 0, 0, "Check List Deleted", CMMS.CMMS_Status.DELETED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Check List Deleted");

            return response;
           
        }
        #endregion

        #region checklistmap
        internal async Task<List<CMCheckListMapList>> GetCheckListMap(int facility_id, int type)
        {
            /*
             * Primary Table - CheckList_Mapping
             * Read All properties mention in model and return list
             * Code goes here
            */
            string myQuery = "";
            myQuery = "SELECT  checklist_mapping.id , checklist_mapping.facility_id ,checklist_mapping.category_id ,"+
            "checklist_mapping.status ,checklist_mapping.checklist_id ,checklist_mapping.plan_id"+
            " FROM  softel_cmms.checklist_mapping; ";
            if (facility_id != 0)
            {
                myQuery += " WHERE facility_id= " + facility_id + " and  checklist_id = " + type;
            }

            List<CMCheckListMapList> _checkList = await Context.GetData<CMCheckListMapList>(myQuery).ConfigureAwait(false);
            return _checkList;
        }

        internal async Task<CMDefaultResponse> CreateCheckListMap(CMCreateCheckListMap request)
        {
            /*
             * Primary Table - CheckList_Mapping
             * Insert All properties mention in model
             * Code goes here
            */
            string query = "INSERT INTO checklist_mapping ( facility_id ,category_id ,status ,"+
 "checklist_id ,plan_id )VALUES "+
                       $"('{request.facility_id}', '{request.category_id}', '{request.status}',  '{request.check_id}'," +
                       $"'{request.plan_id}'); select LAST_INSERT_ID();";

            DataTable dt = await Context.FetchData(query).ConfigureAwait(false);

            int id = Convert.ToInt32(dt.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "");

            return response;
        }

        internal async Task<CMDefaultResponse> UpdateCheckListMap(CMCreateCheckListMap request)
        {
            /* Primary Table - CheckList_Mapping
             * Update All properties mention in model
             * Code goes here
            */
            string updateQry = $"UPDATE  checklist_mapping SET facility_id  = '{request.facility_id}', category_id  = '{request.category_id}',"+
            $"status = '{request.status}', checklist_id = '{request.checklist_ids}', plan_id = '{request.plan_id}'"+
            $" WHERE id = '{request.mapping_id}'; ";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(retVal, CMMS.RETRUNSTATUS.SUCCESS, "");

            return response;
            
        }
        #endregion

        #region CheckPoint
        internal async Task<List<CMCheckPointList>> GetCheckPointList(int checklist_id)
        {
            /*
             * Primary Table - CheckPoint
             * Supporting table - Checklist_Number - to get checklist name
             * Read All properties mention in CMCheckPointList and return list
             * Code goes here
            */
            string myQuery = "SELECT " +
                                "checkpoint.id as id, check_point, check_list_id as checklist_id, checklist_number.checklist_number as checklist_name, requirement, is_document_required, checkpoint.created_by as created_by_id, CONCAT(created_user.firstName,' ',created_user.lastName) as created_by_name, checkpoint.created_at, checkpoint.updated_by as updated_by_id, CONCAT(updated_user.firstName,' ',updated_user.lastName) as updated_by_name, checkpoint.updated_at, checkpoint.status " +
                             "FROM " + 
                                "checkpoint " + 
                             "LEFT JOIN " + 
                                "checklist_number ON checklist_number.id=checkpoint.check_list_id " + 
                             "LEFT JOIN " + 
                                "users as created_user ON created_user.id=checkpoint.created_by " +
                             "LEFT JOIN " +
                                "users as updated_user ON updated_user.id=checkpoint.updated_by ";
            if (checklist_id > 0)
            {
                myQuery += $" WHERE check_list_id = {checklist_id} ";
            }
            else
            {
                throw new ArgumentException("Invalid checklist_id");
            }
            myQuery += "ORDER BY checkpoint.id DESC";

            List<CMCheckPointList> _checkList = await Context.GetData<CMCheckPointList>(myQuery).ConfigureAwait(false);
            return _checkList;
        }

        internal async Task<CMDefaultResponse> CreateCheckPoint(List<CMCreateCheckPoint> requestList, int userID)
        {
            /*
             * Primary Table - CheckPoint
             * Insert all properties mention in model to CheckPoint table
             * Code goes here
            */
            List<int> idList = new List<int>();
            foreach (CMCreateCheckPoint request in requestList)
            {
                string query = "INSERT INTO  checkpoint (check_point, check_list_id, requirement, is_document_required, " +
                "created_by, created_at, status) VALUES " +
                 $"('{request.check_point}', {request.checklist_id}, '{request.requirement}', {request.is_document_required}," +
                 $"{userID}, '{UtilsRepository.GetUTCTime()}', {request.status}); select LAST_INSERT_ID();";

                DataTable dt = await Context.FetchData(query).ConfigureAwait(false);

                int id = Convert.ToInt32(dt.Rows[0][0]);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKPOINTS, id, 0, 0, "Check Point Created", CMMS.CMMS_Status.CREATED, userID);
                idList.Add(id);
            }
            CMDefaultResponse response = new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, $"{idList.Count} checkpoint(s) created successfully");

            return response;
        }

        internal async Task<CMDefaultResponse> UpdateCheckPoint(CMCreateCheckPoint request, int userID)
        {
            /*
             * Primary Table - CheckPoint
             * Update all properties mention in model to CheckPoint table for requisted id
             * Code goes here
            */
            string updateQry = $"UPDATE checkpoint SET ";
            if (request.check_point != "" && request.check_point != null)
                updateQry += $"check_point = '{request.check_point}', ";
            if (request.checklist_id != 0)
                updateQry += $"check_list_id = {request.checklist_id}, ";
            if (request.requirement != "" && request.requirement != null)
                updateQry += $"requirement = '{request.requirement}', ";
            if (request.is_document_required != null)
                updateQry += $"is_document_required = {request.is_document_required}, ";
            if (request.status != null)
                updateQry += $"status = {request.status} ";
            updateQry += $"updated_by = {userID}, updated_at='{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKPOINTS, request.id, 0, 0, "Check Point Updated", CMMS.CMMS_Status.UPDATED, userID);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Checkpoint updated successfully");

            return response;
        }

        internal async Task<CMDefaultResponse> DeleteCheckPoint(int id, int userID)
        {
            /*
             * Primary Table - CheckPoint
             * Set status 0 for requested id in CheckPoint table
             * Code goes here
            */
            string updateQry = $"DELETE FROM checkpoint WHERE id = {id};";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKPOINTS, id, 0, 0, "Check Point Deleted", CMMS.CMMS_Status.DELETED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Checkpoint deleted successfully");
            return response;
        }
        #endregion
    }
}
