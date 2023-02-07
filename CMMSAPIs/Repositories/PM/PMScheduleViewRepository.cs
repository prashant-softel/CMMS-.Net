using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.PM
{
    public class PMScheduleViewRepository : GenericRepository
    {
        public PMScheduleViewRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

        }

        internal async Task<List<CMPMScheduleView>> GetScheduleViewList(int facility_id, DateTime start_date, DateTime end_date)
        {
            /*
             * Primary Table - PMExecution & PMSchedule
             * Read All properties mention in model and return list
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CancelPMScheduleView(int schedule_id)
        {
            /*
             * Primary Table - PMSchedule
             * Delete the requested id from primary table
             * Code goes here
            */
            return null;
        }

        internal async Task<CMPMScheduleViewDetail> GetPMScheduleViewDetail(int schedule_id)
        {
            /*
             * Primary Table - PMExecution & PMSchedule
             * Other supporting tables - Facility, Asset, AssetCategory, Users
             * Read All properties mention in model and return list
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> SetPMScheduleView(CMPMScheduleExecution request)
        {
            /*
             * Primary Table - PMExecution
             * Add all the details present in CMPMScheduleExecution model
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> UpdatePMScheduleExecution(CMPMScheduleExecution request)
        {
            /*
             * Primary Table - PMExecution
             * Update all the details present in CMPMScheduleExecution model
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> ApprovePMScheduleExecution(CMApproval request)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table for approval
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> RejectPMScheduleExecution(CMApproval request)
        {
            /*
             * Primary Table - PMExecution
             * Set the required field in primary table as Rejection
             * Code goes here
            */
            return null;
        }
    }

}
