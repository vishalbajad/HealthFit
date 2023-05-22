using Microsoft.AspNetCore.Mvc;
using HealthFit.Object_Provider.Model;
using Data_Layer.Services;
using Data_Layer.Repositories;
using Data_Layer.DBContext;
using HealthFit_Libs.InterfaceLibrary;

namespace HealthFit_APIs.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public bool CreateUser(User user)
        {
            UserContext userContext = new UserContext();
            UserRepository userRepository = new UserRepository(userContext);
            UserService userService = new UserService(userRepository);
            return userService.CreateUser(user);
        }
        [HttpGet]
        public List<User> GetUser()
        {
            UserContext userContext = new UserContext();
            UserRepository userRepository = new UserRepository(userContext);
            UserService userService = new UserService(userRepository);
            return userService.GetUsers();
        }
    }
}