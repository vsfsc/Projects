<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListDataUserControl.ascx.cs" Inherits="VAWebParts.ListData.ListDataUserControl" %>
<style type="text/css">
    .divshow {
        
    }
     .Ul {
         padding: 5px;
         list-style-type:none;
     }
    .Ul li {
        border-bottom: 1px #808080 solid;    
        list-style-type:none;
        padding-top: 10px;
    }
    .Ul li:hover {
        color: #ff4500;
    }
    .Ul li span {
        font-size: 22px;
        font-family:arial
    }
</style>
<asp:Label ID="lbTset" runat="server" Text=""></asp:Label>
<div id="aresult" runat="server">
    <ul class="Ul">
        <li>由我发布：<span>10</span></li>
        <li>条目总数：<span>50</span></li>
        <li>今日更新：<span>8</span></li>
        <li>本周更新：<span>22</span></li>
    </ul>
</div>