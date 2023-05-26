using HealthFit.Object_Provider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.InterfaceCollections.Service
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User? GetUser(int id);
        User? GetUserByUsername(string userName);
        bool CreateUser(User user);
        bool DeleteUser(int id);
        List<User>? GetAllPublisherList(int publisherId = 0, bool active = true);
        List<User>? GetAllPublicUserList(int userId = 0, bool active = true);
        bool SubscribeForJournal(User user, Journal journal);
    }
}