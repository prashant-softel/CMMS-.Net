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
        Task<List<CMFinancialYear>> GetFinancialYear();
        Task<List<CMFacility>> GetFacility(int facility_id);
        Task<List<CMFacility>> GetFacilityList();
        Task<List<CMFacility>> GetBlockList(int facility_id);
        Task<List<CMAssetCategory>> GetAssetCategoryList();
        Task<List<CMAsset>> GetAssetList(int facility_id);
        Task<List<CMEmployee>> GetEmployeeList(int facility_id);
        Task<List<CMSupplier>> GetSupplierList();
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

        public async Task<List<CMFinancialYear>> GetFinancialYear()
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

       public async Task<List<CMFacility>> GetFacility(int facility_id)
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

        public async Task<List<CMFacility>> GetFacilityList()
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

        public async Task<List<CMFacility>> GetBlockList(int facility_id)
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

        public async Task<List<CMAssetCategory>> GetAssetCategoryList()
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

        public async Task<List<CMAsset>> GetAssetList(int facility_id)
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

        public async Task<List<CMEmployee>> GetEmployeeList(int facility_id)
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

        public async Task<List<CMSupplier>> GetSupplierList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetSupplierList();

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
