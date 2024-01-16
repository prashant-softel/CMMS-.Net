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
using OfficeOpenXml;
using System.IO;

namespace CMMSAPIs.Repositories.SM
{
    public class SMMasterRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public SMMasterRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        
        internal async Task<List<CMAssetTypes>> GetAssetTypeList(int ID)
        {
            /*
             * Return id, asset_type from SMAssetTypes
            */
            string myQuery = "";
            if (ID == 0)
            {
                myQuery = "SELECT * FROM smassettypes WHERE status = 1";
            }
            else
            {
                myQuery = "SELECT * FROM smassettypes WHERE ID = "+ID+ " and status = 1";
            }
            
            List<CMAssetTypes> _checkList = await Context.GetData<CMAssetTypes>(myQuery).ConfigureAwait(false);
            return _checkList;
        }

        internal async Task<CMDefaultResponse> AddAssetType(CMSMMaster request, int userID)
        {
            /*
             * Add record in SMAssetTypes
            */
            string mainQuery = $"INSERT INTO smassettypes (asset_type,status) VALUES ('" +request.asset_type+"',1); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);

            List<CMAssetTypes> _ViewAssetList = await GetAssetTypeList(id);

            //await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MASTER, id, 0, 0, "Asset Created", CMMS.CMMS_Status.CREATED);
            //CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_CREATED, _ViewAssetList[0]);

            //string strJobStatusMsg = $"Job {id} Created";
            //if (_ViewAssetList[0].ID > 0)
            //{
            //    strJobStatusMsg = $"New Asset type Created with name of " + _ViewAssetList[0].asset_type;
            //    await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, id, 0, 0, "Job Assigned", CMMS.CMMS_Status.CREATED);
            //    CMMSNotification.sendNotification(CMMS.CMMS_Modules.SM_MASTER, CMMS.CMMS_Status.CREATED, _ViewAssetList[0]);
            //}

            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset type added successfully.");
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
            string mainQuery = $"UPDATE smassettypes SET status = 0 where ID = " + Id + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Asset type deleted successfully.");
            return response;
        }

        internal async Task<List<CMItemCategory>> GetAssetCategoryList(int ID)
        {
            /*
             * Return id, name from SMItemCategory
            */
            string myQuery = "";
            if (ID == 0)
            {
                myQuery = "SELECT * FROM SMItemCategory WHERE status = 1";
            }
            else
            {
                myQuery = "SELECT * FROM SMItemCategory WHERE ID = "+ID+ " and status = 1";
            }


            List<CMItemCategory> _List = await Context.GetData<CMItemCategory>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> AddAssetCategory(CMItemCategory request, int userID)
        {
            /*
             * Add record in SMItemCategory
            */
            string mainQuery = $"INSERT INTO SMItemCategory (cat_name,status) VALUES ('" + request.cat_name + "',1); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset category added successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateAssetCategory(CMItemCategory request, int userID)
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
            string mainQuery = $"UPDATE SMItemCategory SET status = 0 where ID = " + acID + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(1, CMMS.RETRUNSTATUS.SUCCESS, "Asset category deleted.");
            return response;
        }

        internal async Task<List<CMUnitMeasurement>> GetUnitMeasurementList(int ID)
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

            List<CMUnitMeasurement> _List = await Context.GetData<CMUnitMeasurement>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> AddUnitMeasurement(CMUnitMeasurement request,int userID)
        {
            string mainQuery = $"INSERT INTO smunitmeasurement (name,flag,decimal_status, spare_multi_selection) VALUES ('"+request.name+"',1,"+request.decimal_status+", "+request.spare_multi_selection+"); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Unit measurement added successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateUnitMeasurement(CMUnitMeasurement request, int userID)
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

        internal async Task<List<CMASSETMASTERLIST>> GetAssetMasterList(int ID)
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
            List<CMASSETMASTERLIST> _List = await Context.GetData<CMASSETMASTERLIST>(myQuery).ConfigureAwait(false);
            return _List;
        }

        internal async Task<CMDefaultResponse> AddAssetMaster(CMSMMaster request, CMAssetMasterFiles fileData, int UserID)
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

        internal async Task<CMDefaultResponse> UpdateAssetMaster(CMSMMaster request, CMAssetMasterFiles fileData, int UserID)
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


        internal async Task<List<CMGETASSETDATALIST>> GetAssetDataList(int facility_id)
        {
            
            string myQuery = "SELECT distinct sat.ID as asset_ID,sm.asset_code,sic.cat_name as CategoryName,sat.serial_number,sm.asset_name," +
                "st.asset_type,if(sm.approval_required=1,'Yes','NO') as approval_required \r\n        FROM smassetitems sat\r\n       " +
                " LEFT JOIN smassetmasters sm ON sm.asset_code = sat.asset_code\r\n        LEFT JOIN smassettypes st ON st.ID = sm.asset_type_ID" +
                " LEFT JOIN smitemcategory sic ON sic.ID = sm.item_category_ID\r\n        " +
                "WHERE sat.facility_ID = "+facility_id+" AND sat.item_condition < 3 AND sat.status >= 1";
            
            List<CMGETASSETDATALIST> _checkList = await Context.GetData<CMGETASSETDATALIST>(myQuery).ConfigureAwait(false);
            return _checkList;
        }
        internal async Task<List<CMVendorList>> GetVendorList()
        {
            string myQuery = "select * from businesstype;";
            List<CMVendorList> list = await Context.GetData<CMVendorList>(myQuery).ConfigureAwait(false);
            return list;
        }

        internal async Task<CMAssetBySerialNo> GetAssetBySerialNo(string serial_number)
        {
            string myQuery = "SELECT * FROM SMAssetItems WHERE serial_number = '"+ serial_number + "';";
            List<CMAssetBySerialNo> items = await Context.GetData<CMAssetBySerialNo>(myQuery).ConfigureAwait(false);
            return items[0];
        }

                internal async Task<List<CMPaidBy>> GetPaidByList(int ID)
        {
            string myQuery = "";
            if (ID > 0)
            {
                myQuery = "select * from smpaidby where id = "+ID+" and status=1;";
            }
            else
            {
                myQuery = "select * from smpaidby where status = 1;";
            }

            List<CMPaidBy> list = await Context.GetData<CMPaidBy>(myQuery).ConfigureAwait(false);
            return list;
        }

        internal async Task<CMDefaultResponse> AddPaidBy(CMPaidBy request, int userID)
        {
            string mainQuery = $"INSERT INTO smpaidby (paid_by,status,created_at, created_by) VALUES ('" + request.paid_by + "',1,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm") +"', "+userID+"); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(mainQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Paid by added successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdatePaidBy(CMPaidBy request, int userID)
        {
            string mainQuery = $"UPDATE smpaidby SET paid_by = '" + request.paid_by + "',updated_at='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"', updated_by = " + userID + " WHERE ID = " + request.ID + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(request.ID, CMMS.RETRUNSTATUS.SUCCESS, "Paid by updated successfully.");
            return response;
        }

        internal async Task<CMDefaultResponse> DeletePaidBy(CMPaidBy request, int userID)
        {
            string mainQuery = $"UPDATE smpaidby SET status = 0 where ID = " + request.ID + "";
            await Context.ExecuteNonQry<int>(mainQuery);
            CMDefaultResponse response = new CMDefaultResponse(request.ID, CMMS.RETRUNSTATUS.SUCCESS, "Paid by deleted.");
            return response;
        }

        internal async Task<CMImportFileResponse> ImportMaterialFile(int file_id, int facility_id, int userID)
        {
            CMImportFileResponse response = null;
            DataTable dt2 = new DataTable();

            string queryCat = "SELECT id, UPPER(name) as name FROM smunitmeasurement GROUP BY name ORDER BY id ASC;";
            DataTable dtCat = await Context.FetchData(queryCat).ConfigureAwait(false);
            List<string> catNames = dtCat.GetColumn<string>("name");
            List<int> catIDs = dtCat.GetColumn<int>("id");
            Dictionary<string, int> unitmeasurement = new Dictionary<string, int>();
            unitmeasurement.Merge(catNames, catIDs);

            string queryType = "SELECT id, UPPER(asset_type) as name FROM smassettypes GROUP BY name ORDER BY id ASC;";
            DataTable dtType = await Context.FetchData(queryType).ConfigureAwait(false);
            List<string> TypeNames = dtType.GetColumn<string>("name");
            List<int> TypeIDs = dtType.GetColumn<int>("id");
            Dictionary<string, int> assettypes = new Dictionary<string, int>();
            assettypes.Merge(TypeNames, TypeIDs);

            string queryCategory = "SELECT id, UPPER(cat_name) as name FROM smitemcategory GROUP BY name ORDER BY id ASC;";
            DataTable dtCategory = await Context.FetchData(queryCategory).ConfigureAwait(false);
            List<string> CategoryNames = dtCategory.GetColumn<string>("name");
            List<int> CategoryIDs = dtCategory.GetColumn<int>("id");
            Dictionary<string, int> itemcategory = new Dictionary<string, int>();
            itemcategory.Merge(CategoryNames, CategoryIDs);

            string queryPlant = "SELECT id, UPPER(name) as name FROM facilities GROUP BY name ORDER BY id ASC;";
            DataTable dtPlant = await Context.FetchData(queryPlant).ConfigureAwait(false);
            List<string> PlantNames = dtPlant.GetColumn<string>("name");
            List<int> PlantIDs = dtPlant.GetColumn<int>("id");
            Dictionary<string, int> itemPlant = new Dictionary<string, int>();
            itemPlant.Merge(PlantNames, PlantIDs);


            Dictionary<string, Tuple<string, Type>> columnNames = new Dictionary<string, Tuple<string, Type>>()
            {
                { "Material", new Tuple<string, Type>("name", typeof(string)) },
                { "Material Description", new Tuple<string, Type>("description", typeof(string)) },
                { "Plant", new Tuple<string, Type>("plantID", typeof(string)) },
                { "Name 1", new Tuple<string, Type>("Name1", typeof(string)) },
                { "Base Unit of Measure", new Tuple<string, Type>("unit_of_measurement", typeof(string)) },
                { "Unrestricted", new Tuple<string, Type>("unrestricted", typeof(int)) },
                { "Value Unrestricted", new Tuple<string, Type>("value_unrestricted", typeof(string)) },
                { "Type", new Tuple<string, Type>("Type", typeof(string)) },
                { "Category", new Tuple<string, Type>("Category", typeof(string)) },
                { "ApprovalRequired", new Tuple<string, Type>("ApprovalRequired", typeof(int)) }

            };

            string query1 = $"SELECT file_path FROM uploadedfiles WHERE id = {file_id};";
            DataTable dt1 = await Context.FetchData(query1).ConfigureAwait(false);
            string path = Convert.ToString(dt1.Rows[0][0]);
            string dir = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);
            if (!Directory.Exists(dir))
                return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"Directory '{dir}' cannot be found");
            else if (!File.Exists(path))
                return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"File '{filename}' cannot be found in directory '{dir}'");
            else
            {
                FileInfo info = new FileInfo(path);
                if (info.Extension == ".xlsx")
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excel = new ExcelPackage(path);
                    var sheet = excel.Workbook.Worksheets["Sheet1"];
                    if (sheet == null)
                    {
                        return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, "Invalid sheet");
                    }
                    else
                    {

                        foreach (var header in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                        {
                            try
                            {
                                dt2.Columns.Add(columnNames[header.Text].Item1, columnNames[header.Text].Item2);
                            }
                            catch (KeyNotFoundException)
                            {
                                dt2.Columns.Add(header.Text);
                            }
                        }
                        List<string> headers = dt2.GetColumnNames();
                        foreach (var item in columnNames.Values)
                        {
                            if (!headers.Contains(item.Item1))
                            {
                                dt2.Columns.Add(item.Item1, item.Item2);
                            }
                        }

                        dt2.Columns.Add("row_no", typeof(int));
                        dt2.Columns.Add("TypeID", typeof(int));
                        dt2.Columns.Add("CategoryID", typeof(int));
                        for (int rN = 2; rN <= sheet.Dimension.End.Row; rN++)
                        {

                            ExcelRange row = sheet.Cells[rN, 1, rN, sheet.Dimension.End.Column];
                            DataRow newR = dt2.NewRow();
                            foreach (var cell in row)
                            {
                                try
                                {
                                    if (cell.Text == null || cell.Text == "")
                                        continue;
                                    newR[cell.Start.Column - 1] = Convert.ChangeType(cell.Text, dt2.Columns[cell.Start.Column - 1].DataType);
                                }
                                catch (Exception ex)
                                {
                                    ex.GetType();
                                    //+ ex.ToString();
                                    //status = status.Substring(0, (status.IndexOf("Exception") + 8));
                                    // m_ErrorLog.SetError("," + status);
                                }
                            }
                            if (newR.IsEmpty())
                            {

                                continue;
                            }
                            newR["name"] = newR[0];
                            newR["description"] = newR[1];
                            try
                            {
                                newR["plantID"] = itemPlant[Convert.ToString(newR[2]).ToUpper()];
                            
                            }
                            catch (KeyNotFoundException)
                            {

                                newR["plantID"] = 0;
                            }
                       
                            newR["Name1"] = newR[3];
                            if (Convert.ToString(newR["description"]) == null || Convert.ToString(newR["description"]) == "")
                            {
                                return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Material Description cannot be null.");
                            }

                            if (Convert.ToString(newR["plantID"]) == null || Convert.ToString(newR["plantID"]) == "")
                            {
                                return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Plant cannot be null.");
                            }

                            try
                            {
                                newR["unit_of_measurement"] = unitmeasurement[Convert.ToString(newR[4]).ToUpper()];

                            }
                            catch (KeyNotFoundException)
                            {

                                return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] unit measurement named '{newR[4]}' does not exist.");
                            }
                            try
                            {
                                newR["TypeID"] = assettypes[Convert.ToString(newR[7]).ToUpper()];

                            }
                            catch (KeyNotFoundException)
                            {

                                return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] asset type '{newR[7]}' does not exist.");
                            }
                            try
                            {
                                newR["CategoryID"] = itemcategory[Convert.ToString(newR[8]).ToUpper()];

                            }
                            catch (KeyNotFoundException)
                            {

                                return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] asset category '{newR[8]}' does not exist.");
                            }
                            newR["unrestricted"] = newR[5];
                            newR["value_unrestricted"] = newR[6];
                            newR["row_no"] = rN;

                         
                          

                            dt2.Rows.Add(newR);
                        }
                        string insertQuery = "INSERT INTO smassetmasters (plant_ID, asset_code, asset_name,description, " +
                            "unit_of_measurement, flag, lastmodifieddate, asset_type_ID, item_category_ID, approval_required)";
                        foreach (DataRow row in dt2.Rows)
                        {
                            insertQuery = insertQuery + $"Select {row.ItemArray[2]},'{row.ItemArray[0]}', '{row.ItemArray[1]}', '{row.ItemArray[1]}'," +
                                $"{row.ItemArray[4]}, 1, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}', {row.ItemArray[11]},{row.ItemArray[12]},{row.ItemArray[9]} UNION ALL ";
                        }
                        int lastIndex = insertQuery.LastIndexOf("UNION ALL ");
                        insertQuery = insertQuery.Remove(lastIndex, "UNION ALL ".Length);
                        var insertedResult = await Context.ExecuteNonQry<int>(insertQuery).ConfigureAwait(false);
                    }
                }
                else //
                {
                    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, "File is not an excel file");

                }
            }
            //string logPath = m_errorLog.SaveAsText($"ImportLog\\ImportMaterial_File{file_id}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}");
            //string logQry = $"UPDATE uploadedfiles SET logfile = '{logPath}' WHERE id = {file_id}";
            //await Context.ExecuteNonQry<int>(logQry).ConfigureAwait(false);
            //logPath = logPath.Replace("\\\\", "\\");
            //if (response == null)
            //    response = new CMImportFileResponse(0, CMMS.RETRUNSTATUS.FAILURE, logPath, null, "Errors found while importing file.");
            //else
            //{

            //   response.error_log_file_path = null;
            //   response.import_log = null;
            //}

            return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.SUCCESS, null, null, "File imported successfully."); 
            //return response;*/
        }
    }
}
