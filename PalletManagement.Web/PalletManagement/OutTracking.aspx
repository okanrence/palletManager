 <%@ Page Title="" Language="C#" MasterPageFile="~/SiteOperator.Master" AutoEventWireup="true" CodeBehind="OutTracking.aspx.cs" Inherits="PalletManagement.Web.Setup.OutTracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="http://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.0.3/css/font-awesome.css"
        rel="stylesheet" type="text/css" />
    <%-- <style type="text/css">
        .chkChoice {
            padding-left: 30px;
            padding-right: 30px;
        }

        .chkChoice td {
            padding-left: 20px;
        }
    </style>--%>
      <div class="row">
            <h4>Out-Going Pallets</h4>
                <hr />
      </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-horizontal">
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                </asp:PlaceHolder>

                <%--<div class="form-group">
                    <%--<asp:Label runat="server" AssociatedControlID="chkAvailablePatllets" CssClass="col-md-4 control-label">Select Pallets</asp:Label>--%>
                    <div class="col-md-8 ">
                        <asp:Panel runat="server" ScrollBars="Auto" Height="210px" GroupingText="Select Pallets" Wrap="true"  >
                           <asp:CheckBoxList ID="chkAvailablePatllets" runat="server" BorderStyle="None" RepeatDirection="Vertical" CssClass="SingleCheckbox"></asp:CheckBoxList>
                        </asp:Panel>
                    </div>
                <%--</div>--%>

            </div>
        </div>

        <div class="col-md-6">
            <div class="form-horizontal">

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlDestinationFacility" CssClass="col-md-4 control-label">Destination</asp:Label>
                    <div class="col-md-5">
                        <asp:DropDownList ID="ddlDestinationFacility" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlDestinationFacility"
                            CssClass="text-danger" ErrorMessage="required." InitialValue="0" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtTruckNumber" CssClass="col-md-4 control-label">Truck Number</asp:Label>
                    <div class="col-md-5">
                        <asp:TextBox runat="server" ID="txtTruckNumber" TextMode="SingleLine" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTruckNumber"
                            CssClass="text-danger" ErrorMessage="required." />
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtShipmentNumber" CssClass="col-md-4 control-label">Shipment Number</asp:Label>
                    <div class="col-md-5">
                        <asp:TextBox runat="server" ID="txtShipmentNumber" TextMode="SingleLine" CssClass="form-control" Enabled="False" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtShipmentNumber"
                            CssClass="text-danger" ErrorMessage="required." />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-offset-7 col-md-4">
                        <asp:Button runat="server" Text="Save" CssClass="btn btn-primary" ID="btnSubmit" OnClick="btnSubmit_Click" />
                        <asp:HiddenField ID="hdfShipmentId" runat="server" />
                    </div>
                </div>


            </div>
        </div>
    </div>
    <div class="row">
    </div>
    <div class="row">
        <div class="col-md-10 form-group col-lg-12 bs-component">
            <asp:GridView ID="gdvShipment" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="ShipmentId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal" OnSelectedIndexChanged="gdvShipment_SelectedIndexChanged" OnRowDeleting="gdvShipment_RowDeleting">
                <Columns>
                    <asp:TemplateField HeaderText="Shipment Number">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="select" CausesValidation="False" Text='<%# Eval("ShipmentNumber") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ShipmentId" HeaderText="ID" Visible="false" />
                    <asp:BoundField DataField="Source" HeaderText="Source" />
                    <asp:BoundField DataField="SourceDateTime" HeaderText="Departure Time" DataFormatString="{0:dd-MM-yy}" />
                    <asp:BoundField DataField="Destination" HeaderText="Destination" />
                    <asp:BoundField DataField="DestinationDateTime" HeaderText="Arrival Time" DataFormatString="{0:dd-MM-yy}" />
                    <asp:BoundField DataField="TruckNumber" HeaderText="Truck Number" />
                    <asp:BoundField DataField="NoOfPalletsOut" HeaderText="Pallets Out" />
                  

                    <asp:TemplateField HeaderText="Checked In">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkInBox" runat="server" Checked='<%# Eval("IsCompleted") %>' Enabled="False" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <%--<asp:LinkButton ID="lnkEdit" runat="server" CommandName="select" CausesValidation="False">edit</asp:LinkButton>--%>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="delete" CausesValidation="False">delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#1A4874" ForeColor="White" />
            </asp:GridView>
        </div>
    </div>

</asp:Content>
