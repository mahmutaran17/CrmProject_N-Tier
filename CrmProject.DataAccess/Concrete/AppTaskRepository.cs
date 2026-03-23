using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CrmProject.DataAccess.Abstract;
using CrmProject.DataAccess.Context;
using CrmProject.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmProject.DataAccess.Concrete
{
    public class AppTaskRepository : GenericRepository<AppTask>, IAppTaskRepository
    {
        public AppTaskRepository(CrmDbContext context) : base(context)
        {
            
        }

    }
}
