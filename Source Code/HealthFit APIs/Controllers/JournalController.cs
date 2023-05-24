using Microsoft.AspNetCore.Mvc;
using HealthFit.Object_Provider.Model;
using Data_Layer.Services;
using Data_Layer.Repositories;
using Data_Layer.DBContext;
using HealthFit_Libs.InterfaceLibrary;
using Microsoft.AspNetCore.Identity;
using HealthFit.Utilities;

namespace HealthFit_APIs.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JournalController : ControllerBase
    {
        private readonly ILogger<JournalController> _logger;
        private readonly JournalContext journalContext;
        private readonly JournalRepository journalRepository;
        private readonly JournalService journalService;
        public JournalController(ILogger<JournalController> logger)
        {
            _logger = logger;
            journalContext = new JournalContext();
            journalRepository = new JournalRepository(journalContext);
            journalService = new JournalService(journalRepository);
        }

        [HttpGet]
        public List<Journal>? GetAllJournal(int publisherId, bool active)
        {
            return journalService.GetAllJournals(publisherId, active);
        }

        [HttpGet]
        public Journal? GetJournal(int id)
        {
            return journalService.GetJournal(id);
        }

        [HttpPost]
        public bool EditJournal(Journal journal)
        {
            return journalService.EditJournal(journal);
        }

        [HttpGet]
        public bool DeleteJournal(int id)
        {
            return journalService.DeleteJournal(id);
        }

        [HttpGet]
        public List<string>? GetAllCategoryList(int publisherId = 0, bool active = true)
        {
            return journalService.GetAllCategoryList(publisherId, active);
        }


    }
}