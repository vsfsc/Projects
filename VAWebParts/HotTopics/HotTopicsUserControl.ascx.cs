using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace VAWebParts.HotTopics
{
    public partial class HotTopicsUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetDiscussions();
            //mychanges.InnerHtml = "<dl>" + GetChanges() + "</dl>";
        }
        private void GetDiscussions()
        {
            string myul = "<fieldset style='border: 1px dotted #ff4500; padding: 5px;'>";
            myul += "<legend style='text-align:center;background-color: #ff4500; color:#f5fffa;padding:5px'>讨论KPI</legend>";
            myul += "<ul class='activiesUl'>";
            string myli = "";
            string todays = "";
            SPSite mysite = SPContext.Current.Site;
            SPWeb rootWeb = mysite.RootWeb;
            SPWeb web = SPContext.Current.Web;
            SPList spList = web.Lists.TryGetList("讨论列表");
            if (spList != null)
            {
                string author = GetAuthor();
                SPQuery qry = new SPQuery();

                //1、由我发布
                qry.Query = @"<Where><Eq><FieldRef Name = 'Author' /><Value Type = 'Integer'><UserID /></Value></Eq></Where>";
                SPListItemCollection myItems = spList.GetItems(qry);
                myli += "<li>由我发布：" + myItems.Count + "</li>";
                                
                //2、为我点赞
                int likes = 0;
                foreach (SPListItem item in myItems)
                {
                    if (item["LikesCount"] != null)
                    {
                        likes += int.Parse(item["LikesCount"].ToString());
                    }
                }
                myli += "<li>为我点赞：" + likes + "</li>";

                //3、今日更新
                DateTime dt = DateTime.Now;
                SPTimeZone timeZone = web.RegionalSettings.TimeZone;
                dt = timeZone.LocalTimeToUTC(dt);
                string dtToday = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(dt.ToString()));
                qry = new SPQuery();
                qry.ViewAttributes = "";
                qry.Query = "<OrderBy><FieldRef Name='Created' Ascending='FALSE' /></OrderBy><Where><Geq><FieldRef Name='Created' /><Value Type='DateTime'>" + dtToday + "</Value></Geq></Where>";//筛选近24小时内发布的记录
                SPListItemCollection todayItems = spList.GetItems(qry);
                myli += "<li>今日更新：" + todayItems.Count+"</li>";

                //4、本周更新
                qry = new SPQuery();
                dt = dt.AddDays(-7);
                string dtWeek = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(dt.ToString()));
                qry.Query = "<Where><Geq><FieldRef Name = 'Created'/><Value Type = 'DateTime' IncludeTimeValue = 'TRUE'>" + dtWeek + "</Value></Geq></Where>";//筛选近7天内发布的记录
                SPListItemCollection WeekItems = spList.GetItems(qry);                
                myli += "<li>本周更新：<span>" + WeekItems.Count + "</span></li>";

                //5、主题总数
                qry = new SPQuery();
                qry.Query = @"<OrderBy><FieldRef Name='Created' Ascending='FALSE' /></OrderBy>";
                SPListItemCollection allItems = spList.GetItems(qry);
                myli += "<li>主题总数：" + allItems.Count + "</li>";
                myul += myli + "</ul>";
                
                if (todayItems.Count > 0)
                {
                    todays += "<fieldset style='border: 1px dotted #ff4500; padding: 5px;'>";
                    todays += "<legend style='text-align:center;background-color: #ff4500; color:#f5fffa;padding:5px'>今日主题</legend>";

                    todays += "<ul>";
                    foreach (SPListItem todayItem in todayItems)
                    {
                        //string iUrl=todayItem.ServerRedirectedPreviewUrl;
                        //所有赞：myItem["DescendantLikesCount"]；主题点赞： myItem["LikesCount"] 
                        todays += "<li style='margin-left:10px;padding-bottom:10px;word-break:break-all;word-wrap:break-word;'><a href='" + rootWeb.Url + todayItem["FileRef"] + "' target='_bank'>" + todayItem["Title"] + "</a>";
                        if (todayItem["LikesCount"]!=null)
                        {
                            todays += "&nbsp;&nbsp;&nbsp;&nbsp;<img src= '../_layouts/15/images/like.11x11x32.png'title = '赞数目' /> <b>" + todayItem["LikesCount"]+"</b>";
                        }
                        todays +=  "</li>";
                    }
                    todays += "</ul>";
                    todays += "</fieldset>";
                }
                else
                {
                    todays += "<dl><dt style='color:red;'>今日无主题更新!</dt></dl>";
                }
            }
            discussions.InnerHtml= myul+ "</fieldset>";
            mychanges.InnerHtml = todays;
        }
        /// <summary>
        /// 获取用户登录账号
        /// </summary>
        /// <returns></returns>
        private static string[] GetUserName(SPWeb web)
        {
            string[] username = new string[2];
            SPUser us = web.CurrentUser;
            if (us != null)
            {
                var accountName = us.LoginName;
                if (accountName.Length > accountName.LastIndexOf("\\", StringComparison.Ordinal) + 1)
                    accountName = accountName.Substring(accountName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                var displayName = us.Name;
                username[0] = accountName;
                username[1] = displayName;
            }
            return username;

        }

        private string GetAuthor()
        {
            string author = null;
            string rootFolder=null;
            if (Page.Request.QueryString["RootFolder"] != null)
            {
                rootFolder = Page.Request.QueryString["RootFolder"];
                int index = rootFolder.LastIndexOf("/", StringComparison.Ordinal);
                rootFolder = rootFolder.Substring(index + 1);
            }
            if (rootFolder!=null)
            {
                SPWeb spWeb = SPContext.Current.Web;
                SPList spList = spWeb.Lists.TryGetList("讨论列表");
                if (spList != null)
                {
                    SPQuery qry = new SPQuery();
                    qry.Query =
                       @"   <Where>
                          <Eq>
                             <FieldRef Name='Title' />
                             <Value Type='Text'>"+rootFolder+@"</Value>
                          </Eq>
                       </Where>";
                    qry.ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='Author' />";
                    SPListItemCollection listItems = spList.GetItems(qry);
                    if (listItems.Count>0)
                    {
                        SPListItem listItem = listItems[0];
                        author = listItem["Author"].ToString();
                    }
                }
            }
            return author;
        }

        //public static string GetChanges()
        //{
        //    string changesString = "";
        //    using (SPSite site = new SPSite("http://localhost"))
        //    {
        //        using (SPWeb web = site.RootWeb)
        //        {
        //            // Construct a query.
        //            SPChangeQuery query = new SPChangeQuery(true, true);

        //            SPTimeZone timeZone = web.RegionalSettings.TimeZone;
        //            long total = 0;

        //            // Get changes in batches.
        //            while (true)
        //            {
        //                // Fetch a set of changes.
        //                SPChangeCollection changes = site.ContentDatabase.GetChanges(query);
        //                total += changes.Count;

        //                // Write info about each change to the console.
        //                foreach (SPChange change in changes)
        //                {
        //                    changesString += "<dt><ul>";
        //                    // Print the date of the change.
        //                    changesString+="<li>"+ $"变更日期: {timeZone.UTCToLocalTime(change.Time)}"+"</li>";

        //                    // Print the ID of the site where the change took place.
        //                    changesString += "<li>" + $"SiteID: {change.SiteId.ToString("B")}" + "</li>";

        //                    // Print the type of object that was changed.
        //                    //   GetType().Name returns SPChangeItem, SPChangeList, etc.
        //                    //   Remove the "SPChange" part of the name.
        //                    string objType = change.GetType().Name.Replace("SPChange", null);
        //                    changesString += "<li>" + $"变更项目: {objType}" + "</li>";

        //                    // Print the nature of the change.
        //                    changesString += "<li>" + $"变更类型: {change.ChangeType}" + "</li>";
        //                    changesString += "</ul></dt>";
        //                }

        //                // Break out of loop if we have the last batch
        //                if (changes.Count < query.FetchLimit)
        //                    break;
        //                // Otherwise, go get another batch
        //                query.ChangeTokenStart = changes.LastChangeToken;
        //            }
        //            changesString += "<li>" + $"变更总计数: {total}" + "</li>";
        //        }
        //    }
            
        //    return changesString;
        //}
    }
}
