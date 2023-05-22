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
        public bool CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
