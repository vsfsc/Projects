<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListCalendarUserControl.ascx.cs" Inherits="VAWebParts.ListCalendar.ListCalendarUserControl" %>
<link rel="stylesheet" href="/_layouts/ListCalendar/style/documentation.css" type="text/css" />
<link rel="stylesheet" href="/_layouts/ListCalendar/style/jalendar.css" type="text/css" />
<script type="text/javascript" src="/_layouts/ListCalendar/js/jquery-1.10.2.min.js"></script><!--jQuery-->
<script type="text/javascript" src="/_layouts/ListCalendar/js/jalendar.js"></script>
<script type="text/javascript">
$(function () {
    $('#myId3').jalendar(
	{
	lang: 'CS'
	});
});

</script>
<script type="text/javascript">
    function getlistitem() {
        var mycontext = new SP.ClientContext();
        var mysite = mycontext.get_web();
        var query = new SP.CamlQuery();
        query.set_viewXml("<View><Query></Query></View>");
        var mylist = mysite.get_lists().getByTitle('Posts');
        myitem = mylist.getItems(query);
        mycontext.load(myitem);
        mycontext.executeQueryAsync(Function.createDelegate(this, this.getsuccessed), Function.createDelegate(this, this.getfailed));
    }
    function getsuccessed()
    {
        var str = "";
        var listsE = myitem.getEnumerator();
        while (listsE.moveNext())
        {
            str += listsE.get_current().get_item("Title") + "(" + listsE.get_current().get_item("Author") + "）：" + listsE.get_current().get_item("Created") + "<br>";
            $("<div class='added-event' data-date='" + listsE.get_current().get_item("Created") + "' data-time='11:12' data-title='" + listsE.get_current().get_item("Title") + "'></div>").appendTo("#myDiv");
            myFunction("added-event", listsE.get_current().get_item("Created"), listsE.get_current().get_item("Created"), listsE.get_current().get_item("Title"), listsE.get_current().get_item("Author"));
        }
        document.getElementById("lists").innerHTML = str;
    }
    function getfailed(sender, args)
    {
        alert("failed~!");
    }
    function myFunction(css,date,time,title,author) {
        var node = document.createElement("div");
        node.setAttribute("class",css);
        node.setAttribute("data-date", date);
        node.setAttribute("data-time", time);
        node.setAttribute("data-title", title);
        node.innerHTML = title;
        document.getElementById("myDiv").appendChild(node);
    }
    //var ddiv=function(data-date,data-time,data-title){
    //    var ip = document.createElement("div");
    //    ip.data-date = data-date;
    //    ip.data-time = data-time;
    //    ip.data-title=data-title;
    //    return ip;
    //};
</script>
<article>
    <div id="myId3" class="jalendar mid">
        <div class="added-event" data-date="28/11/2016" data-time="11:12" data-title="欢迎访问我的博客"></div>
        <div class="added-event" data-date="16/11/2016" data-time="20:45" data-title="欢迎访问我的博客"></div>
        <div class="added-event" data-date="17/11/2016" data-time="21:00" data-title="欢迎访问我的博客"></div>
        <div class="added-event" data-date="17/11/2016" data-time="22:00" data-title="欢迎访问我的博客"></div>
        <div class="added-event" data-date="19/11/2016" data-time="22:00" data-title="欢迎访问我的博客"></div>
    </div>
</article>
<input id="Button1" type="button" value="button" onclick="getlistitem()"/>
<div id="lists">

</div>
<div id="myDiv"></div>
