using AA2_CS.Model;
using AA2_CS.Repository;

namespace AA2_CS.Service
{
    public class RoomService : IService<Room>
    {
        private readonly RoomRepository _repository;

        public RoomService(RoomRepository repository)
        {
            _repository = repository;
        }

        public int Add(Room entity)
        {
            // Mantenemos la lógica de forzar el uso de CreateRoomWithUser
            throw new NotImplementedException("Use CreateRoomWithUser instead.");
        }

        public int CreateRoomWithUser(Room entity, int userId)
        {
            return _repository.CreateRoomWithUser(entity, userId);
        }

        public int Delete(Room entity)
        {
            return _repository.Delete(entity);
        }

        public List<Room> FindAll()
        {
            return _repository.FindAll();
        }

        public List<UserRoomDTO> FindAllWithUsers()
        {
            return _repository.FindAllWithUsers();
        }

        public Room FindById(int id)
        {
            return _repository.FindById(id);
        }

        public List<Room> FindByCharacteristic(string name)
        {
            return _repository.FindByCharacteristic(name);
        }

        public int Update(Room entity)
        {
            return _repository.Update(entity);
        }

        public List<Room> SortByLevelAsc()
        {
            return _repository.SortByLevelAsc();
        }

        public List<Room> SortByLevelDesc()
        {
            return _repository.SortByLevelDesc();
        }

        public List<Room> SortByStatsAsc()
        {
            return _repository.SortByStatsAsc();
        }

        public List<Room> SortByStatsDesc()
        {
            return _repository.SortByStatsDesc();
        }
    }
}