using Byaxiom.Logger;
using MyAppTools.Services;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Services;
using PalletManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PalletManagement.Web.Setup
{
    public partial class OutTracking : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly IShipmentServices _shipmentService = null;
        private readonly IFacilityServices _facilityService = null;
        private static User CurrentUser = null;

        public OutTracking()
        {
            _palletService = new PalletServices();
            _shipmentService = new ShipmentServices();
            _facilityService = new FacilityServices();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            CurrentUser = Session["CurrentUser"] as User;
            displayShipmentNumber();
            if (CurrentUser != null)
            {
                LoadPallets(CurrentUser.AssignedFacility.FacilityId);
                LoadFacilities(CurrentUser.AssignedFacility.CustomerId);
            }
            LoadShipments();
        }

        private void displayShipmentNumber()
        {
            txtShipmentNumber.Text = _shipmentService.GetShipmentNumber(CurrentUser.AssignedFacility.FacilityName);
        }

        private void LoadShipments()
        {
            var oShipment = _shipmentService.GetList()
                .Where(x => x.OutTrackerId == CurrentUser.UserId && x.ShipmentSourceId == CurrentUser.AssignedFacilityId)
                .ToList();
            gdvShipment.DataSource = _shipmentService.GetDisplayList(oShipment);
            gdvShipment.DataBind();
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ErrorMessage.Visible = false;
            if (IsUpdateMode())
                UpdateShipment();
            else
                SaveShipment();

            LoadPallets(CurrentUser.AssignedFacilityId.Value);
            LoadShipments();
            displayShipmentNumber();
        }
        private void SaveShipment()
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    List<string> selectedPallets = chkAvailablePatllets.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Text).ToList();

                   


                    var oShipment = new Shipment
                    {
                        DateAdded = DateTime.Now,
                        OutTrackerId = CurrentUser.UserId,
                        ShipmentDestinationId = int.Parse(ddlDestinationFacility.SelectedValue),
                        ShipmentNumber = txtShipmentNumber.Text,
                        ShipmentSourceId = CurrentUser.AssignedFacility.FacilityId,
                        TruckNumber = txtTruckNumber.Text,
                        ShipmentStatusId = (int)SHIPMENT_STATUS.Checked_Out,
                        SourceDateTime = DateTime.Now,
                        NoOfPalletsOut = selectedPallets.Count,
                        IsCompleted = false,
                        NoOfPalletsIn = 0,
                        CustomerId = CurrentUser.AssignedFacility.CustomerId
                    };

                    var oShipmentPallets = new List<ShipmentPallet>();

                    ShipmentPallet oShipmentPallet = null;

                    foreach (var p in selectedPallets)
                    {
                        oShipmentPallet = new ShipmentPallet();
                        oShipmentPallet.PalletCode = p;
                        oShipmentPallet.CheckedIn = false;
                        oShipmentPallet.DateCheckedIn = null;
                        oShipmentPallets.Add(oShipmentPallet);
                        oShipmentPallet = null;
                    }

                    oShipment.PalletList = SerializationServices.SerializeJson(oShipmentPallets);


                    _shipmentService.Add(oShipment);

                    var palletsToAdd = _palletService.GetList().Where(x => selectedPallets.Contains(x.PalletCode)).ToList();

                    var updatedPallets = palletsToAdd.Select(c =>
                    {
                        c.LastMovementDate = DateTime.Now;
                        c.CurrentShipmentId = oShipment.ShipmentId;
                        return c;
                    }).ToList();
                    _palletService.Update(updatedPallets);
                    _shipmentService.SaveChanges();
                    tran.Complete();
                }

                //redirect to view shipment
                ResetForm();

                displayMessage("Shipment Created Successfully", true);

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.StackTrace + ex.Message + ex.InnerException, false);
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
            txtShipmentNumber.Text = string.Empty;
            txtTruckNumber.Text = string.Empty;
            ddlDestinationFacility.SelectedValue = "0";
            chkAvailablePatllets.ClearSelection();
            ErrorMessage.Visible = false;
            hdfShipmentId.Value = string.Empty;
            btnSubmit.Text = "Save";
        }

        private void UpdateShipment()
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    var shipmentId = int.Parse(hdfShipmentId.Value);
                    List<string> checkedPallets = chkAvailablePatllets.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Text).ToList();

                    var oShipment = _shipmentService.GetbyId(shipmentId);

                    oShipment.ShipmentDestinationId = int.Parse(ddlDestinationFacility.SelectedValue);
                    oShipment.ShipmentNumber = txtShipmentNumber.Text;
                    oShipment.TruckNumber = txtTruckNumber.Text;
                    oShipment.SourceDateTime = DateTime.Now;
                    //oShipment.PalletOutList = SerializationServices.SerializeJson(checkedPallets);
                    oShipment.NoOfPalletsOut = checkedPallets.Count;

                    var facilityPallets = _palletService.GetList().Where(x => x.FacilityId == CurrentUser.AssignedFacilityId).ToList();

                    var updatedPallets = facilityPallets.Select(c =>
                    {
                        if (checkedPallets.Contains(c.PalletCode))
                        {
                            c.LastMovementDate = DateTime.Now;
                            c.CurrentShipmentId = oShipment.ShipmentId;
                        }
                        else
                        {
                            c.LastMovementDate = null;
                            c.CurrentShipmentId = null;
                        }
                        return c;
                    }).ToList();

                    var oShipmentPallets = new List<ShipmentPallet>();

                    ShipmentPallet oShipmentPallet = null;

                    foreach (var p in checkedPallets)
                    {
                        oShipmentPallet = new ShipmentPallet();
                        oShipmentPallet.PalletCode = p;
                        oShipmentPallet.CheckedIn = false;
                        oShipmentPallet.DateCheckedIn = null;
                        oShipmentPallets.Add(oShipmentPallet);
                        oShipmentPallet = null;
                    }

                    oShipment.PalletList = SerializationServices.SerializeJson(oShipmentPallets);


                    _shipmentService.Update(oShipment);
                    _palletService.Update(updatedPallets);

                    tran.Complete();
                }

                ResetForm();
                displayMessage("Shipment Updated Successfully", true);
                btnSubmit.Text = "Save";
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private void DeleteShipment(int shipmentId)
        {
            try
            {

                var shipment = _shipmentService.GetbyId(shipmentId);
                if (!shipment.IsCompleted)
                {
                    var shipmentPallets = _palletService.GetbyShipmentId(shipmentId).ToList();
                    var updatedPallets = shipmentPallets.Select(c => { c.CurrentShipmentId = null; return c; }).ToList();
                    //foreach (var pallet in shipmentPallets)
                    //{
                    //    pallet.CurrentShipmentId = null;
                    //    modifiedPallets.Add(pallet);
                    //}
                    using (var transaction = new TransactionScope())
                    {
                        _palletService.Update(updatedPallets);
                        _shipmentService.Delete(shipmentId);
                        transaction.Complete();
                    }

                    ResetForm();
                    LoadShipments();

                }
                else
                {
                    displayMessage("This shipment can no longer be deleted. It has already been Checked-In.", false);
                }
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
                ddlDestinationFacility.DataSource = _facilityService.GetList()
                     .Where(x => x.CustomerId == customerId && x.FacilityId != CurrentUser.AssignedFacilityId)
                    .ToList();
                ddlDestinationFacility.DataTextField = "FacilityName";
                ddlDestinationFacility.DataValueField = "FacilityId";
                ddlDestinationFacility.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private void LoadPallets(int facilityId, int? shipmentId = null)
        {
            try
            {
                var pallets = _palletService.GetList()
                     .Where(x => x.FacilityId == facilityId && x.StatusId == (int)PALLET_STATUS.Available && (x.CurrentShipmentId == null | x.CurrentShipmentId == shipmentId))
                    .ToList();

                chkAvailablePatllets.DataSource = pallets;
                chkAvailablePatllets.DataTextField = "PalletCode";
                chkAvailablePatllets.DataValueField = "PalletId";
                chkAvailablePatllets.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private bool IsUpdateMode()
        {
            if (btnSubmit.Text == "Update")
                return true;
            return false;
        }
        private void SelectShipment(int shipmentId)
        {
            try
            {
                var oShipment = _shipmentService.GetbyId(shipmentId);
                if (!oShipment.IsCompleted)
                {
                    hdfShipmentId.Value = shipmentId.ToString();
                    txtShipmentNumber.Text = oShipment.ShipmentNumber;
                    txtTruckNumber.Text = oShipment.TruckNumber;
                    ddlDestinationFacility.SelectedValue = oShipment.ShipmentDestinationId.ToString();
                    LoadPallets(CurrentUser.AssignedFacilityId.Value, shipmentId);
                    var selectedpallets = SerializationServices.DeserializeJson<List<string>>(oShipment.PalletList);

                    foreach (var selectedpallet in selectedpallets)
                    {
                        chkAvailablePatllets.Items.FindByText(selectedpallet).Selected = true;
                    }
                    btnSubmit.Text = "Update";
                }
                else
                {
                    displayMessage("This shipment can no longer be edited. It has already been Checked-In.", false);
                }

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
            ErrorMessage.Visible = false;
            displayShipmentNumber();

            SelectShipment(shipmentId);
        }

        protected void gdvShipment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var shipmentId = int.Parse(e.Keys["ShipmentId"].ToString());
            ErrorMessage.Visible = false;
            DeleteShipment(shipmentId);
            LoadPallets(CurrentUser.AssignedFacilityId.Value);
            displayShipmentNumber();

        }
    }
}