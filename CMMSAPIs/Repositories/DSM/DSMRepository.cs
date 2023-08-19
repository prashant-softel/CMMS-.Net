using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.DSM;
using CMMSAPIs.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Reflection.Metadata;
using System.Net;
using Microsoft.AspNetCore.Http;
using static OfficeOpenXml.ExcelErrorValue;

namespace CMMSAPIs.Repositories.DSM
{
    public class DSMRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        ErrorLog m_errorLog;

        public DSMRepository(MYSQLDBHelper sqlDBHelper ,IWebHostEnvironment iwebhostobj) : base(sqlDBHelper)
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
                        m_errorLog.SetWarning("Sheet containing DSM Data should be named 'DSM'");
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

                                string qry = "insert into dsm (year, month, site,dsmType, vendor, category, dsmPenalty, scheduleKwh, actualKwh) Values ";

                                foreach (DataRow dr in dt.Rows)
                                {

                                    CMDSMImportData unit = new CMDSMImportData
                                    {
                                        year = dr["Year"] is DBNull || string.IsNullOrEmpty((string)dr["Year"]) ? 0 : Convert.ToInt32(dr["Year"]),

                                        month = dr["Month"] is DBNull || string.IsNullOrEmpty((string)dr["Month"]) ? "Nil" : Convert.ToString(dr["Month"]),

                                        site = dr["Site"] is DBNull || string.IsNullOrEmpty((string)dr["Site"]) ? "Nil" : Convert.ToString(dr["Site"]),

                                        dsmType = dr["DSM Type"] is DBNull || string.IsNullOrEmpty((string)dr["DSM Type"]) ? "Nil" : Convert.ToString(dr["DSM Type"]),

                                        vendor = dr["Vendor"] is DBNull || string.IsNullOrEmpty((string)dr["Vendor"]) ? "Nil" : Convert.ToString(dr["Vendor"]),

                                        category = dr["Category"] is DBNull || string.IsNullOrEmpty((string)dr["Category"]) ? "Nil" : Convert.ToString(dr["Category"]),

                                        dsmPenalty = dr["DSM Penalty (Rs.)"] is DBNull || string.IsNullOrEmpty((string)dr["DSM Penalty (Rs.)"]) ? 0 : Convert.ToInt32(dr["DSM Penalty (Rs.)"]),

                                        scheduleKwh = dr["Schedule (kWh)"] is DBNull || string.IsNullOrEmpty((string)dr["Schedule (kWh)"]) ? 0 : Convert.ToInt32(dr["Schedule (kWh)"]),

                                        actualKwh = dr["Actual (kWh)"] is DBNull || string.IsNullOrEmpty((string)dr["Actual (kWh)"]) ? 0 : Convert.ToInt32(dr["Actual (kWh)"])
                                    };

                                    qry += "('" + unit.year + "','" + unit.month + "','" + unit.site + "','" + unit.dsmType + "','" + unit.vendor + "','" + unit.category + "','" + unit.dsmPenalty + "','" + unit.scheduleKwh + "','" + unit.actualKwh + "'), ";
                                }

                                qry = qry.Substring(0, (qry.Length - 2)) + ";";
                                await Context.ExecuteNonQry<int>(qry).ConfigureAwait(false);

                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle exceptions
                            Console.WriteLine("An error occurred: " + ex.Message); 
                            
                        }
                    }
                }
            }
            return response;
        }

        public async Task<List<CMDSMData>> getDSMData(CMDSMFilter request)
        {
            string filter = "";

            filter += (request?.fy?.Length > 0 ? " AND fy IN ( '" + string.Join("' , '", request.fy) + "' )" :string.Empty);
            filter += (request?.month?.Length > 0 ? " AND month IN ( '" + string.Join("' , '", request.month) + "' )" : string.Empty);
            filter += (request?.stateId?.Length > 0 ? " AND sm.stateId IN (" + string.Join(",", request.stateId) + ")" : string.Empty);
            filter += (request?.spvId?.Length > 0 ? " AND sm.spvId IN (" + string.Join(",", request.spvId) + ")" : string.Empty);
            filter += (request?.siteId?.Length > 0 ? " AND sm.id IN (" + string.Join(",", request.siteId) + ")" : string.Empty);

            string qry = "select fy , month, dsm.site, spv.name as spv, states.name as state, category, dmsType, vendor as forcasterName ,dsmPenalty, actualKwh , scheduleKwh ,sum(dsmPenalty/actualKwh)*100 as dsmPer from dsm " +
                " left join site_master as sm on sm.site = dsm.site" +
                " left join spv on spv.id = sm.spvId" +
                " left join states on states.id = sm.stateId " +
                " where 1 " + filter + " group by fy, month, site";

            List<CMDSMData> data = await Context.GetData<CMDSMData>(qry).ConfigureAwait(false);

            return data;
        }
       
    }
}
