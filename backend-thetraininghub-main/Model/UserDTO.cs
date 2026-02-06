namespace AA2_CS.Model
{
    public class UserDTO
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
        
    }
}