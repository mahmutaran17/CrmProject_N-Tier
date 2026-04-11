namespace CrmProject.Entity.Entities
{
    public class DashboardDto
    {
        public int TotalProjects { get; set; }
        public int TotalTasks { get; set; }
        public int ActiveUsers { get; set; }
        public int PendingTasks { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public List<AppTask> LastTasks { get; set; } = new();
    }
}
