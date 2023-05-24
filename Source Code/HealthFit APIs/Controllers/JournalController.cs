using Microsoft.AspNetCore.Mvc;
using HealthFit.Object_Provider.Model;
using Data_Layer.Services;
using Data_Layer.Repositories;
using Data_Layer.DBContext;
using HealthFit_Libs.InterfaceLibrary;
using Microsoft.AspNetCore.Identity;
using HealthFit.Utilities;
using Microsoft.Extensions.Options;
using HealthFit_APIs.Model;

namespace HealthFit_APIs.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JournalController : ControllerBase
    {
        private readonly ILogger<JournalController> _logger;
        private readonly JournalContext journalContext;
        private readonly JournalRepository journalRepository;
        private readonly JournalService journalService;
        private readonly IWebHostEnvironment _environment;
        private readonly AppSettingsConfigurations appSettingsConfigurations;
        public JournalController(ILogger<JournalController> logger, IWebHostEnvironment environment, IOptions<AppSettingsConfigurations> options)
        {
            _logger = logger;
            journalContext = new JournalContext();
            journalRepository = new JournalRepository(journalContext);
            journalService = new JournalService(journalRepository);
            _environment = environment;
            appSettingsConfigurations = options.Value;
        }

        [HttpGet]
        public List<Journal>? GetAllJournal(int publisherId, bool active)
        {
            return journalService.GetAllJournals(publisherId, active);
        }

        [HttpGet]
        public Journal? GetJournal(int id)
        {
            return journalService.GetJournal(id);
        }

        [HttpPost]
        public bool EditJournal(Journal journal)
        {
            return journalService.EditJournal(journal);
        }

        [HttpGet]
        public bool DeleteJournal(int id)
        {
            return journalService.DeleteJournal(id);
        }

        [HttpGet]
        public List<string>? GetAllCategoryList(int publisherId = 0, bool active = true)
        {
            return journalService.GetAllCategoryList(publisherId, active);
        }


        [HttpPost]
        public async Task<IActionResult> UploadJournalCoverPhotoAndJournalFile([FromForm] JournalFileUpload journalFileUpload)
        {
            try
            {
                if (!(journalFileUpload.JournalId > 0))
                    return StatusCode(500, $"Invalid Journal ID");

                IFormFile JournalFile = journalFileUpload.JournalFile;
                IFormFile coverPhotofile = journalFileUpload.CoverPhotofile;
                Journal? journal = journalService.GetJournal(journalFileUpload.JournalId);

                if (journal?.JournalID > 0)
                {
                    bool coverPhotofileUploadStatus = false;
                    string coverPhotofileUploadedName = string.Empty;
                    bool JournalFileUploadStatus = false;
                    string JournalFileUploadedName = string.Empty;

                    if (coverPhotofile != null && coverPhotofile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(appSettingsConfigurations.FileServerPath, "Cover Photos");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + coverPhotofile.FileName;

                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await coverPhotofile.CopyToAsync(fileStream);
                        }
                        coverPhotofileUploadedName = uniqueFileName;
                        coverPhotofileUploadStatus = true;
                    }

                    if (JournalFile != null && JournalFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(appSettingsConfigurations.FileServerPath, "Journals");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + JournalFile.FileName;

                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await JournalFile.CopyToAsync(fileStream);
                        }
                        JournalFileUploadedName = uniqueFileName;
                        JournalFileUploadStatus = true;
                    }

                    if (coverPhotofileUploadStatus)
                        journal.JournalCoverPhotoPath = coverPhotofileUploadedName;

                    if (coverPhotofileUploadStatus || JournalFileUploadStatus)
                        journal.JournalPdfPath = JournalFileUploadedName;

                    if (coverPhotofileUploadStatus || JournalFileUploadStatus)
                        journalService.EditJournal(journal);

                    return Ok();
                }
                else
                    return StatusCode(500, $"Invalid Journal ID");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}