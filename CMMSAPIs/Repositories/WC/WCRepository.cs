using CMMSAPIs.Helper;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.WC
{
    public class WCRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        private Dictionary<int, string> StatusDictionary = new Dictionary<int, string>() {
            { 191, "In Draft" },
            { 192, "submitted - waiting for approval" },
            { 193, "Submit Request Rejected" },
            { 194, "Open Claim" },
            { 195, "Asset Dispatched" },
            { 196, "Rejected By Manufacturer" },
            { 197, "Approved By Manufacturer" },
            { 198, "Item Replenished" },
            { 199, "Closed Waiting for Approval" },
            { 200, "Closed-Approved" },
            { 201, "Closed- Reject" },

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
                    retValue = "submitted - waiting for approval"; break;
                case CMMS.CMMS_Status.WC_SUBMIT_REJECTED:
                    retValue = "Submit Rejected"; break;
                case CMMS.CMMS_Status.WC_SUBMIT_APPROVED:
                    retValue = "Open Claim"; break;
                case CMMS.CMMS_Status.WC_DISPATCHED:
                    retValue = "Dispatched"; break;
                case CMMS.CMMS_Status.WC_REJECTED_BY_MANUFACTURER:
                    retValue = "Rejected By Manufacturer"; break;
                case CMMS.CMMS_Status.WC_APPROVED_BY_MANUFACTURER:
                    retValue = "Approved By Manufacturer"; break;
                case CMMS.CMMS_Status.WC_ITEM_REPLENISHED:
                    retValue = "Item Replenished"; break;
                case CMMS.CMMS_Status.WC_CLOSED_REJECTED:
                    retValue = "Closed- Reject"; break;
                case CMMS.CMMS_Status.WC_CLOSE_APPROVED:
                    retValue = "Closed-Approved"; break;
                case CMMS.CMMS_Status.WC_CLOSED:
                    retValue = "Closed Waiting for Approval "; break;
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
                case CMMS.CMMS_Status.WC_CLOSED_REJECTED:
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
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN wc.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";
            string myQuery = "SELECT  wc.id as wc_id, wc.facilityID as facility_Id, f.name as facility_name,approxdailyloss,wc.cost_of_replacement as estimated_cost, ac.name AS equipment_category, a.name AS equipment_name, equipment_sr_no," +
                "b1.name AS supplier_name, good_order_id, affected_part, order_reference_number, affected_sr_no,wc.date_of_claim, cost_of_replacement, wc.currency," +
                " warranty_start_date, warranty_end_date, warranty_claim_title, warranty_description, " +
                "corrective_action_by_buyer, request_to_supplier, concat(user.firstName , ' ' , user.lastName) AS approver_name," +
                $" created_by, issued_on, {statusOut} as status,wc.status as status_code, approved_by, wc_fac_code, failure_time, " +
                "wc.claim_status,case when wc.claim_status=1 then 'WC done-closed'" +
               " when wc.claim_status=2 then 'WC rejected-closed'" +
               " when wc.claim_status=3 then 'WC partially done-closed' " +
               " else 'In Process' end as long_claim_status " +
                " FROM wc " +
                " LEFT JOIN facilities as f ON f.id = wc.facilityId " +
                " LEFT JOIN assets as a ON a.id = wc.equipment_id " +
                " LEFT JOIN business as b1 ON b1.id = wc.supplier_id " +
                " LEFT JOIN assetcategories AS ac ON ac.id = wc.equipment_cat_id  " +
                " LEFT JOIN users as user ON user.id = wc.approver_id";
            if (facilityId > 0)
            {
                myQuery += " WHERE wc.facilityId = " + facilityId;
                //if (startDate.Length() > 0 && endDate.Length > 0)
                //{
                //    myQuery += " AND failure_time >= " + startDate + " AND " +
                //           "failure_time <= " + endDate;
                //}
                if (statusId > 0)
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

        internal async Task<CMDefaultResponse> CreateWC(List<CMWCCreate> set, int userID)
        {
            /*
             * Insert all data in WC table in their respective columns 
            */
            int count = 0;
            List<int> idList = new List<int>();
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            string strRetMessage = "";
            int fid = 0;
            int cw_id = 0;

            foreach (CMWCCreate unit in set)
            {
                fid = unit.facilityId;


                CMMS.CMMS_Status draftStatus = (CMMS.CMMS_Status)(int)CMMS.CMMS_Status.WC_DRAFT;
                if (unit.status == 1)
                {
                    draftStatus = (CMMS.CMMS_Status)(int)CMMS.CMMS_Status.WC_SUBMITTED;
                }
                if (unit.status == 0)
                {
                    draftStatus = (CMMS.CMMS_Status)(int)CMMS.CMMS_Status.WC_DRAFT;
                }

                string qry = "insert into wc(status_updated_at, facilityId, equipment_id, good_order_id, affected_part, order_reference_number, affected_sr_no, " +
                                "cost_of_replacement,approxdailyloss , currencyId, warranty_start_date, warranty_end_date, warranty_claim_title, warranty_description, " +
                                "corrective_action_by_buyer,severity, request_to_supplier, approver_id, status, wc_fac_code, failure_time,date_of_claim, created_by,comment) values" +
                                $"('{UtilsRepository.GetUTCTime()}', {unit.facilityId}, {unit.equipmentId}, '{unit.goodsOrderId}', '{unit.affectedPart}', '{unit.orderReference}', " +
                                $"'{unit.affectedSrNo}', '{unit.costOfReplacement}',{unit.approxdailyloss}, {unit.currencyId}, '{((DateTime)unit.warrantyStartAt).ToString("yyyy'-'MM'-'dd")}', " +
                                $"'{((DateTime)unit.warrantyEndAt).ToString("yyyy'-'MM'-'dd")}', '{unit.warrantyClaimTitle}', '{unit.warrantyDescription}', " +
                                $"'{unit.correctiveActionByBuyer}','{unit.severity}' ,'{unit.requestToSupplier}', {unit.approverId},{(int)draftStatus}, " +
                                $"'FAC{1000 + unit.facilityId}', '{((DateTime)unit.failureTime).ToString("yyyy-MM-dd HH-mm")}','{((DateTime)unit.date_of_claim).ToString("yyyy-MM-dd HH-mm")}', {userID},'{unit.comment}'); select LAST_INSERT_ID(); ";
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                int id = Convert.ToInt32(dt.Rows[0][0]);
                cw_id = id;
                string updateQry = $"UPDATE wc SET equipment_cat_id = (SELECT categoryId FROM assets WHERE assets.id = {unit.equipmentId}), " +
                                    $"equipment_sr_no = (SELECT serialNumber FROM assets WHERE assets.id = {unit.equipmentId}), " +
                                    $"supplier_id = (SELECT supplierId FROM assets WHERE assets.id = {unit.equipmentId}), " +
                                    $"currency = (SELECT code FROM currency WHERE currency.id = {unit.currencyId}) WHERE wc.id = {id};";
                await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
                if (unit.additionalEmailEmployees.Count > 0)
                {
                    string addMailQry = "INSERT INTO wc_emails (wc_id, email, name, user_id, type,role,rolename,mobile) VALUES ";
                    string idToMail = $"SELECT id, loginId as email, CONCAT(firstName,' ',lastName) as name,roleId  FROM users " +
                                        $"WHERE id IN ({string.Join(',', unit.additionalEmailEmployees)});";
                    DataTable mailList = await Context.FetchData(idToMail).ConfigureAwait(false);
                    foreach (DataRow mail in mailList.Rows)
                    {
                        addMailQry += $"({id}, '{mail["email"]}', '{mail["name"]}', {mail["id"]}, 'Internal','{mail["roleId"]}','0','0'), ";
                    }
                    string getAllMails = "SELECT id, loginId,roleId FROM users;";
                    DataTable allMails = await Context.FetchData(getAllMails).ConfigureAwait(false);
                    Dictionary<string, int> mailToId = new Dictionary<string, int>();
                    Dictionary<string, int> mailToRole = new Dictionary<string, int>();
                    mailToId.Merge(allMails.GetColumn<string>("loginId"), allMails.GetColumn<int>("id"));
                    mailToRole.Merge(allMails.GetColumn<string>("loginId"), allMails.GetColumn<int>("roleId"));
                    foreach (var mail in unit.externalEmails)
                    {
                        string role_name = mail.role;
                        int u_id;
                        int roleId;
                        try
                        {
                            u_id = mailToId[mail.email];
                            roleId = mailToRole[mail.email];
                        }
                        catch (KeyNotFoundException)
                        {
                            u_id = 0;
                            roleId = 0;
                        }
                        addMailQry += $"({id}, '{mail.email}', '{mail.name}', {u_id}, '{(u_id != 0 ? "Internal" : "External")}','{roleId}','{role_name}','{mail.mobile}'), ";
                    }
                    addMailQry = addMailQry.Substring(0, addMailQry.Length - 2) + ";";
                    await Context.ExecuteNonQry<int>(addMailQry).ConfigureAwait(false);
                }
                if (unit.supplierActions.Count > 0)
                {
                    string addSupplierActions = "INSERT INTO wcschedules (warranty_id, supplier_action, input_value, input_date,srNumber,is_required, created_at) VALUES ";
                    foreach (var action in unit.supplierActions)
                    {
                        addSupplierActions += $"({id}, '{action.name}',0, '{((DateTime)action.required_by_date).ToString("yyyy-MM-dd")}', " +
                                                $"'{action.srNumber}',{action.is_required},'{UtilsRepository.GetUTCTime()}'), ";
                    }
                    addSupplierActions = addSupplierActions.Substring(0, addSupplierActions.Length - 2) + ";";
                    await Context.ExecuteNonQry<int>(addSupplierActions).ConfigureAwait(false);
                }

                count++;
                idList.Add(id);
                if (unit.affectedParts != null)
                {
                    foreach (var data in unit.affectedParts)
                    {

                        string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {fid}, module_type={(int)CMMS.CMMS_Modules.WARRANTY_CLAIM},module_ref_id={cw_id},status=1 where id = {data}";
                        await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                    }
                }
                if (unit.uploadfile_ids != null)
                {
                    foreach (int data in unit.uploadfile_ids)
                    {

                        string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {fid}, module_type={(int)CMMS.CMMS_Modules.WARRANTY_CLAIM}, status=0, module_ref_id={cw_id} where id = {data}";
                        await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                    }
                }
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, id, 0, 0, unit.comment, draftStatus, userID);
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

            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID");
            }

            string myQuery = "SELECT  wc.id as wc_id, wc.facilityId as facility_Id, f.name as facility_name,ac.id AS equipment_category_id, ac.name AS equipment_category, a.id AS equipment_id, a.name AS equipment_name, equipment_sr_no," +
            "b1.id AS supplier_id, b1.name AS supplier_name,bs.name as manufacture_name, good_order_id, affected_part, order_reference_number, affected_sr_no, cost_of_replacement, wc.currencyId,  wc.currency, approxdailyloss ," + // add cost_of_replacement,
            " warranty_start_date, warranty_end_date,wc.severity, warranty_claim_title, warranty_description, " +
            "corrective_action_by_buyer, request_to_supplier, concat(user.firstName , ' ' , user.lastName) AS approver_name,ws.is_required as is_required ," +
            " created_by, issued_on, wc.status,approved_by, wc.date_of_claim AS date_of_claim, wc_fac_code, failure_time, startDate warrantyStartDate,  endDate warrantyEndDate" +
            " , wc.claim_status,case when wc.claim_status=1 then 'WC done-closed'" +
            " when wc.claim_status=2 then 'WC rejected-closed'" +
            " when wc.claim_status=3 then 'WC partially done-closed' " +
            " else 'In Process' end as long_claim_status " +
            " FROM wc " +
            "LEFT JOIN facilities as f ON f.id = wc.facilityId " +
            "LEFT JOIN assets as a ON a.id = wc.equipment_id " +
            "LEFT JOIN business as bs ON a.ownerId = bs.id " +
            "LEFT JOIN wcschedules as ws ON ws.id=wc.id " +
            "LEFT JOIN business as b1 ON b1.id = wc.supplier_id " +
            "LEFT JOIN assetcategories AS ac ON ac.id = wc.equipment_cat_id  " +
            "LEFT JOIN users as user ON user.id = wc.approver_id";

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
            string internalEmailsQuery = $"SELECT user_id, name, email,role  FROM wc_emails WHERE wc_id = {id} and type = 'Internal'";
            List<CMWCExternalEmail> internalEmails = await Context.GetData<CMWCExternalEmail>(internalEmailsQuery).ConfigureAwait(false);
            GetWCDetails[0].additionalEmailEmployees = internalEmails;

            // Retrieve external emails associated with the warranty claim
            string externalEmailsQuery = $"SELECT user_id, name, email,rolename as rolename,mobile as mobile FROM wc_emails WHERE wc_id = {id} and type = 'External'";
            List<CMWCExternalEmail> externalEmails = await Context.GetData<CMWCExternalEmail>(externalEmailsQuery).ConfigureAwait(false);
            GetWCDetails[0].externalEmails = externalEmails;

            // Retrieve supplier actions associated with the warranty claim
            string supplierActionsQuery = $"SELECT supplier_action as name,srNumber, is_required AS is_required, input_date AS required_by_date FROM wcschedules WHERE warranty_id =  {id}";
            List<CMWCSupplierActions> supplierActions = await Context.GetData<CMWCSupplierActions>(supplierActionsQuery).ConfigureAwait(false);
            GetWCDetails[0].supplierActions = supplierActions;

            // Retrieve affected parts
            string affectedParts_Q = $"SELECT affected_part as name FROM wc_parts WHERE wc_id =  {id}";
            List<affectedParts> affectedParts = await Context.GetData<affectedParts>(affectedParts_Q).ConfigureAwait(false);

            //Normal Images
            string myQuery18 = "SELECT c.id as id,U.file_path as fileName,U.id as file_id,U.description FROM uploadedfiles AS U " +
                              "Left JOIN wc as c on c.id= U.module_ref_id  " +
                              "where module_ref_id =" + id + " and u.status=0 and U.module_type = " + (int)CMMS.CMMS_Modules.WARRANTY_CLAIM + ";";

            List<WCFileDetail> WC_image = await Context.GetData<WCFileDetail>(myQuery18).ConfigureAwait(false);
            //Affected Images
            string myQuery19 = "SELECT c.id as id,U.file_path as fileName,U.id as file_id,U.description  FROM uploadedfiles AS U " +
                             "Left JOIN wc as c on c.id= U.module_ref_id  " +
                             "where module_ref_id =" + id + " and u.status=1  and U.module_type = " + (int)CMMS.CMMS_Modules.WARRANTY_CLAIM + ";";

            List<WCFileDetail> WC_image_affected = await Context.GetData<WCFileDetail>(myQuery19).ConfigureAwait(false);


            GetWCDetails[0].affectedParts = affectedParts;
            GetWCDetails[0].supplierActions = supplierActions;
            GetWCDetails[0].Images = WC_image;
            GetWCDetails[0].affectedPartsImages = WC_image_affected;


            return GetWCDetails[0];
        }


        internal async Task<CMDefaultResponse> UpdateWC(CMWCCreate request, int userID)
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
                updateQry += $"facilityId = {request.facilityId}, wc_fac_code = 'FAC{1000 + request.facilityId}', ";
            if (request.equipmentId > 0)
                updateQry += $"equipment_id = {request.equipmentId}, " +
                                $"equipment_cat_id = (SELECT categoryId FROM assets WHERE assets.id = {request.equipmentId}), " +
                                $"equipment_sr_no = (SELECT serialNumber FROM assets WHERE assets.id = {request.equipmentId}), " +
                                $"supplier_id = (SELECT supplierId FROM assets WHERE assets.id = {request.equipmentId}), ";
            if (request.status == 1)
            {
                updateQry += $"status = {(int)CMMS.CMMS_Status.WC_SUBMITTED}, ";
            }
            if (request.status == 0)
            {
                updateQry += $"status = {(int)CMMS.CMMS_Status.WC_DRAFT}, ";
            }
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
            if (request.approxdailyloss > 0)
                updateQry += $"approxdailyloss = '{request.approxdailyloss}', ";
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
            if (request.severity != null)
                updateQry += $"severity= '{request.severity}', ";
            if (request.approverId > 0)
                updateQry += $"approver_id = {request.approverId}, ";
            if (request.date_of_claim != null)
                updateQry += $"date_of_claim =' {request.date_of_claim.ToString("yyyy'-'MM'-'dd HH-mm")}', ";

            if (request.failureTime != null)
            {
                updateQry += $"failure_time = '{request.failureTime.ToString("yyyy'-'MM'-'dd HH-mm")}', ";
            }
            if (request.resubmit == true)
            {
                updateQry += $"status = {(int)CMMS.CMMS_Status.WC_SUBMITTED}, ";
            }
            updateQry = updateQry.Substring(0, updateQry.Length - 2) + $" WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

            if (request.additionalEmailEmployees != null || request.externalEmails != null)
            {
                if (request.additionalEmailEmployees.Count > 0 || request.externalEmails.Count > 0)
                {
                    string deleteMail = $"DELETE FROM wc_emails WHERE wc_id = {request.id}";
                    await Context.ExecuteNonQry<int>(deleteMail).ConfigureAwait(false);
                    string addMailQry = "INSERT INTO wc_emails (wc_id, email, name, user_id, type ,role,rolename,mobile) VALUES ";
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
                                addMailQry += $"({request.id}, '{mail["email"]}', '{mail["name"]}', {mail["id"]}, 'Internal','0','0','0'), ";
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
                            string getAllMails = "SELECT id, loginId,roleId FROM users;";
                            DataTable allMails = await Context.FetchData(getAllMails).ConfigureAwait(false);
                            Dictionary<string, int> mailToId = new Dictionary<string, int>();
                            mailToId.Merge(allMails.GetColumn<string>("loginId"), allMails.GetColumn<int>("id"));

                            foreach (var mail in request.externalEmails)
                            {

                                string role_name = mail.role;
                                int u_id;
                                int roleId = 0;
                                try
                                {

                                    u_id = mailToId[mail.email];

                                }
                                catch (KeyNotFoundException)
                                {
                                    u_id = 0;
                                }
                                addMailQry += $"({request.id}, '{mail.email}', '{mail.name}', {u_id}, '{(u_id != 0 ? "Internal" : "External")}','{roleId}','{role_name}','{mail.mobile}'), ";
                            }
                        }
                    }
                    addMailQry = addMailQry.Substring(0, addMailQry.Length - 2) + ";";
                    await Context.ExecuteNonQry<int>(addMailQry).ConfigureAwait(false);
                }
            }
            if (request.affectedParts != null)
            {
                foreach (var data in request.affectedParts)
                {

                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facilityId}, module_type={(int)CMMS.CMMS_Modules.WARRANTY_CLAIM},module_ref_id={request.id},status=1 where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }
            if (request.uploadfile_ids != null)
            {
                foreach (int data in request.uploadfile_ids)
                {

                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facilityId}, module_type={(int)CMMS.CMMS_Modules.WARRANTY_CLAIM}, status=0, module_ref_id={request.id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }
            if (request.supplierActions != null)
            {
                if (request.supplierActions.Count > 0)
                {
                    string deleteActions = $"DELETE FROM wcschedules WHERE warranty_id = {request.id}";
                    await Context.ExecuteNonQry<int>(deleteActions).ConfigureAwait(false);
                    string addSupplierActions = "INSERT INTO wcschedules (warranty_id, supplier_action, input_value, input_date,srNumber,is_required, created_at) VALUES ";
                    foreach (var action in request.supplierActions)
                    {
                        addSupplierActions += $"({request.id}, '{action.name}', 0, '{((DateTime)action.required_by_date).ToString("yyyy-MM-dd")}', " +
                                                $"'{action.srNumber}',{action.is_required},'{UtilsRepository.GetUTCTime()}'), ";
                    }
                    addSupplierActions = addSupplierActions.Substring(0, addSupplierActions.Length - 2) + ";";
                    await Context.ExecuteNonQry<int>(addSupplierActions).ConfigureAwait(false);
                }
            }

            if (request.resubmit == true)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(request.comment);
                sb.Append(" " + request.comment);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, request.id, 0, 0, sb.ToString(), CMMS.CMMS_Status.WC_SUBMITTED, userID);

                return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"WC Resubmitted for Approval");

            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, request.id, 0, 0, request.comment, CMMS.CMMS_Status.UPDATED, userID);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "WC Details Updated Successfully");
        }

        internal async Task<CMDefaultResponse> ApproveWC(CMApproval request, int userId)
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
            string approveQuery = $"Update wc set status = {(int)CMMS.CMMS_Status.WC_SUBMIT_APPROVED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                $"approval_reccomendations = '{request.comment}',  " +
                $" approved_by = {userId}, approvedAt = '{UtilsRepository.GetUTCTime()}'" +
                $" where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            retCode = CMMS.RETRUNSTATUS.SUCCESS;
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, request.id, 0, 0, request.comment, CMMS.CMMS_Status.WC_SUBMIT_APPROVED, userId);
            string myQuery = $"SELECT * from wc where id = {request.id}";
            List<CMWCDetail> _WCList = await Context.GetData<CMWCDetail>(myQuery).ConfigureAwait(false);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.APPROVED, new[] { _WCList[0].created_by }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Approved warranty claim Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> RejectWC(CMApproval request, int userId)
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

            string approveQuery = $"Update wc set status = {(int)CMMS.CMMS_Status.WC_SUBMIT_REJECTED}, status_updated_at = '{UtilsRepository.GetUTCTime()}' , " +
                $"reject_reccomendations = '{request.comment}',  " +
                $" rejecctedBy = {userId}, rejectedAt = '{UtilsRepository.GetUTCTime()}' " +
                $" where id = {request.id}";
            int reject_id = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (reject_id > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, request.id, 0, 0, request.comment, CMMS.CMMS_Status.WC_SUBMIT_REJECTED, userId);
            string myQuery = $"SELECT * from wc where id = {request.id}";
            List<CMWCDetail> _WCList = await Context.GetData<CMWCDetail>(myQuery).ConfigureAwait(false);
            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.REJECTED, new[] { _WCList[0].created_by }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Rejected warranty claim Successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> ClosedWC(CMApproval request, int userId)
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

            string approveQuery = $"Update wc set status = {(int)CMMS.CMMS_Status.WC_CLOSED}, status_updated_at = '{UtilsRepository.GetUTCTime()}' , " +
                $"reject_reccomendations = '{request.comment}',  " +
                $" closed_by = {userId}, closed_at = '{UtilsRepository.GetUTCTime()}' , claim_status = {request.claim_status}" +
                $" where id = {request.id} ;";
            int cloesd = await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (cloesd > 0)
            {
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, request.id, 0, 0, request.comment, CMMS.CMMS_Status.WC_CLOSED, userId);
            string myQuery = $"SELECT * from wc where id = {request.id}";
            List<CMWCDetail> _WCList = await Context.GetData<CMWCDetail>(myQuery).ConfigureAwait(false);
            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.CANCELLED, new[] { _WCList[0].closed_by }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "WC Closed Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> updateWCimages(filesforwc request, int userId)
        {
            foreach (var data in request.uploadfile_ids)
            {

                string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {request.facilityId}, module_type={(int)CMMS.CMMS_Modules.WARRANTY_CLAIM},module_ref_id={request.id},status=0 where id = {data} ";
                await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);

            }
            if (request.supplierActions != null)
            {
                if (request.supplierActions.Count > 0)
                {
                    string deleteActions = $"DELETE FROM wcschedules WHERE warranty_id = {request.id}";
                    await Context.ExecuteNonQry<int>(deleteActions).ConfigureAwait(false);
                    string addSupplierActions = "INSERT INTO wcschedules (warranty_id, supplier_action, input_value, input_date,srNumber,is_required, created_at) VALUES ";
                    foreach (var action in request.supplierActions)
                    {
                        addSupplierActions += $"({request.id}, '{action.name}', 0, '{((DateTime)action.required_by_date).ToString("yyyy-MM-dd")}', " +
                                                $"'{action.srNumber}',{action.is_required},'{UtilsRepository.GetUTCTime()}'), ";
                    }
                    addSupplierActions = addSupplierActions.Substring(0, addSupplierActions.Length - 2) + ";";
                    await Context.ExecuteNonQry<int>(addSupplierActions).ConfigureAwait(false);
                }
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, request.id, 0, 0, request.comment, CMMS.CMMS_Status.UPDATED, userId);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "WC Updated");
            return response;

        }
        internal async Task<CMDefaultResponse> ApprovedClosedWC(CMApproval request, int userId)
        {
            string UpdateQ = $"update wc " +
             $"set Status = {(int)CMMS.CMMS_Status.WC_CLOSE_APPROVED} , closed_by = {userId}, " +
             $"closed_at = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', close_remarks = '{request.comment}' " +
             $"where id = {request.id} ;";
            var result = await Context.ExecuteNonQry<int>(UpdateQ);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, request.id, 0, 0, request.comment, CMMS.CMMS_Status.WC_CLOSE_APPROVED, userId);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, " approved closed successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> RejectClosedWC(CMApproval request, int userID)
        {
            string approveQuery = $"Update  wc set Status = {(int)CMMS.CMMS_Status.WC_SUBMIT_APPROVED},  " +
                $" rejecctedBy={userID},RejectedRemarks='{request.comment}', rejectedAt='{UtilsRepository.GetUTCTime()}' " +
                $"  where id = {request.id} ;";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.WARRANTY_CLAIM, request.id, 0, 0, request.comment, CMMS.CMMS_Status.WC_CLOSED_REJECTED, userID);
            string myQuery = $"SELECT * from wc where id = {request.id}";
            List<CMWCDetail> _WCList = await Context.GetData<CMWCDetail>(myQuery).ConfigureAwait(false);
            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.WARRANTY_CLAIM, CMMS.CMMS_Status.REJECTED, new[] { _WCList[0] }, _WCList[0]);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Reject closed successfully");
            return response;
        }

    }

}
