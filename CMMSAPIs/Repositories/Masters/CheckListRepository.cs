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
        private List<string> checklistNames;
        public CheckListRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment webHostEnvironment = null) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(webHostEnvironment);
        }

        #region checklist
        internal async Task<List<CMCheckList>> GetCheckList(int facility_id, int type, int frequency_id, int category_id)
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
                                "assetcategories as asset_cat ON asset_cat.id = checklist_number.asset_category_id "+
                             "LEFT JOIN "+
                                "frequency ON frequency.id=checklist_number.frequency_id "+
                             "LEFT JOIN "+
                                "users as created_user ON created_user.id = checklist_number.created_by " +
                             "LEFT JOIN "+
                                "users as updated_user ON updated_user.id = checklist_number.updated_by where 1 ";
            if (facility_id > 0)
            {
                myQuery += $" and ( checklist_number.facility_id = { facility_id } or checklist_number.facility_id = 0)";
                if (type > 0)
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
            if (frequency_id > 0)
                myQuery += $" and  checklist_number.frequency_id = {frequency_id} ";

            if (category_id > 0)
                myQuery += $" and  checklist_number.asset_category_id = {category_id} ";

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
            string updateQry = $"UPDATE  checklist_number SET ";
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
            string deleteQry = $"DELETE FROM  checklist_number " +
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
        internal async Task<List<CMCheckPointList>> GetCheckPointList(int checklist_id, int facility_id)
        {
            /*
             * Primary Table - CheckPoint
             * Supporting table - Checklist_Number - to get checklist name
             * Read All properties mention in CMCheckPointList and return list
             * Code goes here
            */
            string myQuery = "SELECT " +
                                "checkpoint.id as id, check_point, check_list_id as checklist_id, checklist_number.checklist_number as checklist_name, requirement, is_document_required, action_to_be_done, checkpoint.created_by as created_by_id, CONCAT(created_user.firstName,' ',created_user.lastName) as created_by_name, checkpoint.created_at, checkpoint.updated_by as updated_by_id, CONCAT(updated_user.firstName,' ',updated_user.lastName) as updated_by_name, checkpoint.updated_at, checkpoint.status ,checkpoint.failure_weightage,checkpoint.type as type,CASE WHEN checkpoint.type = 1 then 'Bool'  WHEN checkpoint.type = 0 then 'Text' WHEN checkpoint.type = 2 then 'Range' ELSE 'Unknown Type' END as checkpoint_type , checkpoint.min_range as min ,checkpoint.max_range as max " +
                             "FROM " + 
                                "checkpoint " + 
                             "LEFT JOIN " + 
                                "checklist_number ON checklist_number.id=checkpoint.check_list_id " + 
                             "LEFT JOIN " + 
                                "users as created_user ON created_user.id=checkpoint.created_by " +
                             "LEFT JOIN " +
                                "users as updated_user ON updated_user.id=checkpoint.updated_by ";

            if (checklist_id > 0 && facility_id ==0)
            {
                myQuery += $" WHERE check_list_id = {checklist_id} ";
            }
            else if (facility_id > 0 && checklist_id == 0)
            {
               // checklist_number.facility_id in(1798, 0)
                myQuery += $" WHERE checklist_number.facility_id in ({facility_id},0) ";
            }
            else if(checklist_id > 0 && facility_id > 0)
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
                                "action_to_be_done,failure_weightage,type,min_range,max_range ,created_by, created_at, status) VALUES " +
                                $"('{request.check_point.Replace("'", "")}', {request.checklist_id}, '{request.requirement.Replace("'", "")}', " +
                                $"{(request.is_document_required==null?0 : request.is_document_required)}, '{request.action_to_be_done}', '{request.failure_weightage}', '{request.checkpoint_type.id}', '{request.checkpoint_type.min}','{request.checkpoint_type.max}'," +
                                $"{userID}, '{UtilsRepository.GetUTCTime()}', 1); select LAST_INSERT_ID();";

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
            if (request.action_to_be_done != "" && request.action_to_be_done != null)
                updateQry += $"action_to_be_done = '{request.action_to_be_done}', ";
            if (request.status != null)
                updateQry += $"status = {request.status}, ";
            if (request.checkpoint_type != null)
                updateQry += $"type = '{request.checkpoint_type.id}', min_range = '{request.checkpoint_type.min}',max_range = '{request.checkpoint_type.max}', ";
            if (request.failure_weightage != 0)
                updateQry += $"failure_weightage = {request.failure_weightage}, ";
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

            string checklistQry = "SELECT UPPER(checklist_number) as name FROM checklist_number WHERE checklist_number is not null and checklist_number != '' GROUP BY checklist_number;";
            DataTable dtChecklist = await Context.FetchData(checklistQry).ConfigureAwait(false);
            checklistNames = dtChecklist.GetColumn<string>("name");

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
                m_errorLog.SetError($"[Checklist] Directory '{dir}' cannot be found");
            else if (!File.Exists(path))
                m_errorLog.SetError($"[Checklist] File '{filename}' cannot be found in directory '{dir}'");
            else
            {
                FileInfo info = new FileInfo(path);
                if (info.Extension != ".xlsx")
                    m_errorLog.SetError("[Checklist] File is not a .xlsx file");
                else
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excel = new ExcelPackage(path);
                    var sheet = excel.Workbook.Worksheets["CheckList"];
                    if (sheet == null)
                        m_errorLog.SetWarning("[Checklist] The file must contain CheckList sheet");
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
                                    string status = ex.ToString();
                                    status = status.Substring(0, (status.IndexOf("Exception") + 8));
                                    m_errorLog.SetError("," + status);
                                }
                            }
                            if (newR.IsEmpty())
                            {
                                m_errorLog.SetInformation($"[Checklist] Row {rN} is empty.");
                                continue;
                            }
                            try
                            {
                                newR["facility_id"] = plants[Convert.ToString(newR["facility_name"]).ToUpper()];
                            }
                            catch(KeyNotFoundException)
                            {
                                //if (Convert.ToString(newR["facility_name"]) == null || Convert.ToString(newR["facility_name"]) == "")
                                //    m_errorLog.SetError($"[Checklist: Row {rN}] Facility Name cannot be empty.");
                                //else
                                //    m_errorLog.SetError($"[Checklist: Row {rN}] Invalid Facility '{newR["facility_name"]}'.");
                                newR["facility_id"] = 0;
                            }
                            if (Convert.ToString(newR["checklist_number"]) == null || Convert.ToString(newR["checklist_number"]) == "")
                            {
                                m_errorLog.SetError($"[Checklist: Row {rN}] Checklist name cannot be empty.");
                            }
                            else if (checklistNames.Contains(Convert.ToString(newR["checklist_number"]).ToUpper()))
                            {
                                string checklist_Validation_Q = "select ifnull(f.name,'') as name from checklist_number left join facilities f on f.id = checklist_number.facility_id where checklist_number='" + Convert.ToString(newR["checklist_number"]) + "';";
                                DataTable dt = await Context.FetchData(checklist_Validation_Q).ConfigureAwait(false);
                                string facility_name = Convert.ToString(dt.Rows[0][0]);
                                m_errorLog.SetError($"[Checklist: Row {rN}] Checklist name : {Convert.ToString(newR["checklist_number"])} already present in plant {facility_name}.");
                            }
                            else
                            {
                                checklistNames.Add(Convert.ToString(newR["checklist_number"]).ToUpper());
                            }
                            try
                            {
                                newR["type"] = checklistTypes[Convert.ToString(newR["type_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["type_name"]) == null || Convert.ToString(newR["type_name"]) == "")
                                    m_errorLog.SetError($"[Checklist: Row {rN}] Checklist Type cannot be empty.");
                                else
                                    m_errorLog.SetError($"[Checklist: Row {rN}] Invalid Checklist Type.");
                            }
                            try
                            {
                                newR["frequency_id"] = frequencies[Convert.ToString(newR["frequency_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["frequency_name"]) == null || Convert.ToString(newR["frequency_name"]) == "")
                                    m_errorLog.SetError($"[Checklist: Row {rN}] Frequency Name cannot be empty.");
                                else
                                    m_errorLog.SetError($"[Checklist: Row {rN}] Invalid Frequency.");
                            }
                            try
                            {
                                newR["category_id"] = categories[Convert.ToString(newR["category_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["category_name"]) == null || Convert.ToString(newR["category_name"]) == "")
                                    m_errorLog.SetError($"[Checklist: Row {rN}] Asset Category Name cannot be empty.");
                                else
                                    m_errorLog.SetError($"[Checklist: Row {rN}] Invalid Asset Category.");
                            }
                            if (newR["manPower"] == DBNull.Value)
                                m_errorLog.SetError($"[Checklist: Row {rN}] Manpower value cannot be empty or 0.");
                            else if (Convert.ToDouble(newR["manPower"]) == 0)
                                m_errorLog.SetError($"[Checklist: Row {rN}] Manpower value cannot be empty or 0.");
                            if (newR["duration"] == DBNull.Value)
                                m_errorLog.SetError($"[Checklist: Row {rN}] Duration cannot be empty or 0.");
                            else if (Convert.ToDouble(newR["duration"]) == 0)
                                m_errorLog.SetError($"[Checklist: Row {rN}] Duration cannot be empty or 0.");
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

        private async Task<DataTable> ConvertExcelToCheckpoints(int file_id)
        {
            Dictionary<string, int> checklists = new Dictionary<string, int>();
            string checklistQry = "SELECT id, UPPER(checklist_number) as name FROM checklist_number WHERE checklist_number is not null and checklist_number != '' GROUP BY checklist_number;";
            DataTable dtChecklist = await Context.FetchData(checklistQry).ConfigureAwait(false);
            checklists.Merge(dtChecklist.GetColumn<string>("name"), dtChecklist.GetColumn<int>("id"));
            foreach(string checklistName in checklistNames)
            {
                if(!checklists.ContainsKey(checklistName))
                    checklists.Add(checklistName, 0);
            }
            List<string> yesNo = new List<string>() { "NO", "YES" };

            Dictionary<string, int> checkpoint_types = new Dictionary<string, int>()
            {
                { "text", 0 },
                { "bool", 1 },
                { "range", 2 }
            };


            Dictionary<string, Tuple<string, Type>> columnNames = new Dictionary<string, Tuple<string, Type>>()
            {
                { "Checkpoint Name", new Tuple<string, Type>("check_point", typeof(string)) },
                { "Checklist Name", new Tuple<string, Type>("checklist_name", typeof(string)) },
                { "Requirement", new Tuple<string, Type>("requirement", typeof(string)) },
                { "Is Image Required", new Tuple<string, Type>("is_document_required", typeof(string)) },
                { "Action to be taken", new Tuple<string, Type>("action_to_be_done", typeof(string)) },
                { "Failure Weightage", new Tuple<string, Type>("failure_weightage", typeof(int)) },
                { "Checkpoint Type", new Tuple<string, Type>("checkpoint_type", typeof(string)) },
               
                { "Range Min", new Tuple<string, Type>("range_min", typeof(string)) },
                { "Range Max", new Tuple<string, Type>("range_max", typeof(string)) }
            };
            string query1 = $"SELECT file_path FROM uploadedfiles WHERE id = {file_id};";
            DataTable dt1 = await Context.FetchData(query1).ConfigureAwait(false);
            string path = Convert.ToString(dt1.Rows[0][0]);
            string dir = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);
            if (!Directory.Exists(dir))
                m_errorLog.SetError($"[Checkpoint] Directory '{dir}' cannot be found");
            else if (!File.Exists(path))
                m_errorLog.SetError($"[Checkpoint] File '{filename}' cannot be found in directory '{dir}'");
            else
            {
                FileInfo info = new FileInfo(path);
                if (info.Extension != ".xlsx")
                    m_errorLog.SetError("[Checkpoint] File is not a .xlsx file");
                else
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excel = new ExcelPackage(path);
                    var sheet = excel.Workbook.Worksheets["CheckPoint"];
                    if (sheet == null)
                        m_errorLog.SetWarning("[Checkpoint] The file must contain CheckPoint sheet");
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
                        dt2.Columns.Add("checklist_id", typeof(int));
                        dt2.Columns.Add("checkpoint_type_id", typeof(int));
                        //Pending: Reasons for skipping 3 rows
                        //
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
                                    string status = ex.ToString();
                                    status = status.Substring(0, (status.IndexOf("Exception") + 8));
                                    m_errorLog.SetError("," + status);
                                }
                            }
                            if (newR.IsEmpty())
                            {
                                m_errorLog.SetInformation($"[Checkpoint] Row {rN} is empty.");
                                continue;
                            }
                            if(Convert.ToString(newR["check_point"]) == null || Convert.ToString(newR["check_point"]) == "")
                            {
                                m_errorLog.SetError($"[Checkpoint: Row {rN}] Checkpoint name cannot be empty.");
                            }
                            try
                            {
                                newR["checklist_id"] = checklists[Convert.ToString(newR["checklist_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["checklist_name"]) == null || Convert.ToString(newR["checklist_name"]) == "")
                                    m_errorLog.SetError($"[Checkpoint: Row {rN}] Checklist Name cannot be empty.");
                                else
                                    m_errorLog.SetError($"[Checkpoint: Row {rN}] Checklist '{Convert.ToString(newR["checklist_name"])}' not found.");
                            }
                            if (Convert.ToString(newR["requirement"]) == null || Convert.ToString(newR["requirement"]) == "")
                            {
                                m_errorLog.SetError($"[Checkpoint: Row {rN}] Requirement cannot be empty.");
                            }
                            string yn = Convert.ToString(newR["is_document_required"]);
                            int yesNoIndex = yesNo.IndexOf(yn.ToUpper());
                            if (yesNoIndex == -1)
                            {
                                if(yn == "" || yn == null)
                                {
                                    yesNoIndex = 0;
                                    m_errorLog.SetInformation($"[Checkpoint: Row {rN}] Is Document Required set to False by default");
                                }    
                                else
                                {
                                    m_errorLog.SetError($"[Checkpoint: Row {rN}] Invalid answer '{yn}'");
                                }
                            }
                            newR["is_document_required"] = $"{yesNoIndex}";
                            try
                            {
                                newR["checkpoint_type_id"] = checkpoint_types[Convert.ToString(newR["checkpoint_type"]).ToLower()];
                            }
                            catch (KeyNotFoundException)
                            {
                                m_errorLog.SetError($"[Checkpoint: Row {rN}] Invalid checkpoint type.");
                            }
                           
                               
                          if(Convert.ToInt32(newR["failure_weightage"]) > 100)
                            {
                                m_errorLog.SetError($"[Checkpoint: Row {rN}] failure weightage cannot be greater than 100.");
                            }
                            else
                            {
                                newR["failure_weightage"] = newR["failure_weightage"].ToInt();
                            }

                            if (Convert.ToString(newR["range_min"]) == null || Convert.ToString(newR["range_min"]) == "")                               
                                newR["range_min"] = 0;
                            else
                                newR["range_min"] = newR["range_min"].ToInt();

                            if (Convert.ToString(newR["range_max"]) == null || Convert.ToString(newR["range_max"]) == "")
                                 newR["range_max"] = 0;
                            else
                                newR["range_min"] = newR["range_min"].ToInt();

                            dt2.Rows.Add(newR);
                            /*
                            Dictionary<string, Tuple<string, Type>> columnNames = new Dictionary<string, Tuple<string, Type>>()
                            {
                                { "CheckPoint Name", new Tuple<string, Type>("check_point", typeof(string)) },
                                { "Checklist_Name", new Tuple<string, Type>("checklist_name", typeof(string)) },
                                { "Requirement", new Tuple<string, Type>("requirement", typeof(string)) },
                                { "Is document required", new Tuple<string, Type>("is_document_required", typeof(string)) },
                                { "Action to be taken", new Tuple<string, Type>("action_to_be_done", typeof(string)) }
                            };
                            */
                        }
                        dt2.ConvertColumnType("is_document_required", typeof(int));
                        return dt2;
                    }
                }
            }
            return null;
        }

        internal async Task<CMImportFileResponse> ValidateChecklistCheckpoint(int file_id)
        {
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            DataTable checklists = await ConvertExcelToChecklists(file_id);
            DataTable checkpoints = await ConvertExcelToCheckpoints(file_id);
            string message;
            if (checklists != null && checkpoints != null && m_errorLog.GetErrorCount() == 0)
            {
                m_errorLog.SetImportInformation("File ready to Import");
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
                string qry = $"UPDATE uploadedfiles SET valid = 1 WHERE id = {file_id};";
                await Context.ExecuteNonQry<int>(qry).ConfigureAwait(false);
                message = "No errors found during validation";
            }
            else
            {
                string qry = $"UPDATE uploadedfiles SET valid = 2 WHERE id = {file_id};";
                await Context.ExecuteNonQry<int>(qry).ConfigureAwait(false);
                message = "Errors found during validation";
            }
            string logPath = m_errorLog.SaveAsText($"ImportLog\\ImportChecklist_File{file_id}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}");
            string logQry = $"UPDATE uploadedfiles SET logfile = '{logPath}' WHERE id = {file_id}";
            await Context.ExecuteNonQry<int>(logQry).ConfigureAwait(false);
            return new CMImportFileResponse(file_id, retCode, logPath, m_errorLog.errorLog(), message);
        }

        internal async Task<CMImportFileResponse> ImportChecklist(int file_id, int userID)
        {
            CMImportFileResponse response;
            string qry = $"SELECT valid, logfile FROM uploadedfiles WHERE id = {file_id}";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int valid = Convert.ToInt32(dt.Rows[0]["valid"]);
            string logfile = Convert.ToString(dt.Rows[0]["logfile"]);
            IEnumerable<string> log;
            try
            {
                log = File.ReadAllLines(logfile);
            }
            catch
            {
                log = null;
                logfile = null;
            }
            if (valid == 1)
            {
                DataTable dtChecklists = await ConvertExcelToChecklists(file_id);
                if (dtChecklists != null && m_errorLog.GetErrorCount() == 0)
                {
                    List<CMCreateCheckList> checklists = dtChecklists.MapTo<CMCreateCheckList>();
                  
                    CMDefaultResponse response1 = await CreateChecklist(checklists, userID);
                    response = new CMImportFileResponse(response1.id, response1.return_status, logfile, log, response1.message);
                }
                else
                {
                    CMImportFileResponse response2 = new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, logfile, log, "Error while importing checklists. Please validate the file again.");
                    response = response2;
                }
            }
            else if (valid == 2)
            {
                CMImportFileResponse response3 = new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, logfile, log, "Cannot import checklists as file has some errors. Please correct the file and re-validate it");
                response = response3;
            }
            else
            {
                await ValidateChecklistCheckpoint(file_id);
                CMImportFileResponse response4 = await ImportChecklist(file_id, userID);
                response = response4;
            }
            return response;
        }

        internal async Task<CMImportFileResponse> ImportCheckpoint(int file_id, int userID)
        {
            CMImportFileResponse response;
            string qry = $"SELECT valid, logfile FROM uploadedfiles WHERE id = {file_id}";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int valid = Convert.ToInt32(dt.Rows[0]["valid"]);
            string logfile = Convert.ToString(dt.Rows[0]["logfile"]);
            IEnumerable<string> log;
            try
            {
                log = File.ReadAllLines(logfile);
            }
            catch
            {
                log = null;
                logfile = null;
            }
            if (valid == 1)
            {
                DataTable dtCheckpoints = await ConvertExcelToCheckpoints(file_id);
                if (dtCheckpoints != null && m_errorLog.GetErrorCount() == 0)
                {
                    List<CMCreateCheckPoint> checkpoints = new List<CMCreateCheckPoint>();
                    foreach (DataRow row in dtCheckpoints.Rows)
                    {
                        CMCPType checkpoint_type = new CMCPType();
                        checkpoint_type.id = Convert.ToInt32(row["checkpoint_type_id"]);
                        checkpoint_type.min = Convert.ToInt32(row["range_min"]);
                        checkpoint_type.max = Convert.ToInt32(row["range_max"]);
                        CMCreateCheckPoint checkpoint = new CMCreateCheckPoint
                        {
                          
                            check_point = Convert.ToString(row["check_point"]),
                            checklist_id = Convert.ToInt32(row["checklist_id"]),
                            requirement = Convert.ToString(row["requirement"]),
                            is_document_required = row["is_document_required"] == DBNull.Value
                                ? (int?)null
                                : Convert.ToInt32(row["is_document_required"]),
                            action_to_be_done = null,
                            failure_weightage = row["failure_weightage"] == DBNull.Value
                                ? (int?)null
                                : Convert.ToInt32(row["failure_weightage"]),                           
                            checkpoint_type = checkpoint_type
                        };

                        checkpoints.Add(checkpoint);
                    }


   
                 
                    CMDefaultResponse response1 = await CreateCheckPoint(checkpoints, userID);
                    response = new CMImportFileResponse(response1.id, response1.return_status, logfile, log, response1.message);
                }
                else
                {
                    CMImportFileResponse response2 = new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, logfile, log, "Error while importing checkpoints. Please validate the file again.");
                    response = response2;
                }
            }
            else if (valid == 2)
            {
                CMImportFileResponse response3 = new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, logfile, log, "Cannot import checkpoints as file has some errors. Please correct the file and re-validate it");
                response = response3;
            }
            else
            {
                await ValidateChecklistCheckpoint(file_id);
                CMImportFileResponse response4 = await ImportCheckpoint(file_id, userID);
                response = response4;
            }
            return response;
        }
        internal async Task<List<CMImportFileResponse>> ImportChecklistCheckpoint(int file_id, int userID)
        {
            List<CMImportFileResponse> responseList = new List<CMImportFileResponse>();
            CMImportFileResponse checklistResponse = await ImportChecklist(file_id, userID);
            responseList.Add(checklistResponse);
            CMImportFileResponse checkpointResponse = await ImportCheckpoint(file_id, userID);
            responseList.Add(checkpointResponse);
            return responseList;
        }
    }
}
