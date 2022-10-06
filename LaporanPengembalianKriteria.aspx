<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="LaporanPengembalianKriteria.aspx.cs" Inherits="INLIS_ENTERPRISE.LaporanPengembalianKriteria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
    #btLaporanPengembalian
    {
        border: solid 4px #e9e06f;
    }
</style>

<link rel="stylesheet" href="css/chosen.css">

<div id="ListArea">
    <div class="titlecontent"><%= PageTitle %></div>
    <div class="ButtonArea">
        <div id="Div2" style='background:#EFEFEF;padding:5px;'>
            <table border="0">
                <tr>
                    <td><b>Kriteria</b></td>
                    <td>:</td>
                    <td colspan="2">
                        <span id="DivPetugas">
                            <asp:dropDownList ID="DdlPetugas" name="DdlPetugas" runat="server" CssClass="chosen-select">
                                <asp:ListItem value="0">--Semua Petugas--</asp:ListItem>
                            </asp:dropDownList>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td><b>Harian</b></td>
                    <td>:</td>
                    <td>
                        <asp:TextBox id="TxtTgl1" name="TxtTgl1" class="dateChar" Width="150px" runat="server" />
                        s/d <asp:TextBox id="TxtTgl2" name="TxtTgl2" class="dateChar" Width="150px" runat="server"  />
                    </td>
                    <td><button id="btnCariHarian" name="btnCariHarian" type="button" class="button-success"><span>Tampilkan Frekuensi</span></button>
                        <button id="btnCariDataHarian" name="btnCariDataHarian" type="button" class="button-primary"><span>Tampilkan Data</span></button>
                    </td>
                </tr>
                <tr>
                    <td><b>Bulanan</b></td>
                    <td>:</td>
                    <td>
                        <asp:dropDownList ID="DdlBulanan1" name="DdlBulanan1" runat="server"></asp:dropDownList>
                        <asp:dropDownList ID="DdlTahunBulan1" name="DdlTahunBulan1" runat="server"></asp:dropDownList>
                        s/d 
                        <asp:dropDownList ID="DdlBulanan2" name="DdlBulanan2" runat="server"></asp:dropDownList>
                        <asp:dropDownList ID="DdlTahunBulan2" name="DdlTahunBulan2" runat="server"></asp:dropDownList>
                    </td>
                    <td><button id="btnCariBulanan" name="btnCariBulanan" type="button" class="button-success"><span>Tampilkan Frekuensi</span></button>
                    <button id="btnCariDataBulanan" name="btnCariDataBulanan" type="button" class="button-primary"><span>Tampilkan Data</span></button>
                    </td>
                </tr>
                <tr>
                    <td><b>Tahunan</b></td>
                    <td>:</td>
                    <td>
                        <asp:dropDownList ID="DdlTahun1" name="DdlTahun1" runat="server"></asp:dropDownList>
                        s/d 
                        <asp:dropDownList ID="DdlTahun2" name="DdlTahun2" runat="server"></asp:dropDownList>
                    </td>
                    <td><button id="btnCariTahunan" name="btnCariTahunan" type="button" class="button-success"><span>Tampilkan Frekuensi</span></button>
                    <button id="btnCariDataTahunan" name="btnCariDataTahunan" type="button" class="button-primary"><span>Tampilkan Data</span></button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<div id="formcontent">
    <iframe src="" width="100%" height="700px" id="report1" frameborder="0" ></iframe>
</div>

<script type="text/javascript">
    $(function () {
        ReportHarian = function (url) {
            var datestart = $("#ContentPlaceHolder1_TxtTgl1").val();
            var dateend = $("#ContentPlaceHolder1_TxtTgl2").val();
            var petugas = $("#ContentPlaceHolder1_DdlPetugas").val();
            $('#report1').attr('src', '' + url + '?mode=date&datestart=' + datestart + '&dateend=' + dateend + '&petugas=' + petugas);
            return false;
        };
        ReportBulanan = function (url) {
            var datestart = "", monthstart = "", yearstart = "", dateend = "", monthend = "", yearend = "";
            var petugas = $("#ContentPlaceHolder1_DdlPetugas").val();
            monthstart = $("#ContentPlaceHolder1_DdlBulanan1").val();
            monthend = parseInt($("#ContentPlaceHolder1_DdlBulanan2").val());
            yearstart = $("#ContentPlaceHolder1_DdlTahunBulan1").val();
            yearend = $("#ContentPlaceHolder1_DdlTahunBulan2").val();
            if (parseInt($("#ContentPlaceHolder1_DdlBulanan1").val()) < 10) monthstart = "0" + $("#ContentPlaceHolder1_DdlBulanan1").val();
            if (parseInt(monthend) < 10) monthend = "0" + monthend;
            datestart = yearstart + "-" + monthstart + "-01";
                
            if ($("#ContentPlaceHolder1_DdlBulanan2").val() != '12') {
                dateend = yearend + "-" + monthend + "-01";
            }
            else {
                monthend = monthend, yearend = parseInt($("#ContentPlaceHolder1_DdlTahunBulan2").val());
                dateend = yearend + "-" + monthend + "-" + "01";
            }

            $('#report1').attr('src', '' + url + '?mode=month&datestart=' + datestart + '&dateend=' + dateend + '&petugas=' + petugas);
            return false;
        };
        ReportTahunan = function (url) {
            var yearstart = $("#ContentPlaceHolder1_DdlTahun1").val();
            var yearend = parseInt($("#ContentPlaceHolder1_DdlTahun2").val());
            var datestart = yearstart + "-01-01";
            var dateend = yearend + "-01-01";
            var petugas = $("#ContentPlaceHolder1_DdlPetugas").val();
            $('#report1').attr('src', '' + url + '?mode=year&datestart=' + datestart + '&dateend=' + dateend + '&petugas=' + petugas);
            return false;
        };
    });

    $(document).ready(function () {
        $(".dateChar").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd-M-yy', yearRange: '1925:c+10' });

        $("#btnCariHarian").click(function () {
            ReportHarian("Rpt_LaporanJumlahPengembalianKriteria.aspx");
        });
        $("#btnCariBulanan").click(function () {
            ReportBulanan("Rpt_LaporanJumlahPengembalianKriteria.aspx");
        });
        $("#btnCariTahunan").click(function () {
            ReportTahunan("Rpt_LaporanJumlahPengembalianKriteria.aspx");
        });

        $("#btnCariDataHarian").click(function () {
            ReportHarian("Rpt_LaporanPengembalianKriteria.aspx");
        });
        $("#btnCariDataBulanan").click(function () {
            ReportBulanan("Rpt_LaporanPengembalianKriteria.aspx");
        });
        $("#btnCariDataTahunan").click(function () {
            ReportTahunan("Rpt_LaporanPengembalianKriteria.aspx");
        });
    });

</script>
<script src="js/chosen/chosen.jquery.js" type="text/javascript"></script>
<script type="text/javascript">
    var config = {
        '.chosen-select': { search_contains: true }
    };
    for (var selector in config) {
        $(selector).chosen(config[selector]);
    };
</script>
</asp:Content>
