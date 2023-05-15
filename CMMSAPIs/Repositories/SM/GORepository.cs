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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Collections;

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
                          "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,po.remarks as rejectedRemark,po.plantID,po.purchaseDate,sam.asset_type_ID,\n\t\t        po.vendorID,po.flag,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty,po.received_on,po.approvedOn,\n\t\t\t\tCONCAT(ed.Emp_First_Name,' ',ed.Emp_Last_Name) as generatedBy," +
                          "CONCAT(ed1.Emp_First_Name,' ',ed1.Emp_Last_Name) as receivedBy,CONCAT(ed2.Emp_First_Name,' ',ed2.Emp_Last_Name) as approvedBy,\n\t\t\t\tbl.Business_Name as vendor_name\n\t\t        FROM smpurchaseorderdetails pod\n\t\t        LEFT JOIN smpurchaseorder po ON po.ID = pod.purchaseID\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
                          "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code    LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\n\t\t            LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\n\t\t        )  t1 ON t1.master_ID = sam.ID\n\t\t        LEFT JOIN (\n\t\t      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic" +
                          "LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID LEFT JOIN employees ed ON ed.ID = po.generated_by\n\t\t        LEFT JOIN employees ed1 ON ed1.ID = po.receiverID\n\t\t\t\tLEFT JOIN employees ed2 ON ed2.ID = po.approved_by\n\t\t\t\tLEFT JOIN business bl ON bl.id = po.vendorID\n\t\t        /*LEFT JOIN FlexiMC_Emp_details ed ON ed.ID = po.generated_by\n\t\t        LEFT JOIN FlexiMC_Emp_details ed1 ON ed1.ID = po.receiverID\n\t\t\t\tLEFT JOIN FlexiMC_Emp_details ed2 ON ed2.ID = po.approved_by\n\t\t\t\tLEFT JOIN FlexiMC_Business_list bl ON bl.id = po.vendorID\n\t\t       WHERE po.ID =1 */";

            string stmt = "SELECT pod.ID as podID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_name,sam.asset_type_ID,pod.purchaseID," +
                "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,po.remarks as rejectedRemark,po.plantID,po.purchaseDate," +
                "sam.asset_type_ID,\r\n\t\t        po.vendorID,po.flag,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty," +
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
                "/*  WHERE po.ID =1 */";
            List<CMGO> _GOList = await Context.GetData<CMGO>(stmt).ConfigureAwait(false);

            //CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_GOList[0].flag + 100);
            //string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOB, _Status);
            //_ViewJobList[0].status_short = _shortStatus;

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
                    $"";
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
            //string mainQuery = $"UPDATE smpurchaseorderdetails SET generate_flag = " +request.generate_flag + ",flag = "+request.flag+", vendorID = "+request.vendorID+" WHERE ID = "+request.id+"";
            string updateQ = $"UPDATE smpurchaseorderdetails SET assetItemID = {request.go_items[0].assetItemID},cost = {request.go_items[0].cost},ordered_qty = {request.go_items[0].ordered_qty},location_ID = {request.location_ID} WHERE ID = {request.id}";
            var result = await Context.ExecuteNonQry<int>(updateQ);

            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Goods order updated successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteGO(int GOid, int userID)
        {
            string mainQuery = $"UPDATE smpurchaseorderdetails SET flag = 0 WHERE ID = "+ GOid + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Goods order deleted.");
            return response;
        }
        internal async Task<CMDefaultResponse> WithdrawGO(CMGO request, int userID)
        {
            string mainQuery = $"UPDATE smpurchaseorderdetails SET withdraw_by = " + userID + ", flag = "+request.status+", remarks = '"+request.remarks+"', withdrawOn = '"+DateTime.Now.ToString()+"' WHERE ID = "+request.id+"";
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

        public async Task<CMDefaultResponse> PurchaseOrderApproval(CMGO request)
        {
            try
            {

                // Get the current date and time.
                DateTime date = DateTime.Now;

                // Update the purchase order.
                string UpdatesqlQ = $" UPDATE smpurchaseorder SET approved_by = {request.approvedBy}, flag = {request.status}, remarks = '{request.remarks}',approvedOn = '{date.ToString("yyyy-MM-dd")}' WHERE ID = {request.id}";
                await Context.ExecuteNonQry<int>(UpdatesqlQ);

                // Get the purchase order details.
                var data = await this.getPurchaseDetailsByID(request.id);

                // Set the subject of the email.
                string subject = "Goods Order Approval";

                // If the status is GO_APPROVED_BY_MANAGER, then send an email to the following people:
                //   - The vendor
                //   - The store manager
                //   - The person who generated the purchase order

                //GO_APPROVED_BY_MANAGER in constant.cs file it is 
                if (request.status == 1)
                {
                    // Get the email addresses of the vendor, store manager, and person who generated the purchase order.
                    var vendorEmail = await this.GetVendorEmailAsync(data[0].PlantId);
                    var storeManagerEmail = await this.GetStoreManagerEmailAsync(data[0].PlantId);
                    var generatedByEmail = await this.GetGeneratedByEmailAsync(data[0].PlantId);

                    // Create an array of email addresses.
                    var emailAddresses = new[]
                    {
                vendorEmail,
                storeManagerEmail,
                generatedByEmail,
            };

                    // Set the subject of the email.
                    subject = "Goods Order Approved";
                }
                else
                {
                    // Set the subject of the email.
                    subject = "Goods Order Rejected";
                }

                // Create an array of employee names.
                var employeeNames = new[]
                {
            data[0].VendorName,
            data[0].StoreManagerName,
            data[0].GeneratedByName,
        };

                // Merge the two arrays.
                var finalArray = emailAddresses.Select((email, i) => new
                {
                    EmailAddress = email,
                    EmployeeName = employeeNames[i],
                });

                // Send the email.
                //await this.transactionObj.sendMail('GOApproval', poId, subject, finalArray);

                // Insert a history record.
                string comment = JsonConvert.SerializeObject (params);
                int moduleType = (int)Constants.PO;

                int historyId = await this.InsertHistoryDataAsync(moduleType, poId, comment, empId, status);

                if (historyId == 0)
                {
                    throw new Exception("history insert failed");
                }

                // Commit the transaction.
                await this.conn.CommitAsync();

                // Return a success response.
                return Ok(new
                {
                    Status = "Success",
                    Message = "Data Updated",
                });
            }
            catch (Exception e)
            {


                // Return an error response.
                return BadRequest(new
                {
                    Status = "Failed",
                    Message = "Sorry something went wrong!!",
                });
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
                "       LEFT JOIN facilities fc ON fc.id = po.plantID\r\n        LEFT JOIN business bl ON bl.id = po.vendorID  WHERE po.ID = "+id+ " GROUP BY pod.ID";
            List<CMGO> _List = await Context.GetData<CMGO>(query).ConfigureAwait(false);
            return _List;
        }

    }
}
