<%@ Page Title="" Language="C#" MasterPageFile="~/SiteOperator.Master" AutoEventWireup="true" CodeBehind="Repairs.aspx.cs" Inherits="PalletManagement.Web.Setup.Repairs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="http://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.0.3/css/font-awesome.css"
        rel="stylesheet" type="text/css" />
    <div class="row">
        <h4>Pallets Repaired</h4>
        <hr />
        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
            <p class="text-danger">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
        </asp:PlaceHolder>
        <div class="row form-horizontal">
            <div class="form-group col-md-12">
                <div class="col-md-5">
                    <asp:Label runat="server" AssociatedControlID="txtStartDate" CssClass="col-md-3 control-label">Start Date</asp:Label>
                    <asp:TextBox runat="server" ID="txtStartDate" TextMode="SingleLine" CssClass="form-control datepicker col-md-2" />
                     <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStartDate"
                        CssClass="text-danger" ErrorMessage="required."  ValidationGroup="SearchBar" />
                </div>
                <div class="col-md-5">
                    <asp:Label runat="server" AssociatedControlID="txtEndDate" CssClass="col-md-3 control-label">End Date</asp:Label>
                    <asp:TextBox runat="server" ID="txtEndDate" TextMode="SingleLine" CssClass="form-control datepicker col-md-2" />
                      <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEndDate"
                        CssClass="text-danger" ErrorMessage="required."  ValidationGroup="SearchBar" />
                </div>
                <div class="col-md-2">
                    <asp:Button runat="server" Text="Search" CssClass="btn btn-default" ID="btnSearch"  ValidationGroup="SearchBar" OnClick="btnSearch_Click"/>
                </div>
            </div>
        </div>

        <div class="row col-md-12">
            <asp:Panel ID="pnlGrid" runat="server" Height="350px" ScrollBars="vertical" BorderStyle="Solid" BorderColor="#CCCCCC" BorderWidth="1px">
                <div class="col-md-12 form-group col-lg-12 bs-component" style="padding-top: 20px">
                    <asp:GridView ID="gdvDamages" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="DamageId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal" OnSelectedIndexChanged="gdvDamages_SelectedIndexChanged">
                        <Columns>
                            <%--<asp:BoundField DataField="DamagedPalletId" HeaderText="palletID" Visible="false" />--%>
                            <asp:BoundField DataField="DamageId" HeaderText="ID" />
                            <asp:BoundField DataField="CollectionFormNo" HeaderText="Collection Form No" />
                            <asp:BoundField DataField="PalletCode" HeaderText="PalletCode" />
                            <asp:BoundField DataField="DamageLevelName" HeaderText="Damage Level" />
                            <asp:BoundField DataField="DateAdded" HeaderText="Date Damaged" DataFormatString="{0:dd-MM-yy hh:mm:ss tt}" />
                            <asp:TemplateField HeaderText="Repaired">
                                <ItemTemplate>
                                    <asp:CheckBox ID="checkInBox" runat="server" Checked='<%# Eval("Repaired") %>' Enabled="False" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DateRepaired" HeaderText="Date Repaired" DataFormatString="{0:dd-MM-yy hh:mm:ss tt}" />

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="select" CausesValidation="False">[edit]</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#1A4874" ForeColor="White" />
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>

        <div class="row form-horizontal col-md-8" style="padding-top: 20px">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtCollectionFormNo" CssClass="col-md-4 control-label">Collection Form No</asp:Label>
                <div class="col-md-5">
                    <asp:TextBox runat="server" ID="txtCollectionFormNo" TextMode="SingleLine" CssClass="form-control" ReadOnly="True" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCollectionFormNo"
                        CssClass="text-danger" ErrorMessage="required." />
                </div>
            </div>


            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtReason" CssClass="col-md-4 control-label">Reason for damage</asp:Label>
                <div class="col-md-5">
                    <asp:TextBox runat="server" ID="txtReason" TextMode="Multiline" CssClass="form-control" ReadOnly="True" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtReason"
                        CssClass="text-danger" ErrorMessage="required." />
                </div>
            </div>


            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtDeckerboard" CssClass="col-md-4 control-label">Deckerboards Used</asp:Label>
                <div class="col-md-5">
                    <asp:TextBox runat="server" ID="txtDeckerboard" TextMode="SingleLine" CssClass="form-control" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtDeckerboard"  CssClass="text-danger" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDeckerboard"
                        CssClass="text-danger" ErrorMessage="required." />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtTDPExtracted" CssClass="col-md-4 control-label">TDP Extracted</asp:Label>
                <div class="col-md-5">
                    <asp:TextBox runat="server" ID="txtTDPExtracted" TextMode="SingleLine" CssClass="form-control" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtTDPExtracted"  CssClass="text-danger" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTDPExtracted"
                        CssClass="text-danger" ErrorMessage="required." />
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtNailedUsed" CssClass="col-md-4 control-label">Nailed Used</asp:Label>
                <div class="col-md-5">
                    <asp:TextBox runat="server" ID="txtNailedUsed" TextMode="SingleLine" CssClass="form-control" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtNailedUsed" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNailedUsed"
                        CssClass="text-danger" ErrorMessage="required." />
                </div>
            </div>

             <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ddlDamageLevel" CssClass="col-md-4 control-label">Status</asp:Label>
                <div class="col-md-5">
                    <asp:DropDownList ID="ddlDamageLevel" runat="server" CssClass="form-control">
                        <asp:ListItem Value="1">Repaired</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-7 col-md-4">
                    <asp:Button runat="server" Text="Submit" CssClass="btn btn-primary" ID="btnSubmit" OnClick="btnSubmit_Click" />
                    <asp:HiddenField ID="hdfPalletId" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-md-offset-7 col-md-4">
                        <asp:HiddenField ID="hdfDamageId" runat="server" />
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
