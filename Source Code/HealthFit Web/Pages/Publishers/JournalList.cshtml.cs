using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace HealthFit_Web.Pages.Publishers
{
    public class JournalListModel : BasePageModel
    {

        private readonly ILogger<JournalListModel> _logger;
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        readonly API_Connector.Journal journalProxy;

        [BindProperty]
        public int JournalIdToDelete { get; set; }

        [BindProperty]
        public List<Journal> JournalCollections { get; set; }
        public JournalListModel(IOptions<SystemConfigurations> options, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            journalProxy = new API_Connector.Journal(this.GetAPIServerDetails());
        }

        public void OnGet()
        {
            JournalCollections = journalProxy.GetAllJournal(LoggedInUser.UserId);
        }

        public void OnPostDelete()
        {
            if (JournalIdToDelete > 0)
            {
                journalProxy.DeleteJournal(JournalIdToDelete);
                RedirectToPage("JournalList");
            }
        }
    }
}