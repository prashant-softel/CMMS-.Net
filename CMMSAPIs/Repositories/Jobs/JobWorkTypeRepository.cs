using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Jobs
{
    public class JobWorkTypeRepository : GenericRepository
    {
        public JobWorkTypeRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
        }

        /*
         * Work Type CRUD Operation
        */
        internal Task<List<JobWorkType>> GetJobWorkTypeList()
        {
            /*
             * Fetch id, categoryId, categroyName, workType from JobWorkTypes table and join AssetCategories to get CategoryName
            */
            /*Your code goes here*/
            return null;
        }

        internal Task<List<JobWorkType>> CreateJobWorkType()
        {
            /*
             * Insert workTypeName, CategoryId in JobWorkTypes
            */
            /*Your code goes here*/
            return null;
        }

        internal Task<List<JobWorkType>> UpdateJobWorkType()
        {
            /*
             * Update Work Type, Category id in JobWorkTypes table for requested workTypeId
            */
            /*Your code goes here*/
            return null;
        }

        internal Task<List<JobWorkType>> DeleteJobWorkType()
        {
            /*
             * Delete the record from JobWorkTypes table for requested workTypeId only if no record present in JobAssociatedWorkTypes
            */
            /*Your code goes here*/
            return null;
        }


        /*
         * Tool Associated to Work Type CRUD Operation ****
        */



        internal Task<List<JobWorkTypeTool>> GetJobWorkTypeToolList()
        {
            /*
             * Fetch id, categoryId, categroyName, workType from worktypeassociatedtools table and join AssetCategories to get CategoryName
            */
            /*Your code goes here*/
            return null;
        }

        internal Task<List<MasterTool>> GetMasterToolList()
        {
            /*
             * Fetch id, workType, ToolName (if more than 1 tool linked concat then return) from WorkTypeAssociatedTools table 
             * and JOIN WorkTypeMasterAssets to get ToolName
            */
            /*Your code goes here*/
            return null;
        }

        internal Task<List<JobWorkTypeTool>> CreateJobWorkTypeTool()
        {
            /*
             * Insert workTypeid, ToolId(s) in WorkTypeAssociatedTools
            */
            /*Your code goes here*/
            return null;
        }

        internal Task<List<JobWorkTypeTool>> UpdateJobWorkTypeTool()
        {
            /*
             * Update workTypeid, ToolId(s) in WorkTypeAssociatedTools table for requested AssociatedToolId
            */
            /*Your code goes here*/
            return null;
        }

        internal Task<List<JobWorkTypeTool>> DeleteJobWorkTypeTool()
        {
            /*
             * Delete the record from WorkTypeAssociatedTools table for requested AssociatedToolId.
            */
            /*Your code goes here*/
            return null;
        }
    }
}
