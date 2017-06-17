using PalletManagement.Core.Domain;
using PalletManagement.Core.Services;
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
                if (CurrentUser.UserRoleId ==  (int)USER_ROLES.Admin)
                    this.MasterPageFile = "~/SiteAdmin.master";
                else if (CurrentUser.UserRoleId == (int)USER_ROLES.Operator)
                    this.MasterPageFile = "~/SiteOperator.master";
                else if (CurrentUser.UserRoleId == (int)USER_ROLES.ReportView)
                    this.MasterPageFile = "~/SiteReports.master";
                else
                    this.MasterPageFile = "~/SiteOuter.master";
            }
        }


    }
}