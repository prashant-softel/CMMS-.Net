using CMMSAPIs.Helper;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Users
{
    public interface IUserAccessBS
    {
        public Task<CMUserAccess> GetUserAccess(int user_id);
        public Task<CMDefaultResponse> SetUserAccess(CMUserAccess request);
        public Task<CMUserNotifications> GetUserNotifications(int user_id);
        public Task<CMDefaultResponse> SetUserNotifications(CMUserNotifications request);
    }

    public class UserAccessBS : IUserAccessBS
    {
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public UserAccessBS(DatabaseProvider dbProvider)
        {
            databaseProvider = dbProvider;
        }

        public async Task<CMUserAccess> GetUserAccess(int role_id)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.GetUserAccess(role_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> SetUserAccess(CMUserAccess request)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.SetUserAccess(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMUserNotifications> GetUserNotifications(int role_id)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.GetUserNotifications(role_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> SetUserNotifications(CMUserNotifications request)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.SetUserNotifications(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
