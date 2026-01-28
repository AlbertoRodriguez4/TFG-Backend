using AA2_CS.Database;
using AA2_CS.Model;
using Microsoft.EntityFrameworkCore;

namespace AA2_CS.Repository
{
    public class RoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public int CreateRoomWithUser(Room entity, int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
                throw new Exception($"No se encontró el usuario con ID {userId}");
            _context.Rooms.Add(entity);
            _context.SaveChanges();

            var userRoom = new UserRoom
            {
                userid = userId,
                roomid = entity.id,
                User = user
            };

            _context.UserRooms.Add(userRoom);
            _context.SaveChanges();

            return entity.id;
        }

        public List<Room> FindAll()
        {
            return _context.Rooms.ToList();
        }

        public int Update(Room entity)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.id == entity.id);
            if (room != null)
            {
                room.name = entity.name;
                room.minlevel = entity.minlevel;
                room.minstats = entity.minstats;
                room.minconsistency = entity.minconsistency;
                

                room.description = entity.description;
                room.date = entity.date; 
                room.localization = entity.localization;

                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        public int Delete(Room entity)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.id == entity.id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        public List<UserRoomDTO> FindAllWithUsers()
        {
            return (from ur in _context.UserRooms
                    join u in _context.Users on ur.userid equals u.id
                    join r in _context.Rooms on ur.roomid equals r.id
                    // <--- IMPORTANTE: Añadir description y date al group by
                    group new { User = u, Room = r } by new { 
                        r.id, 
                        r.name, 
                        r.minlevel, 
                        r.minstats, 
                        r.minconsistency, 
                        r.description, 
                        r.date,
                        r.localization     
                    } into roomGroup
                    select new UserRoomDTO
                    {
                        room = new Room
                        {
                            id = roomGroup.Key.id,
                            name = roomGroup.Key.name,
                            minlevel = roomGroup.Key.minlevel,
                            minstats = roomGroup.Key.minstats,
                            minconsistency = roomGroup.Key.minconsistency,
                            description = roomGroup.Key.description, 
                            date = roomGroup.Key.date,
                            localization = roomGroup.Key.localization             
                        },
                        users = roomGroup.Select(g => new UserDTO
                        {
                            id = g.User.id,
                            name = g.User.name,
                            level = g.User.level,
                            strength = g.User.strength,
                            endurance = g.User.endurance
                        }).ToList()
                    }).ToList();
        }

        public List<Room> FindByCharacteristic(string name)
        {
            return _context.Rooms
                .Where(r => r.name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        public Room FindById(int id)
        {
            return _context.Rooms.FirstOrDefault(r => r.id == id);
        }

        public List<Room> SortByLevelAsc()
        {
            return _context.Rooms.OrderBy(r => r.minlevel).ToList();
        }

        public List<Room> SortByLevelDesc()
        {
            return _context.Rooms.OrderByDescending(r => r.minlevel).ToList();
        }

        public List<Room> SortByStatsAsc()
        {
            return _context.Rooms.OrderBy(r => r.minstats).ToList();
        }

        public List<Room> SortByStatsDesc()
        {
            return _context.Rooms.OrderByDescending(r => r.minstats).ToList();
        }
    }
}