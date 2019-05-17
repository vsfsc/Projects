<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonalInforUserControl.ascx.cs" Inherits="PowerPA.PersonalInfor.PersonalInforUserControl" %>
<style type="text/css">
    body {
        font-size: 12px;
    }

    ul,
    li,
    h2 {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style: none;
    }

    #top {
        width: 900px;
        height: 40px;
        margin: 0 auto;
        background-color: #CCCC00;
    }

        #top h2 {
            width: 150px;
            height: 40px;
            background-color: #99CC00;
            float: left;
            font-size: 14px;
            text-align: center;
            line-height: 40px;
        }

    #topTags {
        width: 750px;
        height: 40px;
        margin: 0 auto;
        background-color: #CCCC00;
        float: left;
    }

        #topTags ul li {
            float: left;
            width: 100px;
            height: 25px;
            margin-right: 5px;
            display: block;
            text-align: center;
            cursor: pointer;
            padding-top: 15px;
        }

    #main {
        width: 900px;
        height: 500px;
        margin: 0 auto;
        background-color: #F5F7E6;
    }

    #leftMenu {
        width: 150px;
        height: 500px;
        background-color: #009900;
        float: left;
    }

        #leftMenu ul {
            margin: 10px;
        }

            #leftMenu ul li {
                width: 130px;
                height: 30px;
                display: block;
                background: #99CC00;
                cursor: pointer;
                line-height: 30px;
                text-align: center;
                margin-bottom: 5px;
            }

                #leftMenu ul li a {
                    color: #000000;
                    text-decoration: none;
                }

    #content {
        width: 750px;
        height: 500px;
        float: left;
    }

    .content {
        width: 740px;
        height: 490px;
        display: none;
        padding: 5px;
        overflow-y: auto;
        line-height: 30px;
    }

    #footer {
        width: 900px;
        height: 30px;
        margin: 0 auto;
        background-color: #ccc;
        line-height: 30px;
        text-align: center;
    }

    .content1 {
        width: 740px;
        height: 490px;
        display: block;
        padding: 5px;
        overflow-y: auto;
        line-height: 30px;
    }

    .Infotable {
        width: 100%;
        font-size: 14px;
        border: none;
    }

        .Infotable th {
            text-align: right;
            font-weight: 600;
        }

        .Infotable tr {
            text-align: left;
        }
</style>
<script type="text/javascript">window.onload = function () {
    function $(id) {
        return document.getElementById(id)
    }
    var menu = $("topTags").getElementsByTagName("ul")[0]; //顶部菜单容器
    var tags = menu.getElementsByTagName("li"); //顶部菜单
    var ck = $("leftMenu").getElementsByTagName("ul")[0].getElementsByTagName("li"); //左侧菜单
    var j;
    //点击左侧菜单增加新标签
    for (i = 0; i < ck.length; i++) {
        ck[i].onclick = function () {
            $("welcome").style.display = "none" //欢迎内容隐藏
            //循环取得当前索引
            for (j = 0; j < 8; j++) {
                if (this == ck[j]) {
                    if ($("p" + j) == null) {
                        openNew(j, this.innerHTML); //设置标签显示文字
                    }
                    clearStyle();
                    $("p" + j).style.backgroundColor = "yellow";
                    clearContent();
                    $("c" + j).style.display = "block";
                }
            }
            return false;
        }
    }
    //增加或删除标签
    function openNew(id, name) {
        var tagMenu = document.createElement("li");
        tagMenu.id = "p" + id;
        tagMenu.innerHTML = name + "   " + "<img src='' style='vertical-align:middle'/>";
        //标签点击事件
        tagMenu.onclick = function (evt) {
            clearStyle();
            tagMenu.style.backgroundColor = "yellow";
            clearContent();
            $("c" + id).style.display = "block";
        }
        //标签内关闭图片点击事件
        tagMenu.getElementsByTagName("img")[0].onclick = function (evt) {
            evt = (evt) ? evt : ((window.event) ? window.event : null);
            if (evt.stopPropagation) {
                evt.stopPropagation()
            } //取消opera和Safari冒泡行为;
            this.parentNode.parentNode.removeChild(tagMenu); //删除当前标签
            var color = tagMenu.style.backgroundColor;
            //设置如果关闭一个标签时，让最后一个标签得到焦点
            if (color == "#ffff00" || color == "yellow") { //区别浏览器对颜色解释
                if (tags.length - 1 >= 0) {
                    clearStyle();
                    tags[tags.length - 1].style.backgroundColor = "yellow";
                    clearContent();
                    var cc = tags[tags.length - 1].id.split("p");
                    $("c" + cc[1]).style.display = "block";
                } else {
                    clearContent();
                    $("welcome").style.display = "block"
                }
            }
        }
        menu.appendChild(tagMenu);
    }
    //清除标签样式
    function clearStyle() {
        for (i = 0; i < tags.length; i++) {
            menu.getElementsByTagName("li")[i].style.backgroundColor = "#FFCC00";
        }
    }
    //清除内容
    function clearContent() {
        for (i = 0; i < 4; i++) {
            $("c" + i).style.display = "none";
        }
    }
}

</script>
<div>
       
    <div id="top">
        <h2>管理菜单</h2>
        <div id="topTags">
            <ul>
            </ul>
        </div>
    </div>
    <div id="main">
        <div id="leftMenu">
            <ul>
                <li>基本资料</li>
                <li>教育经历</li>
                <li>职业经历</li>
                <li>帮助信息</li>
            </ul>
        </div>
        <div id="welcome" class="content" style="display: block;">
            <div align="center">
                <p></p>
                <p><strong>个人信息设置</strong></p>
<asp:UpdatePanel ID="mysettings" runat="server">
                <ContentTemplate>

                        <table class="Infotable" cellpadding="4" cellspacing="4">

                            <tr>
                                <th>账户：</th>
                                <td>
                                    <asp:Label ID="lbAccount" runat="server" Text="" Width="200"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th>姓名：</th>
                                <td>
                                    <asp:TextBox ID="tbName" runat="server" Text="" Width="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>身份证号：</th>
                                <td>
                                    <asp:TextBox ID="tbIDCard" runat="server" Text="" Width="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>性别：</th>
                                <td>
                                    <div style="width: 200px; text-align: center">
                                        <asp:RadioButtonList ID="rblistSex" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0">女</asp:ListItem>
                                            <asp:ListItem Value="1">男</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>

                                </td>
                            </tr>
                            <tr>
                                <th>生日：</th>
                                <td>
                                    <asp:TextBox ID="tbBirthday" runat="server" TextMode="Date"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>电话：</th>
                                <td>
                                    <asp:TextBox ID="tbTelephone" runat="server" TextMode="Phone" Text='<%#Bind("Telephone") %>' Width="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>邮箱：</th>
                                <td>
                                    <asp:TextBox ID="tbEmail" runat="server" TextMode="Email" Text="" Width="200"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>职业：</th>
                                <td>
                                    <asp:DropDownList ID="ddlProfession" runat="server" Width="210"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>行业：</th>
                                <td>
                                    <asp:DropDownList ID="ddlIndustry" runat="server" Width="210"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>最高学历：</th>
                                <td>
                                    <asp:DropDownList ID="ddlDegree" runat="server" Width="210"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>地区：</th>
                                <td>
                                    <asp:DropDownList ID="ddlSheng" runat="server" Width="60" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlSheng_SelectedIndexChanged"></asp:DropDownList>
                                    &nbsp;
				<asp:DropDownList ID="ddlShi" runat="server" Width="60" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlShi_SelectedIndexChanged"></asp:DropDownList>
                                    &nbsp;
				<asp:DropDownList ID="ddlXian" runat="server" Width="60"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>个人简介：</th>
                                <td>
                                    <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine" Rows="3" Width="200">
						我是一个***
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="保 存" OnClick="btnSave_Click" />
                                    &nbsp;&nbsp;
				<%--<asp:Button ID="btnCancel" runat="server" Text="取 消" />--%>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lbErr" runat="server" Text=""></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
    
                <p></p>
            </div>
        </div>
        <div id="c0" class="content">
            填写基本资料
        </div>
        <div id="c1" class="content">
            增加教育经历
        </div>
        <div id="c2" class="content">
            增加职业经历
        </div>
        <div id="c3" class="content">
            帮助信息列表
        </div>
    </div>

</div>
