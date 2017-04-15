using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Domain
{
    public class Shipment : baseDomain
    {
        public Shipment()
        {
            this.Pallets = new HashSet<Pallet>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }

        public int ShipmentStatusId { get; set; }
        [ForeignKey("ShipmentStatusId")]
        public virtual ShipmentStatus ShipmentStatus { get; set; }

        public int? ShipmentSourceId { get; set; }
        [ForeignKey("ShipmentSourceId")]
        public virtual Facility ShipmentSource { get; set; }

        public int? ShipmentDestinationId { get; set; }
        [ForeignKey("ShipmentDestinationId")]
        public virtual Facility ShipmentDestination { get; set; }

        public DateTime? SourceDateTime { get; set; }
        public DateTime? DestinationDateTime { get; set; }

        public int? InTrackerId { get; set; }
        [ForeignKey("InTrackerId")]
        public virtual User InTracker { get; set; }

        public int? OutTrackerId { get; set; }
        [ForeignKey("OutTrackerId")]
        public virtual User OutTracker { get; set; }

        public string TruckNumber { get; set; }
        public string PalletList { get; set; }
        public int NoOfPallets { get; set; }
        public bool IsCompleted { get; set; }
        //public bool IsCompleted { get { return this.ShipmentStatusId == (int)Services.SHIPMENT_STATUS.Incoming; } set() }
        public virtual ICollection<Pallet> Pallets { get; set; }

    }

    public class ShipmentStatus : baseDomain
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipmentStatusId { get; set; }
        [MaxLength(20)]
        public string ShipmentStatusName { get; set; }

    }
}
