using System.Collections.Specialized;
using System.Net;
using System.Text;
using HealthFit.Object_Provider.Model;
using HealthFit_LogClient;
namespace HealthFit.API_Connector
{
    public class HTTPConnector
    {
        protected struct RequestMethod
        {
            public const string GET = "GET";
            public const string POST = "POST";
            public const string PUT = "PUT";
            public const string DELETE = "DELETE";
        }

        private string requestContentType = "application/json; charset=utf-8";

        private readonly HttpClient _httpClient;
        private readonly APIServer _apiserver;

        public HTTPConnector(HttpClient httpClient , APIServer apiserver)
        {
            _httpClient = httpClient;
            _apiserver = apiserver;
        }

        public HttpResponseMessage ConnectToRemoteApiAsync(string action, string queryString, string requestMethod, string jsonData)
        {
            UriBuilder actionUri = new UriBuilder(_apiserver.ServerBaseUrl.TrimEnd('/') + "/" + action.TrimStart('/'));
            string ResponseText = String.Empty;
            HttpStatusCode ResponseStatusCode = HttpStatusCode.Unused;

            Logs.WriteMessage(string.Format(" Preparing Request{0}Action: {1}{0}QueryString: {2}{0}RequestMethod: {3}{0}JsonData: {4}{0}", Environment.NewLine, action, queryString, requestMethod, jsonData));

            if (!String.IsNullOrEmpty(queryString))
            {
                if (String.IsNullOrEmpty(actionUri.Query))
                    actionUri.Query = queryString.TrimStart(new char[] { '?' });
                else
                    actionUri.Query = actionUri.Query.TrimStart(new char[] { '?' }) + "&" + queryString;
            }

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpWebRequest request = WebRequest.Create(actionUri.Uri) as HttpWebRequest;
            request.Method = requestMethod;
            Logs.WriteMessage(string.Format(" Web Request: {0}{1} Web Request Method: ", actionUri.Uri, Environment.NewLine, requestMethod));
            if (!String.IsNullOrEmpty(jsonData))
            {

                byte[] bytes = UTF8Encoding.UTF8.GetBytes(jsonData);
                request.ContentType = requestContentType;
                request.ContentLength = bytes.Length;
                Logs.WriteMessage(string.Format(" Web Request ContentType: {0}{1}Web Request ContentLength: {2}{1}Web Request Content: {3}", requestContentType, Environment.NewLine, bytes.Length, jsonData));
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
            return new HttpResponseMessage { StatusCode = ResponseStatusCode, Content = new StringContent(ResponseText) };
        }

        public enum AuthorizationType
        {
            BEARER,
            BASIC
        }
    }
}