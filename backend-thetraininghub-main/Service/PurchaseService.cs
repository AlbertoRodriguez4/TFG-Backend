using AA2_CS.Model;
using AA2_CS.Repository;
// Asegúrate de importar el namespace de tus DTOs si está separado
// using AA2_CS.DTOs; 

namespace AA2_CS.Service
{
    public class PurchaseService : IService<Purchase>
    {
        private readonly PurchaseRepository _repository;

        public PurchaseService(PurchaseRepository repository)
        {
            _repository = repository;
        }

        public int Add(Purchase entity)
        {
            return _repository.Add(entity);
        }

        public int Delete(Purchase entity)
        {
            return _repository.Delete(entity);
        }

        public List<Purchase> FindAll()
        {
            return _repository.FindAll();
        }

        public Purchase FindById(int id)
        {
            return _repository.FindById(id);
        }

        public List<Purchase> FindByCharacteristic(string name)
        {
            return _repository.FindByCharacteristic(name);
        }

        public int Update(Purchase entity)
        {
            return _repository.Update(entity);
        }

        // --- MÉTODOS ELIMINADOS/MODIFICADOS ---

        // 1. ELIMINADO: FindByUser(email, password) ya no existe por seguridad.

        // 2. MODIFICADO: Ahora devuelve PurchaseDTO para que el frontend reciba 
        // los datos completos (Nombre del ítem, Precio, etc.)
        public List<PurchaseDTO> FindByUserId(int userId)
        {
            return _repository.FindByUserId(userId);
        }
    }
}