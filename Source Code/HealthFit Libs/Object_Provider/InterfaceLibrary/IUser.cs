using HealthFit.Object_Provider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthFit_Libs.InterfaceLibrary
{
    public interface IUser : IBaseInterfact
    {
        bool CreateUser(User user);
        User? AunthenticateUser(string userName, string password);
        List<HealthFit.Object_Provider.Model.User>? GetAllPublisherList(int publisherId = 0, bool active = true);
        List<HealthFit.Object_Provider.Model.User>? GetAllPublicUserList(int userId = 0, bool active = true);
    }
}
