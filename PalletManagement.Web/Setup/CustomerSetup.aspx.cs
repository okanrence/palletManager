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
    public partial class CustomerSetup : System.Web.UI.Page
    {
        private readonly ICustomerServices _customerService = null;

        public CustomerSetup()
        {
            _customerService = new CustomerServices();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if ()
            LoadCustomers();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (isUpdateMode())
                UpdateCustomer();
            else
                AddCustomer();

            LoadCustomers();
        }

        private bool isUpdateMode()
        {
            if (btnSubmit.Text == "Update")
                return true;
            return false;
        }
        private void LoadCustomers()
        {
            try
            {
                gdvCustomers.DataSource = _customerService.GetList().ToList();
                gdvCustomers.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }

        }

        private void AddCustomer()
        {
            try
            {
                var oCustomer = new Customer()
                {
                    Address = txtAddress.Text,
                    CustomerName = txtCustomerName.Text,
                    EmailAddress = txtEmailAddress.Text,
                    ContactPerson = txtContactPerson.Text,
                    PhoneNumber = txtPhoneNumber.Text, DateAdded = DateTime.Now
                };

                _customerService.Add(oCustomer);
                ResetForm();
                displayMessage("Customer Added Successfully", true);


            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }

        private void UpdateCustomer()
        {
            try
            {
                var oCustomer = new Customer()
                {
                    Address = txtAddress.Text,
                    CustomerName = txtCustomerName.Text,
                    EmailAddress = txtEmailAddress.Text,
                    PhoneNumber = txtPhoneNumber.Text,
                    ContactPerson = txtContactPerson.Text,
                    CustomerId = int.Parse(hdfCustomerId.Value)
                };
                _customerService.Update(oCustomer);
                ResetForm();
                displayMessage("Customer Updated Successfully", true);

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

        private void SelectCustomer(EventArgs e)
        {
            try
            {
                hdfCustomerId.Value = gdvCustomers.SelectedDataKey["CustomerId"].ToString();
                txtCustomerName.Text = gdvCustomers.Rows[gdvCustomers.SelectedIndex].Cells[1].Text.ToString();
                txtAddress.Text = gdvCustomers.Rows[gdvCustomers.SelectedIndex].Cells[2].Text.ToString();
                txtEmailAddress.Text = gdvCustomers.Rows[gdvCustomers.SelectedIndex].Cells[3].Text.ToString();
                txtContactPerson.Text = gdvCustomers.Rows[gdvCustomers.SelectedIndex].Cells[4].Text.ToString();
                txtPhoneNumber.Text = gdvCustomers.Rows[gdvCustomers.SelectedIndex].Cells[5].Text.ToString();

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }
        protected void gdvCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectCustomer(e);
            btnSubmit.Text = "Update";
        }

        private void ResetForm()
        {
            txtAddress.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;
            txtContactPerson.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            hdfCustomerId.Value = string.Empty;
            btnSubmit.Text = "Submit";
            ErrorMessage.Visible = false;
            //Response.Redirect(Page.Request.RawUrl);
        }


        private void DeleteCustomer(GridViewDeleteEventArgs e)
        {
            try
            {
                var customerID = int.Parse(e.Keys["CustomerId"].ToString());
                var Customer = _customerService.GetbyId(customerID);

                if (Customer.Facilities == null || Customer.Facilities.Count < 1)
                {
                    _customerService.Delete(customerID);
                    ResetForm();
                    displayMessage("Customer Deleted Successfully", true);
                }
                else
                {
                    displayMessage("Unable to delete customer. Kindly delete customer facilities", false);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }

        protected void gdvCustomers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteCustomer(e);
            LoadCustomers();
        }

        protected void gdvCustomers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            NavigateToFacility(e);
        }

        private void NavigateToFacility(GridViewUpdateEventArgs e)
        {
            try
            {
                var customerID = int.Parse(e.Keys["CustomerId"].ToString());
                Response.Redirect($"FacilitySetup?query={customerID}");
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }
    }
}