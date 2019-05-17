<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeLinePerDayUserControl.ascx.cs" Inherits="PowerPA.TimeLinePerDay.TimeLinePerDayUserControl" %>


<!--核心样式-->
<link rel="stylesheet" href="../../../../_layouts/15/PowerPA/css/an-skill-bar.css">
<%--<link rel="stylesheet" href="../../../../_layouts/15/PowerPA/css/main.css">--%>
<script src="../../../../_layouts/15/PowerPA/js/jquery-1.11.0.min.js" type="text/javascript"></script>
<script src="../../../../_layouts/15/PowerPA/js/an-skill-bar.js"></script>
<script src="../../../../_layouts/15/PowerPA/js/main.js"></script>


<meta charset="UTF-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<meta name="viewport" content="width=device-width, minimum-scale= 1.0, initial-scale= 1.0">
<div class="container" id="divContainter" runat="server">
    <div id="skill">
        <h3>任务进度</h3>
        <div class="skillbar html" title="总体工作量">
            <div class="filled" data-width="50%" title="已完成进度" style="width:50%"></div>
            <span class="title" title="任务名称">作品</span>
            <span class="percent" title="进度百分比">50%</span>
        </div>
        <div style="color:red;font-weight:bold;">该任务未按预期完成</div>
    </div>
</div>
