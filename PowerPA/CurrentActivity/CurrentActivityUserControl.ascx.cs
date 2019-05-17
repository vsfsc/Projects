using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Data;
using System.Timers;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PowerPA.CurrentActivity
{
    public partial class CurrentActivityUserControl : UserControl
    {
        public CurrentActivity webObj;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cUrl = Page.Request.Url.ToString();
            SPUser spUser = SPContext.Current.Web.CurrentUser;
            if (!cUrl.Contains("_layouts/15")&&spUser != null)
            {
                Timer1.Interval = 1000 * webObj.Seconds;
                int userId = spUser.ID;
                divC.Visible = true;
                GetCurrentActivity(userId);

            }
            else
            {
                divC.Visible = false;
            }
        }

        #region 方法


        /// <summary>
        /// 个人当前活动记录数据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
         private void GetCurrentActivity(int userId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
           {
               try
               {
                   string siteUrl = SPContext.Current.Site.Url;
                   if (!string.IsNullOrEmpty(webObj.SiteUrl))
                   {
                       siteUrl = webObj.SiteUrl;
                   }
                   using (SPSite spSite = new SPSite(siteUrl)) //找到网站集
                   {

                       using (SPWeb spWeb = spSite.OpenWeb())
                       {
                           string planList = webObj.ListName;
                           SPList spList = spWeb.Lists.TryGetList(planList);
                           if (spList != null)
                           {
                               string ihtml = "";
                               if (webObj.IsScroll=="是")
                               {
                                   ihtml += "<marquee id='affiche' align='absmiddle' behavior='scroll' bgcolor='#FFF19D' direction='left' height='20' width='100%' hspace='5' vspace='10' loop='-1' scrollamount='3' scrolldelay='5' onMouseOut='this.start()' onMouseOver='this.stop()'>";
                               }
                               ihtml += "<ul>";
                               string tempStr = "";
                               DateTime dtNow = DateTime.Now;
                               string nowStr = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtNow);
                               SPQuery qry1 = new SPQuery();
                               qry1.RowLimit =1;//只选择一行
                               qry1.Query = @" <Where><And><Leq><FieldRef Name='PlanDate' /><Value Type='DateTime'  IncludeTimeValue='TRUE'>" + nowStr + "</Value></Leq><Eq><FieldRef Name = 'Author' LookupId = 'TRUE'></FieldRef><Value Type = 'User'>" + userId + "</Value></Eq></And></Where><OrderBy><FieldRef Name='PlanDate' Ascending = 'FALSE'/><FieldRef Name='PlanDate' Ascending ='FALSE'/></OrderBy>";//获取当前用户最近开始的一条活动
                               DataTable tb1 = spList.GetItems(qry1).GetDataTable();
                               if (tb1!=null)
                               {
                                   if (tb1.Rows.Count>0)
                                   {
                                       if (DBNull.Value != (tb1.Rows[0]["ActivityType"]))
                                       {
                                           tempStr = tb1.Rows[0]["ActivityType"]+"-";
                                       }

                                       DateTime dt = (DateTime)(tb1.Rows[0]["PlanDate"]);//计划开始
                                       double during = double.Parse(tb1.Rows[0]["PlanDuring"].ToString());
                                       dt = dt.AddMinutes(during);
                                       if (dt>dtNow)//当前用户最近开始的一条活动正在进行
                                       {
                                           tempStr = "<li>当前计划：<b>"+tempStr+tb1.Rows[0]["Title"]+"</b>,结束还剩：<b>"+DiffTime(dt,dtNow)+"</b></li>";
                                       }
                                       else//当前用户在这之前开始的最近一条活动已完成
                                       {

                                           tempStr = "<li>上一计划：<b>"+tempStr+tb1.Rows[0]["Title"]+"</b>,时间已过：<b>"+DiffTime(dtNow,dt)+"</b></li><li>当前无计划在执行中</li>";
                                       }
                                   }
                                   else
                                   {
                                       tempStr = "<li>此刻之前无计划安排</li>";
                                   }
                               }
                               else
                               {
                                   tempStr = "<li>无任何计划安排</li>";
                               }
                               ihtml += tempStr;

                               //查找比当前时间晚的计划 大于now
                               SPQuery qry2 = new SPQuery();
                               qry2.RowLimit = 1;
                               qry2.Query = @"<Where><And><Gt><FieldRef Name='PlanDate' /><Value Type='DateTime'  IncludeTimeValue='TRUE'>" + nowStr + "</Value></Gt><Eq><FieldRef Name = 'Author' LookupId = 'TRUE'></FieldRef><Value Type = 'User'>" + userId + "</Value></Eq></And></Where><OrderBy><FieldRef Name='PlanDate'/></OrderBy>";//获取当前用户即将开始的一条活动
                               DataTable tb2 = spList.GetItems(qry2).GetDataTable();
                               if (tb2!=null)
                               {
                                   if (tb2.Rows.Count > 0)
                                   {
                                       tempStr = "";
                                       if (DBNull.Value != (tb2.Rows[0]["ActivityType"]))
                                       {
                                           tempStr = tb2.Rows[0]["ActivityType"] + "-";
                                       }
                                       DateTime dt = (DateTime)(tb2.Rows[0]["PlanDate"]);//计划开始
                                       ihtml += "<li>下一计划：<b>" +tempStr+ tb2.Rows[0]["Title"] + "</b>,开始时间：<b>" + string.Format("{0:g}",dt)+"</b></li>";


                                   }
                                   else
                                   {
                                       ihtml += "<li>尚无下一个计划安排</li>";
                                   }
                               }
                               else
                               {
                                   if (tempStr!="<li>无任何计划安排！</li>")
                                   {
                                       ihtml += "<li>无下一个计划安排</li>";
                                   }
                               }
                               ihtml += "</ul>";
                               if (webObj.IsScroll == "是")
                               {
                                   ihtml += "</marquee>";
                               }
                               divC.Visible = true;
                               divC.InnerHtml = ihtml;
                           }
                           else
                           {
                               divC.Visible = false;
                           }
                       }
                   }
               }
               catch (Exception ex)
               {
                   divC.Visible = true;
                   divC.InnerHtml = ex.ToString();
               }
           });
        }
        private string DiffTime(DateTime t1,DateTime t2)
        {
            TimeSpan ts = t1.Subtract(t2);
            return ts.Hours.ToString("00")+":" +ts.Minutes.ToString("00")+ ":" +ts.Seconds.ToString("00");
        }



        #endregion

        #region 事件

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = true;
            Timer1.Interval =1000 * webObj.Seconds;
            SPUser spUser = SPContext.Current.Web.CurrentUser;
            if (spUser != null)
            {
                int userId = spUser.ID;
                divC.Visible = true;
                GetCurrentActivity(userId);
            }
            else
            {
                divC.Visible = false;
            }
        }
        #endregion
    }
}
