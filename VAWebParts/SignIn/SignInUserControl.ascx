<%@ Assembly Name="VAWebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2913bbd2fc551ce8" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignInUserControl.ascx.cs" Inherits="VAWebParts.SignIn.SignInUserControl" %>
<style type="text/css">
    .yuanjiao{
     font-family: Arial;
     border: 1px solid #ddd;
     border-radius: 10px;
     padding: 10px 10px 10px 5px;
     background-color:#E3E3E3;
	 margin-bottom:5px;
     }
    .to{width:80px;height:80px;border-radius:80px}
    #signin{
        border:dashed 1px blue;
    }
    .loop_div{width:500px;background:#CCC;margin:10px;float:left;}
    .loop_div div,.loop_div ul{width:100%;overflow:hidden;}
    .loop_div div img,.loop_div div p{float:left;}
    .loop_div div img{width:160px;}
    .loop_div div p{width:300px;}


    /* 无边图片容器-默认定宽定高、图片拉伸 */
    .u-img{display:block;width:90px;height:90px;text-align:center;}
    .u-img img{display:block;width:100%;height:100%;}
    /* 图片高适应 */
    .u-img-ha,.u-img-ha img{height:auto;}
    /* 有边图片容器-默认定宽定高、图片拉伸 */
    .u-img2{display:block;position:relative;width:104px;height:104px;padding:2px;border:1px solid #ddd;}
    .u-img2 img{display:block;width:100%;height:100%;}
    /* 图片高适应 */
    .u-img2-ha,.u-img2-ha img{height:auto;}
</style>
<script type="text/javascript" >
    function $(element){
        return element = document.getElementById(element);
    }
    function $D(){
        var d=$('mingxi');
        var h=d.offsetHeight;
        var maxh=220;
    function dmove(){
        h+=50; //设置层展开的速度
        if(h>=maxh){
        d.style.height='220px';
        clearInterval(iIntervalId);
        }
        else {
        d.style.display='block';
        d.style.height=h+'px';
        }
    }
    iIntervalId=setInterval(dmove,2);
    }
    function $D2(){
    var d=$('mingxi');
    var h=d.offsetHeight;
    var maxh=220;
    function dmove(){
    h-=50;//设置层收缩的速度
    if(h<=0){
    d.style.display='none';
    clearInterval(iIntervalId);
    }else{
    d.style.height=h+'px';
    }
    }
    iIntervalId=setInterval(dmove,2);
    }
    function $use(targetid,objN){
    var d=$(targetid);
    var sb=$(objN);
     if (d.style.display=="block"){
        $D2();
           d.style.display="none";
           //sb.innerHTML="<img src='/_layouts/15/images/show.png'/>";
      } else {
        $D();
           d.style.display="block";
           //sb.innerHTML="<img src='/_layouts/15/images/show.png'/>";
       }
    }
</script>
<div id="ss" runat="server" style="width:300px;font-size:14px;font-family:'Microsoft YaHei'">
    <div id="nows" class="yuanjiao" style="text-align:center;">
        <script>
            var nows = new Date().toLocaleString();
            document.getElementById('nows').innerHTML = "现在时刻：" + nows;
            setInterval("document.getElementById('nows').innerHTML='现在时刻：'+new Date().toLocaleString();", 1000);
        </script> 
    </div>
        <div class="yuanjiao" >
            <table>
                <tr>
                    <td style="margin:10px">
                        <div class="u-img" >
                            <img src="/_layouts/15/images/PersonPlaceholder.200x150x32.png" alt="" class="to" title="点击查看签到详情"/>
                            <asp:Image ID="imghp" runat="server" CssClass="to" Visible="false" />
                            <asp:ImageButton ID="imgPhoto" runat="server" CssClass="to" Visible="false"/>                                           
                        </div>
                        <asp:Image ID="imgStar" runat="server" ImageUrl="/_layouts/15/images/star.png" ImageAlign="Middle"/>
                    </td>
                    <td style="text-align:center;">						
                        <asp:ImageButton ID="SigninBtn" runat="server" ImageUrl="/_layouts/15/images/SignIn.png" ToolTip="点击签到" OnClick="SigninBtn_Click"/>
                        <asp:Image ID="ImgSigned" runat="server" ImageUrl="/_layouts/15/images/Signed.png" ToolTip="今日已签到成功" Visible="false"/>
                        
                        <div style="margin:5px;font-weight:bold;">                            
                            <asp:Label ID="lbDegree" runat="server" Text="★" Visible="false" ></asp:Label>
                            <asp:Label ID="lbDisplayName" runat="server" Text=""></asp:Label>
                            <div id="stateBut" onClick="$use('mingxi','stateBut')" style="float:right;cursor:pointer;font-weight:bold;" >...</div>
						</div>						
                    </td>
                </tr>
            </table>           
        </div>
    
    <div class="yuanjiao" id="mingxi" style="display:none;">
        <h3>签到明细</h3>
        <ul style="text-align:left;">
            <li>签到等级：</li>
            <li>累计积分：<asp:Label ID="lbAllScores" runat="server" Text="0"></asp:Label></li>
            <li>累计天数：<asp:Label ID="lbAllDays" runat="server" Text="0"></asp:Label></li>
            <li>签到排行：<asp:Label ID="lbRank" runat="server" Text="-"></asp:Label></li>                
        </ul>        
        <ul style="text-align:left;">
            <li>连续签到天数：<asp:Label ID="lbDays" runat="server" Text="0"></asp:Label></li>
            <li>上次签到时间：<asp:Label ID="lbLastTime" runat="server" Text="2016年12月2日12:52:12"></asp:Label></li>
            <li>上次签到积分：<asp:Label ID="lbLastScore" runat="server" Text="0"></asp:Label></li>
            <li>上次签到排行：<asp:Label ID="lbLastOrder" runat="server" Text="0"></asp:Label></li>
            <li>上次签到IP：<asp:Label ID="lbIP" runat="server" Text="202.118.11.166"></asp:Label></li>
        </ul>
    </div>
</div>