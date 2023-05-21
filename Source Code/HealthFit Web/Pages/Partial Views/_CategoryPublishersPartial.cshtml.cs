using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthFit_Web.Pages
{
    public class CategoryPublishersModel : PageModel
    {
        private readonly ILogger<CategoryPublishersModel> _logger;

        public CategoryPublishersModel(ILogger<CategoryPublishersModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}