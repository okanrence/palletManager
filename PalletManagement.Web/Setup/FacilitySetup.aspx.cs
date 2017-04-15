using Byaxiom.Logger;
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
    public partial class FacilitySetup : System.Web.UI.Page
    {
        private readonly ICustomerServices _customerService = null;
        private readonly IFacilityServices _facilityService = null;

        public FacilitySetup()
        {
            _customerService = new CustomerServices();
            _facilityService = new FacilityServices();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCustomers();
                LoadFacilities(GetSelectedCustomer());
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (isUpdateMode())
                UpdateFacility();
            else
                AddFacility(GetSelectedCustomer());
           
            LoadFacilities(GetSelectedCustomer());
           
        }

        private int GetSelectedCustomer()
        {
            return int.Parse(ddlCustomer.SelectedValue);
        }
        private bool isUpdateMode()
        {
            if (btnSubmit.Text == "Update")
                return true;
            return false;
        }
        private void LoadFacilities(int CustomerId)
        {
            try
            {
                if (CustomerId > 0)
                    gdvFacilities.DataSource = _facilityService.GetList().Where(x => x.CustomerId == CustomerId).ToList();
                else
                    gdvFacilities.DataSource = null;

                gdvFacilities.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private void AddFacility(int CustomerId)
        {
            try
            {
                var oFacility = new Facility() { CustomerId = CustomerId, FacilityAddress = txtFacilityAddress.Text, FacilityName = txtFacilityName.Text, FacilityType = ddlFacilityType.SelectedValue };
                _facilityService.Add(oFacility);
                ResetForm();
                displayMessage("Facility Saved Successfully", true);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private void UpdateFacility()
        {
            try
            {
                var oFacility = new Facility()
                {
                    FacilityAddress = txtFacilityAddress.Text,
                    FacilityName = txtFacilityName.Text,
                    FacilityType = ddlFacilityType.SelectedValue,
                    FacilityId = int.Parse(hdfFacilityId.Value)
                };
                _facilityService.Update(oFacility);
                ResetForm();
                displayMessage("Facility Updated Successfully", true);

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
                var data = _customerService.GetList().ToList();
                ddlCustomer.DataSource = data;

                ddlCustomer.DataValueField = "CustomerId";
                ddlCustomer.DataTextField = "CustomerName";

                ddlCustomer.DataBind();

                if (Request.QueryString["query"] != null)
                {
                    ddlCustomer.SelectedValue = Request.QueryString["query"].ToString(); ;
                }
                else
                {
                    ddlCustomer.SelectedValue = "0";
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

        private void SelectFacility(EventArgs e)
        {
            try
            {
                hdfFacilityId.Value = gdvFacilities.SelectedDataKey["FacilityId"].ToString();
                txtFacilityAddress.Text = gdvFacilities.Rows[gdvFacilities.SelectedIndex].Cells[3].Text.ToString();
                txtFacilityName.Text = gdvFacilities.Rows[gdvFacilities.SelectedIndex].Cells[1].Text.ToString();
                ddlFacilityType.SelectedValue = gdvFacilities.Rows[gdvFacilities.SelectedIndex].Cells[2].Text.ToString();

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }
        protected void gdvCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectFacility(e);
            btnSubmit.Text = "Update";
        }

        private void ResetForm()
        {
            txtFacilityAddress.Text = string.Empty;
            txtFacilityName.Text = string.Empty;
            ddlFacilityType.SelectedIndex = 0;
            hdfFacilityId.Value = string.Empty;
            btnSubmit.Text = "Submit";
            ErrorMessage.Visible = false;
        }


        private void DeleteCustomer(int facilityId)
        {
            try
            {
                _facilityService.Delete(facilityId);
                ResetForm();
                displayMessage("Facility Deleted Successfully", true);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }

        protected void gdvCustomers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var facilityId = int.Parse(e.Keys["FacilityId"].ToString());
            DeleteCustomer(facilityId);
            LoadFacilities(GetSelectedCustomer());
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFacilities(GetSelectedCustomer());
        }
    }
}