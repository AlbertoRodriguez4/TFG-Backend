using System.Text.Json.Serialization;

namespace AA2_CS.Model
{
    public class UserRoom
    {
        public int userid { get; set; }

        // [JsonIgnore]  // Evitar la serialización del ciclo en la propiedad User
        public virtual User? User { get; set; }

        public int roomid { get; set; }
        public virtual Room? Room { get; set; }

        public UserRoom() { }

        public UserRoom(int userId, int roomId)
        {
            this.userid = userId;
            this.roomid = roomId;
        }
    }
}
