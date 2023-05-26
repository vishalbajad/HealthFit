using Data_Layer.Repositories;
using HealthFit.Object_Provider.Model;
using HealthFit_Libs.InterfaceLibrary;
using Object_Provider.Enum;
using Data_Layer.InterfaceCollections.Service;
using Data_Layer.InterfaceCollections.Repository;

namespace Data_Layer.Services
{
    public class JournalService : IJournalService
    {
        private readonly IJournalRepository _journalRepository;
        /// <summary>
        /// Journal Service Contructor
        /// </summary>
        /// <param name="journalRepository"></param>
        public JournalService(IJournalRepository journalRepository)
        {
            _journalRepository = journalRepository;
        }
        /// <summary>
        /// Add Or Edit Journal Details
        /// </summary>
        /// <param name="journal"></param>
        /// <returns></returns>
        public bool EditJournal(Journal journal)
        {
            _journalRepository.EditJournal(journal);
            return true;
        }
        /// <summary>
        /// Delete Journal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteJournal(int id)
        {
            _journalRepository.DeleteJournal(id);
            return true;
        }
        /// <summary>
        /// Get Journal Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Journal? GetJournal(int id)
        {
            return _journalRepository.GetJournal(id).Result;
        }
        /// <summary>
        /// Get All Journals List
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public List<Journal>? GetAllJournals(UserType userType, int userId = 0, bool active = true)
        {
            return (List<Journal>?)_journalRepository.GetAllJournals(userType, userId, active).Result;
        }
        /// <summary>
        /// Get List of Top Selling journal list
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public List<Journal>? GetTopSellerJournals(UserType userType, int userId = 0, bool active = true)
        {
            return (List<Journal>?)_journalRepository.GetAllJournals(userType, userId, active).Result;
        }
        /// <summary>
        /// Get All Category List
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public List<string>? GetAllCategoryList(int publisherId = 0, bool active = true)
        {
            return (List<string>?)_journalRepository.GetAllCategoryList(publisherId, active).Result;
        }
    }
}
