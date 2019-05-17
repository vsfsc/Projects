using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Text;

namespace CustListEvent.CustTaskEventReceiver
{
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class CustTaskEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// 任务（操作和作品合成任务名称）和日程（人+操作+作品）
        /// </summary>
        /// <param name="properties"></param>

        public override void ItemAdding(SPItemEventProperties properties)
        {
            if (properties.List.Fields.ContainsField("操作") && properties.List.Fields.ContainsField("作品"))
            {
                SPItemEventDataCollection afterData = properties.AfterProperties;
                SPList myList=properties.OpenWeb().Lists[properties.ListId ];
                string nameFieldOper=myList.Fields.GetField("操作").InternalName;
                string nameFieldWorks=myList.Fields.GetField("作品").InternalName;

                string newValue = afterData[nameFieldOper].ToString() + afterData[nameFieldWorks];
                properties.AfterProperties.ChangedProperties.Add("Title", newValue);

            }
        }
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            SPList myList = properties.List;
        }
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            //合并字段
            if (properties.List.Fields.ContainsField("操作") && properties.List.Fields.ContainsField("作品"))
            {
                if (properties.ListItem["操作"] != null && properties.ListItem["作品"] != null)
                {
                    string title= "" + properties.ListItem["操作"] + properties.ListItem["作品"];
                    using (SPWeb oWebsite = new SPSite(properties.SiteId).OpenWeb(properties.RelativeWebUrl))
                    {
                        oWebsite.AllowUnsafeUpdates = true;
                        SPListItemCollection collItems = oWebsite.Lists[properties.ListTitle].Items;
                        SPListItem item = collItems.GetItemById(properties.ListItemId);
                        item["Title"] = title;
                        item.Update();
                        oWebsite.AllowUnsafeUpdates = false;
                    }
                }
            }
            //更新任务的状态

        }
        /// <summary>
        /// 已更新项.
        /// </summary>
        public  void ItemUpdatedBack(SPItemEventProperties properties)
        {
            string nameField = properties.List.Fields.GetField("完成百分比").InternalName;

            if (properties.ListItem[nameField] == null) return;//百分比发生改变

            SPList oList = properties.List;
            StringBuilder Query = new StringBuilder();
            SPListItemCollection objItems = null;
            SPQuery objSPQuery;
            objSPQuery = new SPQuery();
            string DYNAMIC_CAML_QUERY_GET_CHILD_NODE = "<Where><Eq><FieldRef Name='{0}' /><Value Type='LookupMulti'>{1}</Value></Eq></Where>";

            if (properties.ListItem["ParentID"] == null)
            {// parent level父级完成，则更新所有子集
                if (properties.ListItem[nameField].ToString() == "1")
                {
                    Query.Append(String.Format(DYNAMIC_CAML_QUERY_GET_CHILD_NODE, "ParentID", properties.ListItemId.ToString()));
                    objSPQuery.Query = Query.ToString();
                    objItems = oList.GetItems(objSPQuery);
                    this.EventFiringEnabled = false;
                    foreach (SPListItem objItem in objItems)
                    {
                        if (objItem[nameField] == null || objItem[nameField].ToString() != "1")
                        {
                            objItem[nameField] = 1;
                            objItem.Update();
                        }
                    }
                    this.EventFiringEnabled = true;
                }
            }
            else
            {//sublevel子集更新，子集相加的和更新父级
                float totalFinish = 0;
                float taskFinish = 0;
                int count = 0;
                string parentID = properties.ListItem["ParentID"].ToString().Substring(0, properties.ListItem["ParentID"].ToString().IndexOf(";"));
                Query.Append(String.Format(DYNAMIC_CAML_QUERY_GET_CHILD_NODE, "ParentID", parentID));//不能带ID
                objSPQuery.Query = Query.ToString();
                objItems = oList.GetItems(objSPQuery);
                foreach (SPListItem objItem in objItems)
                {
                    if (objItem[nameField] != null)
                    {
                        taskFinish = float.Parse(objItem[nameField].ToString());
                        if (taskFinish > 0)
                        {
                            totalFinish += taskFinish;
                            count += 1;
                        }
                    }
                }
                if (count > 0)
                    totalFinish = totalFinish / float.Parse(count.ToString());

                Query = new StringBuilder();
                Query.Append(string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='int'>{1}</Value></Eq></Where>", "ID", int.Parse(parentID)));
                objSPQuery = new SPQuery();
                objSPQuery.Query = Query.ToString();
                objItems = oList.GetItems(objSPQuery);
                this.EventFiringEnabled = false;
                foreach (SPListItem objItem in objItems)
                {
                    if (objItem[nameField] == null || float.Parse(objItem[nameField].ToString()) != totalFinish)
                    {
                        objItem[nameField] = totalFinish;
                        objItem.Update();
                    }
                }
                this.EventFiringEnabled = true;
            }
        }
    
    }
}