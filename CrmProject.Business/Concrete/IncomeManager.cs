using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;

namespace CrmProject.Business.Concrete
{
    public class IncomeManager : GenericManager<Income>, IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;

        public IncomeManager(IIncomeRepository incomeRepository) : base(incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }
    }
}