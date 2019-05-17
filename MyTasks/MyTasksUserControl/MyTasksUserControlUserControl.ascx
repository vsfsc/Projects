<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyTasksUserControlUserControl.ascx.cs" Inherits="MyTasks.MyTasksUserControl.MyTasksUserControlUserControl" %>
<style type="text/css">
        body{margin:0px; font-family:Arial, Sans-Serif; font-size:13px;}
        /* dock */
        #dock{margin:0px; padding:0px; list-style:none; position:fixed; top:100px; height:70%; 
              z-index:100; background-color:#f0f0f0; right:0px;}
        #dock > li {width:40px; height:120px; margin: 0 0 1px 0; background-color:#dcdcdc;
                     background-repeat:no-repeat; background-position:left center;}
        
        #dock #links {background-image:url(http://ismartweb/Style%20Library/Images/links.png);}
        #dock #files {background-image:url(http://ismartweb/Style%20Library/Images/files.png);}
        #dock #tools {background-image:url(http://ismartweb/Style%20Library/Images/tools.png);}

        #dock > li:hover {background-position:-40px 0px;}
        
        /* panels */
        #dock ul li {padding:5px; border: solid 1px #0000;line-height:25px;}
        #dock ul li:hover {background:#D3DAED url(http://ismartweb/Style%20Library/Images/item_bkg.png) repeat-x; border: solid 1px #A8D8EB;}
        #dock ul li.header, #dock ul li .header:hover {background:#D3DAED url(http://ismartweb/Style%20Library/Images/header_bkg.png) repeat-x;border: solid 1px #F1F1F1;}
      
        #dock > li:hover ul {display:block;}
        #dock > li ul {position:absolute; top:0px; right:-180px;  z-index:-1;width:180px; display:none;
                       background-color:#F1F1F1; border:solid 1px #969696; padding:0px; margin:0px; list-style:none;}
        #dock > li ul.docked { display:block;z-index:-2;}
        
        .dock,.undock{float:right;}
       .undock {display:none;}
        #content {margin: 10px 0 0 60px;}
		.free{margin:5px;}
     .shuli{ margin:0 auto;width:20px;line-height:24px;font-size:16px;text-align:right;} 
    </style>
    <script type="text/javascript" src="http://ismartweb/Style%20Library/jquery-1.2.6.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function(){
            var docked = 0;
            
            $("#dock li ul").height(3*$(window).height()/4);
            
            $("#dock .dock").click(function(){
                $(this).parent().parent().addClass("docked").removeClass("free");
                
                docked += 1;
                var dockH = (3*$(window).height()/4) / docked
                var dockT = 0;               
                
                $("#dock li ul.docked").each(function(){
                    $(this).height(dockH).css("top", dockT + "px");
                    dockT += dockH;
                });
                $(this).parent().find(".undock").show();
                $(this).hide();
                /**
                if (docked > 0)
                    $("#content").css("margin-right","250px");
                else
                    $("#content").css("margin-right", "60px");
					**/
            });
            
             $("#dock .undock").click(function(){
                $(this).parent().parent().addClass("free").removeClass("docked")
                    .animate({right:"-180px"}, 200).height(3*$(window).height()/4).css("top", "0px");
                
                docked = docked - 1;
                var dockH = (3*$(window).height()/4) / docked
                var dockT = 0;               
                
                $("#dock li ul.docked").each(function(){
                    $(this).height(dockH).css("top", dockT + "px");
                    dockT += dockH;
                });
                $(this).parent().find(".dock").show();
                $(this).hide();
                /**
                if (docked > 0)
                    $("#content").css("margin-right", "250px");
                else
                    $("#content").css("margin-right", "60px");
					**/
            });

            $("#dock li").hover(function(){
                $(this).find("ul").animate({right:"40px"}, 200);
            }, function(){
                $(this).find("ul.free").animate({right:"-180px"}, 200);
           });
        }); 
    </script>
	
<ul id="dock">
		<li style="height:50px;"><img src="http://ismartweb/Style%20Library/Images/mytask.png" title="我的任务" alt=""/></li>
        <li id="links">
            <ul class="free"  style="overflow-x:hidden;overflow-y:auto;"  id="Starting" runat="server">
                <li class="header"><a href="#" class="dock">固定显示</a><a href="#" class="undock">取消固定</a>即将开始的任务</li>
                <li><div><a href="#">任务名称</a> </div><div>进度：20%</div><div>开始时间：2017年1月13日</div><div>截至时间：2017年1月13日</div></li>
				<li><div><a href="#">任务名称</a> 进度：20%</div><div>开始时间：2017年1月13日</div><div>截至时间：2017年1月13日</div></li>
            </ul>
        </li>
        <li id="files" >
            <ul class="free"  style="overflow-x:hidden;overflow-y:auto;"  id="OnGoing" runat="server">
                <li class="header"><a href="#" class="dock">固定显示</a><a href="#" class="undock">取消固定</a>正在进行的任务</li>
                <li><div><a href="#">任务名称</a> 进度：20%</div><div>开始时间：2017年1月13日</div><div>截至时间：2017年1月13日</div></li>
            </ul>
        </li>
        <li id="tools">
            <ul class="free"  style="overflow-x:hidden;overflow-y:auto;" id="Finished" runat="server">
                <li class="header"><a href="#" class="dock">固定显示</a><a href="#" class="undock">取消固定</a>已完成的任务</li>
                <li><div><a href="#">任务名称</a> 进度：20%</div><div>开始时间：2017年1月13日</div><div>截至时间：2017年1月13日</div></li>
           </ul>
        </li>
		
    </ul>
<div id="content"></div>