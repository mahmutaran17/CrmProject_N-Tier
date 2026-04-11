using CrmProject.Entity.Entities;

namespace CrmProject.Business.Abstract
{
    public interface ICustomerService : IGenericService<Customer>
    {
        Task<List<Customer>> GetActiveCustomersAsync();
        Task<(bool Success, string Message)> AddCustomerAsync(Customer customer);
        Task<(bool Success, string Message)> UpdateCustomerAsync(Customer customer);
        Task<(bool Success, string Message)> SoftDeleteCustomerAsync(int id);
    }
}
