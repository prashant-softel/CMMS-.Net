
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace CMMS_Services
{
    internal class APIHelper<T>
    {
        private RestClient restClient;
        private RestSharp.RestRequest restRequest;
        //public string baseUrl = "http://localhost:23835";
        public string baseUrl = "http://172.20.43.9:83";

        string token = "";

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
        public RestSharp.RestRequest CreatePostRequestFromForm(Dictionary<string, Tuple<bool, List<string>>> payload)
        {
            restRequest = new RestRequest("", Method.Post);
            if(token != null && token != "")
                restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "*/*");
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            string boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            restRequest.AddHeader("Content-Type", "multipart/form-data; boundary=" + boundary);
            string requestBody = "";
            foreach (var pair in payload)
            {
                if(pair.Value.Item1 == false)
                {
                    //if(pair.Value.Item2.Count == 1)
                    //    requestBody += "--" + boundary + "\r\n" +
                    //                    $"Content-Disposition: form-data; name=\"{pair.Key}\"\r\n" +
                    //                    $"\r\n" +
                    //                    $"{pair.Value.Item2[0]}\r\n";
                    //else
                    //{
                        foreach(var item in pair.Value.Item2)
                            requestBody += "--" + boundary + "\r\n" +
                                            $"Content-Disposition: form-data; name=\"{pair.Key}\"\r\n" +
                                            $"\r\n" +
                                            $"{item}\r\n";
                    //}
                }
                else
                {
                    foreach (var file in pair.Value.Item2)
                    {
                        // Read the file content
                        byte[] fileBytes = File.ReadAllBytes(file);

                        requestBody += "--" + boundary + "\r\n";
                        requestBody += $"Content-Disposition: form-data; name=\"{pair.Key}\"; filename=\"{Path.GetFileName(file)}\"\r\n";
                        requestBody += "Content-Type: application/octet-stream\r\n";
                        requestBody += "\r\n";
                        requestBody += Encoding.Default.GetString(fileBytes);
                        requestBody += "\r\n";
                    }
                }
            }
            requestBody += "--" + boundary + "--\r\n";
            restRequest.AddParameter("multipart/form-data", requestBody, ParameterType.RequestBody);
            return restRequest;
        }
        public RestSharp.RestRequest CreatePostRequestFromForm(Dictionary<string, string> payload)
        {
            restRequest = new RestRequest("", Method.Post);
            if (token != null && token != "")
                restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "*/*");
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            string boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            restRequest.AddHeader("Content-Type", "multipart/form-data; boundary=" + boundary);
            string requestBody = "";
            foreach (var pair in payload)
            {
                requestBody += "--" + boundary + "\r\n" +
                                $"Content-Disposition: form-data; name=\"{pair.Key}\"\r\n" +
                                $"\r\n" +
                                $"{pair.Value}\r\n";
            }
            requestBody += "--" + boundary + "--\r\n";
            restRequest.AddParameter("multipart/form-data", requestBody, ParameterType.RequestBody);
            return restRequest;
        }
        public RestSharp.RestRequest CreatePutRequestFromForm(Dictionary<string, Tuple<bool, List<string>>> payload)
        {
            restRequest = new RestRequest("", Method.Put);
            if (token != null && token != "")
                restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "*/*");
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            string boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            restRequest.AddHeader("Content-Type", "multipart/form-data; boundary=" + boundary);
            string requestBody = "";
            foreach (var pair in payload)
            {
                if (pair.Value.Item1 == false)
                {
                    if (pair.Value.Item2.Count == 1)
                        requestBody += "--" + boundary + "\r\n" +
                                        $"Content-Disposition: form-data; name=\"{pair.Key}\"\r\n" +
                                        $"\r\n" +
                                        $"{pair.Value.Item2[0]}\r\n";
                    else
                    {
                        foreach (var item in pair.Value.Item2)
                            requestBody += "--" + boundary + "\r\n" +
                                            $"Content-Disposition: form-data; name=\"{pair.Key}[]\"\r\n" +
                                            $"\r\n" +
                                            $"{item}\r\n";
                    }
                }
                else
                {
                    foreach (var file in pair.Value.Item2)
                    {
                        // Read the file content
                        byte[] fileBytes = File.ReadAllBytes(file);

                        requestBody += "--" + boundary + "\r\n";
                        requestBody += $"Content-Disposition: form-data; name=\"{pair.Key}\"; filename=\"{Path.GetFileName(file)}\"\r\n";
                        requestBody += "Content-Type: application/octet-stream\r\n";
                        requestBody += "\r\n";
                        requestBody += Encoding.Default.GetString(fileBytes);
                        requestBody += "\r\n";
                    }
                }
            }
            requestBody += "--" + boundary + "--\r\n";
            restRequest.AddParameter("multipart/form-data", requestBody, ParameterType.RequestBody);
            return restRequest;
        }
        public RestSharp.RestRequest CreatePutRequestFromForm(Dictionary<string, string> payload)
        {
            restRequest = new RestRequest("", Method.Put);
            if (token != null && token != "")
                restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "*/*");
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            string boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            restRequest.AddHeader("Content-Type", "multipart/form-data; boundary=" + boundary);
            string requestBody = "";
            foreach (var pair in payload)
            {
                requestBody += "--" + boundary + "\r\n" +
                                $"Content-Disposition: form-data; name=\"{pair.Key}\"\r\n" +
                                $"\r\n" +
                                $"{pair.Value}\r\n";
            }
            requestBody += "--" + boundary + "--\r\n";
            restRequest.AddParameter("multipart/form-data", requestBody, ParameterType.RequestBody);
            return restRequest;
        }
        public RestSharp.RestRequest CreatePostRequest(string payload)
        {
            restRequest = new RestRequest("", Method.Post);
            if (token != null && token != "")
                restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "application.json");
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            return restRequest;
        }
        public RestSharp.RestRequest CreatePutRequest(string payload)
        {
            restRequest = new RestRequest("", Method.Put);
            if (token != null && token != "")
                restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "application.json");
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            return restRequest;
        }
        public RestSharp.RestRequest CreatePatchRequest(string payload)
        {
            restRequest = new RestRequest("", Method.Patch);
            if (token != null && token != "")
                restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "application.json");
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            return restRequest;
        }
        public RestSharp.RestRequest CreateGetRequest()
        {
            restRequest = new RestRequest("", Method.Get);
            if (token != null && token != "")
                restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Accept", "application.json");
            return restRequest;
        }
        public RestSharp.RestRequest CreateDeleteRequest(string payload)
        {
            restRequest = new RestRequest("", Method.Delete);
            if (token != null && token != "")
                restRequest.AddHeader("Authorization", "Bearer " + token);
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
