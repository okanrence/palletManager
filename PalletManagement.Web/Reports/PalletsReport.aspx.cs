using Byaxiom.Logger;
using MyAppTools.Services;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Models;
using PalletManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PalletManagement.Web.Reports
{
    public partial class PalletsReport : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly IPalletStatusServices _palletStatusService = null;
        private readonly IFacilityServices _facilityService = null;
        private readonly ICustomerServices _customerService = null;
        private static User CurrentUser = null;

        public PalletsReport()
        {
            _palletService = new PalletServices();
            _palletStatusService = new PalletStatusServices();
            _facilityService = new FacilityServices();
            _customerService = new CustomerServices();
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            CurrentUser = (User)Session["CurrentUser"];

            if (CurrentUser != null)
            {
                if (CurrentUser.UserRoleId == (int)USER_ROLES.Admin)
                    this.MasterPageFile = "~/SiteAdmin.master";
                else if (CurrentUser.UserRoleId == (int)USER_ROLES.Operator)
                    this.MasterPageFile = "~/SiteOperator.master";
                else if (CurrentUser.UserRoleId == (int)USER_ROLES.ReportView)
                    this.MasterPageFile = "~/SiteReports.master";
                else
                    this.MasterPageFile = "~/SiteOuter.master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CurrentUser = Session["CurrentUser"] as User;
                LoadCustomers();
                LoadPalletStatus();
                // pnlByFacility.Visible = false;
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the run time error "  
            //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
        }

        private void LoadCustomers()
        {
            try
            {
                ddlCustomer.DataSource = _customerService.GetList().ToList();
                ddlCustomer.DataValueField = "CustomerId";
                ddlCustomer.DataTextField = "CustomerName";
                ddlCustomer.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private void LoadPalletStatus()
        {
            try
            {
                ddlStatus.DataSource = _palletStatusService.GetList().ToList();
                ddlStatus.DataValueField = "StatusId";
                ddlStatus.DataTextField = "StatusName";
                ddlStatus.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }
        private void LoadFacilities(int customerId)
        {
            try
            {
                if (ddlCustomer.SelectedIndex > 0)
                {
                    var customerFacilities = _customerService
                                           .GetList()
                                           .FirstOrDefault(x => x.CustomerId == customerId)
                               ?.Facilities
                                           .ToList();
                    ddlFacilities.Items.Clear();
                    ddlFacilities.Items.Add("--Select--");
                    ddlFacilities.DataSource = customerFacilities;
                    ddlFacilities.DataTextField = "FacilityName";
                    ddlFacilities.DataValueField = "FacilityId";
                    ddlFacilities.DataBind();
                }
                else
                {
                    ddlFacilities.Items.Clear();
                    // ddlFacilities.DataBind();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private void displayMessage(string message, bool isSuccessMsg)
        {
            ErrorMessage.Visible = true;
            if (isSuccessMsg)
                FailureText.Text = $"{message}";
            else
                FailureText.Text = $"ERROR:{message}";
        }

        private void ResetForm()
        {
            ErrorMessage.Visible = false;
            //hdfShipmentId.Value = string.Empty;
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFacilities(int.Parse(ddlCustomer.SelectedValue));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GetReports();
        }

        private void GetReports()
        {
            try
            {
                string palletCode = null;
                int? statusId = null;

                int? customerId = null;
                int? facilityId = null;

                if (!string.IsNullOrEmpty(txtPalletCode.Text))
                    palletCode = txtPalletCode.Text;

                if (ddlStatus.SelectedIndex > 0)
                    statusId = int.Parse(ddlStatus.SelectedValue);

                if (ddlCustomer.SelectedIndex > 0)
                    customerId = int.Parse(ddlCustomer.SelectedValue);

                if (ddlFacilities.SelectedIndex > 0)
                    facilityId = int.Parse(ddlFacilities.SelectedValue);

                var report = _palletService.GetPalletList(palletCode, statusId, customerId, facilityId);
                var formatedReport = _palletService.GetPalletListDisplay(report);
                gdvPallets.DataSource = formatedReport;
                gdvPallets.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

       
        protected void lnkOverallSummaryExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Pallets_Details_Report_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gdvPallets.GridLines = GridLines.Both;
            gdvPallets.HeaderStyle.Font.Bold = true;
            gdvPallets.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

    }
}