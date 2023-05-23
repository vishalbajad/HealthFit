using Microsoft.AspNetCore.Mvc;
using HealthFit.Object_Provider.Model;
using Data_Layer.Services;
using Data_Layer.Repositories;
using Data_Layer.DBContext;
using HealthFit_Libs.InterfaceLibrary;
using Microsoft.AspNetCore.Identity;
using HealthFit.Utilities;

namespace HealthFit_APIs.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserContext userContext;
        private readonly UserRepository userRepository;
        private readonly UserService userService;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            userContext = new UserContext();
            userRepository = new UserRepository(userContext);
            userService = new UserService(userRepository);

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
                if(isPasswordValid) return objUser;
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

    }
}