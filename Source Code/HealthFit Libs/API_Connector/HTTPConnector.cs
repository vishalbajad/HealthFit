using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.Json;
using HealthFit.Object_Provider.Model;
using HealthFit_LogClient;
using Newtonsoft.Json.Linq;

namespace HealthFit.API_Connector
{
    public class HTTPConnector
    {
        /// <summary>
        /// Rest API Request Methods
        /// </summary>
        public struct RequestMethod
        {
            public static string GET = "GET";
            public static string POST = "POST";
            public static string PUT = "PUT";
            public static string DELETE = "DELETE";
        }

        /// <summary>
        /// Content Type
        /// </summary>
        private string requestContentType = "application/json; charset=utf-8";

        /// <summary>
        /// API Server Details
        /// </summary>
        private readonly ApiServerDetails _apiserver;
        /// <summary>
        /// This Logic is being use to communicate with API ends points
        /// </summary>
        /// <param name="apiserver"></param>
        public HTTPConnector(ApiServerDetails apiserver)
        {
            _apiserver = apiserver;
        }

        /// <summary>
        /// Send Request to API Endpoint
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiUrl"></param>
        /// <param name="apiMethod"></param>
        /// <param name="apiJson"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public T SendJsonRequest<T>(string apiUrl, string apiMethod, string apiJson = "", string queryString = "")
        {
            T result;

            try
            {
                UriBuilder actionUri = new UriBuilder(_apiserver.ServerBaseUrl.TrimEnd('/') + "/" + apiUrl.TrimStart('/'));
                string ResponseText = String.Empty;
                HttpStatusCode ResponseStatusCode = HttpStatusCode.Unused;

                if (!String.IsNullOrEmpty(queryString))
                {
                    if (String.IsNullOrEmpty(actionUri.Query))
                        actionUri.Query = queryString.TrimStart(new char[] { '?' });
                    else
                        actionUri.Query = actionUri.Query.TrimStart(new char[] { '?' }) + "&" + queryString;
                }

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                HttpWebRequest request = WebRequest.Create(actionUri.Uri) as HttpWebRequest;
                request.Method = apiMethod;
                request.Headers["Authorization"] = $"Bearer {_apiserver.Token}";

                Logs.WriteMessage(string.Format(" Web Request: {0}{1} Web Request Method: ", actionUri.Uri, Environment.NewLine, apiJson));
                if (!String.IsNullOrEmpty(apiJson))
                {
                    byte[] bytes = UTF8Encoding.UTF8.GetBytes(apiJson);
                    request.ContentType = requestContentType;
                    request.ContentLength = bytes.Length;
                    Logs.WriteMessage(string.Format(" Web Request ContentType: {0}{1}Web Request ContentLength: {2}{1}Web Request Content: {3}", requestContentType, Environment.NewLine, bytes.Length, apiJson));
                    using (Stream postStream = request.GetRequestStream())
                    {
                        postStream.Write(bytes, 0, bytes.Length);
                    }
                }
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    ResponseStatusCode = response.StatusCode;

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        ResponseText = reader.ReadToEnd();
                    }

                    Logs.WriteMessage(" Response: " + ResponseText);

                }
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                result = JsonSerializer.Deserialize<T>(ResponseText, options);
            }
            catch
            {
                return default(T);
            }

            return result;
        }

        /// <summary>
        /// Autherization Types
        /// </summary>
        public enum AuthorizationType
        {
            BEARER,
            BASIC
        }
    }
}