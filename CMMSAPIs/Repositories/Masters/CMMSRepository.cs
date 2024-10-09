using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Notifications;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.SM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Repositories.Calibration;
using CMMSAPIs.Repositories.Incident_Reports;
using CMMSAPIs.Repositories.JC;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Repositories.Permits;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Repositories.WC;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Masters
{
    public class CMMSRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;
        public static Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        private MYSQLDBHelper getDB;
        private ErrorLog m_errorLog;

        public const string MA_Actual = "MA_Actual";
        public const string MA_Contractual = "MA_Contractual";
        public const string Internal_Grid = "Internal_Grid";
        public const string External_Grid = "External_Grid";
        private Dictionary<CMMS.CMMS_Modules, int> module_dict = new Dictionary<CMMS.CMMS_Modules, int>()
        {
            { CMMS.CMMS_Modules.DASHBOARD, 1 },
            { CMMS.CMMS_Modules.JOB, 2 },
            { CMMS.CMMS_Modules.PTW, 3 },
            { CMMS.CMMS_Modules.JOBCARD, 4 },
            { CMMS.CMMS_Modules.CHECKLIST_NUMBER, 5 },
            { CMMS.CMMS_Modules.CHECKPOINTS, 6 },
            { CMMS.CMMS_Modules.CHECKLIST_MAPPING, 7 },
            { CMMS.CMMS_Modules.PM_SCHEDULE, 8 },
            { CMMS.CMMS_Modules.PM_SCEHDULE_VIEW, 9 },
            { CMMS.CMMS_Modules.PM_EXECUTION, 10 },
            { CMMS.CMMS_Modules.PM_SCHEDULE_REPORT, 11 },
            { CMMS.CMMS_Modules.PM_SUMMARY, 12 },
            { CMMS.CMMS_Modules.SM_MASTER, 13 },
            { CMMS.CMMS_Modules.SM_GO, 14 },
            { CMMS.CMMS_Modules.SM_MRS, 15 },
            { CMMS.CMMS_Modules.SM_MRS_RETURN, 16 },
            { CMMS.CMMS_Modules.SM_S2S, 17 },
            { CMMS.CMMS_Modules.AUDIT_PLAN, 18 },
            { CMMS.CMMS_Modules.AUDIT_SCHEDULE, 19 },
            { CMMS.CMMS_Modules.AUDIT_SCEHDULE_VIEW, 20 },
            { CMMS.CMMS_Modules.AUDIT_EXECUTION, 21 },
            { CMMS.CMMS_Modules.AUDIT_SUMMARY, 22 },
            { CMMS.CMMS_Modules.HOTO_PLAN, 23 },
            { CMMS.CMMS_Modules.HOTO_SCHEDULE, 24 },
            { CMMS.CMMS_Modules.HOTO_SCEHDULE_VIEW, 25 },
            { CMMS.CMMS_Modules.HOTO_EXECUTION, 26 },
            { CMMS.CMMS_Modules.HOTO_SUMMARY, 27 },
            { CMMS.CMMS_Modules.INVENTORY, 28 },
            { CMMS.CMMS_Modules.WARRANTY_CLAIM, 30 },
            { CMMS.CMMS_Modules.CALIBRATION, 31 },
           // { CMMS.CMMS_Modules.MODULE_CLEANING, 32 },
            { CMMS.CMMS_Modules.VEGETATION_PLAN, 33 }
        };
        public CMMSRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment _webHost = null) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(_webHost);
            getDB = sqlDBHelper;
        }
        internal static CMDashboardStatus getDashboardStatus(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue = "";
            string retCurrentStatus = "";
            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    retValue = "Job Created";
                    retCurrentStatus = "Open";
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue = "Job Assigned";
                    retCurrentStatus = "Open";
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                    retValue = "Job Linked to PTW";
                    retCurrentStatus = "Open";
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue = "Job Closed";
                    retCurrentStatus = "Open";
                    break;
                case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                    retValue = "Job Cancelled";
                    retCurrentStatus = "Open";
                    break;
                case CMMS.CMMS_Status.JC_CREATED:
                    retValue = "JC Created";
                    retCurrentStatus = "Open";
                    break;
                case CMMS.CMMS_Status.JC_STARTED:
                    retValue = "JC Started";
                    retCurrentStatus = "Inprogress";
                    break;
                case CMMS.CMMS_Status.JC_CLOSED:
                    retValue = "JC Closed Waiting for Approval";
                    retCurrentStatus = "Inprogress";
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_REJECTED:
                    retValue = "JC Closed Rejected";
                    retCurrentStatus = "Inprogress";
                    break;
                case CMMS.CMMS_Status.JC_CLOSE_APPROVED:
                    retValue = "JC Closed Approved";
                    retCurrentStatus = "Close";
                    break;
                case CMMS.CMMS_Status.JC_CARRY_FORWARDED:
                    retValue = "JC Carry Forwarded - Waiting for Approval";
                    retCurrentStatus = "Inprogress";
                    break;
                case CMMS.CMMS_Status.JC_CF_APPROVED:
                    retValue = "JC Carry Forwarded Approved";
                    retCurrentStatus = "Inprogress";
                    break;
                case CMMS.CMMS_Status.JC_CF_REJECTED:
                    retValue = "JC Carry Forwarded Rejected";
                    retCurrentStatus = "Inprogress";
                    break;

                case CMMS.CMMS_Status.PTW_CREATED:
                    retValue = "Waiting for Approval"; // 121
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_ISSUED:
                    retValue = "Issued"; // 122
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_REJECTED_BY_ISSUER:
                    retValue = "Rejected By Issuer"; // 123
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER: // 124
                    retValue = "Rejected By Approver";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_APPROVED: // 125
                    retValue = "Approved";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_CLOSED: // 126
                    retValue = "Closed";
                    retCurrentStatus = "Closed";
                    break;

                case CMMS.CMMS_Status.PTW_RESUBMIT:
                    retValue = "Resubmitted"; // Corrected spelling
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_CANCELLED_BY_ISSUER:
                    retValue = "Cancelled By Issuer";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_CANCELLED_BY_HSE:
                    retValue = "Cancelled By HSE";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_CANCELLED_BY_APPROVER:
                    retValue = "Cancelled By Approver";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_CANCEL_REQUESTED: // 130
                    retValue = "Cancel Requested";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_REJECTED:
                    retValue = "Cancel Request Rejected";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_CANCEL_REQUEST_APPROVED: // 132
                    retValue = "Cancelled";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_EXTEND_REQUESTED:
                    retValue = "Requested for Extension"; // 133
                    retCurrentStatus = "Inprogress";
                    break;

                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_REJECTED:
                    retValue = "Extension Rejected"; // 134
                    retCurrentStatus = "Inprogress";
                    break;

                case CMMS.CMMS_Status.PTW_EXTEND_REQUEST_APPROVE: // 135
                    retValue = "Extension Approved";
                    retCurrentStatus = "Inprogress";
                    break;

                case CMMS.CMMS_Status.PTW_LINKED_TO_JOB:
                    retValue = "Linked to Job";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_LINKED_TO_PM:
                    retValue = "Linked to PM";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_LINKED_TO_AUDIT:
                    retValue = "Linked to Audit";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_LINKED_TO_HOTO:
                    retValue = "Linked to HOTO";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_EXPIRED:
                    retValue = "Expired";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_UPDATED:
                    retValue = "Updated";
                    retCurrentStatus = "Open";
                    break;

                case CMMS.CMMS_Status.PTW_UPDATED_WITH_TBT:
                    retValue = "Updated With TBT";
                    retCurrentStatus = "Inprogress";
                    break;

                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;

            }

            CMDashboardStatus retDashboardValue = new CMDashboardStatus();
            retDashboardValue.currentStatus = retCurrentStatus;
            retDashboardValue.shortStatus = retValue;
            return retDashboardValue;
        }
        internal async Task<CMDefaultResponse> AddModule(CMModule request)
        {
            /*
             * Read property of CMModule and insert into Features table
            */
            CMDefaultResponse response = new CMDefaultResponse();
            //if (request.software_id == 0)
            //{
            //    response = new CMDefaultResponse(request.software_id, CMMS.RETRUNSTATUS.FAILURE, "For Module Name <" + request.moduleName + "> Software Id Is Not Passed. Please contact Backend Team For Getting Software_id For This Module.");
            //    return response;
            //}
            string myQuery = "INSERT INTO features(`moduleName`,softwareid, `featureName`, `menuImage`, `add`, `edit`, `delete`, `view`, `issue`, `approve`, `selfView`,isactive,serialNo) " +
                $"VALUES('{request.moduleName}',{request.software_id}, '{request.featureName}', '{request.menuImage}', {request.add}, {request.edit}, " +
                $"{request.delete}, {request.view}, {request.issue}, {request.approve}, {request.selfView},1, id*100); SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);

            myQuery = "update features set serialNo = " + id * 100 + " where id = " + id + "";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);

            string insertIntoRoles = $"INSERT INTO roleaccess(roleId , featureId , `add` , edit , `delete` , `view` , issue , approve , selfView , " +
               $"lastModifiedAt , lastModifiedBy ) " +
               $" select id ,{id},0,0,0,0,0,0,0,NOW(),0 from userroles; ";
            DataTable dt_Roles = await Context.FetchData(insertIntoRoles).ConfigureAwait(false);

            string insertIntoUserRoles = $"INSERT INTO UsersAccess(userId , featureId , `add` , edit , `delete` , `view` , issue , approve , selfView , " +
                                         $"lastModifiedAt , lastModifiedBy ) " +
                                         $" select id ,{id},0,0,0,0,0,0,0,NOW(),0 from users; ";
            DataTable dt_UserRoles = await Context.FetchData(insertIntoUserRoles).ConfigureAwait(false);

            response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Module Added Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> UpdateModule(CMModule request)
        {
            /*
             * Read property of CMModule and Update into Features table
            */
            string myQuery = "UPDATE features SET ";
            if (request.moduleName != null && request.moduleName != "")
                myQuery += $"`moduleName` = '{request.moduleName}', ";
            if (request.featureName != null && request.featureName != "")
                myQuery += $"`featureName` = '{request.featureName}', ";
            if (request.menuImage != null && request.menuImage != "")
                myQuery += $"`menuImage` = '{request.menuImage}', ";
            if (request.add != null)
                myQuery += $"`add` = {request.add}, ";
            if (request.edit != null)
                myQuery += $"`edit` = {request.edit}, ";
            if (request.delete != null)
                myQuery += $"`delete` = {request.delete}, ";
            if (request.view != null)
                myQuery += $"`view` = {request.view}, ";
            if (request.approve != null)
                myQuery += $"`approve` = {request.approve}, ";
            if (request.issue != null)
                myQuery += $"`issue` = {request.issue}, ";
            if (request.selfView != null)
                myQuery += $"`selfView` = {request.selfView}, ";
            myQuery = myQuery.Substring(0, myQuery.LastIndexOf(','));
            myQuery += $" WHERE id={request.id};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Module Updated Successfully");
            return response;
        }
        internal async Task<CMDefaultResponse> DeleteModule(int id)
        {
            /*
             * Disable the status for requested id in Features table
            */
            string myQuery = $"DELETE FROM features WHERE id={id};";

            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Module Deleted Successfully");
            return response;
        }
        internal async Task<CMModule> GetModuleDetail(int id)
        {
            /*
             * Return request id detail from Features table
            */
            string myQuery = $"SELECT * FROM features WHERE id = {id}; ";
            List<CMModule> _moduleDetails = await Context.GetData<CMModule>(myQuery).ConfigureAwait(false);
            return _moduleDetails[0];
        }
        internal async Task<List<CMModule>> GetModuleList(bool return_all)
        {
            /*
             * Return List of modules from Features table
            */
            string myQuery = "";
            if (return_all)
            {
                myQuery = "SELECT id as id, softwareId as software_id, moduleName, featureName, menuImage, `add` , edit, `delete`, `view`, issue, approve, selfView," +
                          " escalate, isActive, serialNo FROM features ";
            }
            else
            {
                myQuery = "SELECT  id as id,softwareId as software_id , moduleName, featureName, menuImage, `add`, edit, `delete`," +
                          " `view`, issue, approve, selfView, escalate, isActive, serialNo FROM features where isActive=1; ";
            }

            List<CMModule> _moduleList = await Context.GetData<CMModule>(myQuery).ConfigureAwait(false);
            return _moduleList;
        }

        //internal async Task<List<CMModule>> GetFeatureList()
        //{
        //    /*
        //     * Return List of modules from Features table
        //    */
        //    string myQuery = "SELECT * FROM features; ";
        //    List<CMModule> _moduleList = await Context.GetData<CMModule>(myQuery).ConfigureAwait(false);
        //    return _moduleList;
        //}

        internal async Task<List<CMStatus>> GetStatusList()
        {
            string myQuery = "SELECT module_a.id as module_id, module_a.softwareId as module_software_id, module_a.featureName as module_name, " +
                                "status_a.softwareId as status_id, status_a.statusName as status_name " +
                                "FROM features as module_a LEFT JOIN statuses AS status_a ON status_a.moduleId = " +
                                "(SELECT CASE WHEN module_b.softwareId IN (SELECT DISTINCT status_b.moduleId FROM statuses as status_b) THEN " +
                                "module_b.softwareId ELSE 0 END AS moduleId FROM features as module_b WHERE module_a.softwareId = module_b.softwareId) " +
                                "WHERE module_a.softwareId > 0 ORDER BY module_a.softwareId ASC, status_a.softwareId ASC;";

            List<CMStatus> _statusList = await Context.GetData<CMStatus>(myQuery).ConfigureAwait(false);
            return _statusList;
        }
        internal async Task<CMStatus1> GetStatusbymodule(CMMS.CMMS_Modules module)
        {
            string myQuery = "SELECT module_a.id as module_id, module_a.softwareId as module_software_id, module_a.featureName as module_name, " +
                                "status_a.softwareId as status_id, status_a.statusName as status_name " +
                                "FROM features as module_a LEFT JOIN statuses AS status_a ON status_a.moduleId = " +
                                "(SELECT CASE WHEN module_b.softwareId IN (SELECT DISTINCT status_b.moduleId FROM statuses as status_b) THEN " +
                                "module_b.softwareId ELSE 0 END AS moduleId FROM features as module_b WHERE module_a.softwareId = module_b.softwareId) " +
                                "WHERE module_a.softwareId > 0 ";
            if (module > 0)
                myQuery += $"AND module_a.softwareId = {(int)module} ";
            myQuery += "ORDER BY module_a.softwareId ASC, status_a.softwareId ASC;";
            List<CMStatus1> _statusList = await Context.GetData<CMStatus1>(myQuery).ConfigureAwait(false);

            string myQuery1 = "SELECT DISTINCT status_a.softwareId as status_id, status_a.statusName as status_name " +
                                "FROM features as module_a LEFT JOIN statuses AS status_a ON status_a.moduleId = " +
                                "(SELECT CASE WHEN module_b.softwareId IN (SELECT DISTINCT status_b.moduleId FROM statuses as status_b) THEN " +
                                "module_b.softwareId ELSE 0 END AS moduleId FROM features as module_b WHERE module_a.softwareId = module_b.softwareId) " +
                                "WHERE module_a.softwareId > 0 ";
            if (module > 0)
                myQuery1 += $"AND module_a.softwareId = {(int)module} ";
            myQuery += "ORDER BY module_a.softwareId ASC, status_a.softwareId ASC;";
            List<Statusformodule> _status = await Context.GetData<Statusformodule>(myQuery1).ConfigureAwait(false);
            _statusList[0].status = _status;
            return _statusList[0];
        }

        internal async Task<List<CMFinancialYear>> GetFinancialYear()
        {
            List<CMFinancialYear> _FinancialYear = new List<CMFinancialYear>();
            _FinancialYear.Add(new CMFinancialYear { financial_year = "2020-21" });
            _FinancialYear.Add(new CMFinancialYear { financial_year = "2021-22" });
            _FinancialYear.Add(new CMFinancialYear { financial_year = "2022-23" });
            return _FinancialYear;
        }

        internal async Task<List<CMFacilityList>> GetFacilityList()
        {
            string myQuery = "SELECT facilities.id, facilities.name, spv.name as spv, facilities.address, facilities.city, facilities.state, facilities.country, facilities.zipcode as pin " +
                ", u.name as customer ,u2.name as owner,u3.name as Operator, Facilities.description,Facilities.timezone" +
                " FROM Facilities LEFT JOIN spv ON facilities.spvId=spv.id LEFT JOIN business as u ON u.id = facilities.customerId LEFT JOIN business as u2 ON u2.id = facilities.ownerId LEFT JOIN business as u3 ON u3.id = facilities.operatorId WHERE isBlock = 0 and facilities.status = 1;";

            List<CMFacilityList> _Facility = await Context.GetData<CMFacilityList>(myQuery).ConfigureAwait(false);
            return _Facility;
        }

        internal async Task<List<CMFacilityList>> GetFacilityListByUserId(int user_id)
        {
            string myQuery = "SELECT facilities.id, facilities.name, spv.name as spv, facilities.address, facilities.city, facilities.state, facilities.country, facilities.zipcode as pin " +
                ", u.name as customer ,u2.name as owner,u3.name as Operator, Facilities.description,Facilities.timezone" +
                " FROM Facilities LEFT JOIN spv ON facilities.spvId=spv.id LEFT JOIN business as u ON u.id = facilities.customerId LEFT JOIN userfacilities as uf ON uf.facilityId = facilities.id LEFT JOIN business as u2 ON u2.id = facilities.ownerId LEFT JOIN business as u3 ON u3.id = facilities.operatorId WHERE uf.userId = " + user_id + " and  isBlock = 0 and facilities.status = 1;";

            List<CMFacilityList> _Facility = await Context.GetData<CMFacilityList>(myQuery).ConfigureAwait(false);
            return _Facility;


        }

        internal async Task<List<CMFacility>> GetFacility(int facility_id)
        {
            string myQuery = "SELECT id, name, address, city, state, country, zipcode as pin FROM Facilities WHERE id= " + facility_id;
            List<CMFacility> _Facility = await Context.GetData<CMFacility>(myQuery).ConfigureAwait(false);
            return _Facility;
        }

        internal async Task<List<CMFacility>> GetBlockList(int facility_id)
        {
            string myQuery = "SELECT id, name, address, city, state, country, zipcode as pin FROM Facilities WHERE isBlock = 1 AND status = 1 AND parentId = " + facility_id;
            List<CMFacility> _Facility = await Context.GetData<CMFacility>(myQuery).ConfigureAwait(false);
            return _Facility;
        }
        internal async Task<List<CMAssetCategory>> GetAssetCategoryList()
        {
            string myQuery = "SELECT id, name, description FROM AssetCategories where status = 1";
            List<CMAssetCategory> _AssetCategory = await Context.GetData<CMAssetCategory>(myQuery).ConfigureAwait(false);
            return _AssetCategory;
        }
        internal async Task<List<CMAsset>> GetAssetList(int facility_id)
        {
            string myQuery = "SELECT id,name FROM Assets where status = 1 AND facilityId = " + facility_id;
            List<CMAsset> _Asset = await Context.GetData<CMAsset>(myQuery).ConfigureAwait(false);
            return _Asset;
        }

        internal async Task<List<CMDefaultList>> GetBloodGroupList()
        {
            string myQuery = "SELECT id, name FROM bloodgroup";
            List<CMDefaultList> _BloodGroup = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _BloodGroup;
        }

        internal async Task<List<CMDefaultList>> GetGenderList()
        {
            string myQuery = "SELECT id, name FROM gender";
            List<CMDefaultList> _Gender = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _Gender;
        }
        internal async Task<List<CMIRRiskType>> GetRiskTypeList()
        {
            string myQuery = "SELECT id, risktype as name, description FROM ir_risktype WHERE status=1 ";
            List<CMIRRiskType> _risktype = await Context.GetData<CMIRRiskType>(myQuery).ConfigureAwait(false);
            return _risktype;
        }
        internal async Task<List<CMIRStatus>> GetInsuranceStatusList()
        {
            string myQuery = "SELECT id, name  FROM ir_status WHERE status=1 ";
            List<CMIRStatus> _risktype = await Context.GetData<CMIRStatus>(myQuery).ConfigureAwait(false);
            return _risktype;
        }

        internal async Task<CMDefaultResponse> CreateRiskType(CMIRRiskType request, int userId)
        {
            string myQuery = $"INSERT INTO ir_risktype(risktype, description, status, addedBy, addedAt) VALUES " +
                                $"('{request.name}','{request.description} ', 1, {userId}, '{UtilsRepository.GetUTCTime()}'); " +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Risk Type Added");
        }
        internal async Task<CMDefaultResponse> UpdateRiskType(CMIRRiskType request, int userID)
        {
            string updateQry = "UPDATE ir_risktype SET ";
            if (request.name != null && request.name != "")
                updateQry += $"risktype = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Risk Type Updated");
        }
        internal async Task<CMDefaultResponse> DeleteRiskType(int id, int userId)
        {
            string deleteQry = $"UPDATE ir_risktype " +
                $" SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Risk Type Deleted");
        }

        internal async Task<List<CMIRInsuranceProvider>> GetInsuranceProviderList()
        {
            string myQuery = "SELECT id,  name FROM ir_insuranceprovider WHERE status=1 ";
            List<CMIRInsuranceProvider> _insuranceProvider = await Context.GetData<CMIRInsuranceProvider>(myQuery).ConfigureAwait(false);
            return _insuranceProvider;
        }

        internal async Task<List<CMSPV>> GetSPVList()
        {
            string myQuery = "SELECT id, name, description FROM spv WHERE status=1 ";
            List<CMSPV> _SPV = await Context.GetData<CMSPV>(myQuery).ConfigureAwait(false);
            return _SPV;
        }
        internal async Task<CMDefaultResponse> CreateSPV(CMSPV request, int userId)
        {
            string myQuery = $"INSERT INTO spv(name, description, status, addedBy, addedAt) VALUES " +
                                $"('{request.name}','{request.description} ', 1, {userId}, '{UtilsRepository.GetUTCTime()}'); " +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "SPV Added");
        }
        internal async Task<CMDefaultResponse> UpdateSPV(CMSPV request, int userID)
        {
            string updateQry = "UPDATE spv SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "SPV Updated");
        }
        internal async Task<CMDefaultResponse> DeleteSPV(int id, int userId)
        {
            string deleteQry = $"UPDATE spv " +
                $" SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "SPV Deleted");
        }

        internal async Task<List<CMEmployee>> GetEmployeeList(int facility_id, CMMS.CMMS_Modules module, CMMS.CMMS_Access access)
        {
            string myQuery = "SELECT " +
                                    "u.id, loginId as login_id, concat(firstName,' ', lastName) as name, birthday as birthdate,TIMESTAMPDIFF(YEAR,u.joiningDate, CURDATE()) AS experince,  " +
                                    "gender, mobileNumber, cities.name as city, states.name as state, countries.name as country, zipcode as pin, usd.designationName as designation " +
                             "FROM " +
                                    "Users as u " +
                             "JOIN " +
                                    "UserFacilities as uf ON u.id = uf.userId " +
                             "LEFT JOIN " +
                                    "cities as cities ON cities.id = u.cityId " +
                             "LEFT JOIN " +
                                    "states as states ON states.id = u.stateId and states.id = cities.state_id " +
                             "LEFT JOIN " +
                                    "countries as countries ON countries.id = u.countryId and countries.id = cities.country_id and countries.id = states.country_id " +
                             "LEFT JOIN " +
                                    "usersaccess as access ON u.id = access.userId " +
                             "LEFT JOIN " +
                                     "userdesignation as usd on  usd.id = u.designation_id   " +
                             "WHERE " +
                                    $"u.status = 1 AND uf.status = 1 AND u.isEmployee = 1 AND uf.facilityId = {facility_id} ";
            if (facility_id < 0)
                throw new ArgumentException("Invalid Facility ID");
            if (module != 0 && access != 0)
            {
                myQuery += $"AND access.featureId = {module_dict[module]} AND ( ";
                if ((access & CMMS.CMMS_Access.ADD) == CMMS.CMMS_Access.ADD)
                {
                    myQuery += "access.`add` = 1 OR ";
                }
                if ((access & CMMS.CMMS_Access.EDIT) == CMMS.CMMS_Access.EDIT)
                {
                    myQuery += "access.`edit` = 1 OR ";
                }
                if ((access & CMMS.CMMS_Access.DELETE) == CMMS.CMMS_Access.DELETE)
                {
                    myQuery += "access.`delete` = 1 OR ";
                }
                if ((access & CMMS.CMMS_Access.VIEW) == CMMS.CMMS_Access.VIEW)
                {
                    myQuery += "access.`view` = 1 OR ";
                }
                if ((access & CMMS.CMMS_Access.ISSUE) == CMMS.CMMS_Access.ISSUE)
                {
                    myQuery += "access.`issue` = 1 OR ";
                }
                if ((access & CMMS.CMMS_Access.APPROVE) == CMMS.CMMS_Access.APPROVE)
                {
                    myQuery += "access.`approve` = 1 OR ";
                }
                if ((access & CMMS.CMMS_Access.SELF_VIEW) == CMMS.CMMS_Access.SELF_VIEW)
                {
                    myQuery += "access.`selfView` = 1 OR ";
                }
                myQuery = myQuery.Substring(0, myQuery.Length - 3);
                myQuery += ") ";
            }
            else
            {
                if (!(module == 0 && access == 0))
                {
                    throw new ArgumentException("Both module type and access types are required");
                }
            }
            myQuery += " GROUP BY u.id ORDER BY u.id;";


            List<CMEmployee> _Employee = await Context.GetData<CMEmployee>(myQuery).ConfigureAwait(false);

            foreach (CMEmployee emp in _Employee)
            {

                emp.responsibilityIds = new int[2];
                emp.responsibilityIds[0] = 1;
                emp.responsibilityIds[1] = 2;

            }

            return _Employee;
        }

        internal async Task<List<CMBusinessType>> GetBusinessTypeList()
        {
            string myQuery = $"SELECT id, name, description FROM businesstype WHERE status=1 ";
            List<CMBusinessType> _Business = await Context.GetData<CMBusinessType>(myQuery).ConfigureAwait(false);
            return _Business;
        }
        internal async Task<CMDefaultResponse> AddBusinessType(CMBusinessType request, int userId)
        {

            //CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            //string strRetMessage = "";
            string qry = "insert into businesstype ( name, description, status , addedBy ,addedAt) values " + $"('{request.name}' ,'{request.description}' , 1 ,'{userId}' , '{UtilsRepository.GetUTCTime()}');" + $"SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Business Type Added");
        }

        internal async Task<CMDefaultResponse> UpdateBusinessType(CMBusinessType request, int userID)
        {
            string updateQry = "UPDATE businesstype SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = '{userID}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Business Type Updated");
        }
        internal async Task<CMDefaultResponse> DeleteBusinessType(int id, int userId)
        {
            string deleteQry = $"UPDATE businesstype SET status = 0 , updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Business Type Deleted");
        }

        internal async Task<CMDefaultResponse> AddBusiness(List<CMBusiness> request, int userId)
        {
            foreach (var item in request)
            {
                string country, state, city;
                string getCountryQry = $"SELECT name FROM countries WHERE id = {item.countryId};";
                DataTable dtCountry = await Context.FetchData(getCountryQry).ConfigureAwait(false);
                if (dtCountry.Rows.Count == 0)
                    throw new ArgumentException("Invalid Country");
                country = Convert.ToString(dtCountry.Rows[0][0]);
                string getStateQry = $"SELECT name FROM states WHERE id = {item.stateId};";
                DataTable dtState = await Context.FetchData(getStateQry).ConfigureAwait(false);
                if (dtState.Rows.Count == 0)
                    throw new ArgumentException("Invalid State");
                state = Convert.ToString(dtState.Rows[0][0]);
                string getCityQry = $"SELECT name FROM cities WHERE id = {item.cityId};";
                DataTable dtCity = await Context.FetchData(getCityQry).ConfigureAwait(false);
                if (dtCity.Rows.Count == 0)
                    throw new ArgumentException("Invalid City");
                city = Convert.ToString(dtCity.Rows[0][0]);
                string myQuery1 = $"SELECT * FROM states WHERE id = {item.stateId} AND country_id = {item.countryId};";
                DataTable dt1 = await Context.FetchData(myQuery1).ConfigureAwait(false);
                if (dt1.Rows.Count == 0)
                    throw new ArgumentException($"{state} is not situated in {country}");
                string myQuery2 = $"SELECT * FROM cities WHERE id = {item.cityId} AND state_id = {item.stateId} AND country_id = {item.countryId};";
                DataTable dt2 = await Context.FetchData(myQuery2).ConfigureAwait(false);
                if (dt2.Rows.Count == 0)
                    throw new ArgumentException($"{city} is not situated in {state}, {country}");
                item.country = country;
                item.state = state;
                item.city = city;
            }
            int count = 0;
            int retID = 0;
            string businessName = "";
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            string strRetMessage = "";
            string qry = "insert into business ( name, email, contactPerson, contactNumber, website, location, address, cityId, city, stateId, state, countryId, country, zip, type, status, addedBy) values ";
            foreach (var unit in request)
            {
                count++;
                businessName = unit.name;
                qry += "('" + unit.name + "','" + unit.email + "','" + unit.contactPerson + "','" + unit.contactNumber + "','" + unit.website + "','" + unit.location + "','" + unit.address + "','" + unit.cityId + "','" + unit.city + "','" + unit.stateId + "','" + unit.state + "','" + unit.countryId + "','" + unit.country + "','" + unit.zip + "','" + unit.type + "','" + 1 + "','" + userId + "'),";
            }
            if (count > 0)
            {
                qry = qry.Substring(0, (qry.Length - 1)) + ";" + "select LAST_INSERT_ID(); ";
                //List<CMInventoryList> newInventory = await Context.GetData<CMInventoryList>(qry).ConfigureAwait(false);
                DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
                retID = Convert.ToInt32(dt.Rows[0][0]);
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
                if (count == 1)
                {
                    strRetMessage = "New busineess <" + businessName + "> added";
                }
                else
                {
                    strRetMessage = "<" + count + "> new businesses added";
                }
            }
            else
            {
                strRetMessage = "No busineess to add";
            }
            return new CMDefaultResponse(retID, retCode, strRetMessage);
        }

        internal async Task<CMDefaultResponse> UpdateBusiness(CMBusiness request, int userId)
        {
            /*
             * Read property of CMModule and Update into Features table
            */
            if (request.id <= 0)
                throw new ArgumentException("Invalid ");
            string locationQry = $"SELECT countryId, stateId, cityId FROM business WHERE id = {request.id};";
            DataTable dt0 = await Context.FetchData(locationQry).ConfigureAwait(false);
            string country, state, city;
            DataTable dtCountry, dtState, dtCity;
            string getCountryQry = $"SELECT name FROM countries WHERE id = {request.countryId};";
            dtCountry = await Context.FetchData(getCountryQry).ConfigureAwait(false);
            if (dtCountry.Rows.Count == 0)
            {
                //throw exception
                throw new ArgumentException($"Their is no country in Database {request.countryId}");

            }
            country = Convert.ToString(dtCountry.Rows[0][0]);

            string myQuery1 = $"SELECT name FROM states WHERE id = {request.stateId} AND country_id = {request.countryId};";
            dtState = await Context.FetchData(myQuery1).ConfigureAwait(false);
            if (dtState.Rows.Count == 0)
                throw new ArgumentException($"StateId {request.stateId} is not situated in {country}");
            state = Convert.ToString(dtState.Rows[0][0]);
            string myQuery2 = $"SELECT name FROM cities WHERE id = {request.cityId} AND state_id = {request.stateId} AND country_id = {request.countryId};";
            dtCity = await Context.FetchData(myQuery2).ConfigureAwait(false);
            if (dtCity.Rows.Count == 0)
                throw new ArgumentException($"CityId {request.cityId} is not situated in {request.stateId}, {country}");
            city = Convert.ToString(dtCity.Rows[0][0]);

            string myQuery = "UPDATE business SET ";
            if (request.name != null && request.name != "")
                myQuery += $"`name` = '{request.name}', ";
            if (request.email != null && request.email != "")
                myQuery += $"`email` = '{request.email}', ";
            if (request.contactPerson != null && request.contactPerson != "")
                myQuery += $"`contactPerson` = '{request.contactPerson}', ";
            if (request.contactNumber != null && request.contactNumber != "")
                myQuery += $"`contactNumber` = '{request.contactNumber}', ";
            // if (request.website != null && request.website != "")
            myQuery += $"`website` = '{request.website}', ";
            if (request.location != null && request.location != "")
                myQuery += $"`location` = '{request.location}', ";
            if (request.address != null && request.address != "")
                myQuery += $"`address` = '{request.address}', ";
            if (city != null && city != "")
                myQuery += $"`city` = '{city}', ";
            if (state != null && state != "")
                myQuery += $"`state` = '{state}', ";
            if (country != null && country != "")
                myQuery += $"`country` = '{country}', ";
            if (request.cityId > 0)
                myQuery += $"`cityId` = {request.cityId}, ";
            if (request.stateId > 0)
                myQuery += $"`stateId` = {request.stateId}, ";
            if (request.countryId > 0)
                myQuery += $"`countryId` = {request.countryId}, ";
            if (request.zip != null && request.zip != "")
                myQuery += $"`zip` = '{request.zip}', ";
            if (request.type > 0)
                myQuery += $"`type` = {request.type}, ";

            myQuery += $"updatedBy = '{userId}' , updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            //myQuery += $" WHERE id={request.id};";
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
            CMDefaultResponse response = new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Business Updated Successfully");
            return response;
        }

        internal async Task<CMDefaultResponse> DeleteBusiness(int id, int userId)
        {
            string deleteQuery = $"UPDATE business SET status = 0 , updatedBy = '{userId}', updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQuery).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Business Deleted");
        }
        internal async Task<List<CMBusiness>> GetBusinessList(int businessType, string facilitytimeZone)
        {
            string myQuery = $"SELECT business.id, business.name, email, contactPerson, contactNumber, website, location, address, cities.id as cityId, cities.name as city, states.id as stateId, states.name as state, countries.id as countryId, countries.name as country, zip, type.id as type, type.name as typeName, business.addedAt " +
                                $"FROM Business " +
                                $"LEFT JOIN businesstype as type ON business.type = type.id " +
                                $"LEFT JOIN cities ON business.cityId = cities.id " +
                                $"LEFT JOIN states ON business.stateId = states.id AND states.id = cities.state_id " +
                                $"LEFT JOIN countries ON business.countryId = countries.id AND countries.id = cities.country_id AND countries.id = states.country_id " +
                                $"WHERE business.status=1 ";
            if (businessType > 0)
                myQuery += $"AND type = {businessType}";
            List<CMBusiness> _Business = await Context.GetData<CMBusiness>(myQuery).ConfigureAwait(false);
            foreach (var business in _Business)
            {
                if (business != null && business.addedAt != null)
                    business.addedAt = await _utilsRepo.ConvertToUTCDTC(facilitytimeZone, business.addedAt);

            }
            return _Business;
        }

        internal async Task<List<CMFrequency>> GetFrequencyList()
        {
            string myQuery = $"SELECT id,name, days FROM Frequency where status = 1";
            List<CMFrequency> _FrequencyList = await Context.GetData<CMFrequency>(myQuery).ConfigureAwait(false);
            return _FrequencyList;
        }

        internal async Task<string> Print(int id, CMMS.CMMS_Modules moduleID, int userID, string facilitytimeZone, params object[] args)
        {
            CMMSNotification.print = true;
            CMMS.CMMS_Status notificationID;

            switch (moduleID)
            {
                case CMMS.CMMS_Modules.JOB:
                    JobRepository obj = new JobRepository(getDB);
                    CMJobView _jobView = await obj.GetJobDetails(id, facilitytimeZone);
                    notificationID = (CMMS.CMMS_Status)(_jobView.status);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _jobView);
                    break;
                case CMMS.CMMS_Modules.PTW:
                    PermitRepository obj1 = new PermitRepository(getDB);
                    CMPermitDetail _Permit = await obj1.GetPermitDetails(id, facilitytimeZone);
                    notificationID = (CMMS.CMMS_Status)(_Permit.ptwStatus);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _Permit);
                    break;
                case CMMS.CMMS_Modules.JOBCARD:
                    JCRepository obj2 = new JCRepository(getDB);
                    List<CMJCDetail> _JobCard = await obj2.GetJCDetail(id, facilitytimeZone);
                    notificationID = (CMMS.CMMS_Status)(_JobCard[0].status);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _JobCard[0]);
                    break;
                case CMMS.CMMS_Modules.INCIDENT_REPORT:
                    IncidentReportRepository obj3 = new IncidentReportRepository(getDB);
                    CMViewIncidentReport _IncidentReport = await obj3.GetIncidentDetailsReport(id, facilitytimeZone);
                    notificationID = (CMMS.CMMS_Status)(_IncidentReport.status);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _IncidentReport);
                    break;
                case CMMS.CMMS_Modules.WARRANTY_CLAIM:
                    WCRepository obj4 = new WCRepository(getDB);
                    CMWCDetail _WC = await obj4.GetWCDetails(id, facilitytimeZone);
                    notificationID = (CMMS.CMMS_Status)(_WC.status);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _WC);
                    break;
                case CMMS.CMMS_Modules.CALIBRATION:
                    CalibrationRepository obj5 = new CalibrationRepository(getDB);
                    CMCalibrationDetails _Calibration = await obj5.GetCalibrationDetails(id, facilitytimeZone);
                    notificationID = (CMMS.CMMS_Status)(_Calibration.statusID + 100);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _Calibration);
                    break;
                case CMMS.CMMS_Modules.INVENTORY:
                    //InventoryRepository obj6 = new InventoryRepository(getDB, _environment);
                    //List<CMViewInventory> _Inventory = await obj6.GetInventoryDetails(id);
                    //notificationID = (CMMS.CMMS_Status)(_Inventory[0].status + 100);
                    //CMMSNotification.sendNotification(moduleID, notificationID, _Inventory[0]);
                    break;
                default:
                    break;
            }

            string HTMLBody = "<html> <head> <style> table{ border:1px solid black; margin-left:auto; margin-right:auto; border-collapse:collapse; /*width:53rem;*/ text-align:left; font-size:16px; } th{ padding:0.5rem; background-color:rgba(119,202,231,.2); } td{ padding:0.5rem; } .title{  background-color:#31576d; border-bottom-left-radius:8rem; height:8rem; width:35rem; text-align:center; padding-top:2rem; color:#ffca5a } </style> </head> <body> <div> <div style='border:4px solid #77cae7 ;margin:1rem'> <div style='display:flex;justify-content:space-between'> <img style='padding:20px;height:8rem;width:10rem' src='https://i.ibb.co/FD60YSY/hfe.png' alt='hfe' /> <div class='title'> <h1>JOB RECIEPT</h1> </div> </div> <div style='align-content:center;padding:2rem'> ";

            HTMLBody += CMMSNotification.HTMLBody;

            HTMLBody += "</table></div></div></div></body></html>";

            return HTMLBody;
        }

        private async Task<DataTable> ConvertExcelToBusinesses(int file_id)
        {
            Dictionary<string, int> countries = new Dictionary<string, int>();
            string countryQry = "SELECT id, UPPER(name) as name FROM countries;";
            DataTable dtCountry = await Context.FetchData(countryQry).ConfigureAwait(false);
            countries.Merge(dtCountry.GetColumn<string>("name"), dtCountry.GetColumn<int>("id"));

            string stateQry = "";
            DataTable dtState = null;
            List<string> stateNames = null;
            List<int> stateIds = null;
            Dictionary<string, int> states = new Dictionary<string, int>();

            string cityQry = "";
            DataTable dtCity = null;
            List<string> cityNames = null;
            List<int> cityIds = null;
            Dictionary<string, int> cities = new Dictionary<string, int>();

            Dictionary<string, int> businessTypes = new Dictionary<string, int>();
            string businessTypeQry = "SELECT id, UPPER(name) as name FROM businesstype GROUP BY name ORDER BY id;";
            DataTable dtBusinessType = await Context.FetchData(businessTypeQry).ConfigureAwait(false);
            businessTypes.Merge(dtBusinessType.GetColumn<string>("name"), dtBusinessType.GetColumn<int>("id"));


            /*
            Facility_Name	CheckList	Type	Frequency	Category	Man Power	Duration
            */
            Dictionary<string, Tuple<string, Type>> columnNames = new Dictionary<string, Tuple<string, Type>>()
            {
                { "Name", new Tuple<string, Type>("name", typeof(string)) },
                { "Email", new Tuple<string, Type>("email", typeof(string)) },
                { "Type", new Tuple<string, Type>("typeName", typeof(string)) },
                { "Contact Person", new Tuple<string, Type>("contactPerson", typeof(string)) },
                { "Contact Number", new Tuple<string, Type>("contactNumber", typeof(string)) },
                { "Website", new Tuple<string, Type>("website", typeof(string)) },
                { "Location", new Tuple<string, Type>("location", typeof(string)) },
                { "Address", new Tuple<string, Type>("address", typeof(string)) },
                { "City", new Tuple<string, Type>("city", typeof(string)) },
                { "State", new Tuple<string, Type>("state", typeof(string)) },
                { "Country", new Tuple<string, Type>("country", typeof(string)) },
                { "ZIP", new Tuple<string, Type>("zip", typeof(string)) }
            };

            string query1 = $"SELECT file_path FROM uploadedfiles WHERE id = {file_id};";
            DataTable dt1 = await Context.FetchData(query1).ConfigureAwait(false);
            string path = Convert.ToString(dt1.Rows[0][0]);
            string dir = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);
            if (!Directory.Exists(dir))
                m_errorLog.SetError($"Directory '{dir}' cannot be found");
            else if (!File.Exists(path))
                m_errorLog.SetError($"File '{filename}' cannot be found in directory '{dir}'");
            else
            {
                FileInfo info = new FileInfo(path);
                if (info.Extension != ".xlsx")
                    m_errorLog.SetError("File is not a .xlsx file");
                else
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excel = new ExcelPackage(path);
                    var sheet = excel.Workbook.Worksheets["Businesses"];
                    if (sheet == null)
                        m_errorLog.SetWarning("The file must contain Businesses sheet");
                    else
                    {
                        DataTable dt2 = new DataTable();
                        foreach (var header in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                        {
                            try
                            {
                                dt2.Columns.Add(columnNames[header.Text].Item1, columnNames[header.Text].Item2);
                            }
                            catch (KeyNotFoundException)
                            {
                                dt2.Columns.Add(header.Text);
                            }
                        }
                        dt2.Columns.Add("cityId", typeof(int));
                        dt2.Columns.Add("stateId", typeof(int));
                        dt2.Columns.Add("countryId", typeof(int));
                        dt2.Columns.Add("type", typeof(int));
                        //Pending: Reasons for skipping 3 rows
                        //
                        for (int rN = 2; rN <= sheet.Dimension.End.Row; rN++)
                        {
                            ExcelRange row = sheet.Cells[rN, 1, rN, sheet.Dimension.End.Column];
                            DataRow newR = dt2.NewRow();
                            foreach (var cell in row)
                            {
                                try
                                {
                                    if (cell.Text == null || cell.Text == "")
                                        continue;
                                    newR[cell.Start.Column - 1] = Convert.ChangeType(cell.Text, dt2.Columns[cell.Start.Column - 1].DataType);
                                }
                                catch (Exception ex)
                                {
                                    string status = ex.ToString();
                                    status = status.Substring(0, (status.IndexOf("Exception") + 8));
                                    m_errorLog.SetError("," + status);
                                }
                            }
                            if (newR.IsEmpty())
                            {
                                m_errorLog.SetInformation($"Row {rN} is empty.");
                                continue;
                            }
                            if (Convert.ToString(newR["name"]) == null || Convert.ToString(newR["name"]) == "")
                            {
                                m_errorLog.SetError($"[Row {rN}] Business name cannot be empty.");
                            }
                            if (Convert.ToString(newR["email"]) == null || Convert.ToString(newR["email"]) == "")
                            {
                                m_errorLog.SetError($"[Row {rN}] Business email cannot be empty.");
                            }
                            if (Convert.ToString(newR["contactPerson"]) == null || Convert.ToString(newR["contactPerson"]) == "")
                            {
                                m_errorLog.SetError($"[Row {rN}] Contact Person name cannot be empty.");
                            }
                            if (Convert.ToString(newR["contactNumber"]) == null || Convert.ToString(newR["contactNumber"]) == "")
                            {
                                m_errorLog.SetError($"[Row {rN}] Contact number cannot be empty.");
                            }
                            //if (Convert.ToString(newR["website"]) == null || Convert.ToString(newR["website"]) == "")
                            // {
                            //     m_errorLog.SetError($"[Row {rN}] Website cannot be empty.");
                            // }
                            if (Convert.ToString(newR["location"]) == null || Convert.ToString(newR["location"]) == "")
                            {
                                m_errorLog.SetError($"[Row {rN}] Location details cannot be empty.");
                            }
                            if (Convert.ToString(newR["address"]) == null || Convert.ToString(newR["address"]) == "")
                            {
                                m_errorLog.SetError($"[Row {rN}] Address cannot be empty.");
                            }
                            if (Convert.ToString(newR["zip"]) == null || Convert.ToString(newR["zip"]) == "")
                            {
                                m_errorLog.SetError($"[Row {rN}] ZIP code cannot be empty.");
                            }
                            try
                            {
                                newR["type"] = businessTypes[Convert.ToString(newR["typeName"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["typeName"]) == null || Convert.ToString(newR["typeName"]) == "")
                                    m_errorLog.SetError($"[Row {rN}] Business Type cannot be empty.");
                                else
                                    m_errorLog.SetError($"[Row {rN}] Invalid Business Type.");
                            }
                            try
                            {
                                newR["countryId"] = countries[Convert.ToString(newR["country"]).ToUpper()];
                                states.Clear();
                                stateQry = $"SELECT id, UPPER(name) as name FROM states WHERE country_id = {newR["countryId"]};";
                                dtState = await Context.FetchData(stateQry).ConfigureAwait(false);
                                stateNames = dtState.GetColumn<string>("name");
                                stateIds = dtState.GetColumn<int>("id");
                                states.Merge(stateNames, stateIds);
                                try
                                {
                                    newR["stateId"] = states[Convert.ToString(newR["state"]).ToUpper()];
                                    cities.Clear();
                                    cityQry = $"SELECT id, UPPER(name) as name FROM cities WHERE state_id = {newR["stateId"]};";
                                    dtCity = await Context.FetchData(cityQry).ConfigureAwait(false);
                                    cityNames = dtCity.GetColumn<string>("name");
                                    cityIds = dtCity.GetColumn<int>("id");
                                    cities.Merge(cityNames, cityIds);
                                    try
                                    {
                                        newR["cityId"] = cities[Convert.ToString(newR["city"]).ToUpper()];
                                    }
                                    catch (KeyNotFoundException)
                                    {
                                        if (Convert.ToString(newR["city"]) == null || Convert.ToString(newR["city"]) == "")
                                            m_errorLog.SetError($"City cannot be empty. [Row: {rN}]");
                                        else
                                            m_errorLog.SetError($"No city named {Convert.ToString(newR["city"])} found in state {Convert.ToString(newR["state"])}. [Row: {rN}]");
                                    }
                                }
                                catch (KeyNotFoundException)
                                {
                                    if (Convert.ToString(newR["state"]) == null || Convert.ToString(newR["state"]) == "")
                                        m_errorLog.SetError($"State cannot be empty. [Row: {rN}]");
                                    else
                                        m_errorLog.SetError($"No state named {Convert.ToString(newR["state"])} found in country {Convert.ToString(newR["country"])}. [Row: {rN}]");
                                    m_errorLog.SetError($"Cannot access cities due to empty or invalid state name. [Row: {rN}]");
                                }
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["country"]) == null || Convert.ToString(newR["country"]) == "")
                                    m_errorLog.SetError($"Country cannot be empty. [Row: {rN}]");
                                else
                                    m_errorLog.SetError($"No country named {Convert.ToString(newR["country"])} found. [Row: {rN}]");
                                m_errorLog.SetError($"Cannot access states and cities due to empty or invalid country name. [Row: {rN}]");
                            }
                            dt2.Rows.Add(newR);

                        }

                        return dt2;
                    }
                }
            }
            return null;
        }

        internal async Task<CMImportFileResponse> ValidateBusiness(int file_id)
        {
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.FAILURE;
            DataTable businesses = await ConvertExcelToBusinesses(file_id);
            string message;
            if (businesses != null && businesses != null && m_errorLog.GetErrorCount() == 0)
            {
                m_errorLog.SetImportInformation("File ready to Import");
                retCode = CMMS.RETRUNSTATUS.SUCCESS;
                string qry = $"UPDATE uploadedfiles SET valid = 1 WHERE id = {file_id};";
                await Context.ExecuteNonQry<int>(qry).ConfigureAwait(false);
                message = "No errors found during validation";
            }
            else
            {
                string qry = $"UPDATE uploadedfiles SET valid = 2 WHERE id = {file_id};";
                await Context.ExecuteNonQry<int>(qry).ConfigureAwait(false);
                message = "Errors found during validation";
            }
            string logPath = m_errorLog.SaveAsText($"ImportLog\\ImportBusiness_File{file_id}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}");
            string logQry = $"UPDATE uploadedfiles SET logfile = '{logPath}' WHERE id = {file_id}";
            await Context.ExecuteNonQry<int>(logQry).ConfigureAwait(false);
            return new CMImportFileResponse(file_id, retCode, logPath, m_errorLog.errorLog(), message);
        }

        internal async Task<CMImportFileResponse> ImportBusiness(int file_id, int userID)
        {
            CMImportFileResponse response;
            string qry = $"SELECT valid, logfile FROM uploadedfiles WHERE id = {file_id}";
            DataTable dt = await Context.FetchData(qry).ConfigureAwait(false);
            int valid = Convert.ToInt32(dt.Rows[0]["valid"]);
            string logfile = Convert.ToString(dt.Rows[0]["logfile"]);
            IEnumerable<string> log;
            try
            {
                log = File.ReadAllLines(logfile);
            }
            catch
            {
                log = null;
                logfile = null;
            }
            if (valid == 1)
            {
                DataTable dtBusinesses = await ConvertExcelToBusinesses(file_id);
                if (dtBusinesses != null && m_errorLog.GetErrorCount() == 0)
                {
                    List<CMBusiness> businesses = dtBusinesses.MapTo<CMBusiness>();
                    CMDefaultResponse response1 = await AddBusiness(businesses, userID);
                    response = new CMImportFileResponse(response1.id, response1.return_status, logfile, log, response1.message);
                }
                else
                {
                    CMImportFileResponse response2 = new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, logfile, log, "Error while importing businesses. Please validate the file again.");
                    response = response2;
                }
            }
            else if (valid == 2)
            {
                CMImportFileResponse response3 = new CMImportFileResponse(file_id, CMMS.RETRUNSTATUS.FAILURE, logfile, log, "Cannot import businesses as file has some errors. Please correct the file and re-validate it");
                response = response3;
            }
            else
            {
                await ValidateBusiness(file_id);
                CMImportFileResponse response4 = await ImportBusiness(file_id, userID);
                response = response4;
            }
            return response;
        }
        public async Task<string> DownloadFile(int id)
        {
            string query1 = $"SELECT file_path FROM templatefile WHERE id = {id};";
            DataTable dt1 = await Context.FetchData(query1).ConfigureAwait(false);
            string path = Convert.ToString(dt1.Rows[0][0]);
            return path;

        }
        internal static string getShortStatus_JOB(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.JOB_CREATED:     //Created
                    retValue = "Job Created";
                    break;
                case CMMS.CMMS_Status.JOB_ASSIGNED:     //Assigned
                    retValue = "Job Assigned";
                    break;
                case CMMS.CMMS_Status.JOB_LINKED:     //Linked
                    retValue = "Job Linked to PTW";
                    break;
                case CMMS.CMMS_Status.JOB_CLOSED:     //Closed
                    retValue = "Job Closed";
                    break;
                case CMMS.CMMS_Status.JOB_CANCELLED:     //Cancelled
                    retValue = "Job Cancelled";
                    break;
                default:
                    retValue = "Unknown Status";
                    break;
            }
            return retValue;

        }
        public async Task<List<CMDashboadModuleWiseList>> getDashboadDetails(string facilityId, string moduleID, DateTime fromDate, DateTime toDate)
        {

            List<CMDashboadModuleWiseList> countResult = new List<CMDashboadModuleWiseList>();
            if (facilityId == null)
            {
                return countResult;
            }
            CMDashboadModuleWiseList modulewiseDetail = new CMDashboadModuleWiseList();
            CMDashboadDetails result = new CMDashboadDetails();
            string input = moduleID;
            int mid = 0;
            string[] separatedValues = new string[0];
            bool condition = moduleID != null;
            if (condition)
            {
                separatedValues = input.Split(',');
            }
            foreach (string sid in separatedValues)
            {
                mid = Convert.ToInt32(sid);
                if (mid == (int)CMMS.CMMS_Modules.JOB)
                {
                    modulewiseDetail.module_name = "Breakdown Maintenance";
                    result = await getJobDashboardDetails(facilityId, fromDate, toDate);
                    modulewiseDetail.CMDashboadDetails = result;
                    countResult.Add(modulewiseDetail);
                }
                else if (mid == (int)CMMS.CMMS_Modules.PM_PLAN)
                {
                    modulewiseDetail.module_name = "Preventive Maintenance";
                    result = await getPMPlanDashboardDetails(facilityId, fromDate, toDate);
                    modulewiseDetail.CMDashboadDetails = result;
                    countResult.Add(modulewiseDetail);
                }
                else if (mid == (int)CMMS.CMMS_Modules.MC_PLAN)
                {
                    modulewiseDetail.module_name = "Module Cleaning";
                    result = await getMCPlanDashboardDetails(facilityId, fromDate, toDate);
                    modulewiseDetail.CMDashboadDetails = result;
                    countResult.Add(modulewiseDetail);
                }
                else if (mid == (int)CMMS.CMMS_Modules.INCIDENT_REPORT)
                {
                    modulewiseDetail.module_name = "Incident Report";
                    result = await getIRDashboardDetails(facilityId, fromDate, toDate);
                    modulewiseDetail.CMDashboadDetails = result;
                    countResult.Add(modulewiseDetail);
                }
                else if (mid == (int)CMMS.CMMS_Modules.SM_GO)
                {
                    modulewiseDetail.module_name = "Stock Management";
                    result = await getSMDashboardDetails(facilityId, fromDate, toDate);
                    modulewiseDetail.CMDashboadDetails = result;
                    countResult.Add(modulewiseDetail);
                }
            }
            if (moduleID == null)
            {
                CMDashboadModuleWiseList modulewiseDetail_Job = new CMDashboadModuleWiseList();
                CMDashboadDetails result_job = new CMDashboadDetails();
                modulewiseDetail_Job.module_name = "Breakdown Maintenance";
                result_job = await getJobDashboardDetails(facilityId, fromDate, toDate);
                modulewiseDetail_Job.CMDashboadDetails = result_job;
                modulewiseDetail_Job.category_total_count = result_job.bm_closed_count + result_job.mc_closed_count + result_job.pm_closed_count;
                modulewiseDetail_Job.category_pm_count = result_job.pm_closed_count;
                modulewiseDetail_Job.category_mc_count = result_job.mc_closed_count;
                modulewiseDetail_Job.category_bm_count = result_job.bm_closed_count;
                countResult.Add(modulewiseDetail_Job);

                CMDashboadModuleWiseList modulewiseDetail_PM = new CMDashboadModuleWiseList();
                CMDashboadDetails result_PM = new CMDashboadDetails();
                modulewiseDetail_PM.module_name = "Preventive Maintenance";
                result_PM = await getPMPlanDashboardDetails(facilityId, fromDate, toDate);
                modulewiseDetail_PM.CMDashboadDetails = result_PM;
                modulewiseDetail_PM.category_total_count = result_PM.bm_closed_count + result_PM.mc_closed_count + result_PM.pm_closed_count;
                modulewiseDetail_PM.category_pm_count = result_PM.pm_closed_count;
                modulewiseDetail_PM.category_mc_count = result_PM.mc_closed_count;
                modulewiseDetail_PM.category_bm_count = result_PM.bm_closed_count;
                countResult.Add(modulewiseDetail_PM);

                CMDashboadModuleWiseList modulewiseDetail_MC = new CMDashboadModuleWiseList();
                CMDashboadDetails result_MC = new CMDashboadDetails();
                modulewiseDetail_MC.module_name = "Module Cleaning";
                result_MC = await getMCPlanDashboardDetails(facilityId, fromDate, toDate);
                modulewiseDetail_MC.CMDashboadDetails = result_MC;
                modulewiseDetail_MC.category_total_count = result_MC.bm_closed_count + result_MC.mc_closed_count + result_MC.pm_closed_count;
                modulewiseDetail_MC.category_pm_count = result_MC.pm_closed_count;
                modulewiseDetail_MC.category_mc_count = result_MC.mc_closed_count;
                modulewiseDetail_MC.category_bm_count = result_MC.bm_closed_count;
                countResult.Add(modulewiseDetail_MC);

                CMDashboadModuleWiseList modulewiseDetail_IR = new CMDashboadModuleWiseList();
                CMDashboadDetails result_IR = new CMDashboadDetails();
                modulewiseDetail_IR.module_name = "Incident Report";
                result_IR = await getIRDashboardDetails(facilityId, fromDate, toDate);
                modulewiseDetail_IR.CMDashboadDetails = result_IR;
                modulewiseDetail_IR.category_total_count = result_IR.bm_closed_count + result_IR.mc_closed_count + result_IR.pm_closed_count;
                modulewiseDetail_IR.category_pm_count = result_IR.pm_closed_count;
                modulewiseDetail_IR.category_mc_count = result_IR.mc_closed_count;
                modulewiseDetail_IR.category_bm_count = result_IR.bm_closed_count;
                countResult.Add(modulewiseDetail_IR);

                CMDashboadModuleWiseList modulewiseDetail_SM = new CMDashboadModuleWiseList();
                CMDashboadDetails result_SM = new CMDashboadDetails();
                modulewiseDetail_SM.module_name = "Stock Management";
                result_SM = await getSMDashboardDetails(facilityId, fromDate, toDate);
                modulewiseDetail_SM.CMDashboadDetails = result_SM;
                countResult.Add(modulewiseDetail_SM);
            }
            return countResult;
        }
        public async Task<CMDashboadDetails> getJobDashboardDetails(string facilityId, DateTime fromDate, DateTime toDate)
        {
            CMDashboadDetails result = new CMDashboadDetails();
            if (facilityId == "")
            {
                return result;
            }
            string filter = "";

            if (fromDate != null && fromDate.ToString("yyyy-MM-dd hh-mm") != "0001-01-01 00-00" && toDate != null && toDate.ToString("yyyy-MM-dd hh-mm") != "0001-01-01 00-00")

            {

                filter = $" and DATE_FORMAT(job.createdAt, '%Y-%m-%d') between '{fromDate.ToString("yyyy-MM-dd")}' and '{toDate.ToString("yyyy-MM-dd")}'";
            }

            string myQuery = $"SELECT job.id  wo_number,job.title as wo_decription ,job.facilityId as facility_id, facilities.name as facility_name, job.status," +
                $" group_concat(distinct asset_cat.name order by asset_cat.id separator ', ') as asset_category, " +
                $" group_concat(distinct asset.name order by asset.id separator ', ') as assetsname, job.breakdownTime as start_date, jc.JC_Date_Stop as end_Date," +
                $" jc.JC_Status as latestJCStatus,  jc.JC_Approved as latestJCApproval, permit.id as ptw_id,jc.id as latestJCid,permit.status as latestJCPTWStatus," +
                $" on_time_status , ABS(TIMESTAMPDIFF(HOUR, job.breakdownTime, jc.JC_Date_Stop)) AS job_time,  CASE WHEN ABS(TIMESTAMPDIFF(HOUR, job.breakdownTime, jc.JC_Date_Stop)) < 8 THEN 1    WHEN ABS(TIMESTAMPDIFF(HOUR, job.breakdownTime, jc.JC_Date_Stop)) > 8 THEN 0   ELSE 2 END AS time_condition" +
                $" FROM jobs as job " +
                $" LEFT JOIN jobcards as jc ON job.latestJC = jc.id " +
                $" LEFT JOIN  facilities as facilities ON job.facilityId = facilities.id " +
                $" LEFT JOIN jobmappingassets as mapAssets ON mapAssets.jobId = job.id " +
                $" LEFT JOIN assets as asset ON mapAssets.assetId  =  asset.id " +
                $" LEFT JOIN assetcategories as asset_cat ON mapAssets.categoryId = asset_cat.id " +
                $" LEFT JOIN permits as permit ON permit.id = job.linkedPermit " +
                $" WHERE job.facilityId in ({facilityId}) {filter} " +
                $" GROUP BY job.id order by job.id DESC;";
            List<CMDashboadItemList> itemList = await Context.GetData<CMDashboadItemList>(myQuery).ConfigureAwait(false);
            Parallel.ForEach(itemList, _Job =>
            {
                //if (_Job.ptw_id == 0)
                //{
                //    _Job.status_long = "Permit not linked";
                //}
                //else if (_Job.latestJCid != 0)
                //{
                //    //if permit status is not yet approved
                //    if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                //    {
                //        _Job.status_long = JCRepository.getShortStatus(CMMS.CMMS_Modules.JOBCARD, (CMMS.CMMS_Status)_Job.latestJCStatus, (CMMS.ApprovalStatus)_Job.latestJCApproval);
                //    }
                //    else if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                //    {
                //        _Job.status_long = "Permit - rejected";
                //    }
                //    else
                //    {
                //        _Job.status_long = "Permit - Waiting For Approval";
                //    }
                //}
                //else
                //{
                //    if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                //    {

                //        _Job.status_long = "Permit - Approved";
                //    }
                //    else if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                //    {
                //        _Job.status_long = "Permit - rejected";
                //    }
                //    else
                //    {
                //        _Job.status_long = "Permit - Pending";
                //    }
                //}

                //if permit status is not yet approved
                if (_Job.latestJCStatus > 0)
                {
                    if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                    {
                        _Job.status_long = JCRepository.getShortStatus(CMMS.CMMS_Modules.JOBCARD, (CMMS.CMMS_Status)_Job.latestJCStatus, (CMMS.ApprovalStatus)_Job.latestJCApproval);

                    }
                    else if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                    {
                        _Job.status_long = "Permit - rejected";
                    }
                    else if (_Job.latestJCStatus == (int)CMMS.CMMS_Status.JC_CLOSE_APPROVED)
                    {
                        _Job.status_long = "JC CLOSE APPROVED";
                    }
                    else if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.JC_CLOSE_REJECTED)
                    {
                        _Job.status_long = "JC CLOSE REJECTED";
                    }
                    else if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_APPROVED)
                    {
                        _Job.status_long = JCRepository.getShortStatus(CMMS.CMMS_Modules.JOBCARD, (CMMS.CMMS_Status)_Job.latestJCStatus, (CMMS.ApprovalStatus)_Job.latestJCApproval);
                    }
                    else if (_Job.latestJCPTWStatus == (int)CMMS.CMMS_Status.PTW_REJECTED_BY_APPROVER)
                    {
                        _Job.status_long = "Permit - rejected";
                    }
                    else
                    {
                        _Job.status_long = "Permit - Waiting For Approval";
                    }
                    ////if permit status is not yet approved

                    //else
                    //{
                    //    _Job.status_long = "Permit - Waiting For Approval";
                    //}
                    CMDashboardStatus status = getDashboardStatus(CMMS.CMMS_Modules.JOBCARD, (CMMS.CMMS_Status)_Job.latestJCPTWStatus);
                    _Job.short_status = status.shortStatus;
                    _Job.current_status = status.currentStatus;
                }
                else
                {
                    CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_Job.status);
                    _Job.status_long = getShortStatus_JOB(CMMS.CMMS_Modules.JOB, _Status);
                    CMDashboardStatus status = getDashboardStatus(CMMS.CMMS_Modules.JOBCARD, _Status);
                    _Job.short_status = status.shortStatus;
                    _Job.current_status = status.currentStatus;
                }
            });

            result.created = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.JOB_CREATED).ToList().Count;
            result.rejected = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.JOB_CANCELLED).ToList().Count;
            result.assigned = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.JOB_ASSIGNED).ToList().Count;
            result.bm_closed_count = itemList.Where(x => x.latestJCStatus == (int)CMMS.CMMS_Status.JC_CLOSE_APPROVED).ToList().Count;


            result.total = itemList.Count;
            result.completed = itemList.Where(x => x.latestJCStatus == (int)CMMS.CMMS_Status.JC_CLOSE_APPROVED).ToList().Count;
            //result.pending = result.total - itemList.Where(x => x.latestJCPTWStatus != (int)CMMS.CMMS_Status.PTW_APPROVED).ToList().Count;
            result.pending = result.total - result.completed;
            if (result.total != result.pending + result.completed + result.created + result.rejected + result.assigned)
            {
                result.unknown_count = result.total - result.pending + result.completed + result.created + result.rejected + result.assigned;
            }



            /*int completed_on_time = itemList.Where(x => x.latestJCStatus == (int)CMMS.CMMS_Status.JC_CLOSE_APPROVED && x.on_time_status == 1).ToList().Count;
            int wo_delay = itemList.Where(x => x.latestJCStatus == (int)CMMS.CMMS_Status.JC_CLOSE_APPROVED && x.on_time_status == 2).ToList().Count;
            int wo_backlog = itemList.Where(x => x.on_time_status == 0).ToList().Count;*/
            int completed_on_time = itemList.Where(x => x.time_condition == 0).ToList().Count;
            int wo_delay = itemList.Where(x => x.time_condition == 1).ToList().Count;
            int wo_backlog = itemList.Where(x => x.time_condition == 2).ToList().Count;
            if (result.total > 0)
            {
                result.wo_on_time = completed_on_time;
                result.wo_delay = wo_delay;
                result.wo_backlog = wo_backlog;
            }
            result.item_list = itemList;
            return result;
        }

        public async Task<CMDashboadDetails> getPMPlanDashboardDetails(string facilityId, DateTime fromDate, DateTime toDate)
        {
            CMDashboadDetails result = new CMDashboadDetails();

            string filter = "";

            if (fromDate != null && fromDate.ToString("yyyy-MM-dd") != "0001-01-01" && toDate != null && toDate.ToString("yyyy-MM-dd") != "0001-01-01")
            {
                filter = $" and pm_task.plan_date between '{fromDate.ToString("yyyy-MM-dd")}' and '{toDate.ToString("yyyy-MM-dd")}'";
            }

            string myQuery = $"SELECT facilities.name as facility_name,pm_task.id as wo_number,pm_plan.plan_name as wo_decription, a.name as assetsname ,pm_plan.plan_name,pm_task.category_id,cat.name as asset_category, " +
                $" CONCAT('PMTASK',pm_task.id) as task_code,pm_plan.plan_name as plan_title,pm_task.facility_id, pm_task.frequency_id as frequency_id, pm_plan.plan_date as start_date,pm_task.closed_at as end_date, " +
                $"freq.name as frequency_name, pm_task.plan_date as due_date,prev_task_done_date as last_done_date, closed_at as done_date, " +
                $"CONCAT(assignedTo.firstName,' ',assignedTo.lastName)  as assigned_to_name, pm_task.PTW_id as permit_id, " +
                $"CONCAT('PTW',pm_task.PTW_id) as permit_code,permit.status as ptw_status, PM_task.status, IFNULL( PM_task.schedule_time, CAST('1900-01-01 00:00:00' AS DATETIME)) as schedule_time  " +
                   "FROM pm_task " +
                   $"left join users as assignedTo on pm_task.assigned_to = assignedTo.id " +
                   $"left join pm_plan  on pm_task.plan_id = pm_plan.id " +
                   $"left join assetcategories as cat  on pm_task.category_id = cat.id " +
                   $"left join permits as permit on pm_task.PTW_id = permit.id " +
                   $"left join frequency as freq on pm_task.frequency_id = freq.id " +
                   $"left join pm_schedule as ps on ps.task_id = pm_task.id " +
                   $"left join assets as a on a.id = ps.Asset_id " +
                   //  $"left join assets as a on a.id=(select Asset_id  from pm_schedule where id=pm_task.id) " +
                   $" JOIN facilities ON pm_task.facility_id = facilities.id " +
                   $" WHERE facilities.id in ({facilityId})  and status_id = 1 {filter};";
            List<CMDashboadItemList> itemList = await Context.GetData<CMDashboadItemList>(myQuery).ConfigureAwait(false);

            foreach (var plan in itemList)
            {
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(plan.status);
                string _shortStatus = getShortStatus_PM_Plan(CMMS.CMMS_Modules.PM_PLAN, _Status);
                plan.status_long = _shortStatus;
            }
            result.created = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.PM_ASSIGNED).ToList().Count;
            result.rejected = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.PM_CLOSE_REJECTED || x.status == (int)CMMS.CMMS_Status.PM_CLOSE_REJECTED || x.status == (int)CMMS.CMMS_Status.PM_PLAN_REJECTED).ToList().Count;
            result.assigned = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.PM_ASSIGNED).ToList().Count;
            result.submitted = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.PM_SCHEDULED).ToList().Count;
            result.pm_closed_count = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.PM_CLOSE_APPROVED).ToList().Count;
            result.approved = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.PM_CLOSE_APPROVED || x.status == (int)CMMS.CMMS_Status.PM_CLOSE_APPROVED || x.status == (int)CMMS.CMMS_Status.PM_PLAN_APPROVED).ToList().Count;


            result.total = itemList.Count;
            result.completed = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.PM_CLOSE_APPROVED).ToList().Count;
            //result.pending = result.total - itemList.Where(x => x.latestJCPTWStatus != (int)CMMS.CMMS_Status.PTW_APPROVED).ToList().Count;
            result.pending = result.total - result.completed;
            result.item_list = itemList;

            /* int completed_on_time = itemList.Where(x => (x.status == (int)CMMS.CMMS_Status.PM_CLOSE_APPROVED) && (x.schedule_time.Value.Hour - x.end_date.Value.Hour <= 8)).ToList().Count;
             int wo_delay = itemList.Where(x => (x.status == (int)CMMS.CMMS_Status.PM_CLOSE_APPROVED) && (x.schedule_time.Value.Hour - x.end_date.Value.Hour > 8)).ToList().Count;
             int wo_backlog = itemList.Where(x => (x.status != (int)CMMS.CMMS_Status.PM_CLOSE_APPROVED) && (x.schedule_time.Value.Hour - x.end_date.Value.Hour > 8)).ToList().Count;*/
            int completed_on_time = itemList
        .Where(x => x.status == (int)CMMS.CMMS_Status.PM_CLOSE_APPROVED &&
                x.schedule_time.HasValue &&
                x.end_date.HasValue &&
                (x.schedule_time.Value.Hour - x.end_date.Value.Hour <= 8))
        .ToList()
        .Count();

            int wo_delay = itemList
                .Where(x => x.status == (int)CMMS.CMMS_Status.PM_CLOSE_APPROVED &&
                            x.schedule_time.HasValue &&
                            x.end_date.HasValue &&
                            (x.schedule_time.Value.Hour - x.end_date.Value.Hour > 8))
                .ToList()
                .Count();

            int wo_backlog = itemList
                .Where(x => x.status != (int)CMMS.CMMS_Status.PM_CLOSE_APPROVED &&
                            x.schedule_time.HasValue &&
                            x.end_date.HasValue &&
                            (x.schedule_time.Value.Hour - x.end_date.Value.Hour > 8))
                .ToList()
                .Count();
            if (result.total > 0)
            {
                result.wo_on_time = completed_on_time;
                result.wo_delay = wo_delay;
                result.wo_backlog = wo_backlog;
            }
            if (result.total != result.pending + result.completed + result.created + result.rejected + result.assigned)
            {
                result.unknown_count = result.total - result.pending + result.completed + result.created + result.rejected + result.assigned;
            }
            return result;
        }
        internal static string getShortStatus_PM_Plan(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;
            retValue = "";

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.PM_SCHEDULED:
                    retValue = "Scheduled"; break;
                case CMMS.CMMS_Status.PM_ASSIGNED:
                    retValue = "Assigned"; break;
                case CMMS.CMMS_Status.PM_LINKED_TO_PTW:
                    retValue = "Linked To PTW"; break;
                case CMMS.CMMS_Status.PM_START:
                    retValue = "Started"; break;
                case CMMS.CMMS_Status.PM_CLOSED:
                    retValue = "Close - Waiting for Approval"; break;
                case CMMS.CMMS_Status.PM_CLOSE_REJECTED:
                    retValue = "Close - Rejected"; break;
                case CMMS.CMMS_Status.PM_CLOSE_APPROVED:
                    retValue = "Close - Approved"; break;
                case CMMS.CMMS_Status.PM_CANCELLED:
                    retValue = "Cancelled"; break;
                case CMMS.CMMS_Status.PM_CANCELLED_REJECTED:
                    retValue = "Cancelled - Rejected"; break;
                case CMMS.CMMS_Status.PM_CANCELLED_APPROVED:
                    retValue = "Cancelled - Approved"; break;
                case CMMS.CMMS_Status.PM_DELETED:
                    retValue = "Deleted"; break;
                case CMMS.CMMS_Status.PM_UPDATED:
                    retValue = "Updated"; break;
                case CMMS.CMMS_Status.PM_TASK_DELETED:
                    retValue = "Deleted"; break;
                default:
                    break;
            }
            return retValue;

        }

        private Dictionary<int, string> StatusDictionary_MC_Plan = new Dictionary<int, string>()
        {
            { (int)CMMS.CMMS_Status.MC_PLAN_DRAFT, "Draft" },
            { (int)CMMS.CMMS_Status.MC_PLAN_SUBMITTED, "Waiting for Approval" },
            { (int)CMMS.CMMS_Status.MC_PLAN_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.MC_PLAN_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.MC_PLAN_DELETED, "Deleted" },
            { (int)CMMS.CMMS_Status.MC_TASK_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.MC_TASK_STARTED, "In Progress" },
            { (int)CMMS.CMMS_Status.MC_TASK_COMPLETED, "Completed" },
            { (int)CMMS.CMMS_Status.MC_TASK_ABANDONED, "Abandoned" },
            { (int)CMMS.CMMS_Status.MC_TASK_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.MC_TASK_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.SCHEDULED_LINKED_TO_PTW,"Schedule_Linked_To_PTW" },
            { (int)CMMS.CMMS_Status.MC_TASK_END_APPROVED,"MC_Task_Ended_Approved" },
            { (int)CMMS.CMMS_Status.MC_TASK_END_REJECTED,"MC_Task_Ended_Reject" },
            { (int)CMMS.CMMS_Status.MC_TASK_SCHEDULE_APPROVED,"MC_Task_Schedule_Approved" },
            { (int)CMMS.CMMS_Status.MC_TASK_SCHEDULE_REJECT,"MC_Task_Schedule_Reject" },
            { (int)CMMS.CMMS_Status.MC_TASK_RESCHEDULED,"Reschedule" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_DRAFT, "Draft" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_SUBMITTED, "Waiting for Approval" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.VEG_PLAN_DELETED, "Deleted" },
            { (int)CMMS.CMMS_Status.VEG_TASK_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.VEG_TASK_STARTED, "In Progress" },
            { (int)CMMS.CMMS_Status.VEG_TASK_COMPLETED, "Completed" },
            { (int)CMMS.CMMS_Status.VEG_TASK_ABANDONED, "Abandoned" },
            { (int)CMMS.CMMS_Status.VEG_TASK_APPROVED, "Approved" },
            { (int)CMMS.CMMS_Status.VEG_TASK_REJECTED, "Rejected" },
            { (int)CMMS.CMMS_Status.EQUIP_CLEANED, "Cleaned" },
            { (int)CMMS.CMMS_Status.EQUIP_ABANDONED, "Abandoned" },
            { (int)CMMS.CMMS_Status.EQUIP_SCHEDULED, "Scheduled" },
            { (int)CMMS.CMMS_Status.MC_TASK_ABANDONED_REJECTED, "TASK ABANDONED REJECTED" },
            { (int)CMMS.CMMS_Status.MC_TASK_ABANDONED_APPROVED, "TASK ABANDONED APPROVED" },
        };
        public async Task<CMDashboadDetails> getMCPlanDashboardDetails(string facilityId, DateTime fromDate, DateTime toDate)
        {
            CMDashboadDetails result = new CMDashboadDetails();
            string filter = "";

            if (fromDate != null && fromDate.ToString("yyyy-MM-dd") != "0001-01-01" && toDate != null && toDate.ToString("yyyy-MM-dd") != "0001-01-01")
            {
                filter = $" and mc.createdAt between '{fromDate.ToString("yyyy-MM-dd")}' and '{toDate.ToString("yyyy-MM-dd")}'";
            }

            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary_MC_Plan)
            {
                statusOut += $"WHEN mc.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";

            string myQuery12 = $"select mc.facilityId as facility_id,F.name as facility_name,mc.planId as plan_id, mc.id as wo_number ,mp.title as wo_decription,mc.planId,mc.status, CONCAT(createdBy.firstName, createdBy.lastName) as responsibility ," +
                $" mc.startDate as start_date, mc.endedAt as doneDate,mc.prevTaskDoneDate as end_date,freq.name as frequency,mc.noOfDays, {statusOut} as " +
                $"status_long , CASE WHEN mc.moduleType=1 THEN 'Wet' WHEN mc.moduleType=2 THEN 'Dry' ELSE 'Robotic' END as MC_Type,  " +
                $"mc.startDate as  Start_Date ,mc.abandonedAt as  End_Date_done,mc.noOfDays as plan_days,sub1.TotalWaterUsed, sub2.no_of_cleaned,SUM(css.moduleQuantity) as Scheduled " +
                $"from cleaning_execution as mc " +
                $"LEFT join cleaning_plan as mp on mp.planId = mc.planId " +
                $"LEFT join cleaning_execution_items as css on css.executionId = mc.id " +
                $"LEFT JOIN (SELECT executionId, SUM(waterUsed) AS TotalWaterUsed FROM cleaning_execution_schedules GROUP BY executionId) sub1 ON mc.id = sub1.executionId " +
                $"LEFT JOIN (SELECT executionId, SUM(moduleQuantity) AS no_of_cleaned FROM cleaning_execution_items where cleanedById>0 GROUP BY executionId) sub2 ON mc.id = sub2.executionId " +
                $"LEFT JOIN Frequency as freq on freq.id = mp.frequencyId " +
                $"LEFT JOIN users as createdBy ON createdBy.id = mc.assignedTo " +
                $"LEFT JOIN users as approvedBy ON approvedBy.id = mc.approvedByID " +
                $" left join facilities as F on F.id = mc.facilityId  " +
                $"where (mc.moduleType=1 and rescheduled = 0)";
            myQuery12 += $" and mc.facilityId in ({facilityId})  group by mc.id ";

            List<CMDashboadItemList> itemList = await Context.GetData<CMDashboadItemList>(myQuery12).ConfigureAwait(false);
            result.WaterUsedTotal = await WaterUsedTotal(facilityId);
            result.created = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.MC_TASK_SCHEDULED).ToList().Count;
            result.rejected = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.MC_TASK_REJECTED).ToList().Count;

            result.submitted = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.MC_TASK_STARTED).ToList().Count;
            result.approved = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.MC_TASK_APPROVED).ToList().Count;
            result.mc_closed_count = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.MC_TASK_END_APPROVED).ToList().Count;


            result.total = itemList.Count;
            result.completed = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.MC_TASK_COMPLETED).ToList().Count;
            //result.pending = result.total - itemList.Where(x => x.latestJCPTWStatus != (int)CMMS.CMMS_Status.PTW_APPROVED).ToList().Count;
            result.pending = result.total - result.completed;
            result.item_list = itemList;

            int completed_on_time = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.MC_TASK_APPROVED && x.start_date == x.start_date).ToList().Count;
            int wo_delay = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.MC_TASK_APPROVED && x.start_date != x.start_date).ToList().Count;
            int wo_backlog = itemList.Where(x => x.status != (int)CMMS.CMMS_Status.MC_TASK_APPROVED && x.start_date != x.start_date).ToList().Count;

            if (result.total > 0)
            {
                result.wo_on_time = completed_on_time;
                result.wo_delay = wo_delay;
                result.wo_backlog = wo_backlog;
            }
            if (result.total != result.pending + result.completed + result.created + result.rejected + result.assigned)
            {
                result.unknown_count = result.total - result.pending + result.completed + result.created + result.rejected + result.assigned;
            }
            return result;
        }
        private async Task<List<CMWATERUESD>> WaterUsedTotal(string facility_id)
        {

            string Details_query = "SELECT f.name AS site_name, " +
                       "SUM(cse.moduleQuantity) AS total_module_count, " +
                       "SUM(cs.waterUsed) AS TotalWaterUsed, " +
                       "SUM(ce.noOfDays) AS plan_days, " +
                       "SUM(CASE WHEN cse.cleanedById > 0 THEN cse.moduleQuantity ELSE 0 END) AS no_of_cleaned " +
                       "FROM cleaning_execution ce " +
                       "LEFT JOIN cleaning_execution_schedules cs ON ce.id = cs.executionId " +
                       "LEFT JOIN cleaning_execution_items cse ON cse.executionId = ce.id " +
                       "LEFT JOIN facilities f ON ce.facilityId = f.id " +
                       $"WHERE ce.moduleType = 1 AND ce.facilityId IN ({facility_id}) " +
                       "GROUP BY f.name ORDER BY f.name;";

            List<CMWATERUESD> result = await Context.GetData<CMWATERUESD>(Details_query).ConfigureAwait(false);

            return result;

        }
        private Dictionary<int, string> StatusDictionary_IR = new Dictionary<int, string>()
        {
            { 181, "Created-waiting for approval" },
            { 182, "Create-Rejected" },
            { 183, "Created" },
            { 184, "Investigation-waiting for approval" },
            { 185, "Investigation-Rejected" },
            { 186, "Investigation-Completed" },
            { 187, "Updated" },
            { 188, "Cancelled" },
        };
        public async Task<CMDashboadDetails> getIRDashboardDetails(string facilityId, DateTime fromDate, DateTime toDate)
        {
            CMDashboadDetails result = new CMDashboadDetails();

            string filter = " incident.facility_id in (" + facilityId + ") ";
            string Datefilter = "";

            if (fromDate != null && fromDate.ToString("yyyy-MM-dd") != "0001-01-01" && toDate != null && toDate.ToString("yyyy-MM-dd") != "0001-01-01")
            {
                Datefilter = $" and DATE_FORMAT(incident.incident_datetime, '%Y-%m-%d')  between '{fromDate.ToString("yyyy-MM-dd")}' and '{toDate.ToString("yyyy-MM-dd")}'";
            }


            string statusOut = "CASE ";
            foreach (KeyValuePair<int, string> status in StatusDictionary_IR)
            {
                statusOut += $"WHEN incident.status = {status.Key} THEN '{status.Value}' ";
            }
            statusOut += $"ELSE 'Invalid Status' END";


            string selectqry = $"SELECT incident.id as wo_number,incident.title as title, incident.description as wo_decription , " +
                $"incident.incident_datetime as incident_datetime,incident.location_of_incident as location_of_incident, " +
                $"incidenttype.incidenttype as  type_of_incident ," +
                $"incident.action_taken_datetime as restoration_datetime,incident.severity as severity, " +
                $"facilities.name as facility_name,blockName.name as block_name, assets.name as asset_name, " +
                $"incident.risk_level as risk_level, CONCAT(created_by.firstName ,' ' , created_by.lastName) as reported_by_name, " +
                $"incident.created_at as reported_at,CONCAT(user.firstName ,' ' , user.lastName) as approved_by," +
                $" incident.approved_at as approved_at, CONCAT(user1.firstName , ' ' , user1.lastName) as reported_by_name , " +
                $"{statusOut} as status_long ,incident.status,blockName.name location_of_incident, incident_datetime," +
                $" type_of_job,title," +
                $" incident.status, incident.is_why_why_required, incident.is_investigation_required " +
                $" FROM incidents as incident " +
                $" left JOIN facilities AS facilities on facilities.id = incident.facility_id " +
                $" LEFT JOIN facilities AS blockName on blockName.id = incident.block_id  and blockName.isBlock = 1 " +
                $" LEFT JOIN assets as assets on incident.equipment_id = assets.id " +
                $" LEFT JOIN users as user on incident.approved_by = user.id " +
                $" LEFT JOIN users as created_by on incident.created_by = created_by.id " +
                $" LEFT JOIN users as user1 on incident.verified_by = user1.id " +
                $" LEFT JOIN incidenttype as incidenttype on incidenttype.id = incident.risk_type " +
                $" where " + filter + " " + Datefilter + " order by incident.id asc";

            List<CMIncidentList> getIncidentList = await Context.GetData<CMIncidentList>(selectqry).ConfigureAwait(false);


            List<CMDashboadItemList> itemList = await Context.GetData<CMDashboadItemList>(selectqry).ConfigureAwait(false);

            result.created = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.IR_CREATED_INITIAL).ToList().Count;
            result.rejected = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.IR_REJECTED_INITIAL || x.status == (int)CMMS.CMMS_Status.IR_REJECTED_INVESTIGATION).ToList().Count;

            result.submitted = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.IR_CREATED_INVESTIGATION).ToList().Count;
            result.approved = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.IR_APPROVED_INITIAL || x.status == (int)CMMS.CMMS_Status.IR_APPROVED_INVESTIGATION).ToList().Count;


            result.total = itemList.Count;
            result.completed = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.IR_APPROVED_INITIAL || x.status == (int)CMMS.CMMS_Status.IR_APPROVED_INVESTIGATION).ToList().Count;
            //result.pending = result.total - itemList.Where(x => x.latestJCPTWStatus != (int)CMMS.CMMS_Status.PTW_APPROVED).ToList().Count;
            result.pending = result.total - result.completed;
            result.item_list = itemList;

            int completed_on_time = getIncidentList.Where(x => x.status == (int)CMMS.CMMS_Status.IR_APPROVED_INITIAL && x.reported_at == x.reported_at).ToList().Count;
            int wo_delay = getIncidentList.Where(x => x.status == (int)CMMS.CMMS_Status.IR_APPROVED_INITIAL && x.reported_at != x.reported_at).ToList().Count;
            int wo_backlog = getIncidentList.Where(x => x.status != (int)CMMS.CMMS_Status.IR_APPROVED_INITIAL && x.reported_at != x.reported_at).ToList().Count;

            if (result.total > 0)
            {
                result.wo_on_time = completed_on_time;
                result.wo_delay = wo_delay;
                result.wo_backlog = wo_backlog;
            }
            if (result.total != result.pending + result.completed + result.created + result.rejected + result.assigned)
            {
                result.unknown_count = result.total - result.pending + result.completed + result.created + result.rejected + result.assigned;
            }
            return result;
        }
        internal static string getShortStatus_GO(CMMS.CMMS_Modules moduleID, CMMS.CMMS_Status m_notificationID)
        {
            string retValue;

            switch (m_notificationID)
            {
                case CMMS.CMMS_Status.GO_DRAFT:
                    retValue = "GO Raised - Drafted";
                    break;
                case CMMS.CMMS_Status.GO_SUBMITTED:
                    retValue = "GO Raised - Waiting for approval";
                    break;

                case CMMS.CMMS_Status.GO_CLOSED:
                    retValue = "GO Raised - Closed";
                    break;

                case CMMS.CMMS_Status.GO_DELETED:
                    retValue = "GO - Deleted";
                    break;


                case CMMS.CMMS_Status.GO_REJECTED:
                    retValue = "GO Raised - Rejected";
                    break;
                case CMMS.CMMS_Status.GO_APPROVED:
                    retValue = "GO Raised - Approved";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVE_DRAFT:
                    retValue = "Goods Partically Received - Open";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_SUBMITTED:
                    retValue = "Goods Receive - Submitted";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_REJECTED:
                    retValue = "Goods Receive - Rejected";
                    break;
                case CMMS.CMMS_Status.GO_RECEIVED_APPROVED:
                    retValue = "Goods Receive - Approved";
                    break;
                default:
                    retValue = "Unknown <" + m_notificationID + ">";
                    break;
            }
            return retValue;

        }
        public async Task<CMDashboadDetails> getSMDashboardDetails(string facilityId, DateTime fromDate, DateTime toDate)
        {
            CMDashboadDetails result = new CMDashboadDetails();


            string filter = " facilityID in (" + facilityId + ")";
            string datefilter = "";

            if (fromDate != null && fromDate.ToString("yyyy-MM-dd") != "0001-01-01" && toDate != null && toDate.ToString("yyyy-MM-dd") != "0001-01-01")
            {
                datefilter = $" and date_format(pod.lastModifiedDate, '%Y-%m-%d') between '{fromDate.ToString("yyyy-MM-dd")}' and '{toDate.ToString("yyyy-MM-dd")}'";
            }
            //string query = "SELECT fc.name as facilityName,pod.ID as podID,pod.remarks as wo_decription, facilityid as       facility_id,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_type_ID," +
            //    "pod.purchaseID,pod.assetItemID,sai.serial_number,sai.location_ID,(select sum(cost) from smgoodsorderdetails where purchaseID = po.id) as cost,pod.ordered_qty,\r\n bl.name as vendor_name,\r\n     " +
            //    " po.purchaseDate,sam.asset_type_ID,sam.asset_name,po.receiverID,\r\n        " +
            //    "po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty," +
            //    "f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by,pod.order_type as asset_type_ID_OrderDetails, receive_later, " +
            //    "added_to_store,   \r\n      " +
            //    "  po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, po.lr_no, po.condition_pkg_received, " +
            //    "po.vehicle_no, po.gir_no, po.job_ref, po.amount,  po.currency as currencyID , curr.name as currency , stt.asset_type as asset_type_Name,  po_no, requested_qty,lost_qty, ordered_qty, CONCAT(ed.firstName,' ',ed.lastName) as generatedBy\r\n  ,po.received_on as receivedAt    " +
            //    "  FROM smgoodsorderdetails pod\r\n        LEFT JOIN smgoodsorder po ON po.ID = pod.purchaseID\r\n     " +
            //    "   LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID\r\n       " +
            //    " LEFT JOIN smassetmasters sam ON sam.ID = pod.assetItemID\r\n      " +
            //    "  LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement\r\n    " +
            //    "    LEFT JOIN (\r\n            SELECT file.file_path,file.Asset_master_id as Asset_master_id FROM smassetmasterfiles file \r\n " +
            //    "           LEFT join smassetmasters sam on file.Asset_master_id =  sam.id )\r\n        " +
            //    "    f1 ON f1.Asset_master_id = sam.id\r\n        LEFT JOIN (\r\n         " +
            //    "   SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\r\n      " +
            //    "      LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\r\n        )  t1 ON t1.master_ID = sam.ID\r\n     " +
            //    "   LEFT JOIN (\r\n            SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic\r\n          " +
            //    "  LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID\r\n        )  t2 ON t2.master_ID = sam.ID\r\n " +
            //    "       LEFT JOIN facilities fc ON fc.id = po.facilityID\r\nLEFT JOIN users as vendor on vendor.id=po.vendorID " +
            //    "       LEFT JOIN business bl ON bl.id = po.vendorID left join smassettypes stt on stt.ID = pod.order_type LEFT JOIN currency curr ON curr.id = po.currency LEFT JOIN users ed ON ed.id = po.generated_by" +
            //    " WHERE " + filter + " " + datefilter + "";

            string updatedQuery = $"SELECT pod.purchaseID as go_id,po.facilityID as facilityId,fc.name as facilityName," +
                $"gir_no as GRNo,po.status,sam.asset_name as product_name,po_no as GONo,\r\nrequested_qty,ordered_qty," +
                $"amount as total_amount,\r\n(select sum(cost) from smgoodsorderdetails where purchaseID = po.id) " +
                $"as unit_amount, requested_qty as grn_qty, purchaseDate as grn_date, purchaseDate as gr_date, " +
                $"po_date as go_date,\r\n facilityid as       facility_id,\t\r\n        " +
                $"CONCAT(ed.firstName,' ',ed.lastName) as generatedBy\r\n  ,po.received_on as receivedAt      " +
                $"FROM smgoodsorderdetails pod\r\n        LEFT JOIN smgoodsorder po ON po.ID = pod.purchaseID\r\n  " +
                $"      LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID\r\n        " +
                $"LEFT JOIN smassetmasters sam ON sam.ID = pod.assetItemID\r\n        " +
                $"LEFT JOIN facilities fc ON fc.id = po.facilityID\r\nLEFT JOIN users as vendor on vendor.id=po.vendorID" +
                $"        \r\nLEFT JOIN users ed ON ed.id = po.generated_by \r\n" +
                $"  WHERE {filter}  {datefilter} ;";

            List<CMGODashboardList> _List1 = await Context.GetData<CMGODashboardList>(updatedQuery).ConfigureAwait(false);



            List<CMDashboadItemList> itemList = _List1.Select(p => new CMDashboadItemList
            {
                wo_number = p.go_id,
                facility_id = p.facilityId,
                facility_name = p.facilityName,
                assetsname = p.product_name,
                status = p.status,
                go_id = p.go_id,
                GRNo = p.GRNo,
                GONo = p.GONo,
                product_name = p.product_name,
                requested_qty = p.requested_qty,
                gr_date = p.gr_date,
                ordered_qty = p.ordered_qty,
                go_date = p.go_date,
                unit_amount = p.unit_amount,
                total_amount = p.total_amount,
                grn_date = p.grn_date,
                grn_qty = p.grn_qty,

            }).GroupBy(p => p.wo_number).Select(group => group.First()).OrderBy(p => p.wo_number).ToList();

            for (var i = 0; i < itemList.Count; i++)
            {

                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(itemList[i].status);
                string _longStatus = getShortStatus_GO(CMMS.CMMS_Modules.SM_GO, _Status);
                itemList[i].status_long = _longStatus;
            }

            result.created = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_DRAFT).ToList().Count;
            result.rejected = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_REJECTED).ToList().Count;

            //result.submitted = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_SUBMITTED).ToList().Count;
            result.submitted = 4;
            result.approved = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_APPROVED).ToList().Count;


            result.total = itemList.Count;
            result.completed = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_APPROVED).ToList().Count;
            result.pending = result.total - result.completed;
            result.item_list = itemList;
            result.po_items_awaited = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_CLOSED).ToList().Count;
            result.low_stock_items = 4;

            int completed_on_time = _List1.Where(x => x.status == (int)CMMS.CMMS_Status.GO_APPROVED && x.go_date != x.go_date).ToList().Count;
            int wo_delay = _List1.Where(x => x.status != (int)CMMS.CMMS_Status.GO_APPROVED && x.go_date != x.go_date).ToList().Count;
            int wo_backlog = _List1.Where(x => x.status != (int)CMMS.CMMS_Status.GO_APPROVED && x.go_date != x.go_date).ToList().Count;

            if (result.total > 0)
            {
                result.wo_on_time = completed_on_time;
                result.wo_delay = wo_delay;
                result.wo_backlog = wo_backlog;
            }
            if (result.total != result.pending + result.completed + result.created + result.rejected + result.assigned)
            {
                result.unknown_count = result.total - result.pending + result.completed + result.created + result.rejected + result.assigned;
            }

            result.StockConsumptionByGoods = await getStockConsumptionByGoods(facilityId, fromDate, toDate);
            result.StockConsumptionBySites = await getStockConsumptionBySites(fromDate, toDate);
            result.StockAvailbleByGoods = await getStockAvailableByGoods(facilityId, fromDate, toDate);
            result.StockAvailbleBySites = await getStockAvailableBySites(fromDate, toDate);
            result.StockOverview = await StockOverview(facilityId, fromDate, toDate);

            var test = await getLowStockItemList(facilityId, fromDate, toDate);
            return result;
        }
        public async Task<CMDashboadDetails> getLowStockItemList(string facilityId, DateTime fromDate, DateTime toDate)
        {
            CMDashboadDetails result = new CMDashboadDetails();


            string filter = " facilityID in (" + facilityId + ")";
            string datefilter = "";

            if (fromDate != null && fromDate.ToString("yyyy-MM-dd") != "0001-01-01" && toDate != null && toDate.ToString("yyyy-MM-dd") != "0001-01-01")
            {
                datefilter = $" and pod.lastModifiedDate between '{fromDate.ToString("yyyy-MM-dd")}' and '{toDate.ToString("yyyy-MM-dd")}'";
            }


            string query = "SELECT fc.name as facilityName,pod.ID as podID, facilityid as       facility_id,pod.spare_status,pod.remarks,sai.orderflag,sam.asset_type_ID," +
                "pod.purchaseID,pod.assetItemID,sai.serial_number,sai.location_ID,(select sum(cost) from smgoodsorderdetails where purchaseID = po.id) as cost,pod.ordered_qty,\r\n bl.name as vendor_name,\r\n     " +
                " po.purchaseDate,sam.asset_type_ID,sam.asset_name,po.receiverID,\r\n        " +
                "po.vendorID,po.status,sai.asset_code,t1.asset_type,t2.cat_name,pod.received_qty,pod.damaged_qty,pod.accepted_qty," +
                "f1.file_path,f1.Asset_master_id,sm.decimal_status,sm.spare_multi_selection,po.generated_by,pod.order_type as asset_type_ID_OrderDetails, receive_later, " +
                "added_to_store,   \r\n      " +
                "  po.challan_no, po.po_no, po.freight, po.transport, po.no_pkg_received, po.lr_no, po.condition_pkg_received, " +
                "po.vehicle_no, po.gir_no, po.job_ref, po.amount,  po.currency as currencyID , curr.name as currency , stt.asset_type as asset_type_Name,  po_no, requested_qty,lost_qty, ordered_qty, CONCAT(ed.firstName,' ',ed.lastName) as generatedBy\r\n  ,po.received_on as receivedAt    " +
                "  FROM smgoodsorderdetails pod\r\n        LEFT JOIN smgoodsorder po ON po.ID = pod.purchaseID\r\n     " +
                "   LEFT JOIN smassetitems sai ON sai.ID = pod.assetItemID\r\n       " +
                " LEFT JOIN smassetmasters sam ON sam.ID = pod.assetItemID\r\n      " +
                "  LEFT JOIN smunitmeasurement sm ON sm.ID = sam.unit_of_measurement\r\n    " +
                "    LEFT JOIN (\r\n            SELECT file.file_path,file.Asset_master_id as Asset_master_id FROM smassetmasterfiles file \r\n " +
                "           LEFT join smassetmasters sam on file.Asset_master_id =  sam.id )\r\n        " +
                "    f1 ON f1.Asset_master_id = sam.id\r\n        LEFT JOIN (\r\n         " +
                "   SELECT sat.asset_type,s1.ID as master_ID FROM smassettypes sat\r\n      " +
                "      LEFT JOIN smassetmasters s1 ON s1.asset_type_ID = sat.ID\r\n        )  t1 ON t1.master_ID = sam.ID\r\n     " +
                "   LEFT JOIN (\r\n            SELECT sic.cat_name,s2.ID as master_ID FROM smitemcategory sic\r\n          " +
                "  LEFT JOIN smassetmasters s2 ON s2.item_category_ID = sic.ID\r\n        )  t2 ON t2.master_ID = sam.ID\r\n " +
                "       LEFT JOIN facilities fc ON fc.id = po.facilityID\r\nLEFT JOIN users as vendor on vendor.id=po.vendorID " +
                "       LEFT JOIN business bl ON bl.id = po.vendorID left join smassettypes stt on stt.ID = pod.order_type LEFT JOIN currency curr ON curr.id = po.currency LEFT JOIN users ed ON ed.id = po.generated_by" +
                " WHERE " + filter + " " + datefilter + "";

            List<CMGoodsOrderList> _List = await Context.GetData<CMGoodsOrderList>(query).ConfigureAwait(false);

            List<CMDashboadItemList> itemList = _List.Select(p => new CMDashboadItemList
            {
                wo_number = p.purchaseID,
                facility_id = p.facility_id,
                facility_name = p.facilityName,
                assetsname = p.asset_name,
                status = p.status,
                start_date = p.purchaseDate

            }).GroupBy(p => p.wo_number).Select(group => group.First()).OrderBy(p => p.wo_number).ToList();


            for (var i = 0; i < _List.Count; i++)
            {
                CMDashboadItemList item = new CMDashboadItemList();
                CMMS.CMMS_Status _Status = (CMMS.CMMS_Status)(_List[i].status);
                string _longStatus = getShortStatus_GO(CMMS.CMMS_Modules.SM_GO, _Status);
                item.status_long = _longStatus;
            }

            result.created = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_DRAFT).ToList().Count;
            result.rejected = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_REJECTED).ToList().Count;

            result.submitted = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_SUBMITTED).ToList().Count;
            result.approved = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_APPROVED).ToList().Count;


            result.total = itemList.Count;
            result.completed = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_APPROVED).ToList().Count;
            result.pending = result.total - result.completed;
            result.po_items_awaited = itemList.Where(x => x.status == (int)CMMS.CMMS_Status.GO_CLOSED).ToList().Count;
            result.item_list = itemList;

            int completed_on_time = _List.Where(x => x.status == (int)CMMS.CMMS_Status.GO_APPROVED && x.purchaseDate != x.purchaseDate).ToList().Count;
            int wo_delay = _List.Where(x => x.status != (int)CMMS.CMMS_Status.GO_APPROVED && x.purchaseDate != x.purchaseDate).ToList().Count;
            int wo_backlog = _List.Where(x => x.status != (int)CMMS.CMMS_Status.GO_APPROVED && x.purchaseDate != x.purchaseDate).ToList().Count;

            if (result.total > 0)
            {
                result.wo_on_time = completed_on_time;
                result.wo_delay = wo_delay;
                result.wo_backlog = wo_backlog;
            }
            return result;
        }

        public async Task<List<CMSMConsumptionByGoods>> getStockAvailableByGoods(string facility_id, DateTime StartDate, DateTime EndDate)
        {

            string Plant_Stock_Opening_Details_query = $"select     asset_type, SUM(opening) AS Opening, SUM(inward) AS inward," +
                 $" SUM(outward) AS outward  from (SELECT ifnull(smc.cat_name,'Others') as asset_type, " +
        $" IFNULL((select sum(ST.creditQty)-sum(ST.debitQty)  FROM smtransition as ST  JOIN smassetmasters as SM ON SM.ID = ST.assetItemID  " +
        $" LEFT JOIN facilities fcc ON fcc.id = ST.facilityID   where   ST.actorType = {(int)CMMS.SM_Actor_Types.Store} and SM.ID=a_master.ID  and ST.facilityID in ({facility_id})" +
        $" and sm_trans.actorID in ({facility_id}) and date_format(ST.lastModifiedDate, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}'  group by SM.asset_code),0) Opening," +
        $"  IFNULL((select sum(si.creditQty) from smtransition si where si.id = sm_trans.id and  date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') " +
        $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' ),0) as inward, " +
        $"   IFNULL((select sum(so.debitQty) from smtransition so where so.id = sm_trans.id and  date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') " +
        $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' ),0) as outward " +
        $" FROM smtransition as sm_trans " +
        $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID " +
        $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
        $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
        $"  left join smitemcategory smc on smc.ID = a_master.item_category_ID " +
        $" where sm_trans.actorType = {(int)CMMS.SM_Actor_Types.Store} and sm_trans.facilityID in ({facility_id}) and " +
        $" sm_trans.actorID in ({facility_id}) group by a_master.asset_code) a GROUP BY asset_type;";

            List<CMPlantStockOpening> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMPlantStockOpening>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);
            List<CMSMConsumptionByGoods> result = new List<CMSMConsumptionByGoods>();
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {
                item.balance = item.Opening + item.inward - item.outward;
                CMSMConsumptionByGoods category_item = new CMSMConsumptionByGoods();
                category_item.key = item.asset_type;
                category_item.value = item.balance;
                result.Add(category_item);
            }



            return result;
        }

        public async Task<List<CMSMConsumptionByGoods>> getStockAvailableBySites(DateTime StartDate, DateTime EndDate)
        {

            string Plant_Stock_Opening_Details_query = $"select     facilityName, SUM(opening) AS Opening, SUM(inward) AS inward," +
                 $" SUM(outward) AS outward  from (SELECT fc.name as facilityName, " +
        $" IFNULL((select sum(ST.creditQty)-sum(ST.debitQty)  FROM smtransition as ST  JOIN smassetmasters as SM ON SM.ID = ST.assetItemID  " +
        $" LEFT JOIN facilities fcc ON fcc.id = ST.facilityID   where   ST.actorType = {(int)CMMS.SM_Actor_Types.Store} and SM.ID=a_master.ID " +
        $" and sm_trans.actorID =  sm_trans.facilityID and date_format(ST.lastModifiedDate, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}'  group by SM.asset_code),0) Opening," +
        $"  IFNULL((select sum(si.creditQty) from smtransition si where si.id = sm_trans.id and  date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') " +
        $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' ),0) as inward, " +
        $"   IFNULL((select sum(so.debitQty) from smtransition so where so.id = sm_trans.id and  date_format(sm_trans.lastModifiedDate, '%Y-%m-%d') " +
        $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' ),0) as outward " +
        $" FROM smtransition as sm_trans " +
        $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID " +
        $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
        $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
        $"  left join smitemcategory smc on smc.ID = a_master.item_category_ID " +
        $" where sm_trans.actorType = {(int)CMMS.SM_Actor_Types.Store} and " +
        $" sm_trans.actorID = sm_trans.facilityID   group by a_master.asset_code) a GROUP BY facilityName;";

            List<CMPlantStockOpening> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMPlantStockOpening>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);
            List<CMSMConsumptionByGoods> result = new List<CMSMConsumptionByGoods>();
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {
                item.balance = item.Opening + item.inward - item.outward;
                CMSMConsumptionByGoods category_item = new CMSMConsumptionByGoods();
                category_item.key = item.facilityName;
                category_item.value = item.balance;
                result.Add(category_item);
            }



            return result;
        }

        public async Task<List<CMSMConsumptionByGoods>> StockOverview(string facility_id, DateTime StartDate, DateTime EndDate)
        {

            //string Plant_Stock_Opening_Details_query = $"select SUM(opening) AS Opening, SUM(inward) AS inward," +
            //    $" SUM(outward) AS outward  from (SELECT ifnull(smc.cat_name,'Others') as asset_type, " +
            //    $" IFNULL((select SUM(smt.qty) from smtransactiondetails smt where date_format(smt.lastInsetedDateTime, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}' and  smt.fromActorType = {(int)CMMS.SM_Actor_Types.Vendor}  and smt.assetItemID = sm_trans.assetItemID and smt.toActorType ={(int)CMMS.SM_Actor_Types.Store} and smt.PlantId in ({facility_id}) ),0) - " +
            //    $" IFNULL((select SUM(smt1.qty) from smtransactiondetails smt1  where date_format(smt1.lastInsetedDateTime, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}' and smt1.assetItemID = sm_trans.assetItemID  and smt1.fromActorType IN ({(int)CMMS.SM_Actor_Types.PM_Task},{(int)CMMS.SM_Actor_Types.JobCard},{(int)CMMS.SM_Actor_Types.Engineer}) and smt1.toActorType ={(int)CMMS.SM_Actor_Types.Inventory} and smt1.PlantId in ({facility_id})),0) as Opening,  " +
            //    $"  IFNULL((select SUM(smt.qty) from smtransactiondetails smt where date_format(smt.lastInsetedDateTime, '%Y-%m-%d') " +
            //    $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' and  smt.fromActorType = {(int)CMMS.SM_Actor_Types.Vendor}  and smt.assetItemID = sm_trans.assetItemID and smt.toActorType ={(int)CMMS.SM_Actor_Types.Store} and smt.PlantId in ({facility_id}) ),0) as inward, " +
            //    $"   IFNULL((select SUM(smt1.qty) from smtransactiondetails smt1  where date_format(smt1.lastInsetedDateTime, '%Y-%m-%d') " +
            //    $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}'  and smt1.assetItemID = sm_trans.assetItemID  and smt1.fromActorType IN ({(int)CMMS.SM_Actor_Types.PM_Task},{(int)CMMS.SM_Actor_Types.JobCard},{(int)CMMS.SM_Actor_Types.Engineer}) and smt1.toActorType ={(int)CMMS.SM_Actor_Types.Inventory} and smt1.PlantId in ({facility_id})),0) as outward  " +
            //    $" FROM smtransition as sm_trans " +
            //    $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID " +
            //    $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
            //    $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
            //    $"  left join smitemcategory smc on smc.ID = a_master.item_category_ID " +
            //    $" where sm_trans.actorType = {(int)CMMS.SM_Actor_Types.Store} and sm_trans.facilityID in ({facility_id}) and " +
            //    $" sm_trans.actorID in ({facility_id}) group by a_master.asset_code) a;";


            string Plant_Stock_Opening_Details_query = $"select SUM(opening) AS Opening, SUM(inward) AS inward,SUM(outward) AS outward  from (" +
                 $"SELECT  sm_trans.facilityID as facilityID, fc.name as facilityName," +
                 $"fc.isBlock as Facility_Is_Block, " +
                 $" '' as Facility_Is_Block_of_name,sm_trans.assetItemID, a_master.asset_name, a_master.asset_code," +
                 $" a_master.asset_type_ID, AST.asset_type,  " +
                 $" IFNULL((select SUM(smt.qty) from smtransactiondetails smt where date_format(smt.lastInsetedDateTime, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}' and  smt.fromActorType = {(int)CMMS.SM_Actor_Types.Vendor}  and smt.assetItemID = sm_trans.assetItemID and smt.toActorType ={(int)CMMS.SM_Actor_Types.Store} and smt.PlantId in ({facility_id}) ),0) - " +
                 $" IFNULL((select SUM(smt1.qty) from smtransactiondetails smt1  where date_format(smt1.lastInsetedDateTime, '%Y-%m-%d') < '{StartDate.ToString("yyyy-MM-dd")}' and smt1.assetItemID = sm_trans.assetItemID  and smt1.fromActorType IN ({(int)CMMS.SM_Actor_Types.PM_Task},{(int)CMMS.SM_Actor_Types.JobCard},{(int)CMMS.SM_Actor_Types.Engineer}) and smt1.toActorType ={(int)CMMS.SM_Actor_Types.Inventory} and smt1.PlantId in ({facility_id})),0) as Opening,  " +
                 $"  IFNULL((select SUM(smt.qty) from smtransactiondetails smt where date_format(smt.lastInsetedDateTime, '%Y-%m-%d') " +
                 $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' and  smt.fromActorType = {(int)CMMS.SM_Actor_Types.Vendor}  and smt.assetItemID = sm_trans.assetItemID and smt.toActorType ={(int)CMMS.SM_Actor_Types.Store} and smt.PlantId in ({facility_id}) ),0) as inward, " +
                 $"   IFNULL((select SUM(smt1.qty) from smtransactiondetails smt1  where date_format(smt1.lastInsetedDateTime, '%Y-%m-%d') " +
                 $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}'  and smt1.assetItemID = sm_trans.assetItemID  and smt1.fromActorType IN ({(int)CMMS.SM_Actor_Types.PM_Task},{(int)CMMS.SM_Actor_Types.JobCard},{(int)CMMS.SM_Actor_Types.Engineer}) and smt1.toActorType ={(int)CMMS.SM_Actor_Types.Inventory} and smt1.PlantId in ({facility_id})),0) as outward  " +
                 $" FROM smtransition as sm_trans " +
                 $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID " +
                 $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
                 $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
                 $" where sm_trans.actorType = {(int)CMMS.SM_Actor_Types.Store} and sm_trans.facilityID in ({facility_id}) and " +
                 $" sm_trans.actorID in ({facility_id})) a ";

            List<CMPlantStockOpening> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMPlantStockOpening>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);
            List<CMSMConsumptionByGoods> result = new List<CMSMConsumptionByGoods>();
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {
                item.balance = item.Opening + item.inward - item.outward;

            }

            CMSMConsumptionByGoods category_item_opening = new CMSMConsumptionByGoods();
            CMSMConsumptionByGoods category_item_inward = new CMSMConsumptionByGoods();
            CMSMConsumptionByGoods category_item_outward = new CMSMConsumptionByGoods();
            CMSMConsumptionByGoods category_item_balance = new CMSMConsumptionByGoods();
            category_item_opening.key = "Opening";
            category_item_opening.value = Plant_Stock_Opening_Details_Reader[0].Opening;
            category_item_inward.key = "Inward";
            category_item_inward.value = Plant_Stock_Opening_Details_Reader[0].inward;
            category_item_outward.key = "Outward";
            category_item_outward.value = Plant_Stock_Opening_Details_Reader[0].outward;
            category_item_balance.key = "Balance";
            category_item_balance.value = Plant_Stock_Opening_Details_Reader[0].balance;
            result.Add(category_item_opening);
            result.Add(category_item_inward);
            result.Add(category_item_outward);
            result.Add(category_item_balance);



            return result;
        }
        public async Task<List<CMSMConsumptionByGoods>> getStockConsumptionByGoods(string facility_id, DateTime StartDate, DateTime EndDate)
        {

            string Plant_Stock_Opening_Details_query = $"select     asset_type, SUM(opening) AS Opening, SUM(inward) AS inward," +
                 $" SUM(outward) AS outward  from (" +
                 $"SELECT ifnull(smc.cat_name,'Others') as asset_type, " +
                $" IFNULL((select SUM(smt1.qty) from smtransactiondetails smt1  where date_format(smt1.lastInsetedDateTime, '%Y-%m-%d') BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}'  and smt1.assetItemID = sm_trans.assetItemID  and smt1.fromActorType IN ({(int)CMMS.SM_Actor_Types.PM_Task},{(int)CMMS.SM_Actor_Types.JobCard},{(int)CMMS.SM_Actor_Types.Engineer}) and smt1.toActorType ={(int)CMMS.SM_Actor_Types.Inventory} and smt1.PlantId in ({facility_id})),0) - " +
                $" IFNULL((select SUM(smt.qty) from smtransactiondetails smt where date_format(smt.lastInsetedDateTime, '%Y-%m-%d') BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' and  smt.fromActorType = {(int)CMMS.SM_Actor_Types.Vendor}  and smt.assetItemID = sm_trans.assetItemID and smt.toActorType ={(int)CMMS.SM_Actor_Types.Store} and smt.PlantId in ({facility_id}) ),0) as Opening," +
                $"  IFNULL((select SUM(smt.qty) from smtransactiondetails smt where date_format(smt.lastInsetedDateTime, '%Y-%m-%d') " +
                $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' and  smt.fromActorType = {(int)CMMS.SM_Actor_Types.Vendor}  and smt.assetItemID = sm_trans.assetItemID and smt.toActorType ={(int)CMMS.SM_Actor_Types.Store} and smt.PlantId in ({facility_id}) ),0) as inward, " +
                $"   IFNULL((select SUM(smt1.qty) from smtransactiondetails smt1  where date_format(smt1.lastInsetedDateTime, '%Y-%m-%d') " +
                $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}'  and smt1.assetItemID = sm_trans.assetItemID  and smt1.fromActorType IN ({(int)CMMS.SM_Actor_Types.PM_Task},{(int)CMMS.SM_Actor_Types.JobCard},{(int)CMMS.SM_Actor_Types.Engineer}) and smt1.toActorType ={(int)CMMS.SM_Actor_Types.Inventory} and smt1.PlantId in ({facility_id})),0) as outward  " +
        $" FROM smtransition as sm_trans " +
        $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID " +
        $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
        $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
        $"  left join smitemcategory smc on smc.ID = a_master.item_category_ID " +
        $" where sm_trans.actorType = {(int)CMMS.SM_Actor_Types.Store} and sm_trans.facilityID in ({facility_id}) and " +
        $" sm_trans.actorID in ({facility_id}) group by a_master.asset_code" +
        $"" +
        $") a GROUP BY asset_type;";

            List<CMPlantStockOpening> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMPlantStockOpening>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);
            List<CMSMConsumptionByGoods> result = new List<CMSMConsumptionByGoods>();
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {
                item.balance = item.Opening + item.inward - item.outward;
                CMSMConsumptionByGoods category_item = new CMSMConsumptionByGoods();
                category_item.key = item.asset_type;
                category_item.value = item.balance;
                result.Add(category_item);
            }



            return result;
        }

        public async Task<List<CMSMConsumptionByGoods>> getStockConsumptionBySites(DateTime StartDate, DateTime EndDate)
        {
            /*
                        string Plant_Stock_Opening_Details_query = $"select     facilityName, SUM(opening) AS Opening, SUM(inward) AS inward, SUM(go_order.amount) as amount ," +
                            $" SUM(outward) AS outward  from (SELECT fc.name as facilityName, " +
                            $" IFNULL((select SUM(smt1.qty) from smtransactiondetails smt1  where date_format(smt1.lastInsetedDateTime, '%Y-%m-%d') BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}'  and smt1.assetItemID = sm_trans.assetItemID  and smt1.fromActorType IN ({(int)CMMS.SM_Actor_Types.PM_Task},{(int)CMMS.SM_Actor_Types.JobCard},{(int)CMMS.SM_Actor_Types.Engineer}) and smt1.toActorType ={(int)CMMS.SM_Actor_Types.Inventory} and smt1.PlantId in (sm_trans.facilityID)),0) - " +
                            $" IFNULL((select SUM(smt.qty) from smtransactiondetails smt where date_format(smt.lastInsetedDateTime, '%Y-%m-%d') BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' and  smt.fromActorType = {(int)CMMS.SM_Actor_Types.Vendor}  and smt.assetItemID = sm_trans.assetItemID and smt.toActorType ={(int)CMMS.SM_Actor_Types.Store} and smt.PlantId in (sm_trans.facilityID) ),0) as Opening," +
                            $"  IFNULL((select SUM(smt.qty) from smtransactiondetails smt where date_format(smt.lastInsetedDateTime, '%Y-%m-%d') " +
                            $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}' and  smt.fromActorType = {(int)CMMS.SM_Actor_Types.Vendor}  and smt.assetItemID = sm_trans.assetItemID and smt.toActorType ={(int)CMMS.SM_Actor_Types.Store} and smt.PlantId in (sm_trans.facilityID) ),0) as inward, " +
                            $"   IFNULL((select SUM(smt1.qty) from smtransactiondetails smt1  where date_format(smt1.lastInsetedDateTime, '%Y-%m-%d') " +
                            $" BETWEEN '{StartDate.ToString("yyyy-MM-dd")}' AND '{EndDate.ToString("yyyy-MM-dd")}'  and smt1.assetItemID = sm_trans.assetItemID  and smt1.fromActorType IN ({(int)CMMS.SM_Actor_Types.PM_Task},{(int)CMMS.SM_Actor_Types.JobCard},{(int)CMMS.SM_Actor_Types.Engineer}) and smt1.toActorType ={(int)CMMS.SM_Actor_Types.Inventory} and smt1.PlantId in (sm_trans.facilityID)),0) as outward  " +
                            $" FROM smtransition as sm_trans " +
                            $" JOIN smassetmasters as a_master ON a_master.ID = sm_trans.assetItemID " +
                            $" JOIN smgoodsorder as go_order ON go_order.facilityID = a_master.plant_ID " +
                            $" LEFT JOIN facilities fc ON fc.id = sm_trans.facilityID " +
                            $" Left join smassettypes AST on AST.id = a_master.asset_type_ID " +
                            $"  left join smitemcategory smc on smc.ID = a_master.item_category_ID " +
                            $" where sm_trans.actorType = {(int)CMMS.SM_Actor_Types.Store} and " +
                            $" sm_trans.actorID = sm_trans.facilityID   group by a_master.asset_code) a GROUP BY facilityName;";*/
            string Plant_Stock_Opening_Details_query = "select fc.name as facilityName, sm.amount " +
                                                      "FROM smgoodsorder AS sm  LEFT JOIN facilities fc ON fc.id = sm.facilityID " +
                                                      "GROUP BY sm.facilityID;";

            List<CMPlantStockOpening> Plant_Stock_Opening_Details_Reader = await Context.GetData<CMPlantStockOpening>(Plant_Stock_Opening_Details_query).ConfigureAwait(false);
            List<CMSMConsumptionByGoods> result = new List<CMSMConsumptionByGoods>();
            foreach (var item in Plant_Stock_Opening_Details_Reader)
            {
                //item.balance = item.Opening + item.inward - item.outward;
                CMSMConsumptionByGoods category_item = new CMSMConsumptionByGoods();
                category_item.key = item.facilityName;
                category_item.value = item.amount;
                result.Add(category_item);
            }

            return result;
        }
    }

}
