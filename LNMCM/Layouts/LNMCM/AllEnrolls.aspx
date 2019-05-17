<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllEnrolls.aspx.cs" Inherits="LNMCM.Layouts.LNMCM.AllEnrolls" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
     <style type="text/css">
         .h3css{ font-family:微软雅黑; color:#4141a4; text-decoration:underline}
       .tdone{text-align:right; padding-right:10px; color:#4a4a4a; width:120px;vertical-align:top;}
         </style>
    <script  type="text/javascript">
        //window.onload=function()
        <%--{
        var tb = document.getElementById('<%=gvMembers.ClientID%>');
        for(var i=0;i<tb.rows.length;i++)//循环Table每一行
        {
            if(tb.rows[i].cells.length<=1)
            {
                 break; //防止分页出现tb.rows[i].cells[1]为空情况
            }
            var clonetd = tb.rows[i].cells[1].cloneNode(true); //克隆第二列（模板列）,
            var newtd = tb.rows[i].insertCell();//插入一个新列
            newtd.replaceNode(clonetd);//把新列用克隆的第二列置换掉
            tb.rows[i].cells[1].style.display = "none";//隐藏第二列,

            clonetd = tb.rows[i].cells[0].cloneNode(true);
            newtd = tb.rows[i].insertCell();//插入一个新列
            newtd.replaceNode(clonetd);//把新列用克隆的第二列置换掉
            tb.rows[i].cells[0].style.display = "none";
            }
        }--%>
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table style="font-size: 14px; color: #494b4c" cellspacing="0" cellpadding="3">
        <tr>
            <td class="tdone">培养单位：</td>
            <td class="tdone">

                <asp:DropDownList ID="ddlSchool" runat="server" AutoPostBack="false" CssClass="txtdoenlist"></asp:DropDownList>
            </td>
            <td style="text-align: left">
                <asp:Button ID="btnQuery" runat="server" Text="查 询" BorderStyle="None" Style="background-color: #88C4FF; cursor: pointer;" />
                &nbsp; &nbsp;
                <asp:Button ID="btnImport" runat="server" Text="导出全部" BorderStyle="None" Style="background-color: #88C4FF; cursor: pointer;" />
                <span runat="server" id="divSaveAs" style="text-align: center; padding-left: 20px; color: red"></span>
                <div style="padding: 10px 20px 10px 10px; float: right;">
                    <asp:Label ID="lblMsg" ForeColor="Red" runat="server" Text="" Font-Size="14"></asp:Label>
                    <asp:HiddenField ID="HiddenField1" Value="Ccc2008neu" runat="server" />
                </div>
            </td>
        </tr>

        <tr>
            <td colspan="3">
                <div style="width: 1250px; overflow-x: auto; overflow-y: hidden;padding:10px;">
                    <asp:GridView ID="gvMembers" runat="server" AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="报名序号" OnRowCommand="gvMembers_RowCommand" AllowPaging="True" CellSpacing="1" CellPadding="4" ForeColor="#333333" GridLines="None" PagerSettings-Mode="NumericFirstLast" PagerSettings-FirstPageText="首页 " PagerSettings-NextPageText="下一页" PagerSettings-PreviousPageText="上一页 " PageSize="20" PagerSettings-LastPageText="末页" Width="1800px">
                        <EmptyDataTemplate>
                            <asp:Label ID="Label1" ForeColor="Red" runat="server" Text="该培养单位尚无报名信息！"></asp:Label>
                        </EmptyDataTemplate>
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>

                            <asp:BoundField DataField="培养单位" HeaderText="培养单位" ReadOnly="true" />
                            <asp:BoundField DataField="报名序号" HeaderText="报名序号" ReadOnly="true" />
                            <asp:BoundField DataField="密码" HeaderText="密码" ReadOnly="true" ItemStyle-ForeColor="Gray"/>
                            <asp:BoundField DataField="邮箱" HeaderText="邮箱" ReadOnly="true" />
                            <asp:BoundField DataField="报名时间" HeaderText="报名时间" ReadOnly="true" />

                            <asp:BoundField DataField="队员1姓名" HeaderText="队员1姓名" ReadOnly="true"  ItemStyle-ForeColor="blue"/>
                            <asp:BoundField DataField="队员1学号" HeaderText="队员1学号" ReadOnly="true"  ItemStyle-ForeColor="blue"/>
                            <asp:BoundField DataField="队员1电话" HeaderText="队员1电话" ReadOnly="true"  ItemStyle-ForeColor="blue"/>

                            <asp:BoundField DataField="队员2姓名" HeaderText="队员2姓名" ReadOnly="true"  ItemStyle-ForeColor="darkgreen"/>
                            <asp:BoundField DataField="队员2学号" HeaderText="队员2学号" ReadOnly="true"  ItemStyle-ForeColor="darkgreen"/>
                            <asp:BoundField DataField="队员2电话" HeaderText="队员2电话" ReadOnly="true"  ItemStyle-ForeColor="darkgreen"/>

                            <asp:BoundField DataField="队员3姓名" HeaderText="队员3姓名" ReadOnly="true"  ItemStyle-ForeColor="darkorange"/>
                            <asp:BoundField DataField="队员3学号" HeaderText="队员3学号" ReadOnly="true"  ItemStyle-ForeColor="darkorange"/>
                            <asp:BoundField DataField="队员3电话" HeaderText="队员3电话" ReadOnly="true"  ItemStyle-ForeColor="darkorange"/>
                            <asp:HyperLinkField DataNavigateUrlFields="报名序号" DataNavigateUrlFormatString="MyEnroll.aspx?Code={0}&amp;Source=/LNMCM/_layouts/15/lnmcm/AllEnrolls.aspx" HeaderText="查看报名" Text="查 看" ItemStyle-HorizontalAlign="center" />
                            <asp:TemplateField HeaderText="删除报名" ItemStyle-HorizontalAlign="center" ItemStyle-ForeColor="red">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbdelete" runat="server" CommandName="Del" Style="font-size: 12px;" CommandArgument='<%#Eval("报名序号")%>' OnClientClick="return confirm('确定删除该报名吗？删除后将不可恢复！');">删 除</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" Height="30px" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />

                    </asp:GridView>
                </div>
            </td>
        </tr>

        <tr>
            <td colspan="3"></td>
        </tr>

    </table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
报名汇总
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
报名汇总
</asp:Content>
