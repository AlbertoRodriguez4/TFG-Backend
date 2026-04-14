namespace AA2_CS.Model
{
    public class NotificationPreferenceDTO
    {
        public int UserId { get; set; }
        public bool InactivityEnabled { get; set; } = true;
        public int InactivityDays { get; set; } = 3;
        public bool RoomsEnabled { get; set; } = true;
        public bool PurchasesEnabled { get; set; } = true;
        public bool SubscriptionExpiryEnabled { get; set; } = true;
    }
}