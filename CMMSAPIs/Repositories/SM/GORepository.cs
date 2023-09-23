using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Repositories.Utils;
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
        private UtilsRepository _utilsRepo;
        public GORepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.GO_DRAFT:
                    retValue = "Drafted";
                    break;
                case CMMS.CMMS_Status.GO_SUBMITTED:
                    retValue = "Submitted";
                    break;
               
                case CMMS.CMMS_Status.GO_CLOSED:
                    retValue = "Closed";
                    break;

                case CMMS.CMMS_Status.GO_DELETED:
                    retValue = "Deleted";
                    break;


                case CMMS.CMMS_Status.GO_REJECTED:
                    retValue = "Rejected";
                    break;
                case CMMS.CMMS_Status.GO_APPROVED:
                    retValue = "Approved";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVE_DRAFT:
                    retValue = "Receive draft";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED:
                    retValue = "Receive submitted";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_REJECTED:
                    retValue = "Receive rejected";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_APPROVED:
                    retValue = "Receive approved";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }

        private static string getLongStatus(CMMS.CMMS_Status m_notificationID, int Id)
        {
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)m_notificationID;
            string retValue = "";
            switch (status)
            {
                case CMMS.CMMS_Status.GO_DRAFT:
                    retValue = $" Goods order {Id} Drafted";
                    break;
                case CMMS.CMMS_Status.GO_SUBMITTED:
                    retValue = $"Goods order {Id} Waiting for Appoval";
                    break;

                case CMMS.CMMS_Status.GO_CLOSED:
                    retValue = $"Goods order {Id} Closed";
                    break;

                case CMMS.CMMS_Status.GO_DELETED:
                    retValue = $"Goods order {Id} deleted";
                    break;
                case CMMS.CMMS_Status.GO_REJECTED:
                    retValue = $"Goods order {Id} rejected";
                    break;
                case CMMS.CMMS_Status.GO_APPROVED:
                    retValue = $"Goods order {Id} approved";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVE_DRAFT:
                    retValue = $"Goods order {Id} receive draft";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED:
                    retValue = $"Goods order {Id} receive submitted";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_REJECTED:
                    retValue = $"Goods order {Id} receive rejected";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_APPROVED:
                    retValue = $"Goods order {Id} receive approved";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;
        }

        //internal async Task<List<CMGoodsOrderDetailList>> GetGOList(int facility_id, DateTime fromDate, DateTime toDate)
        //{
        //    /*
        //     * 
        //    */
        //    string filter = " (DATE(po.purchaseDate) >= '" + fromDate.ToString("yyyy-MM-dd") + "'  and DATE(po.purchaseDate) <= '" + toDate.ToString("yyyy-MM-dd") + "')";

        //    filter = filter + " and facilityID = " + facility_id + "";

        //    //var myQuery = "SELECT pod.ID as podID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_name,sam.asset_type_ID,pod.purchaseID," +
        //    //              "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,po.remarks as rejectedRemark,po.plantID,po.purchaseDate,sam.asset_type_ID,\n\t\t        po.vendorID,po.flag,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty,po.received_on,po.approvedOn,\n\t\t\t\tCONCAT(ed.Emp_First_Name,' ',ed.Emp_Last_Name) as generatedBy," +
        //    //              "CONCAT(ed1.Emp_First_Name,' ',ed1.Emp_Last_Name) as receivedBy,CONCAT(ed2.Emp_First_Name,' ',ed2.Emp_Last_Name) as approvedBy,\n\t\t\t\tbl.Business_Name as vendor_name\n\t\t        FROM smpurchaseorderdetails pod\n\t\t        LEFT JOIN smpurchaseorder po ON po.ID = pod.purchaseID\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
        //    //              "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code    LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\n\t\t            LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\n\t\t        )  t1 ON t1.master_ID = sam.ID\n\t\t        LEFT JOIN (\n\t\t      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic" +
        //    //              "LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID LEFT JOIN employees ed ON ed.ID = po.generated_by\n\t\t        LEFT JOIN employees ed1 ON ed1.ID = po.receiverID\n\t\t\t\tLEFT JOIN employees ed2 ON ed2.ID = po.approved_by\n\t\t\t\tLEFT JOIN business bl ON bl.id = po.vendorID\n\t\t        /*LEFT JOIN FlexiMC_Emp_details ed ON ed.ID = po.generated_by\n\t\t        LEFT JOIN FlexiMC_Emp_details ed1 ON ed1.ID = po.receiverID\n\t\t\t\tLEFT JOIN FlexiMC_Emp_details ed2 ON ed2.ID = po.approved_by\n\t\t\t\tLEFT JOIN FlexiMC_Business_list bl ON bl.id = po.vendorID\n\t\t       WHERE po.ID =1 */";

        //    string stmt = "SELECT pod.ID ,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_name,sam.asset_type_ID,pod.purchaseID," +
        //        "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost ,pod.ordered_qty,po.remarks as rejectedRemark,po.facilityID as facility_id,po.purchaseDate,fac.name as facility_name, " +
        //        "sam.asset_type_ID,\r\n\t\t        po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty," +
        //        "pod.accepted_qty,po.received_on as receivedAt,po.approvedOn as approvedAt,\r\n\t\t\t\tCONCAT(ed.firstName,' ',ed.lastName) as generatedBy," +
        //        "CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy,CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy," +
        //        "\r\n\t\t\t\tbl.name as vendor_name\r\n\t\t        FROM smpurchaseorderdetails pod\r\n\t\t       " +
        //        " LEFT JOIN smpurchaseorder po ON po.ID = pod.purchaseID\r\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
        //        " LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code   " +
        //        " LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\r\n\t\t            " +
        //        "LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\r\n\t\t        )  t1 ON t1.master_ID = sam.ID\r\n\t\t        " +
        //        "LEFT JOIN (\r\n\t\t      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic \r\n            " +
        //        "  LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID " +
        //        "LEFT JOIN employees ed ON ed.ID = po.generated_by\r\n\t\t        LEFT JOIN employees ed1 ON ed1.ID = po.receiverID\r\n\t\t\t\t " +
        //        "LEFT JOIN employees ed2 ON ed2.ID = po.approved_by\r\n\t\t\t\t" +
        //        "LEFT JOIN business bl ON bl.id = po.vendorID  " +
        //        "LEFT JOIN facilities fac on fac.id =  po.facilityID  WHERE " + filter +
        //        "/*  WHERE po.ID =1 */";
        //    List<CMGoodsOrderDetailList> _GOList = await Context.GetData<CMGoodsOrderDetailList>(stmt).ConfigureAwait(false);

        //    for (var i = 0; i < _GOList.Count; i++)
        //    {
        //        CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_GOList[i].status);
        //        string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_PO, _Status);
        //        _GOList[i].status_short = _shortStatus;
        //    }

        //    return _GOList;
        //}
        internal async Task<CMGoodsOrderList> GetGOItemByID(int id)
        {
            string query = "SELECT fc.name as facilityName,pod.ID as podID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_type_ID," +
          "pod.purchaseID,pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,\r\nbl.name as vendor_name,\r\n     " +
          "   po.facilityID as facility_id,po.purchaseDate,sam.asset_type_ID,sam.asset_name,po.receiverID,\r\n        " +
          "po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty," +
          "f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by,pod.order_type, receive_later, " +
          "added_to_store,   \r\n      " +
          "  po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, po.lr_no, po.condition_pkg_received, " +
          "po.vehicle_no, po.gir_no, po.challan_date,  po.job_ref, po.amount, po.currency as currencyID , curr.name as currency \r\n      " +
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
          "       LEFT JOIN facilities fc ON fc.id = po.facilityID\r\n        LEFT JOIN business bl ON bl.id = po.vendorID   LEFT JOIN currency curr ON curr.id = po.currency WHERE po.ID = " + id + " /*GROUP BY pod.ID*/";
            List<CMGoodsOrderList> _List = await Context.GetData<CMGoodsOrderList>(query).ConfigureAwait(false);
            return _List[0];
        }



        internal Task<List<CMGoodsOrderList>> GetAssetCodeDetails(int asset_code)
        {
            /*
             * 
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CreateGO(CMGoodsOrderList request, int userID)
        {
            int poid = 0;
            if (request.go_items != null)
            {
                string poInsertQuery = $" INSERT INTO smpurchaseorder (facilityID,vendorID,receiverID,generated_by,purchaseDate,orderDate,status," +
                    $" challan_no,po_no, freight,transport, " +
                    $"no_pkg_received,lr_no,condition_pkg_received,vehicle_no, gir_no, challan_date,po_date, job_ref,amount, currency,withdraw_by,withdrawOn,order_type,received_on) " +
                    $"VALUES({request.facility_id},{request.vendorID}, {request.receiverID}, {userID}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', {(int)CMMS.CMMS_Status.GO_SUBMITTED}," +
                    $"'{request.challan_no}','{request.po_no}','{request.freight}','', '{request.no_pkg_received}', '{request.lr_no}', '{request.condition_pkg_received}','{request.vehicle_no}','{request.gir_no}','{request.challan_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}'," +
                    $"'{request.po_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}','{request.job_ref}',{request.amount}, {request.currencyID},0,'0001-01-01',0,'{request.receivedAt.ToString("yyyy-MM-dd HH:mm:ss")}');" +
                    $" SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(poInsertQuery).ConfigureAwait(false);
                poid = Convert.ToInt32(dt2.Rows[0][0]);

                for (var i = 0; i < request.go_items.Count; i++)
                {
                    string poDetailsQuery = $"INSERT INTO smpurchaseorderdetails (purchaseID,assetItemID,cost,ordered_qty,location_ID, paid_by_ID) " +
                    "values(" + poid + ", " + request.go_items[i].assetItemID + ",  " + request.go_items[i].cost + ", " + request.go_items[i].ordered_qty + ", " + request.location_ID + ", " + request.go_items[i].paid_by_ID +") ; SELECT LAST_INSERT_ID();";
                    DataTable dtInsertPO = await Context.FetchData(poDetailsQuery).ConfigureAwait(false);
                    int id = Convert.ToInt32(dtInsertPO.Rows[0][0]);
                }
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, poid, 0, 0, "Goods order created", CMMS.CMMS_Status.GO_DRAFT);
            CMDefaultResponse response = new CMDefaultResponse(poid, CMMS.RETRUNSTATUS.SUCCESS, "Goods order created successfully.");
            return response;
        }
          internal async Task<CMDefaultResponse> UpdateGO(CMGoodsOrderList request, int userID)
        {
            string OrderQuery = "";
            if (request.is_submit == 0)
            {
                OrderQuery = $"UPDATE smpurchaseorder SET " +
                        $"challan_no = '{request.challan_no}',po_no='{request.po_no}', freight='{request.freight}',no_pkg_received='{request.no_pkg_received}'," +
                        $"lr_no='{request.lr_no}',condition_pkg_received='{request.condition_pkg_received}',vehicle_no='{request.vehicle_no}', gir_no='{request.gir_no}', " +
                        $"challan_date = '{request.challan_date.Value.ToString("yyyy-MM-dd")}', " +
                        $"job_ref='{request.job_ref}',amount='{request.amount}', currency={request.currencyID},updated_by = {userID},updatedOn = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.GO_DRAFT}, received_on = '{request.receivedAt.ToString("yyyy-MM-dd HH:mm:ss")}', po_date = '{request.po_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', purchaseDate= '{request.purchaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}' where ID={request.id}";
            }
            else
            {
                OrderQuery = $"UPDATE smpurchaseorder SET " +
                        $"challan_no = '{request.challan_no}',po_no='{request.po_no}', freight='{request.freight}',no_pkg_received='{request.no_pkg_received}'," +
                        $"lr_no='{request.lr_no}',condition_pkg_received='{request.condition_pkg_received}',vehicle_no='{request.vehicle_no}', gir_no='{request.gir_no}', " +
                        $"challan_date = '{request.challan_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                        $"job_ref='{request.job_ref}',amount='{request.amount}', currency={request.currencyID},updated_by = {userID},updatedOn = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.GO_SUBMITTED}, received_on = '{request.receivedAt.ToString("yyyy-MM-dd HH:mm:ss")}', po_date = '{request.po_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', purchaseDate= '{request.purchaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}' where ID={request.id}";
            }
            await Context.ExecuteNonQry<int>(OrderQuery);

            for (var i = 0; i < request.go_items.Count; i++)
            {
                string itemsQuery = $"UPDATE smpurchaseorderdetails SET location_ID = {request.location_ID},assetItemID = {request.go_items[i].assetItemID},cost = {request.go_items[i].cost}, accepted_qty = {request.go_items[i].accepted_qty},ordered_qty = {request.go_items[i].ordered_qty} , requested_qty = {request.go_items[i].requested_qty}, received_qty= {request.go_items[i].received_qty},lost_qty = {request.go_items[i].lost_qty}, damaged_qty={request.go_items[i].damaged_qty}, paid_by_ID = {request.go_items[i].paid_by_ID}" +
                    $" WHERE ID = {request.go_items[i].goItemID}";
                //string itemsQuery = $"UPDATE smpurchaseorderdetails SET location_ID = {request.location_ID},assetItemID = {request.go_items[i].assetItemID},cost = {request.go_items[i].cost}, accepted_qty = {request.go_items[i].ordered_qty + request.go_items[i].received_qty},ordered_qty = {request.go_items[i].ordered_qty} , requested_qty = {request.go_items[i].requested_qty}, received_qty= {request.go_items[i].received_qty},lost_qty = {request.go_items[i].lost_qty}, damaged_qty={request.go_items[i].damaged_qty}, paid_by_ID = {request.go_items[i].paid_by_ID}" +
                //    $" WHERE ID = {request.go_items[i].goItemID}";
                var result = await Context.ExecuteNonQry<int>(itemsQuery);
            }

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Goods order updated successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteGO(CMApproval request, int userID)
        {
            string mainQuery = $"UPDATE smpurchaseorder SET status = {(int)CMMS.CMMS_Status.GO_DELETED}, remarks = '{request.comment}', updated_by = {userID}, updatedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}' WHERE ID = " + request.id + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, request.id, 0, 0, "Goods order deleted.", CMMS.CMMS_Status.GO_DELETED);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Goods order deleted.");
            return response;
        }
        internal async Task<CMDefaultResponse> CloseGO(CMGoodsOrderList request, int userID)
        {
            string mainQuery = $"UPDATE smpurchaseorder SET withdraw_by = " + userID + ", status = " + (int)CMMS.CMMS_Status.GO_CLOSED + ", remarks = '" + request.remarks + "', withdrawOn = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE ID = " + request.id + "";
            await Context.ExecuteNonQry<int>(mainQuery);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, request.id, 0, 0, "Goods order withdrawn.", CMMS.CMMS_Status.GO_CLOSED);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Goods order withdrawn successfully.");
            return response;
        }


        //public async Task<CMDefaultResponse> GOApproval(CMGoodsOrderList request, int userID)
        //{
        //    try
        //    {

        //        DateTime date = DateTime.Now;
        //        string UpdatesqlQ = $" UPDATE smpurchaseorder SET approved_by = {request.approvedBy}, status = {request.status}, remarks = '{request.remarks}',approvedOn = '{date.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
        //        await Context.ExecuteNonQry<int>(UpdatesqlQ);
        //        var data = await this.GetGODetailsByID(request.id);

        //        string subject = "Goods Order Approval";


        //        //GO_APPROVED_BY_MANAGER in constant.cs file it is 
        //        if (request.status == 335)
        //        {

        //            // Update the asset status.
        //            for (int i = 0; i < data.Count; i++)
        //            {
        //                if (data[i].receive_later == 0 && data[i].added_to_store == 0)
        //                {

        //                    var tResult = await TransactionDetails(data[i].plantID, data[i].vendorID, 1, data[i].plantID, 2, data[i].assetItemID, (double)data[i].accepted_qty, 6, request.id, "Goods Order");

        //                    // Update the order type.
        //                    var update_order_type = await updateGOType(data[i].order_by_type, data[i].id);

        //                    // Update the asset status.
        //                    if (data[i].spare_status == 2)
        //                    {
        //                        await updateAssetStatus(data[i].assetItemID, 4);
        //                    }
        //                    else
        //                    {
        //                        await updateAssetStatus(data[i].assetItemID, 1);
        //                    }
        //                }
        //            }


        //            // Get the email addresses of the vendor, store manager, and person who generated the Goods Order.
        //            //var vendorEmail = await this.GetVendorEmailAsync(data[0].PlantId);
        //            //var storeManagerEmail = await this.GetStoreManagerEmailAsync(data[0].PlantId);
        //            //var generatedByEmail = await this.GetGeneratedByEmailAsync(data[0].PlantId);


        //            // Set the subject of the email.
        //            subject = "Goods Order Approved";
        //        }
        //        else
        //        {
        //            // Set the subject of the email.
        //            subject = "Goods Order Rejected";
        //        }




        //        // Send the email.
        //        //await this.transactionObj.sendMail('GOApproval', poId, subject, finalArray);

        //        // Insert a history record.

        //        CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Data Updated.");
        //        return response;
        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }
        //}

        internal async Task<CMDefaultResponse> ApproveGoodsOrder(CMApproval request, int userId)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string UpdatesqlQ = $" UPDATE smpurchaseorder SET approved_by = {userId}, status = {(int)CMMS.CMMS_Status.GO_APPROVED}, remarks = '{request.comment}',approvedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(UpdatesqlQ).ConfigureAwait(false);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;



            // Entry in TransactionDetails
            var data = await this.getPurchaseDetailsByID(request.id);
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].receive_later == 1 && data[i].added_to_store == 0)
                {

                    decimal stock_qty = data[i].ordered_qty + data[i].received_qty;
                    var tResult = await TransactionDetails(data[i].facility_id, data[i].vendorID, (int)CMMS.SM_Types.Vendor, data[i].facility_id, (int)CMMS.SM_Types.Store, data[i].assetItemID, (double)stock_qty, (int)CMMS.CMMS_Modules.SM_GO, request.id, "Goods Order");

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
            string myQuery = $"SELECT   ID ,facilityID ,vendorID ,receiverID ,generated_by ,purchaseDate ,orderDate ,challan_no ,po_no ,\r\n     " +
                $" freight ,transport ,no_pkg_received ,lr_no ,\r\n      " +
                $"condition_pkg_received ,vehicle_no ,gir_no ,challan_date ,po_date ,\r\n      " +
                $"job_ref ,amount ,currency as currencyID,status ,\r\n     " +
                $" lastModifiedDate ,generate_flag ,s2s_generated_by ,received_on ,invoice_copy ,issued_by ,issued_on ,\r\n    " +
                $"  approved_by ,approvedOn ,withdraw_by ,withdrawOn ,reorder_flag ,\r\n     " +
                $" order_type ,remarks ,updated_by ,updatedOn ,rejected_by ,rejectedOn" +
                $" \r\nFROM smpurchaseorder where id = {request.id}";
            List<CMGoodsOrderList> _List = await Context.GetData<CMGoodsOrderList>(myQuery).ConfigureAwait(false);
            //CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_PO, CMMS.CMMS_Status.SM_PO_CLOSED_APPROVED, _List[0]);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, request.id, 0, 0, "Goods order approved.", CMMS.CMMS_Status.GO_APPROVED);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Goods Order {request.id} approved successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectGoodsOrder(CMApproval request, int userId)
        {

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }


            string approveQuery = $"Update smpurchaseorder set status = {(int)CMMS.CMMS_Status.GO_REJECTED} , remarks = '{request.comment}' , " +
                $" rejected_by = {userId}, rejectedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}'" +
                $" where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, request.id, 0, 0, "Rejected goods order", CMMS.CMMS_Status.GO_REJECTED);



            string myQuery = $"SELECT   ID ,facilityID ,vendorID ,receiverID ,generated_by ,purchaseDate ,orderDate ,challan_no ,po_no ,\r\n     " +
       $" freight ,transport ,no_pkg_received ,lr_no ,\r\n      " +
       $"condition_pkg_received ,vehicle_no ,gir_no ,challan_date ,po_date ,\r\n      " +
       $"job_ref ,amount ,currency as currencyID,status ,\r\n     " +
       $" lastModifiedDate ,generate_flag ,s2s_generated_by ,received_on ,invoice_copy ,issued_by ,issued_on ,\r\n    " +
       $"  approved_by ,approvedOn ,withdraw_by ,withdrawOn ,reorder_flag ,\r\n     " +
       $" order_type ,remarks ,updated_by ,updatedOn ,rejected_by ,rejectedOn" +
       $" \r\nFROM smpurchaseorder where id = {request.id}";
            List<CMGoodsOrderList> _WCList = await Context.GetData<CMGoodsOrderList>(myQuery).ConfigureAwait(false);
            //CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_PO, CMMS.CMMS_Status.SM_PO_CLOSED_REJECTED, _WCList[0]);

            

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Goods Order {request.id} rejected successfully.");
            
            return response;
        }

        // Get order Item Lists
        public async Task<List<CMGoodsOrderList>> getPurchaseDetailsByID(int id)
        {
            string query = "SELECT fc.name as facilityName,pod.ID as podID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_type_ID," +
                "pod.purchaseID,pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,\r\nbl.name as vendor_name,\r\n     " +
                "   po.facilityID as facility_id,po.purchaseDate,sam.asset_type_ID,sam.asset_name,po.receiverID,\r\n        " +
                "po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty," +
                "f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by,pod.order_type, receive_later, " +
                "added_to_store,   \r\n      " +
                "  po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, po.lr_no, po.condition_pkg_received, " +
                "po.vehicle_no, po.gir_no, po.challan_date,  po.job_ref, po.amount, po.currency as currencyID , curr.name as currency \r\n      " +
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
                "       LEFT JOIN facilities fc ON fc.id = po.facilityID\r\n        LEFT JOIN business bl ON bl.id = po.vendorID   LEFT JOIN currency curr ON curr.id = po.currency WHERE po.ID = " + id + " /*GROUP BY pod.ID*/";
            List<CMGoodsOrderList> _List = await Context.GetData<CMGoodsOrderList>(query).ConfigureAwait(false);
            return _List;
        }


        public async Task<bool> TransactionDetails(int plantID, int fromActorID, int fromActorType, int toActorID, int toActorType, int assetItemID, double qty, int refType, int refID, string remarks, int mrsID = 0, int natureOfTransaction = 0, int assetItemStatus = 0)
        {
            try
            {
                string stmt = "INSERT INTO smtransactiondetails (plantID,fromActorID,fromActorType,toActorID,toActorType,assetItemID,qty,referedby,reference_ID,remarks,Nature_Of_Transaction,Asset_Item_Status)" +
                              $"VALUES ({plantID},{fromActorID},{fromActorType},{toActorID},{toActorType},{assetItemID},{qty},{refType},{refID},'{remarks}',{natureOfTransaction},{assetItemStatus}) ; SELECT LAST_INSERT_ID();";

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


        public async Task<int> updateGOType(int typeID, int poid)
        {
            string stmt = "UPDATE smpurchaseorderdetails set order_type='" + typeID + "', added_to_store = 1 where ID='" + poid + "'";
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

        private async Task<int> DebitTransation(int transactionID, int actorID, int  actorType, double debitQty, int assetItemID, int mrsID)
        {
            string stmt = $"INSERT INTO smtransition (transactionID, actorType, actorID, debitQty, assetItemID, mrsID) VALUES ({transactionID}, '{actorType}', {actorID}, {debitQty}, {assetItemID}, {mrsID}); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            return id;
        }

        private async Task<int> CreditTransation(int transactionID, int actorID, int actorType, double qty, int assetItemID, int mrsID)
        {
            string query = $"INSERT INTO smtransition (transactionID,actorType,actorID,creditQty,assetItemID,mrsID) VALUES ({transactionID},'{actorType}',{actorID},{qty},{assetItemID},{mrsID}) ; SELECT LAST_INSERT_ID();";
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
                qry2 = $"SELECT ID FROM smtransition WHERE ID = '{debitTransactionID}' AND actorType = '{fromActorID}' AND actorID = '{fromActorType}' AND debitQty = '{qty}' AND assetItemID = '{assetItemID}'";
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

        public async Task<List<CMPURCHASEDATA>> GetPurchaseData(int plantID, string empRole, DateTime fromDate, DateTime toDate, string status, string order_type)
        {
            var stmt = $"SELECT fc.Name as facilityName, po.ID as orderID,po.purchaseDate,po.generate_flag,po.received_on,po.status,bl.name as vendor_name,po.vendorID," +
                $"ed.id as generatedByID,po.remarks, CONCAT(ed.firstName,' ',ed.lastName) as generatedBy, CONCAT(ed1.firstName,' ',ed1.lastName) as receivedOn, DATE_FORMAT(po.lastModifiedDate,'%Y-%m-%d') as receivedDate," +
                $" CONCAT(ed2.Firstname,' ',ed2.lastname) as approvedBy,   DATE_FORMAT(po.approvedOn,'%Y-%m-%d') as approvedOn,    po.status as statusFlag " +
                //$" CASE WHEN po.flag = {GO_SAVE_BY_PURCHASE_MANAGER} THEN 'Draft' WHEN po.flag = {GO_SUBMIT_BY_PURCHASE_MANAGER} THEN 'Submitted' " +
                //$"WHEN po.flag = {GO_SAVE_BY_STORE_KEEPER} THEN 'In Process' ELSE 'Received' END" +

                $"FROM smpurchaseorder po " +
                $"LEFT JOIN business bl ON bl.id = po.vendorID " +
                $"LEFT JOIN users ed ON ed.id = po.generated_by " +
                $"LEFT JOIN users ed1 ON ed1.id = po.receiverID " +
                $"LEFT JOIN users ed2 ON ed2.id = po.approved_by " +
                $"LEFT JOIN facilities fc ON fc.id = po.facilityID WHERE po.facilityID = {plantID} ";
            if (!string.IsNullOrEmpty(status))
            {
                stmt += $" AND po.status = {status} ";
            }
            else
            {
                stmt += $" AND po.status > 0 ";
            }
            //stmt += $" AND po.order_type = {order_type} AND DATE_FORMAT(po.lastModifiedDate, '%Y-%m-%d') BETWEEN '{fromDate.ToString("yyyy-MM-dd")}' AND '{toDate.ToString("yyyy-MM-dd")}'";


            List<CMPURCHASEDATA> _List = await Context.GetData<CMPURCHASEDATA>(stmt).ConfigureAwait(false);
            return _List;
        }


        public async Task<CMDefaultResponse> SubmitPurchaseData(CMSUBMITPURCHASEDATA request)
        {
            try
            {
                // Start a transaction.

                // Get the purchase ID if it is not provided.
                int purchaseId = 0;
                //if (request.purchaseID == null || request.purchaseID == 0)
                //    {
                //    purchaseId = await PurchaseOrderAsync(request.facilityId, request.vendor, request.empId, request.purchaseDate, request.generateFlag);
                //    if (purchaseId == 0)
                //    {
                //        throw new Exception("Purchase ID not found");
                //    }
                //}
                //else
                //{
                //    purchaseId = request.purchaseID;
                //}

                purchaseId = request.purchaseID;

                // Update the Goods Order.
                var stmtUpdateP = $"UPDATE smpurchaseorder SET status = {(int)CMMS.CMMS_Status.GO_SUBMITTED} WHERE ID = {purchaseId}";
                var result = await Context.ExecuteNonQry<int>(stmtUpdateP);

                // Delete the existing Goods Order details.
                var stmtDelete = $"DELETE FROM smpurchaseorderdetails WHERE purchaseID = {purchaseId}";
                await Context.ExecuteNonQry<int>(stmtDelete);

                // Insert the new Goods Order details.
                foreach (var item in request.submitItems)
                {
                    var assetCode = item.assetCode;
                    var orderedQty = item.orderedQty;
                    var type = item.type;
                    var cost = item.cost;
                    int purchaseOrderDetailsID = 0;
                    // Get the asset type ID.
                    int assetTypeId = 0;
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
                                var stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,item_condition,status) VALUES ({request.purchaseID},'{assetCode}',1,0); SELECT LAST_INSERT_ID();";
                                DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                                assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);
                                //assetItemIDByCode[assetCode] = assetItemId;
                                stmtI = "";
                                stmtI = $"INSERT INTO smpurchaseorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
                                            $"VALUES({purchaseId},{assetItemId},{item.type},{item.cost},1,0); SELECT LAST_INSERT_ID();";
                                DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                                purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                            }
                            else
                            {
                                // Insert the asset item.
                                var stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,item_condition,status) VALUES ({request.purchaseID},'{assetCode}',1,0); SELECT LAST_INSERT_ID();";
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

                        // Insert the Goods Order detail.
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
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, purchaseId, 0, 0, "Goods order submitted.", CMMS.CMMS_Status.GO_SUBMITTED);

                CMDefaultResponse response = new CMDefaultResponse(purchaseId, CMMS.RETRUNSTATUS.SUCCESS, "Goods order submitted successfully.");
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

            string stmtSelect = $"SELECT * FROM smpurchaseorder WHERE facilityID = {facilityId}" +
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
                string stmt = $"INSERT INTO smpurchaseorder (facilityID,vendorID,generated_by,purchaseDate,status,generate_flag) " +
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

                string stmt = $"SELECT ID FROM smassetitems WHERE asset_code = '{asset_code}' AND facility_ID = {facility_id}";
                DataTable dt = await Context.FetchData(stmt).ConfigureAwait(false);
                asset_item_ID = Convert.ToInt32(dt.Rows[0][0]);
            }
            else
            {
                string stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,location_ID,item_condition,status) VALUES ({facility_id},'{asset_code}',{location_ID},1,1); SELECT LAST_INSERT_ID();";
                DataTable dt = await Context.FetchData(stmtI).ConfigureAwait(false);
                asset_item_ID = Convert.ToInt32(dt.Rows[0][0]);
            }


            return asset_item_ID;
        }
        public async Task<CMGOMaster> GetGODetailsByID(int id)
        {
            string query = "SELECT fc.name as facilityName,pod.ID as podID, facilityid as       facility_id,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_type_ID," +
                "pod.purchaseID,pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,\r\n bl.name as vendor_name,\r\n     " +
                " po.purchaseDate,sam.asset_type_ID,sam.asset_name,po.receiverID,\r\n        " +
                "po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty," +
                "f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by,pod.order_type as asset_type_ID_OrderDetails, receive_later, " +
                "added_to_store,   \r\n      " +
                "  po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, po.lr_no, po.condition_pkg_received, " +
                "po.vehicle_no, po.gir_no, po.challan_date, po.job_ref, po.amount,  po.currency as currencyID , curr.name as currency , stt.asset_type as asset_type_Name,  po_no, po_date, requested_qty,lost_qty, ordered_qty\r\n    ,paid_by_ID, smpaidby.paid_by paid_by_name , po.received_on as receivedAt,sam.asset_type_ID,sam.asset_code,sam.asset_name" +
                " , sic.cat_name,smat.asset_type FROM smpurchaseorderdetails pod\r\n        LEFT JOIN smpurchaseorder po ON po.ID = pod.purchaseID\r\n     " +
                "   LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID\r\n       " +
                " LEFT JOIN smassetmasters sam ON  sam.asset_code = sai.asset_code\r\n      " +
                "  LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement\r\n    " +
                "    LEFT JOIN (\r\n            SELECT file.file_path,file.Asset_master_id as Asset_master_id FROM smassetmasterfiles file \r\n " +
                "           LEFT join smassetmasters sam on file.Asset_master_id =  sam.id )\r\n        " +
                "    f1 ON f1.Asset_master_id = sam.id\r\n        LEFT JOIN (\r\n         " +
                "   SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\r\n      " +
                "      LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\r\n        )  t1 ON t1.master_ID = sam.ID\r\n     " +
                "   LEFT JOIN (\r\n            SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic\r\n          " +
                "  LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID\r\n        )  t2 ON t2.master_ID = sam.ID\r\n " +
                "       LEFT JOIN facilities fc ON fc.id = po.facilityID\r\nLEFT JOIN users as vendor on vendor.id=po.vendorID " +
                "       LEFT JOIN business bl ON bl.id = po.vendorID left join smassettypes stt on stt.ID = pod.order_type LEFT JOIN currency curr ON curr.id = po.currency LEFT JOIN smpaidby as smpaidby on smpaidby.ID = pod.paid_by_ID " +
                " LEFT JOIN smitemcategory sic ON sic.ID = sam.item_category_ID\r\n    LEFT JOIN smassettypes smat ON smat.ID = sam.asset_type_ID " +
                " WHERE po.ID = " + id + " /*GROUP BY pod.ID*/";
            List<CMGoodsOrderList> _List = await Context.GetData<CMGoodsOrderList>(query).ConfigureAwait(false);

            CMGOMaster _MasterList = _List.Select(p => new CMGOMaster
            {
                Id = p.purchaseID,
                facility_id = p.facility_id,
                facilityName = p.facilityName,
                //asset_type_ID = p.asset_type_ID,
                vendorID = p.vendorID,
                vendor_name = p.vendor_name,
                status = p.status,
                accepted_qty = p.ordered_qty,
                currencyID = p.currencyID,
                currency = p.currency,
                amount = p.amount,
                job_ref = p.job_ref,
                gir_no = p.gir_no,
                vehicle_no = p.vehicle_no,
                condition_pkg_received = p.condition_pkg_received,
                lr_no = p.lr_no,
                no_pkg_received = p.no_pkg_received,
                receivedAt = p.receivedAt,
                freight = p.freight,
                po_date = p.po_date,
                po_no = p.po_no,
                challan_date = p.challan_date,
                challan_no = p.challan_no,
                purchaseDate = p.purchaseDate,
                location_ID = p.location_ID,

            }).FirstOrDefault();
            if (_List.Count > 0)
            {
                List<CMGODetails> _itemList = _List.Select(p => new CMGODetails
                {
                    id = p.podID,
                    cost = p.cost,
                    assetItem_Name = p.asset_name,
                    assetItemID = p.assetItemID,
                    location_ID = p.location_ID,
                    accepted_qty = p.accepted_qty,
                    spare_status = p.spare_status,
                    remarks = p.remarks,
                    receive_later = p.receive_later,
                    requested_qty = p.requested_qty,
                    received_qty = p.received_qty,
                    lost_qty = p.lost_qty,
                    damaged_qty = p.damaged_qty,
                    ordered_qty = p.ordered_qty,
                    paid_by_name = p.paid_by_name,
                    paid_by_ID = p.paid_by_ID,
                    asset_type_ID = p.asset_type_ID,
                    asset_code = p.asset_code,
                    cat_name = p.cat_name,
                    asset_type = p.asset_type


    }).ToList();
                _MasterList.GODetails = _itemList;

                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_MasterList.status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_PO, _Status);
                string _longStatus = getLongStatus(_Status, _MasterList.Id);
                
                _MasterList.status_short = _shortStatus;
                _MasterList.status_long = _longStatus;
            }
         
            return _MasterList;
        }

        internal async Task<List<CMGOListByFilter>> GetGOList(int facility_id, DateTime fromDate, DateTime toDate, int is_purchaseorder)
        {

            string filter = " (DATE(po.purchaseDate) >= '" + fromDate.ToString("yyyy-MM-dd") + "'  and DATE(po.purchaseDate) <= '" + toDate.ToString("yyyy-MM-dd") + "')";

            filter = filter + " and facilityID = " + facility_id + "";

            if (is_purchaseorder == 0)
            {
                filter = filter + " and (is_purchaseorder = 0 OR is_purchaseorder is null) ";
            }
            else
            {
                filter = filter + " and is_purchaseorder = 1 ";
            } 

            string query = "SELECT fc.name as facilityName,pod.ID as podID, facilityid as       facility_id,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_type_ID," +
                "pod.purchaseID,pod.assetItemID,sai.serial_number,sai.location_ID,(select sum(cost) from smpurchaseorderdetails where purchaseID = po.id) as cost,pod.ordered_qty,\r\n bl.name as vendor_name,\r\n     " +
                " po.purchaseDate,sam.asset_type_ID,sam.asset_name,po.receiverID,\r\n        " +
                "po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty," +
                "f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by,pod.order_type as asset_type_ID_OrderDetails, receive_later, " +
                "added_to_store,   \r\n      " +
                "  po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, po.lr_no, po.condition_pkg_received, " +
                "po.vehicle_no, po.gir_no, po.challan_date, po.job_ref, po.amount,  po.currency as currencyID , curr.name as currency , stt.asset_type as asset_type_Name,  po_no, po_date, requested_qty,lost_qty, ordered_qty, CONCAT(ed.firstName,' ',ed.lastName) as generatedBy\r\n  ,po.received_on as receivedAt    " +
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
                "       LEFT JOIN facilities fc ON fc.id = po.facilityID\r\nLEFT JOIN users as vendor on vendor.id=po.vendorID " +
                "       LEFT JOIN business bl ON bl.id = po.vendorID left join smassettypes stt on stt.ID = pod.order_type LEFT JOIN currency curr ON curr.id = po.currency LEFT JOIN users ed ON ed.id = po.generated_by" +
                " WHERE "+ filter + "";


            List<CMGoodsOrderList> _List = await Context.GetData<CMGoodsOrderList>(query).ConfigureAwait(false);

            List<CMGOListByFilter> _MasterList = _List.Select(p => new CMGOListByFilter
            {
                Id = p.purchaseID,
                facility_id = p.facility_id,
                facilityName = p.facilityName,
                asset_type_ID = p.asset_type_ID,
                vendorID = p.vendorID,
                vendor_name = p.vendor_name,
                status = p.status,
                accepted_qty = p.ordered_qty,
                currencyID = p.currencyID,
                currency = p.currency,
                amount = p.amount,
                job_ref = p.job_ref,
                gir_no = p.gir_no,
                vehicle_no = p.vehicle_no,
                condition_pkg_received = p.condition_pkg_received,
                lr_no = p.lr_no,
                no_pkg_received = p.no_pkg_received,
                receivedAt = p.receivedAt,
                freight = p.freight,
                po_date = p.po_date,
                po_no = p.po_no,
                challan_date = p.challan_date,
                challan_no = p.challan_no,
                purchaseDate = p.purchaseDate,
                location_ID = p.location_ID,
                cost = p.cost,
                generatedBy = p.generatedBy
            }).GroupBy(p => p.Id).Select(group => group.First()).OrderBy(p => p.Id).ToList();
        


                for (var i = 0; i < _MasterList.Count; i++)
                {
                    CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_MasterList[i].status);
                    string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_PO, _Status);
                    string _longStatus = getLongStatus(_Status, _MasterList[i].Id);

                    _MasterList[i].status_short = _shortStatus;
                    _MasterList[i].status_long = _longStatus;
                }
   
            return _MasterList;
        }


        internal async Task<CMDefaultResponse> UpdateGOReceive(CMGoodsOrderList request, int userID)
        {
            string OrderQuery = "";
            if (request.is_submit == 0)
            {
                OrderQuery = $"UPDATE smpurchaseorder SET " +
                    $"status= updated_by = {userID},updatedOn = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.GO_RECEIVE_DRAFT} where ID={request.id}";
            }
            else
            {
                OrderQuery = $"UPDATE smpurchaseorder SET " +
                    $"status= updated_by = {userID},updatedOn = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED} where ID={request.id}";
            }
            await Context.ExecuteNonQry<int>(OrderQuery);

            for (var i = 0; i < request.go_items.Count; i++)
            {
                string itemsQuery = $"UPDATE smpurchaseorderdetails SET location_ID = {request.location_ID},assetItemID = {request.go_items[i].assetItemID},cost = {request.go_items[i].cost}, accepted_qty = {request.go_items[i].accepted_qty},ordered_qty = {request.go_items[i].ordered_qty} , requested_qty = {request.go_items[i].requested_qty}, received_qty= {request.go_items[i].received_qty},lost_qty = {request.go_items[i].lost_qty}, damaged_qty={request.go_items[i].damaged_qty}, paid_by_ID = {request.go_items[i].paid_by_ID}" +
                    $" WHERE ID = {request.go_items[i].goItemID}";
                var result = await Context.ExecuteNonQry<int>(itemsQuery);
            }

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Goods order updated successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> ApproveGoodsOrderReceive(CMApproval request, int userId)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string UpdatesqlQ = $" UPDATE smpurchaseorder SET approved_by = {userId}, status = {(int)CMMS.CMMS_Status.GO_RECEIVED_APPROVED}, remarks = '{request.comment}',approvedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(UpdatesqlQ).ConfigureAwait(false);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;



            // Entry in TransactionDetails
            var data = await this.getPurchaseDetailsByID(request.id);
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].added_to_store == 0)
                {

                    decimal stock_qty = data[i].ordered_qty + data[i].received_qty;
                    var tResult = await TransactionDetails(data[i].facility_id, data[i].vendorID, (int)CMMS.SM_Types.Vendor, data[i].facility_id, (int)CMMS.SM_Types.Store, data[i].assetItemID, (double)stock_qty, (int)CMMS.CMMS_Modules.SM_GO, request.id, "Goods Order");

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
            string myQuery = $"SELECT   ID ,facilityID ,vendorID ,receiverID ,generated_by ,purchaseDate ,orderDate ,challan_no ,po_no ,\r\n     " +
                $" freight ,transport ,no_pkg_received ,lr_no ,\r\n      " +
                $"condition_pkg_received ,vehicle_no ,gir_no ,challan_date ,po_date ,\r\n      " +
                $"job_ref ,amount ,currency as currencyID,status ,\r\n     " +
                $" lastModifiedDate ,generate_flag ,s2s_generated_by ,received_on ,invoice_copy ,issued_by ,issued_on ,\r\n    " +
                $"  approved_by ,approvedOn ,withdraw_by ,withdrawOn ,reorder_flag ,\r\n     " +
                $" order_type ,remarks ,updated_by ,updatedOn ,rejected_by ,rejectedOn" +
                $" \r\nFROM smpurchaseorder where id = {request.id}";
            List<CMGoodsOrderList> _List = await Context.GetData<CMGoodsOrderList>(myQuery).ConfigureAwait(false);
            //CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_PO, CMMS.CMMS_Status.SM_PO_CLOSED_APPROVED, _List[0]);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, request.id, 0, 0, "Goods order receive approved.", CMMS.CMMS_Status.GO_RECEIVED_APPROVED);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Goods order receive {request.id} approved successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectGoodsOrderReceive(CMApproval request, int userId)
        {

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }


            string approveQuery = $"Update smpurchaseorder set status = {(int)CMMS.CMMS_Status.GO_RECEIVED_REJECTED} , remarks = '{request.comment}' , " +
                $" rejected_by = {userId}, rejectedOn = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                $" where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, request.id, 0, 0, "Rejected goods order receive", CMMS.CMMS_Status.GO_RECEIVED_REJECTED);



            string myQuery = $"SELECT   ID ,facilityID ,vendorID ,receiverID ,generated_by ,purchaseDate ,orderDate ,challan_no ,po_no ,\r\n     " +
       $" freight ,transport ,no_pkg_received ,lr_no ,\r\n      " +
       $"condition_pkg_received ,vehicle_no ,gir_no ,challan_date ,po_date ,\r\n      " +
       $"job_ref ,amount ,currency as currencyID,status ,\r\n     " +
       $" lastModifiedDate ,generate_flag ,s2s_generated_by ,received_on ,invoice_copy ,issued_by ,issued_on ,\r\n    " +
       $"  approved_by ,approvedOn ,withdraw_by ,withdrawOn ,reorder_flag ,\r\n     " +
       $" order_type ,remarks ,updated_by ,updatedOn ,rejected_by ,rejectedOn" +
       $" \r\nFROM smpurchaseorder where id = {request.id}";
            List<CMGoodsOrderList> _WCList = await Context.GetData<CMGoodsOrderList>(myQuery).ConfigureAwait(false);
            //CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_PO, CMMS.CMMS_Status.SM_PO_CLOSED_REJECTED, _WCList[0]);


            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Goods order receive {request.id} rejected successfully.");
     

            return response;
        }
    }
}
