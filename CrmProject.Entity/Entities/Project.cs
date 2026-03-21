using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Entity.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = null!;


        //nav properties

        public ICollection<AppTask> Tasks { get; set; } = new List<AppTask>();
        public ICollection<Income> Incomes { get; set; } = new List<Income>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    }
}
