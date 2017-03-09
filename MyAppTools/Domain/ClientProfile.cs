using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAppTools.Domain
{
    public  class ClientProfile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientProfileId { get; set; }
        public string clientId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid clientKey { get; set; }
        public string clientIpAddress { get; set; }
        public string clientDescription { get; set; }
        public bool unRestricted { get; set; }
        public string status { get; set; }
        public DateTime dateCreated { get; set; }
          
    }
}
