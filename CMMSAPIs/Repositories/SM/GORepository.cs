using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Collections;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace CMMSAPIs.Repositories
{
    public class GORepository : GenericRepository
    {
        public GORepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }


        internal async Task<List<CMGO>> GetGOList(int plantID, DateTime fromDate, DateTime toDate)
        {
            /*
             * 
            */
            var myQuery = "SELECT pod.ID as podID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_name,sam.asset_type_ID,pod.purchaseID," +
                          "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,po.remarks as rejectedRemark,po.plantID,po.purchaseDate,sam.asset_type_ID,\n\t\t        po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty,po.received_on,po.approvedOn,\n\t\t\t\tCONCAT(ed.Emp_First_Name,' ',ed.Emp_Last_Name) as generatedBy," +
                          "CONCAT(ed1.Emp_First_Name,' ',ed1.Emp_Last_Name) as receivedBy,CONCAT(ed2.Emp_First_Name,' ',ed2.Emp_Last_Name) as approvedBy,\n\t\t\t\tbl.Business_Name as vendor_name\n\t\t        FROM smpurchaseorderdetails pod\n\t\t        LEFT JOIN smpurchaseorder po ON po.ID = pod.purchaseID\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
                          "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code    LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\n\t\t            LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\n\t\t        )  t1 ON t1.master_ID = sam.ID\n\t\t        LEFT JOIN (\n\t\t      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic" +
                          "LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID LEFT JOIN employees ed ON ed.ID = po.generated_by\n\t\t        LEFT JOIN employees ed1 ON ed1.ID = po.receiverID\n\t\t\t\tLEFT JOIN employees ed2 ON ed2.ID = po.approved_by\n\t\t\t\tLEFT JOIN business bl ON bl.id = po.vendorID\n\t\t        /*LEFT JOIN FlexiMC_Emp_details ed ON ed.ID = po.generated_by\n\t\t        LEFT JOIN FlexiMC_Emp_details ed1 ON ed1.ID = po.receiverID\n\t\t\t\tLEFT JOIN FlexiMC_Emp_details ed2 ON ed2.ID = po.approved_by\n\t\t\t\tLEFT JOIN FlexiMC_Business_list bl ON bl.id = po.vendorID\n\t\t       WHERE po.ID =1 */";

            string stmt = "SELECT pod.ID as podID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_name,sam.asset_type_ID,pod.purchaseID," +
                "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,po.remarks as rejectedRemark,po.plantID,po.purchaseDate," +
                "sam.asset_type_ID,\r\n\t\t        po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty," +
                "pod.accepted_qty,po.received_on,po.approvedOn,\r\n\t\t\t\tCONCAT(ed.firstName,' ',ed.lastName) as generatedBy," +
                "CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy,CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy," +
                "\r\n\t\t\t\tbl.name as vendor_name\r\n\t\t        FROM smpurchaseorderdetails pod\r\n\t\t       " +
                " LEFT JOIN smpurchaseorder po ON po.ID = pod.purchaseID\r\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
                " LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code   " +
                " LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\r\n\t\t            " +
                "LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\r\n\t\t        )  t1 ON t1.master_ID = sam.ID\r\n\t\t        " +
                "LEFT JOIN (\r\n\t\t      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic \r\n            " +
                "  LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID " +
                "LEFT JOIN employees ed ON ed.ID = po.generated_by\r\n\t\t        LEFT JOIN employees ed1 ON ed1.ID = po.receiverID\r\n\t\t\t\t " +
                "LEFT JOIN employees ed2 ON ed2.ID = po.approved_by\r\n\t\t\t\t" +
                "LEFT JOIN business bl ON bl.id = po.vendorID\r\n\r\n\t\t     " +
                "WHERE po.plantID = "+plantID+ " and po.purchaseDate >= '"+fromDate.ToString("yyyy-MM-dd")+ "' and po.purchaseDate <= '"+toDate.ToString("yyyy-MM-dd")+"'";
            List<CMGO> _GOList = await Context.GetData<CMGO>(stmt).ConfigureAwait(false);

            //CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_GOList[0].status + 100);
            //string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOB, _Status);
            //_ViewJobList[0].status_short = _shortStatus;

            return _GOList;
        }

        internal async Task<List<CMGO>> GetGOItemByID(int id)
        {
            string stmt = "select * from smpurchaseorder where ID = "+id+"";
            List<CMGO> _GOList = await Context.GetData<CMGO>(stmt).ConfigureAwait(false);
            return _GOList;
        }
        


        internal Task<List<CMGO>> GetAssetCodeDetails(int asset_code)
        {
            /*
             * 
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CreateGO(CMGO request, int userID)
        {

            if (request.go_items != null)
            {
                string poInsertQuery = $" INSERT INTO smpurchaseorder (plantID,vendorID,receiverID,generated_by,purchaseDate,orderDate,status," +
                    $" challan_no,po_no, freight,transport, " +
                    $"no_pkg_received,lr_no,condition_pkg_received,vehicle_no, gir_no, challan_date,po_date, job_ref,amount, currency,withdraw_by,withdrawOn,order_type) " +
                    $"VALUES({request.plantID},{request.vendorID}, {request.receiverID}, {userID}, '{DateTime.Now.ToString("yyyy-MM-dd")}', '{DateTime.Now.ToString("yyyy-MM-dd")}', {request.status}," +
                    $"'','','','', '', '', '','','','00001-01-01','00001-01-01','',0, '',0,'0001-01-01',0);" + 
                    $" SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(poInsertQuery).ConfigureAwait(false);
                int poid = Convert.ToInt32(dt2.Rows[0][0]);

                for (var i = 0; i < request.go_items.Count; i++)
                {
                    string poDetailsQuery = $"INSERT INTO smpurchaseorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID) " +
                    "values(" + poid + ", " + request.go_items[i].assetItemID + ", 0, " + request.go_items[i].cost + ", " + request.go_items[i].ordered_qty + ", " + request.location_ID + ") ; SELECT LAST_INSERT_ID();";
                    DataTable dtInsertPO = await Context.FetchData(poDetailsQuery).ConfigureAwait(false);
                    int id = Convert.ToInt32(dtInsertPO.Rows[0][0]);
                }
            }


            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Goods order created successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateGO(CMGO request, int userID)
        {
            //string mainQuery = $"UPDATE smpurchaseorderdetails SET generate_flag = " +request.generate_flag + ",status = "+request.status+", vendorID = "+request.vendorID+" WHERE ID = "+request.id+"";

            for (var i = 0; i < request.go_items.Count; i++)
            {
                string updateQ = $"UPDATE smpurchaseorderdetails SET assetItemID = {request.go_items[i].assetItemID},cost = {request.go_items[i].cost},ordered_qty = {request.go_items[i].ordered_qty},location_ID = {request.location_ID} WHERE ID = {request.go_items[i].poID}";
                var result = await Context.ExecuteNonQry<int>(updateQ);
            }

            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Goods order updated successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteGO(int GOid, int userID)
        {
            string mainQuery = $"UPDATE smpurchaseorder SET status = 0 WHERE ID = " + GOid + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Goods order deleted.");
            return response;
        }
        internal async Task<CMDefaultResponse> WithdrawGO(CMGO request, int userID)
        {
            string mainQuery = $"UPDATE smpurchaseorder SET withdraw_by = " + userID + ", status = "+request.status+", remarks = '"+request.remarks+"', withdrawOn = '"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE ID = "+request.id+"";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Goods order withdrawn successfully.");
            return response;
        }

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    retValue = "Created";
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue = "Assigned";
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                    retValue = "Linked to PTW";
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue = "Closed";
                    break;
                case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                    retValue = "Cancelled";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }

        public async Task<CMDefaultResponse> GOApproval(CMGO request, int userID)
        {
            try
            {

                DateTime date = DateTime.Now;
                string UpdatesqlQ = $" UPDATE smpurchaseorder SET approved_by = {request.approvedBy}, status = {request.status}, remarks = '{request.remarks}',approvedOn = '{date.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
                await Context.ExecuteNonQry<int>(UpdatesqlQ);
                var data = await this.getPurchaseDetailsByID(request.id);

                string subject = "Goods Order Approval";

                // If the status is GO_APPROVED_BY_MANAGER, then send an email to the following people:
                //   - The vendor
                //   - The store manager
                //   - The person who generated the purchase order

                //GO_APPROVED_BY_MANAGER in constant.cs file it is 
                if (request.status == 1)
                {

                    // Update the asset status.
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].receive_later == 0 && data[i].added_to_store == 0)
                        {

                            var tResult = await TransactionDetails(data[i].plantID, data[i].vendorID, 1, data[i].plantID, 2, data[i].assetItemID, (double)data[i].accepted_qty, 6, request.id, "Purchase Order");

                            // Update the order type.
                            var update_order_type = await updateGOType(data[i].order_by_type, data[i].id);

                            // Update the asset status.
                            if (data[i].spare_status == 2)
                            {
                                await updateAssetStatus(data[i].assetItemID, 4);
                            }
                            else
                            {
                                await updateAssetStatus(data[i].assetItemID, 1);
                            }
                        }
                    }


                    // Get the email addresses of the vendor, store manager, and person who generated the purchase order.
                    //var vendorEmail = await this.GetVendorEmailAsync(data[0].PlantId);
                    //var storeManagerEmail = await this.GetStoreManagerEmailAsync(data[0].PlantId);
                    //var generatedByEmail = await this.GetGeneratedByEmailAsync(data[0].PlantId);


                    // Set the subject of the email.
                    subject = "Goods Order Approved";
                }
                else
                {
                    // Set the subject of the email.
                    subject = "Goods Order Rejected";
                }

          

                
                // Send the email.
                //await this.transactionObj.sendMail('GOApproval', poId, subject, finalArray);

                // Insert a history record.
                
                CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Data Updated.");
                return response;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        // Get order Item Lists
        public async Task<List<CMGO>> getPurchaseDetailsByID(int id)
        {
            string query = "SELECT fc.name as facilityName,pod.ID as podID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_type_ID," +
                "pod.purchaseID,pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,\r\nbl.name as vendor_name,\r\n     " +
                "   po.plantID,po.purchaseDate,sam.asset_type_ID,sam.asset_name,po.receiverID,\r\n        " +
                "po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty," +
                "f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by,pod.order_type, receive_later, " +
                "added_to_store,   \r\n      " +
                "  po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, po.lr_no, po.condition_pkg_received, " +
                "po.vehicle_no, po.gir_no, po.challan_date, po.po_date, po.job_ref, po.amount, po.currency\r\n      " +
                "  FROM smpurchaseorderdetails pod\r\n        LEFT JOIN smpurchaseorder po ON po.ID = pod.purchaseID\r\n     " +
                "   LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID\r\n       " +
                " LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code\r\n      " +
                "  LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement\r\n    " +
                "    LEFT JOIN (\r\n            SELECT file.file_path,file.Asset_master_id as Asset_master_id FROM smassetmasterfiles file \r\n " +
                "           LEFT join smassetmasters sam on file.Asset_master_id =  sam.id )\r\n        " +
                "    f1 ON f1.Asset_master_id = sam.id\r\n        LEFT JOIN (\r\n         " +
                "   SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\r\n      " +
                "      LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\r\n        )  t1 ON t1.master_ID = sam.ID\r\n     " +
                "   LEFT JOIN (\r\n            SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic\r\n          " +
                "  LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID\r\n        )  t2 ON t2.master_ID = sam.ID\r\n " +
                "       LEFT JOIN facilities fc ON fc.id = po.plantID\r\n        LEFT JOIN business bl ON bl.id = po.vendorID  WHERE po.ID = "+id+ " /*GROUP BY pod.ID*/";
            List<CMGO> _List = await Context.GetData<CMGO>(query).ConfigureAwait(false);
            return _List;
        }

        public async Task<bool> TransactionDetails(int plantID, int fromActorID, int fromActorType, int toActorID, int toActorType, int assetItemID, double qty, int refType, int refID, string remarks, int mrsID = 0, int natureOfTransaction = 0, int assetItemStatus = 0)
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


        public async Task<int> updateGOType(int typeID , int poid)
        {
            string stmt = "UPDATE smpurchaseorderdetails set order_type='" + typeID + "', added_to_store = 1 where ID='"+poid+ "'";
            var result = await Context.ExecuteNonQry<int>(stmt);
            return result;
        }

        public async Task<int> updateAssetStatus(int assetItemID, int status)
        {

            string stmt = $"SELECT sam.asset_type_ID FROM smassetitems sai " +
                $"LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code" +
                $" WHERE sai.ID = {assetItemID}";
            DataTable dt = await Context.FetchData(stmt).ConfigureAwait(false);
            int asset_type_ID = Convert.ToInt32(dt.Rows[0][0]);
            if (asset_type_ID > 1)
            {
                string stmtUpdate = $"UPDATE smassetitems SET status = {status} WHERE ID = {assetItemID}";
                var result = await Context.ExecuteNonQry<int>(stmt);
            }
            return 1;
        }

        private async Task<int> DebitTransation(int transactionID, int actorType, int actorID, double debitQty, int assetItemID, int mrsID)
        {
            string stmt = $"INSERT INTO smtransition (transactionID, actorType, actorID, debitQty, assetItemID, mrsID) VALUES ({transactionID}, '{actorType}', {actorID}, {debitQty}, {assetItemID}, {mrsID})";
            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            return id;
        }

        private async Task<int> CreditTransation(int transactionID, int actorID, int actorType, double qty, int assetItemID, int mrsID)
        {
            string query = $"INSERT INTO FlexiMC_SM_Transition (transactionID,actorType,actorID,creditQty,assetItemID,mrsID) VALUES ({transactionID},'{actorType}',{actorID},{qty},{assetItemID},{mrsID})";
            DataTable dt2 = await Context.FetchData(query).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            return id;
        }

        public async Task<bool> VerifyTransactionDetails(int transaction_ID, int debitTransactionID, int creditTransactionID, int plantID, int fromActorID, int fromActorType, int toActorID, int toActorType, int assetItemID, double qty, int refType, int refID, string remarks, int mrsID, bool isS2STransaction = false, int dispatchedQty = 0, int vendor_assetItemID = 0)
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

        // Get PO List Data

        public async Task<List<PurchaseData>> GetPurchaseData(int plantID, string empRole, DateTime fromDate, DateTime toDate, string status, string order_type)
        {
            var stmt = $"SELECT fc.Name as facilityName, po.ID as orderID,po.purchaseDate,po.generate_flag,po.received_on,po.status,bl.name as vendor_name,po.vendorID," +
                $"ed.id as generatedByID,po.remarks, CONCAT(ed.Firstname,' ',ed.lastname) as generatedBy, CONCAT(ed1.Firstname,' ',ed1.lastname) as receivedOn, DATE_FORMAT(po.lastModifiedDate,'%Y-%m-%d') as receivedDate," +
                $" CONCAT(ed2.Firstname,' ',ed2.lastname) as approvedBy,   DATE_FORMAT(po.approvedOn,'%Y-%m-%d') as approvedOn,    po.status as statusFlag "+
                //$" CASE WHEN po.flag = {GO_SAVE_BY_PURCHASE_MANAGER} THEN 'Draft' WHEN po.flag = {GO_SUBMIT_BY_PURCHASE_MANAGER} THEN 'Submitted' " +
                //$"WHEN po.flag = {GO_SAVE_BY_STORE_KEEPER} THEN 'In Process' ELSE 'Received' END" +
             
                $"FROM smpurchaseorder po " +
                $"LEFT JOIN business bl ON bl.id = po.vendorID " +
                $"LEFT JOIN employees ed ON ed.ID = po.generated_by " +
                $"LEFT JOIN employees ed1 ON ed1.ID = po.receiverID " +
                $"LEFT JOIN employees ed2 ON ed2.ID = po.approved_by " +
                $"LEFT JOIN facilities fc ON fc.id = po.plantID WHERE po.plantID = {plantID} ";
            if (!string.IsNullOrEmpty(status))
            {
                stmt += $" AND po.status = {status} ";
            }
            else
            {
                stmt += $" AND po.status > 0 ";
            }
            stmt += $" AND po.order_type = {order_type} AND DATE_FORMAT(po.lastModifiedDate, '%Y-%m-%d') BETWEEN '{fromDate.ToString("yyyy-MM-dd")}' AND '{toDate.ToString("yyyy-MM-dd")}'";


            List<PurchaseData> _List = await Context.GetData<PurchaseData>(stmt).ConfigureAwait(false);
            return _List;
        }


        public async Task<CMDefaultResponse> SubmitPurchaseData(SubmitPurchaseData request)
        {
            try
            {
                // Start a transaction.
                
                // Get the purchase ID if it is not provided.
                int purchaseId;
                if (request.purchaseID == null || request.purchaseID == 0)
                    {
                    purchaseId = await PurchaseOrderAsync(request.facilityId, request.vendor, request.empId, request.purchaseDate, request.generateFlag);
                    if (purchaseId == 0)
                    {
                        throw new Exception("Purchase ID not found");
                    }
                }
                else
                {
                    purchaseId = request.purchaseID;
                }

                // Update the purchase order.
                var stmtUpdateP = $"UPDATE smpurchaseorder SET status = {request.generateFlag}, vendorID = {request.vendor}, purchaseDate = '{request.purchaseDate.ToString("yyyy-MM-dd")}' WHERE ID = {purchaseId}";
                var result = await Context.ExecuteNonQry<int>(stmtUpdateP);

                // Delete the existing purchase order details.
                var stmtDelete = $"DELETE FROM smpurchaseorderdetails WHERE purchaseID = {purchaseId}";
                await Context.ExecuteNonQry<int>(stmtDelete);

                // Insert the new purchase order details.
                foreach (var item in request.submitItems)
                {
                    var assetCode = item.assetCode;
                    var orderedQty = item.orderedQty;
                    var type = item.type;
                    var cost = item.cost;
                    int purchaseOrderDetailsID = 0;
                    // Get the asset type ID.
                    int assetTypeId =0;
                    string stmtAssetType = $"SELECT asset_type_ID FROM smassetmasters WHERE asset_code = '{item.assetCode}'";
                    DataTable dtAssetType = await Context.FetchData(stmtAssetType).ConfigureAwait(false);
                  
                    if (dtAssetType == null && dtAssetType.Rows.Count == 0)
                    {
                        throw new Exception("Asset type ID not found");
                    }
                    else
                    {
                        assetTypeId = Convert.ToInt32(dtAssetType.Rows[0][0]);
                    }
                    int isMultiSelectionEnabled = await getMultiSpareSelectionStatus(item.assetCode);
                    // Check if the asset type is consumable.
                    if (assetTypeId != 307)
                    {
                        // Check if the asset item ID is already exists.
                        int assetItemId;
                        if (item.assetItemID == null || item.assetItemID == 0)
                        {
                            if (isMultiSelectionEnabled > 0)
                            {
                                // Insert the asset item.
                                var stmtI = $"INSERT INTO smassetitems (plant_ID,asset_code,item_condition,status) VALUES ({request.purchaseID},'{assetCode}',1,0); SELECT LAST_INSERT_ID();";
                                DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                                assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);
                                //assetItemIDByCode[assetCode] = assetItemId;
                                stmtI = "";
                                stmtI = $"INSERT INTO smpurchaseorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)"+
                                            $"VALUES({purchaseId},{assetItemId},{item.type},{item.cost},1,0); SELECT LAST_INSERT_ID();";
                                DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                                purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                            }
                            else
                            {
                                // Insert the asset item.
                                var stmtI = $"INSERT INTO smassetitems (plant_ID,asset_code,item_condition,status) VALUES ({request.purchaseID},'{assetCode}',1,0); SELECT LAST_INSERT_ID();";
                                DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                                assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);
                                //assetItemIDByCode[assetCode] = assetItemId;
                                stmtI = "";
                                stmtI = $"INSERT INTO smpurchaseorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
                                            $"VALUES({purchaseId},{assetItemId},{item.type},{item.cost},0,0); SELECT LAST_INSERT_ID();";
                                DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                                purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                            }
                        }
                        else
                        {
                            string stmtI = $"INSERT INTO smpurchaseorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
                                        $"VALUES({purchaseId},{item.assetItemID},{item.type},{item.cost},0,0); SELECT LAST_INSERT_ID();";
                            DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                            purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                        }

                        // Insert the purchase order detail.
                    }
                    else
                    {
                        // Get the asset item ID.
                        int assetItemId = 0;
                        assetItemId = await getAssetItemID(item.assetCode, request.facilityId, 0);
                        if (assetItemId == 0)
                        {
                            throw new Exception("asset_item_ID is empty");
                        }
                        else
                        {
                            string stmtI = $"INSERT INTO smpurchaseorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
                                        $"VALUES({purchaseId},{item.assetItemID},{item.type},{item.cost},0,0); SELECT LAST_INSERT_ID();";
                            DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                            purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                        }

                        
                    }
                }

                CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Goods order submitted successfully.");
                return response;
            }
            catch (Exception e)
            {
                CMDefaultResponse response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.SUCCESS, "Goods order failed to submit.");
                return response;
            }
        }

        public async Task<int> PurchaseOrderAsync(int facilityId, int vendor, int empId, DateTime? purchaseDate, int generateFlag)
        {
            int purchaseID = 0;

            string stmtSelect = $"SELECT * FROM smpurchaseorder WHERE plantID = {facilityId}"+
                                $"AND vendorID = {vendor} AND generated_by = {empId} AND purchaseDate = '{purchaseDate.Value.ToString("yyyy-MM-dd")}' AND status = {generateFlag}";

            DataTable dt1 = await Context.FetchData(stmtSelect).ConfigureAwait(false);
            if (dt1 != null)
            {
                purchaseID = Convert.ToInt32(dt1.Rows[0]["ID"]);
                string stmtUpdate = $"UPDATE smpurchaseorder SET generate_flag = {generateFlag}, status = {generateFlag}, vendorID = {vendor} WHERE ID = {purchaseID}";
                var result = await Context.ExecuteNonQry<int>(stmtUpdate);
            }
            else
            {
                string stmt = $"INSERT INTO smpurchaseorder (plantID,vendorID,generated_by,purchaseDate,status,generate_flag) " +
                    $"VALUES({facilityId}, {vendor}, {empId}, '{purchaseDate.Value.ToString("yyyy-MM-dd")}', {generateFlag},{generateFlag}); SELECT LAST_INSERT_ID();";
                DataTable dtInsert = await Context.FetchData(stmt).ConfigureAwait(false);
                purchaseID = Convert.ToInt32(dtInsert.Rows[0][0]);
            }
            return purchaseID;
        }

        protected async Task<int> getMultiSpareSelectionStatus(string asset_code = "", int asset_ID = 0)
        {
            string stmt = "";
            if (!string.IsNullOrEmpty(asset_code))
            {

                stmt = $"SELECT f_sum.spare_multi_selection FROM smassetmasters sam JOIN smunitmeasurement f_sum ON sam.unit_of_measurement = f_sum.ID WHERE sam.asset_code = '{asset_code}'";
            }
            else if (asset_ID > 0)
            {

                stmt = $"SELECT f_sum.spare_multi_selection FROM smassetitems sai JOIN smassetmasters sam ON sai.asset_code = sam.asset_code JOIN smunitmeasurement f_sum ON sam.unit_of_measurement = f_sum.ID WHERE sai.ID = {asset_ID}";
            }

            List<CMUnitMeasurement> _checkList = await Context.GetData<CMUnitMeasurement>(stmt).ConfigureAwait(false);
            return _checkList[0].spare_multi_selection;
        }

        protected async Task<int> getAssetItemID(string asset_code = "", int facility_id = 0, int location_ID = 0)
        {
            int asset_item_ID = 0;
            if (!string.IsNullOrEmpty(asset_code))
            {

                string stmt = $"SELECT ID FROM smassetitems WHERE asset_code = '{asset_code}' AND plant_ID = {facility_id}";
                DataTable dt = await Context.FetchData(stmt).ConfigureAwait(false);
                asset_item_ID = Convert.ToInt32(dt.Rows[0][0]);
            }
            else
            {
                string stmtI = $"INSERT INTO smassetitems (plant_ID,asset_code,location_ID,item_condition,status) VALUES ({facility_id},'{asset_code}',{location_ID},1,1); SELECT LAST_INSERT_ID();";
                DataTable dt = await Context.FetchData(stmtI).ConfigureAwait(false);
                asset_item_ID = Convert.ToInt32(dt.Rows[0][0]);
            }


            return asset_item_ID;
        }
    }
}
