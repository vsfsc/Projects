<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberInfo.aspx.cs" Inherits="LNMCM.Layouts.LNMCM.MemberInfo" DynamicMasterPageFile="~masterurl/default.master" %>

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
    <div id="divregInfo" runat="server">
    <table style=" font-size:14px; color:#494b4c" cellspacing="0" cellpadding="3">

        <tr>
            <td class="tdone">姓名：<span style="color:red">*</span></td>
            <td>
                <asp:TextBox ID="txtName" runat="server" CssClass="txtcss" onfocus="onftxtAccount()" onblur="onbtxtAccount()" ToolTip="必填项！" AutoPostBack="false"></asp:TextBox>(真实姓名) <span  style="font-size:14px; color:red;  margin-left:10px" id="spanName"></span><br/><asp:Label ID="lblNameMsg" ForeColor="Red" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td class="tdone">性别：</td>
            <td style="text-align:center;">
                <asp:RadioButtonList ID="rblSex" runat="server" RepeatDirection="Horizontal" Width="160px">
                    <asp:ListItem  Selected="True" Value="1">男</asp:ListItem>
                    <asp:ListItem Value="0">女</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="tdone">学号：<span style="color:red">*</span></td>
            <td>
                <asp:TextBox ID="txtNum" runat="server"  CssClass="txtcss" onfocus="onfocusjs()" onblur="onblurjs()" ToolTip="必填项！" CausesValidation="True" AutoPostBack="false"></asp:TextBox><span  style="font-size:14px; color:red;  margin-left:10px" id="spanNum"></span><br/>
            </td>
        </tr>

        <tr>
            <td class="tdone">专业：<span style="color:red">*</span></td>
            <td>
                <asp:HiddenField ID="HiddenField1" Value="Ccc2008neu" runat="server" />
                <asp:TextBox ID="txtProf" runat="server"   CssClass="txtcss" onfocus="onft(this)" onblur="onblurjs()" ToolTip="必填项！" CausesValidation="True" AutoPostBack="false"></asp:TextBox><span  style="font-size:14px; color:red;  margin-left:10px" id="spanProf"></span><br/>
            </td>
        </tr>

        <tr>
            <td class="tdone">联系电话：<span style="color:red">*</span></td>
            <td>
                <asp:TextBox ID="txtTelephone" runat="server" CssClass="txtcss" onfocus="onft(this)" onblur="onbtTel()" ToolTip="必填项！电话格式必须正确，比如：18888888888" AutoPostBack="false"></asp:TextBox><span  style="font-size:14px; color:red;  margin-left:10px" id="spanTel"></span><br/>
               <asp:Label ID="lblTelMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
            </td>
        </tr>

        <tr>
            <td class="tdone">验证码：<span style="color:red">*</span></td>
            <td>
                <input type="text" id="input" style="width:100px;height:25px;" />
                <input type="button" id="code" style="background-color: #ebebeb;cursor:pointer;font-size:13px;" onclick="createCode1()" value="a1b2" title="点此刷新验证码" /><span  style="font-size:14px; color:red;  margin-left:10px" id="spanValid"></span>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height:20px;"></td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center">
                <asp:Button ID="btnSave" OnClientClick="return IsValid();" runat="server" Text="保 存" BorderStyle="None" Style="background-color:#88C4FF;" CssClass="buttoncss" ToolTip="保存修改,提交信息"/>
                    <asp:Button ID="btnUnSave" runat="server" Text="取 消" BorderStyle="None" Style="background-color:#ccc;" CssClass="buttoncss" ToolTip="取消修改,返回主页"/>
            </td>
        </tr>
        <tr><td colspan ="2">
            <asp:Label ID="lblMsg" ForeColor="Red" runat="server" Text=""></asp:Label></td></tr>

    </table>
</div>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
成员信息
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
成员
</asp:Content>
