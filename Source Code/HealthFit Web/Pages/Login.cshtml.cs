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
        public LoginModel(IOptions<SystemConfigurations> options, ILogger<IndexModel> logger) : base(options, logger)
        {
            userProxy = new API_Connector.User(this.GetAPIServerDetails());
        }

        public void OnGet()
        {

        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(HealthFit_Web.Models.User UserDetails)
        {
            var user = userProxy.AunthenticateUser(UserDetails.UserDetails.UserName, UserDetails.Password);

            if (user != null)
            {
                this.LoggedInUser = user;
                if (user.UserType == 1)
                    return RedirectToPage("./Index");
                else if (user.UserType == 2)
                    return RedirectToPage("Publishers/JournalList");
            }

            return RedirectToPage("./Login");
        }
    }
}