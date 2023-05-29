using System;
using System.Collections.Generic;
using System.Text;

namespace CMMS_Services
{
    public class APIService<T>
    {
        string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwibmJmIjoxNjgzNDQ2OTkwLCJleHAiOjE2ODM0NDg3OTAsImlhdCI6MTY4MzQ0Njk5MH0.r2Inj4PXo3EnfcjDBZ7MQs7BBPvTO05h8KX1N3PbJYs";
        public string baseUrl = "http://3.111.196.218/CMMS_API";
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
