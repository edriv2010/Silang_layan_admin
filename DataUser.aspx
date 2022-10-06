<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" CodeBehind="DataUser.aspx.cs" Inherits="DataUser" %>
<%@ Register src="usercontrol/MessageBoxUsc/MsgBoxUsc.ascx" tagname="MsgBoxUsc" tagprefix="uc1" %>
<%@ Register src="usercontrol/Exporter.ascx" tagname="Exporter" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>  

<div id="ListArea">
    <div class="titlecontent">
        <asp:Label ID="lbPageTitle" runat="server" Text="Authority ► Pengaturan Umum ► Data User"></asp:Label>
    </div>
    <div id="data-list" class="ListArea">
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="Filtering" style="width: 100%">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="padding-right: 20px;">
                                    <button id="btnAddNew" type="button" class="button-primary">
                                        <span><i class="fa fa-plus"></i>Tambah</span>
                                    </button>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlKriteria" runat="server" Width="154px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKataKunci" runat="server" 
                                        ontextchanged="txtKataKunci_TextChanged" Width="150px" AutoPostBack="True"></asp:TextBox>
                                </td>
                                <td style="vertical-align: middle;">
                                    <asp:ImageButton ID="ibPencarian" runat="server" Height="20px" 
                                        ImageUrl="~/images/search.png" onclick="ibPencarian_Click" Width="20px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 225px; vertical-align: middle;">
                        <span class="PageLabel" style="vertical-align: middle; padding-bottom: 4px;">Tampilkan data :</span>
                        <asp:DropDownList ID="ddlPage" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlPage_SelectedIndexChanged" Width="130px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:DataGrid ID="dgData" runat="server" 
                CellPadding="4" Font-Names="Tahoma" 
                Font-Size="12px" GridLines="None" Width="100%" 
                AutoGenerateColumns="False" ShowFooter="False"
                OnSortCommand="dgData_OnSortCommand" OnItemCommand="dgData_ItemCommand" 
                OnEditCommand="dgData_EditCommand" OnDeleteCommand="dgData_DeleteCommand"
                OnItemDataBound="dgData_OnItemDataBound"
                AllowSorting="True" CssClass="Grid">
                <HeaderStyle CssClass="GridHeader" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle CssClass="GridItem"></ItemStyle>
                <AlternatingItemStyle CssClass="GridAltItem"></AlternatingItemStyle>
                <Columns>
                    <asp:EditCommandColumn CancelText="Cancel" EditText="Edit" UpdateText="Update" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" Visible="false">
                    </asp:EditCommandColumn>
                    <asp:ButtonColumn CommandName="Delete" Text="Hapus" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="#CC0000">
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="No" HeaderText="No." ReadOnly="true" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="id" Visible="false">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="UserName" HeaderText="Nama User" ItemStyle-Width="100px" SortExpression="UserName">
                    </asp:BoundColumn>
                    <asp:ButtonColumn DataTextField="Fullname" HeaderText="Nama Lengkap" ItemStyle-Width="200px" SortExpression="FullName" CommandName="Detail">
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="EmailAddress" HeaderText="Email" ItemStyle-Width="100px" SortExpression="EmailAddress">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="HakAkses" HeaderText="Hak Akses" ItemStyle-Width="50px" SortExpression="HakAkses">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="PropinsiName" HeaderText="Provinsi" ItemStyle-Width="100px" SortExpression="Propinsi.NAMAPROPINSI">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="KabupatenName" HeaderText="Kabupaten" ItemStyle-Width="100px" SortExpression="Kabupaten.NAMAKAB">
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
            <div id="divDataTidakTersedia" runat="server" style="text-align: center; font-weight: bold; font-size: 14px; color: #808080;">Data tidak tersedia</div>
            <div id="divBottomPageLabel" class="BottomPageLabel" style="text-align: center">
                <asp:Label ID="lbPage0" runat="server" Text="Halaman ke-1" CssClass="PageLabel"></asp:Label>
            </div>
            <div id="divPaging" runat="server" class="Paging">
                <asp:Label ID="lbJumlahCantuman" runat="server" CssClass="JumlahData"></asp:Label>
                <asp:Label ID="lbPageNumber" runat="server" Text="Halaman :" CssClass="PageNumberLabel"></asp:Label>
                <asp:Repeater ID="RepeaterPage" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtPage" runat="server" OnClick="lbtPage_Click" 
                            Text='<%# DataBinder.Eval(Container.DataItem, "PageNumber") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:Repeater>
                <table align="right">
                    <tr>
                        <td>
                            <asp:Label ID="lbGoToPage" runat="server" Text="Ke Halaman : " CssClass="PageNumberLabel"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPageGoTo" runat="server" 
                                ontextchanged="txtPageGoTo_TextChanged"
                                onkeypress="NumericValidation(event)"
                                Width="25px" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <br />
            <uc1:MsgBoxUsc ID="MsgBoxUsc1" runat="server" />

            <asp:UpdateProgress ID="UpdateProgress2" runat="server" 
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
    <uc2:Exporter ID="Exporter1" runat="server" />
    </div>

</div>

<script type="text/javascript">
//<![CDATA[
    function openpopup() {
        w = ((screen.width - 600) / 2);
        h = ((screen.height - 400) / 2);
        var Link = "xmldata/" + "<%= TableName %>" + ".xml";
        newwindow = window.open(Link, '_blank', 'width=600,height=400,left=' + w + ',top=' + h + ',resizable=yes,scrollbars=yes,toolbar=yes,location=yes,directories=no,status=yes,menubar=yes,copyhistory=no');
        if (window.focus) { newwindow.focus(); }
    };
    $(document.ready)
    {
        RegisterScript = function () {
            $("#btnAddNew").removeAttr('disabled');
            $("#btnAddNew").click(function () {
                window.open('<%= PageAddOrEditURL  %>', '_self');
                return false;
            });
        };
    };
//]]>
</script>

</asp:Content>
