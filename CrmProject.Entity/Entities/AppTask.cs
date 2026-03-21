using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Entity.Entities
{
    public class AppTask
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int AssignedByUserId { get; set; }
        public int AssignedToUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = null!;
        public string Priority { get; set; } = null!;


        public Project Project { get; set; } = null!;
        public User AssignedByUser { get; set; } = null!;
        public User AssignedToUser { get; set; } = null!;

        public ICollection<TaskLog> TaskLogs { get; set; } = new List<TaskLog>();
    }
}
