using PalletManagement.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PalletManagement.Web
{
    public partial class Landing : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //  this.MasterPageFile = "/SiteAdmin.master";
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            var CurrentUser = (User)Session["CurrentUser"];

            if (CurrentUser != null)
            {
                if (CurrentUser.UserRole.UserRoleName == "Admin")
                    this.MasterPageFile = "~/SiteAdmin.master";
                
            }
        }


    }
}