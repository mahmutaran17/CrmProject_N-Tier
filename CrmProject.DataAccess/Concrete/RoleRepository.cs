using CrmProject.DataAccess.Abstract;
using CrmProject.DataAccess.Context;
using CrmProject.Entity.Entities;

namespace CrmProject.DataAccess.Concrete
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(CrmDbContext context) : base(context)
        {
        }
    }
}
