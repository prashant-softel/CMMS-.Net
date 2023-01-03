using CMMSAPIs.Helper;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;


namespace CMMSAPIs.Repositories.Users
{
    public class UserAccessRepository : GenericRepository
    {
        private MYSQLDBHelper _conn;
        public UserAccessRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _conn = sqlDBHelper;
        }

        internal async Task<List<CMUserAccess>> GetUserAccess(int user_id)
        {
            string qry = $"SELECT " +
                            $"`userId` as user_id, `featureId` as feature_id, `add`, `edit`, `delete`, `view`, `issue`, `approve`, `selfView` " +
                         $"FROM " +
                            $"`UsersAccess` " +
                         $"WHERE " +
                            $"userId = {user_id}";

            List<CMUserAccess> access_list = await Context.GetData<CMUserAccess>(qry).ConfigureAwait(false);
            return access_list;
        }

        internal async Task<CMDefaultResponse> SetUserAccess(List<CMUserAccess> request)
        {
            try
            {
                // Get previous settings
                int user_id = request[0].user_id;
                List<CMUserAccess> old_access_list = await GetUserAccess(user_id);

                if (old_access_list != request)
                {
                    // Delete the previous setting
                    string delete_qry = $" DELETE FROM UsersAccess WHERE UserId = {user_id}";
                    await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                    // Insert the new setting
                    List<string> user_access = new List<string>();

                    foreach (var access in request)
                    {
                        user_access.Add($"({user_id}, {access.feature_id}, {access.add}, {access.edit}, " +
                                        $"{access.delete}, {access.view}, {access.issue}, {access.approve}, {access.selfView}, " +
                                        $"'{UtilsRepository.GetUTCTime()}', {UtilsRepository.GetUserID()})");
                    }
                    string user_access_insert_str = string.Join(',', user_access);

                    string insert_query = $"INSERT INTO UsersAccess" +
                                                $"(userId, featureId, `add`, `edit`, `view`, `delete`, `issue`, `approve`, `selfView`, `lastModifiedAt`, `lastModifiedBy`) " +
                                          $" VALUES {user_access_insert_str}";
                    await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);

                    using (var repos = new UtilsRepository(_conn))
                    {
                        // Add previous setting to log table
                        CMLog _log = new CMLog();
                        _log.module_type = Constant.USER;
                        _log.module_ref_id = user_id;
                        _log.comment = JsonSerializer.Serialize(old_access_list);
                        _log.status = Constant.UPDATED;
                        await repos.AddLog(_log);
                    }
                }
                CMDefaultResponse response = new CMDefaultResponse(user_id, 200, "Updated User Access Successfully");
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal async Task<List<CMUserNotifications>> GetUserNotifications(int user_id)
        {
            string qry = $"SELECT " +
                            $"`userId` as user_id, `notificationId` as notification_id, `canChange` as can_change, userPreference as user_preference " +
                         $"FROM " +
                            $"`UserNotifications` " +
                         $"WHERE " +
                            $"userId = {user_id}";

            List<CMUserNotifications> access_list = await Context.GetData<CMUserNotifications>(qry).ConfigureAwait(false);
            return access_list;
        }

        internal async Task<CMDefaultResponse> SetUserNotifications(List<CMUserNotifications> request)
        {
            try
            {
                // Get previous settings
                int user_id = request[0].user_id;
                List<CMUserNotifications> old_access_list = await GetUserNotifications(user_id);

                if (old_access_list != request)
                {
                    // Delete the previous setting
                    string delete_qry = $" DELETE FROM UserNotifications WHERE UserId = {user_id}";
                    await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                    // Insert the new setting
                    List<string> user_access = new List<string>();

                    foreach (var access in request)
                    {
                        user_access.Add($"({user_id}, {access.notification_id}, {access.can_change}, {access.user_preference}, " +
                                        $"'{UtilsRepository.GetUTCTime()}', {UtilsRepository.GetUserID()})");
                    }
                    string user_access_insert_str = string.Join(',', user_access);

                    string insert_query = $"INSERT INTO UserNotifications" +
                                                $"(userId, notificationId, `canChange`, `userPreference`, `lastModifiedAt`, `lastModifiedBy`) " +
                                          $" VALUES {user_access_insert_str}";
                    await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);

                    using (var repos = new UtilsRepository(_conn))
                    {
                        // Add previous setting to log table
                        CMLog _log = new CMLog();
                        _log.module_type = Constant.USER_NOTIFICATIONS;
                        _log.module_ref_id = user_id;
                        _log.comment = JsonSerializer.Serialize(old_access_list);
                        _log.status = Constant.UPDATED;
                        await repos.AddLog(_log);
                    }
                    CMDefaultResponse response = new CMDefaultResponse(user_id, 200, "Updated User Notifications Successfully");
                    return response;
                }
                else 
                {
                    CMDefaultResponse response = new CMDefaultResponse(user_id, 200, "User Notifications Failed to Update");
                    return response;
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
