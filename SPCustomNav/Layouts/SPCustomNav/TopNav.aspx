<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TopNav.aspx.cs" Inherits="SPCustomNav.Layouts.SPCustomNav.TopNav" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table border="0" width="100%" cellspacing="0" cellpadding="0">
        <wssuc:InputFormSection ID="InputFormSection" Title="顶部导航" runat="server" Description="顶部导航">
            <Template_InputFormControls>
                <wssuc:InputFormControl ID="InputFormControl" runat="server">
                    <Template_Control>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="right" style="padding-bottom: 5px">
                                    <a href="javascript:topnav_showFullScreen('<%= txtNavXml.ClientID %>', '<%= txtNavXmlPop.ClientID %>');"
                                        style="color: #0072bc;">
                                        <asp:Literal ID="ltl" runat="server" Text="全屏编辑" />
                                    </a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNavXml" runat="server" Wrap="false" TextMode="MultiLine" Rows="22"
                                        Width="600px" />
                                </td>
                            </tr>
                        </table>
                    </Template_Control>
                </wssuc:InputFormControl>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <wssuc:ButtonSection ID="bts" runat="server" ShowStandardCancelButton="false">
            <Template_Buttons>
                <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="保存" ID="btnSave" OnClick="btnSave_OnClick" />
                <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="取消" ID="btnClose" OnClientClick="TopNav_Close_Click(); return false;" />
            </Template_Buttons>
        </wssuc:ButtonSection>
    </table>
    <table id="tblFullScreen" border="0" cellpadding="0" cellspacing="0" class="tblPop">
        <tr>
            <td align="center" valign="top">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNavXmlPop" runat="server" Wrap="false" TextMode="MultiLine" Rows="28"
                                Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <a href="javascript:topnav_hideFullScreen('<%= txtNavXml.ClientID %>', '<%= txtNavXmlPop.ClientID %>');"
                                style="color: #0072bc;">
                                <asp:Literal ID="ltlClose" runat="server" Text="关闭" />
                            </a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
顶部导航
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
顶部导航
</asp:Content>
