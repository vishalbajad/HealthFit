using Data_Layer.DBContext;
using HealthFit.Object_Provider.Model;
using Microsoft.EntityFrameworkCore;
using Object_Provider.Enum;
using System.Collections.Generic;

namespace Data_Layer.Repositories
{
    public class JournalRepository
    {
        private readonly HealthFitDbContext _dbContext;

        public JournalRepository(HealthFitDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool EditJournal(Journal journal)
        {
            if (journal.JournalID > 0)
            {
                var entity = _dbContext.Journals.Find(journal.JournalID);
                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(journal);
                    _dbContext.SaveChanges();
                }
            }
            else if (journal.JournalID == 0)
            {
                _dbContext.Journals.Add(journal);
                _dbContext.SaveChanges();
            }
            return true;
        }

        public bool DeleteJournal(int id)
        {
            Journal? objJournal = GetJournal(id);
            if (objJournal?.JournalID > 0)
            {
                _dbContext.Journals.Remove(objJournal);
                _dbContext.SaveChanges();
            }
            return true;
        }

        public Journal? GetJournal(int id)
        {
            return _dbContext.Journals.FirstOrDefault(obj => obj.JournalID == id);
        }

        public List<Journal>? GetAllJournals(UserType userType, int userId = 0, bool active = true)
        {
            if (userId == 0) return _dbContext.Journals.Where(obj => obj.IsActive == active)?.ToList();

            if (userType == UserType.Publisher)
            {
                return _dbContext.Journals.Where(obj => obj.PublisherID == userId && obj.IsActive == active)?.ToList();
            }
            else if (userType == UserType.PublicUser)
            {
                User? usr = _dbContext.Users.SingleOrDefault(u => u.UserId == userId);
                if (usr != null)
                {
                    List<int> journalid = _dbContext.UserSubscriptionsDetails.Where(obj => obj.UserId == userId).Select(x => x.JournalId).ToList();
                    return  _dbContext.Journals.Where(obj => journalid.Contains(obj.JournalID)).ToList();
                }
                return null;
            }
            else
                return null;
        }

        public List<Journal>? GetTopSellerJournals(int publisherId = 0, bool active = true)
        {
            if (publisherId == 0)
                return _dbContext.Journals.Where(obj => obj.IsActive == active)?.ToList();
            else
                return _dbContext.Journals.Where(obj => obj.PublisherID == publisherId && obj.IsActive == active)?.ToList();
        }
        public List<string>? GetAllCategoryList(int publisherId = 0, bool active = true)
        {
            if (publisherId == 0)
                return _dbContext.Journals.Where(obj => obj.IsActive == active)?.Select(cat => cat.Category).ToList();
            else
                return _dbContext.Journals.Where(obj => obj.PublisherID == publisherId && obj.IsActive == active)?.Select(cat => cat.Category).ToList();
        }
    }
}
