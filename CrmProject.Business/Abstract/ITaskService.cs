using CrmProject.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Business.Abstract
{
    public interface ITaskService
    {
        Task<AppTask?> GetByIdAsync(int id);
        Task<List<AppTask>> GetAllAsync();
        Task<List<AppTask>> GetWhereAsync(Expression<Func<AppTask, bool>> predicate);
        Task AddAsync(AppTask entity);
        void Update(AppTask entity);
        void Delete(AppTask entity);
        Task<int> SaveAsync();

    }
}
