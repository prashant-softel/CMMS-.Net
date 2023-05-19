using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;

namespace CMMSAPIs.Repositories.Masters
{
    public class CMMSRepository : GenericRepository
    {
        private UtilsRepository _utilsRepo;

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
            { CMMS.CMMS_Modules.MODULE_CLEANING, 32 },
            { CMMS.CMMS_Modules.VEGETATION, 33 }
        };
        public CMMSRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _utilsRepo = new UtilsRepository(sqlDBHelper);
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

        internal async Task<List<CMFinancialYear>> GetFinancialYear()
        {
            List<CMFinancialYear> _FinancialYear = new List<CMFinancialYear>();
            _FinancialYear.Add(new CMFinancialYear { financial_year = "2020-21" });
            _FinancialYear.Add(new CMFinancialYear { financial_year = "2021-22" });
            _FinancialYear.Add(new CMFinancialYear { financial_year = "2022-23" });
            return _FinancialYear;

        }

        internal async Task<List<CMFacility>> GetFacilityList()
        {
            string myQuery = "SELECT id, name, address, city, state, country, zipcode as pin FROM Facilities WHERE isBlock = 0";
            List<CMFacility> _Facility = await Context.GetData<CMFacility>(myQuery).ConfigureAwait(false);
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

        internal async Task<List<CMDefaultList>> GetSPVList()
        {
            string myQuery = "SELECT id, name FROM spv";
            List<CMDefaultList> _SPV = await Context.GetData<CMDefaultList>(myQuery).ConfigureAwait(false);
            return _SPV;
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
            string myQuery = $"SELECT id, name, description, status FROM businesstype";
            List<CMBusinessType> _Business = await Context.GetData<CMBusinessType>(myQuery).ConfigureAwait(false);
            return _Business;
        }
        internal async Task<CMDefaultResponse> AddBusiness(List<CMBusiness> request)
        {
            int count = 0;
            int retID = 0;
            string businessName = "";
            CMMS.RETRUNSTATUS retCode = CMMS.RETRUNSTATUS.INVALID_ARG;
            string strRetMessage = "";
            int useId = 11;     //pending : get from session
            string qry = "insert into business ( name, email, contactPerson, contactNumber, website, location, address, city, state, country, zip, type, status, addedBy) values ";
            foreach (var unit in request)
            {
                count++;
                businessName = unit.name;
                qry += "('" + unit.name + "','" + unit.email + "','" + unit.contactPerson + "','" + unit.contactNumber+ "','" + unit.website + "','" + unit.location + "','" + unit.address + "','" + unit.city + "','" + unit.state + "','" + unit.country + "','" + unit.zip + "','" + unit.type + "','" + unit.status + "','" + useId + "'),";
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
        internal async Task<List<CMBusiness>> GetBusinessList(int businessType)
        {
            string myQuery = $"SELECT id, name, email, contactPerson, contactNumber, website, location, address, city, state, country, zip, type, status, addedAt FROM Business ";
            if(businessType > 0)
                myQuery += $"where type = {businessType}";
            List<CMBusiness> _Business = await Context.GetData<CMBusiness>(myQuery).ConfigureAwait(false);
            return _Business;
        }

        internal async Task<List<CMFrequency>> GetFrequencyList()
        {
            string myQuery = $"SELECT id,name, days FROM Frequency where status = 1";
            List<CMFrequency> _FrequencyList = await Context.GetData<CMFrequency>(myQuery).ConfigureAwait(false);
            return _FrequencyList;
        }
    }

}
