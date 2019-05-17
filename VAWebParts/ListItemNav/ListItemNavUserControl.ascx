<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListItemNavUserControl.ascx.cs" Inherits="VAWebParts.ListItemNav.ListItemNavUserControl" %>
<style type="text/css">
.wrapper { width: 980px; margin: 0 auto; }
/*#header {width: 100%; background: #cfc;}*/
#container { width: 100%; }
#container #left,#container #content, #container #right {float:left;}
#left {font-size:14px;margin-right:20px;}
#content {margin:0 10px;font-size:12px;}
#right {font-size:14px;margin-left:20px;}
</style>
<div id="itemNav" runat="server" class="wrapper">
    <%--<div id="header">新闻导航</div>--%>
     <div id="container">
         <div id="left">上一条</div>
         <div id="content">本条</div>
         <div id="right">下一条</div>
     </div>
</div>