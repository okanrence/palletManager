using PalletManagement.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Models
{
    public class PalletSummary
    {
        public int id { get; set; }
        public int CustomerId { get; set; }
        public Customer oCustomer { get; set; }
        public int Total { get; set; }
        public int Damaged { get; set; }
        public int Repaired { get; set; }
        public int Available { get; set; }

    }
}
