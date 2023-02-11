using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Masters
{
    public class CheckListRepository : GenericRepository
    {
        public CheckListRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

        }

        internal async Task<List<CMCheckList>> GetCheckList(int facility_id, int type)
        {
            /* Table - CheckList_Number
             * supporting table - AssetCategory - to get Category Name, Frequency - To get Frequency Name
             * Read All properties from above table and return the list
             * Code goes here
             * 
             * If type is Audit then we need to return only checklist number and Is active or not. Category and other information is not required
            */
            return null;
        }

        internal async Task<CMDefaultResponse> CreateChecklist(CMCreateCheckList request)
        {
            /*
             * Table - CheckList_Number
             * Insert all properties in CMCreateCheckList model to CheckList_Number
             * 
             * If type is Audit then we need to insert only checklist number. Category and other information is not required
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> UpdateCheckList(CMCreateCheckList request)
        {
            /*
             * Update the changed value in CheckList_Number for requested id
             * 
             *  If type is Audit then we need to update only checklist number. Category and other information is not required
             * Code goes here
            */
            return null;
        }

        internal async Task<CMDefaultResponse> DeleteChecklist(int id)
        {
            /* 
             * Set Status to 0 in CheckList_Number table for requested id
             * Code goes here
            */
            return null;
        }
    }
}
