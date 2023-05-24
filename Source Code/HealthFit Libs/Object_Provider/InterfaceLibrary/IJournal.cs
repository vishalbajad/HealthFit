using HealthFit.Object_Provider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthFit_Libs.InterfaceLibrary
{
    public interface IJournal : IBaseInterfact
    {
        HealthFit.Object_Provider.Model.Journal GetJournal(int id);
        List<HealthFit.Object_Provider.Model.Journal> GetAllJournal(int publisherId);
        bool EditJournal(Journal journal);
        bool DeleteJournal(int id);
        List<string>? GetAllCategoryList(int publisherId = 0, bool active = true);
    }
}
