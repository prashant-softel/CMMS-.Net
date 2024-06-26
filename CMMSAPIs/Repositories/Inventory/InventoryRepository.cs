using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using static System.Net.WebRequestMethods;
//using IronXL;

namespace CMMSAPIs.Repositories.Inventory
{
    public class InventoryRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;
        public InventoryRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(iwebhostobj);
        }

        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.INVENTORY_IMPORTED:
                    retValue = "Imported";
                    break;
                case CMMS.CMMS_Status.INVENTORY_ADDED:
                    retValue = "Added";
                    break;
                case CMMS.CMMS_Status.INVENTORY_UPDATED:
                    retValue = "Updated";
                    break;
                case CMMS.CMMS_Status.INVENTORY_DELETED:
                    retValue = "Deleted";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }


        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMViewInventory InvObj)
        {
            string retValue = "Job";

            switch (notificationID)
            {
                case CMMS.CMMS_Status.INVENTORY_IMPORTED:
                    retValue += String.Format("Assets Imported by {0} at {1}</p>", InvObj.added_by, InvObj.Imported_at);
                    break;
                case CMMS.CMMS_Status.INVENTORY_ADDED:
                    retValue += String.Format("Asset {0} Added by {1} at {2}</p>", InvObj.name, InvObj.added_by, InvObj.added_at);
                    break;
                case CMMS.CMMS_Status.INVENTORY_UPDATED:
                    retValue += String.Format("Asset {0} Updated by {1} at {2}</p>", InvObj.name, InvObj.updated_by, InvObj.updated_at);
                    break;
                case CMMS.CMMS_Status.INVENTORY_DELETED:
                    retValue += String.Format("Asset {0} Deleted by {1} at {2}</p>", InvObj.name, InvObj.deleted_by, InvObj.deleted_at);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        internal async Task<CMDefaultResponse> SetParentAsset(int parent, int child, int userID)
        {
            string myQuery = $"UPDATE assets SET parentId = {parent} WHERE id = {child};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, child, 0, 0, "Parent assigned", CMMS.CMMS_Status.UPDATED, userID);
            return new CMDefaultResponse(child, CMMS.RETRUNSTATUS.SUCCESS, $"Asset no. {parent} assigned as parent to Asset no. {child}.");
        }
        internal async Task<CMImportFileResponse> ImportInventories(int file_id, int facility_id, int userID)
        {
            int Total_no_row_excel = 0;
            int Total_Inserted_Rows = 0;

            CMImportFileResponse response = null;
            /*
            string queryAsset = "SELECT id, UPPER(name) as name FROM assets GROUP BY name ORDER BY id ASC;";
            DataTable dtAsset = await Context.FetchData(queryAsset).ConfigureAwait(false);
            List<string> assetNames = dtAsset.GetColumn<string>("name");
            List<int> assetIDs = dtAsset.GetColumn<int>("id");
            Dictionary<string, int> assets = new Dictionary<string, int>();
            assets.Merge(assetNames, assetIDs);/**/

            string queryCat = "SELECT id, UPPER(name) as name FROM assetcategories GROUP BY name ORDER BY id ASC;";
            DataTable dtCat = await Context.FetchData(queryCat).ConfigureAwait(false);
            List<string> catNames = dtCat.GetColumn<string>("name");
            List<int> catIDs = dtCat.GetColumn<int>("id");
            Dictionary<string, int> categories = new Dictionary<string, int>();
            categories.Merge(catNames, catIDs);

            string queryBusiness = "SELECT id, UPPER(name) as name FROM business GROUP BY name ORDER BY id ASC;";
            DataTable dtBusiness = await Context.FetchData(queryBusiness).ConfigureAwait(false);
            List<string> businessNames = dtBusiness.GetColumn<string>("name");
            List<int> businessIDs = dtBusiness.GetColumn<int>("id");
            Dictionary<string, int> businesses = new Dictionary<string, int>();
            businesses.Merge(businessNames, businessIDs);

            string queryBusinessType = "SELECT id, UPPER(name) as name FROM businesstype GROUP BY name ORDER BY id ASC;";
            DataTable dtBusinessType = await Context.FetchData(queryBusinessType).ConfigureAwait(false);
            List<string> businessTypeNames = dtBusinessType.GetColumn<string>("name");
            List<int> businessTypeIDs = dtBusinessType.GetColumn<int>("id");
            Dictionary<string, int> businessTypes = new Dictionary<string, int>();
            businessTypes.Merge(businessTypeNames, businessTypeIDs);

            string queryFacility = $"SELECT id, UPPER(name) as name FROM facilities WHERE parentId = {facility_id} OR id = {facility_id} GROUP BY name ORDER BY id ASC;";
            DataTable dtFacility = await Context.FetchData(queryFacility).ConfigureAwait(false);
            List<string> facilityNames = dtFacility.GetColumn<string>("name");
            List<int> facilityIDs = dtFacility.GetColumn<int>("id");
            Dictionary<string, int> facilities = new Dictionary<string, int>();
            facilities.Merge(facilityNames, facilityIDs);

            string queryPlant = $"SELECT id, UPPER(name) as name FROM facilities WHERE id = {facility_id} GROUP BY name ORDER BY id ASC;";
            DataTable dtPlant = await Context.FetchData(queryPlant).ConfigureAwait(false);
            List<string> plantNames = dtPlant.GetColumn<string>("name");
            List<int> plantIDs = dtPlant.GetColumn<int>("id");
            Dictionary<int, string> plants = new Dictionary<int, string>();
            plants.Merge(plantIDs, plantNames);

            string queryAssetStatus = "SELECT id, UPPER(name) as name FROM assetstatus GROUP BY name ORDER BY id ASC;";
            DataTable dtAssetStatus = await Context.FetchData(queryAssetStatus).ConfigureAwait(false);
            List<string> assetStatusNames = dtAssetStatus.GetColumn<string>("name");
            List<int> assetStatusIDs = dtAssetStatus.GetColumn<int>("id");
            Dictionary<string, int> assetStatuses = new Dictionary<string, int>();
            assetStatuses.Merge(assetStatusNames, assetStatusIDs);

            string queryAssetType = "SELECT id, UPPER(name) as name FROM assettypes GROUP BY name ORDER BY id ASC;";
            DataTable dtAssetType = await Context.FetchData(queryAssetType).ConfigureAwait(false);
            List<string> assetTypeNames = dtAssetType.GetColumn<string>("name");
            List<int> assetTypeIDs = dtAssetType.GetColumn<int>("id");
            Dictionary<string, int> assetTypes = new Dictionary<string, int>();
            assetTypes.Merge(assetTypeNames, assetTypeIDs);

            string queryCurrency = "SELECT id, code FROM currency GROUP BY code ORDER BY id ASC;";
            DataTable dtCurrency = await Context.FetchData(queryCurrency).ConfigureAwait(false);
            List<string> currencyCodes = dtCurrency.GetColumn<string>("code");
            List<int> currencyIDs = dtCurrency.GetColumn<int>("id");
            Dictionary<string, int> currencies = new Dictionary<string, int>();
            currencies.Merge(currencyCodes, currencyIDs);

            /*string queryMapBlocks = "SELECT id, CASE WHEN parentId=0 THEN id ELSE parentId END as parent FROM facilities;";
            DataTable dtMapBlocks = await Context.FetchData(queryMapBlocks).ConfigureAwait(false);
            List<int> blockParents = dtMapBlocks.GetColumn<int>("parent");
            List<int> blockIDs = dtMapBlocks.GetColumn<int>("id");
            Dictionary<int, int> mapBlockToParent = new Dictionary<int, int>();
            mapBlockToParent.Merge(blockIDs, blockParents);/**/

            string queryWarrantyType = "SELECT id, UPPER(name) as name FROM warrantytype GROUP BY name ORDER BY id ASC;";
            DataTable dtWarrantyTypes = await Context.FetchData(queryWarrantyType).ConfigureAwait(false);
            List<string> warrantyTypeNames = dtWarrantyTypes.GetColumn<string>("name");
            List<int> warrantyTypesIDs = dtWarrantyTypes.GetColumn<int>("id");
            Dictionary<string, int> warrantyTypes = new Dictionary<string, int>();
            warrantyTypes.Merge(warrantyTypeNames, warrantyTypesIDs);

            string queryWarrantyTerm = "SELECT id, UPPER(name) as name FROM warrantyusageterm GROUP BY name ORDER BY id ASC;";
            DataTable dtWarrantyTerms = await Context.FetchData(queryWarrantyTerm).ConfigureAwait(false);
            List<string> warrantyTermNames = dtWarrantyTerms.GetColumn<string>("name");
            List<int> warrantyTermIDs = dtWarrantyTerms.GetColumn<int>("id");
            Dictionary<string, int> warrantyTerms = new Dictionary<string, int>();
            warrantyTerms.Merge(warrantyTermNames, warrantyTermIDs);

            string queryAsset = $"SELECT id, REPLACE(UPPER(name), '_', '') as name FROM assets where facilityId = {facility_id} GROUP BY name  ORDER BY id ASC ;";
            DataTable dtAsset = await Context.FetchData(queryAsset).ConfigureAwait(false);
            List<string> assetNames = dtAsset.GetColumn<string>("name");
            List<int> assetIDs = dtAsset.GetColumn<int>("id");
            Dictionary<string, int> assets = new Dictionary<string, int>();
            assets.Merge(assetNames, assetIDs);

            Dictionary<string, Tuple<string, Type>> columnNames = new Dictionary<string, Tuple<string, Type>>()
            {
                { "Plant_Name", new Tuple<string, Type>("siteName", typeof (string)) },
                { "Asset_Name", new Tuple<string, Type>("name", typeof(string)) },
                { "Asset_Description", new Tuple<string, Type>("assetdescription", typeof(string)) },
                { "Asset_Serial_no", new Tuple<string, Type>("serialNumber", typeof(string)) },
                { "DC_Capacity", new Tuple<string, Type>("dcCapacity", typeof(int)) },
                { "AC_Capacity", new Tuple<string, Type>("acCapacity", typeof(int)) },
                { "Module_Quantity", new Tuple<string, Type>("moduleQuantity", typeof(int)) },
                { "Asset_Model_Name", new Tuple<string, Type>("model", typeof(string)) },
                { "Asset_cost", new Tuple<string, Type>("cost", typeof(int)) },
                { "Asset_Currency", new Tuple<string, Type>("currency", typeof(string)) },
                { "Stock_Count", new Tuple<string, Type>("stockCount", typeof(int)) },
                { "Asset_calibration/testing_date", new Tuple<string, Type>("calibrationFirstDueDate", typeof(DateTime)) },
                { "Asset_Last_calibration_date", new Tuple<string, Type>("calibrationLastDate", typeof(DateTime)) },
                { "Asset_calibration_frequency", new Tuple<string, Type>("calibrationFrequency", typeof(int)) },
                { "Calibration_reminder_days", new Tuple<string, Type>("calibrationReminderDays", typeof(int)) },
                { "Warranty_Description", new Tuple<string, Type>("warranty_description", typeof(string)) },
                { "Asset_Warranty_Start_Date", new Tuple<string, Type>("start_date", typeof(DateTime)) },
                { "Asset_Warranty_Expiry_Date", new Tuple<string, Type>("expiry_date", typeof(DateTime)) },
                { "Asset_Warranty_Certificate_No", new Tuple<string, Type>("certificate_number", typeof(string)) },
                { "Asset_Facility_Name", new Tuple<string, Type>("blockName", typeof (string)) },
                { "Asset_category_name", new Tuple<string, Type>("categoryName", typeof(string)) },
                { "Asset_Parent_Name", new Tuple<string, Type>("parentName", typeof(string)) },
                { "Asset_Customer_Name", new Tuple<string, Type>("customerName", typeof(string)) },
                { "Asset_Owner_Name", new Tuple<string, Type>("ownerName", typeof(string)) },
                { "Asset_Operator_Name", new Tuple<string, Type>("operatorName", typeof(string)) },
                { "Asset_Supplier_Name", new Tuple<string, Type>("supplierName", typeof(string)) },
                { "Asset_Manufacturer_Name", new Tuple<string, Type>("manufacturerName", typeof(string)) },
                { "Asset_Type_Name", new Tuple<string, Type>("typeName", typeof(string)) },
                { "Asset_Status_Name", new Tuple<string, Type>("statusName", typeof(string)) },
                { "Warranty Type", new Tuple<string, Type>("warranty_type_name", typeof(string)) },
                { "warranty_term_names", new Tuple<string, Type>("warranty_term_type_name", typeof(string)) },
                { "Asset_warranty_Provider", new Tuple<string, Type>("warranty_provider_name", typeof(string)) },


                { "DC_Rating", new Tuple<string, Type>("dcRating", typeof(string)) },
                { "AC_Rating", new Tuple<string, Type>("acRating", typeof(string)) },
                { "Description_Maintenance", new Tuple<string, Type>("descMaintenace", typeof(string)) },
                { "Warranty Tenure (How many years/month/days)", new Tuple<string, Type>("warrantyTenture", typeof(int)) },
                { "Asset_BarCode", new Tuple<string, Type>("barcode", typeof(string)) },
                { "Asset_UNSPC_Code", new Tuple<string, Type>("unspCode", typeof(string)) },
                { "Asset_Purchase_Code", new Tuple<string, Type>("purchaseCode", typeof(string)) },
               // { "Asset_SPV_Name", new Tuple<string, Type>("spvName", typeof(string)) },
                { "Calibration_Next_Due_date", new Tuple<string, Type>("calibrationNextDueDate", typeof(DateTime)) },
               { "Area", new Tuple<string,Type>("area", typeof(double)) },

            };
            /*
                        dt2.Columns.Add("facilityId", typeof(int));
                        dt2.Columns.Add("blockId", typeof(int));
                        dt2.Columns.Add("categoryId", typeof(int));
                        dt2.Columns.Add("parentId", typeof(int));
                        dt2.Columns.Add("customerId", typeof(int));
                        dt2.Columns.Add("ownerId", typeof(int));
                        dt2.Columns.Add("operatorId", typeof(int));
                        dt2.Columns.Add("supplierId", typeof(int));
                        dt2.Columns.Add("manufacturerId", typeof(int));
                        dt2.Columns.Add("currencyId", typeof(int));
                        dt2.Columns.Add("typeId", typeof(int));
                        dt2.Columns.Add("statusId", typeof(int));
                        dt2.Columns.Add("warranty_type", typeof(int));
                        dt2.Columns.Add("warrranty_term_type", typeof(int));
                        dt2.Columns.Add("warranty_provider_id", typeof(int));
            /**/
            //CMDefaultResponse response = null;
            string query1 = $"SELECT file_path FROM uploadedfiles WHERE id = {file_id};";
            DataTable dt1 = await Context.FetchData(query1).ConfigureAwait(false);
            string path = Convert.ToString(dt1.Rows[0][0]);
            string dir = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);
            if (!Directory.Exists(dir))
                m_errorLog.SetError($"Directory '{dir}' cannot be found");
            else if (!File.Exists(path))
                m_errorLog.SetError($"File '{filename}' cannot be found in directory '{dir}'");
            else
            {
                FileInfo info = new FileInfo(path);
                if (info.Extension == ".xlsx")
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excel = new ExcelPackage(path);
                    var sheet = excel.Workbook.Worksheets["Assets"];
                    if (sheet == null)
                    {
                        //m_errorLog.SetWarning("Sheet containing assets should be named 'Assets'");
                        return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"Sheet containing assets should be named Assets");

                    }
                    else
                    {
                        DataTable dt2 = new DataTable();
                        try
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
                        }
                        catch (Exception ex)
                        {
                            return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, ex.Message);
                        }
                        List<string> headers = dt2.GetColumnNames();
                        foreach (var item in columnNames.Values)
                        {
                            if (!headers.Contains(item.Item1))
                            {
                                dt2.Columns.Add(item.Item1, item.Item2);
                            }
                        }
                        dt2.Columns.Add("facilityId", typeof(int));
                        dt2.Columns.Add("blockId", typeof(int));
                        dt2.Columns.Add("categoryId", typeof(int));
                        dt2.Columns.Add("parentId", typeof(int));
                        dt2.Columns.Add("customerId", typeof(int));
                        dt2.Columns.Add("ownerId", typeof(int));
                        dt2.Columns.Add("operatorId", typeof(int));
                        dt2.Columns.Add("supplierId", typeof(int));
                        dt2.Columns.Add("manufacturerId", typeof(int));
                        dt2.Columns.Add("currencyId", typeof(int));
                        dt2.Columns.Add("typeId", typeof(int));
                        dt2.Columns.Add("statusId", typeof(int));
                        dt2.Columns.Add("warranty_type", typeof(int));
                        dt2.Columns.Add("warranty_term_type", typeof(int));
                        dt2.Columns.Add("warranty_provider_id", typeof(int));
                        dt2.Columns.Add("id", typeof(int));

                        dt2.Columns.Add("row_no", typeof(int));

                        int updateCount = 0;
                        List<int> idList = new List<int>();
                        List<int> updatedIdList = new List<int>();

                        Total_no_row_excel = sheet.Dimension.End.Row - 1;

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
                                m_errorLog.SetInformation($"Row {rN} is empty.");
                                continue;
                            }
                            newR["row_no"] = rN;

                            // string myQuery1 = $"Select name from facilities where id = {facility_id} and parentId = 0";
                            // DataTable dt12 = await Context.FetchData(myQuery1).ConfigureAwait(false);
                            // string PlantName = Convert.ToString(dt12.Rows[0][0]).ToUpper();
                            newR["row_no"] = rN;
                            string siteName = Convert.ToString(newR[0]).ToUpper();

                            if (plants.ContainsValue(siteName))
                            {

                            }
                            else
                            {
                                m_errorLog.SetError($"[Row: {rN}] Invalid Site Name '{newR[0]}'.");
                                newR.Delete();
                                continue;

                            }

                            if (Convert.ToString(newR["name"]) == null || Convert.ToString(newR["name"]) == "")
                            {
                                m_errorLog.SetError($"[Row: {rN}] Name cannot be null.");
                                newR.Delete();
                                continue;
                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Name cannot be null.");
                            }
                            //else if (assets.Contains(Convert.ToString(newR["name"]).ToUpper()))
                            //{
                            //    m_errorLog.SetError($"[Checklist: Row {rN}] Checklist name cannot be duplicate.");
                            //}


                            try
                            {
                                newR["blockId"] = facilities[Convert.ToString(newR["blockName"]).ToUpper()];
                                newR["facilityId"] = facility_id;
                            }
                            catch (KeyNotFoundException)
                            {
                                //string name = Convert.ToString(newR["blockName"]).Replace("_", "");
                                string name = Convert.ToString(newR["blockName"]);
                                string chk_isBlock_added = "select * from facilities where (name = '" + name + "' or name = '" + Convert.ToString(newR["blockName"]) + "')";
                                DataTable dt_block_chk = await Context.FetchData(chk_isBlock_added).ConfigureAwait(false);

                                if (dt_block_chk.Rows.Count == 0)
                                {

                                    string addNewBlock = $"INSERT INTO facilities(name,isBlock, parentId, status, createdBy, createdAt) VALUES " +
                                                            $"('{name}',1, {facility_id},1,{userID},'{UtilsRepository.GetUTCTime()}'); " +
                                                            $"SELECT LAST_INSERT_ID();";
                                    DataTable dt = await Context.FetchData(addNewBlock).ConfigureAwait(false);
                                    int id = Convert.ToInt32(dt.Rows[0][0]);

                                    facilities.Add(name.ToUpper(), id);
                                    newR["blockId"] = id;
                                    newR["facilityId"] = facility_id;

                                    //m_errorLog.SetInformation($"[Row: {rN}] New Block named '{newR["blockName"]}' added in plant '{plants[facility_id]}'.");
                                }
                                else
                                {
                                    newR["blockId"] = Convert.ToInt32(dt_block_chk.Rows[0][0]); ;
                                    newR["facilityId"] = facility_id;
                                }
                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Block named '{newR["blockName"]}' does not exist in plant '{plants[facility_id]}'.");
                            }
                            try
                            {
                                newR["categoryId"] = categories[Convert.ToString(newR["categoryName"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Invalid Asset Category named '{Convert.ToString(newR["categoryName"]).ToUpper()}'.");
                                newR.Delete();
                                continue;
                                //string name = Convert.ToString(newR["categoryName"]);
                                //string queryCat2 = $"SELECT id, UPPER(name) as name FROM assetcategories where name = '{name}' GROUP BY name ;";
                                //List<CMItem> _cat = await Context.GetData<CMItem>(queryCat2).ConfigureAwait(false);
                                //if (_cat.Count > 0)
                                //{
                                //    newR["categoryId"] = _cat[0].id;
                                //}
                                //else
                                //{
                                //    try
                                //    {
                                //        string addCategoryQry = $"INSERT INTO assetcategories(name, description, createdAt, createdBy, status) VALUES " +
                                //                            $"('{name}', '{name}','{UtilsRepository.GetUTCTime()}',{userID}, 1); " +
                                //                            $"SELECT LAST_INSERT_ID();";
                                //        DataTable dt = await Context.FetchData(addCategoryQry).ConfigureAwait(false);
                                //        int id = Convert.ToInt32(dt.Rows[0][0]);
                                //        businesses.Add(name.ToUpper(), id);
                                //        newR["categoryId"] = id;
                                //        m_errorLog.SetInformation($"New Asset Category '{name}' added.");
                                //    }
                                //    catch (KeyNotFoundException)
                                //    {
                                //        m_errorLog.SetError($"[Row: {rN}] Asset Category named '{name}' not found.");
                                //        return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Asset Category named '{name}' not found.");

                                //    }
                                //}
                            }

                            if (newR["area"] != DBNull.Value)
                            {
                                newR["area"] = Convert.ToDouble(newR["area"]);
                            }
                            else
                            {
                                newR["area"] = 0;
                            }


                            try
                            {
                                newR["customerId"] = businesses[Convert.ToString(newR["customerName"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                string name = Convert.ToString(newR["customerName"]);
                                try
                                {
                                    string addBusinessQry = $"INSERT INTO business(name, type) VALUES " +
                                                        $"('{name}', {businessTypes["CUSTOMER"]}); " +
                                                        $"SELECT LAST_INSERT_ID();";
                                    DataTable dt = await Context.FetchData(addBusinessQry).ConfigureAwait(false);
                                    int id = Convert.ToInt32(dt.Rows[0][0]);
                                    businesses.Add(name.ToUpper(), id);
                                    newR["customerId"] = id;
                                    //m_errorLog.SetInformation($"New business '{name}' added.");
                                }
                                catch (KeyNotFoundException)
                                {
                                    m_errorLog.SetError($"[Row: {rN}] Customer business named '{name}' not found.");
                                    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Customer business named '{name}' not found.");

                                }
                            }
                            try
                            {
                                newR["ownerId"] = businesses[Convert.ToString(newR["ownerName"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                string name = Convert.ToString(newR["ownerName"]);
                                try
                                {
                                    string addBusinessQry = $"INSERT INTO business(name, type) VALUES " +
                                                        $"('{name}', {businessTypes["OWNER"]}); " +
                                                        $"SELECT LAST_INSERT_ID();";
                                    DataTable dt = await Context.FetchData(addBusinessQry).ConfigureAwait(false);
                                    int id = Convert.ToInt32(dt.Rows[0][0]);
                                    businesses.Add(name.ToUpper(), id);
                                    newR["ownerId"] = id;
                                    //m_errorLog.SetInformation($"New business '{name}' added.");
                                }
                                catch (KeyNotFoundException)
                                {
                                    m_errorLog.SetError($"[Row: {rN}] Owner business named '{name}' not found.");
                                    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Owner business named '{name}' not found.");

                                }
                            }
                            try
                            {
                                newR["operatorId"] = businesses[Convert.ToString(newR["operatorName"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                string name = Convert.ToString(newR["operatorName"]);
                                try
                                {
                                    string addBusinessQry = $"INSERT INTO business(name, type) VALUES " +
                                                        $"('{name}', {businessTypes["OPERATOR"]}); " +
                                                        $"SELECT LAST_INSERT_ID();";
                                    DataTable dt = await Context.FetchData(addBusinessQry).ConfigureAwait(false);
                                    int id = Convert.ToInt32(dt.Rows[0][0]);
                                    businesses.Add(name.ToUpper(), id);
                                    newR["operatorId"] = id;
                                    //m_errorLog.SetInformation($"New business '{name}' added.");
                                }
                                catch (KeyNotFoundException)
                                {
                                    m_errorLog.SetError($"[Row: {rN}] Operator business named '{name}' not found.");
                                    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Operator business named '{name}' not found.");

                                }
                            }
                            try
                            {
                                newR["supplierId"] = businesses[Convert.ToString(newR["supplierName"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                string name = Convert.ToString(newR["supplierName"]);
                                try
                                {
                                    string addBusinessQry = $"INSERT INTO business(name, type) VALUES " +
                                                        $"('{name}', {businessTypes["SUPPLIER"]}); " +
                                                        $"SELECT LAST_INSERT_ID();";
                                    DataTable dt = await Context.FetchData(addBusinessQry).ConfigureAwait(false);
                                    int id = Convert.ToInt32(dt.Rows[0][0]);
                                    businesses.Add(name.ToUpper(), id);
                                    newR["supplierId"] = id;
                                    //m_errorLog.SetInformation($"New business '{name}' added.");
                                }
                                catch (KeyNotFoundException)
                                {
                                    m_errorLog.SetError($"[Row: {rN}] Supplier business named '{name}' not found.");
                                    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Supplier business named '{name}' not found.");

                                }
                            }
                            try
                            {
                                newR["manufacturerId"] = businesses[Convert.ToString(newR["manufacturerName"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                string name = Convert.ToString(newR["manufacturerName"]);
                                try
                                {
                                    string addBusinessQry = $"INSERT INTO business(name, type) VALUES " +
                                                        $"('{name}', {businessTypes["MANUFACTURER"]}); " +
                                                        $"SELECT LAST_INSERT_ID();";
                                    DataTable dt = await Context.FetchData(addBusinessQry).ConfigureAwait(false);
                                    int id = Convert.ToInt32(dt.Rows[0][0]);
                                    businesses.Add(name.ToUpper(), id);
                                    newR["manufacturerId"] = id;
                                    //m_errorLog.SetInformation($"New business '{name}' added.");
                                }
                                catch (KeyNotFoundException)
                                {
                                    m_errorLog.SetError($"[Row: {rN}] Manufacturer business named '{name}' not found.");
                                    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Manufacturer business named '{name}' not found.");

                                }
                            }
                            try
                            {
                                newR["currencyId"] = currencies[Convert.ToString(newR["currency"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (newR["currency"] != null && (Convert.ToString(newR["currency"]) != ""))
                                    m_errorLog.SetError($"[Row: {rN}] Currency with code '{newR["currency"]}' not found.");
                                else
                                    newR["currencyId"] = 0;
                            }
                            try
                            {
                                newR["typeId"] = assetTypes[Convert.ToString(newR["typeName"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["typeName"]) == "" || Convert.ToString(newR["typeName"]) == null || newR["typeName"] == null)
                                {
                                    newR["typeId"] = assetTypes["MAIN INVENTRY"];
                                }

                                else
                                {
                                    string addAssetsType = $"INSERT INTO assettypes(name, description, addedAt, Status, AddedBy) VALUES " +
                                             $"('{Convert.ToString(newR["typeName"]).ToUpper()}','{Convert.ToString(newR["typeName"]).ToUpper()}','{UtilsRepository.GetUTCTime()}',1, {userID}); " +
                                             $"SELECT LAST_INSERT_ID();";
                                    DataTable dt = await Context.FetchData(addAssetsType).ConfigureAwait(false);
                                    int id = Convert.ToInt32(dt.Rows[0][0]);
                                    assetTypes.Add(Convert.ToString(newR["typeName"]).ToUpper(), id);
                                    newR["typeId"] = id;
                                    //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Asset type named '{newR["typeName"]}' not found.");

                                }

                            }
                            try
                            {
                                newR["statusId"] = assetStatuses[Convert.ToString(newR["statusName"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["statusName"]) == "" || Convert.ToString(newR["statusName"]) == null || newR["statusName"] == null)
                                    newR["statusId"] = assetStatuses["IN OPERATION"];
                                else
                                {
                                    string addAssets = $"INSERT INTO assetstatus(name, description, addedAt, Status, AddedBy) VALUES " +
                                                    $"('{Convert.ToString(newR["statusName"]).ToUpper()}','{Convert.ToString(newR["statusName"]).ToUpper()}','{UtilsRepository.GetUTCTime()}',1, {userID}); " +
                                                    $"SELECT LAST_INSERT_ID();";
                                    DataTable dt = await Context.FetchData(addAssets).ConfigureAwait(false);
                                    int id = Convert.ToInt32(dt.Rows[0][0]);
                                    assetStatuses.Add(Convert.ToString(newR["statusName"]).ToUpper(), id);
                                    newR["statusId"] = id;

                                    // return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Asset status named '{newR["statusName"]}' not found.");

                                }


                            }
                            try
                            {
                                newR["warranty_type"] = warrantyTypes[Convert.ToString(newR["warranty_type_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                //m_errorLog.SetWarning($"[Row: {rN}] Warranty Type named '{newR["warranty_type_name"]}' not found. Setting warranty Type ID as 0.");
                                newR["warranty_type"] = 0;
                            }
                            try
                            {
                                newR["warranty_term_type"] = warrantyTerms[Convert.ToString(newR["warranty_term_type_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                //m_errorLog.SetWarning($"[Row: {rN}] Warranty Term named '{newR["warranty_term_type_name"]}' not found. Setting warranty term ID as 0.");
                                newR["warranty_term_type"] = 0;
                            }
                            try
                            {
                                newR["warranty_provider_id"] = businesses[Convert.ToString(newR["warranty_provider_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                string name = Convert.ToString(newR["warranty_provider_name"]);
                                try
                                {
                                    string addBusinessQry = $"INSERT INTO business(name, type) VALUES " +
                                                        $"('{name}', {businessTypes["MANUFACTURER"]}); " +
                                                        $"SELECT LAST_INSERT_ID();";
                                    DataTable dt = await Context.FetchData(addBusinessQry).ConfigureAwait(false);
                                    int id = Convert.ToInt32(dt.Rows[0][0]);
                                    businesses.Add(name.ToUpper(), id);
                                    newR["warranty_provider_id"] = id;
                                    //m_errorLog.SetInformation($"New business '{name}' added.");
                                }
                                catch (KeyNotFoundException)
                                {
                                    //m_errorLog.SetWarning($"[Row: {rN}] Warranty Provider named '{name}' not found. Setting warranty provider ID as 0.");
                                    newR["warranty_provider_id"] = 0;
                                }
                            }

                            if (!(Convert.ToInt32(newR["warranty_type"]) > 0 && Convert.ToInt32(newR["warranty_term_type"]) > 0 && Convert.ToInt32(newR["warranty_provider_id"]) > 0))
                            {
                                //m_errorLog.SetWarning($"[Row: {rN}] Warranty data does not exist. ");
                            }

                            if (Convert.ToString(newR["parentName"]) == null || Convert.ToString(newR["parentName"]) == "")
                            {
                                newR["parentName"] = "";
                            }
                            try
                            {
                                newR["id"] = assets[Convert.ToString(newR["name"]).Replace("_", "").ToUpper()];
                                try
                                {
                                    newR["parentID"] = assets[Convert.ToString(newR["parentName"]).Replace("_", "").ToUpper()];
                                }
                                catch
                                {
                                    newR["parentID"] = 0;
                                }

                                string myQuery = $"Select id from assets where id = {newR["id"]} and parentId = {newR["parentID"]}";
                                DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
                                //int id = Convert.ToInt32(dt.Rows[0][0]);
                                //int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
                                if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0)
                                {

                                    DataTable updateTable = dt2.Clone();
                                    DataRow newRow = updateTable.NewRow();


                                    newRow.ItemArray = newR.ItemArray;
                                    updateTable.Rows.Add(newRow);


                                    //CMAddInventory updateAsset = new CMAddInventory();    
                                    List<CMAddInventory> updateAsset = updateTable.MapTo<CMAddInventory>();

                                    var resPlan = await UpdateInventory(updateAsset[0], userID);
                                    updateCount++;
                                    updatedIdList.Add(Convert.ToInt32(newR["id"]));
                                    newR.Delete();
                                    continue;

                                }

                                //CMPMPlanDetail updatePlan = new CMPMPlanDetail();

                                //updatePlan.plan_id = Convert.ToInt32(newR["planID"]);
                                //updatePlan.plan_name = Convert.ToString(newR["PlanName"]);
                                //updatePlan.plan_date = startDate;
                                //updatePlan.plan_freq_id = Convert.ToInt32(newR["frequencyID"]);
                                //updatePlan.facility_id = Convert.ToInt32(newR["plantID"]);
                                //updatePlan.category_id = Convert.ToInt32(newR["categoryID"]);
                                //updatePlan.assigned_to_id = Convert.ToInt32(newR["assignedToID"]);
                                //var resPlan = await UpdatePMPlan(updatePlan, userID);
                                //updateCount++;
                                //string myQuery2 = $"Update pm_plan set status = {(int)CMMS.CMMS_Status.PM_PLAN_CREATED},approved_by = null where id = {updatePlan.plan_id};";
                                //await Context.ExecuteNonQry<int>(myQuery2).ConfigureAwait(false);
                                //int app = Convert.ToInt32(newR["Approval"]);
                                //if (app == 1)
                                //{
                                //    CMApproval planApproval = new CMApproval
                                //    {
                                //        id = Convert.ToInt32(newR["planID"]),
                                //        comment = "Approved"
                                //    };

                                //    approval.Add(planApproval);
                                //}
                                ////m_errorLog.SetWarning($"[Row: {rN}] Updated Plan '{newR["PlanName"]}'. Plan ID :{newR["planID"]}. ");


                            }
                            catch (KeyNotFoundException)
                            {

                                //return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, $"[Row: {rN}] Equipment named '{newR["EquipmentName"]}' does not exist.");
                            }
                            /*
                        dt2.Columns.Add("warranty_type", typeof(int));
                        dt2.Columns.Add("warrranty_term_type", typeof(int));
                        dt2.Columns.Add("warranty_provider_id", typeof(int));*/
                            dt2.Rows.Add(newR);
                        }
                        //excel data exctrat complete
                        string updateQry = $"UPDATE facilities as blocks JOIN facilities as plants ON blocks.parentId = plants.id SET " +
                               $"blocks.customerId = plants.customerId,blocks.spvId = plants.spvId, blocks.ownerId = plants.ownerId, blocks.operatorId = plants.operatorId, " +
                               $"blocks.countryId = plants.countryId, blocks.stateId = plants.stateId, blocks.cityId = plants.cityId, blocks.address = plants.name, " +
                               $"blocks.country = plants.country, blocks.state = plants.state, blocks.city = plants.city, blocks.timezone = plants.timezone, " +
                               $"blocks.zipcode = plants.zipcode, blocks.latitude = plants.latitude, blocks.longitude = plants.longitude;";
                        await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);

                        //if (m_errorLog.GetErrorCount() == 0)
                        //{
                        int childListCount = 1;
                        DataTable insertedTable = dt2.Clone();

                        string filter = "";
                        string ids = "";
                        string assetQry = "";

                        for (int i = 0; i < childListCount; i++)
                        {
                            if (i == 0)
                            {
                                //assetQry = $"SELECT REPLACE(UPPER(name), '_', '') as name FROM facilities WHERE parentId = {facility_id} union" +
                                assetQry = $" SELECT REPLACE(UPPER(name), '_', '') as name FROM assets WHERE facilityId = {facility_id} {filter};";
                            }
                            else
                            {
                                assetQry = $"SELECT UPPER(name) as name FROM assets WHERE facilityId = {facility_id} {filter};";
                            }

                            DataTable assetDt = await Context.FetchData(assetQry).ConfigureAwait(false);

                            List<List<string>> assetList = new List<List<string>>() { assetDt.GetColumn<string>("name") };

                            List<DataTable> assetPriority = new List<DataTable>();

                            List<string> assetnames = new List<string>();
                            assetnames.AddRange(assetDt.GetColumn<string>("name"));
                            //assetnames.AddRange(dt2.GetColumn<string>("name"));
                            assetnames.Contains("");
                            //DataRow[] filterRows = dt2.AsEnumerable()
                            //           .Where(row => assetnames.Contains(row.Field<string>("parentName").Replace("_", "").ToUpper(), StringComparison.OrdinalIgnoreCase) ||
                            //            assetnames.Contains(ConvertString(row.Field<string>("parentName")).Replace("_", "").ToUpper(), StringComparison.OrdinalIgnoreCase))
                            //           .ToArray();

                            DataRow[] filterRows = dt2.AsEnumerable()
                                        .Where(row => assetnames.Contains(row.Field<string>("parentName").Replace("_", "").ToUpper(), StringComparison.OrdinalIgnoreCase))
                                        .ToArray();


                            if (filterRows.Length <= 0)
                            {
                                List<string> assetnames2 = new List<string>();
                                assetnames2.AddRange(insertedTable.GetColumn<string>("name"));
                                DataRow[] notInserted = dt2.AsEnumerable()
                                       .Where(row => !assetnames2.Contains(row.Field<string>("name")))
                                       .ToArray();

                                DataTable notInsertedTable = dt2.Clone();

                                foreach (DataRow row in notInserted)
                                {
                                    notInsertedTable.ImportRow(row);
                                }
                                List<int> ids_ = (await AddInventoryWithParent(notInsertedTable, facility_id, userID)).id;
                                idList.AddRange(ids_);

                                //foreach (DataRow row in notInserted)
                                //{
                                //    m_errorLog.SetInformation($"Asset <{row.Field<string>("name")}> not inserted");
                                //}

                                break;
                            }
                            else
                            {
                                DataTable filteredDataTable = dt2.Clone();

                                foreach (DataRow row in filterRows)
                                {
                                    filteredDataTable.ImportRow(row);
                                }
                                //List<int> ids_ = (await AddInventoryWithParent(filteredDataTable, facility_id, userID)).id;
                                List<int> ids_ = (await UpdateInventoryWithParent(filteredDataTable, facility_id, userID)).id;
                                insertedTable.Merge(filteredDataTable);
                                childListCount++;
                                ids = string.Join(", ", ids_.Select(x => x.ToString()));
                                if (ids != "")
                                {
                                    filter = $" and id IN ({ids})";
                                }
                                idList.AddRange(ids_);

                            }
                            //if (filterRows.Length > 0)
                            //{
                            //    assetPriority.Insert(0, filterRows.CopyToDataTable());
                            //    assetList.Insert(0, assetPriority[assetPriority.Count - 1].GetColumn<string>("name"));
                            //}

                            //foreach (var item in assetList)
                            //{
                            //    List<string> temp = item;
                            //    do
                            //    {
                            //        filterRows = dt2.AsEnumerable()
                            //           .Where(row => temp.Contains(row.Field<string>("parentName"), StringComparison.OrdinalIgnoreCase))
                            //           .ToArray();
                            //        if (filterRows.Length == 0)
                            //            continue;
                            //        assetPriority.Add(filterRows.CopyToDataTable());
                            //        temp = assetPriority[assetPriority.Count - 1].GetColumn<string>("name");
                            //    } while (filterRows.Length != 0);
                            //}
                            //List<int> idList = new List<int>();


                            //foreach (DataTable dtUsers in assetPriority)
                            //{
                            //    idList.AddRange((await AddInventoryWithParent(dtUsers, facility_id, userID)).id);
                            //}
                        }

                        string checkParentID = $"select id,name from assets  where parentId = 0 and facilityId = {facility_id} GROUP BY name  ORDER BY id ASC ";
                        DataTable dt_checkingParentID = await Context.FetchData(checkParentID).ConfigureAwait(false);
                        List<string> assetParentName = dt_checkingParentID.GetColumn<string>("name");
                        List<int> assetParentIDs = dt_checkingParentID.GetColumn<int>("id");
                        Dictionary<string, int> diccheckParentID = new Dictionary<string, int>();
                        diccheckParentID.Merge(assetParentName, assetParentIDs);

                        string queryAssetWithParent = $"SELECT id, REPLACE(UPPER(name), '_', '') as name FROM assets where facilityId = {facility_id} GROUP BY name  ORDER BY id ASC ;";
                        DataTable dtAssetWithParent = await Context.FetchData(queryAssetWithParent).ConfigureAwait(false);
                        List<string> assetNamesWithParent = dtAssetWithParent.GetColumn<string>("name");
                        List<int> assetIDsWithParent = dtAssetWithParent.GetColumn<int>("id");
                        Dictionary<string, int> assetsWithParent = new Dictionary<string, int>();
                        assetsWithParent.Merge(assetNames, assetIDs);

                        DataTable dt_updateParentIDs = new DataTable();

                        var result = from parentName in dt2.AsEnumerable()
                                     join row2 in dtAssetWithParent.AsEnumerable() on parentName.Field<string>("parentName") equals row2.Field<string>("name") into gj
                                     from subRow in gj.DefaultIfEmpty()
                                     where subRow == null
                                     select parentName;

                        // Create the third DataTable
                        DataTable dt3 = dt2.Clone(); // Clone the structure of dt1
                        foreach (DataRow row in result)
                        {
                            dt3.ImportRow(row);
                        }
                        for (int i = 0; i < diccheckParentID.Count; i++)
                        {

                        }


                        Total_Inserted_Rows = updatedIdList.Count + idList.Count;
                        response = new CMImportFileResponse(file_id, idList, updatedIdList, CMMS.RETRUNSTATUS.SUCCESS, null, null, $"{idList.Count} new assets added.{updatedIdList.Count} assets Updated.");

                        // }
                    }
                }
                else //
                {
                    m_errorLog.SetError("File is not an excel file");
                    return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, "File is not an excel file");

                }
            }
            string logPath = m_errorLog.SaveAsText($"ImportLog\\ImportInventories_File{file_id}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}");
            string logQry = $"UPDATE uploadedfiles SET logfile = '{logPath}' WHERE id = {file_id}";
            await Context.ExecuteNonQry<int>(logQry).ConfigureAwait(false);
            logPath = logPath.Replace("\\\\", "\\");
            if (response == null)
                response = new CMImportFileResponse(0, CMMS.RETRUNSTATUS.FAILURE, logPath, m_errorLog.errorLog(), "Errors found while importing file.");
            else
            {
                //m_errorLog.SetImportInformation("File imported successfully");
                m_errorLog.SetImportInformation("Total Rows Inserted-Updated " + Total_Inserted_Rows + " Out of  Total Excel Record  " + Total_no_row_excel);
                response.error_log_file_path = logPath;
                response.import_log = m_errorLog.errorLog();
            }

            return response;
            //return response;*/
        }
        static string ConvertString(string input)
        {
            // Define a regular expression pattern to match the parts of the input string
            string pattern = @"^(.+)_SMB(\d+)\.(\d+)$";

            // Use regular expressions to match and capture the parts
            Match match = Regex.Match(input, pattern);

            if (match.Success && match.Groups.Count == 4)
            {
                // Rearrange the captured parts into the desired format
                string prefix = match.Groups[1].Value;
                string smbPart = match.Groups[2].Value;
                string decimalPart = match.Groups[3].Value;

                // Create the converted string
                string convertedString = $"{prefix}_INV_{smbPart}_SMB_{decimalPart}";

                return convertedString;
            }

            // Return the original input if it doesn't match the pattern
            return input;
        }

        internal async Task<CMDefaultResponse> AddInventoryWithParent(DataTable assets, int facility_id, int userID)
        {
            string assetQry = $" SELECT id, REPLACE(UPPER(name), '_', '') as name FROM assets WHERE facilityId = {facility_id} GROUP BY name ORDER BY id;";
            DataTable dtAsset = await Context.FetchData(assetQry).ConfigureAwait(false);
            List<string> assetNames = dtAsset.GetColumn<string>("name");
            List<int> assetIds = dtAsset.GetColumn<int>("id");
            Dictionary<string, int> assetDict = new Dictionary<string, int>();
            assetDict.Merge(assetNames, assetIds);

            /*string assetQry2 = $"SELECT id, REPLACE(UPPER(name), '_', '') as name FROM facilities WHERE parentId = {facility_id}  GROUP BY name ORDER BY id;";
            DataTable dtAsset2 = await Context.FetchData(assetQry2).ConfigureAwait(false);
            List<string> assetNames2 = dtAsset2.GetColumn<string>("name");
            List<int> assetIds2 = dtAsset2.GetColumn<int>("id");
            Dictionary<string, int> assetDict2 = new Dictionary<string, int>();
            assetDict2.Merge(assetNames2, assetIds2);*/

            foreach (DataRow row in assets.Rows)
            {
                try
                {
                    row["parentId"] = assetDict[Convert.ToString(row["parentName"]).ToUpper()];
                }
                catch (KeyNotFoundException)
                {

                    //m_errorLog.SetWarning($"[Row: {row["row_no"]}] Asset named '{Convert.ToString(row["parentName"])}' '{Convert.ToString(row["parentName"]).Replace("_", "").ToUpper()}'not found. Setting parent ID as 0.");
                    row["parentId"] = 0;

                }
            }

            List<CMAddInventory> importAssets = assets.MapTo<CMAddInventory>();
            CMDefaultResponse response = await CreateInventory(importAssets, userID);
            return response;
        }
        //update
        internal async Task<CMDefaultResponse> UpdateInventoryWithParent(DataTable assets, int facility_id, int userID)
        {
            string assetQry = $" SELECT id, REPLACE(UPPER(name), '_', '') as name FROM assets WHERE facilityId = {facility_id} GROUP BY name ORDER BY id;";
            DataTable dtAsset = await Context.FetchData(assetQry).ConfigureAwait(false);
            List<string> assetNames = dtAsset.GetColumn<string>("name");
            List<int> assetIds = dtAsset.GetColumn<int>("id");
            Dictionary<string, int> assetDict = new Dictionary<string, int>();
            assetDict.Merge(assetNames, assetIds);

            string assetQry2 = $"SELECT id, REPLACE(UPPER(name), '_', '') as name FROM facilities WHERE parentId = {facility_id} GROUP BY name ORDER BY id;";
            DataTable dtAsset2 = await Context.FetchData(assetQry2).ConfigureAwait(false);
            List<string> assetNames2 = dtAsset2.GetColumn<string>("name");
            List<int> assetIds2 = dtAsset2.GetColumn<int>("id");
            Dictionary<string, int> assetDict2 = new Dictionary<string, int>();
            assetDict2.Merge(assetNames2, assetIds2);

            foreach (DataRow row in assets.Rows)
            {
                try
                {
                    row["parentId"] = assetDict[Convert.ToString(row["parentName"]).Replace("_", "").ToUpper()];
                }
                catch (KeyNotFoundException)
                {
                    try
                    {
                        row["parentId"] = assetDict2[Convert.ToString(row["parentName"]).ToUpper()];
                        row["parentId"] = 0;
                    }
                    catch
                    {
                        //m_errorLog.SetWarning($"[Row: {row["row_no"]}] Asset named '{Convert.ToString(row["parentName"])}' '{Convert.ToString(row["parentName"]).Replace("_", "").ToUpper()}'not found. Setting parent ID as 0.");
                        row["parentId"] = 0;
                    }
                }
            }

            List<CMAddInventory> importAssets = assets.MapTo<CMAddInventory>();
            CMDefaultResponse response = await UpdateInventory(importAssets, userID);
            return response;
        }

        internal async Task<List<CMInventoryList>> GetInventoryList(int facilityId, int linkedToBlockId, int status, string categoryIds, string facilitytimeZone)
        {
            /*
             * get all details mentioned in model
             * Primary Table - Assets
             * Asset_category - asset category nam
             * AssetWarranty - warranty related information 
             * AssetType - type of inventory 
             * AssetSatus - status of inventory, 
             * Business - owner, operator, customer
            */
            /*Your code goes here*/
            string myQuery =
            " select a.id, a.name,a.moduleQuantity, a.description, ast.name as type, b2.name as supplierName, b5.name as operatorName, a.categoryId, ac.name as categoryName, a.serialNumber,a.specialTool, a.warrantyId,f.name AS facilityName, f.id AS facilityId, bl.id AS blockId, bl.name AS blockName,linkedbl.id AS linkedToBlockId, linkedbl.name AS linkedToBlockName, a.parentId, a2.name as parentName, custbl.name as customerName, owntbl.name as ownerName, s.name AS status, a.dccapacity,a.acrating, a.dcRating, a.acCapacity, a.descMaintenace, wt.name as warrantyType, wp.id as warrantyProviderId, wp.name as warrantyProviderName, " +
            "w.start_date,w.expiry_date,w.warrantyTenture,w.certificate_number,a.cost,cy.name as currency,a.barcode,a.unspCode,a.purchaseCode,a.calibrationDueDate,a.calibrationReminderDays,a.calibrationFrequency,a.calibrationLastDate,a.calibrationDueDate as calibration_testing_date,a.model,a.supplierId,a.manufacturerId,manufacturertlb.name as manufacturername " +

            " from assets as a " +
            "left join assettypes as ast on ast.id = a.typeId " +
            "" +

            "left join assetwarranty as w ON a.warrantyId = w.id " +
             "" +
            "left join warrantytype as wt ON w.warranty_type = wt.id " +
             "" +
            "left join business as wp ON w.warranty_provider = wp.id " +
            "left JOIN business AS manufacturertlb ON a.ownerId = manufacturertlb.id " +
            "left JOIN currency as cy on a.id = cy.id " +

            "left join assetcategories as ac on ac.id= a.categoryId " +
            "" +
            "left join business as custbl on custbl.id = a.customerId " +
            "" +
            "left join business as owntbl on owntbl.id = a.ownerId " +
            "" +
            "left JOIN assets as a2 ON a.parentId = a2.id " +
            "" +
            "left JOIN assetstatus as s on s.id = a.status " +
            "" +
            "left JOIN business AS b2 ON a.ownerId = b2.id " +

            "left JOIN business as b5 ON b5.id = a.operatorId " +
            "" +
            "left JOIN facilities as f ON f.id = a.facilityId " +
            "" +
            "left JOIN facilities as linkedbl ON linkedbl.id = a.linkedToBlockId " +
            "" +
            "left JOIN facilities as bl ON bl.id = a.blockId   WHERE a.statusId != 0 AND a.status = 1 ";

            myQuery += (facilityId > 0 ? " AND a.facilityId= " + facilityId + "" : " ");
            myQuery += (linkedToBlockId > 0 ? " AND a.linkedToBlockId= " + linkedToBlockId + "" : " ");
            myQuery += (categoryIds?.Length > 0 ? " AND a.categoryId IN (" + categoryIds + ")" : " ");

            List<CMInventoryList> inventory = await Context.GetData<CMInventoryList>(myQuery).ConfigureAwait(false);
            foreach (var a in inventory)
            {
                if (a != null && a.calibrationDueDate != null)
                    a.calibrationDueDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, a.calibrationDueDate);


            }
            return inventory;
        }


        internal async Task<CMViewInventory> GetInventoryDetails(int id, string facilitytimeZone)
        {
            /*
            * get all details mentioned in model
            * Primary Table - Assets
            * Asset_category - asset category nam
            * AssetWarranty - warranty related information 
            * AssetType - type of inventory 
            * AssetSatus - status of inventory, 
            * Business - owner, operator, customer
           */
            /*Your code goes here*/

            //string myQuery = "SELECT a.name, a.description, s.name AS status, f.name AS block_name, a2.name as parent_name, " +
            //    "b3.name as manufacturer_name, a.currency FROM assets AS a JOIN assetstatus as s on s.id = a.statusId " +
            //    "JOIN facilities as f ON f.id = a.blockId JOIN assets as a2 ON a.parentId = a2.id " +
            //    "JOIN business AS b2 ON a.ownerId = b2.id JOIN business AS b3 ON a.manufacturerId = b3.id";
            string myQuery = "SELECT a.id ,a.name, a.description as asset_description,a.calibrationDueDate as calibrationDueDate ,a.calibrationLastDate as calibrationLastDate,a.vendorId as vendorid,a.stockCount as stockCount,a.photoId as photoId,a.retirementStatus as retirementStatus,w.meter_limit as meter_limit,w.meter_unit as meter_unit,a.moduleQuantity, ast.id as typeId, ast.name as type, a.supplierId as supplierId, b2.name as supplierName, manufacturertlb.id as manufacturerId, manufacturertlb.name as manufacturerName,a.parent_equipment_no ,b5.id as operatorId, b5.name as operatorName, ac.id as categoryId, ac.name as categoryName, a.serialNumber,a.cost as cost,a.currency as currencyId ,c.name as currency, a.model,a.calibrationFrequency,frequency.name as calibrationFreqType, a.calibrationReminderDays, " +
            //      "CASE WHEN a.calibrationLastDate = '0000-00-00 00:00:00' THEN NULL ELSE a.calibrationLastDate END as calibrationLastDate, CASE WHEN a.calibrationDueDate = '0000-00-00 00:00:00' THEN NULL ELSE a.calibrationDueDate END AS calibrationDueDate," +
            //    " a.model, a.currency, a.cost, a.acCapacity, a.dcCapacity, a.moduleQuantity, " +
            //"a.firstDueDate as calibrationDueDate, "+
            "f.id as facilityId, f.name AS facilityName, bl.id as blockId, bl.name AS blockName, a2.id as parentId, a2.name as parentName, a2.serialNumber as parentSerial, custbl.id as customerId, custbl.name as customerName, owntbl.id as ownerId, owntbl.name as ownerName, s.id as statusId, s.name AS status,a.purchaseCode as purchaseCode, a.unspCode as unspCode, a.barcode as barcode,a.descMaintenace as descMaintenace,a.dcRating as dcRating ,a.acRating as acRating, a.specialTool,a.specialToolEmpId as specialToolEmp,w.start_date as start_date,w.expiry_date as expiry_date, w.id as warrantyId, w.warranty_description, w.certificate_number,wut.name as warranty_term_type,wt.id as warrantyTypeId, wt.name as warrantyType, wut.id as warrantyTermTypeId, wp.id as warrantyProviderId, wp.name as warrantyProviderName, files.file_path as warranty_certificate_path " +     //use a.specialToolEmpId to put specialToolEmp,
            "from assets as a " +
            "left join assettypes as ast on ast.id = a.typeId " +
            "left join currency as c ON c.id =a.currency " +
            "left join assetcategories as ac on ac.id= a.categoryId " +
            "left join business as custbl on custbl.id = a.customerId " +
            "left join business as owntbl" + " on owntbl.id = a.ownerId " +
            "left JOIN business AS manufacturertlb ON a.ownerId = manufacturertlb.id " +
            "left JOIN business AS b2 ON a.supplierId= b2.id " +
            "left JOIN business as b5 ON b5.id = a.operatorId " +
            "left JOIN assets as a2 ON a.parentId = a2.id " +
            "left JOIN assetstatus as s on s.id = a.statusId " +
            "left JOIN facilities as f ON f.id = a.facilityId " +
            "left JOIN facilities as bl ON bl.id = a.blockId " +
            "left join assetwarranty as w ON a.warrantyId = w.id " +
            "left join uploadedfiles as files ON files.id = w.certificate_file_id " +
            "left join warrantytype as wt ON w.warranty_type = wt.id " +
            "left join warrantyusageterm as wut ON w.warranty_term_type = wut.id " +
            "left join business as wp ON w.warranty_provider = wp.id " +
            "left join frequency as frequency ON frequency.id = a.calibrationFrequency ";
            if (id != 0)
            {
                myQuery += " WHERE a.id= " + id;
            }
            List<CMViewInventory> _ViewInventoryList = await Context.GetData<CMViewInventory>(myQuery).ConfigureAwait(false);
            string myQuery18 = "SELECT  asset.id as id, file_path as fileName,  U.File_Size as fileSize, U.status,U.description FROM uploadedfiles AS U " +
                             "Left JOIN assets as  asset on asset.id = U.module_ref_id  " +
                             "where module_ref_id =" + id + " and U.module_type = " + (int)CMMS.CMMS_Modules.INVENTORY + ";";
            List<CMFileDetailJc> in_image = await Context.GetData<CMFileDetailJc>(myQuery18).ConfigureAwait(false);
            _ViewInventoryList[0].inventory_image = in_image;
            foreach (var a in _ViewInventoryList)
            {
                if (a != null && a.added_at != null)
                    a.added_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.added_at);
                if (a != null && a.calibrationDueDate != null)
                    a.calibrationDueDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.calibrationDueDate);
                if (a != null && a.calibrationLastDate != null)
                    a.calibrationLastDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.calibrationLastDate);
                if (a != null && a.deleted_at != null)
                    a.deleted_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.deleted_at);
                if (a != null && a.Imported_at != null)
                    a.Imported_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.Imported_at);
                if (a != null && a.updated_at != null)
                    a.updated_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.updated_at);







            }


            return _ViewInventoryList[0];


        }

        internal async Task<CMDefaultResponse> AddInventory(List<CMAddInventory> request, int userID)
        {
            /*
             * Add all data in assets table and warranty table
            */

            int count = 0;
            int retID = 0;
            string assetName = "";
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            string strRetMessage = "";
            int linkedToBlockId = 0;
            // List<int> idList = new List<int>();
            foreach (var unit in request)
            {
                /* string qry = "insert into assets (name, description, parentId, acCapacity, dcCapacity, categoryId, typeId, statusId, facilityId, blockId, linkedToBlockId, customerId, ownerId,operatorId, manufacturerId,supplierId,serialNumber,createdBy,photoId,model,stockCount,moduleQuantity, cost,currency,specialTool,specialToolEmpId,calibrationDueDate,calibrationLastDate,calibrationFreqType,calibrationFrequency,calibrationReminderDays,retirementStatus,multiplier,vendorId,calibrationNextDueDate,acRating,dcRating,descMaintenace,barcode,unspCode,purchaseCode,createdAt) values ";*/

                string qry = "insert into assets (name, description, parentId, acCapacity, dcCapacity, categoryId, typeId, statusId, facilityId, blockId, linkedToBlockId, customerId, ownerId,operatorId, manufacturerId,parent_equipment_no,supplierId,serialNumber,createdBy,photoId,model,stockCount,moduleQuantity, cost,currency,specialTool,specialToolEmpId,calibrationDueDate,calibrationLastDate,calibrationFreqType,calibrationFrequency,calibrationReminderDays,retirementStatus,multiplier,vendorId,calibrationNextDueDate,acRating,dcRating,descMaintenace,barcode,unspCode,purchaseCode,createdAt,num_of_module) values ";
                count++;
                assetName = unit.name;
                if (assetName.Length <= 0)
                {
                    throw new ArgumentException($"name of asset cannot be empty on line {count}");
                }
                string firstCalibrationDate = (unit.calibrationFirstDueDate == null) ? "NULL" : "'" + ((DateTime)unit.calibrationFirstDueDate.Value).ToString("yyyy-MM-dd") + "'";
                string lastCalibrationDate = (unit.calibrationLastDate == null) ? "NULL" : "'" + ((DateTime)unit.calibrationLastDate.Value).ToString("yyyy-MM-dd") + "'";
                string nextCalibrationDate = (unit.calibrationNextDueDate == null) ? "NULL" : "'" + ((DateTime)unit.calibrationNextDueDate.Value).ToString("yyyy-MM-dd") + "'";

                if (unit.blockId > 0)
                {
                    linkedToBlockId = unit.blockId;
                    //pending :validate if blockId exist
                }
                else if (unit.facilityId > 0)
                {
                    linkedToBlockId = unit.facilityId;
                    //pending :validate if blockId exist
                }
                else
                {
                    throw new ArgumentException($"{assetName} does not have facility or block mapping on line {count}");
                }
                if (unit.categoryId <= 0)
                {
                    throw new ArgumentException($"{assetName} does not have category mapping on line {count}");
                    //pending :validate if category id exist
                }
                if (unit.vendorId <= 0)
                {
                    unit.vendorId = unit.manufacturerId;
                }
                /*
string warrantyQry = "insert into assetwarranty 
(warranty_type, warranty_description, warranty_term_type, asset_id, start_date, expiry_date, meter_limit, meter_unit, warranty_provider, certificate_number,
                addedAt, addedBy, updatedAt, updatedBy, status) VALUES ";
            */
                /* qry += "('" + unit.name + "','" + unit.description + "','" + unit.parentId + "','" + unit.acCapacity + "','" + unit.dcCapacity + "','" + unit.categoryId + "','" + unit.typeId + "','" + unit.statusId + "','" + unit.facilityId + "','" + unit.blockId + "','" + unit.blockId + "','" + unit.customerId + "','" + unit.ownerId + "','" + unit.operatorId + "','" + unit.manufacturerId + "','" + unit.supplierId + "','" + unit.serialNumber + "','" + userID + "','" + unit.photoId + "','" + unit.model + "','" + unit.stockCount + "','" + unit.moduleQuantity + "','" + unit.cost + "','" + unit.currency + "','" + unit.specialToolId + "','" + unit.specialToolEmpId + "'," + firstCalibrationDate + "," + lastCalibrationDate + ",'" + unit.calibrationFrequencyType + "','" + unit.calibrationFrequency + "','" + unit.calibrationReminderDays + "','" + unit.retirementStatus + "','" + unit.multiplier + "','" + unit.vendorId + "'," + nextCalibrationDate + ",'" + unit.acRating + "','" + unit.dcRating + "','" + unit.descMaintenace + "','" + unit.barcode + "','" + unit.unspCode + "','" + unit.purchaseCode + "','"+ UtilsRepository.GetUTCTime() + "'); ";
                 qry += "select LAST_INSERT_ID(); ";*/

                qry += "('" + unit.name + "','" + unit.assetdescription + "','" + unit.parentId + "','" + unit.acCapacity + "','" + unit.dcCapacity + "','" + unit.categoryId + "','" + unit.typeId + "','" + unit.statusId + "','" + unit.facilityId + "','" + unit.blockId + "','" + unit.blockId + "','" + unit.customerId + "','" + unit.ownerId + "','" + unit.operatorId + "','" + unit.manufacturerId + "','" + unit.parent_equipment_no + "','" + unit.supplierId + "','" + unit.serialNumber + "','" + userID + "','" + unit.photoId + "','" + unit.model + "','" + unit.stockCount + "','" + unit.moduleQuantity + "','" + unit.cost + "','" + unit.currency + "','" + unit.specialToolId + "','" + unit.specialToolEmpId + "'," + firstCalibrationDate + "," + lastCalibrationDate + ",'" + unit.calibrationFrequencyType + "','" + unit.calibrationFrequency + "','" + unit.calibrationReminderDays + "','" + unit.retirementStatus + "','" + unit.multiplier + "','" + unit.vendorId + "'," + nextCalibrationDate + ",'" + unit.acRating + "','" + unit.dcRating + "','" + unit.descMaintenace + "','" + unit.barcode + "','" + unit.unspCode + "','" + unit.purchaseCode + "','" + UtilsRepository.GetUTCTime() + "','" + unit.num_of_module + "'); ";
                qry += "select LAST_INSERT_ID(); ";

                //List<CMInventoryList> newInventory = await Context.GetData<CMInventoryList>(qry).ConfigureAwait(false);
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                retID = Convert.ToInt32(dt.Rows[0][0]);
                if (unit.warranty_type >= 0 || unit.warranty_term_type >= 0 || unit.warranty_provider_id >= 0)
                {
                    string start_date = unit.start_date != null ? ((DateTime)unit.start_date).ToString("yyyy-MM-dd HH:mm:ss") : UtilsRepository.GetUTCTime().ToString();
                    string warranty_description = unit.warranty_description == null ? "" : unit.warranty_description;
                    string expiry_date = unit.expiry_date == null ? "" : ((DateTime)unit.expiry_date).ToString("yyyy-MM-dd HH:mm:ss");
                    string warrantyQry = "insert into assetwarranty (certificate_file_id, warranty_type, warranty_description, warranty_term_type, asset_id, start_date, expiry_date, meter_limit, meter_unit, warranty_provider, certificate_number, addedAt, addedBy, status,warrantyTenture) VALUES ";
                    warrantyQry += $"({unit.warranty_certificate_file_id}, {unit.warranty_type}, '{warranty_description}', {unit.warranty_term_type}, {retID}, '{start_date}', '{expiry_date}', {unit.meter_limit}, {unit.meter_unit}, {unit.warranty_provider_id}, '{unit.certificate_number}', '{UtilsRepository.GetUTCTime()}', {userID}, 1,{unit.warrantyTenture});" +
                        $" SELECT LAST_INSERT_ID();";
                    DataTable dt2 = await Context.FetchData(warrantyQry).ConfigureAwait(false);
                    int warrantyId = Convert.ToInt32(dt2.Rows[0][0]);
                    string addWarrantyId = $"UPDATE assets SET warrantyId = {warrantyId} WHERE id = {retID}";
                    await Context.ExecuteNonQry<int>(addWarrantyId).ConfigureAwait(false);
                }
                else
                {
                    strRetMessage = "Warranty data for <" + assetName + "> does not exist. ";
                }
                if (unit.uplaodfile_ids.Count > 0)
                {
                    foreach (int data in unit.uplaodfile_ids)
                    {
                        string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {unit.facilityId}, module_type={(int)CMMS.CMMS_Modules.INVENTORY},module_ref_id={retID} where id = {data}";
                        await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                    }
                }
                CMViewInventory _inventoryAdded = await GetInventoryDetails(retID, "");

                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED);
                _inventoryAdded.status_short = _shortStatus;

                string _longStatus = getLongStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED, _inventoryAdded);
                _inventoryAdded.status_long = _longStatus;

                //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED, new[] { userID }, _inventoryAdded);
            }
            if (count > 0)
            {

                retCode = CMMS.RETRUNSTATUS.SUCCESS;
                if (count == 1)
                {
                    strRetMessage = "New asset <" + assetName + "> added";
                }
                else
                {
                    strRetMessage = "<" + count + "> new assets added";
                }
            }
            else
            {
                strRetMessage = "No assets to add";
            }


            return new CMDefaultResponse(retID, retCode, strRetMessage);
        }
        internal async Task<CMDefaultResponse> CreateInventory(List<CMAddInventory> request, int userID)
        {
            /*
             * Add all data in assets table and warranty table
            */

            int count = 0;
            int retID = 0;
            string assetName = "";
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            string strRetMessage = "";
            int linkedToBlockId = 0;
            List<int> idList = new List<int>();
            foreach (var unit in request)
            {
                /* string qry = "insert into assets (name, description, parentId, acCapacity, dcCapacity, categoryId, typeId, statusId, facilityId, blockId, linkedToBlockId, customerId, ownerId,operatorId, manufacturerId,supplierId,serialNumber,createdBy,photoId,model,stockCount,moduleQuantity, cost,currency,specialTool,specialToolEmpId,calibrationDueDate,calibrationLastDate,calibrationFreqType,calibrationFrequency,calibrationReminderDays,retirementStatus,multiplier,vendorId,calibrationNextDueDate,acRating,dcRating,descMaintenace,barcode,unspCode,purchaseCode,createdAt) values ";*/

                string qry = "insert into assets (name, description, parentId, acCapacity, dcCapacity, categoryId, typeId, statusId, facilityId, blockId, linkedToBlockId, customerId, ownerId,operatorId, manufacturerId,parent_equipment_no,supplierId,serialNumber,createdBy,photoId,model,stockCount,moduleQuantity, cost,currency,specialTool,specialToolEmpId,calibrationDueDate,calibrationLastDate,calibrationFreqType,calibrationFrequency,calibrationReminderDays,retirementStatus,multiplier,vendorId,calibrationNextDueDate,acRating,dcRating,descMaintenace,barcode,unspCode,purchaseCode,createdAt,num_of_module,area) values ";
                count++;
                assetName = unit.name;
                if (assetName.Length <= 0)
                {
                    throw new ArgumentException($"name of asset cannot be empty on line {count}");
                }
                string firstCalibrationDate = (unit.calibrationFirstDueDate == null) ? "NULL" : "'" + ((DateTime)unit.calibrationFirstDueDate.Value).ToString("yyyy-MM-dd") + "'";
                string lastCalibrationDate = (unit.calibrationLastDate == null) ? "NULL" : "'" + ((DateTime)unit.calibrationLastDate.Value).ToString("yyyy-MM-dd") + "'";
                string nextCalibrationDate = (unit.calibrationNextDueDate == null) ? "NULL" : "'" + ((DateTime)unit.calibrationNextDueDate.Value).ToString("yyyy-MM-dd") + "'";

                if (unit.blockId > 0)
                {
                    linkedToBlockId = unit.blockId;
                    //pending :validate if blockId exist
                }
                else if (unit.facilityId > 0)
                {
                    linkedToBlockId = unit.facilityId;
                    //pending :validate if blockId exist
                }
                else
                {
                    throw new ArgumentException($"{assetName} does not have facility or block mapping on line {count}");
                }
                if (unit.categoryId <= 0)
                {
                    throw new ArgumentException($"{assetName} does not have category mapping on line {count}");
                    //pending :validate if category id exist
                }
                if (unit.vendorId <= 0)
                {
                    unit.vendorId = unit.manufacturerId;
                }
                /*
string warrantyQry = "insert into assetwarranty 
(warranty_type, warranty_description, warranty_term_type, asset_id, start_date, expiry_date, meter_limit, meter_unit, warranty_provider, certificate_number,
                addedAt, addedBy, updatedAt, updatedBy, status) VALUES ";
            */
                /* qry += "('" + unit.name + "','" + unit.description + "','" + unit.parentId + "','" + unit.acCapacity + "','" + unit.dcCapacity + "','" + unit.categoryId + "','" + unit.typeId + "','" + unit.statusId + "','" + unit.facilityId + "','" + unit.blockId + "','" + unit.blockId + "','" + unit.customerId + "','" + unit.ownerId + "','" + unit.operatorId + "','" + unit.manufacturerId + "','" + unit.supplierId + "','" + unit.serialNumber + "','" + userID + "','" + unit.photoId + "','" + unit.model + "','" + unit.stockCount + "','" + unit.moduleQuantity + "','" + unit.cost + "','" + unit.currency + "','" + unit.specialToolId + "','" + unit.specialToolEmpId + "'," + firstCalibrationDate + "," + lastCalibrationDate + ",'" + unit.calibrationFrequencyType + "','" + unit.calibrationFrequency + "','" + unit.calibrationReminderDays + "','" + unit.retirementStatus + "','" + unit.multiplier + "','" + unit.vendorId + "'," + nextCalibrationDate + ",'" + unit.acRating + "','" + unit.dcRating + "','" + unit.descMaintenace + "','" + unit.barcode + "','" + unit.unspCode + "','" + unit.purchaseCode + "','"+ UtilsRepository.GetUTCTime() + "'); ";
                 qry += "select LAST_INSERT_ID(); ";*/

                qry += "('" + unit.name + "','" + unit.assetdescription + "','" + unit.parentId + "','" + unit.acCapacity + "','" + unit.dcCapacity + "','" + unit.categoryId + "','" + unit.typeId + "','" + unit.statusId + "','" + unit.facilityId + "','" + unit.blockId + "','" + unit.blockId + "','" + unit.customerId + "','" + unit.ownerId + "','" + unit.operatorId + "','" + unit.manufacturerId + "','" + unit.parent_equipment_no + "','" + unit.supplierId + "','" + unit.serialNumber + "','" + userID + "','" + unit.photoId + "','" + unit.model + "','" + unit.stockCount + "','" + unit.moduleQuantity + "','" + unit.cost + "','" + unit.currency + "','" + unit.specialToolId + "','" + unit.specialToolEmpId + "'," + firstCalibrationDate + "," + lastCalibrationDate + ",'" + unit.calibrationFrequencyType + "','" + unit.calibrationFrequency + "','" + unit.calibrationReminderDays + "','" + unit.retirementStatus + "','" + unit.multiplier + "','" + unit.vendorId + "'," + nextCalibrationDate + ",'" + unit.acRating + "','" + unit.dcRating + "','" + unit.descMaintenace + "','" + unit.barcode + "','" + unit.unspCode + "','" + unit.purchaseCode + "','" + UtilsRepository.GetUTCTime() + "','" + unit.num_of_module + "'," + unit.area + "); ";
                qry += "select LAST_INSERT_ID(); ";

                //List<CMInventoryList> newInventory = await Context.GetData<CMInventoryList>(qry).ConfigureAwait(false);
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                retID = Convert.ToInt32(dt.Rows[0][0]);
                if (unit.warranty_type > 0 || unit.warranty_term_type > 0 || unit.warranty_provider_id > 0)
                {
                    string start_date = unit.start_date != null ? ((DateTime)unit.start_date).ToString("yyyy-MM-dd HH:mm:ss") : UtilsRepository.GetUTCTime().ToString();
                    string warranty_description = unit.warranty_description == null ? "" : unit.warranty_description;
                    string expiry_date = unit.expiry_date == null ? "" : ((DateTime)unit.expiry_date).ToString("yyyy-MM-dd HH:mm:ss");
                    string warrantyQry = "insert into assetwarranty (certificate_file_id, warranty_type, warranty_description, warranty_term_type, asset_id, start_date, expiry_date, meter_limit, meter_unit, warranty_provider, certificate_number, addedAt, addedBy, status,warrantyTenture) VALUES ";
                    warrantyQry += $"({unit.warranty_certificate_file_id}, {unit.warranty_type}, '{warranty_description}', {unit.warranty_term_type}, {retID}, '{start_date}', '{expiry_date}', {unit.meter_limit}, {unit.meter_unit}, {unit.warranty_provider_id}, '{unit.certificate_number}', '{UtilsRepository.GetUTCTime()}', {userID}, 1,{unit.warrantyTenture});" +
                        $" SELECT LAST_INSERT_ID();";
                    DataTable dt2 = await Context.FetchData(warrantyQry).ConfigureAwait(false);
                    int warrantyId = Convert.ToInt32(dt2.Rows[0][0]);
                    string addWarrantyId = $"UPDATE assets SET warrantyId = {warrantyId} WHERE id = {retID}";
                    await Context.ExecuteNonQry<int>(addWarrantyId).ConfigureAwait(false);
                }
                else
                {
                    strRetMessage = "Warranty data for <" + assetName + "> does not exist. ";
                }

                idList.Add(retID);

                CMViewInventory _inventoryAdded = await GetInventoryDetails(retID, "");

                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED);
                _inventoryAdded.status_short = _shortStatus;

                string _longStatus = getLongStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED, _inventoryAdded);
                _inventoryAdded.status_long = _longStatus;

                //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED, new[] { userID }, _inventoryAdded);
            }
            if (count > 0)
            {

                retCode = CMMS.RETRUNSTATUS.SUCCESS;
                if (count == 1)
                {
                    strRetMessage = "New asset <" + assetName + "> added";
                }
                else
                {
                    strRetMessage = "<" + count + "> new assets added";
                }
            }
            else
            {
                strRetMessage = "No assets to add";
            }


            return new CMDefaultResponse(idList, retCode, strRetMessage);
        }
        //update
        internal async Task<CMDefaultResponse> UpdateInventory(List<CMAddInventory> request, int userID)
        {
            /*
             * Add all data in assets table and warranty table
            */

            int count = 0;
            int retID = 0;
            string assetName = "";
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            string strRetMessage = "";
            int linkedToBlockId = 0;
            List<int> idList = new List<int>();
            foreach (var unit in request)
            {

                count++;
                assetName = unit.name;
                if (assetName.Length <= 0)
                {
                    throw new ArgumentException($"name of asset cannot be empty on line {count}");
                }
                string firstCalibrationDate = (unit.calibrationFirstDueDate == null) ? "NULL" : "'" + ((DateTime)unit.calibrationFirstDueDate.Value).ToString("yyyy-MM-dd") + "'";
                string lastCalibrationDate = (unit.calibrationLastDate == null) ? "NULL" : "'" + ((DateTime)unit.calibrationLastDate.Value).ToString("yyyy-MM-dd") + "'";
                string nextCalibrationDate = (unit.calibrationNextDueDate == null) ? "NULL" : "'" + ((DateTime)unit.calibrationNextDueDate.Value).ToString("yyyy-MM-dd") + "'";

                if (unit.blockId > 0)
                {
                    linkedToBlockId = unit.blockId;
                    //pending :validate if blockId exist
                }
                else if (unit.facilityId > 0)
                {
                    linkedToBlockId = unit.facilityId;
                    //pending :validate if blockId exist
                }
                else
                {
                    throw new ArgumentException($"{assetName} does not have facility or block mapping on line {count}");
                }
                if (unit.categoryId <= 0)
                {
                    throw new ArgumentException($"{assetName} does not have category mapping on line {count}");
                    //pending :validate if category id exist
                }
                if (unit.vendorId <= 0)
                {
                    unit.vendorId = unit.manufacturerId;
                }
                string qry = "update  assets set description='" + unit.assetdescription + "', parentId='" + unit.parentId + "', acCapacity='" + unit.acCapacity + "', dcCapacity='" + unit.dcCapacity + "', categoryId='" + unit.categoryId + "', typeId='" + unit.typeId + "', statusId='" + unit.statusId + "', facilityId='" + unit.facilityId + "', blockId='" + unit.blockId + "', linkedToBlockId='" + unit.blockId + "', customerId='" + unit.customerId + "', ownerId='" + unit.ownerId + "',operatorId='" + unit.operatorId + "', manufacturerId='" + unit.manufacturerId + "',supplierId='" + unit.supplierId + "',serialNumber='" + unit.serialNumber + "',createdBy='" + userID + "',photoId='" + unit.photoId + "',model='" + unit.model + "',stockCount='" + unit.stockCount + "',moduleQuantity='" + unit.moduleQuantity + "', cost='" + unit.cost + "',currency='" + unit.currency + "',specialTool='" + unit.specialToolId + "',specialToolEmpId='" + unit.specialToolEmpId + "',calibrationDueDate=" + firstCalibrationDate + ",calibrationLastDate=" + lastCalibrationDate + ",calibrationFreqType='" + unit.calibrationFrequencyType + "',calibrationFrequency='" + unit.calibrationFrequency + "',calibrationReminderDays='" + unit.calibrationReminderDays + "',retirementStatus='" + unit.retirementStatus + "',multiplier='" + unit.multiplier + "',vendorId='" + unit.vendorId + "',calibrationNextDueDate=" + nextCalibrationDate + ",acRating='" + unit.acRating + "',dcRating='" + unit.dcRating + "',descMaintenace='" + unit.descMaintenace + "',barcode='" + unit.barcode + "',unspCode='" + unit.unspCode + "'" +
                    ",purchaseCode='" + unit.purchaseCode + "' where name = '" + unit.name + "' and facilityid='" + unit.facilityId + "' ";
                //qry += "('" + unit.name + "','" + unit.description + "','" + unit.parentId + "','" + unit.acCapacity + "','" + unit.dcCapacity + "','" + unit.categoryId + "','" + unit.typeId + "','" + unit.statusId + "','" + unit.facilityId + "','" + unit.blockId + "','" + unit.blockId + "','" + unit.customerId + "','" + unit.ownerId + "','" + unit.operatorId + "','" + unit.manufacturerId + "','" + unit.supplierId + "','" + unit.serialNumber + "','" + userID + "','" + unit.photoId + "','" + unit.model + "','" + unit.stockCount + "','" + unit.moduleQuantity + "','" + unit.cost + "','" + unit.currency + "','" + unit.specialToolId + "','" + unit.specialToolEmpId + "'," + firstCalibrationDate + "," + lastCalibrationDate + ",'" + unit.calibrationFrequencyType + "','" + unit.calibrationFrequency + "','" + unit.calibrationReminderDays + "','" + unit.retirementStatus + "','" + unit.multiplier + "','" + unit.vendorId + "'," + nextCalibrationDate + ",'" + unit.acRating + "','" + unit.dcRating + "','" + unit.descMaintenace + "','" + unit.barcode + "','" + unit.unspCode + "','" + unit.purchaseCode + "'); ";


                //List<CMInventoryList> newInventory = await Context.GetData<CMInventoryList>(qry).ConfigureAwait(false);
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);

                //string qry = $" update  assets set description = '{ unit.description}', parentId='{ unit.parentId}' " +
                //    $"where name = '{ unit.name}' and facilityid='{ unit.facilityId }' ;";


                try
                {
                    // await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, 3333333, 0, 0, qry, CMMS.CMMS_Status.INVENTORY_DELETED, 0);

                    await Context.GetData<List<int>>(qry).ConfigureAwait(false);
                    //DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, 3333333, 0, 0, ex.Message, CMMS.CMMS_Status.INVENTORY_DELETED, 0);
                    //return new CMDefaultResponse(idList, retCode, strRetMessage);
                }




                //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED, new[] { userID }, _inventoryAdded);
            }



            return new CMDefaultResponse(idList, retCode, strRetMessage);
        }


        internal async Task<CMDefaultResponse> UpdateInventory(CMAddInventory request, int userID)
        {

            /*
             * update all data in assets table and warranty table
            */
            /*Your code goes here*/
            // "SELECT* FROM assets JOIN assetwarranty ON assets.warrantyId = assetwarranty.id";

            // string updateQry = $" name = '{request.name}  ', description = ' {request.description}  ', parentId = ' {request.parentId}  ', acCapacity = '  {request.acCapacity}  ', moduleQuantity = '  {request.moduleQuantity}' , categoryId = ' {request.categoryId} ', typeId = ' {request.typeId} ', statusId = ' {request.statusId}' , facilityId = ' {request.facilityId} ', blockId = ' {request.blockId} ' customer id = ' {request.customerId}', owner Id = ' {request.ownerId} ', manufacturer Id = ' {request.manufacturerId} ', supplier Id = ' {request.supplierId} ', serial Number = ' {request.serialNumber}' warranty Id = ' {request.warrantyId} ', Created At = ' {request.createdAt}', Created By = '{request.createdBy} ', Updated At = '{request.updatedAt}' , Updated By = ' {request.updatedBy} ', status = ' {request.status} ', photo Id = '  {request.photoId}' , cost = ' {request.cost} ' currency = ' {request.currency} ', stock Count = ' {request.stockCount}' , Special Tool = ' {request.specialTool} ' , specialToolEmpId = ' {request.specialToolEmpId} ', first Due Date = ' {request.firstDueDate} ', frequency ' {request.frequency}' , Description Maintainence = ' {request.descriptionMaintainence} 'Calibration Frequency = ' {request.calibrationFrequency} ', Calibration Reminder = '{request.calibrationReminder}', retirement Status = ' {request.retirementStatus} ', Multiplier = '{request.multiplier}' ";
            /* if (request.id > 0)
             {
                 updateQry += $" WHERE a.id= '{request.id}'";

             }
            */

            string updateQry = "";
            if (request.name != null)
            {
                updateQry += $" name= '{request.name}',";
            }
            if (request.assetdescription != null)
            {
                updateQry += $" description = '{request.assetdescription}',";
            }
            if (request.typeId != 0)
            {
                updateQry += $" typeId= '{request.typeId}',";
            }
            if (request.acCapacity != 0)
            {
                updateQry += $" acCapacity= '{request.acCapacity}',";
            }
            if (request.dcCapacity != 0)
            {
                updateQry += $" dcCapacity= '{request.dcCapacity}',";
            }
            if (request.categoryId != 0)
            {
                updateQry += $" categoryId= '{request.categoryId}',";
            }
            if (request.moduleQuantity != 0)
            {
                updateQry += $" moduleQuantity= '{request.moduleQuantity}',";
            }
            if (request.area != 0)
            {
                updateQry += $" area= '{request.area}',";
            }
            if (request.categoryId != 0)
            {
                updateQry += $" categoryId = '{request.categoryId}',";
            }
            if (request.statusId != 0)
            {
                updateQry += $" statusId = '{request.statusId}',";
                updateQry += $" status = '{request.statusId}',";
            }
            if (request.parentId != 0)
            {
                updateQry += $" parentId = '{request.parentId}',";
            }
            if (request.facilityId != 0)
            {
                updateQry += $" facilityId= '{request.facilityId}',";

            }
            if (request.blockId != 0) //here you may have check if its not 0. verify during testig
            {
                updateQry += $" blockId= '{request.blockId}',";

            }
            if (request.customerId != 0)
            {
                updateQry += $" customerId= '{request.customerId}',";

            }
            if (request.ownerId != 0)
            {
                updateQry += $" ownerId= '{request.ownerId}',";

            }
            if (request.operatorId != 0)
            {
                updateQry += $" operatorId = '{request.operatorId}',";

            }
            if (request.manufacturerId != 0)
            {
                updateQry += $" manufacturerid= '{request.manufacturerId}',";

            }
            if (request.supplierId != 0)
            {
                updateQry += $" supplierId= '{request.supplierId}',";

            }
            if (request.acCapacity != 0)
            {
                updateQry += $" acCapacity= '{request.acCapacity}',";
            }
            if (request.dcCapacity != 0)
            {
                updateQry += $" dcCapacity= '{request.dcCapacity}',";
            }
            if (request.model != null)
            {
                updateQry += $" model = '{request.model}',";
            }
            if (request.serialNumber != null)
            {
                updateQry += $" serialNumber= '{request.serialNumber}',";
            }
            if (request.currency != null)
            {
                updateQry += $" currency = '{request.currency}',";
            }
            //if (request.currencyId != 0)
            //{
            //    updateQry += $" currencyId = '{request.currencyId}',";

            //}
            if (request.photoId != 0)
            {
                updateQry += $" photoId= '{request.photoId}',";

            }
            if (request.cost != 0)
            {
                updateQry += $" cost= '{request.cost}',";

            }
            if (request.stockCount != 0)
            {
                updateQry += $" stockCount= '{request.stockCount}',";

            }
            if (request.moduleQuantity != 0)
            {
                updateQry += $" moduleQuantity= '{request.moduleQuantity}',";

            }
            if (request.specialToolId != 0)
            {
                updateQry += $" specialTool= '{request.specialToolId}',";

            }
            if (request.specialToolEmpId != 0)
            {
                updateQry += $" specialToolEmpId= '{request.specialToolEmpId}',";

            }
            //if (request.calibrationFirstDueDate != null)
            //{
            //    updateQry += $" calibrationDueDate= '{request.calibrationFirstDueDate}',";

            //}
            //if (request.calibrationLastDate != 0)
            //{
            //    updateQry += $" calibrationLastDate = '{request.calibrationLastDate}',";

            //}
            if (request.calibrationFrequency != 0)
            {
                updateQry += $" calibrationFrequency = '{request.calibrationFrequency}',";

            }
            if (request.calibrationFrequencyType != 0)
            {
                updateQry += $" calibrationFreqType= '{request.calibrationFrequencyType}',";

            }
            if (request.calibrationReminderDays != 0)
            {
                updateQry += $" calibrationReminderDays = '{request.calibrationReminderDays}',";

            }
            if (request.retirementStatus != 0)
            {
                updateQry += $" retirementStatus= '{request.retirementStatus}',";

            }
            if (request.retirementStatus != 0)
            {
                updateQry += $" retirementStatus = '{request.retirementStatus}',";

            }/*
            if (request.lstWarrantyDetail != null)
            {
                CMWarrantyDetail warrentyDetails = request.lstWarrantyDetail[0];
                string certificateNumber = warrentyDetails.certificate_number;
                //warrentyDetails.expiry_date;
                int status = warrentyDetails.status;
                string warrantyDescription = warrentyDetails.warranty_description;
            }/**/
            //        public List<CMWarrantyDetail> lstWarrantyDetail { get; set; }

            if (request.multiplier != 0)
            {
                updateQry += $" multiplier = '{request.multiplier}',";

            }
            if (updateQry != null)
            {
                updateQry += $" updatedAt= '{UtilsRepository.GetUTCTime()}',";
                updateQry += $" updatedBy= '{userID}',";
                updateQry = "UPDATE assets SET " + updateQry.Substring(0, updateQry.Length - 1);
                updateQry += $" WHERE id= '{request.id}'";
                await Context.GetData<List<int>>(updateQry).ConfigureAwait(false);
            }
            CMDefaultResponse obj = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Inventory  <" + request.id + "> has been updated");

            CMViewInventory _inventoryAdded = await GetInventoryDetails(request.id, "");

            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_UPDATED);
            _inventoryAdded.status_short = _shortStatus;

            string _longStatus = getLongStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_UPDATED, _inventoryAdded);
            _inventoryAdded.status_long = _longStatus;

            //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_UPDATED, new[] { userID } ,_inventoryAdded);

            return obj;





        }

        internal async Task<CMDefaultResponse> DeleteInventory(int id, int userID)
        {
            /*?ID=34
             * delete from assets and warranty table
            */
            /*Your code goes here*/
            if (id <= 0)
            {
                throw new ArgumentException("Invalid argument <" + id + ">");

            }

            CMViewInventory _inventoryAdded = await GetInventoryDetails(id, "");

            string qry = $"SELECT CONCAT(firstName,' ',lastName) as deleted_by FROM users WHERE id = {id}";

            List<CMViewInventory> deleted_by = await Context.GetData<CMViewInventory>(qry).ConfigureAwait(false);

            _inventoryAdded.deleted_by = deleted_by[0].deleted_by;

            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_DELETED);
            _inventoryAdded.status_short = _shortStatus;

            string _longStatus = getLongStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_DELETED, _inventoryAdded);
            _inventoryAdded.status_long = _longStatus;

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_DELETED, new[] { userID }, _inventoryAdded);

            string delQuery1 = $"UPDATE assets SET statusId = 0, status = 0 WHERE id = {id}";
            string delQuery2 = $"UPDATE assetwarranty SET status = 0 where asset_id = {id}";
            await Context.GetData<List<int>>(delQuery1).ConfigureAwait(false);
            await Context.GetData<List<int>>(delQuery2).ConfigureAwait(false);

            CMDefaultResponse obj = null;
            //if (retVal1 && retVal2)
            {
                obj = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Inventory <" + id + "> has been deleted");
            }
            return obj;
            // DELETE t1, t2 FROM t1 INNER JOIN t2 INNER JOIN t3
            //WHERE t1.id = t2.id AND t2.id = t3.id;
        }

        internal async Task<CMDefaultResponse> DeleteInventoryByFacilityId(int facilityId, int userID)
        {
            /*?ID=34
             * delete from assets and warranty table
            */
            /*Your code goes here*/
            if (facilityId <= 0)
            {
                throw new ArgumentException("Invalid argument <" + facilityId + ">");

            }

            string delQuery1 = $"UPDATE assets SET statusId = 0, status = 0 WHERE facilityId = {facilityId}";
            string delQuery2 = $"UPDATE assetwarranty left join assets on assetwarranty.asset_id =  assets.id SET assetwarranty.status = 0 where facilityId = {facilityId}";
            await Context.GetData<List<int>>(delQuery1).ConfigureAwait(false);
            await Context.GetData<List<int>>(delQuery2).ConfigureAwait(false);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, facilityId, 0, 0, $"Deleted Inventory of facility Id {facilityId}.", CMMS.CMMS_Status.INVENTORY_DELETED, userID);

            CMDefaultResponse obj = null;
            //if (retVal1 && retVal2)
            {
                obj = new CMDefaultResponse(facilityId, CMMS.RETRUNSTATUS.SUCCESS, "Inventory of facility Id <" + facilityId + "> has been deleted");
            }
            return obj;

        }

        #region Inventory Masters
        internal async Task<List<CMInventoryTypeList>> GetInventoryTypeList()
        {
            /*
             * Fetch data from assetType
            */
            /*Your code goes here*/
            // "SELECT * FROM assetTypes";
            string myQuery = "SELECT id, name, description FROM assettypes where status = 1";
            List<CMInventoryTypeList> _InventoryTypeList = await Context.GetData<CMInventoryTypeList>(myQuery).ConfigureAwait(false);
            return _InventoryTypeList;
        }
        internal async Task<CMDefaultResponse> AddInventoryType(CMInventoryTypeList request, int userID)
        {
            string myQuery = $"INSERT INTO assettypes(name, description, status, addedBy, addedAt) VALUES " +
                                $"('{request.name}', '{request.description}', 1, {userID}, '{UtilsRepository.GetUTCTime()}');" +
                                $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset Type Added");
        }
        internal async Task<CMDefaultResponse> UpdateInventoryType(CMInventoryTypeList request, int userID)
        {
            string updateQry = "UPDATE assettypes SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Asset Type Updated");
        }
        internal async Task<CMDefaultResponse> DeleteInventoryType(int id)
        {
            string deleteQry = $"UPDATE assettypes SET status = 0 WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset Type Deleted");
        }
        internal async Task<List<CMInventoryStatusList>> GetInventoryStatusList()
        {
            /*
             * Fetch data from assetStatus
            */
            /*Your code goes here*/
            // "SELECT * FROM assetStatus";
            string myQuery = "SELECT id, name, description FROM assetstatus where status = 1";
            List<CMInventoryStatusList> _InventoryStatusList = await Context.GetData<CMInventoryStatusList>(myQuery).ConfigureAwait(false);
            return _InventoryStatusList;
        }

        internal async Task<CMDefaultResponse> AddInventoryStatus(CMInventoryStatusList request, int userID)
        {
            string myQuery = $"INSERT INTO assetstatus(name, description, status, addedBy, addedAt) VALUES " +
                                $"('{request.name}', '{request.description}', 1, {userID}, '{UtilsRepository.GetUTCTime()}');" +
                                $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset Status Added");
        }
        internal async Task<CMDefaultResponse> UpdateInventoryStatus(CMInventoryStatusList request, int userID)
        {
            string updateQry = "UPDATE assetstatus SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Asset Status Details Updated");
        }
        internal async Task<CMDefaultResponse> DeleteInventoryStatus(int id)
        {
            string deleteQry = $"UPDATE assetstatus SET status = 0 WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset Status Deleted");
        }

        internal async Task<List<CMInventoryCategoryList>> GetInventoryCategoryList()
        {
            string myQuery = "SELECT id, name, description, calibrationStatus as calibration_required FROM assetcategories where status = 1";
            List<CMInventoryCategoryList> _AssetCategory = await Context.GetData<CMInventoryCategoryList>(myQuery).ConfigureAwait(false);
            return _AssetCategory;
        }

        internal async Task<CMDefaultResponse> AddInventoryCategory(CMInventoryCategoryList request, int userID)
        {
            string myQuery = $"INSERT INTO assetcategories(name, description, status, calibrationStatus, createdBy, createdAt) " +
                                $"VALUES ('{request.name}', '{(request.description == null ? "" : request.description)}', 1, " +
                                $"{(request.calibration_required == null ? 0 : request.calibration_required)}, {userID}, " +
                                $"'{UtilsRepository.GetUTCTime()}'); SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset Category Added");
        }
        internal async Task<CMDefaultResponse> UpdateInventoryCategory(CMInventoryCategoryList request, int userID)
        {
            string updateQry = "UPDATE assetcategories SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            if (request.calibration_required != null)
                updateQry += $"calibrationStatus = {request.calibration_required}, ";
            updateQry += $"updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Asset Category Details Updated");
        }
        internal async Task<CMDefaultResponse> DeleteInventoryCategory(int id)
        {
            string deleteQry = $"UPDATE assetcategories SET status = 0 WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Asset Category Deleted");
        }

        internal async Task<List<CMDefaultList>> GetWarrantyTypeList()
        {
            List<CMDefaultList> warrentyType = new List<CMDefaultList>();
            warrentyType.Add(new CMDefaultList() { id = 1, name = "Basic" });
            warrentyType.Add(new CMDefaultList() { id = 2, name = "Comprehensive" });
            return warrentyType;
        }

        internal async Task<List<CMDefaultList>> GetWarrantyUsageTermList()
        {
            List<CMDefaultList> warrentyUsageTermType = new List<CMDefaultList>();
            warrentyUsageTermType.Add(new CMDefaultList() { id = 1, name = "Duration" });
            warrentyUsageTermType.Add(new CMDefaultList() { id = 2, name = "Meter" });
            warrentyUsageTermType.Add(new CMDefaultList() { id = 3, name = "Earier of duration or meter" });
            warrentyUsageTermType.Add(new CMDefaultList() { id = 4, name = "Process completion" });
            return warrentyUsageTermType;
        }
        internal async Task<List<CMWarrantyCertificate>> GetWarrantyCertificate()
        {
            string myQuery = "SELECT assets.id as asset_id, assets.name as asset_name, category.id as categoryId, category.name as categoryName, " +
                "assetwarranty.warranty_description, warrantytype.id as warrantyTypeId, warrantytype.name as warrantyTypeName, " +
                "warrantyusageterm.id as warrantyTermId, warrantyusageterm.name as warrantyTermName, assetwarranty.certificate_number, " +
                "certificate.file_path AS warranty_certificate_file_path, CASE WHEN assetwarranty.start_date = '0000-00-00 00:00:00' " +
                "THEN NULL ELSE assetwarranty.start_date END AS warrantyStartDate, CASE WHEN assetwarranty.expiry_date = '0000-00-00 00:00:00' " +
                "THEN NULL ELSE assetwarranty.expiry_date END AS warrantyExpiryDate, provider.id as warrantyProviderId, " +
                "provider.name as warrantyProviderName\r\nFROM assetwarranty \r\nJOIN assets ON assetwarranty.asset_id = assets.id\r\n" +
                "LEFT JOIN warrantytype ON assetwarranty.warranty_type = warrantytype.id\r\nLEFT JOIN warrantyusageterm " +
                "ON assetwarranty.warranty_term_type = warrantyusageterm.id\r\nLEFT JOIN business as provider " +
                "ON assetwarranty.warranty_provider = provider.id\r\nLEFT JOIN assetcategories as category " +
                "ON assets.categoryId = category.id\r\nLEFT JOIN uploadedfiles as certificate ON assetwarranty.certificate_file_id = certificate.id  WHERE provider.id > 0";
            List<CMWarrantyCertificate> _AssetCategory = await Context.GetData<CMWarrantyCertificate>(myQuery).ConfigureAwait(false);
            return _AssetCategory;
        }
        internal async Task<List<CMCalibrationAssets>> GetCalibrationList(int facilityId, string facilitytimeZone)
        {
            string myQuery = "SELECT assets.id, assets.name, category.id as categoryId, category.name as categoryName, " +
            "vendor.id as vendorId, vendor.name as vendorName, assets.calibrationFreqType, " +
            "frequency.id as frequencyId, frequency.name as frequencyName, CASE WHEN assets.calibrationLastDate = '0000-00-00 00:00:00' THEN NULL ELSE assets.calibrationLastDate END AS calibrationLastDate, " +
            "CASE WHEN assets.calibrationDueDate = '0000-00-00 00:00:00' THEN NULL ELSE assets.calibrationDueDate END AS calibrationDueDate, assets.calibrationReminderDays " +
            "FROM assets " +
            "LEFT JOIN assetcategories as category ON category.id = assets.categoryId " +
            "LEFT JOIN business as vendor ON vendor.id = assets.vendorId " +
            "LEFT JOIN frequency ON assets.calibrationFrequency = frequency.id " +
            $"WHERE category.calibrationStatus = 1 AND facilityId = {facilityId} ";
            List<CMCalibrationAssets> _AssetCategory = await Context.GetData<CMCalibrationAssets>(myQuery).ConfigureAwait(false);
            foreach (var a in _AssetCategory)
            {

                if (a != null && a.calibrationDueDate != null)
                    a.calibrationDueDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.calibrationDueDate);
                if (a != null && a.calibrationLastDate != null)
                    a.calibrationLastDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.calibrationLastDate);

            }
            return _AssetCategory;


        }

        #endregion

    }
}
