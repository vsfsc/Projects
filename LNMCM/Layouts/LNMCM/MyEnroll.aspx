<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyEnroll.aspx.cs" Inherits="LNMCM.Layouts.LNMCM.MyEnroll" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
        <style>
    	.linkbtn {
			height: 25px;
			line-height: 25px;
			padding: 0 11px;
			background: #e4e4e4;
			border: 1px #26bbdb solid;
			border-radius: 3px;
			/*color: #fff;*/
			display: inline-block;
			text-decoration: none;
			font-size: 12px;
			outline: none;
			cursor: pointer;
		}

		.ch_cls {
			background: #e4e4e4;
		}

		fieldset {
			padding:10px;
			margin:10px;
			color:#333;
			border:#06c solid 1px;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            -khtml-border-radius: 5px;
            border-radius: 5px;
            line-height: 25px;
            list-style: none;
            width:100%;
            max-width:800px;
            min-width:500px;
		}
		legend {
			color:#06c;
			font-weight:800;
			background:#fff;
			border:#b6b6b6 solid 1px;
			padding:5px 5px;
            line-height:25px;
		}
    </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="divInfo" runat="server">
    	<div id="divHead" style="text-align: right;margin-right: 20px;">
            <asp:Button ID="btnCancelEnroll" runat="server" Text="取消报名" visible="false"></asp:Button>
    		<%--<a href="/LNMCM/_layouts/15/ChangePassword/ChangePassword.aspx?Resource=/LNMCM/" class="linkbtn">修改密码</a>--%>
    	</div>
    	<fieldset id="fsEnrollInfo">
    		<legend>报名信息</legend>
    		<div id="divEInfo" runat="server" style="line-height:30px;">
    			<h2>报名序号：10145001</h2>
    			<h3>培养单位：东北大学</h3>
    			<h3>报名时间：2018-9-6 12:00</h3>
    		</div>
    	</fieldset>
    	<p></p>
    	<fieldset id="fsMembers">
    		<legend>团队成员</legend>
            <div style="text-align:right;margin-right:10px">
                <asp:LinkButton ID="lnkNewMember" runat="server" CssClass="linkbtn">新增成员</asp:LinkButton>
            </div>
            <asp:GridView ID="gvMembers" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ID" OnRowCommand="gvMembers_RowCommand" AllowPaging="false" CellPadding="1" cellspacing="3" ForeColor="#333333" GridLines="None" >
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" Visible="false" />
                    <asp:TemplateField  HeaderText="序 号" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lbindex" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Number" HeaderText="学 号" ReadOnly="true" />
                    <asp:BoundField DataField="Name" HeaderText="姓 名" ReadOnly="true" />
                    <asp:BoundField DataField="Sex" HeaderText="性 别" ReadOnly="true" />
                    <asp:BoundField DataField="Mobile" HeaderText="手 机" ReadOnly="true" />
                    <asp:BoundField DataField="Major" HeaderText="专 业" ReadOnly="true" />
                    <asp:BoundField DataField="Created" HeaderText="加入时间" ReadOnly="true" />

                    <asp:TemplateField  HeaderText="删 除"  ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:LinkButton  ID="lkbdelete" runat="server" CommandName="Del" Style="font-size: 12px;" CommandArgument='<%#Eval("ID")%>' OnClientClick="return confirm('确定删除该成员吗？删除后将不可恢复！');">删除</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField  DataNavigateUrlFields="ID,EnrollCode" DataNavigateUrlFormatString="MemberInfo.aspx?ID={0}&amp;Code={1}&amp;Source=/LNMCM/_layouts/15/lnmcm/MyEnroll.aspx" HeaderText="修 改" Text="修改"  />
                </Columns>

                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#CADAF5" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />

            </asp:GridView>
    	</fieldset>
    	<p></p>
    	<fieldset id="fsWorks" style="display:none" >
    		<legend>我的作品</legend>


    	</fieldset>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
报名信息
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
报名信息
</asp:Content>
