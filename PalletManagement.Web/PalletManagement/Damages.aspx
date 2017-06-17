<%@ Page Title="" Language="C#" MasterPageFile="~/SiteOperator.Master" AutoEventWireup="true" CodeBehind="Damages.aspx.cs" Inherits="PalletManagement.Web.Setup.Damages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="http://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.0.3/css/font-awesome.css"
        rel="stylesheet" type="text/css" />
    <div class="row">
        <div class="form-group form-horizontal col-md-10">
            <h4>Pallets Damaged</h4>
            <hr />
            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ddlDamageLevel" CssClass="col-md-4 control-label">Pallets</asp:Label>

                <div class="col-md-8 ">
                    <asp:Panel runat="server" ScrollBars="Auto" Height="210px" Width="150px" Wrap="true">
                        <asp:CheckBoxList ID="chkPatllets" runat="server" BorderStyle="None" RepeatDirection="Vertical" CssClass="SingleCheckbox"></asp:CheckBoxList>
                    </asp:Panel>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="ddlDamageLevel" CssClass="col-md-4 control-label">Damage Level</asp:Label>
                <div class="col-md-5">
                    <asp:DropDownList ID="ddlDamageLevel" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlDamageLevel"
                        CssClass="text-danger" ErrorMessage="required." InitialValue="0" />
                </div>
            </div>

             <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtCollectionFormNo" CssClass="col-md-4 control-label">Collection Form No</asp:Label>
                    <div class="col-md-5">
                        <asp:TextBox runat="server" ID="txtCollectionFormNo" TextMode="SingleLine" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCollectionFormNo"
                            CssClass="text-danger" ErrorMessage="required." />
                    </div>
                </div>


             <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtReason" CssClass="col-md-4 control-label">Reason for damage</asp:Label>
                    <div class="col-md-5">
                        <asp:TextBox runat="server" ID="txtReason" TextMode="SingleLine" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtReason"
                            CssClass="text-danger" ErrorMessage="required." />
                    </div>
                </div>

            <div class="form-group">
                <div class="col-md-offset-8 col-md-4">
                    <asp:Button runat="server" Text="Submit" CssClass="btn btn-primary" ID="btnSubmit" OnClick="btnSubmit_Click" />
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <div style="padding-top: 20px" class="row">
        <div class="col-md-10 form-group col-lg-12 bs-component">
            <asp:GridView ID="gdvDamages" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="DamageId" EmptyDataText="No Records Found." ShowHeaderWhenEmpty="True" GridLines="Horizontal" OnSelectedIndexChanged="gdvDamages_SelectedIndexChanged" OnRowUpdating="gdvDamages_RowUpdating">
                <Columns>
                    <%--<asp:TemplateField HeaderText="Shipment Number">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="select" CausesValidation="False" data-toggle="modal" data-target="#myModal" Text='<%# Eval("DamageId") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="DamagedPalletId" HeaderText="palletID" Visible="false" />
                    <asp:BoundField DataField="DamageId" HeaderText="ID" />
                    <asp:BoundField DataField="CollectionFormNo" HeaderText="Collection Form No" />
                    <asp:BoundField DataField="PalletCode" HeaderText="PalletCode" />
                    <asp:BoundField DataField="DamageLevelName" HeaderText="Damage Level" />
                    <asp:BoundField DataField="DateAdded" HeaderText="Date of Damage" DataFormatString="{0:dd-MM-yy hh:mm:ss tt}" />
                   <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                           <asp:LinkButton ID="lnkFacilities" runat="server" CommandName="update" CausesValidation="False">[repair]</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
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
