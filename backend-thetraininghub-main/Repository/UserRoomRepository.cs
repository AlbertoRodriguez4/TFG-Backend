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

        // 1. Obtener TODOS los registros
        public IEnumerable<UserRoom> GetAll()
        {
            return _context.UserRooms
                .Include(ur => ur.User)
                    .ThenInclude(u => u.EquippedStrengthItem)
                .Include(ur => ur.User)
                    .ThenInclude(u => u.EquippedEnduranceItem)
                .Include(ur => ur.Room)
                .ToList();
        }

        // 2. Buscar por ID de USUARIO
        public IEnumerable<UserRoom> FindByUserId(int userId)
        {
            return _context.UserRooms
                .Include(ur => ur.User)
                    .ThenInclude(u => u.EquippedStrengthItem)
                .Include(ur => ur.User)
                    .ThenInclude(u => u.EquippedEnduranceItem)
                .Include(ur => ur.Room)
                .Where(ur => ur.userid == userId)
                .ToList();
        }

        // 3. Buscar por ID de SALA (CORREGIDO)
        // Antes devolvía DTO, ahora devuelve UserRoom completo con los items cargados
        public IEnumerable<UserRoom> FindUsersByRoomId(int roomId)
        {
            return _context.UserRooms
                .Include(ur => ur.User)
                    .ThenInclude(u => u.EquippedStrengthItem) // Cargar item fuerza
                .Include(ur => ur.User)
                    .ThenInclude(u => u.EquippedEnduranceItem) // Cargar item resistencia
                .Include(ur => ur.Room) // Cargar datos de la sala
                .Where(ur => ur.roomid == roomId)
                .ToList();
        }

        // 4. Buscar un registro específico
        public UserRoom? FindByCompositeKey(int userId, int roomId)
        {
            return _context.UserRooms
                .Include(ur => ur.User)
                    .ThenInclude(u => u.EquippedStrengthItem)
                .Include(ur => ur.User)
                    .ThenInclude(u => u.EquippedEnduranceItem)
                .Include(ur => ur.Room)
                .FirstOrDefault(ur => ur.userid == userId && ur.roomid == roomId);
        }

        public int Add(UserRoom userRoom)
        {
            _context.UserRooms.Add(userRoom);
            return _context.SaveChanges();
        }

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