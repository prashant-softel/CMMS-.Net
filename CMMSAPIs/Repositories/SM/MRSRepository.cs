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
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Org.BouncyCastle.Ocsp;
using System.Collections;
using System.Security.Cryptography.Xml;

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
                            $"INSERT INTO smrsitems (mrs_ID,asset_item_ID,asset_MDM_code,requested_qty,status,flag,approval_required)" +
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
                            $"INSERT INTO smrsitems (mrs_ID,asset_item_ID,asset_MDM_code,requested_qty,status,flag,approval_required)" +
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

                stmt = $"SELECT f_sum.spare_multi_selection FROM smassetitems sai JOIN smassetmasters sam ON sai.asset_code = sam.asset_code JOIN smunitmeasurement f_sum ON sam.unit_of_measurement = f_sum.ID WHERE sai.ID = {asset_ID}";
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
            string stmt = "SELECT smi.ID,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID,smi.asset_MDM_code," +
                "smi.returned_qty,smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,\r\nsm.flag," +
                "DATE_FORMAT(sm.returnDate,'%Y-%m-%d') as returnDate,sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d') as approved_date," +
                "\r\nDATE_FORMAT(sm.requested_date,'%Y-%m-%d') as issued_date,DATE_FORMAT(sm.returnDate, '%Y-%m-%d') as returnDate," +
                " sum(smi.requested_qty) as requested_qty,if(smi.approval_required = 1,'Yes','No') as approval_required, " +
                "\r\n        '' as asset_name, sam.ID as asset_type_ID,sat.asset_type,COALESCE(file.file_path,'') as file_path," +
                "file.Asset_master_id, f_sum.spare_multi_selection, sai.serial_number \r\n       " +
                " FROM smrsitems smi \r\n        LEFT JOIN smmrs sm ON sm.ID = smi.mrs_ID \r\n       " +
                " LEFT JOIN smassetitems sam ON sam.asset_code = smi.asset_MDM_code \r\n       " +
                " LEFT JOIN smassetmasterfiles file ON file.Asset_master_id = sam.ID \r\n        " +
                "LEFT JOIN smassettypes sat ON sat.ID = sam.plant_ID\r\n        " +
                "LEFT JOIN smunitmeasurement f_sum ON  f_sum.id = sam.plant_ID\r\n        " +
                "LEFT JOIN smassetitems sai ON  sai.ID = smi.asset_item_ID" +
                " WHERE smi.mrs_ID = "+ID+" GROUP BY smi.asset_MDM_code";
            List<MRS> _List = await Context.GetData<MRS>(stmt).ConfigureAwait(false);
            return _List;
        }

        internal async Task<List<MRS>> getMRSDetails(int ID)
        {
            string stmt = $"SELECT * FROM smmrs WHERE ID = {ID}";
            List<MRS> _List = await Context.GetData<MRS>(stmt).ConfigureAwait(false);
            return _List;
        }
        internal async Task<List<MRS>> getReturnDataByID(int ID)
        {
            string stmt = $"SELECT * FROM smrsitems WHERE ID = {ID}";
            List<MRS> _List = await Context.GetData<MRS>(stmt).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> mrsApproval(MRS request)
        {
            CMDefaultResponse response = null;
            bool Queryflag = false;
            var flag = "MRS_RETURN_REQUEST";
            string comment = "";
            string remarks = "";
            string stmtSelect = $"SELECT * FROM smmrs WHERE ID = {request.ID}";
            List<MRS> mrsList = await Context.GetData<MRS>(stmtSelect).ConfigureAwait(false);

            var equipmentName = new List<int>();
            for (int i = 0; i < request.equipments.Count; i++)
            {
                // equipmentID = 3 for SM_ASSET_TYPE_SPARE
                if (request.equipments[i].equipmentID == 3)
                {
                    decimal availableQty = Convert.ToDecimal(GetAvailableQty(request.asset_item_ID, mrsList[0].plant_ID));

                    if (availableQty < request.equipments[i].issued_qty)
                    {
                        equipmentName.Add(request.equipments[i].id);
                    }
                }
            }

            if (equipmentName != null && equipmentName.Count >0)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Sorry, " + string.Join(",", equipmentName) + " serial no.(s) has lower quantity than issued.");
                return response;
            }

            for(int i=0; i < request.equipments.Count; i++)
            {
                bool callUpdateAssetStatus = false;
                string stmtUpdate = $"UPDATE smrsitems SET available_qty={request.equipments[i].qty}, issued_qty={request.equipments[i].issued_qty}";
                // SM_ASSET_TYPE_SPARE = 3
                if (request.equipments[i].equipmentID == 3 && request.asset_item_ID != null)
                {
                    stmtUpdate = stmtUpdate + $" , asset_item_ID = {request.asset_item_ID}";
                    request.asset_item_ID = request.asset_item_ID;
                    if (request.equipments[i].equipmentID == 1)
                    {
                        callUpdateAssetStatus = true;
                    }
                }

                stmtUpdate += $" WHERE ID = {request.equipments[i].id}";
                await Context.ExecuteNonQry<int>(stmtUpdate);
                Queryflag = true;   

                if(request.approval_status == 1)
                {
                    if (callUpdateAssetStatus)
                    {
                        UpdateAssetStatus(request.asset_item_ID, 3);
                    }
                    int requested_by_emp_ID = mrsList[0].requested_by_emp_ID;
                    if (Convert.ToInt32(mrsList[0].reference) == 4)
                    {
                        remarks = "JOBCard : JC" + mrsList[0].referenceID;
                    }
                    if (Convert.ToInt32(mrsList[0].reference) == 5)
                    {
                        remarks = "Prementive Maintainance : PM" + mrsList[0].referenceID;
                    }
                    var refID = mrsList[0].ID;
                    var tResult = await TransactionDetails(mrsList[0].plant_ID, mrsList[0].plant_ID,2 , mrsList[0].requested_by_emp_ID,3, request.equipments[0].id, Convert.ToInt32(request.equipments[0].issued_qty), Convert.ToInt32(mrsList[0].reference), Convert.ToInt32(mrsList[0].referenceID),"", refID);
                    if (!tResult) {

                        throw new Exception("transaction details failed");
                    }
                    comment = "MRS Request Approved";
                }
                else
                {
                      comment = "MRS Request Rejected";
                        UpdateAssetStatus(request.asset_item_ID, 1); // if it is MRS reject reset the status of assets
                }
                }

            if (!Queryflag)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to update data.");
            }
            else
            {
                string stmt = $"UPDATE smmrs SET approved_by_emp_ID = {request.approved_by_emp_ID}, approved_date={request.approved_date},"+ 
                               $"approval_status ={request.approval_status},approval_comment = {request.return_remarks},flag = 1 WHERE ID = {request.ID}";
                await Context.ExecuteNonQry<int>(stmt);
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Status updated.");
            }
            return response;
        }

        public async Task<bool> TransactionDetails(int plantID, int fromActorID, int fromActorType, int toActorID, int toActorType, int assetItemID, int qty, int refType, int refID, string remarks, int mrsID = 0, int natureOfTransaction = 0, int assetItemStatus = 0)
        {
            try
            {
                string stmt = "INSERT INTO smtransactiondetails (plantID,fromActorID,fromActorType,toActorID,toActorType,assetItemID,qty,referedby,reference_ID,remarks,Nature_Of_Transaction,Asset_Item_Status)" +
                              $"VALUES ({plantID},{fromActorID},{fromActorType},{toActorID},{toActorType},{assetItemID},{qty},{refType},{refID},'{remarks}',{natureOfTransaction},{assetItemStatus})";
      
                DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
                int transaction_ID = 0;
                if (dt2.Rows.Count > 0)
                {
                    transaction_ID = Convert.ToInt32(dt2.Rows[0][0]);
                    int debitTransactionID = await DebitTransation(transaction_ID, fromActorID, fromActorType, qty, assetItemID, mrsID);
                    int creditTransactionID = await CreditTransation(transaction_ID, toActorID, toActorType, qty, assetItemID, mrsID);
                    bool isValid = await VerifyTransactionDetails(transaction_ID, debitTransactionID, creditTransactionID, plantID, fromActorID, fromActorType, toActorID, toActorType, assetItemID, qty, refType, refID, remarks, mrsID);
                    if (isValid)
                    {
                        //minQtyReminder(assetItemID, plantID);
                        return true;
                    }
                    else
                    {
                        throw new Exception("transaction table not updated properly");
                    }
                }
                else
                {
                    throw new Exception("transaction table not updated properly");
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<int> DebitTransation(int transactionID, int actorType, int actorID, int debitQty, int assetItemID, int mrsID)
        {
            string stmt = $"INSERT INTO smtransition (transactionID, actorType, actorID, debitQty, assetItemID, mrsID) VALUES ({transactionID}, '{actorType}', {actorID}, {debitQty}, {assetItemID}, {mrsID})";
            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            return id;
        }

        private async Task<int> CreditTransation(int transactionID, int actorID, int actorType, int qty, int assetItemID, int mrsID)
        {
            string query = $"INSERT INTO FlexiMC_SM_Transition (transactionID,actorType,actorID,creditQty,assetItemID,mrsID) VALUES ({transactionID},'{actorType}',{actorID},{qty},{assetItemID},{mrsID})";
            DataTable dt2 = await Context.FetchData(query).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            return id;
        }

        public async Task<bool>  VerifyTransactionDetails(int transaction_ID, int debitTransactionID, int creditTransactionID, int plantID, int fromActorID, int fromActorType, int toActorID, int toActorType, int assetItemID, int qty, int refType, int refID, string remarks, int mrsID, bool isS2STransaction = false, int dispatchedQty = 0, int vendor_assetItemID = 0)
        {
            string qry = $"SELECT ID FROM smtransactiondetails WHERE ID = '{transaction_ID}' AND plantID = '{plantID}' AND fromActorID = '{fromActorID}' AND fromActorType = '{fromActorType}' AND toActorID = '{toActorID}' AND toActorType = '{toActorType}' AND assetItemID = '{assetItemID}' AND qty = '{qty}' AND referedby = '{refType}' AND reference_ID ='{refID}'";

            string qry2;
            if (isS2STransaction)
            {
                qry2 = $"SELECT ID FROM smtransition WHERE ID = '{debitTransactionID}' AND actorType = '{fromActorType}' AND actorID = '{fromActorID}' AND debitQty = '{dispatchedQty}' AND assetItemID = '{vendor_assetItemID}'";
            }
            else
            {
                qry2 = $"SELECT ID FROM smtransition WHERE ID = '{debitTransactionID}' AND actorType = '{fromActorType}' AND actorID = '{fromActorID}' AND debitQty = '{qty}' AND assetItemID = '{assetItemID}'";
            }

            string qry3 = $"SELECT ID FROM smtransition WHERE ID = '{creditTransactionID}' AND actorType = '{toActorType}' AND actorID = '{toActorID}' AND creditQty = '{qty}' AND assetItemID = '{assetItemID}'";

            DataTable dt1 = await Context.FetchData(qry).ConfigureAwait(false);
            DataTable dt2 = await Context.FetchData(qry2).ConfigureAwait(false);
            DataTable dt3 = await Context.FetchData(qry3).ConfigureAwait(false);

            int transactionDetailID = 0;
            int transactionDebitID = 0;
            int transactionCreditID = 0;

            transactionDetailID = Convert.ToInt32(dt1.Rows[0][0]);        
            transactionDebitID = Convert.ToInt32(dt2.Rows[0][0]);
            transactionCreditID = Convert.ToInt32(dt3.Rows[0][0]);


            if (transactionDetailID != 0 && transactionDebitID != 0 && transactionCreditID != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal async Task<CMDefaultResponse> mrsReturn(MRS request)
        {
            CMDefaultResponse response = null; 
            bool Queryflag = false;
            var flag = "MRS_RETURN_REQUEST";
            
            if (request.isEditMode == 1)
            {
                // Updating existing MRS;

                var lastMRSID = request.ID;
                var refType = "MRSReturnEdit";
                var mailSub = "MRS Return Request Updated.";
                string updatestmt = $" START TRANSACTION; UPDATE smmrs SET plant_ID = {request.plant_ID}, requested_by_emp_ID = {request.requested_by_emp_ID}, requested_date = {request.returnDate}," +
                    $"status = '0', flag = {request.flag}, approval_status = {request.approval_status} WHERE ID = {request.ID}" +
                    $"DELETE FROM smrsitems WHERE mrs_ID =  {lastMRSID} ; IF @@ERROR <> 0 THEN\r\n    ROLLBACK;\r\nELSE\r\n    COMMIT;\r\nEND IF;";
                try
                {
                    await Context.ExecuteNonQry<int>(updatestmt);
                }catch(Exception ex)
                {
                    Queryflag = false;
                    throw ex;
                }

            }
            else
            {
                var refType = "MRSReturn";
                var mailSub = "MRS Return Request";
                string insertStmt = $"START TRANSACTION; INSERT INTO smmrs (plant_ID,requested_by_emp_ID,requested_date," +
                    $"returnDate,flag)\r\n VALUES ({request.plant_ID},{request.requested_by_emp_ID},{request.requestd_date}" +
                    $",{request.returnDate},{request.flag}); SELECT LAST_INSERT_ID(); IF @@ERROR <> 0 THEN\r\n    ROLLBACK;\r\nELSE\r\n    COMMIT;\r\n END IF;";
                try
                {
                    DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                    request.ID = Convert.ToInt32(dt2.Rows[0][0]);
                }catch(Exception ex)
                {
                    Queryflag = false;
                    throw ex;
                }
            }
            if (request.equipments != null)
            {
                for (var i = 0; i < request.equipments.Count; i++)
                {

                    int equipmentID = request.equipments[i].equipmentID;
                    decimal quantity = request.equipments[i].qty;

                    
                        try
                        {
                            string insertStmt = $"START TRANSACTION; " +
                            $"INSERT INTO smrsitems (mrs_ID,mrs_return_ID,asset_item_ID,available_qty,requested_qty,return_remarks,flag)" +
                            $"VALUES ({request.ID},{request.equipments[i].id},{request.equipments[i].equipmentID},{request.equipments[i].qty}, {request.equipments[i].requested_qty}, {request.equipments[i].return_remarks}, 2)" +
                            $" SELECT LAST_INSERT_ID(); IF @@ERROR <> 0 THEN\r\n    ROLLBACK;\r\nELSE\r\n    COMMIT;\r\n END IF;";
                            DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);

                        string updatestmt = $"UPDATE smassetitems SET item_condition = {request.item_condition}, status = {request.status} WHERE ID = {request.asset_item_ID}";
                        await Context.ExecuteNonQry<int>(updatestmt);
                    }
                        catch (Exception ex)
                        {
                        Queryflag = false;
                        throw ex; }
                    
                    
                    Queryflag = true;
                }
            }
            if (Queryflag)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to submit MRS return.");
            }
            else
            {
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "MRS return submitted.");
            }
            return response;
        }
        internal async Task<CMDefaultResponse> mrsReturnApproval(MRS request)
        {
            CMDefaultResponse response = null;
            var executeUpdateStmt = 0 ;
            bool Queryflag = false;
            string comment = "";

            string stmtSelect = "SELECT * FROM smmrs WHERE ID = "+request.ID+"";
            List<MRS> mrsList = await Context.GetData<MRS>(stmtSelect).ConfigureAwait(false);
            for (int i = 0; i < request.equipments.Count; i++)
            {
                decimal req_qty = request.equipments[i].returned_qty;
                //MRS_REQUEST_REJECT contains 2 as enum value
                if (request.approval_status == 2)
                {
                    req_qty = 0;
                }
                string stmt = "UPDATE smrsitems SET returned_qty = " + request.equipments[i].returned_qty + ", finalRemark = '" + request.equipments[i].return_remarks + "' WHERE ID = " + request.equipments[i].id;  
                try
                {
                  executeUpdateStmt = await Context.ExecuteNonQry<int>(stmt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (executeUpdateStmt == null || executeUpdateStmt == 0)
                {
                    return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to update MRS item return.");
                }
                Queryflag = true;

                // MRS_REQUEST_APPROVE == 1 in constant.cs file
                if (request.approval_status == 1)
                {
                    var tResult = await TransactionDetails(mrsList[0].plant_ID, mrsList[0].requested_by_emp_ID, 2, mrsList[0].plant_ID, 2, request.equipments[0].id, Convert.ToInt32(request.equipments[0].received_qty), Convert.ToInt32(mrsList[0].reference), request.ID, request.return_remarks, request.mrs_return_ID);
                    if (!tResult)
                    {
                        return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Transaction details failed.");                        
                    }
                }
            }

            if (request.approval_status == 1) {
                comment = "MRS Return Approved";
            } else
            {
                comment = "MRS Return Rejected. Reason : "+request.return_remarks+"";
            }

            if (Queryflag)
            {
                var stmtUpdate = $"UPDATE smmrs SET approved_by_emp_ID = {request.approved_by_emp_ID}, approved_date = '{request.approved_date}'," + 
                $"approval_status = {request.approval_status}, approval_comment = '{request.return_remarks}'"+
                $"WHERE ID = {request.ID}";
                try
                {
                    executeUpdateStmt = await Context.ExecuteNonQry<int>(stmtUpdate);
                }
                catch (Exception ex)
                {
                    return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "MRS Item update details failed.");
                }
                if(executeUpdateStmt == 0 || executeUpdateStmt == null)
                {
                    return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "MRS Item update details failed.");
                }

            }
            string msg = request.approval_status == 1 ? "Equipment returned to store." : "MRS Return Rejected.";
            response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, msg);
            return response;
        }
        internal async Task<List<MRS>> getAssetTypeByItemID(int ItemID)
        {
                   string stmt = "SELECT sat.asset_type,sam.asset_code,sam.asset_name,sat.ID,sai.ID as item_ID,sai.plant_ID,sai.serial_number,sam.asset_type_ID,sm.decimal_status,COALESCE(file.file_path,'') as file_path,file.Asset_master_id, f_sum.spare_multi_selection FROM smassetitems sai " +
                            "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code " +
                            "LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement " +
                            "LEFT JOIN smassetmasterfiles file ON file.Asset_master_id = sam.ID " +
                            "LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID " +
                            "LEFT JOIN smunitmeasurement f_sum ON f_sum.ID = sam.unit_of_measurement " +
                            "WHERE(sai.ID = " +ItemID+ " OR sai.serial_number = " + ItemID + " OR  sai.asset_code = " + ItemID + ")";
            List<MRS> _List = await Context.GetData<MRS>(stmt).ConfigureAwait(false);
            var isMultiSpareSelectionStatus = getMultiSpareSelectionStatus("", ItemID);
           
            if (_List[0].asset_type_ID == 2 || (_List[0].asset_type_ID == 3 && Convert.ToInt32(isMultiSpareSelectionStatus) == 0))
            {
                _List[0].available_qty = await GetAvailableQty(_List[0].asset_item_ID, _List[0].plant_ID);
            }
            else
            {
                _List[0].available_qty = await GetAvailableQtyByCode(_List[0].asset_MDM_code, _List[0].plant_ID);
            }

            return _List;
        }

        public async Task<int> GetAvailableQty(int assetItemID, int plantID)
        {
            string actorType = "Store";
            string stmt = "SELECT SUM(debitQty) as drQty, SUM(creditQty) as crQty FROM smtransition WHERE assetItemID = " + assetItemID.ToString() + " AND actorType = '" + actorType + "' AND transactionID IN (SELECT ID FROM smtransactiondetails WHERE plantID = " + plantID.ToString() + ")";
            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
            int crQty = 0, drQty = 0;
            while (dt2 != null && dt2.Rows.Count >0)
            {
                crQty = Convert.ToInt32(dt2.Rows[0]["crQty"]);
                drQty = Convert.ToInt32(dt2.Rows[0]["drQty"]);
            }
            int availableQty = crQty - drQty;
            return availableQty;
        }
        public async Task<int> GetAvailableQtyByCode(string assetCode, int plantID)
        {
            string actorType = "Store";
            string stmt = "SELECT SUM(debitQty) as drQty, SUM(creditQty) as crQty FROM  smtransition WHERE assetItemID IN (SELECT ID FROM smassetitems WHERE asset_code = '" + assetCode + "' AND plant_ID = " + plantID.ToString() + ") AND actorType = '" + actorType + "' AND transactionID IN (SELECT ID FROM smtransactiondetails WHERE plantID = " + plantID.ToString() + ")";
            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
            int crQty = 0, drQty = 0;
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                crQty = dt2.Rows[0]["crQty"] == "" || dt2.Rows[0]["crQty"] == DBNull.Value ? 0 : Convert.ToInt32(dt2.Rows[0]["crQty"]);
                drQty = dt2.Rows[0]["drQty"] == "" || dt2.Rows[0]["drQty"] == DBNull.Value ? 0 : Convert.ToInt32(dt2.Rows[0]["drQty"]);
            }
            int availableQty = crQty - drQty;
            return availableQty;
        }

        

    }
}
