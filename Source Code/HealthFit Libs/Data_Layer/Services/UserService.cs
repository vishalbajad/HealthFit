using Data_Layer.Repositories;
using HealthFit.Object_Provider.Model;

namespace Data_Layer.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetUsers()
        {
            return _userRepository.GetAllUsers();
        }
        public bool CreateUser(User user)
        {
            return _userRepository.CreateUser(user);
        }
    }
}
