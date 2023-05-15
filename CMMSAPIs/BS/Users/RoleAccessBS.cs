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
        public Task<List<KeyValuePairs>> GetRoleList();
        public Task<CMRoleAccess> GetRoleAccess(int role_id);
        public Task<CMDefaultResponse> SetRoleAccess(CMSetRoleAccess request, int userID);
        public Task<CMRoleNotifications> GetRoleNotifications(int role_id);
        public Task<CMDefaultResponse> SetRoleNotifications(CMSetRoleNotifications request, int userID);      
    }

    public class RoleAccessBS : IRoleAccessBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public RoleAccessBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<List<KeyValuePairs>> GetRoleList()
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.GetRoleList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMRoleAccess> GetRoleAccess(int role_id)
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

        public async Task<CMDefaultResponse> SetRoleAccess(CMSetRoleAccess request, int userID)
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.SetRoleAccess(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMRoleNotifications> GetRoleNotifications(int role_id)
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

        public async Task<CMDefaultResponse> SetRoleNotifications(CMSetRoleNotifications request, int userID)
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.SetRoleNotifications(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
