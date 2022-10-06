<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" Inherits="ChangePassword" Codebehind="ChangePassword.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style type="text/css">
        .right_col 
        {
            padding: 0px !important;    
        }
        .chosen-container
        {
            margin-top: 5px;
        }
    </style>

<div class="titlecontent">
    <span>Pengaturan ► Ganti Password</span>
</div>
<div class="ButtonArea" style="display: none;">
    <button id="btSave" runat="server" type="button" class="button-primary" onserverclick="btSave_Click">
        <span><i class="fa fa-save"></i> Simpan</span>
    </button>
</div>

<div id="formcontent" class="MainTable">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <br />
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="x_panel">
                    <div id="divInputContent" class="x_content">
                        <br />
                        <div class="form-group">
                            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="first-name">
                                Password Lama
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <asp:TextBox ID="txtOldPassword" runat="server" Width="300px" CssClass="form-control col-md-7 col-xs-12 color-picker" required="required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group" style="margin-top: 8px;">
                            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="first-name">
                                Password Baru
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <asp:TextBox ID="txtNewPassword" runat="server" Width="300px" CssClass="form-control col-md-7 col-xs-12 color-picker" required="required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="ln_solid">
                        </div>
                        <div class="form-group" style="padding-top: 20px;">
                            <div class="col-md-6 col-sm-6 col-xs-12 col-md-offset-3">
                                <button class="button-warning reset" type="reset">
                                    <i class="fa fa-refresh"></i> Reset</button>
                                <button id="btSave2" runat="server" type="button" class="button-primary" onserverclick="btSave_Click">
                                    <span><i class="fa fa-save"></i> Simpan</span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        
        <asp:UpdateProgress ID="UpdateProgress1" DynamicLayout="false" runat="server" DisplayAfter="0" 
            AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <div class="modal">
                    <div class="center">
                        <asp:Image ID="Image1_1" runat="server" ImageUrl="~/images/ajax-loader.gif" AlternateText="Proses..." />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </ContentTemplate>
</asp:UpdatePanel>
           
</div>

<script type="text/javascript">
//<![CDATA[
    RegisterScripts = function () {
        $(function () {
            $(document.ready)
            {
                $(".reset").each(function () {
                    $(this).click(function () {
                        resetInput($("#divInputContent"));
                        return false;
                    });
                });

            };
        });
    };
//]]>
</script>

</asp:Content>

