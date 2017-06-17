using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Domain
{
    public class DamageLevel : baseDomain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DamageLevelId { get; set; }
        [MaxLength(20)]
        public string DamageLevelName { get; set; }
    }
}
