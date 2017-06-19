<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="PalletsReport.aspx.cs" Inherits="PalletManagement.Web.Reports.PalletsReport" EnableEventValidation="false" %>

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
                    <asp:Label runat="server" AssociatedControlID="ddlStatus" CssClass="col-md-4 control-label">Status</asp:Label>
                    <div class="col-md-7">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlCustomer" CssClass="col-md-4 control-label">Customer</asp:Label>
                    <div class="col-md-7">
                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

            </div>
        </div>
        <div class="col-md-6">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtPalletCode" CssClass="col-md-4 control-label">PalletCode</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtPalletCode" CssClass="form-control datepicker" TextMode="SingleLine" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlFacilities" CssClass="col-md-4 control-label">Facility</asp:Label>
                    <div class="col-md-7">
                        <asp:DropDownList ID="ddlFacilities" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-8 col-md-4">
                        <asp:Button runat="server" Text="Search" CssClass="btn btn-primary" ID="btnSubmit" OnClick="btnSubmit_Click" CausesValidation="False" />
                        <asp:HiddenField ID="hdfUserId" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" >
        <asp:Panel ID="pnlRepairs" runat="server" Height="500px" ScrollBars="vertical" BorderStyle="Solid" BorderColor="#CCCCCC" BorderWidth="1px">
            <div class="col-md-10 form-group col-lg-12 bs-component" style="padding-top: 20px">
                <asp:LinkButton ID="lnkOverallSummaryExcel" runat="server" CssClass="btn btn-default pull-right" OnClick="lnkOverallSummaryExcel_Click">Export to Excel</asp:LinkButton>
                <asp:GridView ID="gdvPallets" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="PalletId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="SN" />
                        <asp:BoundField DataField="PalletId" HeaderText="PalletId" Visible="false" />
                        <asp:BoundField DataField="PalletCode" HeaderText="PalletCode" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="CurentLocation" HeaderText="CurentLocation" />
                        <asp:BoundField DataField="LastMovementDate" HeaderText="Last Shipment Date" />
                    </Columns>
                    <HeaderStyle BackColor="#1A4874" ForeColor="White" />
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>

    

</asp:Content>
