using HealthFit.Object_Provider.Model;
using Object_Provider.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.InterfaceCollections.Service
{
    public interface IJournalService
    {
        bool EditJournal(Journal journal);
        bool DeleteJournal(int id);
        Journal? GetJournal(int id);
        List<Journal>? GetAllJournals(UserType userType, int userId = 0, bool active = true);
        List<Journal>? GetTopSellerJournals(UserType userType, int userId = 0, bool active = true);
        List<string>? GetAllCategoryList(int publisherId = 0, bool active = true);
    }
}

