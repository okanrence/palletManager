<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="UsersSetup.aspx.cs" Inherits="PalletManagement.Web.Setup.UserSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <h4>Setup Users</h4>
        <hr />
        <div class="col-md-6">
            <div class="form-horizontal">

                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                </asp:PlaceHolder>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtFirstName" CssClass="col-md-4 control-label">First Name</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtFirstName" CssClass="form-control" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName"
                            CssClass="text-danger" ErrorMessage="The first name field is required." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtLastName" CssClass="col-md-4 control-label">Last Name</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtLastName" CssClass="form-control" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName"
                            CssClass="text-danger" ErrorMessage="The last name field is required." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtStaffNumber" CssClass="col-md-4 control-label">Staff Number</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtStaffNumber" CssClass="form-control" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStaffNumber"
                            CssClass="text-danger" ErrorMessage="The staff number field is required." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtEmailAddress" CssClass="col-md-4 control-label">Email Address</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtEmailAddress" CssClass="form-control" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmailAddress"
                            CssClass="text-danger" ErrorMessage="The email address field is required." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtPhoneNumber" CssClass="col-md-4 control-label">PhoneNumber</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtPhoneNumber" CssClass="form-control" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhoneNumber"
                            CssClass="text-danger" ErrorMessage="The phone number field is required." />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlCustomer" CssClass="col-md-4 control-label">Assigned Customer</asp:Label>
                    <div class="col-md-7">
                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlFacilities" CssClass="col-md-4 control-label">Assigned Facility</asp:Label>
                    <div class="col-md-7">
                        <asp:DropDownList ID="ddlFacilities" runat="server" CssClass="form-control">
                        </asp:DropDownList>

                    </div>
                </div>

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlUserRole" CssClass="col-md-4 control-label">Role</asp:Label>
                    <div class="col-md-7">
                        <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlUserRole"
                            CssClass="text-danger" ErrorMessage="The role field is required." InitialValue="0" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlProfileStatus" CssClass="col-md-4 control-label">Profile Status</asp:Label>
                    <div class="col-md-7">
                        <asp:DropDownList ID="ddlProfileStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                            <asp:ListItem Value="A">Active</asp:ListItem>
                            <asp:ListItem Value="D">Deactivated</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlProfileStatus"
                            CssClass="text-danger" ErrorMessage="The profile status field is required." InitialValue="0" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-8 col-md-4">
                        <asp:Button runat="server" Text="Clear" CssClass="btn btn-default" ID="btnClear" />
                        <asp:Button runat="server" Text="Submit" CssClass="btn btn-primary" ID="btnSubmit" OnClick="btnSubmit_Click" />
                        <asp:HiddenField ID="hdfUserId" runat="server" />
                    </div>
                </div>
                <hr />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="row form-group col-md-10 col-lg-12 bs-component">
            <asp:GridView ID="gdvUsers" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="UserId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal" OnSelectedIndexChanged="gdvUsers_SelectedIndexChanged" OnRowDeleting="gdvUsers_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="UserId" HeaderText="ID" />
                    <asp:BoundField DataField="StaffNumber" HeaderText="Staff Number" />
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                    <%--<asp:BoundField DataField="AssignedCustomer" HeaderText="Assigned Customer" />--%>
                    <asp:BoundField DataField="AssignedFacility" HeaderText="Facility" />
                    <asp:BoundField DataField="UserRoleName" HeaderText="Role" />
                    <asp:BoundField DataField="ProfileStatus" HeaderText="Status" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="select" CausesValidation="False">edit</asp:LinkButton>
                            &nbsp;|
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="delete" CausesValidation="False">delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#1A4874" ForeColor="White" />
            </asp:GridView>
        </div>
    </div>

</asp:Content>
