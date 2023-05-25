using Microsoft.AspNetCore.Mvc;
using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.Extensions.Options;

namespace HealthFit_Web.Pages
{
    public class JournalDetailsModel : BasePageModel
    {
        private readonly ILogger<JournalDetailsModel> _logger;
        readonly API_Connector.Journal journalProxy;
        readonly API_Connector.User userlProxy;

        public JournalDetailsModel(IOptions<SystemConfigurations> options, ILogger<JournalDetailsModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            _logger = logger;
            journalProxy = new API_Connector.Journal(this.GetAPIServerDetails());
            userlProxy = new API_Connector.User(this.GetAPIServerDetails());
        }
        [BindProperty]
        public Journal JournalVM { get; set; }
        [BindProperty]
        public bool IsSubscribedForJournal { get; set; }

        [BindProperty]
        public string PublisherName { get; set; }

        public void OnGet(string journalId)
        {
            ViewData["LoggedInUser"] = LoggedInUser;
            if (!string.IsNullOrEmpty(journalId))
            {

                string decruptedjournalId = journalId; //  HealthFit.Utilities.EncryptionHelper.Decrypt(journalId);
                if (!string.IsNullOrEmpty(decruptedjournalId))
                {
                    int journalid = 0;
                    int.TryParse(decruptedjournalId, out journalid);
                    if (journalid > 0)
                    {
                        JournalVM = journalProxy.GetJournal(journalid);
                        PublisherName = userlProxy.GetAllPublisherList(JournalVM.PublisherID).SingleOrDefault().FullName;
                        IsSubscribedForJournal = JournalVM.PublisherID == LoggedInUser?.UserId;
                    }
                }
            }
        }
    }
}