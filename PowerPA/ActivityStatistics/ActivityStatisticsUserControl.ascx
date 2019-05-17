<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivityStatisticsUserControl.ascx.cs" Inherits="PowerPA.ActivityStatistics.ActivityStatisticsUserControl" %>
<style type="text/css">
      .dt
        {
            width:80px;
        }
</style>
<div id="AppraiseDiv" runat="server">
    <table style="font-size: 14px; color: #494b4c;margin-right:10px;" cellspacing="0" cellpadding="3" runat="server" id="tbSettings">
        <tr><td>
            <table id ="tbDate" runat ="server" border ="0"><tr>
                <td>开始日期：</td>
                <td><SharePoint:DateTimeControl ID="dtStart" runat="server" DateOnly="True"  AutoPostBack="true" OnDateChanged="dtStart_DateChanged"  CssClassTextBox="dt" /></td>
                <td>结束日期：</td>
                <td><SharePoint:DateTimeControl ID="dtEnd" runat="server" DateOnly="True" AutoPostBack="true" OnDateChanged="dtStart_DateChanged" CssClassTextBox="dt" /></td>
                <td>操作：</td>
                <td><asp:DropDownList ID="ddlActions" runat="server" Width="192px" AutoPostBack="true" OnSelectedIndexChanged="ddlActions_SelectedIndexChanged" ></asp:DropDownList></td>
             </tr></table>
        </td></tr>
        <tr><td>
        <asp:GridView ID="gvShowData" runat="server" CellPadding="5" cellspacing="5" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" >
            <EmptyDataTemplate>
                <asp:Label ID="lblEmpty" ForeColor="Red" runat="server" Text="无统计数据！"></asp:Label>
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="ActionID" HeaderText="操作" ReadOnly="true" />
                <asp:BoundField DataField="DuringMax" HeaderText="最大时长" ReadOnly="true" />
                <asp:BoundField DataField="DuringMin" HeaderText="最小时长" ReadOnly="true" />
                <asp:BoundField DataField="DuringAvg" HeaderText="平均时长" ReadOnly="true" />
                <asp:BoundField DataField="QuantityMax" HeaderText="最大数量" ReadOnly="true" />
                <asp:BoundField DataField="QuantityMin" HeaderText="最小数量" ReadOnly="true" />
                <asp:BoundField DataField="QuantityAvg" HeaderText="平均数量" ReadOnly="true" />
                <asp:BoundField DataField="Attachments" HeaderText="附件个数" ReadOnly="true" Visible ="false"  />
            </Columns>
            <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" Height="10px" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
        </td></tr>
        <tr><td>

            </td></tr>
        <tr><td>
            <asp:Label ID="lblMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
        </td></tr>
    </table>
</div>
