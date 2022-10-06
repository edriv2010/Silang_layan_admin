<%@ Page Language="C#" MasterPageFile="~/DefaultBlank.master" AutoEventWireup="True" Inherits="HistoryData" Codebehind="HistoryData.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="titlecontent">History Update</div>
<div>
    <table cellpadding="0" cellspacing="0" border="0" 
        style="width: 99%; background-color: #FFFFFF;">
        <tr>
            <td valign="top">            
                <asp:DataGrid ID="dgData" runat="server" 
                    CellPadding="4" Font-Names="Tahoma" 
                    Font-Size="12px" GridLines="None" Width="100%" 
                    AutoGenerateColumns="False" ShowFooter="False"
                    AllowSorting="False" CssClass="Grid">
                    <HeaderStyle CssClass="GridHeader" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle CssClass="GridItem"></ItemStyle>
                    <AlternatingItemStyle CssClass="GridAltItem"></AlternatingItemStyle>
                    <Columns>
                        <asp:BoundColumn DataField="No" HeaderText="No." ReadOnly="true" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Right">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ActionBy" HeaderText="Update By" ItemStyle-Width="150px" SortExpression="ActionBy">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ActionDate" HeaderText="Update Date" ItemStyle-Width="150px" SortExpression="ActionDate" DataFormatString="{0:dd MMMM yyyy, HH:mm:ss}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ActionTerminal" HeaderText="Update Terminal" ItemStyle-Width="150px" SortExpression="ActionTerminal">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Note" HeaderText="Catatan" SortExpression="Note" HeaderStyle-HorizontalAlign="Left">
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    <br />
    <br />
</div>

</asp:Content>

