<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/SiteOuter.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="PalletManagement.Web.ChangePassword" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
  
    <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                    <h4>Change your password</h4>
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtPassword" CssClass="col-md-3 control-label">Old Password</asp:Label>
                        <div class="col-md-9">
                            <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="text-danger" ErrorMessage="The password field is required." />
                        </div>
                    </div>

                     <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtNewPassword" CssClass="col-md-3 control-label">Password</asp:Label>
                        <div class="col-md-9">
                            <asp:TextBox runat="server" ID="txtNewPassword" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNewPassword" CssClass="text-danger" ErrorMessage="The new password field is required." />
                        </div>
                    </div>

                     <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtConfirmPassword" CssClass="col-md-3 control-label">Confirm Password</asp:Label>
                         <asp:HiddenField ID="hdfUserId" runat="server" />
                        <div class="col-md-9">
                            <asp:TextBox runat="server" ID="txtConfirmPassword" TextMode="Password" CssClass="form-control" />
                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The confirm password field is required." />--%>
                            <asp:CompareValidator runat="server" CssClass="text-danger" ErrorMessage="Password do not match" ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword"></asp:CompareValidator>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="ChangePass" Text="Change" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>