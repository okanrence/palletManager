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
    public partial class ShipmentReports : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly IShipmentServices _shipmentServices = null;
        private readonly IFacilityServices _facilityService = null;
        private readonly ICustomerServices _customerService = null;
        private static User CurrentUser = null;

        public ShipmentReports()
        {
            _palletService = new PalletServices();
            _shipmentServices = new ShipmentServices();
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
            txtEndDate.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            ddlCustomer.SelectedIndex = 0;
            ddlFacilities.Items.Clear();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFacilities(int.Parse(ddlCustomer.SelectedValue));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            gdvRepairsBreakdown.DataSource = null;
            gdvRepairsBreakdown.DataBind();
            pnlBreakDown.Visible = false;
            GetReports();
        }

        private void GetReports()
        {
            try
            {
                DateTime? startDate = null;
                DateTime? endDate = null;

                int? customerId = null;
                int? facilityId = null;

                if (!string.IsNullOrEmpty(txtStartDate.Text))
                    startDate = DateTime.Parse(txtStartDate.Text);

                if (!string.IsNullOrEmpty(txtEndDate.Text))
                    endDate = DateTime.Parse(txtEndDate.Text).AddDays(1);

                if (ddlCustomer.SelectedIndex > 0)
                    customerId = int.Parse(ddlCustomer.SelectedValue);

                if (ddlFacilities.SelectedIndex > 0)
                    facilityId = int.Parse(ddlFacilities.SelectedValue);

                var report = _shipmentServices.GetPalletSummary(startDate, endDate, customerId, facilityId, true);
                var formatedReport = _shipmentServices.GetDisplayList(report);
                gdvDamages.DataSource = formatedReport;
                gdvDamages.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        protected void gdvDamages_SelectedIndexChanged(object sender, EventArgs e)
        {
            var facilityId = int.Parse(gdvDamages.SelectedDataKey["FacilityId"].ToString());
            LoadDamagesByFacility(facilityId);
        }

        private void LoadDamagesByFacility(int facilityId)
        {
            try
            {
                pnlBreakDown.Visible = true;
                var damages = _shipmentServices.GetList().Where(x => x.ShipmentSourceId == facilityId);

                if (!string.IsNullOrEmpty(txtStartDate.Text))
                {
                    var startDate = DateTime.Parse(txtStartDate.Text);
                    damages = damages.Where(x => x.DateAdded >= startDate);
                }
                if (!string.IsNullOrEmpty(txtStartDate.Text))
                {
                    var endDate = DateTime.Parse(txtEndDate.Text).AddDays(1);
                    damages = damages.Where(x => x.DateAdded <= endDate);
                }
                gdvRepairsBreakdown.DataSource = _shipmentServices.GetDisplayList(damages.ToList());
                gdvRepairsBreakdown.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.StackTrace + ex.Message + ex.InnerException, false);
            }
        }
        protected void lnkOverallSummaryExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Shipment_Summary_Report_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gdvDamages.GridLines = GridLines.Both;
            gdvDamages.HeaderStyle.Font.Bold = true;
            gdvDamages.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

        protected void lnkbreakdown_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Shipment_Breakdown_Report_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gdvRepairsBreakdown.GridLines = GridLines.Both;
            gdvRepairsBreakdown.HeaderStyle.Font.Bold = true;
            gdvRepairsBreakdown.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
        }
    }
}