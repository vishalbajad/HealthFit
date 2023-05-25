
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthFit.Object_Provider.Model
{
    [Serializable]
    public class UserSubscriptionsDetails
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int JournalId { get; set; }

        public Journal Journals { get; set; }

        [Required]
        public int UserId { get; set; }

        public User Users { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime SubscriptionStartDate { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime SubscriptionEndDate { get; set; }

    }
}