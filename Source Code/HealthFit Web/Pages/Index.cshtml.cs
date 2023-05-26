using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using NUnit.Framework.Interfaces;
using User = HealthFit.Object_Provider.Model.User;

namespace HealthFit_Web.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        readonly API_Connector.Journal journalProxy;
        readonly API_Connector.User userlProxy;
        public IndexModel(IOptions<SystemConfigurations> options, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            _logger = logger;
            journalProxy = new API_Connector.Journal(this.ApiServerDetails);
            userlProxy = new API_Connector.User(this.ApiServerDetails);
        }

        [BindProperty]
        public List<Journal> JournalCollections { get; set; }


        [BindProperty]
        public List<string>? CategoryCollections { get; set; }

        [BindProperty]
        public List<User>? PublishersCollections { get; set; }

        [ValidateAntiForgeryToken]
        public void OnGet()
        {
            _logger.Log(LogLevel.Information, " Start Lodding the Dashboards");

            JournalCollections = journalProxy.GetAllJournal((Object_Provider.Enum.UserType)1, 0, true);
            CategoryCollections = journalProxy.GetAllCategoryList();
            PublishersCollections = userlProxy.GetAllPublisherList();

            if (LoggedInUser?.UserId > 0)
            {
                List<int>? ListJournalCollections = LoggedInUser.Journals.Select(obj => obj.JournalID)?.ToList();

                if (ListJournalCollections?.Count > 0)
                {
                    _logger.Log(LogLevel.Information, " Mapped user journal subscriptions details");
                    for (int index = 0; index < JournalCollections.Count(); index++)
                    {
                        if (ListJournalCollections.Contains(JournalCollections[index].JournalID))
                        {
                            if (JournalCollections[index].Subscribers == null)
                                JournalCollections[index].Subscribers = new List<User>();

                            JournalCollections[index].Subscribers.Add(LoggedInUser);
                        }
                    }
                }
            }

            if (JournalCollections == null) JournalCollections = new List<Journal>();
            if (CategoryCollections == null) CategoryCollections = new List<string>();
            if (PublishersCollections == null) PublishersCollections = new List<User>();

            ViewData["LoggedInUser"] = LoggedInUser;
        }
    }
}