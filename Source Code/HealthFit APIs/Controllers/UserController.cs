using Microsoft.AspNetCore.Mvc;
using HealthFit.Object_Provider.Model;
using Data_Layer.Services;
using Data_Layer.Repositories;
using Data_Layer.DBContext;
using HealthFit_Libs.InterfaceLibrary;
using Microsoft.AspNetCore.Identity;
using HealthFit.Utilities;
using HealthFit_APIs.Model;
using Microsoft.Extensions.Options;
using HealthFit.AzureCompoenents;

namespace HealthFit_APIs.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly HealthFitDbContext _dbContext;
        private readonly UserRepository userRepository;
        private readonly UserService userService;
        private readonly JournalRepository journalRepository;
        private readonly JournalService journalService;
        private readonly AppSettingsConfigurations appSettingsConfigurations;
        private readonly AzureFileUploadComponent _azureFileUploadComponent;

        public UserController(IOptions<AppSettingsConfigurations> options, ILogger<UserController> logger, IWebHostEnvironment environment)
        {
            appSettingsConfigurations = options.Value;
            _logger = logger;
            _dbContext = new HealthFitDbContext(appSettingsConfigurations.HealthFitDBConnectionString);
            userRepository = new UserRepository(_dbContext);
            userService = new UserService(userRepository);
            journalRepository = new JournalRepository(_dbContext);
            journalService = new JournalService(journalRepository);
            _azureFileUploadComponent = new AzureFileUploadComponent(appSettingsConfigurations.AzureBlobStoarageConnectionString, appSettingsConfigurations.BlobContainerName, appSettingsConfigurations.StorageSharedKeyCredential_AccountName, appSettingsConfigurations.StorageSharedKeyCredential_AccountKey);
        }

        /// <summary>
        /// Get All User Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<User> GetAllUsers()
        {
            _logger.Log(LogLevel.Information, " Start Method Execution GetAllUsers ");
            return userService.GetAllUsers();
        }

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public User? GetUser(int id)
        {
            _logger.Log(LogLevel.Information, " Start Method Execution GetUser ");
            return userService.GetUser(id);
        }
        /// <summary>
        /// Authenticate use by API
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public User? AunthenticateUser(string userName, string password)
        {
            _logger.Log(LogLevel.Information, " Start Method Execution AunthenticateUser ");
            string passwordSalt = string.Empty;

            User? objUser = userService.GetUserByUsername(userName);

            if (objUser?.UserId > 0)
            {
                bool isPasswordValid = HealthFit.Utilities.PasswordHasher.VerifyPassword(password, objUser.PasswordSalt, objUser.HashedPassword);
                if (isPasswordValid)
                {
                    objUser.PasswordSalt = string.Empty;
                    objUser.HashedPassword = string.Empty;
                    return objUser;
                }
                else
                    _logger.Log(LogLevel.Debug, " User Passoword validation failed");
            }
            else
            {
                _logger.Log(LogLevel.Debug, " User Not found ");
            }

            return null;
        }

        /// <summary>
        /// Add or Update User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public bool CreateUser(User user)
        {
            _logger.Log(LogLevel.Information, "Start Method Execution CreateUser");
            return userService.CreateUser(user);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public bool DeleteUser(int id)
        {
            _logger.Log(LogLevel.Information, "Start Method Execution DeleteUser");
            return userService.DeleteUser(id);
        }
        /// <summary>
        /// Get All Publisher List
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [HttpGet]
        public List<User>? GetAllPublisherList(int publisherId = 0, bool active = true)
        {
            _logger.Log(LogLevel.Information, "Start Method Execution GetAllPublisherList");
            return userService.GetAllPublisherList(publisherId, active);
        }

        /// <summary>
        /// Get ALl Public User List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [HttpGet]
        public List<User>? GetAllPublicUserList(int userId = 0, bool active = true)
        {
            _logger.Log(LogLevel.Information, "Start Method Execution GetAllPublicUserList");
            return userService.GetAllPublicUserList(userId, active);
        }

        /// <summary>
        /// Subscribe user for journal
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="journalId"></param>
        /// <returns></returns>
        [HttpGet]
        public bool SubscribeForJournal(int userId, int journalId)
        {
            _logger.Log(LogLevel.Information, "Start Method Execution SubscribeForJournal");
            User? user = userService.GetUser(userId);
            Journal? journal = journalService.GetJournal(journalId);
            if (user?.UserId > 0 && journal?.JournalID > 0)
            {
                return userService.SubscribeForJournal(user, journal);
            }
            else
                _logger.Log(LogLevel.Debug, "Journal Details not found !");

            return false;

        }

    }
}