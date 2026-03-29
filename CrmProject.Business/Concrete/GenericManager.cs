using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;

namespace CrmProject.Business.Concrete
{
    public class GenericManager<T>: IGenericService<T> where T : class 
    {
        private readonly IGenericRepository<T> _repository;
        public GenericManager(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<List<T>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate) => await _repository.GetWhereAsync(predicate);
        public async Task AddAsync(T entity) => await _repository.AddAsync(entity);
        public void Update(T entity) => _repository.Update(entity);
        public void Delete(T entity) => _repository.Delete(entity);
        public async Task<int> SaveAsync() => await _repository.SaveAsync();

        public async Task<T?> GetSingleWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return await _repository.GetSingleWithIncludesAsync(predicate, includes);
        }

        public async Task<List<T>> GetListWithIncludesAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            return await _repository.GetListWithIncludesAsync(filter, includes);
        }
    }
}
