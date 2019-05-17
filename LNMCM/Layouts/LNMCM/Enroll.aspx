<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Enroll.aspx.cs" Inherits="LNMCM.Layouts.LNMCM.Enroll" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type ="text/javascript" src="Validate.js" ></script>
    <script  type="text/javascript">
        window.onbeforeunload = function () {
            var n = window.event.screenX - window.screenLeft;
            var b = n > document.documentElement.scrollWidth - 20;
            if (b && window.event.clientY < 0 || window.event.altKey)  // 是关闭而非刷新
            {
                document.getElementById('btnClose').click();
            }
        }
        var code; //在全局定义验证码
        window.onbeforeunload = function () {
            var n = window.event.screenX - window.screenLeft;
            var b = n > document.documentElement.scrollWidth - 20;
            if (b && window.event.clientY < 0 || window.event.altKey)  // 是关闭而非刷新
            {
                document.getElementById('btnClose').click();
            }
        }
        window.onload = function createCode() {

            var checkCode = document.getElementById("code");
            code=returnCode();
            checkCode.value = code;//把code值赋给验证码
        }
        function createCode1() {

            var checkCode = document.getElementById("code");
            code = returnCode();
            checkCode.value = code;//把code值赋给验证码
        }
        function IsValid() {


             var name = document.getElementById('<%=txtName.ClientID%>');
            if (name.value.length == 0) {
                //alert("姓名不能为空！");
                spanName.innerHTML = "姓名不能为空！";
                name.select();
                return false;
            }
            else if (!checkChineseCharacter(name.value)) {
                //alert("姓名只能输入中文汉字！");
                spanName.innerHTML = "姓名只能输入中文汉字！";
                name.select();
                return false;
            }
            else
                spanName.innerHTML = "";

              var num = document.getElementById('<%=txtNum.ClientID%>');
            if (num.value.length == 0) {
                //alert("姓名不能为空！");
                spanNum.innerHTML = "学号不能为空！";
                num.select();
                return false;
            }
             else
                spanNum.innerHTML = "";

            var pass = document.getElementById('<%=txtPwd.ClientID%>');
         if (!pass.getAttribute("readonly")) {
             if (pass.value.length < 6) {
                 //alert("密码长度不能小于6！");
                 spanPwd.innerHTML = "密码长度不能小于6！";
                 pass.select();
                 return false;
             }
             if (pass.value != document.getElementById('<%=txtPwd1.ClientID%>').value) {
                 //alert("密码与确认密码不一致！");
                 spanPwd1.innerHTML = "密码与确认密码不一致！";
                document.getElementById('<%=txtPwd1.ClientID%>').select();
                return false;
             }
             else
             {
                 spanPwd.innerHTML = "";
                 spanPwd1.innerHTML = "";
             }
        }

          var prof = document.getElementById('<%=txtProf.ClientID%>');
            if (prof.value.length == 0) {
                //alert("姓名不能为空！");
                spanProf .innerHTML = "专业不能为空！";
                prof.select();
                return false;
            }
             else
                spanProf.innerHTML = "";

         var tel = document.getElementById('<%=txtTelephone.ClientID%>');
            var tel = document.getElementById('<%=txtTelephone.ClientID%>');
            if ( tel.value.length == 0)
            {
                spanTel.innerHTML = "电话不能为空！";
                tel.select();
                return false;
            }
                else if (!CheckTelephone(tel.value)) {
                spanTel.innerHTML = "电话输入格式错误！";
                tel.select();
                return false;
            }
            else
                spanTel.innerHTML = "";

        var email = document.getElementById('<%=txtEmail.ClientID%>');
            if (email.value.length == 0) {
                //alert("邮箱不能为空！");
                spanEmail.innerHTML = "邮箱不能为空！";
                email.select();
                return false;
            }
            else if (!CheckEmail(email.value)) {
                spanEmail.innerHTML = "E-mail地址格式错误!"
                email.select();
                return false;
            }
            else
                spanEmail.innerHTML = "";

            var inputCode = document.getElementById("input").value.toUpperCase(); //取得输入的验证码并转化为大写
            if (inputCode.length <= 0) { //若输入的验证码长度为0
                //alert("请输入验证码！"); //则弹出请输入验证码
                spanValid.innerHTML = "验证码输入错误！";
                document.getElementById("input").select();
                return false;
            }
            else if (inputCode != code) { //若输入的验证码与产生的验证码不一致时
                //alert("验证码输入错误！"); //则弹出验证码输入错误
                spanValid.innerHTML = "验证码输入错误！";
                code = returnCode();
                document.getElementById("code").value = code//刷新验证码
                //document.getElementById("input").value = "";//清空文本框
                document.getElementById("input").select();
                return false;
            }
            else
                spanValid.innerHTML = "";
            return true;
        }
        </script>
    <style type="text/css">
         .h3css{ font-family:微软雅黑; color:#4141a4; text-decoration:underline}
       .tdone{text-align:right; padding-right:10px; color:#4a4a4a; width:120px;vertical-align:top;}
       .txtcss{ width:200px; border:1px #bebee1 solid; height:25px; vertical-align:middle; line-height:25px; padding:0 10px;
                color:Black;}
        .txtdoenlist{ width:103px; height:25px; line-height:25px; vertical-align:middle; padding:3px;border:1px #bebee1 solid;color:#494b4c; }
        .txtdoenlista{ width:210px; height:25px; line-height:25px; vertical-align:middle; padding:3px;border:1px #bebee1 solid;color:#494b4c; }
        .buttoncss{ width:120px;height:40px; background-color:#3776a9; color:#fff;font-family:微软雅黑; font-size:20px ; border:1px solid #3776a9; cursor:pointer }
     </style>
      <style type="text/css">
            .stable {
                width: 800px;
                margin: 0px auto;
                text-align: left;
                position: relative;
                border-top-left-radius: 5px;
                border-top-right-radius: 5px;
                border-bottom-right-radius: 5px;
                border-bottom-left-radius: 5px;
                font-size: 14px;
                font-family: 微软雅黑, 黑体;
                line-height: 1.5;
                box-shadow: rgb(153, 153, 153) 0px 0px 5px;
                border-collapse: collapse;
                background-position: initial initial;
                background-repeat: initial initial;
                background: #fff;
            }

            .stable th {
                height: 25px;
                text-align: center;
                line-height: 25px;
                padding: 15px 35px;
                border-bottom-width: 1px;
                border-bottom-style: solid;
                border-bottom-color: #C46200;
                background-color: #FEA138;
                border-top-left-radius: 5px;
                border-top-right-radius: 5px;
                border-bottom-right-radius: 0px;
                border-bottom-left-radius: 0px;
            }

            .f_blue:link {
                color: #0000ff;
                text-decoration: none;
            }

            .f_blue:visited {
                color: #0000ff;
                text-decoration: none;
            }

            .f_blue:hover {
                color: #ff2020;
                text-decoration: underline;
            }

            .f_blue:actived {
                color: #0000ff;
                text-decoration: none;
            }
        </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="divregInfo" runat="server" visible="true">
        <div style="padding-left: 60px; padding-bottom: 20px; font-size: 14px;">
            <h3 style="color: red">温馨提示：</h3>
            <ul style="line-height: 30px;">
                <li>​报名成功后，系统会生成报名序号，并发送报名信息到你预设的邮箱，请采用“ccc\报名序号”作为用户名登录系统，</li>
                <li>登录后，在“我的报名”页面，你即可查看报名信息，并可以管理团队成员。</li>
            </ul>
        </div>
        <table style="font-size: 14px; color: #494b4c" cellspacing="0" cellpadding="3">
            <tr>
                <td class="tdone">培养单位：</td>
                <td>
                    <asp:DropDownList ID="ddlSchool" runat="server" AutoPostBack="false" CssClass="txtdoenlist" Width="210px"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tdone">姓 名：<span style="color: red">*</span></td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" CssClass="txtcss" onfocus="onftxtAccount()" onblur="onbtxtAccount()" ToolTip="必填项！" AutoPostBack="false"></asp:TextBox>(真实姓名) <span style="font-size: 14px; color: red; margin-left: 10px" id="spanName"></span>
                    <br />
                    <asp:Label ID="lblNameMsg" ForeColor="Red" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="tdone">性 别：</td>
                <td style="text-align: center;">
                    <asp:RadioButtonList ID="rblSex" runat="server" RepeatDirection="Horizontal" Width="160px">
                        <asp:ListItem Selected="True" Value="1">男</asp:ListItem>
                        <asp:ListItem Value="0">女</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="tdone">学 号：<span style="color: red">*</span></td>
                <td>
                    <asp:TextBox ID="txtNum" runat="server" CssClass="txtcss" onfocus="onfocusjs()" onblur="onblurjs()" ToolTip="必填项！" CausesValidation="True" AutoPostBack="true"></asp:TextBox><span style="font-size: 14px; color: red; margin-left: 10px" id="spanNum"></span><br />
                    <asp:Label ID="lblNumMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tdone">密 码：<span style="color: red">*</span></td>
                <td>
                    <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" CssClass="txtcss" onfocus="onfocusjs()" onblur="onblurjs()" ToolTip="必填项！密码最少6位" CausesValidation="True" AutoPostBack="false"></asp:TextBox><span style="font-size: 14px; color: red; margin-left: 10px" id="spanPwd"></span><br />
                    <asp:Label ID="lblPwdMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tdone">确认密码：<span style="color: red">*</span></td>
                <td>
                    <asp:TextBox ID="txtPwd1" runat="server" TextMode="Password" CssClass="txtcss" onfocus="onft(this)" onblur="onblurjs()" ToolTip="必填项！两次密码必须一致" CausesValidation="True" AutoPostBack="false"></asp:TextBox><span style="font-size: 14px; color: red; margin-left: 10px" id="spanPwd1"></span><br />
                    <asp:Label ID="lblPwd1Msg" ForeColor="Red" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tdone">专 业：<span style="color: red">*</span></td>
                <td>
                    <asp:HiddenField ID="HiddenField1" Value="Ccc2008neu" runat="server" />
                    <asp:TextBox ID="txtProf" runat="server" CssClass="txtcss" onfocus="onft(this)" onblur="onblurjs()" ToolTip="必填项！" CausesValidation="True" AutoPostBack="false"></asp:TextBox><span style="font-size: 14px; color: red; margin-left: 10px" id="spanProf"></span><br />
                </td>
            </tr>

            <tr>
                <td class="tdone">联系电话：<span style="color: red">*</span></td>
                <td>
                    <asp:TextBox ID="txtTelephone" runat="server" CssClass="txtcss" onfocus="onft(this)" onblur="onbtTel()" ToolTip="必填项！电话格式必须正确，比如：18888888888" AutoPostBack="false"></asp:TextBox><span style="font-size: 14px; color: red; margin-left: 10px" id="spanTel"></span><br />
                    <asp:Label ID="lblTelMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tdone">电子邮箱：<span style="color: red">*</span></td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="txtcss" onfocus="onft(this)" onblur="onbtEmail()" ToolTip="必填项！邮箱格式必须正确，比如：example@mail.com" AutoPostBack="false"></asp:TextBox><span style="font-size: 14px; color: red; margin-left: 10px" id="spanEmail"></span><br />
                    <asp:Label ID="lblEmailMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="tdone">验证码：<span style="color: red">*</span></td>
                <td>
                    <input type="text" id="input" style="width: 100px; height: 25px;" />
                    <input type="button" id="code" style="background-color: #ebebeb; cursor: pointer; font-size: 13px;" onclick="createCode1()" value="a1b2" title="点此刷新验证码" /><span style="font-size: 14px; color: red; margin-left: 10px" id="spanValid"></span>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 20px;"></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align: left">
                    <asp:Button ID="btnSave" OnClientClick="return IsValid();" runat="server" Text="报 名" BorderStyle="None" Style="background-color: #88C4FF; cursor: pointer;" />
                    &nbsp; &nbsp;
                <asp:Button ID="btnClose" runat="server" Text="取 消" BorderStyle="None" Style="background-color: #dedede; cursor: pointer;" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="padding: 10px 0 10px 30px;">
                        <asp:Label ID="lblMsg" ForeColor="Red" runat="server" Text="" Font-Size="14"></asp:Label>
                    </div>
                </td>
            </tr>

        </table>
    </div>
    <div id="divregSuccess" runat="server" visible="false">
        <table cellpadding="0" align="center" class="stable">
            <tbody>
                <tr>
                    <th valign="middle">
                        <span style="color: rgb(255, 255, 255); font-size: 20px; font-family: 微软雅黑;">辽宁省第二届研究生数学建模竞赛</span>
                    </th>
                </tr>
                <tr>
                    <td>
                        <div style="padding: 25px 35px 40px; background-color: #F3F3F3;">
                            <h2 style="margin: 5px 0px; line-height: 22px; color: #333333;">尊敬的<asp:Label ID="lbuserName" runat="server" Text=""></asp:Label>：
                            </h2>
                            <div style="line-height: 25px; font-size: 14px;">
                                您好，欢迎您报名参加辽宁省第二届研究生数学建模竞赛！<br />
                                您的报名序号是：<asp:Label ID="lbuseAcc" runat="server" Text=""></asp:Label>，也是您登录本站的账号
                                    <br />
                                请在登录页面中输入“<asp:Label ID="lbuserADAcc" runat="server" Text=""></asp:Label>”账号格式和您设置的密码登录。
                                    <br />
                                请牢记您的报名账号和密码，这些报名信息已同时发送到您预留的邮箱<asp:Label ID="lbEmail" runat="server" Text=""></asp:Label>中，请注意查收！

                                    <div style="text-align: center; padding: 20px">
                                        <a href="/LNMCM/_layouts/15/Authenticate.aspx?Source=%2FLNMCM/_layouts/15/lnmcm/MyEnroll.aspx" style="font-size: 16px; color: #C46200" class="f_blue">点此马上登录</a>&nbsp;&nbsp;
                                        <a href="/LNMCM/" class="f_blue" style="font-size: 16px; color: royalblue">返回网站首页</a>
                                    </div>
                                <br />

                            </div>
                            <p align="right">辽宁省第二届研究生数学建模竞赛组委会</p>
                            <p align="right">
                                <asp:Label ID="lbDateNow" runat="server" Text=""></asp:Label>
                            </p>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <asp:Label ID="lbnoEnroll" runat="server" Text=""></asp:Label>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
数学建模竞赛报名
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
竞赛报名
</asp:Content>
