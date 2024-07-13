using CMMSAPIs.Helper;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.SM
{
    public class MRSRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public MRSRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }

        internal async Task<List<CMMRSList>> getMRSList(int facility_ID, int emp_id, DateTime toDate, DateTime fromDate, int status, string facilitytimeZone)

        {

            string filter = " WHERE sm.facility_ID = " + facility_ID + "  AND  (DATE_FORMAT(sm.lastmodifieddate,'%Y-%m-%d') BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "' OR DATE_FORMAT(sm.returnDate,'%Y-%m-%d') BETWEEN '" + fromDate.ToString("yyyy-MM-dd") + "' AND '" + toDate.ToString("yyyy-MM-dd") + "')";



            string stmt = "SELECT is_mrs_return, sm.ID,sm.requested_by_emp_ID,CONCAT(ed1.firstName,' ',ed1.lastName) as approver_name,DATE_FORMAT(sm.requested_date,'%Y-%m-%d %H:%i') as requestd_date," +
                "DATE_FORMAT(sm.returnDate,'%Y-%m-%d %H:%i') as returnDate,if(sm.approval_status != '',DATE_FORMAT(sm.approved_date,'%Y-%m-%d %H:%i'),'') as approval_date,sm.approval_status," +
                "sm.approval_comment,CONCAT(ed.firstName,' ',ed.lastName) as requested_by_name, sm.status, sm.activity, sm.whereUsedType, " +
    " case when sm.whereUsedType = 1 then 'Job' when sm.whereUsedType = 2 then 'PM' when sm.whereUsedType = 4 then 'JOBCARD' when sm.whereUsedType = 27 then 'PMTASK' else 'Invalid' end as whereUsedTypeName, sm.whereUsedRefID, sm.remarks " +
                "FROM smmrs sm LEFT JOIN users ed ON ed.id = sm.requested_by_emp_ID LEFT JOIN users ed1 ON ed1.id = sm.approved_by_emp_ID " +
                "" + filter + "";
            List<CMMRSList> _List = await Context.GetData<CMMRSList>(stmt).ConfigureAwait(false);


            for (var i = 0; i < _List.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_MRS, _Status);
                string _status_long = getLongStatus(_Status, _List[i].ID);
                _List[i].status_short = _shortStatus;
                _List[i].status_long = _status_long;
                if (_List[i].is_mrs_return == 0)
                {
                    _List[i].CMMRSItems = await getMRSItems(_List[i].ID, facilitytimeZone);
                }
                else
                {
                    _List[i].CMMRSItems = await getMRSItemsReturn(_List[i].ID, facilitytimeZone);
                }

            }
            foreach (var list in _List)
            {


                /* if (list != null && list.requestd_date != null && list.requestd_date!="")
                 {
                     DateTime requestd_date = Convert.ToDateTime(list.requestd_date);
                     requestd_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, requestd_date);
                     list.requestd_date = requestd_date.ToString();
                 }*/
                if (list != null && list.returnDate != null && list.returnDate != "")
                {
                    DateTime returnDate = DateTime.Parse(list.returnDate);
                    returnDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, returnDate);
                    list.returnDate = returnDate.ToString();
                }
                if (list != null && list.issued_date != null && list.issued_date != "")
                {
                    DateTime issued_date = DateTime.Parse(list.issued_date);
                    issued_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, issued_date);
                    list.issued_date = issued_date.ToString();
                }
            }

            return _List;
        }

        private DateTime TimeSpanConverter(string approval_date)
        {
            throw new NotImplementedException();
        }

        internal async Task<List<CMMRSListByModule>> getMRSListByModule(int jobId, int pmId, string facilitytimeZone)
        {
            int Id = 0;
            string filter = "";
            List<CMMRSListByModule> _List = null;
            if (jobId > 0)
            {
                Id = jobId;
                filter = " and whereUsedType = 4 ";
                string stmt = $"SELECT \r\n    sm.ID AS mrsId,\r\n  is_mrs_return,  jc.jobId AS jobId,\r\n    sm.whereUsedRefID AS jobCardId,\r\n    sm.status AS status,\r\n    GROUP_CONCAT(DISTINCT items.name\r\n        ORDER BY items.id\r\n        SEPARATOR ', ') AS mrsItems\r\nFROM\r\n    smmrs sm\r\n        LEFT JOIN\r\n    (SELECT \r\n        sai.ID AS id, si.mrs_ID, sam.asset_name as name\r\n    FROM smrsitems si left join\r\n        smassetitems sai on si.asset_item_ID = sai.ID\r\n    LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code) AS items ON items.mrs_ID = sm.ID\r\n        LEFT JOIN\r\n    jobcards jc ON jc.id = sm.whereUsedRefID\r\nWHERE\r\n    jc.jobId = {Id} {filter} group by mrsId;";

                _List = await Context.GetData<CMMRSListByModule>(stmt).ConfigureAwait(false);
            }
            if (pmId > 0)
            {
                //string pmQry = $"SELECT id as pmId , linked_job_id as jobId from pm_execution where task_id ={pmId} ";
                //List<CMMRSListByModule> _pm = await Context.GetData<CMMRSListByModule>(pmQry).ConfigureAwait(false);
                //Id = _pm[0].pmId;
                Id = pmId;
                filter = " and whereUsedType = 10 ";
                string stmt = $"    select m.ID AS mrsId,\r\n    m.whereUsedType AS whereUsedType,\r\n is_mrs_return,   m.whereUsedRefID AS jobCardId,\r\n    m.status AS status from smmrs m\r\n     inner join pm_task t on t.id = m.whereUsedRefID\r\n     where m.whereUsedType = 27 and m.whereUsedRefID  = {pmId} order by m.id desc;";

                _List = await Context.GetData<CMMRSListByModule>(stmt).ConfigureAwait(false);
            }



            for (var i = 0; i < _List.Count; i++)
            {
                string assetItemNames = "";
                _List[i].pmId = pmId;
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_MRS, _Status);
                _List[i].status_short = _shortStatus;
                if (_List[i].is_mrs_return == 0)
                {
                    // filling original MRS Items list
                    _List[i].CMMRSItems = await getMRSItems(_List[i].mrsId, facilitytimeZone);
                }
                else
                {
                    // filling MRS Return Items list
                    _List[i].CMMRSItems = await getMRSItemsReturn(_List[i].mrsId, facilitytimeZone);
                    if (_List[i].CMMRSItems.Count > 0)
                    {
                        _List[i].mrsId = _List[i].CMMRSItems[0].original_mrs_ID;
                    }

                }

                if (_List[i].CMMRSItems.Count > 0)
                {
                    for (var k = 0; k < _List[i].CMMRSItems.Count; k++)
                    {
                        _List[i].CMMRSItems[k].available_qty = _List[i].CMMRSItems[k].available_qty - _List[i].CMMRSItems[k].issued_qty;
                        if (_List[i].CMMRSItems[k].available_qty < 0)
                        {
                            _List[i].CMMRSItems[k].available_qty = 0;
                        }
                    }
                }
                for (var j = 0; j < _List[i].CMMRSItems.Count; j++)
                {
                    assetItemNames = assetItemNames + _List[i].CMMRSItems[j].asset_name + ", ";
                }
                if (assetItemNames.Count() > 0)
                {
                    assetItemNames = assetItemNames.Substring(0, assetItemNames.Length - 2);
                }
                _List[i].mrsItems = assetItemNames;
                _List[i].mrs_return_ID = _List[i].CMMRSItems[0].mrs_return_ID;

            }

            return _List;
        }
        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.MRS_SUBMITTED:
                    retValue = "Submitted";
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_REJECTED:
                    retValue = "Request Rejected";
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_APPROVED:
                    retValue = "Request Approved";
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED:
                    retValue = "Request Issued";
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED_REJECTED:
                    retValue = "Request Issued Rejected";
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED_APPROVED:
                    retValue = "Request Issued Approved";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;
        }

        internal async Task<CMDefaultResponse> CreateMRS(CMMRS request, int UserID)
        {
            /* isEditMode =0 for creating new MRS*/

            // added validation for issue_qty > 0 and issue_qty < requested_qty for each item

            bool isApproval_required = false;

            for (var j = 0; j < request.cmmrsItems.Count; j++)
            {
                if (request.cmmrsItems[j].issued_qty < 0)
                {
                    return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Issued quantity should be less than 0 for asset item " + request.cmmrsItems[j].asset_item_ID + ".");
                }
            }

            request.isEditMode = 0;
            request.requested_by_emp_ID = UserID;
            bool Queryflag = false;
            CMDefaultResponse response = null;
            string flag = "MRS_REQUEST";
            int setAsTemplate = 0;
            if (request.setTemplateflag != null)
            {
                setAsTemplate = 1;
            }
            if (request.isEditMode == 1)
            {
                // update CMMRS

                var lastMRSID = request.ID;
                var refType = "MRSEdit";
                var mailSub = "CMMRS Request Updated";
                string updatestmt = $" START TRANSACTION; UPDATE smmrs SET facility_ID = {request.facility_ID}, requested_by_emp_ID = {request.requested_by_emp_ID}, requested_date = {DateTime.Now.ToString("yyyy-MM-dd  HH:mm")}," +
                    $" setAsTemplate = '{request.setAsTemplate}', templateName = {request.templateName}, approval_status = {request.approval_status}, activity='{request.activity}',whereUsedType={request.whereUsedType},whereUsedRefID={request.whereUsedRefID}, remarks = '{request.remarks}' WHERE ID = {request.ID}" +
                    $"DELETE FROM smrsitems WHERE mrs_ID =  {lastMRSID} ; COMMIT;";
                await Context.ExecuteNonQry<int>(updatestmt);

            }
            else
            {
                var refType = "MRS";
                var mailSub = "MRS Request";
                string insertStmt = $"START TRANSACTION; INSERT INTO smmrs (facility_ID,requested_by_emp_ID,requested_date," +
                    $"status,flag,setAsTemplate,templateName, approved_by_emp_ID, approved_date,activity,whereUsedType,whereUsedRefID,remarks," +
                    $"from_actor_type_id,from_actor_id,to_actor_type_id,to_actor_id)" +
                    $" VALUES ({request.facility_ID},{request.requested_by_emp_ID},'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                    $",{(int)CMMS.CMMS_Status.MRS_SUBMITTED},{1},'{request.setAsTemplate}','{request.templateName}',0,'{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}','{request.activity}',{request.whereUsedType},{request.whereUsedRefID}, '{request.remarks}'," +
                    $" {request.from_actor_type_id}, {request.from_actor_id},{request.to_actor_type_id}, {request.to_actor_id}); SELECT LAST_INSERT_ID(); COMMIT;";
                DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                request.ID = Convert.ToInt32(dt2.Rows[0][0]);
            }
            int is_splited = 0;
            if (request.ID != null)
            {
                for (var i = 0; i < request.cmmrsItems.Count; i++)
                {
                    int available_qty = await GetAvailableQty(request.cmmrsItems[i].asset_item_ID, request.facility_ID);

                    request.cmmrsItems[i].available_qty = available_qty; //- request.cmmrsItems[i].requested_qty;
                    int equipmentID = request.cmmrsItems[i].asset_item_ID;
                    decimal quantity = request.cmmrsItems[i].qty;

                    string selectQuery = "SELECT approval_required, asset_code, asset_type_ID FROM  " +
                               " smassetmasters  " +
                               "WHERE ID = " + equipmentID;

                    List<CMSMMaster> assetList = await Context.GetData<CMSMMaster>(selectQuery).ConfigureAwait(false);
                    var approval_required = assetList[0].approval_required;
                    var asset_type_ID = assetList[0].asset_type_ID;
                    var asset_code = assetList[0].asset_code;
                    var IsSpareSelectionEnable = await getMultiSpareSelectionStatus(asset_code, asset_type_ID);
                    if (approval_required == 1)
                    {
                        isApproval_required = true;
                    }

                    if (asset_type_ID == (int)CMMS.SM_AssetTypes.Spare && request.cmmrsItems[i].requested_qty > 0)
                    {
                        is_splited = 0;
                    }
                    else
                    {
                        is_splited = 1;
                    }
                    if (Convert.ToInt32(IsSpareSelectionEnable) == 0 || asset_type_ID != 0)
                    {
                        try
                        {
                            string insertStmt = $"START TRANSACTION; " +
                            $"INSERT INTO smrsitems (mrs_ID,asset_item_ID,asset_MDM_code,requested_qty,status,flag,approval_required,mrs_return_ID,issued_qty,returned_qty,used_qty,available_qty,is_splited,serial_number)" +
                            $"VALUES ({request.ID},{request.cmmrsItems[i].asset_item_ID},'{asset_code}',{request.cmmrsItems[i].requested_qty},0,0,{approval_required},0,{request.cmmrsItems[i].issued_qty}, {request.cmmrsItems[i].returned_qty}, {request.cmmrsItems[i].used_qty}, {request.cmmrsItems[i].available_qty},1,'{request.cmmrsItems[i].serial_number}')" +
                            $"; SELECT LAST_INSERT_ID();COMMIT;";
                            DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        { throw ex; }
                    }
                    else
                    {
                        try
                        {
                            string insertStmt = $"START TRANSACTION; " +
                            $"INSERT INTO smrsitems (mrs_ID,asset_item_ID,asset_MDM_code,requested_qty,status,flag,approval_required,mrs_return_ID,issued_qty,returned_qty,used_qty,available_qty,is_splited, serial_number)" +
                            $"VALUES ({request.ID},{request.cmmrsItems[i].asset_item_ID},'{asset_code}',1,0,0,{approval_required},0,{request.cmmrsItems[i].issued_qty}," +
                            $" {request.cmmrsItems[i].returned_qty}, {request.cmmrsItems[i].used_qty}, {request.cmmrsItems[i].available_qty}, 1,'{request.cmmrsItems[i].serial_number}')" +
                            $"; SELECT LAST_INSERT_ID();COMMIT;";
                            DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        { throw ex; }
                    }
                    if (Convert.ToInt32(IsSpareSelectionEnable) == 1 && asset_type_ID != 2)
                    {
                        UpdateAssetStatus(request.cmmrsItems[i].asset_item_ID, 2);
                    }
                    Queryflag = true;
                }
            }
            if (!Queryflag)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to submit MRS.");
            }
            else
            {
                response = new CMDefaultResponse(request.ID, CMMS.RETRUNSTATUS.SUCCESS, "Request has been submitted.");
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MRS, request.ID, 0, 0, request.remarks, CMMS.CMMS_Status.MRS_SUBMITTED);
            // Adding code for isApproval_required=0 then go for MRS approval automatically.
            if (isApproval_required == false)
            {
                CMMrsApproval approve_request = new CMMrsApproval();
                approve_request.id = request.ID;
                approve_request.comment = "MRS approval not required.";
                var mrsApproved = await mrsApproval(approve_request, UserID);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MRS, request.ID, 0, 0, approve_request.comment, CMMS.CMMS_Status.MRS_REQUEST_APPROVED);
            }

            return response;
        }

        public async Task<CMDefaultResponse> updateMRS(CMMRS request, int UserID)
        {
            int status = 0;
            if (request.is_submit == 0)
            {
                status = (int)CMMS.CMMS_Status.MRS_SUBMITTED;
            }
            else
            {
                status = (int)CMMS.CMMS_Status.MRS_SUBMITTED;
            }

            for (var j = 0; j < request.cmmrsItems.Count; j++)
            {
                if (request.cmmrsItems[j].issued_qty < 0)
                {
                    return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Issued quantity should be less than 0 for asset item " + request.cmmrsItems[j].asset_item_ID + ".");
                }
            }
            var lastMRSID = request.ID;
            request.requested_by_emp_ID = UserID;
            CMDefaultResponse response = null;

            string updatestmt = $" START TRANSACTION; UPDATE smmrs SET facility_ID = {request.facility_ID}, requested_by_emp_ID = {request.requested_by_emp_ID}, requested_date = '{DateTime.Now.ToString("yyyy-MM-dd  HH:mm")}'," +
                                $" setAsTemplate = '{request.setAsTemplate}',  approval_status = {request.approval_status}, activity='{request.activity}',whereUsedType={request.whereUsedType},whereUsedRefID={request.whereUsedRefID}, remarks = '{request.remarks}'" +
                                $" , from_actor_type_id = {request.from_actor_type_id}, from_actor_id = {request.from_actor_id}, to_actor_type_id = {request.to_actor_type_id} " +
                                $" , to_actor_id = {request.to_actor_id}, status={status} WHERE ID = {request.ID} ;" +
                                $"DELETE FROM smrsitems WHERE mrs_ID =  {lastMRSID} ;COMMIT;";
            await Context.ExecuteNonQry<int>(updatestmt);
            for (var i = 0; i < request.cmmrsItems.Count; i++)
            {
                int available_qty = await GetAvailableQty(request.cmmrsItems[i].asset_item_ID, request.facility_ID);

                request.cmmrsItems[i].available_qty = available_qty; //- request.cmmrsItems[i].requested_qty;
                int equipmentID = request.cmmrsItems[i].asset_item_ID;
                decimal quantity = request.cmmrsItems[i].qty;

                string selectQuery = "SELECT sam.approval_required as approval_required_ID, sat.asset_code, asset_type_ID FROM smassetitems sat " +
                           "LEFT JOIN smassetmasters sam ON sam.asset_code = sat.asset_code " +
                           "WHERE sam.ID = " + equipmentID;

                List<CMSMMaster> assetList = await Context.GetData<CMSMMaster>(selectQuery).ConfigureAwait(false);
                var approval_required = assetList[0].approval_required;
                var asset_type_ID = assetList[0].asset_type_ID;
                var asset_code = assetList[0].asset_code;
                var IsSpareSelectionEnable = await getMultiSpareSelectionStatus(asset_code, asset_type_ID);
                if (Convert.ToInt32(IsSpareSelectionEnable) == 0 || asset_type_ID != 0)
                {
                    try
                    {
                        string insertStmt = $"START TRANSACTION; " +
                        $"INSERT INTO smrsitems (mrs_ID,asset_item_ID,asset_MDM_code,requested_qty,status,flag,approval_required,mrs_return_ID,issued_qty,returned_qty,used_qty,available_qty, is_splited,serial_number)" +
                        $"VALUES ({request.ID},{request.cmmrsItems[i].asset_item_ID},'{asset_code}',{request.cmmrsItems[i].requested_qty},0,0,{approval_required},0,{request.cmmrsItems[i].issued_qty}, {request.cmmrsItems[i].returned_qty}, {request.cmmrsItems[i].used_qty}, {request.cmmrsItems[i].available_qty},1,'{request.cmmrsItems[i].serial_number}')" +
                        $"; SELECT LAST_INSERT_ID();COMMIT;";
                        DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    { throw ex; }
                }
                else
                {
                    try
                    {
                        string insertStmt = $"START TRANSACTION; " +
                        $"INSERT INTO smrsitems (mrs_ID,asset_item_ID,asset_MDM_code,requested_qty,status,flag,approval_required,mrs_return_ID,issued_qty,returned_qty,used_qty,available_qty,is_splited,serial_number)" +
                        $"VALUES ({request.ID},{request.cmmrsItems[i].asset_item_ID},'{asset_code}',1,0,0,{approval_required},0,{request.cmmrsItems[i].issued_qty}, {request.cmmrsItems[i].returned_qty}, {request.cmmrsItems[i].used_qty}, {request.cmmrsItems[i].available_qty},1,'{request.cmmrsItems[i].serial_number}')" +
                        $"; SELECT LAST_INSERT_ID();COMMIT;";
                        DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    { throw ex; }
                }
                if (Convert.ToInt32(IsSpareSelectionEnable) == 1 && asset_type_ID != 2)
                {
                    UpdateAssetStatus(request.cmmrsItems[i].asset_item_ID, 2);
                }

            }
            response = new CMDefaultResponse(request.ID, CMMS.RETRUNSTATUS.SUCCESS, "Request has been updated.");
            return response;
        }

        public async void UpdateAssetStatus(int assetItemID, int status)
        {
            string stmt = "SELECT sam.asset_type_ID FROM smassetitems sai " +
                          "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code " +
                          $"WHERE sam.ID = {assetItemID}";
            List<CMMRS> _List = await Context.GetData<CMMRS>(stmt).ConfigureAwait(false);
            if (_List != null && _List[0].asset_type_ID > 0)
            {
                if (Context != null)
                {
                    string stmtUpdate = $"UPDATE smassetitems SET status = {status} WHERE ID = {assetItemID}";
                    await Context.ExecuteNonQry<int>(stmtUpdate);
                }
                else
                {
                    var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    string connectionstring = MyConfig.GetValue<string>("ConnectionStrings:Con");
                    MYSQLDBHelper db = new MYSQLDBHelper(connectionstring);
                    string stmtUpdate = $"UPDATE smassetitems SET status = {status} WHERE ID = {assetItemID}";
                    await db.ExecuteNonQry<int>(stmtUpdate);
                }
            }

        }



        protected async Task<int> getMultiSpareSelectionStatus(string asset_code = "", int asset_ID = 0)
        {
            string stmt = "";
            if (!string.IsNullOrEmpty(asset_code))
            {

                stmt = $"SELECT f_sum.spare_multi_selection FROM smassetmasters sam JOIN smunitmeasurement f_sum ON sam.unit_of_measurement = f_sum.ID WHERE sam.asset_code = '{asset_code}'";
            }
            else if (asset_ID > 0)
            {

                stmt = $"SELECT f_sum.spare_multi_selection FROM smassetitems sai JOIN smassetmasters sam ON sai.asset_code = sam.asset_code JOIN smunitmeasurement f_sum ON sam.unit_of_measurement = f_sum.ID WHERE sai.ID = {asset_ID}";
            }

            List<CMUnitMeasurement> _checkList = await Context.GetData<CMUnitMeasurement>(stmt).ConfigureAwait(false);
            return _checkList[0].spare_multi_selection;
        }
        internal async Task<List<CMMRSItems>> getMRSItems(int ID, string facilitytimeZone)
        {
            //string stmt = "SELECT smi.ID,smi.return_remarks,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID,smi.asset_MDM_code," +
            //    "t1.serial_number,smi.returned_qty,smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,sm.flag as status,DATE_FORMAT(sm.returnDate,'%Y-%m-%d') as returnDate," +
            //    "sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d') as approved_date,DATE_FORMAT(sm.requested_date,'%Y-%m-%d') as issued_date," +
            //    "DATE_FORMAT(sm.returnDate, '%Y-%m-%d') as returnDate, smi.requested_qty,if(smi.approval_required = 1,'Yes','No') as approval_required,\r\n " +
            //    "t1.asset_name,t1.asset_type_ID,t1.asset_type,COALESCE(t1.file_path,'') as file_path,t1.Asset_master_id , smi.is_splited\r\n        FROM smrsitems smi\r\n " +
            //    " LEFT JOIN smmrs sm ON sm.ID = smi.mrs_ID         \r\n        LEFT JOIN (SELECT sai.ID as asset_item_ID, sai.serial_number, sam.asset_name, " +
            //    "sam.asset_type_ID,sat.asset_type,COALESCE(file.file_path,'') as file_path,file.Asset_master_id\r\n        FROM smassetitems sai  " +
            //    "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code LEFT JOIN smassetmasterfiles  file ON file.Asset_master_id =  sam.ID " +
            //    "LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID) as t1 ON t1.asset_item_ID = smi.asset_item_ID" +
            //    "  WHERE smi.mrs_ID = " + ID + " /*GROUP BY smi.ID*/";

            string stmt = "SELECT smi.ID,smi.return_remarks,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID," +
                "smi.asset_MDM_code as asset_code,smi.returned_qty," +
                "smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,sm.flag as status, " +
                "DATE_FORMAT(sm.returnDate,'%Y-%m-%d %H:%i') as returnDate,sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d %H:%i') as approved_date," +
                "DATE_FORMAT(sm.requested_date,'%Y-%m-%d %H:%i') as issued_date,DATE_FORMAT(sm.returnDate, '%Y-%m-%d %H:%i') as returnDate, smi.requested_qty, " +
                "if(smi.approval_required = 1,'Yes','No') as approval_required, smi.is_splited, sam.ID as asset_item_ID," +
                " smi.serial_number as serial_number, sam.asset_name, sam.asset_type_ID,sat.asset_type,COALESCE(file.file_path,'') as file_path," +
                "file.Asset_master_id,sai.materialID, sai.assetMasterID " +
                " FROM smrsitems smi " +
                " LEFT JOIN smmrs sm ON sm.ID = smi.mrs_ID " +
                " left join smassetmasters sam ON sam.id = smi.asset_item_ID " +
                " left join smassetitems sai on sai.assetMasterID =  sam.id " +
                " LEFT JOIN smassetmasterfiles  file ON file.Asset_master_id =  sam.ID    " +
                " LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID     " +
                " WHERE (smi.mrs_ID = " + ID + ")and smi.is_splited = 1 GROUP BY smi.ID";
            List<CMMRSItems> _List = await Context.GetData<CMMRSItems>(stmt).ConfigureAwait(false);
            for (var i = 0; i < _List.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_MRS, _Status);
                string _status_long = getLongStatus(_Status, _List[i].ID);
                _List[i].status_short = _shortStatus;
                _List[i].status_long = _status_long;
            }

            return _List;

        }

        internal async Task<List<CMMRSItems>> getMRSItemsReturn(int ID, string facilitytimeZone)
        {
            //string stmt = "SELECT smi.ID,smi.return_remarks,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID,smi.asset_MDM_code," +
            //    "t1.serial_number,smi.returned_qty,smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,sm.flag as status,DATE_FORMAT(sm.returnDate,'%Y-%m-%d') as returnDate," +
            //    "sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d') as approved_date,DATE_FORMAT(sm.requested_date,'%Y-%m-%d') as issued_date," +
            //    "DATE_FORMAT(sm.returnDate, '%Y-%m-%d') as returnDate, smi.requested_qty,if(smi.approval_required = 1,'Yes','No') as approval_required,\r\n " +
            //    "t1.asset_name,t1.asset_type_ID,t1.asset_type,COALESCE(t1.file_path,'') as file_path,t1.Asset_master_id , smi.is_splited\r\n        FROM smrsitems smi\r\n " +
            //    " LEFT JOIN smmrs sm ON sm.ID = smi.mrs_ID         \r\n        LEFT JOIN (SELECT sai.ID as asset_item_ID, sai.serial_number, sam.asset_name, " +
            //    "sam.asset_type_ID,sat.asset_type,COALESCE(file.file_path,'') as file_path,file.Asset_master_id\r\n        FROM smassetitems sai  " +
            //    "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code LEFT JOIN smassetmasterfiles  file ON file.Asset_master_id =  sam.ID " +
            //    "LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID) as t1 ON t1.asset_item_ID = smi.asset_item_ID" +
            //    "  WHERE smi.mrs_ID = " + ID + " /*GROUP BY smi.ID*/";

            string stmt = "SELECT smi.ID,smi.return_remarks,smi.mrs_ID as original_mrs_ID,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID," +
                "smi.asset_MDM_code,smi.returned_qty," +
                "smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,sm.flag as status, " +
                "DATE_FORMAT(sm.returnDate,'%Y-%m-%d %H:%i') as returnDate,sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d %H:%i') as approved_date," +
                "DATE_FORMAT(sm.requested_date,'%Y-%m-%d %H:%i') as issued_date,DATE_FORMAT(sm.returnDate, '%Y-%m-%d %H:%i') as returnDate, smi.requested_qty, " +
                "if(smi.approval_required = 1,'Yes','No') as approval_required, smi.is_splited, sam.ID as asset_item_ID," +
                " smi.serial_number, sam.asset_name, sam.asset_type_ID,sat.asset_type,COALESCE(file.file_path,'') as file_path," +
                "file.Asset_master_id,sai.materialID, sai.assetMasterID " +
                " FROM smrsitems smi " +
                " LEFT JOIN smmrs sm ON sm.ID = smi.mrs_return_ID " +
                " left join smassetmasters sam ON sam.id = smi.asset_item_ID " +
                " left join smassetitems sai on sai.assetMasterID =  sam.id " +
                " LEFT JOIN smassetmasterfiles  file ON file.Asset_master_id =  sam.ID    " +
                " LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID     " +
                " WHERE (smi.mrs_return_ID = " + ID + " )and smi.is_splited = 1 and is_mrs_return= 1 GROUP BY smi.ID";
            List<CMMRSItems> _List = await Context.GetData<CMMRSItems>(stmt).ConfigureAwait(false);
            for (var i = 0; i < _List.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_MRS, _Status);
                string _status_long = getLongStatus(_Status, _List[i].ID);
                _List[i].status_short = _shortStatus;
                _List[i].status_long = _status_long;
            }

            return _List;

        }

        internal async Task<List<CMMRSItemsBeforeIssue>> getMRSItemsBeforeIssue(int ID, string facilitytimeZone)
        {

            string stmt = "SELECT smi.ID,smi.return_remarks,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID,smi.asset_MDM_code," +
                "smi.returned_qty,smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,sm.flag,DATE_FORMAT(sm.returnDate,'%Y-%m-%d %H:%i') as returnDate," +
                "sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d %H:%i') as approved_date,DATE_FORMAT(sm.requested_date,'%Y-%m-%d %H:%i') as issued_date," +
                "DATE_FORMAT(sm.returnDate, '%Y-%m-%d') as returnDate, smi.requested_qty,if(smi.approval_required = 1,'Yes','No') as approval_required," +
                "\r\n        t1.asset_name,t1.asset_type_ID,t1.asset_type,COALESCE(t1.file_path,'') as file_path,t1.Asset_master_id, t1.spare_multi_selection" +
                "     FROM smrsitems smi\r\n        LEFT JOIN smmrs sm ON sm.ID = smi.mrs_ID   LEFT JOIN (SELECT sai.ID as asset_item_ID, sai.asset_MDM_code, " +
                "sam.asset_name, sam.asset_type_ID,sat.asset_type,spare_multi_selection, COALESCE(file.file_path,'') as file_path,file.Asset_master_id   " +
                "FROM smrsitems sai  LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_MDM_code  LEFT JOIN smassetmasterfiles  file " +
                "ON file.Asset_master_id =  sam.ID  LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID  LEFT JOIN smunitmeasurement f_sum " +
                "ON  f_sum.id = sam.unit_of_measurement      ) as t1 ON t1.asset_MDM_code = smi.asset_MDM_code where smi.mrs_ID = " + ID + " GROUP BY smi.ID";
            List<CMMRSItemsBeforeIssue> _List = await Context.GetData<CMMRSItemsBeforeIssue>(stmt).ConfigureAwait(false);

            return _List;
        }


        internal async Task<List<CMMRSItemsBeforeIssue>> getMRSItemsWithCode(int ID, string facilitytimeZone)
        {
            string stmt = "SELECT smi.ID,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID,smi.asset_MDM_code," +
                "smi.returned_qty,smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,\r\nsm.flag," +
                "DATE_FORMAT(sm.returnDate,'%Y-%m-%d %H:%i') as returnDate,sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d %H:%i') as approved_date," +
                "\r\nDATE_FORMAT(sm.requested_date,'%Y-%m-%d %H:%i') as issued_date,DATE_FORMAT(sm.returnDate, '%Y-%m-%d %H:%i') as returnDate," +
                " sum(smi.requested_qty) as requested_qty,if(smi.approval_required = 1,'Yes','No') as approval_required, " +
                "\r\n        '' as asset_name, sam.ID as asset_type_ID,sat.asset_type,COALESCE(file.file_path,'') as file_path," +
                "file.Asset_master_id, f_sum.spare_multi_selection, sai.serial_number \r\n       " +
                " FROM smrsitems smi \r\n        LEFT JOIN smmrs sm ON sm.ID = smi.mrs_ID \r\n       " +
                " LEFT JOIN smassetitems sam ON sam.asset_code = smi.asset_MDM_code \r\n       " +
                " LEFT JOIN smassetmasterfiles file ON file.Asset_master_id = sam.ID \r\n        " +
                "LEFT JOIN smassettypes sat ON sat.ID = sam.facility_ID\r\n        " +
                "LEFT JOIN smunitmeasurement f_sum ON  f_sum.id = sam.facility_ID\r\n        " +
                "LEFT JOIN smassetitems sai ON  sai.ID = smi.asset_item_ID" +
                " WHERE smi.mrs_ID = " + ID + " GROUP BY smi.asset_MDM_code";

            List<CMMRSItemsBeforeIssue> _List = await Context.GetData<CMMRSItemsBeforeIssue>(stmt).ConfigureAwait(false);

            return _List;
        }

        internal async Task<CMMRSList> getMRSDetails(int ID, string facilitytimeZone)
        {
            //string stmt = $"SELECT * FROM smmrs WHERE ID = {ID}";
            //List<CMMRS> _List = await Context.GetData<CMMRS>(stmt).ConfigureAwait(false);
            //for (var i = 0; i < _List.Count; i++)
            //{
            //    CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
            //    string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_MRS, _Status);
            //    _List[i].status_short = _shortStatus;
            //}
            //return _List;
            string stmt = "SELECT sm.ID,sm.requested_by_emp_ID,CONCAT(ed1.firstName,' ',ed1.lastName) as approver_name,DATE_FORMAT(sm.requested_date,'%Y-%m-%d %H:%i') as requestd_date," +
    "DATE_FORMAT(sm.returnDate,'%Y-%m-%d %H:%i') as returnDate,if(sm.approval_status != '',DATE_FORMAT(sm.approved_date,'%Y-%m-%d %H:%i'),'') as approval_date,sm.approval_status," +
    "sm.approval_comment,CONCAT(ed.firstName,' ',ed.lastName) as requested_by_name, sm.status, sm.activity, sm.whereUsedType," +
    " case when sm.whereUsedType = 1 then 'Job' when sm.whereUsedType = 2 then 'PM' when sm.whereUsedType = 4 then 'JOBCARD' when sm.whereUsedType = 27 then 'PMTASK' else 'Invalid' end as whereUsedTypeName,  sm.whereUsedRefID, sm.remarks " +
    ", DATE_FORMAT(sm.issuedAt,'%Y-%m-%d %H:%i') as issued_date, CONCAT(issuedUser.firstName,' ',issuedUser.lastName) as issued_name " +
    " FROM smmrs sm LEFT JOIN users ed ON ed.id = sm.requested_by_emp_ID " +
    " LEFT JOIN users ed1 ON ed1.id = sm.approved_by_emp_ID " +
    " LEFT JOIN users issuedUser ON issuedUser.id = sm.issued_by_emp_ID " +
    "WHERE sm.id = " + ID + ";";
            List<CMMRSList> _List = await Context.GetData<CMMRSList>(stmt).ConfigureAwait(false);

            for (var i = 0; i < _List.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_MRS, _Status);
                string _status_long = getLongStatus(_Status, _List[i].ID);
                _List[i].status_short = _shortStatus;
                _List[i].status_long = _status_long;
                _List[i].CMMRSItems = await getMRSItems(_List[i].ID, facilitytimeZone);
            }

            return _List[0];
        }

        internal async Task<CMMRSReturnList> getReturnDataByID(int ID, string facilitytimeZone)
        {
            string stmt = "SELECT sm.ID,sm.requested_by_emp_ID as requested_by_emp_ID,CONCAT(ed1.firstName,' ',ed1.lastName) as approver_name,sm.remarks as remarks ," +
    "DATE_FORMAT(sm.returnDate,'%Y-%m-%d %H:%i') as returnDate,if(sm.approval_status != '',DATE_FORMAT(sm.approved_date,'%Y-%m-%d %H:%i'),'') as approval_date,sm.approval_status," +
    "sm.approval_comment,CONCAT(ed.firstName,' ',ed.lastName) as requested_by_name, sm.status, sm.activity, sm.whereUsedType, " +
    " case when sm.whereUsedType = 1 then 'Job' when sm.whereUsedType = 2 then 'PM' when sm.whereUsedType = 4 then 'JOBCARD' when sm.whereUsedType = 27 then 'PMTASK' else 'Invalid' end as whereUsedTypeName, sm.whereUsedRefID, COALESCE(sm.remarks,'') as  remarks " +
    "FROM smmrs sm LEFT JOIN users ed ON ed.id = sm.requested_by_emp_ID " +
    " LEFT JOIN users ed1 ON ed1.id = sm.approved_by_emp_ID " +
    " WHERE sm.ID = " + ID + "  and sm.flag = 2";
            List<CMMRSReturnList> _List = await Context.GetData<CMMRSReturnList>(stmt).ConfigureAwait(false);
            foreach (var _Item in _List)
            {
                string s = $"select whereUsedRefID from smmrs where id={_Item.ID}";
                DataTable dtInsert = await Context.FetchData(s).ConfigureAwait(false);
                int id = Convert.ToInt32(dtInsert.Rows[0][0]);
                string sp = $"select ID from smmrs where whereUsedRefID={id} and is_mrs_return=0 ";
                DataTable dtnsert = await Context.FetchData(sp).ConfigureAwait(false);
                _Item.mrs_id = Convert.ToInt32(dtnsert.Rows[0][0]);
            }

            for (var i = 0; i < _List.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_MRS, _Status);
                string _status_long = getLongStatus(_Status, _List[i].ID);
                _List[i].status_short = _shortStatus;
                _List[i].status_long = _status_long;
                _List[i].CMMRSItems = await getMRSReturnItems(_List[i].ID, facilitytimeZone);
                _List[i].CMMRSFaultyItems = _List[i].CMMRSItems.Where(x => x.is_faulty == 1).ToList();
                _List[i].CMMRSItems = _List[i].CMMRSItems.Where(x => x.is_faulty == 0).ToList();
            }

            if (_List.Count > 0)
            {
                return _List[0];
            }
            else
            {
                return null;
            }


        }

        //internal async Task<CMDefaultResponse> mrsApproval(CMMRS request)
        //{
        //    CMDefaultResponse response = null;
        //    bool Queryflag = false;
        //    var flag = "MRS_RETURN_REQUEST";
        //    string comment = "";
        //    string remarks = "";
        //    string stmtSelect = $"SELECT * FROM smmrs WHERE ID = {request.ID}";
        //    List<CMMRS> mrsList = await Context.GetData<CMMRS>(stmtSelect).ConfigureAwait(false);

        //    var equipmentName = new List<int>();
        //    for (int i = 0; i < request.cmmrsItems.Count; i++)
        //    {
        //        // equipmentID = 3 for SM_ASSET_TYPE_SPARE
        //        if (request.cmmrsItems[i].equipmentID == 3)
        //        {
        //            decimal availableQty = Convert.ToDecimal(GetAvailableQty(request.asset_item_ID, mrsList[0].facility_ID));

        //            if (availableQty < request.cmmrsItems[i].issued_qty)
        //            {
        //                equipmentName.Add(request.cmmrsItems[i].id);    
        //            }
        //        }
        //    }

        //    if (equipmentName != null && equipmentName.Count >0)
        //    {
        //        response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Sorry, " + string.Join(",", equipmentName) + " serial no.(s) has lower quantity than issued.");
        //        return response;
        //    }

        //    for(int i=0; i < request.cmmrsItems.Count; i++)
        //    {
        //        bool callUpdateAssetStatus = false;
        //        string stmtUpdate = $"UPDATE smrsitems SET available_qty={request.cmmrsItems[i].qty}, issued_qty={request.cmmrsItems[i].issued_qty}";
        //        // SM_ASSET_TYPE_SPARE = 3
        //        if (request.cmmrsItems[i].equipmentID == 3 && request.asset_item_ID != null)
        //        {
        //            stmtUpdate = stmtUpdate + $" , asset_item_ID = {request.asset_item_ID}";
        //            request.asset_item_ID = request.asset_item_ID;
        //            if (request.cmmrsItems[i].equipmentID == 1)
        //            {
        //                callUpdateAssetStatus = true;
        //            }
        //        }

        //        stmtUpdate += $" WHERE ID = {request.cmmrsItems[i].id}";
        //        await Context.ExecuteNonQry<int>(stmtUpdate);
        //        Queryflag = true;   

        //        if(request.approval_status == 1)
        //        {
        //            if (callUpdateAssetStatus)
        //            {
        //                UpdateAssetStatus(request.asset_item_ID, 3);
        //            }
        //            int requested_by_emp_ID = mrsList[0].requested_by_emp_ID;
        //            if (Convert.ToInt32(mrsList[0].reference) == 4)
        //            {
        //                remarks = "JOBCard : JC" + mrsList[0].referenceID;
        //            }
        //            if (Convert.ToInt32(mrsList[0].reference) == 5)
        //            {
        //                remarks = "Prementive Maintainance : PM" + mrsList[0].referenceID;
        //            }
        //            var refID = mrsList[0].ID;
        //            var tResult = await TransactionDetails(mrsList[0].facility_ID, mrsList[0].facility_ID,2 , mrsList[0].requested_by_emp_ID,3, request.cmmrsItems[0].id, Convert.ToInt32(request.cmmrsItems[0].issued_qty), Convert.ToInt32(mrsList[0].reference), Convert.ToInt32(mrsList[0].referenceID),"", refID);
        //            if (!tResult) {

        //                throw new Exception("transaction details failed");
        //            }
        //            comment = "MRS Request Approved";
        //        }
        //        else
        //        {
        //              comment = "MRS Request Rejected";
        //                UpdateAssetStatus(request.asset_item_ID, 1); // if it is MRS reject reset the status of assets
        //        }
        //        }

        //    if (!Queryflag)
        //    {
        //        response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to update data.");
        //    }
        //    else
        //    {
        //        string stmt = $"UPDATE smmrs SET approved_by_emp_ID = {request.approved_by_emp_ID}, approved_date='{request.approved_date.ToString("yyyy-MM-dd HH:mm")}',"+ 
        //                       $"approval_status ={request.approval_status},approval_comment = '{request.return_remarks}',flag = 1 WHERE ID = {request.ID}";
        //        await Context.ExecuteNonQry<int>(stmt);
        //        response = new CMDefaultResponse(request.ID, CMMS.RETRUNSTATUS.SUCCESS, "Status updated.");
        //    }
        //    return response;
        //}

        internal async Task<CMDefaultResponse> mrsApproval(CMMrsApproval request, int userId)
        {
            CMDefaultResponse response = null;
            string stmtSelect = $"SELECT ID,facility_ID, requested_by_emp_ID,reference,from_actor_type_id,from_actor_id,to_actor_type_id,to_actor_id FROM smmrs WHERE ID = {request.id}";
            List<CMMRS> mrsList = await Context.GetData<CMMRS>(stmtSelect).ConfigureAwait(false);

            string stmtSelectItems = "SELECT ID,requested_qty,asset_item_ID FROM smrsitems WHERE mrs_ID = " + request.id + "";
            List<CMMRSItems> mrsItemList = await Context.GetData<CMMRSItems>(stmtSelectItems).ConfigureAwait(false);


            if (mrsList.Count > 0)
            {
                string stmt = $"UPDATE smmrs SET approved_by_emp_ID = {userId}, approved_date='{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
                                   $" status ={(int)CMMS.CMMS_Status.MRS_REQUEST_APPROVED} ,approval_status = 1,approval_comment = '{request.comment}' WHERE ID = {request.id}";
                await Context.ExecuteNonQry<int>(stmt);
                // update approval quantity in mrsitems
                if (request.cmmrsItems != null)
                {
                    for (int i = 0; i < request.cmmrsItems.Count; i++)
                    {
                        decimal approved_qty = 0;
                        if (request.cmmrsItems[i].approval_qty == 0 && request.cmmrsItems[i].approval_qty == null)
                        {
                            approved_qty = mrsItemList.Where(item => item.ID == request.cmmrsItems[i].mrs_item_id).Select(item => item.requested_qty).FirstOrDefault();
                        }
                        else
                        {
                            approved_qty = request.cmmrsItems[i].approval_qty;
                        }
                        string updateStatement = $"update smrsitems set approved_qty = {approved_qty} where id= {request.cmmrsItems[i].mrs_item_id};";
                        await Context.ExecuteNonQry<int>(updateStatement);
                    }
                }
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Status updated.");
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Invalid mrs updated.");
            }



            // Adding code to 

            //if (mrsList[0].from_actor_id > 0 && mrsList[0].from_actor_type_id > 0 && mrsList[0].to_actor_type_id > 0 && mrsList[0].to_actor_id > 0)
            //{
            //    for (var i = 0; i < mrsItemList.Count(); i++)
            //    {
            //        var tResult = await TransactionDetails(mrsList[0].facility_ID, mrsList[0].from_actor_id, mrsList[0].from_actor_type_id, mrsList[0].to_actor_id, mrsList[0].to_actor_type_id, mrsItemList[i].asset_item_ID, Convert.ToInt32(mrsItemList[i].requested_qty), (int)CMMS.CMMS_Modules.SM_MRS, request.id, request.comment, mrsList[0].ID);
            //        if (!tResult)
            //        {
            //            return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Transaction details failed.");
            //        }
            //    }
            //}
            //else
            //{
            //    for (var i = 0; i < mrsItemList.Count(); i++)
            //    {
            //        var tResult = await TransactionDetails(mrsList[0].facility_ID, mrsList[0].facility_ID, (int)CMMS.SM_Actor_Types.Store, mrsList[0].requested_by_emp_ID, (int)CMMS.SM_Actor_Types.Engineer, mrsItemList[i].asset_item_ID, Convert.ToInt32(mrsItemList[i].requested_qty), (int)CMMS.CMMS_Modules.SM_MRS, request.id, request.comment, mrsList[0].ID);
            //        if (!tResult)
            //        {
            //            return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Transaction details failed.");
            //        }
            //    }
            //}

            //for (var i = 0; i < mrsItemList.Count(); i++)
            //{
            //    var tResult = await TransactionDetails(mrsList[0].facility_ID, mrsList[0].facility_ID, (int)CMMS.SM_Actor_Types.Store, mrsList[0].requested_by_emp_ID, (int)CMMS.SM_Actor_Types.Task, mrsItemList[i].asset_item_ID, Convert.ToInt32(mrsItemList[i].requested_qty), (int)CMMS.CMMS_Modules.SM_MRS, request.id, request.comment, mrsList[0].ID);
            //    if (!tResult)
            //    {
            //        return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Transaction details failed.");
            //    }
            //}


            var data = await getMRSItems(request.id, "");

            foreach (var item in data)
            {
                int assetTypeId = 0;
                string stmtAssetType = $"SELECT asset_type_ID FROM smassetmasters WHERE id = '{item.assetMasterID}'";
                DataTable dtAssetType = await Context.FetchData(stmtAssetType).ConfigureAwait(false);

                if (dtAssetType == null && dtAssetType.Rows.Count == 0)
                {
                    throw new Exception("Asset type ID not found");
                }
                else
                {
                    assetTypeId = Convert.ToInt32(dtAssetType.Rows[0][0]);
                }
                if (assetTypeId == (int)CMMS.SM_AssetTypes.Spare)
                {
                    var stmtU = $"UPDATE smrsitems set is_splited = 0 where id = {item.ID}";
                    int resultU = await Context.ExecuteNonQry<int>(stmtU).ConfigureAwait(false);
                }
            }

            foreach (var item in data)
            {
                var assetCode = item.asset_code;
                var orderedQty = item.requested_qty;
                var available_qty = item.available_qty;
                var type = item.asset_type;

                // Get the asset type ID.
                int assetTypeId = 0;
                string stmtAssetType = $"SELECT asset_type_ID FROM smassetmasters WHERE id = '{item.assetMasterID}'";
                DataTable dtAssetType = await Context.FetchData(stmtAssetType).ConfigureAwait(false);

                if (dtAssetType == null && dtAssetType.Rows.Count == 0)
                {
                    throw new Exception("Asset type ID not found");
                }
                else
                {
                    assetTypeId = Convert.ToInt32(dtAssetType.Rows[0][0]);
                }
                int isMultiSelectionEnabled = await getMultiSpareSelectionStatus(item.asset_code);
                // Check if the asset type is Spare.
                // assetTypeId == 2 is Spare
                if (assetTypeId == (int)CMMS.SM_AssetTypes.Spare || assetTypeId == (int)CMMS.SM_AssetTypes.Tools || assetTypeId == (int)CMMS.SM_AssetTypes.SpecialTools)
                {
                    int assetItemId;
                    for (var i = 0; i < item.requested_qty; i++)
                    {
                        int assetMasterID = item.asset_item_ID;
                        try
                        {
                            var stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,item_condition,status,assetMasterID,asset_type) VALUES ({mrsList[0].facility_ID},'{assetCode}',1,0,{assetMasterID}, {assetTypeId}); SELECT LAST_INSERT_ID();";
                            DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                            assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);
                            var stmtU = $"UPDATE smassetitems set materialID = {assetItemId} where id = {assetItemId}";
                            int resultU = await Context.ExecuteNonQry<int>(stmtU).ConfigureAwait(false);

                            string insertStmt = $"START TRANSACTION; " +
                            $"INSERT INTO smrsitems (mrs_ID,asset_item_ID,asset_MDM_code,requested_qty,status,flag,approval_required,mrs_return_ID,issued_qty,returned_qty,used_qty,available_qty, is_splited,approved_qty)" +
                            $"VALUES ({request.id},{item.asset_item_ID},'{item.asset_code}',1,0,0,1,0,0,0, 0, {available_qty},1,1)" +
                            $"; SELECT LAST_INSERT_ID();COMMIT;";
                            DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        { throw ex; }
                    }

                }
                else
                {
                    int assetMasterID = 0;
                    assetMasterID = item.asset_item_ID;
                    var stmtI = $"INSERT INTO smassetitems (facility_ID,asset_code,item_condition,status,assetMasterID,materialID,asset_type) VALUES ({mrsList[0].facility_ID},'{assetCode}',1,0,{assetMasterID},{assetMasterID}, {assetTypeId}); SELECT LAST_INSERT_ID();";
                    DataTable dtInsert = await Context.FetchData(stmtI).ConfigureAwait(false);
                    var assetItemId = Convert.ToInt32(dtInsert.Rows[0][0]);

                    var stmtU = $"UPDATE smrsitems set approved_qty = {orderedQty} where id = {item.ID}";
                    int resultU = await Context.ExecuteNonQry<int>(stmtU).ConfigureAwait(false);
                }

            }

            string historyComment = "";
            if (request.comment != "")
            {
                historyComment = request.comment;
            }
            else
            {
                historyComment = "MRS Approved";
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MRS, request.id, 0, 0, request.comment, CMMS.CMMS_Status.MRS_REQUEST_APPROVED);

            return response;
        }

        internal async Task<CMDefaultResponse> mrsReject(CMApproval request, int userId)
        {
            CMDefaultResponse response = null;
            string stmtSelect = $"SELECT ID FROM smmrs WHERE ID = {request.id}";
            List<CMMRS> mrsList = await Context.GetData<CMMRS>(stmtSelect).ConfigureAwait(false);

            if (mrsList.Count > 0)
            {
                string stmt = $"UPDATE smmrs SET rejected_by_emp_ID = {userId}, rejected_date='{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
                                   $" status ={(int)CMMS.CMMS_Status.MRS_REQUEST_REJECTED} , rejected_comment = '{request.comment}' WHERE ID = {request.id}";
                await Context.ExecuteNonQry<int>(stmt);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Status updated.");
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Invalid mrs updated.");
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MRS, request.id, 0, 0, "MRS rejected.", CMMS.CMMS_Status.MRS_REQUEST_REJECTED);

            return response;
        }

        public async Task<bool> TransactionDetails(int facilityID, int fromActorID, int fromActorType, int toActorID, int toActorType, int assetItemID, int qty, int refType, int refID, string remarks, int mrsID = 0, int natureOfTransaction = 0, int assetItemStatus = 0)
        {
            try
            {
                string stmt = "INSERT INTO smtransactiondetails (plantID,fromActorID,fromActorType,toActorID,toActorType,assetItemID,qty,referedby,reference_ID,remarks,Nature_Of_Transaction,Asset_Item_Status)" +
                              $"VALUES ({facilityID},{fromActorID},{fromActorType},{toActorID},{toActorType},{assetItemID},{qty},{refType},{refID},'{remarks}',{natureOfTransaction},{assetItemStatus}); SELECT LAST_INSERT_ID(); ";

                DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
                int transaction_ID = 0;
                if (dt2.Rows.Count > 0)
                {
                    transaction_ID = Convert.ToInt32(dt2.Rows[0][0]);
                    int debitTransactionID = await DebitTransation(facilityID, transaction_ID, fromActorType, fromActorID, qty, assetItemID, mrsID);
                    int creditTransactionID = await CreditTransation(facilityID, transaction_ID, toActorType, toActorID, qty, assetItemID, mrsID);
                    bool isValid = await VerifyTransactionDetails(transaction_ID, debitTransactionID, creditTransactionID, facilityID, fromActorID, fromActorType, toActorID, toActorType, assetItemID, qty, refType, refID, remarks, mrsID);
                    if (isValid)
                    {
                        //minQtyReminder(assetItemID, plantID);
                        return true;
                    }
                    else
                    {
                        throw new Exception("transaction table not updated properly");
                    }
                }
                else
                {
                    throw new Exception("transaction table not updated properly");
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<int> updateUsedQty(int facilityID, int fromActorID, int fromActorType, int toActorID, int toActorType, int assetItemID, int qty, int refType, int refID, string remarks, int mrsID = 0, int natureOfTransaction = 0, int assetItemStatus = 0, int mrsitemID = 0)
        {
            try
            {
                //string stmt = " select i.issued_qty,i.used_qty from smrsitems i inner join smmrs m on m.ID = i.mrs_ID where i.mrs_ID = " + mrsID + " and asset_item_ID = " + assetItemID + " and is_splited=1; ";
                string stmt = " select i.issued_qty,i.used_qty from smrsitems i inner join smmrs m on m.ID = i.mrs_ID where i.mrs_ID = " + mrsID + " and ID = " + mrsitemID + " and is_splited=1; ";
                DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
                string stmt_inseret = $" insert into log4netlog(Date,Message,Exception)value('{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}','{stmt}','mrs_item_id : {mrsitemID}, mrs_id : {mrsID} ');";
                var resultq = await Context.ExecuteNonQry<int>(stmt_inseret);
                decimal issued_qty = 0;
                int stored_used_qty = 0;
                if (dt2.Rows.Count > 0)
                {
                    issued_qty = Convert.ToInt32(dt2.Rows[0][0]);
                    stored_used_qty = Convert.ToInt32(dt2.Rows[0][1]);
                    qty = qty + stored_used_qty;
                    if (issued_qty >= qty)
                    {
                        string stmt_update = " update smrsitems set used_qty=" + qty + " where id = " + mrsitemID + ";";
                        var result = await Context.ExecuteNonQry<int>(stmt_update);
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }

                }
                else
                {
                    return 2;
                }
            }
            catch (Exception e)
            {
                return 3;
            }
        }


        private async Task<int> DebitTransation(int facilityID, int transactionID, int actorType, int actorID, int debitQty, int assetItemID, int mrsID)
        {
            string stmt = $"INSERT INTO smtransition (facilityID, transactionID, actorType, actorID,creditQty, debitQty, assetItemID, mrsID) VALUES ({facilityID},{transactionID}, '{actorType}', {actorID},0, {debitQty}, {assetItemID}, {mrsID}); SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            return id;
        }

        private async Task<int> CreditTransation(int facilityID, int transactionID, int actorType, int actorID, int qty, int assetItemID, int mrsID)
        {
            string query = $"INSERT INTO smtransition (facilityID, transactionID,actorType,actorID,creditQty,debitQty,assetItemID,mrsID) VALUES ({facilityID},{transactionID},'{actorType}',{actorID},{qty},0,{assetItemID},{mrsID});SELECT LAST_INSERT_ID();";
            DataTable dt2 = await Context.FetchData(query).ConfigureAwait(false);
            int id = Convert.ToInt32(dt2.Rows[0][0]);
            return id;
        }

        public async Task<bool> VerifyTransactionDetails(int transaction_ID, int debitTransactionID, int creditTransactionID, int plantID, int fromActorID, int fromActorType, int toActorID, int toActorType, int assetItemID, int qty, int refType, int refID, string remarks, int mrsID, bool isS2STransaction = false, int dispatchedQty = 0, int vendor_assetItemID = 0)
        {
            string qry = $"SELECT ID FROM smtransactiondetails WHERE ID = '{transaction_ID}' AND plantID = '{plantID}' AND fromActorID = '{fromActorID}' AND fromActorType = '{fromActorType}' AND toActorID = '{toActorID}' AND toActorType = '{toActorType}' AND assetItemID = '{assetItemID}' AND qty = '{qty}' AND referedby = '{refType}' AND reference_ID ='{refID}'";

            string qry2;
            if (isS2STransaction)
            {
                qry2 = $"SELECT ID FROM smtransition WHERE ID = '{debitTransactionID}' AND actorType = '{fromActorType}' AND actorID = '{fromActorID}' AND debitQty = '{dispatchedQty}' AND assetItemID = '{vendor_assetItemID}'";
            }
            else
            {
                qry2 = $"SELECT ID FROM smtransition WHERE ID = '{debitTransactionID}' AND actorType = '{fromActorType}' AND actorID = '{fromActorID}' AND debitQty = '{qty}' AND assetItemID = '{assetItemID}'";
            }

            string qry3 = $"SELECT ID FROM smtransition WHERE ID = '{creditTransactionID}' AND actorType = '{toActorType}' AND actorID = '{toActorID}' AND creditQty = '{qty}' AND assetItemID = '{assetItemID}'";

            DataTable dt1 = await Context.FetchData(qry).ConfigureAwait(false);
            DataTable dt2 = await Context.FetchData(qry2).ConfigureAwait(false);
            DataTable dt3 = await Context.FetchData(qry3).ConfigureAwait(false);

            int transactionDetailID = 0;
            int transactionDebitID = 0;
            int transactionCreditID = 0;

            transactionDetailID = Convert.ToInt32(dt1.Rows[0][0]);
            transactionDebitID = Convert.ToInt32(dt2.Rows[0][0]);
            transactionCreditID = Convert.ToInt32(dt3.Rows[0][0]);


            if (transactionDetailID != 0 && transactionDebitID != 0 && transactionCreditID != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal async Task<CMDefaultResponse> ReturnMRS(CMMRS request, int UserID)
        {
            CMDefaultResponse response = null;
            bool Queryflag = false;
            int MRS_ReturnID = 0;
            var flag = "MRS_RETURN_REQUEST";

            if (request.mrsreturnID > 0)
            {
                // Updating existing MRS;

                var lastMRSID = request.mrsreturnID;
                var refType = "MRSReturnEdit";
                var mailSub = "MRS Return Request Updated.";

                string updatestmt = $"UPDATE smrsitems SET is_splited = 0 WHERE mrs_ID = {request.mrsreturnID};";
                await Context.ExecuteNonQry<int>(updatestmt);

                //string updatestmt = $" START TRANSACTION; UPDATE smmrs SET facility_ID = {request.facility_ID}, requested_by_emp_ID = {UserID}, requested_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
                //    $" activity='{request.activity}',whereUsedType={request.whereUsedType},whereUsedRefID={request.whereUsedRefID},is_mrs_return=1 WHERE ID = {request.mrsreturnID}" +
                //    $" ; DELETE FROM smrsitems WHERE mrs_ID =  {lastMRSID} ; COMMIT;";
                string insertStmt = $"START TRANSACTION; INSERT INTO smmrs (facility_ID,requested_by_emp_ID,requested_date," +
                        $"returnDate,status,flag, activity,whereUsedType,whereUsedRefID,is_mrs_return,from_actor_type_id,from_actor_id,to_actor_type_id,to_actor_id)\r\n VALUES ({request.facility_ID},{UserID},'{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'" +
                        $",'{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}',{(int)CMMS.CMMS_Status.MRS_SUBMITTED}, {2},'{request.activity}',{request.whereUsedType},{request.whereUsedRefID},1,{request.from_actor_type_id},{request.from_actor_id},{request.to_actor_type_id},{request.to_actor_id}); SELECT LAST_INSERT_ID(); COMMIT;";
                try
                {
                    DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                    //request.mrsreturnID = Convert.ToInt32(dt2.Rows[0][0]);
                    MRS_ReturnID = Convert.ToInt32(dt2.Rows[0][0]);
                }
                catch (Exception ex)
                {
                    Queryflag = false;
                    throw ex;
                }
            }
            else
            {
                var refType = "MRSReturn";
                var mailSub = "MRS Return Request";
                string insertStmt = $"START TRANSACTION; INSERT INTO smmrs (facility_ID,requested_by_emp_ID,requested_date," +
                    $"returnDate,status,flag, activity,whereUsedType,whereUsedRefID,is_mrs_return,from_actor_type_id,from_actor_id,to_actor_type_id,to_actor_id)\r\n VALUES ({request.facility_ID},{UserID},'{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'" +
                    $",'{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}',{(int)CMMS.CMMS_Status.MRS_SUBMITTED}, {2},'{request.activity}',{request.whereUsedType},{request.whereUsedRefID},1,{request.from_actor_type_id},{request.from_actor_id},{request.to_actor_type_id},{request.to_actor_id}); SELECT LAST_INSERT_ID(); COMMIT;";
                try
                {
                    DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                    //request.mrsreturnID = Convert.ToInt32(dt2.Rows[0][0]);
                    MRS_ReturnID = Convert.ToInt32(dt2.Rows[0][0]);
                }
                catch (Exception ex)
                {
                    Queryflag = false;
                    throw ex;
                }
            }
            if (request.cmmrsItems != null)
            {
                for (var i = 0; i < request.cmmrsItems.Count; i++)
                {

                    int equipmentID = request.cmmrsItems[i].asset_item_ID;
                    decimal quantity = request.cmmrsItems[i].qty;


                    try
                    {
                        //if (request.cmmrsItems[i].is_faulty == 1)
                        //{
                        //    string insertStmt = $"START TRANSACTION; " +
                        //                $"Update smmrs SET status = {(int)CMMS.CMMS_Status.MRS_SUBMITTED} where id = {request.mrsreturnID} ;INSERT INTO smrsitems (mrs_ID,mrs_return_ID,asset_item_ID,available_qty,requested_qty,returned_qty,return_remarks,flag, is_faulty,is_splited,issued_qty)" +
                        //                $"VALUES ({request.mrsreturnID},0,{request.cmmrsItems[i].asset_item_ID},{request.cmmrsItems[i].qty}, {request.cmmrsItems[i].requested_qty}, {request.cmmrsItems[i].returned_qty}, '{request.cmmrsItems[i].return_remarks}', 2, {request.cmmrsItems[i].is_faulty},1,{request.cmmrsItems[i].issued_qty})" +
                        //                $"; SELECT LAST_INSERT_ID(); COMMIT;";
                        //    DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                        //}
                        //else
                        //{
                        //    string insertStmt = $"START TRANSACTION; " +
                        //        $"INSERT INTO smrsitems (mrs_ID,mrs_return_ID,asset_item_ID,available_qty,requested_qty,returned_qty,return_remarks,flag, is_faulty,is_splited,issued_qty)" +
                        //        $"VALUES ({request.mrsreturnID},{MRS_ReturnID},{request.cmmrsItems[i].asset_item_ID},{request.cmmrsItems[i].qty}, {request.cmmrsItems[i].requested_qty}, {request.cmmrsItems[i].returned_qty}, '{request.cmmrsItems[i].return_remarks}', 2, {request.cmmrsItems[i].is_faulty},1,{request.cmmrsItems[i].issued_qty})" +
                        //        $"; SELECT LAST_INSERT_ID(); COMMIT;";
                        //    DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                        //}

                        int asset_item_ID = 0;
                        int qty = 0;
                        int requested_qty = 0;
                        int issued_qty = 0;
                        string serial_number = "";

                        string chkSrNoAvailableQuery = "select asset_item_ID,available_qty,requested_qty,issued_qty,serial_number from smrsitems where id = " + request.cmmrsItems[i].mrs_item_id + "";
                        DataTable dt_chk = await Context.FetchData(chkSrNoAvailableQuery).ConfigureAwait(false);

                        if (dt_chk.Rows.Count > 0)
                        {
                            asset_item_ID = (dt_chk.Rows[0]["asset_item_ID"].ToInt());
                            qty = (dt_chk.Rows[0]["available_qty"].ToInt());
                            requested_qty = (dt_chk.Rows[0]["requested_qty"].ToInt());
                            issued_qty = (dt_chk.Rows[0]["issued_qty"].ToInt());
                            serial_number = (dt_chk.Rows[0]["serial_number"].ToString());
                        }

                        string insertStmt = $"START TRANSACTION; " +
                            $"INSERT INTO smrsitems (mrs_ID,mrs_return_ID,asset_item_ID,available_qty,requested_qty,returned_qty,return_remarks,flag, is_faulty,is_splited,issued_qty,serial_number)" +
                            $"VALUES ({request.mrsreturnID},{MRS_ReturnID},{asset_item_ID},{qty}, {requested_qty}, {request.cmmrsItems[i].returned_qty}, '{request.cmmrsItems[i].return_remarks}', 2, 0,1,{issued_qty},'{serial_number}')" +
                            $"; SELECT LAST_INSERT_ID(); COMMIT;";
                        DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);


                        string updatestmt = $"UPDATE smassetitems SET item_condition = {request.cmmrsItems[i].is_faulty} WHERE ID = {request.cmmrsItems[i].asset_item_ID};";
                        await Context.ExecuteNonQry<int>(updatestmt);
                    }
                    catch (Exception ex)
                    {
                        Queryflag = false;
                        throw ex;
                    }


                    Queryflag = true;
                }
            }
            int from_actor_id = 0;
            int from_actor_type_id = 0;
            int to_actor_id = 0;
            int to_actor_type_id = 0;

            string getActorIds = "select from_actor_id,from_actor_type_id,to_actor_id,to_actor_type_id  from smmrs where id = " + request.mrsreturnID + ";";
            DataTable dt_actorids = await Context.FetchData(getActorIds).ConfigureAwait(false);

            if (dt_actorids.Rows.Count > 0)
            {
                from_actor_id = (dt_actorids.Rows[0]["from_actor_id"].ToInt());
                from_actor_type_id = (dt_actorids.Rows[0]["from_actor_type_id"].ToInt());
                to_actor_id = (dt_actorids.Rows[0]["to_actor_id"].ToInt());
                to_actor_type_id = (dt_actorids.Rows[0]["to_actor_type_id"].ToInt());


            }
            if (request.faultyItems.Count > 0)
            {
                if (request.mrsreturnID == 0)
                {
                    request.mrsreturnID = MRS_ReturnID;
                }
                for (var i = 0; i < request.faultyItems.Count; i++)
                {
                    int asset_item_ID = request.faultyItems[i].assetMasterItemID;
                    int qty = 0;
                    int requested_qty = 0;
                    int issued_qty = 0;
                    string asset_MDM_code = "";

                    string chkSrNoAvailableQuery = "";
                    DataTable dt_chk = new DataTable();
                    if (request.faultyItems[i].serial_number != null)
                    {
                        chkSrNoAvailableQuery = "select asset_item_ID,available_qty,requested_qty,issued_qty,asset_MDM_code from smrsitems where serial_number = '" + request.faultyItems[i].serial_number + "'";
                        dt_chk = await Context.FetchData(chkSrNoAvailableQuery).ConfigureAwait(false);
                    }


                    if (dt_chk.Rows.Count > 0)
                    {
                        asset_item_ID = (dt_chk.Rows[0]["asset_item_ID"].ToInt());
                        qty = (dt_chk.Rows[0]["available_qty"].ToInt());
                        requested_qty = (dt_chk.Rows[0]["requested_qty"].ToInt());
                        issued_qty = (dt_chk.Rows[0]["issued_qty"].ToInt());
                        asset_MDM_code = (dt_chk.Rows[0]["asset_MDM_code"].ToString());
                    }
                    if (dt_chk.Rows.Count > 0)
                    {
                        string updatestmt = $"Update smmrs SET status = {(int)CMMS.CMMS_Status.MRS_SUBMITTED} where id = {request.mrsreturnID} ;UPDATE smrsitems SET mrs_return_ID = {request.mrsreturnID}, returned_qty={request.faultyItems[i].returned_qty},return_remarks='{request.faultyItems[i].return_remarks}', is_faulty = 1 WHERE ID = {request.faultyItems[i].mrs_item_ID};";
                        await Context.ExecuteNonQry<int>(updatestmt);


                        var tResult = await TransactionDetails(request.facility_ID, from_actor_id, (int)CMMS.SM_Actor_Types.Inventory, to_actor_id, (int)CMMS.SM_Actor_Types.NonOperational, asset_item_ID, Convert.ToInt32(request.faultyItems[i].returned_qty), (int)CMMS.CMMS_Modules.SM_MRS_RETURN, request.mrsreturnID, request.faultyItems[i].return_remarks, 0);


                    }
                    else
                    {
                        string chkSrNoAvailableQuery_faulty = "select asset_item_ID,available_qty,requested_qty,issued_qty,asset_MDM_code from smrsitems where id = " + request.faultyItems[i].mrs_item_ID + "";
                        DataTable dt_chk_faulty = await Context.FetchData(chkSrNoAvailableQuery_faulty).ConfigureAwait(false);

                        if (dt_chk_faulty.Rows.Count > 0)
                        {
                            asset_item_ID = request.faultyItems[i].assetMasterItemID;
                            qty = (dt_chk_faulty.Rows[0]["available_qty"].ToInt());
                            requested_qty = (dt_chk_faulty.Rows[0]["requested_qty"].ToInt());
                            issued_qty = (dt_chk_faulty.Rows[0]["issued_qty"].ToInt());
                            asset_MDM_code = (dt_chk_faulty.Rows[0]["asset_MDM_code"].ToString());

                        }

                        string insertStmt = $"START TRANSACTION; " +
                            $"Update smmrs SET status = {(int)CMMS.CMMS_Status.MRS_SUBMITTED} where id = {request.mrsreturnID} ;INSERT INTO smrsitems (mrs_ID,mrs_return_ID,asset_item_ID,available_qty,requested_qty,returned_qty,return_remarks,flag, is_faulty,is_splited,issued_qty,serial_number,asset_MDM_code)" +
                            $"VALUES ({request.mrsreturnID},{MRS_ReturnID},{asset_item_ID},{qty}, {requested_qty}, {request.faultyItems[i].returned_qty}, '{request.faultyItems[i].return_remarks}', 2, 1,1,{issued_qty},'{request.faultyItems[i].serial_number}','{asset_MDM_code}')" +
                            $"; SELECT LAST_INSERT_ID(); COMMIT;";
                        DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);

                        var tResult = await TransactionDetails(request.facility_ID, from_actor_id, (int)CMMS.SM_Actor_Types.Inventory, to_actor_id, (int)CMMS.SM_Actor_Types.NonOperational, asset_item_ID, Convert.ToInt32(request.faultyItems[i].returned_qty), (int)CMMS.CMMS_Modules.SM_MRS_RETURN, request.mrsreturnID, request.faultyItems[i].return_remarks, 0);
                    }
                }
            }

            if (!Queryflag)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to submit MRS return.");
            }
            else
            {
                response = new CMDefaultResponse(MRS_ReturnID, CMMS.RETRUNSTATUS.SUCCESS, "MRS return submitted.");
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MRS, MRS_ReturnID, 0, 0, "MRS return submitted.", CMMS.CMMS_Status.MRS_SUBMITTED);

            return response;
        }

        internal async Task<CMDefaultResponse> UpdateReturnMRS(CMMRS request, int UserID)
        {
            CMDefaultResponse response = null;
            bool Queryflag = false;
            int MRS_ReturnID = 0;
            var flag = "MRS_RETURN_REQUEST";

            // Updating existing MRS;

            var lastMRSID = request.ID;
            var refType = "MRSReturnEdit";
            var mailSub = "MRS Return Request Updated.";
            string updatestmt = $" START TRANSACTION; UPDATE smmrs SET facility_ID = {request.facility_ID}, requested_by_emp_ID = {UserID}, requested_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
                $" activity='{request.activity}',whereUsedType={request.whereUsedType},whereUsedRefID={request.whereUsedRefID},from_actor_type_id={request.from_actor_type_id},from_actor_id={request.from_actor_id},to_actor_type_id={request.to_actor_type_id},to_actor_id={request.to_actor_id},remarks='{request.remarks}', status= {(int)CMMS.CMMS_Status.MRS_SUBMITTED} WHERE ID = {request.ID}" +
                $" ; COMMIT;";
            try
            {
                await Context.ExecuteNonQry<int>(updatestmt);
            }
            catch (Exception ex)
            {
                Queryflag = false;
                throw ex;
            }
            MRS_ReturnID = request.ID;

            string commaSeparatedIds = "";
            if (request.cmmrsItems != null)
            {
                commaSeparatedIds = string.Join(",", request.cmmrsItems.Select(item => item.mrs_item_id.ToString()));
            }
            if (request.faultyItems != null)
            {
                if (commaSeparatedIds.Length > 1)
                {
                    commaSeparatedIds = commaSeparatedIds + ",";
                }
                commaSeparatedIds = commaSeparatedIds + string.Join(",", request.faultyItems.Select(item => item.mrs_item_ID.ToString()));
            }


            string deleteQueryForItems = $" delete from smrsitems where mrs_return_ID = {request.ID} and id not in ({commaSeparatedIds});  ";
            await Context.ExecuteNonQry<int>(deleteQueryForItems).ConfigureAwait(false);

            if (request.cmmrsItems != null)
            {


                for (var i = 0; i < request.cmmrsItems.Count; i++)
                {
                    int equipmentID = request.cmmrsItems[i].asset_item_ID;
                    decimal quantity = request.cmmrsItems[i].qty;

                    try
                    {
                        // Construct the SQL UPDATE statement for smrsitem
                        string updateStmt = $"START TRANSACTION; " +
                            $"UPDATE smrsitems " +
                            $"SET " +

                            $"returned_qty = {request.cmmrsItems[i].returned_qty}, " +

                            $"return_remarks = '{request.cmmrsItems[i].return_remarks}' " +
                            $"WHERE ID = {request.cmmrsItems[i].mrs_item_id} ; " +
                            "COMMIT;";

                        await Context.ExecuteNonQry<int>(updateStmt).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Queryflag = false;

                    }

                    Queryflag = true;
                }

                for (var i = 0; i < request.faultyItems.Count; i++)
                {
                    try
                    {
                        // Construct the SQL UPDATE statement for smrsitem
                        if (request.faultyItems[i].mrs_item_ID > 0)
                        {
                            string updateStmt = $"START TRANSACTION; " +
                          $"UPDATE smrsitems " +
                          $"SET  " +
                          $"returned_qty = {request.faultyItems[i].returned_qty}, " +
                          $"serial_number = '{request.faultyItems[i].serial_number}', " +
                          $"return_remarks = '{request.faultyItems[i].return_remarks}' " +
                          $"WHERE ID = {request.faultyItems[i].mrs_item_ID} ; " +
                          "COMMIT;";

                            await Context.ExecuteNonQry<int>(updateStmt).ConfigureAwait(false);
                        }
                        else
                        {
                            string insertStmt = $"START TRANSACTION; " +
                            $"INSERT INTO smrsitems (mrs_return_ID,mrs_ID,asset_item_ID,available_qty,requested_qty,returned_qty,return_remarks,flag, is_faulty,is_splited,issued_qty,serial_number)" +
                            $"VALUES ({request.ID},0,{request.faultyItems[i].assetMasterItemID},0, {request.faultyItems[i].assetMasterItemID}, {request.faultyItems[i].returned_qty}, '{request.faultyItems[i].return_remarks}', 2, 1,1,0,'{request.faultyItems[i].serial_number}')" +
                            $"; SELECT LAST_INSERT_ID(); COMMIT;";
                            DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                        }

                    }
                    //try
                    //{
                    //    if (request.faultyItems[i].mrs_item_ID != null)
                    //    {
                    //        // Construct the SQL UPDATE statement for smrsitem
                    //        string updateStmt = $"START TRANSACTION; " +
                    //            $"UPDATE smrsitems " +
                    //            $"SET  " +
                    //            $"returned_qty = {request.faultyItems[i].returned_qty}, " +
                    //            $"serial_number = '{request.faultyItems[i].sr_no}', " +
                    //            $"return_remarks = '{request.faultyItems[i].return_remarks}' " +
                    //            $"WHERE ID = {request.faultyItems[i].mrs_item_ID} ; " +
                    //            "COMMIT;";
                    //        await Context.ExecuteNonQry<int>(updateStmt).ConfigureAwait(false);
                    //    }
                    //else
                    //{
                    //    string insertStmt = $"START TRANSACTION; " +
                    //$"INSERT INTO smrsitems (mrs_ID,mrs_return_ID,asset_item_ID,available_qty,requested_qty,returned_qty,return_remarks,flag,serial_number ,is_faulty,is_splited,issued_qty)" +
                    //$"VALUES ({request.ID},{request.ID},{request.faultyItems[i].assetMasterItemID},{request.cmmrsItems[i].qty}, {request.cmmrsItems[i].requested_qty}, {request.faultyItems[i].returned_qty}, '{request.faultyItems[i].return_remarks}', 2,'{request.faultyItems[0].sr_no}',{request.cmmrsItems[i].is_faulty},1,{request.cmmrsItems[i].issued_qty})" +
                    //$"; SELECT LAST_INSERT_ID(); COMMIT;";
                    //    DataTable dt = await Context.FetchData(insertStmt).ConfigureAwait(false);
                    //    int id = Convert.ToInt32(dt.Rows[0][0]);
                    //}

                    catch (Exception ex)
                    {
                        Queryflag = false;

                    }

                    Queryflag = true;
                }

            }
            if (!Queryflag)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to submit MRS return.");
            }
            else
            {
                response = new CMDefaultResponse(MRS_ReturnID, CMMS.RETRUNSTATUS.SUCCESS, "MRS return submitted.");
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MRS, MRS_ReturnID, 0, 0, "MRS return submitted.", CMMS.CMMS_Status.MRS_SUBMITTED);

            return response;
        }
        internal async Task<CMDefaultResponse> ApproveMRSReturn(CMApproval request, int UserID)
        {
            CMDefaultResponse response = null;
            var executeUpdateStmt = 0;
            bool Queryflag = false;
            string comment = "";

            string stmtSelect = "SELECT ID,facility_ID,requested_by_emp_ID, reference,from_actor_id,from_actor_type_id,to_actor_id,to_actor_type_id FROM smmrs WHERE ID = " + request.id + "";
            List<CMMRS> mrsList = await Context.GetData<CMMRS>(stmtSelect).ConfigureAwait(false);
            //for (int i = 0; i < request.cmmrsItems.Count; i++)
            //{
            //    decimal req_qty = request.cmmrsItems[i].returned_qty;
            //    //MRS_REQUEST_REJECT contains 2 as enum value
            //    if (request.approval_status ==   (int)CMMS.CMMS_Status.MRS_REQUEST_REJECTED)
            //    {
            //        req_qty = 0;
            //    }
            //    string stmt = "UPDATE smrsitems SET returned_qty = " + request.cmmrsItems[i].returned_qty + ", finalRemark = '" + request.cmmrsItems[i].return_remarks + "' WHERE ID = " + request.cmmrsItems[i].id;  
            //    try
            //    {
            //      executeUpdateStmt = await Context.ExecuteNonQry<int>(stmt);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //    if (executeUpdateStmt == null || executeUpdateStmt == 0)
            //    {
            //        return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to update MRS item return.");
            //    }
            //    Queryflag = true;

            //    // MRS_REQUEST_APPROVE == 1 in constant.cs file
            //    if (request.approval_status == (int)CMMS.CMMS_Status.MRS_REQUEST_APPROVED)
            //    {
            //        var tResult = await TransactionDetails(mrsList[0].facility_ID, mrsList[0].requested_by_emp_ID, 2, mrsList[0].facility_ID, 2, request.cmmrsItems[0].id, Convert.ToInt32(request.cmmrsItems[0].received_qty), Convert.ToInt32(mrsList[0].reference), request.ID, request.return_remarks, request.mrs_return_ID);
            //        if (!tResult)
            //        {
            //            return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Transaction details failed.");                        
            //        }
            //    }
            //}

            comment = "MRS Return Approved";



            var stmtUpdate = $"UPDATE smmrs SET approved_by_emp_ID = {UserID}, approved_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
            $"approval_status = {(int)CMMS.CMMS_Status.MRS_REQUEST_APPROVED},status = {(int)CMMS.CMMS_Status.MRS_REQUEST_APPROVED}, approval_comment = '{request.comment}'" +
            $" WHERE ID = {request.id}";
            try
            {
                executeUpdateStmt = await Context.ExecuteNonQry<int>(stmtUpdate);
            }
            catch (Exception ex)
            {
                return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "MRS Item update details failed.");
            }
            if (executeUpdateStmt == 0 || executeUpdateStmt == null)
            {
                return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "MRS Item update details failed.");
            }


            string stmtSelectItems = "SELECT * FROM smrsitems WHERE mrs_return_ID = " + request.id + "";
            List<CMMRSItems> mrsItemList = await Context.GetData<CMMRSItems>(stmtSelectItems).ConfigureAwait(false);


            for (var i = 0; i < mrsItemList.Count(); i++)
            {
                var tResult = await TransactionDetails(mrsList[0].facility_ID, mrsList[0].from_actor_id, mrsList[0].from_actor_type_id, mrsList[0].to_actor_id, mrsList[0].to_actor_type_id, mrsItemList[i].asset_item_ID, Convert.ToInt32(mrsItemList[i].returned_qty), (int)CMMS.CMMS_Modules.SM_MRS_RETURN, request.id, request.comment, mrsList[0].ID);
                if (!tResult)
                {
                    return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Transaction details failed.");
                }
            }
            string msg = "Equipment returned to store.";
            response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, msg);
            return response;
        }

        internal async Task<CMDefaultResponse> RejectMRSReturn(CMApproval request, int UserID)
        {
            CMDefaultResponse response = null;
            var executeUpdateStmt = 0;
            bool Queryflag = false;
            string comment = "";

            //string stmtSelect = "SELECT * FROM smmrs WHERE ID = " + request.id + "";
            //List<CMMRS> mrsList = await Context.GetData<CMMRS>(stmtSelect).ConfigureAwait(false);
            //for (int i = 0; i < request.cmmrsItems.Count; i++)
            //{
            //    decimal req_qty = request.cmmrsItems[i].returned_qty;                
            //    req_qty = 0;
            //    string stmt = "UPDATE smrsitems SET returned_qty = " + request.cmmrsItems[i].returned_qty + ", finalRemark = '" + request.cmmrsItems[i].return_remarks + "' WHERE ID = " + request.cmmrsItems[i].id;
            //    try
            //    {
            //        executeUpdateStmt = await Context.ExecuteNonQry<int>(stmt);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //    if (executeUpdateStmt == null || executeUpdateStmt == 0)
            //    {
            //        return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to update MRS item return.");
            //    }
            //    Queryflag = true;

            //    // MRS_REQUEST_APPROVE == 1 in constant.cs file

            //}


            comment = "MRS Return Rejected. Reason : " + request.comment + "";



            var stmtUpdate = $"UPDATE smmrs SET rejected_by_emp_ID = {UserID}, rejected_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
            $"status = {(int)CMMS.CMMS_Status.MRS_REQUEST_REJECTED}, approval_comment = '{request.comment}'" +
            $" WHERE ID = {request.id}";
            try
            {
                executeUpdateStmt = await Context.ExecuteNonQry<int>(stmtUpdate);
            }
            catch (Exception ex)
            {
                return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "MRS Item update details failed.");
            }
            if (executeUpdateStmt == 0 || executeUpdateStmt == null)
            {
                return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "MRS Item update details failed.");
            }

            response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, comment);
            return response;
        }
        internal async Task<CMMRSAssetTypeList> getAssetTypeByItemID(int ItemID)
        {
            string stmt = "SELECT sat.asset_type,sam.asset_code,sam.asset_name,sat.ID,sai.ID as item_ID,sai.facility_ID,COALESCE(sai.serial_number,'') serial_number,sam.asset_type_ID,sm.decimal_status,COALESCE(file.file_path,'') as file_path,file.Asset_master_id, f_sum.spare_multi_selection FROM smassetitems sai " +
                     "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code " +
                     "LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement " +
                     "LEFT JOIN smassetmasterfiles file ON file.Asset_master_id = sam.ID " +
                     "LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID " +
                     "LEFT JOIN smunitmeasurement f_sum ON f_sum.ID = sam.unit_of_measurement " +
                     "WHERE(sai.ID = " + ItemID + " OR sai.serial_number = " + ItemID + " OR  sai.asset_code = " + ItemID + ")";
            List<CMMRSAssetTypeList> _List = await Context.GetData<CMMRSAssetTypeList>(stmt).ConfigureAwait(false);
            var isMultiSpareSelectionStatus = getMultiSpareSelectionStatus("", ItemID);

            if (_List[0].asset_type_ID == 2 || (_List[0].asset_type_ID == 3 && Convert.ToInt32(isMultiSpareSelectionStatus) == 0))
            {
                _List[0].available_qty = await GetAvailableQty(_List[0].item_ID, _List[0].facility_ID);
            }
            else
            {
                _List[0].available_qty = await GetAvailableQtyByCode(_List[0].Asset_master_id.ToString(), _List[0].facility_ID);
            }

            return _List[0];
        }

        public async Task<int> GetAvailableQty(int assetItemID, int plantID)
        {
            // actor Type 2 : Store
            string actorType = "2";
            string stmt = "SELECT SUM(debitQty) as drQty, SUM(creditQty) as crQty FROM smtransition WHERE assetItemID = " + assetItemID.ToString() + " AND actorType = '" + actorType + "' AND transactionID IN (SELECT ID FROM smtransactiondetails WHERE plantID = " + plantID.ToString() + ")";
            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
            int crQty = 0, drQty = 0;
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                crQty = (dt2.Rows[0]["crQty"].ToInt());
                drQty = (dt2.Rows[0]["drQty"].ToInt());
            }
            int availableQty = crQty - drQty;
            return availableQty;
        }
        public async Task<int> GetAvailableQtyByCode(string assetMasterIDs, int plantID)
        {
            // actor Type 2 : Store
            int actorType = (int)CMMS.SM_Actor_Types.Store;
            string stmt = "SELECT SUM(debitQty) as drQty, SUM(creditQty) as crQty FROM  smtransition WHERE assetItemID IN (" + assetMasterIDs + ") AND actorType = '" + actorType + "' AND transactionID IN (SELECT ID FROM smtransactiondetails WHERE plantID = " + plantID.ToString() + ")";
            DataTable dt2 = await Context.FetchData(stmt).ConfigureAwait(false);
            int crQty = 0, drQty = 0;
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                crQty = dt2.Rows[0]["crQty"] == "" || dt2.Rows[0]["crQty"] == DBNull.Value ? 0 : Convert.ToInt32(dt2.Rows[0]["crQty"]);
                drQty = dt2.Rows[0]["drQty"] == "" || dt2.Rows[0]["drQty"] == DBNull.Value ? 0 : Convert.ToInt32(dt2.Rows[0]["drQty"]);
            }
            int availableQty = crQty - drQty;
            return availableQty;
        }

        public async Task<CMMRS> getLastTemplateData(int ID)
        {
            string sqlstmt = $" SELECT mri.asset_item_ID,  mri.requested_qty, mr.templateName FROM smrsitems mri" +
                              $" LEFT JOIN smmrs mr ON mr.ID = mri.mrs_ID  " +
                              $"WHERE mr.ID = {ID};";
            List<CMMRS> mrsItem = await Context.GetData<CMMRS>(sqlstmt).ConfigureAwait(false);
            return mrsItem[0];
        }

        public async Task<List<CMAssetItem>> GetAssetItems(int facility_ID, bool isGroupByCode = false)
        {
            //spare 

            string spareAssetIds = "SELECT sai.ID FROM smassetitems as sai JOIN smassetmasters as sam ON sai.asset_code = sam.asset_code " +
                "JOIN smunitmeasurement as f_sum ON f_sum.ID = sam.unit_of_measurement WHERE f_sum.spare_multi_selection = 0 " +
                "AND sam.asset_type_ID =2 AND sai.facility_ID = " + facility_ID + ";";
            DataTable dtSA = await Context.FetchData(spareAssetIds).ConfigureAwait(false);
            string spareAssetIdsString = "";
            if (dtSA.Rows.Count > 0)
            {
                IEnumerable<string> columnValues = dtSA.AsEnumerable().Select(row => row.Field<int>("ID").ToString());

                spareAssetIdsString = string.Join(",", columnValues);
            }



            //var_dump(spareAssetIds);

            var stmt = @"SELECT sm.asset_type_ID, sm.ID as asset_ID, sm.asset_code, sic.cat_name, sat.serial_number, sat.ID, sm.asset_name, st.asset_type, if(sm.approval_required = 1, 'Yes', 'NO') as approval_required, COALESCE(file.file_path, '') as file_path, file.Asset_master_id, f_sum.spare_multi_selection,if(sm.approval_required = 1, 1, 0) as approval_required,
                        sat.materialID
                        FROM smassetitems sat
                        LEFT JOIN smassetmasters sm ON sm.id = sat.assetMasterID
                        LEFT JOIN smassettypes st ON st.ID = sm.asset_type_ID
                        LEFT JOIN smassetmasterfiles file ON file.Asset_master_id = sm.ID
                        LEFT JOIN smitemcategory sic ON sic.ID = sm.item_category_ID
                        LEFT JOIN smunitmeasurement f_sum ON f_sum.ID = sm.unit_of_measurement
                        WHERE sat.facility_ID = " + facility_ID + " AND sm.asset_type_ID is not null  ";



            if (!spareAssetIdsString.IsNullOrEmpty())
            {
                stmt += "AND sat.ID NOT IN(" + spareAssetIdsString + ")";
            }

            if (isGroupByCode)
            {
                stmt += "GROUP BY sm.asset_code";
            }
            else
            {
                stmt += "GROUP BY asset_ID";
            }

            if (!spareAssetIdsString.IsNullOrEmpty())
            {
                stmt += "  UNION ALL SELECT sm.asset_type_ID, sm.ID as asset_ID, sm.asset_code, sic.cat_name, sat.serial_number, sat.ID, sm.asset_name, st.asset_type, if(sm.approval_required = 1, 'Yes', 'NO') as approval_required, COALESCE(file.file_path, '') as file_path, file.Asset_master_id, f_sum.spare_multi_selection, if(sm.approval_required = 1, 1, 0) as is_approval_required, sat.materialID"
                + " FROM smassetitems sat"
                + " LEFT JOIN smassetmasters sm ON sm.id = sat.assetMasterID"
                + " LEFT JOIN smassettypes st ON st.ID = sm.asset_type_ID"
                + " LEFT JOIN smassetmasterfiles file ON file.Asset_master_id = sm.ID"
                + " LEFT JOIN smitemcategory sic ON sic.ID = sm.item_category_ID"
                + " LEFT JOIN smunitmeasurement f_sum ON f_sum.ID = sm.unit_of_measurement"
                + " WHERE sat.facility_ID = " + facility_ID + " AND sat.ID IN(" + spareAssetIdsString + ") and sm.asset_type_ID is not null";
            }

            //echo $stmt;
            List<CMAssetItem> Listitem = await Context.GetData<CMAssetItem>(stmt).ConfigureAwait(false);

            for (int i = 0; i < Listitem.Count; i++)
            {
                Listitem[i].available_qty = await GetAvailableQtyByCode(Listitem[i].asset_ID.ToString(), facility_ID);
            }

            return Listitem;
        }

        private static string getLongStatus(CMMS.CMMS_Status m_notificationID, int Id)
        {
            CMMS.CMMS_Status status = (CMMS.CMMS_Status)m_notificationID;
            string retValue = "";
            switch (status)
            {

                case CMMS.CMMS_Status.MRS_SUBMITTED:
                    retValue = $"MRS {Id} Submitted.";
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_REJECTED:
                    retValue = $"MRS {Id} Request Rejected";
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_APPROVED:
                    retValue = $"MRS {Id} Request Approved";
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED:
                    retValue = $"MRS {Id} Request Issued";
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED_REJECTED:
                    retValue = $"MRS {Id} Request Issued Rejected";
                    break;
                case CMMS.CMMS_Status.MRS_REQUEST_ISSUED_APPROVED:
                    retValue = $"MRS {Id} Request Issued Approved";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;
        }

        internal async Task<CMDefaultResponse> MRSIssue(CMMRS request, int UserID)
        {
            CMDefaultResponse response = null;
            bool Queryflag = false;
            string stmtSelect = "SELECT * FROM smmrs WHERE ID = " + request.ID + "";
            List<CMMRS> mrsList = await Context.GetData<CMMRS>(stmtSelect).ConfigureAwait(false);

            if (mrsList.Count > 0)
            {
                string updatestmt = $" START TRANSACTION; UPDATE smmrs SET status={(int)CMMS.CMMS_Status.MRS_REQUEST_ISSUED}, issue_comment = {request.facility_ID}, issued_by_emp_ID = {UserID}, issuedAt = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}' " +
                       $"  WHERE ID = {request.ID}" +
                       $";COMMIT;";
                try
                {
                    Queryflag = true;
                    await Context.ExecuteNonQry<int>(updatestmt);
                }
                catch (Exception ex)
                {
                    Queryflag = false;
                    throw ex;
                }

                if (request.cmmrsItems != null)
                {
                    for (var i = 0; i < request.cmmrsItems.Count; i++)
                    {

                        string getassetitemIDQ = $"select id from smassetitems where serial_number = '{request.cmmrsItems[i].serial_number}'";

                        string updateStmtForItemList = $"update smrsitems set serial_number= '{request.cmmrsItems[i].serial_number}', issued_qty = {request.cmmrsItems[i].issued_qty}, issue_remarks = '{request.cmmrsItems[i].issue_remarks}' where ID = {request.cmmrsItems[i].mrs_item_id};";


                        DataTable dt = await Context.FetchData(getassetitemIDQ).ConfigureAwait(false);
                        int smassetitemsID = 0;
                        if (dt.Rows.Count > 0)
                        {
                            smassetitemsID = Convert.ToInt32(dt.Rows[0]["id"]);
                        }
                        updateStmtForItemList = updateStmtForItemList + $" update smassetitems set  item_issued_mrs = {request.ID} where id = " + smassetitemsID + ";";
                        try
                        {
                            Queryflag = true;
                            await Context.ExecuteNonQry<int>(updateStmtForItemList);
                        }
                        catch (Exception ex)
                        {
                            Queryflag = false;
                            throw ex;
                        }
                    }
                }

                if (!Queryflag)
                {
                    response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to submit MRS return.");
                }
                else
                {
                    response = new CMDefaultResponse(request.ID, CMMS.RETRUNSTATUS.SUCCESS, "MRS Request Issued.");
                }
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MRS, request.ID, 0, 0, request.approval_comment, CMMS.CMMS_Status.MRS_REQUEST_ISSUED);

                for (var i = 0; i < request.cmmrsItems.Count; i++)
                {

                    var tResult = await TransactionDetails(mrsList[0].facility_ID, mrsList[0].from_actor_id, mrsList[0].from_actor_type_id, mrsList[0].to_actor_id, mrsList[0].to_actor_type_id, Convert.ToInt32(request.cmmrsItems[i].asset_item_ID), Convert.ToInt32(request.cmmrsItems[i].issued_qty), Convert.ToInt32(CMMS.CMMS_Modules.SM_MRS), request.ID, "", request.ID);
                    if (!tResult)
                    {
                        return new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Transaction details failed.");
                    }
                }
            }
            else
            {
                response = new CMDefaultResponse(request.ID, CMMS.RETRUNSTATUS.FAILURE, "Invalid MRS Request Id sent.");
            }

            return response;
        }

        internal async Task<CMDefaultResponse> ApproveMRSIssue(CMApproval request, int userId)
        {
            CMDefaultResponse response = null;
            string stmtSelect = $"SELECT ID FROM smmrs WHERE ID = {request.id}";
            List<CMMRS> mrsList = await Context.GetData<CMMRS>(stmtSelect).ConfigureAwait(false);

            if (mrsList.Count > 0)
            {
                string stmt = $"UPDATE smmrs SET issue_approved_by_emp_ID = {userId}, issue_approved_date='{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
                                   $" status ={(int)CMMS.CMMS_Status.MRS_REQUEST_ISSUED_APPROVED} , issue_approval_comment = '{request.comment}' WHERE ID = {request.id}";
                await Context.ExecuteNonQry<int>(stmt);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Status updated.");
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Invalid MRS Id passed.");
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MRS, request.id, 0, 0, "MRS Request Issued Approved.", CMMS.CMMS_Status.MRS_REQUEST_ISSUED_APPROVED);

            return response;
        }
        internal async Task<CMDefaultResponse> RejectMRSIssue(CMApproval request, int userId)
        {
            CMDefaultResponse response = null;
            string stmtSelect = $"SELECT ID FROM smmrs WHERE ID = {request.id}";
            List<CMMRS> mrsList = await Context.GetData<CMMRS>(stmtSelect).ConfigureAwait(false);

            if (mrsList.Count > 0)
            {
                string stmt = $"UPDATE smmrs SET issue_rejected_by_emp_ID = {userId}, issue_rejected_date='{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
                                   $" status ={(int)CMMS.CMMS_Status.MRS_REQUEST_ISSUED_REJECTED} , issue_rejected_comment = '{request.comment}' WHERE ID = {request.id}";
                await Context.ExecuteNonQry<int>(stmt);
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Status updated.");
            }
            else
            {
                response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, "Invalid MRS Id Passed.");
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MRS, request.id, 0, 0, "MRS Request Issued rejected.", CMMS.CMMS_Status.MRS_REQUEST_ISSUED_REJECTED);

            return response;
        }

        internal async Task<List<CMMRSList>> GetMRSReturnList(int facility_ID, bool self_view, int userID, string facilitytimeZone)
        {

            string stmt = "SELECT sm.ID,sm.requested_by_emp_ID as requested_by_emp_ID,CONCAT(ed1.firstName,' ',ed1.lastName) as approver_name," +
                           "DATE_FORMAT(sm.returnDate,'%Y-%m-%d %H:%i') as returnDate,if(sm.approval_status != '',DATE_FORMAT(sm.approved_date,'%Y-%m-%d %H:%i'),'') as approval_date,sm.approval_status," +
                           "sm.approval_comment,CONCAT(ed.firstName,' ',ed.lastName) as requested_by_name, sm.status, sm.activity, sm.whereUsedType, sm.whereUsedRefID, COALESCE(sm.remarks,'') as  remarks," +
                           "case when sm.whereUsedType = 1 then 'Job' when sm.whereUsedType = 2 then 'PM' when sm.whereUsedType = 4 then 'JOBCARD' when sm.whereUsedType = 27 then 'PMTASK' else 'Invalid' end as whereUsedTypeName " +
                           "FROM smmrs sm LEFT JOIN users ed ON ed.id = sm.requested_by_emp_ID LEFT JOIN users ed1 ON ed1.id = sm.approved_by_emp_ID ";

            if (facility_ID > 0)
            {
                stmt += $" WHERE sm.facility_ID = " + facility_ID + "  and  sm.flag = 2 ";


                if (self_view)
                    stmt += $" AND ( sm.requested_by_emp_ID = {userID}) ";
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            List<CMMRSList> _List = await Context.GetData<CMMRSList>(stmt).ConfigureAwait(false);
            /* foreach (var list in _List)
             {
                 if (list!= null && list.approval_date!=null && list.approval_date!="" &&list.approval_date!="0000-00-00")
                 {
                     DateTime approval_date = DateTime.Parse(list.approval_date);
                     approval_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, approval_date);
                     list.approval_date = approval_date.ToString();
                 }
                 if (list!=null && list.issued_date!=null && list.issued_date != "" && list.issued_date!="0000-00-00")
                 {
                     DateTime issued_date = DateTime.Parse(list.issued_date);
                     issued_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, issued_date);
                     list.issued_date = issued_date.ToString();
                 }
                 if (list!=null && list.returnDate!=null && list.returnDate != "" && list.returnDate!="0000-00-00")
                 {
                     DateTime returnDate = DateTime.Parse(list.returnDate);
                     returnDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, returnDate);
                     list.issued_date = returnDate.ToString();
                 }
             }*/
            for (var i = 0; i < _List.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_MRS, _Status);
                string _status_long = getLongStatus(_Status, _List[i].ID);
                _List[i].status_short = _shortStatus;
                _List[i].status_long = _status_long;
                _List[i].CMMRSItems = await getMRSReturnItems(_List[i].ID, facilitytimeZone);
            }

            return _List;
        }

        internal async Task<List<CMMRSItems>> getMRSReturnItems(int ID, string facilitytimeZone)
        {
            /* string stmt = "SELECT smi.ID,smi.return_remarks,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID,smi.asset_MDM_code," +
                 "t1.serial_number,smi.returned_qty,smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,sm.flag as status,DATE_FORMAT(sm.returnDate,'%Y-%m-%d %H:%i') as returnDate," +
                 "sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d %H:%i') as approved_date,DATE_FORMAT(sm.requested_date,'%Y-%m-%d %H:%i') as issued_date," +
                 "DATE_FORMAT(sm.returnDate, '%Y-%m-%d %H:%i') as returnDate, smi.requested_qty,if(smi.approval_required = 1,'Yes','No') as approval_required,\r\n " +
                 "t1.asset_name,t1.asset_type_ID,t1.asset_type,COALESCE(t1.file_path,'') as file_path,t1.Asset_master_id\r\n        FROM smrsitems smi\r\n " +
                 " LEFT JOIN smmrs sm ON sm.ID = smi.mrs_return_ID         \r\n        LEFT JOIN (SELECT sai.ID as asset_item_ID, sai.serial_number, sam.asset_name, " +
                 "sam.asset_type_ID,sat.asset_type,COALESCE(file.file_path,'') as file_path,file.Asset_master_id\r\n        FROM smassetitems sai  " +
                 "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code LEFT JOIN smassetmasterfiles  file ON file.Asset_master_id =  sam.ID " +
                 "LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID) as t1 ON t1.asset_item_ID = smi.asset_item_ID" +
                 "  WHERE smi.mrs_return_ID = " + ID + " /*GROUP BY smi.ID*/

            string stmt = "SELECT smi.ID,smi.ID as mrs_item_id,smi.return_remarks,smi.mrs_return_ID,smi.finalRemark,smi.asset_item_ID,smi.asset_MDM_code as asset_code," +
           "smi.serial_number,smi.is_faulty as is_faulty,smi.returned_qty,smi.available_qty,smi.used_qty,smi.ID,smi.issued_qty,sm.flag as status,DATE_FORMAT(sm.returnDate,'%Y-%m-%d %H:%i') as returnDate," +
           "sm.approval_status,DATE_FORMAT(sm.approved_date,'%Y-%m-%d %H:%i') as approved_date,DATE_FORMAT(sm.requested_date,'%Y-%m-%d %H:%i') as issued_date," +
           "DATE_FORMAT(sm.returnDate, '%Y-%m-%d %H:%i') as returnDate, smi.requested_qty,if(smi.approval_required = 1,'Yes','No') as approval_required,\r\n " +
           "t1.asset_name,t1.asset_type_ID,t1.asset_type,COALESCE(t1.file_path,'') as file_path,t1.Asset_master_id\r\n        FROM smrsitems smi\r\n " +
           " LEFT JOIN smmrs sm ON sm.ID = smi.mrs_return_ID   \r\n   LEFT JOIN (SELECT distinct sai.assetMasterID as asset_item_ID,  sam.asset_name, " +
           "sam.asset_type_ID,sat.asset_type,COALESCE(file.file_path,'') as file_path,file.Asset_master_id\r\n  FROM smassetitems sai " +
           "LEFT JOIN smassetmasters sam ON sam.ID = sai.assetMasterID LEFT JOIN smassetmasterfiles  file ON file.Asset_master_id = sam.ID " +
           "LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID)  as t1 ON t1.asset_item_ID = smi.asset_item_ID " +
           " WHERE smi.mrs_return_ID = " + ID + "/*GROUP BY smi.ID*/";


            List<CMMRSItems> _List = await Context.GetData<CMMRSItems>(stmt).ConfigureAwait(false);
            /*  foreach (var list in _List)
              {
                  if (list != null && list.approved_date != null && list.approved_date!="" && list.approved_date!="0000-00-00")
                  {
                      DateTime approved_date = DateTime.Parse(list.approved_date);
                      approved_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, approved_date);
                      list.approved_date = approved_date.ToString();
                  }
                  if (list != null && list.issued_date != null && list.issued_date != "")
                  {
                      DateTime issued_date = DateTime.Parse(list.issued_date);
                      issued_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, issued_date);
                      list.approved_date = issued_date.ToString();
                  }
              }*/
            for (var i = 0; i < _List.Count; i++)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.SM_MRS, _Status);
                string _status_long = getLongStatus(_Status, _List[i].ID);
                _List[i].status_short = _shortStatus;
                _List[i].status_long = _status_long;
            }
            return _List;
        }

        internal async Task<CMIssuedAssetItems> getIssuedAssetItems(int ID)
        {
            string stmt = "SELECT item_issued_mrs, assetMasterID,materialID,sat.asset_type,sam.asset_name,sai.facility_ID, " +
                "COALESCE(sai.serial_number,'') serial_number,sam.asset_type_ID, f_sum.spare_multi_selection " +
                "FROM smassetitems sai  " +
                "LEFT JOIN smassetmasters sam ON sam.asset_code = sai.asset_code  " +
                "LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement  " +
                "LEFT JOIN smassettypes sat ON sat.ID = sam.asset_type_ID  " +
                "LEFT JOIN smunitmeasurement f_sum ON f_sum.ID = sam.unit_of_measurement  " +
                "WHERE sai.assetMasterID = " + ID + " and item_issued_mrs = 0;";
            List<CMIssuedItems> _List = await Context.GetData<CMIssuedItems>(stmt).ConfigureAwait(false);
            CMIssuedAssetItems _MasterList = _List.Select(p => new CMIssuedAssetItems
            {
                facility_ID = p.facility_ID,
                assetMasterID = p.assetMasterID,
                asset_type_ID = p.asset_type_ID,
                asset_type = p.asset_type,
                asset_name = p.asset_name


            }).FirstOrDefault();
            if (_List.Count > 0)
            {
                List<CMIssuedAssetItemsWithSerialNo> _itemList = _List.Select(p => new CMIssuedAssetItemsWithSerialNo
                {
                    serial_number = p.serial_number,
                    materialID = p.materialID,
                }).ToList();
                _MasterList.CMIssuedAssetItemsWithSerialNo = _itemList;

            }

            return _MasterList;
        }

        public async Task<List<CMPlantStockOpeningResponse_MRSRetrun>> getMRSReturnStockItems(int mrs_id)
        {

            int facility_id = 0;
            int actorTypeID = 0;
            int actorID = 0;

            string mrs_dtls_query = "select facility_ID,from_actor_type_id,from_actor_id from smmrs where id = " + mrs_id + "; ";
            DataTable dt2 = await Context.FetchData(mrs_dtls_query).ConfigureAwait(false);
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                facility_id = (dt2.Rows[0]["facility_ID"].ToInt());
                actorTypeID = (dt2.Rows[0]["from_actor_type_id"].ToInt());
                actorID = (dt2.Rows[0]["from_actor_id"].ToInt());
            }
            List<string> Asset_Item_Ids = new List<string>();
            List<CMPlantStockOpening_MRSReturn> Asset_Item_Opening_Balance_details = new List<CMPlantStockOpening_MRSReturn>();
            List<CMPlantStockOpeningResponse_MRSRetrun> Response = new List<CMPlantStockOpeningResponse_MRSRetrun>();
            string itemCondition = "";
            string Plant_Stock_Opening_Details_query = "";

            //Plant_Stock_Opening_Details_query = $"select distinct smrsitems.asset_item_ID,smrsitems.serial_number as serial_no," +
            //    $" sm_trans.facilityID as facilityID, fc.name as facilityName,fc.isBlock as Facility_Is_Block,  '' as Facility_Is_Block_of_name, " +
            //    $"sm_trans.assetItemID, a_master.asset_name, a_master.asset_code, a_master.asset_type_ID, AST.asset_type, " +
            //    $"  smrsitems.available_qty,smrsitems.requested_qty, smrsitems.used_qty,smrsitems.issued_qty, smrsitems.approved_qty " +
            //    $"from smrsitems " +
            //    $" join smtransition as sm_trans  on sm_trans.assetItemID = smrsitems.asset_item_ID  " +
            //    $"  JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID " +
            //    $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID  " +
            //    $" Left join smassettypes AST on AST.id = a_master.asset_type_ID  " +
            //    $"where mrs_ID = {mrs_id} and is_splited = 1 order by sm_trans.id desc;";

            Plant_Stock_Opening_Details_query = $"select smrsitems.id mrs_item_id, smrsitems.asset_item_ID,smrsitems.serial_number, smmrs.facility_ID   as facilityID," +
                $" fc.name as facilityName, asset_item_ID assetItemID, a_master.asset_name, a_master.asset_code, a_master.asset_type_ID, AST.asset_type, " +
                $"  smrsitems.available_qty,smrsitems.requested_qty, smrsitems.used_qty,smrsitems.issued_qty, smrsitems.approved_qty " +
                $" from smrsitems   " +
                $" join smmrs on smmrs.id = smrsitems.mrs_ID" +
                $" JOIN smassetmasters as a_master ON a_master.ID = smrsitems.asset_item_ID  " +
                $" LEFT JOIN facilities fc ON fc.id = smmrs.facility_ID   " +
                $" Left join smassettypes AST on AST.id = a_master.asset_type_ID  " +
                $" where mrs_ID = {mrs_id} and is_splited = 1 " +
                $" order by  smmrs.id desc;";


            List<CMPlantStockOpening_MRSReturn> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMPlantStockOpening_MRSReturn>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);



            List<CMPlantStockOpeningItemWiseResponse_MRSReturn> itemWiseResponse = new List<CMPlantStockOpeningItemWiseResponse_MRSReturn>();
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {
                string facility_name = "";
                if (Convert.ToInt32(item.Facility_Is_Block) == 0)
                {
                    facility_name = Convert.ToString(item.facilityName);
                }
                else
                {
                    facility_name = Convert.ToString(item.Facility_Is_Block_of_name);
                }

                Asset_Item_Ids.Add(Convert.ToString(item.assetItemID));

                CMPlantStockOpening_MRSReturn openingBalance = new CMPlantStockOpening_MRSReturn();

                openingBalance.mrs_item_id = item.mrs_item_id;
                openingBalance.facilityID = item.facilityID;
                openingBalance.facilityName = facility_name;
                openingBalance.available_qty = item.available_qty;
                openingBalance.assetItemID = item.assetItemID;
                openingBalance.asset_name = item.asset_name;
                openingBalance.asset_code = item.asset_code;
                openingBalance.asset_type_ID = item.asset_type_ID;
                openingBalance.asset_type = item.asset_type;
                openingBalance.serial_number = item.serial_number;
                openingBalance.requested_qty = item.requested_qty;
                openingBalance.used_qty = item.used_qty;
                openingBalance.issued_qty = item.issued_qty;
                openingBalance.approved_qty = item.approved_qty;
                Asset_Item_Opening_Balance_details.Add(openingBalance);
            }


            var uniqueValues = Asset_Item_Opening_Balance_details.GroupBy(p => p.facilityID)
            .Select(g => g.First())
            .ToList();
            foreach (var item in uniqueValues)
            {
                CMPlantStockOpeningResponse_MRSRetrun cMPlantStockOpeningResponse = new CMPlantStockOpeningResponse_MRSRetrun();
                cMPlantStockOpeningResponse.facilityID = item.facilityID;
                cMPlantStockOpeningResponse.facilityName = item.facilityName;
                Response.Add(cMPlantStockOpeningResponse);
            }
            foreach (var item in Response)
            {
                List<CMPlantStockOpeningItemWiseResponse_MRSReturn> itemResponseList = new List<CMPlantStockOpeningItemWiseResponse_MRSReturn>();
                CMPlantStockOpeningResponse_MRSRetrun cMPlantStockOpeningResponse = new CMPlantStockOpeningResponse_MRSRetrun();
                cMPlantStockOpeningResponse.facilityID = item.facilityID;
                cMPlantStockOpeningResponse.facilityName = item.facilityName;
                var itemResponse = Asset_Item_Opening_Balance_details.Where(item => item.facilityID == item.facilityID).ToList();
                foreach (var itemDetail in itemResponse)
                {
                    CMPlantStockOpeningItemWiseResponse_MRSReturn itemWise = new CMPlantStockOpeningItemWiseResponse_MRSReturn();
                    itemWise.mrs_item_id = itemDetail.mrs_item_id;
                    itemWise.assetItemID = itemDetail.assetItemID;
                    itemWise.asset_name = itemDetail.asset_name;
                    itemWise.asset_code = itemDetail.asset_code;
                    itemWise.asset_type_ID = itemDetail.asset_type_ID;
                    itemWise.asset_type = itemDetail.asset_type;
                    itemWise.serial_number = itemDetail.serial_number;
                    itemWise.available_qty = itemDetail.available_qty;
                    itemWise.requested_qty = itemDetail.requested_qty;
                    itemWise.consumed_qty = itemDetail.used_qty;
                    itemWise.issued_qty = itemDetail.issued_qty;
                    itemWise.approved_qty = itemDetail.approved_qty;
                    itemResponseList.Add(itemWise);
                }
                item.stockDetails = itemResponseList;

            }
            return Response;

        }
        internal async Task<CMDefaultResponse> CreateReturnFaultyMRS(CMMRS request, int UserID)
        {
            CMDefaultResponse response = null;
            bool Queryflag = false;


            string updatestmt = $" START TRANSACTION; UPDATE smmrs SET status = {(int)CMMS.CMMS_Status.MRS_SUBMITTED}, facility_ID = {request.facility_ID}, requested_by_emp_ID = {UserID}, requested_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}'," +
                $" activity='{request.activity}',whereUsedType={request.whereUsedType},whereUsedRefID={request.whereUsedRefID},is_mrs_return=1 WHERE ID = {request.ID}" +
                $" ; DELETE FROM smrsitems WHERE mrs_ID =  {request.ID} ; COMMIT;";

            try
            {
                await Context.ExecuteNonQry<int>(updatestmt);
            }
            catch (Exception ex)
            {
                Queryflag = false;
                throw ex;
            }

            if (request.cmmrsItems != null)
            {
                for (var i = 0; i < request.cmmrsItems.Count; i++)
                {

                    int equipmentID = request.cmmrsItems[i].asset_item_ID;
                    decimal quantity = request.cmmrsItems[i].qty;


                    try
                    {
                        string insertStmt = $"START TRANSACTION; " +
                        $"INSERT INTO smrsitems (mrs_ID,mrs_return_ID,asset_item_ID,available_qty,requested_qty,returned_qty,return_remarks,flag, is_faulty,is_splited,issued_qty)" +
                        $"VALUES ({request.ID},{request.ID},{request.cmmrsItems[i].asset_item_ID},{request.cmmrsItems[i].qty}, {request.cmmrsItems[i].requested_qty}, {request.cmmrsItems[i].returned_qty}, '{request.cmmrsItems[i].return_remarks}', 2, {request.cmmrsItems[i].is_faulty},1,{request.cmmrsItems[i].issued_qty})" +
                        $"; SELECT LAST_INSERT_ID(); COMMIT;";
                        DataTable dt2 = await Context.FetchData(insertStmt).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Queryflag = false;
                        throw ex;
                    }


                    Queryflag = true;
                }
            }
            if (!Queryflag)
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, "Failed to submit MRS return.");
            }
            else
            {
                response = new CMDefaultResponse(request.ID, CMMS.RETRUNSTATUS.SUCCESS, "MRS return faluty submitted.");
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.SM_MRS, request.ID, 0, 0, "MRS return faluty submitted.", CMMS.CMMS_Status.MRS_SUBMITTED);

            return response;
        }
    }
}
