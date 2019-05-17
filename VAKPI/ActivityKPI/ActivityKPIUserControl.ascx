<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<add tagPrefix="asp" namespace="System.Web.UI.DataVisualization.Charting" assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, 
PublicKeyToken=31bf3856ad364e35" />
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivityKPIUserControl.ascx.cs" Inherits="VAKPI.ActivityKPI.ActivityKPIUserControl" %>
<style type="text/css">
    .kpidiv{color:red;font-family:微软雅黑;padding:5px 5px 10px 20px;line-height:25px;}
    .kpidiv a:link {color:#0066CC;}    /* 未被访问的链接 */
    .kpidiv a:visited {color:#1c9f1f;} /* 已被访问的链接 */
    .kpidiv a:hover {color:#FF00FF;}   /* 鼠标指针移动到链接上 */
    .kpidiv a:active {color:#0000FF;} /* 正在被点击的链接 */
    /*.mytable tr:first-child{background:#0066CC; color:#fff;font-weight:bold;padding:10px;}*/ /*第一行标题蓝色背景*/
    .mytable th{color:#0066CC;font-weight:bold;padding:10px;border:1pt solid #d1e0fb;text-align:center;} /*第一行标题蓝色背景*/
    .mytable{border:1pt solid #d1e0fb;margin: 0 auto;width:100%;} 
    .mytable td{ padding:10px; border:1pt solid #d1e0fb;text-align:center;}
    .mytable a:link {color:#0066CC;}    /* 未被访问的链接 */
    .mytable a:visited {color:#0066CC;} /* 已被访问的链接 */
    .mytable a:hover {color:#FF00FF;}   /* 鼠标指针移动到链接上 */
    .mytable a:active {color:#FF0000;} /* 正在被点击的链接 */
    .mytable tr:nth-of-type(odd){border:1pt solid #d1e0fb; background:#F5FAFA;} /* odd 标识奇数行，even标识偶数行 */
    .mytable tr:hover{ background: #b6dede;border:1pt solid #d1e0fb;} /*鼠标悬停后表格背景颜色*/
</style>
<div id="kpiDiv" runat="server">

</div>
