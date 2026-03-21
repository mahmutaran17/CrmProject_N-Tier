using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Entity.Entities
{
    public class TaskLog
    {
        public int Id { get; set; }
        public int TaskItemId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }


        //nav prop
        public AppTask TaskItem { get; set; } = null!;

    }
}
