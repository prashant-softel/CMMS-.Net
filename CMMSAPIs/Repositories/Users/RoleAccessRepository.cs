﻿using CMMSAPIs.BS.Users;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;


namespace CMMSAPIs.Repositories.Users
{
    public class RoleAccessRepository : GenericRepository
    {
        private MYSQLDBHelper _conn;
        //private UserAccessRepository _userAccessRepo;
        private UtilsRepository _utilsRepo;
        public RoleAccessRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _conn = sqlDBHelper;
            _utilsRepo = new UtilsRepository(_conn);
            //_userAccessRepo = new UserAccessRepository(_conn);
        }

        internal async Task<CMRoleAccess> GetRoleAccess(int role_id)
        {
            string qry = $"SELECT " +
                            $"featureId as feature_id, f.featureName as feature_name, r.add, r.edit, r.delete, r.view, r.issue, r.approve, r.selfView " +
                         $"FROM " +
                            $"`RoleAccess` r " +
                            $"JOIN Features as f ON r.featureId = f.id " +
                         $"WHERE " +
                            $"roleId = {role_id}";

            List<CMAccessList> access_list = await Context.GetData<CMAccessList>(qry).ConfigureAwait(false);

            List<KeyValuePairs> roleDetail = await GetRoleList(role_id);

            CMRoleAccess role_access = new CMRoleAccess();
            role_access.access_list = access_list;
            role_access.role_id = role_id;
            role_access.role_name = roleDetail[0].name;
            return role_access;
        }

        internal async Task<List<KeyValuePairs>> GetRoleList(int role_id = 0) 
        {
            string roleQry = $"SELECT id, name FROM UserRoles ";
            if(role_id > 0) 
            {
                roleQry += $"WHERE id = {role_id}";
            }
            List<KeyValuePairs> roleList = await Context.GetData<KeyValuePairs>(roleQry).ConfigureAwait(false);
            return roleList;
        }

        internal async Task<List<CMDesignation>> GetDesignationList()
        {
            string designationQry = $"SELECT id, designationName as name, designationDescriptions as description FROM userdesignation where status=1 ";
            
            List<CMDesignation> designationList = await Context.GetData<CMDesignation>(designationQry).ConfigureAwait(false);
            return designationList;
        }

        internal async Task<CMDefaultResponse> SetRoleAccess(CMSetRoleAccess request, int userID)
        {
            try
            {                
                // Get previous settings
                CMRoleAccess old_access = await GetRoleAccess(request.role_id);
                string featureAccessQry = "SELECT id as feature_id, concat(moduleName,': ',featureName) as feature_name, menuImage as menu_image, features.* FROM features;";
                List<CMAccessList> featureAccessList = await Context.GetData<CMAccessList>(featureAccessQry).ConfigureAwait(false);
                string featureIDquery = "SELECT id FROM features;";
                DataTable dt = await Context.FetchData(featureIDquery).ConfigureAwait(false);
                List<int> featureIDs = dt.GetColumn<int>("id");
                Dictionary<int, CMAccessList> features = new Dictionary<int, CMAccessList>();
                features.Merge(featureIDs, featureAccessList);
                // Check the whether to change existing settings or not
                if (request.set_role) 
                {
                    if (old_access.access_list != request.access_list)
                    {
                        // Delete the previous setting
                        string delete_qry = $" DELETE FROM RoleAccess WHERE RoleId = {request.role_id}";
                        await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                        // Insert the new setting
                        List<string> role_access = new List<string>();

                        foreach (var access in request.access_list)
                        {
                            CMAccessList feature = features[access.feature_id];
                            role_access.Add($"({request.role_id}, {access.feature_id}, {(feature.add == 0 ? -1 : access.add)}, " +
                                            $"{(feature.edit == 0 ? -1 : access.edit)}, {(feature.view == 0 ? -1 : access.view)}, " +
                                            $"{(feature.delete == 0 ? -1 : access.delete)}, {(feature.issue == 0 ? -1 : access.issue)}, " +
                                            $"{(feature.approve == 0 ? -1 : access.approve)}, {(feature.selfView == 0 ? -1 : access.selfView)}, " +
                                            $"'{UtilsRepository.GetUTCTime()}', {UtilsRepository.GetUserID()})");
                        }
                        string role_access_insert_str = string.Join(',', role_access);

                        string insert_query = $"INSERT INTO RoleAccess" +
                                                    $"(roleId, featureId, `add`, `edit`, `view`, `delete`, `issue`, `approve`, `selfView`, `lastModifiedAt`, `lastModifiedBy`) " +
                                              $" VALUES {role_access_insert_str}";
                        await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);

                        // Add previous setting to log table
                        CMLog _log = new CMLog();
                        _log.module_type = CMMS.CMMS_Modules.ROLE_DEFAULT_ACCESS_MODULE;
                        _log.module_ref_id = request.role_id;
                        _log.comment = JsonSerializer.Serialize(old_access.access_list);
                        _log.status = CMMS.CMMS_Status.UPDATED;
                        await _utilsRepo.AddLog(_log);                        
                    }
                }

                if (request.set_existing_users)
                {
                    List<CMUserID> user_list  = await getUsersByRoleId(request.role_id);
                    CMUserAccess user_access  = new CMUserAccess();
                    user_access.access_list   = request.access_list;

                    using (var repos = new UserAccessRepository(_conn))
                    {
                        foreach (var user in user_list)
                        {
                            user_access.user_id = user.id;
                            await repos.SetUserAccess(user_access, userID);
                        }
                    }
                }

                CMDefaultResponse response = new CMDefaultResponse(request.role_id, CMMS.RETRUNSTATUS.SUCCESS, "Updated Role Access Successfully");
                return response;
            }
            catch (Exception)
            {
                throw;
            }            
        }

        internal async Task<List<CMUserID>> getUsersByRoleId(int role_id) 
        {
            try
            {
                string query = $"SELECT id FROM Users WHERE roleId = {role_id}";
                List<CMUserID> user_list = await Context.GetData<CMUserID>(query).ConfigureAwait(false);
                return user_list;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to get Users List");
            }            
        }

        internal async Task<CMRoleNotifications> GetRoleNotifications(int role_id)
        {
            string qry = $"SELECT " +
                            $"rn.notificationId as notification_id, n.notification as notification_name, f.moduleName as module_name, f.featureName as feature_name, rn.defaultFlag as default_flag, rn.canChange as can_change " +
                         $"FROM " +
                            $"RoleNotifications as rn " +
                         $"JOIN Notifications n ON rn.notificationId = n.id " +
                         $"JOIN features f ON n.featureId=f.id " +
                         $"WHERE " +
                            $"roleId = {role_id}";

            List<CMNotificationList> access_list = await Context.GetData<CMNotificationList>(qry).ConfigureAwait(false);
            
            List<KeyValuePairs> roleDetail = await GetRoleList(role_id);

            CMRoleNotifications role_notification_list = new CMRoleNotifications();
            role_notification_list.notification_list = access_list;
            role_notification_list.role_id = role_id;
            role_notification_list.role_name = roleDetail[0].name;
            return role_notification_list;
        }

        internal async Task<CMDefaultResponse> SetRoleNotifications(CMSetRoleNotifications request, int userID)
        {
            try
            {
                // Get previous settings
                int role_id = request.role_id;
                CMRoleNotifications old_access_list = await GetRoleNotifications(role_id);

                if (request.set_role)
                {
                    // Check the whether to change existing settings or not
                    if (old_access_list.notification_list != request.notification_list)
                    {
                        // Delete the previous setting
                        string delete_qry = $" DELETE FROM RoleNotifications WHERE RoleId = {role_id}";
                        await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                        // Insert the new setting
                        List<string> role_access = new List<string>();

                        foreach (var access in request.notification_list)
                        {
                            role_access.Add($"({role_id}, {access.notification_id}, {access.default_flag}, {access.can_change}, " +
                                            $"'{UtilsRepository.GetUTCTime()}', {UtilsRepository.GetUserID()})");
                        }
                        string role_access_insert_str = string.Join(',', role_access);

                        string insert_query = $"INSERT INTO RoleNotifications" +
                                                    $"(roleId, notificationId, `defaultFlag`, `canChange`, `lastModifiedAt`, `lastModifiedBy`) " +
                                              $" VALUES {role_access_insert_str}";
                        await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);

                        // Add previous setting to log table
                        CMLog _log = new CMLog();
                        _log.module_type = CMMS.CMMS_Modules.ROLE_DEFAULT_NOTIFICATIONS;
                        _log.module_ref_id = role_id;
                        _log.comment = JsonSerializer.Serialize(old_access_list);
                        _log.status = CMMS.CMMS_Status.UPDATED;
                        await _utilsRepo.AddLog(_log);
                    }
                }

                if (request.set_existing_users)
                {
                    List<CMUserID> user_list = await getUsersByRoleId(request.role_id);
                    CMUserNotifications user_notification = new CMUserNotifications();
                    user_notification.notification_list = request.notification_list;

                    using(var repos = new UserAccessRepository(_conn))
                    {
                        foreach (var user in user_list)
                        {
                            user_notification.user_id = user.id;
                            await repos.SetUserNotifications(user_notification, userID);
                        }
                    }
                }                

                CMDefaultResponse response = new CMDefaultResponse(role_id, CMMS.RETRUNSTATUS.SUCCESS, "Updated Role Notifications Successfully");
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
