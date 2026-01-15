namespace AA2_CS.Service
{
    using AA2_CS.Model;
    using AA2_CS.Repository;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UserRoomService
    {
        private readonly UserRoomRepository _userRoomRepository;

        public UserRoomService(UserRoomRepository userRoomRepository)
        {
            _userRoomRepository = userRoomRepository;
        }

        public IEnumerable<UserRoom> GetAll()
        {
            return _userRoomRepository.GetAll();
        }

        // Nuevo: Buscar todas las salas de un usuario
        public IEnumerable<UserRoom> FindByUserId(int userId)
        {
            return _userRoomRepository.FindByUserId(userId);
        }

        // Nuevo: Buscar todos los usuarios de una sala
        public IEnumerable<UserRoomResponseDTO> FindUsersByRoomId(int roomId)
        {
            return _userRoomRepository.FindUsersByRoomId(roomId);
        }

        // Buscar una relación específica
        public UserRoom? FindByCompositeKey(int userId, int roomId)
        {
            return _userRoomRepository.FindByCompositeKey(userId, roomId);
        }

        public int Add(UserRoom userRoom)
        {
            return _userRoomRepository.Add(userRoom);
        }

        // Actualizado para recibir ambos IDs
        public async Task<UserRoom?> Update(int userId, int roomId, UserRoom updatedUserRoom)
        {
            return await _userRoomRepository.Update(userId, roomId, updatedUserRoom);
        }

        // Actualizado para recibir ambos IDs
        public int Delete(int userId, int roomId)
        {
            return _userRoomRepository.Delete(userId, roomId);
        }
    }
}