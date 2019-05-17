using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace MyTasks.GetMyAction
{
    public partial class GetMyActionUserControl : UserControl
    {
        public GetMyAction wpObj { get; set; } 
        protected void Page_Load(object sender, EventArgs e)
        {
            AddChoice(wpObj.SubWebUrl,wpObj.FromList, wpObj.ToList);
        }

        /// <summary>
        /// 查询一个列表，将其某一列数据作为另一个列表的选项列的选项
        /// </summary>
        /// <param name="subWebUrl">列表所在子网站</param>
        /// <param name="fromList">选项来源列表</param>
        /// <param name="ToList">写入选项的列表</param>
        private static void AddChoice(string subWebUrl, string fromList,string ToList)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
               {
                   using (SPSite site =new SPSite ( SPContext.Current.Site.ID ))
                   {
                       using (SPWeb spWeb = site.OpenWeb(subWebUrl))
                       {
                           SPList list = spWeb.Lists[ToList];
                           SPFieldChoice fieldchoice = (SPFieldChoice)list.Fields["操作"];
                           fieldchoice.Choices.Clear();
                           SPListItemCollection splistItems = GetSPItems(fromList, subWebUrl);
                           if (splistItems != null)
                           {
                               spWeb.AllowUnsafeUpdates = true;
                               foreach (SPListItem item in splistItems)
                               {
                                   string Action = item["操作"].ToString();//此时的数据格式为：“ID;#查阅字段值”，需要做拆分处理
                                   Action = Action.Split('#')[1];
                                   fieldchoice.AddChoice(Action);
                               }
                               fieldchoice.Update();
                           }
                       }
                   }
               });
              
            }
            catch (Exception)
            {
                
            }

        }

        private static SPListItemCollection GetSPItems(string fromList,string subWebUrl)
        {
            try
            {
                using (SPSite site = SPContext.Current.Site)
                {
                    using (SPWeb spWeb = site.OpenWeb(subWebUrl))
                    {
                        SPList spList = spWeb.Lists.TryGetList(fromList);
                        if (spList != null)
                        {
                            SPQuery qry = new SPQuery
                            {
                                Query = @"<Where>
                              <Eq>
                                 <FieldRef Name='User' />
                                 <Value Type='Integer'>
                                    <UserID />
                                 </Value>
                              </Eq>
                           </Where>
                           <OrderBy>
                              <FieldRef Name='Frequency' Ascending='FALSE' />
                           </OrderBy>",
                                ViewFields = @"<FieldRef Name='ActionID' />"
                            };
                            SPListItemCollection listItems = spList.GetItems(qry);
                            return listItems;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            //SPList spList = SPContext.Current.Web.Lists["年份"];
            //SPListItemCollection splistItems = spList.GetItems();
            //return splistItems;
        }
    }
}
