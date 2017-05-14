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
    public partial class UserSetup : System.Web.UI.Page
    {
        private readonly IUserServices _userService = null;
        private readonly IUserRoleServices _userRoleService = null;
        private readonly ICustomerServices _customerService = null;

        public UserSetup()
        {
            _userService = new UserServices();
            _userRoleService = new UserRoleServices();
            _customerService = new CustomerServices();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserRoles();
                LoadUsers();
                LoadCustomers();
            }
        }
        private void LoadUserRoles()
        {
            try
            {
                ddlUserRole.DataSource = _userRoleService.GetList().ToList();
                ddlUserRole.DataValueField = "UserRoleId";
                ddlUserRole.DataTextField = "UserRoleName";
                ddlUserRole.DataBind();
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
                ddlCustomer.DataValueField = "CustomerId";
                ddlCustomer.DataTextField = "CustomerName";
                ddlCustomer.DataBind();
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
                if (ddlCustomer.SelectedIndex > 0)
                {
                    ddlFacilities.DataSource = _customerService
                                    .GetList()
                                    .FirstOrDefault(x => x.CustomerId == customerId)
                        ?.Facilities
                                    .ToList();
                    ddlFacilities.DataTextField = "FacilityName";
                    ddlFacilities.DataValueField = "FacilityId";
                    ddlFacilities.DataBind();
                }
                else
                {
                    ddlFacilities.Items.Clear();
                    // ddlFacilities.DataBind();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }




        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsUpdateMode())
                UpdateUser();
            else
                AddUser();

            LoadUsers();
        }

        private bool IsUpdateMode()
        {
            return btnSubmit.Text == @"Update";
        }
        private void LoadUsers()
        {
            try
            {

                var oUsers = _userService.GetList().ToList();
                gdvUsers.DataSource = _userService.GetDisplayList(oUsers); ;
                gdvUsers.DataBind();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);
            }
        }

        private void AddUser()
        {
            try
            {
                int? assignedFacilityId = null;

                if (ddlFacilities.SelectedIndex >= 0)
                    assignedFacilityId = int.Parse(ddlFacilities.SelectedValue);

                var oUser = new User()
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    EmailAddress = txtEmailAddress.Text.Trim(),
                    Password = "default@123",
                    PhoneNumber = txtPhoneNumber.Text.Trim(),
                    DateAdded = DateTime.Now,
                    ProfileStatus = ddlProfileStatus.SelectedValue,
                    StaffNumber = txtStaffNumber.Text.Trim(),
                    UserRoleId = int.Parse(ddlUserRole.SelectedValue),
                    AssignedFacilityId = assignedFacilityId
                };

                _userService.Add(oUser);
                ResetForm();
                displayMessage("User Added Successfully", true);

            }

            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }

        private void UpdateUser()
        {
            try
            {
                int? assignedFacilityId = null;

                if (ddlFacilities.SelectedIndex >= 0)
                    assignedFacilityId = int.Parse(ddlFacilities.SelectedValue);

                var oUser = new User()
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    EmailAddress = txtEmailAddress.Text.Trim(),
                    PhoneNumber = txtPhoneNumber.Text.Trim(),
                    ProfileStatus = ddlProfileStatus.SelectedValue,
                    StaffNumber = txtStaffNumber.Text.Trim(),
                    UserRoleId = int.Parse(ddlUserRole.SelectedValue),
                    UserId = int.Parse(hdfUserId.Value),
                    AssignedFacilityId = assignedFacilityId
                };

                if (ddlProfileStatus.SelectedValue == "D")
                    oUser.DateDeactivated = DateTime.Now;

                _userService.Update(oUser);

                ResetForm();
                displayMessage("User Updated Successfully", true);

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
            FailureText.Text = isSuccessMsg ? $"{message}" : $"ERROR:{message}";
        }

        private void SelectUser(int userId)
        {
            try
            {

                var oUser = _userService.GetbyId(userId);
                txtEmailAddress.Text = oUser.EmailAddress;
                txtFirstName.Text = oUser.FirstName;
                txtLastName.Text = oUser.LastName;
                txtStaffNumber.Text = oUser.StaffNumber;
                txtPhoneNumber.Text = oUser.PhoneNumber;
                ddlProfileStatus.SelectedValue = oUser.ProfileStatus;
                ddlUserRole.SelectedValue = oUser.UserRoleId.ToString();
                ddlCustomer.SelectedValue = oUser.AssignedFacility == null ? "0" : oUser.AssignedFacility.CustomerId.ToString();
                LoadFacilities(oUser.AssignedFacility.CustomerId);
                ddlFacilities.SelectedValue = oUser.AssignedFacility == null ? "0" : oUser.AssignedFacilityId.ToString();

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }
        protected void gdvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdfUserId.Value = gdvUsers.SelectedDataKey["UserId"].ToString();
            SelectUser(int.Parse(hdfUserId.Value));
            btnSubmit.Text = "Update";
        }

        private void ResetForm()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            ddlProfileStatus.SelectedValue = string.Empty;
            txtStaffNumber.Text = string.Empty;
            ddlUserRole.SelectedIndex = 0;
            ddlProfileStatus.SelectedIndex = 0;
        }


        private void DeleteUser(int userId)
        {
            try
            {

                _userService.Delete(userId);
                ResetForm();
                displayMessage("User Deleted Successfully", true);

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                displayMessage(ex.Message, false);

            }
        }

        protected void gdvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var UserID = int.Parse(e.Keys["UserId"].ToString());
            DeleteUser(UserID);
            LoadUsers();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFacilities(int.Parse(ddlCustomer.SelectedValue));
        }

        //protected void gdvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    NavigateToFacility(e);
        //}

        //private void NavigateToFacility(GridViewUpdateEventArgs e)
        //{
        //    try
        //    {
        //        var UserID = int.Parse(e.Keys["UserId"].ToString());
        //        Response.Redirect($"FacilitySetup?query={UserID}");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Log(ex);
        //        //displayMessage(ex.Message, false);

        //    }
        //}




    }
}