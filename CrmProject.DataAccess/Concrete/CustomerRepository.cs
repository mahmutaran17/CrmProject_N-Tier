using CrmProject.DataAccess.Abstract;
using CrmProject.DataAccess.Context;
using CrmProject.Entity.Entities;

namespace CrmProject.DataAccess.Concrete
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CrmDbContext context) : base(context)
        {
        }
    }
}
