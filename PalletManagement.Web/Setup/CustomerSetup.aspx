<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CustomerSetup.aspx.cs" Inherits="PalletManagement.Web.Setup.CustomerSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-6">
            <div class="form-horizontal">
                <h4>Setup Company</h4>
                <hr />
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                </asp:PlaceHolder>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtCustomerName" CssClass="col-md-4 control-label">Customer Name</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtCustomerName" CssClass="form-control" TextMode="SingleLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCustomerName"
                            CssClass="text-danger" ErrorMessage="The customer name field is required." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtAddress" CssClass="col-md-4 control-label">Address</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtAddress" TextMode="MultiLine" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress" CssClass="text-danger" ErrorMessage="The address field is required." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtEMailAddress" CssClass="col-md-4 control-label">Email Address</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtEmailAddress" CssClass="form-control" TextMode="Email" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtContactPerson" CssClass="col-md-4 control-label">Contact Person</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtContactPerson" CssClass="form-control" TextMode="SingleLine" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtPhoneNumber" CssClass="col-md-4 control-label">PhoneNumber</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtPhoneNumber" CssClass="form-control" TextMode="SingleLine" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-4 col-md-8">
                        <asp:Button runat="server" Text="Submit" CssClass="btn btn-primary" ID="btnSubmit" OnClick="btnSubmit_Click" />
                        <asp:HiddenField ID="hdfCustomerId" runat="server" />
                    </div>
                </div>
                <hr />


            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-10 form-group col-lg-12 bs-component">
            <asp:GridView ID="gdvCustomers" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="CustomerId" EmptyDataText="No Records Found." OnSelectedIndexChanged="gdvCustomers_SelectedIndexChanged" ShowHeaderWhenEmpty="True" OnRowDeleting="gdvCustomers_RowDeleting" GridLines="Horizontal" OnRowUpdating="gdvCustomers_RowUpdating">
                <Columns>
                    <asp:BoundField DataField="CustomerId" HeaderText="ID" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                    <asp:BoundField DataField="Address" HeaderText="Address" />
                    <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
                    <asp:BoundField DataField="ContactPerson" HeaderText="ContactPerson" />
                    <asp:BoundField DataField="PhoneNumber" HeaderText="PhoneNumber" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="select" CausesValidation="False">[edit]</asp:LinkButton>
                            &nbsp;|
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="delete" CausesValidation="False">[delete]</asp:LinkButton>
                            &nbsp;|
                                            <asp:LinkButton ID="lnkFacilities" runat="server" CommandName="update" CausesValidation="False">[facilities]</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#1A4874" ForeColor="White" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
