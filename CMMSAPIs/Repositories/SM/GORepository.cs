using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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
                    retValue = "Waiting for approval";
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

        private static string getLongStatus(CMMS.CMMS_Status m_notificationID, int Id, CMGOMaster m_GOObj)
        {
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)m_notificationID;
            string retValue = "";
            switch (status)
            {
                case CMMS.CMMS_Status.GO_DRAFT:
                    retValue = String.Format("GO{0} Updated By {1}", m_GOObj.Id, m_GOObj.go_updated_by_name);
                    break;
                case CMMS.CMMS_Status.GO_SUBMITTED:
                    retValue = String.Format("GO{0} Submitted and Waiting for Appoval By {1}", m_GOObj.Id, m_GOObj.submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_CLOSED:
                    retValue = String.Format("GO{0} Closed By {1}", m_GOObj.Id, m_GOObj.closed_by_name);
                    break;
                case CMMS.CMMS_Status.GO_DELETED:
                    retValue = String.Format("GO{0} deleted By {1}", m_GOObj.Id, m_GOObj.deleted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_REJECTED:
                    retValue = String.Format("GO{0} rejected By {1}", m_GOObj.Id, m_GOObj.rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_APPROVED:
                    retValue = String.Format("GO{0} approved By {1}", m_GOObj.Id, m_GOObj.approved_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVE_DRAFT:
                    retValue = String.Format("GO{0} receive draft By {1}", m_GOObj.Id, m_GOObj.receive_submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED:
                    retValue = String.Format("GO{0} receive submitted By {1}", m_GOObj.Id, m_GOObj.receive_submitted_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_REJECTED:
                    retValue = String.Format("GO{0} receive rejected By {1}", m_GOObj.Id, m_GOObj.receive_rejected_by_name);
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_APPROVED:
                    retValue = String.Format("GO{0} receive approved By {1}", m_GOObj.Id, m_GOObj.receive_approved_by_name);
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
        //    //              "CONCAT(ed1.Emp_First_Name,' ',ed1.Emp_Last_Name) as receivedBy,CONCAT(ed2.Emp_First_Name,' ',ed2.Emp_Last_Name) as approvedBy,\n\t\t\t\tbl.Business_Name as vendor_name\n\t\t        FROM smgoodsorderdetails pod\n\t\t        LEFT JOIN smgoodsorder po ON po.ID = pod.purchaseID\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
        //    //              "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code    LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\n\t\t            LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\n\t\t        )  t1 ON t1.master_ID = sam.ID\n\t\t        LEFT JOIN (\n\t\t      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic" +
        //    //              "LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID LEFT JOIN employees ed ON ed.ID = po.generated_by\n\t\t        LEFT JOIN employees ed1 ON ed1.ID = po.receiverID\n\t\t\t\tLEFT JOIN employees ed2 ON ed2.ID = po.approved_by\n\t\t\t\tLEFT JOIN business bl ON bl.id = po.vendorID\n\t\t        /*LEFT JOIN FlexiMC_Emp_details ed ON ed.ID = po.generated_by\n\t\t        LEFT JOIN FlexiMC_Emp_details ed1 ON ed1.ID = po.receiverID\n\t\t\t\tLEFT JOIN FlexiMC_Emp_details ed2 ON ed2.ID = po.approved_by\n\t\t\t\tLEFT JOIN FlexiMC_Business_list bl ON bl.id = po.vendorID\n\t\t       WHERE po.ID =1 */";

        //    string stmt = "SELECT pod.ID ,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_name,sam.asset_type_ID,pod.purchaseID," +
        //        "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost ,pod.ordered_qty,po.remarks as rejectedRemark,po.facilityID as facility_id,po.purchaseDate,fac.name as facility_name, " +
        //        "sam.asset_type_ID,\r\n\t\t        po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty," +
        //        "pod.accepted_qty,po.received_on as receivedAt,po.approvedOn as approvedAt,\r\n\t\t\t\tCONCAT(ed.firstName,' ',ed.lastName) as generatedBy," +
        //        "CONCAT(ed1.firstName,' ',ed1.lastName) as receivedBy,CONCAT(ed2.firstName,' ',ed2.lastName) as approvedBy," +
        //        "\r\n\t\t\t\tbl.name as vendor_name\r\n\t\t        FROM smgoodsorderdetails pod\r\n\t\t       " +
        //        " LEFT JOIN smgoodsorder po ON po.ID = pod.purchaseID\r\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
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
        //        string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_GO, _Status);
        //        _GOList[i].status_short = _shortStatus;
        //    }

        //    return _GOList;
        //}
        internal async Task<CMGoodsOrderList> GetGOItemByID(int id, string facilitytimeZone)
        {
            string query = "SELECT fc.name as facilityName,pod.ID as podID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_type_ID," +
          "pod.purchaseID,pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,\r\nbl.name as vendor_name,\r\n     " +
          "   po.facilityID as facility_id,po.purchaseDate,sam.asset_type_ID,sam.asset_name,po.receiverID,\r\n        " +
          "po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty," +
          "f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by,pod.order_type, receive_later, " +
          "added_to_store,   \r\n      " +
          "  po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, po.lr_no, po.condition_pkg_received, " +
          "po.vehicle_no, po.gir_no, po.challan_date,  po.job_ref, po.amount, po.currency as currencyID , curr.name as currency \r\n      " +
          "  FROM smgoodsorderdetails pod\r\n        LEFT JOIN smgoodsorder po ON po.ID = pod.purchaseID\r\n     " +
          "   LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID\r\n       " +
          " LEFT JOIN smassetmasters sam ON sam.ID = pod.assetItemID\r\n      " +
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
            foreach (var list in _List)
            {
                if (list != null && list.approvedAt != null)
                    list.approvedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.approvedAt.Value);
                if (list != null && list.challan_date != null)
                    list.challan_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.challan_date);
                if (list != null && list.po_date != null)
                    list.po_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.po_date);
                if (list != null && list.receivedAt != null)
                    list.receivedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.receivedAt);
            }
            return _List[0];

        }



        internal Task<List<CMGoodsOrderList>> GetAssetCodeDetails(int asset_code)
        {
            /*
             * 
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CreateGO(CMGoodsOrderList request, int userID, string facilitytimeZone)
        {
            int goid = 0;
            int assetTypeId = 0;
            if (request.go_items != null)
            {
                int status = 0;
                if (request.is_submit == 0)
                {
                    status = (int)CMMS.CMMS_Status.GO_DRAFT;
                }
                else
                {
                    status = (int)CMMS.CMMS_Status.GO_SUBMITTED;
                }

                string purchaseDate = (request.purchaseDate == null) ? "0001-01-01 00:00:00" : ((DateTime)request.purchaseDate).ToString("yyyy-MM-dd HH:mm:ss");
                string po_date = (request.po_date == null) ? "0001-01-01 00:00:00" : ((DateTime)request.po_date).ToString("yyyy-MM-dd HH:mm:ss");
                string challan_date = (request.challan_date == null) ? "0001-01-01 00:00:00" : ((DateTime)request.challan_date).ToString("yyyy-MM-dd HH:mm:ss");
                string received_date = (request.receivedAt == null) ? "0001-01-01 00:00:00" : ((DateTime)request.receivedAt).ToString("yyyy-MM-dd HH:mm:ss");




                string poInsertQuery = $" INSERT INTO smgoodsorder (facilityID,vendorID,receiverID,generated_by,purchaseDate,orderDate,status," +
                    $" challan_no,po_no, freight,transport, " +
                    $"no_pkg_received,lr_no,condition_pkg_received,vehicle_no, gir_no, challan_date,po_date, job_ref,amount, currency,withdraw_by,withdrawOn,order_type,received_on) " +
                    $"VALUES({request.facility_id},{request.vendorID}, {request.receiverID}, {userID}, '{purchaseDate}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{status}'," +
                    $"'{request.challan_no}','{request.po_no}','{request.freight}','', '{request.no_pkg_received}', '{request.lr_no}', '{request.condition_pkg_received}','{request.vehicle_no}','{request.gir_no}','{challan_date}'," +
                    $"'{po_date}','{request.job_ref}','{request.amount}', '{request.currencyID}',0,'0001-01-01',0,'{received_date}');" +
                    $" SELECT LAST_INSERT_ID();";

                DataTable dt2 = await Context.FetchData(poInsertQuery).ConfigureAwait(false);
                goid = Convert.ToInt32(dt2.Rows[0][0]);
                int is_splited = 0;
                for (var i = 0; i < request.go_items.Count; i++)
                {
                    string stmtAssetType = $"SELECT asset_type_ID FROM smassetmasters WHERE id = '{request.go_items[i].assetMasterItemID}'";
                    DataTable dtAssetType = await Context.FetchData(stmtAssetType).ConfigureAwait(false);

                    if (dtAssetType == null && dtAssetType.Rows.Count == 0)
                    {
                        throw new Exception("Asset type ID not found");
                    }
                    else
                    {
                        assetTypeId = Convert.ToInt32(dtAssetType.Rows[0][0]);
                    }



                    string poDetailsQuery = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,cost,ordered_qty,location_ID, paid_by_ID, requested_qty,sr_no,spare_status,is_splited, requestOrderId, requestOrderItemID) " +
                    "values(" + goid + ", " + request.go_items[i].assetMasterItemID + ",  " + request.go_items[i].cost + ", " + request.go_items[i].ordered_qty + ", " + request.location_ID + ", " + request.go_items[i].paid_by_ID + ", " + request.go_items[i].requested_qty + ", '" + request.go_items[i].sr_no + "', " + assetTypeId + ", 1, " + request.go_items[i].requestOrderId + "," + request.go_items[i].requestOrderItemID + ") ; SELECT LAST_INSERT_ID();";
                    DataTable dtInsertPO = await Context.FetchData(poDetailsQuery).ConfigureAwait(false);
                    int id = Convert.ToInt32(dtInsertPO.Rows[0][0]);
                }
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, goid, 0, 0, request.remarks, CMMS.CMMS_Status.GO_DRAFT);
            CMGOMaster _GOList = await GetGODetailsByID(goid, facilitytimeZone);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_SUBMITTED, new[] { userID }, _GOList);
            CMDefaultResponse response = new CMDefaultResponse(goid, CMMS.RETRUNSTATUS.SUCCESS, "Goods order created successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateGO(CMGoodsOrderList request, int userID, string facilitytimeZone)
        {
            string OrderQuery = "";
            string received_date = (request.receivedAt == null) ? "0001-01-01 00:00:00" : ((DateTime)request.receivedAt).ToString("yyyy-MM-dd HH:mm:ss");

            if (request.is_submit == 0)
            {

                OrderQuery = $"UPDATE smgoodsorder SET " +
                        $"challan_no = '{request.challan_no}',po_no='{request.po_no}', freight='{request.freight}',no_pkg_received='{request.no_pkg_received}'," +
                        $"lr_no='{request.lr_no}',condition_pkg_received='{request.condition_pkg_received}',vehicle_no='{request.vehicle_no}', gir_no='{request.gir_no}', " +
                        $"challan_date = '{request.challan_date.Value.ToString("yyyy-MM-dd")}', " +
                        $"job_ref='{request.job_ref}',amount='{request.amount}', currency={request.currencyID},go_updated_by = {userID},go_updatedOn = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.GO_DRAFT}," +
                        $" received_on = '{received_date}', po_date = '{request.po_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', purchaseDate= '{request.purchaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                        $"  where ID={request.id}";
            }
            else
            {
                OrderQuery = $"UPDATE smgoodsorder SET " +
                        $"challan_no = '{request.challan_no}',po_no='{request.po_no}', freight='{request.freight}',no_pkg_received='{request.no_pkg_received}'," +
                        $"lr_no='{request.lr_no}',condition_pkg_received='{request.condition_pkg_received}',vehicle_no='{request.vehicle_no}', gir_no='{request.gir_no}', " +
                        $"challan_date = '{request.challan_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                        $"job_ref='{request.job_ref}',amount='{request.amount}', currency={request.currencyID},go_updated_by = {userID},go_updatedOn = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.GO_SUBMITTED}, received_on = '{received_date}', po_date = '{request.po_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', purchaseDate= '{request.purchaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}' where ID={request.id}";
            }
            await Context.ExecuteNonQry<int>(OrderQuery);

            for (var i = 0; i < request.go_items.Count; i++)
            {
                string itemsQuery = "";
                if (request.go_items[i].ordered_qty > 0)
                {
                    if (request.go_items[i].goItemID > 0)
                    {
                        itemsQuery = $"UPDATE smgoodsorderdetails SET location_ID = {request.location_ID},cost = {request.go_items[i].cost}, accepted_qty = {request.go_items[i].accepted_qty},ordered_qty = {request.go_items[i].ordered_qty} , requested_qty = {request.go_items[i].requested_qty}, received_qty= {request.go_items[i].received_qty},lost_qty = {request.go_items[i].lost_qty}, damaged_qty={request.go_items[i].damaged_qty}, paid_by_ID = {request.go_items[i].paid_by_ID}" +
                            $" , sr_no = '{request.go_items[i].sr_no}', requestOrderId= {request.go_items[i].requestOrderId}, requestOrderItemID = {request.go_items[i].requestOrderItemID}  WHERE ID = {request.go_items[i].goItemID}";
                    }
                    else
                    {
                        int assetTypeId = 0;
                        string stmtAssetType = $"SELECT asset_type_ID FROM smassetmasters WHERE id = '{request.go_items[i].assetMasterItemID}'";
                        DataTable dtAssetType = await Context.FetchData(stmtAssetType).ConfigureAwait(false);

                        if (dtAssetType == null && dtAssetType.Rows.Count == 0)
                        {
                            assetTypeId = 0;
                        }
                        else
                        {
                            assetTypeId = Convert.ToInt32(dtAssetType.Rows[0][0]);
                        }

                        string poDetailsQuery = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,cost,ordered_qty,location_ID, paid_by_ID, requested_qty,sr_no,spare_status,is_splited, requestOrderId, requestOrderItemID) " +
                        "values(" + request.id + ", " + request.go_items[i].assetMasterItemID + ",  " + request.go_items[i].cost + ", " + request.go_items[i].ordered_qty + ", " + request.location_ID + ", " + request.go_items[i].paid_by_ID + ", " + request.go_items[i].requested_qty + ", '" + request.go_items[i].sr_no + "', " + assetTypeId + ", 1, " + request.go_items[i].requestOrderId + "," + request.go_items[i].requestOrderItemID + ") ; SELECT LAST_INSERT_ID();";
                        DataTable dtInsertPO = await Context.FetchData(poDetailsQuery).ConfigureAwait(false);
                    }

                }
                else
                {
                    itemsQuery = $"DELETE from smgoodsorderdetails where ID = {request.go_items[i].goItemID}";
                }
                if (itemsQuery != "")
                {
                    var result = await Context.ExecuteNonQry<int>(itemsQuery);
                }

            }
            if (request.is_submit == 0)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, request.id, 0, 0, request.remarks, CMMS.CMMS_Status.GO_DRAFT);
            }
            else
            {
                CMGOMaster _GOList = await GetGODetailsByID(request.id, facilitytimeZone);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_SUBMITTED, new[] { userID }, _GOList);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, request.id, 0, 0, request.remarks, CMMS.CMMS_Status.GO_SUBMITTED);
            }
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Goods order updated successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteGO(CMApproval request, int userID, string facilitytimeZone)
        {
            string mainQuery = $"UPDATE smgoodsorder SET status = {(int)CMMS.CMMS_Status.GO_DELETED}, remarks = '{request.comment}', updated_by = {userID}, updatedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}' WHERE ID = " + request.id + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, request.id, 0, 0, "Goods order deleted.", CMMS.CMMS_Status.GO_DELETED);


            CMGOMaster _GOList = await GetGODetailsByID(request.id, facilitytimeZone);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_DELETED, new[] { userID }, _GOList);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Goods order deleted.");
            return response;
        }
        internal async Task<CMDefaultResponse> CloseGO(CMGoodsOrderList request, int userID,string facilitytimeZone)
        {
            string mainQuery = $"UPDATE smgoodsorder SET withdraw_by = " + userID + ", status = " + (int)CMMS.CMMS_Status.GO_CLOSED + ", remarks = '" + request.remarks + "', withdrawOn = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE ID = " + request.id + "";
            await Context.ExecuteNonQry<int>(mainQuery);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, request.id, 0, 0, request.remarks, CMMS.CMMS_Status.GO_CLOSED);

            CMGOMaster _GOList = await GetGODetailsByID(request.id, facilitytimeZone);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_CLOSED, new[] { userID }, _GOList);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Goods order withdrawn successfully.");
            return response;
        }


        //public async Task<CMDefaultResponse> GOApproval(CMGoodsOrderList request, int userID)
        //{
        //    try
        //    {

        //        DateTime date = DateTime.Now;
        //        string UpdatesqlQ = $" UPDATE smgoodsorder SET approved_by = {request.approvedBy}, status = {request.status}, remarks = '{request.remarks}',approvedOn = '{date.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
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
        //        //await this.transactionObj.sendMail('GOApproval', goid, subject, finalArray);

        //        // Insert a history record.

        //        CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Data Updated.");
        //        return response;
        //    }
        //    catch (Exception e)
        //    {

        //        throw e;
        //    }
        //}

        internal async Task<CMDefaultResponse> ApproveGoodsOrder(CMApproval request, int userId, string facilitytimeZone)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            var data = await this.getPurchaseDetailsByID(request.id);

            if (data[0].status != (int)CMMS.CMMS_Status.GO_SUBMITTED)
            {
                return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, $"Store keeper not updated quantity for Goods Order {request.id}.");
            }



            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            string UpdatesqlQ = $" UPDATE smgoodsorder SET approved_by = {userId}, status = {(int)CMMS.CMMS_Status.GO_APPROVED}, remarks = '{request.comment}',approvedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(UpdatesqlQ).ConfigureAwait(false);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;



            // Entry in TransactionDetails


            /*string myQuery = $"SELECT   ID ,facilityID ,vendorID ,receiverID ,generated_by ,purchaseDate ,orderDate ,challan_no ,po_no ,\r\n     " +
                $" freight ,transport ,no_pkg_received ,lr_no ,\r\n      " +
                $"condition_pkg_received ,vehicle_no ,gir_no ,challan_date ,po_date ,\r\n      " +
                $"job_ref ,amount ,currency as currencyID,status ,\r\n     " +
                $" lastModifiedDate ,generate_flag ,s2s_generated_by ,case when received_on = '0000-00-00 00:00:00' then null else received_on end as received_on ,invoice_copy ,issued_by ,issued_on ,\r\n    " +
                $"  approved_by ,approvedOn ,withdraw_by ,withdrawOn ,reorder_flag ,\r\n     " +
                $" order_type ,remarks ,updated_by ,updatedOn ,rejected_by ,rejectedOn" +
                $" \r\nFROM smgoodsorder where id = {request.id}";
            List<CMGoodsOrderList> _List = await Context.GetData<CMGoodsOrderList>(myQuery).ConfigureAwait(false);*/
            //CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.SM_PO_CLOSED_APPROVED, _List[0]);
            // Entry for stock items


            foreach (var item in data)
            {
                if (item.spare_status == (int)CMMS.SM_AssetTypes.Spare)
                {
                    var stmtU = $"UPDATE smgoodsorderdetails set is_splited = 0 where id = {item.podID}";
                    int resultU = await Context.ExecuteNonQry<int>(stmtU).ConfigureAwait(false);
                }
            }


            foreach (var item in data)
            {
                var assetCode = item.asset_code;
                var orderedQty = item.ordered_qty;
                var type = item.asset_type;
                var cost = item.cost;
                int purchaseOrderDetailsID = 0;
                // Get the asset type ID.
                int assetTypeId = 0;
                string stmtAssetType = $"SELECT asset_type_ID FROM smassetmasters WHERE asset_code = '{item.asset_code}'";
                DataTable dtAssetType = await Context.FetchData(stmtAssetType).ConfigureAwait(false);

                if (dtAssetType == null && dtAssetType.Rows.Count == 0)
                {
                    throw new Exception("Asset type ID not found");
                }
                else
                {
                    assetTypeId = Convert.ToInt32(dtAssetType.Rows[0][0]);
                }
                int isMultiSelectionEnabled = await getMultiSpareSelectionStatus(item.asset_code);
                // Check if the asset type is Spare.
                // assetTypeId == 2 is Spare
                if (assetTypeId == (int)CMMS.SM_AssetTypes.Spare || assetTypeId == (int)CMMS.SM_AssetTypes.Tools || assetTypeId == (int)CMMS.SM_AssetTypes.SpecialTools)
                {
                    int assetItemId;
                    for (var i = 0; i < item.ordered_qty; i++)
                    {
                        // Insert the asset item.

                        int assetMasterID = 0;

                        assetMasterID = item.assetItemID;

                        var stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,item_condition,status,assetMasterID,asset_type,goods_order_ID) VALUES ({item.facility_id},'{assetCode}',1,0, {assetMasterID}, {assetTypeId}, {request.id}); SELECT LAST_INSERT_ID();";
                        DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                        assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);

                        var stmtU = $"UPDATE smassetitems set materialID = {assetItemId} where id = {assetItemId}";
                        int resultU = await Context.ExecuteNonQry<int>(stmtU).ConfigureAwait(false);

                        //assetItemIDByCode[assetCode] = assetItemId;
                        stmtI = "";
                        stmtI = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,spare_status,cost,ordered_qty,location_ID,received_qty,paid_by_ID,is_splited, requested_qty)" +
                                    $"VALUES({item.purchaseID},{item.assetItemID},{item.asset_type_ID},{item.cost},1,0,1,{item.paid_by_ID},1,1); SELECT LAST_INSERT_ID();";
                        DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                        purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                    }


                    // Check if the asset item ID is already exists.

                    //if (item.assetItemID == null || item.assetItemID == 0)
                    //{

                    //    // Have to check later

                    //    //if (isMultiSelectionEnabled > 0)
                    //    //{
                    //    //    // Insert the asset item.
                    //    //    var stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,item_condition,status) VALUES ({item.facility_id},'{assetCode}',1,0); SELECT LAST_INSERT_ID();";
                    //    //    DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                    //    //    assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);
                    //    //    //assetItemIDByCode[assetCode] = assetItemId;
                    //    //    stmtI = "";
                    //    //    stmtI = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
                    //    //                $"VALUES({item.purchaseID},{assetItemId},{item.asset_type_ID},{item.cost},1,0); SELECT LAST_INSERT_ID();";
                    //    //    DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                    //    //    purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                    //    //}
                    //    //else
                    //    //{
                    //    //    // Insert the asset item.
                    //    //    var stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,item_condition,status) VALUES ({item.facility_id},'{assetCode}',1,0); SELECT LAST_INSERT_ID();";
                    //    //    DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                    //    //    assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);
                    //    //    //assetItemIDByCode[assetCode] = assetItemId;
                    //    //    stmtI = "";
                    //    //    stmtI = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
                    //    //                $"VALUES({item.purchaseID},{assetItemId},{item.asset_type_ID},{item.cost},0,0); SELECT LAST_INSERT_ID();";
                    //    //    DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                    //    //    purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                    //    //}
                    //}
                    //else
                    //{
                    //    string stmtI = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
                    //                $"VALUES({item.purchaseID},{item.assetItemID},{item.asset_type_ID},{item.cost},0,0); SELECT LAST_INSERT_ID();";
                    //    DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                    //    purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                    //}

                    // Insert the Goods Order detail.
                }
                else
                {
                    // Get the asset item ID.
                    //int assetItemId = 0;
                    //assetItemId = await getAssetItemID(item.asset_code, item.facility_id, 0);
                    //if (assetItemId == 0)
                    //{
                    //    throw new Exception("asset_item_ID is empty");
                    //}
                    //else
                    //{

                    //}
                    //string stmtI = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID, received_qty)" +
                    //               $"VALUES({item.purchaseID},{item.assetItemID},{item.asset_type_ID},{item.cost},0,0,{item.requested_qty}); SELECT LAST_INSERT_ID();";
                    //DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                    //purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                    int assetMasterID = 0;
                    assetMasterID = item.assetItemID;

                    var stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,item_condition,status,assetMasterID,materialID,asset_type,goods_order_ID) VALUES ({item.facility_id},'{assetCode}',1,0, {assetMasterID},{assetMasterID},{assetTypeId}, {request.id}); SELECT LAST_INSERT_ID();";
                    DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                    var assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);


                }
            }

            string historyRemark = "";
            if (request.comment == null || request.comment == "")
            {
                historyRemark = "Goods order withdrawn successfully.";
            }
            else
            {
                historyRemark = request.comment;
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, request.id, 0, 0, request.comment, CMMS.CMMS_Status.GO_APPROVED);

            CMGOMaster _GOList = await GetGODetailsByID(request.id, facilitytimeZone);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_APPROVED, new[] { userId }, _GOList);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Goods Order {request.id} approved successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectGoodsOrder(CMApproval request, int userId, string facilitytimeZone)
        {

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }


            string approveQuery = $"Update smgoodsorder set status = {(int)CMMS.CMMS_Status.GO_REJECTED} , remarks = '{request.comment}' , " +
                $" rejected_by = {userId}, rejectedOn = '{DateTime.Now.ToString("yyyy-MM-dd")}'" +
                $" where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, request.id, 0, 0, request.comment, CMMS.CMMS_Status.GO_REJECTED);

            CMGOMaster _GOList = await GetGODetailsByID(request.id, facilitytimeZone);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_REJECTED, new[] { userId }, _GOList);
            /*string myQuery = $"SELECT   ID ,facilityID ,vendorID ,receiverID ,generated_by ,purchaseDate ,orderDate ,challan_no ,po_no ,\r\n     " +
       $" freight ,transport ,no_pkg_received ,lr_no ,\r\n      " +
       $"condition_pkg_received ,vehicle_no ,gir_no ,challan_date ,po_date ,\r\n      " +
       $"job_ref ,amount ,currency as currencyID,status ,\r\n     " +
       $" lastModifiedDate ,generate_flag ,s2s_generated_by ,case when received_on = '0000-00-00 00:00:00' then null else received_on end as received_on ,invoice_copy ,issued_by ,issued_on ,\r\n    " +
       $"  approved_by ,approvedOn ,withdraw_by ,withdrawOn ,reorder_flag ,\r\n     " +
       $" order_type ,remarks ,updated_by ,updatedOn ,rejected_by ,rejectedOn" +
       $" \r\nFROM smgoodsorder where id = {request.id}";
            List<CMGoodsOrderList> _WCList = await Context.GetData<CMGoodsOrderList>(myQuery).ConfigureAwait(false);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_REJECTED, new[] { userId }, _WCList);*/



            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Goods Order {request.id} rejected successfully.");

            return response;
        }

        // Get order Item Lists
        public async Task<List<CMGoodsOrderList>> getPurchaseDetailsByID(int id)
        {
            string query = "";

            query = "SELECT fc.name as facilityName,pod.ID as podID,pod.spare_status,pod.remarks,sam.asset_type_ID,pod.purchaseID,assetItemID,sam.asset_code," +
                " pod.cost,pod.ordered_qty, bl.name as vendor_name, po.facilityID as facility_id,po.purchaseDate,sam.asset_type_ID," +
                " sam.asset_name,po.receiverID, po.vendorID,po.status,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty, " +
                " pod.accepted_qty,pod.requested_qty,f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by," +
                " pod.order_type, receive_later, added_to_store, po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, " +
                " po.lr_no, po.condition_pkg_received, po.vehicle_no, po.gir_no, po.challan_date,  po.job_ref, po.amount, po.currency as currencyID ," +
                " curr.name as currency ,pod.paid_by_ID ,pod.is_splited      " +
                " FROM smgoodsorderdetails pod  LEFT JOIN smgoodsorder po ON po.ID = pod.purchaseID     " +
                " LEFT JOIN smassetmasters sam ON sam.id = pod.assetItemID       " +
                " LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement       " +
                " LEFT JOIN ( SELECT file.file_path,file.Asset_master_id as Asset_master_id FROM " +
                " smassetmasterfiles file LEFT join smassetmasters sam on file.Asset_master_id =  sam.id ) f1 ON f1.Asset_master_id = sam.id" +
                " LEFT JOIN ( SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat " +
                " LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID )  t1 ON t1.master_ID = sam.ID  " +
                " LEFT JOIN ( SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic " +
                " LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID )  t2 ON t2.master_ID = sam.ID " +
                " LEFT JOIN facilities fc ON fc.id = po.facilityID  LEFT JOIN business bl ON bl.id = po.vendorID  " +
                " LEFT JOIN currency curr ON curr.id = po.currency " +
                " WHERE po.ID = " + id + "";

            List<CMGoodsOrderList> _List = await Context.GetData<CMGoodsOrderList>(query).ConfigureAwait(false);
            return _List;
        }


        public async Task<bool> TransactionDetails(int facilityID, int fromActorID, int fromActorType, int toActorID, int toActorType, int assetItemID, double qty, int refType, int refID, string remarks, int mrsID = 0, int natureOfTransaction = 0, int assetItemStatus = 0, int userId = 0)
        {
            try
            {
                string stmt = "INSERT INTO smtransactiondetails (plantID,fromActorID,fromActorType,toActorID,toActorType,assetItemID,qty,referedby,reference_ID,remarks,Nature_Of_Transaction,Asset_Item_Status)" +
                              $"VALUES ({facilityID},{fromActorID},{fromActorType},{toActorID},{toActorType},{assetItemID},{qty},{refType},{refID},'{remarks}',{natureOfTransaction},{assetItemStatus}) ; SELECT LAST_INSERT_ID();";

                DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
                int transaction_ID = 0;

                if (dt2.Rows.Count > 0)
                {
                    transaction_ID = Convert.ToInt32(dt2.Rows[0][0]);
                    int debitTransactionID = await DebitTransation(transaction_ID, fromActorID, fromActorType, qty, assetItemID, refID, facilityID, userId);
                    int creditTransactionID = await CreditTransation(transaction_ID, toActorID, toActorType, qty, assetItemID, refID, facilityID, userId);
                    bool isValid = await VerifyTransactionDetails(transaction_ID, debitTransactionID, creditTransactionID, facilityID, fromActorID, fromActorType, toActorID, toActorType, assetItemID, qty, refType, refID, remarks, mrsID);
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


        public async Task<int> updateGOType(int typeID, int goid)
        {
            string stmt = "UPDATE smgoodsorderdetails set order_type='" + typeID + "', added_to_store = 1 where ID='" + goid + "'";
            var result = await Context.ExecuteNonQry<int>(stmt);
            return result;
        }

        public async Task<int> updateAssetStatus(int assetItemID, int status)
        {

            string stmt = $"SELECT ifnull(sam.asset_type_ID,0) asset_type_ID FROM smassetitems sai " +
                $"LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code" +
                $" WHERE sai.ID = {assetItemID}";
            DataTable dt = await Context.FetchData(stmt).ConfigureAwait(false);
            int asset_type_ID = 0;
            if (dt.Rows.Count > 0)
            {
                asset_type_ID = Convert.ToInt32(dt.Rows[0][0]);
            }

            if (asset_type_ID > 1)
            {
                string stmtUpdate = $"UPDATE smassetitems SET status = {status} WHERE ID = {assetItemID}";
                var result = await Context.ExecuteNonQry<int>(stmt);
            }
            return 1;
        }

        private async Task<int> DebitTransation(int transactionID, int actorID, int actorType, double debitQty, int assetItemID, int mrsID, int facilityID, int createdBy)
        {
            string stmt = $"INSERT INTO smtransition (transactionID, actorType, actorID, debitQty, assetItemID, goID, facilityID, createdBy) VALUES ({transactionID}, '{actorType}', {actorID}, {debitQty}, {assetItemID}, {mrsID}, {facilityID},{createdBy}); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            return id;
        }

        private async Task<int> CreditTransation(int transactionID, int actorID, int actorType, double qty, int assetItemID, int mrsID, int facilityID, int createdBy)
        {
            string query = $"INSERT INTO smtransition (transactionID,actorType,actorID,creditQty,assetItemID,goID, facilityID,createdBy) VALUES ({transactionID},'{actorType}',{actorID},{qty},{assetItemID},{mrsID}, {facilityID},{createdBy}) ; SELECT LAST_INSERT_ID();";
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

        // Get Goods Order List Data

        public async Task<List<CMPURCHASEDATA>> GetGoodsOrderData(int plantID, string empRole, DateTime fromDate, DateTime toDate, string status, string order_type, string facilitytimeZone)
        {
            var stmt = $"SELECT fc.Name as facilityName, po.ID as orderID,po.purchaseDate,po.generate_flag,case when po.received_on = '0000-00-00 00:00:00' then null else po.received_on end as received_on,po.status,bl.name as vendor_name,po.vendorID," +
                $"ed.id as generatedByID,po.remarks, CONCAT(ed.firstName,' ',ed.lastName) as generatedBy, CONCAT(ed1.firstName,' ',ed1.lastName) as receivedOn, DATE_FORMAT(po.lastModifiedDate,'%Y-%m-%d') as receivedDate," +
                $" CONCAT(ed2.Firstname,' ',ed2.lastname) as approvedBy,   DATE_FORMAT(po.approvedOn,'%Y-%m-%d') as approvedOn,    po.status as statusFlag " +
                //$" CASE WHEN po.flag = {GO_SAVE_BY_PURCHASE_MANAGER} THEN 'Draft' WHEN po.flag = {GO_SUBMIT_BY_PURCHASE_MANAGER} THEN 'Submitted' " +
                //$"WHEN po.flag = {GO_SAVE_BY_STORE_KEEPER} THEN 'In Process' ELSE 'Received' END" +

                $"FROM smgoodsorder po " +
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
            foreach (var list in _List)
            {
                if (list != null && list.purchaseDate != null)
                    list.purchaseDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.purchaseDate);
                if (list != null && list.receivedDate != null)
                    list.received_on = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.received_on);
            }
            return _List;
        }


        public async Task<CMDefaultResponse> SubmitPurchaseData(CMSUBMITPURCHASEDATA request,int userId, string facilitytimeZone)
        {
            try
            {
                
                int purchaseId = 0;
                

                purchaseId = request.purchaseID;

                // Update the Goods Order.
                var stmtUpdateP = $"UPDATE smgoodsorder SET status = {(int)CMMS.CMMS_Status.GO_SUBMITTED} WHERE ID = {purchaseId}";
                var result = await Context.ExecuteNonQry<int>(stmtUpdateP);

                // Delete the existing Goods Order details.
                var stmtDelete = $"DELETE FROM smgoodsorderdetails WHERE purchaseID = {purchaseId}";
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
                                var stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,item_condition,status) VALUES ({request.facilityId},'{assetCode}',1,0); SELECT LAST_INSERT_ID();";
                                DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                                assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);
                                //assetItemIDByCode[assetCode] = assetItemId;
                                stmtI = "";
                                stmtI = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
                                            $"VALUES({purchaseId},{assetItemId},{item.type},{item.cost},1,0); SELECT LAST_INSERT_ID();";
                                DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                                purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                            }
                            else
                            {
                                // Insert the asset item.
                                var stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,item_condition,status) VALUES ({request.facilityId},'{assetCode}',1,0); SELECT LAST_INSERT_ID();";
                                DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                                assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);
                                //assetItemIDByCode[assetCode] = assetItemId;
                                stmtI = "";
                                stmtI = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
                                            $"VALUES({purchaseId},{assetItemId},{item.type},{item.cost},0,0); SELECT LAST_INSERT_ID();";
                                DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                                purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                            }
                        }
                        else
                        {
                            string stmtI = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
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
                            string stmtI = $"INSERT INTO smgoodsorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID)" +
                                        $"VALUES({purchaseId},{item.assetItemID},{item.type},{item.cost},0,0); SELECT LAST_INSERT_ID();";
                            DataTable dtInsertOD = await Context.FetchData(stmtI).ConfigureAwait(false);
                            purchaseOrderDetailsID = Convert.ToInt32(dtInsertOD.Rows[0][0]);
                        }


                    }
                }
                string historyRemark = "";
                if (request.remarks == null || request.remarks == "")
                {
                    historyRemark = "Goods order submitted";
                }
                else
                {
                    historyRemark = request.remarks;
                }
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, purchaseId, 0, 0, historyRemark, CMMS.CMMS_Status.GO_SUBMITTED);
                CMGOMaster _GOList = await GetGODetailsByID(request.purchaseID, facilitytimeZone);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_SUBMITTED, new[] { userId }, _GOList);
                CMDefaultResponse response = new CMDefaultResponse(purchaseId, CMMS.RETRUNSTATUS.SUCCESS, "Goods order submitted successfully.");
                return response;
            }
            catch (Exception e)
            {
                CMDefaultResponse response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.SUCCESS, "Goods order failed to submit.");
                return response;
            }
        }

        public async Task<int> GoodsOrderAsync(int facilityId, int vendor, int empId, DateTime? purchaseDate, int generateFlag)
        {
            int purchaseID = 0;

            string stmtSelect = $"SELECT * FROM smgoodsorder WHERE facilityID = {facilityId}" +
                                $"AND vendorID = {vendor} AND generated_by = {empId} AND purchaseDate = '{purchaseDate.Value.ToString("yyyy-MM-dd")}' AND status = {generateFlag}";

            DataTable dt1 = await Context.FetchData(stmtSelect).ConfigureAwait(false);
            if (dt1 != null)
            {
                purchaseID = Convert.ToInt32(dt1.Rows[0]["ID"]);
                string stmtUpdate = $"UPDATE smgoodsorder SET generate_flag = {generateFlag}, status = {generateFlag}, vendorID = {vendor} WHERE ID = {purchaseID}";
                var result = await Context.ExecuteNonQry<int>(stmtUpdate);
            }
            else
            {
                string stmt = $"INSERT INTO smgoodsorder (facilityID,vendorID,generated_by,purchaseDate,status,generate_flag) " +
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
        public async Task<CMGOMaster> GetGODetailsByID(int id, string facilitytimeZone)
        {
            string query = "SELECT fc.name as facilityName, pod.ID as podID, facilityid as facility_id, pod.spare_status, pod.remarks, sai.orderflag, sam.asset_type_ID, " +
                "pod.purchaseID, pod.assetItemID, sai.serial_number, sai.location_ID, pod.cost, pod.ordered_qty, bl.name as vendor_name, " +
                "po.purchaseDate, sam.asset_type_ID, sam.asset_name, po.receiverID, po.vendorID, po.status, sam.asset_code, t1.asset_type, t2.cat_name, " +
                "pod.received_qty, pod.damaged_qty, pod.accepted_qty, f1.file_path, f1.Asset_master_id, sm.decimal_status, sm.spare_multi_selection, po.generated_by, " +
                "pod.order_type as asset_type_ID_OrderDetails, receive_later, added_to_store, po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, " +
                "po.lr_no, po.condition_pkg_received, po.vehicle_no, po.gir_no, po.challan_date, po.job_ref, po.amount, po.currency as currencyID, curr.name as currency, " +
                "stt.asset_type as asset_type_Name, po_no, po_date, requested_qty, lost_qty, ordered_qty, paid_by_ID, smpaidby.paid_by as paid_by_name, " +
                "CASE WHEN po.received_on = '0000-00-00 00:00:00' THEN NULL ELSE po.received_on END as receivedAt, sam.asset_type_ID, sam.asset_code, sam.asset_name, " +
                "sic.cat_name, smat.asset_type, pod.is_splited, pod.sr_no, requestOrderId, requestOrderItemID, freight_value, inspection_report, " +
                "(SELECT storage_rack_no FROM smrequestreorder WHERE requestID=" + id + " AND assetItemID = pod.assetItemID LIMIT 1) as storage_rack_no, " +
                "(SELECT storage_row_no FROM smrequestreorder WHERE requestID=" + id + " AND assetItemID = pod.assetItemID LIMIT 1) as storage_row_no, " +
                "(SELECT storage_column_no FROM smrequestreorder WHERE requestID=" + id + " AND assetItemID = pod.assetItemID LIMIT 1) as storage_column_no, " +
                "CONCAT(submitUser.firstname, ' ', submitUser.lastname) as submitted_by_name, po.orderDate, " +
                "CONCAT(closeUser.firstName, ' ', closeUser.lastName) as closed_by_name, po.withdrawOn, " +
                "CONCAT(rejectUser.firstName, ' ', rejectUser.lastName) as rejected_by_name, po.rejectedOn , "+
                "CONCAT(approveUser.firstName, ' ', approveUser.lastName) as approved_by_name, po.approvedOn, " +
                "CONCAT(receiveSubmitUser.firstName, ' ', receiveSubmitUser.lastName) as receive_submit_by_name, po.updatedOn, " +
                "CONCAT(receiveRejectUser.firstName, ' ', receiveRejectUser.lastName) as receive_rejected_by_name , po.receive_rejected_at, " +
                "CONCAT(receiveApproveUser.firstName, ' ', receiveApproveUser.lastName) as receive_approved_by_name, po.receive_approved_at, " +
                "CONCAT(goupdate.firstName, ' ', goupdate.lastName) as go_updated_by_name, po.go_updatedOn " +
                "FROM smgoodsorderdetails pod " +
                "LEFT JOIN smgoodsorder po ON po.ID = pod.purchaseID " +
                "LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID " +
                "LEFT JOIN smassetmasters sam ON sam.ID = pod.assetItemID " +
                "LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement " +
                "LEFT JOIN (SELECT file.file_path, file.Asset_master_id FROM smassetmasterfiles file LEFT JOIN smassetmasters sam ON file.Asset_master_id = sam.id) f1 ON f1.Asset_master_id = sam.id " +
                "LEFT JOIN (SELECT sat.asset_type, s1.ID as master_ID FROM smassettypes sat LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID) t1 ON t1.master_ID = sam.ID " +
                "LEFT JOIN (SELECT sic.cat_name, s2.ID as master_ID FROM smitemcategory sic LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID) t2 ON t2.master_ID = sam.ID " +
                "LEFT JOIN facilities fc ON fc.id = po.facilityID " +
                "LEFT JOIN users as vendor ON vendor.id = po.vendorID " +
                "LEFT JOIN business bl ON bl.id = po.vendorID " +
                "LEFT JOIN smassettypes stt ON stt.ID = pod.order_type " +
                "LEFT JOIN currency curr ON curr.id = po.currency " +
                "LEFT JOIN smpaidby as smpaidby ON smpaidby.ID = pod.paid_by_ID " +
                "LEFT JOIN smitemcategory sic ON sic.ID = sam.item_category_ID " +
                "LEFT JOIN smassettypes smat ON smat.ID = sam.asset_type_ID " +
                "LEFT JOIN users submitUser ON submitUser.id = po.generated_by " +
                "LEFT JOIN users closeUser ON closeUser.id = po.withdraw_by " +
                "LEFT JOIN users rejectUser ON rejectUser.id = po.rejected_by " +
                "LEFT JOIN users approveUser ON approveUser.id = po.approved_by " +
                "LEFT JOIN users receiveSubmitUser ON receiveSubmitUser.id = po.updated_by " +
                "LEFT JOIN users receiveRejectUser ON receiveRejectUser.id = po.receive_rejected_by_id " +
                "LEFT JOIN users receiveApproveUser ON receiveApproveUser.id = po.receive_approved_by_id " +
                "LEFT JOIN users goupdate ON goupdate.id = po.go_updated_by " +
                "WHERE po.ID =" + id + " and pod.is_splited = 1 /*GROUP BY pod.ID*/";

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
                freight_value = p.freight_value,
                inspection_report = p.inspection_report,
                approved_by_name = p.approved_by_name,
                receive_rejected_by_name = p.receive_rejected_by_name,
                drafted_by_name = p.drafted_by_name,
                closed_by_name = p.closed_by_name,
                receive_approved_by_name = p.receive_approved_by_name,
                rejected_by_name = p.rejected_by_name,
                receive_submitted_by_name = p.receive_submit_by_name,
                submitted_by_name = p.submitted_by_name,
                rejected_at = p.rejectedOn,
                submitted_at = p.orderDate,
                closed_at = p.withdrawOn,
                approved_at = p.approvedOn,
                receive_submitted_at = p.updatedOn,
                receive_rejected_at = p.receive_rejected_at,
                receive_approved_at = p.receive_approved_at,
                go_updated_by_name = p.go_updated_by_name,
                go_updatedOn = p.go_updatedOn



            }).FirstOrDefault();
            if (_List.Count > 0)
            {
                List<CMGODetails> _itemList = _List.Select(p => new CMGODetails
                {
                    id = p.podID,
                    cost = p.cost,
                    assetItem_Name = p.asset_name,
                    assetMasterItemID = p.assetItemID,
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
                    asset_type = p.asset_type,
                    is_splited = p.is_splited,
                    requestOrderId = p.requestOrderId,
                    requestOrderItemID = p.requestOrderItemID,
                    sr_no = p.sr_no,
                    storage_column_no = p.storage_column_no,
                    storage_row_no = p.storage_row_no,
                    storage_rack_no = p.storage_rack_no,
                }).ToList();
                _MasterList.GODetails = _itemList;

                //CMGOMaster m_GOObj = new CMGOMaster();
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_MasterList.status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_GO, _Status);
                string _longStatus = getLongStatus(_Status, _MasterList.Id, _MasterList);

                _MasterList.status_short = _shortStatus;
                _MasterList.status_long = _longStatus;
            }

            return _MasterList;
        }

        internal async Task<List<CMGOListByFilter>> GetGOList(int facility_id, DateTime fromDate, DateTime toDate, int is_purchaseorder, string facilitytimeZone)
        {

            string filter = " (DATE(po.po_date) >= '" + fromDate.ToString("yyyy-MM-dd") + "'  and DATE(po.po_date) <= '" + toDate.ToString("yyyy-MM-dd") + "')";

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
                "pod.purchaseID,pod.assetItemID,sai.serial_number,sai.location_ID,(select sum(cost) from smgoodsorderdetails where purchaseID = po.id) as cost,pod.ordered_qty,\r\n bl.name as vendor_name,\r\n     " +
                " po.purchaseDate,sam.asset_type_ID,sam.asset_name,po.receiverID,\r\n        " +
                "po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty," +
                "f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by,pod.order_type as asset_type_ID_OrderDetails, receive_later, " +
                "added_to_store,   \r\n      " +
                "  po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, po.lr_no, po.condition_pkg_received, " +
                "po.vehicle_no, po.gir_no, po.challan_date, po.job_ref, po.amount,  po.currency as currencyID , curr.name as currency , stt.asset_type as asset_type_Name,  po_no, po_date, requested_qty,lost_qty, ordered_qty, CONCAT(ed.firstName,' ',ed.lastName) as generatedBy\r\n  ,case when po.received_on = '0000-00-00 00:00:00' then null else po.received_on end as receivedAt    " +
                "  FROM smgoodsorderdetails pod\r\n        LEFT JOIN smgoodsorder po ON po.ID = pod.purchaseID\r\n     " +
                "   LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID\r\n       " +
                " LEFT JOIN smassetmasters sam ON sam.ID = pod.assetItemID\r\n      " +
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
                " WHERE " + filter + "";


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
                CMGOMaster m_GOObj = new CMGOMaster();
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_MasterList[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_GO, _Status);
                string _longStatus = getLongStatus(_Status, _MasterList[i].Id, m_GOObj);

                _MasterList[i].status_short = _shortStatus;
                _MasterList[i].status_long = _longStatus;
            }
            foreach (var list in _MasterList)
            {
                if (list != null && list.challan_date != null)
                    list.challan_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.challan_date);
                if (list != null && list.po_date != null)
                    list.po_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.po_date);
                if (list != null && list.receivedAt != null)
                    list.receivedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)list.receivedAt);


            }

            return _MasterList;
        }


        internal async Task<CMDefaultResponse> UpdateGOReceive(CMGoodsOrderList request, int userID, string facilitytimeZone)
        {
            string OrderQuery = "";
            string received_date = (request.receivedAt == null) ? "0001-01-01 00:00:00" : ((DateTime)request.receivedAt).ToString("yyyy-MM-dd HH:mm:ss");
            if (request.is_submit == 0)
            {

                OrderQuery = $"UPDATE smgoodsorder SET " +
                          $"challan_no = '{request.challan_no}',po_no='{request.po_no}', freight='{request.freight}',no_pkg_received='{request.no_pkg_received}'," +
                          $"lr_no='{request.lr_no}',condition_pkg_received='{request.condition_pkg_received}',vehicle_no='{request.vehicle_no}', gir_no='{request.gir_no}', " +
                          $"challan_date = '{request.challan_date.Value.ToString("yyyy-MM-dd")}', " +
                          $"job_ref='{request.job_ref}',amount='{request.amount}', currency={request.currencyID},updated_by = {userID},updatedOn = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.GO_RECEIVE_DRAFT}," +
                          $" received_on = '{received_date}', po_date = '{request.po_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', purchaseDate= '{request.purchaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}', freight_value='{request.freight_value}', inspection_report='{request.inspection_report}'" +
                          $"  where ID={request.id}";
            }
            else
            {
                OrderQuery = $"UPDATE smgoodsorder SET " +
                        $"challan_no = '{request.challan_no}',po_no='{request.po_no}', freight='{request.freight}',no_pkg_received='{request.no_pkg_received}'," +
                        $"lr_no='{request.lr_no}',condition_pkg_received='{request.condition_pkg_received}',vehicle_no='{request.vehicle_no}', gir_no='{request.gir_no}', " +
                        $"challan_date = '{request.challan_date.Value.ToString("yyyy-MM-dd")}', " +
                        $"job_ref='{request.job_ref}',amount='{request.amount}', currency={request.currencyID},updated_by = {userID},updatedOn = '{UtilsRepository.GetUTCTime()}', status = {(int)CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED}," +
                        $" received_on = '{received_date}', po_date = '{request.po_date.Value.ToString("yyyy-MM-dd HH:mm:ss")}', purchaseDate= '{request.purchaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}',  freight_value='{request.freight_value}', inspection_report='{request.inspection_report}'" +
                        $"  where ID={request.id}";
            }
            await Context.ExecuteNonQry<int>(OrderQuery);

            for (var i = 0; i < request.go_items.Count; i++)
            {
                string itemsQuery = $"UPDATE smgoodsorderdetails SET location_ID = {request.location_ID},assetItemID = {request.go_items[i].assetMasterItemID},cost = {request.go_items[i].cost}, accepted_qty = {request.go_items[i].accepted_qty}, received_qty= {request.go_items[i].received_qty},lost_qty = {request.go_items[i].lost_qty}, damaged_qty={request.go_items[i].damaged_qty}," +
                    $" sr_no = '{request.go_items[i].sr_no}' WHERE ID = {request.go_items[i].goItemID} ;";
                itemsQuery = itemsQuery + $"update smassetitems set serial_number = '{request.go_items[i].sr_no}' where materialID = {request.go_items[i].assetMasterItemID}";
                var result = await Context.ExecuteNonQry<int>(itemsQuery);
            }
            // Assuming request.go_items is a list of items to insert into smrequestreorder

            foreach (var item in request.go_items)
            {
                string insertQuery = $@"INSERT INTO smrequestreorder 
                            (requestID, assetItemID, location_ID, storage_rack_no, storage_row_no, storage_column_no, cost, ordered_qty, received_qty, lost_qty, requested_qty, damaged_qty, accepted_qty, remarks)
                            VALUES 
                            ({request.id}, {item.assetMasterItemID}, {request.location_ID}, '{item.storage_rack_no}', '{item.storage_row_no}', '{item.storage_column_no}', {item.cost}, {item.ordered_qty}, {item.received_qty}, {item.lost_qty}, {item.requested_qty}, {item.damaged_qty}, {item.accepted_qty},  '{item.remarks}')";

                // Execute the query
                var result = await Context.ExecuteNonQry<int>(insertQuery);
            }
            CMGOMaster _GOList = await GetGODetailsByID(request.id, facilitytimeZone);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED, new[] { userID }, _GOList);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Goods order updated successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> ApproveGoodsOrderReceive(CMApproval request, int userId, string facilitytimeZone)
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

            string UpdatesqlQ = $" UPDATE smgoodsorder SET receive_approved_by_id = {userId}, status = {(int)CMMS.CMMS_Status.GO_RECEIVED_APPROVED}, remarks = '{request.comment}',receive_approved_at = '{DateTime.Now.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(UpdatesqlQ).ConfigureAwait(false);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;



            // Entry in TransactionDetails
            var data = await this.getPurchaseDetailsByID(request.id);

            for (int i = 0; i < data.Count; i++)
            {
                // Check if it is spare/ consumable ,  aacording to this split items in transaction details
                if (data[i].asset_type_ID == (int)CMMS.SM_AssetTypes.Spare && data[i].is_splited == 1)
                {
                    for (var j = 0; j < data[i].ordered_qty; j++)
                    {
                        decimal stock_qty = 1;
                        var tResult = await TransactionDetails(data[i].facility_id, data[i].vendorID, (int)CMMS.SM_Actor_Types.Vendor, data[i].facility_id, (int)CMMS.SM_Actor_Types.Store, data[i].assetItemID, (double)stock_qty, (int)CMMS.CMMS_Modules.SM_GO, request.id, "Goods Order", userId);

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
                else
                {
                    if (data[i].added_to_store == 0 && data[i].is_splited == 1)
                    {

                        //decimal stock_qty = data[i].ordered_qty + data[i].received_qty;
                        decimal stock_qty = data[i].received_qty;
                        var tResult = await TransactionDetails(data[i].facility_id, data[i].vendorID, (int)CMMS.SM_Actor_Types.Vendor, data[i].facility_id, (int)CMMS.SM_Actor_Types.Store, data[i].assetItemID, (double)stock_qty, (int)CMMS.CMMS_Modules.SM_GO, request.id, "Goods Order");

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
            }

            //for (int i = 0; i < data.Count; i++)
            //{
            //    if (data[i].added_to_store == 0)
            //    {

            //        decimal stock_qty = data[i].ordered_qty + data[i].received_qty;
            //        var tResult = await TransactionDetails(data[i].facility_id, data[i].vendorID, (int)CMMS.SM_Actor_Types.Vendor, data[i].facility_id, (int)CMMS.SM_Actor_Types.Store, data[i].assetItemID, (double)stock_qty, (int)CMMS.CMMS_Modules.SM_GO, request.id, "Goods Order");

            //        // Update the order type.
            //        var update_order_type = await updateGOType(data[i].order_by_type, data[i].id);

            //        // Update the asset status.
            //        if (data[i].spare_status == 2)
            //        {
            //            await updateAssetStatus(data[i].assetItemID, 4);
            //        }
            //        else
            //        {
            //            await updateAssetStatus(data[i].assetItemID, 1);
            //        }
            //    }
            //}
            
            string historyRemark = "";
            if (request.comment == null || request.comment == "")
            {
                historyRemark = "Goods order receive approved.";
            }
            else
            {
                historyRemark = request.comment;
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, request.id, 0, 0, historyRemark, CMMS.CMMS_Status.GO_RECEIVED_APPROVED);
            CMGOMaster _GOList = await GetGODetailsByID(request.id, facilitytimeZone);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_RECEIVED_APPROVED, new[] { userId }, _GOList);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Goods order receive {request.id} approved successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectGoodsOrderReceive(CMApproval request, int userId, string facilitytimeZone)
        {

            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }


            string approveQuery = $"Update smgoodsorder set status = {(int)CMMS.CMMS_Status.GO_RECEIVED_REJECTED} , remarks = '{request.comment}' , " +
                $" receive_rejected_by_id = {userId}, receive_rejected_at = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                $" where id = {request.id}"; 
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }
            string historyRemark = "";
            if (request.comment == null || request.comment == "")
            {
                historyRemark = "Rejected goods order receive.";
            }
            else
            {
                historyRemark = request.comment;
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, request.id, 0, 0, historyRemark, CMMS.CMMS_Status.GO_RECEIVED_REJECTED);
            CMGOMaster _GOList = await GetGODetailsByID(request.id, facilitytimeZone);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_RECEIVED_REJECTED, new[] { userId }, _GOList);




              /*string myQuery = $"SELECT   ID ,facilityID ,vendorID ,receiverID ,generated_by ,purchaseDate ,orderDate ,challan_no ,po_no ,\r\n     " +
         $" freight ,transport ,no_pkg_received ,lr_no ,\r\n      " +
         $"condition_pkg_received ,vehicle_no ,gir_no ,challan_date ,po_date ,\r\n      " +
         $"job_ref ,amount ,currency as currencyID,status ,\r\n     " +
         $" lastModifiedDate ,generate_flag ,s2s_generated_by ,received_on ,invoice_copy ,issued_by ,issued_on ,\r\n    " +
         $"  approved_by ,approvedOn ,withdraw_by ,withdrawOn ,reorder_flag ,\r\n     " +
         $" order_type ,remarks ,updated_by ,updatedOn ,rejected_by ,rejectedOn" +
         $" \r\nFROM smgoodsorder where id = {request.id}";
              List<CMGoodsOrderList> _WCList = await Context.GetData<CMGoodsOrderList>(myQuery).ConfigureAwait(false);
              await CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_GO, CMMS.CMMS_Status.GO_RECEIVED_REJECTED,new[] { userId }, _WCList);*/


              CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Goods order receive {request.id} rejected successfully.");


            return response;
        }

        internal async Task<CMDefaultResponse> CloseRO(CMGoodsOrderList request, int userID)
        {
            string mainQuery = $"UPDATE smgoodsorder SET withdraw_by = " + userID + ", status = " + (int)CMMS.CMMS_Status.SM_RO_CLOSED + ", remarks = '" + request.remarks + "', withdrawOn = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE ID = " + request.id + "";
            await Context.ExecuteNonQry<int>(mainQuery);

            string historyRemark = "";
            if (request.remarks == null || request.remarks == "")
            {
                historyRemark = "Goods order withdrawn successfully.";
            }
            else
            {
                historyRemark = request.remarks;
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_GO, request.id, 0, 0, request.remarks, CMMS.CMMS_Status.SM_RO_CLOSED);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Goods order withdrawn successfully.");
            return response;
        }

    }
}
