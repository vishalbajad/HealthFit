using Data_Layer.Repositories;
using HealthFit.Object_Provider.Model;
using System.Security.Policy;

namespace Data_Layer.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User? GetUser(int id)
        {
            return _userRepository.GetUser(id);
        }

        public User? GetUserByUsername(string userName)
        {
            return _userRepository.GetUserByUsername(userName);
        }
        public bool CreateUser(User user)
        {
            _userRepository.CreateUser(user);
            return true;
        }
        public bool DeleteUser(int id)
        {
            return _userRepository.DeleteUser(id);
        }
        public List<User>? GetAllPublisherList(int publisherId = 0, bool active = true)
        {
            return _userRepository.GetAllPublisherList(publisherId, active);
        }
        public List<User>? GetAllPublicUserList(int userId = 0, bool active = true)
        {
            return _userRepository.GetAllPublicUserList(userId, active);
        }
    }
}
