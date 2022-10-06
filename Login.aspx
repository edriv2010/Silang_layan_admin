<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultLogin.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="Login" EnableEventValidation="False" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div style="padding: 0px 20px 20px 20px;">
    <br />
    <div class="container-fluid" style="margin-top: 40px;">
        <div class="row-fluid" style="width: auto;">
            <div class="span12" style="width: 400px; margin-left: auto; margin-right: auto; float: none;">
                <div class="social-box" style="background-color: #D8D8FF; font-family: Verdana; ">
                    <div class="header" style="text-align: center">
                        <h4><i class="fa fa-key" style="vertical-align: top; margin-top: 1px;"></i>Login</h4>
                    </div>
                    <div class="body">
                        <br />
                        <div>
                            <table align="center" style="margin-left: auto; margin-right: auto; color: #3f3c84;">
                                <tr>
                                    <td>
                                        Nama Pengguna
                                    </td>
                                    <td>&nbsp; </td>
                                    <td>
                                        <asp:TextBox id="txtUserName" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Kata Sandi
                                    </td>
                                    <td>&nbsp; </td>
                                    <td>
                                        <asp:TextBox id="txtPassword" runat="server" Type="Password"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center; padding-top: 30px;">
                                        <asp:Button id="btSubmit" runat="server" Text="Submit" class="btn btn-primary pull-left span6" Width="100%" />
                                    </td>
                                </tr>
                                <tr id="trCapctha" runat="server">
                                    <td style="text-align: center" colspan="3">
                                        <div class="g-recaptcha" data-sitekey="6LeY4yEUAAAAAM3oavbStXTqJLSUCEwOhNODOD_M"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
