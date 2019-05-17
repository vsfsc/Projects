<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrentActivityUserControl.ascx.cs" Inherits="PowerPA.CurrentActivity.CurrentActivityUserControl" %>


<asp:UpdatePanel ID="upCurrentActivity" runat="server">
    <ContentTemplate>

        <style type="text/css">
            .myCu {
                font-size: 12px;
                background-color: #FFF19D;
                height: 25px;
                line-height: 25px;
            }

                .myCu ul {
                    text-decoration: none;
                    vertical-align: middle;
                }

                    .myCu ul li {
                        text-decoration: none;
                        float: left;
                        margin-right: 20px;
                        padding-right: 10px;
                    }

                        .myCu ul li:hover {
                            color: #ff0000;
                            cursor: pointer;
                        }
        </style>
        <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick">
        </asp:Timer>


        <div id="divC" class="myCu" runat="server">
            <marquee id="affiche" align="absmiddle" behavior="scroll" bgcolor="#FFF19D" direction="left" width="100%" hspace="5" vspace="10" loop="-1" scrollamount="3" scrolldelay="5" onmouseout="this.start()" onmouseover="this.stop()">
                        <ul>
					        <li>前一计划</li>
					        <li>当前计划</li>
					        <li>下一计划</li>
				        </ul>
                    </marquee>

        </div>

    </ContentTemplate>
</asp:UpdatePanel>
