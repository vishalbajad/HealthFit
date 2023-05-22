using HealthFit.Object_Provider.Model;
using System.Data.Entity;

namespace Data_Layer.DBContext
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserContext(): base("HealthFitDatabase")
        {

        }
    }
}
