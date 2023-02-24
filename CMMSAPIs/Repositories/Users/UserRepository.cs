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

        internal async Task<CMUserAccess> GetUserAccess(int user_id)
        {
            string qry = $"SELECT " +
                            $"`featureId` as feature_id, `add`, `edit`, `delete`, `view`, `issue`, `approve`, `selfView` " +
                         $"FROM " +
                            $"`UsersAccess` " +
                         $"WHERE " +
                            $"userId = {user_id}";

            List<CMAccessList> access_list = await Context.GetData<CMAccessList>(qry).ConfigureAwait(false);
            CMUserAccess user_access = new CMUserAccess();
            user_access.user_id = user_id;
            user_access.access_list = access_list;
            return user_access;
        }

        internal async Task<CMDefaultResponse> SetUserAccess(CMUserAccess request)
        {
            try
            {
                // Get previous settings
                int user_id = request.user_id;
                CMUserAccess old_user_access = await GetUserAccess(user_id);

                if (old_user_access.access_list != request.access_list)
                {
                    // Delete the previous setting
                    string delete_qry = $" DELETE FROM UsersAccess WHERE UserId = {user_id}";
                    await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                    // Insert the new setting
                    List<string> user_access = new List<string>();

                    foreach (var access in request.access_list)
                    {
                        user_access.Add($"({user_id}, {access.feature_id}, {access.add}, {access.edit}, " +
                                        $"{access.view},{access.delete}, {access.issue}, {access.approve}, {access.selfView}, " +
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
                        _log.module_type = CMMS.CMMS_Modules.USER;
                        _log.module_ref_id = user_id;
                        _log.comment = JsonSerializer.Serialize(old_user_access.access_list);
                        _log.status = CMMS.CMMS_Status.UPDATED;
                        await repos.AddLog(_log);
                    }
                }
                CMDefaultResponse response = new CMDefaultResponse(user_id, CMMS.RETRUNSTATUS.SUCCESS, "Updated User Access Successfully");
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal async Task<CMUserNotifications> GetUserNotifications(int user_id)
        {
            string qry = $"SELECT " +
                            $"`notificationId` as notification_id, `canChange` as can_change, userPreference as flag " +
                         $"FROM " +
                            $"`UserNotifications` " +
                         $"WHERE " +
                            $"userId = {user_id}";

            List<CMNotificationList> notification_list = await Context.GetData<CMNotificationList>(qry).ConfigureAwait(false);
            CMUserNotifications user_notification = new CMUserNotifications();
            user_notification.user_id = user_id;
            user_notification.notification_list = notification_list;
            return user_notification;
        }

        internal async Task<CMDefaultResponse> SetUserNotifications(CMUserNotifications request)
        {
            try
            {
                // Get previous settings
                int user_id = request.user_id;
                CMUserNotifications user_old_notification = await GetUserNotifications(user_id);

                if (user_old_notification.notification_list != request.notification_list)
                {
                    // Delete the previous setting
                    string delete_qry = $" DELETE FROM UserNotifications WHERE UserId = {user_id}";
                    await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                    // Insert the new setting
                    List<string> user_access = new List<string>();

                    foreach (var access in request.notification_list)
                    {
                        user_access.Add($"({user_id}, {access.notification_id}, {access.can_change}, {access.flag}, " +
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
                        _log.module_type = CMMS.CMMS_Modules.USER_NOTIFICATIONS;
                        _log.module_ref_id = user_id;
                        _log.comment = JsonSerializer.Serialize(user_old_notification.notification_list);
                        _log.status = CMMS.CMMS_Status.UPDATED;
                        await repos.AddLog(_log);
                    }
                    CMDefaultResponse response = new CMDefaultResponse(user_id, CMMS.RETRUNSTATUS.SUCCESS, "Updated User Notifications Successfully");
                    return response;
                }
                else 
                {
                    CMDefaultResponse response = new CMDefaultResponse(user_id, CMMS.RETRUNSTATUS.SUCCESS, "User Notifications Failed to Update");
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
