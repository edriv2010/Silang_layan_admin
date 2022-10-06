<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" CodeBehind="DataPerpustakaan.aspx.cs" Inherits="DataPerpustakaan" %>
<%@ Register src="usercontrol/MessageBoxUsc/MsgBoxUsc.ascx" tagname="MsgBoxUsc" tagprefix="uc1" %>
<%@ Register src="usercontrol/Exporter.ascx" tagname="Exporter" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
    #btDataPerpustakaan
    {
        border: solid 4px #e9e06f;
    }
</style>

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>  

<div id="ListArea">
    <div class="titlecontent">
        <asp:Label ID="lbPageTitle" runat="server" Text=""></asp:Label></div>
    <div class="ButtonArea">
        <button id="btnAddNew" type="button" runat="server">
            <span>Tambah</span>
        </button>
    </div>
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
            <table id="Filtering" style="width: 100%">
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbPage" runat="server" Text="Halaman ke-1" CssClass="PageLabel"></asp:Label>
                        <br />
                        <br />
                        Tampilkan data :
                        <br />
                        <asp:DropDownList ID="ddlPage" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlPage_SelectedIndexChanged" Width="154px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">
                        <table align="right">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlKriteria" runat="server" Width="154px">
                                    </asp:DropDownList>
                                </td>
                                <td>
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
                    <asp:BoundColumn DataField="id" Visible="false">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Code" HeaderText="Kode" ItemStyle-Width="80px" SortExpression="Code">
                    </asp:BoundColumn>
                    <asp:ButtonColumn DataTextField="Name" HeaderText="Nama" ItemStyle-Width="300px" SortExpression="Name" CommandName="Detail">
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="ISSERVICEREG" HeaderText="Service<br/>Diregistrasi" ItemStyle-Width="20px" SortExpression="ISSERVICEREG" ItemStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="ONOFF" HeaderText="ON/OFF" ItemStyle-Width="20px" SortExpression="ONOFF" ItemStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="LASTONSTATUS" HeaderText="ON<br/>Terakhir" ItemStyle-Width="80px" SortExpression="LASTONSTATUS" ItemStyle-HorizontalAlign="Center">
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
    $(document.ready)
    {
        $("#ContentPlaceHolder1_btnAddNew").removeAttr('disabled');
        $("#ContentPlaceHolder1_btnAddNew").click(function() {
            window.open('<%= PageAddOrEditURL  %>','_self');
            return false;
        });
    };
//]]>
</script>

</asp:Content>
