using Byaxiom.Logger;
using PalletManagement.Core.Domain;
using PalletManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PalletManagement.Web
{
    public partial class _Default : Page
    {
        private readonly IUserServices _userServices;

        public _Default()
        {
            _userServices = new UserServices();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Session.Remove("CurrentUser");
                LogHelper.Info("Here");
                var userName = Email.Text;
                var password = Password.Text;

                var currentUser = AuthenticateUser(userName, password);

                if (currentUser == null)
                {
                    FailureText.Text = @"Invalid login attempt";
                    ErrorMessage.Visible = true;
                }
                else
                {
                    if (currentUser.ProfileStatus == PROFILE_STATUS.DEACTIVATED)
                    {
                        displayMessage("You profile has been deactivated. Please contact administrator", false);
                    }
                    else if (currentUser.MustChangePassword)
                    {
                        Response.Redirect($"/ChangePassword?query={currentUser.UserId}");
                    }
                    else
                    {
                        Session.Add("CurrentUser", currentUser );
                        var facilityRole = currentUser.UserRoleId == (int)USER_ROLES.Admin ? "Administrator" : $"{currentUser.AssignedFacility?.FacilityName} | {currentUser?.UserRole?.UserRoleName}";
                        Session.Add("UserDetails", $"{currentUser.FirstName} | {facilityRole}");
                        //var d = Request.UserAgent;
                        //var r = Request.UserHostAddress + Request.UserHostName;
                        currentUser.LastLoginDate = DateTime.Now;
                        //if (currentUser.UserRoleId != (int)USER_ROLES.Admin)
                        //    _userServices.Update(currentUser);
                        Response.Redirect("~/Landing");
                    }

                }
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
        private User AuthenticateUser(string emailaddress, string password)
        {
            return _userServices.Authenticate(emailaddress, password);
        }
    }
}