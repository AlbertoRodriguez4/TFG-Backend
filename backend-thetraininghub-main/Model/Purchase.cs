namespace AA2_CS.Model
{
    public class Purchase
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int itemid { get; set; }
        public Item? Item { get; set; }
        public User? User { get; set; }
        public DateTime purchasedate { get; set; } = DateTime.UtcNow;

        

        public Purchase( int userId, User user, int itemId, Item item, DateTime purchaseDate)
        {
            this.userid = userId;
            this.itemid = itemId;
            this.purchasedate = purchaseDate.Kind == DateTimeKind.Utc
                ? purchaseDate
                : purchaseDate.ToUniversalTime(); //Cambiar el tipo de fecha para que la acepte

        }


        public Purchase() { }
    }
}
