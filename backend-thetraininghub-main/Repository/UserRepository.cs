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
                user.experience = entity.experience; // Opcional, pero buena práctica si cambia
                
                // Actualizar referencias de equipo
                user.equippedStrengthId = entity.equippedStrengthId;
                user.equippedEnduranceId = entity.equippedEnduranceId;
                
                // --- CAMBIO AÑADIDO: Guardar la nueva URL del avatar ---
                user.avatarUrl = entity.avatarUrl;

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
            user.experience = updatedUser.experience;
            
            // Actualizar referencias de equipo
            user.equippedStrengthId = updatedUser.equippedStrengthId;
            user.equippedEnduranceId = updatedUser.equippedEnduranceId;

            // --- CAMBIO AÑADIDO: Guardar la nueva URL del avatar ---
            user.avatarUrl = updatedUser.avatarUrl;

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

        public List<User> FindAll()
        {
            return _context.Users.ToList();
        }

        public User FindById(int id)
        {
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

        public List<User> GetTopThreeUsers()
        {
            return _context.Users
                .Include(u => u.EquippedStrengthItem)
                .Include(u => u.EquippedEnduranceItem)
                .OrderByDescending(u => u.level)
                .Take(3)
                .ToList();
        }
        public async Task<bool> ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                return false; // Usuario no encontrado

            // 1. Verificamos que la contraseña actual proporcionada coincida con el hash de la BD
            bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, user.passwordhash);
            
            if (!isCurrentPasswordValid)
                return false; // La contraseña actual no coincide

            // 2. Si es válida, hasheamos la nueva contraseña y la guardamos
            user.passwordhash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            try
            {
                await _context.SaveChangesAsync();
                return true; // Contraseña cambiada con éxito
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error si tienes un logger configurado
                Console.WriteLine($"Error al cambiar contraseña: {ex.Message}");
                return false;
            }
        }
    }
}