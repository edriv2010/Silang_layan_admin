<%@ Control Language="C#" AutoEventWireup="true" Inherits="MessageBoxUsc_MsgBoxUsc" Codebehind="MsgBoxUsc.ascx.cs" %>
<asp:UpdatePanel ID="udpMsj" runat="server" UpdateMode="Conditional" RenderMode="Block">
    <ContentTemplate>
        <asp:Button ID="btnD" runat="server" Text="" Style="display: none" Width="0" Height="0" />
        <asp:Button ID="btnD2" runat="server" Text="" Style="display: none" Width="0" Height="0" />
        <asp:Panel ID="pnlMsg" runat="server" CssClass="mp" Style="display: none" Width="550px">
            <asp:Panel ID="pnlMsgHD" runat="server" CssClass="mpHd">
                &nbsp;Message</asp:Panel>
            <asp:GridView ID="grvMsg" runat="server" ShowHeader="false" Width="100%" 
                AutoGenerateColumns="false" BackColor="White">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Image ID="imgErr" runat="server" ImageUrl="~/USERCONTROL/MessageBoxUsc/err.png"
                                            Visible=' <%# (((Message)Container.DataItem).MessageType == enmMessageType.Error) ? true : false %>' />
                                        <asp:Image ID="imgSuc" runat="server" ImageUrl="~/USERCONTROL/MessageBoxUsc/suc.png"
                                            Visible=' <%# (((Message)Container.DataItem).MessageType == enmMessageType.Success) ? true : false %>' />
                                        <asp:Image ID="imgAtt" runat="server" ImageUrl="~/USERCONTROL/MessageBoxUsc/att.png"
                                            Visible=' <%# (((Message)Container.DataItem).MessageType == enmMessageType.Attention) ? true : false %>' />
                                        <asp:Image ID="imgInf" runat="server" ImageUrl="~/USERCONTROL/MessageBoxUsc/inf.png"
                                            Visible=' <%# (((Message)Container.DataItem).MessageType == enmMessageType.Info) ? true : false %>' />
                                    </td>
                                    <td>
                                        <%# Eval("MessageText")%>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="mpClose">
                <asp:Button ID="btnOK" runat="server" Text="OK" CausesValidation="false" Width="60px" Height="35px" CssClass="button-primary" />
                <asp:Button ID="btnPostOK" runat="server" Text="OK" CausesValidation="false" OnClick="btnPostOK_Click"
                    Visible="false" Width="60px" Height="35px" CssClass="button-primary"/>
                <asp:Button ID="btnPostCancel" runat="server" Text="Cancel" CausesValidation="false"
                    OnClick="btnPostCancel_Click" Visible="false" Width="60px" Height="35px" CssClass="button-success"/>
            </div>
        </asp:Panel>
        <AjaxControls:ModalPopupExtender ID="mpeMsg" runat="server" TargetControlID="btnD"
            PopupControlID="pnlMsg" PopupDragHandleControlID="pnlMsgHD" BackgroundCssClass="mpBg"
            DropShadow="true" OkControlID="btnOK">
        </AjaxControls:ModalPopupExtender>
    </ContentTemplate>
</asp:UpdatePanel>