<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" CodeBehind="PerpusMitraPengiriman.aspx.cs" Inherits="PerpusMitraPengiriman" %>
<%@ Register src="usercontrol/MessageBoxUsc/MsgBoxUsc.ascx" tagname="MsgBoxUsc" tagprefix="uc1" %>
<%@ Register src="usercontrol/Exporter.ascx" tagname="Exporter" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
    #btPengirimanBuku
    {
        border: solid 4px #e9e06f;
    }
</style>

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>  

<div id="ListArea">
    <div class="titlecontent">Perpustakaan Mitra ► Pengiriman Buku dari Perpustakaan Mitra ke Perpusnas</div>
    
    <div id="data-list" class="ListArea">
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Image ID="Image1_1" runat="server" ImageUrl="~/images/loader.gif" AlternateText="Proses..." />
                        </td>
                        <td>
                            <asp:Label ID="Label1_1" runat="server" Text="Data sedang diproses. Harap Tunggu ..."></asp:Label>
                        </td>
                    </tr>
                </table>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <table class="InputTable" style="margin: 16px; box-shadow: 0px 0px 2px #666;">
                <tr>
                    <td colspan="3" style="border-bottom: dashed 1px #cec8bf; text-align: center; font-weight: bold; padding-bottom: 10px;">Tambah Item</td>
                </tr>
                <tr>
                    <td style="padding-top: 10px;">
                        Tanggal Kirim
                    </td>
                    <td style="padding-top: 10px;">:</td>
                    <td style="padding-top: 10px;">
                        <asp:TextBox ID="txtTanggalKirim" runat="server" Width="80px" CssClass="datepicker"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Item ID / Nomor Barcode
                    </td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="txtNomorBarcode" runat="server" Width="150px" 
                            ontextchanged="txtNomorBarcode_TextChanged"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table id="Filtering" style="width: 100%; border-top: solid 4px #e7e7e7; padding-top: 20px;">
                <tr>
                    <td class="style4">
                        <asp:Label ID="lbPage" runat="server" Text="Halaman ke-1" CssClass="PageLabel"></asp:Label>
                        <br />
                        <br />
                        Tampilkan data :
                        <br />
                        <asp:DropDownList ID="ddlPage" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlPage_SelectedIndexChanged" Width="154px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right" class="style5">
                        <table align="right">
                            <tr>
                                <td style="text-align: right;">
                                    Filter Tanggal Kirim : 
                                    <asp:TextBox ID="txtFilterTanggalKirimAwal" runat="server" Width="80px" CssClass="datepicker" AutoPostBack="True" OnTextChanged="txtFilterTanggalKirimAwal_TextChanged"></asp:TextBox> s/d <asp:TextBox ID="txtFilterTanggalKirimAkhir" runat="server" Width="80px" CssClass="datepicker" AutoPostBack="True" OnTextChanged="txtFilterTanggalKirimAkhir_TextChanged"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;<asp:DropDownList ID="ddlKriteria" runat="server" Width="154px">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtKataKunci" runat="server" 
                                        ontextchanged="txtKataKunci_TextChanged" Width="150px" AutoPostBack="True"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:ImageButton ID="ibPencarian" runat="server" Height="20px" 
                                        ImageUrl="~/images/search.png" onclick="ibPencarian_Click" Width="20px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div class="titlecontent">
                Daftar Pengiriman Buku dari Perpustakaan Mitra ke Perpusnas
            </div>
            <asp:DataGrid ID="dgData" runat="server" 
                CellPadding="4" Font-Names="Tahoma" 
                Font-Size="12px" GridLines="None" Width="100%" 
                AutoGenerateColumns="False" ShowFooter="False"
                OnSortCommand="dgData_OnSortCommand" OnItemCommand="dgData_ItemCommand" 
                OnEditCommand="dgData_EditCommand" OnDeleteCommand="dgData_DeleteCommand"
                AllowSorting="True" CssClass="Grid">
                <HeaderStyle CssClass="GridHeader" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle CssClass="GridItem"></ItemStyle>
                <AlternatingItemStyle CssClass="GridAtlItem"></AlternatingItemStyle>
                <Columns>
                    <asp:EditCommandColumn CancelText="Cancel" EditText="Edit" UpdateText="Update" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" Visible="false">
                    </asp:EditCommandColumn>
                    <asp:ButtonColumn CommandName="Delete" Text="Hapus" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="No" HeaderText="No." ReadOnly="true" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right">
                    </asp:BoundColumn>
                    <asp:ButtonColumn DataTextField="ID" Visible="false">
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="CreateDate" HeaderText="Tgl Kirim" ItemStyle-Width="100px" SortExpression="Tanggal_Kirim" DataFormatString ="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="NomorBarcode" HeaderText="Item ID" ItemStyle-Width="100px" SortExpression="NomorBarcode" ItemStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="NoInduk" HeaderText="No. Induk" ItemStyle-Width="100px" SortExpression="NoInduk">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Title" HeaderText="Judul" ItemStyle-Width="200px" SortExpression="Title">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Author" HeaderText="Pengarang" ItemStyle-Width="200px" SortExpression="Author">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Publikasi" HeaderText="Publikasi" ItemStyle-Width="200px" SortExpression="Publikasi">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="CreateBy" HeaderText="CreateBy" ItemStyle-Width="100px" SortExpression="pm_kirim.CreateBy">
                    </asp:BoundColumn>
                </Columns>
            </asp:DataGrid>
            <div id="divDataTidakTersedia" runat="server" style="text-align: center; font-weight: bold; font-size: 14px; color: #808080;">Data tidak tersedia</div>
            <div id="divBottomPageLabel" class="BottomPageLabel" style="text-align: right">
                <asp:Label ID="lbPage0" runat="server" Text="Halaman ke-1" CssClass="PageLabel"></asp:Label>
            </div>
            <div id="divPaging" runat="server" class="Paging">
                <asp:Label ID="lbPageNumber" runat="server" Text="Halaman :" CssClass="PageNumberLabel"></asp:Label>
                <asp:Repeater ID="RepeaterPage" runat="server" OnItemDataBound="rptItems_ItemDataBound">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtPage" runat="server" OnClick="lbtPage_Click" 
                            Text='<%# DataBinder.Eval(Container.DataItem, "PageNumber") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:Repeater>
                <br />
                <table>
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
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" 
                AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="Image1_2" runat="server" ImageUrl="~/images/loader.gif" AlternateText="Proses..." />
                            </td>
                            <td>
                                <asp:Label ID="Label1_2" runat="server" Text="Data sedang diproses. Harap Tunggu ..."></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <br />
            <asp:Label ID="lbJumlahCantuman" runat="server" CssClass="JumlahData"></asp:Label>
            <br />
            <br />
            <uc1:MsgBoxUsc ID="MsgBoxUsc1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc2:Exporter ID="Exporter1" runat="server" />
    </div>

    <br />
    <br />

</div>

<script type="text/javascript">
//<![CDATA[
    function openpopup() {
        w = ((screen.width - 600) / 2);
        h = ((screen.height - 400) / 2);
        var Link = "xmldata/" + <%= TableName %> + ".xml";
        newwindow = window.open(Link, '_blank', 'width=600,height=400,left=' + w + ',top=' + h + ',resizable=yes,scrollbars=yes,toolbar=yes,location=yes,directories=no,status=yes,menubar=yes,copyhistory=no');
        if (window.focus) { newwindow.focus(); }
    };
    RegisterScript = function () {
        $(document.ready)
        {
            $(".datepicker").datepicker({ changeMonth: true, changeYear: true, altformat: 'yy-mm-dd', dateFormat: 'yy-mm-dd', yearRange: '1925:c+10' });

        };
    };
//]]>
</script>

</asp:Content>
