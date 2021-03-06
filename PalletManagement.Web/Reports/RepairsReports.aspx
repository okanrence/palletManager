﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RepairsReports.aspx.cs" Inherits="PalletManagement.Web.Reports.RepairsReports" EnableEventValidation="false" %>

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
                    <asp:Label runat="server" AssociatedControlID="txtStartDate" CssClass="col-md-4 control-label">Start Date</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtStartDate" CssClass="form-control datepicker" TextMode="SingleLine" />
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
                    <asp:Label runat="server" AssociatedControlID="txtEndDate" CssClass="col-md-4 control-label">End Date</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtEndDate" CssClass="form-control datepicker" TextMode="SingleLine" />
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
        <asp:Panel ID="pnlRepairs" runat="server" Height="250px" ScrollBars="vertical" BorderStyle="Solid" BorderColor="#CCCCCC" BorderWidth="1px">
            <div class="col-md-10 form-group col-lg-12 bs-component" style="padding-top: 20px">
                <asp:LinkButton ID="lnkOverallSummaryExcel" runat="server" CssClass="btn btn-default pull-right" OnClick="lnkOverallSummaryExcel_Click">Export to Excel</asp:LinkButton>
                <asp:GridView ID="gdvDamages" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="FacilityId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal" OnSelectedIndexChanged="gdvDamages_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="SN" />
                        <asp:BoundField DataField="FacilityId" HeaderText="FacilityId" Visible="false" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                        <asp:TemplateField HeaderText="Facility">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="select" CausesValidation="False" Text='<%# Eval("FacilityName") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Damaged" HeaderText="Damaged" />
                        <asp:BoundField DataField="Repairable" HeaderText="Repairable" />
                        <asp:BoundField DataField="Repaired" HeaderText="Repaired" />
                        <asp:BoundField DataField="TotallyDamaged" HeaderText="TotallyDamaged" />
                        <asp:BoundField DataField="DeckerboardsUsed" HeaderText="DeckerboardsUsed" />
                        <asp:BoundField DataField="DP_TDP_Extracted" HeaderText="DP_TDP_Extracted" />
                        <asp:BoundField DataField="NailsUsed" HeaderText="NailsUsed" />
                    </Columns>
                    <HeaderStyle BackColor="#1A4874" ForeColor="White" />
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>

    <div class="row" style="padding-top: 20px">
        <asp:Panel ID="pnlBreakDown" runat="server" Height="350px" ScrollBars="vertical" BorderStyle="Solid" BorderColor="#CCCCCC" BorderWidth="1px" Visible ="false">
            <div class="col-md-10 form-group col-lg-12 bs-component" style="padding-top: 20px">
                <asp:LinkButton ID="lnkbreakdown" runat="server" CssClass="btn btn-default pull-right" OnClick="lnkbreakdown_Click">Export to Excel</asp:LinkButton>
                <asp:GridView ID="gdvRepairsBreakdown" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="FacilityID" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal" OnSelectedIndexChanged="gdvDamages_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="SN" />
                        <%--<asp:BoundField DataField="CustomerName" HeaderText="CustomerName" />--%>
                        <asp:BoundField DataField="FacilityName" HeaderText="FacilityName" />
                        <asp:BoundField DataField="CollectionFormNo" HeaderText="Collection Form No" />
                        <asp:BoundField DataField="PalletCode" HeaderText="PalletCode" />
                        <asp:BoundField DataField="DamageLevelName" HeaderText="Damage Level" />
                        <asp:BoundField DataField="DateAdded" HeaderText="Date Damaged" DataFormatString="{0:dd-MM-yy hh:mm:ss tt}" />
                        <asp:BoundField DataField="HasBeenRepaired" HeaderText="Repaired" />                       
                        <asp:BoundField DataField="DateRepaired" HeaderText="Date Repaired" DataFormatString="{0:dd-MM-yy hh:mm:ss tt}" />
                    </Columns>
                    <HeaderStyle BackColor="#1A4874" ForeColor="White" />
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>

</asp:Content>
