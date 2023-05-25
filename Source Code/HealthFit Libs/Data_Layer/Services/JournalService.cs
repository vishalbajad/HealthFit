using Data_Layer.Repositories;
using HealthFit.Object_Provider.Model;
using HealthFit_Libs.InterfaceLibrary;
using Object_Provider.Enum;

namespace Data_Layer.Services
{
    public class JournalService
    {
        private readonly JournalRepository _journalRepository;

        public JournalService(JournalRepository journalRepository)
        {
            _journalRepository = journalRepository;
        }

        public bool EditJournal(Journal journal)
        {
            return _journalRepository.EditJournal(journal);
        }

        public bool DeleteJournal(int id)
        {
            return _journalRepository.DeleteJournal(id);
        }

        public Journal? GetJournal(int id)
        {
            return _journalRepository.GetJournal(id);
        }

        public List<Journal>? GetAllJournals(UserType userType, int userId = 0, bool active = true)
        {
            return _journalRepository.GetAllJournals(userType, userId, active);
        }

        //public List<Journal>? GetTopSellerJournals(int publisherId = 0, bool active = true)
        //{
        //    return _journalRepository.GetAllJournals(publisherId, active);
        //}

        public List<string>? GetAllCategoryList(int publisherId = 0, bool active = true)
        {
            return _journalRepository.GetAllCategoryList(publisherId, active);
        }
    }
}
