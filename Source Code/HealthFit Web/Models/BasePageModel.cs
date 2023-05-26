using HealthFit.Object_Provider.Model;
using HealthFit_Web.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthFit_Web.Models
{
    public class BasePageModel : PageModel
    {
        private readonly SystemConfigurations sysConfig;
        private readonly ILogger<BasePageModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// This is a base model of applications
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="httpContextAccessor"></param>
        public BasePageModel(IOptions<SystemConfigurations> options, ILogger<BasePageModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            sysConfig = options.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get Systeam Configuration from appsetting.sjon
        /// </summary>
        public SystemConfigurations HealthFitSystemConfigurations
        {
            get
            {
                SystemConfigurations systemConfigurations = new SystemConfigurations();

                string? serializedObject = _httpContextAccessor.HttpContext?.Session?.GetString("HealthFitSystemConfigurations");

                if (string.IsNullOrWhiteSpace(serializedObject))
                {
                    systemConfigurations.FileServerBaseUrl = sysConfig.FileServerBaseUrl;

                    string serializedapiObject = JsonSerializer.Serialize(systemConfigurations);
                    _httpContextAccessor.HttpContext?.Session?.SetString("HealthFitSystemConfigurations", serializedapiObject);

                    return systemConfigurations;
                }

                return JsonSerializer.Deserialize<SystemConfigurations>(serializedObject) ?? new SystemConfigurations();


            }
        }
        /// <summary>
        /// API Server Details COnfigurations
        /// </summary>
        /// <returns></returns>
        public APIServer GetAPIServerDetails()
        {
            string? serializedObject = _httpContextAccessor.HttpContext?.Session?.GetString("APIServer");
            if (string.IsNullOrWhiteSpace(serializedObject))
            {
                APIServer apiserver = new APIServer
                {
                    ServerBaseUrl = sysConfig.APIServerBaseUrl,
                    Username = sysConfig.APIServerUsername,
                    Password = sysConfig.APIServerPassword,
                    Token = sysConfig.APIServerToken
                };


                string serializedapiObject = JsonSerializer.Serialize(apiserver);
                _httpContextAccessor.HttpContext?.Session?.SetString("APIServer", serializedapiObject);
                return apiserver;
            }
            return JsonSerializer.Deserialize<APIServer>(serializedObject) ?? new APIServer();
        }

        /// <summary>
        /// Get Loggedin Use Details
        /// </summary>
        public HealthFit.Object_Provider.Model.User? LoggedInUser
        {
            get
            {
                HealthFit.Object_Provider.Model.User? objUser = null;
                string? serializedObject = _httpContextAccessor.HttpContext?.Session?.GetString("LoggedInUser");
                if (!string.IsNullOrWhiteSpace(serializedObject))
                {
                    HttpContext?.Session?.SetString("LoggedInUser", serializedObject);
                    objUser = JsonSerializer.Deserialize<HealthFit.Object_Provider.Model.User>(serializedObject);
                }
                return objUser;
            }
            set
            {
                HealthFit.Object_Provider.Model.User? objUser = value;
                if (objUser != null)
                {
                    _httpContextAccessor.HttpContext?.Session?.SetString("LoggedInUser", JsonSerializer.Serialize<HealthFit.Object_Provider.Model.User>(objUser));
                }
            }
        }

        /// <summary>
        /// Log out and clear all the session
        /// </summary>
        public void LoggedOut()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }
    }
}
