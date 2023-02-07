using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.PM
{
    public class PMRepository : GenericRepository
    {
        public PMRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

        }

        internal async Task<List<CMScheduleData>> GetScheduleData(int facility_id, int category_id)
        {
            /*
             * Primary Table - PMSchedule
             * Read All properties mention in model and return list
             * Code goes here
            */ 
            return null;
        }

        internal async Task<CMDefaultResponse> SetScheduleData(List<CMSetScheduleData> request)
        {
            /*
             * Primary Table - PMSchedule
             * Set All properties mention in model and return list
             * Code goes here
            */
            return null;
        }
    }
}
