namespace AA2_CS.Model
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string type { get; set; } // "Strength" o "Endurance"
        public int bonus { get; set; }
        public int price { get; set; }
        public string? imageUrl { get; set; } 

        public Item() { }

        public Item(int id, string name, string type, int bonus, int price, string? imageUrl = null)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.bonus = bonus;
            this.imageUrl = imageUrl;
            this.price = price;
        }
    }
}
