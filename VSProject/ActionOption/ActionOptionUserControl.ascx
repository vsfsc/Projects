<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActionOptionUserControl.ascx.cs" Inherits="VSProject.ActionOption.ActionOptionUserControl" %>
        <style type="text/css">
            .ssdiv {
                margin: 0 auto;
                width: 100%;
            }

                .ssdiv h2 {
                    font-weight: 600;
                    font-size: 18px;
                    margin-left: 10px;
                    padding-bottom: 5px;
                    color: rgba(25, 25, 25, 0.72);
                }

                .ssdiv div dl {
                    margin-left: 20px;
                    line-height: 25px;
                    color: #0094ff;
                    text-align: left;
                }

                .ssdiv table {
                    margin-left: 10px;
                    border: 1px solid #bebebe;
                }

            .mybtn {
                margin-left: 100px;
                cursor: pointer;
                text-align: center;
            }

                .mybtn:hover {
                    color: blue;
                }

            .gvlink {
                color: #000;
                text-decoration: none;
            }

                .gvlink:visited {
                    color: rgba(25, 25, 25, 0.72);
                }

                .gvlink:hover {
                    cursor: pointer;
                    color: red;
                }

            .nonegvlink {
                pointer-events: none;
                color: #000;
                cursor: default;
            }

        </style>
        <script type="text/javascript" src="../../../../_layouts/15/VSProject/js/ActionOption.js">
        </script>
<script type="text/javascript">
    function cacul()
{
    var gv_chkCount = 0;
    var gvCheck;
    var lbAction;
    var table = document.getElementById('<%=gvActions.ClientID%>'); //.getElementsByTagName("input");
    var tr = table.getElementsByTagName("tr");
    var pattem = /^\d+(\.\d+)?$/;
    for (i = 1; i < tr.length; i++) {
        gvCheck = tr[i].getElementsByTagName("td")[7].getElementsByTagName("input")[0];
        lbAction = tr[i].getElementsByTagName("td")[0].getElementsByTagName("input")[0];
        if (gvCheck.checked) {
            gv_chkCount = gv_chkCount + 1;
        }
    }
}
function selectDpList(ddl)
{
    var sIndex = ddl.selectedIndex; //返回选中是第几项 0,1....
    //var sText = ddl.options[sIndex].text; //返回选中的文本--文本1,文本2 ...
    var sValue = ddl.options[sIndex].value; //返回选中的值--v1,v2 ...
    ddl.style.background = getBackCorlor(sValue);
    changeIsMy(ddl);
}
function changeIsMy(thisctl)
{
    var str = thisctl.id;
    var index = str.lastIndexOf("_");
    str = str.substring(0, index) + "_hdfIsMy";
    var isMy = document.getElementById(str);
    if (isMy.value === "0")
    {
        isMy.value = "01";
    }
    else
    {
        isMy.value = "11";
    }
}
function getBackCorlor(flag)
{
    var style = "background-color:#F0F0F0";
    switch (flag)
    {
        case "6":
            style = "background-color:#FFCCCC";
            break;
        case "5":
            style = "background-color: #FFE5CC";
            break;
        case "4":
            style = "background-color:#FFFFCC";
            break;
        case "3":
            style = "background-color:#CCE5FF";
            break;
        case "2":
            style = "background-color:#CCFFCC";
            break;
        case "1":
            style = "background-color:#F0F0F0";
            break;
        default:
            style = "background-color:#F0F0F0";
            break;
    }
    return style;
}
function IsFormChanged()
{
    var isChanged = false;
    var form = document.forms[0];
    for (var i = 0; i < form.elements.length; i++)
    {
        var element = form.elements[i];
        var type = element.type;
        if (type === "text" || type === "hidden" || type === "textarea" || type === "button")
        {
            if (element.value !== element.defaultValue)
            {
                isChanged = true;
                break;
            }
        }
        else if (type === "radio" || type === "checkbox")
        {
            if (element.checked !== element.defaultChecked)
            {
                isChanged = true;
                break;
            }
        }
        else if (type === "select-one" || type === "select-multiple")
        {
            for (var j = 0; j < element.options.length; j++)
            {
                if (element.options[j].selected!==element.options[j].defaultSelected)
                {
                    isChanged = true;
                    break;
                }
            }
        }
    }
    return isChanged;
}
function close_sure() {
    var ischanged = IsFormChanged();
    if (ischanged) {
        var gnl = confirm("您设置的内容尚未保存，确认保存并关闭页面吗?若不保存，请点击取消！");
        if (gnl === true) {
            <%--//return true;
            var a = " <%=SaveGVToDB()%>";
            console.log(a);--%>
            window.location.href = getbackUrl();
        }
        else {
            window.location.href = getbackUrl();
        }
    }
    else {
        window.location.href = getbackUrl();
    }
}


function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r !== null) return unescape(r[2]);
    return null;
}
function getbackUrl() {
    if (getQueryString("Source") !== null) {
        return getQueryString("Source");
    }
    else {
        var clientContext = SP.ClientContext.get_current();
        return clientContext.get_url();
    }
}
    function init()
            {
                var bodyGridView=document.getElementById("<%=gvActions.ClientID%>");
                var headGridView=bodyGridView.cloneNode(true);
                for(i=headGridView.rows.length-1;i>0;i--)
                headGridView.deleteRow(i);
                bodyGridView.deleteRow(0);//删掉数据行
                headdiv.appendChild(headGridView);//删掉表头行
            }
            window.onload=init;
</script>
<div id="actionSetting" class="ssdiv" runat="server">
    <div id="divBtn" style="width: 1025px; text-align: right; margin-bottom: 10px; font-size: 14px; color: #0094ff; left: 10px;" runat="server">
        <table border="0" style="width: 100%">
            <tr style="vertical-align: bottom; line-height: 25px; background-color: #f7f7de">
                <td style="height: 18px; text-align: left; vertical-align: bottom;">

                    <div id="divdesc" runat="server">
                        <dl>
                            <dt>1. 表格中，除操作外均可自定义填写内容或设置频度和可能时间段；</dt>
                            <dt>2. 每个操作设置完成后，请确保行尾已勾选，以备保存本次设置。</dt>
                        </dl>
                    </div>
                </td>
                <td style="height: 18px; text-align: right; vertical-align: bottom;">
                    <asp:Button ID="btnSave" runat="server" Text="保 存" CssClass="mybtn" />
                    <input id="btnClose" type="button" value="关 闭" onclick="close_sure();" class="mybtn" />

                </td>
            </tr>
        </table>

    </div>
    <div id="headdiv" style="text-align: center; left: 10px; width: 1060px; height: 25px; word-wrap: break-word; overflow: hidden; font-weight: 700;">
        <!--不需要显示表头水平滚动条-->
    </div>
    <div id="bodydiv" style="left: 10px; width: 1060px; height: 400px; overflow: auto;" onscroll="headdiv.scrollLeft=scrollLeft">
        <!--表体的水平滚动条拖动触发表头的水平滚动事件-->
        <!--Gridview中必须定义表头和表体相同的宽度-->
        <asp:GridView ID="gvActions" runat="server" AutoGenerateColumns="False" BorderColor="Black" OnRowDataBound="gvActions_RowDataBound" DataKeyNames="ActionID">
            <Columns>
                <asp:TemplateField HeaderText="序号" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbNo" runat="server" Text="0" Width="40px" ClientIDMode="AutoID"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="40px" Height="25px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="操作" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdfIsMy" runat="server" Value='<%#Eval("IsMy")%>' ClientIDMode="AutoID" />
                        <asp:HiddenField ID="hdfacId" runat="server" Value='<%#Eval("ActionID")%>' />
                        <asp:HiddenField ID="hdfTitle" runat="server" Value='<%#Eval("Title")%>' />
                        <asp:HiddenField ID="hdfUrl" runat="server" Value='<%#Eval("Url")%>' />
                        <div style="margin-left: 5px; margin-right: 5px;">
                            <asp:HyperLink ID="lnkaction" runat="server" Width="80px" Text='<%#Eval("Title")%>'></asp:HyperLink>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="90px"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="设定频度" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdfFrequency" runat="server" Value='<%#Eval("FrequencyID")%>' />
                        <asp:DropDownList ID="ddlFrequency" runat="server" BorderStyle="None" Width="80px" ToolTip='<%#Eval("SysFrequencyID")%>' onchange="JavaScript:selectDpList(this)">
                        </asp:DropDownList>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="80px"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="可能时段" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdfShiDuan" runat="server" Value='<%#Eval("ShiDuanID")%>' />
                        <asp:DropDownList ID="ddlShiDuan" runat="server" BorderStyle="None" Width="100px" ToolTip='<%#Eval("SysShiDuanID")%>'>
                        </asp:DropDownList>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="100px"></HeaderStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="最小值" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="tbMin" Text='<%#Eval("MinDuring")%>' runat="server" Width="60px" ToolTip='<%#Eval("SysMinDuring")%>' BorderStyle="None"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="70px"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="最大值" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="tbMax" Text='<%#Eval("MaxDuring")%>' runat="server" Width="60px" ToolTip='<%#Eval("SysMaxDuring")%>' BorderStyle="None" onchange="changeIsMy(this)"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="70px"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="合适值" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="tbNormal" Text='<%#Eval("NormalDuring")%>' runat="server" Width="60px" ToolTip='<%#Eval("SysNormalDuring")%>' BorderStyle="None"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="70px"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="健康相关" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="tbHealthy" Text='<%#Eval("Healthy")%>' runat="server" Width="80px" ToolTip='<%#Eval("SysHealthy")%>' BorderStyle="None" onchange="changeIsMy(this)"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="90px"></HeaderStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="互动值相关" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="tbInteraction" Text='<%#Eval("Interaction")%>' runat="server" Width="80px" ToolTip='<%#Eval("SysInteraction")%>' BorderStyle="None" onchange="changeIsMy(this)"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="90px"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="活动概率" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="tbProbability" Text='<%#Eval("Probability")%>' runat="server" Width="80px" ToolTip='<%#Eval("SysProbability")%>' BorderStyle="None" onchange="changeIsMy(this)"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="90px"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="说 明" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="tbDesc" runat="server" Text='<%#Eval("Description")%>' Width="200px" BorderStyle="None" ToolTip='<%#Eval("SysDescription")%>'></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle BackColor="Azure" HorizontalAlign="Center" Font-Bold="True" Width="210px"></HeaderStyle>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
    <br />
    <asp:Label ID="lbErr" runat="server" Text="" Font-Bold="true" ForeColor="red"></asp:Label>
</div>
