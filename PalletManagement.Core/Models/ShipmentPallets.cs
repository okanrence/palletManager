using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Models
{
   public class ShipmentPallet
    {
        public string PalletCode { get; set; }
        public bool CheckedIn { get; set; }
        public DateTime? DateCheckedIn { get; set; }
    }
}
