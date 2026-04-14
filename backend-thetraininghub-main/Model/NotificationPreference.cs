using System.ComponentModel.DataAnnotations.Schema;

namespace AA2_CS.Model
{
    [Table("notificationpreferences")]
    public class NotificationPreference
    {
        public int id { get; set; }

        public int userid { get; set; }

        public bool inactivityEnabled { get; set; } = true;

        public int inactivityDays { get; set; } = 3;

        public bool roomsEnabled { get; set; } = true;

        public bool purchasesEnabled { get; set; } = true;

        public bool subscriptionExpiryEnabled { get; set; } = true;

        public DateTime createdat { get; set; } = DateTime.UtcNow;

        public DateTime updatedat { get; set; } = DateTime.UtcNow;

        public virtual User? User { get; set; }
    }
}