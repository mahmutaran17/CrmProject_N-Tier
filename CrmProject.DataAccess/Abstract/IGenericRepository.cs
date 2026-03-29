using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.DataAccess.Abstract
{
    //class keyword here means, you cannot send int, string, only entities !
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T Entity);
        void Update(T Entity);
        void Delete(T Entity);
        Task<int> SaveAsync();
        Task<T?> GetSingleWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetListWithIncludesAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);

    }
}
