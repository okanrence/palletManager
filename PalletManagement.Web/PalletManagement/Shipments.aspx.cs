using Byaxiom.Logger;
using MyAppTools.Services;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PalletManagement.Web.Setup
{
    public partial class Shipments : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly IShipmentServices _shipmentService = null;
        private readonly IFacilityServices _facilityService = null;
        private static User CurrentUser = null;

        public Shipments()
        {
            _palletService = new PalletServices();
            _shipmentService = new ShipmentServices();
            _facilityService = new FacilityServices();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CurrentUser = Session["CurrentUser"] as User;
                LoadShipments();
            }
        }

        private void LoadShipments( DateTime? startDate = null, DateTime? endDate = null)
        {
            var oShipment = _shipmentService.GetList()
                .Where(x => x.InTrackerId == CurrentUser.UserId || x.OutTrackerId == CurrentUser.UserId).OrderBy(x => x.IsCompleted).OrderByDescending(x => x.DateAdded)
                .ToList();
            gdvShipment.DataSource = _shipmentService.GetDisplayList(oShipment);
            gdvShipment.DataBind();
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
            hdfShipmentId.Value = string.Empty;
        }


        private void SelectShipment(int shipmentId)
        {
            try
            {
                var oShipment = _shipmentService.GetbyId(shipmentId);


                hdfShipmentId.Value = shipmentId.ToString();

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }

        protected void gdvShipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            var shipmentId = int.Parse(gdvShipment.SelectedDataKey["ShipmentId"].ToString());
            SelectShipment(shipmentId);
        }


    }
}