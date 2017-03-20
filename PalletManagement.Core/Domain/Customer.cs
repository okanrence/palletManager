using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Domain
{
    public class Customer : BaseDomain
    {
        public Customer()
        {
            this.Facilities = new HashSet<Facility>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Facility> Facilities { get; set; }
    }

}
