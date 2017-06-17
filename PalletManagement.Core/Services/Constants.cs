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
    public enum USER_ROLES
    {
        Admin = 1,
        Operator,
        ReportView

    }
    public class FACILITY_TYPES
    {
        public const string DEPOT = "Depot";
        public const string PLANT = "Plant";

    }
    public enum PALLET_STATUS
    {
        Available = 1,
        Damaged,
        Unaccounted
    }

    public enum DAMAGE_LEVEL
    {
        Repairable = 1,
        Total_Damage

    }
    public enum SHIPMENT_STATUS
    {
        Checked_Out = 1,
        Checked_In,
    }
}
