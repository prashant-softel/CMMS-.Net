using CMMSAPIs.BS.Users;
using CMMSAPIs.Helper;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;


namespace CMMSAPIs.Repositories.Users
{
    public class RoleAccessRepository : GenericRepository
    {
        private MYSQLDBHelper _conn;
        private UserAccessRepository _userAccessRepo;
        private UtilsRepository _utilsRepo;
        public RoleAccessRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _conn = sqlDBHelper;
            _utilsRepo = new UtilsRepository(_conn);
            _userAccessRepo = new UserAccessRepository(_conn);
        }

        internal async Task<List<CMAccess>> GetRoleAccess(int role_id)
        {
            string qry = $"SELECT " +
                            $"`featureId` as feature_id, `add`, `edit`, `delete`, `view`, `issue`, `approve`, `selfView` " +
                         $"FROM " +
                            $"`RoleAccess` " +
                         $"WHERE " +
                            $"roleId = {role_id}";

            List<CMAccess> access_list = await Context.GetData<CMAccess>(qry).ConfigureAwait(false);
            return access_list;
        }

        internal async Task<CMDefaultResponse> SetRoleAccess(CMRoleAccess request)
        {
            try
            {                
                // Get previous settings
                List<CMAccess> old_access_list = await GetRoleAccess(request.role_id);

                // Check the whether to change existing settings or not
                if (request.set_role) 
                {
                    if (old_access_list != request.access_list)
                    {
                        // Delete the previous setting
                        string delete_qry = $" DELETE FROM RoleAccess WHERE RoleId = {request.role_id}";
                        await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                        // Insert the new setting
                        List<string> role_access = new List<string>();

                        foreach (var access in request.access_list)
                        {
                            role_access.Add($"({request.role_id}, {access.feature_id}, {access.add}, {access.edit}, " +
                                            $"{access.delete}, {access.view}, {access.issue}, {access.approve}, {access.selfView}, " +
                                            $"'{UtilsRepository.GetUTCTime()}', {UtilsRepository.GetUserID()})");
                        }
                        string role_access_insert_str = string.Join(',', role_access);

                        string insert_query = $"INSERT INTO RoleAccess" +
                                                    $"(roleId, featureId, `add`, `edit`, `view`, `delete`, `issue`, `approve`, `selfView`, `lastModifiedAt`, `lastModifiedBy`) " +
                                              $" VALUES {role_access_insert_str}";
                        await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);

                        // Add previous setting to log table
                        CMLog _log = new CMLog();
                        _log.module_type = Constant.ROLE_DEFAULT_ACCESS_MODULE;
                        _log.module_ref_id = request.role_id;
                        _log.comment = JsonSerializer.Serialize(old_access_list);
                        _log.status = Constant.UPDATED;
                        await _utilsRepo.AddLog(_log);                        
                    }
                }

                if (request.set_existing_users)
                {
                    // fetch existing record from role access and compare with each users belongs in that role
                    // Don't overwrite the custom changes
                    // If user don't have access then only set the flag
                    // If user already have record then don't remove
                    //await SetUsersAccess(request.role_id, old_access_list);
                }

                CMDefaultResponse response = new CMDefaultResponse(request.role_id, 200, "Updated Role Access Successfully");
                return response;
            }
            catch (Exception)
            {
                throw;
            }            
        }

        internal async Task<List<CMRoleNotifications>> GetRoleNotifications(int role_id)
        {
            string qry = $"SELECT " +
                            $"roleId as role_id, notificationId as notification_id, defaultFlag as default_flag, canChange as can_change " +
                         $"FROM " +
                            $"`RoleNotifications` " +
                         $"WHERE " +
                            $"roleId = {role_id}";

            List<CMRoleNotifications> access_list = await Context.GetData<CMRoleNotifications>(qry).ConfigureAwait(false);
            return access_list;
        }

        internal async Task<CMDefaultResponse> SetRoleNotifications(List<CMRoleNotifications> request)
        {
            try
            {
                // Get previous settings
                int role_id = request[0].role_id;
                List<CMRoleNotifications> old_access_list = await GetRoleNotifications(role_id);

                // Check the whether to change existing settings or not
                if (old_access_list != request)
                {
                    // Delete the previous setting
                    string delete_qry = $" DELETE FROM RoleNotifications WHERE RoleId = {role_id}";
                    await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                    // Insert the new setting
                    List<string> role_access = new List<string>();

                    foreach (var access in request)
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
                    _log.module_type = Constant.ROLE_DEFAULT_NOTIFICATIONS;
                    _log.module_ref_id = role_id;
                    _log.comment = JsonSerializer.Serialize(old_access_list);
                    _log.status = Constant.UPDATED;
                    await _utilsRepo.AddLog(_log);
                }

                CMDefaultResponse response = new CMDefaultResponse(role_id, 200, "Updated Role Access Successfully");
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //internal async Task<int> CompareAndSetUserAccess(int role_id, List<CMAccess> role_default_access)
        //{
        //    // Get Specific role users access list
        //    string qry = "SELECT " +
        //                    "u.id as user_id, `featureId` as feature_id, `add`, `view`, `edit`, `delete`, `issue`, `approve`, `selfView` " +
        //                 "FROM " +
        //                    "UsersAccess as UA " +
        //                 "JOIN USERS as U " +
        //                    "ON UA.userId = U.id " +
        //                 "JOIN UserRoles as UR " +
        //                    "ON UR.id = U.roleId " +
        //                 "WHERE " +
        //                    $"U.roleId = {role_id}";

        //    List<CMUserAccess> userAccessList = await Context.GetData<CMUserAccess>(qry).ConfigureAwait(false);

        //    Dictionary<int, Dictionary<int, CMUserAccess>> _userAccessList = new Dictionary<int, Dictionary<int, CMUserAccess>>();
        //    List<string> access_type_list = new List<string>() { "add", "edit", "view", "delete", "issue", "approve", "selfView" };

        //    foreach (var uAccess in userAccessList)
        //    {
        //        Dictionary<int, CMUserAccess> _feature_dictionary = new Dictionary<int, CMUserAccess>();
        //        CMUserAccess _cmAccess = new CMUserAccess();

        //        _cmAccess.add = uAccess.add;
        //        _cmAccess.view = uAccess.view;
        //        _cmAccess.delete = uAccess.delete;
        //        _cmAccess.edit = uAccess.edit;
        //        _cmAccess.issue = uAccess.issue;
        //        _cmAccess.approve = uAccess.approve;
        //        _cmAccess.selfView = uAccess.selfView;
        //        _feature_dictionary.Add(uAccess.feature_id, _cmAccess);
        //        _userAccessList.Add(uAccess.user_id, _feature_dictionary);
        //    }

        //    foreach (KeyValuePair<int, Dictionary<int, CMUserAccess>> userAccess in _userAccessList)
        //    {
        //        int user_id = userAccess.Key;
        //        List<CMUserAccess> new_user_access = new List<CMUserAccess>();
        //        foreach (var roleAccess in role_default_access)
        //        {
        //            CMUserAccess current_user_access = userAccess.Value[roleAccess.feature_id];

        //            if (current_user_access.add == 0 && roleAccess.add == 1)
        //            {
        //                current_user_access.add = 1;
        //            }

        //            if (current_user_access.edit == 0 && roleAccess.edit == 1)
        //            {
        //                current_user_access.edit = 1;
        //            }

        //                if (current_user_access.view == 0 && roleAccess.view == 1)
        //            {
        //                current_user_access.view = 1;
        //            }

        //            if (current_user_access.delete == 0 && roleAccess.delete == 1)
        //            {
        //                current_user_access.delete = 1;
        //            }

        //            if (current_user_access.issue == 0 && roleAccess.issue == 1)
        //            {
        //                current_user_access.issue = 1;
        //            }

        //            if (current_user_access.approve == 0 && roleAccess.approve == 1)
        //            {
        //                current_user_access.approve = 1;
        //            }

        //            if (current_user_access.selfView == 0 && roleAccess.selfView == 1)
        //            {
        //                current_user_access.selfView = 1;
        //            }
        //            new_user_access.Add(current_user_access);
        //        }

        //        await _userAccessRepo.SetUserAccess(new_user_access);
        //    }
        //    return 0;
        //}
    }
}
