using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.Social;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint.Utilities;

namespace VAKPI.ActivityKPI
{
    [ToolboxItemAttribute(false)]
    public partial class ActivityKPI : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ActivityKPI()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        private void StatisticList(SPUser logUser)
        {
            string[] lstName = ListName.Split(';');

            SPQuery oQuery;
            SPList sList;
            int[] itmCounts = new int[4];
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.AllWebs[SPContext.Current.Web.ID])
                    {
                        //设置sharepoint时间格式
                        SPTimeZone timeZone = web.RegionalSettings.TimeZone;
                        foreach (string mList in lstName)
                        {
                            try
                            {
                                sList = web.Lists.TryGetList(mList);
                                oQuery = new SPQuery();
                                oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                oQuery.Query = "<Where><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq></Where>";
                                SPListItemCollection lstItems = sList.GetItems(oQuery);
                                itmCounts[0] = lstItems.Count;//个人
                                itmCounts[1] = sList.ItemCount;//全部
                                oQuery = new SPQuery();
                                DateTime currentDate = DateTime.Now;
                                DateTime qDate = currentDate.AddDays(-1);
                                DateTime cDate = timeZone.LocalTimeToUTC(currentDate);
                                string dt = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(cDate.ToString()));

                                oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                oQuery.Query = "<Where><And><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + dt + "</Value></Geq><And/></Where>";
                                lstItems = sList.GetItems(oQuery);
                                itmCounts[2] = lstItems.Count;//当日更新

                                qDate = currentDate.AddDays(-7);
                                cDate = timeZone.LocalTimeToUTC(currentDate);
                                dt = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(cDate.ToString()));

                                oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                oQuery.Query = "<Where><And><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + dt + "</Value></Geq><And/></Where>";
                                lstItems = sList.GetItems(oQuery);
                                itmCounts[3] = lstItems.Count;//本周更新

                                this.Controls.Add(new LiteralControl(mList + "<br/>"));
                                this.Controls.Add(new LiteralControl("个人总数 : " + itmCounts[0].ToString() + "<br/>"));
                                this.Controls.Add(new LiteralControl("团队总数 : " + itmCounts[1].ToString() + "<br/>"));
                                this.Controls.Add(new LiteralControl("当日更新 : " + itmCounts[2].ToString() + "<br/>"));
                                this.Controls.Add(new LiteralControl("本周总数 : " + itmCounts[3].ToString() + "<br/>"));
                            }
                            catch
                            { }
                        }
                    }
                }
            }
                );
        }
        #region 属性
        string listName = "新闻公告;文档;网站页面;页面;讨论列表";
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
        #endregion
    }
}
