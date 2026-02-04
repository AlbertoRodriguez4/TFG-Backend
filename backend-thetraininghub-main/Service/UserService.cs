using AA2_CS.Model;
using AA2_CS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AA2_CS.Service
{
    public class UserService 
    {
        private readonly UserRepository _repository;
        // 1. AÑADIMOS EL REPOSITORIO DE COMPRAS
        private readonly PurchaseRepository _purchaseRepository; 

        // CONSTANTES (Lógica de negocio)
        private const int BASE_XP = 100;
        private const double EXPONENT = 1.5;

        // 2. ACTUALIZAMOS EL CONSTRUCTOR PARA INYECTAR PurchaseRepository
        public UserService(UserRepository repository, PurchaseRepository purchaseRepository)
        {
            _repository = repository;
            _purchaseRepository = purchaseRepository;
        }

        // --- LÓGICA DE EXPERIENCIA (YA ESTABA) ---

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

        // --- 3. NUEVOS MÉTODOS PARA EQUIPAR OBJETOS ---

        public string EquipItem(int userId, int itemId)
        {
            // A. Buscar al usuario
            var user = _repository.FindById(userId);
            if (user == null) return "User not found";
            Console.WriteLine($"Comprando item {itemId} para usuario {userId}");
            var purchases = _purchaseRepository.FindByUserId(userId);
            
            var purchase = purchases.FirstOrDefault(p => p.ItemId == itemId);

            if (purchase == null)
            {
                return "No posees este objeto, debes comprarlo primero.";
            }

            if (purchase.ItemType == "Strength" || purchase.ItemType == "Fuerza") 
            {
                user.equippedStrengthId = itemId;
            }
            else if (purchase.ItemType == "Endurance" || purchase.ItemType == "Resistencia")
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

            if (type == "Strength" || type == "Fuerza") 
            {
                user.equippedStrengthId = null; // Quitar objeto (null)
            }
            else if (type == "Endurance" || type == "Resistencia") 
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
        // MÉTODOS CRUD ESTÁNDAR (Sin cambios)

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