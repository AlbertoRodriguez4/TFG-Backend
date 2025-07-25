using AA2_CS.Model;
using AA2_CS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AA2_CS.Service
{
    public class UserService : IService<User>
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public int Add(User entity)
        {
            return _repository.Add(entity);
        }

        public int Update(User entity)
        {
            return _repository.Update(entity);
        }

        public Task<ActionResult<User>> UpdateById(int id, User updatedUser)
        {
            return _repository.UpdateById(id, updatedUser);
        }

        public int Delete(User entity)
        {
            return _repository.Delete(entity);
        }

        public List<UserDTO> FindAll()
        {
            return _repository.FindAll();
        }

        List<User> IService<User>.FindAll()
        {
            throw new NotImplementedException();
        }

        public User FindById(int id)
        {
            return _repository.FindById(id);
        }

        public List<User> FindByCharacteristic(string name)
        {
            return _repository.FindByCharacteristic(name);
        }

        public User Login(string email, string passwordhash)
        {
            return _repository.Login(email, passwordhash);
        }

        public User Register(User user)
        {
            return _repository.Register(user);
        }

        public List<UserDTO> GetTopThreeUsers()
        {
            return _repository.GetTopThreeUsers();
        }
    }
}
