using AA2_CS.Database;
using AA2_CS.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                user.passwordhash = entity.passwordhash;
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
            user.passwordhash = updatedUser.passwordhash;
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

        public User Login(string email, string passwordhash)
        {
            return _context.Users.FirstOrDefault(u => u.email == email && u.passwordhash == passwordhash);
        }

        public User Register(User user)
        {
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
