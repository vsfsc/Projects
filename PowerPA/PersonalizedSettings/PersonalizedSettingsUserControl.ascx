<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonalizedSettingsUserControl.ascx.cs" Inherits="PowerPA.PersonalizedSettings.PersonalizedSettingsUserControl" %>

<asp:UpdatePanel ID="mysettings" runat="server">
    <ContentTemplate>

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
             <script type="text/javascript">
            function cacul() {
                var gv_chkCount = 0;
                var gvCheck;
                var lbAction
                var table = document.getElementById('<%=gvActions.ClientID%>');//.getElementsByTagName("input");
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
            function IsFormChanged() {
                var isChanged = false;
                var form = document.forms[0];
                for (var i = 0; i < form.elements.length; i++) {
                    var element = form.elements[i];
                    var type = element.type;
                    if (type == "text" || type == "hidden" || type == "textarea" || type == "button") {
                        if (element.value != element.defaultValue) {
                            isChanged = true;
                            break;
                        }
                    } else if (type == "radio" || type == "checkbox") {
                        if (element.checked != element.defaultChecked) {
                            isChanged = true;
                            break;
                        }
                    } else if (type == "select-one"|| type == "select-multiple") {
                        for (var j = 0; j < element.options.length; j++) {
                            if (element.options[j].selected != element.options[j].defaultSelected) {
                                isChanged = true;
                                break;
                            }
                        }
                    } else {
                        //  etc...
                    }
                }
                return isChanged;
            }
            function close_sure() {
                var ischanged=IsFormChanged();
                if(ischanged){
                    var gnl = confirm("您设置的内容尚未保存，确认保存并关闭页面吗?若不保存，请点击取消！");
                    if (gnl == true) {
                        //return true;
                        var a = " <%=WriteDataToList()%>";
                        console.log(a);
                        window.location.href = getbackUrl();
                    }
                    else {
                        window.location.href = getbackUrl();
                    }
                }
                else{
                    window.location.href = getbackUrl();
                }
            }

            function getQueryString(name) {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
                var r = window.location.search.substr(1).match(reg);
                if (r != null) return unescape(r[2]);
                return null;
            }
            function getbackUrl() {
                if (getQueryString("Source") != null) {
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
            <div id="divBtn" style="width: 1025px; text-align: right; margin-bottom: 10px; font-size: 14px; color: #0094ff;  left: 10px;" runat="server">
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
                            <asp:Button ID="btnSave" runat="server" Text="保 存" OnClick="btnSave_Click" CssClass="mybtn" />
                            <input id="btn_Close" type="button" value="关 闭" onclick="close_sure();" class="mybtn" />

                        </td>
                    </tr>

                </table>

            </div>
            <div id="headdiv" style="text-align: center; left: 10px;; width: 1060px; height:25px; word-wrap: break-word; overflow: hidden;font-weight:700;">
                <!--不需要显示表头水平滚动条-->
            </div>
            <div id="bodydiv" style="left: 10px;width: 1060px; height:400px; overflow: auto;" onscroll="headdiv.scrollLeft=scrollLeft">
                <!--表体的水平滚动条拖动触发表头的水平滚动事件-->
                <!--Gridview中必须定义表头和表体相同的宽度-->
                <asp:GridView ID="gvActions" runat="server" AutoGenerateColumns="False" BorderColor="Black" OnRowDataBound="gvActions_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <div style="margin-left: 5px; margin-right: 5px;">
                                    <asp:Label ID="lbID" runat="server" Text='<%#Eval("ID")%>'></asp:Label>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure"></HeaderStyle>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="序号" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lbNo" runat="server" Text="0" Width="40px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="40px" Height="25px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="操作" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div style="margin-left: 5px; margin-right: 5px;">
                                    <asp:HiddenField ID="hdfacId" runat="server" Value='<%#Eval("ActionID")%>' />
                                    <asp:HiddenField ID="hdfTitle" runat="server" Value='<%#Eval("Title")%>' />
                                    <asp:HiddenField ID="hdfUrl" runat="server" Value='<%#Eval("Url")%>' />
                                    <asp:HyperLink ID="lnkaction" runat="server" NavigateUrl='<%#Eval("Url")%>' Width="80px" Text='<%#Eval("Title")%>'></asp:HyperLink>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="90px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="频度" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfpindu" runat="server" Value='<%#Eval("Frequency")%>' />
                                <asp:DropDownList ID="ddlpindu" runat="server" BorderStyle="None" Width="80px" ToolTip='<%#Eval("SysFrequency")%>'>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="80px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="可能时段" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfperiod" runat="server" Value='<%#Eval("Period")%>' />
                                <asp:DropDownList ID="ddlperiod" runat="server" BorderStyle="None" Width="100px" ToolTip='<%#Eval("SysPeriod")%>'>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="100px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="设置值内容" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfDorM" runat="server" Value='<%#Eval("DorM")%>' />
                                <asp:DropDownList ID="ddlDorM" runat="server" BorderStyle="None" Width="80px" ToolTip='<%#Eval("SysDorM")%>'>
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="80px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="最小值" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbmin" Text='<%#Eval("MinDuring")%>' runat="server" Width="60px" ToolTip='<%#Eval("SysMinDuring")%>' BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="70px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="最大值" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbmax" Text='<%#Eval("MaxDuring")%>' runat="server" Width="60px" ToolTip='<%#Eval("SysMaxDuring")%>' BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True"  Width="70px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="合适值" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbduring" Text='<%#Eval("NormalDuring")%>' runat="server" Width="60px" ToolTip='<%#Eval("SysNormalDuring")%>' BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="70px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="健康相关" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbHealthy" Text='<%#Eval("Healthy")%>' runat="server" Width="80px" ToolTip='<%#Eval("SysHealthy")%>' BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="90px"></HeaderStyle>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="互动值相关" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbInteraction" Text='<%#Eval("Interaction")%>' runat="server" Width="80px" ToolTip='<%#Eval("SysInteraction")%>' BorderStyle="None"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" BackColor="Azure" Font-Bold="True" Width="90px"></HeaderStyle>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="说 明" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDesc" runat="server" Text='<%#Eval("Desc")%>' Width="200px" BorderStyle="None" ToolTip='<%#Eval("SysDesc")%>'></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle BackColor="Azure" HorizontalAlign="Center" Font-Bold="True" Width="210px"></HeaderStyle>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>

        </div>

        <div style="margin-left: 10px;top:450px;position:absolute;">
            <asp:Label ID="lbErr" runat="server" Text="" ForeColor="red"></asp:Label>
        </div>


    </ContentTemplate>
</asp:UpdatePanel>
