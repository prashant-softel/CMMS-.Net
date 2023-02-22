using CMMSAPIs.BS.Masters;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Masters
{
    public class CheckListRepository : GenericRepository
    {
        public CheckListRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

        }

#region checklist

        internal async Task<List<CMCheckList>> GetCheckList(int facility_id, string type)
        {
            /* Table - CheckList_Number
             * supporting table - AssetCategory - to get Category Name, Frequency - To get Frequency Name
             * Read All properties from above table and return the list
             * Code goes here
            */
            string myQuery = "";
            myQuery = "SELECT  checklist_number.id , checklist_number.checklist_number , checklist_number.checklist_type , checklist_number.status , " +
                "checklist_number.created_by, checklist_number.created_at, checklist_number.asset_category_id, checklist_number.frequency_id , " +
                "checklist_number.manpower ,checklist_number.duration FROM  softel_cmms.checklist_number ";
            if (facility_id != 0)
            {
                myQuery += " WHERE asset_category_id= " + facility_id + " and  checklist_type = " + CMMS.checklist_type.Common;

            }

            List<CMCheckList> _checkList = await Context.GetData<CMCheckList>(myQuery).ConfigureAwait(false);
            return _checkList;

    
        }

        internal async Task<CMDefaultResponse> CreateChecklist(CMCreateCheckList request)
        {
            /*
             * Table - CheckList_Number
             * Insert all properties in CMCreateCheckList model to CheckList_Number
             * Code goes here
            */


           
            string query = "INSERT INTO  softel_cmms.checklist_number(checklist_number, checklist_type ,status ,created_by ," +
                " created_at , asset_category_id ,frequency_id ,manpower , duration,updated_at )VALUES" +
                            $"('{request.checklist_number}', '{request.type}', '{request.status}',  '{request.createdBy}'," +
                            $"'{UtilsRepository.GetUTCTime()}','{request.category_id}','{request.frequency_id}', '{request.manPower}','{request.duration}','{UtilsRepository.GetUTCTime()}'); select LAST_INSERT_ID();";

            DataTable dt = await Context.FetchData(query).ConfigureAwait(false);

            int id = Convert.ToInt32(dt.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "");

            return response;
            
        }

        internal async Task<CMDefaultResponse> UpdateCheckList(CMCreateCheckList request)
        {
            /*
             * Update the changed value in CheckList_Number for requested id
             * Code goes here
            */
            string updateQry = $"UPDATE  softel_cmms.checklist_number SET checklist_number  = '{request.checklist_number}', checklist_type  = '{request.type}'," +
                $"status  = '{request.status}', updated_by  = '{request.updatedBy}', updated_at  = '{UtilsRepository.GetUTCTime()}', asset_category_id  = '{request.category_id}'," +
                $"frequency_id  = '{request.frequency_id}',manpower  = '{request.manPower}', duration  = '{request.duration}' WHERE  id  = '{request.id}';";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(retVal, CMMS.RETRUNSTATUS.SUCCESS, "");

            return response;
        }

        internal async Task<CMDefaultResponse> DeleteChecklist(CMCreateCheckList request)
        {
            /* 
             * Set Status to 0 in CheckList_Number table for requested id
             * Code goes here
            */
            string updateQry = $"UPDATE  softel_cmms.checklist_number SET " +
               $"status  = 0,  updated_at  = '{UtilsRepository.GetUTCTime()}' WHERE  id  = {request.id};";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(retVal, CMMS.RETRUNSTATUS.SUCCESS, "");

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
            string myQuery = "";
            myQuery = "SELECT check_point, check_list_id, requirement, is_document_required,"+
                        "created_by, created_at, updated_by, updated_at, status FROM  checkpoint; ";
            if (checklist_id != 0)
            {
                myQuery += " WHERE asset_category_id= " + checklist_id ;

            }
            else
            {
                throw new ArgumentException("Invalid checklist_id <" + checklist_id  + ">");
            }

            List<CMCheckPointList> _checkList = await Context.GetData<CMCheckPointList>(myQuery).ConfigureAwait(false);
            return _checkList;
        }

        internal async Task<CMDefaultResponse> CreateCheckPoint(CMCreateCheckPoint request)
        {
            /*
             * Primary Table - CheckPoint
             * Insert all properties mention in model to CheckPoint table
             * Code goes here
            */
            string query = "INSERT INTO  checkpoint (check_point, check_list_id, requirement, is_document_required, "+
            "created_by, created_at, status) VALUES " +
             $"('{request.check_point}', '{request.checklist_id}', '{request.requirement}',  '{request.is_document_required}'," +
             $"'{request.created_by}', '{UtilsRepository.GetUTCTime()}', '{request.status}'); select LAST_INSERT_ID();";

            DataTable dt = await Context.FetchData(query).ConfigureAwait(false);

            int id = Convert.ToInt32(dt.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "");

            return response;
        }

        internal async Task<CMDefaultResponse> UpdateCheckPoint(CMCreateCheckPoint request)
        {
            /*
             * Primary Table - CheckPoint
             * Update all properties mention in model to CheckPoint table for requisted id
             * Code goes here
            */
            string updateQry = $"UPDATE checkpoint SET check_point  = '{request.check_point}', check_list_id  = '{request.checklist_id}', requirement  = '{request.requirement}',"+
            $"is_document_required = '{request.is_document_required}', updated_by = '{request.updated_by}',updated_at = '{UtilsRepository.GetUTCTime()}',status = '{request.status}' "+
            $" WHERE id = '{request.id}'; ";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(retVal, CMMS.RETRUNSTATUS.SUCCESS, "");

            return response;
        }

        internal async Task<CMDefaultResponse> DeleteCheckPoint(CMCreateCheckPoint request)
        {
            /*
             * Primary Table - CheckPoint
             * Set status 0 for requested id in CheckPoint table
             * Code goes here
            */
            string updateQry = $"UPDATE  softel_cmms.checkpoint SET " +
            $"status  = 0,  updated_at  = '{UtilsRepository.GetUTCTime()}' WHERE  id  = {request.id};";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(retVal, CMMS.RETRUNSTATUS.SUCCESS, "");

            return response;
        }
        #endregion
    }
}
