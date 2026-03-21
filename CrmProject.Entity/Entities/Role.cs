using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Entity.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;

        //nav properties
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
