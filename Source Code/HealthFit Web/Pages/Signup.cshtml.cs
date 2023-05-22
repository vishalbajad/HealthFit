using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HealthFit_Web.Models;
using Microsoft.Extensions.Options;
using HealthFit.Object_Provider.Model;
using Microsoft.AspNetCore.Http;
namespace HealthFit_Web.Pages
{
    public class SignupModel : BasePageModel
    {
        public SignupModel(IOptions<SystemConfigurations> options, ILogger<IndexModel> logger) : base(options, logger)
        {
        }

        public string responseMessage { get; set; }
        public string responseCode { get; set; }
       
        public void OnGet()
        {

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(HealthFit_Web.Models.User UserDetails)
        {
            
            if (!TryValidateModel(UserDetails, nameof(UserDetails)))
            {
                responseMessage = ModelState.SelectMany(state => state.Value.Errors).Aggregate("", (current, error) => current + (error.ErrorMessage + ". " + Environment.NewLine));
                responseCode = "error";
                return Page();
            }
            
            API_Connector.User userProxy = new API_Connector.User(this.GetAPIServerDetails());
            HttpResponseMessage responce = userProxy.CreateUser(UserDetails.UserDetails);
            
            if (responce.IsSuccessStatusCode)
            {
                responseMessage = "Congradulations ! . You have been successfully sign up !!";
                responseCode = "success";
            }

            return Page();
        }
    }
}