using AA2_CS.Database;
using Microsoft.EntityFrameworkCore;

namespace AA2_CS.Repository
{
    public class TasksRepository : IRepository<Model.Task>
    {
        private readonly AppDbContext _context;

        public TasksRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Add(Model.Task entity)
        {
            _context.Tasks.Add(entity);
            _context.SaveChanges();
            return entity.id;
        }

        public int Delete(Model.Task entity)
        {
            _context.Tasks.Remove(entity);
            return _context.SaveChanges();
        }

        public List<Model.Task> FindAll()
        {
            return _context.Tasks.ToList();
        }

        public List<Model.Task> FindByCharacteristic(string value)
        {
            _context.Tasks.Include(t => t.userId).Where(t => t.name.Contains(value) || t.description.Contains(value)).ToList();
            return _context.Tasks.Where(t => t.name.Contains(value) || t.description.Contains(value)).ToList();
        }

        public Model.Task FindById(int id)
        {
            return _context.Tasks.FirstOrDefault(t => t.id == id);
        }


        public int Update(Model.Task entity)
        {
            _context.Tasks.Update(entity);
            return _context.SaveChanges();
        }
        public List<Model.Task> FindByUserId(int userId)
        {
            return _context.Tasks.Where(t => t.userId == userId).ToList();
        }
        public string CompleteTask(int taskId)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.id == taskId);
            if (task == null)
            {
                return "Task not found";
            }

            if (task.iscompleted)
            {
                return "Task is already completed";
            }

            var user = _context.Users.FirstOrDefault(u => u.id == task.userId);
            if (user == null)
            {
                return "User not found";
            }

            task.iscompleted = true;

            if (task.difficulty <= 2)
            {
                user.endurance += task.reward;
            }
            else
            {
                user.strength += task.reward;
            }

            user.level += task.reward / 100; 

            user.gold += task.reward;

            // Buscar última tarea completada por el usuario
            var lastCompleted = _context.Tasks
                .Where(t => t.userId == user.id && t.iscompleted && t.id != task.id)
                .OrderByDescending(t => t.createdat)
                .FirstOrDefault();

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
                    user.consistencystreak = 1; // Reiniciar la racha si pasó más de un día
                }
            }
            else
            {
                user.consistencystreak = 1; // Primera tarea completada
            }

            _context.SaveChanges();
            return "Task completed and rewards applied successfully";
        }
    }
}