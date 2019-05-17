using System;
using System.Linq;
using System.Web.UI;
using Microsoft.Office.Server.Social;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;

namespace VAWebParts.UserActivities
{
    public partial class UserActivitiesUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowNewsFeed();
        }

        private void ShowNewsFeed()
        {
            int[] newsCount = NewsCount;
            string ul = "<ul class='activiesUl'>";
            string li = "";
            li += "<li>由我发布：<span style='font-size:25px'>" + newsCount[0] + "</span><hr/></li>";
            li += "<li>微博总计：<span style='font-size:25px'>" + newsCount[1] + "</span><hr/></li>";
            
            li += "<li>近24小时：<span style='font-size:25px'>" + newsCount[2] + "</span><hr/></li>";
            li += "<li>本周更新：<span style='font-size:25px'>" + newsCount[3] + "</span><hr/></li>";
            ul += li;
            ul += "</ul>";
            acounts.InnerHtml = ul;
        }
        private void ShowActivies()
        {
            string ul = "<ul class='activiesUl'>";
            string li = "";
            if (GetMicroBlogs() != null)//微博统计
            {
                li+=GetMicroBlogs().ToString();
            }
            if (GetBlogs() != null)//博客统计
            {
                li+=GetBlogs().ToString();
            }
            if (GetDiscussions() != null)//社区讨论统计
            {
                li+=GetDiscussions();
            }

            if (GetDocs() != null)//文档统计
            {
                li+=GetDocs().ToString();
            }

            if (GetWikis() != null)//Wiki统计
            {
                li+=GetWikis().ToString();
            }
            ul+=li;
            ul += "</ul>";
            acounts.InnerHtml = ul;

        }
        /// <summary>
        /// 获取Wiki统计
        /// </summary>
        /// <returns></returns>
        private object GetWikis()
        {
            return null;
        }

        private object GetDocs()
        {
            return null;
        }

        private string GetDiscussions()
        {
            int count = 0;
            int likes = 0;
            SPWeb web = SPContext.Current.Web;
            string dispName=GetUserName(web)[1];
            SPList spList = web.Lists.TryGetList("讨论列表");
            if (spList != null)
            {
                SPQuery qry = new SPQuery();
                qry.Query =
                @"   <Where>
                          <Eq>
                             <FieldRef Name='Author' />
                             <Value Type='Integer'>
                                <UserID />
                             </Value>
                          </Eq>
                       </Where>";
                qry.ViewAttributes = "Scope=\"RecursiveAll\"";
                SPListItemCollection listItems = spList.GetItems(qry);
                count = listItems.Count;
                for (int i = 0; i < count; i++)
                {
                    likes+=int.Parse(listItems[i]["LikesCount"].ToString());
                }
            }
            return "<li>发帖总数："+count+";获赞总数："+likes+"</li>";
        }

        private object GetBlogs()
        {
            return null;
        }

        private object GetMicroBlogs()
        {
            return null;
        }

        /// <summary>
        /// 获取用户登录账号
        /// </summary>
        /// <returns></returns>
        private static string[] GetUserName(SPWeb web)
        {
            string[] username=new string[2];
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
        #region 属性

        private int[] NewsCount
        {
            get
            {
                if (ViewState["newsCount"] == null)
                {
                    ViewState["newsCount"] = GetPublishdNews();
                }
                return (int[])ViewState["newsCount"];
            }

        }
        #endregion
        #region 方法
        /// <summary>
        ///统计不同时间的个人用户和团队的新闻源
        /// </summary>
        /// MaxThreadCount最大值是100，即最多只能返回100，默认值是20，
        /// <returns>返回一维数组，0-个人总数，1-团队总数，2-当日更新，3-本周更新</returns>
        private int[] GetPublishdNews()
        {
            SPSocialFeedOptions socialOptions = new SPSocialFeedOptions();
            socialOptions.MaxThreadCount = int.MaxValue;
            int i = 0;
            int j = 0;
            int[] totalTimes = new int[4];

            try
            {
                string acountName = GetAccountName();
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        //this.Controls.Add(new LiteralControl("site:" + site.Url + "<br/>"));
                        using (SPWeb web = site.AllWebs[SPContext.Current.Web.ID])
                        {
                            //this.Controls.Add(new LiteralControl("web:"+web.Url + "<br/>"));
                            SPServiceContext serviceContext = SPServiceContext.GetContext(site);
                            UserProfileManager upm = new UserProfileManager(serviceContext);
                            string accountName = GetAccountName();
                            //this.Controls.Add(new LiteralControl("name:" + accountName + "<br/>"));
                            UserProfile u = upm.GetUserProfile(accountName);
                            SPSocialFeedManager feedManager = new SPSocialFeedManager(u, serviceContext);
                            SPSocialFeed feed = feedManager.GetFeedFor(web.Url, socialOptions);
                            SPSocialThread[] threads = feed.Threads;
                            //this.Controls.Add(new LiteralControl("count:" + threads.Length + "<br/>"));
                            foreach (SPSocialThread thread in threads)
                            {
                                //只统计用户发布的新闻源，thread.Attributes.ToString() == "None"表示用户关注了哪些内容，这部分没有统计
                                if (thread.Attributes.ToString() != "None")
                                {
                                    string actorAccount;
                                    
                                    if (thread.Actors.Length == 1)
                                    {
                                        actorAccount = thread.Actors[0].AccountName;
                                    }
                                    else
                                        actorAccount = thread.Actors[1].AccountName;
                                    //string temp = "";
                                    //for (int k = 0; k < thread.Actors.Length; k++)
                                    //{
                                    //    temp += thread.Actors[k].AccountName+" 、 ";
                                    //}
                                    //this.Controls.Add(new LiteralControl("actorlength:" + thread.Actors.Length + "；actorAccount:" + temp + "<br/>"));
                                    //当前用户
                                    if (actorAccount.ToLower() == accountName.ToLower())
                                        i = i + 1;
                                    
                                    j = j + 1;
                                }
                            }
                            totalTimes[0] = i;//个人总数
                            //this.Controls.Add(new LiteralControl("my:" + i + "<br/>"));
                            totalTimes[1] = j;//团队总数
                            //this.Controls.Add(new LiteralControl("all:" + j + "<br/>"));

                            socialOptions = new SPSocialFeedOptions();
                            socialOptions.MaxThreadCount = int.MaxValue;
                            //this.Controls.Add(new LiteralControl("Now:" + DateTime.Now + "<br/>24小时前：" + DateTime.Now.AddHours(-24) + "<br/>一天前：" + DateTime.Now.AddDays(-1)));
                            socialOptions.NewerThan = DateTime.Now.AddHours(-32);//.Date.AddDays(-1).AddHours(8);
                            feed = feedManager.GetFeedFor(web.Url, socialOptions);
                            threads = feed.Threads;
                            totalTimes[2] = threads.Length;//当日更新
                            
                            socialOptions = new SPSocialFeedOptions();
                            socialOptions.MaxThreadCount = int.MaxValue;
                            socialOptions.NewerThan = DateTime.Now.Date.AddDays(-6).AddHours(-8);//.AddHours(8);
                            feed = feedManager.GetFeedFor(web.Url, socialOptions);
                            threads = feed.Threads;
                            totalTimes[3] = threads.Length;//本周更新
                            //this.Controls.Add(new LiteralControl("week:" + threads.Length + "<br/>"));
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                this.Controls.Add(new LiteralControl(ex.Message));
            }
            return totalTimes;
        }
        /// <summary>
        /// 获取当前用户的登录名称，带有域名
        /// </summary>
        /// <returns></returns>
        private string GetAccountName()
        {
            string loginName;
            if (Page.Request.QueryString["accountname"] == null)
            {
                loginName = Context.User.Identity.Name;// loginUser.LoginName;
                loginName = loginName.Substring(loginName.IndexOf("|") + 1).ToLower();
            }
            else
                loginName = Page.Request.QueryString["accountname"];
            return loginName;
        }
        #endregion
    }
}
