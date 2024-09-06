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
            CMImportFileResponse response = null;
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

                            using (var package = new ExcelPackage(path))
                            {
                                if (package.Workbook.Worksheets.Count == 0)
                                {
                                    throw new Exception("No worksheets found in the Excel file. Total worksheets in the file: " + package.Workbook.Worksheets.Count);
                                }

                                dt.TableName = "DSM";
                                var workSheet = package.Workbook.Worksheets["DSM"]; // Assuming data is in the first worksheet

                                int rowNumber = 0;
                                try
                                {
                                    foreach (var header in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                                    {
                                        dt.Columns.Add(header.Text);
                                    }
                                    for (int rN = 2; rN <= workSheet.Dimension.End.Row; rN++)
                                    {
                                        ExcelRange row = workSheet.Cells[rN, 1, rN, workSheet.Dimension.End.Column];
                                        DataRow newR = dt.NewRow();
                                        foreach (var cell in row)
                                        {
                                            try
                                            {
                                                newR[cell.Start.Column - 1] = cell.Text;
                                            }
                                            catch (Exception ex)
                                            {
                                                m_errorLog.SetError($"Exception Caught : " + ex.Message);
                                            }
                                        }
                                        dt.Rows.Add(newR);
                                    }
                                    DataTable dt2 = dt;
                                }
                                catch (Exception ex)
                                {

                                    m_errorLog.SetError($"Exception Caught : " + ex.Message);

                                }
                                foreach (DataRow dr in dt.Rows.Cast<DataRow>().ToArray())
                                {
                                    bool isEmpty = true;
                                    foreach (var item in dr.ItemArray)
                                    {
                                        if (item != DBNull.Value && !string.IsNullOrWhiteSpace(item.ToString()))
                                        {
                                            isEmpty = false;
                                            break;
                                        }
                                    }

                                    if (isEmpty)
                                    {
                                        dr.Delete();
                                    }
                                }
                                string qry = "insert into dsm (fy, month, site,dmsType, vendor, category, dsmPenalty, scheduleKwh, actualKwh) Values ";

                                foreach (DataRow dr in dt.Rows)
                                {

                                    CMDSMImportData unit = new CMDSMImportData
                                    {
                                        fy = dr["Year"] is DBNull || string.IsNullOrEmpty((string)dr["Year"]) ? "" : Convert.ToString(dr["Year"]),

                                        month = dr["Month"] is DBNull || string.IsNullOrEmpty((string)dr["Month"]) ? "Nil" : Convert.ToString(dr["Month"]),

                                        site = dr["Site"] is DBNull || string.IsNullOrEmpty((string)dr["Site"]) ? "Nil" : Convert.ToString(dr["Site"]),

                                        dsmType = dr["DSM Type"] is DBNull || string.IsNullOrEmpty((string)dr["DSM Type"]) ? "Nil" : Convert.ToString(dr["DSM Type"]),

                                        vendor = dr["Vendor"] is DBNull || string.IsNullOrEmpty((string)dr["Vendor"]) ? "Nil" : Convert.ToString(dr["Vendor"]),

                                        category = dr["Category"] is DBNull || string.IsNullOrEmpty((string)dr["Category"]) ? "Nil" : Convert.ToString(dr["Category"]),

                                        dsmPenalty = dr["DSM Penalty (Rs.)"] is DBNull || string.IsNullOrEmpty((string)dr["DSM Penalty (Rs.)"]) ? 0 : Convert.ToInt32(dr["DSM Penalty (Rs.)"]),

                                        scheduleKwh = dr["Schedule (kWh)"] is DBNull || string.IsNullOrEmpty((string)dr["Schedule (kWh)"]) ? 0 : Convert.ToInt32(dr["Schedule (kWh)"]),

                                        actualKwh = dr["Actual (kWh)"] is DBNull || string.IsNullOrEmpty((string)dr["Actual (kWh)"]) ? 0 : Convert.ToInt32(dr["Actual (kWh)"])
                                    };

                                    qry += "('" + unit.fy + "','" + unit.month + "','" + unit.site + "','" + unit.dsmType + "','" + unit.vendor + "','" + unit.category + "','" + unit.dsmPenalty + "','" + unit.scheduleKwh + "','" + unit.actualKwh + "'), ";
                                }

                                qry = qry.Substring(0, (qry.Length - 2)) + ";";
                                await Context.ExecuteNonQry<int>(qry).ConfigureAwait(false);

                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle exceptions
                            Console.WriteLine("An error occurred: " + ex.Message);
                            throw ex;

                        }
                    }
                }
            }
            response = new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.SUCCESS, "", null, "File imported successfully.");

            return response;
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
                         "LEFT JOIN spv ON spv.id = sm.spvId " +
                         "LEFT JOIN states ON states.id = sm.stateId " +
                         "WHERE 1 = 1 " + filter + " " +
                         "GROUP BY fy, month, site";*/
            /* string qry = "SELECT fy, month, sm.site, spv.name AS spv, states.name AS state,ast.name as category, " +
                 "dsm.dsm_type as dsmType, dsm.vendor_id AS forcasterName, dsmPenalty, actualKwh, " +
                 "scheduleKwh, SUM(dsmPenalty / actualKwh) * 100 AS dsmPer FROM dsm  " +
                 "LEFT JOIN site_master AS sm ON sm.site = dsm.site_id \r\nLEFT JOIN spv ON spv.id = sm.spvId  " +
                 "LEFT JOIN  assetcategories ast on ast.id=dsm.category_id\r\nLEFT JOIN states ON states.id = sm.stateId " +
                 "WHERE 1 = 1  GROUP BY fy, month, site;";

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
