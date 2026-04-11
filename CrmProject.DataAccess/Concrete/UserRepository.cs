using CrmProject.DataAccess.Abstract;
using CrmProject.DataAccess.Context;
using CrmProject.Entity.Entities;

namespace CrmProject.DataAccess.Concrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CrmDbContext context) : base(context)
        {
        }
    }
}
