namespace AA2_CS.Model
{
    public class Plan
    {
        public int id { get; set; }
        public int userid { get; set; }
        public string description { get; set; } = string.Empty;

        public Plan() { }

        public Plan(int id, int userId, string description)
        {
            this.id = id;
            this.userid = userId;
            this.description = description;
        }
    }
}
