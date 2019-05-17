<%@ Assembly Name="WorksShowDll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9242a88229ae4d9c" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" Inherits="WorksShowDll.Inherits.WorksList" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link rel="stylesheet" type="text/css" href="ListView/css/default.css" />
    <link rel="stylesheet" type="text/css" href="ListView/css/component.css" />
    <script src="ListView/js/modernizr.custom.js" type="text/javascript"></script>
    <style type="text/css">
        .shaixuan {
            background-color: #d3d3d3;
        }
        
        .shaixuan ul {
            margin-top: 20px;
            list-style: none;
        }
        
        .shaixuan li {
            float: left;
        }
    </style>
    <%--<script type="text/javascript">
    $(function () {
        $(".cbp-vm-view-grid .cbp-vm-details").each(function (i) {
            var divH = $(this).height();
            var $p = $("a", $(this)).eq(0);
            while ($p.outerHeight() > divH) {
                $p.text($p.text().replace(/(\s)*([a-zA-Z0-9]+|\W)(\.\.\.)?$/, "..."));
            };
        });
        });
    </script>--%>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <div class="container">
        <div class="main">
            <div class="shaixuan">
                <ul>
                    <li>
                        <asp:LinkButton ID="lnkAll" runat="server" width="100px">所有作品</asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkMy" runat="server" width="100px">我的作品</asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkNews" runat="server" width="80px">最 新</asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkHots" runat="server" width="80px">热 门</asp:LinkButton>
                    </li>
                    <li>
                        选择网站：<asp:DropDownList ID="ddlWebSites" runat="server" AutoPostBack="True" width="200"></asp:DropDownList>
                    </li>
                    <li>
                        选择类别：<asp:DropDownList ID="ddlWorksTypes" runat="server" AutoPostBack="True" width="200"></asp:DropDownList>
                    </li>
                </ul>
            </div>
            <div id="cbp-vm" class="cbp-vm-switcher cbp-vm-view-grid">

                <div class="cbp-vm-options">
                    <a href="#" class="cbp-vm-icon cbp-vm-grid cbp-vm-selected" data-view="cbp-vm-view-grid" title="网格">网格</a>
                    <a href="#" class="cbp-vm-icon cbp-vm-list" data-view="cbp-vm-view-list" title="列表">列表</a>
                </div>
                <div id="worksListDiv" runat="server">
                   未有任何作品哦！
                </div>
                

            </div>
            <div>
                <asp:LinkButton ID="lnkFirst" runat="server">首页</asp:LinkButton>
                <asp:LinkButton ID="lnkPre" runat="server">上一页</asp:LinkButton>
                <asp:Label ID="lbPages" runat="server" Text="1/1"></asp:Label>
                <asp:LinkButton ID="lnkNext" runat="server">下一页</asp:LinkButton>
                <asp:LinkButton ID="lnkLast" runat="server">尾页</asp:LinkButton>
                <asp:HiddenField ID="hfCurrentPage" runat="server" />
                <asp:HiddenField ID="hfItemCount" runat="server" />
            </div>
        </div>
        <!-- /main -->
    </div>
    <!-- /container -->
    <script src="ListView/js/classie.js" type="text/javascript"></script>
    <script src="ListView/js/cbpViewModeSwitch.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    作品列表
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    <asp:Label ID="lbPageTitle" runat="server" Text="作品列表"></asp:Label>
</asp:Content>