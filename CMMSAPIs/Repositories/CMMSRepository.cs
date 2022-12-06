using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Helper;
using CMMSAPIs.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMSAPIs.Repositories
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

        internal async Task<List<FinancialYear>> GetFinancialYear()
        {
            List<FinancialYear> _FinancialYear = new List<FinancialYear>();
            _FinancialYear.Add(new FinancialYear { financial_year = "2020-21" });
            _FinancialYear.Add(new FinancialYear { financial_year = "2021-22"});
            _FinancialYear.Add(new FinancialYear { financial_year = "2022-23" });
            return _FinancialYear;

        }

        internal async Task<List<Facility>> GetFacilityList()
        {
            string myQuery = "SELECT id, name, address, city, state, country, zipcode as pin FROM Facilities WHERE isBlock = 0";
            List<Facility> _Facility = await Context.GetData<Facility>(myQuery).ConfigureAwait(false);
            return _Facility;
        }

        internal async Task<List<Facility>> GetFacility(int facility_id)
        {
            string myQuery = "SELECT id, name, address, city, state, country, zipcode as pin FROM Facilities WHERE id= " + facility_id;
            List<Facility> _Facility = await Context.GetData<Facility>(myQuery).ConfigureAwait(false);
            return _Facility;
        }

        internal async Task<List<Facility>> GetBlockList(int facility_id)
        {
            string myQuery = "SELECT id, name, address, city, state, country, zipcode as pin FROM Facilities WHERE isBlock = 1 AND status = 1 AND parentId = " + facility_id;
            List<Facility> _Facility = await Context.GetData<Facility>(myQuery).ConfigureAwait(false);
            return _Facility;
        }
        internal async Task<List<AssetCategory>> GetAssetCategoryList()
        {
            string myQuery = "SELECT id, name FROM AssetCategories where status = 1";
            List<AssetCategory> _AssetCategory = await Context.GetData<AssetCategory>(myQuery).ConfigureAwait(false);
            return _AssetCategory;
        }
        internal async Task<List<Asset>> GetAssetList(int facility_id)
        {
            string myQuery = "SELECT id,name FROM Assets where status = 1 AND facilityId = " + facility_id;
            List<Asset> _Asset = await Context.GetData<Asset>(myQuery).ConfigureAwait(false);
            return _Asset;
        }

        internal async Task<List<Employee>> GetEmployeeList(int facility_id)
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
            List<Employee> _Employee = await Context.GetData<Employee>(myQuery).ConfigureAwait(false);
            return _Employee;
        }
    }

}
