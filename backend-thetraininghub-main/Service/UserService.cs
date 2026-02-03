using AA2_CS.Model;
using AA2_CS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AA2_CS.Service
{
    public class UserService // Si usas interfaz IService<User>, asegúrate de agregar AddExperience a la interfaz o dejarlo como método propio de la clase
    {
        private readonly UserRepository _repository;

        // CONSTANTES MOVIDAS AQUÍ (Lógica de negocio)
        private const int BASE_XP = 100;
        private const double EXPONENT = 1.5;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        // --- LÓGICA MOVIDA DEL REPOSITORIO AL SERVICIO ---

        public int GetXpRequiredForNextLevel(int currentLevel)
        {
            return (int)Math.Floor(BASE_XP * Math.Pow(currentLevel, EXPONENT));
        }

        public void AddExperience(int userId, int xpGained)
        {
            // 1. Pedimos el usuario al repo
            var user = _repository.FindById(userId);
            if (user == null) return;

            // 2. Calculamos (Lógica de negocio)
            user.experience += xpGained;

            bool leveledUp = false;
            int xpRequired = GetXpRequiredForNextLevel(user.level);

            while (user.experience >= xpRequired)
            {
                user.experience -= xpRequired;
                user.level++;
                leveledUp = true;

                // Recompensas
                user.gold += 50 * user.level;
                user.strength += 1;

                xpRequired = GetXpRequiredForNextLevel(user.level);
            }

            // 3. Guardamos los cambios usando el repo
            _repository.Update(user); 
        }

        // ---------------------------------------------------

        public int Add(User entity)
        {
            return _repository.Add(entity);
        }

        public int Update(User entity)
        {
            return _repository.Update(entity);
        }

        public Task<ActionResult<User>> UpdateById(int id, User updatedUser)
        {
            return _repository.UpdateById(id, updatedUser);
        }

        public int Delete(User entity)
        {
            return _repository.Delete(entity);
        }

        public List<UserDTO> FindAll()
        {
            return _repository.FindAll();
        }

        public User FindById(int id)
        {
            return _repository.FindById(id);
        }

        public List<User> FindByCharacteristic(string name)
        {
            return _repository.FindByCharacteristic(name);
        }

        public User Login(string email, string passwordhash)
        {
            return _repository.Login(email, passwordhash);
        }

        public User Register(User user)
        {
            return _repository.Register(user);
        }

        public List<UserDTO> GetTopThreeUsers()
        {
            return _repository.GetTopThreeUsers();
        }
    }
}