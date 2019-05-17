<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanManagerUserControl.ascx.cs" Inherits="VSProject.PlanManager.PlanManagerUserControl" %>
<asp:UpdatePanel ID="up1" runat="server" ChildrenAsTriggers="True">
    <ContentTemplate>
        <div id="divhead">
            <style type="text/css">
                .addBtn {
                    font-weight: bold;
                    border: none;
                    background-color: #6699FF;
                    padding: 5px;
                    font-size: 2em;
                    color: #fff;
                    border-radius: 100%;
                    width: 60px;
                    height: 60px;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    cursor: pointer;
                    cursor: hand;
                    margin-top: 5px;
                    outline: none;
                    user-select: none;
                }

                .myform {
                    padding: 5px;
                }

                    .myform tr {
                        line-height: 30px;
                    }

                        .myform tr th {
                            text-align: right;
                            width: 120px;
                        }

                        .myform tr td {
                            text-align: left;
                        }

                            .myform tr td text {
                                width: 280px;
                            }
            </style>
            <script type="text/javascript">
                var prevselitem = null;
                function selectx(row) {
                    if (prevselitem != null) {
                        prevselitem.style.backgroundColor = '#ffffff';
                    }
                    row.style.backgroundColor = '#CDE6F7';
                    prevselitem = row;

                }

            </script>
            <script type="text/javascript" src="../../../../_layouts/15/VSProject/js/jquery-3.1.1.min.js"></script>
            <script type="text/javascript" src="../../../../_layouts/15/VSProject/js/jquery-ui.min.js"></script>
            <link href="../../../../_layouts/15/VSProject/css/jquery-ui.min.css" rel="stylesheet" />
            <script type="text/javascript" src="../../../../_layouts/15/VSProject/js/jquery-ui-timepicker-addon.min.js"></script>
            <script type="text/javascript" src="../../../../_layouts/15/VSProject/js/jquery-ui-timepicker-zh-CN.js"></script>
            <link href="../../../../_layouts/15/VSProject/css/jquery-ui-timepicker-addon.min.css" rel="stylesheet" />
            <script type="text/javascript">
                (function ($) {
                    $(function () {
                        $.datepicker.regional['zh-CN'] = {
                            changeMonth: true,
                            changeYear: true,
                            clearText: '清除',
                            clearStatus: '清除已选日期',
                            closeText: '关闭',
                            closeStatus: '不改变当前选择',
                            prevText: '<上月',
                            prevStatus: '显示上月',
                            prevBigText: '<<',
                            prevBigStatus: '显示上一年',
                            nextText: '下月>',
                            nextStatus: '显示下月',
                            nextBigText: '>>',
                            nextBigStatus: '显示下一年',
                            currentText: '今天',
                            currentStatus: '显示本月',
                            monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                            monthNamesShort: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
                            monthStatus: '选择月份',
                            yearStatus: '选择年份',
                            weekHeader: '周',
                            weekStatus: '年内周次',
                            dayNames: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
                            dayNamesShort: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
                            dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
                            dayStatus: '设置 DD 为一周起始',
                            dateStatus: '选择 m月 d日, DD',
                            dateFormat: 'yy-mm-dd',
                            firstDay: 1,
                            initStatus: '请选择日期',
                            isRTL: false
                        };

                    });

                    $(function () {
                        $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
                        $("#<%=tbStart.ClientID %>").datepicker({
                changeMonth: true,
                dateFormat: "yy-mm-dd",
            });
            $("#<%=tbDue.ClientID %>").datepicker({
                changeMonth: true,
                dateFormat: "yy-mm-dd",
            });
        });
    }(jQuery));
            </script>
        </div>
        <asp:Label ID="lbPTitle" runat="server" Font-Size="16" Text="计划列表"></asp:Label><hr />
        <table>
            <tr>
                <td>
                    <div id="divList" runat="server">
                        <div style="padding: 5px;">
                            <asp:Button ID="btnAddSingle" runat="server" Text="添加单操作计划" ToolTip="仅有一个操作来执行该计划" />&nbsp;&nbsp;
        <asp:Button ID="btnAddMulti" runat="server" Text="添加多操作计划" ToolTip="该计划需要一个或多个操作来完成" />
                        </div>
                        <asp:GridView ID="gvPlans" runat="server" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" BorderColor="#CCCCCC" BorderStyle="None" GridLines="Horizontal" DataKeyNames="ID">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="&nbsp;计 划&nbsp;">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdfPlanID" runat="server" Value='<%#Eval("ID")%>' />
                                        <asp:HiddenField ID="hdfActionID" runat="server" Value='<%#Eval("ActionID")%>' />
                                        <asp:HiddenField ID="hdfParentID" runat="server" Value='<%#Eval("ParentID")%>' />
                                        <asp:Label runat="server" Text='<%#Eval("Title")%>' ID="lbTitle"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="&nbsp;开始日期&nbsp;">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("StartDate","{0:d}")%>' ID="lbStart"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="&nbsp;结束日期&nbsp;">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("DueDate","{0:d}")%>' ID="lbDue"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="&nbsp;时长目标&nbsp;">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("During")%>' ID="lbDuring"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="&nbsp;数量目标&nbsp;">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Quantity")%>' ID="lbQuantity"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="&nbsp;操作比重&nbsp;">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Percent")%>' ID="lbPercent"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="&nbsp;描述&nbsp;">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Description")%>' ID="lbDescription"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="&nbsp;管 理&nbsp;">
                                    <ItemTemplate>
                                        <div style="padding: 5px;">
                                            <asp:LinkButton ID="lnkAdd" runat="server" Font-Underline="false" CommandArgument='<%#Eval("ID") %>' CommandName="AddPlan" Visible="false">
                                <img src="../../../../_layouts/15/VSProject/images/AddT.png" width="20" height="20" alt="" title="添加子计划"/>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnkEdit" runat="server" Font-Underline="false" CommandArgument='<%#Eval("ID") %>' CommandName="EditPlan">
                                 <img src="../../../../_layouts/15/VSProject/images/Edit.png" width="20" height="20" alt="" title="修改计划"/>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lnkDel" runat="server" Font-Underline="false" CommandArgument='<%#Eval("ID") %>' CommandName="DelPlan" OnClientClick="return confirm('确定要删除该计划吗？')" ToolTip="删除计划">
                                 <img src="../../../../_layouts/15/VSProject/images/Del.png" width="20" height="20" alt="" title="删除计划"/>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <EmptyDataTemplate>
                                <asp:Label ID="lbEmptPlan" runat="server" Text="你尚未设置任何目标计划，请点击对应按钮添加" ForeColor="red"></asp:Label>
                            </EmptyDataTemplate>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" Height="28" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
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
                <td>
                    <div id="divPlan" runat="server" visible="false">

                        <table cellspacing="2" cellspadding="2" class="myform">
                            <tr>
                                <th>
                                    <asp:Label ID="lbTitle" runat="server" Text="计划："></asp:Label>
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddlPlanAction" runat="server" Width="220px" AutoPostBack="true"></asp:DropDownList>
                                    <span style="color: red; font-size: 12px;">*(必选)</span>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label ID="lbPlanActions" runat="server" Text="可选操作："></asp:Label>
                                </th>
                                <td>
                                    <asp:CheckBoxList ID="cblPlanActions" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label ID="lbStart" runat="server" Text="开始日期："></asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="tbStart" runat="server" Width="210px" ToolTip="请选择计划的开始日期"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label ID="lbDue" runat="server" Text="截止日期："></asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="tbDue" runat="server" Width="210px" ToolTip="请选择计划的截止日期，必须大于开始日期"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th style="vertical-align: top;">
                                    <asp:Label ID="lbDuring" runat="server" Text="目标值："></asp:Label>
                                    <br />

                                </th>
                                <td>

                                    <table>
                                        <tr>
                                            <td style="text-align: right;">时长&nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbDuring" runat="server" Width="185px" TextMode="Number" ToolTip="设置计划的目标时长，单位：分钟"></asp:TextBox>
                                                <%--  <asp:CompareValidator ID="cvDuring" runat="server" ErrorMessage="时长必须是整数,且必须大于0" ForeColor="red" Display="Dynamic" Type="Integer" Operator="GreaterThan" ValueToCompare="0" Font-Size="Small"></asp:CompareValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">数量&nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tbQuantity" runat="server" Width="185px" TextMode="Number" ToolTip="设置计划的目标数量，可以是整数或小数，可以为负值"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lbvalidforGoal" runat="server" Text="计划目标中时长和数量至少要填写一项" Font-Bold="false" ForeColor="red" Font-Size="10"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <th style="vertical-align: top;">
                                    <asp:Label ID="lbDesc" runat="server" Text="描述："></asp:Label>
                                </th>
                                <td>
                                    <asp:TextBox ID="tbDesc" runat="server" TextMode="MultiLine" Rows="3" Width="205px" ToolTip="请填写计划的简要描述！"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <div style="padding: 10px;">
                                        <%--<asp:Button ID="btnSetAction" runat="server" Text="设置计划操作" />&nbsp;&nbsp;--%>
                               &nbsp;&nbsp;
                                        <asp:Button ID="btnSubmit" runat="server" Text="保 存" />&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="取 消" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div>
                                        <asp:HiddenField ID="hdfID" runat="server" Value="" />
                                        <asp:HiddenField ID="hdfParentID" runat="server" Value="" />
                                        <asp:HiddenField ID="hdfPlanType" runat="server" Value="" />
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divAction" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvActions" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField FooterText="计划操作" HeaderText="计划操作">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfPlanID" runat="server" Value='<%#Eval("PlanID")%>' />
                                                    <asp:HiddenField ID="hdfActionID" runat="server" Value='<%#Eval("ActionID")%>' />
                                                    <asp:Label ID="lbAction" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField FooterText="时长目标" HeaderText="时长目标">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tbDuring" runat="server" Text='<%#Eval("During")%>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField FooterText="数量目标" HeaderText="数量目标">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tbQuantity" runat="server" Text='<%#Eval("Quantity")%>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField FooterText="计划比重" HeaderText="计划比重">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tbPercent" runat="server" Text='<%#Eval("Percent")%>' TextMode="Range"></asp:TextBox>
                                                    <asp:Label ID="lbPercent" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lbEmpty" runat="server" Text="该计划尚未选择任何操作！" ForeColor="red"></asp:Label>
                                        </EmptyDataTemplate>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">

                                    <%--<asp:Button ID="btnSaveToPlan" runat="server" Text="保存并返回本计划" />&nbsp;&nbsp;--%>
                                    <asp:Button ID="btnSaveToList" runat="server" Text="保 存" />&nbsp;&nbsp;
        <asp:Button ID="btnCancelToList" runat="server" Text="取 消" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <asp:Label ID="lbErr" runat="server" Text="" ForeColor="red"></asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>
