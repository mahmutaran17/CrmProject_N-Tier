using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmProject.Entity.Entities;

namespace CrmProject.DataAccess.Abstract
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
    }
}
