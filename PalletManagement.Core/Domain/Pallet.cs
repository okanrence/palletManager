using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Domain
{
    public class Pallet : BaseDomain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PalletId { get; set; }
        [MaxLength(20)]
        public string PalletCode { get; set; }
        public PalletStatus PalletStatus { get; set; }
        public Facility CurrentLocation { get; set; }
        public Customer AssignedCustomer { get; set; }
    }
}
