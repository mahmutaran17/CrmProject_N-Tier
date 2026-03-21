using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Entity.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string PasswordHash { get; set; } = null!;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }


        //navigation properties

        public Role Role { get; set; } = null!;
        public ICollection<AppTask> AssignedTasks { get; set; } = new List<AppTask>();

        public ICollection<AppTask> CreatedTasks { get; set; } = new List<AppTask>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        


    }
}
