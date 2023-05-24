using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
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
            journalProxy = new API_Connector.Journal(this.GetAPIServerDetails());
            userlProxy = new API_Connector.User(this.GetAPIServerDetails());
        }

        [BindProperty]
        public List<Journal> JournalCollections { get; set; }

        [BindProperty]
        public List<string>? CategoryCollections { get; set; }

        [BindProperty]
        public List<User>? PublishersCollections { get; set; }
        public void OnGet(int? logout)
        {
            if (logout.HasValue && logout.Value == 1)
            {
                LoggedInUser = new User();
            }

            if (LoggedInUser?.UserId > 0)
            {
                JournalCollections = journalProxy.GetAllJournal(LoggedInUser.UserId);
                CategoryCollections = journalProxy.GetAllCategoryList(LoggedInUser.UserId);
                PublishersCollections = userlProxy.GetAllPublisherList(LoggedInUser.UserId);
            }
            else
            {
                JournalCollections = journalProxy.GetAllJournal();
                CategoryCollections = journalProxy.GetAllCategoryList();
                PublishersCollections = userlProxy.GetAllPublisherList();
            }

            if (JournalCollections == null) JournalCollections = new List<Journal>();
            if (CategoryCollections == null) CategoryCollections = new List<string>();
            if (PublishersCollections == null) PublishersCollections = new List<User>();
        }
    }
}