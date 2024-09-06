using CMMSAPIs.Helper;
using CMMSAPIs.Models.DSM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.DSM
{
    public class DSMRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;

        public DSMRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(iwebhostobj);

        }

        internal async Task<CMImportFileResponse> importDSMFile(int file_id, int userID)
        {
            var dictionayfordsm = new Dictionary<string, string>()
            {
                {"site","SELECT id,site as name FROM site_master;" },
                { "Vendor","SELECT id,name FROM business where type=10 ;" },
                { "category","SELECT id,name FROM dsm_category Where status=1 ;"},
                {"dsmtype","SELECT id,name FROM dsm_type where status=1 ;"}
            };
            Dictionary<string, DataTable> dsmdic = await Context.ExecuteNonQryDictonary(dictionayfordsm).ConfigureAwait(false);
            DataTable siteTable = dsmdic["site"];
            DataTable vendorTable = dsmdic["Vendor"];
            DataTable categoryTable = dsmdic["category"];
            DataTable dsmtypeTable = dsmdic["dsmtype"];

            Dictionary<string, int> siteDictionary = new Dictionary<string, int>();
            Dictionary<string, int> dsmTypeDictionary = new Dictionary<string, int>();
            Dictionary<string, int> vendorDictionary = new Dictionary<string, int>();
            Dictionary<string, int> CategoryDictionary = new Dictionary<string, int>();

            List<string> siteNames = siteTable.AsEnumerable().Select(row => row.Field<string>("name")).ToList();
            List<int> siteIds = siteTable.AsEnumerable().Select(row => row.Field<int>("id")).ToList();
            siteDictionary.Merge(siteNames.ToUpper(), siteIds);

            List<string> dsmTypeNames = dsmtypeTable.AsEnumerable().Select(row => row.Field<string>("name")).ToList();
            List<int> dsmTypeIds = dsmtypeTable.AsEnumerable().Select(row => row.Field<int>("id")).ToList();
            dsmTypeDictionary.Merge(dsmTypeNames.ToUpper(), dsmTypeIds);

            List<string> CategoryType = categoryTable.AsEnumerable().Select(row => row.Field<string>("name")).ToList();
            List<int> CategoryIds = categoryTable.AsEnumerable().Select(row => row.Field<int>("id")).ToList();
            CategoryDictionary.Merge(CategoryType.ToUpper(), CategoryIds);

            List<string> vendorNames = vendorTable.AsEnumerable().Select(row => row.Field<string>("name")).ToList();
            List<int> vendorIds = vendorTable.AsEnumerable().Select(row => row.Field<int>("id")).ToList();
            vendorDictionary.Merge(vendorNames.ToUpper(), vendorIds);

            Dictionary<string, Tuple<string, Type>> Columnames = new Dictionary<string, Tuple<string, Type>>()
            {

                {"FY",new Tuple<string, Type>("FY",typeof(string))},
                {"Month",new Tuple<string, Type>("Month",typeof(string))},
                {"Site",new Tuple<string, Type>("Site",typeof(string))},
                {"DSM Type",new Tuple<string, Type>("DSM Type",typeof(string))},
                {"Vendor",new Tuple<string, Type>("Vendor",typeof(string))},
                {"Category",new Tuple<string, Type>("Category",typeof(string))},
                {"DSM Penalty (Rs.)",new Tuple<string, Type>("DSM Penalty (Rs.)",typeof(Int64))},
                {"Schedule (kWh)",new Tuple<string, Type>("Schedule (kWh)",typeof(Int64))},
                {"Actual (kWh)",new Tuple<string, Type>("Actual (kWh)",typeof(Int64))},
            };

            CMImportFileResponse response = null;
            string query1 = $"SELECT file_path FROM uploadedfiles WHERE id = {file_id};";
            DataTable dt1 = await Context.FetchData(query1).ConfigureAwait(false);
            string path = Convert.ToString(dt1.Rows[0][0]);
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                m_errorLog.SetError($"Directory '{dir}' cannot be found");
            }
            else if (!File.Exists(path))
            {
                m_errorLog.SetError($"File '{path}' cannot be found in directory '{dir}'");
            }
            else
            {
                FileInfo info = new FileInfo(path);
                if (info.Extension == ".xlsx")
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var excel = new ExcelPackage(new FileInfo(path)))
                    {
                        var sheet = excel.Workbook.Worksheets["DSM"];
                        if (sheet == null)
                        {
                            return new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, null, null, "Invalid sheet name. Sheet name must be DSM");
                        }
                        else
                        {

                            List<CMDSMImportData> dataList = new List<CMDSMImportData>();

                            try
                            {
                                DataTable dt = new DataTable();
                                foreach (var header in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                                {
                                    try
                                    {
                                        dt.Columns.Add(Columnames[header.Text].Item1, Columnames[header.Text].Item2);
                                    }
                                    catch (KeyNotFoundException)
                                    {
                                        dt.Columns.Add(header.Text);
                                    }
                                }
                                dt.Columns.Add("year", typeof(string));
                                dt.Columns.Add("month", typeof(string));
                                dt.Columns.Add("site_id", typeof(int));
                                dt.Columns.Add("dsm_type", typeof(int));
                                dt.Columns.Add("vendor_id", typeof(int));
                                dt.Columns.Add("category_id", typeof(int));
                                dt.Columns.Add("dsm_panelty", typeof(Int64));
                                dt.Columns.Add("schedule", typeof(Int64));
                                dt.Columns.Add("actual", typeof(Int64));

                                List<string> headers = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
                                foreach (var item in Columnames.Values)
                                {
                                    if (!headers.Contains(item.Item1))
                                    {
                                        dt.Columns.Add(item.Item1, item.Item2);
                                    }
                                }
                                for (int rN = 2; rN <= sheet.Dimension.End.Row; rN++)
                                {
                                    DataRow newR = dt.NewRow();
                                    ExcelRange row = sheet.Cells[rN, 1, rN, sheet.Dimension.End.Column];

                                    foreach (var cell in row)
                                    {

                                        try
                                        {
                                            if (string.IsNullOrWhiteSpace(cell.Text)) continue;
                                            newR[cell.Start.Column - 1] = Convert.ChangeType(cell.Text, dt.Columns[cell.Start.Column - 1].DataType);
                                        }
                                        catch (Exception ex)
                                        {
                                            m_errorLog.SetError($"Error in cell {cell.Address}: {ex.Message}");
                                        }
                                    }
                                    if (newR.IsEmpty())
                                    {
                                        m_errorLog.SetInformation($"Row {rN} is empty.");
                                        continue;
                                    }

                                    if (newR["FY"].ToString() != "")
                                    {
                                        newR["year"] = newR["FY"].ToString();
                                    }
                                    else
                                    {
                                        m_errorLog.SetError($"FY cannot be empty. [Row: {rN}]");
                                    }
                                    if (newR["Month"].ToString() != "")
                                    {
                                        newR["month"] = newR["Month"].ToString();
                                    }
                                    else
                                    {
                                        m_errorLog.SetError($"Month cannot be empty. [Row: {rN}]");
                                    }
                                    try
                                    {

                                        newR["site_id"] = siteDictionary[Convert.ToString(newR["Site"]).ToUpper()];
                                    }
                                    catch
                                    {
                                        m_errorLog.SetError($"Invalid  Site Name. [Row: {rN}]");
                                        newR["site_id"] = 0;

                                    }
                                    try
                                    {
                                        newR["dsm_type"] = dsmTypeDictionary[Convert.ToString(newR["DSM Type"]).ToUpper()];
                                    }
                                    catch
                                    {
                                        m_errorLog.SetError($"Invalid  DSM Type. [Row: {rN}]");
                                        newR["dsm_type"] = 0;
                                    }
                                    try
                                    {
                                        newR["vendor_id"] = vendorDictionary[Convert.ToString(newR["Vendor"]).ToUpper()];
                                    }
                                    catch
                                    {
                                        m_errorLog.SetError($"Invalid Vendro Name. [Row: {rN}]");
                                        newR["vendor_id"] = 0;
                                    }
                                    try
                                    {
                                        newR["category_id"] = CategoryDictionary[Convert.ToString(newR["Category"]).ToUpper()];
                                    }
                                    catch
                                    {
                                        m_errorLog.SetError($"Invalid Category Name. [Row: {rN}]");
                                        newR["category_id"] = 0;
                                    }
                                    if (newR["DSM Penalty (Rs.)"].ToString() != "")
                                    {
                                        newR["dsm_panelty"] = Convert.ToInt64(newR["DSM Penalty (Rs.)"]);
                                    }
                                    else
                                    {
                                        m_errorLog.SetError($"DSM Penalty cannot be empty. [Row: {rN}]");
                                    }
                                    if (newR["Schedule (kWh)"].ToString() != "")
                                    {
                                        newR["schedule"] = Convert.ToInt64(newR["Schedule (kWh)"]);
                                    }
                                    else
                                    {
                                        m_errorLog.SetError($"Schedule (kWh) cannot be empty. [Row: {rN}]");
                                    }
                                    if (newR["Actual (kWh)"].ToString() != "")
                                    {
                                        newR["actual"] = Convert.ToInt64(newR["Actual (kWh)"]);
                                    }
                                    else
                                    {
                                        m_errorLog.SetError($"Actual (kWh) cannot be empty. [Row: {rN}]");
                                    }
                                    dt.Rows.Add(newR);
                                }

                                dt.Clone();
                                List<CMDSMImportData> importDSm = dt.MapTo<CMDSMImportData>();
                                List<int> idList = new List<int>();

                                idList.AddRange((await Createdsmdata(importDSm, userID)).id);

                                response = new CMImportFileResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, null, null, $"{idList.Count} Dsm Imported successfully");

                            }
                            catch (Exception ex)
                            {
                                m_errorLog.SetError($"Exception Caught : " + ex.Message);
                            }

                        }
                    }
                }
            }
            return response;
        }

        internal async Task<CMDefaultResponse> Createdsmdata(List<CMDSMImportData> request, int userID)
        {

            CMDefaultResponse response;
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.SUCCESS;
            string strRetMessage = "";
            List<int> idList = new List<int>();
            int id = 0;
            string qry = "insert into dsm (fy, month, site_id,dsm_type, vendor_id, category_id, dsmPenalty, scheduleKwh, actualKwh) Values ";
            foreach (CMDSMImportData unit in request)
            {
                qry += "('" + unit.fy + "','" + unit.month + "','" + unit.site_id + "','" + unit.dsm_type + "','" + unit.vendor_id + "','" + unit.category_id + "','" + unit.dsm_panelty + "','" + unit.schedule + "','" + unit.actual + "') ,";
            }
            qry = qry = qry.TrimEnd(',') + ";";
            try
            {
                id = await Context.ExecuteNonQry<int>(qry).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            idList.Add(id);
            if (idList.Count > 0)
            {
                strRetMessage = "DSM  <" + id + "> added";
            }
            else
            {
                strRetMessage = "No Dsm Data Added";
            }
            return new CMDefaultResponse(idList, retCode, strRetMessage);
        }
        public async Task<List<CMDSMData>> getDSMData(string fy, string month, string stateId, string spvId, string siteId)
        {
            string[] fyArray = null;
            string[] monthArray = null;



            if (!string.IsNullOrEmpty(fy))
            {
                fyArray = fy.Split(',');
            }
            else
            {
                fyArray = new string[] { };
            }

            if (!string.IsNullOrEmpty(month))
            {
                monthArray = month.Split(',');
            }
            else
            {
                monthArray = new string[] { };
            }

            string filter = "";

            filter += (fyArray.Length > 0 ? " AND fy IN ( '" + string.Join("', '", fyArray) + "' )" : string.Empty);
            filter += (monthArray.Length > 0 ? " AND month IN ( '" + string.Join("', '", monthArray) + "' )" : string.Empty);
            filter += (!string.IsNullOrEmpty(stateId) ? " AND sm.stateId IN (" + string.Join(",", stateId) + ")" : string.Empty);
            filter += (!string.IsNullOrEmpty(spvId) ? " AND sm.spvId IN (" + string.Join(",", spvId) + ")" : string.Empty);
            filter += (!string.IsNullOrEmpty(siteId) ? " AND sm.id IN (" + string.Join(",", siteId) + ")" : string.Empty);

            /*string qry = "SELECT fy, month, dsm.site, spv.name AS spv, states.name AS state, category, dmsType as dsmType, vendor AS forcasterName, dsmPenalty, actualKwh, scheduleKwh, " +
                         "SUM(dsmPenalty / actualKwh) * 100 AS dsmPer " +
                         "FROM dsm " +
                         "LEFT JOIN site_master AS sm ON sm.site = dsm.site_id " +
            string qry = "SELECT dsm.id as id , fy, month,sm.site as site,dsm.site_id ,spv.id as spv_id, sm.stateId, spv.name AS spv, states.name AS state,dct.name as  category, " +
                         "dst.name as dsmtype ,dst.id as dsm_type_id , " +
                         "bus.name  AS forcasterName, dsmPenalty, actualKwh, scheduleKwh, " +
                         "SUM(dsmPenalty / actualKwh) * 100 AS dsmPer " +
                         "FROM dsm " +
                         "LEFT JOIN site_master AS sm ON sm.id = dsm.site_id " +
                         "LEFT JOIN spv ON spv.id = sm.spvId " +
                         "LEFT JOIN dsm_category as dct on dsm.category_id = dct.id " +
                         "LEFT JOIN business as bus ON bus.id = dsm.vendor_id " +
                         "LEFT JOIN dsm_type as dst on dsm.dsm_type = dst.id  " +
                         "LEFT JOIN states ON states.id = sm.stateId " +
                         "WHERE 1 = 1 " + filter + " " +
                         "GROUP BY fy, month, site";*/
            /* string qry = "SELECT fy, month, sm.site, spv.name AS spv, states.name AS state,ast.name as category, " +
                 "dsm.dsm_type as dsmType, dsm.vendor_id AS forcasterName, dsmPenalty, actualKwh, " +
                 "scheduleKwh, SUM(dsmPenalty / actualKwh) * 100 AS dsmPer FROM dsm  " +
                 "LEFT JOIN site_master AS sm ON sm.site = dsm.site_id \r\nLEFT JOIN spv ON spv.id = sm.spvId  " +
                 "LEFT JOIN  assetcategories ast on ast.id=dsm.category_id\r\nLEFT JOIN states ON states.id = sm.stateId " +
                 "WHERE 1 = 1  GROUP BY fy, month, site;";

                         "GROUP BY dsm.id, fy, month, site";
             List<CMDSMData> data = await Context.GetData<CMDSMData>(qry).ConfigureAwait(false);*/
            return null;
        }
        public async Task<List<DSMTYPE>> getDSMType()
        {
            string competencyQry = $"SELECT id, name FROM dsm_type where status=1 ";
            List<DSMTYPE> competencyList = await Context.GetData<DSMTYPE>(competencyQry).ConfigureAwait(false);
            return competencyList;


        }
    }
}
