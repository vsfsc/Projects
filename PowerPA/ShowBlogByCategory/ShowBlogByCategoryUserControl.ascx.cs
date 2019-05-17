using Microsoft.SharePoint;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PowerPA.ShowBlogByCategory
{
    public partial class ShowBlogByCategoryUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                ReadContents();
        }
        #region 属性
        public ShowBlogByCategory webObj { get; set; }
        #endregion
        #region 方法
        private void ReadContents()
        {
            if (Request.QueryString["CategoryId"] != null)
            {
                int id = int.Parse(Request.QueryString["CategoryId"].ToString());
                SPWeb web = SPContext.Current.Web;
                SPList blogList = web.Lists.TryGetList(webObj.BlogList);
                if (blogList == null)
                {
                    lblMsg.Text = "列表不存在！";
                }
                SPQuery qry = new SPQuery();
                qry.Query = @"<Where><Eq><FieldRef Name='PostCategory' LookupId='True' /><Value Type='LookupMulti'>" + id + "</Value></Eq></Where><OrderBy><FieldRef Name='Title' /></OrderBy>";
                SPListItemCollection items = blogList.GetItems(qry);
                if (items.Count > 0)
                {
                    string itemUrl = blogList.DefaultViewUrl.Replace("AllPosts.aspx", "Post.aspx");
                    string editUrl = blogList.DefaultViewUrl.Replace("AllPosts.aspx", "EditPost.aspx");
                    FillContent(items, itemUrl, editUrl);
                }
                else
                    divBlogContent.InnerHtml = "<div class=\"ms-metadata\">此类别中没有" + webObj.BlogList + "。</div>";
            }
        }
        private void FillContent(SPListItemCollection items, string blogItemUrl,string blogEditUrl)
        {
            StringBuilder txt = new StringBuilder();
            txt.AppendLine("<ul class=\"ms-blog-postList\">");
            string css = "ms-blog-postBody";
            foreach (SPListItem item in items)
            {
                txt.AppendLine("<li><div><h2 class=\"ms-h1\"><a href=\"" + blogItemUrl + "?ID=" + item.ID.ToString() + "\"");
                txt.Append(" class=\"ms-accentText\">" + item["Title"] + "</a></h2><div class=\"ms-metadata\">" + ((DateTime)item["Created"]).ToString("yyyy年M月d日") + "</div>");
                if (item["Body"] != null)
                {
                    txt.AppendLine(@"<div class=\" + css + "\"><div dir=\"\" class=\"ms-rtestate-field\"><div class=\"");
                    txt.Append("ExternalClassCA0D5B2C96EB4A1A9F27E03AE68EC396\"><p>" + item["Body"].ToString() + "</p></div></div></div>");
                }
                SPFieldLookupValue user = new SPFieldLookupValue(item["Author"].ToString ());
                txt.AppendLine("<div class=\"ms-metadata ms-textSmall\"><span class=\"ms-blog-command-noLeftPadding ms-comm-metalineItemSeparator\">作者: <span class=\"ms-noWrap ms-imnSpan\"><span class=\"ms-spimn-presenceLink\"><span class=\"ms-hide\"><img class=\"ms-hide\" name=\"imnempty\" src=\"/_layouts/15/images/spimn.png?rev=23\" alt=\"\"></span></span><a class=\"ms-subtleLink\" onclick=\"GoToLinkOrDialogNewWindow(this);return false;\" href=\"/blog/VSBlogs/_layouts/15/userdisp.aspx?ID="+user.LookupId+"\">"+user.LookupValue+"</a></span>；时间: "+((DateTime)item["Created"]).ToShortTimeString( )+"</span><span class=\"ms-blog-command\"><a href=\""+blogEditUrl+"?ID="+item.ID  +"\">编辑</a></span>");

                txt.AppendLine("</div></div><div class=ms-blog-postDivider></div></li>");
            }
            txt.AppendLine("</ul>");
            divBlogContent.InnerHtml = txt.ToString();
        }
        #endregion
    }
}
