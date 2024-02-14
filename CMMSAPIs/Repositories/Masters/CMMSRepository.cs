using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using CMMSAPIs.Repositories.Jobs;
using CMMSAPIs.Repositories.Permits;
using CMMSAPIs.Repositories.JC;
using CMMSAPIs.Repositories.Incident_Reports;
using CMMSAPIs.Repositories.WC;
using CMMSAPIs.Repositories.Calibration;
using CMMSAPIs.Repositories.Inventory;
using CMMSAPIs.Models.Jobs;
using CMMSAPIs.Models.Permits;
using CMMSAPIs.Models.JC;
using CMMSAPIs.Models.Incident_Reports;
using CMMSAPIs.Models.WC;
using CMMSAPIs.Models.Calibration;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Notifications;
using System.IO;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;

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
            { CMMS.CMMS_Modules.VEGETATION, 33 }
        };
        public CMMSRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment _webHost = null) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            m_errorLog = new ErrorLog(_webHost);
            getDB = sqlDBHelper;
        }

        internal async Task<CMDefaultResponse> AddModule(CMModule request)
        {
            /*
             * Read property of CMModule and insert into Features table
            */
            CMDefaultResponse response = new CMDefaultResponse();
            if (request.software_id == 0)
            {
                //throw new ArgumentException("ModuleName <" +request.moduleName+ "> For This Module Name Software Id Is Not Present Please contact Backend Team For Adding Software_id");
                // response = new CMDefaultResponse(request.software_id, CMMS.RETRUNSTATUS.FAILURE, "For This Module Name <" + request.moduleName + "> Software Id Is Not Present Please contact Backend Team For Adding Software_id");
                // return response;
                string Q = "select max(softwareid)+1 from features ; ";
                DataTable dt_software = await Context.FetchData(Q).ConfigureAwait(false);
                request.software_id = Convert.ToInt32(dt_software.Rows[0][0]);
            }
            string myQuery = "INSERT INTO features(`moduleName`,softwareid, `featureName`, `menuImage`, `add`, `edit`, `delete`, `view`, `issue`, `approve`, `selfView`) " +
                $"VALUES('{request.moduleName}',{request.software_id}, '{request.featureName}', '{request.menuImage}', {request.add}, {request.edit}, " +
                $"{request.delete}, {request.view}, {request.issue}, {request.approve}, {request.selfView}); SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
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
            await Context.ExecuteNonQry<int>(myQuery).ConfigureAwait(false);
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
        internal async Task<List<CMModule>> GetModuleList()
        {
            /*
             * Return List of modules from Features table
            */
            string myQuery = "SELECT * FROM features; ";
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

        internal async Task<List<CMStatus>> GetStatusList(CMMS.CMMS_Modules module)
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
            List<CMStatus> _statusList = await Context.GetData<CMStatus>(myQuery).ConfigureAwait(false);
            return _statusList;
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
                                    "u.id, loginId as login_id, concat(firstName,' ', lastName) as name, birthday as birthdate, " +
                                    "gender, mobileNumber, cities.name as city, states.name as state, countries.name as country, zipcode as pin " +
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
                             "WHERE " +
                                    $"u.status = 1 AND uf.status = 1 AND isEmployee = 1 AND uf.facilityId = {facility_id} ";
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

            foreach(CMEmployee emp in _Employee)
            {
                CMResposibility reponsibility1 = new CMResposibility();
                reponsibility1.id = 1;
                reponsibility1.name = "Testing";
                reponsibility1.since = DateTime.Parse("12-02-2020");
                reponsibility1.experianceYears = 2;

                CMResposibility reponsibility2 = new CMResposibility();
                reponsibility2.id = 2;
                reponsibility2.name = "Plumber";
                reponsibility2.since = DateTime.Parse("12-12-2018");
                reponsibility2.experianceYears = 4;
                emp.responsibility = new List<CMResposibility> () ;
                emp.responsibility.Add(reponsibility1);
                emp.responsibility.Add(reponsibility2);

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
            if (request.website != null && request.website != "")
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
        internal async Task<List<CMBusiness>> GetBusinessList(int businessType)
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
            return _Business;
        }

        internal async Task<List<CMFrequency>> GetFrequencyList()
        {
            string myQuery = $"SELECT id,name, days FROM Frequency where status = 1";
            List<CMFrequency> _FrequencyList = await Context.GetData<CMFrequency>(myQuery).ConfigureAwait(false);
            return _FrequencyList;
        }

        internal async Task<string> Print(int id, CMMS.CMMS_Modules moduleID, params object[] args)
        {
            CMMSNotification.print = true;
            CMMS.CMMS_Status notificationID;

            switch (moduleID)
            {

                case CMMS.CMMS_Modules.JOB:
                    JobRepository obj = new JobRepository(getDB);
                    CMJobView _jobView = await obj.GetJobDetails(id);
                    notificationID = (CMMS.CMMS_Status)(_jobView.status);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _jobView);
                    break;
                case CMMS.CMMS_Modules.PTW:
                    PermitRepository obj1 = new PermitRepository(getDB);
                    CMPermitDetail _Permit = await obj1.GetPermitDetails(id);
                    notificationID = (CMMS.CMMS_Status)(_Permit.ptwStatus);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _Permit);
                    break;
                case CMMS.CMMS_Modules.JOBCARD:
                    JCRepository obj2 = new JCRepository(getDB);
                    List<CMJCDetail> _JobCard = await obj2.GetJCDetail(id);
                    notificationID = (CMMS.CMMS_Status)(_JobCard[0].status);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _JobCard[0]);
                    break;
                case CMMS.CMMS_Modules.INCIDENT_REPORT:
                    IncidentReportRepository obj3 = new IncidentReportRepository(getDB);
                    CMViewIncidentReport _IncidentReport = await obj3.GetIncidentDetailsReport(id);
                    notificationID = (CMMS.CMMS_Status)(_IncidentReport.status);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _IncidentReport);
                    break;
                case CMMS.CMMS_Modules.WARRANTY_CLAIM:
                    WCRepository obj4 = new WCRepository(getDB);
                    CMWCDetail _WC = await obj4.GetWCDetails(id);
                    notificationID = (CMMS.CMMS_Status)(_WC.status);
                    await CMMSNotification.sendNotification(moduleID, notificationID, null, _WC);
                    break;
                case CMMS.CMMS_Modules.CALIBRATION:
                    CalibrationRepository obj5 = new CalibrationRepository(getDB);
                    CMCalibrationDetails _Calibration = await obj5.GetCalibrationDetails(id);
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

            HTMLBody += CMMSNotification.printBody;

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
                            if (Convert.ToString(newR["website"]) == null || Convert.ToString(newR["website"]) == "")
                            {
                                m_errorLog.SetError($"[Row {rN}] Website cannot be empty.");
                            }
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
    }

}
