<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="PalletSetup.aspx.cs" Inherits="PalletManagement.Web.Setup.PalletSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-6">
            <div class="form-horizontal">
                <h4>Add Pallets</h4>
                <hr />
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                </asp:PlaceHolder>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="rdbSetupType" CssClass="col-md-4 control-label">Setup Options</asp:Label>
                    <div class="col-md-8">
                        <asp:RadioButtonList ID="rdbSetupType" runat="server" AutoPostBack="True" CellPadding="0" CellSpacing="2" OnSelectedIndexChanged="rdbSetupType_SelectedIndexChanged">
                            <asp:ListItem Value="0" Selected="True">Single Entry</asp:ListItem>
                            <asp:ListItem Value="1">Multiple Entry</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblStartSerial" AssociatedControlID="txtStartSerial" CssClass="col-md-4 control-label">Serial No</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtStartSerial" TextMode="SingleLine" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStartSerial" CssClass="text-danger" ErrorMessage="required." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtEndSerial" CssClass="col-md-4 control-label">End Serial No</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtEndSerial" TextMode="SingleLine" CssClass="form-control" Enabled="False" />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlCustomer" CssClass="col-md-4 control-label">Customer</asp:Label>
                    <div class="col-md-8">
                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCustomer"
                            CssClass="text-danger" ErrorMessage="required." InitialValue="0" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlPlant" CssClass="col-md-4 control-label">Plant</asp:Label>
                    <div class="col-md-8">
                        <asp:DropDownList ID="ddlPlant" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlPlant"
                            CssClass="text-danger" ErrorMessage="required." InitialValue="0" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-4 col-md-8">
                        <asp:Button runat="server" Text="Submit" CssClass="btn btn-primary" ID="btnSubmit" OnClick="btnSubmit_Click" />
                        <a href="PalletsView.aspx" runat="server" class="btn btn-default">View Pallets List</a>
                        <%--<asp:LinkButton ID="lnkViewList" runat="server" class="btn btn-default" OnClick="lnkViewList_Click">View Pallets List</asp:LinkButton>--%>
                        <asp:HiddenField ID="hdfPalletId" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    
</asp:Content>
