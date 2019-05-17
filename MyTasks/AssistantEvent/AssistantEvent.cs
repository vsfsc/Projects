using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Data;

namespace MyTasks.AssistantEvent
{
    /// <summary>
    /// 列表项事件,在新建 个人生活记录时，检查操作，如果是新的，则写入到操作和我的操作列表中
    /// 如果存在，新频次加1
    /// 2018-08-31事件变更
    /// 活动操作，添加或编辑时，将类别写入到活动类型字段中，活动操作为自定义字段，无法保存类别
    /// </summary>
    public class AssistantEvent : SPItemEventReceiver
    {
        #region 事件
        /// <summary>
        /// 已添加项.活动类型-ActivityType
        /// 活动操作-=CustAction
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            //if (properties.ListTitle != "个人学习助手") return;
            //WriteResultToField(properties);
            if (!AllowEvent(properties ) ) return;
            UpdateResults(properties );
            UpdateActivityType(properties);
        }
        /// <summary>
        /// 编辑时没有进行操作
        /// </summary>
        /// <param name="properties"></param>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            if (!AllowEvent(properties ) ) return;
            UpdateActivityType(properties);
            //SetActionField(properties );
        }
        public override void ItemAdding(SPItemEventProperties properties)
        {
            base.ItemAdding(properties);
            //if (!AllowEvent(properties ) ) return;
            //PlanEvent(properties);

        }
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
            //if (!AllowEvent(properties ) ) return;
            //PlanEvent(properties);
        }
        //上传结果时，没有活动中的结果字段没有内容，则将当前文档链接写入字段，以便直接浏览
        private void WriteResultToField(SPItemEventProperties properties)
        {
            if (!properties.List.Fields.ContainsFieldWithStaticName("AssistantID") && properties.List.Fields.ContainsFieldWithStaticName("Result"))
                return;
            string lstName = properties.ListTitle.Replace("结果", "");//对应活动或计划
            int id =  int.Parse(properties.ListItem["AssistantID"].ToString ());
            SPWeb web = properties.Web;
            SPList lst = web.Lists.TryGetList(lstName);
            SPListItem item = lst.GetItemById(id);
           
            item[lst.Fields.GetFieldByInternalName("Result").Title] = properties.ListItem["Result"];
            this.EventFiringEnabled = false;
            item.SystemUpdate();
            this.EventFiringEnabled = true;

        }
        //录入数据时执行，在活动中或计划中更新活动类型，新建时写入结果
        private bool AllowEvent(SPItemEventProperties properties)
        {
            if (properties.List.Fields.ContainsFieldWithStaticName("ActualDuring")|| properties.List.Fields.ContainsFieldWithStaticName("PlanDuring"))
                return true;
            else
                return false;
        }
        private void PlanEvent(SPItemEventProperties properties)
        {
            bool allowAdd = !AssistantExits(properties);
            if (!allowAdd)
            {
                //properties.Status = SPEventReceiverStatus.CancelNoError;
                //properties.ErrorMessage = "此时间段时已经存在其他操作，指重新指定计划开始或时长！";
                properties.Cancel = true;
                properties.RedirectUrl = "/_layouts/CustomErrorPage/AssistantError.aspx";
                properties.Status = SPEventReceiverStatus.CancelWithRedirectUrl;
            }
        }
        #endregion
        #region 新建保存结果
        /// <summary>
        /// 新建保存的时候是否存在结果，如果存在则保存，否则不保存
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>

        private bool UpdateResults(SPItemEventProperties properties)
        {
            SPUser user = properties.Web.CurrentUser;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(properties.Site.ID))
                {
                    using (SPWeb sWeb = spSite.OpenWeb(properties.Web.ID))
                    {
                        SPList sList = sWeb.Lists[properties.ListTitle + "临时文件"];
                        SPQuery qry = new SPQuery();
                        SPListItemCollection docITems = null;
                        qry.ViewFields = "<FieldRef Name='ID' /><FieldRef Name='Author' /><FieldRef Name='Title' /><FieldRef Name='Created' />";
                        qry.Query = @"<Where><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Lookup'>" + user.ID + "</Value></Eq></Where>";
                        docITems = sList.GetItems(qry);//当前用户只保存最后一条数据
                        bool hasResults = false;
                        if (docITems.Count > 0)
                        {
                            if (docITems[0]["标题"].ToString () == "1")
                                hasResults = true;
                        }
                        if (!hasResults) return;
                        DateTime resultTime = (DateTime )docITems[0]["创建时间"];
                        sList = sWeb.Lists[properties.ListTitle + "结果"];
                        qry = new SPQuery();

                        //当前个人学习记录的文档
                        qry.ViewFields = "<FieldRef Name='ID' /><FieldRef Name='Author' /><FieldRef Name='Result' /><FieldRef Name='AssistantID' /><FieldRef Name='Title' /><FieldRef Name='Created' />";

                        //根据时间获取新建项目的结果，根据用户和时间
                        //string txtDate = SPUtility.CreateISO8601DateTimeFromSystemDateTime(resultTime);
                        qry.Query = @"<Where><And><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Lookup'>" + user.ID + "</Value></Eq><Eq><FieldRef Name='AssistantID'/><Value Type='Number'>0</Value></Eq></And></Where><OrderBy><FieldRef Name = 'Modified' Ascending = 'FALSE' /></OrderBy>";
                        //qry.Query = @"<Where><And><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Lookup'>" + user.ID + "</Value></Eq><Eq><FieldRef Name='Created'/><Value Type='DateTime' IncludeTimeValue='TRUE'>" + txtDate + "</Value></Eq></And></Where><OrderBy><FieldRef Name='Created' Ascending='FALSE' /></OrderBy>";

                        docITems = sList.GetItems(qry);
                        if (docITems.Count > 0)//
                        {
                            //DateTime dtNew = (DateTime)docITems[0]["创建时间"];
                            SPListItem itm;
                            for (int i = docITems.Count - 1; i >= 0; i--)
                            {
                                itm = docITems[i];

                                if ((DateTime)itm["创建时间"] == resultTime )
                                {
                                    itm["AssistantID"] = properties.ListItemId;
                                    itm["修改者"] = user;
                                    itm.Update ();
                                }
                                else
                                    itm.Delete();
                            }

                        }
                    }
                }
            });
            return true;
        }
        #endregion
        #region 时间不能重复的方法
        private bool AssistantExits(SPItemEventProperties properties)
        {
            DateTime dt;
            double during = 0;
            if (properties.ListItem == null)
            {
                if (properties.AfterProperties["PlanDate"] == null || properties.AfterProperties["PlanDate"].ToString() == "") return false;
                dt = SPUtility.CreateDateTimeFromISO8601DateTimeString(properties.AfterProperties["PlanDate"].ToString ());
                during = properties.AfterProperties["PlanDuring"] == null||properties.AfterProperties["PlanDuring"].ToString()==""  ? 0 :  double.Parse (properties.AfterProperties["PlanDuring"].ToString() );
            }
            else
            {
                if (properties.ListItem["PlanDate"] == null || properties.ListItem["PlanDate"].ToString() == "") return false;
                dt = (DateTime)properties.ListItem["PlanDate"];
                during = properties.ListItem["PlanDuring"] == null ? 0 : (double)properties.ListItem["PlanDuring"];
            }
            int userID = properties.Web.CurrentUser.ID; ;
            SPList lstAction = properties.List;
            string txtDate = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dt.AddDays(-1));

            SPQuery qry = new SPQuery();
            qry.ViewFields = "<FieldRef Name='ID' /><FieldRef Name='PlanDate' /><FieldRef Name='PlanDuring' />";
            qry.Query = @"<Where><And><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Integer'>" + userID +
                "</Value></Eq><Geq><FieldRef Name='PlanDate' /><Value Type='DateTime' IncludeTimeValue='TRUE'>" + txtDate + "</Value></Geq></And></Where>";

            DataTable myItems = lstAction.GetItems(qry).GetDataTable();
            if (myItems == null) return false;
            DateTime dtStart;
            DateTime dtEnd;
            DateTime dt1 = dt.AddMinutes(during);
            foreach (DataRow dr in myItems.Rows)
            {
                if (properties.ListItem != null && (int)dr["ID"] == properties.ListItemId)
                    continue;
                dtStart = (DateTime)dr["PlanDate"];
                dtEnd = dtStart.AddMinutes(dr["PlanDuring"] == null || dr["PlanDuring"].ToString() == "" ? 0 : (double)dr["PlanDuring"]);
                if (dtEnd == dtStart)
                {
                    if (dt >= dtStart && dt <= dtEnd)
                        return true;
                    else
                    {
                        if (dt1 >= dtStart && dt1 <= dtEnd)
                            return true;
                    }
                }
                else
                {
                    if (dt >= dtStart && dt < dtEnd)
                        return true;
                    else
                    {
                        if (dt1 > dtStart && dt1 < dtEnd)//上一条活动的结束时间可以下一条的开始时间
                            return true;
                    }
                }
            }
            return false;
        }
        #endregion
        #region 新的方法
        //活动动类型、结果更新
        private void UpdateActivityType(SPItemEventProperties properties)
        {
            string actiType = "";
            string fldName = "CustAction";
            SPFieldLookup fld = properties.List.Fields.GetFieldByInternalName(fldName) as SPFieldLookup;
            if (properties.ListItem[fld.Title] == null) return;
            SPWeb spWeb1 = properties.Web;
            if (properties.List.Fields.ContainsFieldWithStaticName(fldName))
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite spSite = new SPSite(spWeb1.Site.ID)) //找到网站集
                    {
                        using (SPWeb spWeb = spSite.OpenWeb(spWeb1.ID))
                        {
                            //SPList lst = spWeb.Lists.TryGetList(properties.List.Title);
                            //SPField fld = lst.Fields.GetFieldByInternalName(fldName);

                            string lstID = fld.LookupList;
                            SPList lstType = spWeb.Lists[new Guid(lstID)];//查阅项所在的列表

                            SPFieldLookupValue custID = properties.ListItem[fld.Title] as SPFieldLookupValue;
                            if (custID == null)
                            {
                                custID = (properties.ListItem[fld.Title] as SPFieldLookupValueCollection)[0];
                            }
                            SPListItem itemType = lstType.GetItemById(custID.LookupId);
                            foreach (SPField fldType in lstType.Fields)
                            {
                                if (fldType.Type == SPFieldType.Lookup && !fldType.Hidden)
                                {
                                    SPFieldLookupValue fldValue = itemType[fldType.Id] as SPFieldLookupValue;
                                    if (fldValue == null)
                                    {
                                        fldValue = new SPFieldLookupValue(itemType[fldType.Id].ToString());
                                    }
                                    actiType = fldValue.LookupValue;
                                    break;
                                }
                            }
                            bool isSave = false;
                            if (properties.ListItem["ActivityType"] == null || properties.ListItem["ActivityType"].ToString() != actiType)
                            {
                                properties.ListItem["ActivityType"] = actiType;
                                isSave = true;
                            }

                            //更新结果
                            string resultTitle = properties.List.Fields.GetFieldByInternalName("Result").Title;
                            //if (properties.ListItem[resultTitle] == null)
                            //{
                            SPList sList = spWeb.Lists[properties.ListTitle + "结果"];
                            SPQuery qry = new SPQuery();

                            //当前个人学习记录的文档
                            qry.ViewFields = "<FieldRef Name='ID' /><FieldRef Name='Author' /><FieldRef Name='Result' /><FieldRef Name='AssistantID' /><FieldRef Name='Title' /><FieldRef Name='Created' />";

                            qry.Query = @"<Where><Eq><FieldRef Name='AssistantID'/><Value Type='Number'>" + properties.ListItemId + "</Value></Eq></Where>";

                            SPListItemCollection docITems = sList.GetItems(qry);
                            if (docITems.Count > 0)//
                            { properties.ListItem[resultTitle] = docITems[0]["Result"]; isSave = true; }

                            //}
                            if (isSave)
                            {
                                this.EventFiringEnabled = false;
                                properties.ListItem.SystemUpdate();
                                this.EventFiringEnabled = true;
                            }

                        }
                    }
                });
            }
        }
        #endregion
        #region 以前的方法
        //在数据保存的时候，将操作字段隐藏
        private void SetActionField(SPItemEventProperties properties)
        {
            string fldName = "Action";
            SPWeb spWeb1 = properties.Web;
            if (properties.List.Fields.ContainsFieldWithStaticName (fldName ))
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite spSite = new SPSite(spWeb1.Site.ID)) //找到网站集
                    {
                        using (SPWeb spWeb = spSite.OpenWeb(spWeb1.ID))
                        {
                            SPList lst = spWeb.Lists.TryGetList(properties.List.Title); 
                            SPField fld = lst.Fields.GetFieldByInternalName(fldName);
                            fld.ShowInDisplayForm = false;
                            fld.ShowInEditForm = false;
                            fld.ShowInNewForm = false;
                            spWeb.AllowUnsafeUpdates = true;
                            fld.Update();
                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
        }
        private int GetAuthorID(string account,SPWeb myWeb)
        {
             int id = 0;
            if (account == "")
            {
                id = myWeb.CurrentUser.ID;
                return id;
            }
            try
            {
                SPUser s = myWeb.EnsureUser(account);
                id = s.ID;
            }
            catch
            {

            }
            return id;
        }
        private static void WriteMyAction(SPWeb spWeb1, int userID ,string action)
        {
            string listMyAction = "我的操作";
            string spFieldName = "ActionID";
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(spWeb1.Site.ID )) //找到网站集
                {
                    using (SPWeb spWeb = spSite.OpenWeb(spWeb1.ID ))
                    {
                        SPList lstMyAction = spWeb.Lists.TryGetList(listMyAction );//有查阅项的列表,我的操作
                        SPFieldLookup task = lstMyAction.Fields.GetFieldByInternalName(spFieldName) as SPFieldLookup;
                        SPList lstAction = spWeb.Lists[new Guid(task.LookupList)];
                        SPQuery qry = new SPQuery();
                        SPListItem newItem;
                        int actionID;
                        if (lstAction != null)
                        {//主表查询
                            qry = new SPQuery();
                            qry.ViewFields = "<FieldRef Name='ID' /><FieldRef Name='Title' />";
                            qry.Query = @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + action +
                        "</Value></Eq></Where>";

                            SPListItemCollection myItems = lstAction.GetItems(qry);
                            if (myItems.Count == 0)//没有此操作则添加
                            {
                                newItem = lstAction.Items.Add();
                                newItem["Title"] = action;
                                newItem.Update();
                            }
                            else
                            {
                                newItem = myItems[0]; 
                            }
                            actionID = newItem.ID;
                            //子表查询 Frequecy字段名称中少一个n Frequency
                            qry = new SPQuery();
                            qry.ViewFields = "<FieldRef Name='ID' /><FieldRef Name='User' /><FieldRef Name='ActionID' /><FieldRef Name='Frequency' />";
                            qry.Query = @"<Where><And><Eq><FieldRef Name='User' LookupId='True' /><Value Type='Integer'>" + userID +
                                "</Value></Eq><Eq><FieldRef Name='ActionID' LookupId='True' /><Value Type='Lookup'>" + actionID  + "</Value></Eq></And></Where>";

                            myItems = lstMyAction.GetItems(qry);
                            if (myItems.Count == 0)//没有此操作则添加
                            {
                                newItem = lstMyAction .Items.Add();
                                newItem["ActionID"] =actionID;
                                newItem["User"] = userID;
                                newItem["Frequency"] = 1;
                                newItem.Update();
                            }
                            else
                            {
                                newItem = myItems[0];
                                newItem["Frequency"] = (int)newItem["Frequency"] + 1;
                                newItem.Update();
                            }
                        }
                

                    }
                }
            });
           
        }
        #endregion
    }
}