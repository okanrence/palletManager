using MyAppTools.Helpers;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PalletManagement.Core.Services
{

    public class PROFILE_STATUS
    {
        public const string ACTIVE = "A";
        public const string DEACTIVATED = "D";

    }
    public class USER_ROLES
    {
        public const string ADMIN = "Admin";
        public const string TRACKER = "Tracker";
        public const string ENGINEER = "Engineer";
       
    }

}
