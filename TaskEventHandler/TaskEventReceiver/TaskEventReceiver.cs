using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Text;

namespace TaskEventHandler
{
    /// <summary>
    /// 列表项事件(子任务事件对父级任务的影响)
    /// </summary>
    public class TaskEventReceiver : SPItemEventReceiver
    {


        /// <summary>
        /// 已更新项.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            
          string DYNAMIC_CAML_QUERY_GET_CHILD_NODE = "<Where><Eq><FieldRef Name='{0}' /><Value Type='LookupMulti'>{1}</Value></Eq></Where>";
          SPList myTask = properties.OpenWeb().Lists[properties.ListId];
            if (!myTask.Fields.ContainsFieldWithStaticName("ParentTask"))
                return;
            string tastTitle = properties.ListItem["Title"].ToString();
            string parentTitle = "";
            if (properties.ListItem["ParentTask"] != null)
                parentTitle = properties.ListItem["ParentTask"].ToString();
            string nameField = myTask.Fields.GetField("完成百分比").InternalName;
            SPItemEventDataCollection beforeData = properties.BeforeProperties;
            if (beforeData[nameField].ToString() != properties.ListItem["nameField"].ToString())//百分比发生改变
            {
                StringBuilder Query = new StringBuilder();
                SPListItemCollection objItems = null;
                SPQuery objSPQuery;
                objSPQuery = new SPQuery();

                if (parentTitle == "")//父任务
                {
                    if (properties.ListItem["nameField"].ToString() == "100")
                    {
                        try
                        {
                            Query.Append(String.Format(DYNAMIC_CAML_QUERY_GET_CHILD_NODE, "ParentTask", tastTitle));
                            objSPQuery.Query = Query.ToString();
                            objItems = myTask.GetItems(objSPQuery);
                            foreach (SPListItem objItem in objItems)
                            {
                                objItem[nameField] = 100;
                                objItem.Update();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                else//当前为子任务
                {
                    try
                    {
                        int totalFinish = 0;
                        int count = 0;
                        Query.Append(String.Format(DYNAMIC_CAML_QUERY_GET_CHILD_NODE, "ParentTask", parentTitle));
                        objSPQuery.Query = Query.ToString();
                        objItems = myTask.GetItems(objSPQuery);
                        foreach (SPListItem objItem in objItems)
                        {
                            totalFinish += (int)objItem[nameField];
                            count += 1;
                        }
                        totalFinish = totalFinish / count;

                        SPFieldLookupValue pValue = properties.ListItem["ParentTask"] as SPFieldLookupValue;
                        Query = new StringBuilder();
                        Query.Append(string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='int'>{1}</Value></Eq></Where>", "ID", pValue.LookupId));
                        objSPQuery.Query = Query.ToString();
                        objItems = myTask.GetItems(objSPQuery);
                        if (objItems.Count > 0)
                        {
                            objItems[0][nameField] = totalFinish;
                            objItems[0].Update();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

    

    }
}