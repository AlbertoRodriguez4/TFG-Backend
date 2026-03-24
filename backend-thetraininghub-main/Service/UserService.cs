using AA2_CS.Model;
using AA2_CS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AA2_CS.Service
{
    public class UserService 
    {
        private readonly UserRepository _repository;
        private readonly PurchaseRepository _purchaseRepository; 

        // CONSTANTES (Lógica de negocio)
        private const int BASE_XP = 100;
        private const double EXPONENT = 1.5;

        public UserService(UserRepository repository, PurchaseRepository purchaseRepository)
        {
            _repository = repository;
            _purchaseRepository = purchaseRepository;
        }

        // --- LÓGICA DE EXPERIENCIA ---

        public int GetXpRequiredForNextLevel(int currentLevel)
        {
            return (int)Math.Floor(BASE_XP * Math.Pow(currentLevel, EXPONENT));
        }

        public void AddExperience(int userId, int xpGained)
        {
            var user = _repository.FindById(userId);
            if (user == null) return;

            user.experience += xpGained;

            bool leveledUp = false;
            int xpRequired = GetXpRequiredForNextLevel(user.level);

            while (user.experience >= xpRequired)
            {
                user.experience -= xpRequired;
                user.level++;
                leveledUp = true;

                user.gold += 50 * user.level;
                user.strength += 1;

                xpRequired = GetXpRequiredForNextLevel(user.level);
            }

            _repository.Update(user); 
        }

        // --- MÉTODOS PARA EQUIPAR OBJETOS ---

        public string EquipItem(int userId, int itemId)
        {
            // A. Buscar al usuario
            var user = _repository.FindById(userId);
            if (user == null) return "User not found";
            
            // B. Verificar compras
            var purchases = _purchaseRepository.FindByUserId(userId);
            var purchase = purchases.FirstOrDefault(p => p.ItemId == itemId);

            if (purchase == null)
            {
                return "No posees este objeto, debes comprarlo primero.";
            }

            // C. Verificar Tipo
            // Usamos ToLower para evitar errores de mayúsculas/minúsculas
            string type = purchase.ItemType.ToLower();

            if (type == "strength" || type == "fuerza") 
            {
                user.equippedStrengthId = itemId;
            }
            else if (type == "endurance" || type == "resistencia")
            {
                user.equippedEnduranceId = itemId;
            }
            else
            {
                return "Este tipo de objeto no se puede equipar.";
            }

            _repository.Update(user);
            return "Item equipped successfully";
        }

        public string UnequipItem(int userId, string type)
        {
            var user = _repository.FindById(userId);
            if (user == null) return "User not found";

            string typeLower = type.ToLower();

            if (typeLower == "strength" || typeLower == "fuerza") 
            {
                user.equippedStrengthId = null;
            }
            else if (typeLower == "endurance" || typeLower == "resistencia") 
            {
                user.equippedEnduranceId = null;
            }
            else
            {
                return "Tipo de equipo inválido.";
            }

            _repository.Update(user);
            return "Item unequipped successfully";
        }

        // ---------------------------------------------------
        // MÉTODOS CRUD ESTÁNDAR 

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

        // --- CORRECCIÓN 1: Cambiado de UserDTO a User para coincidir con el Repo ---
        public List<User> FindAll()
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

        // --- CORRECCIÓN 2: Cambiado de UserDTO a User para coincidir con el Repo ---
        public List<User> GetTopThreeUsers()
        {
            return _repository.GetTopThreeUsers();
        }
        public async Task<bool> ChangePassword(int userId, string currentPassword, string newPassword)
        {
            return await _repository.ChangePassword(userId, currentPassword, newPassword);
        }
    }
}