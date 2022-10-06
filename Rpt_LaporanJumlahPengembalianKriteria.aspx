<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rpt_LaporanJumlahPengembalianKriteria.aspx.cs" Inherits="INLIS_ENTERPRISE.Rpt_LaporanJumlahPengembalianKriteria" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<link href="css/styles.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript" src="js/jquery/jquery.1.8.2.min.js"></script>
<script language="javascript" type="text/javascript">
    function printdiv() {
        var headstr = "<html><head><title></title></head><body>";
        var footstr = "</body>";
        var newstr = $("#ReportViewer1_ctl10").html()
        var popupWin = window.open('', '_blank');
        popupWin.document.write(headstr + newstr + footstr);
        popupWin.print();
        return false;
    }
   
   
</script>

<head id="Head1" runat="server">
   <title>AS</title>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        
    <%--<div id='printReport'><input name="b_print" type="button" onclick="printdiv();" value="Print Report" class="" /></div>--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    </asp:ScriptManager>
    <rsweb:reportviewer runat="server" id="ReportViewer1" BorderStyle="Groove" Width="100%" BorderWidth="0">
        <LocalReport EnableExternalImages="True" EnableHyperlinks="True" ></LocalReport>
    </rsweb:reportviewer>
    </div>
    </form>
</body>