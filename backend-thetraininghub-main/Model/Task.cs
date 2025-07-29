namespace AA2_CS.Model
{
    public class Task
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int difficulty { get; set; }
        public int reward { get; set; }
        public bool iscompleted { get; set; } = false;
        public DateTime createdat { get; set; } = DateTime.Now;
        public int userId { get; set; } // Foreign key to User
         public Task() { }

        public Task(string name, string description, int difficulty, int reward, int userId)
        {
            this.name = name;
            this.description = description;
            this.difficulty = difficulty;
            this.reward = reward;
            this.userId = userId;
        }
    }

       
}