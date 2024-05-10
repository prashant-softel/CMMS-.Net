using CMMSAPIs.Helper;
using CMMSAPIs.Models.Authentication;
using CMMSAPIs.Models.Users;
using CMMSAPIs.Models.Utils;
using CMMSAPIs.Repositories.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace CMMSAPIs.Repositories.Users
{
    public class UserAccessRepository : GenericRepository
    {
        private MYSQLDBHelper _conn;
        private readonly IConfiguration _configuration;
        private ErrorLog m_errorLog;
        //private RoleAccessRepository _roleAccessRepo;
        public UserAccessRepository(MYSQLDBHelper sqlDBHelper, IWebHostEnvironment webHostEnvironment = null, IConfiguration configuration = null) : base(sqlDBHelper)
        {
            m_errorLog = new ErrorLog(webHostEnvironment);
            _configuration = configuration;
            _conn = sqlDBHelper;

            //_roleAccessRepo = new RoleAccessRepository(sqlDBHelper);
        }

        //internal async Task<UserToken> Authenticate(CMUserCrentials userCrentials)
        //{
        //    string myQuery = "SELECT id FROM Users WHERE loginId = '" + userCrentials.user_name + "' AND password = '" + userCrentials.password + "'";
        //    List<CMUser> _List = await Context.GetData<CMUser>(myQuery).ConfigureAwait(false);
        //    if (_List.Count == 0)
        //    {
        //        return null;
        //    }
        //    var user_id = _List[0].id;
        //    var key = _configuration.GetValue<string>("JwtConfig:Key");
        //    var keyBytes = Encoding.ASCII.GetBytes(key);

        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    var tokenDescriptor = new SecurityTokenDescriptor()
        //    {
        //        Subject = new ClaimsIdentity(new Claim[] {
        //                new Claim(ClaimTypes.NameIdentifier, ""+user_id)
        //            }),
        //        Expires = DateTime.UtcNow.AddMinutes(CMMS.TOKEN_EXPIRATION_TIME),
        //        SigningCredentials = new SigningCredentials(
        //        new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    CMUserDetail userDetail = await GetUserDetail(user_id);
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    UserToken user_token = new UserToken(tokenHandler.WriteToken(token), userDetail);
        //    return user_token;
        //}
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

            // Update token in db
            string logQuery = $"INSERT INTO userlog(customtoken, tokenIssueTime, tokenExpiryTime, tokenRefreshCount, userID) " +
                $"VALUES ( '{tokenHandler.WriteToken(token)}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{DateTime.Now.AddMinutes((int)CMMS.TOKEN_EXPIRATION_TIME).ToString("yyyy-MM-dd HH:mm:ss")}', 0, {user_id});";
            var updateResult = await Context.ExecuteNonQry<int>(logQuery).ConfigureAwait(false);
            UserToken user_token = new UserToken(tokenHandler.WriteToken(token), userDetail);
            return user_token;
        }

        internal async Task<CMUserDetail> GetUserDetail(int user_id)
        {
            // Pending - Include all the property listed in CMUserDetail Model
            string qry = "SELECT " +
                            "u.id as id, firstName as first_name, lastName as last_name, u.secondaryEmail, u.landlineNumber as landline_number, CONCAT(firstName, ' ', lastName) as full_name, loginId as user_name, r.id as role_id, r.name as role_name, mobileNumber as contact_no, u.genderId as gender_id, gender.name as gender_name, u.bloodGroupId as blood_group_id, bloodgroup.name as blood_group_name, birthday as DOB, countries.name as country_name, u.countryId as country_id, states.name as state_name, u.stateId as state_id, cities.name as city_name, u.cityId as city_id, zipcode, CASE WHEN u.status=0 THEN 'Inactive' ELSE 'Active' END AS status, u.isEmployee, u.joiningDate, photo.id as photoId, photo.file_path AS photoPath, sign.id AS signatureId, sign.file_path AS signaturePath ,co.name as company_name ,companyId as company_id " +
                         "FROM " +
                            "Users as u " +
                         "LEFT JOIN " +
                            "gender ON u.genderId = gender.id " +
                         "LEFT JOIN " +
                            "bloodgroup ON u.bloodGroupId = bloodgroup.id " +
                         "LEFT JOIN " +
                            "uploadedfiles as photo ON photo.id = u.photoId " +
                         "LEFT JOIN " +
                            "uploadedfiles as sign ON sign.id = u.signatureId " +
                         "LEFT JOIN " +
                            "cities as cities ON cities.id = u.cityId " +
                         "LEFT JOIN " +
                            "states as states ON states.id = u.stateId and states.id = cities.state_id " +
                         "LEFT JOIN " +
                            "countries as countries ON countries.id = u.countryId and countries.id = cities.country_id and countries.id = states.country_id " +
                         "LEFT JOIN " +
                            "UserRoles as r ON u.roleId = r.id " +
                         "LEFT JOIN " +
                            "business as co on  u.companyId = co.id   " +
                         " WHERE " +
                            $"u.id = {user_id}";

            List<CMUserDetail> user_detail = await Context.GetData<CMUserDetail>(qry).ConfigureAwait(false);


            if (user_detail.Count > 0)
            {
                string facilitiesQry = $"SELECT facilities.id as id, facilities.name as name, spv.id as spv_id, spv.name as spv,facilities.address as location, isemployee as isEmployees  FROM userfacilities JOIN facilities ON userfacilities.facilityId = facilities.id LEFT JOIN spv ON facilities.spvId=spv.id WHERE userfacilities.userId = {user_id}  and userfacilities.status = 1;";
                List<CMPlantAccess> facilities = await Context.GetData<CMPlantAccess>(facilitiesQry).ConfigureAwait(false);
                foreach(var f in facilities)
                {

                    if (f.isEmployees == 1)
                    {
                        f.isEmployees = "true";
                    }
                    else
                    {
                        f.isEmployees = "false";
                    }
                }
                user_detail[0].plant_list = facilities;
                string reportToQuery = $"SELECT reportToId FROM users WHERE id = {user_id};";
                DataTable dt = await Context.FetchData(reportToQuery).ConfigureAwait(false);
                int reportTo = Convert.ToInt32(dt.Rows[0][0]);
                string reportToDetailsQry = $"SELECT " +
                                                $"u.id, CONCAT(firstName, ' ', lastName) as full_name, loginId as user_name, mobileNumber as contact_no, r.id as role_id, r.name as role_name, CASE WHEN u.status=0 THEN 'Inactive' ELSE 'Active' END AS status, photo.id AS photoId, photo.file_path AS photoPath, sign.id AS signatureId, sign.file_path AS signaturePath " +
                                            $"FROM " +
                                                $"Users as u " +
                                            $"JOIN " +
                                                $"UserRoles as r ON u.roleId = r.id " +
                                            $"LEFT JOIN " +
                                                $"uploadedfiles as photo ON photo.id = u.photoId " +
                                            $"LEFT JOIN " +
                                                $"uploadedfiles as sign ON sign.id = u.signatureId " +
                                            $"WHERE " +
                                                $"u.id = {reportTo}";
                List<CMUser> reportToDetails = await Context.GetData<CMUser>(reportToDetailsQry).ConfigureAwait(false);
     

                //CMResposibility reponsibility2 = new CMResposibility();
                //reponsibility2.id = 2;
                //reponsibility2.name = "Plumber";
                //reponsibility2.since = DateTime.Parse("12-12-2018");
                //reponsibility2.experianceYears = 4;
                user_detail[0].responsibility = await GetUserResponsibity(user_id);
        
                //user_detail[0].responsibility.Add(reponsibility2);
         
                if (reportToDetails.Count > 0)
                    user_detail[0].report_to = reportToDetails[0];
                return user_detail[0];
            }
            return null;
        }

        internal async Task<List<CMResposibility>> GetUserResponsibity(int UserID)
        {
            string query = "select id , user_id,responsibility  ,since_when  from user_responsibility where user_id = " + UserID + "";
            List<CMResposibility> result = await Context.GetData<CMResposibility>(query).ConfigureAwait(false);
            return result;
        }

        internal async Task<CMImportFileResponse> ImportUsers(int file_id, int userID)
        {
            CMImportFileResponse response = null;

            string bloodGroupQry = "SELECT UPPER(name) as name, blood_group_id FROM bloodgroupaltnames";
            DataTable dtBloodGroups = await Context.FetchData(bloodGroupQry).ConfigureAwait(false);
            List<string> bloodGroupNames = dtBloodGroups.GetColumn<string>("name");
            List<int> bloodGroupIds = dtBloodGroups.GetColumn<int>("blood_group_id");
            Dictionary<string, int> bloodGroups = new Dictionary<string, int>();
            bloodGroups.Merge(bloodGroupNames, bloodGroupIds);

            string genderQry = "SELECT id, UPPER(name) as name, UPPER(initial) as initial FROM gender";
            DataTable dtGender = await Context.FetchData(genderQry).ConfigureAwait(false);
            List<string> genderNames = dtGender.GetColumn<string>("name");
            genderNames.AddRange(dtGender.GetColumn<string>("initial"));
            List<int> genderIds = dtGender.GetColumn<int>("id");
            genderIds.AddRange(genderIds);
            Dictionary<string, int> gender = new Dictionary<string, int>();
            gender.Merge(genderNames, genderIds);

            string roleQry = "SELECT id, UPPER(name) as name FROM userroles";
            DataTable dtRole = await Context.FetchData(roleQry).ConfigureAwait(false);
            List<string> roleNames = dtRole.GetColumn<string>("name");
            List<int> roleIds = dtRole.GetColumn<int>("id");
            Dictionary<string, int> roles = new Dictionary<string, int>();
            roles.Merge(roleNames, roleIds);

            string countryQry = "SELECT id, UPPER(name) as name FROM countries";
            DataTable dtCountry = await Context.FetchData(countryQry).ConfigureAwait(false);
            List<string> countryNames = dtCountry.GetColumn<string>("name");
            List<int> countryIds = dtCountry.GetColumn<int>("id");
            Dictionary<string, int> countries = new Dictionary<string, int>();
            countries.Merge(countryNames, countryIds);

            string plantQry = "SELECT id, UPPER(name) as name FROM facilities WHERE parentId = 0 GROUP BY name;";
            DataTable dtPlant = await Context.FetchData(plantQry).ConfigureAwait(false);
            List<string> plantNames = dtPlant.GetColumn<string>("name");
            List<int> plantIds = dtPlant.GetColumn<int>("id");
            Dictionary<string, int> plants = new Dictionary<string, int>();
            plants.Merge(plantNames, plantIds);

            string stateQry = "";
            DataTable dtState = null;
            List<string> stateNames = null;
            List<int> stateIds = null;
            Dictionary<string, int> states = new Dictionary<string, int>();

            string cityQry = "";
            DataTable dtCity = null;
            List<string> cityNames = null;
            List<int> cityIds = null;
            Dictionary<string, int> cities = new Dictionary<string, int>();

            List<string> yesNo = new List<string>() { "NO", "YES" };

            /*
            Username	Password	Secondary Email	First Name	Last Name	Date of Birth	Gender	Blood Group	
            Mobile Number	Landline Number	Country	State	City	Zipcode	Role	Is Employee	Joining Date

             */
            Dictionary<string, Tuple<string, Type>> columnNames = new Dictionary<string, Tuple<string, Type>>()
            {
                { "Username", new Tuple<string, Type>("user_name", typeof(string)) },
                { "Password", new Tuple<string, Type>("password", typeof(string)) },
                { "Secondary Email", new Tuple<string, Type>("secondaryEmail", typeof(string)) },
                { "First Name", new Tuple<string, Type>("first_name",typeof(string)) },
                { "Last Name", new Tuple<string, Type>("last_name",typeof(string)) },
                { "Date of Birth", new Tuple<string, Type>("DOB", typeof(DateTime)) },
                { "Gender", new Tuple<string, Type>("gender_name", typeof(string)) },
                { "Blood Group", new Tuple<string, Type>("blood_group_name", typeof(string)) },
                { "Mobile Number", new Tuple<string, Type>("contact_no", typeof(string)) },
                { "Landline Number", new Tuple<string, Type>("landline_number", typeof(string)) },
                { "Country", new Tuple<string, Type>("country_name", typeof(string)) },
                { "State", new Tuple<string, Type>("state_name", typeof(string)) },
                { "City", new Tuple<string, Type>("city_name", typeof(string)) },
                { "Zipcode", new Tuple<string, Type>("zipcode", typeof(double)) },
                { "Role", new Tuple<string, Type>("role_name", typeof(string)) },
                { "Is Employee", new Tuple<string, Type>("isEmployee", typeof(string)) },
                { "Joining Date", new Tuple<string, Type>("joiningDate", typeof(DateTime)) },
                { "Plant Associated", new Tuple<string, Type>("plant_name", typeof(string)) },
                { "Report To", new Tuple<string, Type>("report_to_email", typeof(string)) }
            };
            string query1 = $"SELECT file_path FROM uploadedfiles WHERE id = {file_id};";
            DataTable dt1 = await Context.FetchData(query1).ConfigureAwait(false);
            string path = Convert.ToString(dt1.Rows[0][0]);
            string dir = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);

            if (!Directory.Exists(dir))
                m_errorLog.SetError($"Directory '{dir}' cannot be found");
            else if (!File.Exists(path))
                m_errorLog.SetError($"File '{filename}' cannot be found in directory '{dir}'");
            else
            {
                FileInfo info = new FileInfo(path);
                if (info.Extension != ".xlsx")
                    m_errorLog.SetError("File is not an excel file");
                else
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excel = new ExcelPackage(path);
                    var sheet = excel.Workbook.Worksheets["User"];
                    if (sheet == null)
                        m_errorLog.SetWarning("Sheet containing user info shold be named 'User'");
                    else
                    {
                        DataTable dt2 = new DataTable();
                        foreach (var header in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                        {
                            try
                            {
                                dt2.Columns.Add(columnNames[header.Text].Item1, columnNames[header.Text].Item2);
                            }
                            catch (KeyNotFoundException)
                            {
                                dt2.Columns.Add(header.Text);
                            }
                        }
                        dt2.Columns.Add("gender_id", typeof(int));
                        dt2.Columns.Add("blood_group_id", typeof(int));
                        dt2.Columns.Add("country_id", typeof(int));
                        dt2.Columns.Add("state_id", typeof(int));
                        dt2.Columns.Add("city_id", typeof(int));
                        dt2.Columns.Add("role_id", typeof(int));
                        dt2.Columns.Add("plant_id", typeof(int));
                        dt2.Columns.Add("report_to_id", typeof(int));
                        dt2.Columns.Add("row_no", typeof(int));
                        for (int rN = 2; rN <= sheet.Dimension.End.Row; rN++)
                        {
                            ExcelRange row = sheet.Cells[rN, 1, rN, sheet.Dimension.End.Column];
                            DataRow newR = dt2.NewRow();
                            foreach (var cell in row)
                            {
                                try
                                {
                                    if (cell.Text == null || cell.Text == "")
                                        continue;
                                    newR[cell.Start.Column - 1] = Convert.ChangeType(cell.Text, dt2.Columns[cell.Start.Column - 1].DataType);
                                }
                                catch (Exception ex)
                                {
                                    ex.GetType();
                                    //+ ex.ToString();
                                    //status = status.Substring(0, (status.IndexOf("Exception") + 8));
                                    // m_ErrorLog.SetError("," + status);
                                }
                            }
                            if (newR.IsEmpty())
                            {
                                m_errorLog.SetInformation($"Row {rN} is empty.");
                                continue;
                            }
                            newR["row_no"] = rN;
                            if (Convert.ToString(newR["user_name"]) == null || Convert.ToString(newR["user_name"]) == "")
                                m_errorLog.SetError($"Login ID cannot be empty. [Row: {rN}]");
                            else if (!Convert.ToString(newR["user_name"]).IsEmail())
                                m_errorLog.SetError($"Invalid format for login ID. Should be in email address format. [Row: {rN}]");
                            else
                            {
                                string loginIdQuery = "SELECT loginId FROM users;";
                                DataTable dtLogin = await Context.FetchData(loginIdQuery).ConfigureAwait(false);
                                List<string> loginList = dtLogin.GetColumn<string>("loginId");
                                if (loginList.Contains(newR["user_name"]))
                                    m_errorLog.SetError($"Login ID already exists. [Row: {rN}]");
                            }
                            if (Convert.ToString(newR["password"]) == null || Convert.ToString(newR["password"]) == "")
                                m_errorLog.SetError($"Password cannot be empty. [Row: {rN}]");
                            else if (Convert.ToString(newR["password"]).Length < 6)
                                m_errorLog.SetError($"Password should have minimum 6 characters. [Row: {rN}]");
                            if (Convert.ToString(newR["secondaryEmail"]) != null && Convert.ToString(newR["secondaryEmail"]) != "")
                            {
                                if (!Convert.ToString(newR["secondaryEmail"]).IsEmail())
                                    m_errorLog.SetError($"Email ID is not valid. [Row: {rN}]");
                            }
                            if (Convert.ToString(newR["first_name"]) == null || Convert.ToString(newR["first_name"]) == "")
                                m_errorLog.SetError($"First name cannot be empty. [Row: {rN}]");
                            if (Convert.ToString(newR["last_name"]) == null || Convert.ToString(newR["last_name"]) == "")
                                m_errorLog.SetError($"Last name cannot be empty. [Row: {rN}]");
                            if (newR["DOB"] == DBNull.Value)
                                m_errorLog.SetError($"Date of Birth should not be empty. [Row {rN}]");
                            if (Convert.ToString(newR["contact_no"]) == null || Convert.ToString(newR["contact_no"]) == "")
                                m_errorLog.SetError($"Mobile Number cannot be empty. [Row: {rN}]");
                            else if (!Convert.ToString(newR["contact_no"]).IsContactNumber())
                                m_errorLog.SetError($"Mobile number is not valid. [Row: {rN}]");
                            if (Convert.ToString(newR["landline_number"]) != null && Convert.ToString(newR["landline_number"]) != "")
                            {
                                if (!Convert.ToString(newR["landline_number"]).IsContactNumber())
                                    m_errorLog.SetError($"Landline number is not valid. [Row: {rN}]");
                            }
                            if (newR["zipcode"] == DBNull.Value)
                                m_errorLog.SetError($"Zipcode cannot be empty or 0. [Row: {rN}]");
                            else if (Convert.ToDouble(newR["zipcode"]) == 0)
                                m_errorLog.SetError($"Zipcode cannot be empty or 0. [Row: {rN}]");
                            if (Convert.ToString(newR["report_to_email"]) != null && Convert.ToString(newR["report_to_email"]) != "")
                            {
                                if (!Convert.ToString(newR["report_to_email"]).IsEmail())
                                    m_errorLog.SetError($"Reporting to person should be in email address format. [Row: {rN}]");
                            }
                            else
                                newR["report_to_email"] = "";
                            try
                            {
                                newR["role_id"] = roles[Convert.ToString(newR["role_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["role_name"]) == null || Convert.ToString(newR["role_name"]) == "")
                                    m_errorLog.SetError($"Role Name cannot be empty. [Row: {rN}]");
                                else
                                    m_errorLog.SetError($"Invalid Role Name. [Row: {rN}]");
                            }
                            try
                            {
                                newR["gender_id"] = gender[Convert.ToString(newR["gender_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["gender_name"]) == null || Convert.ToString(newR["gender_name"]) == "")
                                    m_errorLog.SetError($"Gender field cannot be empty. [Row: {rN}]");
                                else
                                    m_errorLog.SetError($"Invalid Gender. [Row: {rN}]");
                            }
                            try
                            {
                                newR["blood_group_id"] = bloodGroups[Convert.ToString(newR["blood_group_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["blood_group_name"]) == null || Convert.ToString(newR["blood_group_name"]) == "")
                                    newR["blood_group_id"] = 0;
                                else
                                    m_errorLog.SetError($"Invalid Blood Group. [Row: {rN}]");
                            }
                            try
                            {
                                newR["plant_id"] = plants[Convert.ToString(newR["plant_name"]).ToUpper()];
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["plant_name"]) == null || Convert.ToString(newR["plant_name"]) == "")
                                    m_errorLog.SetError($"User should be linked to at least one plant. [Row: {rN}]");
                                else
                                    m_errorLog.SetError($"Invalid plant name. [Row: {rN}]");
                            }
                            try
                            {
                                newR["country_id"] = countries[Convert.ToString(newR["country_name"]).ToUpper()];
                                states.Clear();
                                stateQry = $"SELECT id, UPPER(name) as name FROM states WHERE country_id = {newR["country_id"]};";
                                dtState = await Context.FetchData(stateQry).ConfigureAwait(false);
                                stateNames = dtState.GetColumn<string>("name");
                                stateIds = dtState.GetColumn<int>("id");
                                states.Merge(stateNames, stateIds);
                                try
                                {
                                    newR["state_id"] = states[Convert.ToString(newR["state_name"]).ToUpper()];
                                    cities.Clear();
                                    cityQry = $"SELECT id, UPPER(name) as name FROM cities WHERE state_id = {newR["state_id"]};";
                                    dtCity = await Context.FetchData(cityQry).ConfigureAwait(false);
                                    cityNames = dtCity.GetColumn<string>("name");
                                    cityIds = dtCity.GetColumn<int>("id");
                                    cities.Merge(cityNames, cityIds);
                                    try
                                    {
                                        newR["city_id"] = cities[Convert.ToString(newR["city_name"]).ToUpper()];
                                    }
                                    catch (KeyNotFoundException)
                                    {
                                        if (Convert.ToString(newR["city_name"]) == null || Convert.ToString(newR["city_name"]) == "")
                                            m_errorLog.SetError($"City cannot be empty. [Row: {rN}]");
                                        else
                                            m_errorLog.SetError($"No city named {Convert.ToString(newR["city_name"])} found in state {Convert.ToString(newR["state_name"])}. [Row: {rN}]");
                                    }
                                }
                                catch (KeyNotFoundException)
                                {
                                    if (Convert.ToString(newR["state_name"]) == null || Convert.ToString(newR["state_name"]) == "")
                                        m_errorLog.SetError($"State cannot be empty. [Row: {rN}]");
                                    else
                                        m_errorLog.SetError($"No state named {Convert.ToString(newR["state_name"])} found in country {Convert.ToString(newR["country_name"])}. [Row: {rN}]");
                                    m_errorLog.SetError($"Cannot access cities due to empty or invalid state name. [Row: {rN}]");
                                }
                            }
                            catch (KeyNotFoundException)
                            {
                                if (Convert.ToString(newR["country_name"]) == null || Convert.ToString(newR["country_name"]) == "")
                                    m_errorLog.SetError($"Country cannot be empty. [Row: {rN}]");
                                else
                                    m_errorLog.SetError($"No country named {Convert.ToString(newR["country_name"])} found. [Row: {rN}]");
                                m_errorLog.SetError($"Cannot access states and cities due to empty or invalid country name. [Row: {rN}]");
                            }
                            if (Convert.ToString(newR["isEmployee"]) == null || Convert.ToString(newR["isEmployee"]) == "")
                                newR["isEmployee"] = "No";
                            int index = yesNo.IndexOf(Convert.ToString(newR["isEmployee"]).ToUpper());
                            if (index == -1)
                                m_errorLog.SetError($"Answer for Is Employee should be Yes or No. [Row: {rN}]");
                            else
                                newR["isEmployee"] = $"{index}";
                            dt2.Rows.Add(newR);
                        }
                        if (m_errorLog.GetErrorCount() == 0)
                        {
                            dt2.ConvertColumnType("isEmployee", typeof(int));
                            dt2.ConvertColumnType("zipcode", typeof(int));

                            string userQry = "SELECT loginId FROM users";
                            DataTable userDt = await Context.FetchData(userQry).ConfigureAwait(false);

                            List<List<string>> loginList = new List<List<string>>() { userDt.GetColumn<string>("loginId") };

                            List<DataTable> userPriority = new List<DataTable>();

                            List<string> usernames = new List<string>();
                            usernames.AddRange(userDt.GetColumn<string>("loginId"));
                            usernames.AddRange(dt2.GetColumn<string>("user_name"));
                            usernames.Contains("");
                            DataRow[] filterRows = dt2.AsEnumerable()
                                       .Where(row => !usernames.Contains(row.Field<string>("report_to_email"), StringComparison.OrdinalIgnoreCase))
                                       .ToArray();
                            if (filterRows.Length > 0)
                            {
                                userPriority.Insert(0, filterRows.CopyToDataTable());
                                loginList.Insert(0, userPriority[userPriority.Count - 1].GetColumn<string>("user_name"));
                            }

                            foreach (var item in loginList)
                            {
                                List<string> temp = item;
                                do
                                {
                                    filterRows = dt2.AsEnumerable()
                                       .Where(row => temp.Contains(row.Field<string>("report_to_email"), StringComparison.OrdinalIgnoreCase))
                                       .ToArray();
                                    if (filterRows.Length == 0)
                                        continue;
                                    userPriority.Add(filterRows.CopyToDataTable());
                                    temp = userPriority[userPriority.Count - 1].GetColumn<string>("user_name");
                                } while (filterRows.Length != 0);
                            }
                            List<int> idList = new List<int>();

                            foreach (DataTable dtUsers in userPriority)
                            {
                                idList.AddRange((await AddUsers(dtUsers, userID)).id);
                            }
                            response = new CMImportFileResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, null, null, $"{idList.Count} user(s) added successfully");
                        }
                    }
                }
            }
                string logPath = m_errorLog.SaveAsText($"ImportLog\\ImportUsers_File{file_id}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}");
            string logQry = $"UPDATE uploadedfiles SET logfile = '{logPath}' WHERE id = {file_id}";
            await Context.ExecuteNonQry<int>(logQry).ConfigureAwait(false);
            logPath = logPath.Replace("\\\\", "\\");
            if (response == null)
                response = new CMImportFileResponse(0, CMMS.RETRUNSTATUS.FAILURE, logPath, m_errorLog.errorLog(), "Errors found while importing file.");
            else
            {
                m_errorLog.SetImportInformation("File imported successfully");
                response.error_log_file_path = logPath;
                response.import_log = m_errorLog.errorLog();
            }
            //Import excel file
            //Create validation
            //use map function to convert datatable to model

            return response;
        }

        internal async Task<CMDefaultResponse> AddUsers(DataTable users, int userID)
        {
            string userQry = "SELECT id, UPPER(loginId) as loginId FROM users;";
            DataTable dtUser = await Context.FetchData(userQry).ConfigureAwait(false);
            List<string> userNames = dtUser.GetColumn<string>("loginId");
            List<int> userIds = dtUser.GetColumn<int>("id");
            Dictionary<string, int> userDict = new Dictionary<string, int>();
            userDict.Merge(userNames, userIds);

            foreach (DataRow row in users.Rows)
            {
                try
                {
                    row["report_to_id"] = userDict[Convert.ToString(row["report_to_email"]).ToUpper()];
                }
                catch (KeyNotFoundException)
                {
                    m_errorLog.SetWarning($"User with login ID '{Convert.ToString(row["report_to_email"])}' not found. Setting reporting to ID as 0. [Row: {row["row_no"]}]");
                    row["report_to_id"] = 0;
                }
            }

            List<string> credCols = new List<string>()
            {
                "user_name", "password"
            };
            Tuple<DataTable, DataTable> splitDt = users.Split(credCols);
            List<CMCreateUser> userList = splitDt.Item2.MapTo<CMCreateUser>();
            List<CMUserCrentials> credList = splitDt.Item1.MapTo<CMUserCrentials>();
            for (int i = 0; i < userList.Count; i++)
            {
                userList[i].credentials = credList[i];
                userList[i].facilitiesid = new List<int>() { Convert.ToInt32(users.Rows[i]["plant_id"]) };
            }
            CMDefaultResponse response = await CreateUser(userList, userID);
            return response;
        }

        internal async Task<CMDefaultResponse> CreateUser(List<CMCreateUser> request, int userID)
        {
            /*
             * Read the required field for insert
             * Table - Users, UserNofication, UserAccess
             * Use existing functions to insert UserNotification and User Access in table
            */
            CMDefaultResponse response = null;
            List<int> idList = new List<int>();
            foreach (CMCreateUser user in request)
            {
                //string.Join("\n", err.errorLog());
                int index = request.FindIndex(x => x == user);
                string loginIdQuery = "SELECT loginId FROM users;";
                DataTable dtLogin = await Context.FetchData(loginIdQuery).ConfigureAwait(false);
                string[] loginList = dtLogin.GetColumn<string>("loginId").ToArray();
                if (Array.Exists(loginList, loginId => loginId == user.credentials.user_name))
                    m_errorLog.SetError($"Login ID already exists. [Index: {index}]");
                string country, state, city;
                string getCountryQry = $"SELECT name FROM countries WHERE id = {user.country_id};";
                DataTable dtCountry = await Context.FetchData(getCountryQry).ConfigureAwait(false);
                if (dtCountry.Rows.Count == 0)
                    m_errorLog.SetError($"Invalid Country. [Index: {index}]");
                country = Convert.ToString(dtCountry.Rows[0][0]);
                string getStateQry = $"SELECT name, country_id FROM states WHERE id = {user.state_id};";
                DataTable dtState = await Context.FetchData(getStateQry).ConfigureAwait(false);
                if (dtState.Rows.Count == 0)
                    m_errorLog.SetError($"Invalid State. [Index: {index}]");
                state = Convert.ToString(dtState.Rows[0]["name"]);
                string getCityQry = $"SELECT name FROM cities WHERE id = {user.city_id};";
                DataTable dtCity = await Context.FetchData(getCityQry).ConfigureAwait(false);
                if (dtCity.Rows.Count == 0)
                    m_errorLog.SetError($"Invalid City. [Index: {index}]");
                city = Convert.ToString(dtCity.Rows[0][0]);
                string myQuery1 = $"SELECT * FROM states WHERE id = {user.state_id} AND country_id = {user.country_id};";
                DataTable dt1 = await Context.FetchData(myQuery1).ConfigureAwait(false);
                if (dt1.Rows.Count == 0)
                {
                    m_errorLog.SetError($"{state} is not situated in {country}. [Index: {index}]");
                    user.country_id = Convert.ToInt32(dtState.Rows[0]["country_id"]);
                    dtCountry = null;
                    getCountryQry = $"SELECT name FROM countries WHERE id = {user.country_id};";
                    dtCountry = await Context.FetchData(getCountryQry).ConfigureAwait(false);
                    country = Convert.ToString(dtCountry.Rows[0][0]);
                }
                string myQuery2 = $"SELECT * FROM cities WHERE id = {user.city_id} AND state_id = {user.state_id} AND country_id = {user.country_id};";
                DataTable dt2 = await Context.FetchData(myQuery2).ConfigureAwait(false);
                if (dt2.Rows.Count == 0)
                    m_errorLog.SetError($"{city} is not situated in {state}, {country}. [Index: {index}]");
            }
            if (m_errorLog.GetErrorCount() == 0)
            {
                foreach (var user in request)
                {
                    string myQuery = "INSERT INTO users(loginId, password, secondaryEmail, firstName, lastName, birthday, genderId, gender, bloodGroupId, bloodGroup, photoId, " +
                                       "mobileNumber, landlineNumber, countryId, stateId, cityId, zipcode, roleId, isEmployee,companyId, joiningDate, createdBy, createdAt, reportToId, status) " +
                                       $"VALUES ('{user.credentials.user_name}', '{user.credentials.password}', '{user.secondaryEmail}', '{user.first_name}', '{user.last_name}', " +
                                       $"'{((DateTime)user.DOB).ToString("yyyy-MM-dd")}', {(int)user.gender_id}, '{user.gender_id}', {user.blood_group_id}, '{CMMS.BLOOD_GROUPS[user.blood_group_id]}', " +
                                       $"{user.photoId}, '{user.contact_no}','{user.landline_number}', {user.country_id}, {user.state_id}, {user.city_id}, {user.zipcode}, " +
                                       $"{user.role_id}, {(user.isEmployee == null ? 0 : user.isEmployee)},{user.company_id},'{((DateTime)user.joiningDate).ToString("yyyy-MM-dd HH:mm:ss")}', " +
                                       $"{userID}, '{UtilsRepository.GetUTCTime()}', {user.report_to_id}, 1); SELECT LAST_INSERT_ID(); ";

                    DataTable dt = await Context.FetchData(myQuery).ConfigureAwait(false);
                    int id = Convert.ToInt32(dt.Rows[0][0]);
                    /*  */
                    if (user.facility_list != null)
                    {
                        foreach (var facility in user.facility_list)

                        {
                            int isemp;
                            isemp = facility.isEmployees == true ? 1 : 0;
                            string addFacility = $"INSERT INTO userfacilities(userId, facilityId, createdAt, createdBy, status,isemployee) " +
                                                    $"VALUES ({id}, {facility.id},'{UtilsRepository.GetUTCTime()}', {userID}, 1,{isemp});";
                            await Context.ExecuteNonQry<int>(addFacility).ConfigureAwait(false);
                        }
                       
                    }

                    CMUserAccess userAccess = new CMUserAccess()
                    {
                        user_id = id,
                        access_list = user.access_list
                    };
                    CMUserNotifications userNotifications = new CMUserNotifications()
                    {
                        user_id = id,
                        notification_list = user.notification_list
                    };
                    using (var repos = new UtilsRepository(_conn))
                    {
                        await repos.AddHistoryLog(CMMS.CMMS_Modules.USER, id, 0, 0, "User Added", CMMS.CMMS_Status.CREATED, userID);
                    }
                    await SetUserAccess(userAccess, userID);
                    await SetUserNotifications(userNotifications, userID);
                    await SetUserResponsibility(user.user_responsibility_list, id, userID);
                    idList.Add(id);
                }
                response = new CMDefaultResponse(idList, CMMS.RETRUNSTATUS.SUCCESS, $"{idList.Count} user(s) added successfully");
            }
            else
            {
                response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.FAILURE, $"{string.Join("\r\n", m_errorLog.errorLog().ToArray())/**/}");
            }
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
            if (request.credentials != null)
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
            if (request.company_id > 0)
                updateQry += $"companyId = {request.company_id}, ";
            if (request.joiningDate != null)
                updateQry += $"joiningDate = '{((DateTime)request.joiningDate).ToString("yyyy-MM-dd HH:mm:ss")}', ";
            if (request.report_to_id > 0)
                updateQry += $"reportToId = {request.report_to_id}, ";
            updateQry += $"updatedBy = {userID}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            if (request.facility_list != null)
            {
                if (request.facility_list.Count > 0)
                {
                    string deleteFacilities = $"DELETE FROM userfacilities WHERE userId = {request.id};";
                    await Context.ExecuteNonQry<int>(deleteFacilities).ConfigureAwait(false);
                    foreach (var facility in request.facility_list)
                    {
                        int isemp;
                        isemp = facility.isEmployees == true ? 1 : 0;
                        string addFacility = $"INSERT INTO userfacilities(userId, facilityId, createdAt, createdBy, status,isemployee) " +
                                                $"VALUES ({request.id}, {facility.id},'{UtilsRepository.GetUTCTime()}', {userID}, 1,{isemp});";
                        await Context.ExecuteNonQry<int>(addFacility).ConfigureAwait(false);
                    }
                }
            }
            if (request.role_id > 0)
            {
                if (request.flagUpdateAccess)
                {
                    string delete_qry = $"DELETE FROM UsersAccess WHERE UserId = {request.id};";
                    await Context.ExecuteNonQry<int>(delete_qry).ConfigureAwait(false);
                    CMUserAccess acc = new CMUserAccess()
                    {
                        user_id = request.id
                    };
                    await SetUserAccess(acc, userID);
                }
                if (request.flagUpdateNotifications)
                {
                    string delete_qry = $"DELETE FROM usernotifications WHERE UserId = {request.id};";
                    await Context.ExecuteNonQry<int>(delete_qry).ConfigureAwait(false);
                    CMUserNotifications notif = new CMUserNotifications()
                    {
                        user_id = request.id
                    };
                    await SetUserNotifications(notif, userID);
                }
            }

            if(request.user_responsibility_list.Count > 0)
            {
                foreach(var item in request.user_responsibility_list)
                {
                    string updateQ = $"update user_responsibility set responsibility = '{item.responsibility}', since_when = '{((DateTime)item.since_when).ToString("yyyy-MM-dd HH:mm:ss")}' where id = {item.responsibility_id} ";
                                             
                    await Context.ExecuteNonQry<int>(updateQ).ConfigureAwait(false);
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
            string user_ids_str = "";
            if (request.user_ids != null)
            {
                user_ids_str += string.Join(",", request.user_ids.ToArray());
            }
            string qry = $"SELECT " +
                            $"u.id as id, u.loginId as user_name, concat(firstName, ' ', lastName) as full_name, ur.id as role_id, ur.name as role_name, u.mobileNumber as contact_no " +
                         $"FROM " +
                            $"UserNotifications as un  " +
                         $"LEFT JOIN " +
                            $"Users as u ON u.id = un.userId " +
                         $"LEFT JOIN " +
                            $"UserFacilities as uf ON uf.userId = u.id " +
                         $"LEFT JOIN " +
                            $"UserRoles as ur ON ur.id = u.roleId " +
                         $"LEFT JOIN " +
                            $"notifications as nt ON nt.id = un.notificationId " +
                         $"WHERE " +
                            $" userPreference = 1 AND softwareId = {(int)request.notification_id} " +
                            $" ";

            if (!user_ids_str.IsNullOrEmpty())
            {
                qry += $" AND (self = 0 and u.id IN({user_ids_str}))";
            }
            else
            {
                qry += $" AND self = 0";
            }
            if (request.facility_id != 0)
            {
                qry += $" AND uf.facilityId = {request.facility_id}";
            }

            qry += " group by un.userId ";

            List<CMUser> user_list = await Context.GetData<CMUser>(qry).ConfigureAwait(false);
            /*
             * Table - Users, UserNotification, Notification
             * Return user based on notification_id and facility_id 
            */
            return user_list;
        }


        internal async Task<List<CMUser>> GetUserList(int facility_id)
        {
            int id=0;
            string qry = $"SELECT " +
                            $"u.id, CONCAT(firstName, ' ', lastName) as full_name, loginId as user_name,DATE_FORMAT(u.createdAt,'%Y-%m-%d ') as  createdAt ,DATE_FORMAT(u.updatedAt,'%Y-%m-%d ') as updatedAt, mobileNumber as contact_no, r.id as role_id, r.name as role_name, CASE WHEN u.status=0 THEN 'Inactive' ELSE 'Active' END AS status, photo.id AS photoId, photo.file_path AS photoPath, sign.id AS signatureId, sign.file_path AS signaturePath " +
                         $"FROM " +
                            $"Users as u " +
                         $"JOIN " +
                            $"UserRoles as r ON u.roleId = r.id " +
                         $"JOIN " +
                            $"UserFacilities as uf ON uf.userId = u.id " +
                         $"LEFT JOIN " +
                            $"uploadedfiles as photo ON photo.id = u.photoId " +
                         $"LEFT JOIN " +
                            $"uploadedfiles as sign ON sign.id = u.signatureId " +
                         $"WHERE " +
                            $"uf.facilityId = {facility_id}";

            List<CMUser> user_list = await Context.GetData<CMUser>(qry).ConfigureAwait(false);
            foreach (var req in user_list)
            {
                id = req.id;
                string qry1 = $"SELECT f.name,uf.userId  from  userfacilities as uf left join   facilities as f on uf.facilityId=f.id where uf.userId={id} ";
                List<CMPlant> plant_list = await Context.GetData<CMPlant>(qry1).ConfigureAwait(false);
                req.Facilities = plant_list;
            }

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
                            $"userId = {user_id} and isActive = 1 order by serialNo";

            List<CMAccessList> access_list = await Context.GetData<CMAccessList>(qry).ConfigureAwait(false);
            CMUserDetail user_detail = await GetUserDetail(user_id);
            CMUserAccess user_access = new CMUserAccess();
            user_access.user_id = user_id;
            user_access.user_name = user_detail.full_name;
            user_access.access_list = access_list;
            return user_access;
        }

        internal async Task<CMDefaultResponse> SetUserAccess(CMUserAccess request, int userID)
        {
            try
            {
                // Get previous settings
                int user_id = request.user_id;
                List<CMAccessList> default_user_access = (await GetUserAccess(user_id)).access_list;
                if (default_user_access == null)
                {
                    string roleQry = $"SELECT roleId FROM users WHERE id = {request.user_id};";
                    DataTable dt = await Context.FetchData(roleQry).ConfigureAwait(false);
                    int role = Convert.ToInt32(dt.Rows[0][0]);
                    using (var repos = new RoleAccessRepository(_conn))
                    {
                        default_user_access = (await repos.GetRoleAccess(role)).access_list;
                        request.access_list = default_user_access;
                    }
                }
                else
                {
                    if (default_user_access.Count == 0)
                    {
                        string roleQry = $"SELECT roleId FROM users WHERE id = {request.user_id};";
                        DataTable dt = await Context.FetchData(roleQry).ConfigureAwait(false);
                        int role = Convert.ToInt32(dt.Rows[0][0]);
                        using (var repos = new RoleAccessRepository(_conn))
                        {
                            default_user_access = (await repos.GetRoleAccess(role)).access_list;
                            request.access_list = default_user_access;
                        }
                    }
                }
          



                default_user_access = default_user_access.ToLookup(item => item.feature_id)
                                    .Select(group => group.First())
                                    .ToList();

                Dictionary<dynamic, CMAccessList> old_access = default_user_access.SetPrimaryKey("feature_id");

                if (request.access_list != null)
                {
                    if (request.access_list.Count > 0)
                    {
                        Dictionary<dynamic, CMAccessList> new_access = request.access_list.SetPrimaryKey("feature_id");
                        foreach (var access in new_access)
                            old_access[access.Key] = access.Value;
                    }
                }

                // Delete the previous setting
                string delete_qry = $" DELETE FROM UsersAccess WHERE UserId = {user_id}";
                await Context.GetData<List<int>>(delete_qry).ConfigureAwait(false);

                // Insert the new setting
                List<string> user_access = new List<string>();

                foreach (var access in old_access.Values)
                {
                    user_access.Add($"union all  Select {user_id}, {access.feature_id}, {access.add}, {access.edit}, " +
                                    $"{access.view},{access.delete}, {access.issue}, {access.approve}, {access.selfView}, " +
                                    $"'{UtilsRepository.GetUTCTime()}', {userID} ");
                }
                if(user_access.Count > 0)
                {
                    user_access[0] = user_access[0].Substring(9);
                }
                string user_access_insert_str = string.Join("", user_access);

                string insert_query = $"INSERT INTO UsersAccess" +
                                        $"(userId, featureId, `add`, `edit`, `view`, `delete`, `issue`, `approve`, `selfView`, `lastModifiedAt`, `lastModifiedBy`) " +
                                        $" {user_access_insert_str}";
                if (user_access_insert_str != "")
                {
                    await Context.GetData<List<int>>(insert_query).ConfigureAwait(false);
                }
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
            CMUserDetail user_detail = await GetUserDetail(user_id);
            CMUserNotifications user_notification = new CMUserNotifications();
            user_notification.user_id = user_id;
            user_notification.user_name = user_detail.full_name;
            user_notification.notification_list = notification_list;
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
                    if (def_notif.ContainsKey(notif.Key))
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
                await Context.ExecuteNonQry<int>(delete_qry).ConfigureAwait(false);

                // Insert the new setting

                List<string> user_access = new List<string>();

                foreach (var access in def_notif.Values)
                {
                    user_access.Add($"({user_id}, {access.notification_id}, {access.can_change}, {access.user_flag}, " +
                                    $"'{UtilsRepository.GetUTCTime()}', {userID})");
                }
                string user_access_insert_str = string.Join(',', user_access);

                if (user_access_insert_str != "" && user_access_insert_str != null)
                {
                    string insert_query = $"INSERT INTO UserNotifications" +
                                        $"(userId, notificationId, `canChange`, `userPreference`, `lastModifiedAt`, `lastModifiedBy`) " +
                                        $" VALUES {user_access_insert_str}";
                    await Context.ExecuteNonQry<int>(insert_query).ConfigureAwait(false);
                }

                using (var repos = new UtilsRepository(_conn))
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

        internal async Task<List<CMCompetency>> GetCompetencyList()
        {
            string competencyQry = $"SELECT id, name, description FROM competencies where status=1 ";
            List<CMCompetency> competencyList = await Context.GetData<CMCompetency>(competencyQry).ConfigureAwait(false);
            return competencyList;
        }

        internal async Task<CMDefaultResponse> AddCompetency(CMCompetency request, int userId)
        {
            string competencyQry = $"INSERT INTO competencies(name, description, status, addedBy, addedAt) VALUES " +
            $"('{request.name}', '{request.description}', 1, {userId}, '{UtilsRepository.GetUTCTime()}');" +
            $"SELECT LAST_INSERT_ID(); ";
            DataTable dt = await Context.FetchData(competencyQry).ConfigureAwait(false);
            int id = Convert.ToInt32(dt.Rows[0][0]);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Competency Added");
        }

        internal async Task<CMDefaultResponse> UpdateCompetency(CMCompetency request, int userId)
        {
            string updateQry = "UPDATE competencies SET ";
            if (request.name != null && request.name != "")
                updateQry += $"name = '{request.name}', ";
            if (request.description != null && request.description != "")
                updateQry += $"description = '{request.description}', ";
            updateQry += $"updatedBy = {userId}, updatedAt = '{UtilsRepository.GetUTCTime()}' WHERE id = {request.id};";
            await Context.ExecuteNonQry<int>(updateQry).ConfigureAwait(false);
            return new CMDefaultResponse(request.id, CMMS.RETRUNSTATUS.SUCCESS, "Competency Details Updated");
        }

        internal async Task<CMDefaultResponse> DeleteCompetency(int id)
        {
            string deleteQry = $"UPDATE competencies SET status = 0 WHERE id = {id};";
            await Context.ExecuteNonQry<int>(deleteQry).ConfigureAwait(false);
            return new CMDefaultResponse(id, CMMS.RETRUNSTATUS.SUCCESS, "Competency Deleted");
        }

        internal async Task<CMDefaultResponse> SetUserResponsibility(List<CMUserResponsibilityList> request, int userID, int addedBy)
        {
            CMDefaultResponse response = new CMDefaultResponse();
            if (request != null)
            {
                foreach (var item in request)
                {
                    string competencyQry = $"INSERT INTO user_responsibility(user_id, responsibility, since_when, added_by, added_at) VALUES " +
                             $"({userID}, '{item.responsibility}', '{((DateTime)item.since_when).ToString("yyyy-MM-dd HH:mm:ss")}', {addedBy}, '{UtilsRepository.GetUTCTime()}');" +
                             $"SELECT LAST_INSERT_ID(); ";
                    DataTable dt = await Context.FetchData(competencyQry).ConfigureAwait(false);
                    int id = Convert.ToInt32(dt.Rows[0][0]);
                }
            }
            response = new CMDefaultResponse(0, CMMS.RETRUNSTATUS.SUCCESS, "Responsibility Added");
            return response;
        }

    }
}
