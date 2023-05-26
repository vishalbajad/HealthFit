using HealthFit.Object_Provider.Model;

namespace Data_Layer.InterfaceCollections.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User?>> GetAllUsers();
        Task<User?> GetUser(int id);
        Task<User?> GetUserByUsername(string userName);
        Task CreateUser(User user);
        Task DeleteUser(int id);
        Task<IEnumerable<User?>> GetAllPublisherList(int publisherId = 0, bool active = true);
        Task<IEnumerable<User?>> GetAllPublicUserList(int userId = 0, bool active = true);
        Task SubscribeForJournal(User user, Journal journal);
    }
}
