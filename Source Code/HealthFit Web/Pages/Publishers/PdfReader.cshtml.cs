using API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text;

namespace HealthFit_Web.Pages
{
    public class PdfReaderModel : BasePageModel
    {
        private readonly ILogger<PdfReaderModel> _logger;
        readonly API_Connector.Journal journalProxy;
        readonly API_Connector.User userlProxy;
        [BindProperty]
        public HealthFit.Object_Provider.Model.Journal JournalVM { get; set; }
        public PdfReaderModel(IOptions<SystemConfigurations> options, ILogger<PdfReaderModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            _logger = logger;
            journalProxy = new API_Connector.Journal(this.GetAPIServerDetails());
        }

        public IActionResult OnGet(int journalId)
        {
            API_Connector.Journal journalProxy = new API_Connector.Journal(this.GetAPIServerDetails());
            JournalVM = journalProxy.GetJournal(journalId, true);
            var fileContent = JournalVM.JournalPdfPathByte;
            return File(fileContent, "application/pdf");
        }
    }
}
