using Data_Layer.DBContext;
using HealthFit.Object_Provider.Model;
using Microsoft.EntityFrameworkCore;
using Object_Provider.Enum;
using Data_Layer.InterfaceCollections.Repository;

namespace Data_Layer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HealthFitDbContext _dbContext;
        /// <summary>
        /// User Reposity Contructor
        /// </summary>
        /// <param name="dbContext"></param>
        public UserRepository(HealthFitDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Get ALl user details
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User?>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }
        /// <summary>
        /// Get User Deatils
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User?> GetUser(int id)
        {
            User usr = _dbContext.Users.SingleOrDefault(u => u.UserId == id);
            if (usr != null)
            {
                List<int> journalid = await _dbContext.UserSubscriptionsDetails.Where(obj => obj.UserId == id).Select(x => x.JournalId).ToListAsync();
                usr.Journals = _dbContext.Journals.Where(obj => journalid.Contains(obj.JournalID)).ToList();
                return usr;
            }
            else
                return null;
        }
        /// <summary>
        /// Get User Details by User name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<User?> GetUserByUsername(string userName)
        {
            User? usr = _dbContext.Users.FirstOrDefault(obj => obj.UserName == userName);
            if (usr != null && usr.UserId > 0)
                usr = await GetUser(usr.UserId);
            return usr;
        }
        /// <summary>
        /// Add Or Update the User Details
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteUser(int id)
        {
            User? objuser = await GetUser(id);
            if (objuser?.UserId > 0)
            {
                _dbContext.Users.Remove(objuser);
                await _dbContext.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Get All Publishers List
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public async Task<IEnumerable<User?>> GetAllPublisherList(int publisherId, bool active)
        {
            if (publisherId == 0)
                return await _dbContext.Users.Where(obj => obj.IsActive == active && obj.UserType == (byte)UserType.Publisher)?.ToListAsync();
            else
                return await _dbContext.Users.Where(obj => obj.UserId == publisherId && obj.IsActive == active && obj.UserType == (byte)UserType.Publisher)?.ToListAsync();
        }
        /// <summary>
        /// Get ALl Public User List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public async Task<IEnumerable<User?>> GetAllPublicUserList(int userId, bool active)
        {
            if (userId == 0)
                return await _dbContext.Users.Where(obj => obj.IsActive == active && obj.UserType == (byte)UserType.PublicUser)?.ToListAsync();
            else
                return await _dbContext.Users.Where(obj => obj.UserId == userId && obj.IsActive == active && obj.UserType == (byte)UserType.PublicUser)?.ToListAsync();
        }
        /// <summary>
        /// Use to suscrive for journal
        /// </summary>
        /// <param name="user"></param>
        /// <param name="journal"></param>
        /// <returns></returns>
        public async Task SubscribeForJournal(User user, Journal journal)
        {
            var userSubscriptionsDetails = new UserSubscriptionsDetails
            {
                Users = user,
                Journals = journal,
                SubscriptionStartDate = DateTime.Now,
                SubscriptionEndDate = DateTime.Now.AddYears(1)
            };
            await _dbContext.AddAsync(userSubscriptionsDetails);
            await _dbContext.SaveChangesAsync();
        }
    }
}
