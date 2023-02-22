using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Repositories.WC
{
    public class WCRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public WCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<List<CMWCList>> GetWCList(int facilityId, string startDate, string endDate, int statusId)
        {
            /*
             * Tables
             * WC - Primary Table
             * WCSchedules - Supplier Action record
             * Facility - Facility Related data
             * AssetCategories - Asset Category Name
             * Assets - Asset Name
             * Users - User related info
             * Business - Supplier info
             * Fetch all data present in CMWCList Model
            */
            /*Your code goes here*/
            string myQuery = "SELECT  wc.id as wc_id, wc.facilityID as facility_Id, f.name as facility_name, ac.name AS equipment_category, a.name AS equipment_name, equipment_sr_no," +

                "b1.name AS supplier_name, good_order_id, affected_part, order_reference_number, affected_sr_no, cost_of_replacement, wc.currency," +

                " warranty_start_date, warranty_end_date, warranty_claim_title, warranty_description, " +

                "corrective_action_by_buyer, request_to_supplier, concat(user.firstName , ' ' , user.lastName) AS approver_name," +

                " created_by, issued_on, wc.status, approved_by, wc_fac_code, failure_time " +

                " FROM wc " +
                "" +
                "JOIN facilities as f ON f.id = wc.facilityId " +
                "" +
                "JOIN assets as a ON a.id = wc.equipment_id " +
                "" +
                "JOIN business as b1 ON b1.id = wc.supplier_id " +
                "" +
                "JOIN assetcategories AS ac ON ac.id = wc.equipment_cat_id  " +
                "" +
                "JOIN users as user ON user.id = wc.approver_id";
            if (facilityId != 0)
            {
                myQuery += " WHERE wc.facilityId = " + facilityId ;
                //if (startDate.Length() > 0 && endDate.Length > 0)
                //{
                //    myQuery += " AND failure_time >= " + startDate + " AND " +
                //           "failure_time <= " + endDate;
                //}
                if(statusId > 0)
                {
                    myQuery += " AND wc.status  = " + statusId;

                }
            }
            else
            {
                throw new ArgumentException("Invalid argument facilityId<" + facilityId + ">");
            }
            List<CMWCList> GetWCData = await Context.GetData<CMWCList>(myQuery).ConfigureAwait(false);
            return GetWCData;
        }

        internal async Task<CMDefaultResponse> CreateWC(List<CMWCCreate> set)
        {
            /*
             * Insert all data in WC table in their respective columns 
            */
            int count = 0;
            int userID = UtilsRepository.GetUserID();
            int retID = 0;
            string wcTitle = "";
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            string strRetMessage = "";

            string qry = "insert into wc(facilityId, equipment_cat_id, equipment_id, equipment_sr_no, supplier_id, good_order_id, affected_part, order_reference_number, affected_sr_no, cost_of_replacement, currencyId, warranty_start_date, warranty_end_date, warranty_claim_title, warranty_description, corrective_action_by_buyer, request_to_supplier, approver_id, status, wc_fac_code, failure_time, created_by) values";

            foreach (var unit in set)
            {
                count++;
                wcTitle = unit.warrantyClaimTitle;
                
                qry += "('" + unit.facilityId + "','" + unit.equipmentCategoryId + "','" + unit.equipmentId + "','" + unit.equipmentSrNo + "','" + unit.manufacturerId + "','" + unit.goodsOrderId + "','" + unit.affectedPart + "','" + unit.orderReference + "','" + unit.affectedSrNo + "','" + unit.costOfReplacement + "','" + unit.currencyId + "','" + unit.warrantyStartAt.ToString("yyyy-MM-dd") + "','" + unit.warrantyEndAt.ToString("yyyy-MM-dd") + "','" + unit.warrantyClaimTitle + "','" + unit.warrantyDescription + "','" + unit.correctiveActionByBuyer + "','" + unit.requestToSupplier + "','" + unit.approverId + "','" + unit.status + "','" + unit.wcFacCode + "','" + unit.failureTime.ToString("yyyy-MM-dd")  + "'," + userID + "),";
            }

            if (count > 0)
            {
                qry = qry.Substring(0, (qry.Length - 1)) + ";" + "select LAST_INSERT_ID(); ";

                //List<CMInventoryList> newInventory = await Context.GetData<CMInventoryList>(qry).ConfigureAwait(false);
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                retID = Convert.ToInt32(dt.Rows[0][0]);
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
                if (count == 1)
                {
                    strRetMessage = "New warranty claim <" + wcTitle + "> added";
                }
                else
                {
                    strRetMessage = "<" + count + "> new warranty claims added";
                }
            }
            else
            {
                strRetMessage = "No warranty claim to add";
            }
            return new CMDefaultResponse(retID, retCode, strRetMessage);
        }

        internal async Task<CMWCDetail> GetWCDetails(int id)  
        {
            /*
             * Tables
             * WC - Primary Table
             * WCSchedules - Supplier Action record
             * Facility - Facility Related data
             * AssetCategories - Asset Category Name
             * Assets - Asset Name
             * Users - User related info
             * Business - Supplier info
             * History - Action Logs
             * Fetch all data present in CMWCDetail Model
            */
            if(id <= 0)
            {
                throw new ArgumentException("Invalid argument. id<" + id + "> cannot be 0");
            }

            string myQuery = "SELECT  wc.id as wc_id, wc.facilityId as facility_Id, f.name as facility_name, ac.name AS equipment_category, a.name AS equipment_name, equipment_sr_no," +

            "b1.name AS supplier_name, good_order_id, affected_part, order_reference_number, affected_sr_no, cost_of_replacement,  wc.currency," + // add cost_of_replacement,

            " warranty_start_date, warranty_end_date, warranty_claim_title, warranty_description, " +

            "corrective_action_by_buyer, request_to_supplier, concat(user.firstName , ' ' , user.lastName) AS approver_name," +

            " created_by, issued_on, wc.status, approved_by, wc_fac_code, failure_time " +

            " FROM wc " +
            "" +
            "JOIN facilities as f ON f.id = wc.facilityId " +
            "" +
            "JOIN assets as a ON a.id = wc.equipment_id " +
            "" +
            "JOIN business as b1 ON b1.id = wc.supplier_id " +
            "" +
            "JOIN assetcategories AS ac ON ac.id = wc.equipment_cat_id  " +
            "" +
            "JOIN users as user ON user.id = wc.approver_id";

            if (id != 0)
            {
                myQuery += " WHERE wc.id= " + id;

            }
            List<CMWCDetail> GetWCDetails = await Context.GetData<CMWCDetail>(myQuery).ConfigureAwait(false);
            return GetWCDetails[0];
        }

           
        internal async Task<CMDefaultResponse> UpdateWC(CMWCCreate request)
        {
            /*
             * Insert all data in WC table in their respective columns 
            */
            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }
            string updateQry = "UPDATE wc SET ";
            return null;
        }

        internal async Task<CMDefaultResponse> ApproveWC(CMApproval request)
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
            int userId = Utils.UtilsRepository.GetUserID();
            string approveQuery = $"Update wc set status = {(int)CMMS.CMMS_Status.APPROVED}, approval_reccomendations = '{request.comment}'  " +
                " approveddBy = {userId}, approvedAt = {Utils.UtilsRepository.GetUTCTime}" +
                " where id = { request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;

            string myQuery = "SELECT * from wc where id = {request.id}";
            List<CMWCDetail> _WCList = await Context.GetData<CMWCDetail>(myQuery).ConfigureAwait(false);
            CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.APPROVED, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Approved warranty claim Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectWC(CMApproval request)
        {
            /*
             * Update the Incidents and also update the history table
             * check create function for history update
             * Your code goes here
            */
            if (request.id <= 0)
            {
                throw new ArgumentException("Invalid argument id<" + request.id + ">");
            }

            int userId = Utils.UtilsRepository.GetUserID();
            string approveQuery = $"Update wc set status = {(int)CMMS.CMMS_Status.REJECTED} , reject_reccomendations = '{request.comment}'  " + 
                " rejecctedBy = {userId}, rejectedAt = {Utils.UtilsRepository.GetUTCTime}" + 
                " where id = { request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, request.id, 0, 0, "Rejected warranty claim", CMMS.CMMS_Status.REJECTED);


            string myQuery = "SELECT * from wc where id = {request.id}";
            List<CMWCDetail> _WCList = await Context.GetData<CMWCDetail>(myQuery).ConfigureAwait(false);
            CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.REJECTED, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Rejected warranty claim Successfully");
            return response;
        }
    }
}
