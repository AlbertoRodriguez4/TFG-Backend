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

        public IEnumerable<UserRoom> FindByUserId(int userId)
        {
            return _userRoomRepository.FindByUserId(userId);
        }

        // --- CAMBIO AQUÍ ---
        // Antes devolvía UserRoomResponseDTO, ahora UserRoom
        public IEnumerable<UserRoom> FindUsersByRoomId(int roomId)
        {
            return _userRoomRepository.FindUsersByRoomId(roomId);
        }

        public UserRoom? FindByCompositeKey(int userId, int roomId)
        {
            return _userRoomRepository.FindByCompositeKey(userId, roomId);
        }

        public int Add(UserRoom userRoom)
        {
            return _userRoomRepository.Add(userRoom);
        }

        public async Task<UserRoom?> Update(int userId, int roomId, UserRoom updatedUserRoom)
        {
            return await _userRoomRepository.Update(userId, roomId, updatedUserRoom);
        }

        public int Delete(int userId, int roomId)
        {
            return _userRoomRepository.Delete(userId, roomId);
        }
    }
}