<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="PalletManagement.Web.Reports.Summary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="http://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.0.3/css/font-awesome.css"
        rel="stylesheet" type="text/css" />
    <div class="row">
        <div class="form-group">
            <div class="col-md-8">
                <div class="form-horizontal">
                    <h4>Pallet Summary Reports</h4>
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <div class="col-md-8 ">
                            <asp:Label runat="server" AssociatedControlID="txtStartDate" CssClass="col-md-4 control-label">Start Date</asp:Label>
                            <asp:TextBox runat="server" ID="txtStartDate" TextMode="SingleLine" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-8 ">
                            <asp:Label runat="server" AssociatedControlID="txtEndDate" CssClass="col-md-4 control-label">End Date</asp:Label>
                            <asp:TextBox runat="server" ID="txtEndDate" TextMode="SingleLine" CssClass="form-control" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <div style="padding-top: 20px" class="row">
        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
        <div class="col-md-10 form-group col-lg-12 bs-component">
            <asp:GridView ID="gdvPalletSummary" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="Id" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal" OnSelectedIndexChanged="gdvShipment_SelectedIndexChanged">
                <Columns>
                   <%-- <asp:TemplateField HeaderText="Shipment Number">
                        <ItemTemplate>
                            <a id="lnkAddClients" href="#" class="btn btn-link " data-toggle="modal" data-target="#myModal" >Add Contacts</a>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="select" CausesValidation="False" data-toggle="modal" data-target="#myModal" Text='<%# Eval("ShipmentNumber") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="Id" HeaderText="No" Visible="false" />
                    <%--<asp:BoundField DataField="ShipmentNumber" HeaderText="SHipment Number" />--%>
                    <%--<asp:BoundField DataField="ShipmentStatus" HeaderText="Status" />--%>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                    <asp:BoundField DataField="Total" HeaderText="Total" />
                    <asp:BoundField DataField="Damaged" HeaderText="Damaged" />
                    <asp:BoundField DataField="Repaired" HeaderText="Repaired" />
                    <asp:BoundField DataField="Available" HeaderText="Available" />
                </Columns>
                <HeaderStyle BackColor="#1A4874" ForeColor="White" />
            </asp:GridView>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-offset-7 col-md-4">
                        <asp:HiddenField ID="hdfShipmentId" runat="server" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div class="modal fade" id="myModal">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">pallets</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-md-4 form-group">
                            </div>
                        </div>
                        <div class="row">
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
