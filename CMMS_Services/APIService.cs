using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace CMMS_Services
{
    public class APIService<T>
    {
        private string token;
        public string baseUrl = "http://172.20.43.9:83";
        private static string username = "Admin@cmms.com";
        private static string password = "Admin@123";
        //public string baseUrl = "http://localhost:23835";
        private static readonly object filelock = new object();
        public APIService(bool authorizationRequired = true)
        {
            if(authorizationRequired)
                SetToken();
        }
        public static void SetCredentials(string user, string pwd)
        {
            username = user;
            password = pwd;
        }
        private bool IsTokenExpired()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            if (jwtToken.ValidTo == null)
            {
                // The token does not have an expiration time
                return false;
            }

            // Check if the token's expiration time is greater than the current time
            return jwtToken.ValidTo <= DateTime.UtcNow;
        }
        private void SetToken()
        {
            string tokenFile = "token.txt";
            if (!File.Exists(tokenFile))
            {
                using(File.Create(tokenFile))
                {

                }
            }
            lock(filelock)
            {
                token = File.ReadAllText(tokenFile);
                if (token == null || token == "")
                {
                    GenerateToken();
                    SetToken();
                }
                else if (IsTokenExpired())
                {
                    GenerateToken();
                    SetToken();
                }
            }
            /**/
            //return token;
        }

        private void GenerateToken()
        {
            string tokenFile = "token.txt";
            if (!File.Exists(tokenFile))
            {
                using (File.Create(tokenFile))
                {

                }
            }
            lock(filelock)
            {
                Dictionary<string, string> credentials = new Dictionary<string, string>()
                {
                    { "user_name", username },
                    { "password", password }
                };
                var toeknService = new CMMS_Services.APIService<CMMSAPIs.Models.Users.UserToken>(false);
                var response = toeknService.FormPostService("/api/User/Authenticate", credentials);
                File.WriteAllText(tokenFile, response[0].token);
            }
        }

        public List<T> GetItemList(string endPoint) // to get multiple items in a GET Request
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var client = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreateGetRequest();
            var response = apiHelper.GetResponse(client, request);
            List<T> content = apiHelper.GetContentList(response);
            return content;
        }

        public T GetItem(string endPoint) // to get a single item in a GET Request
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var client = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreateGetRequest();
            var response = apiHelper.GetResponse(client, request);
            T content = apiHelper.GetContent(response);
            //            if (content == null)
            //                return new T();
            return content;

        }

        public T CreateItem(string endPoint, dynamic payload) // to get a single response after sending a POST request
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePostRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            T content = apiHelper.GetContent(response);
            return content;
        }
        public List<T> CreateItems(string endPoint, dynamic payload) // to get multiple responses after sending POST request
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePostRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = apiHelper.GetContentList(response);
            return content;
        }

        public List<T> PostService(string endPoint, dynamic payload) // for running POST requests (accept JSON data)
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePostRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = new List<T>();
            content.Add(apiHelper.GetContent(response));
            return content;
        }


        // also for running POST requests (accept form data, for accepting files)
        public List<T> FormPostService(string endPoint, Dictionary<string, Tuple<bool,List<string>>> payload) 
        {
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePostRequestFromForm(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = new List<T>();
            content.Add(apiHelper.GetContent(response));
            return content;
        }

        // also for running POST requests (accept form data, without accepting any file)
        public List<T> FormPostService(string endPoint, Dictionary<string, string> payload)
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePostRequestFromForm(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = new List<T>();
            content.Add(apiHelper.GetContent(response));
            return content;
        }

        // for running PUT requests (accepting JSON data)
        public List<T> PutService(string endPoint, dynamic payload)
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePutRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = new List<T>();
            content.Add(apiHelper.GetContent(response));
            return content;
        }

        public List<T> FormPutService(string endPoint, Dictionary<string, Tuple<bool, List<string>>> payload)
        {
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePutRequestFromForm(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = new List<T>();
            content.Add(apiHelper.GetContent(response));
            return content;
        }
        public List<T> FormPutService(string endPoint, Dictionary<string, string> payload)
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePutRequestFromForm(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = new List<T>();
            content.Add(apiHelper.GetContent(response));
            return content;
        }
        public List<T> PatchService(string endPoint, dynamic payload)
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePatchRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = new List<T>();
            content.Add(apiHelper.GetContent(response));
            return content;
        }

        public List<T> DeleteService(string endPoint, dynamic payload)
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreateDeleteRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = new List<T>();
            content.Add(apiHelper.GetContent(response));
            return content;
        }

        public List<T> PatchServiceList(string endPoint, dynamic payload)
        {
            //SetToken();
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePatchRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = apiHelper.GetContentList(response);
            return content;
        }
    }
}
