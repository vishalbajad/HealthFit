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
        public UserController(IOptions<AppSettingsConfigurations> options, ILogger<UserController> logger, IWebHostEnvironment environment)
        {
            appSettingsConfigurations = options.Value;
            _logger = logger;
            _dbContext = new HealthFitDbContext(appSettingsConfigurations.HealthFitDBConnectionString);
            userRepository = new UserRepository(_dbContext);
            userService = new UserService(userRepository);
            journalRepository = new JournalRepository(_dbContext);
            journalService = new JournalService(journalRepository);


        }

        [HttpGet]
        public List<User> GetAllUsers()
        {
            return userService.GetAllUsers();
        }

        [HttpGet]
        public User? GetUser(int id)
        {
            return userService.GetUser(id);
        }

        [HttpGet]
        public User? AunthenticateUser(string userName, string password)
        {
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
            }

            return null;
        }

        [HttpPost]
        public bool CreateUser(User user)
        {
            return userService.CreateUser(user);
        }

        [HttpGet]
        public bool DeleteUser(int id)
        {
            return userService.DeleteUser(id);
        }
        [HttpGet]
        public List<User>? GetAllPublisherList(int publisherId = 0, bool active = true)
        {
            return userService.GetAllPublisherList(publisherId, active);
        }
        [HttpGet]
        public List<User>? GetAllPublicUserList(int userId = 0, bool active = true)
        {
            return userService.GetAllPublicUserList(userId, active);
        }
        [HttpGet]
        public bool SubscribeForJournal(int userId, int journalId)
        {
            User? user = userService.GetUser(userId);
            Journal? journal = journalService.GetJournal(journalId);
            if (user?.UserId > 0 && journal?.JournalID > 0)
            {
                return userService.SubscribeForJournal(user, journal);
            }

            return false;

        }

    }
}