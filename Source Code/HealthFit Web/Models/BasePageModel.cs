using HealthFit.Object_Provider.Model;
using HealthFit_Web.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
namespace HealthFit_Web.Models
{
    public class BasePageModel : PageModel
    {
        private readonly SystemConfigurations sysConfig ;
        private readonly ILogger<IndexModel> _logger;
        public BasePageModel(IOptions<SystemConfigurations> options , ILogger<IndexModel> logger)
        {
            sysConfig = options.Value;
            _logger = logger;
        }

        public APIServer GetAPIServerDetails()
        {
            string? serializedObject = HttpContext?.Session?.GetString("APIServer");
            if (string.IsNullOrWhiteSpace(serializedObject))
            {
                APIServer apiserver = new APIServer { 
                ServerBaseUrl = sysConfig.APIServerBaseUrl,
                Username = sysConfig.APIServerUsername,
                Password = sysConfig.APIServerPassword,
                Token = sysConfig.APIServerToken
                };
              

                string serializedapiObject = JsonSerializer.Serialize(apiserver);
                HttpContext?.Session?.SetString("APIServer", serializedapiObject);
                return apiserver;
            }
            return JsonSerializer.Deserialize<APIServer>(serializedObject) ?? new APIServer();
        }
    }
}
