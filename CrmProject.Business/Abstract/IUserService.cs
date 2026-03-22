using CrmProject.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Business.Abstract
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<List<User>> GetWhereAsync(Expression<Func<User, bool>> predicate);
        Task AddAsync(User entity);
        void Update(User entity);
        void Delete(User entity);
        Task<int> SaveAsync();
    }
}
