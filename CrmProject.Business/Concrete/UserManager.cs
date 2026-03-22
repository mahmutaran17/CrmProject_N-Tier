using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IGenericRepository<User> _repository;
        public UserManager(IGenericRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<User?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<List<User>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<List<User>> GetWhereAsync(Expression<Func<User, bool>> predicate) => await _repository.GetWhereAsync(predicate);
        public async Task AddAsync(User entity) => await _repository.AddAsync(entity);
        public void Update(User entity) => _repository.Update(entity);
        public void Delete(User entity) => _repository.Delete(entity);
        public async Task<int> SaveAsync() => await _repository.SaveAsync();

    }
}
