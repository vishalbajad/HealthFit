using Data_Layer.DBContext;
using Data_Layer.InterfaceCollections.Repository;
using HealthFit.Object_Provider.Model;
using Microsoft.EntityFrameworkCore;
using Object_Provider.Enum;
namespace Data_Layer.Repositories
{
    public class JournalRepository : IJournalRepository
    {
        private readonly HealthFitDbContext _dbContext;
        /// <summary>
        /// Jouranl Repository Contructor
        /// </summary>
        /// <param name="dbContext"></param>
        public JournalRepository(HealthFitDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Add Or Edit Journal Details
        /// </summary>
        /// <param name="journal"></param>
        /// <returns></returns>
        public async Task EditJournal(Journal journal)
        {
            if (journal.JournalID > 0)
            {
                var entity = _dbContext.Journals.Find(journal.JournalID);
                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(journal);
                    await _dbContext.SaveChangesAsync();
                }
            }
            else if (journal.JournalID == 0)
            {
                await _dbContext.Journals.AddAsync(journal);
                await _dbContext.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Delete Journal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteJournal(int id)
        {
            Journal? objJournal = await GetJournal(id);
            if (objJournal?.JournalID > 0)
            {
                _dbContext.Journals.Remove(objJournal);
                await _dbContext.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Get Journal Details by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Journal?> GetJournal(int id)
        {
            return await _dbContext.Journals.FirstOrDefaultAsync(obj => obj.JournalID == id);
        }
        /// <summary>
        /// Get All Journal Details
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Journal?>> GetAllJournals(UserType userType, int userId = 0, bool active = true)
        {
            if (userId == 0) return await _dbContext.Journals.Where(obj => obj.IsActive == active)?.ToListAsync();

            if (userType == UserType.Publisher)
            {
                return await _dbContext.Journals.Where(obj => obj.PublisherID == userId && obj.IsActive == active)?.ToListAsync();
            }
            else if (userType == UserType.PublicUser)
            {
                User? usr = _dbContext.Users.SingleOrDefault(u => u.UserId == userId);
                if (usr != null)
                {
                    List<int> journalid = await _dbContext.UserSubscriptionsDetails.Where(obj => obj.UserId == userId).Select(x => x.JournalId).ToListAsync();
                    return await _dbContext.Journals.Where(obj => journalid.Contains(obj.JournalID)).ToListAsync();
                }
                return null;
            }
            else
                return null;
        }
        /// <summary>
        /// Get Top Setting Joural Details
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Journal?>> GetTopSellerJournals(int publisherId = 0, bool active = true)
        {
            if (publisherId == 0)
                return await _dbContext.Journals.Where(obj => obj.IsActive == active)?.ToListAsync();
            else
                return await _dbContext.Journals.Where(obj => obj.PublisherID == publisherId && obj.IsActive == active)?.ToListAsync();
        }
        /// <summary>
        /// Get All Category
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string?>> GetAllCategoryList(int publisherId = 0, bool active = true)
        {
            if (publisherId == 0)
                return await _dbContext.Journals.Where(obj => obj.IsActive == active)?.Select(cat => cat.Category).ToListAsync();
            else
                return await _dbContext.Journals.Where(obj => obj.PublisherID == publisherId && obj.IsActive == active)?.Select(cat => cat.Category).ToListAsync();
        }
    }
}
