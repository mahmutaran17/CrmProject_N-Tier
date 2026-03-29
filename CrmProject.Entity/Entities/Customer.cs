using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Entity.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } // Firma veya Şahıs Adı
        public string ContactPerson { get; set; } // Yetkili Kişi
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; } // True: Aktif, False: Silinmiş (Soft-Delete)

        // Bir müşterinin birden fazla projesi olabilir (İlişki)
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
