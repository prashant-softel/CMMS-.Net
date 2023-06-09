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
        private Dictionary<int, string> StatusDictionary = new Dictionary<int, string>() {
            { 191, "In Draft" },
         //   { 192, "Warranty Claim Created" },
            { 192, "Waiting for Submit Approval" },
            { 193, "Submit Request Rejected" },
            { 194, "Submitted" },
            { 195, "Asset Dispatched" },
            { 196, "Rejected By Manufacturer" },
            { 197, "Approved By Manufacturer" },
            { 198, "Item Replenished" },
        //    { 199, "Warranty Claim Waiting for Close Approval" },
            { 199, "Close Request Rejected" },
            { 200, "Claim Closed" },
            { 201, "Cancelled" }
        };
        public WCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;
            retValue = "";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.WC_DRAFT:
                    retValue = "Draft"; break;
                case CMMS.CMMS_Status.WC_SUBMITTED:
                    retValue = "Submitted"; break;
                case CMMS.CMMS_Status.WC_SUBMIT_REJECTED:
                    retValue = "Submit Rejected"; break;
                case CMMS.CMMS_Status.WC_SUBMIT_APPROVED:
                    retValue = "Submit Approved"; break;
                case CMMS.CMMS_Status.WC_DISPATCHED:
                    retValue = "Dispatched"; break; 
                case CMMS.CMMS_Status.WC_REJECTED_BY_MANUFACTURER:
                    retValue = "Rejected By Manufacturer"; break;
                case CMMS.CMMS_Status.WC_APPROVED_BY_MANUFACTURER:
                    retValue = "Approved By Manufacturer"; break;
                case CMMS.CMMS_Status.WC_ITEM_REPLENISHED:
                    retValue = "Item Replenished"; break;
                case CMMS.CMMS_Status.WC_CLOSE_REJECTED:
                    retValue = "Close Rejected"; break;
                case CMMS.CMMS_Status.WC_CLOSE_APPROVED:
                    retValue = "Closed"; break;
                case CMMS.CMMS_Status.WC_CANCELLED:
                    retValue = "Cancelled"; break;
                default:
                    break;
            }
            return retValue;

        }


        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMWCDetail WCObj)
        {
            string retValue = " ";


            switch (notificationID)
                {
                case CMMS.CMMS_Status.WC_DRAFT:
                    retValue = String.Format("Warranty Claim in Draft by {0}", WCObj.created_by);
                    break;
                case CMMS.CMMS_Status.WC_SUBMITTED:
                    retValue = String.Format("Warranty Claim submitted by {0}", WCObj.created_by);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_REJECTED:
                    retValue = String.Format("Warranty Claim  Submit Rejected by {0}", WCObj.approved_by);
                    break;
                case CMMS.CMMS_Status.WC_SUBMIT_APPROVED:
                    retValue = String.Format("Warranty Claim Submit Approved by {0}", WCObj.created_by);
                    break;
                case CMMS.CMMS_Status.WC_DISPATCHED:
                    //retValue = String.Format("Warranty Claim Dispachted by {0} at {1}", WCObj.dispatched_by, WCObj.dispatched_at);
                    retValue = String.Format("Warranty Claim Dispachted by {0} at {1}", WCObj.created_by, WCObj.closed_at);
                    break;
                case CMMS.CMMS_Status.WC_REJECTED_BY_MANUFACTURER:
                    retValue = String.Format("Warranty Claim Rejected by Manufacturer {0}", WCObj.created_by);
                    break;
                case CMMS.CMMS_Status.WC_APPROVED_BY_MANUFACTURER:
                    retValue = String.Format("Warranty Claim Approved by Manufacturer {0}", WCObj.approver_name);
                    break;
                case CMMS.CMMS_Status.WC_ITEM_REPLENISHED:
                    retValue = String.Format("Warranty Claim Item Replenished {0}", WCObj.created_by);
                    break;
                case CMMS.CMMS_Status.WC_CLOSE_REJECTED:
                    retValue = String.Format("Warranty Claim Close Rejected by {0}", WCObj.approver_name);
                    break;
                case CMMS.CMMS_Status.WC_CLOSE_APPROVED:
                    retValue = String.Format("Warranty Claim Close Approved by {0} at {1}", WCObj.approver_name, WCObj.closed_at);
                    break;
                case CMMS.CMMS_Status.WC_CANCELLED:
                    // retValue = String.Format("Warranty Claim Cancelled by {0} at {1}", WCObj.cancelled_by, WCObj.cancelled_at);
                    retValue = String.Format("Warranty Claim Cancelled by {0} at {1}", WCObj.approver_name, WCObj.closed_at);
                    break;
                default:
                    break;
            }
                return retValue;

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
            string statusOut = "CASE ";
            foreach(KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN wc.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";
            string myQuery = "SELECT  wc.id as wc_id, wc.facilityID as facility_Id, f.name as facility_name, ac.name AS equipment_category, a.name AS equipment_name, equipment_sr_no," +
                "b1.name AS supplier_name, good_order_id, affected_part, order_reference_number, affected_sr_no, cost_of_replacement, wc.currency," +
                " warranty_start_date, warranty_end_date, warranty_claim_title, warranty_description, " +
                "corrective_action_by_buyer, request_to_supplier, concat(user.firstName , ' ' , user.lastName) AS approver_name," +
                $" created_by, issued_on, {statusOut} as status, approved_by, wc_fac_code, failure_time " +
                " FROM wc " +
                "JOIN facilities as f ON f.id = wc.facilityId " +
                "JOIN assets as a ON a.id = wc.equipment_id " +
                "JOIN business as b1 ON b1.id = wc.supplier_id " +
                "JOIN assetcategories AS ac ON ac.id = wc.equipment_cat_id  " +
                "JOIN users as user ON user.id = wc.approver_id";
            if (facilityId > 0)
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
                throw new ArgumentException("Invalid Facility ID");
            }
            List<CMWCList> GetWCData = await Context.GetData<CMWCList>(myQuery).ConfigureAwait(false);
            return GetWCData;
        }

        internal async Task<CMDefaultResponse>  CreateWC(List<CMWCCreate> set, int userID)
        {
            /*
             * Insert all data in WC table in their respective columns 
            */
            int count = 0;
            List<int> idList = new List<int>();
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            string strRetMessage = "";

            
            foreach (CMWCCreate unit in set)
            {
                CMMS.CMMS_Status draftStatus = (CMMS.CMMS_Status)(int)CMMS.CMMS_Status.WC_DRAFT;
                if (unit.status == 1)
                {
                    draftStatus = (CMMS.CMMS_Status)(int)CMMS.CMMS_Status.WC_SUBMITTED;
                }
                string qry = "insert into wc(facilityId, equipment_id, good_order_id, affected_part, order_reference_number, affected_sr_no, " + 
                                "cost_of_replacement, currencyId, warranty_start_date, warranty_end_date, warranty_claim_title, warranty_description, " + 
                                "corrective_action_by_buyer, request_to_supplier, approver_id, status, wc_fac_code, failure_time, created_by) values" + 
                                $"({unit.facilityId}, {unit.equipmentId}, '{unit.goodsOrderId}', '{unit.affectedPart}', '{unit.orderReference}', " + 
                                $"'{unit.affectedSrNo}', '{unit.costOfReplacement}', {unit.currencyId}, '{((DateTime)unit.warrantyStartAt).ToString("yyyy'-'MM'-'dd")}', " + 
                                $"'{((DateTime)unit.warrantyEndAt).ToString("yyyy'-'MM'-'dd")}', '{unit.warrantyClaimTitle}', '{unit.warrantyDescription}', " + 
                                $"'{unit.correctiveActionByBuyer}', '{unit.requestToSupplier}', {unit.approverId},{(int)draftStatus}, " + 
                                $"'FAC{1000 + unit.facilityId}', '{((DateTime)unit.failureTime).ToString("yyyy'-'MM'-'dd")}', {userID}); select LAST_INSERT_ID(); ";
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                int id = Convert.ToInt32(dt.Rows[0][0]);
                string updateQry = $"UPDATE wc SET equipment_cat_id = (SELECT categoryId FROM assets WHERE assets.id = {unit.equipmentId}), " +
                                    $"equipment_sr_no = (SELECT serialNumber FROM assets WHERE assets.id = {unit.equipmentId}), " +
                                    $"supplier_id = (SELECT supplierId FROM assets WHERE assets.id = {unit.equipmentId}), " +
                                    $"currency = (SELECT code FROM currency WHERE currency.id = {unit.currencyId}) WHERE wc.id = {id};";
                await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                string addMailQry = "INSERT INTO wc_emails (wc_id, email, name, user_id, type) VALUES ";
                string idToMail = $"SELECT id, loginId as email, CONCAT(firstName,' ',lastName) as name FROM users " +
                                    $"WHERE id IN ({string.Join(',', unit.additionalEmailEmployees)});";
                DataTable mailList = await Context.FetchData(idToMail).ConfigureAwait(false);
                foreach (DataRow mail in mailList.Rows)
                {
                    addMailQry += $"({id}, '{mail["email"]}', '{mail["name"]}', {mail["id"]}, 'Internal'), ";
                }
                string getAllMails = "SELECT id, loginId FROM users;";
                DataTable allMails = await Context.FetchData(getAllMails).ConfigureAwait(false);
                Dictionary<string, int> mailToId = new Dictionary<string, int>();
                mailToId.Merge(allMails.GetColumn<string>("loginId"), allMails.GetColumn<int>("id"));
                foreach(var mail in unit.externalEmails)
                {
                    int u_id;
                    try
                    {
                        u_id = mailToId[mail.email];
                    }
                    catch(KeyNotFoundException)
                    {
                        u_id = 0;
                    }
                    addMailQry += $"({id}, '{mail.email}', '{mail.name}', {u_id}, '{(u_id==0?"Internal":"External")}'), ";
                }
                addMailQry = addMailQry.Substring(0, addMailQry.Length - 2) + ";";
                await Context.ExecuteNonQry<int>(addMailQry).ConfigureAwait(false);
                string addSupplierActions = "INSERT INTO wcschedules (warranty_id, supplier_action, input_value, input_date, created_at) VALUES ";
                foreach (var action in unit.supplierActions)
                {
                    addSupplierActions += $"({id}, '{action.name}', {(action.is_required)}, '{((DateTime)action.required_by_date).ToString("yyyy-MM-dd")}', " +
                                            $"'{UtilsRepository.GetUTCTime()}'), ";
                }
                addSupplierActions = addSupplierActions.Substring(0, addSupplierActions.Length - 2) + ";";
                await Context.ExecuteNonQry<int>(addSupplierActions).ConfigureAwait(false);

                count++;
                idList.Add(id);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, id, 0, 0, "Warranty Claim Created", draftStatus, userID);
            }
            if (count > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
                strRetMessage = $"{count} warranty claim(s) added";
            }
            else
            {
                strRetMessage = "No warranty claim(s) added";
            }
            return new CMDefaultResponse(idList, retCode, strRetMessage);
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
                throw new ArgumentException("Invalid ID");
            }

            string myQuery = "SELECT  wc.id as wc_id, wc.facilityId as facility_Id, f.name as facility_name,ac.id AS equipment_category_id, ac.name AS equipment_category, a.id AS equipment_id, a.name AS equipment_name, equipment_sr_no," +
            "b1.id AS supplier_id, b1.name AS supplier_name, good_order_id, affected_part, order_reference_number, affected_sr_no, cost_of_replacement, wc.currencyId,  wc.currency," + // add cost_of_replacement,
            " warranty_start_date, warranty_end_date, warranty_claim_title, warranty_description, " +
            "corrective_action_by_buyer, request_to_supplier, concat(user.firstName , ' ' , user.lastName) AS approver_name," +
            " created_by, issued_on, wc.status,approved_by, wc.created_at AS date_of_claim, wc_fac_code, failure_time" +
            " FROM wc " +
            "JOIN facilities as f ON f.id = wc.facilityId " +
            "JOIN assets as a ON a.id = wc.equipment_id " +
            "JOIN business as b1 ON b1.id = wc.supplier_id " +
            "JOIN assetcategories AS ac ON ac.id = wc.equipment_cat_id  " +
            "JOIN users as user ON user.id = wc.approver_id";

            if (id != 0)
            {
                myQuery += " WHERE wc.id= " + id;

            }
            List<CMWCDetail> GetWCDetails = await Context.GetData<CMWCDetail>(myQuery).ConfigureAwait(false);
            if (GetWCDetails.Count == 0)
                throw new NullReferenceException($"Warranty Claim with ID {id} not found");

            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(GetWCDetails[0].status);
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.WARRANTY_CLAIM, _Status);
            GetWCDetails[0].status_short = _shortStatus;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(GetWCDetails[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.WARRANTY_CLAIM, _Status_long, GetWCDetails[0]);
            GetWCDetails[0].status_long = _longStatus;

            // Retrieve external emails associated with the warranty claim
            string internalEmailsQuery = $"SELECT user_id, name, email  FROM wc_emails WHERE wc_id = {id} and type = 'Internal'";
            List<CMWCExternalEmail> internalEmails = await Context.GetData<CMWCExternalEmail>(internalEmailsQuery).ConfigureAwait(false);
            GetWCDetails[0].additionalEmailEmployees = internalEmails;

            // Retrieve external emails associated with the warranty claim
            string externalEmailsQuery = $"SELECT user_id, name, email FROM wc_emails WHERE wc_id = { id } and type = 'External'";
            List<CMWCExternalEmail> externalEmails = await Context.GetData<CMWCExternalEmail>(externalEmailsQuery).ConfigureAwait(false);
            GetWCDetails[0].externalEmails = externalEmails;

            // Retrieve supplier actions associated with the warranty claim
            string supplierActionsQuery = $"SELECT supplier_action as name, input_value AS is_required, input_date AS required_by_date FROM wcschedules WHERE warranty_id =  {id}";
            List<CMWCSupplierActions> supplierActions = await Context.GetData<CMWCSupplierActions>(supplierActionsQuery).ConfigureAwait(false);
            GetWCDetails[0].supplierActions = supplierActions;



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
            if (request.facilityId > 0)
                updateQry += $"facilityId = {request.facilityId}, wc_fac_code = 'FAC{1000+request.facilityId}', ";
            if (request.equipmentId > 0)
                updateQry += $"equipment_id = {request.equipmentId}, " +
                                $"equipment_cat_id = (SELECT categoryId FROM assets WHERE assets.id = {request.equipmentId}), " +
                                $"equipment_sr_no = (SELECT serialNumber FROM assets WHERE assets.id = {request.equipmentId}), " +
                                $"supplier_id = (SELECT supplierId FROM assets WHERE assets.id = {request.equipmentId}), " ;
            if (request.status == 1)
                updateQry += $"status = {(int)CMMS.CMMS_Status.WC_SUBMITTED}, ";
            if (request.goodsOrderId > 0)
                updateQry += $"good_order_id = {request.goodsOrderId}, ";
            if (request.affectedPart != null && request.affectedPart != "")
                updateQry += $"affected_part = '{request.affectedPart}', ";
            if (request.orderReference != null && request.orderReference != "")
                updateQry += $"order_reference_number = '{request.orderReference}', ";
            if (request.affectedSrNo != null && request.affectedSrNo != "")
                updateQry += $"affected_sr_no = '{request.affectedSrNo}', ";
            if (request.costOfReplacement > 0)
                updateQry += $"cost_of_replacement = '{request.costOfReplacement}', ";
            if (request.currencyId > 0)
                updateQry += $"currencyId = {request.currencyId}, " +
                                $"currency = (SELECT code FROM currency WHERE currency.id = {request.currencyId}), ";
            if (request.warrantyStartAt != null)
                updateQry += $"warranty_start_date = '{((DateTime)request.warrantyStartAt).ToString("yyyy'-'MM'-'dd")}', ";
            if (request.warrantyEndAt != null)
                updateQry += $"warranty_end_date = '{((DateTime)request.warrantyEndAt).ToString("yyyy'-'MM'-'dd")}', ";
            if (request.warrantyClaimTitle != null && request.warrantyClaimTitle != "")
                updateQry += $"warranty_claim_title = '{request.warrantyClaimTitle}', ";
            if (request.warrantyDescription != null && request.warrantyDescription != "")
                updateQry += $"warranty_description = '{request.warrantyDescription}', ";
            if (request.correctiveActionByBuyer != null && request.correctiveActionByBuyer != "")
                updateQry += $"corrective_action_by_buyer = '{request.correctiveActionByBuyer}', ";
            if (request.requestToSupplier != null && request.requestToSupplier != "")
                updateQry += $"request_to_supplier = '{request.requestToSupplier}', ";
            if (request.approverId > 0)
                updateQry += $"approver_id = {request.approverId}, ";
            if (request.failureTime != null)
                updateQry += $"failure_time = '{((DateTime)request.failureTime).ToString("yyyy'-'MM'-'dd")}', ";
            updateQry = updateQry.Substring(0, updateQry.Length - 2) + $" WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            if(request.additionalEmailEmployees != null || request.externalEmails != null)
            {
                if (request.additionalEmailEmployees.Count > 0 || request.externalEmails.Count > 0)
                {
                    string deleteMail = $"DELETE FROM wc_emails WHERE wc_id = {request.id}";
                    await Context.ExecuteNonQry<int>(deleteMail).ConfigureAwait(false);
                    string addMailQry = "INSERT INTO wc_emails (wc_id, email, name, type) VALUES ";
                    if (request.additionalEmailEmployees != null)
                    {
                        if (request.additionalEmailEmployees.Count > 0)
                        {
                            /*
                            string idToMail = $"SELECT loginId as email, CONCAT(firstName,' ',lastName) as name FROM users " +
                                    $"WHERE id IN ({string.Join(',', request.additionalEmailEmployees)});";
                            DataTable mailList = await Context.FetchData(idToMail).ConfigureAwait(false);
                            foreach (DataRow mail in mailList.Rows)
                            {
                                addMailQry += $"({request.id}, '{mail["email"]}', '{mail["name"]}', 'Internal'), ";
                            }
                            /**/
                            string idToMail = $"SELECT id, loginId as email, CONCAT(firstName,' ',lastName) as name FROM users " +
                                    $"WHERE id IN ({string.Join(',', request.additionalEmailEmployees)});";
                            DataTable mailList = await Context.FetchData(idToMail).ConfigureAwait(false);
                            foreach (DataRow mail in mailList.Rows)
                            {
                                addMailQry += $"({request.id}, '{mail["email"]}', '{mail["name"]}', {mail["id"]}, 'Internal'), ";
                            }
                        }
                    }
                    if (request.externalEmails != null)
                    {
                        if (request.externalEmails.Count > 0)
                        {
                            //string getAllMails = "SELECT loginId FROM users;";
                            //DataTable allMails = await Context.FetchData(getAllMails).ConfigureAwait(false);
                            //foreach (var mail in request.externalEmails)
                            //{
                            //    bool exists = allMails.GetColumn<string>("loginId").Contains(mail.email);
                            //    addMailQry += $"({request.id}, '{mail.email}', '{mail.name}', '{(exists ? "Internal" : "External")}'), ";
                            //}
                            string getAllMails = "SELECT id, loginId FROM users;";
                            DataTable allMails = await Context.FetchData(getAllMails).ConfigureAwait(false);
                            Dictionary<string, int> mailToId = new Dictionary<string, int>();
                            mailToId.Merge(allMails.GetColumn<string>("loginId"), allMails.GetColumn<int>("id"));
                            foreach (var mail in request.externalEmails)
                            {
                                int u_id;
                                try
                                {
                                    u_id = mailToId[mail.email];
                                }
                                catch (KeyNotFoundException)
                                {
                                    u_id = 0;
                                }
                                addMailQry += $"({request.id}, '{mail.email}', '{mail.name}', {u_id}, '{(u_id == 0 ? "Internal" : "External")}'), ";
                            }
                        }
                    }
                    addMailQry = addMailQry.Substring(0, addMailQry.Length - 2) + ";";
                    await Context.ExecuteNonQry<int>(addMailQry).ConfigureAwait(false);
                }
            }
            if(request.supplierActions != null)
            {
                if(request.supplierActions.Count > 0)
                {
                    string deleteActions = $"DELETE FROM wcschedules WHERE warranty_id = {request.id}";
                    await Context.ExecuteNonQry<int>(deleteActions).ConfigureAwait(false);
                    string addSupplierActions = "INSERT INTO wcschedules (warranty_id, supplier_action, input_value, input_date, created_at) VALUES ";
                    foreach (var action in request.supplierActions)
                    {
                        addSupplierActions += $"({request.id}, '{action.name}', {(action.is_required)}, '{((DateTime)action.required_by_date).ToString("yyyy-MM-dd")}', " +
                                                $"'{UtilsRepository.GetUTCTime()}'), ";
                    }
                    addSupplierActions = addSupplierActions.Substring(0, addSupplierActions.Length - 2) + ";";
                    await Context.ExecuteNonQry<int>(addSupplierActions).ConfigureAwait(false);
                }
            }
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "WC Details Updated Successfully");
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
