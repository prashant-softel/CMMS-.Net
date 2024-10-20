using CMMSAPIs.Helper;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.SM
{
    public class RequestOrderRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public RequestOrderRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<CMDefaultResponse> CreateRequestOrder(CMCreateRequestOrder request, int userID, string facilityTimeZone)
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
                        string poDetailsQuery = $"INSERT INTO smrequestorderdetails (requestID,assetItemID,cost, currencyId, ordered_qty,remarks) " +
                        "values(" + roid + ", " + request.request_order_items[i].assetMasterItemID + ",  " + request.request_order_items[i].cost + ", " + request.request_order_items[i].currencyId + ", " + request.request_order_items[i].ordered_qty + ", '" + request.request_order_items[i].comment + "') ; SELECT LAST_INSERT_ID();";
                        DataTable dtInsertPO = await Context.FetchData(poDetailsQuery).ConfigureAwait(false);
                        int id = Convert.ToInt32(dtInsertPO.Rows[0][0]);
                    }
                    response = new CMDefaultResponse(ReturnID, CMMS.RETRUNSTATUS.SUCCESS, "Request order created successfully.");
                }
            }
            catch (Exception ex)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, ex.Message);
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, ReturnID, 0, 0, request.comment, CMMS.CMMS_Status.SM_RO_SUBMITTED);
            List<CMCreateRequestOrderGET> _ROList = await GetRODetailsByID(ReturnID.ToString(), facilityTimeZone);
            foreach (var ro in _ROList)
            {
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_RO, CMMS.CMMS_Status.SM_RO_SUBMITTED, new[] { userID }, ro);
                break;
            }



            return response;
        }
        internal async Task<CMDefaultResponse> UpdateRequestOrder(CMCreateRequestOrder request, int userID, string facilityTimeZone)
        {
            //string mainQuery = $"UPDATE smgoodsorderdetails SET generate_flag = " +request.generate_flag + ",status = "+request.status+", vendorID = "+request.vendorID+" WHERE ID = "+request.id+"";

            string updateRO = $" UPDATE smrequestorder SET facilityID = '{request.facilityID}'," +
                $" remarks = '{request.comment}', updated_by = {userID},updated_at='{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}' where id = {request.request_order_id}";
            var ResultROQuery = await Context.ExecuteNonQry<int>(updateRO);
            for (var i = 0; i < request.request_order_items.Count; i++)
            {
                if (request.request_order_items[i].itemID > 0)
                {
                    string updateQ = $"UPDATE smrequestorderdetails SET assetItemID = {request.request_order_items[i].assetMasterItemID} , cost = {request.request_order_items[i].cost} , ordered_qty = {request.request_order_items[i].ordered_qty}, remarks = '{request.request_order_items[i].comment}', currencyId={request.request_order_items[i].currencyId} " +
                        $" WHERE ID = {request.request_order_items[i].itemID}";
                    var result = await Context.ExecuteNonQry<int>(updateQ);
                }
                else
                {
                    string poDetailsQuery = $"INSERT INTO smrequestorderdetails (requestID,assetItemID,cost,ordered_qty,remarks,currencyId) " +
                       "values(" + request.request_order_id + ", " + request.request_order_items[i].assetMasterItemID + ",  " + request.request_order_items[i].cost + ", " + request.request_order_items[i].ordered_qty + ", '" + request.request_order_items[i].comment + "'," + request.request_order_items[i].currencyId + " ) ; SELECT LAST_INSERT_ID();";
                    DataTable dtInsertPO = await Context.FetchData(poDetailsQuery).ConfigureAwait(false);
                    int id = Convert.ToInt32(dtInsertPO.Rows[0][0]);
                }
            }
            CMDefaultResponse response = new CMDefaultResponse(request.request_order_id, CMMS.RETRUNSTATUS.SUCCESS, "Request order updated successfully.");
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, request.request_order_id, 0, 0, request.comment, CMMS.CMMS_Status.SM_RO_UPDATED);
            List<CMCreateRequestOrderGET> _ROList = await GetRODetailsByID(request.request_order_id.ToString(), facilityTimeZone);
            foreach (var ro in _ROList)
            {
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_RO, CMMS.CMMS_Status.SM_RO_UPDATED, new[] { userID }, ro);
                break;
            }
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteRequestOrder(CMApproval request, int userID, string facilityTimeZone)
        {
            string mainQuery = $"UPDATE smrequestorder SET status = {(int)CMMS.CMMS_Status.SM_RO_DELETED}, remarks= '{request.comment}' WHERE ID = " + request.id + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Request order deleted.");
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, request.id, 0, 0, request.comment, CMMS.CMMS_Status.SM_RO_DELETED);
            List<CMCreateRequestOrderGET> _ROList = await GetRODetailsByID(request.id.ToString(), facilityTimeZone);
            foreach (var ro in _ROList)
            {
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_RO, CMMS.CMMS_Status.SM_RO_DELETED, new[] { userID }, ro);
                break;
            }
            return response;
        }
        internal async Task<CMDefaultResponse> CloseRequestOrder(CMApproval request, int userID, string facilityTimeZone)
        {
            //validate request.id
            string mainQuery = $"UPDATE smrequestorder SET status = {(int)CMMS.CMMS_Status.SM_RO_CLOSED}, remarks= '{request.comment}' WHERE ID = " + request.id + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Request order {" + request.id + "} closed.");
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, request.id, 0, 0, request.comment, CMMS.CMMS_Status.SM_RO_CLOSED);
            List<CMCreateRequestOrderGET> _ROList = await GetRODetailsByID(request.id.ToString(), facilityTimeZone);
            foreach (var ro in _ROList)
            {
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_RO, CMMS.CMMS_Status.SM_RO_CLOSED, new[] { userID }, ro);
                break;
            }
            return response;
        }

        internal async Task<List<CMCreateRequestOrder>> GetRequestOrderList(int facilityID, DateTime fromDate, DateTime toDate, string facilitytimeZone)
        {
            string filter = " facilityID = " + facilityID + " and  DATE_FORMAT(po.request_date, '%Y-%m-%d') >= '" + fromDate.ToString("yyyy-MM-dd") + "'  and  DATE_FORMAT(po.request_date, '%Y-%m-%d') <= '" + toDate.ToString("yyyy-MM-dd") + "'";

            string query = "SELECT fc.name as facilityName,pod.ID as requestDetailsID, facilityid as      " +
                           " facility_id,pod.spare_status,po.remarks,sai.orderflag," +
                           " sam.asset_type_ID,pod.requestID,pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty," +
                           " po.request_date,sam.asset_type_ID,sam.asset_name,po.receiverID," +
                           " po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty, " +
                           " f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by, " +
                           " pod.order_type as asset_type_ID_OrderDetails, receive_later, added_to_store,reject_reccomendations as  rejectedRemark, " +
                           " po.amount,  pod.currencyId as currencyId , curr.name as currency ,  \r\n    CONCAT(ed.firstName,' ',ed.lastName) as generatedBy, " +
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
                           " LEFT JOIN currency curr ON curr.id = pod.currencyId  LEFT JOIN users ed2 ON ed2.id = po.approved_by " +
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
                    comment = p.itemcomment,
                    currency = p.currency,
                    currencyId = p.currencyId

                }).ToList();
                _MasterList[i].number_of_masters = _itemList.Where(group => group.requestID == _MasterList[i].request_order_id).Count();
                _MasterList[i].number_of_item_count = (int)_itemList.Where(group => group.requestID == _MasterList[i].request_order_id).Sum(x => x.ordered_qty);
                _MasterList[i].cost = _itemList.Where(group => group.requestID == _MasterList[i].request_order_id).Sum(x => x.cost);
                _MasterList[i].request_order_items = _itemList.Where(group => group.requestID == _MasterList[i].request_order_id).ToList();
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_MasterList[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_RO, _Status);
                CMCreateRequestOrderGET m_SMROObj = new CMCreateRequestOrderGET();
                string _longStatus = getLongStatus(_MasterList[i].request_order_id, _Status, m_SMROObj);
                _MasterList[i].status_short = _shortStatus;
                _MasterList[i].status_long = _longStatus;
            }
            foreach (var list in _MasterList)
            {
                if (list != null && list.approvedAt != null)
                    list.approvedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.approvedAt);
                if (list != null && list.generatedAt != null)
                    list.generatedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.generatedAt);
                if (list != null && list.rejectedAt != null)
                    list.rejectedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.rejectedAt);

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


        internal async Task<CMDefaultResponse> ApproveRequestOrder(CMApproval request, int userId, string facilityTimeZone)
        {

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string UpdatesqlQ = $" UPDATE smrequestorder SET approved_by = {userId}, status = {(int)CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED}, remarks = '{request.comment}',approvedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(UpdatesqlQ).ConfigureAwait(false);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Approved request order  " + request.id + "  successfully.");
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, request.id, 0, 0, request.comment, CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED);

            List<CMCreateRequestOrderGET> _ROList = await GetRODetailsByID(request.id.ToString(), facilityTimeZone);
            foreach (var ro in _ROList)
            {
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_RO, CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED, new[] { userId }, ro);
                break;
            }
            return response;
        }

        internal async Task<CMDefaultResponse> RejectRequestOrder(CMApproval request, int userId, string facilityTimeZone)
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

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_RO, request.id, 0, 0, request.comment, CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED);

            List<CMCreateRequestOrderGET> _ROList = await GetRODetailsByID(request.id.ToString(), facilityTimeZone);
            foreach (var ro in _ROList)
            {
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_RO, CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED, new[] { userId }, ro);
                break;
            }
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Rejected request order.");
            return response;
        }

        /*  public async Task<CMCreateRequestOrder> GetRODetailsByID(int id,string facilitytimeZone)
          {

              string query = "SELECT fc.name as facilityName,pod.ID as requestDetailsID, facilityid as      " +
                  " facility_id,pod.spare_status,po.remarks,sai.orderflag," +
                  " sam.asset_type_ID,pod.requestID,pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty," +
                  " po.request_date,sam.asset_type_ID,sam.asset_name,po.receiverID," +
                  " po.status,sam.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty, " +
                  " f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by, " +
                  " pod.order_type as asset_type_ID_OrderDetails, receive_later, added_to_store,reject_reccomendations as  rejectedRemark, " +
                  " po.amount,  po.currency as currencyID , curr.name as currency ,  \r\n    CONCAT(ed.firstName,' ',ed.lastName) as generatedBy, " +
                  " po.received_on as generatedAt,approvedOn as approvedAt,CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy, " +
                  " pod.remarks as itemcomment, CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy , " +
                  " CONCAT(ed3.firstName,' ',ed3.lastName) as rejectedBy , po.rejected_at as rejectedAt, sic.cat_name asset_type_Name" +
                  " FROM smrequestorderdetails pod" +
                  " LEFT JOIN smrequestorder po ON po.ID = pod.requestID" +
                  " LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
                  " LEFT JOIN smassetmasters sam ON sam.id = pod.assetItemID" +
                  " LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement LEFT JOIN smitemcategory sic ON sic.ID = sam.item_category_ID" +
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
                  comment = p.itemcomment,
                  asset_code = p.asset_code,
                  asset_type = p.asset_type,
                  asset_cat = p.asset_type_Name
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

              foreach (var list in _List)
              {
                  if (list != null && list.approvedAt != null)
                      list.approvedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.approvedAt);
                  if (list != null && list.challan_date != null)
                      list.challan_date = (DateTime)(list.approvedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.challan_date));
                  if (list != null && list.receivedAt != null)
                      list.receivedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.receivedAt);
                  if (list != null && list.request_date != null)
                      list.request_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.request_date);
              }
              return _MasterList;

          }*/
        //changes
        public async Task<List<CMCreateRequestOrderGET>> GetRODetailsByID(string IDs, string facilityTimeZone)
        {
            // Convert list of IDs to comma-separated string
            // string idList = string.Join(",", IDs);
            if (IDs == null || IDs == "")
            {
                IDs = "0";
            }

            string query = " SELECT fc.name as facilityName, pod.ID as requestDetailsID, facilityid as facility_id, pod.spare_status, po.remarks, sai.orderflag, " +
                           "sam.asset_type_ID, pod.requestID, pod.assetItemID, sai.serial_number, sai.location_ID, pod.cost, pod.ordered_qty, " +
                           "po.request_date, sam.asset_type_ID, sam.asset_name, po.receiverID, po.status, sam.asset_code, t1.asset_type, t2.cat_name, " +
                           "pod.received_qty, pod.damaged_qty, pod.accepted_qty, f1.file_path, f1.Asset_master_id, sm.decimal_status, sm.spare_multi_selection, " +
                           "po.generated_by, pod.order_type as asset_type_ID_OrderDetails, receive_later, added_to_store, reject_reccomendations as rejectedRemark, " +
                           "po.amount, pod.currencyId as currencyId, curr.name as currency, CONCAT(ed.firstName,' ',ed.lastName) as generatedBy, " +
                           "po.received_on as generatedAt, approvedOn as approvedAt, CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy, " +
                           "pod.remarks as itemcomment, CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy, " +
                           "CONCAT(ed3.firstName,' ',ed3.lastName) as rejectedBy, " +
                           "CONCAT(ed4.firstName,' ',ed4.lastName) as updated_by,po.updated_at, " +
                           "po.rejected_at as rejectedAt, sic.cat_name as asset_type_Name " +
                           "FROM smrequestorderdetails pod " +
                           "LEFT JOIN smrequestorder po ON po.ID = pod.requestID " +
                           "LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID " +
                           "LEFT JOIN smassetmasters sam ON sam.id = pod.assetItemID " +
                           "LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement " +
                           "LEFT JOIN smitemcategory sic ON sic.ID = sam.item_category_ID " +
                           "LEFT JOIN (SELECT file.file_path, file.Asset_master_id as Asset_master_id FROM smassetmasterfiles file " +
                           "           LEFT JOIN smassetmasters sam ON file.Asset_master_id = sam.id) f1 ON f1.Asset_master_id = sam.id " +
                           "LEFT JOIN (SELECT sat.asset_type, s1.ID as master_ID FROM smassettypes sat " +
                           "           LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID) t1 ON t1.master_ID = sam.ID " +
                           "LEFT JOIN (SELECT sic.cat_name, s2.ID as master_ID FROM smitemcategory sic " +
                           "           LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID) t2 ON t2.master_ID = sam.ID " +
                           "LEFT JOIN facilities fc ON fc.id = po.facilityID " +
                           "LEFT JOIN currency curr ON curr.id = pod.currencyId " +
                           "LEFT JOIN users ed2 ON ed2.id = po.approved_by " +
                           "LEFT JOIN users ed ON ed.id = po.generated_by " +
                           "LEFT JOIN users ed1 ON ed1.id = po.receiverID " +
                           "LEFT JOIN users ed3 ON ed3.id = po.rejeccted_by " +
                           "LEFT JOIN users ed4 ON ed4.id = po.updated_by " +
                           "WHERE  po.ID  IN (" + IDs + ") ;";

            List<CMRequestOrderList> _List = await Context.GetData<CMRequestOrderList>(query).ConfigureAwait(false);
            List<CMCreateRequestOrderGET> _MasterList = _List.Select(p => new CMCreateRequestOrderGET
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
                rejectedAt = p.rejectedAt,
                currency = p.currency,
                updated_by = p.updated_by,
                updated_at = p.updated_at

            }).ToList();
            List<CMRequestOrder_ITEMS_GET> _itemList = _List.Select(p => new CMRequestOrder_ITEMS_GET
            {
                itemID = p.requestDetailsID,
                requestID = p.requestID,
                cost = p.cost,
                currencyId = p.currencyId,
                currency = p.currency,
                asset_name = p.asset_name,
                id = p.assetItemID,
                ordered_qty = p.ordered_qty,
                comment = p.itemcomment,
                asset_code = p.asset_code,
                asset_type = p.asset_type,
                asset_cat = p.asset_type_Name
            }).ToList();
            foreach (var MasterList in _MasterList)
            {
                MasterList.cost = _itemList.Sum(x => x.cost);
                MasterList.request_order_items = _itemList;
                MasterList.number_of_item_count = (int)_itemList.Sum(x => x.ordered_qty);
                MasterList.number_of_masters = _itemList.Count;
            }

            foreach (var MasterList in _MasterList)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(MasterList.status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_RO, _Status);
                string _longStatus = getLongStatus(MasterList.request_order_id, _Status, MasterList);

                MasterList.status_short = _shortStatus;
                MasterList.status_long = _longStatus;
            }
            foreach (var list in _List)
            {
                if (list != null && list.approvedAt != null)
                    list.approvedAt = await _utilsRepo.ConvertToUTCDTC(facilityTimeZone, (DateTime)list.approvedAt);
                if (list != null && list.challan_date != null)
                    list.challan_date = (DateTime)(list.approvedAt = await _utilsRepo.ConvertToUTCDTC(facilityTimeZone, list.challan_date));
                if (list != null && list.receivedAt != null)
                    list.receivedAt = await _utilsRepo.ConvertToUTCDTC(facilityTimeZone, (DateTime)list.receivedAt);
                if (list != null && list.request_date != null)
                    list.request_date = await _utilsRepo.ConvertToUTCDTC(facilityTimeZone, (DateTime)list.request_date);
            }
            return _MasterList;
        }


        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.SM_RO_DRAFT:
                    retValue = "Drafted";
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMITTED:
                    retValue = "Waiting for approval";
                    break;
                case CMMS.CMMS_Status.SM_RO_UPDATED:
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

        internal static string getLongStatus(int ID, CMMS.CMMS_Status m_notificationID, CMCreateRequestOrderGET m_SMROObj)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.SM_RO_DRAFT:
                    retValue = $"Request order{ID} drafted";
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMITTED:
                    retValue = String.Format("RO{0} Submitted and Waiting for Appoval By {1}", m_SMROObj.request_order_id, m_SMROObj.generatedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_REJECTED:
                    retValue = String.Format("RO{0} Rejected By {1}", m_SMROObj.request_order_id, m_SMROObj.rejectedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_UPDATED:
                    retValue = String.Format("RO{0} Updated By {1}", m_SMROObj.request_order_id, m_SMROObj.updated_by);
                    break;
                case CMMS.CMMS_Status.SM_RO_SUBMIT_APPROVED:
                    retValue = String.Format("RO{0} Approved By {1}", m_SMROObj.request_order_id, m_SMROObj.approvedBy);
                    break;
                case CMMS.CMMS_Status.SM_RO_DELETED:
                    retValue = String.Format("RO{0} Deleted By {1}", m_SMROObj.request_order_id, m_SMROObj.deleted_by);
                    break;
                case CMMS.CMMS_Status.SM_RO_CLOSED:
                    retValue = String.Format("RO{0} Closed By {1}", m_SMROObj.request_order_id, m_SMROObj.closed_by);
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }
    }
}
