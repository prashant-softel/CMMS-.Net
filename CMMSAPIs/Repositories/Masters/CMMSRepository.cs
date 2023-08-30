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

namespace CMMSAPIs.Repositories.Masters
{
    public class CMMSRepository : GenericRepository  
    {
        private UtilsRepository _utilsRepo;
        public static Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        private MYSQLDBHelper getDB;

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
            { CMMS.CMMS_Modules.SM_PO, 14 },
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
        public CMMSRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
            getDB = sqlDBHelper;
        }

        internal async Task<CMDefaultResponse> AddModule(CMModule request)
        {
            /*
             * Read property of CMModule and insert into Features table
            */
            string myQuery = "INSERT INTO features(`moduleName`, `featureName`, `menuImage`, `add`, `edit`, `delete`, `view`, `issue`, `approve`, `selfView`) " + 
                $"VALUES('{request.moduleName}', '{request.featureName}', '{request.menuImage}', {request.add}, {request.edit}, " + 
                $"{request.delete}, {request.view}, {request.issue}, {request.approve}, {request.selfView}); SELECT LAST_INSERT_ID();";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Module Added Successfully");
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
                ", CONCAT(u.firstName , ' ' , u.lastName) as customer ,CONCAT(u2.firstName , ' ' , u2.lastName) as owner,CONCAT(u3.firstName , ' ' , u3.lastName) as Operator" +
                " FROM Facilities LEFT JOIN spv ON facilities.spvId=spv.id LEFT JOIN users as u ON u.id = facilities.customerId LEFT JOIN users as u2 ON u2.id = facilities.ownerId LEFT JOIN users as u3 ON u3.id = facilities.operatorId WHERE isBlock = 0 and facilities.status = 1;"; 
            
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
            if(module != 0 && access != 0)
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

        internal  async Task<string> Print(int id, CMMS.CMMS_Modules moduleID, params object[] args)
        {
            CMMSNotification.print = true;
            CMMS.CMMS_Status notificationID;

            switch (moduleID)
            {

                case CMMS.CMMS_Modules.JOB:
                    JobRepository obj = new JobRepository(getDB);
                    CMJobView _jobView = await obj.GetJobDetails(id);
                    notificationID = (CMMS.CMMS_Status)(_jobView.status);
                    await CMMSNotification.sendNotification(moduleID, notificationID,null, _jobView);
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
    }

}
