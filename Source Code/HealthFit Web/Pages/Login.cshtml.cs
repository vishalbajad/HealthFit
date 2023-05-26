using HealthFit.Object_Provider.Model;
using HealthFit.Utilities;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace HealthFit_Web.Pages
{
    public class LoginModel : BasePageModel
    {
        readonly API_Connector.User userProxy;
        readonly ILogger<LoginModel> _logger;
        public LoginModel(IOptions<SystemConfigurations> options, ILogger<LoginModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            userProxy = new API_Connector.User(this.ApiServerDetails);
            _logger = logger;
        }

        [BindProperty]
        public HealthFit_Web.Models.User UserDetails { get; set; }

        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        
        [ValidateAntiForgeryToken]
        public void OnGet(int logout = 0)
        {
            if (logout == 1)
                LoggedOut();

            ViewData["LoggedInUser"] = LoggedInUser;
        }
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.Log(LogLevel.Information, " User Login validation Start");

            var user = userProxy.AunthenticateUser(UserDetails.UserDetails.UserName, UserDetails.Password);

            if (user != null)
            {
                this.LoggedInUser = user;
                _logger.Log(LogLevel.Information, " User Successfully validation and logged into system");
                return RedirectToPage("Index");
            }
            else
            {
                _logger.Log(LogLevel.Warning, " User validation failed.");
                responseMessage = "Invalid Username or Password !";
                responseCode = "error";
            }


            return Page();
        }
    }
}