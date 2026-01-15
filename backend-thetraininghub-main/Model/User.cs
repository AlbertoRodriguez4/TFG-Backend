namespace AA2_CS.Model
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string passwordhash { get; set; } = string.Empty;
        public int level { get; set; } = 1;
        public int strength { get; set; } = 0;
        public int endurance { get; set; } = 0;
        public int consistencystreak { get; set; } = 0;
        public int gold { get; set; } = 0;
        public string role { get; set; } = string.Empty;
        public int experience { get; set; } = 0;

        // Relación con las salas a través de la tabla intermedia UserRoom
       

        public User() { }

        public User(string name, string email, string passwordHash, int level, int strength, int endurance, int consistencyStreak, int gold, string role, int experience)
        {
            this.name = name;
            this.email = email;
            this.passwordhash = passwordHash;
            this.level = level;
            this.strength = strength;
            this.endurance = endurance;
            this.consistencystreak = consistencyStreak;
            this.gold = gold;
            this.role = role;
            this.experience = experience;
        }
    }
}
