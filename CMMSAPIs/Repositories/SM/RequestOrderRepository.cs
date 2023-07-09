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
            try
            {
                if (request.go_items != null)
                {
                    string poInsertQuery = $" INSERT INTO smrequestorder (plantID,vendorID,receiverID,generated_by,request_date,status," +
                        $" challan_no, freight,transport, request_no, " +
                        $"no_pkg_received,lr_no,condition_pkg_received,vehicle_no, gir_no, challan_date, job_ref,amount, currency,withdraw_by,withdrawOn,order_type) " +
                        $"VALUES({request.plantID},{request.vendorID}, {request.receiverID}, {userID}, '{DateTime.Now.ToString("yyyy-MM-dd")}', {(int)CMMS.CMMS_Status.SM_RO_SUBMITTED}," +
                        $"'{request.challan_no}','{request.freight}','','', '{request.no_pkg_received}', '{request.lr_no}', '{request.condition_pkg_received}','{request.vehicle_no}','{request.gir_no}','{request.challan_date.Value.ToString("yyyy-MM-dd")}'," +
                        $"'{request.job_ref}',{request.amount}, '{request.currency}',0,'0001-01-01',0);" +
                        $" SELECT LAST_INSERT_ID();";
                    DataTable dt2 = await Context.FetchData(poInsertQuery).ConfigureAwait(false);
                    int roid = Convert.ToInt32(dt2.Rows[0][0]);

                    for (var i = 0; i < request.go_items.Count; i++)
                    {
                        string poDetailsQuery = $"INSERT INTO smrequestorderdetails (requestID,assetItemID,order_type,cost,ordered_qty,location_ID) " +
                        "values(" + roid + ", " + request.go_items[i].assetItemID + ", " + request.go_items[i].asset_type_ID + ", " + request.go_items[i].cost + ", " + request.go_items[i].ordered_qty + ", " + request.location_ID + ") ; SELECT LAST_INSERT_ID();";
                        DataTable dtInsertPO = await Context.FetchData(poDetailsQuery).ConfigureAwait(false);
                        int id = Convert.ToInt32(dtInsertPO.Rows[0][0]);
                    }
                }
            }catch(Exception ex)
            {
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.FAILURE, ex.Message);
            }

            response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Request order created successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateRO(CMRequestOrder request, int userID)
        {
            //string mainQuery = $"UPDATE smpurchaseorderdetails SET generate_flag = " +request.generate_flag + ",status = "+request.status+", vendorID = "+request.vendorID+" WHERE ID = "+request.id+"";

            string updateRO = $" UPDATE smrequestorder SET vendorID = {request.vendorID},request_date = '{request.request_date.Value.ToString("yyyy-MM-dd")}', challan_no = '{request.challan_no}'," +
                $"challan_date = '{request.challan_date.Value.ToString("yyyy-MM-dd")}', freight='{request.freight}', received_on='{request.received_on.Value.ToString("yyyy-MM-dd")}', no_pkg_received='{request.no_pkg_received}'," +
                $" lr_no='{request.lr_no}', condition_pkg_received='{request.condition_pkg_received}', vehicle_no = '{request.vehicle_no}', gir_no = '{request.gir_no}', " +
                $"job_ref = '{request.job_ref}', amount = '{request.amount}', currency= '{request.currency}' where id = {request.id}";
            var ResultROQuery = await Context.ExecuteNonQry<int>(updateRO);
            for (var i = 0; i < request.go_items.Count; i++)
            {

                string updateQ = $"UPDATE smrequestorderdetails SET assetItemID = {request.go_items[i].assetItemID} , cost = {request.go_items[i].cost} , ordered_qty = {request.go_items[i].ordered_qty}" +
                    $" WHERE ID = {request.go_items[i].requestID}";
                var result = await Context.ExecuteNonQry<int>(updateQ);
            }

            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Request order updated successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteRequestOrder(int RO_ID, int userID)
        {
            string mainQuery = $"UPDATE smrequestorder SET status = 0 WHERE ID = " + RO_ID + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Request order deleted.");
            return response;
        }

        internal async Task<List<CMRequestOrder>> GetRequestOrderList(int plantID, DateTime fromDate, DateTime toDate)
        {

            string filter = " (DATE(po.request_date) >= '" + fromDate.ToString("yyyy-MM-dd") + "'  and DATE(po.request_date) <= '" + toDate.ToString("yyyy-MM-dd") + "')";

            string stmt = "SELECT pod.ID as podID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_name,sam.asset_type_ID,pod.requestID," +
                "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost ,pod.ordered_qty,po.remarks as rejectedRemark,po.plantID,po.request_date," +
                "sam.asset_type_ID,\r\n\t\t        po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty," +
                "pod.accepted_qty,po.received_on,po.approvedOn,\r\n\t\t\t\tCONCAT(ed.firstName,' ',ed.lastName) as generatedBy," +
                "CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy,CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy," +
                "\r\n\t\t\t\tbl.name as vendor_name\r\n\t\t        FROM smrequestorderdetails pod\r\n\t\t       " +
                " LEFT JOIN smrequestorder po ON po.ID = pod.requestID\r\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
                " LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code   " +
                " LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\r\n\t\t            " +
                "LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\r\n\t\t        )  t1 ON t1.master_ID = sam.ID\r\n\t\t        " +
                "LEFT JOIN (\r\n\t\t      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic \r\n            " +
                "  LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID " +
                "LEFT JOIN employees ed ON ed.ID = po.generated_by\r\n\t\t        LEFT JOIN employees ed1 ON ed1.ID = po.receiverID\r\n\t\t\t\t " +
                "LEFT JOIN employees ed2 ON ed2.ID = po.approved_by\r\n\t\t\t\t" +
                "LEFT JOIN business bl ON bl.id = po.vendorID \r\n\r\n\t\t    WHERE " + filter +
                " ";
            List<CMRequestOrder> _List = await Context.GetData<CMRequestOrder>(stmt).ConfigureAwait(false);

            
            for (var i = 0; i < _List.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_PO, _Status);
                _List[i].status_short = _shortStatus;
            }


            return _List;
        }

        internal async Task<CMDefaultResponse> ApproveRequestOrder(CMApproval request)
        {
 
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }
            int userId = Utils.UtilsRepository.GetUserID();
            string UpdatesqlQ = $" UPDATE smrequestorder SET approved_by = {userId}, status = {(int)CMMS.CMMS_Status.SM_RO_CLOSED_APPROVED}, remarks = '{request.comment}',approvedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(UpdatesqlQ).ConfigureAwait(false);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;


            string myQuery = $"SELECT * from smrequestorder where id = {request.id}";
            List<CMRequestOrder> _List = await Context.GetData<CMRequestOrder>(myQuery).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Approved request order successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectGoodsOrder(CMApproval request)
        {

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            int userId = Utils.UtilsRepository.GetUserID();
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


            string myQuery = $"SELECT * from smrequestorder where id = {request.id}";
            List<CMGoodsOrderList> _WCList = await Context.GetData<CMGoodsOrderList>(myQuery).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Rejected request order.");
            return response;
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
    }
}
