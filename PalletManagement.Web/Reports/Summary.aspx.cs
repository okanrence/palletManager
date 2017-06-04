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
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CurrentUser = Session["CurrentUser"] as User;
                LoadShipments();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the run time error "  
            //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
        }
        private void LoadShipments(DateTime? startDate = null, DateTime? endDate = null)
        {
            var data = _palletService.GetPalletSummary();
            var fdsv = data.Select(i => new
            {
                i.id,
                CustomerName = _customerService.GetbyId(i.CustomerId).CustomerName,
                i.Total,
                i.Damaged, i.Repaired, i.Available
                //var oShipment = _shipmentService.GetList()
                //    .Where(x => x.InTrackerId == CurrentUser.UserId || x.OutTrackerId == CurrentUser.UserId).OrderBy(x => x.IsCompleted).OrderByDescending(x => x.DateAdded)
                //    .ToList();
                //gd.DataSource = _shipmentService.GetDisplayList(oShipment);
                //gdvShipment.DataBind();
            });
            gdvPalletSummary.DataSource = fdsv;
            gdvPalletSummary.DataBind();
        }

        private void displayMessage(string message, bool isSuccessMsg)
        {
            //ErrorMessage.Visible = true;
            //if (isSuccessMsg)
            //    FailureText.Text = $"{message}";
            //else
            //    FailureText.Text = $"ERROR:{message}";
        }

        private void ResetForm()
        {
            //ErrorMessage.Visible = false;
            //hdfShipmentId.Value = string.Empty;
        }


        private void SelectShipment(int shipmentId)
        {
            try
            {
                var oShipment = _shipmentService.GetbyId(shipmentId);


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
            //var shipmentId = int.Parse(gdvShipment.SelectedDataKey["ShipmentId"].ToString());
            //SelectShipment(shipmentId);
        }

        protected void lnkExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Vithal" + DateTime.Now + ".xls";
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
    }
}