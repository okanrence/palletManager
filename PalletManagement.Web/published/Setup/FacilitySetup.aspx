<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FacilitySetup.aspx.cs" Inherits="PalletManagement.Web.Setup.FacilitySetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-6">
            <div class="form-horizontal">
                <h4>Setup Company Facilities</h4>
                <hr />
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                </asp:PlaceHolder>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlCustomer" CssClass="col-md-4 control-label">Customer</asp:Label>
                    <div class="col-md-8">
                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCustomer"
                            CssClass="text-danger" ErrorMessage="The customer field is required." InitialValue="0" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="row">
                <div class="form-group">
                    <div class="col-lg-12">
                        <div class="bs-component">
                            <asp:GridView ID="gdvFacilities" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="FacilityId" EmptyDataText="No Records Found For This Customer." OnSelectedIndexChanged="gdvCustomers_SelectedIndexChanged" OnRowDeleting="gdvCustomers_RowDeleting" GridLines="Horizontal">
                                <Columns>
                                    <asp:BoundField DataField="FacilityId" HeaderText="ID" />
                                    <asp:BoundField DataField="FacilityName" HeaderText="Facility Name" />
                                    <asp:BoundField DataField="FacilityType" HeaderText="Type" />
                                    <asp:BoundField DataField="FacilityAddress" HeaderText="Address" />
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
    <hr />
    <div class="row">
        <div class="col-md-6">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddlFacilityType" CssClass="col-md-4 control-label">Facility Type</asp:Label>
                    <div class="col-md-8">
                        <asp:DropDownList ID="ddlFacilityType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                            <asp:ListItem>Depot</asp:ListItem>
                            <asp:ListItem>Plant</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlFacilityType"
                            CssClass="text-danger" ErrorMessage="The facility type field is required." InitialValue="0" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtFacilityName" CssClass="col-md-4 control-label">Facility Name</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtFacilityName" CssClass="form-control" TextMode="SingleLine" />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtFacilityAddress" CssClass="col-md-4 control-label">Facility Address</asp:Label>
                    <div class="col-md-8">
                        <asp:TextBox runat="server" ID="txtFacilityAddress" CssClass="form-control" TextMode="Multiline" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-4 col-md-8">
                        <asp:Button runat="server" Text="Submit" CssClass="btn btn-primary" ID="btnSubmit" OnClick="btnSubmit_Click" />
                        <asp:HiddenField ID="hdfFacilityId" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
