<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelfQuantifiedUserControl.ascx.cs" Inherits="VAKPI.SelfQuantified.SelfQuantifiedUserControl" %>

<%-- CSS --%>
<link rel="stylesheet" href="../../../../_layouts/15/VAKPI/css/style-tabBlock.css" media="screen" type="text/css" />

<%-- js --%>
  <script src="../../../../_layouts/15/VAKPI/js/index-tabBlock.js"></script>
<script src="../../../../_layouts/15/VAKPI/js/jquery-tabBlock.js"></script>
    <script type="text/javascript">
        var TabBlock = {
            s: {
                animLen: 200
            },

            init: function () {
                TabBlock.bindUIActions();
                TabBlock.hideInactive();
            },

            bindUIActions: function () {
                $('.tabBlock-tabs').on('click', '.tabBlock-tab', function () {
                    TabBlock.switchTab($(this));
                });
            },

            hideInactive: function () {
                var $tabBlocks = $('.tabBlock');

                $tabBlocks.each(function (i) {
                    var
                      $tabBlock = $($tabBlocks[i]),
                      $panes = $tabBlock.find('.tabBlock-pane'),
                      $activeTab = $tabBlock.find('.tabBlock-tab.is-active');

                    $panes.hide();
                    $($panes[$activeTab.index()]).show();
                });
            },

            switchTab: function ($tab) {
                var $context = $tab.closest('.tabBlock');

                if (!$tab.hasClass('is-active')) {
                    $tab.siblings().removeClass('is-active');
                    $tab.addClass('is-active');

                    TabBlock.showPane($tab.index(), $context);
                }
            },

            showPane: function (i, $context) {
                var $panes = $context.find('.tabBlock-pane');

                // Normally I'd frown at using jQuery over CSS animations, but we can't transition between unspecified variable heights, right? If you know a better way, I'd love a read it in the comments or on Twitter @johndjameson
                $panes.slideUp(TabBlock.s.animLen);
                $($panes[i]).slideDown(TabBlock.s.animLen);
            }
        };

        $(function () {
            TabBlock.init();
        });
    </script>
<%-- Html --%>
<div style="margin-left:20px;">
    <h2>个人活动记录达标分析</h2>
    <asp:RadioButtonList ID="rbPeriods" runat="server" CellPadding="5" CellSpacing="5" Height="30px" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbPeriods_SelectedIndexChanged" AutoPostBack="true">
        <asp:ListItem Value="0" Selected="True">七天</asp:ListItem>
        <asp:ListItem Value="1">一个月</asp:ListItem>
        <asp:ListItem Value="2">三个月</asp:ListItem>
        <asp:ListItem Value="3">半年</asp:ListItem>
        <asp:ListItem Value="4">一年</asp:ListItem>
        <%--<asp:ListItem Value="5">所有</asp:ListItem>--%>
    </asp:RadioButtonList>
    <div id="divAnaContent" runat="server">
      <figure class="tabBlock">
        <ul class="tabBlock-tabs">
          <li class="tabBlock-tab is-active">雷达图</li>
          <li class="tabBlock-tab">元数据</li>
            <li class="tabBlock-tab">系统结论</li>
        </ul>
        <div class="tabBlock-content">
          <div class="tabBlock-pane">
                <asp:Chart ID="chartAnalysis" runat="server" Width="400">
                    <Series>
                        <asp:Series Name="sr0" ChartType="Radar"></asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                    </ChartAreas>
                    <Legends>
                        <asp:Legend Alignment="Center" Docking="Bottom"></asp:Legend>
                    </Legends>
                </asp:Chart>
          </div>
          <div class="tabBlock-pane">
            <asp:GridView ID="gvWeekAnalysis" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="400">
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
          </div>
          <div class="tabBlock-pane">
            <asp:Label ID="lbComments" runat="server" Text=""></asp:Label>
          </div>
        </div>
      </figure>
    </div>

    <div id="divWaring" runat="server" style="padding:5px;line-height:25px;color:red;"></div>
    <div style="padding:5px;line-height:25px;color:red;" id="divErr" runat="server"></div>

  </div>
