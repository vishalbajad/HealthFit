using API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Text;

namespace HealthFit_Web.Pages
{
    public class PdfViewerModel : BasePageModel
    {
        private readonly ILogger<PdfViewerModel> _logger;
        readonly API_Connector.Journal journalProxy;
        readonly API_Connector.User userlProxy;
        [BindProperty]
        public HealthFit.Object_Provider.Model.Journal JournalVM { get; set; }
        public PdfViewerModel(IOptions<SystemConfigurations> options, ILogger<PdfViewerModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            _logger = logger;
            journalProxy = new API_Connector.Journal(this.GetAPIServerDetails());
        }

        public void OnGet(int journalId)
        {
            JournalVM = journalProxy.GetJournal(journalId);
        }
    }
}
