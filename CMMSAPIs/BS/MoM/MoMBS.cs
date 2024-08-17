using CMMSAPIs.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Models;
using CMMSAPIs.Models.MoM;
using CMMSAPIs.Repositories.MoM;

namespace CMMSAPIs.BS.MoM
{
    public interface IMoMBS
    {
        Task<List<CMMoM>> GetMoMList(int facility_id, string facilitytime);
        Task<CMMoM> GetMoMDetails(int facility_id, int mom_id, string facilitytime);
        Task<CMDefaultResponse> CreateMoM(CMMoM request, int userId);
        Task<CMDefaultResponse> UpdateMoM(CMMoM request, int userId);
        Task<CMDefaultResponse> DeleteMoM(int momId);
        Task<CMDefaultResponse> CloseMoM(int momId);
        Task<List<CMMoMAssignTo>> GetMoMAssignToDetails(int mom_id);
        Task<List<CMMoMTargetDate>> GetMoMTargetDateDetails(int mom_id);
    }
    public class MoMBS : IMoMBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public MoMBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

         
        public async Task<List<CMMoM>> GetMoMList(int facility_id, string facilitytime)
        {
            try
            {
                using (var repos = new MoMRepository(getDB))
                {
                    return await repos.GetMoMList(facility_id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMMoM> GetMoMDetails(int facility_id, int mom_id, string facilitytime)
        {
            try
            {
                using (var repos = new MoMRepository(getDB))
                {
                    return await repos.GetMoMDetails(facility_id, mom_id, facilitytime);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateMoM(CMMoM request, int userId)
        {
            try
            {
                using (var repos = new MoMRepository(getDB))
                {
                    return await repos.CreateMoM(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateMoM(CMMoM request, int userId)
        {
            try
            {
                using (var repos = new MoMRepository(getDB))
                {
                    return await repos.UpdateMoM(request, userId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteMoM(int momId)
        {
            try
            {
                using (var repos = new MoMRepository(getDB))
                {
                    return await repos.DeleteMoM(momId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CloseMoM(int momId)
        {
            try
            {
                using (var repos = new MoMRepository(getDB))
                {
                    return await repos.CloseMoM(momId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMoMAssignTo>> GetMoMAssignToDetails(int mom_id)
        {
            try
            {
                using (var repos = new MoMRepository(getDB))
                {
                    return await repos.GetMoMAssignToDetails(mom_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMMoMTargetDate>> GetMoMTargetDateDetails(int mom_id)
        {
            try
            {
                using (var repos = new MoMRepository(getDB))
                {
                    return await repos.GetMoMTargetDateDetails(mom_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
