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
    public partial class Repairs : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly IDamageLevelServices _damageLevelService = null;
        private readonly IDamageServices _damageService = null;
        private readonly IFacilityServices _facilityService = null;
        private static User CurrentUser = null;

        public Repairs()
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
                if (Request.QueryString["query"] != null)
                    LoadReferencedDamage();
                else
                    LoadDamages(CurrentUser.AssignedFacilityId.Value);

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
            hdfDamageId.Value = string.Empty;
            txtCollectionFormNo.Text = string.Empty;
            txtReason.Text = string.Empty;
            txtDeckerboard.Text = string.Empty;
            txtNailedUsed.Text = string.Empty;
            txtTDPExtracted.Text = string.Empty;
        }

        private void DoSearch()
        {
            try
            {
                var oDamages = _damageService.GetList();

                if (!string.IsNullOrEmpty(txtStartDate.Text))
                {
                    var startDate = DateTime.Parse(txtStartDate.Text);
                    oDamages = oDamages.Where(x => x.DateAdded >= startDate);
                }
                if (!string.IsNullOrEmpty(txtEndDate.Text))
                {
                    var endDate = DateTime.Parse(txtEndDate.Text).AddDays(1);
                    oDamages = oDamages.Where(x => x.DateAdded <= endDate);
                }

                gdvDamages.DataSource = _damageService.GetDisplayList(oDamages.ToList());
                gdvDamages.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.StackTrace + ex.Message + ex.InnerException, false);
            }

        }
        private void LoadDamages(int facilityId)
        {
            try
            {
                var damages = _damageService.GetList().Where(x => x.facilityId == CurrentUser.AssignedFacilityId.Value);
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
                gdvDamages.DataSource = _damageService.GetDisplayList(damages.ToList());
                gdvDamages.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.StackTrace + ex.Message + ex.InnerException, false);
            }
        }
        private void LoadReferencedDamage()
        {
            hdfDamageId.Value = Request.QueryString["query"].ToString();
            int damageId = int.Parse(hdfDamageId.Value);
            var oDamage = _damageService.GetList().Where(x => x.DamageId == damageId).ToList();
            gdvDamages.DataSource = _damageService.GetDisplayList(oDamage);
            gdvDamages.DataBind();
            //if (oDamage.Count() > 0)
            //{
            //    //txtStartDate.Text = oDamage[0].DateAdded.Value.ToString("dd-MM-yyyy");
            //    //txtEndDate.Text = oDamage[0].DateAdded.Value.ToString("dd-MM-yyyy");
            //    txtCollectionFormNo.Text = oDamage[0].CollectionFormNo;
            //    txtReason.Text = oDamage[0].Reason;
            //    hdfPalletId.Value = oDamage[0].DamagedPalletId.ToString();
            //}

        }
        private void SaveRepair()
        {
            try
            {
                
                using (var tran = new TransactionScope())
                {

                    var damage = new Damage
                    {
                        DamageId = int.Parse(hdfDamageId.Value),
                        DateRepaired = DateTime.Now,
                        DeckerboardsUsed = !string.IsNullOrEmpty(txtDeckerboard.Text) ? int.Parse(txtDeckerboard.Text) : 0,
                        DP_TDP_Extracted = !string.IsNullOrEmpty(txtTDPExtracted.Text) ? int.Parse(txtTDPExtracted.Text) : 0,
                        LastUpdatedDate = DateTime.Now,
                        NailsUsed = !string.IsNullOrEmpty(txtNailedUsed.Text) ? int.Parse(txtNailedUsed.Text) : 0,
                        Repaired = true,
                        Reason = txtReason.Text,
                        CollectionFormNo = txtCollectionFormNo.Text,
                        DamagedPalletId = int.Parse(hdfPalletId.Value)

                    };
                    _damageService.Update(damage);
                    _palletService.RepairPallet(new Pallet() { PalletId = damage.DamagedPalletId, StatusId = (int)PALLET_STATUS.Available, LastUpdatedDate = DateTime.Now });
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



        protected void gdvDamages_SelectedIndexChanged(object sender, EventArgs e)
        {
            var damageId = int.Parse(gdvDamages.SelectedDataKey["DamageId"].ToString());
            SelectDamage(damageId);
        }

        private void SelectDamage(int damageId)
        {
            try
            {
                var oDamage = _damageService.GetbyId(damageId);


                hdfDamageId.Value = damageId.ToString();
                hdfPalletId.Value = oDamage.DamagedPalletId.ToString();
                txtCollectionFormNo.Text = oDamage.CollectionFormNo;
                txtReason.Text = oDamage.Reason;
                if (oDamage.Repaired || oDamage.DamageLevelId == (int)DAMAGE_LEVEL.Total_Damage)
                    btnSubmit.Enabled = false;
                else
                    btnSubmit.Enabled = true;

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveRepair();
            LoadDamages(CurrentUser.AssignedFacilityId.Value);
            ResetForm();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        //protected void gdvDamages_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    NavigateToFacility(e);
        //}

        //private void NavigateToFacility(GridViewUpdateEventArgs e)
        //{
        //    try
        //    {
        //        var DamageId = int.Parse(e.Keys["DamageId"].ToString());
        //        Response.Redirect($"Repairs?query={DamageId}");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Log(ex);
        //        displayMessage(ex.Message, false);

        //    }
        //}

    }
}