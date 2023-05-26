using HealthFit.Object_Provider.Model;
using Data_Layer.InterfaceCollections.Service;
using Data_Layer.InterfaceCollections.Repository;
namespace Data_Layer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// User Service Constructor
        /// </summary>
        /// <param name="userRepository"></param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// Get List of ALl User
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers().Result.ToList();
        }
        /// <summary>
        /// Get User by User ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User? GetUser(int id)
        {
            return _userRepository.GetUser(id).Result;
        }
        /// <summary>
        /// Get User By User Name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User? GetUserByUsername(string userName)
        {
            return _userRepository.GetUserByUsername(userName).Result;
        }
        /// <summary>
        /// Add or Update the User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CreateUser(User user)
        {
            _userRepository.CreateUser(user);
            return true;
        }
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
            return true;
        }
        /// <summary>
        /// Get All Publishers Lists
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public List<User>? GetAllPublisherList(int publisherId = 0, bool active = true)
        {
            return (List<User>?)_userRepository.GetAllPublisherList(publisherId, active).Result;
        }
        /// <summary>
        /// Get All Public User List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public List<User>? GetAllPublicUserList(int userId = 0, bool active = true)
        {
            return (List<User>?)_userRepository.GetAllPublicUserList(userId, active).Result;
        }
        /// <summary>
        /// Subscibe for the journal
        /// </summary>
        /// <param name="user"></param>
        /// <param name="journal"></param>
        /// <returns></returns>
        public bool SubscribeForJournal(User user, Journal journal)
        {
            _userRepository.SubscribeForJournal(user, journal);
            return true;
        }
    }
}
