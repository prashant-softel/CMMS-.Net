using CMMSAPIs.Helper;
using CMMSAPIs.Models.MC;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.MC
{
    public class MCRepository : GenericRepository
    {
        public MCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper) { }

        internal async Task<List<CMMC>> GetCMList(CMMCFilter request)
        {
           /*
            * Primary Table - ModuleCleaning
            * Return all property in CMMCList Model
           */
           return null;
        }
        internal async Task<CMDefaultResponse> CreateMCPlan(CMMCPlan request)
        {
            /*
             * Insert Basic detail in ModuleCleaning table and schedule information in ModuleCleaningSchedule
             * Add history log table
             * Return
            */
            return null;
        }
        internal async Task<CMDefaultResponse> ApproveMCPlan(CMApproval request)
        {
            /*
             * Update the status ModuleCleaning table
             * Add history log table
            */
            return null;
        }
        internal async Task<CMDefaultResponse> RejectMCPlan(CMApproval request)
        {
            /*
             * Update the status ModuleCleaning table
             * Add history log table
            */
            return null;
        }

        internal async Task<CMDefaultResponse> StartMCExecution(int id)
        {
            /*
             * Update the status ModuleCleaning table
             * Add history log table
            */
            return null;
        }

        internal async Task<CMDefaultResponse> MCExecution(CMMCExecution request)
        {
            /*
             * Insert the execution details in ModuleCleaning and ModuleCleaningExecution table
             * Add history log table
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CompleteMCExecution(CMMCExecution request)
        {
            /*
             * Update/Insert the execution details in ModuleCleaning and ModuleCleaningExecution table
             * Add history log table
            */
            return null;
        }

        internal async Task<CMDefaultResponse> ApproveMCExecution(CMApproval request)
        {
            /*
             * Update the status ModuleCleaning table
             * Add history log table
            */
            return null;
        }

        internal async Task<CMDefaultResponse> RejectMCExecution(CMApproval request)
        {
            /*
             * Update the status ModuleCleaning table
             * Add history log table
            */
            return null;
        }

    }
}
