﻿using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System;

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
            { 211, "Calibration Request Waiting for Approval" },
            { 212, "Calibration Request Approved" },
            { 213, "Calibration Request Rejected" },
            { 214, "Calibration Started" },
            { 215, "Calibration Completed" },
            { 216, "Calibration Closed" },
            { 217, "Calibration Approved" },
            { 218, "Calibration Rejected" }
        };

        internal async Task<List<CMCalibrationList>> GetCalibrationList(int facility_id)
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
                                $"a_calibration.asset_id, assets.name as asset_name, assets.serialNumber as asset_serial, CASE WHEN categories.name is null THEN 'Others' ELSE categories.name END as category_name, {statusOut} as calibration_status, a_calibration.due_date as last_calibration_date, vendor.name as vendor_name, CONCAT(request_by.firstName,' ',request_by.lastName) as responsible_person, a_calibration.received_date, a_calibration.health_status as asset_health_status " +
                             "FROM assets " +
                             "JOIN " +
                                "calibration as a_calibration on a_calibration.asset_id=assets.id " + 
                             "LEFT JOIN " +
                                "assetcategories as categories on categories.id=assets.categoryId " + 
                             "LEFT JOIN " +
                                "business as vendor ON a_calibration.vendor_id=vendor.id " + 
                             "LEFT JOIN " +
                                "users as request_by ON a_calibration.requested_by=request_by.id " +
                             "WHERE due_date = (SELECT MAX(due_date) FROM calibration as b_calibration WHERE a_calibration.asset_id = b_calibration.asset_id AND b_calibration.due_date <= now() AND " +
                                "(b_calibration.requested_at = (SELECT MAX(requested_at) FROM calibration as c_calibration WHERE b_calibration.asset_id = c_calibration.asset_id) OR b_calibration.requested_at is null)) ";
            if(facility_id > 0)
            {
                myQuery += $"AND a_calibration.facility_id = {facility_id} ";
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            myQuery += "GROUP BY a_calibration.asset_id ORDER BY a_calibration.id DESC;";
            List<CMCalibrationList> _calibrationList = await Context.GetData<CMCalibrationList>(myQuery).ConfigureAwait(false);
            return _calibrationList;
        }

        internal async Task<CMDefaultResponse> RequestCalibration(CMRequestCalibration request, int userID)
        {
            /*
             * Create new record in calibration table and insert all field mentioned in CMRequestCalibration model
             * Your Code goes here
            */
            if (request.asset_id <= 0)
                throw new ArgumentException("Invalid Asset ID");
            if (request.vendor_id <= 0)
                throw new ArgumentException("Invalid Vendor ID");
            if (request.next_calibration_date == null)
                throw new ArgumentException("Invalid Due Date");
            string statusQuery = $"SELECT status FROM calibration where due_date=(SELECT MAX(due_date) FROM calibration WHERE asset_id={request.asset_id} AND due_date is not null) and asset_id={request.asset_id};";
            DataTable dt0=await Context.FetchData(statusQuery).ConfigureAwait(false);
            bool exists=true;
            if (dt0.Rows.Count > 0)
            {
                int status = Convert.ToInt32(dt0.Rows[0][0]);
                int[] status_array = { 213, 216, 218, 0 };
                exists = Array.Exists(status_array, element => element == status);
            }
            if(exists)
            {
                string facilityQuery = $"SELECT facilityId FROM assets WHERE assets.id = {request.asset_id};";
                DataTable dt1 = await Context.FetchData(facilityQuery).ConfigureAwait(false);
                int facilityId = Convert.ToInt32(dt1.Rows[0][0]);
                string myQuery = "INSERT INTO calibration(asset_id, facility_id, vendor_id, due_date, requested_by, requested_at, status) " +
                                $"VALUES({request.asset_id}, {facilityId}, {request.vendor_id}, '{request.next_calibration_date.ToString("yyyy'-'MM'-'dd")}', " +
                                $"{userID}, '{UtilsRepository.GetUTCTime()}', {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST}); SELECT LAST_INSERT_ID();";
                DataTable dt2 = await Context.FetchData(myQuery).ConfigureAwait(false);
                int id = Convert.ToInt32(dt2.Rows[0][0]);
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, request.asset_id, CMMS.CMMS_Modules.CALIBRATION, id, "Calibration Requested", CMMS.CMMS_Status.CALIBRATION_REQUEST, userID);
                CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Calibration requested successfully");
                return response;
            }
            else
            {
                throw new ArgumentException("Asset is already sent or requested for calibration");
            }
        }

        internal async Task<CMDefaultResponse> ApproveRequestCalibration(CMApproval request, int userID)
        {
            /*
             * Update the status in Calibration table and update history log
             * Your Code goes here
             * 
            */
            string myQuery = $"UPDATE calibration SET status = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED}, request_approved_by = {userID}, " +
                                $"request_approved_at = '{UtilsRepository.GetUTCTime()}', request_approve_remark = '{request.comment}' " + 
                                $"WHERE id = {request.id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            string assetIDQuery = $"SELECT asset_id FROM calibration where id = {request.id};";
            DataTable dtAsset = await Context.FetchData(assetIDQuery).ConfigureAwait(false);
            int assetID = Convert.ToInt32(dtAsset.Rows[0][0]);
            CMMS.RETRUNSTATUS returnStatus = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                returnStatus = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response;
            if(returnStatus == CMMS.RETRUNSTATUS.SUCCESS)
            {
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, assetID, CMMS.CMMS_Modules.CALIBRATION, request.id, "Calibration Request Approved", CMMS.CMMS_Status.CALIBRATION_REQUEST_APPROVED, userID);
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
            string myQuery = $"UPDATE calibration SET status = {(int)CMMS.CMMS_Status.CALIBRATION_REQUEST_REJECTED}, request_rejected_by = {userID}, " +
                                $"request_rejected_at = '{UtilsRepository.GetUTCTime()}', request_reject_remark = '{request.comment}' " +
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
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration Request Rejected Successfully");
            }
            else
            {
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration Request Could Not Be Rejected");
            }
            return response;
        }
        internal async Task<CMPreviousCalibration> GetPreviousCalibration(int asset_id)
        {
            /*
             * While requesting user need to show the previous calibrated details.
             * Fetch required properties mentioned in CMPreviousCalibration and return
             * Your Code goes here
            */
            string myQuery = "SELECT " + 
                                "asset_id, vendor_id, due_date as previous_calibration_date " + 
                             "FROM " + 
                                "calibration " + 
                             $"WHERE due_date<current_date() AND asset_id = {asset_id}";
            List<CMPreviousCalibration> _calibrationList = await Context.GetData<CMPreviousCalibration>(myQuery).ConfigureAwait(false);
            if (_calibrationList.Count > 1)
                return _calibrationList[1];
            else
                throw new NullReferenceException("No Previous Calibration was found");
        }
        internal async Task<CMDefaultResponse> StartCalibration(int calibration_id)
        {
            /*
             * Update the Calibration table status and History log
             * Your Code goes here
            */
            DateTime start_date = DateTime.Parse(UtilsRepository.GetUTCTime());
            string myQuery = $"UPDATE calibration SET status = {(int)CMMS.CMMS_Status.CALIBRATION_STARTED}, " + 
                                $"start_date = '{start_date.ToString("yyyy'-'MM'-'dd")}' " + 
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
                                $"completed_remark = '{request.comment}', is_damaged = {(request.is_damaged == null ? 0 : request.is_damaged)}, " + 
                                $"status = {(int)CMMS.CMMS_Status.CALIBRATION_COMPLETED} " + 
                                $"WHERE id = {request.calibration_id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_STARTED};";
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
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.INVENTORY, assetID, CMMS.CMMS_Modules.CALIBRATION, request.calibration_id, "Calibration Completed", CMMS.CMMS_Status.CALIBRATION_COMPLETED, userID);
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
                                $"close_remark = '{request.comment}', " +
                                $"status = {(int)CMMS.CMMS_Status.CALIBRATION_CLOSED} " +
                                $"WHERE id = {request.calibration_id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_COMPLETED};";
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
                response = new CMDefaultResponse(request.calibration_id, returnStatus, "Calibration Closed");
            }
            else
            {
                response = new CMDefaultResponse(request.calibration_id, returnStatus, "Calibration cannot be closed or is already closed");
            }
            return response;
        }
        internal async Task<CMDefaultResponse> ApproveCalibration(CMApproval request, int userID)
        {
            /* Update the Calibration status and history log
             * Also update the CalibrationDueDate with new date as per frequency setup 
             * Your Code goes here
            */
            string myQuery1 = "SELECT asset_id, vendor_id, received_date as next_calibration_date FROM calibration " + 
                                $"WHERE id = {request.id};";
            List<CMRequestCalibration> nextRequest = await Context.GetData<CMRequestCalibration>(myQuery1).ConfigureAwait(false);
            string myQuery2 = $"SELECT calibrationFrequency FROM assets " +
                                $"WHERE id = {nextRequest[0].asset_id};";
            DataTable dt = await Context.FetchData(myQuery2).ConfigureAwait(false);
            int frequencyId = Convert.ToInt32(dt.Rows[0][0]);
            DateTime nextDate = UtilsRepository.Reschedule(nextRequest[0].next_calibration_date, frequencyId);
            nextRequest[0].next_calibration_date = nextDate;
            CMDefaultResponse newCalibration = await RequestCalibration(nextRequest[0], userID);
            CMApproval approval = new CMApproval()
            {
                id = newCalibration.id[0],
                comment = null
            };
            await ApproveRequestCalibration(approval, userID);
            string myQuery3 = $"UPDATE calibration SET approved_by = {userID}, approved_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"approve_remark = '{request.comment}', status = {(int)CMMS.CMMS_Status.CALIBRATION_APPROVED} " +
                                $"WHERE id = {request.id} AND status = {(int)CMMS.CMMS_Status.CALIBRATION_CLOSED};";
            int retVal = await Context.ExecuteNonQry<int>(myQuery3).ConfigureAwait(false);
            CMMS.RETRUNSTATUS returnStatus = CMMS.RETRUNSTATUS.FAILURE;
            if (retVal > 0)
                returnStatus = CMMS.RETRUNSTATUS.SUCCESS;
            CMDefaultResponse response;
            if (returnStatus == CMMS.RETRUNSTATUS.SUCCESS)
            {
                await _utilsRepo.AddHistoryLog( CMMS.CMMS_Modules.INVENTORY, nextRequest[0].asset_id, CMMS.CMMS_Modules.CALIBRATION, request.id, $"Calibration Approved to ID {newCalibration.id[0]}", CMMS.CMMS_Status.CALIBRATION_COMPLETED, userID);
                response = new CMDefaultResponse(request.id, returnStatus, $"Calibration Approved. Next Calibration with ID {newCalibration.id[0]} on '{nextRequest[0].next_calibration_date.ToString("yyyy'-'MM'-'dd")}'");
            }
            else
            {
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration could not be approved");
            }
            return response;
        }

        internal async Task<CMDefaultResponse> RejectCalibration(CMApproval request, int userID)
        {
            /* Update the Calibration status and history log
             * Your Code goes here
            */
            string myQuery = $"UPDATE calibration SET rejected_by = {userID}, rejected_at = '{UtilsRepository.GetUTCTime()}', " +
                                $"reject_remark = '{request.comment}', status = {(int)CMMS.CMMS_Status.CALIBRATION_REJECTED} " +
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
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration Rejected");
            }
            else
            {
                response = new CMDefaultResponse(request.id, returnStatus, "Calibration could not get rejected.");
            }
            return response;
        }
    }
}
