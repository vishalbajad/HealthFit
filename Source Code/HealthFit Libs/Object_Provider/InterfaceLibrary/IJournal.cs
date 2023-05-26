using HealthFit.Object_Provider.Model;
using Object_Provider.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthFit_Libs.InterfaceLibrary
{
    public interface IJournal : IBaseInterfact
    {
        HealthFit.Object_Provider.Model.Journal GetJournal(int id, bool pdfByteData = false);
        List<HealthFit.Object_Provider.Model.Journal> GetAllJournal(UserType userType, int userId = 0, bool active = true, bool pdfByteData = false);
        bool EditJournal(Journal journal);
        bool DeleteJournal(int id);
        List<string>? GetAllCategoryList(int publisherId = 0, bool active = true);
        string CopyJouranlToTempPath(int userId, int journalId);
    }
}
