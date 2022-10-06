<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" Inherits="DataPerpustakaanAdd" Codebehind="DataPerpustakaanAdd.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="titlecontent">Administrasi ► <a href="<%= BackPage %>">Data Perpustakaan Mitra</a> ► <%= AddOrEdit %> Perpustakaan Mitra</div>
    <div class="ButtonArea">
        <asp:Button ID="btSave" runat="server" onclick="btSave_Click" Text="Simpan"  />
        <% if (EditID != "") {%>
        <button id="btDelete" type="button" onclick="dodelete()">
            <span>Hapus</span>
        </button>
        <%} %>
        <button id="btnCancel" type="button">
            <span>Selesai</span>
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
                            <td>Kode</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox id="txtKode" runat="server" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Nama Perpustakaan</td>
                            <td>:</td>
                            <td><asp:TextBox ID="txtNama" runat="server" Width="250px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                Service Diregistrasi</td>
                            <td>
                                :</td>
                            <td>
                                <asp:CheckBox ID="cbIsServiceReg" runat="server" />
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
        
        <input id="HiddenCodeOriginal" type="hidden" runat="server"/>
        <input id="HiddenNameOriginal" type="hidden" runat="server"/>
        <div id="dialog-history"></div>

    </ContentTemplate>
</asp:UpdatePanel>
           
</div>

<script type="text/javascript">
//<![CDATA[
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
                            window.open('<%= BackPage %>','_self');
                        }
                        else {
                            alert(data);
                        };
                    });
                };
                return false;
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
//]]>
</script>
</asp:Content>

