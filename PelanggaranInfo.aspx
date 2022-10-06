<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PelanggaranInfo.aspx.cs" Inherits="INLIS_ENTERPRISE.PelanggaranInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" value="" runat="server" id="OriginalID" />

    <div id="DivDetail" runat="server" align="center">
        <!-- start detail anggota -->
         <table cellpadding="4" cellspacing="1" class="InputTable" id="TblInfo" runat="server" visible="false">
        <tr>
            <td style="width: 100px">
                No.Anggota</td>
            <td style="width: 3px">
                :
            </td>
            <td style="width: 300px">
                <asp:textbox id="NoAnggotaTextBox" class="alphNum" runat="server" columns="40"
                    Width="100%" Enabled="false"/>
            </td>
            <td style="width: 2px">
                &nbsp;</td>
            <td colspan="3" rowspan="3" valign="top" style="width: 100px">
               <!-- tempat fotoo dewa -->
               <div id="fotoanggotaarea" style="text-align: center; width: 100%;"> 
                    <img id="fotoanggota" class="border" runat="server" alt="" src="-" style='max-height:200px;'/>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                Nama</td>
            <td>
                :
            </td>
            <td>
                <asp:textbox id="NamaTextBox" class="alphNum" runat="server" 
                    columns="40" Width="100%" Enabled="false" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                Alamat Sesuai KTP</td>
            <td>
                :
            </td>
            <td>
                <asp:textbox id="AlamatRumahTextBox" runat="server" class="alphNum" columns="40"
                    height="40px" textmode="MultiLine" Width="100%" Enabled="false"></asp:textbox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                Alamat Tempat Tinggal Sekarang</td>
            <td>
                :</td>
            <td>
                <asp:textbox id="AlamatTempatTinggal" runat="server" class="alphNum" columns="40"
                    height="40px" textmode="MultiLine" Width="100%" Enabled="false"></asp:textbox>
            </td>
            <td>
                &nbsp;</td>
            <td style="width: 120px">
                &nbsp;</td>
            <td style="width: 3px;">
                &nbsp;</td>
            <td class="style2" style="width: 100px">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                No. HP</td>
            <td>
                :</td>
            <td>
                <asp:textbox id="TlpHp" runat="server" class="alphNum" 
                    onkeypress="PhoneValidation(event);" Enabled="false"></asp:textbox>
            </td>
            <td>
                &nbsp;</td>
            <td style="width: 120px">
                No. Telepon
                Rumah</td>
            <td style="width: 3px;">
                :</td>
            <td class="style2" style="width: 100px">
                <asp:textbox id="TlpTextBox" runat="server" class="alphNum" Enabled="false"></asp:textbox>
            </td>
        </tr>
        <tr>
            <td colspan="7">
                <strong>Detail Pelanggaran :</strong></td>
        </tr>
        <tr>
            <td colspan="7">
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                Jenis Pelanggaran </td>
            <td>
                :</td>
            <td>
                <asp:textbox id="TxtJenis" runat="server" class="alphNum" 
                    columns="40" Width="100%" Enabled="false"></asp:textbox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                Sangsi</td>
            <td>
                :</td>
            <td>
                <asp:textbox id="TxtSangsi" runat="server" class="alphNum" 
                    columns="40" Width="100%" Enabled="false"></asp:textbox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                Jumlah Denda</td>
            <td>
                :</td>
            <td align="left">
                <asp:textbox id="TxtDenda" runat="server" class="alphNum" 
                    Enabled="false"></asp:textbox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                Jumlah Suspend</td>
            <td>
                &nbsp;</td>
            <td align="left">
                <asp:textbox id="TxtSuspen" runat="server" class="alphNum" 
                    Enabled="false"></asp:textbox>
            &nbsp; Hari</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
        <!-- end detail anggota -->
    </div>
    </form>
</body>
</html>
