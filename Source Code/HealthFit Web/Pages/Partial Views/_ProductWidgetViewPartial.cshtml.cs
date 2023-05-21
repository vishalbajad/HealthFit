using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthFit_Web.Pages
{
    public class ProductWidgetViewModel : PageModel
    {
        private readonly ILogger<ProductWidgetViewModel> _logger;

        public ProductWidgetViewModel(ILogger<ProductWidgetViewModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}