using HealthFit.Object_Provider.Model;
using Object_Provider.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.InterfaceCollections.Repository
{
    public interface IJournalRepository
    {
        Task EditJournal(Journal journal);
        Task DeleteJournal(int id);
        Task<Journal?> GetJournal(int id);
        Task<IEnumerable<Journal?>> GetAllJournals(UserType userType, int userId = 0, bool active = true);
        Task<IEnumerable<Journal?>> GetTopSellerJournals(int publisherId = 0, bool active = true);
        Task<IEnumerable<string?>> GetAllCategoryList(int publisherId = 0, bool active = true);
    }
}
