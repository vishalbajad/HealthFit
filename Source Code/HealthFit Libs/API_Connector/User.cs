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

        public List<HealthFit.Object_Provider.Model.User>? GetAllPublisherList(int publisherId = 0, bool active = true)
        {
            return apiConnector.SendJsonRequest<List<HealthFit.Object_Provider.Model.User>?>("/User/GetAllPublisherList/", HTTPConnector.RequestMethod.GET, string.Empty, string.Format("publisherId={0}&active={1}", publisherId, true));
        }
        public List<HealthFit.Object_Provider.Model.User>? GetAllPublicUserList(int userId = 0, bool active = true)
        {
            return apiConnector.SendJsonRequest<List<HealthFit.Object_Provider.Model.User>?>("/User/GetAllPublicUserList/", HTTPConnector.RequestMethod.GET, string.Empty, string.Format("userId={0}&active={1}", userId, true));
        }

        public bool SubscribeForJournal(int userId, int journalId)
        {
            return apiConnector.SendJsonRequest<bool>("/User/SubscribeForJournal/", HTTPConnector.RequestMethod.GET, string.Empty, string.Format("userId={0}&journalId={1}", userId, journalId));
        }

        
    }
}
