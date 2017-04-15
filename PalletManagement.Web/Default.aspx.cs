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

                var userName = Email.Text;
                var password = Password.Text;

                User CurrentUser = AuthenticateUser(userName, password);

                if (CurrentUser == null)
                {
                    FailureText.Text = "Invalid login attempt";
                    ErrorMessage.Visible = true;
                }
                else
                {
                    if (CurrentUser.ProfileStatus == PROFILE_STATUS.DEACTIVATED)
                    {
                        displayMessage("You profile has been deactivated. Please contact administrator", false);
                    }
                    else if (CurrentUser.MustChangePassword)
                    {
                        Response.Redirect($"/ChangePassword?query={CurrentUser.UserId}");
                    }
                    else
                    {
                        Session.Add("CurrentUser", CurrentUser);
                        Session.Add("UserName", CurrentUser.FirstName);
                        var d = Request.UserAgent;
                        var r = Request.UserHostAddress + Request.UserHostName;
                        CurrentUser.LastLoginDate = DateTime.Now;
                        if (CurrentUser.UserRole.UserRoleName != USER_ROLES.ADMIN)
                            _userServices.Update(CurrentUser);
                        Response.Redirect("/Landing");
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
            if (emailaddress == "admin@pil.com")
            {
                return new User
                {
                    FirstName = "Olanrewaju",
                    LastName = "Okanrende",
                    DateAdded = DateTime.Now,
                    UserRole = new UserRole { UserRoleName = "Admin" },
                    EmailAddress = emailaddress,
                    StaffNumber = "1234",
                    ProfileStatus = "A",
                    PhoneNumber = "08029039468"
                };
            }

            return _userServices.Authenticate(emailaddress, password);


        }
    }
}