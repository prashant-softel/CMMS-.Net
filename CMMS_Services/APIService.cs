using System;
using System.Collections.Generic;
using System.Text;

namespace CMMS_Services
{
    public class APIService<T>
    {
        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwibmJmIjoxNjg5NTM1NTg3LCJleHAiOjE2ODk1MzczODcsImlhdCI6MTY4OTUzNTU4N30.i15PoG34A2FkqgL0tHTyohBbcMf33fFso8Zdtqtf6X8";
        public string baseUrl = "http://65.0.20.19/CMMS_API";
        //public string baseUrl = "localhost";

        string getToken()
        {
            /*
            var toeknService = new CMMS_Services.APIService<CMMSAPIs.Models.Users.UserToken>();
            var response = toeknService.GetItem("/api/Token/Authenticate");
            token = response.token;
            int len = token.Length;
            Assert.AreNotEqual(0, len);
            */
            return token;
        }


        public List<T> GetItemList(string endPoint) 
        {

            var apiHelper = new APIHelper<T>(token);
            var client = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreateGetRequest();
            var response = apiHelper.GetResponse(client, request);
            List<T> content = apiHelper.GetContentList(response);
            return content;
        }

        public T GetItem(string endPoint)
        {
            var apiHelper = new APIHelper<T>(token);
            var client = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreateGetRequest();
            var response = apiHelper.GetResponse(client, request);
            T content = apiHelper.GetContent(response);
//            if (content == null)
//                return new T();
            return content;

        }

        public T CreateItem(string endPoint, dynamic payload)
        {
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePostRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            T content = apiHelper.GetContent(response);
            return content;
        }
        public List<T> CreateItems(string endPoint, dynamic payload)
        {
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePostRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = apiHelper.GetContent(response);
            return content;
        }

        public List<T> PostService(string endPoint, dynamic payload)
        {
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePostRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = apiHelper.GetContent(response);
            return content;
        }
        public List<T> PutService(string endPoint, dynamic payload)
        {
            var apiHelper = new APIHelper<T>(token);
            var url = apiHelper.SetUrl(endPoint);
            var request = apiHelper.CreatePutRequest(payload);
            var response = apiHelper.GetResponse(url, request);
            List<T> content = apiHelper.GetContent(response);
            return content;
        }


    }
}
