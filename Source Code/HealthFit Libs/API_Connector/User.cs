using HealthFit.API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Libs.InterfaceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public bool CreateUser(HealthFit.Object_Provider.Model.User user)
        {
            HttpClient httpClient = new HttpClient();
            HTTPConnector apiConnector = new HTTPConnector(httpClient, _apiserver);
            return apiConnector.SendJsonRequest<bool>("/User/CreateUser/", HTTPConnector.RequestMethod.POST, JsonSerializer.Serialize(user), string.Empty);
        }

        public HealthFit.Object_Provider.Model.User GetUser(int UserId)
        {
            return new HealthFit.Object_Provider.Model.User();
        }
    }
}
