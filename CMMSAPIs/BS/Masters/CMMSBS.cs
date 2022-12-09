using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.Masters;
using CMMSAPIs.Models.Masters;

namespace CMMSAPIs.BS.Masters
{
    public interface ICMMSBS
    {
        //sTask<int> eQry(string qry); 
        Task<List<FinancialYear>> GetFinancialYear();
        Task<List<Facility>> GetFacility(int facility_id);
        Task<List<Facility>> GetFacilityList();
        Task<List<Facility>> GetBlockList(int facility_id);
        Task<List<AssetCategory>> GetAssetCategoryList();
        Task<List<Asset>> GetAssetList(int facility_id);
        Task<List<Employee>> GetEmployeeList(int facility_id);
    }
    public class CMMSBS : ICMMSBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public CMMSBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }


        #region helper

        public async Task<List<FinancialYear>> GetFinancialYear()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetFinancialYear();

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<Facility>> GetFacility(int facility_id)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetFacility(facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Facility>> GetFacilityList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetFacilityList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Facility>> GetBlockList(int facility_id)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetBlockList(facility_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<AssetCategory>> GetAssetCategoryList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetAssetCategoryList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Asset>> GetAssetList(int facility_id)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetAssetList(facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Employee>> GetEmployeeList(int facility_id)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetEmployeeList(facility_id);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion //helper functions


        /*
        public async Task<int> eQry(string qry)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.eQry(qry);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        */


    }
}
