namespace AA2_CS.Model
{
    public class Room
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int minlevel { get; set; } = 1;
        public int minstats { get; set; } = 0;
        public int minconsistency { get; set; } = 0;


        public Room() { }

        public Room(int id, string name, int minLevel, int minStats, int minConsistency)
        {
            this.id = id;
            this.name = name;
            this.minlevel = minLevel;
            this.minstats = minStats;
            this.minconsistency = minConsistency;
        }
    }
}
