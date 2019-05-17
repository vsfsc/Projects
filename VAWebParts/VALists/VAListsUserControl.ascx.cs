using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint.Utilities;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace VAWebParts.VALists
{
    public partial class VAListsUserControl : UserControl
    {
        public VALists WebPartObj { get; set; }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string listTitle = "Statistics";
            if (string.IsNullOrEmpty(WebPartObj.ListTitle) || WebPartObj.ListTitle != "")
            {
                listTitle = WebPartObj.ListTitle;
            }
            else
            {
                aresult.InnerHtml = "列表不存在！";
            }

            string itemType = WebPartObj.GroupField;
            int displayType = WebPartObj.DisplayType;

            aresult.InnerHtml = GetItemCount(listTitle)+GetLists(listTitle);
        }
        # region 输出列表内容

        /// <summary>
        /// 输出列表的最新更新的五条数据
        /// </summary>
        /// <param name="listName">列表名称</param>
        private string GetLists(string listName)
        {
            string ul = "<ul class='Ul'>";
            string li = "";
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite mySite = SPContext.Current.Site)
                {
                    using (SPWeb myWeb = SPContext.Current.Web)
                    {
                        SPList myList = myWeb.Lists.TryGetList(listName);
                        SPQuery myQuery = new SPQuery();
                        myQuery.Query = "<OrderBy><FieldRef Name='Created' Ascending='False' /></OrderBy>";
                        SPListItemCollection myListItemCol;
                        myListItemCol = myList.GetItems(myQuery);
                        int top = 5;//输出前5条
                        if (myListItemCol.Count < 5)
                        {
                            top = myListItemCol.Count;
                        }
                        for (int i = 0; i < top; i++)
                        {
                            li += "<li>" + myListItemCol[i]["Title"].ToString() + "</li>";
                        }                        
                    }
                }
            });
            return ul + li + "</ul>";

        }

        /// <summary>
        /// 获取数据统计
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        private string GetItemCount(string listName)
        {
            string ul = "<ul class='Ul'>";
            string li = "";
            int itemCount = 0;
            using (SPWeb spWeb = SPContext.Current.Web)
            {
                SPList spList = spWeb.Lists.TryGetList(listName);
                if (spList != null)
                {
                    SPQuery qry = new SPQuery();
                    SPListItemCollection listItems;
                    qry.ViewAttributes = "Scope=\"RecursiveAll\"";
                    listItems = spList.GetItems(qry);
                    itemCount = listItems.Count;//所有记录数
                    li += "<li>所有记录：<span>" + itemCount + "</span></li>";

                    qry = new SPQuery();
                    qry.ViewAttributes = "Scope=\"RecursiveAll\"";
                    qry.Query = @"<Where><Eq><FieldRef Name = 'Author' /><Value Type = 'Integer'><UserID /></Value></Eq></Where>";//筛选当前用户发布的记录
                    listItems = spList.GetItems(qry);
                    itemCount = listItems.Count;//由我发布
                    li += "<li>由我发布：<span>" + itemCount + "</span></li>";

                    DateTime dt = DateTime.Now;
                    SPTimeZone timeZone = spWeb.RegionalSettings.TimeZone;
                    dt = timeZone.LocalTimeToUTC(dt);
                    dt= DateTime.Parse(SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(dt.ToShortDateString())));
                    qry = new SPQuery();
                    qry.ViewAttributes = "Scope=\"RecursiveAll\"";
                    qry.Query = "<Where><Geq><FieldRef Name='Created' /><Value Type='DateTime' IncludeTimeValue='TRUE'>"+dt+"</Value></Geq></Where>";//筛选近24小时内发布的记录
   
                    listItems = spList.GetItems(qry);
                    itemCount = listItems.Count;//今日更新
                    li += "<li>今日更新：<span>" + itemCount + "</span></li>";

                    dt = dt.AddDays(-7);
                    qry = new SPQuery();
                    qry.ViewAttributes = "Scope=\"RecursiveAll\"";                    
                    qry.Query = "<Where><Geq><FieldRef Name = 'Created'/><Value Type = 'DateTime' IncludeTimeValue = 'TRUE'>" + dt+ "</Value></Geq></Where>";//筛选近7天内发布的记录
                    listItems = spList.GetItems(qry);
                    itemCount = listItems.Count;//本周更新
                    li += "<li>本周更新：<span>" + itemCount + "</span></li>";                   
                }
            }
            return ul +li+"</ul>";
        }

        //private void linqListData()
        //{
        //    DataContext teamSite = new DataContext(SPContext.Current.Web.Url);
        //    EntityList<Announcement> announcements = teamSite.GetList<Announcement>("Announcements");
        //}
        //[ContentType(Name = "Announcement", Id = "0x0104")]
        //public partial class Announcement
        //{
        //    [Column(Name = "Title", FieldType = "Text")]
        //    public String Title { get; set; }
        //}
        #endregion

        public static bool UserInItemExists(SPWeb web, SPListItem item, string strUserLoginName)
        {
            bool boolResult = false;
            try
            {
                if (item != null)
                {
                    SPRoleAssignmentCollection roles = item.RoleAssignments;
                    foreach (SPRoleAssignment role in roles)
                    {
                        //sbResult.Append(role.Member.Name.ToUpper().Trim());
                        SPUser loginuser = null;
                        try
                        {
                            loginuser = new SPSite(SPContext.Current.Site.Url).RootWeb.Users[strUserLoginName];
                        }
                        catch
                        {
                            loginuser = null;
                        }
                        SPUser memberuser = null;
                        try
                        {
                            memberuser = ((Microsoft.SharePoint.SPUser)(role.Member));
                        }
                        catch
                        {
                            memberuser = null;
                        }
                        if (loginuser != null && memberuser != null)
                        {
                            if (loginuser.Sid == memberuser.Sid)
                            {
                                boolResult = true; break;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return boolResult;
        }
    }
}
