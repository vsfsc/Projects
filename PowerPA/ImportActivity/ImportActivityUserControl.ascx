﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportActivityUserControl.ascx.cs" Inherits="PowerPA.ImportActivity.ImportActivityUserControl" %>
<div id="AppAction" runat="server" visible="true">

    <table>
        <tr>
            <td colspan="4">
                <table id="spanDate" runat="server" border="0">
                    <tr>
                        <td>开始日期：</td>
                        <td>
                            <SharePoint:DateTimeControl ID="dtStart" runat="server" DateOnly="True" />
                        </td>
                        <td>结束日期：</td>
                        <td>
                            <SharePoint:DateTimeControl ID="dtEnd" runat="server" DateOnly="True" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:FileUpload ID="FileUpload1" runat="server" /></td>
            <td>
                <asp:Button ID="btnImport" runat="server" Text="从Excel导入" Width="100" Height="30" CssClass="btn" />
            </td>
            <td style="padding: 10px; text-align: left">
                <asp:Button ID="btnExport" runat="server" Text="导出到Excel" Width="100" Height="30" CssClass="btn" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="4" style="padding: 10px;">

                <div runat="server" id="divSaveAs" style="text-align: center; padding-left: 20px; color: red"></div>
            </td>
        </tr>
    </table>

    <asp:Label ID="lblMsg" ForeColor="Red" runat="server" Text=""></asp:Label>

</div>
