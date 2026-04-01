using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Entity.Entities
{
    public enum AppTaskStatus
    {
        Beklemede = 1,
        DevamEdiyor = 2,
        Tamamlandi = 3,
        IptalEdildi = 4
    }
    public enum AppTaskPriority
    {
        Dusuk = 1,
        Orta = 2,
        Yuksek = 3,
        Acil = 4
    }

    public class AppTask
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int AssignedByUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        //

        public AppTaskStatus Status { get; set; }
        public AppTaskPriority Priority { get; set; }


        //(Navigation Properties) 
        public Project Project { get; set; } = null!;
        public User AssignedByUser { get; set; } = null!;

        public ICollection<TaskLog> TaskLogs { get; set; } = new List<TaskLog>();
        public ICollection<User> AssignedUsers { get; set; } = new List<User>();
    }


}
