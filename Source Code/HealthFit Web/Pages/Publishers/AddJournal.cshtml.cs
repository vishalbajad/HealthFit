using API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit.Utilities;
using HealthFit_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace HealthFit_Web.Pages
{
    public class AddJournalModel : BasePageModel
    {
        private readonly ILogger<AddJournalModel> _logger;
        private API_Connector.Journal journalProxy;
        public string responseMessage { get; set; }
        public string responseCode { get; set; }

        [BindProperty]
        public HealthFit.Object_Provider.Model.Journal JournalVM { get; set; }

        public AddJournalModel(IOptions<SystemConfigurations> options, ILogger<AddJournalModel> logger, IHttpContextAccessor httpContextAccessor) : base(options, logger, httpContextAccessor)
        {
            _logger = logger;
            journalProxy = new API_Connector.Journal(this.ApiServerDetails);
        }
        [ValidateAntiForgeryToken]
        public void OnGet(string journalId)
        {
            if (!string.IsNullOrWhiteSpace(journalId))
            {
                string decryptedJourID = EncryptionHelper.DecryptString(journalId);
                int jourId = 0;
                int.TryParse(decryptedJourID, out jourId);
                if (!(jourId > 0)) throw new Exception("Invalid Journal Id");
                JournalVM = journalProxy.GetJournal(jourId);
            }

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
            _logger.Log(LogLevel.Information, " Starting adding journal details");

            string[] allowedExtensionsForJournalCoverPhoto = { ".png", ".jpg", ".jpeg" };
            if (journalCoverPhoto != null && !IsFileExtensionAllowed(journalCoverPhoto.FileName, allowedExtensionsForJournalCoverPhoto))
            {
                ModelState.AddModelError("", "Only .png, .jpg, .jpeg files are allowed.");
                _logger.Log(LogLevel.Warning, "Cover photo extension valiation failed");
                return Page();
            }

            string[] allowedExtensionsForJournalDataFile = { ".pdf" };
            if (journalDataFile != null && !IsFileExtensionAllowed(journalDataFile.FileName, allowedExtensionsForJournalDataFile))
            {
                ModelState.AddModelError("", "Only PDF files are allowed.");
                _logger.Log(LogLevel.Warning, "Joural file extension valiation failed");
                return Page();
            }

            if (journalCoverPhoto?.Length >0)
            {
                var journalCoverPhotoFileSize = (journalCoverPhoto.Length / 1024f) / 1024f;
                if (journalCoverPhotoFileSize > 2) ModelState.AddModelError("", "Cover photo with Max 2 MB files are allowed.");
            }
            if (journalDataFile?.Length > 0)
            {
                var journalDataFileFileSize = (journalDataFile.Length / 1024f) / 1024f;
                if (journalDataFileFileSize > 5) ModelState.AddModelError("", "Journal with Max 5 MB files are allowed.");
            }

            JournalVM.PublisherID = LoggedInUser.UserId;

            ModelState.ClearValidationState(nameof(JournalVM));

            if (!TryValidateModel(JournalVM, nameof(JournalVM)))
            {
                return Page();
            }

            bool responce = journalProxy.EditJournal(JournalVM);
            _logger.Log(LogLevel.Information, "Joural saved successfuly");

            journalProxy.UploadJournalCoverPhotoAndJournalFile(new JournalFileUpload { JournalId = JournalVM.JournalID, CoverPhotofile = journalCoverPhoto, JournalFile = journalDataFile });

            _logger.Log(LogLevel.Information, "Joural Cover photo and files uploaded successfully !");

            responseMessage = "Journal have been successfully saved!!";
            responseCode = "success";

            return Page();
        }
    }
}