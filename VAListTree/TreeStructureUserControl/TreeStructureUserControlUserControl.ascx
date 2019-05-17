<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreeStructureUserControlUserControl.ascx.cs" Inherits="VAListTree.TreeStructureUserControl.TreeStructureUserControlUserControl" %>
<style type="text/css">
    .TreeView  
{
    border-bottom:1px dotted #B2B2B2 !important;
    background-color:#E5E5E5;
}

.TreeView div
{
    margin-left:5px;
}

.TreeView table
{
    border-top:1px dotted #B2B2B2 !important;
}

.TreeView div table
{
    border-bottom:none !important;
    border-top:none !important;
}

.TreeView table td
{
    padding:2px 0;
}

.LeafNodesStyle 
{
    
}


.RootNodeStyle 
{
    
}

/* ALL ELEMENTS */
.NodeStyle 
{

}

.ParentNodeStyle 
{
    background:yellow;
}
/*四个不同状态的任务节点样式*/
    .Finished{
        color:green;
    }
    .Ongoing{
        color:blue;
    }
    .Starting{
        color:yellow;
    }
    .Delayed{
        color:red;
    }
    .normalNode{
        color:black;
    }
a.SelectedNodeStyle 
{
    background:#E5E5E5;
    display:block;
    padding:2px 0 2px 3px;
}

</style>
 <asp:TreeView ID="treeViewCategories" runat="server" ShowLines="True" ExpandDepth="0" CssClass="TreeView">
    <ParentNodeStyle Font-Bold="False" />
    <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px" />
</asp:TreeView>
