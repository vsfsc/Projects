using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Collections.Generic;
using System.Text;

namespace VADocumentOnline.OfficeOnshow
{
    
    /// <summary>
    /// 将用户上传的文档，或文档链接在线显示，必须是网站内的文档，外面的无法显示
    /// </summary>
    public class OfficeOnshow : SPItemEventReceiver
    {
        private const string _dlPath = @"C:\excel\";
        /// <summary>
        /// 已添加项.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            ReflexContent(properties);
        }

        /// <summary>
        /// 已更新项.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            ReflexContent(properties);
        }
        //Attachments0 ,附件链接地址,首先存在这个字段，否则不进行操作
        //由原来的附件更改为对正文进行操作
        private void ReflexContent(SPItemEventProperties properties)
        {
            string fldName = "Body";
            string srcContent;
            
            if (properties.ListItem.Fields.ContainsFieldWithStaticName(fldName))
            {
                string attachContent = "<hr/>附件下载：<div>";
                string attachString = properties.ListItem[fldName].ToString();//原字符串
                List<string> docsUrl = GetHrefString(attachString, properties);
                //add
                StringBuilder builder = new StringBuilder();
                builder.Append(attachContent);
                int i = 1;
                foreach (string docUrl in docsUrl)
                {
                    string[] urls = docUrl.Split(';');
                    //在线浏览
                    srcContent = "<div><iframe width=\"600\" height = \"400\" src =\"" + properties.Site.RootWeb.Url + "webUrl/_layouts/15/WopiFrame.aspx?sourcedoc=docUrl&amp;action=embedview\" frameborder = \"0\" ></iframe></div>";
                    srcContent = srcContent.Replace("webUrl", urls[1]);
                    srcContent = srcContent.Replace("docUrl", urls[2]);
                    attachString = attachString.Replace(urls[0], srcContent);
                    //download
                    string fileName = urls[2].Substring(urls[2].LastIndexOf('/') + 1 );
                    builder.Append(i.ToString()+".");
                    builder.Append("<a href=\"/_layouts/15/VADocumentOnline/download.aspx?docUrl="+ urls[2] + "\">" + fileName.Substring(0, fileName.LastIndexOf('.')) + "</a><br/>");
                    i++;
                }
                if (docsUrl.Count >0)
                {
                    builder.Append("<hr/></div>");
                    attachString = attachString + builder.ToString();
                    properties.ListItem[fldName] = attachString;
                    this.EventFiringEnabled = false;
                    properties.ListItem.SystemUpdate();
                    this.EventFiringEnabled = true;
                }
            }
        }

        /// <summary>
        /// 获取包含文档的超链接字符串<a href=,</a>
        /// <param name="attachContent">附件html内容</param>
        /// <param name="properties">项事件实体</param>
        ///返回超链接字符串，weburl,文档url
        private List<string> GetHrefString(string attachContent, SPItemEventProperties properties)
        {
            string[] docTypes = new string[] { ".doc",".docx",".ppt",".pptx",".xls",".xlsx",".pdf" };
            List<string> hrefString = new List<string>();
            int i = 0;
            int j = 0;
            int k = 0;
            string fString;
            string webUrl;
            string docUrl;    
            bool isExist = false;
            while (i!=-1)
            {
                i = attachContent.IndexOf("<a href=", i>0?i+1:i);
                if (i == -1) break;
                j = attachContent.IndexOf("</a>", i);
                if (i>-1)//找超链接，如果存在
                {
                    fString = attachContent.Substring(i, j - i + 4);//获取到</a>,所以要加4
                    isExist = false;
                    foreach (string type in docTypes )
                    {
                        if (fString.Contains (type ) && !fString.Contains("15/VADocumentOnline/download.aspx?docUrl=") )
                        {
                            isExist = true;
                            break;

                        }
                    }
                    //指定类型的文档名称存在，则找子网站
                    //<a href = "/Projects/VAExtension/WorksShow/WorksFile/作品秀设计文案.docx" > 作品秀设计文档 </ a >
                    if (isExist )
                    {
                        //j = fString.IndexOf("/");
                        //k = fString.LastIndexOf("/", fString.IndexOf (">")-1);
                        webUrl = GetWebUrl(fString, properties);
                        //    fString.Substring (j,k-j);///blog/Documents获取文档库所在的网址<a href=\"/blog/Documents/初审名单.xlsx\">
                        //webUrl = webUrl.Substring(0, webUrl.LastIndexOf ("/"));///blog去掉文档库部分
                        j = fString.IndexOf("\"") + 1;
                        k = fString.IndexOf("\"",j);
                        docUrl = fString.Substring(j,k-j) ;//开头两个分号中间的部分就是文档链接地址
                        if (webUrl!="")
                            hrefString.Add(fString+";"+webUrl+";"+docUrl  );
                    }
                }
            }
            return hrefString;
        }

        /// <param name="fString">todo: describe fString parameter on GetWebUrl</param>
        /// <param name="properties">todo: describe properties parameter on GetWebUrl</param>
        /// /blog/PublishingImages/default/初审名单.xlsx
        /// 文档多个级别,从后住前找，逐级别进行判断
        private string GetWebUrl(string fString, SPItemEventProperties properties)
        {
            string webUrl="";
            string tmpString = fString;
            int j = 0;
            int k = 0;
            j = fString.IndexOf("/");
            k = fString.LastIndexOf("/", fString.IndexOf(">") - 1);
            while (k > j)
            {
                webUrl = tmpString.Substring(j, k - j);///blog/Documents获取文档库所在的网址<a href=\"/blog/Documents/初审名单.xlsx\">
                //webUrl = webUrl.Substring(0, webUrl.LastIndexOf("/"));///blog去掉文档库部分
                if (WebExits(properties, webUrl))
                    break;
                tmpString = webUrl;
                j = tmpString.IndexOf("/");
                k = tmpString.LastIndexOf("/");
            }
            return webUrl;
        }
       // 文档库所在的网站是否在va网站中，外面的网站不能解析
        private bool WebExits(SPItemEventProperties properties,string webUrl)
        {
            SPSite mySite = properties.Site;
            SPWeb myWeb = mySite.AllWebs[webUrl];
            return myWeb.Exists ? true : false;
        }
    }
}