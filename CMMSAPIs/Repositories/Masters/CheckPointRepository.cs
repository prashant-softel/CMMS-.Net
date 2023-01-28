using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Masters
{
    public class CheckPointRepository : GenericRepository
    {
        public CheckPointRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

        }

        internal async Task<List<CMCheckPointList>> GetCheckPointList(int checklist_id)
        {
            /*
             * Primary Table - CheckPoint
             * Supporting table - Checklist_Number - to get checklist name
             * Read All properties mention in CMCheckPointList and return list
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CreateCheckPoint(CMCreateCheckPoint request)
        {
            /*
             * Primary Table - CheckPoint
             * Insert all properties mention in model to CheckPoint table
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> UpdateCheckPoint(CMCreateCheckPoint request)
        {
            /*
             * Primary Table - CheckPoint
             * Update all properties mention in model to CheckPoint table for requisted id
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> DeleteCheckPoint(int id)
        {
            /*
             * Primary Table - CheckPoint
             * Set status 0 for requested id in CheckPoint table
             * Code goes here
            */
            return null;
        }
    }
}
