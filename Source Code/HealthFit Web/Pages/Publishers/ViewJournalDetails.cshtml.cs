using Microsoft.AspNetCore.Mvc;
using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.Extensions.Options;
using API_Connector;

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
        public HealthFit.Object_Provider.Model.Journal JournalVM { get; set; }
        [BindProperty]
        public bool IsSubscribedForJournal { get; set; }

        [BindProperty]
        public string PublisherName { get; set; }

        public string responseMessage { get; set; }
        public string responseCode { get; set; }

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

                        IsSubscribedForJournal = (JournalVM.PublisherID == LoggedInUser?.UserId) || (LoggedInUser?.Journals?.Where(obj => obj.JournalID == JournalVM.JournalID).Count() > 0);
                    }
                }
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            API_Connector.User userProxy = new API_Connector.User(this.GetAPIServerDetails());

            if (LoggedInUser?.UserId > 0)
            {
                bool responce = userProxy.SubscribeForJournal(LoggedInUser.UserId, JournalVM.JournalID);

                if (responce)
                {
                    responseMessage = "Congradulations ! . You have been successfully Subscribed for the journal !!";
                    responseCode = "success";
                    
                    bool isSubscribed = LoggedInUser?.Journals?.Where(obj => obj.JournalID == JournalVM.JournalID).Count() > 0;
                    if (!isSubscribed)
                        LoggedInUser?.Journals.Add(JournalVM);
                }
            }
            else
            {
                responseMessage = "Please login to subscive for the journal !!";
                responseCode = "error";
            }

            return RedirectToPage("ViewJournalDetails", new { journalId = JournalVM.JournalID });
        }
    }
}