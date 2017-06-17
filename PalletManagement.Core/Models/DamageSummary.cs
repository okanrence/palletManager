using PalletManagement.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Models
{
    public class DamageSummary
    {
        public int id { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer oCustomer { get; set; }
        public int FacilityId { get; set; }
        public virtual Facility oFacility { get; set; }
        public int Damaged { get; set; }
        public int TotallyDamaged { get; set; }
        public int Repaired { get; set; }
        public int Repairable { get; set; }
        public int DeckerboardsUsed { get; set; }
        public int DP_TDP_Extracted { get; set; }
        public int NailsUsed { get; set; }

    }
}
