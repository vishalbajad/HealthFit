using HealthFit.API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Libs.InterfaceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HealthFit.JwtAuthentication.Model;
namespace API_Connector
{
    public class AuthenticateUser
    {
        public static readonly string Authentication_Api_EndPoint_Login = "/Authentication/Login/";

        readonly ApiServerDetails _apiserver;
        readonly HTTPConnector apiConnector;

        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="apiserver"></param>
        public AuthenticateUser(ApiServerDetails apiserver)
        {
            _apiserver = apiserver;
            apiConnector = new HTTPConnector(_apiserver);
        }
        /// <summary>
        /// Authenticate User for Api Endpoint access
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JwtToken Login(LoginModel user)
        {
            return apiConnector.SendJsonRequest<JwtToken>(Authentication_Api_EndPoint_Login, HTTPConnector.RequestMethod.POST, JsonSerializer.Serialize(user), string.Empty);
        }
    }
}
