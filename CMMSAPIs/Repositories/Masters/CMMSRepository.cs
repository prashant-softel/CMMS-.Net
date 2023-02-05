using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMSAPIs.Repositories.Masters
{
    public class CMMSRepository : GenericRepository
    {

        public const string MA_Actual = "MA_Actual";
        public const string MA_Contractual = "MA_Contractual";
        public const string Internal_Grid = "Internal_Grid";
        public const string External_Grid = "External_Grid";

        public CMMSRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {

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
            string myQuery = "SELECT id, name FROM AssetCategories where status = 1";
            List<CMAssetCategory> _AssetCategory = await Context.GetData<CMAssetCategory>(myQuery).ConfigureAwait(false);
            return _AssetCategory;
        }
        internal async Task<List<CMAsset>> GetAssetList(int facility_id)
        {
            string myQuery = "SELECT id,name FROM Assets where status = 1 AND facilityId = " + facility_id;
            List<CMAsset> _Asset = await Context.GetData<CMAsset>(myQuery).ConfigureAwait(false);
            return _Asset;
        }

        internal async Task<List<CMEmployee>> GetEmployeeList(int facility_id)
        {
            string myQuery = "SELECT " +
                                    "u.id, loginId, concat(firstName,' ', lastName) as name, birthday as birthdate, " +
                                    "gender, mobileNumber, city, state, country, zipcode as pin " +
                             "FROM " +
                                    "Users as u " +
                             "JOIN " +
                                    "UserFacilities as uf ON u.id = uf.userId " +
                             "WHERE " +
                                    "u.status = 1 AND uf.status = 1 AND isEmployee = 1 AND uf.facilityId = " + facility_id;
            List<CMEmployee> _Employee = await Context.GetData<CMEmployee>(myQuery).ConfigureAwait(false);
            return _Employee;
        }

        internal async Task<List<CMBusinessType>> GetBusinessTypeList()
        {
            string myQuery = $"SELECT id, name, description, status FROM businesstype";
            List<CMBusinessType> _Business = await Context.GetData<CMBusinessType>(myQuery).ConfigureAwait(false);
            return _Business;
        }
        internal async Task<List<CMBusiness>> GetBusinessList(CMMS.CMMS_BusinessType businessType)
        {
            string myQuery = $"SELECT id, name, email, contactPerson, contactNumber, address, city, state, country, zip, type, status, addedAt FROM Business where type = " + (int) businessType;
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
