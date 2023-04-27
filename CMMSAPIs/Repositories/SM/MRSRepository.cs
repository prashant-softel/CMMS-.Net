using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.SM;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Utils;
using System.Data;
using CMMSAPIs.Models.Users;
using MySql.Data.MySqlClient;
using System.Transactions;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.ComponentModel;
using Google.Protobuf.WellKnownTypes;


namespace CMMSAPIs.Repositories.SM
{
    public class MRSRepository : GenericRepository
    {

        public MRSRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<MRS>> getMRSList(int plant_ID, int emp_id, DateTime toDate, DateTime fromDate)
        {
            string stmt = "SELECT sm.ID,sm.requested_by_emp_ID,CONCAT(ed1.firstName,' ',ed1.lastName) as approver_name,DATE_FORMAT(sm.requested_date,'%Y-%m-%d') as requestd_date,DATE_FORMAT(sm.returnDate,'%Y-%m-%d') as returnDate,if(sm.approval_status != '',DATE_FORMAT(sm.approved_date,'%d-%m-%Y'),'') as approval_date,sm.approval_status,sm.approval_comment,CONCAT(ed.firstName,' ',ed.lastName) as emp_name, sm.flag FROM smmrs sm LEFT JOIN users ed ON ed.id = sm.requested_by_emp_ID LEFT JOIN users ed1 ON ed1.id = sm.approved_by_emp_ID WHERE sm.plant_ID = " + plant_ID + " AND ed.id = " + emp_id + " AND (DATE_FORMAT(sm.requested_date,'%Y-%m-%d') BETWEEN '" + toDate + "' AND '" + fromDate + "' OR DATE_FORMAT(sm.returnDate,'%Y-%m-%d') BETWEEN '" + toDate + "' AND '" + fromDate + "')";
            List<MRS> _List = await Context.GetData<MRS>(stmt).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> requestMRS(MRS request)
        {
            /* This is incomplete code */
            bool Queryflag = false;
            CMDefaultResponse response = null;
            string flag = "MRS_REQUEST";
            int setAsTemplate = 0;
            if(request.setTemplateflag != null)
            {
                setAsTemplate = 1;
            }
            if (request.isEditMode == 1)
            {
                // update MRS

                var lastMRSID = request.ID;
                var refType = "MRSEdit";
                var mailSub = "MRS Request Updated";
                string updatestmt = $" START TRANSACTION; UPDATE smmrs SET plant_ID = {request.plant_ID}, requested_by_emp_ID = {request.requested_by_emp_ID}, requested_date = {request.requestd_date}," +
                    $"status = '0', flag = {request.flag}, setAsTemplate = {request.setAsTemplate}, templateName = {request.templateName}, approval_status = {request.approval_status} WHERE ID = {request.ID}" +
                    $"DELETE FROM smrsitems WHERE mrs_ID =  {lastMRSID} ; IF @@ERROR <> 0 THEN\r\n    ROLLBACK;\r\nELSE\r\n    COMMIT;\r\nEND IF;";
                await Context.ExecuteNonQry<int>(updatestmt);

            }
            else
            {
                var refType = "MRS";
                var mailSub = "MRS Request";
                string insertStmt = $"START TRANSACTION; INSERT INTO smmrs (plant_ID,requested_by_emp_ID,requested_date," +
                    $"status,flag,setAsTemplate,templateName)\r\n VALUES ({request.plant_ID},{request.requested_by_emp_ID},{request.requestd_date}" +
                    $",0,{request.flag},{request.setAsTemplate},'{request.templateName}'); SELECT LAST_INSERT_ID(); IF @@ERROR <> 0 THEN\r\n    ROLLBACK;\r\nELSE\r\n    COMMIT;\r\n END IF;";
                DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                request.ID = Convert.ToInt32(dt2.Rows[0][0]);
            }

            if (request.ID != null)
            {
             for(var i=0; i<request.equipments.Count; i++) {

                    int equipmentID = request.equipments[i].equipmentID;
                    decimal quantity = request.equipments[i].qty;

                    string selectQuery = "SELECT sam.approval_required, sat.asset_code, asset_type_ID FROM smassetitems sat " +
                               "LEFT JOIN smassetmasters sam ON sam.asset_code = sat.asset_code " +
                               "WHERE sat.ID = " + equipmentID;
                    
                   List<CMSMMaster> assetList = await Context.GetData<CMSMMaster>(selectQuery).ConfigureAwait(false);
                    var approval_required = assetList[0].approval_required;
                    var asset_type_ID = assetList[0].asset_type_ID;
                    var asset_code = assetList[0].asset_code;
                    var IsSpareSelectionEnable = getMultiSpareSelectionStatus(asset_code, asset_type_ID);
                    if(Convert.ToInt32(IsSpareSelectionEnable) == 0 || asset_type_ID != 0)
                    {
                        try
                        {
                            string insertStmt = $"START TRANSACTION; " +
                            $"INSERT INTO FlexiMC_SM_MRS_Items (mrs_ID,asset_item_ID,asset_MDM_code,requested_qty,status,flag,approval_required)" +
                            $"VALUES ({request.ID},{request.equipments[i].equipmentID},'{asset_code}',{request.equipments[i].qty},0,0,{approval_required})" +
                            $" SELECT LAST_INSERT_ID(); IF @@ERROR <> 0 THEN\r\n    ROLLBACK;\r\nELSE\r\n    COMMIT;\r\n END IF;";
                            DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                        }catch(Exception ex) 
                        { throw ex; }
                    }
                    else
                    {
                        try
                        {
                            string insertStmt = $"START TRANSACTION; " +
                            $"INSERT INTO FlexiMC_SM_MRS_Items (mrs_ID,asset_item_ID,asset_MDM_code,requested_qty,status,flag,approval_required)" +
                            $"VALUES ({request.ID},{request.equipments[i].equipmentID},'{asset_code}',1,0,0,{approval_required})" +
                            $" SELECT LAST_INSERT_ID(); IF @@ERROR <> 0 THEN\r\n    ROLLBACK;\r\nELSE\r\n    COMMIT;\r\n END IF;";
                            DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                        }catch (Exception ex)
                        { throw ex; }
                    }
                    if (Convert.ToInt32(IsSpareSelectionEnable) == 1 && asset_type_ID != 2){
                         UpdateAssetStatus(request.equipments[i].equipmentID, 2);
                }
                    Queryflag = true;
            }
        }
            if (Queryflag)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to submit MRS.");
            }else
            {
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Request has been submitted.");
            }
                return response;
        }


        public async void UpdateAssetStatus(int assetItemID, int status)
        {
            string stmt = "SELECT sam.asset_type_ID FROM smassetitems sai " +
                          "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code " +
                          $"WHERE sai.ID = {assetItemID}";
            List<MRS> _List = await Context.GetData<MRS>(stmt).ConfigureAwait(false);
            if (_List != null  && _List[0].asset_type_ID > 0)
            {
                string stmtUpdate = $"UPDATE smassetitems SET status = {status} WHERE ID = {assetItemID}";
                await Context.ExecuteNonQry<int>(stmtUpdate);
            }

        }



        protected async Task<int> getMultiSpareSelectionStatus(string asset_code = "", int asset_ID = 0)
        {
            string stmt = "";
            if (!string.IsNullOrEmpty(asset_code))
            {

                stmt = $"SELECT f_sum.spare_multi_selection FROM smassetmasters sam JOIN smunitmeasurement f_sum ON sam.unit_of_measurement = f_sum.ID WHERE sam.asset_code = '{asset_code}'";
            }
            else if (asset_ID>0)
            {

                stmt = $"SELECT f_sum.spare_multi_selection FROM smassetitems sai JOIN smassetmasters sam ON sai.asset_code = sam.asset_code JOIN smunitmeasurement f_sum ON sam.unit_of_measurement = f_sum.ID WHERE sai.ID = {asset_code}";
            }

            List<UnitMeasurement> _checkList = await Context.GetData<UnitMeasurement>(stmt).ConfigureAwait(false);
            return _checkList[0].spare_multi_selection;
        } 
        internal async Task<List<MRS>> getMRSItems(int ID)
        {
            string stmt = "SELECT smi.ID,smi.return_remarks,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID,smi.asset_MDM_code," +
                "t1.serial_number,smi.returned_qty,smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,sm.flag,DATE_FORMAT(sm.returnDate,'%Y-%m-%d') as returnDate," +
                "sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d') as approved_date,DATE_FORMAT(sm.requested_date,'%Y-%m-%d') as issued_date," +
                "DATE_FORMAT(sm.returnDate, '%Y-%m-%d') as returnDate, smi.requested_qty,if(smi.approval_required = 1,'Yes','No') as approval_required,\r\n " +
                "t1.asset_name,t1.asset_type_ID,t1.asset_type,COALESCE(t1.file_path,'') as file_path,t1.Asset_master_id\r\n        FROM smrsitems smi\r\n " +
                " LEFT JOIN smmrs sm ON sm.ID = smi.mrs_ID         \r\n        LEFT JOIN (SELECT sai.ID as asset_item_ID, sai.serial_number, sam.asset_name, " +
                "sam.asset_type_ID,sat.asset_type,COALESCE(file.file_path,'') as file_path,file.Asset_master_id\r\n        FROM smassetitems sai  " +
                "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code LEFT JOIN smassetmasterfiles  file ON file.Asset_master_id =  sam.ID " +
                "LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID) as t1 ON t1.asset_item_ID = smi.asset_item_ID" +
                "  WHERE smi.mrs_ID = "+ID+" GROUP BY smi.ID";
            List<MRS> _List = await Context.GetData<MRS>(stmt).ConfigureAwait(false);
            return _List;
        }

        internal async Task<List<MRS>> getMRSItemsBeforeIssue(int ID)
        {

            string stmt = "SELECT smi.ID,smi.return_remarks,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID,smi.asset_MDM_code," +
                "smi.returned_qty,smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,sm.flag,DATE_FORMAT(sm.returnDate,'%Y-%m-%d') as returnDate," +
                "sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d') as approved_date,DATE_FORMAT(sm.requested_date,'%Y-%m-%d') as issued_date," +
                "DATE_FORMAT(sm.returnDate, '%Y-%m-%d') as returnDate, smi.requested_qty,if(smi.approval_required = 1,'Yes','No') as approval_required," +
                "\r\n        t1.asset_name,t1.asset_type_ID,t1.asset_type,COALESCE(t1.file_path,'') as file_path,t1.Asset_master_id, t1.spare_multi_selection" +
                "     FROM smrsitems smi\r\n        LEFT JOIN smmrs sm ON sm.ID = smi.mrs_ID   LEFT JOIN (SELECT sai.ID as asset_item_ID, sai.asset_MDM_code, " +
                "sam.asset_name, sam.asset_type_ID,sat.asset_type,spare_multi_selection, COALESCE(file.file_path,'') as file_path,file.Asset_master_id   " +
                "FROM smrsitems sai  LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_MDM_code  LEFT JOIN smassetmasterfiles  file " +
                "ON file.Asset_master_id =  sam.ID  LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID  LEFT JOIN smunitmeasurement f_sum " +
                "ON  f_sum.id = sam.unit_of_measurement      ) as t1 ON t1.asset_MDM_code = smi.asset_MDM_code where smi.mrs_ID = "+ID+" GROUP BY smi.ID";
            List<MRS> _List = await Context.GetData<MRS>(stmt).ConfigureAwait(false);
            return _List;
        }


        internal async Task<List<MRS>> getMRSItemsWithCode(int ID)
        {
            string stmt = "SELECT smi.ID,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID,smi.asset_MDM_code,smi.returned_qty," +
                "smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,sm.flag,DATE_FORMAT(sm.returnDate,'%Y-%m-%d') as returnDate," +
                "sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d') as approved_date,DATE_FORMAT(sm.requested_date,'%Y-%m-%d') as issued_date," +
                "DATE_FORMAT(sm.returnDate, '%Y-%m-%d') as returnDate, sum(smi.requested_qty) as requested_qty,if(smi.approval_required = 1," +
                "'Yes','No') as approval_required, \r\n        sam.asset_name, sam.asset_type_ID,sat.asset_type," +
                "COALESCE(file.file_path,'') as file_path,file.Asset_master_id, f_sum.spare_multi_selection, sai.serial_number" +
                " FROM smrsitems smi  \r\n   LEFT JOIN smmrs sm ON sm.ID = smi.mrs_ID \r\n LEFT JOIN smassetmasterfilessmassetmasters sam " +
                "ON sam.asset_code = smi.asset_MDM_code \r\n    LEFT JOIN smassettypessmassetmasterfiles file ON file.Asset_master_id = sam.ID " +
                "\r\n        LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID\r\n      " +
                "  LEFT JOIN smunitmeasurement f_sum ON  f_sum.id = sam.unit_of_measurement\r\n      " +
                "  LEFT JOIN smassetitems sai ON  sai.ID = smi.asset_item_ID\r\n       " +
                " WHERE smi.mrs_ID = "+ID+" GROUP BY smi.asset_MDM_code";
            List<MRS> _List = await Context.GetData<MRS>(stmt).ConfigureAwait(false);
            return _List;
        }

        internal async Task<List<MRS>> getMRSDetails(int ID)
        {
            string stmt = $"SELECT * FROM FlexiMC_SM_MRS WHERE ID = {ID}";
            List<MRS> _List = await Context.GetData<MRS>(stmt).ConfigureAwait(false);
            return _List;
        }
    }
}
