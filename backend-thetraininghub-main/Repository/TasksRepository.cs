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

        // HE ELIMINADO CompleteTask DE AQUÍ. AHORA IRÁ EN EL SERVICIO.
        
        // Método auxiliar para obtener la última completada (lo usaremos en el servicio)
        public Model.Task GetLastCompletedTask(int userId, int currentTaskId)
        {
            return _context.Tasks
                .Where(t => t.userId == userId && t.iscompleted && t.id != currentTaskId)
                .OrderByDescending(t => t.createdat)
                .FirstOrDefault();
        }

        // Devuelve la fecha de la última tarea completada por el usuario (o null si no hay)
        public DateTime? GetLastCompletedTaskDate(int userId)
        {
            return _context.Tasks
                .Where(t => t.userId == userId && t.iscompleted)
                .OrderByDescending(t => t.createdat)
                .Select(t => (DateTime?)t.createdat)
                .FirstOrDefault();
        }
    }
}