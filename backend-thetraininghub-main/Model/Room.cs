namespace AA2_CS.Model
{
    public class Room
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int minlevel { get; set; } = 1;
        public int minstats { get; set; } = 0;
        public int minconsistency { get; set; } = 0;
        public string description { get; set; } = string.Empty;
        public string date { get; set; } = string.Empty;
        public string localization { get; set; } = string.Empty;

        public Room() { }

        public Room(int id, string name, int minLevel, int minStats, int minConsistency, string description, string date, string localization)
        {
            this.id = id;
            this.name = name;
            this.minlevel = minLevel;
            this.minstats = minStats;
            this.minconsistency = minConsistency;
            this.description = description;
            this.date = date;
            this.localization = localization;
        }
    }
}
