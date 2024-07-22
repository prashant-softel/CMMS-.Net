using CMMSAPIs.Helper;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace CMMSAPIs.Repositories.Calibration
{
    public class CalibrationRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public CalibrationRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
        }
        private Dictionary<int, string> StatusDictionary = new Dictionary<int, string>()
        {
            { (int)CMMS.CMMS_Status.CALIBRATION_REQUEST, "Calibration Request Waiting for Approval" },
            { (int)CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED, "Calibration Request Approved" },
            { (int)CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED, "Calibration Request Rejected" },
            { (int)CMMS.CMMS_Status.CALIBRATION_STARTED, "Calibration Started" },
            { (int)CMMS.CMMS_Status.CALIBRATION_COMPLETED, "Calibration Completed" },
            { (int)CMMS.CMMS_Status.CALIBRATION_CLOSED, "Calibration Closed" },
            { (int)CMMS.CMMS_Status.CALIBRATION_APPROVED, "Calibration Approved" },
            { (int)CMMS.CMMS_Status.CALIBRATION_REJECTED, "Calibration Rejected" },
            { (int)CMMS.CMMS_Status.CALIBRATION_SKIPPED, "Calibration Skipped" },
            { (int)CMMS.CMMS_Status.CALIBRATION_SCHEDULED, "Calibration Scheduled" }
        };


        internal string getLongStatus(CMMS.CMMS_Status notificationID, CMCalibrationDetails CaliObj)
        {
            string retValue = " ";


            switch (notificationID)
            {
                case CMMS.CMMS_Status.CALIBRATION_REQUEST:
                    retValue = String.Format("Calibration Requested by {0}", CaliObj.responsible_person);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED:
                    retValue = String.Format("Calibration Request Approved by {0}", CaliObj.request_approved_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED:
                    retValue = String.Format("Calibration  Request Rejected by {0}", CaliObj.request_rejected_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_STARTED:
                    retValue = String.Format("Calibration started at {0}", CaliObj.started_at);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_COMPLETED:
                    //retValue = String.Format("Calibration Dispachted by {0} at {1}", CaliObj.dispatched_by, CaliObj.dispatched_at);
                    retValue = String.Format("Calibration Completed by {0} at {1}", CaliObj.completed_by, CaliObj.completed_at);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_CLOSED:
                    retValue = String.Format("Calibration by {0} at {1}", CaliObj.Closed_at, CaliObj.Closed_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_APPROVED:
                    retValue = String.Format("Calibration Approved by {0}", CaliObj.approved_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_REJECTED:
                    retValue = String.Format("Calibration Rejected by {0}", CaliObj.rejected_by);
                    break;
                case CMMS.CMMS_Status.CALIBRATION_SKIPPED:
                    retValue = String.Format("Calibration Skipped by {0}", CaliObj.approved_by);
                    break;
                default:
                    break;
            }
            return retValue;

        }

        internal async Task<List<CMCalibrationList>> GetCalibrationList(int facility_id, string facilitytimeZone)
        {
            /* Fetch all the Asset table which calibration due date is lower than current date
             * JOIN Calibration table to fetch their previous details for list model
             * Your Code goes here
            */
            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN a_calibration.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";
            string myQuery = "SELECT " +
                                $" a_calibration.id as calibration_id, assets.id as asset_id, assets.name as asset_name, assets.serialNumber as asset_serial, CASE WHEN categories.name is null THEN 'Others' ELSE categories.name END as category_name, frequency.id as frequency_id, frequency.name as frequency_name,a_calibration.is_damaged, a_calibration.status as statusID, {statusOut} as calibration_status, " +
                                $" IF(assets.calibrationLastDate = '0000-00-00 00:00:00', CAST( '0001-01-01 00:00:01' as datetime), CAST(assets.calibrationLastDate AS DATETIME)) AS last_calibration_date, " +
                                $"IF(assets.calibrationDueDate = '0000-00-00 00:00:00', CAST( '0001-01-01 00:00:01' as datetime), CAST(assets.calibrationDueDate AS DATETIME)) AS next_calibration_due_date, vendor.id as vendor_id, vendor.name as vendor_name, CONCAT(request_by.firstName,' ',request_by.lastName) as responsible_person, a_calibration.received_date,a_calibration.start_date as Schedule_start_date,a_calibration.due_date AS calibration_due_date ,a_calibration.health_status as asset_health_status " +
                             "FROM assets " +
                             "LEFT JOIN " +
                                "frequency ON assets.calibrationFrequency = frequency.id " +
                             "inner JOIN " +
                                 "calibration as a_calibration on a_calibration.asset_id=assets.id " +
                             "LEFT JOIN " +
                                "assetcategories as categories on categories.id=assets.categoryId " +
                             "LEFT JOIN " +
                                "business as vendor ON a_calibration.vendor_id=vendor.id " +
                             "LEFT JOIN " +
                                "users as request_by ON a_calibration.requested_by=request_by.id " +
                             " WHERE categories.calibrationStatus = 1 AND (a_calibration.requested_at = (SELECT MAX(requested_at) FROM calibration as b_calibration WHERE a_calibration.asset_id = b_calibration.asset_id) OR a_calibration.requested_at is null) ";
            if (facility_id > 0)
            {
                myQuery += $"AND assets.facilityId = {facility_id} ";
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            myQuery += "GROUP BY assets.id  ORDER BY next_calibration_due_date ASC ;";
            List<CMCalibrationList> _calibrationList = await Context.GetData<CMCalibrationList>(myQuery).ConfigureAwait(false);
            foreach (var a in _calibrationList)
            {
                if (a != null && a.last_calibration_date != null)
                    a.last_calibration_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.last_calibration_date);
                if (a != null && a.next_calibration_due_date != null)
                    a.next_calibration_due_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.next_calibration_due_date);
                if (a != null && a.received_date != null)
                    a.received_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.received_date);

            }
            return _calibrationList;
        }
        internal async Task<CMCalibrationDetails> GetCalibrationDetails(int id, string facilitytimeZone)
        {

            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary)
            {
                statusOut += $"WHEN a_calibration.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";
            string myQuery = "SELECT " +
                                $"a_calibration.asset_id, assets.name as asset_name, assets.serialNumber as asset_serial, files.file_path as calibration_certificate_path, CASE WHEN categories.name is null THEN 'Others' ELSE categories.name END as category_name, {statusOut} as calibration_status, {statusOut} as status_short, a_calibration.status as statusID, frequency.id as frequency_id, frequency.name as frequency_name, CASE WHEN assets.calibrationLastDate = '0000-00-00 00:00:00' THEN NULL ELSE assets.calibrationLastDate END as last_calibration_date, CASE WHEN assets.calibrationDueDate = '0000-00-00 00:00:00' THEN NULL ELSE assets.calibrationDueDate END as next_calibration_due_date, vendor.id as vendor_id, vendor.name as vendor_name, CONCAT(request_by.firstName,' ',request_by.lastName) as responsible_person, CONCAT(request_approved_by.firstName,' ',request_approved_by.lastName) as request_approved_by, CONCAT(request_rejected_by.firstName,' ',request_rejected_by.lastName) as request_rejected_by,CONCAT(completed_by.firstName,' ',completed_by.lastName) as completed_by, CONCAT(approved_by.firstName,' ',approved_by.lastName) as approved_by,CONCAT(rejected_by.firstName,' ',rejected_by.lastName) as rejected_by,CONCAT(close_by.firstName,' ',close_by.lastName) as Closed_by, CASE WHEN a_calibration.received_date = '0000-00-00 00:00:00' THEN NULL ELSE a_calibration.received_date END AS received_date, a_calibration.requested_by,CASE WHEN a_calibration.requested_at = '0000-00-00 00:00:00' THEN NULL ELSE a_calibration.requested_at END AS requested_at, CASE WHEN a_calibration.request_approved_at = '0000-00-00 00:00:00' THEN NULL ELSE a_calibration.request_approved_at END AS request_approved_at, CASE WHEN a_calibration.request_rejected_at = '0000-00-00 00:00:00' THEN NULL ELSE a_calibration.request_rejected_at END AS request_rejected_at,a_calibration.is_damaged, CASE WHEN a_calibration.start_date = '0000-00-00 00:00:00' THEN NULL ELSE a_calibration.start_date END AS started_at,CASE WHEN a_calibration.done_date = '0000-00-00 00:00:00' THEN NULL ELSE a_calibration.done_date END AS completed_at, CASE WHEN a_calibration.approved_at = '0000-00-00 00:00:00' THEN NULL ELSE a_calibration.approved_at END AS approved_at, a_calibration.health_status as asset_health_status, CASE WHEN a_calibration.due_date = '0000-00-00 00:00:00' THEN NULL ELSE a_calibration.due_date END AS calibration_due_date " +
                             "FROM assets " +
                             "LEFT JOIN " +
                                "frequency ON assets.calibrationFrequency = frequency.id " +
                             "JOIN " +
                                "calibration as a_calibration on a_calibration.asset_id=assets.id " +
                             "LEFT JOIN " +
                                "assetcategories as categories on categories.id=assets.categoryId " +
                             "LEFT JOIN " +
                                "business as vendor ON a_calibration.vendor_id=vendor.id " +
                             "left join uploadedfiles as files ON files.id = a_calibration.calibration_certificate_file_id " +
                             "LEFT JOIN users as request_by ON a_calibration.requested_by= request_by.id " +
                             "LEFT JOIN users as request_approved_by on a_calibration.request_approved_by = request_approved_by.id " +
                             "LEFT JOIN users as request_rejected_by on a_calibration.request_rejected_by = request_rejected_by.id " +
                             "LEFT JOIN users as completed_by on a_calibration.completed_by = completed_by.id " +
                             "LEFT JOIN users as approved_by on a_calibration.approved_by = approved_by.id " +
                             "LEFT JOIN users as rejected_by on a_calibration.rejected_by = rejected_by.id " +
                             "LEFT JOIN users as close_by on a_calibration.close_by = close_by.id " +
                             "WHERE   ";
            if (id > 0)
            {
                myQuery += $"a_calibration.id = {id} ";
            }
            else
            {
                throw new ArgumentException("Invalid ID");
            }
            List<CMCalibrationDetails> _calibrationDetails = await Context.GetData<CMCalibrationDetails>(myQuery).ConfigureAwait(false);


            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_calibrationDetails[0].statusID);
            string _longStatus = getLongStatus(_Status_long, _calibrationDetails[0]);
            _calibrationDetails[0].status_long = _longStatus;
            _calibrationDetails[0].calibration_id = id;
            foreach (var a in _calibrationDetails)
            {
                if (a != null && a.started_at != null)
                    a.started_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.started_at);
                if (a != null && a.request_rejected_at != null)
                    a.request_rejected_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.request_rejected_at);
                if (a != null && a.request_approved_at != null)
                    a.request_approved_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.request_approved_at);
                if (a != null && a.requested_at != null)
                    a.requested_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.requested_at);
                if (a != null && a.received_date != null)
                    a.received_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.received_date);
                if (a != null && a.next_calibration_due_date != null)
                    a.next_calibration_due_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.next_calibration_due_date);
                if (a != null && a.last_calibration_date != null)
                    a.last_calibration_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.last_calibration_date);
                if (a != null && a.completed_at != null)
                    a.completed_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.completed_at);
                if (a != null && a.Closed_at != null)
                    a.Closed_at = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.Closed_at);
                if (a != null && a.calibration_due_date != null)
                    a.calibration_due_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.calibration_due_date);


            }

            string myQuery4 = "SELECT U.id, file_path as fileName, FC.name as fileCategory, U.File_Size as fileSize, U.status,U.description, '' as ptwFiles FROM uploadedfiles AS U " +
                         " LEFT JOIN calibration  as calibration on calibration.id = U.module_ref_id Left join filecategory FC on FC.Id = U.file_category " +
                         " where calibration.id = " + id + " and U.module_type = " + (int)CMMS.CMMS_Modules.CALIBRATION + ";";

            List<CMFileDetailCalibration> _UploadFileList = await Context.GetData<CMFileDetailCalibration>(myQuery4).ConfigureAwait(false);
            _calibrationDetails[0].file_list = _UploadFileList;

            return _calibrationDetails[0];
        }

        internal async Task<CMDefaultResponse> RequestCalibration(CMRequestCalibration request, int userID)
        {
            /*
             * Create new record in calibration table and insert all field mentioned in CMRequestCalibration model
             * Your Code goes here
            
            int id;
            string facilityQuery = $"SELECT facilityId FROM assets WHERE assets.id = {request.asset_id};";
            DataTable dt1 = await Context.FetchData(facilityQuery).ConfigureAwait(false);
            int facilityId = Convert.ToInt32(dt1.Rows[0][0]);
            if (request.asset_id <= 0)
                throw new ArgumentException("Invalid Asset ID");
            if (request.vendor_id <= 0)
            {
                string vendorQry = $"SELECT vendorId FROM assets WHERE id = {request.asset_id};";
                DataTable dtVendor = await Context.FetchData(vendorQry).ConfigureAwait(false);
                if (dtVendor.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dtVendor.Rows[0][0]) == 0)
                        throw new ArgumentException("Invalid Vendor ID");
                    else
                        request.vendor_id = Convert.ToInt32(dtVendor.Rows[0][0]);
                }
                else
                {
                    throw new ArgumentException("Invalid Vendor ID");
                }
            }
            if (request.next_calibration_date == null)
            {
                string dateQry = $"SELECT CASE WHEN calibrationDueDate = '0000-00-00 00:00:00' THEN NULL ELSE calibrationDueDate " +
                                    $"END AS next_calibration_date FROM assets WHERE id = {request.asset_id};";
                DataTable dtDate = await Context.FetchData(dateQry).ConfigureAwait(false);
                if (dtDate.Rows.Count > 0)
                {
                    if (dtDate.Rows[0][0] == DBNull.Value)
                        throw new ArgumentException("Invalid Due Date");
                    else
                        request.next_calibration_date = Convert.ToDateTime(dtDate.Rows[0][0]);
                }
                else
                {
                    throw new ArgumentException("Invalid Due Date");
                }
            }
            string statusQuery = $"SELECT id, status FROM calibration where due_date=(SELECT MAX(due_date) FROM calibration WHERE asset_id={request.asset_id} AND due_date is not null ORDER BY requested_at DESC) and asset_id={request.asset_id};";
            DataTable dt0 = await Context.FetchData(statusQuery).ConfigureAwait(false);
            bool exists = false;
            if (dt0.Rows.Count > 0)
            {

                int status = Convert.ToInt32(dt0.Rows[0]["status"]);
                int[] status_array = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST,
                            (int)CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED,
                            (int)CMMS.CMMS_Status.CALIBRATION_STARTED,
                            (int)CMMS.CMMS_Status.CALIBRATION_COMPLETED,
                            (int)CMMS.CMMS_Status.CALIBRATION_CLOSED,
                            (int)CMMS.CMMS_Status.CALIBRATION_REJECTED};
                exists = Array.Exists(status_array, element => element == status);
            }

            if (!exists)
            {


                // No existing calibration record to update, so insert a new one
                string insertQuery = "INSERT INTO calibration(asset_id, facility_id, vendor_id, due_date, requested_by, requested_at, status, status_updated_at) " +
                                    $"VALUES({request.asset_id}, {facilityId}, {request.vendor_id}, '{request.next_calibration_date.ToString("yyyy'-'MM'-'dd")}', " +
                                    $"{userID}, '{UtilsRepository.GetUTCTime()}', {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST}, '{UtilsRepository.GetUTCTime()}'); SELECT LAST_INSERT_ID();";
                DataTable dt21 = await Context.FetchData(insertQuery).ConfigureAwait(false);
                id = Convert.ToInt32(dt21.Rows[0][0]);
            }

            else
            {
                 string getIdQuery = $"SELECT id FROM calibration WHERE asset_id = {request.asset_id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST};";
                 DataTable dt2 = await Context.FetchData(getIdQuery).ConfigureAwait(false);
                 id = Convert.ToInt32(dt2.Rows[0][0]);
            // Update query instead of insert
            string updateQuery = $"UPDATE calibration SET facility_id = {facilityId}, vendor_id = {request.vendor_id}, " +
                                $"due_date = '{request.next_calibration_date.ToString("yyyy-MM-dd")}', " +
                               $"requested_by = {userID}, requested_at = '{UtilsRepository.GetUTCTime()}', " +
                               $"status = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST}, " +
                               $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                               $"WHERE asset_id = {request.asset_id} " +
                               $"AND status != {(int)CMMS.CMMS_Status.CALIBRATION_COMPLETED};";
            int affectedRows = await Context.ExecuteNonQry<int>(updateQuery).ConfigureAwait(false);
        }


        string setDueDate = $"UPDATE assets SET calibrationDueDate = '{request.next_calibration_date.ToString("yyyy-MM-dd")}' WHERE id = {request.asset_id};";
        await Context.ExecuteNonQry<int>(setDueDate).ConfigureAwait(false);

        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, request.asset_id, CMMS.CMMS_Modules.CALIBRATION, id, "Calibration Requested", CMMS.CMMS_Status.CALIBRATION_REQUEST, userID);

        CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(id, "");

        await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_REQUEST, new[] { userID
    }, _ViewCalibration);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Calibration requested successfully");
            return response;

              else
              {
                  int id = Convert.ToInt32(dt0.Rows[0]["id"]);
    CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.FAILURE, "Calibration cannot be requested as asset is already sent or requested for calibration");
                  return response;
              }
           
*/
            int status;
            if (request.asset_id <= 0)
                throw new ArgumentException("Invalid Asset ID");
            if (request.vendor_id <= 0)
            {
                string vendorQry = $"SELECT vendorId FROM assets WHERE id = {request.asset_id};";
                DataTable dtVendor = await Context.FetchData(vendorQry).ConfigureAwait(false);
                if (dtVendor.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dtVendor.Rows[0][0]) == 0)
                        throw new ArgumentException("Invalid Vendor ID");
                    else
                        request.vendor_id = Convert.ToInt32(dtVendor.Rows[0][0]);
                }
                else
                {
                    throw new ArgumentException("Invalid Vendor ID");
                }
            }
            if (request.next_calibration_date == null)
            {
                string dateQry = $"SELECT CASE WHEN calibrationDueDate = '0000-00-00 00:00:00' THEN NULL ELSE calibrationDueDate " +
                                    $"END AS next_calibration_date FROM assets WHERE id = {request.asset_id};";
                DataTable dtDate = await Context.FetchData(dateQry).ConfigureAwait(false);
                if (dtDate.Rows.Count > 0)
                {
                    if (dtDate.Rows[0][0] == DBNull.Value)
                        throw new ArgumentException("Invalid Due Date");
                    else
                        request.next_calibration_date = Convert.ToDateTime(dtDate.Rows[0][0]);
                }
                else
                {
                    throw new ArgumentException("Invalid Due Date");
                }
            }
            string statusQuery = $"SELECT id, status FROM calibration where due_date=(SELECT MAX(due_date) FROM calibration WHERE asset_id={request.asset_id} AND due_date is not null ORDER BY requested_at DESC) and asset_id={request.asset_id};";
            DataTable dt0 = await Context.FetchData(statusQuery).ConfigureAwait(false);
            status = Convert.ToInt32(dt0.Rows[0]["status"]);
            bool exists = false;
            if (dt0.Rows.Count > 0)
            {

                int[] status_array = { (int)CMMS.CMMS_Status.CALIBRATION_REQUEST,
                              (int)CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED,
                              (int)CMMS.CMMS_Status.CALIBRATION_STARTED,
                              (int)CMMS.CMMS_Status.CALIBRATION_COMPLETED,
                              (int)CMMS.CMMS_Status.CALIBRATION_CLOSED,
                              (int)CMMS.CMMS_Status.CALIBRATION_APPROVED,
                              (int)CMMS.CMMS_Status.CALIBRATION_REJECTED};
                exists = Array.Exists(status_array, element => element == status);
            }
            //  if (status >211 (int)CMMS.CMMS_Status.CALIBRATION_REQUEST)
            if (status > 211)
            {
                int id = Convert.ToInt32(dt0.Rows[0]["id"]);
                CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.FAILURE, "Calibration cannot be requested as asset is already sent or requested for calibration");
                return response;
            }
            string facilityQuery = $"SELECT facilityId FROM assets WHERE assets.id = {request.asset_id};";
            DataTable dt1 = await Context.FetchData(facilityQuery).ConfigureAwait(false);
            int facilityId = Convert.ToInt32(dt1.Rows[0][0]);
            string facilityq = $"SELECT reschedule FROM calibration  WHERE asset_id = {request.asset_id};";
            DataTable dt12 = await Context.FetchData(facilityq).ConfigureAwait(false);
            int res = Convert.ToInt32(dt12.Rows[0][0]);
            if (exists && res != 1)
            {

                string myQuery = "INSERT INTO calibration(asset_id, facility_id, vendor_id, due_date, requested_by, requested_at, status, status_updated_at) " +
                            $"VALUES({request.asset_id}, {facilityId}, {request.vendor_id}, '{request.next_calibration_date.ToString("yyyy'-'MM'-'dd")}', " +
                            $"{userID}, '{UtilsRepository.GetUTCTime()}', {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST}, '{UtilsRepository.GetUTCTime()}'); SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(myQuery).ConfigureAwait(false);
                int id = Convert.ToInt32(dt2.Rows[0][0]);
                string setDueDate = $"UPDATE assets SET calibrationDueDate = '{request.next_calibration_date.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE id = {request.asset_id};";
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, request.asset_id, CMMS.CMMS_Modules.CALIBRATION, id, "Calibration Requested", CMMS.CMMS_Status.CALIBRATION_REQUEST, userID);

                CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(id, "");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_SCHEDULED, new[] { userID }, _ViewCalibration);
                CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Calibration requested successfully");
                return response;

            }
            else
            {
                int id;

                string getIdQuery = $"SELECT id FROM calibration WHERE asset_id = {request.asset_id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_SCHEDULED};";
                DataTable dt2 = await Context.FetchData(getIdQuery).ConfigureAwait(false);
                id = Convert.ToInt32(dt2.Rows[0][0]);
                // Update query instead of insert
                string updateQuery = $"UPDATE calibration SET facility_id = {facilityId}, vendor_id = {request.vendor_id}, " +
                                    $"due_date = '{request.next_calibration_date.ToString("yyyy-MM-dd")}', " +
                                   $"requested_by = {userID}, requested_at = '{UtilsRepository.GetUTCTime()}', " +
                                   $"status = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST}, " +
                                   $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                                   $"WHERE asset_id = {request.asset_id} " +
                                   $"AND status != {(int)CMMS.CMMS_Status.CALIBRATION_COMPLETED};";
                int affectedRows = await Context.ExecuteNonQry<int>(updateQuery).ConfigureAwait(false);
                string setDueDate = $"UPDATE assets SET calibrationDueDate = '{request.next_calibration_date.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE id = {request.asset_id};";
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, request.asset_id, CMMS.CMMS_Modules.CALIBRATION, id, "Calibration Requested", CMMS.CMMS_Status.CALIBRATION_REQUEST, userID);

                CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(id, "");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_REQUEST, new[] { userID }, _ViewCalibration);
                CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Calibration requested successfully");
                return response;
            }
        }

        internal async Task<CMDefaultResponse> ApproveRequestCalibration(CMApproval request, int userID)
        {
            /*
             * Update the status in Calibration table and update history log
             * Your Code goes here
             * 
            */
            string myQuery = $"UPDATE calibration SET status = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"request_approved_by = {userID}, request_approved_at = '{UtilsRepository.GetUTCTime()}', request_approve_remark = '{request.comment}' " +
                                $"WHERE id = {request.id} AND status IN ({(int)CMMS.CMMS_Status.CALIBRATION_REQUEST}, {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED});";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            string assetIDQuery = $"SELECT asset_id FROM calibration where id = {request.id};";
            DataTable dtAsset = await Context.FetchData(assetIDQuery).ConfigureAwait(false);
            int assetID = Convert.ToInt32(dtAsset.Rows[0][0]);
            CMMS.RETRUNSTATUS returnStatus = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                returnStatus = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response;
            if (returnStatus == CMMS.RETRUNSTATUS.SUCCESS)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, assetID, CMMS.CMMS_Modules.CALIBRATION, request.id, "Calibration Request Approved", CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED, userID);
                CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(request.id, "");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED, new[] { userID }, _ViewCalibration);
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration Request Approved Successfully");
            }
            else
            {
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration Request Could Not Be Approved");
            }
            return response;
        }

        internal async Task<CMDefaultResponse> RejectRequestCalibration(CMApproval request, int userID)
        {
            /*
             * Update the status in Calibration table and update history log
             * Your Code goes here
            */
            string myQuery = $"UPDATE calibration SET status = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"request_rejected_by = {userID}, request_rejected_at = '{UtilsRepository.GetUTCTime()}', request_reject_remark = '{request.comment}' " +
                                $"WHERE id = {request.id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            string assetIDQuery = $"SELECT asset_id FROM calibration where id = {request.id};";
            DataTable dtAsset = await Context.FetchData(assetIDQuery).ConfigureAwait(false);
            int assetID = Convert.ToInt32(dtAsset.Rows[0][0]);
            CMMS.RETRUNSTATUS returnStatus = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                returnStatus = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response;
            if (returnStatus == CMMS.RETRUNSTATUS.SUCCESS)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, assetID, CMMS.CMMS_Modules.CALIBRATION, request.id, "Calibration Request Rejected", CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED, userID);
                CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(request.id, "");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED, new[] { userID }, _ViewCalibration);
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration Request Rejected Successfully");
            }
            else
            {
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration Request Could Not Be Rejected");
            }
            return response;
        }
        internal async Task<CMPreviousCalibration> GetPreviousCalibration(int asset_id, string facilitytimeZone)
        {
            /*
             * While requesting user need to show the previous calibrated details.
             * Fetch required properties mentioned in CMPreviousCalibration and return
             * Your Code goes here
            */
            string myQuery = "SELECT " +
                                "assets.id as asset_id, assets.name as asset_name, vendor.id as vendor_id, vendor.name as vendor_name, CASE WHEN assets.calibrationLastDate = '0000-00-00 00:00:00' THEN NULL ELSE assets.calibrationLastDate END as previous_calibration_date " +
                             "FROM assets " +
                             "LEFT JOIN business as vendor ON assets.vendorId = vendor.id " +
                             $"WHERE assets.id = {asset_id}";
            List<CMPreviousCalibration> _calibrationList = await Context.GetData<CMPreviousCalibration>(myQuery).ConfigureAwait(false);
            foreach (var a in _calibrationList)
            {

                if (a != null && a.previous_calibration_date != null)
                    a.previous_calibration_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, (DateTime)a.previous_calibration_date);
            }
            return _calibrationList[0];
        }
        internal async Task<CMDefaultResponse> StartCalibration(int calibration_id)
        {
            /*
             * Update the Calibration table status and History log
             * Your Code goes here
            */
            string Query = $"SELECT due_date FROM calibration where id = {calibration_id};";
            DataTable dt = await Context.FetchData(Query).ConfigureAwait(false);
            DateTime start_date = Convert.ToDateTime(dt.Rows[0][0]);
            string myQuery = $"UPDATE calibration SET status = {(int)CMMS.CMMS_Status.CALIBRATION_STARTED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"start_date = '{start_date.ToString("yyyy-MM-dd")}' " +
                                $"WHERE id = {calibration_id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            string userIDQuery = $"SELECT requested_by FROM calibration where id = {calibration_id};";
            DataTable dtUser = await Context.FetchData(userIDQuery).ConfigureAwait(false);
            int userID = Convert.ToInt32(dtUser.Rows[0][0]);
            string assetIDQuery = $"SELECT asset_id FROM calibration where id = {calibration_id};";
            DataTable dtAsset = await Context.FetchData(assetIDQuery).ConfigureAwait(false);
            int assetID = Convert.ToInt32(dtAsset.Rows[0][0]);
            CMMS.RETRUNSTATUS returnStatus = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                returnStatus = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response;
            if (returnStatus == CMMS.RETRUNSTATUS.SUCCESS)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, assetID, CMMS.CMMS_Modules.CALIBRATION, calibration_id, "Calibration Started", CMMS.CMMS_Status.CALIBRATION_STARTED, userID);
                CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(calibration_id, "");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_STARTED, new[] { userID }, _ViewCalibration);

                response = new CMDefaultResponse(calibration_id, returnStatus, $"Calibration Started for Asset {assetID}");
            }
            else
            {
                response = new CMDefaultResponse(calibration_id, returnStatus, "Calibration Cannot be Started");
            }
            return response;
        }
        internal async Task<CMDefaultResponse> CompleteCalibration(CMCompleteCalibration request, int userID)
        {
            /*
             * Update the required fields in CMCompleteCalibration model and history Log
             * File upload will be in file table with ref of calibration id
             * Your Code goes here
            */
            string myQuery = $"UPDATE calibration SET done_date = '{DateTime.UtcNow.ToString("yyyy'-'MM'-'dd")}', completed_by = {userID}, " +
                                $"completed_remark = '{request.comment}', status = {(int)CMMS.CMMS_Status.CALIBRATION_COMPLETED}, " +
                                $"is_damaged ={request.is_damaged} ," +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                                $"WHERE id = {request.calibration_id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_STARTED};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            string assetIDQuery = $"SELECT asset_id FROM calibration where id = {request.calibration_id};";
            DataTable dtAsset = await Context.FetchData(assetIDQuery).ConfigureAwait(false);
            int assetID = Convert.ToInt32(dtAsset.Rows[0][0]);

            if (request.uploaded_file_id.Count > 0)
            {
                for (var i = 0; i < request.uploaded_file_id.Count; i++)
                {
                    string update_Q = $"update uploadedfiles set module_type = {(int)CMMS.CMMS_Modules.CALIBRATION} , module_ref_id = {request.calibration_id} where id = {request.uploaded_file_id[i]};";
                    int result = await Context.ExecuteNonQry<int>(update_Q).ConfigureAwait(false);
                }
            }

            CMMS.RETRUNSTATUS returnStatus = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                returnStatus = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response;
            if (returnStatus == CMMS.RETRUNSTATUS.SUCCESS)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, assetID, CMMS.CMMS_Modules.CALIBRATION, request.calibration_id, "Calibration Completed", CMMS.CMMS_Status.CALIBRATION_COMPLETED, userID);
                CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(request.calibration_id, "");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_COMPLETED, new[] { userID }, _ViewCalibration);
                response = new CMDefaultResponse(request.calibration_id, returnStatus, "Calibration Completed");
            }
            else
            {
                response = new CMDefaultResponse(request.calibration_id, returnStatus, "Calibration Failed To Complete or is already completed");
            }
            return response;
        }
        internal async Task<CMDefaultResponse> CloseCalibration(CMCloseCalibration request, int userID)
        {
            /*
             * Update the Calibration status and update the history log
             * Your Code goes here
            */
            string myQuery = $"UPDATE calibration SET close_by = {userID}, received_date = '{DateTime.UtcNow.ToString("yyyy'-'MM'-'dd")}', " +
                                $"close_remark = '{request.comment}', is_damaged = {(request.is_damaged == null ? 0 : request.is_damaged)}, " +
                                $"calibration_certificate_file_id = {request.calibration_certificate_file_id}, " +
                                $"status = {(int)CMMS.CMMS_Status.CALIBRATION_CLOSED}, status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                                $"WHERE id = {request.calibration_id} " +
                                $"AND status IN ({(int)CMMS.CMMS_Status.CALIBRATION_COMPLETED}, {(int)CMMS.CMMS_Status.CALIBRATION_REJECTED});";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            string assetIDQuery = $"SELECT asset_id FROM calibration where id = {request.calibration_id};";
            DataTable dtAsset = await Context.FetchData(assetIDQuery).ConfigureAwait(false);
            int assetID = Convert.ToInt32(dtAsset.Rows[0][0]);
            CMMS.RETRUNSTATUS returnStatus = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                returnStatus = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response;
            if (returnStatus == CMMS.RETRUNSTATUS.SUCCESS)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, assetID, CMMS.CMMS_Modules.CALIBRATION, request.calibration_id, "Calibration Closed", CMMS.CMMS_Status.CALIBRATION_CLOSED, userID);
                CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(request.calibration_id, "");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_CLOSED, new[] { userID }, _ViewCalibration);
                response = new CMDefaultResponse(request.calibration_id, returnStatus, "Calibration Closed");
            }
            else
            {
                response = new CMDefaultResponse(request.calibration_id, returnStatus, "Calibration cannot be closed or is already closed");
            }
            return response;
        }
        internal async Task<CMRescheduleApprovalResponse> ApproveCalibration(CMApproval request, int userID)
        {
            /* Update the Calibration status and history log
             * Also update the CalibrationDueDate with new date as per frequency setup 
             * Your Code goes here
            */
            string myQuery1 = "SELECT asset_id, vendor_id, due_date as next_calibration_date FROM calibration " +
                                $"WHERE id = {request.id};";
            List<CMRequestCalibration> nextRequest = await Context.GetData<CMRequestCalibration>(myQuery1).ConfigureAwait(false);
            string myQuery2 = $"SELECT calibrationFrequency FROM assets " +
                                $"WHERE id = {nextRequest[0].asset_id};";
            DataTable dt = await Context.FetchData(myQuery2).ConfigureAwait(false);
            int frequencyId = Convert.ToInt32(dt.Rows[0][0]);
            DateTime nextDate = UtilsRepository.Reschedule(nextRequest[0].next_calibration_date, frequencyId);
            string myQuery3 = $"UPDATE assets SET vendorId = {nextRequest[0].vendor_id}, calibrationDueDate = '{nextDate.ToString("yyyy-MM-dd HH:mm:ss")}', calibrationLastDate = '{nextRequest[0].next_calibration_date.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE id = {nextRequest[0].asset_id};";
            nextRequest[0].next_calibration_date = nextDate;
            await Context.ExecuteNonQry<int>(myQuery3).ConfigureAwait(false);
            string myQuery4 = $"UPDATE calibration SET approved_by = {userID},reschedule=1,LastcalibrationDoneDate='{nextRequest[0].next_calibration_date}',approved_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"approve_remark = '{request.comment}', status = {(int)CMMS.CMMS_Status.CALIBRATION_APPROVED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                                $"WHERE id = {request.id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_CLOSED};";
            //need add lastcalibration
            int retVal = await Context.ExecuteNonQry<int>(myQuery4).ConfigureAwait(false);
            CMDefaultResponse newCalibration = await RequestCalibration(nextRequest[0], userID);
            CMApproval approval = new CMApproval()
            {
                id = newCalibration.id[0],
                comment = "Approved"
            };
            await ApproveRequestCalibration(approval, userID);
            CMMS.RETRUNSTATUS returnStatus = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                returnStatus = CMMS.RETRUNSTATUS.SUCCESS;
            CMRescheduleApprovalResponse response;
            if (returnStatus == CMMS.RETRUNSTATUS.SUCCESS)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, nextRequest[0].asset_id, CMMS.CMMS_Modules.CALIBRATION, request.id, $"Calibration Approved to ID {newCalibration.id[0]}", CMMS.CMMS_Status.CALIBRATION_COMPLETED, userID);
                CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(request.id, "");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_APPROVED, new[] { userID }, _ViewCalibration);
                response = new CMRescheduleApprovalResponse(newCalibration.id[0], request.id, returnStatus, $"Calibration Approved. Next Calibration with ID {newCalibration.id[0]} on '{nextRequest[0].next_calibration_date.ToString("yyyy'-'MM'-'dd")}'");
            }
            else
            {
                response = new CMRescheduleApprovalResponse(0, request.id, returnStatus, "Calibration could not be approved");
            }
            return response;
        }

        internal async Task<CMDefaultResponse> RejectCalibration(CMApproval request, int userID)
        {
            /* Update the Calibration status and history log
             * Your Code goes here
            */
            string myQuery = $"UPDATE calibration SET rejected_by = {userID}, rejected_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"reject_remark = '{request.comment}', status = {(int)CMMS.CMMS_Status.CALIBRATION_REJECTED}, " +
                                $"status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                                $"WHERE id = {request.id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_CLOSED};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMMS.RETRUNSTATUS returnStatus = CMMS.RETRUNSTATUS.FAILURE;
            string assetIDQuery = $"SELECT asset_id FROM calibration where id = {request.id};";
            DataTable dtAsset = await Context.FetchData(assetIDQuery).ConfigureAwait(false);
            int assetID = Convert.ToInt32(dtAsset.Rows[0][0]);
            if (retVal > 0)
                returnStatus = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response;
            if (returnStatus == CMMS.RETRUNSTATUS.SUCCESS)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, assetID, CMMS.CMMS_Modules.CALIBRATION, request.id, "Calibration Rejected", CMMS.CMMS_Status.CALIBRATION_REJECTED, userID);
                CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(request.id, "");

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_REJECTED, new[] { userID }, _ViewCalibration);
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration Rejected");
            }
            else
            {
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration could not get rejected.");
            }
            return response;
        }

        internal async Task<CMDefaultResponse> SkipCalibration(CMCloseCalibration request, int userID)
        {


            DateTime due_date = DateTime.Parse("0001-01-01");
            string due_date_q = $"select  DATE_ADD(due_date, INTERVAL frequency.days DAY) due_date " +
                $" FROM assets " +
                $" LEFT JOIN frequency ON assets.calibrationFrequency = frequency.id " +
                $" LEFT JOIN calibration as a_calibration on a_calibration.asset_id=assets.id " +
                $" where a_calibration.id = {request.calibration_id};";
            DataTable dt_due_date = await Context.FetchData(due_date_q).ConfigureAwait(false);
            if (dt_due_date.Rows.Count > 0 && dt_due_date.Rows[0][0] != DBNull.Value)
            {
                due_date = Convert.ToDateTime(dt_due_date.Rows[0][0]);
            }

            // creating new entry for start date based on frequeny
            string insertQuery = $"INSERT INTO calibration ( asset_id, facility_id, due_date, start_date, calibration_certificate_file_id, status, status_updated_at, received_date, is_damaged, requested_by, requested_at, request_approved_by, request_approved_at, request_approve_remark, request_rejected_by, request_rejected_at, request_reject_remark, vendor_id, completed_by, completed_remark, close_by, close_remark, approved_by, approved_at, approve_remark, rejected_by, rejected_at, reject_remark, health_status) " +
                      $"SELECT asset_id, facility_id, '{due_date.ToString("yyyy-MM-dd")}', start_date, calibration_certificate_file_id, status, status_updated_at, received_date, is_damaged, requested_by, requested_at, request_approved_by, request_approved_at, request_approve_remark, request_rejected_by, request_rejected_at, request_reject_remark, vendor_id, completed_by, completed_remark, close_by, close_remark, approved_by, approved_at, approve_remark, rejected_by, rejected_at, reject_remark, health_status " +
                      $"FROM calibration WHERE id = {request.calibration_id};";

            await Context.ExecuteNonQry<int>(insertQuery).ConfigureAwait(false);

            // updating old to CALIBRATION_SKIPPED

            string myQuery = $"UPDATE calibration SET " +
                                $"status = {(int)CMMS.CMMS_Status.CALIBRATION_SKIPPED}, status_updated_at = '{UtilsRepository.GetUTCTime()}' " +
                                $"WHERE id = {request.calibration_id} ;";



            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            string assetIDQuery = $"SELECT asset_id FROM calibration where id = {request.calibration_id};";
            DataTable dtAsset = await Context.FetchData(assetIDQuery).ConfigureAwait(false);
            int assetID = Convert.ToInt32(dtAsset.Rows[0][0]);
            CMMS.RETRUNSTATUS returnStatus = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                returnStatus = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response;
            if (returnStatus == CMMS.RETRUNSTATUS.SUCCESS)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, assetID, CMMS.CMMS_Modules.CALIBRATION, request.calibration_id, "Calibration Skipped", CMMS.CMMS_Status.CALIBRATION_SKIPPED, userID);
                CMCalibrationDetails _ViewCalibration = await GetCalibrationDetails(request.calibration_id, "");

                //await CMMSNotification.sendNotification(CMMS.CMMS_Modules.CALIBRATION, CMMS.CMMS_Status.CALIBRATION_SKIPPED, new[] { userID }, _ViewCalibration);
                response = new CMDefaultResponse(request.calibration_id, returnStatus, "Calibration Skipped");
            }
            else
            {
                response = new CMDefaultResponse(request.calibration_id, returnStatus, "Calibration cannot be skipped.");
            }
            return response;
        }
    }
}
