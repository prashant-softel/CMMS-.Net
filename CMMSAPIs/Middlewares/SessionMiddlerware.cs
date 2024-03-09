using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using System.IO;
using MySql.Data.MySqlClient;
using CMMSAPIs.Helper;
using System.Data.Common;
using System.Data;
using Microsoft.Extensions.Configuration;
using CMMSAPIs.Models.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CMMSAPIs.Middlewares
{
    public class SessionMiddlerware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly string MainConnection;

        public SessionMiddlerware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            MainConnection = configuration.GetConnectionString("Con");
        }
       //public MySqlConnection TheConnection => new MySqlConnection("server=172.20.43.9;User Id=hfeadmin;password=SDG%edg*&;database=hfe_cmms_latest;default command timeout=0;");
        //public async Task Invoke(HttpContext httpContext)
        //{
        //    var headers = httpContext.Request.Headers;           
        //    foreach (var hList in headers)
        //    {
        //        if (hList.Key == "Authorization") {

        //            String authorizeStr = hList.Value;
        //            char[] spearator = { ' ' };
        //            Int32 count = 2;
        //            String[] strlist = authorizeStr.Split(spearator, count, StringSplitOptions.None);
        //            var handler = new JwtSecurityTokenHandler();
        //            var jsonToken = handler.ReadToken(strlist[1]);
        //            var tokenS = jsonToken as JwtSecurityToken;
        //            var userId = tokenS.Claims.First(claim => claim.Type == "nameid").Value;

        //            httpContext.Session.SetString("_User_Id", userId);
        //        }                
        //    }
        //    await _next(httpContext); // calling next middleware

        //}

        public async Task Invoke(HttpContext httpContext)
        {
            var headers = httpContext.Request.Headers;
            string savedToken = "";
            DateTime expiryTokenTime = DateTime.Now;
            int tokenLogID = 0;
            int tokenRefreshCount = 0;
            var routeData = httpContext.GetRouteData();
            string requestBody = "";
            var httpRequestMethod = httpContext.Request.Method;

            if (httpRequestMethod == "GET")
            {
                requestBody = httpContext.Request.QueryString.ToString();
            }
            //else
            //{
            //    if (!httpContext.Request.HasFormContentType)
            //    {
            //        using (var buffer = new MemoryStream())
            //        {
            //            await httpContext.Request.Body.CopyToAsync(buffer);
            //            buffer.Seek(0, SeekOrigin.Begin);
            //            using (var reader = new StreamReader(buffer))
            //            {
            //                requestBody = await reader.ReadToEndAsync();
            //            }
            //        }
            //    }


            //}

            foreach (var hList in headers)
            {
                if (hList.Key == "Authorization")
                {
                    var endpoint = httpContext.GetEndpoint();
                    var endpointAddress = endpoint.DisplayName;
                    String authorizeStr = hList.Value;
                    char[] spearator = { ' ' };
                    Int32 count = 2;
                    String[] strlist = authorizeStr.Split(spearator, count, StringSplitOptions.None);
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(strlist[1]);
                    var tokenS = jsonToken as JwtSecurityToken;
                    var userId = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
                    DataTable dt = new DataTable();
                    using (MySqlConnection conn = new MySqlConnection(MainConnection))
                    {
                        using (MySqlCommand cmd = getQryCommand("select * from userlog where userID = " + userId + " and customToken = '"+ strlist[1] + "' order by id desc;", conn))
                        {
                            await conn.OpenAsync();
                            cmd.CommandTimeout = 99999;
                            cmd.CommandType = CommandType.Text;

                            using (DbDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                            {
                                DataSet ds = new DataSet();

                                ds.Tables.Add(dt);
                                dt.DataSet.EnforceConstraints = false;
                                dt.Load(dataReader);
                                cmd.Parameters.Clear();

                            }
                        }
                    }

                    if(dt.Rows.Count == 0)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await httpContext.Response.WriteAsync("Token has expired.");
                        return;
                    }

                    // fetching latest record
                    tokenLogID = Convert.ToInt32(dt.Rows[0]["id"]);
                    savedToken = Convert.ToString(dt.Rows[0]["customToken"]);
                    expiryTokenTime = Convert.ToDateTime(dt.Rows[0]["tokenExpiryTime"]);
                    tokenRefreshCount = Convert.ToInt32(dt.Rows[0]["tokenRefreshCount"]);
           

                    // Check token validity
                    if (!savedToken.Equals(strlist[1]))
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await httpContext.Response.WriteAsync("Invalid token is sent.");
                        return;
                    }


                    // Step 1: If token is about to expire refresh expiry
                    TimeSpan remainingTime = expiryTokenTime - DateTime.Now;
                    int remainingMinutes = (int)remainingTime.TotalMinutes;
                    if (remainingMinutes < (int)CMMS.TOKEN_RENEW_TIME && remainingMinutes > 0)
                    {
                        using (MySqlConnection conn = new MySqlConnection(MainConnection))
                        {
                            using (MySqlCommand cmd = getQryCommand("update userlog set tokenExpiryTime = '" + DateTime.Now.AddMinutes((int)CMMS.TOKEN_EXPIRATION_TIME).ToString("yyyy-MM-dd HH:mm:ss") + "', tokenRefreshCount = " + (tokenRefreshCount + 1) + "  where id = " + tokenLogID + "", conn))
                            {
                                await conn.OpenAsync();
                                int i = await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }

                    // Step 2: If token is already  expired then request for new token
                    if (expiryTokenTime < DateTime.Now)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await httpContext.Response.WriteAsync("Token has expired.");
                        return;
                    }

                    // Log Endpoint of user requested
                    using (MySqlConnection conn = new MySqlConnection(MainConnection))
                    {
                        string query = $"INSERT INTO loguserrequest(tokenLogID,userID, requestMethod, apiEndPoint, apiPayLoad, apiHitTime) VALUES( {tokenLogID}, {userId}, '{httpRequestMethod}', '{endpointAddress}','{requestBody}',  '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' );";
                        using (MySqlCommand cmd = getQryCommand(query, conn))
                        {
                            await conn.OpenAsync();
                            int i = await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    httpContext.Session.SetString("_User_Id", userId);
                }
            }


            List<CMFacilityInfo> timeZone = new List<CMFacilityInfo>();
            DataTable dt_timeZones = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(MainConnection))
            {
                using (MySqlCommand cmd = getQryCommand("SELECT case when timezone = 'default_hardcoded' then 'Asia/Kolkata' else timezone end as timezone,id as facility_id from facilities where parentId = 0;", conn))
                {
                    await conn.OpenAsync();
                    cmd.CommandTimeout = 99999;
                    cmd.CommandType = CommandType.Text;

                    using (DbDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                    {
                        DataSet ds = new DataSet();

                        ds.Tables.Add(dt_timeZones);
                        dt_timeZones.DataSet.EnforceConstraints = false;
                        dt_timeZones.Load(dataReader);
                        cmd.Parameters.Clear();

                    }
                }
            }
            if (dt_timeZones.Rows.Count > 0)
            {
                for (var i = 0; i < dt_timeZones.Rows.Count; i++)
                {
                    CMFacilityInfo item = new CMFacilityInfo();
                    item.facility_id = Convert.ToInt32(dt_timeZones.Rows[i]["facility_id"]);
                    item.timezone = Convert.ToString(dt_timeZones.Rows[i]["timezone"]);
                    timeZone.Add(item);
                }
            }
            httpContext.Session.SetString("FacilitiesInfo", JsonConvert.SerializeObject(timeZone));
            await _next(httpContext); // calling next middleware

        }

        internal MySqlCommand getQryCommand(string qry, MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand(qry);   //check if this line is required? see next line
            cmd = conn.CreateCommand();
            cmd.CommandTimeout = conn.ConnectionTimeout;
            cmd.CommandText = qry;
            return cmd;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionMiddlerware>();
        }
    }
}
