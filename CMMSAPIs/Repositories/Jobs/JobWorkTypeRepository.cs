using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
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
        internal async Task<List<CMJobWorkType>> GetJobWorkTypeList()
        {
            /*
             * Fetch id, categoryId, categroyName, workType from JobWorkTypes table and join AssetCategories to get CategoryName
            */
            /*Your code goes here*/

            string myQuery = "SELECT jwt.id, jwt.equipmentCategoryId as categoryId , jwt.workTypeName as workType, assetCat.name as categoryName , jwt.status as status FROM jobworktypes as jwt JOIN AssetCategories AS assetCat on jwt.equipmentCategoryId = assetCat.id WHERE jwt.status = 1";
            List<CMJobWorkType> _WorkType = await Context.GetData<CMJobWorkType>(myQuery).ConfigureAwait(false);
            return _WorkType;
        }

        internal async Task<int> CreateJobWorkType(CMADDJobWorkType request)
        {
            /*
             * Insert workTypeName, CategoryId in JobWorkTypes ask categoryName
            */
            /*Your code goes here*/
            string qryWorkTypeInsert = "insert into jobworktypes " +
                                   "( equipmentCategoryId, workTypeName , createdAt, createdBy ) values" +
                                $"({ request.categoryid }, '{ request.workType }', '{ UtilsRepository.GetUTCTime() }', { UtilsRepository.GetUserID() } )";

            int insertedId = await Context.ExecuteNonQry<int>(qryWorkTypeInsert).ConfigureAwait(false);

            return insertedId;
        }

        internal async Task<CMDefaultResponse> UpdateJobWorkType(CMUpdateJobWorkType request)
        {
            /*
             * Update Work Type, Category id in JobWorkTypes table for requested workTypeId
            */
            /*Your code goes here*/
            string qryFacilityUpdate = $"update jobworktypes set equipmentCategoryId = { request.categoryid }, workTypeName = '{ request.workType }',  updatedAt='{ UtilsRepository.GetUTCTime() }', updatedBy={ UtilsRepository.GetUserID() } where id = { request.id } ;";

            int UpdatededId = await Context.ExecuteNonQry<int>(qryFacilityUpdate).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Update Work Type");

            return response;
        }

        internal async Task<CMDefaultResponse> DeleteJobWorkType(int id)
        {
            /*
             * Delete the record from JobWorkTypes table for requested workTypeId only if no record present in JobAssociatedWorkTypes
            */
            /*Your code goes here*/
            string DeleteQry = $"delete from jobworktypes where id = {id};";
            await Context.ExecuteNonQry<int>(DeleteQry).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Delete Work Type List");

            return response;
        }


        /*
         * Tool Associated to Work Type CRUD Operation ****
        */



        internal async Task<List<CMJobWorkTypeTool>> GetJobWorkTypeToolList(int jobId)
        {
            /*
             * Fetch id, categoryId, categroyName, workType from worktypeassociatedtools table and join AssetCategories to get CategoryName
            */
            /*Your code goes here*/
            string myQuery = "SELECT a.otherWorkTypeName as toolName, a.workTypeId as workTypeId, b.workTypeName as workTypeName, c.name as CategoryName FROM jobassociatedworktypes as a LEFT JOIN jobworktypes as b ON a.workTypeId = b.id LEFT JOIN  assetcategories as c ON c.id = b.equipmentCategoryId where a.jobId =" + jobId;
            List<CMJobWorkTypeTool> _WorkType = await Context.GetData<CMJobWorkTypeTool>(myQuery).ConfigureAwait(false);
            return _WorkType;
        }

        internal async Task<List<CMMasterTool>> GetMasterToolList(int id)
        {
            /*
             * Fetch id, workType, ToolName (if more than 1 tool linked concat then return) from WorkTypeAssociatedTools table 
             * and JOIN WorkTypeMasterAssets to get ToolName
            */
            /*Your code goes here*/
       /*     string myQuery = "SELECT wt.id as id, jwt.workTypeName as workTypeName, group_concat(a.assetName SEPARATOR ', ') as linkedToolName FROM worktypeassociatedtools as wt LEFT JOIN jobworktypes as jwt ON jwt.id = wt.workTypeId LEFT JOIN worktypemasterassets as a ON FIND_IN_SET(a.id, wt.ToolId) where wt.id = " + id + " group by wt.workTypeId";*/

            string myQuery = "SELECT wt.id as id, jwt.workTypeName as workTypeName, group_concat(a.assetName SEPARATOR ', ') as linkedToolName FROM worktypeassociatedtools as wt LEFT JOIN jobworktypes as jwt ON jwt.id = wt.workTypeId LEFT JOIN worktypemasterassets as a ON a.id = wt.ToolId where wt.workTypeId = " + id + " group by wt.workTypeId";
            List<CMMasterTool> _MasterToolList = await Context.GetData<CMMasterTool>(myQuery).ConfigureAwait(false);
            return _MasterToolList;
        }

        internal async Task<int> CreateJobWorkTypeTool(CMAddJobWorkTypeTool request)
        {
            /*
             * Insert workTypeid, ToolId(s) in WorkTypeAssociatedTools
            */
            /*Your code goes here*/
            int inseartedId = 0;

            string qryAssetsIds = $"insert into worktypeassociatedtools(workTypeId, ToolId, createdAt, createdBy ) value ";
                  foreach (var data in request.ToolIds)
                  {
                        qryAssetsIds += $" ({ data.workTypeId }, { data.toolId },'{ UtilsRepository.GetUTCTime() }', { UtilsRepository.GetUserID() }),";
                  }
             inseartedId = await Context.ExecuteNonQry<int>(qryAssetsIds.Substring(0, (qryAssetsIds.Length - 1)) + ";").ConfigureAwait(false);

            return inseartedId;
        }

        internal async Task<CMDefaultResponse> UpdateJobWorkTypeTool(CMUpdateJobWorkTypeTool request)
        {
            /*
             * Update workTypeid, ToolId(s) in WorkTypeAssociatedTools table for requested AssociatedToolId
            */
            /*Your code goes here*/
            string DeleteQry = $"delete from worktypeassociatedtools where workTypeId = { request.id };";
            await Context.ExecuteNonQry<int>(DeleteQry).ConfigureAwait(false);

            string qryAssetsIds = $"insert into worktypeassociatedtools(workTypeId, ToolId, updatedAt, updatedBy ) value ";
                foreach (var data in request.ToolIds)
                {
                    qryAssetsIds += $"({ data.workTypeId }, { data.toolId },'{ UtilsRepository.GetUTCTime() }', { UtilsRepository.GetUserID() }),";               
                }
             await Context.ExecuteNonQry<int>(qryAssetsIds.Substring(0, (qryAssetsIds.Length - 1)) + ";").ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Update Work Type Tool");

            return response;
        }

        internal Task<CMDefaultResponse> DeleteJobWorkTypeTool(int id)
        {
            /*
             * Delete the record from WorkTypeAssociatedTools table for requested AssociatedToolId.
            */
            /*Your code goes here*/
            return null;
        }
    }
}
