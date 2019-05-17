using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace PlantoActivity.PlanToActivityEvent
{
    struct Activity
    {
        public string name;
        public DateTime start;
        public int during;
        public string aType;
        public string aPosition;
        public string aObject;
        public string aDescption;
        public string aWorksUrl;
        public SPFieldUserValueCollection author;
        public SPFieldLookupValue task;
    };
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class PlanToActivityEvent : SPItemEventReceiver
    {
        /// <summary>
        /// 已更新项.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
           SPList activ = properties.Web.Lists.TryGetList("活动");
            if (activ == null)
            {
                 activ = CreateActivityList(properties.SiteId, properties.Web.ID, "活动", "计划生成活动", properties.List.Title);
            }

            //获取字段内部名
            string nameField = properties.List.Fields.GetField("完成百分比").InternalName;
            string fType = "";
            string fDiDian = "";
            string fObject = "";
            if (properties.List.Fields.ContainsField("活动类型"))
            {
                fType = properties.List.Fields.GetField("活动类型").InternalName;
                fDiDian = properties.List.Fields.GetField("活动地点").InternalName;
                fObject = properties.List.Fields.GetField("活动对象").InternalName;
            }
            string fStart = properties.List.Fields.GetField("开始日期").InternalName;
            string fEnd = properties.List.Fields.GetField("截止日期").InternalName;
            string fAssigned = properties.List.Fields.GetField("分配对象").InternalName;
            string fDesc = properties.List.Fields.GetField("说明").InternalName;
            if (properties.ListItem["ParentID"] != null)
            {// sub level
                if (properties.ListItem[nameField].ToString() == "1")
                {
                    string taskName = properties.ListItem["Title"].ToString();
                    SPFieldLookupValue taskID = new SPFieldLookupValue(properties.ListItem.ID, taskName);

                    Activity myActivity = new Activity();
                    myActivity.name = taskName;
                    if (properties.List.Fields.ContainsField("活动类型"))
                    {
                        myActivity.aType = properties.ListItem[fType].ToString();
                        myActivity.aPosition = properties.ListItem[fDiDian].ToString();
                        myActivity.aObject = properties.ListItem[fObject].ToString();
                    }
                    if (properties.ListItem[fStart] != null)
                    {
                        myActivity.start = DateTime.Parse(properties.ListItem[fStart].ToString());
                        if (properties.ListItem[fEnd] != null)
                        {
                            DateTime end = DateTime.Parse(properties.ListItem[fEnd].ToString());
                            myActivity.during = GetDuring(myActivity.start, end);
                        }
                    }
                    if (properties.ListItem[fAssigned] != null)
                    {
                        SPFieldUserValueCollection assignedTo = (SPFieldUserValueCollection)properties.ListItem[fAssigned];
                        myActivity.author = assignedTo;
                    }
                    myActivity.task = taskID;
                    if (properties.ListItem[fDesc] != null)
                        myActivity.aDescption = properties.ListItem[fDesc].ToString();
                    WriteToList(properties.SiteId, properties.OpenWeb().ID, activ.Title, myActivity);
                }
            }
        }
        
        #region 方法

        /// <summary>
        /// 返回持续时间
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private int GetDuring(DateTime start, DateTime end)
        {
            if (DateTime.Now.Subtract(end).TotalMinutes < 0)
                return (int)DateTime.Now.Subtract(start).TotalMinutes;
            else
                return (int)end.Subtract(start).TotalMinutes;
        }
        /// <summary>
        /// 数据写入活动列表，只写一次.
        /// </summary>
        private void WriteToList(Guid siteID, Guid webID, string lstTitle, Activity myActivity)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPWeb oWebsite = new SPSite(siteID).AllWebs[webID])
                {
                    oWebsite.AllowUnsafeUpdates = true;
                    try
                    {
                        SPList oList = oWebsite.Lists.TryGetList(lstTitle);
                        string fTaskID = oList.Fields.GetField("计划ID").InternalName;
                        if (ActivityIsExits(oList, fTaskID, myActivity.task.LookupValue) == 1) return;

                        string fType = oList.Fields.GetField("活动类型").InternalName;
                        string fDiDian = oList.Fields.GetField("活动地点").InternalName;
                        string fObject = oList.Fields.GetField("活动对象").InternalName;
                        string fStart = oList.Fields.GetField("开始时间").InternalName;
                        string fDuration = oList.Fields.GetField("持续时长").InternalName;
                        string fAuthor = oList.Fields.GetField("执行者").InternalName;
                        string fDesc = oList.Fields.GetField("活动描述").InternalName;

                        SPListItemCollection collItems = oList.Items;
                        SPListItem item = collItems.Add();
                        item["Title"] = myActivity.name;
                        if (myActivity.start.Year != 1)
                            item[fStart] = myActivity.start;
                        item[fDuration] = myActivity.during;
                        item[fDiDian] = myActivity.aPosition;
                        item[fType] = myActivity.aType;
                        item[fObject] = myActivity.aObject;
                        item[fDesc] = myActivity.aDescption;
                        item[fAuthor] = myActivity.author;
                        item[fTaskID] = myActivity.task;
                        item.Update();
                    }
                    catch
                    {
                    }
                    oWebsite.AllowUnsafeUpdates = false;
                }
            });
        }

        /// <summary>
        /// 如果活动列表不存在，自动创建
        /// </summary>
        private  SPList CreateActivityList(Guid siteID, Guid webID, string lstTitle, string desc, string lookupListTitle)
        {
            SPList retList=null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPWeb oWebsite = new SPSite(siteID).AllWebs[webID])
                {
                    oWebsite.AllowUnsafeUpdates = true;
                    try
                    {
                        oWebsite.Lists.Add(lstTitle, desc, SPListTemplateType.GenericList);

                        SPList list = oWebsite.Lists[lstTitle];

                        SPField myField = list.Fields.GetFieldByInternalName("Title");
                        myField.Title = "活动名称";
                        myField.Update();

                        list.Fields.Add("活动类型", SPFieldType.Text, false);
                        list.Fields.Add("活动地点", SPFieldType.Text, false);
                        list.Fields.Add("活动对象", SPFieldType.Text, false);
                        list.Fields.Add("开始时间", SPFieldType.DateTime, false);
                        list.Fields.Add("持续时长", SPFieldType.Integer, false);
                        list.Fields.Add("执行者", SPFieldType.User, false);
                        list.Fields.Add("活动描述", SPFieldType.Text, false);
                        list.Fields.Add("作品地址", SPFieldType.Text, false);

                        string fieldName = "计划ID"; //新增的Lookup类型字段的名字
                        SPList lookupList = oWebsite.Lists[lookupListTitle]; //设置这个Lookup类型字段要从哪个List中去取值
                        Guid lookupGuid = new Guid(lookupList.ID.ToString());// 取得这个Lookup数据源List的Guid
                        list.Fields.AddLookup(fieldName, lookupGuid, false);  //把上面取得的参数引入到AddLookup方法中，从而创建一个Lookup字段
                        SPFieldLookup splookup = list.Fields[fieldName] as SPFieldLookup; //绑定数据List到Lookup字段
                        splookup.LookupField = "Title";
                        splookup.Update();

                        SPView defaultView = list.Views[0];

                        defaultView.ViewFields.Add(list.Fields["活动地点"]);
                        defaultView.ViewFields.Add(list.Fields["活动类型"]);
                        defaultView.ViewFields.Add(list.Fields["活动对象"]);
                        defaultView.ViewFields.Add(list.Fields["开始时间"]);
                        defaultView.ViewFields.Add(list.Fields["持续时长"]);
                        defaultView.ViewFields.Add(list.Fields["执行者"]);
                        defaultView.ViewFields.Add(list.Fields["活动描述"]);
                        defaultView.ViewFields.Add(list.Fields["作品地址"]);
                        defaultView.ViewFields.Add(list.Fields["计划ID"]);
                        defaultView.Update();
                        list.Update();
                        retList = list;
                    }
                    catch
                    {
                    }
                    oWebsite.AllowUnsafeUpdates = false;
                }
            });
            return retList;
        }
        /// <summary>
        /// 是否存在子任务生成的活动
        /// </summary>
        /// <param name="oList"></param>
        /// <param name="fName"></param>
        /// <param name="fValue"></param>
        /// <returns></returns>
        private int ActivityIsExits(SPList oList, string fName, string fValue)
        {
            SPQuery objSPQuery = new SPQuery();
            string queryString = String.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Lookup'>{1}</Value></Eq></Where>", fName, fValue);
            objSPQuery.Query = queryString;
            SPListItemCollection objItems = oList.GetItems(objSPQuery);
            return objItems.Count;

        }
        #endregion
    }
}