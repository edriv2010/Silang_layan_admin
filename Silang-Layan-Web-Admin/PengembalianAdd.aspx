<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="PengembalianAdd.aspx.cs" Inherits="INLIS_ENTERPRISE.PengembalianAdd" %>

<%@ Register Src="usercontrol/MessageBoxUsc/MsgBoxUsc.ascx" TagName="MsgBoxUsc" TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">

<style type="text/css">
    #btEntriPengembalian
    {
        border: solid 4px #e9e06f;
    }
</style>

<style type="text/css">
    .classname
    {
        -moz-box-shadow: inset 0px 1px 0px 0px #bbdaf7;
        -webkit-box-shadow: inset 0px 1px 0px 0px #bbdaf7;
        box-shadow: inset 0px 1px 0px 0px #bbdaf7;
        background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #79bbff), color-stop(1, #378de5) );
        background: -moz-linear-gradient( center top, #79bbff 5%, #378de5 100% );
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#79bbff', endColorstr='#378de5');
        background-color: #79bbff;
        -webkit-border-top-left-radius: 20px;
        -moz-border-radius-topleft: 20px;
        border-top-left-radius: 20px;
        -webkit-border-top-right-radius: 20px;
        -moz-border-radius-topright: 20px;
        border-top-right-radius: 20px;
        -webkit-border-bottom-right-radius: 20px;
        -moz-border-radius-bottomright: 20px;
        border-bottom-right-radius: 20px;
        -webkit-border-bottom-left-radius: 20px;
        -moz-border-radius-bottomleft: 20px;
        border-bottom-left-radius: 20px;
        text-indent: 0;
        border: 1px solid #84bbf3;
        display: inline-block;
        color: #ffffff;
        font-family: Arial;
        font-size: 12px;
        font-weight: bold;
        font-style: normal;
        height: 28px;
        line-height: 28px;
        width: 90px;
        text-decoration: none;
        text-align: center;
        text-shadow: 1px 1px 0px #528ecc;
    }
    .classname:hover
    {
        background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #378de5), color-stop(1, #79bbff) );
        background: -moz-linear-gradient( center top, #378de5 5%, #79bbff 100% );
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#378de5', endColorstr='#79bbff');
        background-color: #378de5;
    }
    .classname:active
    {
        position: relative;
        top: 1px;
    }
</style>


<div id="ListArea">
    <div class="titlecontent">
        <asp:Label ID="lbPageTitle" runat="server" Text=""></asp:Label></div>
    <div class="ButtonArea">
        <asp:Button ID="btnSave" runat="server" Text="Selesai" onclick="btnSave_Click" class="button-success"/>
        <% if (EditID != "") {%>
        <button id="btDelete" type="button" onclick="dodelete()" class="button-danger">
            <span>Hapus</span>
        </button>
        <%} %>
        <%--<button id="btnCancel" type="button">
            <span>Selesai</span>
        </button>--%>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/loader.gif" AlternateText="Proses..." />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="formContent" runat="server">
                <div style="background:#73b9d7;border-radius:5px;margin:5px;position:relative;color:#fff;">
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px; text-indent: 10px;" ><b>ITEM ID</b></td>
                        <td style="width: 3px" >:</td>
                        <td >
                            <table>
                                <tr>
                                    <td>   
                                        <asp:TextBox ID="TxtItemID" runat="server"   AutoPostBack="true"  Width="180px" 
                                            ontextchanged="TxtItemID_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCariItem" runat="server" Text="Cari" onclick="btnCariItem_Click" CssClass="button-primary"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </div>
            </div>
            <asp:Panel ID="panelInfo" runat="server" Visible="false" style="margin:5px;padding:5px;" >
                <asp:label runat="server" text="-" id="lbMemberId" Visible="false" style="font-weight: 700" />
                <table style="border: 0px solid #CCCCCC; width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" width="170px"><img id="fotoanggota" runat="server" class="border" height="190" /></td>
                        <td valign="top">
                        <div style="background:#f6f6f6;padding:10px 20px;border-radius:5px;">
                            <table width="100%">
                                <tr>
                                    <td height="40px" width="150px"><b>Nama Anggota</b></td>
                                    <td>: <asp:label runat="server" text="-" id="lblNamaAnggota" style="font-weight:bold;" /></td>
                                </tr>
                                <tr>
                                    <td height="40px" style="border-top:1px solid #ccc;"><b>Profesi</b></td>
                                    <td style="border-top:1px solid #ccc;">: <asp:label runat="server" text="-" id="lblTipeKeanggotaan" style="font-weight: 700" /></td>
                                </tr>
                                <tr>
                                    <td height="40px" style="border-top:1px solid #ccc;"><b>Tgl Pendaftaran</b></td>
                                    <td style="border-top:1px solid #ccc;">: <asp:label runat="server" text="-" id="lblTglRegistrasi" style="font-weight: 700" /></td>
                                </tr>
                                <tr>
                                    <td height="40px" style="border-top:1px solid #ccc;"><b>Masa Berlaku Sampai</b></td>
                                    <td style="border-top:1px solid #ccc;">: <asp:label runat="server" text="-" id="lblBerlakuHingga" style="font-weight: 700" /></td>
                                </tr>
                            </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:Panel ID="panelReturnLoan" runat="server" Visible="false" style="padding:5px;">
                <div style=" text-align: center;font-size:14px; 
                    background-color: #73b9d7;padding:10px;color:#fff;text-shadow: 0 1px 2px #222;margin-bottom:3px; border-radius:5px;">
                <b>KOLEKSI YANG MASIH DIPINJAM</b></div>
                <asp:DataGrid ID="gdReturnLoan" runat="server" CellPadding="4" Font-Names="Tahoma" 
                 Font-Size="12px" GridLines="None" Width="100%" AutoGenerateColumns="False" CssClass="Grid" 
                 onitemdatabound="gdReturnLoan_ItemDataBound" onitemcommand="gdReturnLoan_ItemCommand" >
                    <HeaderStyle CssClass="GridHeader" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle CssClass="GridItem"></ItemStyle>
                    <AlternatingItemStyle CssClass="GridAtlItem"></AlternatingItemStyle>                        
                    <Columns>
                        <asp:TemplateColumn HeaderText="Aksi" ItemStyle-Width="280px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lb" runat="server" Text="Kembali" CommandName="Kembali" Style="background-color: Lime" class="classname"/>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lbRenew" runat="server" Text="Perpanjang" CommandName="Perpanjang" Style="background-color: Lime" class="classname"/>
                            </ItemTemplate>
                        </asp:TemplateColumn>                    
                        <asp:BoundColumn DataField="NomorPinjam" HeaderText="No.Peminjaman" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Width="100px"/>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="NomorBarcode" HeaderText="Item ID" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Width="100px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Title" HeaderText="Judul" ItemStyle-Width="450px">
                            <ItemStyle Width="650px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TglPinjam" HeaderText="Tgl Pinjam" ItemStyle-Width="150px" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Width="150px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TglHarusKembali" HeaderText="Tgl Harus Kembali" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle Width="150px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="NomorPinjam" HeaderText="NomorPinjam" ItemStyle-Width="300px" Visible="false">
                            <ItemStyle Width="300px" />
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </asp:Panel>
            <br />
            <asp:Panel ID="panelRealReturn" runat="server" Visible="false">
                <div style=" text-align: center;font-size:14px; 
                    background-color: #73b9d7;padding:10px;color:#fff;text-shadow: 0 1px 2px #222;margin-bottom:3px; border-radius:5px;">
                <b>Daftar Koleksi Yang Dikembalikan Hari Ini</b></div>
                <asp:DataGrid ID="gdRealReturn" runat="server" CellPadding="4" Font-Names="Tahoma" 
                 Font-Size="12px" GridLines="None" Width="100%" 
                 AutoGenerateColumns="False" ShowFooter="False" CssClass="Grid" 
                 onitemcommand="gdRealReturn_ItemCommand" onitemdatabound="gdRealReturn_ItemDataBound" >
                    <HeaderStyle CssClass="GridHeader" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle CssClass="GridItem"></ItemStyle>
                    <AlternatingItemStyle CssClass="GridAtlItem"></AlternatingItemStyle>                    
                    <Columns>
                        <asp:TemplateColumn HeaderText="Aksi" ItemStyle-Width="100px" Visible="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbPelanggaran" runat="server" Text="Pelanggaran" CommandName="Pelanggaran" Style="background-color: Lime" class="classname"/>
                            </ItemTemplate>
                        </asp:TemplateColumn>                        
                        <asp:BoundColumn DataField="ControlNumber" HeaderText="ControlNumber" ItemStyle-Width="300px" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ColTitle" HeaderText="Judul" ItemStyle-Width="600px" ></asp:BoundColumn>
                        <asp:BoundColumn DataField="ActualReturn" HeaderText="Tgl.Kembali" ItemStyle-Width="100px" DataFormatString="{0:dd-MM-yyyy}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="LateDays" HeaderText="Lama Keterlambatan" ItemStyle-Width="50px" ></asp:BoundColumn>
                        <asp:BoundColumn DataField="PenaltyAmount" HeaderText="Jumlah Denda" ItemStyle-Width="50px" Visible="false" ></asp:BoundColumn>
                        <asp:BoundColumn DataField="Collection_ID" HeaderText="Collection_ID" ItemStyle-Width="300px" Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CollectionLoan_id" HeaderText="CollectionLoan_id" ItemStyle-Width="300px" Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NomorBarcode" HeaderText="NomorBarcode" ItemStyle-Width="300px" Visible="false"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </asp:Panel>
            <br />
            <asp:Panel ID="panelPelanggaran" runat="server" Visible="false">
                <div style=" text-align: center;font-size:14px; 
                    background-color: #73b9d7;padding:10px;color:#fff;text-shadow: 0 1px 2px #222;margin-bottom:3px; border-radius:5px;">
                <b>Koleksi Yang Terkena Pelanggaran</b></div>
                <asp:gridview ID="Gridview1" runat="server" CssClass="Grid" ShowFooter="True" 
                 AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                 GridLines="None" onrowdatabound="Gridview1_RowDataBound" Width="100%" >
                    <HeaderStyle CssClass="GridHeader" HorizontalAlign="Center" />
                    
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="RowNumber" HeaderText="No" />
                        <asp:TemplateField HeaderText="Item ID">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" class="input-flat" ReadOnly="true" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No.Peminjaman">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox2" class="input-flat" ReadOnly="true" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pelanggaran">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlPelanggaran" runat="server" Width="150px">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Jenis Denda">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDenda" runat="server" Width="150px"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Jumlah Denda">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox3" class="input-flat" runat="server"  Text="0" onkeypress="NumericValidation(event)"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Jumlah Hari Suspend">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox4" class="input-flat" runat="server"  Text="0" onkeypress="NumericValidation(event)"></asp:TextBox>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <div class="button">
                                <asp:Button ID="ButtonAdd" runat="server" Text="Simpan" OnClick="ButtonAdd_Click" css="classname"/>
                                </div>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:gridview>
                <asp:Button ID="ButtonAdd2" runat="server" Text="Add New Row" OnClick="ButtonAdd2_Click" Visible="false"/>
                
            </asp:Panel>
            <br />
            <table style="width: 30%;">
                <tr>
                    <td class="style4" colspan="3">
                        &nbsp;Keterangan :</td>
                </tr>
                <tr>
                    <td class="style4">
                        &nbsp;</td>
                    <td style="background-color:Red">
                        &nbsp;</td>
                    <td>
                        &nbsp;Sudah jatuh tempo.</td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:content>
