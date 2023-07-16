
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using CMMSAPIs.Models.Utils;


namespace CMMS_Services
{
    internal class APIHelper<T>
    {
        private RestClient restClient;
        private RestSharp.RestRequest restRequest;
        //public string baseUrl = "http://localhost:23835";
        public string baseUrl = "http://65.0.20.19/CMMS_API";

        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwibmJmIjoxNjg5NTM1NTg3LCJleHAiOjE2ODk1MzczODcsImlhdCI6MTY4OTUzNTU4N30.i15PoG34A2FkqgL0tHTyohBbcMf33fFso8Zdtqtf6X8";

        public APIHelper(string token)
        {
            this.token = token;
        }

        //public void SetToken(string token)
        //{
        //    this.token = token;
        //}
        
        public RestClient SetUrl(string endpoint)
        {
            //var url = Path.Combine(baseUrl, endpoint);
            var url = baseUrl + endpoint;
            restClient = new RestClient(url);
            return restClient;
        }

        public RestSharp.RestRequest CreatePostRequest(string payload)
        {
            restRequest = new RestRequest("", Method.Post);
            restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "application.json");
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            return restRequest;
        }

        public RestSharp.RestRequest CreatePutRequest(string payload)
        {
            restRequest = new RestRequest("", Method.Put);
            restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "application.json");
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            return restRequest;
        }
        public RestSharp.RestRequest CreateGetRequest()
        {
            restRequest = new RestRequest("",Method.Get);
            restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "application.json");
            return restRequest;
        }
        public RestSharp.RestRequest CreateDeleteRequest(string payload)
        {
            restRequest = new RestRequest("",Method.Delete);
            restRequest.AddHeader("Accept", "application.json");
            return restRequest;
        }
        public RestResponse GetResponse(RestClient client, RestRequest request)
        {
            return client.Execute(request);
        }

        public List<T> GetContentList(RestResponse response)
        {
            var content = response.Content;
            var test = JsonConvert.DeserializeObject<List<T>>(content);
            List<T> dtoObject = JsonConvert.DeserializeObject<List<T>>(content);
            return dtoObject;
           
        }
        public T GetContent(RestResponse response)
        {
            var content = response.Content.ToString();
            T dtoObject = JsonConvert.DeserializeObject<T>(content);

            return dtoObject;
        }
    }
}
