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
        public static readonly string User_Api_EndPoint_CreateUser = "/User/CreateUser/";
        public static readonly string User_Api_EndPoint_AunthenticateUser = "/User/AunthenticateUser/";
        public static readonly string User_Api_EndPoint_GetAllPublisherList = "/User/GetAllPublisherList/";
        public static readonly string User_Api_EndPoint_GetAllPublicUserList = "/User/GetAllPublicUserList/";
        public static readonly string User_Api_EndPoint_SubscribeForJournal = "/User/SubscribeForJournal/";

        readonly ApiServerDetails _apiserver;
        readonly HTTPConnector apiConnector;
        /// <summary>
        /// Use APi Connector
        /// </summary>
        /// <param name="apiserver"></param>
        public User(ApiServerDetails apiserver)
        {
            _apiserver = apiserver;
            apiConnector = new HTTPConnector(_apiserver);
        }
        /// <summary>
        /// Add or update the User details
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CreateUser(HealthFit.Object_Provider.Model.User user)
        {
            return apiConnector.SendJsonRequest<bool>(User_Api_EndPoint_CreateUser, HTTPConnector.RequestMethod.POST, JsonSerializer.Serialize(user), string.Empty);
        }
        /// <summary>
        /// AUthenticate user to login to web portal
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public HealthFit.Object_Provider.Model.User? AunthenticateUser(string userName, string password)
        {
            return apiConnector.SendJsonRequest<HealthFit.Object_Provider.Model.User?>(User_Api_EndPoint_AunthenticateUser, HTTPConnector.RequestMethod.GET, string.Empty, string.Format("userName={0}&password={1}", userName, password));
        }
        /// <summary>
        /// Get list of all publishers
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public List<HealthFit.Object_Provider.Model.User>? GetAllPublisherList(int publisherId = 0, bool active = true)
        {
            return apiConnector.SendJsonRequest<List<HealthFit.Object_Provider.Model.User>?>(User_Api_EndPoint_GetAllPublisherList, HTTPConnector.RequestMethod.GET, string.Empty, string.Format("publisherId={0}&active={1}", publisherId, true));
        }
        /// <summary>
        /// Get list of all public users
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public List<HealthFit.Object_Provider.Model.User>? GetAllPublicUserList(int userId = 0, bool active = true)
        {
            return apiConnector.SendJsonRequest<List<HealthFit.Object_Provider.Model.User>?>(User_Api_EndPoint_GetAllPublicUserList, HTTPConnector.RequestMethod.GET, string.Empty, string.Format("userId={0}&active={1}", userId, true));
        }
        /// <summary>
        /// Subscrive user for journal
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="journalId"></param>
        /// <returns></returns>
        public bool SubscribeForJournal(int userId, int journalId)
        {
            return apiConnector.SendJsonRequest<bool>(User_Api_EndPoint_SubscribeForJournal, HTTPConnector.RequestMethod.GET, string.Empty, string.Format("userId={0}&journalId={1}", userId, journalId));
        }
    }
}
