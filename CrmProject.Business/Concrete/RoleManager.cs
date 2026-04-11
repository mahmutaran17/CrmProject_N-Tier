using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;

namespace CrmProject.Business.Concrete
{
    public class RoleManager : GenericManager<Role>, IRoleService
    {
        public RoleManager(IRoleRepository repository) : base(repository)
        {
        }
    }
}
