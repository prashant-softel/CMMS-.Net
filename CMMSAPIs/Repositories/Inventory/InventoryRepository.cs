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
using IronXL;

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
        internal async Task<CMDefaultResponse> SetParentAsset(CMSetParentAsset parent_child_group, int userID)
        {
            string child_list = string.Join(", ", parent_child_group.childAssets);
            string myQuery = $"UPDATE assets SET parentId = {parent_child_group.parentId} WHERE id IN ({child_list});";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            foreach (int child in parent_child_group.childAssets)
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, child, 0, 0, "Parent assigned", CMMS.CMMS_Status.UPDATED, userID);
            return new CMDefaultResponse(parent_child_group.childAssets, CMMS.RETRUNSTATUS.SUCCESS, $"Asset no. {parent_child_group.parentId} assigned as parent to {parent_child_group.childAssets.Count} child assets.");
        }
        internal async Task<List<CMDefaultResponse>> ImportInventories(int file_id, int userID)
        {
            List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();

            string queryAsset = "SELECT id, name FROM assetcategories GROUP BY name ORDER BY id ASC;";
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
                { "Calibration_reminder_days", new Tuple<string, Type>("calibrationReminderDays", typeof(int)) }
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
                            try
                            {
                                newR["blockId"] = facilities[Convert.ToString(newR["Asset_Facility_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"Invalid Block named '{newR["Asset_Facility_Name"]}'. [Reference: '{newR["name"]}']");
                            }
                            try
                            {
                                newR["facilityId"] = mapBlockToParent[Convert.ToInt32(newR["blockId"])];
                            }
                            catch (InvalidCastException)
                            {
                                m_errorLog.SetError($"Facility cannot be linked to asset if block named '{newR["Asset_Facility_Name"]}' does not exist. [Reference: '{newR["name"]}']");
                            }
                            try
                            {
                                newR["categoryId"] = categories[Convert.ToString(newR["Asset_category_name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"Invalid Asset Category named '{newR["Asset_category_name"]}'. [Reference: '{newR["name"]}']");
                            }
                            try
                            {
                                newR["parentId"] = assets[Convert.ToString(newR["Asset_Parent_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetWarning($"Parent Asset named '{newR["Asset_Parent_Name"]}' not found. Setting parent ID as 0. [Reference: '{newR["name"]}']");
                                newR["parentId"] = 0;
                            }
                            try
                            {
                                newR["customerId"] = businesses[Convert.ToString(newR["Asset_Customer_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"Customer business named '{newR["Asset_Customer_Name"]}' not found. [Reference: '{newR["name"]}']");
                            }
                            try
                            {
                                newR["ownerId"] = businesses[Convert.ToString(newR["Asset_Owner_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"Owner business named '{newR["Asset_Owner_Name"]}' not found. [Reference: '{newR["name"]}']");
                            }
                            try
                            {
                                newR["operatorId"] = businesses[Convert.ToString(newR["Asset_Operator_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"Operator business named '{newR["Asset_Operator_Name"]}' not found. [Reference: '{newR["name"]}']");
                            }
                            try
                            {
                                newR["supplierId"] = businesses[Convert.ToString(newR["Asset_Supplier_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"Supplier business named '{newR["Asset_Supplier_Name"]}' not found. [Reference: '{newR["name"]}']");
                            }
                            try
                            {
                                newR["manufacturerId"] = businesses[Convert.ToString(newR["Asset_Manufacturer_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"Manufacturer business named '{newR["Asset_Manufacturer_Name"]}' not found. [Reference: '{newR["name"]}']");
                            }
                            try
                            {
                                newR["currencyId"] = currencies[Convert.ToString(newR["currency"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"Currency with code '{newR["currency"]}' not found. [Reference: '{newR["name"]}']");
                            }
                            try
                            {
                                newR["typeId"] = assetTypes[Convert.ToString(newR["Asset_Type_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"Asset type named '{newR["Asset_Type_Name"]}' not found. [Reference: '{newR["name"]}']");
                            }
                            try
                            {
                                newR["statusId"] = assetStatuses[Convert.ToString(newR["Asset_Status_Name"])];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"Asset status named '{newR["Asset_Status_Name"]}' not found. [Reference: '{newR["name"]}']");
                            }
                            dt2.Rows.Add(newR);
                        }
                        if(m_errorLog.GetErrorCount() == 0)
                        {
                            List<CMAddInventory> importAssets = dt2.MapTo<CMAddInventory>();
                            CMDefaultResponse res = await AddInventory(importAssets, userID);
                            responseList.Add(res);
                        }
                    }
                }
                else //
                {
                    m_errorLog.SetError("File is not an excel file");
                }   
            }
            foreach(var message in m_errorLog.errorLog())
            {
                CMDefaultResponse resp = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, message.ToString());
                responseList.Add(resp);
            }    
            return responseList;           
            //return response;*/
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
                "join business as custbl on custbl.id = a.customerId " +
                "" +
                "join business as owntbl" + " on owntbl.id = a.ownerId " +
                "" +
                "JOIN assets as a2 ON a.parentId = a2.id " +
                "" +
                "JOIN assetstatus as s on s.id = a.status " +
                "" +
                "JOIN business AS b2 ON a.ownerId = b2.id " +

                "JOIN business as b5 ON b5.id = a.operatorId " +
                "" +
                "JOIN facilities as f ON f.id = a.facilityId " +
                "" +
                "JOIN facilities as linkedbl ON linkedbl.id = a.linkedToBlockId " +
                "" +
                "JOIN facilities as bl ON bl.id = a.blockId";
            if (facilityId > 0 || linkedToBlockId > 0)
            {
                if (linkedToBlockId > 0)
                {
                    myQuery += " WHERE a.linkedToBlockId= " + linkedToBlockId;
                }
                else
                {
                    myQuery += " WHERE a.facilityId= " + facilityId;
                }
                if (categoryIds?.Length > 0)
                {
                    myQuery += " AND a.categoryId IN (" + categoryIds + ")";
                }
                if (status > 0)
                {
                    myQuery += " AND a.status = " + status;
                }
            }
            else
            {
                if (status > 0)
                {
                    myQuery += " WHERE a.status = " + status;
                }

                //                throw new ArgumentException("FacilityId or linkedToBlockId cannot be 0");
            }
            List<CMInventoryList> inventory = await Context.GetData<CMInventoryList>(myQuery).ConfigureAwait(false);
            return inventory;
        }

        internal async Task<List<CMViewInventory>> GetInventoryDetails(int id)
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
            string myQuery = "SELECT a.id ,a.name, a.description, ast.name as type, b2.name as supplierName, manufacturertlb.name as manufacturerName, b5.name as operatorName, ac.name as categoryName, a.serialNumber, a.calibrationFrequency, a.calibrationFreqType, a.calibrationReminderDays, a.calibrationLastDate as calibrationLastDate, a.calibrationDueDate, a.model, a.currency, a.cost, a.acCapacity, a.dcCapacity, a.moduleQuantity" +
            //a.firstDueDate as calibrationDueDate, 
            ", f.name AS facilityName, bl.name AS blockName,a2.name as parentName,  custbl.name as customerName,owntbl.name as ownerName, s.name AS status, b5.name AS operatorName, a.specialTool, a.warrantyId" +     //use a.specialToolEmpId to put specialToolEmp,

            " from assets as a " +
            "join assettypes as ast on ast.id = a.typeId " +
            "" +
            "join assetcategories as ac on ac.id= a.categoryId " +
            "" +
            "join business as custbl on custbl.id = a.customerId " +
            "" +
            "join business as owntbl" + " on owntbl.id = a.ownerId " +
            "" +
            "JOIN business AS manufacturertlb ON a.ownerId = manufacturertlb.id " +
            "" +
            "JOIN business AS b2 ON a.ownerId = b2.id " +

            "JOIN business as b5 ON b5.id = a.operatorId " +
            "" +
            "JOIN assets as a2 ON a.parentId = a2.id " +
            "" +
            "JOIN assetstatus as s on s.id = a.status " +
            "" +
            "JOIN facilities as f ON f.id = a.facilityId " +
            "" +
            "JOIN facilities as bl ON bl.id = a.blockId";
            if (id != 0)
            {
                myQuery += " WHERE a.id= " + id;
            }
            List<CMViewInventory> _ViewInventoryList = await Context.GetData<CMViewInventory>(myQuery).ConfigureAwait(false);
            
            return _ViewInventoryList;

            
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
            string qry = "insert into assets (name, description, parentId, acCapacity, dcCapacity, categoryId, typeId, statusId, facilityId, blockId, linkedToBlockId, customerId, ownerId,operatorId, manufacturerId,supplierId,serialNumber,createdBy,photoId,model,stockCount,moduleQuantity, cost,currency,specialTool,specialToolEmpId,calibrationDueDate,calibrationLastDate,calibrationFreqType,calibrationFrequency,calibrationReminderDays,retirementStatus,multiplier) values ";
            int linkedToBlockId = 0;
            foreach (var unit in request)
            {
                count++;
                assetName = unit.name;
                if(assetName.Length <= 0)
                {
                    throw new ArgumentException($"name of asset cannot be empty on line {count}");
                }
                string firstCalibrationDate    = (unit.calibrationFirstDueDate == null)?"NULL":((DateTime)unit.calibrationFirstDueDate).ToString("yyyy-MM-dd");
                string lastCalibrationDate = (unit.calibrationLastDate == null) ? "NULL" : ((DateTime)unit.calibrationLastDate).ToString("yyyy-MM-dd");
                if(unit.blockId > 0)
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
                qry += "('" + unit.name + "','" + unit.description + "','" + unit.parentId + "','" + unit.acCapacity + "','" + unit.dcCapacity + "','" + unit.categoryId + "','" + unit.typeId + "','" + unit.statusId + "','" + unit.facilityId + "','" + unit.blockId + "','" + unit.blockId + "','" + unit.customerId + "','" + unit.ownerId + "','" + unit.operatorId + "','" + unit.manufacturerId + "','" + unit.supplierId + "','" + unit.serialNumber + "','" + userID + "','" + unit.photoId + "','" + unit.model + "','" + unit.stockCount + "','" + unit.moduleQuantity + "','" + unit.cost + "','" + unit.currency + "','" + unit.specialToolId + "','" + unit.specialToolEmpId + "','" + firstCalibrationDate + "','" + lastCalibrationDate + "','" + unit.calibrationFrequencyType + "','" + unit.calibrationFrequencyType + "','" + unit.calibrationReminderDays + "','"+ unit.retirementStatus+ "','"+ unit.multiplier + "'),";
            }
            if (count > 0)
            {
                qry = qry.Substring(0, (qry.Length - 1)) + ";" + "select LAST_INSERT_ID(); ";

                //List<CMInventoryList> newInventory = await Context.GetData<CMInventoryList>(qry).ConfigureAwait(false);
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                retID = Convert.ToInt32(dt.Rows[0][0]);
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
            if (request.blockId!= 0) //here you may have check if its not 0. verify during testig
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

            }
            if (request.lstWarrantyDetail != null)
            {
                CMWarrantyDetail warrentyDetails = request.lstWarrantyDetail[0];
                string certificateNumber = warrentyDetails.certificate_number;
                //warrentyDetails.expiry_date;
                int status = warrentyDetails.status;
                string warrantyDescription = warrentyDetails.warranty_description;
            }
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

        internal async Task<List<CMInventoryTypeList>> GetInventoryTypeList()
        {
            /*
             * Fetch data from assetType
            */
            /*Your code goes here*/
            // "SELECT * FROM assetTypes";
            string myQuery = "SELECT id, name, description, status FROM assettypes where status = 1";
            List<CMInventoryTypeList> _InventoryTypeList = await Context.GetData<CMInventoryTypeList>(myQuery).ConfigureAwait(false);
            return _InventoryTypeList;
        }

        internal async Task<List<CMInventoryStatusList>> GetInventoryStatusList()
        {
            /*
             * Fetch data from assetStatus
            */
            /*Your code goes here*/
            // "SELECT * FROM assetStatus";
            string myQuery = "SELECT id, name, description, status FROM assetstatus where status = 1";
            List<CMInventoryStatusList> _InventoryStatusList = await Context.GetData<CMInventoryStatusList>(myQuery).ConfigureAwait(false);
            return _InventoryStatusList;

        }
        internal async Task<List<CMInventoryCategoryList>> GetInventoryCategoryList()
        {
            string myQuery = "SELECT id, name FROM assetcategories where status = 1";
            List<CMInventoryCategoryList> _AssetCategory = await Context.GetData<CMInventoryCategoryList>(myQuery).ConfigureAwait(false);
            return _AssetCategory;
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
            List<CMDefaultList> warrentyUsageTermType = new List<CMDefaultList> ();
            warrentyUsageTermType.Add(new CMDefaultList() { id = 1, name = "Duration" });
            warrentyUsageTermType.Add(new CMDefaultList() { id = 2, name = "Meter" });
            warrentyUsageTermType.Add(new CMDefaultList() { id = 3, name = "Earier of duration or meter" });
            warrentyUsageTermType.Add(new CMDefaultList() { id = 4, name = "Process completion" });
            return warrentyUsageTermType;
        }
    }
}
