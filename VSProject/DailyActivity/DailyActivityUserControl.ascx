<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DailyActivityUserControl.ascx.cs" Inherits="VSProject.DailyActivity.DailyActivityUserControl" %>

<style type="text/css">
       .mytable{
        word-wrap:break-word;
        width:100%;
        border:none;
        border-collapse: collapse;
        }
        .mytable tr:nth-child(odd){
        background-color:#eee;
        border:1px solid #eee;
        }
        .mytable tr:hover{
        background-color:#bbb;
        color:#fff;
        }
        .mytable tr td{
        max-width:360px;
        word-wrap:break-word;
        min-width:50px;
        border:0px;
        padding:2px 5px 2px 5px;
        }
         .mytable tr td:last-child{
        text-align:left;
        border-right:0px;
        }
         .spanDesc{
             font-size:12px;
             color: #0072c6;
         }
      .widStyle
      {
          width:180px;
      }

      .menuBtn{
          list-style:none;
          margin: 0px;
          padding: 0px;
          width: auto;
      }
      .menuBtn li{
          float:right;
          padding-right:10px;
      }

      .menuBtn li button {
          display: inline-block;
          border: none;
          padding: 10px 20px;
          width: 70px;
          margin-bottom: 10px;
          cursor: pointer;
          background-color: #fff;
          border: 1px solid #dcdfe6;
          border-radius: 4px;
          appearance: none;
          outline: none;
        }

        .menuBtn li button:hover {
          color: #409eff;
          border-color: #c6e2ff;
          background-color: #ecf5ff;
        }
     .auto-style1 {
        height: 36px;
    }
     </style>
<script type="text/javascript">
    function isnull(obj) {
        outtips();
        var val = obj.value;
        var str = val.replace(/(^\s*)|(\s*$)/g, '');//去除空格;
        if (str == '' || str == undefined || str == null) {
            //obj.focus();
            //tips(obj.id, "活动说明必须填写，不可为空");
            //return;
        }
        else {
            outtips();
        }
    }
    function isDuring(obj) {
        outtips();
        var val = obj.value;
        var str = val.replace(/(^\s*)|(\s*$)/g, ''); //去除空格;
        var reg = /^\+?[1-9][0-9]*$/;
        if (str == '' || str == undefined || str == null) {
            outtips();
        }
        else {
            if (!str.match(reg)||str>1440) {
                obj.focus();
                tips(obj.id, "时长必须为数字，且不得大于1440，单位：分钟。");
            }
            else {
                outtips();
            }
        }

    }
    function isAmount(obj) {
        outtips();
        var val = obj.value;
        var str = val.replace(/(^\s*)|(\s*$)/g, ''); //去除空格;

        if (str == '' || str == undefined || str == null) {//可以不填项允许为空
            outtips();
        }
        else {
            var reg = /^(-?\d+)(\.\d+)?$/;
            if (!str.match(reg)) {
                obj.focus();
                tips(obj.id, "数量必须为整数或小数，支出可以为负数");
                //return;
            }
            else {
                outtips();
            }
        }

    }

    function istime(obj) {
        outtips();
        var val = obj.value;
        var str = val.replace(/(^\s*)|(\s*$)/g, ''); //去除空格;
        if (str == '' || str == undefined || str == null) {//可以不填项允许为空
            outtips();
        }
        else {
            var reg = /^(([0-1]?\d)|(2[0-4])):[0-5]?\d$/;
            if (!str.match(reg)||str>1440) {
                obj.focus();
                tips(obj.id, "你输入的时间格式不正确，请输入24小时制时间格式 13:00");
                //return;
            }
            else {
                outtips();
            }
        }
    }
    function tips(id, str) {
        var obj = document.getElementById(id);
        t= getTop(obj)+obj.offsetHeight;
        l = getLeft(obj);
        var tipsdiv = document.getElementById("tips");
        tipsdiv.innerHTML="<span style='font-weight:bold;'>特别提示</span><br/><span>"+str+"</span>";
        tipsdiv.style.left=l+"px";
        tipsdiv.style.top=t+"px";
        //tipsdiv.style.display = "";
    }

    /*移除说明性文字,并隐藏提醒*/
    function outtips(){
        var tipsdiv = document.getElementById("tips");
        tipsdiv.style.display = 'none';
        tipsdiv.innerHTML = "";
    }

    //获取元素的纵坐标
    function getTop(e){
        var offset=e.offsetTop;
        if(e.offsetParent!=null) offset+=getTop(e.offsetParent);
        return offset;
    }

    //获取元素的横坐标
    function getLeft(e){
        var offset=e.offsetLeft;
        if(e.offsetParent!=null) offset+=getLeft(e.offsetParent);
        return offset;
    }
</script>
<script  type="text/javascript" src="../../../../_layouts/15/VSProject/js/gv.js"></script>
<script  type="text/javascript" src="../../../../_layouts/15/VSProject/js/AddAttachRel.js"></script>
<script  type="text/javascript">
    function cacul()
    {
        var table = document.getElementById('<%=gvActivities.ClientID%>');
        var txtTitle=cacul1(table);
        document.getElementById('<%=spShowInfo.ClientID %>').innerText = txtTitle;
    }
    function GetIds()
    {
        var ids = GetItemsID();
        document.getElementById('<%=txtIds.ClientID %>').value = ids;
    }
    function chkAll(obj) {
        var chkList = $get('<%=gvActivities.ClientID%>').getElementsByTagName("input");
        for (var i = 0; i < chkList.length; i++) {
            if (chkList[i].type == 'checkbox' && chkList[i].name.lastIndexOf('cbSel') == chkList[i].name.length - 5)
                chkList[i].checked = obj.checked;
        }
        cacul();
    }
</script>
<asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="AppraiseDiv" runat="server">
    <table style="font-size: 14px; color: #494b4c; margin-right: 10px;" cellspacing="0" cellpadding="3" runat="server" id="tbSettings">
        <tr runat="server" id="trTitle">
            <td style="width: 40px;">日期:</td>
            <td style="width: 40px;"><asp:Button ID="btnPre" CausesValidation="false" runat="server" Text="前一日" /></td>
            <td style="width: 140px;"><SharePoint:DateTimeControl ID="dtCurrentDate" runat="server" DateOnly="True" TabIndex="1" AutoPostBack="True" /></td>
            <td style="width: 40px;">
                <asp:Button ID="btnNext" runat="server" Text="后一日" /></td>
            <td style="width: 60px;"></td>
            <td><span style="color: #0072c6; padding-left: 10px;" id="spShowInfo" runat="server"></span></td>
        </tr>
        <tr>
            <td colspan="6">
                <table>
                    <tr>
                        <td>您去过哪里：
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblistLocal" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">家</asp:ListItem>
                                <asp:ListItem Value="2">办公室</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <div id="tips" style="position: absolute; border: 1px solid #ccc; padding: 0px 3px;
                    color: #f00; display: none; height: 20px; line-height: 20px; background: #fff;"></div>
                <table runat="server" id="tbQuick">
                    <tr>
                        <td>

                            <asp:GridView ID="gvActivities" runat="server" CellPadding="0" CellSpacing="0" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" DataKeyNames="ActivityID">
                                <EmptyDataTemplate>
                                    <asp:Label ID="Label1" ForeColor="Red" runat="server" Text="该日没有活动录入记录！"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>

                                    <asp:TemplateField HeaderText="操作" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlActions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="gv_ddlActions_SelectedIndexChanged" Width="75px" ToolTip="活动的操作"></asp:DropDownList>
                                        </ItemTemplate>
                                         <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="开始时间" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbStartTime" Text='<%#Eval("StartTime")%>' runat="server" Width="50px" ToolTip="活动开始时间，24小时制时间格式 13:00"  onblur="istime(this)"></asp:TextBox>
                                        </ItemTemplate>

                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="时长(分钟)" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbDuring" Text='<%#Eval("During")%>' runat="server" onchange="cacul()" Width="60px" ToolTip="活动时长，单位：分钟；不能超过1440" onblur="isDuring(this)"></asp:TextBox>
                                        </ItemTemplate>

                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="数量" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbQuantity" Text='<%#Eval("Quantity")%>' runat="server" Width="50px" ToolTip="产出或消耗的数量，仅填写数字" onblur="isAmount(this)"></asp:TextBox>
                                        </ItemTemplate>

                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="说明" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbDesc" Text='<%#Eval("Description")%>' runat="server" Width="250px" ToolTip="活动的简要描述,必须填写" onblur="isnull(this)"></asp:TextBox>
                                        </ItemTemplate>

                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="任务文档" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlTasks" runat="server" Width="180px" ToolTip="活动关联的任务文档"></asp:DropDownList>
                                        </ItemTemplate>

                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="是否正常" ItemStyle-HorizontalAlign="center" >
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbSelNormal" runat="server" Checked="true" ToolTip="本次活动是否与往常一样" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flag" ItemStyle-HorizontalAlign="center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFlag" runat="server" Text='<%#Eval("Flag")%>' Width="40"></asp:Label>
                                        </ItemTemplate>

                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ActionID" ItemStyle-HorizontalAlign="center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActionID" runat="server" Text='<%#Eval("ActionID")%>' Width="40"></asp:Label>
                                        </ItemTemplate>

                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="TaskID" ItemStyle-HorizontalAlign="center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaskID" runat="server" Text='<%#Eval("TaskID")%>' Width="40"></asp:Label>
                                        </ItemTemplate>

                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                </Columns>
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#E6E6E6" Font-Bold="False" ForeColor="#00CC66" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" Height="10px" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table runat="server" style="margin-right: 10px; border: none; line-height: 30px;" border="0" cellspacing="0" cellpadding="3" id="tbSingle">
                    <tr>
                        <td colspan="3" class="auto-style1"></td>
                    </tr>
                    <tr>
                        <td>操作
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlActions" runat="server" Width="192px" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblTypeShow" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>时长

                        </td>
                        <td>
                            <asp:TextBox ID="txtHours" Width="180px" runat="server"></asp:TextBox>

                        </td>
                        <td>
                            <span class="spanDesc" runat="server" id="spanHours">活动的时长度量，单位：分钟</span>

                        </td>
                    </tr>
                    <tr>
                        <td>数量

                        </td>
                        <td>
                            <asp:TextBox ID="txtQuantity" Width="180px" runat="server"></asp:TextBox>

                        </td>
                        <td>
                            <span class="spanDesc" runat="server" id="spanQuantity">活动的数量计量，以操作设置的度量单位和度量方式为准，仅填写数字即可</span>
                        </td>
                    </tr>
                    <tr>
                        <td>说明</td>
                        <td>
                            <asp:TextBox ID="txtDesc" Width="180px" runat="server"></asp:TextBox></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>是否正常</td>
                        <td style="text-align: center">
                            <asp:CheckBox ID="cbNormal" runat="server" Checked="true" /></td>
                        <td><span class="spanDesc" runat="server" id="spanNormal">标记活动是否与常规一致，或有其他特殊情况</span></td>
                    </tr>
                    <tr>
                        <td>任务文档</td>
                        <td>
                            <asp:DropDownList ID="ddlTypes" runat="server" Width="192px" ToolTip=""></asp:DropDownList></td>
                        <td><span class="spanDesc" runat="server" id="spanTasks">活动关联的任务</span></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div runat="server" id="divDocs" style="height: 0px; overflow-x: hidden; overflow-y: auto">
                                <asp:Table ID="tbContent" CssClass="mytable" runat="server"></asp:Table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                 <span runat="server" id="spanUpdate">
                    <input type="button" onclick="AddAttach()" style="display: none" value="添加活动附件" title="活动附件包含：照片、短视频、录音等" />
                </span>
            </td>
        </tr>
        <tr>
            <td colspan="6" style="text-align: right;padding-right:10px;">
                <ul class="menuBtn">
                    <li style="float:left;">
                        <asp:Button ID="btnNew" runat="server" Text="添加更多活动" OnClick="btnNew_Click" />
                    </li>
                    <li>
                        <asp:Button ID="btn7Days"  runat="server" Text="七日量化分析" Enabled="false"/>
                    </li>
                    <li>
                        <asp:Button ID="btnCancel" runat="server" Text="取 消"  ForeColor="red"/>
                    </li>
                    <li>
                        <asp:Button ID="btnSave" OnClientClick="GetIds()" runat="server" Text="保 存"  ForeColor="green"/>
                    </li>
                </ul>
            </td>
        </tr>

        <tr>
            <td colspan="6">
                <div style="color: #0072c6; padding-left: 75px;" id="udesp" runat="server"><%=webObj.UserDesp %></div>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:Label ID="lblMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                <asp:HiddenField runat="server" ID="txtIds" />
            </td>
        </tr>
    </table>
</div>
    </ContentTemplate>
</asp:UpdatePanel>

