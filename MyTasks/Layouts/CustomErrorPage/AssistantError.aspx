<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssistantError.aspx.cs" %>
<html>
    
    <head>
        <title></title>
        <style type="text/css">
            #div1 {
                width:100%;
                height:100%;
                vertical-align:middle;
                text-align:center;
                position:relative;
            }
             #div2 {
               
                margin: auto; 
                text-align:center;
                top:30%;
                left:20%; 
                position: absolute;
                
            }

        </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <div id="div1">
               <div id="div2">
                    <span style="color:red ;font-weight:bold;line-height:35px;margin-bottom:20px;font-size:20px;" >
                        此时间段时已经存在其他操作，指重新指定计划开始或计划时长！
                    </span>
                    <br/> 
                    <a href="javascript:window.history.go(-1);" style="text-align:center;font-size:16px;">返回上一页</a>
              </div>
            </div>
        </form>
    </body>
</html>
    
