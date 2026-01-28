using AA2_CS.Database;
using AA2_CS.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net; // Necesario para BCrypt

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

        // ... (Update y UpdateById se mantienen similares, ver nota abajo) ...

        public int Update(User entity)
        {
            var user = _context.Users.FirstOrDefault(u => u.id == entity.id);
            if (user != null)
            {
                user.name = entity.name;
                user.email = entity.email;
                
                // OPCIONAL: Solo actualiza la contraseña si ha cambiado y no parece ya un hash
                // Nota: Lo ideal es manejar el cambio de contraseña en un método separado.
                if (!string.IsNullOrEmpty(entity.passwordhash) && entity.passwordhash != user.passwordhash)
                {
                     user.passwordhash = BCrypt.Net.BCrypt.HashPassword(entity.passwordhash);
                }

                user.level = entity.level;
                user.strength = entity.strength;
                user.endurance = entity.endurance;
                user.consistencystreak = entity.consistencystreak;
                user.gold = entity.gold;

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

            // Lógica básica para actualizar password si viene uno nuevo en texto plano
            if (!string.IsNullOrEmpty(updatedUser.passwordhash) && updatedUser.passwordhash != user.passwordhash) 
            {
                 user.passwordhash = BCrypt.Net.BCrypt.HashPassword(updatedUser.passwordhash);
            }

            user.level = updatedUser.level;
            user.strength = updatedUser.strength;
            user.endurance = updatedUser.endurance;
            user.consistencystreak = updatedUser.consistencystreak;
            user.gold = updatedUser.gold;

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

        public List<UserDTO> FindAll()
        {
            return _context.Users.Select(user => new UserDTO
            {
                id = user.id,
                name = user.name,
                email = user.email,
                passwordhash = user.passwordhash,
                level = user.level,
                strength = user.strength,
                endurance = user.endurance,
                consistencystreak = user.consistencystreak,
                gold = user.gold
            }).ToList();
        }

        public User FindById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.id == id);
        }

        public List<User> FindByCharacteristic(string name)
        {
            return _context.Users
                .Where(u => EF.Functions.ILike(u.name, $"{name}%"))
                .ToList();
        }

        // LOGICA DE LOGIN CAMBIADA
        public User Login(string email, string plainPassword)
        {
            // 1. Buscar usuario solo por email
            var user = _context.Users.FirstOrDefault(u => u.email == email);

            // 2. Si no existe, retornar null
            if (user == null) return null;

            // 3. Verificar el hash
            // plainPassword: la contraseña escrita en el login (ej: "123456")
            // user.passwordhash: el hash guardado en la DB (ej: "$2a$11$Z...")
            bool isValid = BCrypt.Net.BCrypt.Verify(plainPassword, user.passwordhash);

            if (isValid)
            {
                return user;
            }
            
            return null;
        }

        public User Register(User user)
        {
            // ENCRIPTAR: Hasheamos la contraseña antes de guardar
            user.passwordhash = BCrypt.Net.BCrypt.HashPassword(user.passwordhash);

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public List<UserDTO> GetTopThreeUsers()
        {
            return _context.Users
                .OrderByDescending(u => u.level)
                .Take(3)
                .Select(user => new UserDTO
                {
                    id = user.id,
                    name = user.name,
                    email = user.email,
                    passwordhash = user.passwordhash,
                    level = user.level,
                    strength = user.strength,
                    endurance = user.endurance,
                    consistencystreak = user.consistencystreak,
                    gold = user.gold
                }).ToList();
        }
    }
}