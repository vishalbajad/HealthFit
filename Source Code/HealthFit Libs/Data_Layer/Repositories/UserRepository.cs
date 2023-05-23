using Data_Layer.DBContext;
using HealthFit.Object_Provider.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace Data_Layer.Repositories
{
    public class UserRepository
    {
        private readonly UserContext _dbContext;

        public UserRepository(UserContext dbContext)
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
    }
}
