using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HealthFit_Web.Models;
using Microsoft.Extensions.Options;
using HealthFit.Object_Provider.Model;
using Microsoft.AspNetCore.Http;
using HealthFit.Utilities;

namespace HealthFit_Web.Pages
{
    public class SignupModel : BasePageModel
    {
        public SignupModel(IOptions<SystemConfigurations> options, ILogger<SignupModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
        }

        [BindProperty] 
        public HealthFit_Web.Models.User UserDetails { get; set; }

        public string responseMessage { get; set; }
        public string responseCode { get; set; }
       
        public void OnGet()
        {
            ViewData["LoggedInUser"] = LoggedInUser;
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!TryValidateModel(UserDetails, nameof(UserDetails)))
            {
                responseMessage = ModelState.SelectMany(state => state.Value.Errors).Aggregate("", (current, error) => current + (error.ErrorMessage + ". " + Environment.NewLine));
                responseCode = "error";
                return Page();
            }
            
            string password = UserDetails.Password;
            string salt;
            string hashedPassword = PasswordHasher.HashPassword(password, out salt);
            UserDetails.UserDetails.HashedPassword = hashedPassword;
            UserDetails.UserDetails.PasswordSalt = salt;

            API_Connector.User userProxy = new API_Connector.User(this.GetAPIServerDetails());

            bool responce = userProxy.CreateUser(UserDetails.UserDetails);
            
            if (responce)
            {
                responseMessage = "Congradulations ! . You have been successfully sign up !!";
                responseCode = "success";
            }

            return Page();
        }
    }
}