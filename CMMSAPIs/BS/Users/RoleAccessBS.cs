using CMMSAPIs.Helper;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Users
{
    public interface IRoleAccessBS
    {
        public Task<List<CMAccess>> GetRoleAccess(int role_id);
        public Task<CMDefaultResponse> SetRoleAccess(CMRoleAccess request);
        public Task<List<CMRoleNotifications>> GetRoleNotifications(int role_id);
        public Task<CMDefaultResponse> SetRoleNotifications(List<CMRoleNotifications> request);
        //public Task<int> SetRoleAccessAll(CMRoleAccess request);
    }

    public class RoleAccessBS : IRoleAccessBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public RoleAccessBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<CMAccess>> GetRoleAccess(int role_id)
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.GetRoleAccess(role_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> SetRoleAccess(CMRoleAccess request)
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.SetRoleAccess(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMRoleNotifications>> GetRoleNotifications(int role_id)
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.GetRoleNotifications(role_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> SetRoleNotifications(List<CMRoleNotifications> request)
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.SetRoleNotifications(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //public async Task<int> SetRoleAccessAll(CMRoleAccess request)
        //{
        //    try
        //    {
        //        using (var repos = new RoleAccessRepository(getDB))
        //        {
        //            return await repos.CompareAndSetUserAccess(request.role_id, request.access_list);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
