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
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="rdbSetupType" runat="server" AutoPostBack="True" CellPadding="0" CellSpacing="2" OnSelectedIndexChanged="rdbSetupType_SelectedIndexChanged">
                            <asp:ListItem Value="0">Single Entry</asp:ListItem>
                            <asp:ListItem Value="1" Selected="True">Upload from Excel</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="singleEntry" runat="server">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblStartSerial" AssociatedControlID="txtStartSerial" CssClass="col-md-4 control-label">Serial No</asp:Label>
                            <div class="col-md-8">
                                <asp:TextBox runat="server" ID="txtStartSerial" TextMode="SingleLine" CssClass="form-control" />
                                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtStartSerial" CssClass="text-danger" ErrorMessage="required." />--%>
                            </div>
                        </div>
                 </asp:View>
                    <asp:View ID="multipleEntry" runat="server">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblExcel" AssociatedControlID="txtStartSerial" CssClass="col-md-4 control-label">Select Excel File</asp:Label>
                            <div class="col-md-4">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnExtract" runat="server" Text="Extract File" OnClick="btnExtract_Click" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
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
        <div class="col-md-6">
            <div class="form-horizontal"></div>
        </div>
        <asp:GridView ID="gdvPallets" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="PalletId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal">
            <Columns>
                <asp:BoundField DataField="No" HeaderText="No" />
                <asp:BoundField DataField="PalletCode" HeaderText="Pallet Code" />
            </Columns>
            <HeaderStyle BackColor="#1A4874" ForeColor="White" />
        </asp:GridView>
    </div>


</asp:Content>
