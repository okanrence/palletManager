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
    public partial class PalletSetup : System.Web.UI.Page
    {
        private readonly IPalletServices _palletService = null;
        private readonly ICustomerServices _customerService = null;

        public PalletSetup()
        {
            _palletService = new PalletServices();
            _customerService = new CustomerServices();
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
            ErrorMessage.Visible = false;
            if (isSingleMode())
                AddSinglePallet();
            else
                AddMulitiplePallets();

            // LoadPallets();
        }

        private bool isSingleMode()
        {
            if (rdbSetupType.SelectedIndex == 0)
                return true;
            return false;
        }
      
        private void AddSinglePallet()
        {
            try
            {
                if (alreadyExists(txtStartSerial.Text))
                {
                    displayMessage("Pallet already exists", false);
                    return;
                }

                var oPallet = new Pallet()
                {
                    FacilityId = int.Parse(ddlPlant.SelectedValue),
                    DateAdded = DateTime.Now,
                    PalletCode = txtStartSerial.Text,
                    StatusId = (int)PALLET_STATUS.Available
                };

                _palletService.Add(oPallet);
                ResetForm();
                displayMessage("Pallet Added Successfully", true);


            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }

        private void AddMulitiplePallets()
        {
            var alreadyAddedPallets = string.Empty;

            var startSerial = int.Parse(txtStartSerial.Text);
            var endSerial = int.Parse(txtEndSerial.Text);

            if (endSerial > startSerial){
                displayMessage("End Serial cannot be greated than Start Serial", false);
                return;
            }
            var PalletList = new List<Pallet>();
            for (var i = startSerial; i <= endSerial; i++)
            {
                var oPallet = new Pallet()
                {
                    FacilityId = int.Parse(ddlPlant.SelectedValue),
                    DateAdded = DateTime.Now,
                    PalletCode = i.ToString(),
                    StatusId = (int)PALLET_STATUS.Available
                };
                if (!alreadyExists(oPallet.PalletCode))
                {
                    PalletList.Add(oPallet);
                }
                else
                {
                    alreadyAddedPallets += $"{oPallet.PalletCode}|";
                }
            }
            if (alreadyAddedPallets.EndsWith("|"))
                alreadyAddedPallets = alreadyAddedPallets.Remove(alreadyAddedPallets.Length - 1, 1);

            var appendDisplay = string.Empty;
            if (!string.IsNullOrEmpty(alreadyAddedPallets))
                appendDisplay = $"The following pallets already exists: { alreadyAddedPallets }";

            var no = _palletService.Add(PalletList);
            ResetForm();
            displayMessage($"{no} Pallets Added Successfully. {appendDisplay}", true);
        }
        private bool alreadyExists(string palletCode)
        {
            return _palletService.GetList().Where(x => x.PalletCode == palletCode).Any();
        }
        private void UpdatePallet()
        {
            try
            {
                var oPallet = new Pallet()
                {
                    //Address = txtAddress.Text,
                    //PalletName = txtPalletName.Text,
                    //EmailAddress = txtEmailAddress.Text,
                    //PhoneNumber = txtPhoneNumber.Text,
                    //ContactPerson = txtContactPerson.Text,
                    //PalletId = int.Parse(hdfPalletId.Value)
                };
                _palletService.Update(oPallet);
                ResetForm();
                displayMessage("Pallet Updated Successfully", true);

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
            btnSubmit.Text = "Update";
        }

        private void ResetForm()
        {
            txtEndSerial.Text = string.Empty;
            txtStartSerial.Text = string.Empty;
            lblStartSerial.Text = "Start Serial No";
            txtEndSerial.Enabled = true;
            ddlCustomer.SelectedIndex = 0;
            hdfPalletId.Value = string.Empty;
            //btnSubmit.Text = "Submit";
            ErrorMessage.Visible = false;
            //Response.Redirect(Page.Request.RawUrl);
        }


        private void DeletePallet(GridViewDeleteEventArgs e)
        {
            try
            {

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

        protected void rdbSetupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSingleMode())
            {
                txtEndSerial.Enabled = false;
                lblStartSerial.Text = "Serial No";

            }
            else
            {
                txtEndSerial.Enabled = true;
                lblStartSerial.Text = "Start Serial No";
            }

            txtEndSerial.Text = string.Empty;
            txtStartSerial.Text = string.Empty;
            ddlCustomer.SelectedIndex = 0;
            ddlPlant.SelectedIndex = 0;
        }

    }
}