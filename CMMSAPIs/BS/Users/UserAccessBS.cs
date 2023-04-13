﻿using CMMSAPIs.Helper;
using CMMSAPIs.Models.Authentication;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Users;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMSAPIs.BS.Users
{
    public interface IUserAccessBS
    {
        Task<UserToken> Authenticate(CMUserCrentials userCrentials);
        public Task<CMUserDetail> GetUserDetail(int user_id);
        public Task<List<CMUser>> GetUserList(int facility_id);
        public Task<CMDefaultResponse> CreateUser(CMUserDetail request);
        public Task<CMDefaultResponse> UpdateUser(CMUserDetail request);
        public Task<CMDefaultResponse> DeleteUser(int user_id);
        public Task<List<CMUser>> GetUserByNotificationId(CMUserByNotificationId request);
        public Task<CMUserAccess> GetUserAccess(int user_id);
        public Task<CMDefaultResponse> SetUserAccess(CMUserAccess request);
        public Task<CMUserNotifications> GetUserNotifications(int user_id);
        public Task<CMDefaultResponse> SetUserNotifications(CMUserNotifications request);
    }

    public class UserAccessBS : IUserAccessBS
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public UserAccessBS(DatabaseProvider dbProvider, IConfiguration configuration)
        {
            _configuration   = configuration;
            databaseProvider = dbProvider;
        }

        public async Task<UserToken> Authenticate(CMUserCrentials userCrentials)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB, _configuration))
                {
                    return await repos.Authenticate(userCrentials);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMUserDetail> GetUserDetail(int user_id)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.GetUserDetail(user_id);
                }
            }   
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CMUser>> GetUserList(int facility_id)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.GetUserList(facility_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateUser(CMUserDetail request)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.CreateUser(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateUser(CMUserDetail request)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.UpdateUser(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteUser(int user_id)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.DeleteUser(user_id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<List<CMUser>> GetUserByNotificationId(CMUserByNotificationId request)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.GetUserByNotificationId(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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
