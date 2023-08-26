using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Repositories.Masters;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Utils;

namespace CMMSAPIs.BS.Masters
{
    public interface ICMMSBS
    {
        //sTask<int> eQry(string qry); 
        Task<List<CMFinancialYear>> GetFinancialYear();
        Task<List<CMFacility>> GetFacility(int facility_id);
        Task<List<CMFacilityList>> GetFacilityList();
        Task<List<CMFacility>> GetBlockList(int facility_id);
        Task<List<CMAssetCategory>> GetAssetCategoryList();
        Task<List<CMAsset>> GetAssetList(int facility_id);
        Task<List<CMEmployee>> GetEmployeeList(int facility_id, CMMS.CMMS_Modules module, CMMS.CMMS_Access access);
        Task<List<CMBusinessType>> GetBusinessTypeList();
        Task<CMDefaultResponse> AddBusinessType(CMBusinessType request, int userId);
        Task<CMDefaultResponse> UpdateBusinessType(CMBusinessType request, int userId);
        Task<CMDefaultResponse> DeleteBusinessType(int id, int userId);
        Task<CMDefaultResponse> AddBusiness(List<CMBusiness> request, int userId);
        Task<CMDefaultResponse> UpdateBusiness(CMBusiness request, int userId);
        Task<CMDefaultResponse> DeleteBusiness(int id, int userId);
        Task<List<CMDefaultList>> GetBloodGroupList();
        Task<List<CMDefaultList>> GetGenderList();
        Task<List<CMIRRiskType>> GetRiskTypeList();
        Task<CMDefaultResponse> CreateRiskType(CMIRRiskType request, int userId);
        Task<CMDefaultResponse> UpdateRiskType(CMIRRiskType request, int userId);
        Task<CMDefaultResponse> DeleteRiskType(int id, int userId);
        Task<List<CMIRInsuranceProvider>> GetInsuranceProviderList();
        Task<List<CMIRStatus>> GetInsuranceStatusList();
        Task<List<CMSPV>> GetSPVList();
        Task<CMDefaultResponse> CreateSPV(CMSPV request, int userId);
        Task<CMDefaultResponse> UpdateSPV(CMSPV request, int userId);
        Task<CMDefaultResponse> DeleteSPV(int id, int userId);
        Task<List<CMBusiness>> GetBusinessList(int businessType);
        Task<CMDefaultResponse> AddModule(CMModule request);
        Task<CMDefaultResponse> UpdateModule(CMModule request);
        Task<CMDefaultResponse> DeleteModule(int id);
        Task<CMModule> GetModuleDetail(int id);
        Task<List<CMModule>> GetModuleList();
        Task<List<CMStatus>> GetStatusList(CMMS.CMMS_Modules module);
        Task<List<CMFrequency>> GetFrequencyList();
        Task<string> Print(int id, CMMS.CMMS_Modules moduleID);

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

        public async Task<CMDefaultResponse> AddModule(CMModule request)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.AddModule(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateModule(CMModule request)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.UpdateModule(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteModule(int id)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.DeleteModule(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMModule> GetModuleDetail(int id)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetModuleDetail(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMModule>> GetModuleList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetModuleList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMStatus>> GetStatusList(CMMS.CMMS_Modules module)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetStatusList(module);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

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

        public async Task<List<CMFacilityList>> GetFacilityList()
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

        public async Task<List<CMEmployee>> GetEmployeeList(int facility_id, CMMS.CMMS_Modules module, CMMS.CMMS_Access access)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetEmployeeList(facility_id, module, access);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMBusinessType>> GetBusinessTypeList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetBusinessTypeList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultList>> GetBloodGroupList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetBloodGroupList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultList>> GetGenderList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetGenderList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMIRStatus>> GetInsuranceStatusList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetInsuranceStatusList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMIRRiskType>> GetRiskTypeList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetRiskTypeList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMIRInsuranceProvider>> GetInsuranceProviderList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetInsuranceProviderList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateRiskType(CMIRRiskType request, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.CreateRiskType(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateRiskType(CMIRRiskType request, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.UpdateRiskType(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteRiskType(int id, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.DeleteRiskType(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<CMSPV>> GetSPVList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetSPVList();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateSPV(CMSPV request, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.CreateSPV(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateSPV(CMSPV request, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.UpdateSPV(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteSPV(int id, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.DeleteSPV(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMBusiness>> GetBusinessList(int businessType)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetBusinessList(businessType);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AddBusinessType(CMBusinessType request, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.AddBusinessType(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateBusinessType(CMBusinessType request, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.UpdateBusinessType(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteBusinessType(int id, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.DeleteBusinessType(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AddBusiness(List<CMBusiness> request, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.AddBusiness(request, userId);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateBusiness(CMBusiness request, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.UpdateBusiness(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteBusiness(int id, int userId)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.DeleteBusiness(id, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMFrequency>> GetFrequencyList()
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.GetFrequencyList();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<string> Print(int id, CMMS.CMMS_Modules moduleID)
        {
            try
            {
                using (var repos = new CMMSRepository(getDB))
                {
                    return await repos.Print(id, moduleID);
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
