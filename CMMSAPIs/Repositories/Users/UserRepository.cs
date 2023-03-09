using CMMSAPIs.Helper;
using CMMSAPIs.Models.Authentication;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace CMMSAPIs.Repositories.Users
{
    public class UserAccessRepository : GenericRepository
    {
        private MYSQLDBHelper _conn;
        private readonly IConfiguration _configuration;
        public UserAccessRepository(MYSQLDBHelper sqlDBHelper, IConfiguration configuration = null) : base(sqlDBHelper)
        {
            _configuration = configuration;
            _conn = sqlDBHelper;
        }

        internal async Task<UserToken> Authenticate(CMUserCrentials userCrentials)
        {
            string myQuery = "SELECT id FROM Users WHERE loginId = '" + userCrentials.user_name + "' AND password = '" + userCrentials.password + "'";
            List<CMUser> _List = await Context.GetData<CMUser>(myQuery).ConfigureAwait(false);
            if (_List.Count == 0)
            {
                return null;
            }
            var user_id = _List[0].id;
            var key = _configuration.GetValue<string>("JwtConfig:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, ""+user_id)
                    }),
                Expires = DateTime.UtcNow.AddMinutes(CMMS.TOKEN_EXPIRATION_TIME),
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            CMUserDetail userDetail = await GetUserDetail(user_id);
            userDetail.user_access = await GetUserAccess(user_id);
            userDetail.user_notification = await GetUserNotifications(user_id);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserToken user_token = new UserToken(tokenHandler.WriteToken(token), userDetail);
            return user_token;
        }

        internal async Task<CMUserDetail> GetUserDetail(int user_id)
        {
            // Pending - Include all the property listed in CMUserDetail Model
            string qry = $"SELECT " +
                            $"u.id, firstName as first_name, lastName as last_name,  CONCAT(firstName, ' ', lastName) as full_name, loginId as user_name, mobileNumber as contact_no " +
                         $"FROM " +
                            $"Users as u " +
                         $"JOIN " +
                            $"UserRoles as r ON u.roleId = r.id " +
                         $" WHERE " +
                            $"u.id = {user_id}";

            List<CMUserDetail> user_detail = await Context.GetData<CMUserDetail>(qry).ConfigureAwait(false);
            return user_detail[0];
        }
    
        internal async Task<CMDefaultResponse> CreateUser(CMUserDetail request)
        {
            /*
             * Read the required field for insert
             * Table - Users, UserNofication, UserAccess
             * Use existing functions to insert UserNotification and User Access in table
            */
            return null;
        }
        internal async Task<CMDefaultResponse> UpdateUser(CMUserDetail request)
        {
            /*
             * Read the required field for update
             * Table - Users, UserNofication, UserAccess
             * Use existing functions to insert UserNotification and User Access in table
            */
            return null;
        }
        internal async Task<CMDefaultResponse> DeleteUser(int user_id)
        {
            /*
             * Table - Users, UserNotification, UserAccess
             * Delete from above tables and add log in history table
            */
            return null;
        }

        internal async Task<List<CMUser>> GetUserByNotificationId(CMUserByNotificationId request)
        {
            // Pending convert user_ids into string for where condition
            string user_ids_str = string.Join(",", request.user_ids.ToArray());
            string qry = $"SELECT " +
                            $"u.loginId as user_name, concat(firstName, ' ', lastName) as full_name " +
                         $"FROM " +
                            $"Users u " +
                         $"JOIN " +
                            $"UserNotifications un ON u.id = un.userId " +
                         $"JOIN " +
                            $"UserFacilities uf ON uf.userId = u.id " +
                         $"WHERE " +
                            $"uf.facilityId = {request.facility_id} AND userPreference = 1 AND notificationId = {(int)request.notification_id} " +
                            $" ";

            if (!user_ids_str.IsNullOrEmpty())
            {
                qry += $" AND (self = 0 OR u.id IN({user_ids_str}))";
            }
            else 
            {
                qry += $" AND self = 0";
            }
            

            List<CMUser> user_list = await Context.GetData<CMUser>(qry).ConfigureAwait(false);
            /*
             * Table - Users, UserNotification, Notification
             * Return user based on notification_id and facility_id 
            */
            return user_list;
        }


        internal async Task<List<CMUser>> GetUserList(int facility_id) 
        {
            string qry = $"SELECT " +
                            $"u.id, firstName as first_name, lastName as last_name, CONCAT(firstName, ' ', lastName) as full_name, loginId as user_name, mobileNumber as contact_no, r.name as role_name " +
                         $"FROM " +
                            $"Users as u " +
                         $"JOIN " +
                            $"UserRoles as r ON u.roleId = r.id " +
                         $"JOIN " +
                            $"UserFacilities as uf ON uf.userId = u.id " +
                         $"WHERE " +
                            $"uf.facilityId = {facility_id}";
            
            List<CMUser> user_list = await Context.GetData<CMUser>(qry).ConfigureAwait(false);
            return user_list;
        }

        internal async Task<CMUserAccess> GetUserAccess(int user_id)
        {           
            string qry = $"SELECT " +
                            $"featureId as feature_id, f.featureName as feature_name, u.add, u.edit, u.delete, u.view, u.issue, u.approve, u.selfView " +
                         $"FROM " +
                            $"`UsersAccess` as u " +
                            $"JOIN Features as f ON u.featureId = f.id " +
                         $"WHERE " +
                            $"userId = {user_id}";

            List<CMAccessList> access_list = await Context.GetData<CMAccessList>(qry).ConfigureAwait(false);
            CMUserDetail user_detail       = await GetUserDetail(user_id);
            CMUserAccess user_access       = new CMUserAccess();
            user_access.user_id            = user_id;
            user_access.user_name          = user_detail.full_name;
            user_access.access_list        = access_list;
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
                            $"`notificationId` as notification_id, notification as notification_name, `canChange` as can_change, userPreference as flag " +
                         $"FROM " +
                            $"`UserNotifications` un JOIN Notifications n ON un.notificationId = n.id " +
                         $"WHERE " +
                            $"userId = {user_id}";

            List<CMNotificationList> notification_list = await Context.GetData<CMNotificationList>(qry).ConfigureAwait(false);
            CMUserDetail user_detail              = await GetUserDetail(user_id);
            CMUserNotifications user_notification = new CMUserNotifications();
            user_notification.user_id             = user_id;
            user_notification.user_name           = user_detail.full_name;
            user_notification.notification_list   = notification_list;
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
