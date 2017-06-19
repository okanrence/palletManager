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
    public partial class Summary : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly IShipmentServices _shipmentService = null;
        private readonly IFacilityServices _facilityService = null;
        private readonly ICustomerServices _customerService = null;
        private static User CurrentUser = null;

        public Summary()
        {
            _palletService = new PalletServices();
            _shipmentService = new ShipmentServices();
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
                LoadByCustomer();
              //  LoadByFacility(2);
               // pnlByFacility.Visible = false;
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the run time error "  
            //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
        }
        private void LoadByCustomer(DateTime? startDate = null, DateTime? endDate = null)
        {
            var palletSummary = _palletService.GetPalletSummary();
           
            var FormatedResult = palletSummary.Select(i => new
            {
                i.CustomerId,
                CustomerName = _customerService.GetbyId(i.CustomerId).CustomerName,
                i.Total,
                i.Damaged,
                i.Repaired,
                i.Available,
                i.Unaccounted
            });
            gdvPalletSummary.DataSource = FormatedResult;
            gdvPalletSummary.DataBind();
        }

        private void LoadByFacility(int CustomerId)
        {
            var palletSummary = _palletService.GetPalletSummary(CustomerId);

            var FormatedResult = palletSummary.Select(i => new
            {
                i.FacilityId,
                CustomerName = i.oCustomer.CustomerName,
                FacilityName = _facilityService.GetbyId(i.FacilityId).FacilityName,
                i.Total,
                i.Damaged,
                i.Repaired,
                i.Available,
                i.Unaccounted
            });
            lblCustomerName.Text = palletSummary.FirstOrDefault().oCustomer.CustomerName;
            gdvByFacility.DataSource = FormatedResult;
            gdvByFacility.DataBind();
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


        private void SelectCustomer(int customerId)
        {
            try
            {
                pnlByFacility.Visible = true;
                LoadByFacility(customerId);
                //hdfShipmentId.Value = shipmentId.ToString();

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }

        protected void gdvShipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            var customerId = int.Parse(gdvPalletSummary.SelectedDataKey["CustomerId"].ToString());
            SelectCustomer(customerId);
        }

        protected void lnkOverallExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Summary_Report_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gdvPalletSummary.GridLines = GridLines.Both;
            gdvPalletSummary.HeaderStyle.Font.Bold = true;
            gdvPalletSummary.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

        protected void lnkByFacilitiesExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Summary_Report_Facilities" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gdvByFacility.GridLines = GridLines.Both;
            gdvByFacility.HeaderStyle.Font.Bold = true;
            gdvByFacility.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
    }
}