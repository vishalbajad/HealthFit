using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HealthFit_Web.Pages
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(IOptions<SystemConfigurations> options, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
        }

        public void OnGet()
        {

        }
    }
}