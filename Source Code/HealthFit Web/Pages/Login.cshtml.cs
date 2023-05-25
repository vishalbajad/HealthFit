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
        public LoginModel(IOptions<SystemConfigurations> options, ILogger<LoginModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            userProxy = new API_Connector.User(this.GetAPIServerDetails());
        }

        [BindProperty]
        public HealthFit_Web.Models.User UserDetails { get; set; }

        public string responseMessage { get; set; }
        public string responseCode { get; set; }

        public void OnGet(int logout = 0)
        {
            if (logout == 1)
            {
                LoggedInUser = new HealthFit.Object_Provider.Model.User();
            }
            ViewData["LoggedInUser"] = LoggedInUser;
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = userProxy.AunthenticateUser(UserDetails.UserDetails.UserName, UserDetails.Password);

            if (user != null)
            {
                this.LoggedInUser = user;
                return RedirectToPage("Index");
            }
            else
            {
                responseMessage = "Invalid Username or Password !";
                responseCode = "error";
            }


            return Page();
        }
    }
}