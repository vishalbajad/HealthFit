using API_Connector;
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
        public HealthFit.Object_Provider.Model.Journal JournalVM { get; set; }

        public AddJournalModel(IOptions<SystemConfigurations> options, ILogger<AddJournalModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {

        }

        public void OnGet(int id)
        {
            API_Connector.Journal journalProxy = new API_Connector.Journal(this.GetAPIServerDetails());
            JournalVM = journalProxy.GetJournal(id);
            ViewData["LoggedInUser"] = LoggedInUser;
        }

        private bool IsFileExtensionAllowed(string fileName, string[] allowedExtensions)
        {
            string fileExtension = Path.GetExtension(fileName);
            return allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(IFormFile journalCoverPhoto, IFormFile journalDataFile)
        {
            string[] allowedExtensionsForJournalCoverPhoto = { ".png", ".jpg", ".jpeg" };
            if (journalCoverPhoto!=null && !IsFileExtensionAllowed(journalCoverPhoto.FileName, allowedExtensionsForJournalCoverPhoto))
            {
                ModelState.AddModelError("", "Only .png, .jpg, .jpeg files are allowed.");
                return Page();
            }

            string[] allowedExtensionsForJournalDataFile = { ".pdf" };
            if (journalDataFile != null && !IsFileExtensionAllowed(journalDataFile.FileName, allowedExtensionsForJournalDataFile))
            {
                ModelState.AddModelError("", "Only PDF files are allowed.");
                return Page();
            }

            JournalVM.PublisherID = LoggedInUser.UserId;

            ModelState.ClearValidationState(nameof(JournalVM));

            if (!TryValidateModel(JournalVM, nameof(JournalVM)))
            {
                return Page();
            }

            API_Connector.Journal journalProxy = new API_Connector.Journal(this.GetAPIServerDetails());

            bool responce = journalProxy.EditJournal(JournalVM);

            journalProxy.UploadJournalCoverPhotoAndJournalFile(new JournalFileUpload { JournalId = JournalVM.JournalID, CoverPhotofile = journalCoverPhoto, JournalFile = journalDataFile });

            if (responce)
            {
                responseMessage = "Journal have been successfully saved!!";
                responseCode = "success";
            }

            return Page();
        }
    }
}