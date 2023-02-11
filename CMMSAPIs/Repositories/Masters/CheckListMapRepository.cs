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
             * 
             * if type is Audit then return Plan_id, Audit_schedule_date and List of Checklist mapped
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CreateCheckListMap(CMCreateCheckListMap request)
        {
            /*
             * Primary Table - CheckList_Mapping
             * Insert All properties mention in model
             * 
             * If type is Audit then insert Plan_id, Audit_schedule_date and List of Checklist mapped. Other properties not required
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> UpdateCheckListMap(CMCreateCheckListMap request)
        {
            /* Primary Table - CheckList_Mapping
             * Update All properties mention in model
             * 
             * If type is Audit then Update Plan_id, Audit_schedule_date and List of Checklist mapped. Other properties not required
             * Code goes here
            */
            return null;
        }
    }
}
