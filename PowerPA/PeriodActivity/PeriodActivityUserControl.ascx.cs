using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PowerPA.PeriodActivity
{
    public partial class PeriodActivityUserControl : UserControl
    {
        public PeriodActivity webObj;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

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
                   if (!string.IsNullOrEmpty(webObj.WebUrl))
                   {
                       siteUrl = webObj.WebUrl;
                   }
                   using (SPSite spSite = new SPSite(siteUrl)) //找到网站集
                   {

                       using (SPWeb spWeb = spSite.OpenWeb())
                       {
                           string planList = webObj.ListName;
                           SPList spList = spWeb.Lists.TryGetList(planList);
                           if (spList != null)
                           {
                               string tempStr = "";
                               DateTime dtNow = DateTime.Now;
                               string nowStr = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtNow);
                               SPQuery qry1 = new SPQuery();
                               qry1.RowLimit =1;//只选择一行
                               qry1.Query = @" <Where><And><Leq><FieldRef Name='PlanDate' /><Value Type='DateTime'  IncludeTimeValue='TRUE'>" + nowStr + "</Value></Leq><Eq><FieldRef Name = 'Author' LookupId = 'TRUE'></FieldRef><Value Type = 'User'>" + userId + "</Value></Eq></And></Where><OrderBy><FieldRef Name='PlanDate' Ascending = 'FALSE'/><FieldRef Name='PlanDate' Ascending ='FALSE'/></OrderBy>";//获取当前用户最近开始的一条活动
                               DataTable tb1 = spList.GetItems(qry1).GetDataTable();
                               if (tb1!=null)
                               {

                               }
                               else
                               {
                                   tempStr = "<li>无任何计划安排</li>";
                               }

                           }
                       }
                   }
               }
               catch (Exception ex)
               {

                   divC.InnerHtml = ex.ToString();
               }
           });
        }
    }
}
