using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using Microsoft.SharePoint;

using Microsoft.SharePoint.Linq;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

namespace VAWebParts.ListItemNav
{
    public partial class ListItemNavUserControl : UserControl
    {
        private int ItemID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    return int.Parse(Request.QueryString["ID"]);
                }
                else
                {
                    return 0;
                }
            }
        }

        public ListItemNav WebPartObj { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string listTitle = "新闻公告"; //WebPartObj.ListName;
            
            GetItemIndex(listTitle);           
        }
        private void GetItemIndex(string ListName)
        {
            if (ItemID!=0)
            {
                string myurl = Request.Url.AbsoluteUri;
                int id = myurl.IndexOf("?ID=");
                id = id + 4;
                myurl = myurl.Substring(0, id);
                id = myurl.IndexOf("Lists");
                string siteUrl = myurl.Substring(0, id - 1);
                //string ItemId = ItemID.ToString();
                try
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()//模拟管理员权限执行，让匿名用户也可以查看此Web部件
                    {
                        
                        using (SPSite mySite = new SPSite(siteUrl))
                        {
                            using (SPWeb myWeb = mySite.OpenWeb())
                            {
                                SPList mylist = myWeb.Lists.TryGetList(ListName);
                                if (mylist != null)
                                {
                                    SPQuery qry = new SPQuery();
                                    qry.Query = @"<OrderBy><FieldRef Name='Created' Ascending='FALSE' /></OrderBy>";
                                    SPListItemCollection myItems = mylist.GetItems(qry);//获取sharepoint列表集合
                                                                                        //SPListItem oListItem = mylist.Items.GetItemById(ItemID);
                                    List<SPListItem> listItems = myItems.Cast<SPListItem>().ToList();//将SharePoint列表数据集合转化为普通列表集合
                                    SPListItem myItem = listItems.FirstOrDefault(p => p.ID == ItemID);//查询指定ID的列表项
                                    int index = listItems.IndexOf(myItem);//获取指定列表项的索引
                                    int rCount = mylist.ItemCount;//获取列表的计数
                                    if (rCount >= 2)//多余两条显示导航
                                    {
                                        if (index == 0)
                                        {
                                            //itemindex = myItems[rCount - 1]["ID"].ToString() + ";" + myItems[rCount - 1]["Title"].ToString() + ";"  + myItems[i+1]["ID"].ToString() + ";" + myItems[i+1]["Title"].ToString();
                                            itemNav.InnerHtml = "<div id='container'><div id='left'> 当前已是第一条 </div><div id='content'> 共计 <b>" + rCount + "</b> 条 </div><div id='right'> 下一条：<a href='" + myurl + myItems[index + 1]["ID"].ToString() + "'> <b>" + myItems[index + 1]["Title"].ToString() + "</b> </a></div></div>";
                                        }
                                        else if (index == rCount - 1)
                                        {
                                            //itemindex = myItems[i - 1]["ID"].ToString() + ";" + myItems[i - 1]["Title"].ToString() + ";" + myItems[0]["ID"].ToString() + ";" + myItems[0]["Title"].ToString();
                                            itemNav.InnerHtml = "<div id='container'><div id='left'>上一条：<a href='" + myurl + myItems[index - 1]["ID"].ToString() + "'> <b>" + myItems[index - 1]["Title"].ToString() + "</b> </a></div><div id='content'> 共计 <b>" + rCount + "</b> 条 </div><div id='right'> 当前已是最后一条 </div></div>";
                                        }
                                        else
                                        {
                                            //itemindex = myItems[i - 1]["ID"].ToString() + ";" + myItems[i-1]["Title"].ToString()+";"+ myItems[i + 1]["ID"].ToString() + ";" + myItems[i+1]["Title"].ToString();
                                            itemNav.InnerHtml = "<div id='container'><div id='left'>上一条：<a href='" + myurl + myItems[index - 1]["ID"].ToString() + "'><b>" + "  " + myItems[index - 1]["Title"].ToString() + "</b> </a></div><div id='content'> 当前第 <b>" + (index + 1).ToString() + "</b> 条，共计 <b>" + rCount + "</b> 条 </div><div id='right'> 下一条：<a href='" + myurl + myItems[index + 1]["ID"].ToString() + "'> <b>" + myItems[index + 1]["Title"].ToString() + "</b> </a></div></div>";
                                        }
                                    }
                                    else
                                    {
                                        itemNav.Visible = false;
                                    }
                                }
                            }
                        }

                    });
                }
                catch (System.Exception ex)
                {
                    itemNav.InnerHtml =ex.ToString();
                }
                
            }
            
        }

        #region 列表与数据表转化

        /// <summary>
        /// 列表转化数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        /// <summary>
        /// 数据表转化列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        /// <summary>
        /// 数据行转化列表项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// 创建数据行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {  //You can log something here     
                       //throw;    
                    }
                }
            }

            return obj;
        }
        #endregion

    }
}
