using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace HealthFit_Web.Pages.Publishers
{
    public class JournalListModel : BasePageModel
    {

        public JournalListModel(IOptions<SystemConfigurations> options, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
        }
        
        public void OnGet()
        {
        }
    }
}
