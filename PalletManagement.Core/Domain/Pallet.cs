using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Domain
{
    public class Pallet : baseDomain
    {
        public Pallet()
        {
            this.Shipments = new HashSet<Shipment>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PalletId { get; set; }
        [MaxLength(20)]
        public string PalletCode { get; set; }
        public int? StatusId { get; set; }
        public virtual PalletStatus PalletStatus { get; set; }
        public int? FacilityId { get; set; }
        public virtual Facility CurrentLocation { get; set; }

        public int? CurrentShipmentId { get; set; }
        //[ForeignKey("CurrentShipmentId")]
        //public virtual Shipment CurrentShipment { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }
        public DateTime? LastMovementDate { get; set; }
       
    }
}
