using CMMSAPIs.Helper;
using CMMSAPIs.Models.PM;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.PM;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.PM
{
    public interface IPMBS
    {
        Task<CMDefaultResponse> CreatePMPlan(CMPMPlanDetail pm_plan, int userID, string facilitytime);
        Task<List<CMPMPlanList>> GetPMPlanList(int facility_id, string category_id, string frequency_id, DateTime? start_date, DateTime? end_date, string facilitytime);
        Task<CMPMPlanDetail> GetPMPlanDetail(int id, string facilitytime);
        Task<List<CMScheduleData>> GetScheduleData(int facility_id, int category_id, string facilitytime);
        Task<List<CMDefaultResponse>> SetScheduleData(CMSetScheduleData request, int userID);
        Task<CMDefaultResponse> ApprovePMPlan(CMApproval request, int userID, string facilitytime);
        Task<CMDefaultResponse> RejectPMPlan(CMApproval request, int userID, string facilitytime);
        Task<CMDefaultResponse> DeletePMPlan(CMApproval request, int userID, string facilitytime);
        Task<CMDefaultResponse> UpdatePMPlan(CMPMPlanDetail request, int userID, string facilitytime);

        Task<CMImportFileResponse> ImportPMPlanFile(int file_id, int facility_id, int userID, string facilitytime);
        Task<CMDefaultResponse> DeletePMTask(CMApproval request, int userID, string facilitytime);
    }
    public class PMBS : IPMBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public static IWebHostEnvironment _environment;

        public PMBS(DatabaseProvider dbProvider, IWebHostEnvironment environment)
        {
            databaseProvider = dbProvider;
            _environment = environment;
        }

        public async Task<CMDefaultResponse> CreatePMPlan(CMPMPlanDetail pm_plan, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.CreatePMPlan(pm_plan, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMPMPlanList>> GetPMPlanList(int facility_id, string category_id, string frequency_id, DateTime? start_date, DateTime? end_date, string facilitytime)
        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.GetPMPlanList(facility_id, category_id, frequency_id, start_date, end_date, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMPMPlanDetail> GetPMPlanDetail(int id, string facilitytime)
        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.GetPMPlanDetail(id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMScheduleData>> GetScheduleData(int facility_id, int category_id, string facilitytime)
        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.GetScheduleData(facility_id, category_id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMDefaultResponse>> SetScheduleData(CMSetScheduleData request, int userID)
        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.SetScheduleData(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> ApprovePMPlan(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.ApprovePMPlan(request, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> RejectPMPlan(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.RejectPMPlan(request, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeletePMPlan(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.DeletePMPlan(request , userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> UpdatePMPlan(CMPMPlanDetail request, int userID, string facilitytime)

        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.UpdatePMPlan(request, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMImportFileResponse> ImportPMPlanFile(int file_id, int facility_id, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.ImportPMPlanFile(file_id, facility_id, userID, facilitytime);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeletePMTask(CMApproval request, int userID, string facilitytime)
        {
            try
            {
                using (var repos = new PMRepository(getDB, _environment))
                {
                    return await repos.DeletePMTask(request, userID, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
