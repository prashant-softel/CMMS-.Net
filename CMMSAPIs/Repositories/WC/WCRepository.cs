using CMMSAPIs.Helper;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.WC
{
    public class WCRepository : GenericRepository
    {
        public WCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        internal async Task<CMWCList> GetWCList(int facility_id)
        {
            /*
             * Tables
             * WC - Primary Table
             * WCSchedules - Supplier Action record
             * Facility - Facility Related data
             * AssetCategories - Asset Category Name
             * Assets - Asset Name
             * Users - User related info
             * Business - Supplier info
             * Fetch all data present in CMWCList Model
            */
            /*Your code goes here*/
            return null;
        }

        internal async Task<CMDefaultResponse> CreateWC(CMWCCreate request)
        {
            /*
             * Insert all data in WC table in their respective columns 
            */
            /*Your code goes here*/
            return null;
        }

        internal async Task<CMWCDetail> ViewWC(int wc_id)
        {
            /*
             * Tables
             * WC - Primary Table
             * WCSchedules - Supplier Action record
             * Facility - Facility Related data
             * AssetCategories - Asset Category Name
             * Assets - Asset Name
             * Users - User related info
             * Business - Supplier info
             * History - Action Logs
             * Fetch all data present in CMWCDetail Model
            */
            /*Your code goes here*/
            return null;
        }
    }
}
