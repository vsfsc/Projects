<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskManagerUserControl.ascx.cs" Inherits="VSProject.TaskManager.TaskManagerUserControl" %>
<style type="text/css">
    .addBtn{
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
    .myform{
        padding:5px;
    }
    .myform tr{
        line-height:30px;
    }
    .myform tr th{
        text-align:right;
        width:120px;
    }
    .myform tr td{
        text-align:left;
    }
    .myform tr td text {
        width:280px;
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

     function onValueChanged() {
            var percent = document.getElementById('<%= tbPercent.ClientID %>').value;
            document.getElementById("txtP").innerText = percent+"%";
        }

        function onLoad() {
            var percent = document.getElementById('<%= tbPercent.ClientID %>').value;
            document.getElementById("txtP").innerText = percent+"%";
        }
        window.onload = onLoad;
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
<div id="pageContent">
    <div id="divList" runat="server">
        <table>
            <tr style="line-height:25px;">
                <td style="padding-bottom:10px;font-weight:bold;font-size:14px;">
                    <asp:DropDownList ID="ddlProjects" runat="server" AutoPostBack="true" Width="200px" Height="25px" Font-Bold="true" ToolTip="切换项目名，筛选对应项目下的任务！">
                        <asp:ListItem Value="0">所有项目</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBoxList ID="cblProjects" runat="server" Visible="false" ToolTip="切换项目名，筛选对应项目下的任务！"></asp:CheckBoxList>
                    <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" ToolTip="切换项目名，筛选对应项目下的任务！"></asp:RadioButtonList>
               </td>
                <td style="text-align:right;">
                     &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkAddFoot" runat="server" Font-Underline="false">
                        <img src="../../../../_layouts/15/VSProject/images/AddP.png" width="20" height="20" alt="" title="添加项目"/>
                    </asp:LinkButton>

                </td>
            </tr>

        </table>
        <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False" BorderColor="#CCCCCC" BorderStyle="None" GridLines="Horizontal" DataKeyNames="ID">
            <Columns>
                <asp:TemplateField HeaderText="&nbsp;名 称&nbsp;">
                    <ItemTemplate>
                        <div style="padding: 5px;">
                            <asp:HiddenField ID="hdfTaskID" runat="server" Value='<%#Eval("ID")%>' />
                            <asp:HiddenField ID="hdfParentID" runat="server" Value='<%#Eval("ParentID")%>' />
                            <asp:Label ID="lbTitle" runat="server" Text='<%#Eval("Title")%>' ToolTip="任务名称" ></asp:Label>
                        </div>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="&nbsp;起止日期&nbsp;">
                    <ItemTemplate>
                        <div style="padding: 5px;">
                            <asp:Label ID="lbStart" runat="server" Text='<%#Eval("StartDate","{0:d}")%>' ToolTip="开始日期"></asp:Label>
                            ~
                            <asp:Label ID="lbDue" runat="server" Text='<%#Eval("DueDate","{0:d}")%>' ToolTip="截止日期"></asp:Label>
                        </div>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" ></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="&nbsp;状 态&nbsp;">
                    <ItemTemplate>
                        <div style="padding: 5px;">
                            <asp:Label ID="lbStatus" runat="server" ToolTip="任务状态"></asp:Label>
                            <asp:HiddenField ID="hdfStatus" runat="server" Value='<%#Eval("Status")%>'/>
                        </div>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="&nbsp;预估工作量&nbsp;">
                    <ItemTemplate>
                        <div style="padding: 5px;">
                            <asp:Label ID="lbDuring" runat="server" Text='<%#Eval("During")%>' ToolTip="预估工作量"></asp:Label>
                        </div>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />

                </asp:TemplateField>
                <asp:TemplateField HeaderText="&nbsp;完成百分比&nbsp;">
                    <ItemTemplate>
                        <div style="padding: 5px;">
                            <asp:Label ID="lbPercent" runat="server" Text='<%# Eval("PercentComplete") %>' ToolTip="完成百分比"></asp:Label>
                        </div>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />

                </asp:TemplateField>
                <asp:TemplateField HeaderText="&nbsp;描 述&nbsp;">
                    <ItemTemplate>
                        <div style="padding: 5px;">
                            <asp:Label ID="lbDescription" runat="server" Text='<%#Eval("Description")%>' ToolTip="任务描述"></asp:Label>
                        </div>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="&nbsp;文 档&nbsp;">
                    <ItemTemplate>
                        <div style="padding: 5px;">
                            <asp:HyperLink ID="lnkDoc" runat="server" ImageHeight="20px" ></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="&nbsp;级 别&nbsp;" Visible="False">
                    <ItemTemplate>
                        <div style="padding: 5px;">
                            <asp:HiddenField ID="hdfDeep" runat="server" Value='<%#Eval("Deep")%>' />
                        </div>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="&nbsp;操 作&nbsp;">
                    <ItemTemplate>
                        <div style="padding: 5px;">
                            <asp:LinkButton ID="lnkAdd" runat="server" Font-Underline="false" CommandArgument='<%#Eval("ID") %>' CommandName="AddTask">
                                <img src="../../../../_layouts/15/VSProject/images/AddT.png" width="20" height="20" alt="" title="添加子任务"/>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkEdit" runat="server" Font-Underline="false" CommandArgument='<%#Eval("ID") %>' CommandName="EditTask">
                                 <img src="../../../../_layouts/15/VSProject/images/Edit.png" width="20" height="20" alt="" title="编辑任务"/>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkDel" runat="server" Font-Underline="false" CommandArgument='<%#Eval("ID") %>' CommandName="DelTask" OnClientClick="return confirm('确定要删除该任务吗？')" ToolTip="删除任务">
                                 <img src="../../../../_layouts/15/VSProject/images/Del.png" width="20" height="20" alt="" title="删除任务"/>
                            </asp:LinkButton>
                        </div>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />

                </asp:TemplateField>
            </Columns>

            <EmptyDataTemplate>
                <asp:Label ID="lbEmpt" runat="server" Text="没有项目或任务！" ForeColor="red"></asp:Label>
            </EmptyDataTemplate>

            <FooterStyle BackColor="#CCCC99" ForeColor="Black" BorderColor="#CCCCCC"  />
            <HeaderStyle BackColor="Azure" Font-Size="13" HorizontalAlign="Center" />
            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F7F7F7" />
            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
            <SortedDescendingCellStyle BackColor="#E5E5E5" />
            <SortedDescendingHeaderStyle BackColor="#242121" />

        </asp:GridView>
    </div>

    <div id="divModify" runat="server" visible="false">
        <table cellspacing="2" cellspadding="2" class="myform">
            <tr>
                <td colspan="2">
                    <div>
                        <asp:Label ID="lbPTitle" runat="server" Font-Size="16" Text=""></asp:Label>
                        <asp:HiddenField ID="hdfPID" runat="server" Value="" />
                    </div>
                    <div style="padding-left:10px;border:solid 1px #D3D3D3;font-size:13px;background-color:#FFFFCC">
                        <asp:Label ID="lbParentTitle" runat="server" Visible="False" Text=""></asp:Label>
                    </div>
                    <hr />
                </td>
            </tr>
            <tr >
                <th>
                    <asp:Label ID="lbParent" runat="server" Text="父级任务："></asp:Label>
                </th>
                <td>
                    <asp:DropDownList ID="ddlParent" runat="server" Width="215px"></asp:DropDownList>
                </td>
            </tr>

            <tr>
                <th>
                    <asp:Label ID="lbTitle" runat="server" Text="任务名称："></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="tbTitle" runat="server" Width="204px" ToolTip="请填写任务或项目的名称，此项必填"> </asp:TextBox>
                    <span style="color: red;font-size:12px;">*(必填)</span>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="lbTitle_EN" runat="server" Text="英文名称："></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="tbTitle_EN" runat="server" Width="204px" ToolTip="英文名称必须填写，且英文名称必须字母开头，由英文、数字或者下划线组成，不能有汉字、空格等其他字符，长度1-50之间！"> </asp:TextBox>
                    <asp:Label ID="lbTitle_ENValid" runat="server" Text="*(项目必填)" ForeColor="red" Font-Size="10"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="lbStart" runat="server" Text="开始日期："></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="tbStart" runat="server" Width="210px" TextMode="Date" ToolTip="请选择任务或项目的开始日期"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="lbDue" runat="server" Text="截止日期："></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="tbDue" runat="server" Width="210px" TextMode="Date" ToolTip="请选择任务或项目的截止日期，必须大于开始日期"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="lbDuring" runat="server" Text="预估工作量："></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="tbDuring" runat="server" Width="210px" TextMode="Number" ToolTip="请填写任务或项目完成需要的工作时数（整数）单位：小时"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="lbPercent" runat="server" Text="完成百分比："></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="tbPercent" runat="server" Text="0" Width="170px" TextMode="Range" ToolTip="请填写或拖动选择任务或项目已完成的百分比：最小0，最大100；支持键盘左右方向键操作！" onchange="onValueChanged()"></asp:TextBox>
                    <span id="txtP"></span>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="lbStatus" runat="server" Text="任务状态："></asp:Label>
                </th>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" Width="215px" ToolTip="选择任务或项目执行的状态"></asp:DropDownList>
                </td>
            </tr>

            <tr>
                <th style="vertical-align:top;">
                    <asp:Label ID="lbDesc" runat="server" Text="任务描述："></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="tbDesc" runat="server" TextMode="MultiLine" Rows="3" Width="204px" ToolTip="请简要描述任务或项目的介绍！"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right; padding-right: 40px;">
                    <div>
                        <asp:Button ID="btnSubmit" runat="server" Text=" 保 存 " />&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text=" 取 消 " />&nbsp;&nbsp;&nbsp;&nbsp;
                    </div>
                    <asp:HiddenField ID="hdfID" runat="server" Value="" />
                </td>
            </tr>
        </table>
    </div>
</div>
<asp:Label ID="lbErr" runat="server" Text="" ForeColor="red"></asp:Label>