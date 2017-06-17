<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="PalletManagement.Web.Reports.Summary" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="http://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.0.3/css/font-awesome.css"
        rel="stylesheet" type="text/css" />
    <div class="row">
        <div class="form-group">
            <div class="col-md-8">
                <div class="form-horizontal">
                    <h4>Pallet Summary Report as at <%: DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt") %></h4>
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
       
        <asp:Panel ID="pnlOverall" runat="server" Height="250px" ScrollBars="vertical" BorderStyle="Solid" BorderColor="#CCCCCC" BorderWidth="1px">
            <div class="col-md-10 form-group col-lg-12 bs-component" style="padding-top: 20px">
             <asp:LinkButton ID="lnkOverallSummaryExcel" runat="server" CssClass="btn btn-default pull-right" OnClick="lnkOverallExcel_Click">Export to Excel</asp:LinkButton>
                <asp:GridView ID="gdvPalletSummary" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="CustomerId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal" OnSelectedIndexChanged="gdvShipment_SelectedIndexChanged">
                    <Columns>

                        <asp:BoundField DataField="CustomerId" HeaderText="No" Visible="false" />
                          <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="select" CausesValidation="False" Text='<%# Eval("CustomerName") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        <%--<asp:BoundField DataField="CustomerName" HeaderText="Customer" />--%>
                        <asp:BoundField DataField="Total" HeaderText="Total" />
                        <asp:BoundField DataField="Available" HeaderText="Available" />
                        <asp:BoundField DataField="Damaged" HeaderText="Damaged" />
                        <asp:BoundField DataField="Unaccounted" HeaderText="Unaccounted" />
                    </Columns>
                    <HeaderStyle BackColor="#1A4874" ForeColor="White" />
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>

     <div class="row"  style="padding-top: 20px">
       
        <asp:Panel ID="pnlByFacility" runat="server" Height="350px" ScrollBars="vertical" BorderStyle="Solid" BorderColor="#CCCCCC" BorderWidth="1px"  Visible="False">
             
            <div class="col-md-10 form-group col-lg-12 bs-component" style="padding-top: 20px">
            <span><asp:Label ID="lblCustomerName" runat="server" Font-Size="Large"></asp:Label><asp:LinkButton ID="lnkByFacilitiesExcel" runat="server"  CssClass="btn btn-default pull-right" OnClick="lnkByFacilitiesExcel_Click">Export to Excel</asp:LinkButton></span>    
                <asp:GridView ID="gdvByFacility" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="FacilityId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal" OnSelectedIndexChanged="gdvShipment_SelectedIndexChanged">
                    <Columns>

                        <asp:BoundField DataField="FacilityId" HeaderText="No" Visible="false" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                        <asp:BoundField DataField="FacilityName" HeaderText="FacilityName" />
                        <asp:BoundField DataField="Total" HeaderText="Total" />
                        <asp:BoundField DataField="Available" HeaderText="Available" />
                        <asp:BoundField DataField="Damaged" HeaderText="Damaged" />
                        <asp:BoundField DataField="Unaccounted" HeaderText="Unaccounted" />
                    </Columns>
                    <HeaderStyle BackColor="#1A4874" ForeColor="White" />
                </asp:GridView>
            </div>
        </asp:Panel>
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
