using Byaxiom.Logger;
using MyAppTools.Services;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PalletManagement.Web.Setup
{
    public partial class PalletsView : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly ICustomerServices _customerService = null;
        private readonly IFacilityServices _facilityService = null;

        public PalletsView()
        {
            _palletService = new PalletServices();
            _customerService = new CustomerServices();
            _facilityService = new FacilityServices();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomers();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {


        }

        private void LoadPallets(int facilityId)
        {
            try
            {
                var oPallets = _palletService.GetList().Where(x => x.FacilityId == facilityId).ToList();
                gdvPallets.DataSource = _palletService.GetDisplayList(oPallets);
                gdvPallets.DataBind();
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

        private void SelectPallet(EventArgs e)
        {
            try
            {
                //hdfPalletId.Value = gdvPallets.SelectedDataKey["PalletId"].ToString();
                //txtPalletName.Text = gdvPallets.Rows[gdvPallets.SelectedIndex].Cells[1].Text.ToString();
                //txtAddress.Text = gdvPallets.Rows[gdvPallets.SelectedIndex].Cells[2].Text.ToString();
                //txtEmailAddress.Text = gdvPallets.Rows[gdvPallets.SelectedIndex].Cells[3].Text.ToString();
                //txtContactPerson.Text = gdvPallets.Rows[gdvPallets.SelectedIndex].Cells[4].Text.ToString();
                //txtPhoneNumber.Text = gdvPallets.Rows[gdvPallets.SelectedIndex].Cells[5].Text.ToString();

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }
        protected void gdvPallets_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectPallet(e);
        }


        //private void DeletePallet(GridViewDeleteEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Log(ex);
        //        displayMessage(ex.Message, false);

        //    }
        //}

        //protected void gdvPallets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    DeletePallet(e);
        //    LoadPallets();
        //}

        //protected void gdvPallets_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    NavigateToFacility(e);
        //}

        //private void NavigateToFacility(GridViewUpdateEventArgs e)
        //{
        //    try
        //    {
        //        var PalletID = int.Parse(e.Keys["PalletId"].ToString());
        //        Response.Redirect($"FacilitySetup?query={PalletID}");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Log(ex);
        //        displayMessage(ex.Message, false);

        //    }
        //}

        private void LoadFacilities(int customerId)
        {
            try
            {
                ddlPlant.DataSource = _customerService.GetList()
                    .Where(x => x.CustomerId == customerId)
                    .FirstOrDefault().Facilities
                    .Where(x => x.FacilityType == FACILITY_TYPES.PLANT)
                    .ToList();
                ddlPlant.DataTextField = "FacilityName";
                ddlPlant.DataValueField = "FacilityId";
                ddlPlant.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private void LoadCustomers()
        {
            try
            {
                ddlCustomer.DataSource = _customerService.GetList().ToList();
                ddlCustomer.DataTextField = "CustomerName";
                ddlCustomer.DataValueField = "CustomerId";
                ddlCustomer.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFacilities(int.Parse(ddlCustomer.SelectedValue));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var plantId = int.Parse(ddlPlant.SelectedValue);
            if (plantId >= 1)
            {
                LoadPallets(plantId);

            }
        }
    }
}