using CMMSAPIs.Helper;
using CMMSAPIs.Models.Audit;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Calibration
{
    public class CalibrationRepository : GenericRepository
    {
        public CalibrationRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<List<CMCalibrationList>> GetCalibrationList(int facility_id)
        {
            /* Fetch all the Asset table which calibration due date is lower than current date
             * JOIN Calibration table to fetch their previous details for list model
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> RequestCalibration(CMRequestCalibration request)
        {
            /*
             * Create new record in calibration table and insert all field mentioned in CMRequestCalibration model
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> ApproveRequestCalibration(CMApproval request)
        {
            /*
             * Update the status in Calibration table and update history log
             * Your Code goes here
             * 
            */
            return null;
        }

        internal async Task<CMDefaultResponse> RejectRequestCalibration(CMApproval request)
        {
            /*
             * Update the status in Calibration table and update history log
             * Your Code goes here
            */
            return null;
        }
        internal async Task<CMDefaultResponse> GetPreviousCalibration(CMPreviousCalibration request)
        {
            /*
             * While requesting user need to show the previous calibrated details.
             * Fetch required properties mentioned in CMPreviousCalibration and return
             * Your Code goes here
            */
            return null;
        }
        internal async Task<CMDefaultResponse> StartCalibration(int calibration_id)
        {
            /*
             * Update the Calibration table status and History log
             * Your Code goes here
            */
            return null;
        }
        internal async Task<CMDefaultResponse> CompleteCalibration(CMCompleteCalibration request)
        {
            /*
             * Update the required fields in CMCompleteCalibration model and history Log
             * File upload will be in file table with ref of calibration id
             * Your Code goes here
            */
            return null;
        }
        internal async Task<CMDefaultResponse> CloseCalibration(CMCloseCalibration request)
        {
            /*
             * Update the Calibration status and update the history log
             * Your Code goes here
            */
            return null;
        }
        internal async Task<CMDefaultResponse> ApproveCalibration(CMApproval request)
        {
            /* Update the Calibration status and history log
             * Also update the CalibrationDueDate with new date as per frequency setup 
             * Your Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> RejectCalibration(CMApproval request)
        {
            /* Update the Calibration status and history log
             * Your Code goes here
            */
            return null;
        }
    }
}
