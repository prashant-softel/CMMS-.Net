using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CMMSAPIs.BS;
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

namespace CMMSAPIs.Repositories.SM
{
    public class RequestOrderRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public RequestOrderRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<CMDefaultResponse> CreateRequestOrder(CMCreateRequestOrder request, int userID)
        {
            CMDefaultResponse response = null;
            int ReturnID = 0;
            try
            {
                if (request.request_order_items != null)
                {
                    string poInsertQuery = $" INSERT INTO smrequestorder (facilityID,generated_by,request_date,status,remarks)" +
                       $"VALUES({request.facilityID},  {userID}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}', {(int)CMMS.CMMS_Status.SM_RO_SUBMITTED}, '{request.comment}');" +
                        $" SELECT LAST_INSERT_ID();";
                    DataTable dt2 = await Context.FetchData(poInsertQuery).ConfigureAwait(false);
                    int roid = Convert.ToInt32(dt2.Rows[0][0]);
                    ReturnID = roid;
                    for (var i = 0; i < request.request_order_items.Count; i++)
                    {
                        string poDetailsQuery = $"INSERT INTO smrequestorderdetails (requestID,assetItemID,cost,ordered_qty,remarks) " +
                        "values(" + roid + ", " + request.request_order_items[i].assetMasterItemID + ",  " + request.request_order_items[i].cost + ", " + request.request_order_items[i].ordered_qty + ", '" + request.request_order_items[i].comment + "') ; SELECT LAST_INSERT_ID();";
                        DataTable dtInsertPO = await Context.FetchData(poDetailsQuery).ConfigureAwait(false);
                        int id = Convert.ToInt32(dtInsertPO.Rows[0][0]);
                    }
                }
            }
            catch (Exception ex)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, ex.Message);
            }

            response = new CMDefaultResponse(ReturnID, CMMS.RETRUNSTATUS.SUCCESS, "Request order created successfully.");
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, ReturnID, 0, 0, "Request order created", CMMS.CMMS_Status.SM_RO_SUBMITTED);
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateRequestOrder(CMCreateRequestOrder request, int userID)
        {
            //string mainQuery = $"UPDATE smpurchaseorderdetails SET generate_flag = " +request.generate_flag + ",status = "+request.status+", vendorID = "+request.vendorID+" WHERE ID = "+request.id+"";

            string updateRO = $" UPDATE smrequestorder SET facilityID = '{request.facilityID}'," +
                $" remarks = '{request.comment}', updated_by = {userID},updated_at='{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}' where id = {request.request_order_id}";
            var ResultROQuery = await Context.ExecuteNonQry<int>(updateRO);
            for (var i = 0; i < request.request_order_items.Count; i++)
            {
                if (request.request_order_items[i].itemID > 0)
                {
                    string updateQ = $"UPDATE smrequestorderdetails SET assetItemID = {request.request_order_items[i].assetMasterItemID} , cost = {request.request_order_items[i].cost} , ordered_qty = {request.request_order_items[i].ordered_qty}, remarks = '{request.request_order_items[i].comment}'" +
                        $" WHERE ID = {request.request_order_items[i].itemID}";
                    var result = await Context.ExecuteNonQry<int>(updateQ);
                }
                else
                {
                    string poDetailsQuery = $"INSERT INTO smrequestorderdetails (requestID,assetItemID,cost,ordered_qty,remarks) " +
                       "values(" + request.request_order_id + ", " + request.request_order_items[i].assetMasterItemID + ",  " + request.request_order_items[i].cost + ", " + request.request_order_items[i].ordered_qty + ", '" + request.request_order_items[i].comment + "') ; SELECT LAST_INSERT_ID();";
                    DataTable dtInsertPO = await Context.FetchData(poDetailsQuery).ConfigureAwait(false);
                    int id = Convert.ToInt32(dtInsertPO.Rows[0][0]);
                }
            }
            CMDefaultResponse response = new CMDefaultResponse(request.request_order_id, CMMS.RETRUNSTATUS.SUCCESS, "Request order updated successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteRequestOrder(CMApproval request, int userID)
        {
            string mainQuery = $"UPDATE smrequestorder SET status = {(int)CMMS.CMMS_Status.SM_RO_DELETED}, remarks= '{request.comment}' WHERE ID = " + request.id + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Request order deleted.");
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, request.id, 0, 0, "Request order deleted", CMMS.CMMS_Status.SM_RO_DELETED);
            return response;
        }
        internal async Task<CMDefaultResponse> CloseRequestOrder(CMApproval request, int userID)
        {
            //validate request.id
            string mainQuery = $"UPDATE smrequestorder SET status = {(int)CMMS.CMMS_Status.SM_RO_CLOSED}, remarks= '{request.comment}' WHERE ID = " + request.id + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Request order {request.id} closed.");
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, request.id, 0, 0, "Request order {request.id} closed", CMMS.CMMS_Status.SM_RO_CLOSED);
            return response;
        }

        internal async Task<List<CMCreateRequestOrder>> GetRequestOrderList(int facilityID, DateTime fromDate, DateTime toDate) 
        {
            string filter = " facilityID = " + facilityID + " and  DATE_FORMAT(po.request_date, '%Y-%m-%d') >= '" + fromDate.ToString("yyyy-MM-dd") + "'  and  DATE_FORMAT(po.request_date, '%Y-%m-%d') <= '" + toDate.ToString("yyyy-MM-dd") + "'";

            string query = "SELECT fc.name as facilityName,pod.ID as requestDetailsID, facilityid as      " +
                           " facility_id,pod.spare_status,po.remarks,sai.orderflag," +
                           " sam.asset_type_ID,pod.requestID,pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty," +
                           " po.request_date,sam.asset_type_ID,sam.asset_name,po.receiverID," +
                           " po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty, " +
                           " f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by, " +
                           " pod.order_type as asset_type_ID_OrderDetails, receive_later, added_to_store,reject_reccomendations as  rejectedRemark, " +
                           " po.amount,  po.currency as currencyID , curr.name as currency ,  \r\n    CONCAT(ed.firstName,' ',ed.lastName) as generatedBy, " +
                           " po.received_on as generatedAt,approvedOn as approvedAt,CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy, " +
                           " pod.remarks as itemcomment, CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy , " +
                           " CONCAT(ed3.firstName,' ',ed3.lastName) as rejectedBy , po.rejected_at as rejectedAt " +
                           " FROM smrequestorderdetails pod" +
                           " LEFT JOIN smrequestorder po ON po.ID = pod.requestID" +
                           " LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
                           " LEFT JOIN smassetmasters sam ON sam.id = pod.assetItemID" +
                           " LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement" +
                           " LEFT JOIN ( SELECT file.file_path,file.Asset_master_id as Asset_master_id FROM smassetmasterfiles file" +
                           "            LEFT join smassetmasters sam on file.Asset_master_id =  sam.id )" +
                           "   f1 ON f1.Asset_master_id = sam.id" +
                           " LEFT JOIN (" +
                           "            SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat" +
                           "            LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID" +
                           "        )  t1 ON t1.master_ID = sam.ID" +
                           " LEFT JOIN (" +
                           "            SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic" +
                           "            LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID " +
                           "        )  t2 ON t2.master_ID = sam.ID" +
                           "        LEFT JOIN facilities fc ON fc.id = po.facilityID" +
                           " LEFT JOIN currency curr ON curr.id = po.currency  LEFT JOIN users ed2 ON ed2.id = po.approved_by " +
                           " LEFT JOIN users ed ON ed.id = po.generated_by" +
                           " LEFT JOIN users ed1 ON ed1.id = po.receiverID     LEFT JOIN users ed3 ON ed3.id = po.rejeccted_by" +
                           "  WHERE " + filter;
          
            List<CMRequestOrderList> _List = await Context.GetData<CMRequestOrderList>(query).ConfigureAwait(false);

            List<CMCreateRequestOrder> _MasterList = _List.Select(p => new CMCreateRequestOrder
            {
                request_order_id = p.requestID,
                facilityID = p.facility_id,
                facilityName = p.facilityName,
                status = p.status,
                comment = p.remarks,
                rejectedRemark = p.rejectedRemark,
                approvedAt = p.approvedAt,
                generatedBy = p.generatedBy,
                generatedAt = p.request_date,
                approvedBy = p.approvedBy,
                rejectedBy = p.rejectedBy,
                rejectedAt = p.rejectedAt
            }).GroupBy(p => p.request_order_id).Select(group => group.First()).ToList();
            for (var i = 0; i < _MasterList.Count; i++)
            {
                List<CMRequestOrder_ITEMS> _itemList = _List.Select(p => new CMRequestOrder_ITEMS
                {
                    itemID = p.requestDetailsID,
                    requestID = p.requestID,
                    cost = p.cost,
                    asset_name = p.asset_name,
                    assetMasterItemID = p.assetItemID,
                    ordered_qty = p.ordered_qty,
                    comment = p.itemcomment
                }).ToList();
                _MasterList[i].number_of_masters = _itemList.Where(group => group.requestID == _MasterList[i].request_order_id).Count();
                _MasterList[i].number_of_item_count = (int)_itemList.Where(group => group.requestID == _MasterList[i].request_order_id).Sum(x => x.ordered_qty);
                _MasterList[i].cost = _itemList.Where(group => group.requestID == _MasterList[i].request_order_id).Sum(x => x.cost);
                _MasterList[i].request_order_items = _itemList.Where(group => group.requestID == _MasterList[i].request_order_id).ToList();
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_MasterList[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_RO, _Status);
                string _longStatus = getLongStatus(_MasterList[i].request_order_id, _Status);

                _MasterList[i].status_short = _shortStatus;
                _MasterList[i].status_long = _longStatus;
            }
          
            return _MasterList;
        }
        //internal async Task<List<CMRequestOrder>> GetRequestOrderList(int facilityID, DateTime fromDate, DateTime toDate)
        //{

        //    string filter = " facilityID = " + facilityID + " and  DATE_FORMAT(po.request_date, '%Y-%m-%d') >= '" + fromDate.ToString("yyyy-MM-dd") + "'  and  DATE_FORMAT(po.request_date, '%Y-%m-%d') <= '" + toDate.ToString("yyyy-MM-dd") + "'";

        //    //string stmt = "SELECT fc.name as facilityName,  pod.ID as requestID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_name,sam.asset_type_ID,pod.requestID as ID," +
        //    //    "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost ,pod.ordered_qty,po.remarks as rejectedRemark,po.facilityID,po.request_date," +
        //    //    "sam.asset_type_ID,\r\n\t\t        po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty," +
        //    //    "pod.accepted_qty,po.received_on as generatedAt,po.approvedOn as approvedAt,\r\n\t\t\t\tCONCAT(ed.firstName,' ',ed.lastName) as generatedBy," +
        //    //    "CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy,CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy," +
        //    //    "\r\n\t\t\t\tbl.name as vendor_name, po.currency  as currencyID,curr.name currency, amount, job_ref, gir_no,vehicle_no, condition_pkg_received, lr_no, no_pkg_received, received_on, freight,  challan_date, challan_no" +
        //    //    " \r\n\t\t        FROM smrequestorderdetails pod\r\n\t\t       " +
        //    //    " LEFT JOIN smrequestorder po ON po.ID = pod.requestID\r\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
        //    //    " LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code   " +
        //    //    " LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\r\n\t\t            " +
        //    //    "LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\r\n\t\t        )  t1 ON t1.master_ID = sam.ID\r\n\t\t        " +
        //    //    "LEFT JOIN (\r\n\t\t      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic \r\n            " +
        //    //    "  LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID " +
        //    //    "LEFT JOIN users ed ON ed.id = po.generated_by\r\n\t\t   " +
        //    //    " Left join currency curr ON curr.id = po.currency " +
        //    //    "     LEFT JOIN users ed1 ON ed1.id = po.receiverID\r\n\t\t\t\t " +
        //    //    "LEFT JOIN users ed2 ON ed2.id = po.approved_by\r\n\t\t\t\t" +
        //    //    "LEFT JOIN business bl ON bl.id = po.vendorID \r\n\r\n\t\t LEFT JOIN facilities fc ON fc.id = po.facilityID    WHERE " + filter +
        //    //    " ";

        //    string stmt = "SELECT fc.name as facilityName,  pod.ID as requestID,pod.remarks,sam.asset_name,sam.asset_type_ID,pod.requestID as ID,pod.assetItemID," +
        //        "pod.cost ,pod.ordered_qty,po.remarks as rejectedRemark,po.facilityID,po.request_date,sam.asset_type_ID," +
        //        " po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty,po.received_on as generatedAt,po.approvedOn as approvedAt," +
        //        " CONCAT(ed.firstName,' ',ed.lastName) as generatedBy,CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy,CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy" +
        //        "  FROM smrequestorderdetails pod" +
        //        " LEFT JOIN smrequestorder po ON po.ID = pod.requestID " +
        //        " LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code   " +
        //        " LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat" +
        //        "            LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID" +
        //        "        )  t1 ON t1.master_ID = sam.ID" +
        //        "        LEFT JOIN (" +
        //        "      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic " +
        //        "              LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID LEFT JOIN users ed ON ed.id = po.generated_by" +
        //        "         LEFT JOIN users ed1 ON ed1.id = po.receiverID " +
        //        " LEFT JOIN users ed2 ON ed2.id = po.approved_by" +
        //        " LEFT JOIN facilities fc ON fc.id = po.facilityID    WHERE "+ filter;
        //    List<CMRequestOrder> _List = await Context.GetData<CMRequestOrder>(stmt).ConfigureAwait(false);


        //    for (var i = 0; i < _List.Count; i++)
        //    {
        //        CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
        //        string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_GO, _Status);
        //        string status_long = getLongStatus(_List[i].requestID, _Status);
        //        _List[i].status_short = _shortStatus;
        //        _List[i].status_long = status_long;
        //    }


        //    return _List;
        //}


        internal async Task<CMDefaultResponse> ApproveRequestOrder(CMApproval request, int userId)
        {

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string UpdatesqlQ = $" UPDATE smrequestorder SET approved_by = {userId}, status = {(int)CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED}, remarks = '{request.comment}',approvedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(UpdatesqlQ).ConfigureAwait(false);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Approved request order {request.id} successfully.");
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, request.id, 0, 0, "Approved request order {request.id}", CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED);
            return response;
        }

        internal async Task<CMDefaultResponse> RejectRequestOrder(CMApproval request, int userId)
        {

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string approveQuery = $"Update smrequestorder set status = {(int)CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED} , reject_reccomendations = '{request.comment}',  " +
                $" rejeccted_by = {userId}, rejected_at = '{DateTime.Now.ToString("yyyy-MM-dd")}'" +
                $" where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, request.id, 0, 0, "Rejected request order", CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Rejected request order.");
            return response;
        }

        public async Task<CMCreateRequestOrder> GetRODetailsByID(int id)
        {
 


            string query = "SELECT fc.name as facilityName,pod.ID as requestDetailsID, facilityid as      " +
                " facility_id,pod.spare_status,po.remarks,sai.orderflag," +
                " sam.asset_type_ID,pod.requestID,pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty," +
                " po.request_date,sam.asset_type_ID,sam.asset_name,po.receiverID," +
                " po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty, " +
                " f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by, " +
                " pod.order_type as asset_type_ID_OrderDetails, receive_later, added_to_store,reject_reccomendations as  rejectedRemark, " +
                " po.amount,  po.currency as currencyID , curr.name as currency ,  \r\n    CONCAT(ed.firstName,' ',ed.lastName) as generatedBy, " +
                " po.received_on as generatedAt,approvedOn as approvedAt,CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy, " +
                " pod.remarks as itemcomment, CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy , " +
                " CONCAT(ed3.firstName,' ',ed3.lastName) as rejectedBy , po.rejected_at as rejectedAt " +
                " FROM smrequestorderdetails pod" +
                " LEFT JOIN smrequestorder po ON po.ID = pod.requestID" +
                " LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
                " LEFT JOIN smassetmasters sam ON sam.id = pod.assetItemID" +
                " LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement" +
                " LEFT JOIN ( SELECT file.file_path,file.Asset_master_id as Asset_master_id FROM smassetmasterfiles file" +
                "            LEFT join smassetmasters sam on file.Asset_master_id =  sam.id )" +
                "   f1 ON f1.Asset_master_id = sam.id" +
                " LEFT JOIN (" +
                "            SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat" +
                "            LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID" +
                "        )  t1 ON t1.master_ID = sam.ID" +
                " LEFT JOIN (" +
                "            SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic" +
                "            LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID " +
                "        )  t2 ON t2.master_ID = sam.ID" +
                "        LEFT JOIN facilities fc ON fc.id = po.facilityID" +
                " LEFT JOIN currency curr ON curr.id = po.currency  LEFT JOIN users ed2 ON ed2.id = po.approved_by " +
                " LEFT JOIN users ed ON ed.id = po.generated_by" +
                " LEFT JOIN users ed1 ON ed1.id = po.receiverID     LEFT JOIN users ed3 ON ed3.id = po.rejeccted_by" +
                "  WHERE po.ID = "+id+" ;";
            List<CMRequestOrderList> _List = await Context.GetData<CMRequestOrderList>(query).ConfigureAwait(false);

            CMCreateRequestOrder _MasterList = _List.Select(p => new CMCreateRequestOrder
            {
                request_order_id = p.requestID,
                facilityID = p.facility_id,
                facilityName = p.facilityName,                
                status = p.status,                
                comment = p.remarks,
                rejectedRemark = p.rejectedRemark,
                approvedAt = p.approvedAt,
                generatedBy = p.generatedBy,
            
                generatedAt = p.request_date,
                approvedBy = p.approvedBy,
                rejectedBy = p.rejectedBy,
                rejectedAt = p.rejectedAt
            }).FirstOrDefault();
            List<CMRequestOrder_ITEMS> _itemList = _List.Select(p => new CMRequestOrder_ITEMS
            {
                itemID = p.requestDetailsID,
                requestID = p.requestID,
                cost = p.cost,
                asset_name = p.asset_name,
                assetMasterItemID = p.assetItemID,
                ordered_qty = p.ordered_qty,
                comment = p.itemcomment
            }).ToList();
            _MasterList.cost = _itemList.Sum(x=> x.cost);
            _MasterList.request_order_items = _itemList;
            _MasterList.number_of_item_count = (int)_itemList.Sum(x => x.ordered_qty);
            _MasterList.number_of_masters = _itemList.Count;


            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_MasterList.status);
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_RO, _Status);
            string _longStatus = getLongStatus(_MasterList.request_order_id, _Status);

            _MasterList.status_short = _shortStatus;
            _MasterList.status_long = _longStatus;
            return _MasterList;
        }

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.SM_RO_SUBMITTED:
                    retValue = "Waiting for approval";
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED:
                    retValue = "Submit rejected";
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED:
                    retValue = "Submit approved";
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED:
                    retValue = "Closed";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
                case CMMS.CMMS_Status.SM_RO_DELETED:
                    retValue = "Deleted";
                    break;
            }
            return retValue;

        }

        internal static string getLongStatus(int ID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.SM_RO_SUBMITTED:
                    retValue = $"Request order {ID} submitted and waiting for approval";
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED:
                    retValue = $"Request order {ID} Submitted but rejected";
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED:
                    retValue = $"Request order {ID} approved";
                    break;
                case CMMS.CMMS_Status.SM_RO_DELETED:
                    retValue = $"Request order {ID} deleted";
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED:
                    retValue = $"Request order {ID} closed and waiting for approval";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }
    }
}
