namespace AA2_CS.Model
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string type { get; set; } // "Strength" o "Endurance"
        public int bonus { get; set; }
        public int price { get; set; }

        public Item() { }

        public Item(int id, string name, string type, int bonus, int price)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.bonus = bonus;
            this.price = price;
        }
    }
}
