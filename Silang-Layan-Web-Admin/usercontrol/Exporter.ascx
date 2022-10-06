<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Exporter.ascx.cs" Inherits="Exporter" %>

<div id="divTools" class="ToolsTable">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                Data yang akan diexport:<br />
                <asp:DropDownList ID="ddlExportData" runat="server" Width="150px">
                    <asp:ListItem Value="0">Halaman ini saja</asp:ListItem>
                    <asp:ListItem Value="1">Semua data</asp:ListItem>
                    <asp:ListItem Value="2">Maks. jumlah data</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtMaksJumlahData" runat="server"
                    onkeypress="NumericValidation(event)"
                    Width="25px">200</asp:TextBox>
            </td>
            <td style="border-right-style: solid; border-right-width: 1px; border-right-color: #EBEBEB">
                &nbsp;</td>
            <td>
                <asp:ImageButton ID="btExportToExcel" runat="server" Font-Names="Arial" 
                    Font-Size="X-Small" Height="30px" ImageUrl="~/images/Export_Excel.png" 
                    onclick="btExportToExcel_Click" 
                    title="Export To Excel" />
            </td>
            <td>
                <asp:ImageButton ID="btExportToWord" runat="server" Font-Names="Arial" title="Export To Word"
                    Font-Size="X-Small" onclick="btExportToWord_Click" 
                    Height="30px" ImageUrl="~/images/Export_Word.png" />
            </td>
            <td>
                <asp:ImageButton ID="btExportToXML" runat="server" Font-Names="Arial" title="Export To XML"
                    Font-Size="X-Small" onclick="btExportToXML_Click" 
                    Height="30px" ImageUrl="~/images/Export_XML.png"/>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript">
    $(document.ready)
    {
        $("#ContentPlaceHolder1_Exporter1_txtMaksJumlahData").hide();
        $("#ContentPlaceHolder1_Exporter1_ddlExportData").change(function(){
            if ($("#ContentPlaceHolder1_Exporter1_ddlExportData").val() == "2")
            {
                $("#ContentPlaceHolder1_Exporter1_txtMaksJumlahData").show();
            } else {
                $("#ContentPlaceHolder1_Exporter1_txtMaksJumlahData").hide();
            }
        });
    }
</script>
