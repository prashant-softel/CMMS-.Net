using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Masters
{
    public class CheckListMapRepository : GenericRepository
    {
        public CheckListMapRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

        }

        internal async Task<List<CMCheckListMapList>> GetCheckListMap(int facility_id, int type)
        {
            /*
             * Primary Table - CheckList_Mapping
             * Read All properties mention in model and return list
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CreateCheckListMap(CMCreateCheckListMap request)
        {
            /*
             * Primary Table - CheckList_Mapping
             * Insert All properties mention in model
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> UpdateCheckListMap(CMCreateCheckListMap request)
        {
            /* Primary Table - CheckList_Mapping
             * Update All properties mention in model
             * Code goes here
            */
            return null;
        }
    }
}
