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
            if (!IsFileExtensionAllowed(journalCoverPhoto.FileName, allowedExtensionsForJournalCoverPhoto))
            {
                ModelState.AddModelError("", "Only .png, .jpg, .jpeg files are allowed.");
                return Page();
            }

            string[] allowedExtensionsForJournalDataFile = { ".pdf" };
            if (!IsFileExtensionAllowed(journalDataFile.FileName, allowedExtensionsForJournalDataFile))
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

            var client1 = new HttpClient();
            client1.BaseAddress = new Uri("https://localhost:7035/Journal/");

            using var request1 = new HttpRequestMessage(HttpMethod.Post, "UploadJournalCoverPhotoAndJournalFile");
            using var content1 = new MultipartFormDataContent { { new StreamContent(journalCoverPhoto.OpenReadStream()), "file", journalCoverPhoto.FileName }, { new StreamContent(journalDataFile.OpenReadStream()), "file", journalDataFile.FileName } };
            request1.Content = content1;

            var status1 = client1.Send(request1);

            if (responce)
            {
                responseMessage = "Journal have been successfully saved!!";
                responseCode = "success";
            }

            return Page();
        }
        private static async Task UploadSampleFile(IFormFile journalCoverPhoto, IFormFile journalDataFile)
        {
           

        }
    }
}