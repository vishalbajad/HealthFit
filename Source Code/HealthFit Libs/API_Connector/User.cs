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
        readonly APIServer _apiserver;
        readonly HTTPConnector apiConnector;
        public User(APIServer apiserver)
        {
            _apiserver = apiserver;
            apiConnector = new HTTPConnector(_apiserver);
        }
        public bool CreateUser(HealthFit.Object_Provider.Model.User user)
        {
            return apiConnector.SendJsonRequest<bool>("/User/CreateUser/", HTTPConnector.RequestMethod.POST, JsonSerializer.Serialize(user), string.Empty);
        }

        public HealthFit.Object_Provider.Model.User? AunthenticateUser(string userName, string password)
        {
            return apiConnector.SendJsonRequest<HealthFit.Object_Provider.Model.User?>("/User/AunthenticateUser/", HTTPConnector.RequestMethod.GET, string.Empty, string.Format("userName={0}&password={1}", userName, password));
        }
    }
}
