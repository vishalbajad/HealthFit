using HealthFit.API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Libs.InterfaceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Connector
{
    public class User : IUser       
    {
        APIServer _apiserver;

        public User(APIServer apiserver)
        {
            _apiserver = apiserver;
        }
        public HttpResponseMessage CreateUser(HealthFit.Object_Provider.Model.User user)
        {
            HttpClient httpClient = new HttpClient();
            HTTPConnector apiConnector = new HTTPConnector(httpClient, _apiserver);
            return apiConnector.ConnectToRemoteApiAsync(string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public HttpResponseMessage GetUser(int UserId)
        {
            throw new NotImplementedException();
        }
    }
}
