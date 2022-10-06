<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="True" CodeBehind="Default.aspx.cs" Inherits="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<link href="css/admin.css" rel="stylesheet" type="text/css" />
<link href="css/dashboard.css" rel="stylesheet" type="text/css" />

<style type="text/css">
    #divInfo
    {
        font-size: 14px;
    }
    #divInfo img 
    {
        width: 80%;
        margin-left: auto;
        margin-right: auto;
        display: block;
    }
    #MainContentRight
    {
        opacity: 0.9 !important;
    }
    #btBeranda
    {
        border: solid 4px #e9e06f;
    }
</style>

<div>
    <div id="divInfoJumlah" class="count_container" style="margin-top: 10px;">
        <div class="col-md-12 col-sm-12 col-xs-12 tile_stats_count smallsuper-box box-green box" style="background-image: url('images/bg-lain.png')">
            <div class="col-md-12 col-sm-12 col-xs-12 count_row_1">
                <table style="margin-left: auto; margin-right: auto;">
                    <tr>
                        <td style="padding: 4px; color: #D98838; width: 100px;"><i class="fa fa-bookmark-o"></i><span> Jumlah Anggota</span></td>
                        <td style="padding: 4px; color: #D98838; width: 100px;"><i class="fa fa-bookmark-o"></i><span> Jumlah Pengembalian</span></td>
                        <td style="padding: 4px; color: #D98838; width: 100px;"><i class="fa fa-bookmark-o"></i><span> Jumlah Pengiriman</span></td>
                    </tr>
                    <tr>
                        <td><span class="count" title="Jumlah Anggota"><%= DisplayJumlahAnggota %></span></td>
                        <td><span class="count" title="Jumlah Pengembalian"><%= DisplayJumlahPengembalian %></span></td>
                        <td><span class="count" title="Jumlah Pengiriman"><%= DisplayJumlahPengiriman %></span></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div style="text-align: center;">
        <table align="center">
            <tr>
                <td style="padding-right: 4px;">
                    Tahun : 
                </td>
                <td>
                    <asp:DropDownList ID="ddlTahun" runat="server" Width="150px" 
                        AutoPostBack="True" onselectedindexchanged="ddlTahun_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>

    <table style="width: 100%">
        <tr>
            <td>
                <div id="divGrafik" runat="server" style="padding: 50px; padding-top: 0px;">
                    <div id="divGraphGetJumlahAnggotaPerPeriode" class="InputTable" style="min-width: 310px; height: 400px; max-width: 100%; margin: 0 auto; margin-top: 8px; background-color: #EAEAFF;"></div>
                    <br /><br />
                </div>
            </td>
        </tr>
    </table>
</div>

<script src="js/highcharts/highcharts.js" type="text/javascript"></script>
<script src="js/highcharts/modules/exporting.js" type="text/javascript"></script>

<script type="text/javascript">
    OpenMultiChart = function (link, graphtype, graphtitle, graphxtitle) {
        var seriesOptions = [],
        seriesCounter = 0,
        names = ['Anggota'];

        $.each(names, function (j, name) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Default.aspx/" + link,
                data: "{ 'Tahun' : '" + $("#ContentPlaceHolder1_ddlTahun").val() + "', 'Name' : '" + name + "' }",
                dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                    var data = [];
                    var total = 0;
                    for (var i in Result) {
                        var serie = new Array(Result[i].Name, Result[i].Value);
                        total += Result[i].Value;
                        data.push(serie);
                    };

                    seriesOptions[j] = {
                        name: name,
                        data: data
                    };

                    seriesCounter += 1;

                    if (seriesCounter === names.length) {
                        DrawMultiChart(link, seriesOptions, total, graphtype, graphtitle, graphxtitle);
                    };
                },
                error: function (Result) {
                    //alert("Error");
                }
            });
        });
    };

    function DrawMultiChart(link, seriesOptions, total, graphtype, graphtitle, graphxtitle) {
        $('#divGraph' + link).highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: 1,
                plotShadow: false
            },
            title: {
                text: graphtitle
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.y}</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            xAxis: {
                categories: []
            },
            yAxis: {
                title: {
                    enabled: true,
                    text: 'Jumlah',
                    style: {
                        fontWeight: 'normal'
                    }
                }
            },
            series: seriesOptions
        });
    };

    OpenAllChart = function () {
        var periode = "Tahun";
        if ($("#ContentPlaceHolder1_ddlTahun").val() != "--Semua Tahun--") {
            periode = "Tahun " + $("#ContentPlaceHolder1_ddlTahun").val();
        };
        OpenMultiChart('GetJumlahAnggotaPerPeriode', 'line', 'Statistik Jumlah Anggota per ' + periode, 'Jumlah Anggota');
    };

    OpenAllChart();

</script>
</asp:Content>
