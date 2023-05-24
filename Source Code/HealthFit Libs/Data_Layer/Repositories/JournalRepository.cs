using Data_Layer.DBContext;
using HealthFit.Object_Provider.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Security.Policy;

namespace Data_Layer.Repositories
{
    public class JournalRepository
    {
        private readonly JournalContext _dbContext;

        public JournalRepository(JournalContext dbContext)
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
                    _dbContext.Journals.AddOrUpdate(journal);
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

        public List<Journal>? GetAllJournals(int publisherId = 0, bool active = true)
        {
            if (publisherId == 0)
                return _dbContext.Journals.Where(obj => obj.IsActive == active)?.ToList();
            else
                return _dbContext.Journals.Where(obj => obj.PublisherID == publisherId && obj.IsActive == active)?.ToList();
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
