using Microsoft.AspNetCore.Mvc;
using HealthFit.Object_Provider.Model;
using Data_Layer.Services;
using Data_Layer.Repositories;
using Data_Layer.DBContext;
using Microsoft.Extensions.Options;
using HealthFit_APIs.Model;
using Object_Provider.Enum;
using HealthFit.AzureCompoenents;
using Microsoft.AspNetCore.Authorization;
using HealthFit.Utilities;
namespace HealthFit_APIs.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class JournalController : ControllerBase
    {
        private readonly ILogger<JournalController> _logger;
        private readonly HealthFitDbContext _dbContext;
        private readonly JournalRepository journalRepository;
        private readonly JournalService journalService;
        private readonly IWebHostEnvironment _environment;
        private readonly AppSettingsConfigurations appSettingsConfigurations;
        private readonly AzureFileUploadComponent _azureFileUploadComponent;

        public JournalController(IOptions<AppSettingsConfigurations> options, ILogger<JournalController> logger, IWebHostEnvironment environment)
        {
            appSettingsConfigurations = options.Value;
            _logger = logger;
            _dbContext = new HealthFitDbContext(appSettingsConfigurations.HealthFitDBConnectionString);
            journalRepository = new JournalRepository(_dbContext);
            journalService = new JournalService(journalRepository);
            _environment = environment;
            _azureFileUploadComponent = new AzureFileUploadComponent(appSettingsConfigurations.AzureBlobStoarageConnectionString, appSettingsConfigurations.BlobContainerName, appSettingsConfigurations.StorageSharedKeyCredential_AccountName, appSettingsConfigurations.StorageSharedKeyCredential_AccountKey);
        }

        /// <summary>
        /// Get All Jouranl List
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <param name="pdfByteData"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Journal>? GetAllJournal(UserType userType, int userId, bool active, bool pdfByteData = false)
        {
            _logger.Log(LogLevel.Information, "Start Method Executions");
            return journalService.GetAllJournals(userType, userId, active);
            //return journalService.GetAllJournals(userType, userId, active)?.Select(x => { x.JournalCoverPhotoPathByte = HealthFit.Utilities.FileOperationsUtility.ImageToBase64(appSettingsConfigurations.FileServerPath, x.JournalCoverPhotoPath, "Default.jpg"); return x; }).ToList();
        }

        /// <summary>
        /// Get Journal Details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pdfByteData"></param>
        /// <returns></returns>
        [HttpGet]
        public Journal? GetJournal(int id, bool pdfByteData = false)
        {
            _logger.Log(LogLevel.Information, "Start Method Executions GetJournal");
            Journal? journal = journalService.GetJournal(id);
            //if (journal != null && journal.JournalID > 0)
            //{
            //    journal.JournalCoverPhotoPathByte = HealthFit.Utilities.FileOperationsUtility.ImageToBase64(appSettingsConfigurations.FileServerPath, journal.JournalCoverPhotoPath, "Default.jpg");
            //    if (pdfByteData)
            //        journal.JournalPdfPathByte = HealthFit.Utilities.FileOperationsUtility.PdfToBytes(appSettingsConfigurations.FileServerPath, journal.JournalPdfPath, "Default.pdf");
            //}
            return journal;
        }

        /// <summary>
        /// Add or Save Journal Details
        /// </summary>
        /// <param name="journal"></param>
        /// <returns></returns>
        [HttpPost]
        public bool EditJournal(Journal journal)
        {
            _logger.Log(LogLevel.Information, "Start Method Executions EditJournal");
            return journalService.EditJournal(journal);
        }

        /// <summary>
        /// Delete Journal Records
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public bool DeleteJournal(int id)
        {
            _logger.Log(LogLevel.Information, "Start Method Executions DeleteJournal");
            return journalService.DeleteJournal(id);
        }

        /// <summary>
        /// Get All Category List
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [HttpGet]
        public List<string>? GetAllCategoryList(int publisherId = 0, bool active = true)
        {
            _logger.Log(LogLevel.Information, "Start Method Executions GetAllCategoryList");
            return journalService.GetAllCategoryList(publisherId, active);
        }

        /// <summary>
        /// Upload Journal Cover letter and File
        /// </summary>
        /// <param name="journalFileUpload"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadJournalCoverPhotoAndJournalFile([FromForm] JournalFileUpload journalFileUpload)
        {
            _logger.Log(LogLevel.Information, "Start Method Executions UploadJournalCoverPhotoAndJournalFile");
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
                        string uploadsFolder = appSettingsConfigurations.FileServerPath;
                        string uniqueFileName = FileOperationsUtility.SanitizeFileName(Guid.NewGuid().ToString() + "_" + coverPhotofile.FileName);

                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await coverPhotofile.CopyToAsync(fileStream);
                            _logger.Log(LogLevel.Debug, "Cover photo uploaded successfully");
                        }
                        coverPhotofileUploadedName = uniqueFileName;
                        coverPhotofileUploadStatus = true;
                        // string filepath = AzureFileUpload.UploadBlob(filePath);
                    }

                    if (JournalFile != null && JournalFile.Length > 0)
                    {
                        string uploadsFolder = appSettingsConfigurations.FileServerPath;
                        string uniqueFileName = FileOperationsUtility.SanitizeFileName(Guid.NewGuid().ToString() + "_" + JournalFile.FileName);

                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await JournalFile.CopyToAsync(fileStream);
                            _logger.Log(LogLevel.Debug, "Journal File uploaded successfully");
                        }

                        JournalFileUploadedName = uniqueFileName;
                        JournalFileUploadStatus = true;
                        // string filepath = AzureFileUpload.UploadBlob(filePath);
                    }
                    else
                        _logger.Log(LogLevel.Debug, "joruanl Deatils not found in UploadJournalCoverPhotoAndJournalFile");


                    if (coverPhotofileUploadStatus)
                        journal.JournalCoverPhotoPath = coverPhotofileUploadedName;

                    if (coverPhotofileUploadStatus || JournalFileUploadStatus)
                        journal.JournalPdfPath = JournalFileUploadedName;

                    if (coverPhotofileUploadStatus || JournalFileUploadStatus)
                        journalService.EditJournal(journal);

                    _logger.Log(LogLevel.Information, "Journal successfuly subscribed");

                    return Ok();
                }
                else
                {
                    _logger.Log(LogLevel.Debug, "Invalid Journal ID");
                    return StatusCode(500, $"Invalid Journal ID");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"Error Occured during file upload : {ex.InnerException.ToString()}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Copy file to temp path to access
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="journalId"></param>
        /// <returns></returns>
        [HttpGet]
        public string CopyJouranlToTempPath(int userId, int journalId)
        {
            _logger.Log(LogLevel.Information, "Start Method Executions copyJouranlToTempPath");
            try
            {
                Journal? journal = journalService.GetJournal(journalId);

                if (journal?.JournalID > 0)
                {
                    string tempFolderFilePath = Path.Join("Temp", Guid.NewGuid().ToString("N"));
                    string tempFullFolderFileDestinationPath = Path.Join(appSettingsConfigurations.FileServerPath, tempFolderFilePath);
                    string originalJournalFilePath = Path.Join(appSettingsConfigurations.FileServerPath, journal.JournalPdfPath);
                    string destinationJournalFilePath = Path.Join(tempFullFolderFileDestinationPath, journal.JournalPdfPath);
                    if (!Directory.Exists(tempFullFolderFileDestinationPath)) Directory.CreateDirectory(tempFullFolderFileDestinationPath);

                    System.IO.File.Copy(originalJournalFilePath, destinationJournalFilePath, true);
                    string newTempFilePath = Path.Join(tempFolderFilePath, journal.JournalPdfPath);
                    return newTempFilePath;
                }
                else
                {
                    _logger.Log(LogLevel.Debug, "Invalid Journal ID");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"Error Occured during file Copy to temp folder : {ex.InnerException.ToString()}");
            }
            return string.Empty;
        }
    }
}