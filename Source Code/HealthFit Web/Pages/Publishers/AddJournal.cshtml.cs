using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace HealthFit_Web.Pages
{
    public class AddJournalModel : BasePageModel
    {
        private readonly ILogger<AddJournalModel> _logger;
        public string responseMessage { get; set; }
        public string responseCode { get; set; }

        [BindProperty]
        public Journal JournalVM { get; set; }

        public AddJournalModel(IOptions<SystemConfigurations> options, ILogger<AddJournalModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
        }

        public void OnGet()
        {

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            JournalVM.PublisherID = LoggedInUser.UserId;
            
            if (!TryValidateModel(JournalVM, nameof(JournalVM)))
            {
                return Page();
            }
            
            API_Connector.Journal journalProxy = new API_Connector.Journal(this.GetAPIServerDetails());

            bool responce = journalProxy.EditJournal(JournalVM);

            if (responce)
            {
                responseMessage = "Journal have been successfully saved!!";
                responseCode = "success";
            }

            return Page();
        }
    }
}