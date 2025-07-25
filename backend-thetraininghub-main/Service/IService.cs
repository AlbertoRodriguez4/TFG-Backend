using AA2_CS.Service;

namespace AA2_CS.Service
{
    public interface IService<T> where T : class
    {
        public int Add(T entity);
        public int Update(T entity);
        public int Delete(T entity);
        public List<T> FindAll();
        public T FindById(int id);
        public List<T> FindByCharacteristic(string name);
        
    }
}