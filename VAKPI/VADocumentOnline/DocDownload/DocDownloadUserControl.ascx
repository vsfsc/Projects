﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocDownloadUserControl.ascx.cs" Inherits="VADocumentOnline.DocDownload.DocDownloadUserControl" %>
<style type="text/css">
    .Appraise {
    box-shadow: 0 1px 3px #ccc;
    background-color: white;
    background-color: rgba(255, 255, 255, 0.6);
    margin-right: 5px;
    margin-top: 5px;
    width:600px;
}
   
    .Appraise .title a.more {
    font-weight: 400;
    font-size: 12px;
    color: #888;
    vertical-align: baseline;
    float: right;
}

       .FormList {
    }
        .showc{

            }
        .formul{
        list-style:none;
        }
        .formul li{
        line-height:25px;
        padding-bottom:10px;
        }
        .formul li button{
        font-size:14px;
        }
        .att{
            background-color:#cfcfcf;
            color:red;
            font-size:14px;
        }
     </style>
<div id="AppAction" runat="server" visible="true">
     <ul class="formul">
          
            <li >
                 <div runat ="server" id ="divUpload">
                     </div>
            </li>
          
            <li>
                <asp:Label ID="lblMsg" ForeColor="Red" runat="server" Text=""></asp:Label>
            </li>
           

        </ul>
</div>