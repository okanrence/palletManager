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
    public partial class InTracking : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly IShipmentServices _shipmentService = null;
        private readonly IFacilityServices _facilityService = null;
        private static User CurrentUser = null;

        public InTracking()
        {
            _palletService = new PalletServices();
            _shipmentService = new ShipmentServices();
            _facilityService = new FacilityServices();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            CurrentUser = Session["CurrentUser"] as User;
            LoadShipments();
        }

        private void LoadShipments()
        {
            var oShipment = _shipmentService.GetList()
                .Where(x => x.ShipmentDestinationId == CurrentUser.AssignedFacilityId && x.IsCompleted == false)
                .ToList();
            gdvShipment.DataSource = _shipmentService.GetDisplayList(oShipment);
            gdvShipment.DataBind();
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ErrorMessage.Visible = false;
            SaveShipment(int.Parse(hdfShipmentId.Value));
            if (CurrentUser.AssignedFacilityId != null) LoadPallets(CurrentUser.AssignedFacilityId.Value);
            LoadShipments();
        }
        private void SaveShipment(int shipmentId)
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    var selectedPallets = chkAvailablePatllets.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Text).ToList();
                    var shipmentIsComplete = chkAvailablePatllets.Items.Cast<ListItem>().Where(x => x.Selected == false).Select(x => x.Text).ToList().Count <= 0;

                    var oShipment = _shipmentService.GetbyId(shipmentId);

                    oShipment.IsCompleted = shipmentIsComplete;
                    oShipment.DestinationDateTime = DateTime.Now;
                    oShipment.InTrackerId = CurrentUser.UserId;


                    var modifiedPallets = new List<Pallet>();
                    var oPallets = _palletService.GetList().Where(x => selectedPallets.Contains(x.PalletCode));

                    foreach (var pallet in oPallets)
                    {
                        if (selectedPallets.Contains(pallet.PalletCode))
                            pallet.CurrentShipmentId = null;
                        else
                            pallet.StatusId = (int)PALLET_STATUS.Unaccounted;

                        pallet.FacilityId = oShipment.ShipmentDestinationId;
                        modifiedPallets.Add(pallet);
                    };

                    _palletService.Update(modifiedPallets);
                    _shipmentService.Update(oShipment);
                    tran.Complete();
                }


                //redirect to view shipment
                ResetForm();

                DisplayMessage("Shipment Checked-In Successfully", true);
                btnSubmit.Visible = false;

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                DisplayMessage(ex.Message, false);
            }
        }

        private void DisplayMessage(string message, bool isSuccessMsg)
        {
            ErrorMessage.Visible = true;
            FailureText.Text = isSuccessMsg ? $@"{message}" : $@"ERROR:{message}";
        }

        private void ResetForm()
        {
            chkAvailablePatllets.ClearSelection();
            ErrorMessage.Visible = false;
            hdfShipmentId.Value = string.Empty;
            btnSubmit.Text = @"Save";
        }


        private void LoadPallets(int shipmentId)
        {
            try
            {
                var pallets = _palletService.GetList()
                     .Where(x => x.CurrentShipmentId == shipmentId)
                    .ToList();

                //if (shipmentId != null)
                //{
                //    pallets.AddRange(_palletService.GetList()
                //     .Where(x => x.FacilityId == facilityId && x.ShipmentId == shipmentId)
                //    .ToList());
                //}

                chkAvailablePatllets.DataSource = pallets;
                chkAvailablePatllets.DataTextField = "PalletCode";
                chkAvailablePatllets.DataValueField = "PalletId";
                chkAvailablePatllets.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                DisplayMessage(ex.Message, false);
            }
        }

        private bool IsUpdateMode()
        {
            return btnSubmit.Text == @"Update";
        }
        private void SelectShipment(int shipmentId)
        {
            try
            {
                var oShipment = _shipmentService.GetbyId(shipmentId);
                if (!oShipment.IsCompleted)
                {
                    hdfShipmentId.Value = shipmentId.ToString();
                    LoadPallets(shipmentId);
                    //var selectedpallets = SerializationServices.DeserializeJson<List<string>>(oShipment.PalletList);

                    //foreach (var selectedpallet in selectedpallets)
                    //{
                    //    chkAvailablePatllets.Items.FindByText(selectedpallet).Selected = true;
                    //}
                    // btnSubmit.Text = "Check";
                }
                else
                {
                    DisplayMessage("This shipment can no longer be edited. It has already been Checked-In.", false);
                }

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                DisplayMessage(ex.Message, false);

            }
        }

        protected void gdvShipment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gdvShipment.SelectedDataKey != null)
            {
                var shipmentId = int.Parse(gdvShipment.SelectedDataKey?["ShipmentId"].ToString());
                SelectShipment(shipmentId);
            }
            btnSubmit.Visible = true;
        }


    }
}