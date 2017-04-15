<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="PalletsView.aspx.cs" Inherits="PalletManagement.Web.Setup.PalletsView" %>

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

                    <asp:Label runat="server" AssociatedControlID="ddlCustomer" CssClass="col-md-2 control-label">Customer</asp:Label>
                    <div class="col-md-5">
                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCustomer"
                            CssClass="text-danger" ErrorMessage="required." InitialValue="0" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlPlant" CssClass="col-md-2 control-label">Plant</asp:Label>
                    <div class="col-md-5">
                        <asp:DropDownList ID="ddlPlant" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlPlant"
                            CssClass="text-danger" ErrorMessage="required." InitialValue="0" />
                    </div>
                     <div class="col-md-2">
                    <asp:Button runat="server" Text="Load" CssClass="btn btn-primary" ID="btnSearch" OnClick="btnSearch_Click" /></div>
                </div>

                <asp:HiddenField ID="hdfPalletId" runat="server" />

                <div class="row">
                    <div class="col-md-10">
                        <div class="form-group">
                            <div class="col-lg-12">
                                <div class="bs-component">
                                    <asp:GridView ID="gdvPallets" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="PalletId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal">
                                        <Columns>
                                            <asp:BoundField DataField="PalletId" HeaderText="ID" />
                                            <asp:BoundField DataField="PalletCode" HeaderText="Pallet Code" />
                                            <asp:BoundField DataField="StatusName" HeaderText="Status" />
                                            <asp:BoundField DataField="FacilityName" HeaderText="Current Location" />
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
                        </div>
                    </div>
                </div>
                
            </div>
        </div>
    </div>


</asp:Content>
