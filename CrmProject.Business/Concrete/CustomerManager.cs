using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;

namespace CrmProject.Business.Concrete
{
    public class CustomerManager : GenericManager<Customer>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository repository) : base(repository)
        {
            _customerRepository = repository;
        }

        public async Task<List<Customer>> GetActiveCustomersAsync()
        {
            return await _customerRepository.GetWhereAsync(x => x.Status == true);
        }

        public async Task<(bool Success, string Message)> AddCustomerAsync(Customer customer)
        {
            customer.Status = true;
            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveAsync();
            return (true, $"{customer.CustomerName} isimli müşteri başarıyla portföye eklendi.");
        }

        public async Task<(bool Success, string Message)> UpdateCustomerAsync(Customer customer)
        {
            customer.Status = true;
            _customerRepository.Update(customer);
            await _customerRepository.SaveAsync();
            return (true, $"{customer.CustomerName} isimli müşteri başarıyla güncellendi.");
        }

        public async Task<(bool Success, string Message)> SoftDeleteCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return (false, "Silinmek istenen müşteri bulunamadı!");

            customer.Status = false;
            _customerRepository.Update(customer);
            await _customerRepository.SaveAsync();
            return (true, $"{customer.CustomerName} isimli müşteri sistemden silindi.");
        }
    }
}
