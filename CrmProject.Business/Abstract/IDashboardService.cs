using CrmProject.Entity.Entities;

namespace CrmProject.Business.Abstract
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync();
    }
}
