using CMMSAPIs.Helper;
using CMMSAPIs.Models.Authentication;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.Linq;
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
        //private RoleAccessRepository _roleAccessRepo;
        public UserAccessRepository(MYSQLDBHelper sqlDBHelper, IConfiguration configuration = null) : base(sqlDBHelper)
        {
            _configuration = configuration;
            _conn = sqlDBHelper;
            //_roleAccessRepo = new RoleAccessRepository(sqlDBHelper);
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
            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserToken user_token = new UserToken(tokenHandler.WriteToken(token), userDetail);
            return user_token;
        }

        internal async Task<CMUserDetail> GetUserDetail(int user_id)
        {
            // Pending - Include all the property listed in CMUserDetail Model
            string qry = "SELECT " +
                            "u.id as id, firstName as first_name, lastName as last_name, u.secondaryEmail, u.landlineNumber as landline_number, CONCAT(firstName, ' ', lastName) as full_name, loginId as user_name, r.id as role_id, r.name as role_name, mobileNumber as contact_no, u.genderId as gender_id, gender.name as gender_name, u.bloodGroupId as blood_group_id, bloodgroup.name as blood_group_name, birthday as DOB, countries.name as country_name, countryId as country_id, states.name as state_name, stateId as state_id, cities.name as city_name, cityId as city_id, zipcode, CASE WHEN u.status=0 THEN 'Inactive' ELSE 'Active' END AS status, u.isEmployee, u.joiningDate, u.photoId, files.file_path AS photoPath " +
                         "FROM " +
                            "Users as u " +
                         "LEFT JOIN " +
                            "gender ON u.genderId = gender.id " +
                         "LEFT JOIN " + 
                            "bloodgroup ON u.bloodGroupId = bloodgroup.id " + 
                         "LEFT JOIN " +
                            "uploadedfiles as files ON files.id = u.photoId " +
                         "LEFT JOIN " +
                            "cities as cities ON cities.id = u.cityId " + 
                         "LEFT JOIN " +
                            "states as states ON states.id = u.stateId and states.id = cities.state_id " + 
                         "LEFT JOIN " +
                            "countries as countries ON countries.id = u.countryId and countries.id = cities.country_id and countries.id = states.country_id " + 
                         "LEFT JOIN " +
                            "UserRoles as r ON u.roleId = r.id " +
                         " WHERE " +
                            $"u.id = {user_id}";

            List<CMUserDetail> user_detail = await Context.GetData<CMUserDetail>(qry).ConfigureAwait(false);
            
            if (user_detail.Count > 0)
            {
                string facilitiesQry = $"SELECT facilities.id, facilities.name FROM userfacilities JOIN facilities ON userfacilities.facilityId = facilities.id WHERE userfacilities.userId = {user_id};";
                List<CMDefaultList> facilities = await Context.GetData<CMDefaultList>(facilitiesQry).ConfigureAwait(false);
                user_detail[0].plant_list = facilities;
                return user_detail[0];
            }
            return null;
        }
    
        internal async Task<CMDefaultResponse> CreateUser(CMCreateUser request, int userID)
        {
            /*
             * Read the required field for insert
             * Table - Users, UserNofication, UserAccess
             * Use existing functions to insert UserNotification and User Access in table
            */
            string loginIdQuery = "SELECT loginId FROM users;";
            DataTable dtLogin = await Context.FetchData(loginIdQuery).ConfigureAwait(false);
            string[] loginList = dtLogin.GetColumn<string>("loginId").ToArray();
            if (Array.Exists(loginList, loginId => loginId == request.user_name))
                throw new ArgumentException("Login ID already exists");
            string country, state, city;
            string getCountryQry = $"SELECT name FROM countries WHERE id = {request.country_id};";
            DataTable dtCountry = await Context.FetchData(getCountryQry).ConfigureAwait(false);
            if (dtCountry.Rows.Count == 0)
                throw new ArgumentException("Invalid Country");
            country = Convert.ToString(dtCountry.Rows[0][0]);
            string getStateQry = $"SELECT name FROM states WHERE id = {request.state_id};";
            DataTable dtState = await Context.FetchData(getStateQry).ConfigureAwait(false);
            if (dtState.Rows.Count == 0)
                throw new ArgumentException("Invalid State");
            state = Convert.ToString(dtState.Rows[0][0]);
            string getCityQry = $"SELECT name FROM cities WHERE id = {request.city_id};";
            DataTable dtCity = await Context.FetchData(getCityQry).ConfigureAwait(false);
            if (dtCity.Rows.Count == 0)
                throw new ArgumentException("Invalid City");
            city = Convert.ToString(dtCity.Rows[0][0]);
            string myQuery1 = $"SELECT * FROM states WHERE id = {request.state_id} AND country_id = {request.country_id};";
            DataTable dt1 = await Context.FetchData(myQuery1).ConfigureAwait(false);
            if (dt1.Rows.Count == 0)
                throw new ArgumentException($"{state} is not situated in {country}");
            string myQuery2 = $"SELECT * FROM cities WHERE id = {request.city_id} AND state_id = {request.state_id} AND country_id = {request.country_id};";
            DataTable dt2 = await Context.FetchData(myQuery2).ConfigureAwait(false);
            if (dt2.Rows.Count == 0)
                throw new ArgumentException($"{city} is not situated in {state}, {country}");
            string myQuery = "INSERT INTO users(loginId, password, secondaryEmail, firstName, lastName, birthday, genderId, gender, bloodGroupId, bloodGroup, photoId, " +
                                "mobileNumber, landlineNumber, countryId, stateId, cityId, zipcode, roleId, isEmployee, joiningDate, createdBy, createdAt, status) " +
                                $"VALUES ('{request.credentials.user_name}', '{request.credentials.password}', '{request.secondaryEmail}', " +
                                $"'{request.first_name}', '{request.last_name}', '{((DateTime)request.DOB).ToString("yyyy-MM-dd")}', {(int)request.gender_id}, '{request.gender_id}', " +
                                $"{request.blood_group_id}, '{CMMS.BLOOD_GROUPS[request.blood_group_id]}', {request.photoId}, '{request.contact_no}','{request.landline_number}', " +
                                $"{request.country_id}, {request.state_id}, {request.city_id}, {request.zipcode}, {request.role_id}, {(request.isEmployee==null?0:request.isEmployee)}, " +
                                $"'{((DateTime)request.joiningDate).ToString("yyyy-MM-dd hh:mm:ss")}', {userID}, '{UtilsRepository.GetUTCTime()}', 1); SELECT LAST_INSERT_ID(); ";
            
            DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            /*  */
            if(request.facilities != null)
            {
                foreach (int facility in request.facilities)
                {
                    string addFacility = $"INSERT INTO userfacilities(userId, facilityId, createdAt, createdBy, status) " +
                                            $"VALUES ({id}, {facility}, '{UtilsRepository.GetUTCTime()}', {userID}, 0);";
                    await Context.ExecuteNonQry<int>(addFacility).ConfigureAwait(false);
                }
            }
            
            CMUserAccess userAccess = new CMUserAccess()
            {
                user_id = id,
                access_list = request.access_list
            };
            CMUserNotifications userNotifications = new CMUserNotifications()
            {
                user_id = id,
                notification_list = request.notification_list
            };
            using(var repos = new UtilsRepository(_conn))
            {
                await repos.AddHistoryLog(CMMS.CMMS_Modules.USER, id, 0, 0, "User Added", CMMS.CMMS_Status.CREATED, userID);
            }
            await SetUserAccess(userAccess, userID);
            await SetUserNotifications(userNotifications, userID);
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "User Created");
            /*    */
            return response;
        }
        internal async Task<CMDefaultResponse> UpdateUser(CMUpdateUser request, int userID)
        {
            /*
             * Read the required field for update
             * Table - Users, UserNofication, UserAccess
             * Use existing functions to insert UserNotification and User Access in table
            */
            string locationQry = $"SELECT countryId, stateId, cityId FROM facilities WHERE id = {request.id};";
            DataTable dt0 = await Context.FetchData(locationQry).ConfigureAwait(false);
            string country, state, city;
            DataTable dtCountry, dtState, dtCity;
            string getCountryQry = $"SELECT name FROM countries WHERE id = {request.country_id};";
            dtCountry = await Context.FetchData(getCountryQry).ConfigureAwait(false);
            if (dtCountry.Rows.Count == 0)
            {
                request.country_id = Convert.ToInt32(dt0.Rows[0]["countryId"]);
                getCountryQry = $"SELECT name FROM countries WHERE id = {request.country_id};";
                dtCountry = await Context.FetchData(getCountryQry).ConfigureAwait(false);
            }
            country = Convert.ToString(dtCountry.Rows[0][0]);
            string getStateQry = $"SELECT name FROM states WHERE id = {request.state_id};";
            dtState = await Context.FetchData(getStateQry).ConfigureAwait(false);
            if (dtState.Rows.Count == 0)
            {
                request.state_id = Convert.ToInt32(dt0.Rows[0]["stateId"]);
                getStateQry = $"SELECT name FROM states WHERE id = {request.state_id};";
                dtState = await Context.FetchData(getStateQry).ConfigureAwait(false);
            }
            state = Convert.ToString(dtState.Rows[0][0]);
            string getCityQry = $"SELECT name FROM cities WHERE id = {request.city_id};";
            dtCity = await Context.FetchData(getCityQry).ConfigureAwait(false);
            if (dtCity.Rows.Count == 0)
            {
                request.city_id = Convert.ToInt32(dt0.Rows[0]["cityId"]);
                getCityQry = $"SELECT name FROM cities WHERE id = {request.city_id};";
                dtCity = await Context.FetchData(getCityQry).ConfigureAwait(false);
            }
            city = Convert.ToString(dtCity.Rows[0][0]);
            string myQuery1 = $"SELECT * FROM states WHERE id = {request.state_id} AND country_id = {request.country_id};";
            DataTable dt1 = await Context.FetchData(myQuery1).ConfigureAwait(false);
            if (dt1.Rows.Count == 0)
                throw new ArgumentException($"{state} is not situated in {country}");
            string myQuery2 = $"SELECT * FROM cities WHERE id = {request.city_id} AND state_id = {request.state_id} AND country_id = {request.country_id};";
            DataTable dt2 = await Context.FetchData(myQuery2).ConfigureAwait(false);
            if (dt2.Rows.Count == 0)
                throw new ArgumentException($"{city} is not situated in {state}, {country}");
            
            string updateQry = "UPDATE users SET ";
            if(request.credentials != null)
            {
                if (request.credentials.user_name != null && request.credentials.user_name != "")
                    updateQry += $"loginId = '{request.credentials.user_name}', ";
                if (request.credentials.password != null && request.credentials.password != "")
                    updateQry += $"password = '{request.credentials.password}', ";
            }
            if (request.secondaryEmail != null && request.secondaryEmail != "")
                updateQry += $"secondaryEmail = '{request.secondaryEmail}', ";
            if (request.first_name != null && request.first_name != "")
                updateQry += $"firstName = '{request.first_name}', ";
            if (request.last_name != null && request.last_name != "")
                updateQry += $"lastName = '{request.last_name}', ";
            if (request.DOB != null)
                updateQry += $"birthday = '{((DateTime)request.DOB).ToString("yyyy-MM-dd")}', ";
            if (Enum.IsDefined(typeof(CMMS.Gender), request.gender_id))
                updateQry += $"genderId = {(int)request.gender_id}, gender = '{request.gender_id}', ";
            if (Array.Exists(CMMS.BLOOD_GROUPS.Keys.ToArray(), element => element == request.blood_group_id))
                updateQry += $"bloodGroupId = {request.blood_group_id}, bloodGroup = '{CMMS.BLOOD_GROUPS[request.blood_group_id]}', ";
            if (request.photoId > 0)
                updateQry += $"photoId = {request.photoId}, ";
            if (request.contact_no != null && request.contact_no != "")
                updateQry += $"mobileNumber = '{request.contact_no}', ";
            if (request.landline_number != null && request.landline_number != "")
                updateQry += $"landlineNumber = '{request.landline_number}', ";
            if (country != null && country != "")
                updateQry += $"countryId = {request.country_id}, ";
            if (state != null && state != "")
                updateQry += $"stateId = {request.state_id}, ";
            if (city != null && city != "")
                updateQry += $"cityId = {request.city_id}, ";
            if (request.zipcode > 0)
                updateQry += $"zipcode = {request.zipcode}, ";
            if (request.role_id > 0)
                updateQry += $"roleId = {request.role_id}, ";
            if (request.isEmployee != null)
                updateQry += $"isEmployee = {request.isEmployee}, ";
            if (request.joiningDate != null)
                updateQry += $"joiningDate = '{((DateTime)request.joiningDate).ToString("yyyy-MM-dd hh:mm:ss")}', ";
            updateQry += $"updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            if(request.facilities != null)
            {
                if(request.facilities.Count > 0)
                {
                    string deleteFacilities = $"DELETE FROM userfacilities WHERE userId = {request.id};";
                    await Context.ExecuteNonQry<int>(deleteFacilities).ConfigureAwait(false);
                    foreach (int facility in request.facilities)
                    {
                        string addFacility = $"INSERT INTO userfacilities(userId, facilityId, createdAt, createdBy, status) " +
                                                $"VALUES ({request.id}, {facility}, '{UtilsRepository.GetUTCTime()}', {userID}, 0);";
                        await Context.ExecuteNonQry<int>(addFacility).ConfigureAwait(false);
                    }
                }
            }
            using (var repos = new UtilsRepository(_conn))
            {
                await repos.AddHistoryLog(CMMS.CMMS_Modules.USER, request.id, 0, 0, "User Details Updated", CMMS.CMMS_Status.UPDATED, userID);
            }
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Updated User Details Successfully");
        }
        internal async Task<CMDefaultResponse> DeleteUser(int id, int userID)
        {
            /*
             * Table - Users, UserNotification, UserAccess
             * Delete from above tables and add log in history table
            */
            string deleteQry = $"DELETE FROM users WHERE id = {id}; " +
                                $"DELETE FROM usernotifications WHERE userId = {id}; " +
                                $"DELETE FROM usersaccess WHERE userId = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            using (var repos = new UtilsRepository(_conn))
            {
                await repos.AddHistoryLog(CMMS.CMMS_Modules.USER, id, 0, 0, "User Deleted", CMMS.CMMS_Status.DELETED, userID);
            }
            CMDefaultResponse response = new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "User Deleted");
            return response;
        }

        internal async Task<List<CMUser>> GetUserByNotificationId(CMUserByNotificationId request)
        {
            // Pending convert user_ids into string for where condition
            string user_ids_str = string.Join(",", request.user_ids.ToArray());
            string qry = $"SELECT " +
                            $"u.id as id, u.loginId as user_name, concat(firstName, ' ', lastName) as full_name, ur.id as role_id, ur.name as role_name, u.mobileNumber as contact_no " +
                         $"FROM " +
                            $"Users as u " +
                         $"JOIN " +
                            $"UserNotifications as un ON u.id = un.userId " +
                         $"JOIN " +
                            $"UserFacilities as uf ON uf.userId = u.id " +
                         $"JOIN " +
                            $"UserRoles as ur ON ur.id = u.roleId " +
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
                            $"u.id, CONCAT(firstName, ' ', lastName) as full_name, loginId as user_name, mobileNumber as contact_no, r.id as role_id, r.name as role_name, CASE WHEN u.status=0 THEN 'Inactive' ELSE 'Active' END AS status, u.photoId AS photoId, files.file_path AS photoPath " +
                         $"FROM " +
                            $"Users as u " +
                         $"JOIN " +
                            $"UserRoles as r ON u.roleId = r.id " +
                         $"JOIN " +
                            $"UserFacilities as uf ON uf.userId = u.id " +
                         $"LEFT JOIN " +
                            $"uploadedfiles as files ON files.id = u.photoId " +
                         $"WHERE " +
                            $"uf.facilityId = {facility_id}";
            
            List<CMUser> user_list = await Context.GetData<CMUser>(qry).ConfigureAwait(false);
            return user_list;
        }

        internal async Task<CMUserAccess> GetUserAccess(int user_id)
        {           
            string qry = $"SELECT " +
                            $"featureId as feature_id, f.featureName as feature_name, f.menuimage as menu_image, u.add, u.edit, u.delete, u.view, u.issue, u.approve, u.selfView " +
                         $"FROM " +
                            $"UsersAccess as u " +
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

        internal async Task<CMDefaultResponse> SetUserAccess(CMUserAccess request, int userID)
        {
            try
            {
                // Get previous settings
                int user_id = request.user_id;
                List<CMAccessList> default_user_access = (await GetUserAccess(user_id)).access_list;
                if(default_user_access == null)
                {
                    string roleQry = $"SELECT roleId FROM users WHERE id = {request.user_id};";
                    DataTable dt = await Context.FetchData(roleQry).ConfigureAwait(false);
                    int role = Convert.ToInt32(dt.Rows[0][0]);
                    using (var repos = new RoleAccessRepository(_conn))
                    {
                        default_user_access = (await repos.GetRoleAccess(role)).access_list;
                    }
                }
                else
                {
                    if(default_user_access.Count == 0)
                    {
                        string roleQry = $"SELECT roleId FROM users WHERE id = {request.user_id};";
                        DataTable dt = await Context.FetchData(roleQry).ConfigureAwait(false);
                        int role = Convert.ToInt32(dt.Rows[0][0]);
                        using (var repos = new RoleAccessRepository(_conn))
                        {
                            default_user_access = (await repos.GetRoleAccess(role)).access_list;
                        }
                    }
                }
                Dictionary<dynamic, CMAccessList> old_access = default_user_access.SetPrimaryKey("feature_id");
                Dictionary<dynamic, CMAccessList> new_access = request.access_list.SetPrimaryKey("feature_id");
                foreach (var access in new_access)
                    old_access[access.Key] = access.Value;
                // Delete the previous setting
                string delete_qry = $" DELETE FROM UsersAccess WHERE UserId = {user_id}";
                await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                // Insert the new setting
                List<string> user_access = new List<string>();

                foreach (var access in old_access.Values)
                {
                    user_access.Add($"({user_id}, {access.feature_id}, {access.add}, {access.edit}, " +
                                    $"{access.view},{access.delete}, {access.issue}, {access.approve}, {access.selfView}, " +
                                    $"'{UtilsRepository.GetUTCTime()}', {userID})");
                }
                string user_access_insert_str = string.Join(',', user_access);

                string insert_query = $"INSERT INTO UsersAccess" +
                                        $"(userId, featureId, `add`, `edit`, `view`, `delete`, `issue`, `approve`, `selfView`, `lastModifiedAt`, `lastModifiedBy`) " +
                                        $" VALUES {user_access_insert_str}";
                await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);
                using (var repos = new UtilsRepository(_conn))
                {
                    await repos.AddHistoryLog(CMMS.CMMS_Modules.USER_MODULE, user_id, 0, 0, "User Access Set", CMMS.CMMS_Status.UPDATED, userID);
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
                            $"un.notificationId as notification_id, n.notification as notification_name, f.moduleName as module_name, f.featureName as feature_name, un.default_flag, un.can_change as can_change, un.user_flag as user_flag " +
                         $"FROM " +
                            $"users_notification_view as un " +
                         $"JOIN Notifications n ON un.notificationId = n.id " +
                         $"JOIN features f ON n.featureId=f.id " +
                         $"WHERE " +
                            $"un.userId = {user_id}";

            List<CMNotificationList> notification_list = await Context.GetData<CMNotificationList>(qry).ConfigureAwait(false);
            CMUserDetail user_detail              = await GetUserDetail(user_id);
            CMUserNotifications user_notification = new CMUserNotifications();
            user_notification.user_id             = user_id;
            user_notification.user_name           = user_detail.full_name;
            user_notification.notification_list   = notification_list;
            return user_notification;
        }

        internal async Task<CMDefaultResponse> SetUserNotifications(CMUserNotifications request, int userID)
        {
            try
            {
                if (request.notification_list == null)
                    request.notification_list = new List<CMNotificationList>();
                // Get previous settings
                int user_id = request.user_id;
                List<CMNotificationList> default_notifications = null; 
                string roleQry = $"SELECT roleId FROM users WHERE id = {request.user_id};";
                DataTable dt = await Context.FetchData(roleQry).ConfigureAwait(false);
                int role = Convert.ToInt32(dt.Rows[0][0]);
                using (var repos = new RoleAccessRepository(_conn))
                {
                    default_notifications = (await repos.GetRoleNotifications(role)).notification_list;
                }
                List<CMNotificationList> old_notifications = (await GetUserNotifications(user_id)).notification_list;

                Dictionary<dynamic, CMNotificationList> def_notif = default_notifications.SetPrimaryKey("notification_id");
                Dictionary<dynamic, CMNotificationList> old_notif = old_notifications.SetPrimaryKey("notification_id");
                Dictionary<dynamic, CMNotificationList> new_notif = request.notification_list.SetPrimaryKey("notification_id");

                foreach (var notif in new_notif)
                    old_notif[notif.Key] = notif.Value;

                foreach (var notif in old_notif)
                {
                    if(def_notif.ContainsKey(notif.Key))
                    {
                        if (def_notif[notif.Key].can_change == 0)
                            def_notif[notif.Key].user_flag = def_notif[notif.Key].default_flag;
                        else
                            def_notif[notif.Key].user_flag = notif.Value.user_flag;
                    }
                    else
                    {
                        CMNotificationList n = new CMNotificationList()
                        {
                            notification_id = notif.Key,
                            default_flag = 0,
                            can_change = 1,
                            user_flag = notif.Value.user_flag
                        };
                        def_notif[notif.Key] = n;
                    }
                }

                foreach (var notif in def_notif)
                {
                    if (!old_notif.ContainsKey(notif.Key))
                        notif.Value.user_flag = notif.Value.default_flag;
                }

                // Delete the previous setting
                string delete_qry = $" DELETE FROM UserNotifications WHERE UserId = {user_id}";
                await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                // Insert the new setting
                List<string> user_access = new List<string>();

                foreach (var access in def_notif.Values)
                {
                    user_access.Add($"({user_id}, {access.notification_id}, {access.can_change}, {access.user_flag}, " +
                                    $"'{UtilsRepository.GetUTCTime()}', {userID})");
                }
                string user_access_insert_str = string.Join(',', user_access);

                string insert_query = $"INSERT INTO UserNotifications" +
                                        $"(userId, notificationId, `canChange`, `userPreference`, `lastModifiedAt`, `lastModifiedBy`) " +
                                        $" VALUES {user_access_insert_str}";
                await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);
                using(var repos = new UtilsRepository(_conn))
                {
                    await repos.AddHistoryLog(CMMS.CMMS_Modules.USER_NOTIFICATIONS, user_id, 0, 0, "User Notifications Set", CMMS.CMMS_Status.UPDATED, userID);
                }
                CMDefaultResponse response = new CMDefaultResponse(user_id, CMMS.RETRUNSTATUS.SUCCESS, "Updated User Notifications Successfully");
                return response;             
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
