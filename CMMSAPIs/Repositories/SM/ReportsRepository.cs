using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.SM;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Utils;
using System.Data;
using CMMSAPIs.Models.Users;
using MySql.Data.MySqlClient;
using System.Transactions;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Notifications;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CMMSAPIs.Repositories.SM
{
    public class ReportsRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public ReportsRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        public async Task<List<CMPlantStockOpeningResponse>> GetPlantStockReport(string facility_id, DateTime StartDate, DateTime EndDate)
        {
            List<string> Asset_Item_Ids = new List<string>();
            List<CMPlantStockOpening> Asset_Item_Opening_Balance_details = new List<CMPlantStockOpening>();
            List<CMPlantStockOpeningResponse> Response = new List<CMPlantStockOpeningResponse>();
          
            //Actors.Store
            string Plant_Stock_Opening_Details_query = $"SELECT  a_item.facility_ID as facilityID, fc.name as facilityName,fc.isBlock as Facility_Is_Block," +
                $"'' as Facility_Is_Block_of_name," +
                $"sm_trans.assetItemID, a_master.asset_name, a_master.asset_code, a_master.asset_type_ID, " +
                $"sum(sm_trans.creditQty)-sum(sm_trans.debitQty) as Opening ,sum(sm_trans.creditQty) as inward, sum(sm_trans.debitQty) as outward " +
                $"FROM smtransition as sm_trans " +
                $"join smassetitems as a_item ON sm_trans.assetItemID = a_item.ID " +
                $"JOIN smassetmasters as a_master ON a_master.asset_code = a_item.asset_code " +
                $"LEFT JOIN facilities fc ON fc.id = a_item.facility_ID " +
                $"where sm_trans.actorType = {(int)CMMS.SM_Types.Store} and a_item.facility_ID in ('{facility_id}') " +
                $"and date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') BETWEEN '{ StartDate.ToString("yyyy-MM-dd")}' AND '{ EndDate.ToString("yyyy-MM-dd")}'  " +
                $"group by a_item.asset_code";
            List<CMPlantStockOpening> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMPlantStockOpening>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);
            List<CMPlantStockOpeningItemWiseResponse> itemWiseResponse = new List<CMPlantStockOpeningItemWiseResponse>();
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {
                string facility_name = "";
                if (Convert.ToInt32(item.Facility_Is_Block) == 0)
                {
                    facility_name = Convert.ToString(item.facilityName);
                }
                else
                {
                    facility_name = Convert.ToString(item.Facility_Is_Block_of_name);
                }

                Asset_Item_Ids.Add(Convert.ToString(item.assetItemID));

                CMPlantStockOpening openingBalance = new CMPlantStockOpening();

                openingBalance.facilityID = item.facilityID;
                openingBalance.facilityName = facility_name;
                openingBalance.Opening = item.Opening;
                openingBalance.assetItemID = item.assetItemID;
                openingBalance.asset_name = item.asset_name;
                openingBalance.asset_code = item.asset_code;
                openingBalance.asset_type_ID = item.asset_type_ID;
                openingBalance.inward = item.inward;
                openingBalance.outward = item.outward;
                openingBalance.balance = item.Opening + item.inward - item.outward;
                Asset_Item_Opening_Balance_details.Add(openingBalance);        
            }

  
            var uniqueValues = Asset_Item_Opening_Balance_details.GroupBy(p => p.facilityID)
            .Select(g => g.First())
            .ToList();
            foreach (var item in uniqueValues)
            {
                CMPlantStockOpeningResponse cMPlantStockOpeningResponse = new CMPlantStockOpeningResponse();
                cMPlantStockOpeningResponse.facilityID = item.facilityID;
                cMPlantStockOpeningResponse.facilityName = item.facilityName;
                Response.Add(cMPlantStockOpeningResponse);
            }
            foreach (var item in Response)
            {
                List<CMPlantStockOpeningItemWiseResponse> itemResponseList = new List<CMPlantStockOpeningItemWiseResponse>();
                CMPlantStockOpeningResponse cMPlantStockOpeningResponse = new CMPlantStockOpeningResponse();
                cMPlantStockOpeningResponse.facilityID = item.facilityID;
                cMPlantStockOpeningResponse.facilityName = item.facilityName;
                var itemResponse = Asset_Item_Opening_Balance_details.Where(item => item.facilityID == item.facilityID).ToList();
                foreach(var itemDetail in itemResponse)
                {
                    CMPlantStockOpeningItemWiseResponse itemWise = new CMPlantStockOpeningItemWiseResponse();
                    itemWise.Facility_Is_Block = itemDetail.Facility_Is_Block;
                    itemWise.Facility_Is_Block_of_name = itemDetail.Facility_Is_Block_of_name;
                    itemWise.assetItemID = itemDetail.assetItemID;
                    itemWise.asset_name = itemDetail.asset_name;
                    itemWise.asset_code = itemDetail.asset_code;
                    itemWise.asset_type_ID = itemDetail.asset_type_ID;
                    itemWise.Opening = itemDetail.Opening;
                    itemWise.inward = itemDetail.inward;
                    itemWise.outward = itemDetail.outward;
                    itemWise.balance = itemDetail.balance;
                    itemResponseList.Add(itemWise);
                }
                item.stockDetails = itemResponseList;
        
            }
            return Response;

        }

public async Task<List<CMEmployeeStockReport>> GetEmployeeStockReport(int facility_id, int Emp_id, DateTime StartDate, DateTime EndDate, string itemID)
        {
            //int Emp_id = Utils.UtilsRepository.GetUserID();
            List<string> Asset_Item_Ids = new List<string>();
            List<CMEmployeeStockReport> EmployeeStockReportList = new List<CMEmployeeStockReport>();
            string Plant_Stock_Opening_Details_query = "";
            if (itemID != null && itemID != "")
            {
                Plant_Stock_Opening_Details_query = "SELECT fc.name as facilityName, fc.isBlock as Facility_Is_Block, '' as Facility_Is_Block_of_name," +
                    " CONCAT(ed.firstName, ' ', ed.lastName) as requested_by_name, sm_trans.assetItemID, a_master.asset_name, a_master.asset_code, a_master.asset_type_ID, " +
                    "SUM(sm_trans.debitQty) - SUM(sm_trans.creditQty) as Opening, SUM(sm_trans.creditQty) as inward, SUM(sm_trans.debitQty) as outward " +
                    "FROM smtransition as sm_trans " +
                    "JOIN smassetitems as a_item ON sm_trans.assetItemID = a_item.ID " +
                    "JOIN smassetmasters as a_master ON a_master.asset_code = a_item.asset_code " +
                    "LEFT JOIN facilities fc ON fc.id = a_item.facility_ID " +
                    "LEFT JOIN users ed ON sm_trans.actorID = ed.id " +
                    "WHERE sm_trans.assetItemID in (" + itemID + ") and sm_trans.actorID = " + Emp_id + " AND sm_trans.actorType = '" + (int)CMMS.SM_Types.Engineer + "' AND a_item.facility_ID = '" + facility_id + "' " +
                    "AND DATE_FORMAT(sm_trans.lastModifiedDate, '%Y-%m-%d') between '" + StartDate.ToString("yyyy-MM-dd") + "' and '" + EndDate.ToString("yyyy-MM-dd") + "' GROUP BY a_item.asset_code";
            }
            else
            {
                Plant_Stock_Opening_Details_query = "SELECT fc.name as facilityName, fc.isBlock as Facility_Is_Block, '' as Facility_Is_Block_of_name," +
                    " CONCAT(ed.firstName, ' ', ed.lastName) as requested_by_name, sm_trans.assetItemID, a_master.asset_name, a_master.asset_code, a_master.asset_type_ID, " +
                    "SUM(sm_trans.debitQty) - SUM(sm_trans.creditQty) as Opening, SUM(sm_trans.creditQty) as inward, SUM(sm_trans.debitQty) as outward " +
                    "FROM smtransition as sm_trans " +
                    "JOIN smassetitems as a_item ON sm_trans.assetItemID = a_item.ID " +
                    "JOIN smassetmasters as a_master ON a_master.asset_code = a_item.asset_code " +
                    "LEFT JOIN facilities fc ON fc.id = a_item.facility_ID " +
                    "LEFT JOIN users ed ON sm_trans.actorID = ed.id " +
                    "WHERE sm_trans.actorID = " + Emp_id + " AND sm_trans.actorType = '" + (int)CMMS.SM_Types.Engineer + "' AND a_item.facility_ID = '" + facility_id + "' " +
                    "AND DATE_FORMAT(sm_trans.lastModifiedDate, '%Y-%m-%d') between '" + StartDate.ToString("yyyy-MM-dd") + "' and '" + EndDate.ToString("yyyy-MM-dd") + "' GROUP BY a_item.asset_code";
            }
            List<CMEmployeeStockReport> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMEmployeeStockReport>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);

            int cnt = 0;
            string plant_name = "";
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {
                if (Convert.ToInt32(item.Facility_Is_Block) == 0)
                {
                    plant_name = Convert.ToString(item.facilityName);
                }
                else
                {
                    plant_name = Convert.ToString(item.Facility_Is_Block_of_name);
                }

                Asset_Item_Ids.Add(Convert.ToString(item.assetItemID));

                CMEmployeeStockReport openingBalance = new CMEmployeeStockReport();
                openingBalance.facilityName = plant_name;
                openingBalance.requested_by_name = Convert.ToString(item.requested_by_name);
                openingBalance.Opening = item.Opening;
                openingBalance.assetItemID = item.assetItemID;
                openingBalance.asset_name = item.asset_name;
                openingBalance.asset_code = item.asset_code;
                openingBalance.asset_type_ID = item.asset_type_ID;
                openingBalance.balance = item.Opening + item.inward - item.outward;
                EmployeeStockReportList.Add(openingBalance);

                cnt++;
            }

            return EmployeeStockReportList;
        }
    
        public async Task<List<CMFaultyMaterialReport>> GetFaultyMaterialReport(string facility_id,string itemID, DateTime StartDate, DateTime EndDate)
        {
            string query = $"SELECT fmItemList.* FROM (SELECT sm_td.*,fc.id as facility_id, fc.name as facilityName,fc.isBlock as Facility_Is_Block," +
                $"'' as Facility_Is_Block_of_name,CONCAT(ed.firstName,' ',ed.lastName) as emp_name, a_item.asset_code," +
                $"a_item.item_condition, a_item.serial_number, IF(a_item1.serial_number != '',a_item1.serial_number,'--')  as replaceSerialNo," +
                $" a_master.asset_name,\r\n a_master.asset_type_ID,a_master.item_category_ID , smt.actorID" +
                $"  FROM smtransactiondetails as sm_td \r\n\tJOIN smtransition as smt ON sm_td.ID = smt.transactionID " +
                $"LEFT JOIN smassetitems as a_item ON sm_td.assetItemID = a_item.ID " +
                $"LEFT JOIN smassetitems as a_item1 ON a_item1.ID = a_item.replaced_asset_id " +
                $"LEFT JOIN smassetmasters as a_master ON  a_item.asset_code = a_master.asset_code  " +
                $"LEFT JOIN facilities fc ON fc.id = sm_td.plantID  " +
                $"LEFT JOIN users ed ON ed.id = sm_td.toActorID " +
                $"where (date_format(sm_td.lastInsetedDateTime, '%Y-%m-%d') between '{StartDate.ToString("yyyy-MM-dd")}' and '{EndDate.ToString("yyyy-MM-dd")}') and " +
                $"sm_td.fromActorType = '{(int)CMMS.SM_Types.Inventory}' and sm_td.toActorType = '{(int)CMMS.SM_Types.Engineer}' and smt.actorType = '{(int)CMMS.SM_Types.Engineer}' and (sm_td.referedby = '4' OR sm_td.referedby = '{(int)CMMS.CMMS_Modules.SM_PO}') and a_item.item_condition IN (2,3,4)" +
                $" and sm_td.plantID in ('{facility_id}')  AND sm_td.Nature_Of_Transaction = 1 ORDER BY sm_td.ID DESC) as fmItemList GROUP BY fmItemList.assetItemID ORDER BY fmItemList.ID DESC";

            List<CMFaultyMaterialReport> result = await Context.GetData<CMFaultyMaterialReport>(query).ConfigureAwait(false);
            return result;
        }

        public async Task<List<CMEmployeeTransactionReport>> GetEmployeeTransactionReport(int isAllEmployees, string facility_id,int Emp_ID, DateTime StartDate, DateTime EndDate)
        {
            //int Emp_id = 187;// Utils.UtilsRepository.GetUserID();
            List<CMEmployeeTransactionReport> EmployeeTransactionReportList = new List<CMEmployeeTransactionReport>();

            string EmpStockTransactionDetailsQuery = $"SELECT sm_td.*, fc.id as facility_id ,fc.name as facilityName,fc.isBlock as Facility_Is_Block," +
                $"'' as Facility_Is_Block_of_name, CONCAT(ed.firstName,' ',ed.lastName) as requested_by_name, a_item.asset_code, " +
                $"a_item.item_condition, a_item.serial_number,a_master.asset_name, a_master.asset_type_ID ,i.available_qty, " +
                $" i.return_remarks, smt.actorID,i.mrs_return_ID  " +
                $"FROM smtransactiondetails as sm_td " +
                $"JOIN smtransition as smt ON sm_td.ID = smt.transactionID " +
                $" JOIN smassetitems as a_item ON sm_td.assetItemID = a_item.ID  " +
                $"JOIN smassetmasters as a_master ON  a_item.asset_code = a_master.asset_code " +
                $"LEFT JOIN smrsitems as i ON i.mrs_return_ID = smt.mrsID  " +
                $"LEFT JOIN smmrs as mrs ON  mrs.ID = sm_td.reference_ID " +
                $" LEFT JOIN facilities fc ON fc.id = sm_td.plantID " +
                $" LEFT JOIN users ed ON ed.id = sm_td.fromActorID"+
                $" WHERE (DATE_FORMAT(sm_td.lastInsetedDateTime, '%Y-%m-%d') BETWEEN '" + StartDate.ToString("yyyy-MM-dd") + "' AND '" + EndDate.ToString("yyyy-MM-dd") + "') AND ";

            if (isAllEmployees != 1)
            {
                EmpStockTransactionDetailsQuery += " sm_td.fromActorID = '" + Emp_ID + "' AND smt.actorID = '" + Emp_ID + "' AND ";
            }

            EmpStockTransactionDetailsQuery += "sm_td.fromActorType = '" + (int)CMMS.SM_Types.Engineer + "' AND smt.actorType = '" + (int)CMMS.SM_Types.Engineer + "' AND mrs.flag = 2 AND mrs.approval_status = 1 AND i.mrs_ID = sm_td.reference_ID AND sm_td.plantID in ( '" + facility_id+ "')";

            List<CMEmployeeTransactionReport> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMEmployeeTransactionReport>(EmpStockTransactionDetailsQuery).ConfigureAwait(false);

            string plant_name = "";
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {
                if (Convert.ToInt32(item.Facility_Is_Block) == 0)
                {
                    plant_name = Convert.ToString(item.facilityName);
                }
                else
                {
                    plant_name = Convert.ToString(item.Facility_Is_Block_of_name);
                }


                CMEmployeeTransactionReport openingBalance = new CMEmployeeTransactionReport();
                openingBalance.facilityName = plant_name;
                openingBalance.fromActorID = item.fromActorID;
                openingBalance.fromActorType = item.fromActorType;
                openingBalance.toActorID = item.toActorID;
                openingBalance.toActorType = item.toActorType;
                openingBalance.assetItemID = item.assetItemID;
                openingBalance.qty = item.qty;
                openingBalance.facility_id = item.facility_id;
                openingBalance.referedby = item.referedby;
                openingBalance.reference_ID = item.reference_ID;
                openingBalance.remarks = item.remarks;
                openingBalance.Nature_Of_Transaction = item.Nature_Of_Transaction;
                openingBalance.Asset_Item_Status = item.Asset_Item_Status;
                openingBalance.flag = item.flag;
                openingBalance.lastInsetedDateTime = item.lastInsetedDateTime;
                openingBalance.Facility_Is_Block = item.Facility_Is_Block;
                openingBalance.Facility_Is_Block_of_name = item.Facility_Is_Block_of_name;
                openingBalance.requested_by_name = item.requested_by_name;
                openingBalance.asset_code = item.asset_code;
                openingBalance.item_condition = item.item_condition;
                openingBalance.serial_number = item.serial_number;
                openingBalance.asset_name = item.asset_name;
                openingBalance.asset_type_ID = item.asset_type_ID;
                openingBalance.available_qty = item.available_qty;
                openingBalance.return_remarks = item.return_remarks;
                openingBalance.actorID = item.actorID;
                openingBalance.mrs_return_ID = item.mrs_return_ID;
                if (item.toActorID == Emp_ID || item.toActorType == Convert.ToString((int)CMMS.SM_Types.Engineer))
                {
                    openingBalance.InwardQty = item.qty;
                }
                 else if (item.fromActorID == Emp_ID || item.fromActorType == Convert.ToString((int)CMMS.SM_Types.Engineer))
                {
                    openingBalance.OutwardQty = item.qty;
                }

                if (item.item_condition == 1){
                    openingBalance.remarks_in_short = "Fresh";
                }else if (item.item_condition == 2){
                    openingBalance.remarks_in_short = "Damaged. Return to repair";
                }else if (item.item_condition == 3){
                    openingBalance.remarks_in_short = "Damaged. Repaired";
                }else if (item.item_condition == 4){
                    openingBalance.remarks_in_short = "Damaged. Return to discard";
                }else if (item.item_condition == 5){
                    openingBalance.remarks_in_short = "Return From Order";
                }

                EmployeeTransactionReportList.Add(openingBalance);
            }
            return EmployeeTransactionReportList;
        }


        internal async Task<CMEmployeeStockList> GetEmployeeStock(int facility_ID, int emp_id)
        {
            List<string> Asset_Item_Ids = new List<string>();
            List<CMEmployeeStockReport> EmployeeStockReportList = new List<CMEmployeeStockReport>();

            string Plant_Stock_Opening_Details_query = "SELECT fc.name as facilityName, fc.isBlock as Facility_Is_Block, '' as Facility_Is_Block_of_name," +
                " CONCAT(ed.firstName, ' ', ed.lastName) as requested_by_name, sm_trans.assetItemID, a_master.asset_name, a_master.asset_code, a_master.asset_type_ID, " +
                " SUM(sm_trans.debitQty) - SUM(sm_trans.creditQty)  as Opening, SUM(sm_trans.creditQty) as inward, SUM(sm_trans.debitQty) as outward " +
                "FROM smtransition as sm_trans " +
                "JOIN smassetitems as a_item ON sm_trans.assetItemID = a_item.ID " +
                "JOIN smassetmasters as a_master ON a_master.asset_code = a_item.asset_code " +
                "LEFT JOIN facilities fc ON fc.id = a_item.facility_ID " +
                "LEFT JOIN users ed ON sm_trans.actorID = ed.id " +
                "WHERE  sm_trans.actorID = " + emp_id + "  AND a_item.facility_ID = '" + facility_ID + "' " +
                " GROUP BY a_item.asset_code";

            List<CMEmployeeStockReport> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMEmployeeStockReport>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);

            CMEmployeeStockList cMEmployeeStockList = new CMEmployeeStockList();
            int cnt = 0;
            string plant_name = "";
            cMEmployeeStockList.emp_ID = emp_id;
            cMEmployeeStockList.emp_name = Plant_Stock_Opening_Details_Reader[0].requested_by_name;
            List<CMEmpStockItems> itemList = new List<CMEmpStockItems>();
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {

                if (item.Opening != 0)
                {
                    CMEmpStockItems openingBalance = new CMEmpStockItems();
                    openingBalance.asset_item_ID = item.assetItemID;
                    openingBalance.item_name = Convert.ToString(item.asset_name);
                    openingBalance.quantity = item.Opening;


                    itemList.Add(openingBalance);

                    cnt++;
                }
            }
            cMEmployeeStockList.CMMRSItems = itemList;
            return cMEmployeeStockList;
        }


    }
}
