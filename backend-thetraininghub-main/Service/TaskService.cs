using AA2_CS.Repository;

namespace AA2_CS.Service
{
    public class TasksService : IService<Model.Task>
    {
        private readonly TasksRepository _repository;
        public TasksService(TasksRepository repository)
        {
            _repository = repository;
        }

        public int Add(Model.Task entity)
        {
            return _repository.Add(entity);
        }

        public int Delete(Model.Task entity)
        {
            return _repository.Delete(entity);
        }

        public List<Model.Task> FindAll()
        {
            return _repository.FindAll();
        }

        public List<Model.Task> FindByCharacteristic(string name)
        {
            return _repository.FindByCharacteristic(name);
        }

        public Model.Task FindById(int id)
        {
            return _repository.FindById(id);
        }

        public int Update(Model.Task entity)
        {
            return _repository.Update(entity);
        }
        public List<Model.Task> FindByUserId(int userId)
        {
            return _repository.FindByUserId(userId);
        }
        public string CompleteTask(int taskId)
        {
            return _repository.CompleteTask(taskId);
        }
        
    }
}