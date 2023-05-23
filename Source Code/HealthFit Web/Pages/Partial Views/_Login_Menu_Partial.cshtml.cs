using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace HealthFit_Web.Pages
{
    public class LoginMenuModel : BasePageModel
    {
        private readonly ILogger<LoginMenuModel> _logger;

        public LoginMenuModel(IOptions<SystemConfigurations> options, ILogger<LoginMenuModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}