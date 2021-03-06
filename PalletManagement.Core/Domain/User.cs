﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletManagement.Core.Domain
{

    public class User : baseDomain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(200)]
        public string FullName => $"{FirstName} {LastName}";

        [Index]
        [MaxLength(20)]
        public string StaffNumber { get; set; }
        [MaxLength(200)]
        public string EmailAddress { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        public int UserRoleId { get; set; }
        public virtual UserRole UserRole { get; set; }
        public int? AssignedFacilityId { get; set; }
        [ForeignKey("AssignedFacilityId")]
        public virtual Facility AssignedFacility { get; set; }
        public string Password { get; set; }
        [MaxLength(1)]
        public string ProfileStatus { get; set; }
        public DateTime? DateDeactivated { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool MustChangePassword { get; set; }
    }

}
