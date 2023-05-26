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
        readonly ILogger<SignupModel> _logger;

        public SignupModel(IOptions<SystemConfigurations> options, ILogger<SignupModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            _logger = logger;
        }

        [BindProperty]
        public HealthFit_Web.Models.User UserDetails { get; set; }

        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        [ValidateAntiForgeryToken]
        public void OnGet()
        {
            ViewData["LoggedInUser"] = LoggedInUser;
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.Log(LogLevel.Information, " Start Execution Signup ");
            if (!TryValidateModel(UserDetails, nameof(UserDetails)))
            {
                responseMessage = ModelState.SelectMany(state => state.Value.Errors).Aggregate("", (current, error) => current + (error.ErrorMessage + ". " + Environment.NewLine));
                responseCode = "error";
                _logger.Log(LogLevel.Information, " Model Validation failed");
                return Page();
            }

            string password = UserDetails.Password;
            string salt;
            string hashedPassword = PasswordHasher.HashPassword(password, out salt);
            UserDetails.UserDetails.HashedPassword = hashedPassword;
            UserDetails.UserDetails.PasswordSalt = salt;

            API_Connector.User userProxy = new API_Connector.User(this.GetAPIServerDetails());

            _logger.Log(LogLevel.Information, " Sending user to API for creation");

            bool responce = userProxy.CreateUser(UserDetails.UserDetails);


            if (responce)
            {
                _logger.Log(LogLevel.Information, " User successfully signed up");

                responseMessage = "Congradulations ! . You have been successfully sign up !!";
                responseCode = "success";
            }
            else
                _logger.Log(LogLevel.Error, " Failed to signup user");

            return Page();
        }
    }
}