<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewActivityUserControl.ascx.cs" Inherits="MyTasks.NewActivity.NewActivityUserControl" %>
<style type="text/css">
     .dt
        {
            width:80px;
        }
</style>
<table  style="font-size: 14px; color: #494b4c" cellspacing="0" cellpadding="3" runat ="server" id ="tbActivity">
    <tr><td colspan="2"></td> </tr>
    <tr><td>活动时间：</td><td>
        <SharePoint:DateTimeControl ID="dtStart" runat="server" DateOnly="false"  CssClassTextBox="dt" />
    </td></tr>
    <tr><td>活动操作：</td><td>
        <asp:DropDownList ID="ddlAction" style="width:100px" AutoPostBack="true" runat="server"></asp:DropDownList><asp:TextBox ID="txtAction" runat="server" Width="100px" Height="20px"></asp:TextBox></td></tr>
    <tr><td></td><td><span id="spanTxtDesc"  runat ="server" style="margin-left:0px; color:#0072c6" >若列表中没有你要的操作,请在上方文本框输入</span></td></tr>
    <tr><td>活动时长：</td><td><asp:TextBox ID="txtActitualDuring" runat="server" Width="200px" Height="20px"></asp:TextBox></td></tr>
    
    <tr><td> </td><td style="text-align:right " ><asp:Button ID="btnSave" runat="server"   Text="保存" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server"   Text="取消" /></td></tr>
    <tr><td colspan="2"><asp:Label ID="lblMsg" ForeColor="Red" runat="server" Text=""></asp:Label></td> </tr>
</table>