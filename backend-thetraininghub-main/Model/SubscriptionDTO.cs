namespace AA2_CS.Model
{
    public class SubscriptionDTO
    {
        public int SubscriptionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string PlanType { get; set; } = string.Empty;
        public decimal MonthlyPrice { get; set; }
        public DateTime? NextRenewalDate { get; set; }
    }
}
