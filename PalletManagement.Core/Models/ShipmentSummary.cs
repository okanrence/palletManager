using PalletManagement.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Models
{
    public class ShipmentSummary
    {
        public int ShipmentId { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer oCustomer { get; set; }
        public int? ShipmentSourceId { get; set; }
        public virtual Facility ShipmentSource { get; set; }
        public int? ShipmentDestinationId { get; set; }
        public virtual Facility ShipmentDestination { get; set; }
        public int TotalPalletsTrackedOut { get; set; }
        public int TotalPalletsTrackedIn { get; set; }
        public int TotalShipments { get; set; }
    }
}
