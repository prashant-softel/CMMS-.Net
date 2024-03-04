using CMMSAPIs.Helper;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Data;
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
         * Work Type relevant tables
         * select * from jobworktypes;
         * select * from jobassociatedworktypes;
         * select * from worktypeassociatedtools;
         * select * from worktypemasterassets;
        */
        internal async Task<List<CMJobWorkType>> GetJobWorkTypeList(string categoryIds)
        {
            /*
             * Fetch id, categoryId, categroyName, workType from JobWorkTypes table and join AssetCategories to get CategoryName
            */

            string myQuery = "SELECT jwt.id, jwt.equipmentCategoryId as categoryId , jwt.workTypeName as workType, assetCat.name as categoryName , jwt.status as status FROM jobworktypes as jwt JOIN AssetCategories AS assetCat on jwt.equipmentCategoryId = assetCat.id WHERE jwt.status = 1 ";

            if (categoryIds != null && categoryIds != "")
            {
                myQuery += "and jwt.equipmentCategoryId IN (" + categoryIds + ")";
            }

            List<CMJobWorkType> _WorkType = await Context.GetData<CMJobWorkType>(myQuery).ConfigureAwait(false);
            return _WorkType;
        }

        internal async Task<CMDefaultResponse> CreateJobWorkType(CMADDJobWorkType request, int userID)
        {
            /*
             * Insert workTypeName, CategoryId in JobWorkTypes ask categoryName
            */
            /*Your code goes here*/
            string qryWorkTypeInsert = "insert into jobworktypes " +
                                   "( equipmentCategoryId, workTypeName , createdAt, createdBy ) values" +
                                $"({ request.categoryid }, '{ request.workType }', '{ UtilsRepository.GetUTCTime() }', { userID } );" +
                                $"SELECT LAST_INSERT_ID();";

            DataTable dt = await Context.FetchData(qryWorkTypeInsert).ConfigureAwait(false);
            int insertedId = Convert.ToInt32(dt.Rows[0][0]);

            return new CMDefaultResponse(insertedId, CMMS.RETRUNSTATUS.SUCCESS, "Job work type created");
        }

        internal async Task<CMDefaultResponse> UpdateJobWorkType(CMADDJobWorkType request, int userID)
        {
            /*
             * Update Work Type, Category id in JobWorkTypes table for requested workTypeId
            */
            /*Your code goes here*/
            string qryFacilityUpdate = $"update jobworktypes set equipmentCategoryId = { request.categoryid }, workTypeName = '{ request.workType }',  updatedAt='{ UtilsRepository.GetUTCTime() }', updatedBy={ userID } where id = { request.id } ;";

            int UpdatededId = await Context.ExecuteNonQry<int>(qryFacilityUpdate).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, $"Updated Job Work Type Details");

            return response;
        }

        internal async Task<CMDefaultResponse> DeleteJobWorkType(int id)
        {
            /*
             * Delete the record from JobWorkTypes table for requested workTypeId only if no record present in JobAssociatedWorkTypes
            */
            /*Your code goes here*/
            string DeleteQry = $"update jobworktypes set status = 0 where id = {id};";
            await Context.ExecuteNonQry<int>(DeleteQry).ConfigureAwait(false);

            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Deleted Job Work Type");

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
            string myQuery = $"SELECT tools.id as toolId, tools.assetName as toolName, workType.id as workTypeId, workType.workTypeName as workTypeName, asset_cat.name as CategoryName FROM jobs AS job JOIN jobmappingassets AS mapAssets ON mapAssets.jobId = job.id JOIN assetcategories AS asset_cat ON mapAssets.categoryId = asset_cat.id LEFT JOIN jobassociatedworktypes as mapWorkTypes on mapWorkTypes.jobId = job.id LEFT JOIN jobworktypes AS workType ON (workType.equipmentCategoryId = asset_cat.id OR mapWorkTypes.workTypeId = workType.id) LEFT JOIN worktypeassociatedtools AS mapTools ON mapTools.workTypeId=workType.id LEFT JOIN worktypemasterassets AS tools ON tools.id=mapTools.ToolId WHERE job.id = {jobId} GROUP BY toolId;";
            List<CMJobWorkTypeTool> _WorkType = await Context.GetData<CMJobWorkTypeTool>(myQuery).ConfigureAwait(false);
            return _WorkType;
        }

        internal async Task<List<CMMasterTool>> GetMasterToolList(string worktypeIds)
        {
            /*
             * Fetch id, workType, ToolName (if more than 1 tool linked concat then return) from WorkTypeAssociatedTools table 
             * and JOIN WorkTypeMaster  Assets to get ToolName
            */
            String myQuery;
            if (worktypeIds=="0" || worktypeIds == null)
            {
                myQuery = "SELECT a.id as id,jws.name as Equipment_name,wt.WorkTypeId ,jwt.equipmentCategoryId as equipmentCategoryId,jwt.workTypeName as workTypeName, a.assetName as linkedToolName FROM worktypeassociatedtools as wt JOIN jobworktypes as jwt ON jwt.id= wt.workTypeId JOIN worktypemasterassets as a ON a.id = wt.ToolId LEFT join assetcategories as jws ON jws.id=jwt.equipmentCategoryId where a.status =1 ";
                   // "a ON a.id = wt.ToolId LEFT join assetcategories as jws ON jws.id=jwt.equipmentCategoryId where a.status =1 ";
            }
            
            else 
            {
                myQuery = "SELECT a.id as id,jws.name as Equipment_name,wt.WorkTypeId ,jwt.equipmentCategoryId as equipmentCategoryId,jwt.workTypeName as workTypeName, a.assetName as linkedToolName FROM worktypeassociatedtools as wt JOIN jobworktypes as jwt ON jwt.id= wt.workTypeId JOIN worktypemasterassets as a ON a.id = wt.ToolId LEFT join assetcategories as jws ON jws.id=jwt.equipmentCategoryId";
                myQuery += " where wt.workTypeId IN  (" + worktypeIds + ") and a.status =1;";
            }
        
            List<CMMasterTool> _MasterToolList = await Context.GetData<CMMasterTool>(myQuery).ConfigureAwait(false);
            return _MasterToolList;
        }

        internal async Task<CMDefaultResponse> CreateMasterTool(CMADDJobWorkTypeTool request, int userID)
        {
           // string myQuery = $"INSERT INTO worktypemasterassets (assetName, status, createdAt, createdBy) " +
             //                   $"VALUES ('{tool_name}', 1, '{UtilsRepository.GetUTCTime()}', {userID});";

            //DataTable dt = 
         //   int insertedId =await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            //int insertedId = Convert.ToInt32(dt.Rows[0][0]);

            string myQuery = $"INSERT INTO worktypemasterassets (assetName,workTypeId,equipmentCategoryId,status, createdAt, createdBy) " +
                                $"VALUES ('{request.Toolname}',{request.workTypeId},{request.equipmentCategoryId}, 1, '{UtilsRepository.GetUTCTime()}', { userID } );" +
                                $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int insertedId = Convert.ToInt32(dt.Rows[0][0]); 
            string qryAssetsIds = $"insert into worktypeassociatedtools(workTypeId,status, ToolId, createdAt, createdBy ) value ";
            qryAssetsIds += $" ({ request.workTypeId },1,{insertedId},'{ UtilsRepository.GetUTCTime() }', { UtilsRepository.GetUserID() })";
            await Context.FetchData(qryAssetsIds).ConfigureAwait(false);
            //int insertedId1 = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(insertedId, CMMS.RETRUNSTATUS.SUCCESS, "Master tool created");
        }
        /*{   
            try
            {
                string myQuery = $"INSERT INTO worktypemasterassets (assetName, status, createdAt, createdBy) " +
                                 $"VALUES ('{name}', 1, '{UtilsRepository.GetUTCTime()}', {userID}) SELECT LAST_INSERT_ID();";

                DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);

                if (dt.Rows.Count > 0)
                {
                    int insertedId = Convert.ToInt32(dt.Rows[0][0]);
                    return new CMDefaultResponse(insertedId, CMMS.RETRUNSTATUS.SUCCESS, "Master tool created");
                }
            }
            catch (Exception ex)
            {

                return ex.Message);
            }*/
    
        //s}
         internal async Task<CMDefaultResponse> UpdateMasterTool(CMADDJobWorkTypeTool request, int userID)
        {
            string myQuery = $"UPDATE worktypemasterassets SET  assetName='{request.Toolname}',workTypeId={request.workTypeId},equipmentCategoryId={request.equipmentCategoryId},updatedAt = '{UtilsRepository.GetUTCTime()}', updatedBy = {userID} WHERE id = {request.id} ";

            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            string qryAssetsIds = $"UPDATE  worktypeassociatedtools SET  workTypeId={ request.workTypeId },updatedAt='{ UtilsRepository.GetUTCTime()}',updatedBy={userID} where ToolId={request.id}";
            await Context.ExecuteNonQry<int>(qryAssetsIds).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Master tool details updated");
        }

        internal async Task<CMDefaultResponse> DeleteMasterTool(int id)
        {
            string myQuery = $"UPDATE worktypemasterassets SET status =0 WHERE id = {id} ";

            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Master tool details updated");
        }

        //internal 

        internal async Task<CMDefaultResponse> CreateJobWorkTypeTool(CMAddJobWorkTypeTool request)
        {
            /*
             * Insert workTypeid, ToolId(s) in WorkTypeAssociatedTools
            */
            /*Your code goes here*/
            int insertedId = 0;

            string qryAssetsIds = $"insert into worktypeassociatedtools(workTypeId, ToolId, createdAt, createdBy ) value ";
                  foreach (var data in request.ToolIds)
                  {
                        qryAssetsIds += $" ({ data.workTypeId }, { data.toolId },'{ UtilsRepository.GetUTCTime() }', { UtilsRepository.GetUserID() }),";
                  }
             insertedId = await Context.ExecuteNonQry<int>(qryAssetsIds.Substring(0, (qryAssetsIds.Length - 1)) + ";").ConfigureAwait(false);

            return new CMDefaultResponse(insertedId, CMMS.RETRUNSTATUS.SUCCESS, "Tool added sucessfully for work type");
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
