namespace AA2_CS.Repository
{
    public interface IRepository<T> where T : class
    {
        int Add(T entity);
        int Update(T entity);
        int Delete(T entity);
        T FindById(int id);
        List<T> FindAll();
        List<T> FindByCharacteristic(string value);
    }
}
