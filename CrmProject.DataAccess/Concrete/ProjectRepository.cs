using CrmProject.DataAccess.Abstract;
using CrmProject.DataAccess.Context;
using CrmProject.Entity.Entities;

namespace CrmProject.DataAccess.Concrete
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(CrmDbContext context) : base(context)
        {
        }
    }
}
