using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Object_Provider.Enum;

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
        public JournalListModel(IOptions<SystemConfigurations> options, ILogger<JournalListModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            journalProxy = new API_Connector.Journal(this.ApiServerDetails);
            _logger = logger;
        }
        [ValidateAntiForgeryToken]
        public void OnGet()
        {
            ViewData["LoggedInUser"] = LoggedInUser;
            JournalCollections = journalProxy.GetAllJournal((Object_Provider.Enum.UserType)LoggedInUser.UserType, LoggedInUser.UserId);
        }
        [ValidateAntiForgeryToken]
        public void OnPost()
        {
            if (JournalIdToDelete > 0)
            {
                journalProxy.DeleteJournal(JournalIdToDelete);
                _logger.Log(LogLevel.Information, "Journal Deleted successfully");
                RedirectToPage("JournalList");
            }
            else
                _logger.Log(LogLevel.Information, " No Journal ID found for deletion");
        }
    }
}