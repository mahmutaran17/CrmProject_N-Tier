using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Entity.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? Description { get; set; }

        //nav prop
        public Project Project { get; set; }
    }
}
