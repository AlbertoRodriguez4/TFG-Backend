namespace AA2_CS.Model
{
    public class Subscription
    {
        public int id { get; set; }
        public int userid { get; set; }
        public User? User { get; set; }
        public DateTime startDate { get; set; } = DateTime.UtcNow;
        public DateTime endDate { get; set; }
        public bool isActive { get; set; } = true;
        public string planType { get; set; } = "Premium";
        public decimal monthlyPrice { get; set; } = 10.00m; // Precio en euros (simulación)

        public Subscription() { }

        public Subscription(int userId, DateTime startDate, DateTime endDate, string planType = "Premium", decimal monthlyPrice = 10.00m)
        {
            this.userid = userId;
            this.startDate = startDate.Kind == DateTimeKind.Utc ? startDate : startDate.ToUniversalTime();
            this.endDate = endDate.Kind == DateTimeKind.Utc ? endDate : endDate.ToUniversalTime();
            this.planType = planType;
            this.monthlyPrice = monthlyPrice;
        }
    }
}
