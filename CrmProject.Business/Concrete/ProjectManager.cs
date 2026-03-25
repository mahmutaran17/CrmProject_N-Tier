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
    public class ProjectManager : GenericManager<Project>, IProjectService
    {

        public ProjectManager(IGenericRepository<Project> repository) : base(repository)
        {
            
        }






    }
}
