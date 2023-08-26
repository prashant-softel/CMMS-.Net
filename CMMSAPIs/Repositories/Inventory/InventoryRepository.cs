using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Calibration;
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
        internal async Task<CMImportFileResponse> ImportInventories(int file_id, int userID)
        {
            CMImportFileResponse response = null;

            string queryAsset = "SELECT id, name FROM assets GROUP BY name ORDER BY id ASC;";
            DataTable dtAsset = await Context.FetchData(queryAsset).ConfigureAwait(false);
            List<string> assetNames = dtAsset.GetColumn<string>("name");
            List<int> assetIDs = dtAsset.GetColumn<int>("id");
            Dictionary<string, int> assets = new Dictionary<string, int>();
            assets.Merge(assetNames, assetIDs);

            string queryCat = "SELECT id, name FROM assetcategories GROUP BY name ORDER BY id ASC;";
            DataTable dtCat = await Context.FetchData(queryCat).ConfigureAwait(false);
            List<string> catNames = dtCat.GetColumn<string>("name");
            List<int> catIDs = dtCat.GetColumn<int>("id");
            Dictionary<string, int> categories = new Dictionary<string, int>();
            categories.Merge(catNames, catIDs);

            string queryBusiness = "SELECT id, name FROM business GROUP BY name ORDER BY id ASC;";
            DataTable dtBusiness = await Context.FetchData(queryBusiness).ConfigureAwait(false);
            List<string> businessNames = dtBusiness.GetColumn<string>("name");
            List<int> businessIDs = dtBusiness.GetColumn<int>("id");
            Dictionary<string, int> businesses = new Dictionary<string, int>();
            businesses.Merge(businessNames, businessIDs);

            string queryFacility = "SELECT id, name FROM facilities GROUP BY name ORDER BY id ASC;";
            DataTable dtFacility = await Context.FetchData(queryFacility).ConfigureAwait(false);
            List<string> facilityNames = dtFacility.GetColumn<string>("name");
            List<int> facilityIDs = dtFacility.GetColumn<int>("id");
            Dictionary<string, int> facilities = new Dictionary<string, int>();
            facilities.Merge(facilityNames, facilityIDs);

            string queryAssetStatus = "SELECT id, name FROM assetstatus GROUP BY name ORDER BY id ASC;";
            DataTable dtAssetStatus = await Context.FetchData(queryAssetStatus).ConfigureAwait(false);
            List<string> assetStatusNames = dtAssetStatus.GetColumn<string>("name");
            List<int> assetStatusIDs = dtAssetStatus.GetColumn<int>("id");
            Dictionary<string, int> assetStatuses = new Dictionary<string, int>();
            assetStatuses.Merge(assetStatusNames, assetStatusIDs);

            string queryAssetType = "SELECT id, name FROM assettypes GROUP BY name ORDER BY id ASC;";
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

            string queryMapBlocks = "SELECT id, CASE WHEN parentId=0 THEN id ELSE parentId END as parent FROM facilities;";
            DataTable dtMapBlocks = await Context.FetchData(queryMapBlocks).ConfigureAwait(false);
            List<int> blockParents = dtMapBlocks.GetColumn<int>("parent");
            List<int> blockIDs = dtMapBlocks.GetColumn<int>("id");
            Dictionary<int, int> mapBlockToParent = new Dictionary<int, int>();
            mapBlockToParent.Merge(blockIDs, blockParents);

            string queryWarrantyType = "SELECT id, name FROM warrantytype;";
            DataTable dtWarrantyTypes = await Context.FetchData(queryWarrantyType).ConfigureAwait(false);
            List<string> warrantyTypeNames = dtWarrantyTypes.GetColumn<string>("name");
            List<int> warrantyTypesIDs = dtWarrantyTypes.GetColumn<int>("id");
            Dictionary<string, int> warrantyTypes = new Dictionary<string, int>();
            warrantyTypes.Merge(warrantyTypeNames, warrantyTypesIDs);

            string queryWarrantyTerm = "SELECT id, name FROM warrantyusageterm;";
            DataTable dtWarrantyTerms = await Context.FetchData(queryWarrantyTerm).ConfigureAwait(false);
            List<string> warrantyTermNames = dtWarrantyTerms.GetColumn<string>("name");
            List<int> warrantyTermIDs = dtWarrantyTerms.GetColumn<int>("id");
            Dictionary<string, int> warrantyTerms = new Dictionary<string, int>();
            warrantyTerms.Merge(warrantyTermNames, warrantyTermIDs);

            Dictionary<string, Tuple<string, Type>> columnNames = new Dictionary<string, Tuple<string, Type>>()
            {
                { "Asset_Name", new Tuple<string, Type>("name", typeof(string)) },
                { "Asset_Description", new Tuple<string, Type>("description", typeof(string)) },
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
                { "Asset_Warranty_Certificate_No", new Tuple<string, Type>("certificate_number", typeof(string)) }
            };

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
                        m_errorLog.SetWarning("Sheet containing assets should be named 'Assets'");
                    else
                    {
                        DataTable dt2 = new DataTable();
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
                        dt2.Columns.Add("row_no", typeof(int));
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
                            if (Convert.ToString(newR["name"]) == null || Convert.ToString(newR["name"]) == "")
                                continue;
                            newR["row_no"] = rN;
                            try
                            {
                                newR["blockId"] = facilities[Convert.ToString(newR["Asset_Facility_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Invalid Block named '{newR["Asset_Facility_Name"]}'.");
                            }
                            try
                            {
                                newR["facilityId"] = mapBlockToParent[Convert.ToInt32(newR["blockId"])];
                            }
                            catch (InvalidCastException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Facility cannot be linked to asset if block named '{newR["Asset_Facility_Name"]}' does not exist.");
                            }
                            try
                            {
                                newR["categoryId"] = categories[Convert.ToString(newR["Asset_category_name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Invalid Asset Category named '{newR["Asset_category_name"]}'.");
                            }
                            try
                            {
                                newR["customerId"] = businesses[Convert.ToString(newR["Asset_Customer_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Customer business named '{newR["Asset_Customer_Name"]}' not found.");
                            }
                            try
                            {
                                newR["ownerId"] = businesses[Convert.ToString(newR["Asset_Owner_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Owner business named '{newR["Asset_Owner_Name"]}' not found.");
                            }
                            try
                            {
                                newR["operatorId"] = businesses[Convert.ToString(newR["Asset_Operator_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Operator business named '{newR["Asset_Operator_Name"]}' not found.");
                            }
                            try
                            {
                                newR["supplierId"] = businesses[Convert.ToString(newR["Asset_Supplier_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Supplier business named '{newR["Asset_Supplier_Name"]}' not found.");
                            }
                            try
                            {
                                newR["manufacturerId"] = businesses[Convert.ToString(newR["Asset_Manufacturer_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Manufacturer business named '{newR["Asset_Manufacturer_Name"]}' not found.");
                            }
                            try
                            {
                                newR["currencyId"] = currencies[Convert.ToString(newR["currency"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Currency with code '{newR["currency"]}' not found.");
                            }
                            try
                            {
                                newR["typeId"] = assetTypes[Convert.ToString(newR["Asset_Type_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Asset type named '{newR["Asset_Type_Name"]}' not found.");
                            }
                            try
                            {
                                newR["statusId"] = assetStatuses[Convert.ToString(newR["Asset_Status_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Row: {rN}] Asset status named '{newR["Asset_Status_Name"]}' not found.");
                            }
                            try
                            {
                                newR["warranty_type"] = warrantyTypes[Convert.ToString(newR["Warranty Type"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetWarning($"[Row: {rN}] Warranty Type named '{newR["Warranty Type"]}' not found. Setting warranty Type ID as 0.");
                                newR["warranty_type"] = 0;
                            }
                            try
                            {
                                newR["warrranty_term_type"] = warrantyTerms[Convert.ToString(newR["Asset_Warranty_Term"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetWarning($"[Row: {rN}] Warranty Term named '{newR["Asset_Warranty_Term"]}' not found. Setting warranty term ID as 0.");
                                newR["warrranty_term_type"] = 0;
                            }
                            try
                            {
                                newR["warranty_provider_id"] = businesses[Convert.ToString(newR["Asset_warranty_Provider"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetWarning($"[Row: {rN}] Warranty Provider named '{newR["Asset_warranty_Provider"]}' not found. Setting warranty provider ID as 0.");
                                newR["warranty_provider_id"] = 0;
                            }
                            if (Convert.ToString(newR["Asset_Parent_Name"]) == null || Convert.ToString(newR["Asset_Parent_Name"]) == "")
                            {
                                newR["Asset_Parent_Name"] = "";
                            }
                            /*
                        dt2.Columns.Add("warranty_type", typeof(int));
                        dt2.Columns.Add("warrranty_term_type", typeof(int));
                        dt2.Columns.Add("warranty_provider_id", typeof(int));*/
                            dt2.Rows.Add(newR);
                        }
                        if (m_errorLog.GetErrorCount() == 0)
                        {
                            string assetQry = "SELECT name FROM assets";
                            DataTable assetDt = await Context.FetchData(assetQry).ConfigureAwait(false);

                            List<List<string>> assetList = new List<List<string>>() { assetDt.GetColumn<string>("name") };

                            List<DataTable> assetPriority = new List<DataTable>();

                            List<string> assetnames = new List<string>();
                            assetnames.AddRange(assetDt.GetColumn<string>("name"));
                            assetnames.AddRange(dt2.GetColumn<string>("name"));
                            assetnames.Contains("");
                            DataRow[] filterRows = dt2.AsEnumerable()
                                       .Where(row => !assetnames.Contains(row.Field<string>("Asset_Parent_Name"), StringComparison.OrdinalIgnoreCase))
                                       .ToArray();
                            if (filterRows.Length > 0)
                            {
                                assetPriority.Insert(0, filterRows.CopyToDataTable());
                                assetList.Insert(0, assetPriority[assetPriority.Count - 1].GetColumn<string>("name"));
                            }

                            foreach (var item in assetList)
                            {
                                List<string> temp = item;
                                do
                                {
                                    filterRows = dt2.AsEnumerable()
                                       .Where(row => temp.Contains(row.Field<string>("Asset_Parent_Name"), StringComparison.OrdinalIgnoreCase))
                                       .ToArray();
                                    if (filterRows.Length == 0)
                                        continue;
                                    assetPriority.Add(filterRows.CopyToDataTable());
                                    temp = assetPriority[assetPriority.Count - 1].GetColumn<string>("name");
                                } while (filterRows.Length != 0);
                            }
                            List<int> idList = new List<int>();

                            foreach (DataTable dtUsers in assetPriority)
                            {
                                idList.AddRange((await AddInventoryWithParent(dtUsers, userID)).id);
                            }

                            response = new CMImportFileResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, null, null, $"{idList.Count} new assets added.");

                        }
                    }
                }
                else //
                {
                    m_errorLog.SetError("File is not an excel file");
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
                m_errorLog.SetImportInformation("File imported successfully");
                response.error_log_file_path = logPath;
                response.import_log = m_errorLog.errorLog();
            }
            
            return response;
            //return response;*/
        }

        internal async Task<CMDefaultResponse> AddInventoryWithParent(DataTable assets, int userID)
        {
            string assetQry = "SELECT id, UPPER(name) as name FROM assets GROUP BY name ORDER BY id;";
            DataTable dtAsset = await Context.FetchData(assetQry).ConfigureAwait(false);
            List<string> assetNames = dtAsset.GetColumn<string>("name");
            List<int> assetIds = dtAsset.GetColumn<int>("id");
            Dictionary<string, int> assetDict = new Dictionary<string, int>();
            assetDict.Merge(assetNames, assetIds);

            foreach (DataRow row in assets.Rows)
            {
                try
                {
                    row["parentId"] = assetDict[Convert.ToString(row["Asset_Parent_Name"]).ToUpper()];
                }
                catch (KeyNotFoundException)
                {
                    m_errorLog.SetWarning($"[Row: {row["row_no"]}] Asset named '{Convert.ToString(row["Asset_Parent_Name"])}' not found. Setting parent ID as 0.");
                    row["parentId"] = 0;
                }
            }
            List<CMAddInventory> importAssets = assets.MapTo<CMAddInventory>();
            CMDefaultResponse response = await AddInventory(importAssets, userID);
            return response;
        }

        internal async Task<List<CMInventoryList>> GetInventoryList(int facilityId, int linkedToBlockId, int status, string categoryIds)
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
                "SELECT a.id, a.name, a.description, ast.name as type, b2.name as supplierName, b5.name as operatorName, a.categoryId , ac.name as categoryName, a.serialNumber,a.specialTool, a.warrantyId" +

                ", f.name AS facilityName, f.id AS facilityId, bl.id AS blockId, bl.name AS blockName, linkedbl.id AS linkedToBlockId, linkedbl.name AS linkedToBlockName, a.parentId, a2.name as parentName, custbl.name as customerName, owntbl.name as ownerName, s.name AS status, b5.name AS operatorName" +

                " from assets as a " +
                "join assettypes as ast on ast.id = a.typeId " +
                "" +
                "join assetcategories as ac on ac.id= a.categoryId " +
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
                "JOIN facilities as f ON f.id = a.facilityId " +
                "" +
                "JOIN facilities as linkedbl ON linkedbl.id = a.linkedToBlockId " +
                "" +
                "JOIN facilities as bl ON bl.id = a.blockId   WHERE a.status = " + status ;

            myQuery += (facilityId > 0 ? " AND a.facilityId= " + facilityId  + "" : " ");
            myQuery += (linkedToBlockId > 0 ? " AND a.linkedToBlockId= " + linkedToBlockId + "" : " ");
            myQuery += (categoryIds?.Length > 0 ? " AND a.categoryId IN (" + categoryIds + ")" : " ");

            List<CMInventoryList> inventory = await Context.GetData<CMInventoryList>(myQuery).ConfigureAwait(false);
            return inventory;
        }

        internal async Task<CMViewInventory> GetInventoryDetails(int id)
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
            string myQuery = "SELECT a.id ,a.name, a.description, ast.id as typeId, ast.name as type, b2.id as supplierId, b2.name as supplierName, manufacturertlb.id as manufacturerId, manufacturertlb.name as manufacturerName, b5.id as operatorId, b5.name as operatorName, ac.id as categoryId, ac.name as categoryName, a.serialNumber, a.calibrationFrequency, a.calibrationFreqType, a.calibrationReminderDays, CASE WHEN a.calibrationLastDate = '0000-00-00 00:00:00' THEN NULL ELSE a.calibrationLastDate END as calibrationLastDate, CASE WHEN a.calibrationDueDate = '0000-00-00 00:00:00' THEN NULL ELSE a.calibrationDueDate END AS calibrationDueDate, a.model, a.currency, a.cost, a.acCapacity, a.dcCapacity, a.moduleQuantity, " +
            //a.firstDueDate as calibrationDueDate, 
            "f.id as facilityId, f.name AS facilityName, bl.id as blockId, bl.name AS blockName, a2.id as parentId, a2.name as parentName, a2.serialNumber as parentSerial, custbl.id as customerId, custbl.name as customerName, owntbl.id as ownerId, owntbl.name as ownerName, s.id as statusId, s.name AS status, a.specialTool, w.id as warrantyId, w.warranty_description, w.certificate_number, wt.id as warrantyTypeId, wt.name as warrantyTypeName, wut.id as warrantyTermTypeId, wut.name as warrantyTermTypeName, wp.id as warrantyProviderId, wp.name as warrantyProviderName, files.file_path as warranty_certificate_path " +     //use a.specialToolEmpId to put specialToolEmp,
            "from assets as a " +
            "left join assettypes as ast on ast.id = a.typeId " +
            "left join assetcategories as ac on ac.id= a.categoryId " +
            "left join business as custbl on custbl.id = a.customerId " +
            "left join business as owntbl" + " on owntbl.id = a.ownerId " +
            "left JOIN business AS manufacturertlb ON a.ownerId = manufacturertlb.id " +
            "left JOIN business AS b2 ON a.ownerId = b2.id " +
            "left JOIN business as b5 ON b5.id = a.operatorId " +
            "left JOIN assets as a2 ON a.parentId = a2.id " +
            "left JOIN assetstatus as s on s.id = a.status " +
            "left JOIN facilities as f ON f.id = a.facilityId " +
            "left JOIN facilities as bl ON bl.id = a.blockId " +
            "left join assetwarranty as w ON a.warrantyId = w.id " +
            "left join uploadedfiles as files ON files.id = w.certificate_file_id " +
            "left join warrantytype as wt ON w.warranty_type = wt.id " +
            "left join warrantyusageterm as wut ON w.warranty_term_type = wut.id " +
            "left join business as wp ON w.warranty_provider = wp.id";
            if (id != 0)
            {
                myQuery += " WHERE a.id= " + id;
            }
            List<CMViewInventory> _ViewInventoryList = await Context.GetData<CMViewInventory>(myQuery).ConfigureAwait(false);

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
            List<int> idList = new List<int>();
            foreach (var unit in request)
            {
                string qry = "insert into assets (name, description, parentId, acCapacity, dcCapacity, categoryId, typeId, statusId, facilityId, blockId, linkedToBlockId, customerId, ownerId,operatorId, manufacturerId,supplierId,serialNumber,createdBy,photoId,model,stockCount,moduleQuantity, cost,currency,specialTool,specialToolEmpId,calibrationDueDate,calibrationLastDate,calibrationFreqType,calibrationFrequency,calibrationReminderDays,retirementStatus,multiplier,vendorId) values ";
                count++;
                assetName = unit.name;
                if (assetName.Length <= 0)
                {
                    throw new ArgumentException($"name of asset cannot be empty on line {count}");
                }
                string firstCalibrationDate = (unit.calibrationFirstDueDate == null) ? "NULL" : ((DateTime)unit.calibrationFirstDueDate).ToString("yyyy-MM-dd");
                string lastCalibrationDate = (unit.calibrationLastDate == null) ? "NULL" : ((DateTime)unit.calibrationLastDate).ToString("yyyy-MM-dd");
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
                if(unit.vendorId <= 0)
                {
                    unit.vendorId = unit.manufacturerId;
                }
                /*
string warrantyQry = "insert into assetwarranty 
(warranty_type, warranty_description, warranty_term_type, asset_id, start_date, expiry_date, meter_limit, meter_unit, warranty_provider, certificate_number,
                addedAt, addedBy, updatedAt, updatedBy, status) VALUES ";
            */
                qry += "('" + unit.name + "','" + unit.description + "','" + unit.parentId + "','" + unit.acCapacity + "','" + unit.dcCapacity + "','" + unit.categoryId + "','" + unit.typeId + "','" + unit.statusId + "','" + unit.facilityId + "','" + unit.blockId + "','" + unit.blockId + "','" + unit.customerId + "','" + unit.ownerId + "','" + unit.operatorId + "','" + unit.manufacturerId + "','" + unit.supplierId + "','" + unit.serialNumber + "','" + userID + "','" + unit.photoId + "','" + unit.model + "','" + unit.stockCount + "','" + unit.moduleQuantity + "','" + unit.cost + "','" + unit.currency + "','" + unit.specialToolId + "','" + unit.specialToolEmpId + "','" + firstCalibrationDate + "','" + lastCalibrationDate + "','" + unit.calibrationFrequencyType + "','" + unit.calibrationFrequencyType + "','" + unit.calibrationReminderDays + "','" + unit.retirementStatus + "','" + unit.multiplier + "','" + unit.vendorId + "'); ";
                qry += "select LAST_INSERT_ID(); ";

                //List<CMInventoryList> newInventory = await Context.GetData<CMInventoryList>(qry).ConfigureAwait(false);
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                retID = Convert.ToInt32(dt.Rows[0][0]);
                if (unit.warranty_type > 0 && unit.warrranty_term_type > 0 && unit.warranty_provider_id > 0)
                {
                    string warrantyQry = "insert into assetwarranty (certificate_file_id, warranty_type, warranty_description, warranty_term_type, asset_id, start_date, expiry_date, meter_limit, meter_unit, warranty_provider, certificate_number, addedAt, addedBy, status) VALUES ";
                    warrantyQry += $"({unit.warranty_certificate_file_id}, {unit.warranty_type}, '{unit.warranty_description}', {unit.warrranty_term_type}, {retID}, '{((DateTime)unit.start_date).ToString("yyyy-MM-dd HH:mm:ss")}', '{((DateTime)unit.expiry_date).ToString("yyyy-MM-dd HH:mm:ss")}', {unit.meter_limit}, {unit.meter_unit}, {unit.warranty_provider_id}, '{unit.certificate_number}', '{UtilsRepository.GetUTCTime()}', {userID}, 1);" +
                        $" SELECT LAST_INSERT_ID();";
                    DataTable dt2 = await Context.FetchData(warrantyQry).ConfigureAwait(false);
                    int warrantyId = Convert.ToInt32(dt2.Rows[0][0]);
                    string addWarrantyId = $"UPDATE assets SET warrantyId = {warrantyId} WHERE id = {retID}";
                    await Context.ExecuteNonQry<int>(addWarrantyId).ConfigureAwait(false);
                }
                idList.Add(retID);
                CMViewInventory _inventoryAdded = await GetInventoryDetails(retID);

                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED);
                _inventoryAdded.status_short = _shortStatus;

                string _longStatus = getLongStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED, _inventoryAdded);
                _inventoryAdded.status_long = _longStatus;

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_ADDED, new[] { userID }, _inventoryAdded);
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
            if (request.description != null)
            {
                updateQry += $" description = '{request.description}',";
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

            CMViewInventory _inventoryAdded = await GetInventoryDetails(request.id);

            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_UPDATED);
            _inventoryAdded.status_short = _shortStatus;

            string _longStatus = getLongStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_UPDATED, _inventoryAdded);
            _inventoryAdded.status_long = _longStatus;

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_UPDATED, new[] { userID } ,_inventoryAdded);

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

            CMViewInventory _inventoryAdded = await GetInventoryDetails(id);

            string qry = $"SELECT CONCAT(firstName,' ',lastName) as deleted_by FROM users WHERE id = {id}";

            List<CMViewInventory> deleted_by = await Context.GetData<CMViewInventory>(qry).ConfigureAwait(false);

            _inventoryAdded.deleted_by = deleted_by[0].deleted_by;

            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_DELETED);
            _inventoryAdded.status_short = _shortStatus;

            string _longStatus = getLongStatus(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_DELETED, _inventoryAdded);
            _inventoryAdded.status_long = _longStatus;

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.INVENTORY, CMMS.CMMS_Status.INVENTORY_DELETED, new[] { userID }, _inventoryAdded);

            string delQuery1 = $"DELETE FROM assets WHERE id = {id}";
            string delQuery2 = $"DELETE FROM assetwarranty where asset_id = {id}";
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
        internal async Task<List<CMCalibrationAssets>> GetCalibrationList(int facilityId)
        {
            string myQuery = "SELECT assets.id, assets.name, category.id as categoryId, category.name as categoryName, " +
            "vendor.id as vendorId, vendor.name as vendorName, assets.calibrationFreqType, " +
            "frequency.id as frequencyId, frequency.name as frequencyName, CASE WHEN assets.calibrationLastDate = '0000-00-00 00:00:00' THEN NULL ELSE assets.calibrationLastDate END AS calibrationLastDate, " +
            "CASE WHEN assets.calibrationDueDate = '0000-00-00 00:00:00' THEN NULL ELSE assets.calibrationDueDate END AS calibrationDueDate, assets.calibrationReminderDays " +
            "FROM assets " +
            "LEFT JOIN assetcategories as category ON category.id = assets.categoryId " +
            "LEFT JOIN business as vendor ON vendor.id = assets.vendorId " +
            "LEFT JOIN frequency ON assets.calibrationFrequency = frequency.id "+
            $"WHERE category.calibrationStatus = 1 AND facilityId = {facilityId} ";
            List<CMCalibrationAssets> _AssetCategory = await Context.GetData<CMCalibrationAssets>(myQuery).ConfigureAwait(false);
            return _AssetCategory;
              

        }

        #endregion

    }
}
