<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoalSettingUserControl.ascx.cs" Inherits="VAKPI.GoalSetting.GoalSettingUserControl" %>
 <table style="font-size: 14px; color: #494b4c" cellspacing="0" cellpadding="3" runat ="server" id="tbSettings">
        <tr><td colspan="2">
<asp:GridView ID="gvGoalSetting" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="558px" AutoGenerateColumns="False" DataKeyNames="ID"  >
    <EmptyDataTemplate>
        <asp:Label ID="Label1" ForeColor="Red" runat="server" Text="没有可用的设置值！"></asp:Label>
    </EmptyDataTemplate>
    <AlternatingRowStyle BackColor="White" />
    <Columns>
    <asp:BoundField DataField="ActivityType" HeaderText="活动类型" ReadOnly="true" />
    <asp:BoundField DataField="Periods" HeaderText="周期" ReadOnly="true" />
    <asp:BoundField DataField="During" HeaderText="默认目标" ReadOnly="true" />
    <asp:TemplateField  HeaderText="我的目标" ItemStyle-HorizontalAlign="center"><ItemTemplate>
        <asp:TextBox ID="txtMyDuring" Text='<%#Eval("MyDuring")%>' runat="server"></asp:TextBox>
      </ItemTemplate>
    </asp:TemplateField>
        <asp:TemplateField HeaderText="MyDuring" Visible="False">
        <ItemTemplate>
        <asp:Label ID="lblMyDuring" runat="server" Text='<%# Eval("MyDuring") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="StartDate" HeaderText="启用日期" ReadOnly="True" DataFormatString="{0:yyyy-MM-dd}" />
    </Columns>
    <EditRowStyle BackColor="#2461BF" />
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#EFF3FB" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#F5F7FB" />
    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
    <SortedDescendingCellStyle BackColor="#E9EBEF" />
    <SortedDescendingHeaderStyle BackColor="#4870BE" />
</asp:GridView>
   </td></tr>
     <tr><td><asp:Label ID="lbDes"  ForeColor="#0072c6" runat="server" Text=""></asp:Label></td><td style ="text-align:right">
         <asp:Button ID="btnSave" runat="server"   Text="保 存" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server"   Text="取 消" /></td></tr>
<tr><td colspan="2">
<asp:Label ID="lbErr" runat="server" Text="" ForeColor="Red"></asp:Label>
 </td></tr>
 </table>