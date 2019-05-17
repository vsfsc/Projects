using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL
{
    public class SP
    {
        /// <summary>
        /// 从SharePoint列表获取数据
        /// </summary>
        /// <param name="listName">列表名称</param>
        /// <param name="query">Caml查询语句</param>
        /// <returns></returns>
        public SPListItemCollection GetDataFromList(string listName, string query)
        {
            SPListItemCollection items = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {

                using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
                {
                    using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList spList = spWeb.Lists.TryGetList(listName);

                        if (spList != null)
                        {
                            if (query != "")
                            {
                                SPQuery qry = new SPQuery()
                                {
                                    Query=query
                                };
                                items = spList.GetItems(qry);
                            }
                            else
                            {
                                items = spList.GetItems();
                            }
                        }
                        else
                        {
                            items = null;
                        }
                    }
                }

            });
            return items;
        }
    }
}
