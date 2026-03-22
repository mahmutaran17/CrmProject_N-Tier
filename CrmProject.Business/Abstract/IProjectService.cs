using CrmProject.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Business.Abstract
{
    public interface IProjectService
    {
        Task<Project?> GetByIdAsync(int id);
        Task<List<Project>> GetAllAsync();
        Task<List<Project>> GetWhereAsync(Expression<Func<Project, bool>> predicate);
        Task AddAsync(Project entity);
        void Update(Project entity);
        void Delete(Project entity);
        Task<int> SaveAsync();
    }
}
