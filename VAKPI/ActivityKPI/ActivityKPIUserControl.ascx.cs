using Microsoft.Office.Server.Social;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Data;
using System.Web.UI;

namespace VAKPI.ActivityKPI
{
    public partial class ActivityKPIUserControl : UserControl
    {
        public ActivityKPI WebPartObj { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            SPUser loguser = SPContext.Current.Web.CurrentUser;
            StatisticList(loguser);
        }
        private void StatisticList(SPUser logUser)
        {
            string lstNames = WebPartObj.ListName;
            if (lstNames=="")
            {
                kpiDiv.InnerHtml = "尚未指定任何列表名称进行数据统计!";
            }
            else
            {
                string[] lstName = WebPartObj.ListName.Split(';');
                SPQuery oQuery;
                SPList sList;
                int[] itmCounts = new int[6];
                DataTable datatable = newTable();
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.AllWebs[SPContext.Current.Web.ID])
                        {
                            string myac = "";
                            string lName = "";
                            //设置sharepoint时间格式
                            SPTimeZone timeZone = web.RegionalSettings.TimeZone;
                            string listsHtml = "<fieldset style='border: 1px dotted #ff4500; padding: 5px;'><legend style='text-align:center;background-color: #ff4500; color:#f5fffa;padding:5px'>活动量化明细表</legend><table class='mytable'>";
                            listsHtml += "<tr><th rowspan='2'>KPI</th><th rowspan='2'>由我发布</th><th colspan='2'>今日更新</th><th colspan='2'>本周更新</th><th rowspan='2'>站内总数</th></tr>";
                            listsHtml += "<tr><td>本人</td><td>本站</td><td>本人</td><td>本站</td></tr>";
                            itmCounts = NewsCount;//微博
                            listsHtml += "<tr>";
                            listsHtml += "<th><a href='" + web.Url + "/newsfeed.aspx' target='_blank'>微 博</a></th>";
                            listsHtml += "<td>" + itmCounts[0].ToString() + "</td>";
                            listsHtml += "<td>" + itmCounts[2].ToString() + "</td>";
                            listsHtml += "<td>" + itmCounts[3].ToString() + "</td>";
                            listsHtml += "<td>" + itmCounts[4].ToString() + "</td>";
                            listsHtml += "<td>" + itmCounts[5].ToString() + "</td>";
                            listsHtml += "<td>" + itmCounts[1].ToString() + "</td>";
                            listsHtml += "</tr>";
                            datatable.Rows.Add("微 博", itmCounts[0], itmCounts[1]);
                            if (itmCounts[4] == 0)
                            {
                                if (myac != "")
                                {
                                    myac += "、";
                                }
                                myac += "“<b><a href='"+SPContext.Current.Web.Url+ "/newsfeed.aspx' target='_blank'>微 博</a></b>”";
                            }
                        
                            foreach (string mList in lstName)
                            {
                            
                                try
                                {
                                    if (mList == "Posts" && WebPartObj.SubWebUrl != "")//统计备忘录
                                    {
                                        SPWeb subWeb = web.Webs[WebPartObj.SubWebUrl];
                                        sList = subWeb.Lists.TryGetList(mList);                                 

                                    }
                                    else
                                    {
                                        sList = web.Lists.TryGetList(mList);                                    
                                    }
                                    lName = "<a href='"+sList.DefaultViewUrl+"' target='_blank'>"+mList+"</a>";

                                
                                    if (mList == "Posts")
                                    {
                                        lName = "<a href='" + sList.DefaultViewUrl + "' target='_blank'>备忘录</a>";
                                    } 
                                
                                    if (mList == "讨论列表")
                                    {
                                        lName = "<a href='" + sList.DefaultViewUrl + "' target='_blank'>讨 论</a>";
                                    } 
                                    if (mList == "文档")
                                    {
                                        lName = "<a href='" + sList.DefaultViewUrl + "' target='_blank'>文 档</a>";
                                    }
                               
                                    oQuery = new SPQuery();
                                    oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                    oQuery.Query = "<Where><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq></Where>";
                                    SPListItemCollection lstItems = sList.GetItems(oQuery);
                                    itmCounts[0] = lstItems.Count;//个人
                                    itmCounts[1] = sList.ItemCount;//全部

                                    /***********今日更新******************/
                                    oQuery = new SPQuery();
                                    DateTime currentDate = DateTime.Now;
                                    DateTime yesterdayDate = currentDate.AddDays(-1);
                                    DateTime yesterdayUTCDate = timeZone.LocalTimeToUTC(yesterdayDate);
                                    string yesterdayUTCDateString = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(yesterdayUTCDate.ToString()));
                                    oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                    oQuery.Query = "<Where><And><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + yesterdayUTCDateString + "</Value></Geq></And></Where>";
                                    lstItems = sList.GetItems(oQuery);
                                    itmCounts[2] = lstItems.Count;//个人当日更新

                                    oQuery = new SPQuery();
                                    oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                    oQuery.Query = "<Where><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + yesterdayUTCDateString + "</Value></Geq></Where>";
                                    lstItems = sList.GetItems(oQuery);
                                    itmCounts[3] = lstItems.Count;//站内当日更新

                                    /***********本周更新******************/
                                    DateTime lastWeekDate = currentDate.AddDays(-7);
                                    DateTime lastWeekUTCDate = timeZone.LocalTimeToUTC(lastWeekDate);
                                    string lastWeekUTCDateString = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(lastWeekUTCDate.ToString()));
                                    oQuery = new SPQuery();
                                    oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                    oQuery.Query = "<Where><And><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + lastWeekUTCDateString + "</Value></Geq></And></Where>";
                                    lstItems = sList.GetItems(oQuery);
                                    itmCounts[4] = lstItems.Count;//个人本周更新

                                    oQuery = new SPQuery();
                                    oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                    oQuery.Query = "<Where><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + lastWeekUTCDateString + "</Value></Geq></Where>";
                                    lstItems = sList.GetItems(oQuery);
                                    itmCounts[5] = lstItems.Count;//站内本周更新

                                    listsHtml += "<tr>";
                                    listsHtml += "<th>" + lName + "</th>";
                                    listsHtml += "<td>" + itmCounts[0].ToString() + "</td>";                                
                                    listsHtml += "<td>" + itmCounts[2].ToString() + "</td>";
                                    listsHtml += "<td>" + itmCounts[3].ToString() + "</td>";
                                    listsHtml += "<td>" + itmCounts[4].ToString() + "</td>";
                                    listsHtml += "<td>" + itmCounts[5].ToString() + "</td>";
                                    listsHtml += "<td>" + itmCounts[1].ToString() + "</td>";
                                    listsHtml += "</tr>";
                                    datatable.Rows.Add(lName,itmCounts[0], itmCounts[1]);
                                    if (itmCounts[4]==0)
                                    {
                                        if(myac!="")
                                        {
                                            myac += "、";
                                        }
                                        myac += "“<b>" + lName + "</b>”";
                                    }
                                }
                                catch
                                { }
                            }

                            listsHtml += "</table></fieldset>";

                            if (myac != "")
                            {
                                listsHtml = "<div class='kpidiv'>亲，你好。<br/>系统发现你近一周都没有参与发布过：" + myac+ "，<br/>快快参与站内活动赢取积分赶超其它小伙伴吧！</div>" + listsHtml;
                            }
                            kpiDiv.InnerHtml = listsHtml;
                            if (datatable.Rows.Count>0)
                            {
                                                    

                            }
                        }
                    }
                });
            }
        }

        private DataTable newTable()
        {
            DataTable dt = new DataTable("量化表");
            dt.Columns.Add("活动类型",typeof(string));
            dt.Columns.Add("由我发布", typeof(int));
            dt.Columns.Add("本站总数", typeof(int));
            return dt;
        }

        #region 微博统计       
        
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
            int[] totalTimes = new int[6];

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
                            totalTimes[3] = threads.Length;//团队当日更新
                            i = 0;j = 0;
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
                            totalTimes[2] = i;//个人当日更新

                            socialOptions = new SPSocialFeedOptions();
                            socialOptions.MaxThreadCount = int.MaxValue;
                            socialOptions.NewerThan = DateTime.Now.Date.AddDays(-6).AddHours(-8);//.AddHours(8);
                            feed = feedManager.GetFeedFor(web.Url, socialOptions);

                            threads = feed.Threads;
                            totalTimes[5] = threads.Length;//团队本周更新

                            i = 0; j = 0;
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
                            totalTimes[4] = i;
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
