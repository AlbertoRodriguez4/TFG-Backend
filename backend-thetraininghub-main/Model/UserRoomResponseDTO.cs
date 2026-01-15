namespace AA2_CS.Model
{
    public class UserRoomResponseDTO
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; } 
        public int Strength { get; set; }
        public int Endurance { get; set; }
        public int ConsistencyStreak { get; set; }

        public UserRoomResponseDTO(string name, int level, int experience, int strength, int endurance, int consistencystreak)
        {
            Name = name;
            Level = level;
            Experience = experience;
            Strength = strength;
            Endurance = endurance;
            ConsistencyStreak = consistencystreak;
        }
    }
}