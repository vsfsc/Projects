<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="WorksShow.Layouts.WorksShow.test" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
	<script src="gallery/js/jquery.js?v=1.83.min"></script>
	<script src="gallery/js/layer.min.js"></script>
	<style type="text/css">
		.box {
			padding: 20px;
			background-color: #fff;
			margin: 20px 100px;
			border-radius: 5px;
		}
		
		.box a {
			padding-right: 15px;
		}
		
		#about_hide {
			display: none
		}
		
		.layer_text {
			background-color: #fff;
			padding: 20px;
		}
		
		.layer_text p {
			margin-bottom: 10px;
			text-indent: 2em;
			line-height: 23px;
		}
		
		.button {
			display: inline-block;
			*display: inline;
			*zoom: 1;
			line-height: 30px;
			padding: 0 20px;
			background-color: #56B4DC;
			color: #fff;
			font-size: 14px;
			border-radius: 3px;
			cursor: pointer;
			font-weight: normal;
		}
		
		.imgs img {
			width: 300px;
			padding: 0 20px 20px;
		}
	</style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
	<div class="box">
		<div id="imgs" class="imgs">
			<img src="gallery/images/1.jpg" layer-pname="1">
			<img src="gallery/images/2.jpg" layer-pname="2">
			<img src="gallery/images/3.jpg" layer-pname="3">
			<img src="gallery/images/4.jpg" layer-pname="4">
		</div>
	</div>
	<script type="text/javascript">
		;
		! function() {
			layer.use('gallery/extend/layer.ext.js', function() {
				//初始加载即调用，所以需放在ext回调里
				layer.ext = function() {
					layer.photosPage({
						html: '',
						title: '',
						id: 100, //相册id，可选
						parent: '#imgs'
					});
				};
			});
		}();
	</script>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
应用程序页
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
我的应用程序页
</asp:Content>
