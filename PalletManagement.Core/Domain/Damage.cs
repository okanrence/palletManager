using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Domain
{
    public class Damage : baseDomain
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DamageId { get; set; }
        public int DamagedPalletId { get; set; }
        [ForeignKey("DamagedPalletId")]
        public virtual Pallet DamagedPallet { get; set; }
        public int DamageLevelId { get; set; }
        public virtual DamageLevel DamageLevel { get; set; }
        public int facilityId { get; set; }
        public virtual Facility Facility { get; set; }
        public string Reason { get; set; }
        public int DeckerboardsUsed { get; set; }
        public int DP_TDP_Extracted { get; set; }
        public int NailsUsed { get; set; }
        public string CollectionFormNo { get; set; }
        public string IssueFormNo { get; set; }
        public bool Repaired { get; set; }
        public DateTime? DateRepaired { get; set; }
    }
}
