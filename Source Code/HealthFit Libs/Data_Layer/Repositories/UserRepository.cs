using Data_Layer.DBContext;
using HealthFit.Object_Provider.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Net;

namespace Data_Layer.Repositories
{
    public class UserRepository
    {
        private readonly HealthFitDbContext _dbContext;

        public UserRepository(HealthFitDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public User? GetUser(int id)
        {
            return _dbContext.Users.FirstOrDefault(obj => obj.UserId == id);
        }

        public User? GetUserByUsername(string userName)
        {
            return _dbContext.Users.FirstOrDefault(obj => obj.UserName == userName);
        }

        public bool CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return true;
        }

        public bool DeleteUser(int id)
        {
            User? objuser = GetUser(id);
            if (objuser?.UserId > 0)
            {
                _dbContext.Users.Remove(objuser);
                _dbContext.SaveChanges();
            }
            return true;
        }

        public List<User>? GetAllPublisherList(int publisherId = 0, bool active = true)
        {
            if (publisherId == 0)
                return _dbContext.Users.Where(obj => obj.IsActive == active && obj.UserType == 2)?.ToList();
            else
                return _dbContext.Users.Where(obj => obj.UserId == publisherId && obj.IsActive == active && obj.UserType == 2)?.ToList();
        }
        public List<User>? GetAllPublicUserList(int userId = 0, bool active = true)
        {
            if (userId == 0)
                return _dbContext.Users.Where(obj => obj.IsActive == active && obj.UserType == 1)?.ToList();
            else
                return _dbContext.Users.Where(obj => obj.UserId == userId && obj.IsActive == active && obj.UserType == 1)?.ToList();
        }

        public bool SubscribeForJournal(User user, Journal journal)
        {
            var userSubscriptionsDetails = new UserSubscriptionsDetails
            {
                Users = user,
                Journals = journal,
                SubscriptionStartDate = DateTime.Now,
                SubscriptionEndDate = DateTime.Now.AddYears(1)
            };
            _dbContext.Add(userSubscriptionsDetails);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
