using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthFit_Web.Pages
{
    public class JournalDetailsModel : PageModel
    {
        private readonly ILogger<JournalDetailsModel> _logger;

        public JournalDetailsModel(ILogger<JournalDetailsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}