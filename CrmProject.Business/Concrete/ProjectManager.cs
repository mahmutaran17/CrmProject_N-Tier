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
    public class ProjectManager : IProjectService
    {
        private readonly IGenericRepository<Project> _repository;

        public ProjectManager(IGenericRepository<Project> repository)
        {
            _repository = repository;
        }

        public async Task<Project?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<List<Project>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<List<Project>> GetWhereAsync(Expression<Func<Project, bool>> predicate) => await _repository.GetWhereAsync(predicate);
        public async Task AddAsync(Project entity) => await _repository.AddAsync(entity);
        public void Update(Project entity) => _repository.Update(entity);
        public void Delete(Project entity) => _repository.Delete(entity);
        public async Task<int> SaveAsync() => await _repository.SaveAsync();





    }
}
