using Microsoft.AspNetCore.Mvc;
using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.Extensions.Options;
using API_Connector;
using HealthFit.Utilities;

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
            journalProxy = new API_Connector.Journal(this.ApiServerDetails);
            userlProxy = new API_Connector.User(this.ApiServerDetails);
        }
        [BindProperty]
        public HealthFit.Object_Provider.Model.Journal JournalVM { get; set; }
        [BindProperty]
        public bool IsSubscribedForJournal { get; set; }

        [BindProperty]
        public string PublisherName { get; set; }

        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        [ValidateAntiForgeryToken]
        public void OnGet(string journalId)
        {
            string decryptedJourID = EncryptionHelper.DecryptString(journalId);
            int jourId = 0;
            int.TryParse(decryptedJourID, out jourId);
            if (!(jourId > 0)) throw new Exception("Invalid Journal Id");

            _logger.Log(LogLevel.Information, "Start GET Method execution for ViewJornal");

            ViewData["LoggedInUser"] = LoggedInUser;

            if (jourId > 0)
            {
                JournalVM = journalProxy.GetJournal(jourId);

                PublisherName = userlProxy.GetAllPublisherList(JournalVM.PublisherID).SingleOrDefault().FullName;

                IsSubscribedForJournal = (JournalVM.PublisherID == LoggedInUser?.UserId) || (LoggedInUser?.Journals?.Where(obj => obj.JournalID == JournalVM.JournalID).Count() > 0);

            }
            else
                _logger.Log(LogLevel.Information, "Journal Id invalid or empty");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            API_Connector.User userProxy = new API_Connector.User(this.ApiServerDetails);

            if (LoggedInUser?.UserId > 0)
            {
                bool responce = userProxy.SubscribeForJournal(LoggedInUser.UserId, JournalVM.JournalID);

                if (responce)
                {
                    responseMessage = "Congradulations ! . You have been successfully Subscribed for the journal !!";
                    responseCode = "success";

                    bool isSubscribed = LoggedInUser?.Journals?.Where(obj => obj.JournalID == JournalVM.JournalID).Count() > 0;

                    if (!isSubscribed)
                    {
                        HealthFit.Object_Provider.Model.User usr = LoggedInUser;
                        usr.Journals.Add(JournalVM);
                        LoggedInUser = usr;
                    }

                }
            }
            else
            {
                responseMessage = "Please login to subscive for the journal !!";
                responseCode = "error";
            }

            return RedirectToPage("ViewJournalDetails", new { journalId = EncryptionHelper.EncryptString(JournalVM.JournalID.ToString()) });
        }
    }
}