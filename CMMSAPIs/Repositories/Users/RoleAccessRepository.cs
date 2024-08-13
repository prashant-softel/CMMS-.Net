using CMMSAPIs.Helper;
using CMMSAPIs.Models.Masters;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CMMSAPIs.Repositories.Users
{
    public class RoleAccessRepository : GenericRepository
    {
        private MYSQLDBHelper _conn;
        //private UserAccessRepository _userAccessRepo;
        private UtilsRepository _utilsRepo;
        public UserAccessRepository _userAccessRepo;
        public RoleAccessRepository(MYSQLDBHelper sqlDBHelper) : base(sqlDBHelper)
        {
            _conn = sqlDBHelper;
            _utilsRepo = new UtilsRepository(_conn);
            _userAccessRepo = new UserAccessRepository(_conn);

        }

        internal async Task<CMRoleAccess> GetRoleAccess(int role_id)
        {
            string qry = $"SELECT " +
                            $"featureId as feature_id, f.featureName as feature_name, r.add, r.edit, r.delete, r.view, r.issue, r.approve, r.selfView " +
                         $"FROM " +
                            $"`RoleAccess` r " +
                            $"JOIN Features as f ON r.featureId = f.id " +
                         $"WHERE " +
                            $"roleId = {role_id} and isActive=1 order by serialNo";

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
            if (role_id > 0)
            {
                roleQry += $"WHERE id = {role_id}";
            }
            else
            {
                roleQry += "WHERE status = 1";
            }
            List<KeyValuePairs> roleList = await Context.GetData<KeyValuePairs>(roleQry).ConfigureAwait(false);
            return roleList;
        }
        internal async Task<CMDefaultResponse> AddRole(CMDefaultList request, int userId)
        {

            string myQuery = $"INSERT INTO userroles(name, status, addedBy, addedAt) VALUES " +
                                $"('{request.name}', 1, {userId}, '{UtilsRepository.GetUTCTime()}'); " +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            int rid = 0;
            string qry = $" SELECT roleId FROM  users where id= {userId}";
            DataTable dt1 = await Context.FetchData(qry).ConfigureAwait(false);
            rid = Convert.ToInt32(dt1.Rows[0][0]);

            List<string> role_access = new List<string>();
            string myf = "SELECT id as featureid FROM features order by id asc ;";
            List<CMAccessList1> fid = await Context.GetData<CMAccessList1>(myf).ConfigureAwait(false);

            foreach (CMAccessList1 access in fid)
            {

                role_access.Add($"({id}, {access.featureid}, {0}, " +
                 $"{0}, {0}, " +
                 $"{0}, {0}, " +
                 $"{0}, {0}, " +
                 $"'{UtilsRepository.GetUTCTime()}', {UtilsRepository.GetUserID()})");
            }
            string role_access_insert_str = string.Join(',', role_access);

            string insert_query = $"INSERT INTO RoleAccess" +
                                        $"(roleId, featureId, `add`, `edit`, `view`, `delete`, `issue`, `approve`, `selfView`, `lastModifiedAt`, `lastModifiedBy`) " +
                                  $" VALUES {role_access_insert_str}";
            await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);

            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Role Added");
        }

        internal async Task<CMDefaultResponse> UpdateRole(CMDefaultList request, int userID)
        {
            string updateQry = "UPDATE userroles SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            updateQry += $"updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Role Updated");
        }

        internal async Task<CMDefaultResponse> DeleteRole(int id)
        {
            string deleteQry = $"UPDATE userroles SET status = 0 WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Role Deleted");
        }

        internal async Task<List<CMDesignation>> GetDesignationList()
        {
            string designationQry = $"SELECT id, designationName as name, designationDescriptions as description FROM userdesignation where status=1 ";

            List<CMDesignation> designationList = await Context.GetData<CMDesignation>(designationQry).ConfigureAwait(false);
            return designationList;
        }

        internal async Task<CMDefaultResponse> AddDesignation(CMDesignation request, int userId)
        {
            string myQuery = $"INSERT INTO userdesignation(designationName, designationDescriptions, status, addedBy, addedAt) VALUES " +
                                $"('{request.name}', '{request.description}', 1, {userId}, '{UtilsRepository.GetUTCTime()}');" +
                                 $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Designation Added");
        }

        internal async Task<CMDefaultResponse> UpdateDesignation(CMDesignation request, int userID)
        {
            string updateQry = "UPDATE userdesignation SET ";
            if (request.name != null && request.name != "")
                updateQry += $"designationName = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"designationDescriptions = '{request.description}', ";
            updateQry += $"updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Designation Details Updated");
        }

        internal async Task<CMDefaultResponse> DeleteDesignation(int id)
        {
            string deleteQry = $"UPDATE userdesignation SET status = 0 WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Designation Status Deleted");
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

                    if (old_access.access_list.Count != request.access_list.Count)
                    {
                        // Delete the previous setting
                        string delete_qry = $" DELETE FROM RoleAccess WHERE RoleId = {request.role_id}";
                        await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                        // Insert the new setting
                        List<string> role_access = new List<string>();

                        foreach (var access in request.access_list)
                        {
                            CMAccessList feature = features[access.feature_id];
                            role_access.Add($"({request.role_id}, {access.feature_id}, {(feature.add == 0 ? 0 : access.add)}, " +
                                            $"{(feature.edit == 0 ? 0 : access.edit)}, {(feature.view == 0 ? 0 : access.view)}, " +
                                            $"{(feature.delete == 0 ? 0 : access.delete)}, {(feature.issue == 0 ? 0 : access.issue)}, " +
                                            $"{(feature.approve == 0 ? 0 : access.approve)}, {(feature.selfView == 0 ? 0 : access.selfView)}, " +
                                            $"'{UtilsRepository.GetUTCTime()}', {UtilsRepository.GetUserID()})");
                        }
                        string role_access_insert_str = string.Join(',', role_access);

                        string insert_query = $"INSERT INTO RoleAccess" +
                                                    $"(roleId, featureId, `add`, `edit`, `view`, `delete`, `issue`, `approve`, `selfView`, `lastModifiedAt`, `lastModifiedBy`) " +
                                              $" VALUES {role_access_insert_str}";
                        await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);
                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.ROLE_DEFAULT_ACCESS_MODULE, request.role_id, 0, 0, JsonSerializer.Serialize(old_access.access_list), CMMS.CMMS_Status.UPDATED, userID);
                        // Add previous setting to log table                       
                    }
                }

                if (request.set_existing_users)
                {
                    List<CMUserID> user_list = await getUsersByRoleId(request.role_id);
                    CMUserAccess user_access = new CMUserAccess();
                    user_access.access_list = request.access_list;

                    using (var repos = new UserAccessRepository(_conn))
                    {
                        foreach (var user in user_list)
                        {
                            user_access.user_id = user.id;
                            await repos.SetUserAccess(user_access, userID);

                        }
                    }

                    // added for old users

                    foreach (var user in user_list)
                    {
                        user_access.user_id = user.id;
                        string roleQry = $"SELECT roleId FROM users WHERE id = {user.id};";
                        DataTable dt_role = await Context.FetchData(roleQry).ConfigureAwait(false);
                        int role = Convert.ToInt32(dt_role.Rows[0][0]);

                        //Adding code for missed feature list
                        var default_user_access = (await GetRoleAccess(role)).access_list;
                        string missed_featureQ = $"select id as feature_id,featureName as feature_name, menuimage as menu_image from Features where id not in ({string.Join(",", default_user_access.Select(item => item.feature_id))}) and isActive=1 ; ";

                        List<CMAccessList> missed_feature_list = await Context.GetData<CMAccessList>(missed_featureQ).ConfigureAwait(false);

                        default_user_access.AddRange(missed_feature_list);
                        default_user_access = default_user_access.ToLookup(item => item.feature_id)
                                      .Select(group => group.First())
                                      .ToList();
                        string delete_qry = $" DELETE FROM RoleAccess WHERE RoleId = {user.id}";
                        await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                        // Insert the new setting
                        List<string> role_access = new List<string>();

                        foreach (var access in default_user_access)
                        {
                            Dictionary<dynamic, CMAccessList> feature_role_access = default_user_access.SetPrimaryKey("feature_id");
                            CMAccessList feature = features[access.feature_id];
                            role_access.Add($"({user.id}, {access.feature_id}, {(feature.add == 0 ? 0 : access.add)}, " +
                                            $"{(access.edit == 0 ? 0 : access.edit)}, {(access.view == 0 ? 0 : access.view)}, " +
                                            $"{(access.delete == 0 ? 0 : access.delete)}, {(access.issue == 0 ? 0 : access.issue)}, " +
                                            $"{(access.approve == 0 ? 0 : access.approve)}, {(access.selfView == 0 ? 0 : access.selfView)}, " +
                                            $"'{UtilsRepository.GetUTCTime()}', {UtilsRepository.GetUserID()})");
                        }
                        string role_access_insert_str = string.Join(',', role_access);

                        string insert_query = $"INSERT INTO RoleAccess" +
                                                    $"(roleId, featureId, `add`, `edit`, `view`, `delete`, `issue`, `approve`, `selfView`, `lastModifiedAt`, `lastModifiedBy`) " +
                                              $" VALUES {role_access_insert_str}";
                        await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);

                    }


                }
                else
                {
                    string delete_qry = $" DELETE FROM RoleAccess WHERE RoleId = {request.role_id}";
                    await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                    // Insert the new setting
                    List<string> role_access = new List<string>();

                    foreach (var access in request.access_list)
                    {
                        Dictionary<dynamic, CMAccessList> feature_role_access = request.access_list.SetPrimaryKey("feature_id");
                        CMAccessList feature = features[access.feature_id];
                        role_access.Add($"({request.role_id}, {access.feature_id}, {(feature.add == 0 ? 0 : access.add)}, " +
                                        $"{(access.edit == 0 ? 0 : access.edit)}, {(access.view == 0 ? 0 : access.view)}, " +
                                        $"{(access.delete == 0 ? 0 : access.delete)}, {(access.issue == 0 ? 0 : access.issue)}, " +
                                        $"{(access.approve == 0 ? 0 : access.approve)}, {(access.selfView == 0 ? 0 : access.selfView)}, " +
                                        $"'{UtilsRepository.GetUTCTime()}', {UtilsRepository.GetUserID()})");
                    }
                    string role_access_insert_str = string.Join(',', role_access);

                    string insert_query = $"INSERT INTO RoleAccess" +
                                                $"(roleId, featureId, `add`, `edit`, `view`, `delete`, `issue`, `approve`, `selfView`, `lastModifiedAt`, `lastModifiedBy`) " +
                                          $" VALUES {role_access_insert_str}";
                    await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);
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
                        if (request.notification_list.Count == 0)
                        {
                            string stmt_notificationIDs = "select featureId as notification_id  from roleaccess where roleId= " + role_id + " order by featureId asc;";
                            List<CMNotificationList> itemList = await Context.GetData<CMNotificationList>(stmt_notificationIDs).ConfigureAwait(false);
                            foreach (var access in itemList)
                            {
                                role_access.Add($"({role_id}, {access.notification_id}, 1, 1, " +
                                                $"'{UtilsRepository.GetUTCTime()}', {UtilsRepository.GetUserID()})");
                            }
                        }
                        string role_access_insert_str = string.Join(',', role_access);

                        string insert_query = $"INSERT INTO RoleNotifications" +
                                                    $"(roleId, notificationId, `defaultFlag`, `canChange`, `lastModifiedAt`, `lastModifiedBy`) " +
                                              $" VALUES {role_access_insert_str}";
                        await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);

                        await _utilsRepo.AddHistoryLog(CMMS.CMMS_Modules.ROLE_DEFAULT_NOTIFICATIONS, role_id, 0, 0, JsonSerializer.Serialize(old_access_list), CMMS.CMMS_Status.UPDATED, userID);
                        // Add previous setting to log table
                    }
                }

                if (request.set_existing_users)
                {
                    List<CMUserID> user_list = await getUsersByRoleId(request.role_id);
                    CMUserNotifications user_notification = new CMUserNotifications();
                    user_notification.notification_list = request.notification_list;

                    using (var repos = new UserAccessRepository(_conn))
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
