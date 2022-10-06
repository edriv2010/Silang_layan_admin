<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" Inherits="DataUserAdd" Codebehind="DataUserAdd.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="titlecontent">Pengaturan Umum ► <a href="<%= BackPage %>">Data User</a> ► <%= AddOrEdit %> User</div>
    <div class="ButtonArea">
        <button id="btSave" runat="server" type="submit" class="button-primary" onserverclick="btSave_Click">
            <span><i class="fa fa-save"></i>Simpan</span>
        </button>
        <% if (EditID != "") {%>
        <button id="btDelete" type="button" onclick="dodelete()" class="button-danger">
            <span><i class="fa fa-trash-o"></i>Hapus</span>
        </button>
        <asp:Button ID="btResetPassword" runat="server" onclick="btResetPassword_Click" OnClientClick="return confirm('Anda yakin akan me-reset password user ini?');" Text="Reset Password" CssClass="button-danger" />
        <%} %>
        <button id="btnCancel" type="button" class="button-success">
            <span><i class="fa fa-sign-out"></i>Selesai</span>
        </button>
    </div>

<div id="formcontent" class="MainTable">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
            AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/loader.gif" AlternateText="Proses..." />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <br />
        <table class="InputTable" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td valign="top">            
                    <table>
                        <tr>
                            <td>Nama User</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox id="txtNamaUser" runat="server" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Nama Lengkap</td>
                            <td>:</td>
                            <td><asp:TextBox ID="txtNamaLengkap" runat="server" Width="250px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                Email</td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                No. Handphone</td>
                            <td>
                                :</td>
                            <td>
                                <asp:TextBox ID="txtNoHp" runat="server" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Hak Akses</td>
                            <td>
                                :</td>
                            <td>
                                <asp:DropDownList ID="ddlHakAkses" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Validator</td>
                            <td>:</td>
                            <td>
                                <asp:CheckBox ID="cbIsValidatorAuthority" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Aktif</td>
                            <td>:</td>
                            <td>
                                <asp:CheckBox ID="cbAktif" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3" style="font-size: 8px;">Default password adalah nama user</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table class="UserInfoTable" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td valign="top">            
                    <table>
                        <tr>
                            <td>Create by</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbCreateBy" runat="server" Text="..."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Create date</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbCreateDate" runat="server" Text="..."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Create terminal</td>
                            <td>
                                :</td>
                            <td>
                                <asp:Label ID="lbCreateTerminal" runat="server" Text="..."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Last Update by</td>
                            <td>
                                :</td>
                            <td>
                                <asp:Label ID="lbUpdateBy" runat="server" Text="..."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Last Update date</td>
                            <td>
                                :</td>
                            <td>
                                <asp:Label ID="lbUpdateDate" runat="server" Text="..."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Last Update terminal</td>
                            <td>
                                :</td>
                            <td>
                                <asp:Label ID="lbUpdateTerminal" runat="server" Text="..."></asp:Label>
                            </td>
                        </tr>
                        <tr id="trHistoryData" runat="server" visible="false">
                            <td colspan="3">
                                <span title="Histori Update" onclick="OpenHistory('<%= TableName %>', '<%= EditID %>');" onmouseover="this.style.cursor='pointer'; this.style.color='Green'; this.style.textDecoration='None';" onmouseout="this.style.color='Blue'; this.style.textDecoration='Underline';" style="color: Blue; text-decoration: underline; cursor: pointer;">History Update</span>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        
        <input id="HiddenUserNameOriginal" type="hidden" runat="server"/>
        <div id="dialog-history"></div>

    </ContentTemplate>
</asp:UpdatePanel>
           
</div>

<script type="text/javascript">
//<![CDATA[
    RegisterScripts = function () {
        $(function () {
            $(document.ready)
            {
                $("#btnCancel").click(function () {
                    window.open('<%= BackPage %>', '_self')
                    return false;
                });

                dodelete = function () {
                    if (confirm("Apakah anda yakin ?")) {
                        Execute('<%= DeletePage %>', 'id=' + '<%= EditID %>' + '&del=1', function (data) {
                            if (data == 'OK') {
                                window.open('<%= BackPage %>', '_self');
                            }
                            else {
                                alert(data);
                            };
                        });
                    };
                    return false;
                };

                AcceptMenuInduk = function (selitem) {
                    var items = selitem.split("|");
                    $("#ContentPlaceHolder1_txtMenuInduk").val(items[1]);
                    $("#ContentPlaceHolder1_HiddenMenuInduk").val(items[0]);
                    $("#dialog-panduan").dialog('close');
                };

                OpenHistory = function (tablename, id) {
                    $("#dialog-history").dialog({
                        title: '',
                        modal: true,
                        width: 1000,
                        height: 400,
                        autoOpen: false
                    });
                    LoadPage("HistoryData.aspx?t=" + tablename + "&id=" + id, "dialog-history");
                    $("#dialog-history").dialog('open');
                };

            };
        });
    };
//]]>
</script>

<script src="js/chosen/chosen.jquery.js" type="text/javascript"></script>
<script type="text/javascript">
//<![CDATA[
    $(document.ready)
    {
        SetChoosenSelect = function () {
            var config = {
                '.chosen-select': { search_contains: true },
                '.chosen-select-deselect': { allow_single_deselect: true },
                '.chosen-select-no-single': { disable_search_threshold: 10 },
                '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            };
        };
        SetChoosenSelect();
    };
//]]>
</script>
</asp:Content>

