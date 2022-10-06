<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="PengembalianList.aspx.cs" Inherits="PengembalianList" %>
<%@ Register src="usercontrol/MessageBoxUsc/MsgBoxUsc.ascx" tagname="MsgBoxUsc" tagprefix="uc1" %>
<%@ Register src="usercontrol/Exporter.ascx" tagname="Exporter" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
    #btDaftarPengembalian
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
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
            <div class="ButtonArea">
                <button id="btnAddNew" type="button" class="button-success">
                    <span>Tambah</span>
                </button>
                <asp:Button ID="btKirim" runat="server" Text="Kirim" OnClientClick="return DoKirim();" OnClick="btKirim_Click" CssClass="button-primary" />
            </div>
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
                                <%--<td>
                                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="chosen-select" Width="100%" AutoPostBack="True" onselectedindexchanged="ddlBranch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>--%>
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
                    <asp:BoundColumn DataField="No" HeaderText="No." ReadOnly="true" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right">
                    </asp:BoundColumn>
                    <asp:TemplateColumn HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbAllCheck" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span id="spanID" title='<%# DataBinder.Eval(Container.DataItem, "id") %>' style="display: none"><%# DataBinder.Eval(Container.DataItem, "id")%></span>
                            <asp:CheckBox ID="cbCheck" runat="server" CssClass="member" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="No Anggota" SortExpression="MemberNo">
                        <ItemTemplate>
                            <span onclick="OpenPencarianLanjut('<%# Eval("idpinjam") %>','<%# Eval("ItemID") %>');return false;" 
                                onmouseout="if(this.style.color=='green'){this.style.color='Blue';}" 
                                onmouseover="this.style.cursor='pointer'; if(this.style.color=='blue'){this.style.color='Green'}" 
                                style="color: Black; text-decoration: underline; cursor: pointer;" 
                                title="Detail Anggota">
                                <asp:Label ID="LastNameTB" runat="server" Text='<%# Bind("MemberNo") %>'></asp:Label>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="MemberName" HeaderText="Nama Anggota" SortExpression="members.FullName" ItemStyle-Width="200px">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="NomorBarcode" HeaderText="Item ID" SortExpression="NomorBarcode" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Title" HeaderText="Judul" SortExpression="collectionloanitems.Title" ItemStyle-Width="200px">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Author" HeaderText="Pengarang" SortExpression="collectionloanitems.Author" ItemStyle-Width="200px">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Publisher" HeaderText="Penerbit" SortExpression="collectionloanitems.Publisher" ItemStyle-Width="200px" Visible="False">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="LoanDate" HeaderText="Tgl Pinjam" SortExpression="LoanDate" ItemStyle-Width="50px" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DueDate" HeaderText="Tgl Harus Kembali" SortExpression="DueDate" ItemStyle-Width="50px" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="ActualReturn" HeaderText="Tgl Dikembalikan" SortExpression="ActualReturn" ItemStyle-Width="50px" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="LateDays" HeaderText="Terlambat" SortExpression="LateDays" ItemStyle-Width="20px" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Center">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="StatusKirim" HeaderText="Status Kirim" SortExpression="StatusKirim" ItemStyle-Width="50px" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Center">
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
            <input id="HiddenCheckeds" runat="server" type="hidden" />
            <uc1:MsgBoxUsc ID="MsgBoxUsc1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc2:Exporter ID="Exporter1" runat="server" />
    </div>
    <div id="dialog-pelanggaraninfo" title="Informasi Pelanggaran Anggota"></div>
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
    
    TableGetCheckedItemsSpecificColumn = function (TableID, JoinDelimeter, PrimaryColumnIndex, PrimaryID) {
        var ListItemID = new Array();
        var i1 = 0;
        $("#" + TableID + " tr").each(function () {
            if ($(this).closest('table').attr("id") == TableID) {
                if (i1 > 0) {
                    var i2 = 0;
                    var IsFoundColumn = false;
                    var IsFoundObject = false;
                    $(this).find('td').each(function () {
                        if (i2 == PrimaryColumnIndex && !IsFoundColumn) {
                            IsFoundColumn = true;
                            if (!IsFoundObject) {
                                $(this).find("span[id*='" + PrimaryID + "']").each(function () {
                                    var PrimaryItemID = $(this).attr("title");
                                    if ($(this).attr("id").indexOf(PrimaryID) != -1) {
                                        $(this).parent('td').find('input').each(function () {
                                            var t = $(this).is(':checked');
                                            if (t) {
                                                ListItemID.push(PrimaryItemID);
                                            };
                                            IsFoundObject = true;
                                            return false;
                                        });
                                    };
                                });
                            };
                        };
                        i2 = i2 + 1;
                    });
                };
            };
            i1 = i1 + 1;
        });
        return ListItemID.join(JoinDelimeter);
    };

    RegisterCheckbox = function()
    {
        $("#ContentPlaceHolder1_dgData input[id*='cbCheck']:checkbox").click(function() {
            //Get number of checkboxes in list either checked or not checked
            var totalCheckboxes = $("#ContentPlaceHolder1_dgData input[id*='cbCheck']:checkbox").size();
            //Get number of checked checkboxes in list
            var checkedCheckboxes = $("#ContentPlaceHolder1_dgData input[id*='cbCheck']:checkbox:checked").size();
            //Check / Uncheck top checkbox if all the checked boxes in list are checked
            $("#ContentPlaceHolder1_dgData input[id*='cbAllCheck']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);
        });

        $("#ContentPlaceHolder1_dgData input[id*='cbAllCheck']:checkbox").click(function () {
            //Check/uncheck all checkboxes in list according to main checkbox
            $("#ContentPlaceHolder1_dgData input[id*='cbCheck']:checkbox").attr('checked', $(this).is(':checked'));
        });

    };
    
    DoKirim = function() {
        var checkedCheckboxes = $("#ContentPlaceHolder1_dgData input[id*='cbCheck']:checkbox:checked").size();
        if (checkedCheckboxes == 0) {
            alert('Tidak ada item yang dipilih');
            return false;
        };

        if (confirm("Apakah Anda yakin melakukan pengiriman atas data yang dipilih?")) {
            var Items = TableGetCheckedItemsSpecificColumn("ContentPlaceHolder1_dgData", "|", 1, "spanID");
            $("#ContentPlaceHolder1_HiddenCheckeds").val(Items);
            return true;
        };
    };

    $(document.ready)
    {
        $("#btnAddNew").removeAttr('disabled');
        $("#btnAddNew").click(function() {
            window.open('<%= PageAddOrEditURL  %>','_self');
            return false;
        });
    };

    $("#dialog-memberinfo").hide();
    OpenPencarianLanjut = function(abc,xyz) {
        $("#dialog-pelanggaraninfo").dialog({
            title: 'Informasi Pelanggaran Anggota',
            modal: true,
            width: 750,
            height: 630,
            autoOpen: false,
            buttons: [
                {
                    text: "Tutup",
                    click: function() { $(this).dialog("close"); }
                }
            ],
            open : function(type, data){
	            $(this).parent().appendTo("form");
	        }
	    });
	    LoadPage("PelanggaranInfo.aspx?id=" + abc +"&item="+xyz, "dialog-pelanggaraninfo");
        $("#dialog-pelanggaraninfo").dialog('open');
    };
//]]>
</script>

<script src="js/chosen/chosen.jquery.js" type="text/javascript"></script>
<script type="text/javascript">
    SetChoosenSelect = function () {
        var config = {
            '.chosen-select': { search_contains: true }
        };
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        };
    };
    SetChoosenSelect();
</script>
</asp:Content>
