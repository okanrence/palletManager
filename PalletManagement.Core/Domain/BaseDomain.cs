﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Domain
{
    public class BaseDomain
    {
        public DateTime? DateAdded { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
