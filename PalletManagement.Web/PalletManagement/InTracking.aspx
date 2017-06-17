<%@ Page Title="" Language="C#" MasterPageFile="~/SiteOperator.Master" AutoEventWireup="true" CodeBehind="InTracking.aspx.cs" Inherits="PalletManagement.Web.Setup.InTracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="http://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.0.3/css/font-awesome.css"
        rel="stylesheet" type="text/css" />

    <div class="row form-horizontal">
        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
            <p class="text-danger">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
        </asp:PlaceHolder>
        <br />
        <h4>In-Coming Pallets</h4>
        <hr />
        <div class="col-md-8 form-group col-lg-10 bs-component ">
            <asp:GridView ID="gdvShipment" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="ShipmentId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal" OnSelectedIndexChanged="gdvShipment_SelectedIndexChanged">
                <Columns>
                    <asp:TemplateField HeaderText="Shipment Number">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="select" CausesValidation="False" Text='<%# Eval("ShipmentNumber") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ShipmentId" HeaderText="ID" Visible="false" />
                    <%--<asp:BoundField DataField="ShipmentStatus" HeaderText="Status" />--%>
                    <asp:BoundField DataField="Source" HeaderText="Source" />
                    <asp:BoundField DataField="SourceDateTime" HeaderText="Departure Time" DataFormatString="{0:dd-MM-yy}" />
                    <asp:BoundField DataField="Destination" HeaderText="Destination" />
                    <asp:BoundField DataField="TruckNumber" HeaderText="Truck Number" />
                    <asp:BoundField DataField="NoOfPalletsOut" HeaderText="Pallets Out" />
                    <asp:BoundField DataField="NoOfPalletsIn" HeaderText="Pallets In" />
                    <%-- <asp:TemplateField HeaderText="Checked In">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkInBox" runat="server" Checked='<%# Eval("IsCompleted") %>' Enabled="False" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
                <HeaderStyle BackColor="#1A4874" ForeColor="White" />
            </asp:GridView>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-8 ">
                        <asp:Panel runat="server" ScrollBars="Auto" Height="210px" Wrap="true">
                            <asp:CheckBoxList ID="chkAvailablePatllets" runat="server" BorderStyle="None" RepeatDirection="Vertical" CssClass="SingleCheckbox"></asp:CheckBoxList>
                        </asp:Panel>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-7 col-md-4">
                        <asp:Button runat="server" Text="Save" CssClass="btn btn-primary" ID="btnSubmit" OnClick="btnSubmit_Click" Visible="False" />
                        <asp:HiddenField ID="hdfShipmentId" runat="server" />
                    </div>
                </div>


            </div>
        </div>
    </div>


</asp:Content>
