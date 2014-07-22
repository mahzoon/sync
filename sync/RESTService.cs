using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sync.classes;
using RestSharp;

namespace sync
{
    class RESTService
    {
        public static Exception Last_Exception;

        public static SBasic<T> MakeAndExecuteGetRequest<T>(string request_url, Dictionary<string, string> get_parameters)
        {
            var client = new RestClient(Configurations.client_url);
            var request = new RestRequest(request_url, Method.GET);
            if (get_parameters != null)
                foreach (KeyValuePair<string, string> kvp in get_parameters)
                    request.AddParameter(kvp.Key, kvp.Value, ParameterType.UrlSegment);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            IRestResponse<SBasic<T>> response = client.Execute<SBasic<T>>(request);
            SBasic<T> result = response.Data;
            Last_Exception = GetPossibleError(response);
            return result;
        }

        public static SBasic<T> MakeAndExecutePostRequest<T>(string request_url, Dictionary<string, string> get_parameters, Dictionary<string, object> post_parameters)
        {
            var client = new RestClient(Configurations.client_url);
            var request = new RestRequest(request_url, Method.POST);
            if (get_parameters != null)
                foreach (KeyValuePair<string, string> kvp in get_parameters)
                    request.AddParameter(kvp.Key, kvp.Value, ParameterType.UrlSegment);
            if (post_parameters != null)
                foreach (KeyValuePair<string, object> kvp in post_parameters)
                    request.AddParameter(kvp.Key, kvp.Value);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            IRestResponse<SBasic<T>> response = client.Execute<SBasic<T>>(request);
            SBasic<T> result = response.Data;
            Last_Exception = GetPossibleError(response);
            return result;
        }

        public static string MakeAndExecutePostRequestWithFile(string request_url, Dictionary<string, string> get_parameters, Dictionary<string, object> post_parameters, string file_name, string file_path)
        {
            var client = new RestClient(Configurations.client_url);
            var request = new RestRequest(request_url, Method.POST);
            if (get_parameters != null)
                foreach (KeyValuePair<string, string> kvp in get_parameters)
                    request.AddParameter(kvp.Key, kvp.Value, ParameterType.UrlSegment);
            if (post_parameters != null)
                foreach (KeyValuePair<string, object> kvp in post_parameters)
                    request.AddParameter(kvp.Key, kvp.Value);
            request.AddFile(file_name, file_path);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            IRestResponse response = client.Execute(request);
            string result = response.Content;
            Last_Exception = GetPossibleError(response);
            return result;
        }

        public static Exception GetPossibleError(IRestResponse response)
        {
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                Exception theException = new ApplicationException(message, response.ErrorException);
                return theException;
            }
            return null;
        }
    }
}
