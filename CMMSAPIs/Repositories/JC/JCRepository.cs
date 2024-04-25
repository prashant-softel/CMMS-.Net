using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Repositories.Permits;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Models.Notifications;


namespace CMMSAPIs.Repositories.JC
{
    public class JCRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        private MYSQLDBHelper _conn;
        public JCRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            _conn = sqlDBHelper;
        }
        internal static string getShortStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID, CMMS.ApprovalStatus approval_id)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:     
                    retValue = "JC Created";
                    break;
                case CMMS.CMMS_Status.JC_STARTED:     
                    retValue = "JC Started";
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue = "JC Closed - Waiting for Approval";                  
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_REJECTED:
                    retValue = "JC Closed - Rejected";
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_APPROVED:
                    retValue = "JC Closed";
                    break;             
                case CMMS.CMMS_Status.JC_CARRY_FORWARDED:     
                    retValue = "JC Carry FWD - Waiting for Approval";               
                   break;
                case CMMS.CMMS_Status.JC_CF_APPROVED:
                    retValue = "JC Carry Forwarded";
                    break;
                case CMMS.CMMS_Status.JC_CF_REJECTED:
                    retValue = "JC Carry FWD - Rejected";
                    break;

                //case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:     
                //    retValue = "PTW Timed Out";
                //    break;

                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }       

        internal string getLongStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status notificationID, CMJCDetail jobObj)
        {
            string retValue = "My job subject";
            int jobId = jobObj.id;

            switch (notificationID)
            {
                case CMMS.CMMS_Status.JC_CREATED:
                    retValue = String.Format("Job Card Created at {0}", jobObj.created_at);
                    break;
                case CMMS.CMMS_Status.JC_STARTED:
                    retValue = String.Format("Job Card Started by at {0}", jobObj.UpdatedByName);
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue = String.Format("Job Card Closed by {0} - Waiting for approval", jobObj.JC_Closed_by_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_APPROVED:
                    retValue = String.Format("Job Card Approved by {0}", jobObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_REJECTED:
                    retValue = String.Format("Job Card Rejected by {0}", jobObj.JC_Rejected_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CARRY_FORWARDED:
                    retValue = String.Format("Job Card Carry Forward by {0} - Waiting for approval ", jobObj.JC_Start_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_APPROVED:
                    retValue = String.Format("Job Card Carry Forward Approved by {0}", jobObj.JC_Approved_By_Name);
                    break;
                case CMMS.CMMS_Status.JC_CF_REJECTED:
                    retValue = String.Format("Job Card Carry Forward Rejected by {0}", jobObj.JC_Rejected_By_Name);
                    break;  
                //case CMMS.CMMS_Status.JC_APPROVED:
                //    retValue = String.Format("Job Card Approved by {0}", jobObj.JC_Approved_By_Name);
                //    break;
                //case CMMS.CMMS_Status.JC_REJECTED5:
                //    retValue = String.Format("Job card Rejected by {0}", jobObj.JC_Rejected_By_Name);
                //    break;
                //case CMMS.CMMS_Status.JC_PTW_TIMED_OUT:
                //    retValue = String.Format("PTW <{0}> Timed Out", jobObj.ptwId);
                //    break;
                default:
                    break;
            }
            return retValue;

        }

        internal async Task<List<CMJCList>> GetJCList(int facility_id, int userID, bool self_view,string facilitytimeZone)
        {
            /* Return all field mentioned in JCListModel model
            *  tables are JobCards, Jobs, Permit, Users
            */
            var checkFilter = 0;

            /*Your code goes here*/
            string myQuery1 = $"select jc.id as jobCardId,jc.JC_Status as status ,jc.JC_Approved as approvedStatus, jc.JC_Date_Start as job_card_date, jc.JC_Date_Start as start_time,jc.JC_Date_Stop as end_time, job.id as jobid, job.title as description, CONCAT(user.firstName, user.lastName) as job_assinged_to,  ptw.id as permit_id, ptw.code as permit_no,JC_Status as current_status  from jobcards as jc JOIN jobs as job ON JC.jobid = job.id JOIN permits as ptw ON JC.PTW_id = PTW.ID LEFT JOIN users as user ON user.id = job.assignedId ";
                                //$"LEFT JOIN  users as user2 ON user2.id = jc.JC_Added_by " +
                                //$"LEFT JOIN  users as user3 ON user3.id = jc.JC_Start_By_id " ;

            if (facility_id > 0)
            {
                myQuery1 += $"WHERE job.facilityId = { facility_id } ";
                checkFilter = 1;

                if (self_view)
                    myQuery1 += $"AND ( job.assignedId = {userID}  ) ";
            }
            else
            {
                throw new ArgumentException("Invalid Facility ID");
            }
            

            List<CMJCList> _ViewJobCardList = await Context.GetData<CMJCList>(myQuery1).ConfigureAwait(false);

            foreach (var jc in _ViewJobCardList)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(jc.status);
                CMMS.ApprovalStatus _Approval = (CMMS.ApprovalStatus)(jc.approvedStatus);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOBCARD, _Status, _Approval);
                jc.status_short = _shortStatus;       
            }
            //job equipment category
            /* string myQuery2 = $"SELECT asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name  FROM assetcategories as asset_cat JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id = {_ViewJobCardList[0].jobid } and job.facilityId = { facility_id }";
             List<equipmentCatList> _equipmentCatList = await Context.GetData<equipmentCatList>(myQuery2).ConfigureAwait(false);

             _ViewJobCardList[0].LstequipmentCatList = _equipmentCatList;*/
            foreach (var v in _ViewJobCardList)
            {
                
                    if (v != null && v.job_card_date != null)
                    v.job_card_date = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, v.job_card_date);
                if (v != null && v.start_time != null)
                    v.start_time = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, v.start_time);
                if (v != null && v.end_time != null)
                    v.end_time = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, v.end_time);


            }
            return _ViewJobCardList;
        }
        internal async Task<List<CMJCListForJob>> GetJCListByJobId(int jobId,string facilitytimeZone)
        {
           

            /*Your code goes here*/
            string myQuery1 = $"select jc.id as jobCardId,jc.JC_code as jobCardNo, jc.JC_Date_Start as jobCardDate,jc.JC_Date_Stop as endTime, job.id as jobId, CONCAT(user.firstName, user.lastName) as jobAssingedTo,  ptw.id as permitId, ptw.code as permitNo,JC_Status as status, JC_Approved as approvedStatus from jobcards as jc LEFT JOIN jobs as job ON JC.jobid = job.id LEFT JOIN permits as ptw ON JC.PTW_id = PTW.ID LEFT JOIN users as user ON user.id = job.assignedId ";
            //$"LEFT JOIN  users as user2 ON user2.id = jc.JC_Added_by " +
            //$"LEFT JOIN  users as user3 ON user3.id = jc.JC_Start_By_id " ;

            if (jobId > 0)
            {
                myQuery1 += $"WHERE JC.jobid = { jobId } ";
            }
            else
            {
                throw new ArgumentException("Invalid Job ID");
            }

            List<CMJCListForJob> _ViewJobCardList = await Context.GetData<CMJCListForJob>(myQuery1).ConfigureAwait(false);

            foreach (var jc in _ViewJobCardList)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(jc.status);
                CMMS.ApprovalStatus _Approval = (CMMS.ApprovalStatus)(jc.approvedStatus);
                string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOBCARD, _Status, _Approval);
                jc.status_short = _shortStatus;
            }
            //job equipment category
            /* string myQuery2 = $"SELECT asset_cat.id as equipmentCat_id, asset_cat.name as equipmentCat_name  FROM assetcategories as asset_cat JOIN jobmappingassets as mapAssets ON mapAssets.categoryId = asset_cat.id JOIN jobs as job ON mapAssets.jobId = job.id WHERE job.id = {_ViewJobCardList[0].jobid } and job.facilityId = { facility_id }";
             List<equipmentCatList> _equipmentCatList = await Context.GetData<equipmentCatList>(myQuery2).ConfigureAwait(false);

             _ViewJobCardList[0].LstequipmentCatList = _equipmentCatList;*/
            foreach (var list in _ViewJobCardList)
            {
                if (list != null && list.endTime != null)
                    list.endTime = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone,list.endTime);
                if (list != null && list.jobCardDate != null)
                    list.jobCardDate = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, list.jobCardDate);
            }
            return _ViewJobCardList;

        }

      /*  internal async Task<CMDefaultResponse> StartJC(int jc_id, int userID)
        {
            string myQuery = $"UPDATE jobcards SET JC_Status = {(int)CMMS.CMMS_Status.JC_STARTED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Date_Start = '{UtilsRepository.GetUTCTime()}', JC_Start_By_id = {userID} WHERE id = {jc_id};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {jc_id}";

            List<CMJCDetail> _jcDetails = await GetJCDetail(jc_id);

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, jc_id, 0, 0, "Job Card Started", CMMS.CMMS_Status.JC_STARTED,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_STARTED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(jc_id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Started");

            return response;
        }*/
        internal async Task<CMDefaultResponse> StartJC(int jc_id, CMJCDetail request, int userID)
        {

            string myQuery = $"UPDATE jobcards SET JC_Status = {(int)CMMS.CMMS_Status.JC_STARTED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Date_Start = '{UtilsRepository.GetUTCTime()}', JC_Start_By_id = {userID} WHERE id = {jc_id};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            //EMP LIST
          
           

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {jc_id}";

            List<CMJCDetail> _jcDetails = await GetJCDetail(jc_id);
            //upload Images
            if (request.uploadfile_ids != null)
            {
                foreach (int data in request.uploadfile_ids)
                {
                    int id = 380;
                    
                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {id}, module_type={(int)CMMS.CMMS_Modules.JOBCARD},module_ref_id={jc_id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }
          

            foreach (var data in request.LstCMJCEmpList)
            {
                string mapChecklistQry = $"insert into  permitemployeelists (employeeId,responsibility,JC_id) VALUES ";

                mapChecklistQry += $"({data.id}, '{data.responsibility}', {jc_id})";
                await Context.ExecuteNonQry<int>(mapChecklistQry).ConfigureAwait(false);
            }
            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, jc_id, 0, 0, "Job Card Started", CMMS.CMMS_Status.JC_STARTED,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_STARTED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(jc_id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Started");

            return response;
        }
       
        internal async Task<List<CMJCDetail>> GetJCDetail(int jc_id)
        {
            /*
             * Fetch data from JobCards table and joins these table for relationship using ids 
             * Users, Assets, AssetCategory, Facility, PermiEmployeeLists, PermitLotoAssets, PermitIsolatedAssetCategories
             * Return all the field listed in JCDetailModel 
            */

            //plant details 
            string myQuery1 = $"SELECT distinct(jc.id ) as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name,fc.name as block_name, " +
                                $"asset_cat.name as asset_category_name, jc.JC_title as title , jc.JC_Description as description, " +
                                $"job.assignedId as currentEmpID, jc.JC_Added_Date as created_at, jc.JC_Date_Start as JC_Start_At, " +
                                $"jc.JC_Approved as JC_Approved, jc. JC_Status as status, " +
                                $"CONCAT(user.firstName , ' ' , user.lastName) as UpdatedByName, " +
                                $"CONCAT(user1.firstName , ' ' , user1.lastName) as JC_Approved_By_Name, " +
                                $"CONCAT(user2.firstName , ' ' , user2.lastName) as created_by, " +
                                $"CONCAT(user3.firstName , ' ' , user3.lastName) as JC_Start_By_Name, " +
                                $"CONCAT(user4.firstName , ' ' , user4.lastName) as JC_Closed_by_Name " +
                                $"FROM jobs as job " +
                                $"LEFT JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                                $"Left join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                                $"JOIN facilities as facilities ON job.facilityId= facilities.id " +
                                $"JOIN facilities as fc ON job.blockId = fc.id "+
                                $"LEFT JOIN jobcards as jc on jc.jobId = job.id " +
                                $"LEFT JOIN users as user ON user.id = jc.JC_Update_by " +
                                $"LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id " +
                                $"LEFT JOIN  users as user2 ON user2.id = jc.JC_Added_by " +
                                $"LEFT JOIN  users as user3 ON user3.id = jc.JC_Start_By_id " +
                                $"LEFT JOIN  users as user4 ON user4.id = jc.JC_End_By_id " +
                                $"where jc.id = { jc_id }";

            List<CMJCDetail> _plantDetails = await Context.GetData<CMJCDetail>(myQuery1).ConfigureAwait(false);
            if (_plantDetails.Count == 0)
                throw new MissingMemberException($"Job Card with ID {jc_id} not found");

            //job details
             string myQuery2 = $"SELECT job.id as job_id ,job.status as status, job.title as job_title , CONCAT(user.firstName, user.lastName) as job_assigned_employee_name , job.description as job_description , workType.workTypeName as work_type FROM jobs as job JOIN jobmappingassets as mapAssets ON mapAssets.jobId = job.id JOIN assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id LEFT JOIN jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id JOIN facilities as facilities ON job.facilityId = facilities.id LEFT JOIN users as user ON user.id = job.assignedId JOIN jobcards as jc on jc.jobId = job.id where jc.id = {jc_id}";
           /* string myQuery2 = $"SELECT distinct(tools.id) as toolId, job.id as job_id ,job.status as status, job.title as job_title , CONCAT(user.firstName, user.lastName) as job_assigned_employee_name , job.description as job_description , workType.workTypeName as work_type, tools.assetName as toolName"+
                      "FROM jobs as job JOIN jobmappingassets as mapAssets ON mapAssets.jobId = job.id JOIN assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id" +
                      "LEFT JOIN jobworktypes as workType ON workType.equipmentCategoryId = asset_cat.id JOIN facilities as facilities ON job.facilityId = facilities.id" +
                      "JOIN assets ON mapAssets.assetId = assets.id" +
                      "LEFT JOIN worktypeassociatedtools AS mapTools ON mapTools.workTypeId = workType.id" +
                      "LEFT JOIN worktypemasterassets AS tools ON tools.id = mapTools.ToolId" +
                      "LEFT JOIN users as user ON user.id = job.assignedId JOIN jobcards as jc on jc.jobId = job.id "+
                     $"where jc.id = { jc_id }";*/

            List<CMJCJobDetail> _jobDetails = await Context.GetData<CMJCJobDetail>(myQuery2).ConfigureAwait(false);
            int id=0;
            foreach (var job in _jobDetails)
            {
                id = job.job_id;
                job.status_short = JobRepository.getShortStatus(CMMS.CMMS_Modules.JOB, (CMMS.CMMS_Status)job.status);
            }
            


            //permit details
            string myQuery3 = $"SELECT ptw.id as permit_id,ptw.status as status, ptw.permitNumber as site_permit_no, permitType.title as permit_type, ptw.description as permit_description, CONCAT(user.firstName, user.lastName) as job_created_by_name, CONCAT(user1.firstName , ' ' , user1.lastName) as permit_issued_by_name, CONCAT(user2.firstName , ' ' , user2.lastName) as permit_approved_by_name,ptw.TBT_Done_Check as TBT_Done_Check   FROM permits as ptw LEFT JOIN permittypelists as permitType ON permitType.id = ptw.typeId JOIN jobs as job ON ptw.id = job.linkedPermit LEFT JOIN users as user ON user.id = job.assignedId LEFT JOIN users as user1 ON user1.id = ptw.issuedById LEFT JOIN users as user2 ON user2.id = ptw.approvedById JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = { jc_id}";
             List<CMJCPermitDetail> _permitDetails = await Context.GetData<CMJCPermitDetail>(myQuery3).ConfigureAwait(false);
            
            foreach (var permit in _permitDetails)
            {
                permit.status_short = PermitRepository.Status(permit.status);
            }

            // isolated details
            string myQuery4 = $"SELECT asset_cat.name as isolated_assestName FROM permitisolatedassetcategories AS ptwISOCat LEFT JOIN assetcategories as asset_cat  ON ptwISOCat.assetCategoryId = asset_cat.id JOIN jobcards as jc on ptwISOCat.permitId = jc.PTW_id  where jc.id = { jc_id }";
            List<CMJCIsolatedDetail> _isolatedDetails = await Context.GetData<CMJCIsolatedDetail>(myQuery4).ConfigureAwait(false);
           
            //loto list
            string myQuery5 = $"SELECT  assets_cat.name as isolated_assest_loto FROM assetcategories as assets_cat LEFT JOIN permits as ptw on ptw.id = assets_cat.id LEFT JOIN permitlotoassets AS LOTOAssets on LOTOAssets.PTW_id = ptw.id JOIN jobcards as jc on jc.jobId = ptw.id where jc.id = { jc_id }";
            List<CMJCLotoDetail> _lotoList = await Context.GetData<CMJCLotoDetail>(myQuery5).ConfigureAwait(false);

            // emp list

            //string myQuery6 = $" SELECT CONCAT(user.firstName,' ',user.lastName) as empName, ptwEmpList.responsibility as resp FROM permitemployeelists as ptwEmpList JOIN permits as ptw ON ptw.id = ptwEmpList.pwtId LEFT JOIN users as user ON user.id = ptwEmpList.employeeId JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = { jc_id }";
            string myQuery6 = $" SELECT CONCAT(user.firstName,' ',user.lastName) as name,ptwEmpList.employeeId as id ,ptwEmpList.responsibility as responsibility FROM permitemployeelists as ptwEmpList  LEFT JOIN users as user ON user.id = ptwEmpList.employeeId  where ptwEmpList.JC_id ={ jc_id } and ptwEmpList.status=1";
            List<CMJCEmpDetail> _empList = await Context.GetData<CMJCEmpDetail>(myQuery6).ConfigureAwait(false);
            //toollist
             string myQuery7 = "SELECT tools.id as toolId, tools.assetName as toolName FROM jobs AS job " +
               "JOIN jobmappingassets AS mapAssets ON mapAssets.jobId = job.id " +
               "JOIN assets ON mapAssets.assetId = assets.id " +
               "JOIN assetcategories AS asset_cat ON assets.categoryId = asset_cat.id " +
               "LEFT JOIN jobassociatedworktypes as mapWorkTypes on mapWorkTypes.jobId = job.id " +
               "LEFT JOIN jobworktypes AS workType ON mapWorkTypes.workTypeId = workType.id " +
               "LEFT JOIN worktypeassociatedtools AS mapTools ON mapTools.workTypeId=workType.id " +
               "LEFT JOIN worktypemasterassets AS tools ON tools.id=mapTools.ToolId " +
               $"WHERE job.latestJC= {jc_id} GROUP BY tools.id";
            List<CMWorkTypeTools> _ToolsLinked = await Context.GetData<CMWorkTypeTools>(myQuery7).ConfigureAwait(false);

            // file upload 
            //  string myQuery7 = $"SELECT jc.id as id, PTWFiles.File_Name as fileName, PTWFiles.File_Category_name as fileCategory, PTWFiles.File_Size as fileSize, PTWFiles.status as status FROM st_ptw_files AS PTWFiles LEFT JOIN permits as ptw on  ptw.id = PTWFiles.PTW_id JOIN jobcards as jc on jc.PTW_id = ptw.id where jc.id = { jc_id }";
           // string q = "select jobId from  WHERE id ="+jc_id;
            //int job_id   = await Context.FetchData<int>(q).ConfigureAwait(false);
            string myQuery17 = "SELECT jc.id as id, file_path as fileName,  U.File_Size as fileSize, U.status,U.description FROM uploadedfiles AS U "+ 
                              "Left JOIN jobcards as jc on jc.jobid = U.module_ref_id  " +                             
                              "where module_ref_id =" + id+ " and U.module_type = " + (int)CMMS.CMMS_Modules.JOB+ ";";
            
            List<CMFileDetail> _fileUpload = await Context.GetData<CMFileDetail>(myQuery17).ConfigureAwait(false);
            //uploadjobcard
            string myQuery18 = "SELECT jc.id as id, file_path as fileName,  U.File_Size as fileSize, U.status,U.description FROM uploadedfiles AS U " +
                              "Left JOIN jobcards as jc on jc.jobid = U.module_ref_id  " +
                              "where module_ref_id =" + jc_id + " and U.module_type = " + (int)CMMS.CMMS_Modules.JOB + ";";

            List<CMFileDetailJc> Jc_image = await Context.GetData<CMFileDetailJc>(myQuery18).ConfigureAwait(false);

            _plantDetails[0].LstCMJCJobDetailList = _jobDetails;
            _plantDetails[0].LstPermitDetailList = _permitDetails;
            _plantDetails[0].LstCMJCIsolatedDetailList = _isolatedDetails;
            _plantDetails[0].LstCMJCLotoDetailList = _lotoList;
            _plantDetails[0].LstCMJCEmpList = _empList;
            _plantDetails[0].file_list = _fileUpload;
            _plantDetails[0].tool_List = _ToolsLinked;
            _plantDetails[0].file_listJc = Jc_image;



            CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_plantDetails[0].status);
            CMMS.ApprovalStatus _Approval = (CMMS.ApprovalStatus)(_plantDetails[0].JC_Approved);
            string _shortStatus = getShortStatus(CMMS.CMMS_Modules.JOBCARD, _Status, _Approval);
            _plantDetails[0].status_short = _shortStatus;

            CMMS.CMMS_Status _Status_long = (CMMS.CMMS_Status)(_plantDetails[0].status);
            string _longStatus = getLongStatus(CMMS.CMMS_Modules.JOBCARD, _Status_long, _plantDetails[0]);
            _plantDetails[0].status_long = _longStatus;
           
            return _plantDetails;
        }

        //internal async Task<CMDefaultResponse> CreateJC(int job_id, int userID)
        //{
        //    /* 
        //     * Data will be inserted in Following tables
        //     * JobCards - Primary table (All basic details in inserted)
        //     * PermiEmployeeLists - All jc linked employee
        //     * JCFiles - All uploaded file data
        //     * Please check the JobCard above tables to get idea what values need to insert. 
        //     * Get details from job, permit and add it to Jobcards table
        //     * Return the All Properties in DefaultResponse model
        //    */

        //    //jc basic details
        //    string myQuery = $"SELECT job.id as job_id, ptw.id as ptw_id  FROM permits AS ptw JOIN jobs as job on job.linkedPermit = ptw.id where job.id = { job_id }";
        //    List<CMJCCreate> _JobList = await Context.GetData<CMJCCreate>(myQuery).ConfigureAwait(false);

        //    int jc_id = 0;
        //    string jobQuery = $"SELECT title as job_title, description as job_description,facilityId as facility_id FROM jobs where id = {job_id}";
        //    List<CMJCJobDetail> job1 = await Context.GetData<CMJCJobDetail>(jobQuery).ConfigureAwait(false);
        //    //_JobList.Count(); pending: add check for only one record is returned

        //    foreach (var data in _JobList)
        //    {
        //        int ptw_id = data.ptw_id;
        //        //JC Already exist. Return same one
        //        string jcQuery = $"SELECT id as jc_id FROM jobcards where jobId = { job_id } and PTW_id = { ptw_id } ";
        //        List<CMJCCreate> jcList = await Context.GetData<CMJCCreate>(jcQuery).ConfigureAwait(false);
        //         //jcList
        //        if (jcList.Count > 0)
        //        {
        //            CMDefaultResponse response = new CMDefaultResponse(jcList[0].jc_id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Exists");
        //            return response;
        //        }
        //        else
        //        {
        //            //create new JC
        //            string qryJCBasic = "insert into jobcards" +
        //                                 "(" +
        //                                   "jobId, PTW_id, PTW_Code, JC_title, JC_Description, JC_Added_by, JC_Added_Date, JC_Status, status_updated_at, Facility_id " +
        //                                  ")" +
        //                                  " values" +
        //                                 "(" +
        //                                   $"{ data.job_id }, { data.ptw_id }, 'PTW{ data.ptw_id }', '{ job1[0].job_title }', '{ job1[0].job_description }', {userID}, '{ UtilsRepository.GetUTCTime() }', {(int)CMMS.CMMS_Status.JC_CREATED}, '{UtilsRepository.GetUTCTime()}', {job1[0].facility_id} " +
        //                                 "); SELECT LAST_INSERT_ID();";

        //            DataTable dt = await Context.FetchData(qryJCBasic).ConfigureAwait(false);
        //            jc_id = Convert.ToInt32(dt.Rows[0][0]);
        //        }
        //    }
        //    CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

        //    if (jc_id > 0)
        //    {
        //        string jcCodeQry = "UPDATE jobcards SET JC_Code = CONCAT('JC',id);";
        //        await Context.ExecuteNonQry<int>(jcCodeQry).ConfigureAwait(false);
        //        string latestJCQry = $"UPDATE jobs SET latestJC = {jc_id} WHERE id = {job_id};";
        //        await Context.ExecuteNonQry<int>(latestJCQry).ConfigureAwait(false);
        //        retCode = CMMS.RETRUNSTATUS.SUCCESS;

        //        //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id order by jc.id desc limit 1";

        //        List<CMJCDetail> _jcDetails = await GetJCDetail(jc_id);

        //        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, jc_id, CMMS.CMMS_Modules.JOB, job_id, "Job Card Created", CMMS.CMMS_Status.JC_CREATED,userID);

        //        await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CREATED, new[] { userID }, _jcDetails[0]);

        //        CMDefaultResponse response = new CMDefaultResponse(_jcDetails[0].id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Created");
        //        return response;
        //    }
        //    CMDefaultResponse responseFailure = new CMDefaultResponse(job_id, CMMS.RETRUNSTATUS.FAILURE, "No permit linked to this job");
        //    return responseFailure;

        //}        
        internal async Task<CMDefaultResponse> CreateJC(int job_id, int userID)
        {
            /* 
             * Data will be inserted in Following tables
             * JobCards - Primary table (All basic details in inserted)
             * PermiEmployeeLists - All jc linked employe
             * JCFiles - All uploaded file data
             * Please check the JobCard above tables to get idea what values need to insert. 
             * Get details from job, permit and add it to Jobcards table
             * Return the All Properties in DefaultResponse model
            */

            //jc basic details
            string myQuery = $"SELECT job.id as job_id, ptw.id as ptw_id  FROM permits AS ptw JOIN jobs as job on job.linkedPermit = ptw.id where job.id = { job_id }";
            List<CMJCCreate> _JobList = await Context.GetData<CMJCCreate>(myQuery).ConfigureAwait(false);

            int jc_id = 0;
            string jobQuery = $"SELECT title as job_title, description as job_description,facilityId as facility_id FROM jobs where id = {job_id}";
            List<CMJCJobDetail> job1 = await Context.GetData<CMJCJobDetail>(jobQuery).ConfigureAwait(false);

            string myQuery7 = "SELECT jc.id as id, file_path as fileName,  U.File_Size as fileSize, U.status,U.description FROM uploadedfiles AS U " +
                            "Left JOIN jobcards as jc on jc.jobid = U.module_ref_id  " +
                            "where U.module_ref_id =" + job_id + " and U.module_type = " + (int)CMMS.CMMS_Modules.JOB + ";";
            List<CMFileDetail> images = await Context.GetData<CMFileDetail>(myQuery7).ConfigureAwait(false);
            //_JobList.Count(); pending: add check for only one record is returned

            foreach (var data in _JobList)
            {
                int ptw_id = data.ptw_id;
                //JC Already exist. Return same one
                string jcQuery = $"SELECT id as jc_id FROM jobcards where jobId = { job_id } and PTW_id = { ptw_id } ";
                List<CMJCCreate> jcList = await Context.GetData<CMJCCreate>(jcQuery).ConfigureAwait(false);
                //jcList
                if (jcList.Count > 0)
                {
                    CMDefaultResponse response = new CMDefaultResponse(jcList[0].jc_id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Exists");
                    return response;
                }
                else
                {
                    //create new JC
                    string qryJCBasic = "insert into jobcards" +
                                         "(" +
                                           "jobId, PTW_id, PTW_Code, JC_title, JC_Description, JC_Added_by, JC_Added_Date, JC_Status, status_updated_at, Facility_id " +
                                          ")" +
                                          " values" +
                                         "(" +
                                           $"{ data.job_id }, { data.ptw_id }, 'PTW{ data.ptw_id }', '{ job1[0].job_title }', '{ job1[0].job_description }', {userID}, '{ UtilsRepository.GetUTCTime() }', {(int)CMMS.CMMS_Status.JC_CREATED}, '{UtilsRepository.GetUTCTime()}', {job1[0].facility_id} " +
                                         "); SELECT LAST_INSERT_ID();";

                    DataTable dt = await Context.FetchData(qryJCBasic).ConfigureAwait(false);
                    jc_id = Convert.ToInt32(dt.Rows[0][0]);
                }
            }
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;

            if (jc_id > 0)
            {
                string jcCodeQry = "UPDATE jobcards SET JC_Code = CONCAT('JC',id);";
                await Context.ExecuteNonQry<int>(jcCodeQry).ConfigureAwait(false);
                string latestJCQry = $"UPDATE jobs SET latestJC = {jc_id} WHERE id = {job_id};";
                await Context.ExecuteNonQry<int>(latestJCQry).ConfigureAwait(false);
                retCode = CMMS.RETRUNSTATUS.SUCCESS;

                //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id order by jc.id desc limit 1";

                List<CMJCDetail> _jcDetails = await GetJCDetail(jc_id);



                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, jc_id, CMMS.CMMS_Modules.JOB, job_id, "Job Card Created", CMMS.CMMS_Status.JC_CREATED, userID);

                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CREATED, new[] { userID }, _jcDetails[0]);

                CMDefaultResponse response = new CMDefaultResponse(_jcDetails[0].id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Created");
                return response;
            }
            CMDefaultResponse responseFailure = new CMDefaultResponse(job_id, CMMS.RETRUNSTATUS.FAILURE, "No permit linked to this job");
            return responseFailure;

        }

        internal async Task<CMDefaultResponse> UpdateJC(CMJCUpdate request, int userID)
        {
            //cmdefault response check
            /*
             * Below thing will happen in update function
             * User can upload new files - (JCFiles Tables)
             * Add new comments - History Table (use Utils AddLog functions)
             * Add/Remove employee list - PermiEmployeeLists table
             * Status - JobCards table
             * Return the CMDefaultResponse
            */
            //ISOLATION LIST
            if (request.is_isolation_required == true)
            {
                foreach (var data in request.isolated_list)
                {
                    //pending optimize the query 
                    string qryisolatedCategory = $"update permitisolatedassetcategories set normalisedStatus = { data.normalisedStatus } , normalisedDate = '{ UtilsRepository.GetUTCTime() }' where id = {data.isolation_id} ";                                                
                    await Context.ExecuteNonQry<int>(qryisolatedCategory).ConfigureAwait(false);
                }
            }            

            //LOTOT LIST
            foreach (var data in request.loto_list)
            {
                string qryPermitlotoAssets = $"update permitlotoassets set lotoRemovedStatus = { data.lotoRemovedStatus } , lotoRemovedDate = '{ UtilsRepository.GetUTCTime() }' where id = {data.loto_id}";
                await Context.ExecuteNonQry<int>(qryPermitlotoAssets).ConfigureAwait(false);
            }

            //EMP LIST
            foreach (var data in request.employee_list)
            {
                string delqryPermitEmpList=$"delete from permitemployeelists where JC_id ={request.id}";
                 await Context.ExecuteNonQry<int>(delqryPermitEmpList).ConfigureAwait(false);

                string qryPermitEmpList = $"insert into  permitemployeelists (employeeId,responsibility,status,JC_id) values({ data.id},'{ data.responsibility}',1,{request.id}); ";
                await Context.ExecuteNonQry<int>(qryPermitEmpList).ConfigureAwait(false);

            }         
            // JCFILES PENDINGidand history 
            string comment = request.comment;
            if (string.IsNullOrEmpty(request.comment))
            {
                comment = "Job Card Updated";
            }
            

           string myQuery1 = $"UPDATE `jobcards` set JC_Update_by = {userID}, JC_Update_date = '{UtilsRepository.GetUTCTime()}' where id = { request.id }";

            var result = await Context.ExecuteNonQry<int>(myQuery1);
            //who updated it means = current emp id and name
            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id);

            if (request.uploadfile_ids != null)
            {
                foreach (int data in request.uploadfile_ids)
                {
                    int facility_id = 380;
                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {facility_id}, module_type={(int)CMMS.CMMS_Modules.JOB},module_ref_id={request.id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, comment, CMMS.CMMS_Status.JC_UPDATED,userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_UPDATED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Updated");

            return response;
        }

        internal async Task<CMDefaultResponse> CloseJC(CMJCClose request, int userID)
        {
            /*
             * AssignedID/PermitID/CancelJob. Out of 3 we can update any one fields based on request
             * Re-assigned employee/ link permit / Cancel Permit. 3 different end points call this function.
             * return boolean true/false
             *                 
            /*Your code goes here*/
                                                                                    //Pending   add JC_End_date
            string queryCloseJc = $"update jobcards set JC_End_By_id={userID}, JC_Date_Stop ='{UtilsRepository.GetUTCTime()}',JC_Done_Description = '{request.comment}', JC_Approved = {(int)CMMS.ApprovalStatus.WAITING_FOR_APPROVAL}, JC_Status={(int)CMMS.CMMS_Status.JC_CLOSED}, status_updated_at = '{UtilsRepository.GetUTCTime()}' where id ={request.id};";
            await Context.ExecuteNonQry<int>(queryCloseJc).ConfigureAwait(false);

            string queryIsolated = $"update permitisolatedassetcategories set normalisedStatus={request.normalisedStatus}, normalisedDate ='{UtilsRepository.GetUTCTime()}' where id ={request.isolationId};";
            await Context.ExecuteNonQry<int>(queryIsolated).ConfigureAwait(false);

            string queryLoto = $"update permitlotoassets set lotoRemovedStatus={request.lotoStatus}, lotoRemovedDate ='{UtilsRepository.GetUTCTime()}' where id ={request.lotoId};";
            await Context.ExecuteNonQry<int>(queryLoto).ConfigureAwait(false);

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id);

            if (request.uploadfile_ids != null)
            {
                foreach (int data in request.uploadfile_ids)
                {
                    int facility_id = 380;
                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {facility_id}, module_type={(int)CMMS.CMMS_Modules.JOB},module_ref_id={request.id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            if (_jcDetails.Count == 0)
                return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.FAILURE, $"Job Card with ID {request.id} not found");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CLOSED, userID);

            await CMMSNotification .sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CLOSED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);

            return response;
        }

        internal async Task<List<CMDefaultResponse>> ApproveJC(CMJCApprove request, int userID)
        {
            /*
             * Read the fields name from JCApprovalModel model and update in JobCard table
             * Add log also using utils addlog function
             * return CMDefaultResponse                       	
            */
            List<CMDefaultResponse> responseList = new List<CMDefaultResponse>();
            string approveQuery = $"Update jobcards set JC_Approved = 1,JC_Status={(int)CMMS.CMMS_Status.JC_CLOSE_APPROVED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Approved_By_id={userID}, JC_Rejected_Reason='{request.comment}', JC_Rejected_TimeStamp ='{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
            // eng = user id = job created id , job appr

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, jc.JC_Status as status, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id);

            if (_jcDetails.Count == 0)
            {
                responseList.Add(new CMDefaultResponse(request.id,CMMS.RETRUNSTATUS.FAILURE,$"Job Card with ID {request.id} not found"));
            }

            if(_jcDetails[0].status == (int)CMMS.CMMS_Status.JC_CLOSED)
            {
                string jobCloseQry = $"UPDATE jobs SET status = {(int)CMMS.CMMS_Status.JOB_CLOSED} WHERE id = {_jcDetails[0].jobid};";
                await Context.ExecuteNonQry<int>(jobCloseQry).ConfigureAwait(false);
                List<CMJobView> job = new List<CMJobView>();
                using (var repos = new JobRepository(_conn))
                {
                    job = await repos.GetJobView(_jcDetails[0].jobid,"");
                }
                await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOB, _jcDetails[0].jobid, 0, 0, "Job Closed", CMMS.CMMS_Status.JOB_CLOSED, userID);
                await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOB, CMMS.CMMS_Status.JOB_CLOSED, new[] { userID }, job[0]);
                responseList.Add(new CMDefaultResponse(_jcDetails[0].jobid, CMMS.RETRUNSTATUS.SUCCESS, "Job closed successfully"));
            }


            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CLOSE_APPROVED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CLOSE_APPROVED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            responseList.Add(response);
            return responseList;
        }

        internal async Task<CMDefaultResponse> RejectJC(CMJCReject request, int userID)
        {           
            //CREATE DIFF MODEL FOR REJECT AND APPROVE
            /*
             * Read the fields name from JCApprovalModel model and update in JobCard table
             * Add log also using utils addlog function
             * return boolean true/false
             */

            string approveQuery = $"Update jobcards set JC_Approved = 2,JC_Status={(int)CMMS.CMMS_Status.JC_CLOSE_REJECTED}, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Rejected_By_id={userID}, JC_Rejected_Reason='{request.comment}', JC_Rejected_TimeStamp ='{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id);

            if (_jcDetails.Count == 0)
                throw new MissingMemberException($"Job Card with ID {request.id} not found");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CLOSE_REJECTED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CLOSE_REJECTED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);

            return response;

        }
        //carry forward jobcard?
        internal async Task<CMDefaultResponse> CarryForwardJC(CMApproval request, int userID)
        {
            string myQuery = $"UPDATE jobcards SET JC_Status = {(int)CMMS.CMMS_Status.JC_CARRY_FORWARDED}, JC_Approved = {(int)CMMS.ApprovalStatus.WAITING_FOR_APPROVAL}, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_Carry_Forward = {userID} WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            //string myQuery1 = $"SELECT  jc.id as id , jc.PTW_id as ptwId, job.id as jobid, facilities.name as plant_name, asset_cat.name as asset_category_name, CONCAT(user.firstName + ' ' + user.lastName) as JC_Closed_by_Name, CONCAT(user1.firstName + ' ' + user1.lastName) as JC_Rejected_By_Name, jc.JC_Approved_By_Name as  JC_Approved_By_Name FROM jobs as job JOIN  jobmappingassets as mapAssets ON mapAssets.jobId = job.id join assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id JOIN facilities as facilities ON job.blockId = facilities.id LEFT JOIN jobcards as jc on jc.jobId = job.id LEFT JOIN users as user ON user.id = jc.JC_Update_by LEFT JOIN  users as user1 ON user1.id = jc.JC_Rejected_By_id where jc.id = {request.id}";

            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id);
            if (request.uploadfile_ids != null)
            {
                foreach (int data in request.uploadfile_ids)
                {
                    int facility_id = 380;
                    string qryuploadFiles = $"UPDATE uploadedfiles SET facility_id = {facility_id}, module_type={(int)CMMS.CMMS_Modules.JOB},module_ref_id={request.id} where id = {data}";
                    await Context.ExecuteNonQry<int>(qryuploadFiles).ConfigureAwait(false);
                }
            }

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, "Job Card Carry forward Requested", CMMS.CMMS_Status.JC_CARRY_FORWARDED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CARRY_FORWARDED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Job Card Carry forward Requested");

            return response;
        }

        internal async Task<CMDefaultResponse> ApproveJCCF(CMJCApprove request, int userID)
        {
            CMDefaultResponse responseList = new CMDefaultResponse();
            string approveQuery = $"Update jobcards set JC_Status = {(int)CMMS.CMMS_Status.JC_CF_APPROVED},JC_Approved = 1, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_CF_Approved_by={userID}, JC_CF_Reason='{request.comment}', JC_CF_Approved_at ='{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
          
            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id);

            if (_jcDetails.Count == 0)
                throw new MissingMemberException($"Job Card with ID {request.id} not found");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CF_APPROVED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CF_APPROVED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);
            return response;
        }

        internal async Task<CMDefaultResponse> RejectJCCF(CMJCReject request, int userID)
        {
            
            string approveQuery = $"Update jobcards set JC_Status = {(int)CMMS.CMMS_Status.JC_CF_REJECTED}, JC_Approved = 2, status_updated_at = '{UtilsRepository.GetUTCTime()}', JC_CF_Rejected_by={userID}, JC_CF_Reason='{request.comment}', JC_CF_Rejected_at ='{UtilsRepository.GetUTCTime()}' where id = {request.id} ";
            await Context.ExecuteNonQry<int>(approveQuery).ConfigureAwait(false);
           

            List<CMJCDetail> _jcDetails = await GetJCDetail(request.id);

            if (_jcDetails.Count == 0)
                throw new MissingMemberException($"Job Card with ID {request.id} not found");

            await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.JOBCARD, request.id, 0, 0, request.comment, CMMS.CMMS_Status.JC_CF_REJECTED, userID);

            await CMMSNotification.sendNotification(CMMS.CMMS_Modules.JOBCARD, CMMS.CMMS_Status.JC_CF_REJECTED, new[] { userID }, _jcDetails[0]);

            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, request.comment);

            return response;

        }
    }
}
