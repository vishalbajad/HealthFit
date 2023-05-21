using System.Collections.Specialized;
using System.Net;
using System.Text;
using HealthFit_LogClient;
namespace HealthFit.API_Connector
{
    public abstract class HTTPConnector
    {
        protected struct RequestMethod
        {
            public const string GET = "GET";
            public const string POST = "POST";
            public const string PUT = "PUT";
            public const string DELETE = "DELETE";
        }

        protected string ResponseText { set; get; }
        protected HttpStatusCode ResponseStatusCode { get; set; }
        protected WebHeaderCollection HttpResponseHeader { get; set; }

        private string requestContentType { get; set; }

        protected HTTPConnector()
        {
            requestContentType = "application/json; charset=utf-8";
            ResponseStatusCode = HttpStatusCode.Unused;
        }

        protected abstract UriBuilder BuildActionUri(string action);
        protected virtual string GetAuthTokenForUCA() { return String.Empty; }
        protected virtual bool IsAuthorizationEnabled() { return false; }
        protected virtual bool isFunctionCallCompatibleWithOceanaVersion() { return true; }

        public enum AuthorizationType
        {
            BEARER,
            BASIC
        }
        #region<General Methods>

        private bool CheckIdIsNotEmpty(string id)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(id) && id != "0")
                result = true;

            return result;
        }


       protected void SendJsonRequest(string action, string queryString, string requestMethod, string jsonData,
            AuthorizationType authorizationType = AuthorizationType.BEARER, string serverUrl = "", NameValueCollection headerDetails = null)
        {
            UriBuilder actionUri;
            ResponseText = String.Empty;
            ResponseStatusCode = HttpStatusCode.Unused;
            try
            {
                #region Log
                Logs.WriteMessage(string.Format(" Preparing Request{0}Action: {1}{0}QueryString: {2}{0}RequestMethod: {3}{0}JsonData: {4}{0}AuthorizationType: {5}",
                    Environment.NewLine, action, queryString, requestMethod, jsonData, authorizationType));
                #endregion
                
                if (!isFunctionCallCompatibleWithOceanaVersion())
                {
                    ResponseStatusCode = HttpStatusCode.NotImplemented;
                    return;
                }

                if (authorizationType.Equals(AuthorizationType.BASIC) && !string.IsNullOrEmpty(serverUrl))
                    actionUri = new UriBuilder(serverUrl.TrimEnd('/') + "/" + action.TrimStart('/'));
                else
                    actionUri = BuildActionUri(action);

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
                if (IsAuthorizationEnabled() && headerDetails == null)
                {
                    Logs.WriteMessage(" Authorization Enabled: " + IsAuthorizationEnabled());
                    request.Headers.Add(HttpRequestHeader.Authorization, GetAuthTokenForUCA());
                }

                #region Log
                Logs.WriteMessage(string.Format(" Web Request: {0}{1}Web Request Method: ", actionUri.Uri, Environment.NewLine, requestMethod));
                #endregion

                if (!String.IsNullOrEmpty(jsonData))
                {

                    byte[] bytes = UTF8Encoding.UTF8.GetBytes(jsonData);

                    request.ContentType = requestContentType;  
                    request.ContentLength = bytes.Length;

                    #region Log
                    Logs.WriteMessage(string.Format(" Web Request ContentType: {0}{1}Web Request ContentLength: {2}{1}Web Request Content: {3}", requestContentType, Environment.NewLine, bytes.Length, jsonData));
                    #endregion

                    using (Stream postStream = request.GetRequestStream())
                    {
                        postStream.Write(bytes, 0, bytes.Length);
                    }
                }

                #region POM APIs Connection
                if (headerDetails != null) request.Headers.Add(headerDetails);
                #endregion

                #region Log
                Logs.WriteMessage( " Sending Request...");
                #endregion
               
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    ResponseStatusCode = response.StatusCode;
                    HttpResponseHeader = response.Headers;

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        ResponseText = reader.ReadToEnd();
                    }

                    #region Log
                    Logs.WriteMessage( " Response: " + ResponseText);
                    #endregion
                }
            }
            catch (WebException we)
            {
                Logs.WriteError(" Exception: " + we.Message);
                try
                {
                    if (we.Response == null)
                    {
                        ResponseText = we.Message;
                        ResponseStatusCode = HttpStatusCode.ServiceUnavailable; 

                        #region Log
                        Logs.WriteError(string.Format(" error description: Server not found / Connection error{0}Response: {1}", Environment.NewLine, ResponseText));
                        #endregion

                        return;
                    }
                    else if (string.IsNullOrEmpty(we.Response.ContentType) ||
                            (!we.Response.ContentType.ToUpper().Contains("JSON") &&
                            !we.Response.ContentType.Contains("/")))
                    {
                        ResponseText = we.Message;
                        ResponseStatusCode = HttpStatusCode.ServiceUnavailable; 

                        #region Log
                        Logs.WriteError(string.Format(" error description: Application url not found{0}Response: {1}", Environment.NewLine, ResponseText));
                        #endregion

                        return;
                    }
                    else
                    {
                        ResponseStatusCode = ((HttpWebResponse)we.Response).StatusCode;

                        #region Log
                        Logs.WriteError(" Response StatusCode: " + ResponseStatusCode);
                        #endregion
                    }
                }
                catch
                {
                    ResponseStatusCode = HttpStatusCode.BadRequest;

                    #region Log
                    Logs.WriteError(" Response StatusCode: --UNKNOWN--");
                    #endregion
                }

                if (we.Response != null)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(we.Response.GetResponseStream()))
                        {
                            ResponseText = reader.ReadToEnd();
                        }

                        #region Log
                        Logs.WriteError(" Response: " + ResponseText);
                        #endregion

                        if (ResponseText.ToUpper().Contains("<HTML"))
                            ResponseText = String.Empty;
                    }
                    catch { }
                }
                else
                {
                    #region Log
                    Logs.WriteError(" Response: --EMPTY--");
                    #endregion
                }
            }
        }

        #endregion


    }
}