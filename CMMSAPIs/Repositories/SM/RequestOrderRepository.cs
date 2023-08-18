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

        internal async Task<CMDefaultResponse> CreateRequestOrder(CMRequestOrder request, int userID)
        {
            CMDefaultResponse response = null;
            int ReturnID = 0;
            try
            {
                if (request.go_items != null)
                {
                    string poInsertQuery = $" INSERT INTO smrequestorder (facilityID,vendorID,receiverID,generated_by,request_date,status," +
                        $" challan_no, freight,transport, request_no, " +
                        $"no_pkg_received,lr_no,condition_pkg_received,vehicle_no, gir_no, challan_date, job_ref,amount, currency,withdraw_by,withdrawOn,order_type) " +
                        $"VALUES({request.facilityID},{request.vendorID}, {request.receiverID}, {userID}, '{DateTime.Now.ToString("yyyy-MM-dd")}', {(int)CMMS.CMMS_Status.SM_RO_SUBMITTED}," +
                        $"'{request.challan_no}','{request.freight}','','', '{request.no_pkg_received}', '{request.lr_no}', '{request.condition_pkg_received}','{request.vehicle_no}','{request.gir_no}','{request.challan_date.Value.ToString("yyyy-MM-dd")}'," +
                        $"'{request.job_ref}',{request.amount}, {request.currencyID},0,'0001-01-01',0);" +
                        $" SELECT LAST_INSERT_ID();";
                    DataTable dt2 = await Context.FetchData(poInsertQuery).ConfigureAwait(false);
                    int roid = Convert.ToInt32(dt2.Rows[0][0]);
                    ReturnID = roid;
                    for (var i = 0; i < request.go_items.Count; i++)
                    {
                        string poDetailsQuery = $"INSERT INTO smrequestorderdetails (requestID,assetItemID,cost,ordered_qty) " +
                        "values(" + roid + ", " + request.go_items[i].assetItemID + ",  " + request.go_items[i].cost + ", " + request.go_items[i].ordered_qty + ") ; SELECT LAST_INSERT_ID();";
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
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, ReturnID, 0, 0, "Request order created", CMMS.CMMS_Status.SM_RO_SUBMITTED);
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateRequestOrder(CMRequestOrder request, int userID)
        {
            //string mainQuery = $"UPDATE smpurchaseorderdetails SET generate_flag = " +request.generate_flag + ",status = "+request.status+", vendorID = "+request.vendorID+" WHERE ID = "+request.id+"";

            string updateRO = $" UPDATE smrequestorder SET vendorID = {request.vendorID},request_date = '{request.request_date.Value.ToString("yyyy-MM-dd")}', challan_no = '{request.challan_no}'," +
                $"challan_date = '{request.challan_date.Value.ToString("yyyy-MM-dd")}', freight='{request.freight}', received_on='{DateTime.Now.ToString("yyyy-MM-dd")}', no_pkg_received='{request.no_pkg_received}'," +
                $" lr_no='{request.lr_no}', condition_pkg_received='{request.condition_pkg_received}', vehicle_no = '{request.vehicle_no}', gir_no = '{request.gir_no}', " +
                $"job_ref = '{request.job_ref}', amount = '{request.amount}', currency= {request.currencyID} where id = {request.id}";
            var ResultROQuery = await Context.ExecuteNonQry<int>(updateRO);
            for (var i = 0; i < request.go_items.Count; i++)
            {

                string updateQ = $"UPDATE smrequestorderdetails SET assetItemID = {request.go_items[i].assetItemID} , cost = {request.go_items[i].cost} , ordered_qty = {request.go_items[i].ordered_qty}" +
                    $" WHERE ID = {request.go_items[i].requestID}";
                var result = await Context.ExecuteNonQry<int>(updateQ);
            }
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Request order updated successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteRequestOrder(CMApproval request, int userID)
        {
            string mainQuery = $"UPDATE smrequestorder SET status = {(int)CMMS.CMMS_Status.SM_RO_DELETED}, remarks= '{request.comment}' WHERE ID = " + request.id + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Request order deleted.");
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, request.id, 0, 0, "Request order deleted", CMMS.CMMS_Status.SM_RO_DELETED);
            return response;
        }

        internal async Task<List<CMRequestOrder>> GetRequestOrderList(int facilityID, DateTime fromDate, DateTime toDate)
        {

            string filter = " facilityID = " + facilityID + " and (DATE(po.request_date) >= '" + fromDate.ToString("yyyy-MM-dd") + "'  and DATE(po.request_date) <= '" + toDate.ToString("yyyy-MM-dd") + "')";

            string stmt = "SELECT fc.name as facilityName,  pod.ID as requestID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_name,sam.asset_type_ID,pod.requestID as ID," +
                "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost ,pod.ordered_qty,po.remarks as rejectedRemark,po.facilityID,po.request_date," +
                "sam.asset_type_ID,\r\n\t\t        po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty," +
                "pod.accepted_qty,po.received_on as receivedAt,po.approvedOn as approvedAt,\r\n\t\t\t\tCONCAT(ed.firstName,' ',ed.lastName) as generatedBy," +
                "CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy,CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy," +
                "\r\n\t\t\t\tbl.name as vendor_name, po.currency  as currencyID,curr.name currency, amount, job_ref, gir_no,vehicle_no, condition_pkg_received, lr_no, no_pkg_received, received_on, freight,  challan_date, challan_no" +
                " \r\n\t\t        FROM smrequestorderdetails pod\r\n\t\t       " +
                " LEFT JOIN smrequestorder po ON po.ID = pod.requestID\r\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
                " LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code   " +
                " LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\r\n\t\t            " +
                "LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\r\n\t\t        )  t1 ON t1.master_ID = sam.ID\r\n\t\t        " +
                "LEFT JOIN (\r\n\t\t      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic \r\n            " +
                "  LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID " +
                "LEFT JOIN users ed ON ed.id = po.generated_by\r\n\t\t   " +
                " Left join currency curr ON curr.id = po.currency " +
                "     LEFT JOIN users ed1 ON ed1.id = po.receiverID\r\n\t\t\t\t " +
                "LEFT JOIN users ed2 ON ed2.id = po.approved_by\r\n\t\t\t\t" +
                "LEFT JOIN business bl ON bl.id = po.vendorID \r\n\r\n\t\t LEFT JOIN facilities fc ON fc.id = po.facilityID    WHERE " + filter +
                " ";
            List<CMRequestOrder> _List = await Context.GetData<CMRequestOrder>(stmt).ConfigureAwait(false);


            for (var i = 0; i < _List.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_PO, _Status);
                string status_long = getLongStatus(_List[i].requestID, _Status);
                _List[i].status_short = _shortStatus;
                _List[i].status_long = status_long;
            }


            return _List;
        }


        internal async Task<CMDefaultResponse> ApproveRequestOrder(CMApproval request, int userId)
        {

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string UpdatesqlQ = $" UPDATE smrequestorder SET approved_by = {userId}, status = {(int)CMMS.CMMS_Status.SM_RO_CLOSED_APPROVED}, remarks = '{request.comment}',approvedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(UpdatesqlQ).ConfigureAwait(false);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Approved request order successfully.");
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, request.id, 0, 0, "Approved request order", CMMS.CMMS_Status.SM_RO_CLOSED_APPROVED);
            return response;
        }

        internal async Task<CMDefaultResponse> RejectRequestOrder(CMApproval request, int userId)
        {

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string approveQuery = $"Update smrequestorder set status = {(int)CMMS.CMMS_Status.SM_RO_CLOSED_REJECTED} , reject_reccomendations = '{request.comment}',  " +
                $" rejecctedBy = {userId}, rejectedAt = '{DateTime.Now.ToString("yyyy-MM-dd")}'" +
                $" where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_PO, request.id, 0, 0, "Rejected request order", CMMS.CMMS_Status.SM_PO_CLOSED_REJECTED);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Rejected request order.");
            return response;
        }

        public async Task<CMRequestOrder> GetRODetailsByID(int id)
        {
            string query = "SELECT fc.name as facilityName,pod.ID as requestDetailsID, facilityid as      " +
                " facility_id,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_type_ID,pod.requestID,pod.assetItemID," +
                "sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,\r\n bl.name as vendor_name,\r\n  " +
                "    po.request_date,sam.asset_type_ID,sam.asset_name,po.receiverID,\r\n        po.vendorID,po.status," +
                "sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty,f1.file_path," +
                "f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by,pod.order_type as asset_type_ID_OrderDetails," +
                " receive_later, added_to_store,reject_reccomendations as  rejectedRemark, \r\n        po.challan_no, po.freight, po.transport, po.no_pkg_received, po.lr_no," +
                " po.condition_pkg_received, po.vehicle_no, po.gir_no, po.challan_date, po.job_ref, po.amount,  po.currency as currencyID" +
                " , curr.name as currency , stt.asset_type as asset_type_Name, \r\n    CONCAT(ed.firstName,' ',ed.lastName) as generatedBy,po.received_on as receivedAt,approvedOn as approvedAt,CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy," +
                "CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy    FROM smrequestorderdetails pod\r\n      " +
                "  LEFT JOIN smrequestorder po ON po.ID = pod.requestID\r\n       " +
                " LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID\r\n     " +
                "   LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code\r\n      " +
                "  LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement\r\n     " +
                "   LEFT JOIN (\r\n            SELECT file.file_path,file.Asset_master_id as Asset_master_id " +
                "FROM smassetmasterfiles file \r\n           " +
                " LEFT join smassetmasters sam on file.Asset_master_id =  sam.id )\r\n          " +
                "  f1 ON f1.Asset_master_id = sam.id\r\n        " +
                "LEFT JOIN (\r\n            SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\r\n      " +
                "      LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\r\n        )  t1 ON t1.master_ID = sam.ID\r\n " +
                "       LEFT JOIN (\r\n            SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic\r\n   " +
                "         LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID\r\n        )  t2 ON t2.master_ID = sam.ID\r\n       " +
                " LEFT JOIN facilities fc ON fc.id = po.facilityID\r\nLEFT JOIN users as vendor on vendor.id=po.vendorID       \r\n " +
                "LEFT JOIN business bl ON bl.id = po.vendorID left join smassettypes stt on stt.ID = pod.order_type\r\n " +
                "LEFT JOIN currency curr ON curr.id = po.currency  LEFT JOIN users ed2 ON ed2.id = po.approved_by\r\n LEFT JOIN users ed ON ed.id = po.generated_by\r\n  LEFT JOIN users ed1 ON ed1.id = po.receiverID " +
                "WHERE po.ID = " + id + " ;";
            List<CMRequestOrderList> _List = await Context.GetData<CMRequestOrderList>(query).ConfigureAwait(false);

            CMRequestOrder _MasterList = _List.Select(p => new CMRequestOrder
            {
                requestID = p.requestID,
                facilityID = p.facility_id,
                facilityName = p.facilityName,
                assetItemID = p.assetItemID,
                vendorID = p.vendorID,
                vendor_name = p.vendor_name,
                status = p.status,
                currencyID = p.currencyID,
                currency = p.currency,
                amount = p.amount,
                job_ref = p.job_ref,
                gir_no = p.gir_no,
                vehicle_no = p.vehicle_no,
                condition_pkg_received = p.condition_pkg_received,
                lr_no = p.lr_no,
                no_pkg_received = p.no_pkg_received,
                freight = p.freight,
                challan_date = p.challan_date,
                challan_no = p.challan_no,
                request_date = p.request_date,
                location_ID = p.location_ID,
                remarks = p.remarks,
                rejectedRemark = p.rejectedRemark,
                approvedAt = p.approvedAt,
                generatedBy = p.generatedBy,
                receivedBy = p.receivedBy,
                receivedAt = p.receivedAt
            }).FirstOrDefault();
            List<CMRequestOrder_ITEMS> _itemList = _List.Select(p => new CMRequestOrder_ITEMS
            {
                ID = p.requestDetailsID,
                requestID = p.requestID,
                cost = p.cost,
                asset_name = p.asset_name,
                assetItemID = p.assetItemID,
                ordered_qty = p.ordered_qty,
            }).ToList();
            _MasterList.go_items = _itemList;

            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_MasterList.status);
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_PO, _Status);
            string _longStatus = getLongStatus(_MasterList.requestID, _Status);

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
                    retValue = "Submitted";
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED:
                    retValue = "Closed";
                    break;
                case CMMS.CMMS_Status.SM_RO_DELETED:
                    retValue = "Deleted";
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED_REJECTED:
                    retValue = "After closed item rejected";
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED_APPROVED:
                    retValue = "After closed item approved";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
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
                    retValue = $"Request order {ID} submitted";
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED:
                    retValue = $"Request order {ID} closed";
                    break;
                case CMMS.CMMS_Status.SM_RO_DELETED:
                    retValue = $"Request order {ID} deleted";
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED_REJECTED:
                    retValue = $"Request order {ID} rejected";
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED_APPROVED:
                    retValue = $"Request order {ID} approved";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }
    }
}
