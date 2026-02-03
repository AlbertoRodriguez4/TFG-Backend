using AA2_CS.Repository;

namespace AA2_CS.Service
{
    public class TasksService : IService<Model.Task>
    {
        private readonly TasksRepository _tasksRepository;
        private readonly UserRepository _userRepository;
        private readonly UserService _userService; // INYECCIÓN DEL USER SERVICE

        public TasksService(TasksRepository tasksRepository, UserRepository userRepository, UserService userService)
        {
            _tasksRepository = tasksRepository;
            _userRepository = userRepository;
            _userService = userService;
        }

        public int Add(Model.Task entity)
        {
            return _tasksRepository.Add(entity);
        }

        public int Delete(Model.Task entity)
        {
            return _tasksRepository.Delete(entity);
        }

        public List<Model.Task> FindAll()
        {
            return _tasksRepository.FindAll();
        }

        public List<Model.Task> FindByCharacteristic(string name)
        {
            return _tasksRepository.FindByCharacteristic(name);
        }

        public Model.Task FindById(int id)
        {
            return _tasksRepository.FindById(id);
        }

        public int Update(Model.Task entity)
        {
            return _tasksRepository.Update(entity);
        }
        public List<Model.Task> FindByUserId(int userId)
        {
            return _tasksRepository.FindByUserId(userId);
        }
        public string CompleteTask(int taskId)
        {
            var task = _tasksRepository.FindById(taskId);
            if (task == null) return "Task not found";
            if (task.iscompleted) return "Task is already completed";

            var user = _userRepository.FindById(task.userId);
            if (user == null) return "User not found";

            // 1. Marcar tarea como completada
            task.iscompleted = true;

            // 2. Dar recompensas de Stats (Lógica original)
            if (task.trainingfocus == "strength")
            {
                user.strength += task.reward / 10;
            }
            else if (task.trainingfocus == "endurance")
            {
                user.endurance += task.reward / 10;
            }
            else if (task.trainingfocus == "ambas")
            {
                user.strength += task.reward / 20;
                user.endurance += task.reward / 20;
            }

            // 3. Dar Oro
            user.gold += task.reward;

            // 4. Calcular Racha (Consistencia)
            var lastCompleted = _tasksRepository.GetLastCompletedTask(user.id, task.id);

            if (lastCompleted != null)
            {
                var previousDate = lastCompleted.createdat.Date;
                var currentDate = task.createdat.Date;

                if ((currentDate - previousDate).TotalDays == 1)
                {
                    user.consistencystreak += 1;
                }
                else if ((currentDate - previousDate).TotalDays > 1)
                {
                    user.consistencystreak = 1;
                }
            }
            else
            {
                user.consistencystreak = 1;
            }

            // 5. Guardar cambios de stats/oro/racha y tarea
            _tasksRepository.Update(task);
            _userRepository.Update(user);

            // 6. APLICAR EXPERIENCIA (Usando el nuevo sistema)
            // Aquí asumimos que la 'reward' de la tarea es la cantidad de XP que gana
            // O puedes definir una fórmula fija, ej: task.reward * 2
            _userService.AddExperience(user.id, task.reward);

            return "Task completed and rewards applied successfully";
        }
    }
}