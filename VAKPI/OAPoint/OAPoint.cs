using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.Social;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint.Utilities;

namespace VAKPI.OAPoint
{
    [ToolboxItemAttribute(false)]
    public class OAPoint : WebPart
    {
        #region 事件
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/VAKPI/OAPoint/OAPointUserControl.ascx";

        protected override void CreateChildControls()
        {
            //注册JS文件

            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "js0001", "/_layouts/STYLES/corechart.js");
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "js0002", "/_layouts/STYLES/jquery.ba-resize.min.js");
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "js0003", "/_layouts/STYLES/jquery.gvChart-1.0.1.min.js");
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "js0004", "/_layouts/STYLES/jquery.js");
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "js0005", "/_layouts/STYLES/jsapi.js");
            //注册CSS文件

            CssRegistration cssControls = new CssRegistration();
            cssControls.Name = "/_layouts/STYLES/myStyle.css";
            Page.Header.Controls.Add(cssControls);

            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
            //test
            
            int[] result = StatisticAllList(ListName, SubWebUrl);
            int points = 0;
            for (int i = 0; i < result.Length; i++)
            {
                points += result[i];
            }
            this.Controls.Add(new LiteralControl("<div><span style='width:120px;font-weight:bold;'>活动总积分：</span>" + points.ToString() + "</div>"));
            this.Controls.Add(new LiteralControl("<ul>"));
            this.Controls.Add(new LiteralControl("<li><span style='width:120px;font-weight:bold;'>" + "微博：</span>" + result[0].ToString() + "</li>"));

            string[] lstName = ListName.Split(';');

            for (int i = 0; i < lstName.Length; i++)
            {
                string lname = lstName[i];
                if (lname=="Posts")
                {
                    lname = "备忘录";
                }
                this.Controls.Add(new LiteralControl("<li><span style='width:120px;font-weight:bold;'>" + lname + "：</span>" + result[i + 1].ToString() + "</li>"));
            }
            this.Controls.Add(new LiteralControl("</ul>"));
            
        }

        #endregion
        #region 方法
        /// <summary>
        /// 获取用户账号
        /// </summary>
        /// <returns></returns>
        private string GetAccountName()
        {
            string loginName;
            if (Page.Request.QueryString["accountname"] == null)
            {
                loginName = this.Context.User.Identity.Name;// loginUser.LoginName;
                loginName = loginName.Substring(loginName.IndexOf("|") + 1).ToLower();
            }
            else
                loginName = Page.Request.QueryString["accountname"];
            return loginName;
        }
        /// <summary>
        /// 获取总的新闻源
        /// </summary>
        /// <returns></returns>
        private int GetTotalNewsFeeds()
        {
            SPSocialFeedOptions socialOptions = new SPSocialFeedOptions();
            socialOptions.MaxThreadCount = int.MaxValue;
            int i = 0;

            try
            {
                string UserName = GetAccountName();
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.AllWebs[SPContext.Current.Web.ID])
                        {
                            SPServiceContext serviceContext = SPServiceContext.GetContext(site);
                            UserProfileManager upm = new UserProfileManager(serviceContext);
                            
                            UserProfile u = upm.GetUserProfile(UserName);
                            SPSocialFeedManager feedManager = new SPSocialFeedManager(u, serviceContext);
                            SPSocialFeed feed = feedManager.GetFeedFor(web.Url, socialOptions);
                            SPSocialThread[] threads = feed.Threads;
                            foreach (SPSocialThread thread in threads)
                            {
                                if (thread.Attributes.ToString() != "None")
                                {
                                    string actorAccount;
                                    if (thread.Actors.Length == 2)
                                        actorAccount = thread.Actors[1].AccountName;
                                    else
                                        actorAccount = thread.Actors[0].AccountName;
                                    if (actorAccount.ToLower() == UserName.ToLower())
                                        i = i + 1;
                                }
                            }

                            EnumerateNewsfeeds(ref i, web, feedManager, socialOptions, UserName);
                        }
                    }
                });
            }
            catch
            {

            }
            return i;
        }
        /// <summary>
        /// 遍历当前网站下面子网站的新闻源
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pWeb"></param>
        private void EnumerateNewsfeeds(ref int totalCount, SPWeb pWeb, SPSocialFeedManager feedManager, SPSocialFeedOptions socialOptions, string accountName)
        {
            foreach (SPWeb subWeb in pWeb.Webs)
            {
                SPSocialFeed feed = feedManager.GetFeedFor(subWeb.Url, socialOptions);
                SPSocialThread[] threads = feed.Threads;
                int i = 0;
                foreach (SPSocialThread thread in threads)
                {
                    if (thread.Attributes.ToString() != "None")
                    {
                        string actorAccount;
                        if (thread.Actors.Length == 2)
                            actorAccount = thread.Actors[1].AccountName;
                        else
                            actorAccount = thread.Actors[0].AccountName;
                        if (actorAccount.ToLower() == accountName.ToLower())
                            i = i + 1;
                    }
                }
                totalCount = totalCount + i;
                EnumerateNewsfeeds(ref totalCount, subWeb, feedManager, socialOptions, accountName);
            }

        }
        /// <summary>
        /// 遍历子网站下的列表,如果用户名为空，则返回所有的
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pWeb"></param>
        /// <param name="mList"></param>
        /// <param name="accountName"></param>
        private void EnumerateList(ref int totalCount, SPWeb pWeb, string mList, string accountName)
        {
            SPList sList;
            SPQuery oQuery;

            foreach (SPWeb subWeb in pWeb.Webs)
            {
                sList = subWeb.Lists.TryGetList(mList);


                oQuery = new SPQuery();
                oQuery.ViewAttributes = "Scope='RecursiveAll'";
                if (accountName != "")
                    oQuery.Query = "<Where><Eq><FieldRef Name='Author'/><Value Type='Text'>" + accountName + "</Value></Eq></Where>";
                try
                {
                    SPListItemCollection lstItems = sList.GetItems(oQuery);
                    totalCount = totalCount + lstItems.Count;//个人
                }
                catch
                { }
            }

        }

        /// </summary>
        /// <param name="ListName">列表名称连接的字符串</param>
        /// <param name="subWebUrl">子网站Url</param>
        /// <returns>统计的各项活动积分数组</returns>
        private int[] StatisticAllList(string ListName, string subWebUrl)
        {
            SPQuery oQuery;
            SPList sList;
            string[] lstName = ListName.Split(';');
            string[] qr = QR.Split(';');
            string[] ppr = PPR.Split(';');
            int[] itmCounts = new int[lstName.Length + 1];
            int[] scores = new int[lstName.Length + 1];
            SPUser logUser = SPContext.Current.Web.CurrentUser;
            if (logUser==null)//获取所有用户的信息
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.AllWebs[SPContext.Current.Web.ID])
                        {
                            itmCounts[0] = GetTotalNewsFeeds();//微博
                            scores[0] = itmCounts[0] * int.Parse(qr[0]) * int.Parse(ppr[0]);
                            int i = 0;
                            int j = 0;
                            foreach (string mList in lstName)
                            {
                                try
                                {
                                    if (mList == "Posts" && subWebUrl != "")//统计备忘录
                                    {
                                        SPWeb subWeb = web.Webs[subWebUrl];
                                        sList = subWeb.Lists.TryGetList(mList);
                                    }
                                    else
                                    {
                                        sList = web.Lists.TryGetList(mList);
                                    }
                                    i = i + 1;
                                    oQuery = new SPQuery();
                                    oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                    //oQuery.Query = "<Where><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq></Where>";
                                    SPListItemCollection lstItems = sList.GetItems(oQuery);
                                    j = lstItems.Count;//个人
                                    EnumerateList(ref j, web, mList,"");
                                    itmCounts[i] = j;
                                    scores[i] = itmCounts[i] * int.Parse(qr[i]) * int.Parse(ppr[i]);
                                }
                                catch
                                { }
                            }
                        }
                    }
                });
            }
            else
            {                
                //int i = 1;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.AllWebs[SPContext.Current.Web.ID])
                        {
                            itmCounts[0] = GetTotalNewsFeeds();//微博
                            scores[0] = itmCounts[0] * int.Parse(qr[0]) * int.Parse(ppr[0]);
                            int i = 0;
                            int j = 0;
                            foreach (string mList in lstName)
                            {
                                try
                                {
                                    if (mList == "Posts" && subWebUrl != "")//统计备忘录
                                    {
                                        SPWeb subWeb = web.Webs[subWebUrl];
                                        sList = subWeb.Lists.TryGetList(mList);
                                    }
                                    else
                                    {
                                        sList = web.Lists.TryGetList(mList);
                                    }
                                    i = i + 1;
                                    oQuery = new SPQuery();
                                    oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                    oQuery.Query = "<Where><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq></Where>";
                                    SPListItemCollection lstItems = sList.GetItems(oQuery);
                                    j = lstItems.Count;//个人
                                    EnumerateList(ref j, web, mList, logUser.Name);
                                    itmCounts[i] = j;
                                    scores[i] = itmCounts[i] * int.Parse(qr[i]) * int.Parse(ppr[i]);
                                }
                                catch
                                { }
                            }
                        }
                    }
                });
            }
            
            return scores;
        }
        #endregion
        #region 属性
        /// <summary>
        /// 博客地址
        /// </summary>
        string subWebUrl = "Blogs";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("用来统计博客的内容，参数为空则统计当前网站下面的")]
        [WebDescription("")]
        public string SubWebUrl
        {
            get
            {
                return subWebUrl;
            }
            set
            {
                subWebUrl = value;
            }
        }

        /// <summary>
        /// 被统计活动列表名称字符串
        /// </summary>
        string listName = "文档;页面;Posts;新闻公告;讨论列表;网站页面";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("对当前网站的列表统计，各列表之间用分号隔开")]
        [WebDescription("")]
        public string ListName
        {
            get
            {
                return listName;
            }
            set
            {
                listName = value;
            }
        }

        /// <summary>
        /// 活动质量系数QualityRatio，缺省为1
        /// </summary>
        string qr = "1;1;1;1;1;1";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("活动质量系数")]
        [WebDescription("对当前网站的各统计列表的活动计算活动质量系数，各系数之间用分号隔开")]
        public string QR
        {
            get
            {
                return qr;
            }
            set
            {
                qr = value;
            }
        }
        /// <summary>
        /// 周期分值系数PeriodPointRatio，缺省不变
        /// </summary>
        string ppr = "1;1;1;1;1;1";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("活动所处周期分值系数")]
        [WebDescription("根据活动所处周期，指定该活动的分值系数，各系数之间用分号隔开")]
        public string PPR
        {
            get
            {
                return ppr;
            }
            set
            {
                ppr = value;
            }
        }
        #endregion
    }
}
