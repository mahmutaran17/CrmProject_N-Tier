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
    public class TaskManager : ITaskService
    {
        private readonly IGenericRepository<AppTask> _repository;

        public TaskManager(IGenericRepository<AppTask> repository)
        {
            _repository = repository;
        }

        public async Task<AppTask?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<List<AppTask>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<List<AppTask>> GetWhereAsync(Expression<Func<AppTask, bool>> predicate) => await _repository.GetWhereAsync(predicate);
        public async Task AddAsync(AppTask entity) => await _repository.AddAsync(entity);
        public void Update(AppTask entity) => _repository.Update(entity);
        public void Delete(AppTask entity) => _repository.Delete(entity);
        public async Task<int> SaveAsync() => await _repository.SaveAsync();


        
    }
}
