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

namespace CMMSAPIs.Repositories.SM
{
    public class SMMasterRepository : GenericRepository
    {
        public SMMasterRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<CMSMMaster>> GetAssetTypeList(int ID)
        {
            /*
             * Return id, asset_type from SMAssetTypes
            */
            string myQuery = "";
            if (ID == 0)
            {
                myQuery = "SELECT * FROM smassettypes WHERE flag = 1";
            }
            else
            {
                myQuery = "SELECT * FROM smassettypes WHERE ID = "+ID+" and flag = 1";
            }
            
            List<CMSMMaster> _checkList = await Context.GetData<CMSMMaster>(myQuery).ConfigureAwait(false);
            return _checkList;
        }

        internal async Task<CMDefaultResponse> AddAssetType(CMSMMaster request, int userID)
        {
            /*
             * Add record in SMAssetTypes
            */
            string mainQuery = $"INSERT INTO smassettypes (asset_type,flag) VALUES ('" +request.asset_type+"',1); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            CMDefaultResponse  response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset type added successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateAssetType(CMSMMaster request, int userID)
        {
            /*
             * Update record in SMAssetTypes
            */
            string mainQuery = $"UPDATE smassettypes SET asset_type = '" + request.asset_type + "' where ID = "+request.ID+"";
            await Context.ExecuteNonQry<int>(mainQuery); 
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Asset type updated successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> DeleteAssetType(int Id, int userID)
        {
            /*
             * Delete record in SMAssetTypes
            */
            string mainQuery = $"UPDATE smassettypes SET flag = 0 where ID = " + Id + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Asset type deleted successfully.");
            return response;
        }

        internal async Task<List<ItemCategory>> GetAssetCategoryList(int ID)
        {
            /*
             * Return id, name from SMItemCategory
            */
            string myQuery = "";
            if (ID == 0)
            {
                myQuery = "SELECT * FROM SMItemCategory WHERE flag = 1";
            }
            else
            {
                myQuery = "SELECT * FROM SMItemCategory WHERE ID = "+ID+" flag = 1";
            }


            List<ItemCategory> _List = await Context.GetData<ItemCategory>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> AddAssetCategory(ItemCategory request, int userID)
        {
            /*
             * Add record in SMItemCategory
            */
            string mainQuery = $"INSERT INTO SMItemCategory (cat_name,flag) VALUES ('" + request.cat_name + "',1); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset category added successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateAssetCategory(ItemCategory request, int userID)
        {
            /*
             * Update record in SMItemCategory
            */
            string mainQuery = $"UPDATE SMItemCategory SET cat_name = '"+request.cat_name+"' where ID = " + request.ID + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Asset category updated successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> DeleteAssetCategory(int acID, int userID)
        {
            /*
             * Delete record in SMItemCategory
            */
            string mainQuery = $"UPDATE SMItemCategory SET flag = 0 where ID = " + acID + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Asset category deleted.");
            return response;
        }

        internal async Task<List<UnitMeasurement>> GetUnitMeasurementList(int ID)
        {
            string myQuery = "";
            if(ID == 0)
            {
                myQuery = "SELECT * FROM smunitmeasurement WHERE flag = 1";
            }
            else
            {
                myQuery = "SELECT * FROM smunitmeasurement WHERE ID = "+ID+" AND flag = 1";
            }

            List<UnitMeasurement> _List = await Context.GetData<UnitMeasurement>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> AddUnitMeasurement(UnitMeasurement request,int userID)
        {
            string mainQuery = $"INSERT INTO smunitmeasurement (name,flag,decimal_status, spare_multi_selection) VALUES ('"+request.name+"',1,"+request.decimal_status+", "+request.spare_multi_selection+"); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Unit measurement added successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateUnitMeasurement(UnitMeasurement request, int userID)
        {
            string mainQuery = $"UPDATE smunitmeasurement SET name = '"+request.name+"',decimal_status="+request.decimal_status+", spare_multi_selection = "+request.spare_multi_selection+" WHERE ID = "+request.ID+"";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Unit measurement updated successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> DeleteUnitMeasurement(int umID, int userID)
        {
            string mainQuery = $"UPDATE smunitmeasurement SET flag = 0 where ID = " + umID + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Unit measurement deleted.");
            return response;
        }

        internal async Task<List<CMSMMaster>> GetAssetMasterList(int ID)
        {
            /*
             * Return id, name, code, description, asset type, asset categroy, unit measurement, attached files  
             * from SMAssetMasters, SMAssetMasterFiles, SMUnitMeasurement, SMAssetTypes, SMAssetCategory
            */
            string myQuery = "";
            if (ID == 0)
            {
                myQuery = "SELECT sam.ID,sam.asset_type_ID,sam.asset_code,sam.asset_name,sam.description,if(sam.approval_required = 1, 'Yes','No') as approval_required," +
                    " sat.asset_type,sic.cat_name,sm.name as measurement,sm.decimal_status " +
                    " FROM smassetmasters sam LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID " +
                    " LEFT JOIN smitemcategory sic ON sic.ID = sam.item_category_ID  LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement " +
                    " WHERE sam.flag = 1;";
            }
            else
            {
                myQuery = "SELECT sam.ID,sam.asset_type_ID,sam.asset_code,sam.asset_name,sam.description,if(sam.approval_required = 1, 'Yes','No') as approval_required," +
    " sat.asset_type,sic.cat_name,sm.name as measurement,sm.decimal_status " +
    " FROM smassetmasters sam LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID " +
    " LEFT JOIN smitemcategory sic ON sic.ID = sam.item_category_ID  LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement " +
    " WHERE sam.ID = "+ID+" AND sam.flag = 1;";
            }
            List<CMSMMaster> _List = await Context.GetData<CMSMMaster>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> AddAssetMaster(CMSMMaster request, AssetMasterFiles fileData, int UserID)
        {
            /*
             * Add record in SMAssetMasters and SMAssetMasterFiles
            */
            string selectQuery = "SELECT ID FROM smassetmasters WHERE asset_code LIKE '"+request.asset_code+"'";
            List<CMSMMaster> checkingCountList = await Context.GetData<CMSMMaster>(selectQuery).ConfigureAwait(false);
            if (checkingCountList != null && checkingCountList.Count > 0 )
            {
                CMDefaultResponse response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "MDM or Asset Code already exist.");
                return response;
            }
            else
            {
                string insertQuery = @"INSERT INTO smassetmasters (plant_ID,asset_code, asset_name, description, asset_type_ID, item_category_ID, unit_of_measurement, approval_required, flag) " +
                    "VALUES (0,'" +request.asset_code+"', '"+request.asset_name+"', '"+request.asset_description + "', "+request.asset_type_ID+", "+request.item_category_ID + ", "+request.unit_measurement_ID + ", "+ request.approval_required_ID + ", 1); SELECT LAST_INSERT_ID()";

                DataTable dt2 = await Context.FetchData(insertQuery).ConfigureAwait(false);
                int assetID = Convert.ToInt32(dt2.Rows[0][0]);

                if (!string.IsNullOrEmpty(fileData.File_path) && !string.IsNullOrEmpty(fileData.File_size) && !string.IsNullOrEmpty(fileData.File_name) && !string.IsNullOrEmpty(fileData.File_type))
                {
                    string filesInsertQuery = "INSERT INTO smassetmasterfiles(Asset_master_id , File_path, File_name, File_type, File_size) "+
                        "VALUES("+assetID+ ", '"+ fileData.File_path + "', '"+fileData.File_name+ "', '"+fileData.File_type+ "', '"+fileData.File_size+ "')";
                    await Context.ExecuteNonQry<int>(filesInsertQuery);
                }
                CMDefaultResponse response = new CMDefaultResponse(assetID, CMMS.RETRUNSTATUS.SUCCESS, "Asset data successfully inserted.");
                return response;

            }

        }

        internal async Task<CMDefaultResponse> UpdateAssetMaster(CMSMMaster request, AssetMasterFiles fileData, int UserID)
        {
            /*
             * Update record in SMAssetMasters and SMAssetMasterFiles
            */
            CMDefaultResponse response ;

            bool condition = false;
            string status = "", message = "";
            string selectStmt = "SELECT ID FROM smassetmasters WHERE asset_code = '"+request.asset_code + "'";
            List<CMSMMaster> checkingCountList = await Context.GetData<CMSMMaster>(selectStmt).ConfigureAwait(false);
            if (checkingCountList != null && checkingCountList.Count > 0)
            {
                for (var i = 0; i < checkingCountList.Count; i++) { 
                if (checkingCountList[i].ID == request.ID)
                    {
                        condition = true;
                    }
                    else
                    {
                        condition = false;
                        status = "Failed";
                        message = "MDM Code already exists";
                    }
                }
            }
            else
            {
                condition = true;
            }

            if (condition)
            {
                string updateStmt = "UPDATE smassetmasters SET asset_code = '" + request.asset_code+"', asset_name = '"+request.asset_name+"', description = '"+request.asset_description+"',"+
                    " asset_type_ID = "+request.asset_type_ID+", item_category_ID = "+request.item_category_ID+", unit_of_measurement = "+request.unit_measurement_ID+", approval_required = "+request.approval_required_ID+""+
                    " WHERE ID = "+request.ID+"";
                await Context.ExecuteNonQry<int>(updateStmt);

                int assetID = request.ID;
                if (!string.IsNullOrEmpty(fileData.File_path) && !string.IsNullOrEmpty(fileData.File_size) && !string.IsNullOrEmpty(fileData.File_name) && !string.IsNullOrEmpty(fileData.File_type))
                {
                    string filesInsertQuery = "INSERT INTO smassetmasterfiles(Asset_master_id , File_path, File_name, File_type, File_size) " +
                        "VALUES(" + assetID + ", '" + fileData.File_path + "', '" + fileData.File_name + "', '" + fileData.File_type + "', '" + fileData.File_size + "')";
                    await Context.ExecuteNonQry<int>(filesInsertQuery);
                }

                status = "Success";

            }

            if(status == "Failed")
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "MDM Code already exists.");
            }
            else
            {
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Master asset data updated.");
            }

            return response;
        }

        

        internal async Task<CMDefaultResponse> DeleteAssetMaster(CMSMMaster request, int UserID)
        {
            /*
             * Delete record in SMAssetMasters and SMAssetMasterFiles
            */
            CMDefaultResponse response;

            bool condition = false;
            string status = "", message = "";
            string selectStmt = "SELECT ID FROM smassetmasters WHERE asset_code = '" + request.asset_code + "'";
            List<CMSMMaster> checkingCountList = await Context.GetData<CMSMMaster>(selectStmt).ConfigureAwait(false);
            if (checkingCountList != null && checkingCountList.Count > 0)
            {
                for (var i = 0; i < checkingCountList.Count; i++)
                {
                    if (checkingCountList[i].ID == request.ID)
                    {
                        condition = true;
                    }
                    else
                    {
                        condition = false;
                        status = "Failed";
                        message = "MDM Code already exists";
                    }
                }
            }
            else
            {
                condition = true;
            }

            if (condition)
            {
                string deleteStmt = "UPDATE smassetmasters SET flag = 0 WHERE ID = " + request.ID + "";
                await Context.ExecuteNonQry<int>(deleteStmt);
                                

                status = "Success";

            }

            if (status == "Failed")
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "MDM Code already exists.");
            }
            else
            {
                response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Master asset data deleted.");
            }

            return response;
        }
    }
}
