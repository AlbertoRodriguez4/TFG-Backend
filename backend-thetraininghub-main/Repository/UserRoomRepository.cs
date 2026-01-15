namespace AA2_CS.Repository
{
    using AA2_CS.Database;
    using AA2_CS.Model;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserRoomRepository
    {
        private readonly AppDbContext _context;

        public UserRoomRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. Obtener TODOS los registros con la información del Usuario y la Sala
        public IEnumerable<UserRoom> GetAll()
        {
            return _context.UserRooms
                .Include(ur => ur.User) // Carga datos del Usuario
                .Include(ur => ur.Room) // Carga datos de la Sala
                .ToList();
        }

        // 2. Buscar por ID de USUARIO (Devuelve todas las salas de ese usuario)
        public IEnumerable<UserRoom> FindByUserId(int userId)
        {
            return _context.UserRooms
                .Include(ur => ur.User)
                .Include(ur => ur.Room)
                .Where(ur => ur.userid == userId)
                .ToList();
        }

        // 3. Buscar por ID de SALA (Devuelve todos los usuarios en esa sala)
        public IEnumerable<UserRoomResponseDTO> FindUsersByRoomId(int roomId)
        {
            return _context.UserRooms
                .Where(ur => ur.roomid == roomId) // Filtramos por la sala
                .Select(ur => new UserRoomResponseDTO(
                    ur.User.name,
                    ur.User.level,
                    ur.User.experience,
                    ur.User.strength,
                    ur.User.endurance,
                    ur.User.consistencystreak
                ))
                .ToList();
        }

        // 4. Buscar un registro específico (Necesitas AMBOS IDs por ser clave compuesta)
        public UserRoom? FindByCompositeKey(int userId, int roomId)
        {
            return _context.UserRooms
                .Include(ur => ur.User)
                .Include(ur => ur.Room)
                .FirstOrDefault(ur => ur.userid == userId && ur.roomid == roomId);
        }

        public int Add(UserRoom userRoom)
        {
            _context.UserRooms.Add(userRoom);
            return _context.SaveChanges();
        }

        // Para editar, necesitas identificar el registro original por sus dos claves
        public async Task<UserRoom?> Update(int userId, int roomId, UserRoom updatedUserRoom)
        {
            var existingUserRoom = await _context.UserRooms
                .FirstOrDefaultAsync(ur => ur.userid == userId && ur.roomid == roomId);

            if (existingUserRoom == null)
            {
                return null;
            }

            await _context.SaveChangesAsync();
            return existingUserRoom;
        }

        public int Delete(int userId, int roomId)
        {
            var userRoom = _context.UserRooms
                .FirstOrDefault(ur => ur.userid == userId && ur.roomid == roomId);

            if (userRoom != null)
            {
                _context.UserRooms.Remove(userRoom);
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}