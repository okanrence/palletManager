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
    public partial class ChangePassword : Page
    {
        private readonly IUserServices _userServices;

        public ChangePassword()
        {
            //_userServices = new UserServices();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["query"] != null)
            {
                hdfUserId.Value = Request.QueryString["query"].ToString(); ;
            }
            else
            {
                var CurrentUser = (User)Session["CurrentUser"];
                if (CurrentUser != null)
                    Response.Redirect($"ChangePassword?query={CurrentUser.UserId}");
                else
                    Response.Redirect("default");
            }
        }

        protected void ChangePass(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    Session.Remove("CurrentUser");

                    var userId = int.Parse(hdfUserId.Value);
                    var oldPassword = txtPassword.Text.Trim();
                    var newPassword = txtNewPassword.Text.Trim();
                    var confirmPassword = txtConfirmPassword.Text.Trim();
                    if (ValidateOldPassword(userId, oldPassword))
                    {
                        if (ComparePasswords(newPassword, confirmPassword))
                        {
                            if (UpdatePassword(userId, newPassword))
                            {
                                displayMessage("Password changed successfully. Kindly login again with your new password", true);
                                Session.Remove("CurrentUser");
                                Session.Remove("Username");
                            }
                            else
                            {
                                displayMessage("Password changed failed. Kindly retry", false);
                            }
                        }
                        else
                        {
                            displayMessage("Password do not match. Kindly retry", false);

                        }
                    }
                    else
                    {
                        displayMessage("Invalid logon credentials", false);

                    }

                }
                catch(Exception ex)
                {
                    LogHelper.Log(ex);
                    displayMessage("An unexpected error occured", false);

                }



            }
        }

        private bool ValidateOldPassword(int userId, string password)
        {
            return _userServices.GetList().Where(x => x.UserId == userId && x.Password == password).FirstOrDefault() != null;
        }

        private void displayMessage(string message, bool isSuccessMsg)
        {
            ErrorMessage.Visible = true;
            if (isSuccessMsg)
                FailureText.Text = $"{message}";
            else
                FailureText.Text = $"ERROR:{message}";

        }
        private bool UpdatePassword(int userId, string newPassword)
        {
            return _userServices.UpdatePassword(userId, newPassword);
        }

        private bool ComparePasswords(string newPassword, string confirmpassword)
        {
            if (newPassword.SequenceEqual(confirmpassword))
                return true;
            return false;
        }
    }
}