<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="MemberPerpusMitraList.aspx.cs" Inherits="INLIS_ENTERPRISE.MemberPerpusMitraList" %>
<%@ Register src="usercontrol/MessageBoxUsc/MsgBoxUsc.ascx" tagname="MsgBoxUsc" tagprefix="uc1" %>
<%@ Register src="usercontrol/Exporter.ascx" tagname="Exporter" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<link rel="stylesheet" href="css/chosen.css">
<style type="text/css">
    #btDaftarAnggota
    {
        border: solid 4px #e9e06f;
    }
</style>

<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div id="ListArea">
    <div class="titlecontent">
        <asp:Label ID="lbPageTitle" runat="server" Text=""></asp:Label>
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
                                    <asp:DropDownList ID="ddlBranch" runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" CssClass="chosen-select">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlKriteria" runat="server" Width="154px" onchange="tglLahir();">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipePencarian" runat="server" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKataKunci" runat="server" 
                                        ontextchanged="txtKataKunci_TextChanged" Width="150px" AutoPostBack="True" onchange="tglLahir();"></asp:TextBox>
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
                    <asp:ButtonColumn CommandName="Delete" Text="Hapus" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" Visible="False">
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="No" HeaderText="No." ReadOnly="true" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="id" Visible="false">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="MemberNo" HeaderText="No.Anggota" ItemStyle-Width="80px" SortExpression="MemberNo">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="FullName" HeaderText="Nama" ItemStyle-Width="200px" SortExpression="FullName">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="PlaceOfBirth" HeaderText="Tempat Lahir" ItemStyle-Width="100px" SortExpression="PlaceOfBirth">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DateOfBirth" HeaderText="Tgl Lahir" ItemStyle-Width="50px" SortExpression="DateOfBirth" DataFormatString="{0:dd-MM-yyyy}">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Address" HeaderText="Alamat" ItemStyle-Width="200px" SortExpression="Address">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="JenisAnggota" HeaderText="Jenis Anggota" ItemStyle-Width="50px" SortExpression="JenisAnggota">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Sex" HeaderText="L/P" ItemStyle-Width="50px" SortExpression="Sex">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Phone" HeaderText="Telepon" ItemStyle-Width="50px" SortExpression="Phone">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Email" HeaderText="Email" ItemStyle-Width="100px" SortExpression="Email">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="IdentityNo" HeaderText="No.Identitas" ItemStyle-Width="50px" SortExpression="IdentityNo">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="StatusAnggota" HeaderText="Status Anggota" ItemStyle-Width="50px" SortExpression="StatusAnggota">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="CreateDate" HeaderText="Tgl Buat" ItemStyle-Width="100px" SortExpression="members.CreateDate NULLS LAST" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}" ItemStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="UpdateDate" HeaderText="Tgl Update Terakhir" ItemStyle-Width="100px" SortExpression="members.UpdateDate NULLS LAST" DataFormatString="{0:dd-MM-yyyy HH:mm:ss}" ItemStyle-HorizontalAlign="Center">
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
        tglLahir();
    };

    function tglLahir() {
        var kriteria=document.getElementById("ContentPlaceHolder1_ddlKriteria");
        if (kriteria[kriteria.selectedIndex].value == "DateOfBirth"){
            $('#ContentPlaceHolder1_txtKataKunci').datepicker({
                changeMonth: true, changeYear: true, dateFormat: 'dd-mm-yy', yearRange: '1925:c+10'
            });
        } else{
            $("#ContentPlaceHolder1_txtKataKunci").datepicker("destroy");
        };
    };
//]]>
</script>

<script src="js/chosen/chosen.jquery.js" type="text/javascript"></script>
<script type="text/javascript">
    SetChosenSelect = function () {
        var config = {
            '.chosen-select': { search_contains: true }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        };
    };
    SetChosenSelect();
</script>
</asp:Content>
