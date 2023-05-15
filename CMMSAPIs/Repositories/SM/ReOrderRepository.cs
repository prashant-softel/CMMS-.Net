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
using System.ComponentModel;
using Google.Protobuf.WellKnownTypes;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Org.BouncyCastle.Ocsp;
using System.Collections;
using System.Security.Cryptography.Xml;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;

namespace CMMSAPIs.Repositories.SM
{
    public class ReOrderRepository : GenericRepository
    {
        public ReOrderRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<ReOrder>> GetReorderDataByID(int assetID, int plantID)
        {
            string stmt = $"SELECT * FROM smreorderdata WHERE asset_code_ID = {assetID}  AND plant_ID = {plantID}";
            List<ReOrder> _List = await Context.GetData<ReOrder>(stmt).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> submitReorderForm(ReOrder request)
        {
            int ID = 0;
            CMDefaultResponse response = null;
            try
            {
                string stmtSelect = $"SELECT * FROM smreorderdata WHERE asset_code_ID = {request.asset_code_ID} AND plant_ID = {request.plant_ID};";
                List<ReOrder> _List = await Context.GetData<ReOrder>(stmtSelect).ConfigureAwait(false);

                if (_List != null && _List.Count > 0)
                {
                    ID = _List[0].ID;
                    string stmtUpdate = $"UPDATE smreorderdata SET asset_code = '{request.asset_code}', max_qty = {request.max_qty}, min_qty = {request.min_qty} WHERE ID = {ID};";
                    await Context.ExecuteNonQry<int>(stmtUpdate);
                }
                else
                {
                    string stmtInsert = $" START TRANSACTION; " +
                        $" INSERT INTO smreorderdata (asset_code_ID, asset_code, plant_ID, max_qty, min_qty)" +
                        $" VALUES ({request.asset_code_ID}, '{request.asset_code}', {request.plant_ID}, {request.max_qty}, {request.min_qty}); SELECT LAST_INSERT_ID();/* IF @@ERROR <> 0 THEN\r\n    ROLLBACK;\r\nELSE\r\n    COMMIT;\r\n END IF;*/";
                    DataTable dt2 = await Context.FetchData(stmtInsert).ConfigureAwait(false);
                    ID = Convert.ToInt32(dt2.Rows[0][0]);
                }


                response = new CMDefaultResponse(ID, CMMS.RETRUNSTATUS.SUCCESS, "Data saved successfully.");
            }catch(Exception ex)
            {
                response = new CMDefaultResponse(ID, CMMS.RETRUNSTATUS.FAILURE, "Failed to save data.");
            }
            return response;
        }

        internal async Task<CMDefaultResponse> updateReorderData(ReOrder request)
        {
            int ID = 0;
            CMDefaultResponse response = null;
            bool QueryFlag = true;
            try
            {
                string stmtSelect = $"SELECT * FROM smreorderdata WHERE asset_code = '{request.asset_code}' AND plant_ID = {request.plant_ID}";
                List<ReOrder> _List = await Context.GetData<ReOrder>(stmtSelect).ConfigureAwait(false);
                if (_List != null && _List.Count > 0)
                {
                    ID = _List[0].ID;
                    string stmtUpdate = $"UPDATE smreorderdata SET max_qty = {request.max_qty}, min_qty = {request.min_qty} WHERE ID = {ID};";
                    await Context.ExecuteNonQry<int>(stmtUpdate);
                }
                else
                {
                    string stmtInsert = $" START TRANSACTION; " +
                        $" INSERT INTO smreorderdata (asset_code_ID, asset_code, plant_ID, max_qty, min_qty)" +
                        $" VALUES ({request.asset_code_ID}, '{request.asset_code}', {request.plant_ID}, {request.max_qty}, {request.min_qty}); SELECT LAST_INSERT_ID();/* IF @@ERROR <> 0 THEN\r\n    ROLLBACK;\r\nELSE\r\n    COMMIT;\r\n END IF;*/";
                    DataTable dt2 = await Context.FetchData(stmtInsert).ConfigureAwait(false);
                    ID = Convert.ToInt32(dt2.Rows[0][0]);
                }

                response = new CMDefaultResponse(ID, CMMS.RETRUNSTATUS.SUCCESS, "Data saved successfully.");
            }
            catch(Exception ex)
            {
                response = new CMDefaultResponse(ID, CMMS.RETRUNSTATUS.FAILURE, "Failed to save data.");
            }

            return response;
        }

        internal async Task<List<ReOrder>> getReorderAssetsData(int plantID)
        {
            string stmt = $"SELECT srd.*,sam.asset_name,sat.asset_type,sic.cat_name \r\n        " +
                $"FROM smreorderdata srd\r\n        " +
                $"LEFT JOIN smassetmasters sam ON sam.asset_code = srd.asset_code\r\n        " +
                $"LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID\r\n        " +
                $"LEFT JOIN smitemcategory sic ON sic.ID = sam.item_category_ID\r\n       " +
                $" WHERE srd.plant_ID = {plantID} ";
            List<ReOrder> _List = await Context.GetData<ReOrder>(stmt).ConfigureAwait(false);
            return _List;
            
        }
        internal async Task<List<ReOrderItems>> getReorderItems(int plantID)
        {
            string stmt = $"SELECT fc.name as facilityName,fc.isBlock as Facility_Is_Block,'' as Facility_Is_Block_of_name," +
                $"srd.plant_ID,t1.asset_name\r\n,sai.asset_code,t1.asset_type,t1.cat_name,(SUM(st.creditQty)-SUM(st.debitQty)) as availableQty," +
                $"srd.max_qty,srd.min_qty,srd.ID as reorderID,srd.ordered_qty \r\nFROM smtransition st \r\n        " +
                $"LEFT JOIN smassetitems sai ON sai.ID = st.assetItemID\r\n        " +
                $"LEFT JOIN facilities fc ON fc.id = sai.plant_ID\r\n        " +
                $"LEFT JOIN (\r\n            " +
                $"SELECT sam.asset_code,sat.asset_type,sic.cat_name,sam.asset_name FROM smassetmasters sam \r\n           " +
                $" LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID\r\n           " +
                $" LEFT JOIN smitemcategory sic ON sic.ID = sam.item_category_ID\r\n        ) as t1 ON t1.asset_code = sai.asset_code\r\n      " +
                $"  LEFT JOIN smreorderdata srd ON srd.asset_code = sai.asset_code AND srd.plant_ID = sai.plant_ID WHERE sai.plant_ID = {plantID} AND st.actorType = 2 " +
                $"GROUP BY sai.asset_code";
            List<ReOrderItems> _List = await Context.GetData<ReOrderItems>(stmt).ConfigureAwait(false);
            return _List;
            
        }

        internal async Task<CMDefaultResponse> reorderAssets(ReOrder request)
        {
            int ID = 0;
            CMDefaultResponse response = null;
            bool QueryFlag = false;
            try
            {
                var purchaseID = await PurchaseOrder(request.plant_ID, 0, request.emp_ID, request.purchase_date, 1);
                var poUpdate = "UPDATE smpurchaseorder SET reorder_flag = 1 WHERE ID = " + purchaseID + "";
                await Context.ExecuteNonQry<int>(poUpdate);
                for(var  i = 0; i < request.ReOrderAsset.Count; i++)
                {
                    if (request.ReOrderAsset[i].reorderID != null && request.ReOrderAsset[i].reorderID != 0)
                    {
                        string reorderUpdate = $"UPDATE smreorderdata SET max_qty = {request.ReOrderAsset[i].max_qty}, min_qty = {request.ReOrderAsset[i].min_qty}, " +
                            $"ordered_qty = {request.ReOrderAsset[i].ordered_qty} WHERE ID = {request.ReOrderAsset[i].reorderID}";
                        await Context.ExecuteNonQry<int>(reorderUpdate);
                        QueryFlag = true;
                    }
                    else
                    {
                        string reorderInsert = $"INSERT INTO smreorderdata (asset_code,plant_ID,max_qty,min_qty,ordered_qty)" +
                                            $"VALUES('{request.ReOrderAsset[i].asset_code}',{request.plant_ID},{request.ReOrderAsset[i].max_qty},{request.ReOrderAsset[i].min_qty},{request.ReOrderAsset[i].ordered_qty}); SELECT LAST_INSERT_ID();";
                        DataTable dt2 = await Context.FetchData(reorderInsert).ConfigureAwait(false);
                        int reorderInsert_ID = Convert.ToInt32(dt2.Rows[0][0]);
                        QueryFlag = true;
                    }
                    int asset_type_ID = await GetAssetTypeByCode(request.ReOrderAsset[i].asset_code);
                    if(asset_type_ID >= 2)
                    {
                        for(var  j = 0; j < request.ReOrderAsset.Count; j++)
                        {
                            string stmt = $"INSERT INTO smassetitems (plant_ID,asset_code,item_condition,status,location_ID)" + 
                                $"VALUES({request.plant_ID}, '{request.ReOrderAsset[i].asset_code}', 1, 0,0);SELECT LAST_INSERT_ID();";
                            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
                            int asset_item_ID1 = Convert.ToInt32(dt2.Rows[0][0]);
                           await PurchaseOrderDetails(purchaseID, asset_item_ID1, 0, 1, 0, 0, 0, 0,0);
                        }
                        QueryFlag = true;

                    }
                    else
                    {
                        int asset_item_ID1 = await GetAssetItemID(request.ReOrderAsset[i].asset_code, request.plant_ID, 0);
                        await PurchaseOrderDetails(purchaseID, asset_item_ID1, 0, 1, 0, 0, 0, 0, 0);
                        QueryFlag = true;
                    }
                }
                if (QueryFlag)
                {
                    response = new CMDefaultResponse(ID, CMMS.RETRUNSTATUS.SUCCESS, "Reorder initiated. Goto purchase order list to confirm purchase order.");
                }
                else
                {
                    response = new CMDefaultResponse(ID, CMMS.RETRUNSTATUS.FAILURE, "Unable place reorder.");
                }
            }
            catch(Exception  ex)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to save data.");
            }
            return response;
        }

        public async Task<int> GetAssetItemID(string asset_code, int facility_id, int location_ID)
        {
            int asset_item_ID = 0;
            string stmt = $"SELECT ID FROM smassetitems WHERE asset_code = '{asset_code}' AND plant_ID = {facility_id} ORDER BY ID LIMIT 0,1";
            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
            if(dt2.Rows.Count > 0)
            {
                asset_item_ID = Convert.ToInt32(dt2.Rows[0][0]);
            }
            else
            {
                string stmtInsert = $"INSERT INTO smassetitems (plant_ID,asset_code,location_ID,item_condition,status) VALUES ({facility_id},'{asset_code}',{location_ID},1,1);SELECT LAST_INSERT_ID(); ";
                DataTable dtInsert = await Context.FetchData(stmtInsert).ConfigureAwait(false);
                asset_item_ID = Convert.ToInt32(dt2.Rows[0][0]);
            }

            return asset_item_ID;
        }


        public async Task<int> PurchaseOrderDetails(int purchaseID, int assetitemID, int type, float cost, int o_qty, int r_qty, int d_qty, int a_qty, int location)
        {
            int insertedID = -1;

            try
            {
                string stmt = $"INSERT INTO smpurchaseorderdetails (purchaseID, assetItemID, order_type, cost, ordered_qty, location_ID) " +
                              $"VALUES ({purchaseID}, {assetitemID}, {type}, {cost}, {o_qty}, {location}); SELECT LAST_INSERT_ID();";
                
                DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
                insertedID = Convert.ToInt32(dt2.Rows[0][0]);
            }
            catch (MySqlException e)
            {
                Console.WriteLine("MySql Error in PurchaseOrderDetails function: " + e.Message);
            }

            return insertedID;
        }


        public async Task<int> GetAssetTypeByCode(string asset_code)
        {
            try
            {
                string query = $"SELECT asset_type_ID FROM smassetmasters WHERE asset_code = '{asset_code}'";
                DataTable dt = await Context.FetchData(query).ConfigureAwait(false);               

                if (dt.Rows.Count > 0)
                {
                    int asset_type_ID = Convert.ToInt32(dt.Rows[0][0]);
                    return asset_type_ID;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<int> PurchaseOrder(int plantID, int vendorID, int generatedBy, DateTime? purchaseDate, int generateFlag)
        {
            int purchaseID = 0;

            string stmtSelect = $"SELECT * FROM smpurchaseorder WHERE plantID = {plantID} AND vendorID = {vendorID} AND generated_by = {generatedBy} AND purchaseDate = '{purchaseDate.Value.ToString("yyyy-MM-dd")}' LIMIT 0,1";
            DataTable dt2 = await Context.FetchData(stmtSelect).ConfigureAwait(false);
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                purchaseID = Convert.ToInt32(dt2.Rows[0]["ID"]);
                string stmtUpdate = $"UPDATE smpurchaseorder SET generate_flag = {generateFlag}, flag = {generateFlag}, vendorID = {vendorID} WHERE ID = {purchaseID}";
                await Context.ExecuteNonQry<int>(stmtUpdate);
            }
            else
            {


                string stmt = $"INSERT INTO smpurchaseorder (plantID, vendorID, generated_by, purchaseDate, flag, generate_flag , challan_no, po_no, freight, transport, no_pkg_received, lr_no, condition_pkg_received, vehicle_no, gir_no,challan_date, po_date, job_ref, amount, currency, withdrawOn, order_type) " +
                    $"VALUES ({plantID},{vendorID},{generatedBy},'{purchaseDate.Value.ToString("yyyy-MM-dd")}',{generateFlag},{generateFlag}," +
                    $"0,0,0, '','', '', '', '', '', '2000-01-01', '2000-01-01', '', 0, '', '2000-01-01',0 );  SELECT LAST_INSERT_ID();";
                DataTable dtInsert = await Context.FetchData(stmt).ConfigureAwait(false);             
                purchaseID = Convert.ToInt32(dtInsert.Rows[0][0]);
            }

            return purchaseID;
        }

    }
}
