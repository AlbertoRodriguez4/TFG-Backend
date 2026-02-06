using AA2_CS.Database;
using AA2_CS.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Vital para usar .Include()
using BCrypt.Net; 

namespace AA2_CS.Repository
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Add(User entity)
        {
            // ENCRIPTAR: Hasheamos la contraseña antes de guardar
            entity.passwordhash = BCrypt.Net.BCrypt.HashPassword(entity.passwordhash);

            _context.Users.Add(entity);
            _context.SaveChanges();
            return entity.id;
        }

        public int Update(User entity)
        {
            var user = _context.Users.FirstOrDefault(u => u.id == entity.id);
            if (user != null)
            {
                user.name = entity.name;
                user.email = entity.email;
                
                if (!string.IsNullOrEmpty(entity.passwordhash) && entity.passwordhash != user.passwordhash)
                {
                     user.passwordhash = BCrypt.Net.BCrypt.HashPassword(entity.passwordhash);
                }

                user.level = entity.level;
                user.strength = entity.strength;
                user.endurance = entity.endurance;
                user.consistencystreak = entity.consistencystreak;
                user.gold = entity.gold;
                
                // Actualizar referencias de equipo
                user.equippedStrengthId = entity.equippedStrengthId;
                user.equippedEnduranceId = entity.equippedEnduranceId;

                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        public async Task<ActionResult<User>> UpdateById(int id, User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return new NotFoundObjectResult($"User with ID {id} not found.");

            user.name = updatedUser.name;
            user.email = updatedUser.email;

            if (!string.IsNullOrEmpty(updatedUser.passwordhash) && updatedUser.passwordhash != user.passwordhash) 
            {
                 user.passwordhash = BCrypt.Net.BCrypt.HashPassword(updatedUser.passwordhash);
            }

            user.level = updatedUser.level;
            user.strength = updatedUser.strength;
            user.endurance = updatedUser.endurance;
            user.consistencystreak = updatedUser.consistencystreak;
            user.gold = updatedUser.gold;
            
            // Actualizar referencias de equipo
            user.equippedStrengthId = updatedUser.equippedStrengthId;
            user.equippedEnduranceId = updatedUser.equippedEnduranceId;

            try
            {
                await _context.SaveChangesAsync();
                return new OkObjectResult(user);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Failed to update user: {ex.Message}");
            }
        }

        public int Delete(User entity)
        {
            var user = _context.Users.FirstOrDefault(u => u.id == entity.id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        // CAMBIADO: Devuelve List<User> en vez de DTO
        public List<User> FindAll()
        {
            return _context.Users.ToList();
        }

        public User FindById(int id)
        {
            // Recomendable añadir Includes aquí también si necesitas ver el equipo en el perfil individual
            return _context.Users
                .Include(u => u.EquippedStrengthItem)
                .Include(u => u.EquippedEnduranceItem)
                .FirstOrDefault(u => u.id == id);
        }

        public List<User> FindByCharacteristic(string name)
        {
            return _context.Users
                .Where(u => EF.Functions.ILike(u.name, $"{name}%"))
                .ToList();
        }

        public User Login(string email, string plainPassword)
        {
            // Añadimos Includes para que al loguearse el token pueda generarse con los items
            var user = _context.Users
                .Include(u => u.EquippedStrengthItem)
                .Include(u => u.EquippedEnduranceItem)
                .FirstOrDefault(u => u.email == email);

            if (user == null) return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(plainPassword, user.passwordhash);

            if (isValid)
            {
                return user;
            }
            
            return null;
        }

        public User Register(User user)
        {
            user.passwordhash = BCrypt.Net.BCrypt.HashPassword(user.passwordhash);
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        // --- CAMBIADO: Devuelve List<User> y usa .Include() ---
        public List<User> GetTopThreeUsers()
        {
            return _context.Users
                .Include(u => u.EquippedStrengthItem) // Carga el objeto de Fuerza
                .Include(u => u.EquippedEnduranceItem) // Carga el objeto de Resistencia
                .OrderByDescending(u => u.level)
                .Take(3)
                .ToList();
        }
    }
}