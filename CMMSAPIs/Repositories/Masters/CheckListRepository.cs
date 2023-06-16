using CMMSAPIs.BS.Masters;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CMMSAPIs.Repositories.Masters
{
    public class CheckListRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        private ErrorLog m_errorLog;
        public CheckListRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment webHostEnvironment = null) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(webHostEnvironment);
        }

        #region checklist
        internal async Task<List<CMCheckList>> GetCheckList(int facility_id, string type)
        {
            /* Table - CheckList_Number
             * supporting table - AssetCategory - to get Category Name, Frequency - To get Frequency Name
             * Read All properties from above table and return the list
             * Code goes here
            */
            string myQuery = "SELECT "+
                                "checklist_number.id , checklist_number.checklist_number , checklist_number.checklist_type as type, checklist_number.status, checklist_number.created_by as createdById, CONCAT(created_user.firstName, ' ', created_user.lastName) as createdByName, checklist_number.created_at as createdAt, checklist_number.updated_by as updatedById, CONCAT(updated_user.firstName, ' ', updated_user.lastName) as updatedByName, checklist_number.updated_at as updatedAt, checklist_number.asset_category_id as category_id, asset_cat.name as category_name, checklist_number.frequency_id, frequency.name as frequency_name, checklist_number.manpower as manPower, checklist_number.duration, checklist_number.facility_id as facility_id, facilities.name as facility_name " + 
                             "FROM "+
                                "checklist_number "+
                             "LEFT JOIN "+
                                "facilities on facilities.id=checklist_number.facility_id "+
                             "LEFT JOIN "+
                                "assetcategories as asset_cat ON asset_cat.id=checklist_number.asset_category_id "+
                             "LEFT JOIN "+
                                "frequency ON frequency.id=checklist_number.frequency_id "+
                             "LEFT JOIN "+
                                "users as created_user ON created_user.id=checklist_number.created_by " +
                             "LEFT JOIN "+
                                "users as updated_user ON updated_user.id=checklist_number.updated_by ";
            if (facility_id > 0)
            {
                myQuery += $" WHERE checklist_number.facility_id= { facility_id } ";
                if (type != null)
                    myQuery += $" and  checklist_number.checklist_type in ({ type }) ";
                else
                {
                    throw new ArgumentException("Type cannot be empty");
                }
            }
            else
            {
                throw new ArgumentException("Facility ID cannot be empty or zero");
            }
            myQuery += " ORDER BY checklist_number.id DESC ";
            List<CMCheckList> _checkList = await Context.GetData<CMCheckList>(myQuery).ConfigureAwait(false);
            return _checkList;
        }

        internal async Task<CMDefaultResponse> CreateChecklist(List<CMCreateCheckList> request_list, int userID)
        {
            /*
             * Table - CheckList_Number
             * Insert all properties in CMCreateCheckList model to CheckList_Number
             * Code goes here
            */
            List<int> id_list = new List<int>();
            foreach (CMCreateCheckList request in request_list)
            {
                string query = "INSERT INTO checklist_number(checklist_number, checklist_type, facility_id, status ,created_by ," +
                    " created_at , asset_category_id ,frequency_id ,manpower , duration)VALUES" +
                            $"('{request.checklist_number}', {request.type}, {request.facility_id}, 1, {userID}," +
                            $"'{UtilsRepository.GetUTCTime()}',{request.category_id},{request.frequency_id}, {request.manPower},{request.duration}); select LAST_INSERT_ID();";

                DataTable dt = await Context.FetchData(query).ConfigureAwait(false);

                int id = Convert.ToInt32(dt.Rows[0][0]);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKLIST_NUMBER, id, 0, 0, "Check List Created", CMMS.CMMS_Status.CREATED, userID);
                id_list.Add(id);
            }
            CMDefaultResponse response = new CMDefaultResponse(id_list, CMMS.RETRUNSTATUS.SUCCESS, $"{id_list.Count} Checklist(s) Created Successfully");

            return response;
            
        }

        internal async Task<CMDefaultResponse> UpdateCheckList(CMCreateCheckList request, int userID)
        {
            /*
             * Update the changed value in CheckList_Number for requested id
             * Code goes here
            */
            string updateQry = $"UPDATE  softel_cmms.checklist_number SET ";
            if (request.checklist_number != null && request.checklist_number != "")
                updateQry += $" checklist_number = '{request.checklist_number}', ";
            if (request.type > 0)
                updateQry += $" checklist_type = {request.type}, ";
            if (request.facility_id > 0)
                updateQry += $" facility_id = {request.facility_id}, ";
            if (request.status != null)
                updateQry += $" status = {request.status}, ";
            if (request.category_id > 0)
                updateQry += $" asset_category_id = {request.category_id}, ";
            if (request.frequency_id > 0)
                updateQry += $" frequency_id = {request.frequency_id}, ";
            if (request.manPower > 0)
                updateQry += $" manpower = {request.manPower}, ";
            if (request.duration > 0)
                updateQry += $" duration = {request.duration}, ";
            updateQry += $" updated_by = {userID}, updated_at = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id}; ";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKLIST_NUMBER, request.id, 0, 0, "Check List Updated", CMMS.CMMS_Status.UPDATED, userID);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Check List Updated Successfully");

            return response;
        }

        internal async Task<CMDefaultResponse> DeleteChecklist(int id, int userID)
        {
            /* 
             * Set Status to 0 in CheckList_Number table for requested id
             * Code goes here
            */
            string deleteQry = $"DELETE FROM  softel_cmms.checklist_number " +
               $"WHERE  id  = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKLIST_NUMBER, id, 0, 0, "Check List Deleted", CMMS.CMMS_Status.DELETED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Check List Deleted");

            return response;
           
        }
        #endregion

        #region checklistmap
        internal async Task<List<CMCheckListMapList>> GetCheckListMap(int facility_id, int category_id = 0, int? type=null)
        {
            /*
             * Primary Table - CheckList_Mapping
             * Read All properties mention in model and return list
             * Code goes here
            */
            string myQuery = "SELECT " + 
                                "checklist_mapping.category_id, asset_cat.name as category_name, checklist_mapping.status, checklist_mapping.plan_id "+
                             "FROM  " + 
                                "checklist_mapping " + 
                             "JOIN " + 
                                "assetcategories as asset_cat ON checklist_mapping.category_id=asset_cat.id ";
            if (facility_id > 0)
            {
                myQuery += $"WHERE facility_id = {facility_id} ";
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            if (category_id > 0)
            {
                myQuery += $"AND checklist_mapping.category_id = {category_id} ";
            }
            myQuery += "GROUP BY checklist_mapping.category_id;";
            List<CMCheckListMapList> _checkListMapList = await Context.GetData<CMCheckListMapList>(myQuery).ConfigureAwait(false);
            foreach (CMCheckListMapList _checkListMap in _checkListMapList)
            {
                string myQuery2 = "SELECT " + 
                                    "checklist_mapping.id as mapping_id, checklist_mapping.checklist_id, checklist_number.checklist_number as checklist_name, checklist_number.checklist_type as type " + 
                                  "FROM " +
                                    "checklist_mapping " + 
                                  "JOIN " + 
                                    "checklist_number ON checklist_number.id = checklist_mapping.checklist_id " + 
                                  $"WHERE checklist_mapping.facility_id = {facility_id} AND checklist_mapping.category_id = {_checkListMap.category_id} ";
                if (type != null)
                {
                    myQuery2 += $"AND checklist_number.checklist_type = {type}";
                }
                List<CMCheckListIdName> _checkLists = await Context.GetData<CMCheckListIdName>(myQuery2).ConfigureAwait(false);
                _checkListMap.checklists = _checkLists;
            }
            return _checkListMapList;
        }

        internal async Task<List<CMDefaultResponse>> CreateCheckListMap(CMCreateCheckListMap request, int userID)
        {
            /*
             * Primary Table - CheckList_Mapping
             * Insert All properties mention in model
             * Code goes here
            */
            List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();
            foreach (CMCheckListMap checkListMap in request.checklist_map_list)
            {
                foreach (int checklist_id in checkListMap.checklist_ids)
                {
                    CMDefaultResponse response = null;
                    string query1 = $"SELECT id FROM checklist_mapping WHERE facility_id = {request.facility_id} AND category_id = {checkListMap.category_id} AND checklist_id = {checklist_id};";
                    DataTable dt1 = await Context.FetchData(query1).ConfigureAwait(false);
                    string query2 = $"SELECT id FROM checklist_number WHERE facility_id = {request.facility_id} AND asset_category_id = {checkListMap.category_id} AND id = {checklist_id};";
                    DataTable dt2 = await Context.FetchData(query2).ConfigureAwait(false);
                    if (dt1.Rows.Count == 0 && dt2.Rows.Count != 0)
                    {
                        string query3 = "INSERT INTO checklist_mapping (facility_id, category_id, status ,checklist_id, plan_id) VALUES " +
                                    $"({request.facility_id}, {checkListMap.category_id}, {checkListMap.status}, {checklist_id}, {checkListMap.plan_id}); " +
                                    "select LAST_INSERT_ID();";
                        DataTable dt3 = await Context.FetchData(query3).ConfigureAwait(false);
                        int id = Convert.ToInt32(dt3.Rows[0][0]);
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKLIST_MAPPING, id, 0, 0, "Checklist Mapped", CMMS.CMMS_Status.CREATED, userID);
                        response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Checklist mapped successfully");
                    }
                    else if (dt1.Rows.Count != 0)
                    {
                        response = new CMDefaultResponse(checklist_id, CMMS.RETRUNSTATUS.FAILURE, $"Checklist {checklist_id} is already mapped with Asset Category {checkListMap.category_id} for Facility {request.facility_id}");
                    }
                    else if (dt2.Rows.Count == 0)
                    {
                        response = new CMDefaultResponse(checklist_id, CMMS.RETRUNSTATUS.FAILURE, $"Checklist {checklist_id} is not from Facility {request.facility_id} or not associated with Asset Category {checkListMap.category_id}");
                    }
                    responseList.Add(response);
                }
            }
            return responseList;
        }

        internal async Task<CMDefaultResponse> UpdateCheckListMap(CMCreateCheckListMap request)
        {
            /* Primary Table - CheckList_Mapping
             * Update All properties mention in model
             * Code goes here
            */
            /*string updateQry = $"UPDATE  checklist_mapping SET facility_id  = '{request.facility_id}', category_id  = '{request.category_id}',"+
            $"status = '{request.status}', checklist_id = '{request.checklist_ids}', plan_id = '{request.plan_id}'"+
            $" WHERE id = '{request.mapping_id}'; ";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(retVal, CMMS.RETRUNSTATUS.SUCCESS, "");

            return response;*/
            return null;
            
        }
        #endregion

        #region CheckPoint
        internal async Task<List<CMCheckPointList>> GetCheckPointList(int checklist_id)
        {
            /*
             * Primary Table - CheckPoint
             * Supporting table - Checklist_Number - to get checklist name
             * Read All properties mention in CMCheckPointList and return list
             * Code goes here
            */
            string myQuery = "SELECT " +
                                "checkpoint.id as id, check_point, check_list_id as checklist_id, checklist_number.checklist_number as checklist_name, requirement, is_document_required, checkpoint.created_by as created_by_id, CONCAT(created_user.firstName,' ',created_user.lastName) as created_by_name, checkpoint.created_at, checkpoint.updated_by as updated_by_id, CONCAT(updated_user.firstName,' ',updated_user.lastName) as updated_by_name, checkpoint.updated_at, checkpoint.status " +
                             "FROM " + 
                                "checkpoint " + 
                             "LEFT JOIN " + 
                                "checklist_number ON checklist_number.id=checkpoint.check_list_id " + 
                             "LEFT JOIN " + 
                                "users as created_user ON created_user.id=checkpoint.created_by " +
                             "LEFT JOIN " +
                                "users as updated_user ON updated_user.id=checkpoint.updated_by ";
            if (checklist_id > 0)
            {
                myQuery += $" WHERE check_list_id = {checklist_id} ";
            }
            else
            {
                throw new ArgumentException("Invalid checklist_id");
            }
            myQuery += "ORDER BY checkpoint.id DESC";

            List<CMCheckPointList> _checkList = await Context.GetData<CMCheckPointList>(myQuery).ConfigureAwait(false);
            return _checkList;
        }

        internal async Task<CMDefaultResponse> CreateCheckPoint(List<CMCreateCheckPoint> requestList, int userID)
        {
            /*
             * Primary Table - CheckPoint
             * Insert all properties mention in model to CheckPoint table
             * Code goes here
            */
            List<int> idList = new List<int>();
            foreach (CMCreateCheckPoint request in requestList)
            {
                string query = "INSERT INTO  checkpoint (check_point, check_list_id, requirement, is_document_required, " +
                "created_by, created_at, status) VALUES " +
                 $"('{request.check_point}', {request.checklist_id}, '{request.requirement}', {request.is_document_required}," +
                 $"{userID}, '{UtilsRepository.GetUTCTime()}', {request.status}); select LAST_INSERT_ID();";

                DataTable dt = await Context.FetchData(query).ConfigureAwait(false);

                int id = Convert.ToInt32(dt.Rows[0][0]);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKPOINTS, id, 0, 0, "Check Point Created", CMMS.CMMS_Status.CREATED, userID);
                idList.Add(id);
            }
            CMDefaultResponse response = new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, $"{idList.Count} checkpoint(s) created successfully");

            return response;
        }

        internal async Task<CMDefaultResponse> UpdateCheckPoint(CMCreateCheckPoint request, int userID)
        {
            /*
             * Primary Table - CheckPoint
             * Update all properties mention in model to CheckPoint table for requisted id
             * Code goes here
            */
            string updateQry = $"UPDATE checkpoint SET ";
            if (request.check_point != "" && request.check_point != null)
                updateQry += $"check_point = '{request.check_point}', ";
            if (request.checklist_id != 0)
                updateQry += $"check_list_id = {request.checklist_id}, ";
            if (request.requirement != "" && request.requirement != null)
                updateQry += $"requirement = '{request.requirement}', ";
            if (request.is_document_required != null)
                updateQry += $"is_document_required = {request.is_document_required}, ";
            if (request.status != null)
                updateQry += $"status = {request.status}, ";
            updateQry += $"updated_by = {userID}, updated_at='{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKPOINTS, request.id, 0, 0, "Check Point Updated", CMMS.CMMS_Status.UPDATED, userID);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Checkpoint updated successfully");

            return response;
        }

        internal async Task<CMDefaultResponse> DeleteCheckPoint(int id, int userID)
        {
            /*
             * Primary Table - CheckPoint
             * Set status 0 for requested id in CheckPoint table
             * Code goes here
            */
            string updateQry = $"DELETE FROM checkpoint WHERE id = {id};";
            int retVal = await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.CHECKPOINTS, id, 0, 0, "Check Point Deleted", CMMS.CMMS_Status.DELETED, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Checkpoint deleted successfully");
            return response;
        }
        #endregion

        
        private async Task<DataTable> ConvertExcelToChecklists(int file_id)
        {
            Dictionary<string, int> plants = new Dictionary<string, int>();
            string plantQry = "SELECT id, UPPER(name) as name FROM facilities WHERE parentId = 0 GROUP BY name;";
            DataTable dtPlant = await Context.FetchData(plantQry).ConfigureAwait(false);
            plants.Merge(dtPlant.GetColumn<string>("name"), dtPlant.GetColumn<int>("id"));

            Dictionary<string, int> categories = new Dictionary<string, int>();
            string categoryQry = "SELECT id, UPPER(name) as name FROM assetcategories GROUP BY name;";
            DataTable dtCategory = await Context.FetchData(categoryQry).ConfigureAwait(false);
            categories.Merge(dtCategory.GetColumn<string>("name"), dtCategory.GetColumn<int>("id"));

            Dictionary<string, int> frequencies = new Dictionary<string, int>();
            string frequencyQry = "SELECT id, UPPER(name) as name FROM frequency GROUP BY name;";
            DataTable dtFrequency = await Context.FetchData(frequencyQry).ConfigureAwait(false);
            frequencies.Merge(dtFrequency.GetColumn<string>("name"), dtFrequency.GetColumn<int>("id"));

            Dictionary<string, int> checklistTypes = new Dictionary<string, int>()
            {
                { "PM", 1 },
                { "HOTO", 2 },
                { "AUDIT", 3 }
            };

            /*
            Facility_Name	CheckList	Type	Frequency	Category	Man Power	Duration
            */
            Dictionary<string, Tuple<string, Type>> columnNames = new Dictionary<string, Tuple<string, Type>>()
            {
                { "Facility_Name", new Tuple<string, Type>("facility_name", typeof(string)) },
                { "CheckList", new Tuple<string, Type>("checklist_number", typeof(string)) },
                { "Type", new Tuple<string, Type>("type_name", typeof(string)) },
                { "Frequency", new Tuple<string, Type>("frequency_name", typeof(string)) },
                { "Category", new Tuple<string, Type>("category_name", typeof(string)) },
                { "Man Power", new Tuple<string, Type>("manPower", typeof(int)) },
                { "Duration", new Tuple<string, Type>("duration", typeof(int)) },
            };

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
                if (info.Extension != ".xlsx")
                    m_errorLog.SetError("File is not a .xlsx file");
                else
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excel = new ExcelPackage(path);
                    var sheet = excel.Workbook.Worksheets["CheckList"];
                    if (sheet == null)
                        m_errorLog.SetWarning("The file must contain CheckList sheet");
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
                        dt2.Columns.Add("facility_id", typeof(int));
                        dt2.Columns.Add("category_id", typeof(int));
                        dt2.Columns.Add("frequency_id", typeof(int));
                        dt2.Columns.Add("type", typeof(int));
                        //Pending: Reasons for skipping 3 rows
                        //
                        for (int rN = 4; rN <= sheet.Dimension.End.Row; rN++)
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
                                    string status = ex.ToString();
                                    status = status.Substring(0, (status.IndexOf("Exception") + 8));
                                    m_errorLog.SetError("," + status);
                                }
                            }
                            if (newR.IsEmpty())
                            {
                                m_errorLog.SetInformation($"Row {rN} is empty.");
                                continue;
                            }
                            try
                            {
                                newR["facility_id"] = plants[Convert.ToString(newR["facility_name"]).ToUpper()];
                            }
                            catch(KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["facility_name"]) == null || Convert.ToString(newR["facility_name"]) == "")
                                    m_errorLog.SetError($"[Row {rN}] Facility Name cannot be empty.");
                                else
                                    m_errorLog.SetError($"[Row {rN}] Invalid Facility '{newR["facility_name"]}'.");
                            }
                            if (Convert.ToString(newR["checklist_number"]) == null || Convert.ToString(newR["checklist_number"]) == "")
                            {
                                m_errorLog.SetError($"[Row {rN}] Checklist name cannot be empty.");
                            }
                            try
                            {
                                newR["type"] = checklistTypes[Convert.ToString(newR["type_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["type_name"]) == null || Convert.ToString(newR["type_name"]) == "")
                                    m_errorLog.SetError($"[Row {rN}] Checklist Type cannot be empty.");
                                else
                                    m_errorLog.SetError($"[Row {rN}] Invalid Checklist Type.");
                            }
                            try
                            {
                                newR["frequency_id"] = frequencies[Convert.ToString(newR["frequency_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["frequency_name"]) == null || Convert.ToString(newR["frequency_name"]) == "")
                                    m_errorLog.SetError($"[Row {rN}] Frequency Name cannot be empty.");
                                else
                                    m_errorLog.SetError($"[Row {rN}] Invalid Frequency.");
                            }
                            try
                            {
                                newR["category_id"] = categories[Convert.ToString(newR["category_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["category_name"]) == null || Convert.ToString(newR["category_name"]) == "")
                                    m_errorLog.SetError($"[Row {rN}] Asset Category Name cannot be empty.");
                                else
                                    m_errorLog.SetError($"[Row {rN}] Invalid Asset Category.");
                            }
                            if (newR["manPower"] == DBNull.Value)
                                m_errorLog.SetError($"[Row {rN}] Manpower value cannot be empty or 0.");
                            else if (Convert.ToDouble(newR["manPower"]) == 0)
                                m_errorLog.SetError($"[Row {rN}] Manpower value cannot be empty or 0.");
                            if (newR["duration"] == DBNull.Value)
                                m_errorLog.SetError($"[Row {rN}] Duration cannot be empty or 0.");
                            else if (Convert.ToDouble(newR["duration"]) == 0)
                                m_errorLog.SetError($"[Row {rN}] Duration cannot be empty or 0.");
                            dt2.Rows.Add(newR);
                            /*
                             { "Facility_Name", new Tuple<string, Type>("facility_name", typeof(string)) },
                             { "CheckList", new Tuple<string, Type>("checklist_number", typeof(string)) },
                             { "Type", new Tuple<string, Type>("type_name", typeof(string)) },
                             { "Frequency", new Tuple<string, Type>("frequency_name", typeof(string)) },
                             { "Category", new Tuple<string, Type>("category_name", typeof(string)) },
                             { "Man Power", new Tuple<string, Type>("manPower", typeof(int)) },
                             { "Duration", new Tuple<string, Type>("duration", typeof(int)) },
                             dt2.Columns.Add("facility_id", typeof(int));
                             dt2.Columns.Add("category_id", typeof(int));
                             dt2.Columns.Add("frequency_id", typeof(int));
                             dt2.Columns.Add("type", typeof(int));
                             */
                        }
                        return dt2;
                    }
                }
            }
            return null;
        }
        
        internal async Task<CMDefaultResponse> ValidateChecklist(int file_id)
        {
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            DataTable checklists = await ConvertExcelToChecklists(file_id);
            if(checklists != null && m_errorLog.GetErrorCount() == 0)
            {
                m_errorLog.SetImportInformation("Checklist ready to Import");
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
                string qry = $"UPDATE uploadedfiles SET valid = 1 WHERE id = {file_id};";
                await Context.ExecuteNonQry<int>(qry).ConfigureAwait(false);
            }
            return new CMDefaultResponse(file_id, retCode, string.Join("\r\n", m_errorLog.errorLog().ToArray()));
        }

        internal async Task<CMDefaultResponse> ImportChecklist(int file_id, int userID)
        {
            CMDefaultResponse response;
            string qry = $"SELECT valid FROM uploadedfiles WHERE id = {file_id}";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int valid = Convert.ToInt32(dt.Rows[0][0]);
            if(valid == 1)
            {
                DataTable dtChecklists = await ConvertExcelToChecklists(file_id);
                if(dtChecklists != null && m_errorLog.GetErrorCount() == 0)
                {
                    List<CMCreateCheckList> checklists = dtChecklists.MapTo<CMCreateCheckList>();
                    response = await CreateChecklist(checklists, userID);
                }
                else
                {
                    response = new CMDefaultResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, "Error while importing checklists. Please validate the file again.");
                }
            }
            else
            {
                response = new CMDefaultResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, "Please validate the file before importing.");
            }
            return response;
        }
    }
}
