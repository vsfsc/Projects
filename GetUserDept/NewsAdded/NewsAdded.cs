using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.IO;
using System.Text;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;

namespace GetUserDept.NewsAdded
{
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class NewsAdded : SPItemEventReceiver
    {
        /// <summary>
        /// 已添加项.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                
                if (properties.List.Fields.ContainsField("所属系部"))
                {
                    string loginName = properties.Web.CurrentUser.LoginName;
                    loginName = loginName.Substring(loginName.IndexOf("|") + 1).ToLower();
                    properties.ListItem["所属系部"] = GetUserDept(loginName);
                    properties.ListItem.Update();
                }
                
            });
            base.ItemAdded(properties);
        }


        private void writeNewsDept(SPItemEventProperties properties)
        {           
            //更新当前事件中的新闻
            using (SPWeb oWebsite = new SPSite(properties.SiteId).OpenWeb())
            {
                string loginName = oWebsite.CurrentUser.LoginName;
                loginName = loginName.Substring(loginName.IndexOf("|") + 1).ToLower();

                oWebsite.AllowUnsafeUpdates = true;
                SPListItemCollection collItems = oWebsite.Lists[properties.ListTitle].Items;
                SPListItem thisItem = collItems.GetItemById(properties.ListItemId);
                thisItem["所属系部"] = GetUserDept(loginName);
                thisItem.Update();
            }
        }
        public static string GetUserDept(string loginName)
        {
            string dept="";
            DirectoryEntry de = ADHelper.GetDirectoryEntryByAccount(loginName);
            DirectorySearcher ds = new DirectorySearcher(de);
            ds.Filter = ("(SAMAccountName=" + loginName + ")");
            SearchResult dss = ds.FindOne();
            if (dss != null)
            {
                string dpath = dss.Path;
                dpath = dpath.Substring(dpath.IndexOf("OU="));
                dept = dpath.Substring(0, dpath.IndexOf(","));
            }
            return dept;
        }

    }
}