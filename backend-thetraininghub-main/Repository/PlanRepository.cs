using AA2_CS.Database;
using AA2_CS.Model;
using AA2_CS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AA2_CS.Repository
{
    public class PlanRepository : IRepository<Plan>
    {
        private readonly AppDbContext _context;

        public PlanRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Add(Plan entity)
        {
            _context.Plans.Add(entity);
            _context.SaveChanges();
            return entity.id;
        }

        public int Delete(Plan entity)
        {
            var plan = _context.Plans.FirstOrDefault(p => p.id == entity.id);
            if (plan != null)
            {
                _context.Plans.Remove(plan);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        public List<Plan> FindAll()
        {
            return _context.Plans.ToList();
        }

        public Plan FindById(int id)
        {
            return _context.Plans.FirstOrDefault(p => p.id == id);
        }

        public List<Plan> FindByCharacteristic(string description)
        {
            return _context.Plans
                .Where(p => p.description.Contains(description, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public int Update(Plan entity)
        {
            var plan = _context.Plans.FirstOrDefault(p => p.id == entity.id);
            if (plan != null)
            {
                plan.description = entity.description;
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }
    }
}
