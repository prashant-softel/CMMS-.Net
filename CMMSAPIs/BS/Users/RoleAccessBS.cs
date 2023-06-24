using CMMSAPIs.Helper;
using CMMSAPIs.Models.Inventory;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Inventory;
using CMMSAPIs.Repositories.Users;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Users
{
    public interface IRoleAccessBS
    {
        public Task<List<KeyValuePairs>> GetRoleList();
        public Task<List<CMDesignation>> GetDesignationList();
        public Task<CMDefaultResponse> AddDesignation(CMDesignation request, int userId);
        public Task<CMDefaultResponse> UpdateDesignation(CMDesignation request, int userId);
        public Task<CMDefaultResponse> DeleteDesignation(int id);

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


        public async Task<List<CMDesignation>> GetDesignationList()
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.GetDesignationList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> AddDesignation(CMDesignation request, int userID)
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.AddDesignation(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateDesignation(CMDesignation request, int userID)
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.UpdateDesignation(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteDesignation(int id)
        {
            try
            {
                using (var repos = new RoleAccessRepository(getDB))
                {
                    return await repos.DeleteDesignation(id);
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
