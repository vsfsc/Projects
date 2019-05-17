<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserActivitiesUserControl.ascx.cs" Inherits="VAWebParts.UserActivities.UserActivitiesUserControl" %>

<style type="text/css">
    .divshow {
        
    }
     .activiesUl {
         padding: 5px;
     }
    .activiesUl li {
            
        list-style-type:none;
    }
    .activiesUl li:hover {
        color: #ff4500;
    }
</style>
<%--<fieldset style="border: 1px dotted #ff4500; padding: 5px;">
    <legend style="text-align: center; background-color: #ff4500; color: #f5fffa;padding: 5px">微博统计</legend>--%>
    <div id="acounts" runat="server">
        <ul class="activiesUl">
            <li><span style="margin-right: 20px">所有微博：10</span>本周新增：6</li>
            <li><span style="margin-right: 20px">所有博客：9 </span>本周新增：5</li>
            <li><span style="margin-right: 20px">所有文档：10</span>本周新增：4</li>
            <li><span style="margin-right: 20px">所有主题：6 </span>本周新增：3</li>
            <li><span style="margin-right: 20px">所有wiki：2 </span>本周新增：2</li>
        </ul>
    </div>
<%--</fieldset>--%>
