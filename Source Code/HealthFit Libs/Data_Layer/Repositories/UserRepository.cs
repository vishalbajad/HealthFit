using Data_Layer.DBContext;
using HealthFit.Object_Provider.Model;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata.Ecma335;

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
            User usr = _dbContext.Users.SingleOrDefault(u => u.UserId == id);
            if (usr != null)
            {
                List<int> journalid = _dbContext.UserSubscriptionsDetails.Where(obj => obj.UserId == id).Select(x => x.JournalId).ToList();
                usr.Journals = _dbContext.Journals.Where(obj => journalid.Contains(obj.JournalID)).ToList();
                return usr;
            }
            else
                return null;
        }

        public User? GetUserByUsername(string userName)
        {
            User? usr = _dbContext.Users.FirstOrDefault(obj => obj.UserName == userName);
            if (usr != null && usr.UserId > 0)
                usr = GetUser(usr.UserId);
            return usr;
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
