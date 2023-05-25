using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthFit.Object_Provider.Model
{
    public class APIServer
    {
        public string ServerBaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public APIServer()
        {

        }

    }
    public class SystemConfigurations
    {
        public string APIServerBaseUrl { get; set; }
        public string APIServerUsername { get; set; }
        public string APIServerPassword { get; set; }
        public string APIServerToken { get; set; }
        public string FileServerBaseUrl { get; set; }
    }
}
