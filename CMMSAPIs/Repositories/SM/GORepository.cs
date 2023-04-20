using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMSAPIs.Repositories
{
    public class GORepository : GenericRepository
    {
        public GORepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }


        internal async Task<List<CMGO>> GetGOList()
        {
            /*
             * 
            */
            var myQuery = "SELECT pod.ID as podID,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_name,sam.asset_type_ID,pod.purchaseID," +
                          "pod.assetItemID,sai.serial_number,sai.location_ID,pod.cost,pod.ordered_qty,po.remarks as rejectedRemark,po.plantID,po.purchaseDate,sam.asset_type_ID,\n\t\t        po.vendorID,po.flag,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty,po.received_on,po.approvedOn,\n\t\t\t\tCONCAT(ed.Emp_First_Name,' ',ed.Emp_Last_Name) as generatedBy," +
                          "CONCAT(ed1.Emp_First_Name,' ',ed1.Emp_Last_Name) as receivedBy,CONCAT(ed2.Emp_First_Name,' ',ed2.Emp_Last_Name) as approvedBy,\n\t\t\t\tbl.Business_Name as vendor_name\n\t\t        FROM smpurchaseorderdetails pod\n\t\t        LEFT JOIN smpurchaseorder po ON po.ID = pod.purchaseID\n\t\t        LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID" +
                          "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code    LEFT JOIN (    SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\n\t\t            LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\n\t\t        )  t1 ON t1.master_ID = sam.ID\n\t\t        LEFT JOIN (\n\t\t      SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic" +
                          "LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID  )  t2 ON t2.master_ID = sam.ID LEFT JOIN employees ed ON ed.ID = po.generated_by\n\t\t        LEFT JOIN employees ed1 ON ed1.ID = po.receiverID\n\t\t\t\tLEFT JOIN employees ed2 ON ed2.ID = po.approved_by\n\t\t\t\tLEFT JOIN business bl ON bl.id = po.vendorID\n\t\t        /*LEFT JOIN FlexiMC_Emp_details ed ON ed.ID = po.generated_by\n\t\t        LEFT JOIN FlexiMC_Emp_details ed1 ON ed1.ID = po.receiverID\n\t\t\t\tLEFT JOIN FlexiMC_Emp_details ed2 ON ed2.ID = po.approved_by\n\t\t\t\tLEFT JOIN FlexiMC_Business_list bl ON bl.id = po.vendorID\n\t\t       WHERE po.ID =1 */";

            List<CMGO> _GOList = await Context.GetData<CMGO>(myQuery).ConfigureAwait(false);
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
            string mainQuery = $"INSERT INTO smpurchaseorderdetails (purchaseID,assetItemID,order_type,cost,ordered_qty,location_ID) " +
                "values("+request.purchaseID+", "+request.assetItemID+", '"+request.order_type + "', "+request.cost+", "+request.ordered_qty+", "+request.location_ID+") ; SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Goods order created successfully.");
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateGO(CMGO request, int userID)
        {
            string mainQuery = $"UPDATE smpurchaseorderdetails SET generate_flag = " +request.generate_flag + ",flag = "+request.flag+", vendorID = "+request.vendorID+" WHERE ID = "+request.id+""; 
            await Context.ExecuteNonQry<int>(mainQuery);
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
            string mainQuery = $"UPDATE smpurchaseorderdetails SET withdraw_by = " + userID + ", flag = "+request.flag+", remarks = '"+request.remarks+"', withdrawOn = '"+DateTime.Now.ToString()+"' WHERE ID = "+request.id+"";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Goods order withdrawn successfully.");
            return response;
        }
    }
}
