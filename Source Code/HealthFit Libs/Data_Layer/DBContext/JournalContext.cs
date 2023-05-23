using HealthFit.Object_Provider.Model;
using System.Data.Entity;

namespace Data_Layer.DBContext
{
    public class JournalContext : DbContext
    {
        public DbSet<Journal> Journals { get; set; }
        public JournalContext() : base("HealthFitDatabase")
        {

        }
    }
}
