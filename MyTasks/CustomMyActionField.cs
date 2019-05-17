using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MyTasks
{
    class CustomMyActionField : SPFieldChoice
    {
      /// <summary>
      /// 自定义的字段，快速编辑时无法编辑
      /// </summary>
      /// <param name="fields"></param>
      /// <param name="fieldName"></param>
        public CustomMyActionField(SPFieldCollection fields, string fieldName)
              : base(fields, fieldName)
        {
            InitValues();
            //InitActionField();
        }
        //此方法没有用
        public CustomMyActionField(SPFieldCollection fields, string typeName, string displayName)
             : base(fields, typeName, displayName)
        {
            InitValues();
            //InitActionField();
        }
        private void InitValues()
        {
            this.Choices.Clear();
            SPListItemCollection splistItems = GetSPItems("我的操作");
            foreach (SPListItem item in splistItems)
            {
                string Action = item["操作"].ToString();//此时的数据格式为：“ID;#查阅字段值”，需要做拆分处理
                Action = Action.Split('#')[1];
                this.Choices.Add(Action);
            }
            this.FillInChoice = true;
        }
        /// <summary>
        /// 填充当前用户的个人操作，
        /// </summary>
        private void InitActionField()
        {
            string fldName = "Action";
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(this.ParentList.ParentWeb.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(this.ParentList.ParentWeb.ID))
                    {
                        SPList lstAction = web.Lists[this.ParentList.Title];
                        web.AllowUnsafeUpdates = true;
                        try
                        {
                            if (lstAction.Fields.ContainsFieldWithStaticName(fldName))
                            {
                                SPFieldChoice fldAction = lstAction.Fields.GetFieldByInternalName(fldName) as SPFieldChoice;
                                fldAction.Choices.Clear();
                                try
                                {
                                    SPListItemCollection splistItems = GetSPItems("我的操作");
                                    foreach (SPListItem item in splistItems)
                                    {
                                        string Action = item["操作"].ToString();//此时的数据格式为：“ID;#查阅字段值”，需要做拆分处理
                                        Action = Action.Split('#')[1];
                                        fldAction.AddChoice(Action);
                                    }
                                }
                                catch
                                { }
                                fldAction.Update();
                            }
                         }
                        catch (Exception ex)
                        { }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        /// <summary>
        /// 读取我的操作列表中当前用户的操作
        /// </summary>
        /// <param name="fromList"></param>
        /// <returns></returns>
        private   SPListItemCollection GetSPItems(string fromList)
        {
            try
            {
                int usrID = UserID ;
                using (SPSite site = SPContext.Current.Site)
                {
                    using (SPWeb spWeb = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList spList = spWeb.Lists.TryGetList(fromList);
                        if (spList != null)
                        {
                            SPQuery qry = new SPQuery();

                            qry.Query = @"<Where><Eq><FieldRef Name='User' LookupId='True' /><Value Type='Integer'>" + usrID
                              + "</Value></Eq></Where><OrderBy><FieldRef Name='Frequency' Ascending='FALSE' /></OrderBy>";
                            qry.ViewFields = @"<FieldRef Name='ActionID' />";

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

        }
        #region 属性
        /// <summary>
        /// 获取当前用户ID,获取当前用户的数据
        /// </summary>
        private int userID=0;
        private   int UserID
        {
            get
            {
                if (userID ==0)
                    userID= SPContext.Current.Web.CurrentUser.ID;
                return userID;
            }
        }
        #endregion
    }
}
