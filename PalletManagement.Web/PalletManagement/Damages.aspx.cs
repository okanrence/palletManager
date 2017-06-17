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
    public partial class Damages : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly IDamageLevelServices _damageLevelService = null;
        private readonly IDamageServices _damageService = null;
        private readonly IFacilityServices _facilityService = null;
        private static User CurrentUser = null;

        public Damages()
        {
            _palletService = new PalletServices();
            _damageLevelService = new DamageLevelServices();
            _damageService = new DamageServices();
            _facilityService = new FacilityServices();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CurrentUser = Session["CurrentUser"] as User;
                if (CurrentUser == null)
                {
                    Response.Redirect("~/Default.aspx");
                }
                LoadPallets(CurrentUser.AssignedFacilityId.Value);
                LoadDamageLevels();
                LoadDamages(CurrentUser.AssignedFacilityId.Value);

            }
        }

        private void LoadPallets(int facilityId)
        {
            try
            {
                var pallets = _palletService.GetList()
                     .Where(x => x.FacilityId == facilityId && new List<int> { 2, 3 }.Contains(x.StatusId.Value) == false)
                    .ToList();

                chkPatllets.DataSource = pallets;
                chkPatllets.DataTextField = "PalletCode";
                chkPatllets.DataValueField = "PalletId";
                chkPatllets.DataBind();
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
            hdfShipmentId.Value = string.Empty;
            ddlDamageLevel.SelectedIndex = 0;
            txtCollectionFormNo.Text = string.Empty;
            txtReason.Text = string.Empty;
        }


        private void LoadDamages(int facilityId)
        {
            try
            {
                var damages = _damageService.GetDisplayList(_damageService.GetList().Where(x => x.facilityId == CurrentUser.AssignedFacilityId.Value && x.Repaired == false).ToList());
                gdvDamages.DataSource = damages;
                gdvDamages.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.StackTrace + ex.Message + ex.InnerException, false);
            }
        }
        private void AddDamage()
        {
            try
            {
                using (var tran = new TransactionScope())
                {
                    List<string> selectedPallets = chkPatllets.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => x.Text).ToList();

                    if (selectedPallets.Count() < 1)
                    {
                        displayMessage("You have not selected any Pallets. Please select damaged pallets from the list below", false);
                        return;
                    }
                    var palletsToAdd = _palletService.GetList().Where(x => selectedPallets.Contains(x.PalletCode)).ToList();
                    var oDamagesList = new List<Damage>();
                    foreach (var pallet in palletsToAdd)
                    {
                        var oDamage = new Damage
                        {
                            DateAdded = DateTime.Now,
                            CollectionFormNo = txtCollectionFormNo.Text,
                            DamagedPalletId = pallet.PalletId,
                            DamageLevelId = int.Parse(ddlDamageLevel.SelectedValue),
                            Reason = txtReason.Text,
                            facilityId = CurrentUser.AssignedFacilityId.Value,
                            DeckerboardsUsed = 0,
                            DP_TDP_Extracted = 0,
                            NailsUsed = 0
                        };
                        oDamagesList.Add(oDamage);
                    }

                    _damageService.Add(oDamagesList);

                    var updatedPallets = palletsToAdd.Select(c =>
                    {
                        c.StatusId = (int)PALLET_STATUS.Damaged;
                        c.LastUpdatedDate = DateTime.Now;
                        return c;
                    }).ToList();
                    _palletService.Update(updatedPallets);
                    tran.Complete();
                }

                //redirect to view shipment
                ResetForm();

                displayMessage("Record Saved Successfully", true);

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.StackTrace + ex.Message + ex.InnerException, false);
            }
        }
        //private void SelectShipment(int shipmentId)
        //{
        //    try
        //    {
        //        var oShipment = _shipmentService.GetbyId(shipmentId);
        //        hdfShipmentId.Value = shipmentId.ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Log(ex);
        //        displayMessage(ex.Message, false);
        //    }
        //}

        private void LoadDamageLevels()
        {
            ddlDamageLevel.DataSource = _damageLevelService.GetList().Where(x => new List<int> { 2, 3 }.Contains(x.DamageLevelId)).ToList();
            ddlDamageLevel.DataTextField = "DamageLevelName";
            ddlDamageLevel.DataValueField = "DamageLevelId";
            ddlDamageLevel.DataBind();
        }
        protected void gdvDamages_SelectedIndexChanged(object sender, EventArgs e)
        {
            var damageId = int.Parse(gdvDamages.SelectedDataKey["DamageId"].ToString());
            //  SelectShipment(shipmentId);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            AddDamage();
            LoadDamages(CurrentUser.AssignedFacilityId.Value);
            LoadPallets(CurrentUser.AssignedFacilityId.Value);
            ResetForm();
        }

        protected void gdvDamages_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            NavigateToFacility(e);
        }

        private void NavigateToFacility(GridViewUpdateEventArgs e)
        {
            try
            {
                var DamageId = int.Parse(e.Keys["DamageId"].ToString());
                Response.Redirect($"Repairs?query={DamageId}");
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }

    }
}