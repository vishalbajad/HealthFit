using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace HealthFit_Web.Pages
{
    public class LoginModel : BasePageModel
    {
        public LoginModel(IOptions<SystemConfigurations> options, ILogger<IndexModel> logger) : base(options, logger)
        {

        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync([FromBody] string username, [FromBody] string password)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}