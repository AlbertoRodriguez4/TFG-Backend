using AA2_CS.Model;
using AA2_CS.Repository;

namespace AA2_CS.Service
{
    public class PlanService : IService<Plan>
    {
        private readonly PlanRepository _planRepository;

        public PlanService(PlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public int Add(Plan entity)
        {
            return _planRepository.Add(entity);
        }

        public int Delete(Plan entity)
        {
            return _planRepository.Delete(entity);
        }

        public List<Plan> FindAll()
        {
            return _planRepository.FindAll();
        }

        public Plan FindById(int id)
        {
            return _planRepository.FindById(id);
        }

        public List<Plan> FindByCharacteristic(string description)
        {
            return _planRepository.FindByCharacteristic(description);
        }

        public int Update(Plan entity)
        {
            return _planRepository.Update(entity);
        }
    }
}
