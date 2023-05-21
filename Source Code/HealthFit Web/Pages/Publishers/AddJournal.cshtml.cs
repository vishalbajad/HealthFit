using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthFit_Web.Pages
{
    public class AddJournalModel : PageModel
    {
        private readonly ILogger<AddJournalModel> _logger;

        public AddJournalModel(ILogger<AddJournalModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}