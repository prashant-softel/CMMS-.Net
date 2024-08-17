using CMMSAPIs.Helper;
using CMMSAPIs.Models.Authentication;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
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
        public Task<CMImportFileResponse> ImportUsers(int file_id, int userID);
        public Task<CMDefaultResponse> CreateUser(List<CMCreateUser> request, int userID);
        public Task<CMDefaultResponse> UpdateUser(CMUpdateUser request, int userID);
        public Task<CMDefaultResponse> DeleteUser(int id, int userID);
        public Task<List<CMUser>> GetUserByNotificationId(string user_ids_str, int notification_id, int facility_id);
        public Task<CMUserAccess> GetUserAccess(int user_id);
        public Task<CMDefaultResponse> SetUserAccess(CMUserAccess request, int userID);
        public Task<CMUserNotifications> GetUserNotifications(int user_id);
        public Task<CMDefaultResponse> SetUserNotifications(CMUserNotifications request, int userID);
        public Task<List<CMCompetency>> GetCompetencyList();
        public Task<CMDefaultResponse> AddCompetency(CMCompetency request, int userID);
        public Task<CMDefaultResponse> UpdateCompetency(CMCompetency request, int userID);
        public Task<CMDefaultResponse> DeleteCompetency(int id);


    }

    public class UserAccessBS : IUserAccessBS
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DatabaseProvider databaseProvider;
        private MYSQLDBHelper getDB => databaseProvider.SqlInstance();
        public UserAccessBS(DatabaseProvider dbProvider, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _configuration   = configuration;
            _webHostEnvironment = webHostEnvironment;
            databaseProvider = dbProvider;
        }

        public async Task<UserToken> Authenticate(CMUserCrentials userCrentials)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB, _webHostEnvironment, _configuration))
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

        public async Task<CMImportFileResponse> ImportUsers(int file_id, int userID)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB, _webHostEnvironment))
                {
                    return await repos.ImportUsers(file_id, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> CreateUser(List<CMCreateUser> request, int userID)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB, _webHostEnvironment))
                {
                    return await repos.CreateUser(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> UpdateUser(CMUpdateUser request, int userID)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.UpdateUser(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteUser(int id, int userID)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.DeleteUser(id, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<List<CMUser>> GetUserByNotificationId(string user_ids_str, int notification_id, int facility_id)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.GetUserByNotificationId(user_ids_str, notification_id, facility_id);
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

        public async Task<CMDefaultResponse> SetUserAccess(CMUserAccess request, int userID)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.SetUserAccess(request, userID);
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

        public async Task<CMDefaultResponse> SetUserNotifications(CMUserNotifications request, int userID)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.SetUserNotifications(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CMCompetency>> GetCompetencyList()
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.GetCompetencyList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CMDefaultResponse> AddCompetency(CMCompetency request, int userID)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB, _webHostEnvironment))
                {
                    return await repos.AddCompetency(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<CMDefaultResponse> UpdateCompetency(CMCompetency request, int userID)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB, _webHostEnvironment))
                {
                    return await repos.UpdateCompetency(request, userID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CMDefaultResponse> DeleteCompetency(int id)
        {
            try
            {
                using (var repos = new UserAccessRepository(getDB))
                {
                    return await repos.DeleteCompetency(id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
