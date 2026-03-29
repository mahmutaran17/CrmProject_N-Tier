using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;

namespace CrmProject.Business.Concrete
{
    public class RoleManager : GenericManager<Role> , IRoleService
    {
        public RoleManager(IGenericRepository<Role> repository) : base(repository)
        {
            
        }
    }
}
