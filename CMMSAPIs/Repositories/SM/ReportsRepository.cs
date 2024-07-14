using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.SM
{
    public class ReportsRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public ReportsRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        public async Task<List<CMPlantStockOpeningResponse>> GetPlantStockReport(string facility_id, DateTime StartDate, DateTime EndDate, string assetMasterIDs)
        {
            List<string> Asset_Item_Ids = new List<string>();
            List<CMPlantStockOpening> Asset_Item_Opening_Balance_details = new List<CMPlantStockOpening>();
            List<CMPlantStockOpeningResponse> Response = new List<CMPlantStockOpeningResponse>();
            string itemCondition = "";
            string Plant_Stock_Opening_Details_query = "";
            if (assetMasterIDs != null && assetMasterIDs != "")
            {
                itemCondition = " AND  a_master.ID  in (" + assetMasterIDs + ") ";
            }

            Plant_Stock_Opening_Details_query = $"SELECT  sm_trans.facilityID as facilityID, fc.name as facilityName," +
                $"fc.isBlock as Facility_Is_Block, " +
                $" '' as Facility_Is_Block_of_name,sm_trans.assetItemID, a_master.asset_name, a_master.asset_code," +
                $" a_master.asset_type_ID, AST.asset_type,  " +
                $" IFNULL((select sum(ST.creditQty)-sum(ST.debitQty)  FROM smtransition as ST  JOIN smassetmasters as SM ON SM.ID = ST.assetItemID  " +
                $" LEFT JOIN facilities fcc ON fcc.id = ST.facilityID   where   ST.actorType = {(int)CMMS.SM_Actor_Types.Store} and SM.ID=a_master.ID  and ST.facilityID in ('{facility_id}')" +
                $" and sm_trans.actorID = {facility_id} and date_format(ST.lastModifiedDate, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}'  group by SM.asset_code),0) Opening," +
                $"  IFNULL((select sum(si.creditQty) from smtransition si where si.id = sm_trans.id and  date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') " +
                $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' ),0) as inward, " +
                $"   IFNULL((select sum(so.debitQty) from smtransition so where so.id = sm_trans.id and  date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') " +
                $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' ),0) as outward " +
                $" FROM smtransition as sm_trans " +
                $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID " +
                $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
                $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
                $" where sm_trans.actorType = {(int)CMMS.SM_Actor_Types.Store} and sm_trans.facilityID in ('{facility_id}') and " +
                $" sm_trans.actorID in ({facility_id}) ";

            Plant_Stock_Opening_Details_query = Plant_Stock_Opening_Details_query + itemCondition;
            Plant_Stock_Opening_Details_query = Plant_Stock_Opening_Details_query + " group by a_master.asset_code;";

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
                openingBalance.asset_type = item.asset_type;
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
                foreach (var itemDetail in itemResponse)
                {
                    CMPlantStockOpeningItemWiseResponse itemWise = new CMPlantStockOpeningItemWiseResponse();
                    itemWise.Facility_Is_Block = itemDetail.Facility_Is_Block;
                    itemWise.Facility_Is_Block_of_name = itemDetail.Facility_Is_Block_of_name;
                    itemWise.assetItemID = itemDetail.assetItemID;
                    itemWise.asset_name = itemDetail.asset_name;
                    itemWise.asset_code = itemDetail.asset_code;
                    itemWise.asset_type_ID = itemDetail.asset_type_ID;
                    itemWise.asset_type = itemDetail.asset_type;
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

                Plant_Stock_Opening_Details_query = $"SELECT  sm_trans.facilityID as facilityID, fc.name as facilityName, " +
                    $" fc.isBlock as Facility_Is_Block, '' as Facility_Is_Block_of_name,sm_trans.assetItemID, a_master.asset_name, " +
                    $" a_master.asset_code, a_master.asset_type_ID, AST.asset_type," +
                    $" IFNULL((select sum(ST.creditQty)-sum(ST.debitQty)  FROM smtransition as ST  " +
                    $" JOIN smassetmasters as SM ON SM.ID = ST.assetItemID  LEFT JOIN facilities fcc ON fcc.id = ST.facilityID  " +
                    $" where   ST.actorType = {(int)CMMS.SM_Actor_Types.Engineer} and SM.ID=a_master.ID  and ST.facilityID in ('{facility_id}') and sm_trans.actorID = {Emp_id} and" +
                    $" date_format(ST.lastModifiedDate, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}'  group by SM.asset_code),0) Opening," +
                    $"  IFNULL((select sum(si.creditQty) from smtransition si where si.id = sm_trans.id and  date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') " +
                    $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' ),0) as inward as inward, " +
                    $"   IFNULL((select sum(so.debitQty) from smtransition so where so.id = sm_trans.id and  date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') " +
                    $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' ),0) as outward  as outward " +
                    $" FROM smtransition as sm_trans " +
                    $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID" +
                    $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
                    $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
                    $" where sm_trans.actorType = {(int)CMMS.SM_Actor_Types.Engineer} and sm_trans.facilityID in ('{facility_id}') and sm_trans.actorID in ({Emp_id})" +
                    $" and a_master.id in ({itemID}) group by a_master.asset_code;";


            }
            else
            {
                Plant_Stock_Opening_Details_query = $"SELECT  sm_trans.facilityID as facilityID, fc.name as facilityName, " +
                    $" fc.isBlock as Facility_Is_Block, '' as Facility_Is_Block_of_name,sm_trans.assetItemID, a_master.asset_name, " +
                    $" a_master.asset_code, a_master.asset_type_ID, AST.asset_type," +
                    $" IFNULL((select sum(ST.creditQty)-sum(ST.debitQty)  FROM smtransition as ST  " +
                    $" JOIN smassetmasters as SM ON SM.ID = ST.assetItemID  LEFT JOIN facilities fcc ON fcc.id = ST.facilityID  " +
                    $" where   ST.actorType = {(int)CMMS.SM_Actor_Types.Engineer} and SM.ID=a_master.ID  and ST.facilityID in ('{facility_id}') and sm_trans.actorID = {Emp_id} and" +
                    $" date_format(ST.lastModifiedDate, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}'  group by SM.asset_code),0) Opening," +
                    $"  IFNULL((select sum(si.creditQty) from smtransition si where si.id = sm_trans.id and  date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') " +
                    $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' ),0) as inward as inward, " +
                    $"   IFNULL((select sum(so.debitQty) from smtransition so where so.id = sm_trans.id and  date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') " +
                    $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' ),0) as outward  as outward " +
                    $" FROM smtransition as sm_trans " +
                    $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID" +
                    $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
                    $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
                    $" where sm_trans.actorType = {(int)CMMS.SM_Actor_Types.Engineer} and sm_trans.facilityID in ('{facility_id}') and sm_trans.actorID in ({Emp_id})" +
                    $" group by a_master.asset_code;";

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

        public async Task<List<CMFaultyMaterialReport>> GetFaultyMaterialReport(string facility_id, string itemID, DateTime StartDate, DateTime EndDate)
        {
            //old
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
                $"sm_td.fromActorType = '{(int)CMMS.SM_Actor_Types.Inventory}' and sm_td.toActorType = '{(int)CMMS.SM_Actor_Types.Engineer}' and smt.actorType = '{(int)CMMS.SM_Actor_Types.Engineer}' and (sm_td.referedby = '4' OR sm_td.referedby = '{(int)CMMS.CMMS_Modules.SM_GO}') and a_item.item_condition IN (2,3,4)" +
                $" and sm_td.plantID in ('{facility_id}')  AND sm_td.Nature_Of_Transaction = 1 ORDER BY sm_td.ID DESC) as fmItemList GROUP BY fmItemList.assetItemID ORDER BY fmItemList.ID DESC";

            // updated

            string assetCondition = "";
            if (itemID != null && itemID != "")
            {
                assetCondition = $"and a_master.id in ({itemID})";
            }


            string Plant_Stock_Opening_Details_query = $"SELECT smrsitems.serial_number, sm_trans.facilityID as facility_id,sm_td.*, fc.name as facilityName, " +
                $" fc.isBlock as Facility_Is_Block, '' as Facility_Is_Block_of_name,sm_trans.assetItemID, a_master.asset_name, " +
                $" a_master.asset_code, a_master.asset_type_ID, AST.asset_type," +
                $" IFNULL((select sum(ST.creditQty)-sum(ST.debitQty)  FROM smtransition as ST  " +
                $" JOIN smassetmasters as SM ON SM.ID = ST.assetItemID  LEFT JOIN facilities fcc ON fcc.id = ST.facilityID  " +
                $" where   ST.actorType = {(int)CMMS.SM_Actor_Types.Inventory} and SM.ID=a_master.ID  and ST.facilityID in ('{facility_id}')  and" +
                $" date_format(ST.lastModifiedDate, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}'  group by SM.asset_code),0) Opening," +
                $" sum(sm_trans.creditQty) as inward, sum(sm_trans.debitQty) as outward, sm_trans.lastModifiedDate lastInsetedDateTime" +
                $" FROM smtransition as sm_trans " +
                $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID join smtransactiondetails as sm_td on sm_td.id = sm_trans.transactionID" +
                $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
                $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
                $" left join smmrs on smmrs.id = sm_trans.mrsID " +
                $"left join smrsitems on smrsitems.mrs_return_ID = smmrs.id " +
                $" where smrsitems.is_faulty = 1  and sm_trans.facilityID in ('{facility_id}') " +
                $" and date_format(sm_trans.lastModifiedDate, '%Y-%m-%d')  BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' {assetCondition} group by a_master.asset_code;";

            List<CMFaultyMaterialReport> result = await Context.GetData<CMFaultyMaterialReport>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);
            return result;
        }

        public async Task<List<CMEmployeeTransactionReport>> GetEmployeeTransactionReport(int isAllEmployees, string facility_id, int Emp_ID, DateTime StartDate, DateTime EndDate)
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
                $" LEFT JOIN users ed ON ed.id = sm_td.fromActorID" +
                $" WHERE (DATE_FORMAT(sm_td.lastInsetedDateTime, '%Y-%m-%d') BETWEEN '" + StartDate.ToString("yyyy-MM-dd") + "' AND '" + EndDate.ToString("yyyy-MM-dd") + "') AND ";

            if (isAllEmployees != 1)
            {
                EmpStockTransactionDetailsQuery += " sm_td.fromActorID = '" + Emp_ID + "' AND smt.actorID = '" + Emp_ID + "' AND ";
            }

            EmpStockTransactionDetailsQuery += "sm_td.fromActorType = '" + (int)CMMS.SM_Actor_Types.Engineer + "' AND smt.actorType = '" + (int)CMMS.SM_Actor_Types.Engineer + "' AND mrs.flag = 2 AND mrs.approval_status = 1 AND i.mrs_ID = sm_td.reference_ID AND sm_td.plantID in ( '" + facility_id + "')";

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
                if (item.toActorID == Emp_ID || item.toActorType == Convert.ToString((int)CMMS.SM_Actor_Types.Engineer))
                {
                    openingBalance.InwardQty = item.qty;
                }
                else if (item.fromActorID == Emp_ID || item.fromActorType == Convert.ToString((int)CMMS.SM_Actor_Types.Engineer))
                {
                    openingBalance.OutwardQty = item.qty;
                }

                if (item.item_condition == 1)
                {
                    openingBalance.remarks_in_short = "Fresh";
                }
                else if (item.item_condition == 2)
                {
                    openingBalance.remarks_in_short = "Damaged. Return to repair";
                }
                else if (item.item_condition == 3)
                {
                    openingBalance.remarks_in_short = "Damaged. Repaired";
                }
                else if (item.item_condition == 4)
                {
                    openingBalance.remarks_in_short = "Damaged. Return to discard";
                }
                else if (item.item_condition == 5)
                {
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
            List<CMEmpStockItems> itemList = new List<CMEmpStockItems>();
            cMEmployeeStockList.emp_ID = emp_id;
            if (Plant_Stock_Opening_Details_Reader.Count > 0)
            {

                int cnt = 0;
                string plant_name = "";

                cMEmployeeStockList.emp_name = Plant_Stock_Opening_Details_Reader[0].requested_by_name;

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
            }
            cMEmployeeStockList.CMMRSItems = itemList;
            return cMEmployeeStockList;
        }
        internal async Task<List<CMTaskStockItems>> GetpmTaskStock(int facility_ID, int task_id)
        {
            string Plant_Stock_Opening_Details_querys = "SELECT sm_trans.assetItemID, sm.asset_name, sm_trans.facilityID,smrsitems.available_qty FROM smtransition AS sm_trans LEFT JOIN smassetmasters AS sm ON sm_trans.assetitemID = sm.ID left  join smrsitems on  smrsitems.mrs_ID=sm_trans.mrsID  " +
              "WHERE actorID = " + task_id + " AND sm_trans.facilityID = '" + facility_ID + "' ";
            List<CMTaskStockItems> result = await Context.GetData<CMTaskStockItems>(Plant_Stock_Opening_Details_querys).ConfigureAwait(false);
            return result;
        }
        internal async Task<List<CMEmployeeStockTransactionReport>> GetTransactionReport(string facility_ID, int actorType, int actorID, DateTime fromDate, DateTime toDate, string facilitytimeZone)
        {

            List<CMEmployeeStockTransactionReport> EmployeeStockReportList = new List<CMEmployeeStockTransactionReport>();

            string Plant_Stock_Opening_Details_query = "select ST.fromActorID, CASE WHEN fromActorType = 1 then 'Vendor'" +
                " WHEN fromActorType = 2 then 'Store' " +
                " WHEN fromActorType = 5 then 'Engineer' " +
                " WHEN fromActorType = 3 then 'Task' " +
                " WHEN fromActorType = 6 then 'Inventory' End AS fromActorType, " +
                " CASE WHEN fromActorType = 1 then (select Name from business B where B.ID = ST.fromActorID) " +
                " WHEN fromActorType = 2 then (select name  from facilities F where F.ID = ST.fromActorID) " +
                " WHEN fromActorType = 3 then (select concat(p.plan_name,' - Task',T.id) actorName from pm_task T inner join pm_plan P on P.id = T.plan_id where T.id = ST.fromActorID) " +
                " WHEN fromActorType in (5) then (select CONCAT(firstName, ' ', lastName)   from users u where u.ID = ST.fromActorID) " +
                " End AS FromActorName , toActorID as toActorID," +
                " CASE WHEN toActorType = 1 then 'Vendor'  WHEN toActorType = 2 then 'Store' WHEN toActorType = 3 then 'Task' " +
                " WHEN toActorType = 5 then 'Engineer'  WHEN toActorType = 6 then 'Inventory' End AS toActorType, " +
                " CASE WHEN toActorType = 1 then (select Name from business B where B.ID = ST.toActorID) " +
                " WHEN toActorType = 2 then (select name  from facilities F where F.ID = ST.toActorID) " +
                " WHEN toActorType = 3 then (select concat(p.plan_name,' - Task',T.id) actorName from pm_task T inner join pm_plan P on P.id = T.plan_id where T.id = ST.toActorID) " +
                " WHEN toActorType in (5) then (select CONCAT(firstName, ' ', lastName)   from users u where u.ID = ST.toActorID) " +
                " When toActorType = 4 then (select JC_title from jobcards j where j.id= st.toActorID) " +
                " When toActorType = 6 then (select a.name from assets a where a.id= st.toActorID)  End AS toActorName," +
                " ST.assetItemID,am.asset_name as assetItemName, qty , FF.name as facilityName, remarks, lastInsetedDateTime as LastUpdated, " +
                " CONCAT(C.firstName, ' ', C.lastName) CreatedBy, ST.createdAt, smtypes.asset_type " +
                " from smtransactiondetails ST  inner join smtransition smt on smt.transactionID = ST.ID" +
                " inner join smassetmasters am on am.ID = ST.assetItemID left join facilities FF on FF.id = ST.plantID " +
                " left join users C on C.id = ST.createdBy left join smassettypes smtypes on smtypes.ID = am.asset_type_ID " +
                " where smt.actorType = " + actorType + " and  smt.actorID = " + actorID + "  and  date_format(smt.lastModifiedDate, '%Y-%m-%d')  BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "' ";

            if (facility_ID != "" && facility_ID != null)
            {
                Plant_Stock_Opening_Details_query = Plant_Stock_Opening_Details_query + " AND ST.plantID in (" + facility_ID + ")";
            }

            Plant_Stock_Opening_Details_query = Plant_Stock_Opening_Details_query + " group by St.id order by ST.id desc;";

            List<CMEmployeeStockTransactionReport> result = await Context.GetData<CMEmployeeStockTransactionReport>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);
            foreach (var detail in result)
            {
                if (detail != null && detail.createdAt != null)
                    detail.createdAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.createdAt);
                if (detail != null && detail.LastUpdated != null)
                    detail.LastUpdated = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.LastUpdated);
            }

            return result;
        }
        internal async Task<List<CMAssetMasterStockItems>> GetAssetMasterStockItems(int assetID)
        {
            string query = "select SI.asset_code,serial_number, asset_type_id, st.asset_type ,item_category_ID, sc.cat_name item_category, " +
                  "unit_of_measurement as unit_of_measurement_ID , su.name unit_of_measurement  " +
                  "from smassetitems SI " +
                  "inner join smassetmasters SM on SM.asset_code = SI.asset_code " +
                  "LEFT JOIN smunitmeasurement su on su.ID = SM.unit_of_measurement " +
                  "LEFT JOIN smitemcategory sc on sc.ID = SM.item_category_ID " +
                  "LEFT JOIN smassettypes st on st.ID = SM.asset_type_id " +
                  "where SM.ID = " + assetID + " and item_condition=1;";

            List<CMAssetMasterStockItems> result = await Context.GetData<CMAssetMasterStockItems>(query).ConfigureAwait(false);
            return result;
        }

        public async Task<List<CMPlantStockOpeningResponse>> GetStockReport(string facility_id, int actorTypeID, int actorID, DateTime StartDate, DateTime EndDate, string assetMasterIDs)
        {
            List<string> Asset_Item_Ids = new List<string>();
            List<CMPlantStockOpening> Asset_Item_Opening_Balance_details = new List<CMPlantStockOpening>();
            List<CMPlantStockOpeningResponse> Response = new List<CMPlantStockOpeningResponse>();
            string itemCondition = "";
            string Plant_Stock_Opening_Details_query = "";
            if (assetMasterIDs != null && assetMasterIDs != "")
            {
                itemCondition = " AND  a_master.ID  in (" + assetMasterIDs + ") ";
            }


            Plant_Stock_Opening_Details_query = $"SELECT  sm_trans.facilityID as facilityID, fc.name as facilityName," +
                $"fc.isBlock as Facility_Is_Block, " +
                $" '' as Facility_Is_Block_of_name,sm_trans.assetItemID, a_master.asset_name, a_master.asset_code," +
                $" a_master.asset_type_ID, AST.asset_type,  " +
                //$" IFNULL((select sum(ST.creditQty)-sum(ST.debitQty)  FROM smtransition as ST  JOIN smassetmasters as SM ON SM.ID = ST.assetItemID  " +
                //$" LEFT JOIN facilities fcc ON fcc.id = ST.facilityID   where   ST.actorType = {actorTypeID} and SM.ID=a_master.ID  and ST.facilityID in ('{facility_id}')" +
                //$" and sm_trans.actorID = {actorID} and date_format(ST.lastModifiedDate, '%Y-%m-%d') <= '{StartDate.ToString("yyyy-MM-dd")}'  group by SM.asset_code),0) Opening," +
                $" IFNULL((select SUM(smt.qty) from smtransactiondetails smt where date_format(smt.lastInsetedDateTime, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}' and  smt.fromActorType = {(int)CMMS.SM_Actor_Types.Vendor}  and smt.assetItemID = sm_trans.assetItemID and smt.toActorType ={(int)CMMS.SM_Actor_Types.Store} and smt.PlantId in ('{facility_id}') ),0) - " +
                $" IFNULL((select SUM(smt1.qty) from smtransactiondetails smt1  where date_format(smt1.lastInsetedDateTime, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}' and smt1.assetItemID = sm_trans.assetItemID  and smt1.fromActorType IN ({(int)CMMS.SM_Actor_Types.PM_Task},{(int)CMMS.SM_Actor_Types.JobCard},{(int)CMMS.SM_Actor_Types.Engineer}) and smt1.toActorType ={(int)CMMS.SM_Actor_Types.Inventory} and smt1.PlantId in ('{facility_id}')),0) as Opening,  " +
                $"  IFNULL((select SUM(smt.qty) from smtransactiondetails smt where date_format(smt.lastInsetedDateTime, '%Y-%m-%d') " +
                $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' and  smt.fromActorType = {(int)CMMS.SM_Actor_Types.Vendor}  and smt.assetItemID = sm_trans.assetItemID and smt.toActorType ={(int)CMMS.SM_Actor_Types.Store} and smt.PlantId in ('{facility_id}') ),0) as inward, " +
                $"   IFNULL((select SUM(smt1.qty) from smtransactiondetails smt1  where date_format(smt1.lastInsetedDateTime, '%Y-%m-%d') " +
                $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}'  and smt1.assetItemID = sm_trans.assetItemID  and smt1.fromActorType IN ({(int)CMMS.SM_Actor_Types.PM_Task},{(int)CMMS.SM_Actor_Types.JobCard},{(int)CMMS.SM_Actor_Types.Engineer}) and smt1.toActorType ={(int)CMMS.SM_Actor_Types.Inventory} and smt1.PlantId in ('{facility_id}')),0) as outward  " +
                //$"  IFNULL((select sum(si.creditQty) from smtransition si where si.assetItemID = sm_trans.assetItemID and  date_format(si.lastModifiedDate, '%Y-%m-%d') " +
                //$" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' and si.actorType = {actorTypeID} and si.facilityID in ('{facility_id}') and  si.actorID in ({actorID}) ),0) as inward, " +
                // $"   IFNULL((select sum(so.debitQty) from smtransition so where so.assetItemID = sm_trans.assetItemID and  date_format(so.lastModifiedDate, '%Y-%m-%d') " +
                //$" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' and so.actorType = {actorTypeID} and so.facilityID in ('{facility_id}') and  so.actorID in ({actorID})),0) as outward  " +
                $" FROM smtransition as sm_trans " +
                $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID " +
                $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
                $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
                $" where sm_trans.actorType = {actorTypeID} and sm_trans.facilityID in ('{facility_id}') and " +
                $" sm_trans.actorID in ({actorID}) ";

            Plant_Stock_Opening_Details_query = Plant_Stock_Opening_Details_query + itemCondition;
            Plant_Stock_Opening_Details_query = Plant_Stock_Opening_Details_query + " group by a_master.asset_code;";

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
                openingBalance.Opening = item.Opening < 0 ? 0 : item.Opening;
                openingBalance.assetItemID = item.assetItemID;
                openingBalance.asset_name = item.asset_name;
                openingBalance.asset_code = item.asset_code;
                openingBalance.asset_type_ID = item.asset_type_ID;
                openingBalance.asset_type = item.asset_type;
                openingBalance.inward = item.inward;
                openingBalance.outward = item.outward;
                openingBalance.balance = openingBalance.Opening + item.inward - item.outward;
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
                foreach (var itemDetail in itemResponse)
                {
                    CMPlantStockOpeningItemWiseResponse itemWise = new CMPlantStockOpeningItemWiseResponse();
                    itemWise.Facility_Is_Block = itemDetail.Facility_Is_Block;
                    itemWise.Facility_Is_Block_of_name = itemDetail.Facility_Is_Block_of_name;
                    itemWise.assetItemID = itemDetail.assetItemID;
                    itemWise.asset_name = itemDetail.asset_name;
                    itemWise.asset_code = itemDetail.asset_code;
                    itemWise.asset_type_ID = itemDetail.asset_type_ID;
                    itemWise.asset_type = itemDetail.asset_type;
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

        internal async Task<List<CMEmployeeStockTransactionReport>> GetPlantItemTransactionReport(string facility_ID, int assetItemId, DateTime fromDate, DateTime toDate, string facilitytimeZone)
        {

            List<CMEmployeeStockTransactionReport> EmployeeStockReportList = new List<CMEmployeeStockTransactionReport>();

            string Plant_Stock_Opening_Details_query = "select ST.fromActorID, CASE WHEN fromActorType = 1 then 'Vendor'" +
                " WHEN fromActorType = 2 then 'Store' " +
                " WHEN fromActorType = 5 then 'Engineer' " +
                " WHEN fromActorType = 3 then 'Task' " +
                " WHEN fromActorType = 6 then 'Inventory' End AS fromActorType, " +
                " CASE WHEN fromActorType = 1 then (select Name from business B where B.ID = ST.fromActorID) " +
                " WHEN fromActorType = 2 then (select name  from facilities F where F.ID = ST.fromActorID) " +
                " WHEN fromActorType = 3 then (select concat(p.plan_name,' - Task',T.id) actorName from pm_task T inner join pm_plan P on P.id = T.plan_id where T.id = ST.fromActorID) " +
                " WHEN fromActorType in (5) then (select CONCAT(firstName, ' ', lastName)   from users u where u.ID = ST.fromActorID) " +
                " End AS FromActorName , toActorID as toActorID," +
                " CASE WHEN toActorType = 1 then 'Vendor'  WHEN toActorType = 2 then 'Store' WHEN toActorType = 3 then 'Task' " +
                " WHEN toActorType = 5 then 'Engineer'  WHEN toActorType = 6 then 'Inventory' End AS toActorType, " +
                " CASE WHEN toActorType = 1 then (select Name from business B where B.ID = ST.toActorID) " +
                " WHEN toActorType = 2 then (select name  from facilities F where F.ID = ST.toActorID) " +
                " WHEN toActorType = 3 then (select concat(p.plan_name,' - Task',T.id) actorName from pm_task T inner join pm_plan P on P.id = T.plan_id where T.id = ST.toActorID) " +
                " WHEN toActorType in (5) then (select CONCAT(firstName, ' ', lastName)   from users u where u.ID = ST.toActorID) " +
                " When toActorType = 4 then (select JC_title from jobcards j where j.id= st.toActorID) " +
                " When toActorType = 6 then (select a.name from assets a where a.id= st.toActorID)  End AS toActorName," +
                " ST.assetItemID,am.asset_name as assetItemName, qty , FF.name as facilityName, remarks, lastInsetedDateTime as LastUpdated, " +
                " CONCAT(C.firstName, ' ', C.lastName) CreatedBy, ST.createdAt, smtypes.asset_type " +
                " from smtransactiondetails ST  inner join smtransition smt on smt.transactionID = ST.ID" +
                " inner join smassetmasters am on am.ID = ST.assetItemID left join facilities FF on FF.id = ST.plantID " +
                " left join users C on C.id = ST.createdBy left join smassettypes smtypes on smtypes.ID = am.asset_type_ID " +
                " where ST.assetItemID = " + assetItemId + "  and  date_format(smt.lastModifiedDate, '%Y-%m-%d')  BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "' ";

            if (facility_ID != "" && facility_ID != null)
            {
                Plant_Stock_Opening_Details_query = Plant_Stock_Opening_Details_query + " AND ST.plantID in (" + facility_ID + ")";
            }

            Plant_Stock_Opening_Details_query = Plant_Stock_Opening_Details_query + " group by St.id order by ST.id desc;";

            List<CMEmployeeStockTransactionReport> result = await Context.GetData<CMEmployeeStockTransactionReport>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);
            foreach (var detail in result)
            {
                if (detail != null && detail.createdAt != null)
                    detail.createdAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.createdAt);
                if (detail != null && detail.LastUpdated != null)
                    detail.LastUpdated = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)detail.LastUpdated);
            }

            return result;
        }


    }
}

