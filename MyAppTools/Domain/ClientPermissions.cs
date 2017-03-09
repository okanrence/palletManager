using System;

namespace MyAppTools.Domain
{
    public  class ClientPermissions
    {
        public int ClientPermissionsId { get; set; }
        public string clientID { get; set; }
        public int endPointId { get; set; }
        public DateTime dateAdded { get; set; }
          
    }
}
