using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PowerPA.QuickEditing
{
    public partial class QuickEditingUserControl : UserControl
    {
        #region 控件
        /// <summary>
        /// UpdatePanel1 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.UpdatePanel UpdatePanel1;
        
        /// <summary>
        /// AppraiseDiv 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl AppraiseDiv;
        
        /// <summary>
        /// tbSettings 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlTable tbSettings;
        
        /// <summary>
        /// trTitle 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlTableRow trTitle;
        
        /// <summary>
        /// btnPre 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnPre;
        
        /// <summary>
        /// dtStart 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::Microsoft.SharePoint.WebControls.DateTimeControl dtStart;
        
        /// <summary>
        /// btnNext 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnNext;

        /// <summary>
        /// dtCurrentDate 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::Microsoft.SharePoint.WebControls.DateTimeControl dtCurrentDate;

        /// <summary>
        /// chkDayType 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.CheckBox chkDayType;
        
        /// <summary>
        /// spShowInfo 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl spShowInfo;
        
        /// <summary>
        /// tbQuick 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlTable tbQuick;
        
        /// <summary>
        /// gvActivities 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.GridView gvActivities;
        
        /// <summary>
        /// tbSingle 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlTable tbSingle;
        
        /// <summary>
        /// ddlActions 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.DropDownList ddlActions;
        
        /// <summary>
        /// lblTypeShow 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label lblTypeShow;
        
        /// <summary>
        /// txtHours 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox txtHours;
        
        /// <summary>
        /// spanHours 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl spanHours;
        
        /// <summary>
        /// txtQuantity 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox txtQuantity;
        
        /// <summary>
        /// spanQuantity 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl spanQuantity;
        
        /// <summary>
        /// txtDesc 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox txtDesc;
        
        /// <summary>
        /// cbNormal 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.CheckBox cbNormal;
        
        /// <summary>
        /// spanNormal 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl spanNormal;
        
        /// <summary>
        /// ddlTypes 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.DropDownList ddlTypes;
        
        /// <summary>
        /// spanTasks 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl spanTasks;
        
        /// <summary>
        /// divDocs 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divDocs;
        
        /// <summary>
        /// tbContent 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.Table tbContent;
        
        /// <summary>
        /// spanUpdate 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl spanUpdate;
        
        /// <summary>
        /// btnSave 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnSave;
        
        /// <summary>
        /// btnCancel 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnCancel;
        
        /// <summary>
        /// udesp 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl udesp;
        
        /// <summary>
        /// lblMsg 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label lblMsg;
        
        /// <summary>
        /// txtIds 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.HiddenField txtIds;
        #endregion
       public QuickEditing webObj { get; set; }
        #region 事件
        /// <summary>
        /// 页面加载事件，设置控件的事件连接，用户的权限判断，是批量录入还是单条录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            gvActivities.RowDataBound += gvActivities_RowDataBound;
            btnPre.Click += BtnNext_Click;
            btnNext.Click += BtnNext_Click;
            btnCancel.Click += BtnCancel_Click;
            ddlActions.SelectedIndexChanged += ddlActions_SelectedIndexChanged;
            dtCurrentDate.DateChanged += dtCurrentDate_DateChanged;
            chkDayType.CheckedChanged += ChkDayType_CheckedChanged;
            chkDayType.AutoPostBack = true;
            if (!Page.IsPostBack)
            {
                if (SPContext.Current.Web.CurrentUser == null)
                {
                    lblMsg.Text = "请先登录！";
                    trTitle.Visible = false;
                    tbQuick.Visible = false;
                    tbSingle.Visible = false;
                    btnSave.Enabled = false;
                    return;
                }

                if (!CheckList())
                    return;
                try
                {
                    if (webObj.QuickEdit == 1)
                    {
                        tbQuick.Visible = true;
                        tbSingle.Visible = false;
                        InitMultiActivity();
                        udesp.Visible = true;
                    }
                    else
                    {
                        chkDayType.Visible = false;
                        tbQuick.Visible = false;
                        tbSingle.Visible = true;
                        udesp.Visible = false;
                        InitSingleActivity();
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.Message;
                }
            }
            btnSave.Click += btnSave_Click;
        }
        /// <summary>
        /// gridview主体列表中操作选项更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = ddl.Parent.Parent as GridViewRow;
            DropDownList ddlTask = row.FindControl("ddlTypes") as DropDownList;
            //更改任务文档
            int tastID = GetActivityTaskID(SPContext.Current.Web.CurrentUser.ID, int.Parse(ddl.SelectedItem.Value));
            SelectDropDownList(ddlTask, tastID.ToString());

        }
        /// <summary>
        /// 工作日与非工作日的切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkDayType_CheckedChanged(object sender, EventArgs e)
        {
            //chkDayType.Text = chkDayType.Checked ? "工作日" : webObj.NonWorkday;
            InitGridView(1);
        }
        /// <summary>
        /// 单条活动中操作改变事件，当操作改变时，显示对应的操作类型，并显示最近一条活动对应的任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            //操作类型关系表，改由原来的关系到保存在操作表
            string actionType = GetActionType(int.Parse(ddlActions.SelectedItem.Value));
            lblTypeShow.Text = "操作类型：" + actionType;

            //更改任务文档
            int tastID = GetActivityTaskID(SPContext.Current.Web.CurrentUser.ID, int.Parse(ddlActions.SelectedItem.Value));
            SelectDropDownList(ddlTypes, tastID.ToString());
        }
        
        /// <summary>
        /// 取消报钮，如果有地址参数，则返回指定的地址，否则直接返回到活动列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Source"] != null)
                Response.Redirect(Request.QueryString["Source"].ToString());
            else if (ViewState["listUrl"] != null)
                Response.Redirect(ViewState["listUrl"].ToString());
        }
        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (((Button)sender).ID == "btnPre")
                dtCurrentDate.SelectedDate = dtCurrentDate.SelectedDate.AddDays(-1);
            else
                dtCurrentDate.SelectedDate = dtCurrentDate.SelectedDate.AddDays(1);
            if (webObj.QuickEdit == 1)
                InitGridView();
        }
        /// <summary>
        /// 日期选择控件，可以选择或填写日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dtCurrentDate_DateChanged(object sender, EventArgs e)
        {
            if (webObj.QuickEdit == 1)
                InitGridView();

        }
        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveItem(dtCurrentDate.SelectedDate);
        }
        /// <summary>
        /// 保存按钮调用的方法
        /// </summary>
        /// <param name="dtDate">活动日期</param>
        protected void SaveItem(DateTime dtDate)
        {
            List<ActivityInfo> results = new List<ActivityInfo>();
            ActivityInfo activity;
            string writeDateType = "";
            if (webObj.QuickEdit == 1)
            {
                #region enumerae gridview
                foreach (GridViewRow gvr in gvActivities.Rows)
                {
                    CheckBox cbSel = (CheckBox)gvr.Cells[0].FindControl("cbSel");
                    CheckBox cbNormal = (CheckBox)gvr.Cells[7].FindControl("cbSelNormal");
                    Label lbl = ((Label)gvr.Cells[10].FindControl("lblItemID")) as Label;
                    Label lblFlag = ((Label)gvr.Cells[8].FindControl("lblFlag")) as Label;
                    if (cbSel.Checked)
                    {
                        DropDownList ddlActions = ((DropDownList)gvr.Cells[2].FindControl("ddlActions"));
                        string actionId = ddlActions.SelectedItem.Value;
                        TextBox txtHours = ((TextBox)gvr.Cells[3].FindControl("tbDuring"));
                        TextBox txtQuantity = ((TextBox)gvr.Cells[4].FindControl("tbQuantity"));
                        TextBox txtDesc = ((TextBox)gvr.Cells[5].FindControl("tbDesc"));
                        DropDownList ddlTypes = ((DropDownList)gvr.Cells[6].FindControl("ddlTypes"));
                        if (txtHours.Text != "" && txtHours.Text != "0" || txtQuantity.Text != "" && txtQuantity.Text != "0")
                        {
                            activity = new ActivityInfo();
                            activity.actionID = int.Parse(actionId);
                            if (txtHours.Text != "")
                                activity.during = int.Parse(txtHours.Text);
                            if (txtQuantity.Text != "")
                                activity.quantity = float.Parse(txtQuantity.Text);
                            activity.isNormal = cbNormal.Checked;
                            activity.desc = txtDesc.Text;
                            activity.tastID = int.Parse(ddlTypes.SelectedItem.Value);
                            activity.action = ddlActions.SelectedItem.Text;
                            activity.itemID = int.Parse(lbl.Text);
                            activity.flag = lblFlag.Text == "2" ? 1 : 0;
                            results.Add(activity);

                        }
                    }
                    else
                    {
                        if (lblFlag.Text == "1")//此条删除,已经保存的
                        {
                            activity = new ActivityInfo();
                            activity.itemID = int.Parse(lbl.Text);
                            activity.flag = -1;
                            results.Add(activity);
                        }
                    }
                }
                #endregion
                if (chkDayType.Enabled && ViewState["chkDayType"].ToString() != (chkDayType.Checked ? "1" : "0"))//将非正常的工作类别写入日常任务
                    writeDateType = chkDayType.Checked ? "工作日" : webObj.NonWorkday;//chkDayType.Text;
            }
            else
            {
                int id = 0;
                if (Request.QueryString["ID"] != null)
                    id = int.Parse(Request.QueryString["ID"].ToString());

                if (txtHours.Text != "" && txtHours.Text != "0" || txtQuantity.Text != "" && txtQuantity.Text != "0")
                {
                    activity = new ActivityInfo();
                    activity.actionID = int.Parse(ddlActions.SelectedItem.Value);
                    if (txtHours.Text != "")
                        activity.during = int.Parse(txtHours.Text);
                    if (txtQuantity.Text != "")
                        activity.quantity = float.Parse(txtQuantity.Text);
                    activity.isNormal = cbNormal.Checked;
                    activity.desc = txtDesc.Text;
                    activity.tastID = int.Parse(ddlTypes.SelectedItem.Value);
                    activity.action = ddlActions.SelectedItem.Text;
                    activity.itemID = id;
                    results.Add(activity);
                }
            }
            WriteToList(results, dtDate, writeDateType);
            //跳转，一般进入“后一日”，如果日期改变后当日是明天时，则进入活动分析界面 
            if (dtDate == DateTime.Today.Date )
            {
                if (Request.QueryString["Source"] != null)
                    Response.Redirect(Request.QueryString["Source"].ToString());
                else
                    lblMsg.Text = "保存成功";
            }
            else
            {
                dtCurrentDate.SelectedDate =dtDate.AddDays(1);
                if (webObj.QuickEdit == 1)
                    InitGridView();
            }
            

        }
        /// <summary>
        /// 活动的结构，读取列表中数据时，以结构的方式进行读取
        /// </summary>
        private struct ActivityInfo
        {
            public int actionID;
            public int during;
            public float quantity;
            public bool isNormal;
            public string desc;
            public int tastID;
            public string action;//合成标题
            public int itemID;
            public int flag;//flag=1为日历活动
        }
        /// <summary>
        /// 更改的数据写到列表中
        /// </summary>
        /// <param name="results">有脏数据的行</param>
        /// <param name="dtDate">当前日期</param>
        /// <param name="dayType">日期类型</param>
        private void WriteToList(List<ActivityInfo> results, DateTime dtDate, string dayType)
        {
            SPQuery qry = new SPQuery();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
                {
                    using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList spList = spWeb.Lists.TryGetList(webObj.ActivityList);
                        if (spList != null)
                        {
                            spWeb.AllowUnsafeUpdates = true;
                            SPUser appraiseUser = SPContext.Current.Web.CurrentUser;
                            if (spList.Fields.ContainsFieldWithStaticName("ActionID"))
                            {
                                foreach (ActivityInfo activity in results)
                                {
                                    SPListItem item=null;
                                    if (activity.itemID > 0)//edit
                                    {
                                        if (activity.flag == -1)
                                        {
                                            spList.Items.DeleteItemById(activity.itemID);
                                            continue;
                                        }
                                        item = spList.GetItemById(activity.itemID);
                                    }
                                    else//add
                                    {
                                        qry = new SPQuery();//是否已经录入
                                        qry.ViewFields = @"<FieldRef Name='ActionID' /><FieldRef Name='TaskID' />";
                                        qry.Query = @"<Where>
                                  <And><And>
                                     <Eq>
                                        <FieldRef Name='ActionID' LookupId='True' />
                                        <Value Type='Integer'>" + activity.actionID + @"</Value>
                                     </Eq>
                                     <Eq>
                                        <FieldRef Name='Date' />
                                        <Value Type='DateTime'>" + dtDate.ToString("yyyy-MM-dd") + @"</Value>
                                     </Eq>
                                  </And><Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>" + appraiseUser.ID + @"</Value>
                                     </Eq></And>
                               </Where>";
                                        SPListItemCollection items = spList.GetItems(qry);//任务和操作相同为同一活动
                                        bool addNew = true;
                                        if (items.Count > 0)
                                        {
                                            for (int i = 0; i < items.Count; i++)
                                            {
                                                if (items[i]["TaskID"] == null && activity.tastID == 0 || items[i]["TaskID"] != null && new SPFieldLookupValue(items[i]["TaskID"].ToString()).LookupId == activity.tastID)
                                                {
                                                    item = items[i];
                                                    addNew = false;
                                                    break;
                                                }
                                            }
                                        }
                                        

                                        if (addNew)
                                        {
                                            item = spList.Items.Add();
                                            item["创建者"] = appraiseUser.ID;
                                            item["Flag"] = activity.flag;
                                        }
                                    }
                                    item[spList.Fields.GetFieldByInternalName("ActionID").Title] = activity.actionID;
                                    item["日期"] = dtDate;
                                    if (activity.during > 0)
                                        item["时长"] = activity.during;
                                    else
                                        item["时长"] = null;
                                    if (activity.quantity != 0)
                                        item["数量"] = activity.quantity;
                                    else
                                        item["数量"] = null;
                                    item["是否正常"] = activity.isNormal;
                                    item["说明"] = activity.desc;
                                    item["Title"] = appraiseUser.Name + dtDate.ToShortDateString() + activity.action;
                                    if (activity.tastID == 0)
                                        item["TaskID"] = null;
                                    else
                                        item["TaskID"] = activity.tastID;

                                    item.Update();
                                    int assistantID = item.ID;
                                    if (webObj.QuickEdit == 0)//单条编辑时添加附件
                                    {
                                        if (txtIds.Value.Length > 0 && ViewState["CurrentUser"] != null)
                                        {
                                            string[] ids = txtIds.Value.Split(',');
                                            WriteResultToList(ids, assistantID, spWeb);
                                        }
                                    }
                                }
                            }
                            //写入非正常的工作类型
                            if (dayType != "")
                            {
                                SPList calList = spWeb.Lists.TryGetList(webObj.CalendarList);
                                if (calList != null)
                                {
                                    SPListItem calItem = calList.AddItem();
                                    calItem["Title"] = dayType;
                                    calItem["Author"] = UserID;
                                    calItem["Editor"] = UserID;
                                    calItem["fAllDayEvent"] = true;//全天事件
                                    calItem["EventDate"] = dtDate;
                                    calItem["EndDate"] = dtDate;
                                    //calItem["Decription"] = "";
                                    calItem.Update();
                                }
                            }
                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });
        }
        /// <summary>
        /// 活动列表主体区数据绑定事件，绑定操作（用户操作，如果没有则所有操作）和任务（按层次显示与用户相关的任务）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //SelectDropDownList(ddlTypes, gvActivities.DataKeys[e.Row.RowIndex]["类别"].ToString());
                Label lbl = e.Row.FindControl("lblFlag") as Label;
                if (lbl.Text == "1")
                    e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(0, 118, 198);
                else
                    e.Row.Cells[0].BackColor = System.Drawing.Color.White;
                CheckBox chk = e.Row.FindControl("cbSel") as CheckBox;
                if (lbl.Text != "4")//1-已有活动；2-日历活动；3-以往推出的活动；4-推出的活动
                    chk.Checked = true;
                Label lblTask = e.Row.FindControl("lblTaskID") as Label;
                //操作绑定lblTaskID
                DropDownList ddlActions = e.Row.FindControl("ddlActions") as DropDownList;
                DataTable dtSource = GetBindData(webObj.MyActionList, "");

                BindDdlActions(ddlActions, dtSource.Copy());
                string actionID = gvActivities.DataKeys[e.Row.RowIndex]["ID"].ToString();
                SelectDropDownList(ddlActions, actionID.Substring(actionID.IndexOf("_") + 1));

                dtSource = GetBindDataOfTask(ViewState["TaskListName"].ToString(), dtCurrentDate.SelectedDate);
                //任务绑定
                DropDownList ddlTypes = e.Row.FindControl("ddlTypes") as DropDownList;
                BindDropDownList(ddlTypes, "ID", "Title", dtSource);
               
                SelectDropDownList(ddlTypes, lblTask.Text);
                 
            }
        }
       
        /// <summary>
        /// 计算时长和选中的条数
        /// </summary>
        private void Caculate()
        {
            CheckBox cbSel;
            int durings = 0;
            int count = 0;
            foreach (GridViewRow gvr in gvActivities.Rows)
            {
                cbSel = (CheckBox)gvr.Cells[0].FindControl("cbSel");
                if (cbSel.Checked)
                {
                    TextBox txtHours = ((TextBox)gvr.Cells[3].FindControl("tbDuring"));
                    count += 1;
                    if (txtHours.Text != "")
                        durings += int.Parse(txtHours.Text);
                }
            }
            spShowInfo.InnerText = "已经选中 " + count.ToString() + " 条；共输入时长 " + durings.ToString() + " 分";
        }
        /// <summary>
        /// 全选的选中与非选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbSelAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbAll = (CheckBox)sender;
            if (cbAll.Text == "全选")
            {
                foreach (GridViewRow gvr in gvActivities.Rows)
                {
                    CheckBox cbSel = (CheckBox)gvr.Cells[0].FindControl("cbSel");
                    cbSel.Checked = cbAll.Checked;
                }
                Caculate();
            }
        }
        /// <summary>
        /// 绑定下拉列表的方法
        /// </summary>
        /// <param name="objDropDownList">下拉列表控件</param>
        /// <param name="vField">显示的值</param>
        /// <param name="tField">显示的文本列</param>
        /// <param name="dtSource">绑定的数据源</param>
        private void BindDropDownList(DropDownList objDropDownList, string vField, string tField, DataTable dtSource)
        {
            objDropDownList.DataSource = dtSource;
            objDropDownList.DataValueField = vField;
            objDropDownList.DataTextField = tField;
            objDropDownList.DataBind();
        }
        /// <summary>
        /// 根据文本或值设置下拉列表的选中项
        /// </summary>
        /// <param name="objDropDownList">下拉列表控件</param>
        /// <param name="valueOrText">显示的文本或值</param>
        public void SelectDropDownList(DropDownList objDropDownList, string valueOrText)
        {
            objDropDownList.ClearSelection();
            ListItem objLI = objDropDownList.Items.FindByValue(valueOrText);
            if (objLI == null)
            {
                objLI = objDropDownList.Items.FindByText(valueOrText);
            }
            if (objLI != null)
            {
                objDropDownList.ClearSelection();
                objLI.Selected = true;
            }
            else if (objDropDownList.ID == ddlTypes.ID)
            {
                //objDropDownList.Items[objDropDownList.Items.Count - 1].Selected = true;
                objLI = objDropDownList.Items.FindByValue("0");
                objDropDownList.ClearSelection();
                objLI.Selected = true;
            }
        }
        //

        /// <summary>
        /// 返回任务，项目和常规个人部分
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="currDt"></param>
        /// <returns></returns>
        private DataTable GetBindDataOfTask(string listName, DateTime currDt)
        {
            DataTable dt = null;
            int r = -1;
            if (ViewState["dtCurrDate"] != null)
                r = currDt.CompareTo(DateTime.Parse(ViewState["dtCurrDate"].ToString()));
            if (ViewState["dtTask"] != null && r == 0)
                dt = (DataTable)ViewState["dtTask"];
            else
            {
                SPWeb web = SPContext.Current.Web;
                SPList list = web.Lists.TryGetList(listName);
                if (list != null && list.ItemCount > 0)
                {
                    SPQuery qry = new SPQuery();
                    qry.ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='ParentID' />";
                    qry.Query = @"<Where>
      <IsNull>
         <FieldRef Name='ParentID' />
      </IsNull>
   </Where>";
                    SPListItemCollection items = list.GetItems(qry);

                    dt = items.GetDataTable().Clone();//items.count=0会出错
                    int level = 1;
                    int parentID;
                    string rootTitle;
                    //SPListItemCollection subItems;
                    int taskType = 1;//1-项目，2-Routine,只显示用户当前日期范围内的任务
                    foreach (SPListItem item in items)
                    {
                        WriteDataTable(ref dt, item, 0);
                        rootTitle = item["Title"].ToString();
                        parentID = item.ID;
                        //if (rootTitle == "项目")
                        //{
                        taskType = 1;
                        GetSubTasks(listName, parentID, level, taskType, currDt, ref dt);

                        //}
                        //else if (rootTitle == "Routine")
                        //{//获取当前用户的
                        //    string docName = GetRoutineDocName(Account, "生活类", currDt);
                        //    //ViewState["lifeType"] = docName;
                        //    qry = new SPQuery();
                        //    qry.ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='ParentID' />";
                        //    qry.Query = @"<Where><And><Eq><FieldRef Name='Title' /><Value Type='Text'>" + Account + "</Value></Eq><Eq><FieldRef Name='ParentID' LookupId='True' /><Value Type='Lookup'>" + parentID + "</Value></Eq></And></Where>";
                        //    subItems = list.GetItems(qry);//用户下面的数据
                        //    taskType = 2;
                        //    GetSubTasks(listName, subItems[0].ID, level, taskType, currDt, ref dt);
                        //}
                    }
                    if (dt == null)//项目任务数据为空
                    {
                        dt = new DataTable();
                        dt.Columns.Add(new DataColumn("ID", typeof(int)));
                        dt.Columns.Add(new DataColumn("Title", typeof(string)));

                    }
                    DataRow dr = dt.NewRow();
                    dr["ID"] = 0;
                    dt.Rows.InsertAt(dr, 0);

                    ViewState["dtTask"] = dt;
                    ViewState["dtCurrDate"] = currDt;
                }
            }
            return dt;
        }
        /// <summary>
        /// 获取任务iD，在推算活动的时候，自动 关联当天的任务
        /// </summary>
        /// <param name="taskName"></param>
        /// <returns></returns>
        private int GetTaskID(string taskName)
        {
            string listName = ViewState["TaskListName"].ToString();
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(listName);
            SPQuery qry = new SPQuery();
            qry.ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='ParentID' />";
            qry.Query = @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + taskName + "</Value></Eq></Where>";
            SPListItemCollection subItems = list.GetItems(qry);
            if (subItems.Count > 0)
                return subItems[0].ID;
            else
                return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="parentID"></param>
        /// <param name="level"></param>
        /// <param name="taskType">1=项目，2=Routine</param>
        /// <param name="dt"></param>
        private void GetSubTasks(string listName, int parentID, int level, int taskType, DateTime currDt, ref DataTable dt)
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(listName);
            SPQuery qry = new SPQuery();
            qry.ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='ParentID' />";
            qry.Query = @"<Where><Eq><FieldRef Name='ParentID' LookupId='True' /><Value Type='Lookup'>" + parentID + "</Value></Eq></Where>";
            SPListItemCollection subItems = list.GetItems(qry);
            string currTask = "";
            if (taskType == 2 && subItems.Count > 0)//月两条，周两条
            {
                if (subItems[0]["Title"].ToString().StartsWith(Account))
                {
                    currTask = GetActivityTaskByDate(subItems[0]["Title"].ToString(), currDt);

                }
            }
            foreach (SPListItem item in subItems)
            {
                if (currTask == "" || item["Title"].ToString() == currTask)
                {
                    WriteDataTable(ref dt, item, level);
                    GetSubTasks(listName, item.ID, level + 1, taskType, currDt, ref dt);
                }
            }
        }
        /// <summary>
        /// 任务是否是日常任务
        /// </summary>
        /// <param name="taskName"></param>
        /// <returns></returns>
        private bool TaskIsRoutine(string taskName)
        {
            bool isResult = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite mySite = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = mySite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList list = web.Lists.TryGetList(webObj.ActivityList);
                        SPList taskList = GetLookupLists(web, list, "TaskID");//获取活动关联的任务文档列表
                        SPQuery qry = new SPQuery();
                        string viewFlds = @"<FieldRef Name='Title' /><FieldRef Name='ParentID' />";
                        qry.ViewFields = viewFlds;
                        qry.Query = @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + taskName + "</Value></Eq></Where>";
                        SPListItemCollection subItems = taskList.GetItems(qry);
                        SPListItem item;
                        int parentID;
                        if (subItems.Count > 0)
                        {
                            item = subItems[0];
                            while (item["ParentID"] != null)
                            {
                                parentID = new SPFieldLookupValue(item["ParentID"].ToString()).LookupId;
                                item = taskList.GetItemById(parentID);
                            }
                            if (item["Title"].ToString() == "Routine")
                                isResult = true;
                        }
                    }
                }
            });
            return isResult;

        }
        private void WriteDataTable(ref DataTable dt, SPItem item, int level)
        {
            DataRow dr = dt.Rows.Add();
            dr["ID"] = item.ID;
            string preStr = "";
            for (int i = 0; i < level; i++)
                preStr += ("&nbsp;&nbsp;&nbsp;&nbsp;");
            string tmp = item["Title"].ToString();
            if (tmp.StartsWith(Account))
                tmp = tmp.Replace(Account, "");
            dr["Title"] = System.Web.HttpUtility.HtmlDecode(preStr) + tmp;

            if (ViewState["lifeType"] != null && ViewState["lifeType"].ToString() == item["Title"].ToString())
                ViewState["taskShowText"] = dr["Title"].ToString();

            dr.AcceptChanges();
        }
        /// <summary>
        /// //返回操作列表
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="parentId"></param>
        /// <param name="userID">编辑时当前用户的ID</param>
        /// <returns></returns>

        #endregion
        #region 2019-2 新的方法
         
        ///获取用户最后一次录入活动的日期 
        private DateTime?  GetLastDateActivity(int userId )
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(webObj.ActivityList);
            SPListItemCollection items = null;
            if (list != null)
            {
                SPQuery qry = new SPQuery();
                qry.RowLimit = 1; 
                qry.Query = @"<Where>
                                      <Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>" + userId + @"</Value>
                                     </Eq>
                                </Where><OrderBy><FieldRef Name='Date' Ascending='false' /></OrderBy>";
                items = list.GetItems(qry);
                if (items.Count > 0)
                    return (DateTime )items[0]["Date"];
            }
            return null;
        }
        /// <summary>
        /// 如果老用户近期有几天没有录入活动数据了，则缺省日期选为第一个没有保存数据的日期
        /// 当前时间12点前为昨天，午后则为今天；
        /// </summary>
        /// <returns></returns>
        private DateTime GetDefaultDate()
        {
            DateTime dtDef;
            DateTime? dt = GetLastDateActivity(UserID);
            if (dt != null && DateTime.Compare(dt.Value, DateTime.Today.AddDays(-1)) < 0)
                dtDef = dt.Value.AddDays(1);//小于昨天，则默认日期为日期加1
            else
            {
                if (DateTime.Now.Hour < 12)
                    dtDef = DateTime.Today.AddDays(-1);
                else
                    dtDef = DateTime.Today;
            }

            return dtDef ;
        }
        #endregion
        #region  初始化控件
        private bool CheckList()
        {
            SPWeb web = SPContext.Current.Web;
            string[] lstNames = new string[] { webObj.ActivityList, webObj.ActionList, webObj.MyActionList };
            SPList list;
            foreach (string lstName in lstNames)
            {
                list = web.Lists.TryGetList(webObj.ActivityList);
                if (list == null)
                {
                    lblMsg.Text = lstName + " 列表不存在！";
                    return false;
                }
            }
            return true;
        }
        private void InitMultiActivity()
        {
            DateTime defDt = GetDefaultDate();
            dtCurrentDate.SelectedDate = defDt;// DateTime.Now.Date;
            InitTaskList();
            //InitDdlControl();
            InitGridView();
        }
        /// <summary>
        /// 先检查并创建任务,来源（1、日历2、操作3、活动）
        ///<param name="ischkDateType">1=用户自行切换；0=根据日期判断</param>
        /// </summary>
        private void InitGridView(int ischkDateType=0)
        {
            DateTime currDate = dtCurrentDate.SelectedDate;
            if (ischkDateType == 0)//如果用户自动切换类型，则不执行
            {
                List<DateTime> retDts = new List<DateTime>();
                int dtype = GetDaysType(webObj.CalendarList, new List<DateTime> { currDate }, 0, ref retDts);
                chkDayType.Checked = dtype == 1 ? true : false;
                //chkDayType.Text = dtype == 1 ? "工作日" : webObj.NonWorkday;
                ViewState["chkDayType"] = dtype;
            }
            SPUser cuser = SPContext.Current.Web.CurrentUser;
            if (cuser != null)//已登录
            {
                int userId = cuser.ID;
                DataTable dtSource;
                DataTable dtResult = CreateData();
                SPListItemCollection items = GetCurrentDateActivity(userId, currDate);
                if (items.Count > 0)//日期类型只在第一次录入时可能更改
                    chkDayType.Enabled = false;
                else
                    chkDayType.Enabled = true;

                SPFieldLookupValue actionID;
                List<string> actions = new List<string>();
                //int durings = 0;
                int icount = 1;
                foreach (SPListItem item in items)
                {
                    DataRow dr = dtResult.Rows.Add();
                    actionID = new SPFieldLookupValue(item["ActionID"].ToString());
                    dr["ID"] =icount.ToString() +"_" +actionID.LookupId.ToString ();
                    dr["操作"] = actionID;
                    actions.Add(actionID.LookupValue);
                    dr["频次"] = 0;
                    dr["说明"] = item["Desc"];
                    if (item["Quantity"] != null)
                        dr["数量"] = item["Quantity"];
                    if (item["During"] != null)
                        dr["时长"] = item["During"];
                    //if (item["During"] != null)
                    //    durings += int.Parse(item["During"].ToString ());
                    if (item["TaskID"] != null)
                        dr["TaskID"] = new SPFieldLookupValue(item["TaskID"].ToString()).LookupId;
                    else
                        dr["TaskID"] = 0;
                    dr["Flag"] = 1;
                    dr["ItemID"] = item.ID;
                    dr["ActivityDate"] = item["创建时间"];
                    icount = icount + 1;
                }
                dtResult.AcceptChanges();

                #region //读取日历中的活动
                if (dtResult.Rows.Count < webObj.ActionCount)
                {
                    List<SPListItem> calendarItems = GetCalendarActivity(webObj.CalendarList, userId, currDate);
                    bool isNew = true;
                    foreach (SPListItem calenItem in calendarItems)
                    {
                        isNew = true;
                        TimeSpan subValue = ((DateTime)calenItem["EndDate"]).Subtract((DateTime)calenItem["EventDate"]);
                        int during = 0;// subValue.Minutes + subValue.Hours * 60;
                        if (items.Count > 0)//当日有活动
                        {
                            foreach (SPListItem tmp in items)
                            {
                                if (tmp["Flag"] != null && tmp["Flag"].ToString() == "1" && tmp["ActionID"].ToString() == calenItem["ActionID"].ToString())
                                {

                                    if (tmp["Desc"] != null && tmp["Desc"].ToString().Contains(((DateTime)calenItem["EventDate"]).ToShortTimeString()))
                                    {
                                        isNew = false;
                                        break;
                                    }

                                }

                            }
                        }
                        if (isNew)
                        {
                            if (calenItem["ActionID"] == null)//日历中活动的操作不能为空
                                continue;
                            DataRow dr = dtResult.Rows.Add();
                            actionID = new SPFieldLookupValue(calenItem["ActionID"].ToString());
                            dr["ID"] = icount.ToString() + "_" + actionID.LookupId.ToString();
                            dr["操作"] = actionID;
                            actions.Add(actionID.LookupValue);
                            dr["频次"] = calenItem.ID;
                            string des = "";
                            //if (calenItem["Title"] != null)
                            //    des = des + calenItem["Title"];
                            //if (calenItem["Location"] != null)
                            //    des = des + calenItem["Location"];
                            //if (calenItem["Description"] != null)
                            des = des + calenItem["Description"];
                            dr["说明"] = des;
                            dr["时长"] = during;

                            dr["TaskID"] = GetTaskIDByAction(actionID.LookupId, currDate);
                            dr["Flag"] = 2;
                            dr["ItemID"] = 0;// calenItem.ID;
                            dr["ActivityDate"] = calenItem["创建时间"];
                            icount = icount + 1;
                        }
                    }
                }
                #endregion

                if (dtResult.Rows.Count < webObj.ActionCount)
                {
                    DateTime dtFrom = dtCurrentDate.SelectedDate.AddDays(-webObj.BeforeDays - 7);//在基础上又延长7天
                    List<DateTime> retdts = new List<DateTime>();
                    //获取工作日的具体日期
                    GetDaysType(webObj.CalendarList, new List<DateTime> { dtFrom, dtCurrentDate.SelectedDate.AddDays(-1) }, chkDayType.Checked ? 1 : 0, ref retdts);

                    dtSource = GetPreData(userId, retdts);//操作不应该重复
                                                          //if (dtSource != null)
                                                          //{
                    DataTable dt = GetDistTable(dtSource, ViewState["CustAction"].ToString(), webObj.ActionCount - dtResult.Rows.Count, actions, dtCurrentDate.SelectedDate);
                    dt.DefaultView.Sort = " Date desc,ActivityDate ";
                    dt.DefaultView.RowFilter = "Flag=3";
                    dtResult.Merge(dt.DefaultView.ToTable());

                    dt.DefaultView.RowFilter = "Flag=4";
                    dtResult.Merge(dt.DefaultView.ToTable());
                    //}
                }
                DataBind(dtResult );
                ViewState["dtResult"] = dtResult;
                //计算时间和条数
                Caculate();
            }
            else
            {
                lblMsg.Text = "您尚未登录，无法快速录入活动！";
            }
        }
        //绑定gridview
        private void DataBind(DataTable dt)
        {
            dt.DefaultView.Sort = "Flag";//相当于Order By,ActivityDate
            //dt.DefaultView.RowFilter = "ID>5";//相当于Where
            gvActivities.DataSource = dt;
            gvActivities.DataBind();
            if (dt.Rows.Count == 0)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
        }
        #endregion
        #region 操作加背景颜色及相关
        private DataTable GetBindData(string listName, string parentId, int userID = 0)
        {
            if (userID == 0)
                userID = SPContext.Current.Web.CurrentUser.ID;
            DataTable dt = null;
            if (ViewState["dtAction"] != null)
                dt = (DataTable)ViewState["dtAction"];
            else
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
             {
                 using (SPSite mySite = new SPSite(SPContext.Current.Site.ID))
                 {
                     using (SPWeb web = mySite.OpenWeb(SPContext.Current.Web.ID))
                     {
                         SPList list = web.Lists.TryGetList(listName);//先到我的操作表中去找
                         SPQuery qry = new SPQuery
                         {
                             DatesInUtc = false
                         };
                         List<string> actions = new List<string>();
                         if (list != null)
                         {
                             qry.Query = "<Where><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Integer'>" + userID + @"</Value></Eq></Where><OrderBy><FieldRef Name='Frequency' Ascending='FALSE' /></OrderBy>";
                             dt = list.GetItems(qry).GetDataTable();
                             SPListItemCollection items = list.GetItems(qry);
                             SPListItem item;
                             if (dt != null && dt.Rows.Count > 0)
                             {
                                 foreach (DataRow dr in dt.Rows)
                                 {
                                     item = items.GetItemById((int)dr["ID"]);
                                     SPFieldLookupValue actionID = new SPFieldLookupValue(item["ActionID"].ToString());
                                     dr["ID"] = actionID.LookupId;
                                     dr["Title"] = actionID.LookupValue;
                                     actions.Add(actionID.LookupValue);
                                 }
                                 dt.AcceptChanges();
                             }
                         }
                         list = web.Lists.TryGetList(webObj.ActionList);
                         if (dt == null || dt.Rows.Count < list.ItemCount)// 我的操作设置了部分操作
                         {
                             qry = new SPQuery();
                             qry.Query = "<OrderBy><FieldRef Name='Frequency' Ascending='FALSE' /></OrderBy>";
                             DataTable dtALL = list.GetItems(qry).GetDataTable();
                             if (dt == null)
                                 dt = dtALL.Copy();
                             else
                             {
                                 foreach (DataRow drTemp in dtALL.Rows)
                                 {
                                     if (!actions.Contains(drTemp["Title"].ToString()))
                                     {
                                         DataRow dr = dt.NewRow();
                                         dr["ID"] = drTemp["ID"];
                                         dr["Title"] = drTemp["Title"];
                                         dr["Frequency"] = drTemp["Frequency"];
                                         dt.Rows.Add(dr);
                                     }
                                 }
                             }
                         }
                     }
                 }
             });
            }
            dt.DefaultView.Sort = "Frequency desc";
            dt = dt.DefaultView.ToTable();
            ViewState["dtAction"] = dt;
            return dt;
        }
        private SPListItemCollection allActions = null;
        private SPListItemCollection GetAllActions 
        {
             get
            {
                if (allActions==null )
                {
                    SPWeb web = SPContext.Current.Web;
                    SPList list = web.Lists.TryGetList(webObj.ActionList);//先到我的操作表中去找
                    SPListItemCollection items = list.Items;
                    allActions = items;
                }
                return allActions;
            }
        }
        /// <summary>
        /// 获取操作对应的任务文档，类型和操作在同一个表中 
        /// </summary>
        /// <returns></returns>
        private int  GetTaskIDByAction (int actionID,DateTime currDate)
        {
            //SPListItemCollection items = GetAllActions;
            //SPListItem item = items.GetItemById(actionID);
            //string typeName = "";
            //if (item["TypeID"] != null)
            //{
            //    SPFieldLookupValueCollection types = (SPFieldLookupValueCollection)item["TypeID"];
            //    foreach (SPFieldLookupValue type in types)
            //        if (type.LookupValue != "项目")
            //        {
            //            if (type.LookupValue != "工作" && type.LookupValue != "学习")
            //                typeName = "生活类";
            //            else
            //                typeName = type.LookupValue;
            //        }
            //}
            //if (typeName == "")
            //    typeName = "生活类";
            //string taskCurr = GetRoutineDocName(Account, typeName, currDate);
            //int taskID = GetTaskID(taskCurr);
            return 0;
        }
        private void BindDdlActions(DropDownList ddl,DataTable dt)
        {
            {
                ddl.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    ListItem li = new ListItem(dr["Title"].ToString(), dr["ID"].ToString());
                    ddl.Items.Add(li);
                    string flag = dr["Frequency"].ToString();
                    ddl.Items[i].Attributes.Add("style", GetStyle(flag));
                }
            }
        }
        private string GetStyle(string flag)
        {
            string style = "background-color:#F0F0F0";
            switch (flag)
            {
                case "6":
                    style = "background-color:#FFCCCC";
                    break;
                case "5":
                    style = "background-color: #FFE5CC";
                    break;
                case "4":
                    style = "background-color:#FFFFCC";
                    break;
                case "3":
                    style = "background-color:#CCE5FF";
                    break;
                case "2":
                    style = "background-color:#CCFFCC";
                    break;
                case "1":
                    style = "background-color:#F0F0F0";
                    break;
                default:
                    style = "background-color:#F0F0F0";
                    break;
            }
            return style;
        }
        #endregion
       
        #region 任务相关
        //活动中的任务ID字段
        private SPList GetLookupLists(SPWeb spWeb, SPList spList, string fldName)
        {
            SPFieldLookup task = spList.Fields.GetFieldByInternalName(fldName) as SPFieldLookup;
            SPList pList = spWeb.Lists[new Guid(task.LookupList)];
            ViewState["TaskListName"] = pList.Title; 
            return pList;
        }
        private void AddSubTask(string taskListName, string account, List<string> types, string title, int parentID,bool addAccount)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite mySite = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb myWeb = mySite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList taskList = myWeb.Lists.TryGetList(taskListName);
                        SPListItem item;
                        string docName;
                        int id2;//类型
                        int id1;//人员
                        SPFile  docStream;
                        int usrID = SPContext.Current.Web.CurrentUser.ID;
                        //SPFieldUrlValue taskDocUrl;
                        myWeb.AllowUnsafeUpdates = true;
                        docStream = GetTemplateStream(taskListName);
                        if (addAccount)//添加人员
                        {
                            item = taskList.Items.Add();
                            item["Title"] = account;
                            item["ParentID"] = parentID;
                            item["Author"] = usrID;
                            item["Editor"] = usrID;
                            item.Update();
                            id1 = item.ID;
                            foreach (string type in types)
                            {
                                item = taskList.Items.Add();
                                item["Title"] = type;
                                item["ParentID"] = id1;
                                item["Author"] = usrID;
                                item["Editor"] = usrID;
                                item.Update();
                                id2 = item.ID;
                                //上传文档并添加链接
                                docName = GetRoutineDocName(account, type,DateTime.Now.Date );
                                //taskDocUrl = UpdateDocumentToOnedrive(docStream, docName);
                                item = taskList.Items.Add();
                                AddRoutineTask(ref item, docStream, docName, id2, usrID);
                            }
                        }
                        else if (types.Count > 0)
                        {
                            foreach (string type in types)
                            {
                                item = taskList.Items.Add();
                                item["Title"] = type;
                                item["ParentID"] = parentID;
                                item["Author"] = usrID;
                                item["Editor"] = usrID;
                                item.Update();
                                id2 = item.ID;
                                //上传文档并添加链接
                                docName = GetRoutineDocName(account, type,DateTime.Now.Date );
                                //taskDocUrl = UpdateDocumentToOnedrive(docStream, docName);
                                item = taskList.Items.Add();
                                AddRoutineTask(ref item, docStream, docName, id2, usrID);
                            }
                        }
                        else if (title != "")
                        {
                            //上传文档并添加链接
                            docName = title;
                            item = taskList.Items.Add();
                            AddRoutineTask(ref item, docStream, docName, parentID, usrID);
                        }
                        myWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        private void AddRoutineTask( ref SPListItem item, SPFile docStream,string docName,int parentID,int usrID )
        {
            SPFieldUrlValue taskDocUrl = UpdateDocumentToOnedrive(docStream, docName);
            
            item["Title"] = docName;
            item["ParentID"] = parentID;
            item["TaskDoc"] = taskDocUrl;
            item["Author"] = usrID;
            item["Editor"] = usrID;
            item["AssignedTo"] = usrID;
            int pValue;
            if (docName.Contains("月"))
            {
                pValue = DateTime.Now.Month;
                item["StartDate"] = new DateTime(DateTime.Now.Year, pValue, 1);
                if (pValue == 12)
                    item["DueDate"] = new DateTime(DateTime.Now.Year, pValue, 31);
                else
                    item["DueDate"] = new DateTime(DateTime.Now.Year, pValue + 1, 1).AddDays(-1);
            }
            else if (docName.Contains("周"))
            {
                pValue = (int)DateTime.Now.Date.DayOfWeek ;
                if (pValue == 0) pValue = 7;//周日，每周从周一开始
                item["StartDate"] = DateTime.Now.Date.AddDays(-pValue + 1);
                item["DueDate"] = DateTime.Now.Date.AddDays(7 - pValue);
            }
            
            item.Update();
        }
        //检查子任务是否存在，通过快速编辑创建的时候，需要触发编辑的事件才能创建
        //在加载的时候，创建Routine子任务
        private void CheckSubTask(string account,  SPList list)
        {
            List<string> typeNames = new List<string> { "工作", "学习", "生活类" };
            int parentID;
            int parentID2;
            string taskDocName;
            string root = "Routine";
            SPQuery qry = new SPQuery();
            qry.ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='ParentID' />";
            qry.Query = @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + root + "</Value></Eq></Where>";
            SPListItemCollection items = list.GetItems(qry);
            if (items.Count == 0)
                return;
            parentID = items[0].ID; //获取用户
            qry = new SPQuery();
            qry.ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='ParentID' />";
            qry.Query = @"<Where><And><Eq><FieldRef Name='Title' /><Value Type='Text'>" + account + "</Value></Eq><Eq><FieldRef Name='ParentID' LookupId='True' /><Value Type='Lookup'>" + parentID + "</Value></Eq></And></Where>";
            items = list.GetItems(qry);
            if (items.Count == 0)
            {
                AddSubTask(list.Title, account, typeNames, "", parentID,true);
            }
            else
            {
                parentID = items[0].ID; //获取用户下面的类别 
                foreach (string typeName in typeNames)
                {
                    qry = new SPQuery();
                    qry.ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='ParentID' />";
                    qry.Query = @"<Where><And><Eq><FieldRef Name='Title' /><Value Type='Text'>" + typeName + "</Value></Eq><Eq><FieldRef Name='ParentID' LookupId='True' /><Value Type='Lookup'>" + parentID + "</Value></Eq></And></Where>";
                    items = list.GetItems(qry);
                    if (items.Count == 0)
                    {
                        AddSubTask(list.Title, account, new List<string> { typeName } , "", parentID,false );
                    }
                    else
                    {
                        parentID2 = items[0].ID; //获取类别下面的任务 
                        taskDocName = GetRoutineDocName(account, typeName,DateTime.Now.Date );
                        qry = new SPQuery();
                        qry.ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='ParentID' />";
                        qry.Query = @"<Where><And><Eq><FieldRef Name='Title' /><Value Type='Text'>" + taskDocName + "</Value></Eq><Eq><FieldRef Name='ParentID' LookupId='True' /><Value Type='Lookup'>" + parentID2 + "</Value></Eq></And></Where>";
                        items = list.GetItems(qry);

                        if (items.Count == 0)
                        {
                            AddSubTask(list.Title, account, new List<string>(),taskDocName , parentID2,false );
                        }
                        //return taskID;
                    }
                }
            }
        }
        /// <summary>
        /// 返回文档名称(即任务名称)，没有扩展名
        /// </summary>
        private string GetRoutineDocName(string account, string typeName,DateTime  dtCurrent    )
        {
             
            GregorianCalendar gc = new GregorianCalendar();
            string titleName = account + dtCurrent.ToString("yyyy") + "年";
            titleName += dtCurrent.Month + "月";
            //if (typeName == "生活类")
            //{
            //    titleName += dtCurrent.Month + "月";
            //}
            //else if (typeName == "工作" || typeName == "学习")
            //{
            //    int weekOfYear = gc.GetWeekOfYear(dtCurrent, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            //    if (weekOfYear == 53)
            //    {
            //        titleName = account + dtCurrent.AddYears(1).ToString("yyyy") + "年";
            //        weekOfYear = 1;//下一年的第一周
            //    }
            //    titleName += "第" + weekOfYear + "周";
            //}
            return titleName + typeName;
        }
        //获取模板文件
        private SPFile  GetTemplateStream(string taskListName)
        {
            string taskDocLibName = taskListName + "库";// 项目所对应的文档库
            SPFile file=null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPDocumentLibrary library = web.Lists[taskDocLibName] as SPDocumentLibrary;
                        string docType = "Excel工作簿.xlsx";// "Word文档.docx";
                        string libraryRelativePath = library.RootFolder.ServerRelativeUrl;
                        string libraryPath = site.MakeFullUrl(libraryRelativePath);//带网站集的url
                        string templatePath = libraryPath + "/Forms/" + docType;// +fileType;// web.Url + "/" + library.DocumentTemplateUrl;  
                        file = web.GetFile(templatePath);//获取模板文件
                     }
                }
            });
            return file ;
        }
       //获取用户登录名称
        private string Account
        {
            get
            {
                if (ViewState["account"]==null )
                {
                    SPUser user = SPContext.Current.Web.CurrentUser;
                    string lngAccount = user.LoginName.Substring(user.LoginName.IndexOf("\\") + 1);
                    if (lngAccount == "system") lngAccount = "xueqingxia";
                    ViewState["account"] = lngAccount;
                }
                return ViewState["account"].ToString();
            }
        }
        //获取用户登录UID
        private int UserID
        {
            get
            {
                if (ViewState["userID"] == null)
                {
                    SPUser user = SPContext.Current.Web.CurrentUser;
                    int userID = user.ID;
                    ViewState["userID"] = userID ;
                }
                return (int)ViewState["userID"] ;
            }
        }
        private SPFieldUrlValue UpdateDocumentToOnedrive(SPFile  fileStream, string fileName)
        {
            SPFieldUrlValue urlValue = null;
            
            try
            {
                string personalUrl = "http://localhost/personal/" + Account ;//进入个人网站

                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite spSite = new SPSite(personalUrl))
                    {
                        using (SPWeb sWeb = spSite.OpenWeb())
                        {
                            sWeb.AllowUnsafeUpdates = true;
                            SPDocumentLibrary library = sWeb.Lists["文档"] as SPDocumentLibrary;
                            string libraryRelativePath = library.Folders[0].Url;
                            string libraryPath = spSite.MakeFullUrl(libraryRelativePath);//带网站集的url
                            string documentPath = libraryPath + "/" + fileName+".xlsx";// documentName + fileType;//创建的文档名称
                            SPFile sFile;
                            try
                            {
                                byte[] docFile = fileStream.OpenBinary();
                                sFile = sWeb.Files.Add(documentPath, docFile , true);//
                                sFile.Update();
                                sFile.CheckIn("", SPCheckinType.MajorCheckIn);
                                sFile.Update();
                            }
                            catch (Exception ex)
                            {
                                sFile = sWeb.GetFile(documentPath);
                            }
                            if (sFile.Exists)
                            {
                                urlValue = new SPFieldUrlValue();
                                int index = sFile.Name.IndexOf(".");//去掉扩展名
                                string docName = sFile.Name.Remove(index, sFile.Name.Substring(index).Length);
                                docName = docName.Replace(Account, "");
                                urlValue.Description = docName;
                                urlValue.Url = documentPath;

                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // "此文档已经存在或者文件类型被限制，请重新选择！";
            }
            return urlValue;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 获取用户活动的最后一条带任务的数据，
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="actionID"></param>
        /// <returns></returns>
        private int GetActivityTaskID(int userId, int actionID)
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(webObj.ActivityList);
            SPListItemCollection items = null;
            int taskID = 0;
            if (list != null)
            {
                SPQuery qry = new SPQuery();
                qry.Query = @"<Where>
                                  <And>
                                     <Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>" + userId + @"</Value>
                                     </Eq>
                                     <Eq>
                                        <FieldRef Name='ActionID' LookupId='True' />
                                        <Value Type='Lookup'>" + actionID + @"</Value>
                                     </Eq>
                                  </And>
                               </Where><OrderBy><FieldRef Name='TaskID' Ascending='FALSE' /></OrderBy>";
                items = list.GetItems(qry);
                if (items.Count > 0 &&  items[0]["TaskID"]!=null)
                    taskID = new SPFieldLookupValue(items[0]["TaskID"].ToString ()).LookupId; 
            }
            return taskID ;
        }
        /// <summary>
        /// 获取当前日期的活动
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private SPListItemCollection GetCurrentDateActivity(int userId, DateTime time)
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(webObj.ActivityList);
            string dateFld = "Date";
            SPListItemCollection items = null;
            if (list != null)
            {
                SPQuery qry = new SPQuery();
                qry.Query = @"<Where>
                                  <And>
                                     <Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>" + userId + @"</Value>
                                     </Eq>
                                     <Eq>
                                        <FieldRef Name='" + dateFld + @"' />
                                        <Value Type='DateTime'>" + time.ToString("yyyy-MM-dd") + @"</Value>
                                     </Eq>
                                  </And>
                               </Where><OrderBy><FieldRef Name='Created' Ascending='true' /></OrderBy>";
                items = list.GetItems(qry);
            }
            return items ;
        }
        private DataTable GetPreData(int userId, List<DateTime> dtDate)
        {
            DataTable dt = new DataTable();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
             {
                 using (SPSite mySite = new SPSite(SPContext.Current.Site.ID))
                 {
                     using (SPWeb web = mySite.OpenWeb(SPContext.Current.Web.ID))
                     {
                         SPList list = web.Lists.TryGetList(webObj.ActivityList);

                         if (list != null)
                         {
                             //查找当前用户一周内的所有活动操作，去重并按照频率由高到低排序，选取前25个操作，不够25个选择全部
                             SPListItemCollection items;
                             //获取最小的日期
                             DateTime time = dtDate[dtDate.Count - 1];// dtDate .AddDays(-webObj.BeforeDays );将参数的日期改过日期列表，去除掉非工作日
                             string timeStr = SPUtility.CreateISO8601DateTimeFromSystemDateTime(time);
                             SPQuery qry = new SPQuery();
                             SPField field;
                             string dateFld = "ActualDate";
                             if (list.Fields.ContainsFieldWithStaticName("CustAction"))
                             {
                                 field = list.Fields.GetFieldByInternalName("CustAction");
                                 ViewState["CustAction"] = field.InternalName; ;
                                 dateFld = "ActualDate";
                             }
                             else if (list.Fields.ContainsFieldWithStaticName("ActionID"))
                             {
                                 field = list.Fields.GetFieldByInternalName("ActionID");
                                 ViewState["CustAction"] = field.InternalName;//由Title改为InternalName
                                 dateFld = "Date";
                             }
                             string viewFld = "<FieldRef Name='ID' /><FieldRef Name='Quantity' /><FieldRef Name='During' /><FieldRef Name='Date' /><FieldRef Name='Title' /><FieldRef Name='ActionID' /><FieldRef Name='Desc' /><FieldRef Name='TaskID' />";
                             qry.ViewFields = viewFld;
                             qry.Query = @"<Where>
                                  <And>
                                     <Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>" + userId + @"</Value>
                                     </Eq>
                                     <Geq>
                                        <FieldRef Name='" + dateFld + @"' />
                                        <Value Type='DateTime'>" + time.ToString("yyyy-MM-dd") + @"</Value>
                                     </Geq>
                                  </And>
                               </Where>";

                             items = list.GetItems(qry);
                             if (items != null && items.Count <= 0)//没有该用户的记录,则用所有用户的记录来代替
                             {
                                 qry = new SPQuery();
                                 qry.ViewFields = viewFld;
                                 qry.Query = @"<Where>
                                         <Geq>
                                            <FieldRef Name='" + dateFld + @"' />
                                            <Value Type='DateTime'>" + time.ToString("yyyy-MM-dd") + @"</Value>
                                         </Geq>
                               </Where>";
                                 items = list.GetItems(qry);
                             }
                             dt = items.GetDataTable();// GetCamlDataListRetTable(items);

                             if (dt != null && dt.Rows.Count > 0)
                             {
                                 DataTable dtTmp = dt.Clone();

                                 DataRow[] drs;
                                 string sql;
                                 SPListItem item;
                                 foreach (DateTime dateTmp in dtDate)
                                 {
                                     sql = dateFld + "='" + dateTmp.ToString("yyyy-MM-dd") + "'";
                                     drs = dt.Select(sql, "Created");
                                     foreach (DataRow dr in drs)
                                     {
                                         item = items.GetItemById((int)dr["ID"]);
                                         dr["ActionID"] = item["ActionID"];
                                         dr["TaskID"] = item["TaskID"];
                                         dr.AcceptChanges();
                                         dtTmp.ImportRow(dr);
                                     }
                                 }

                                 dt = dtTmp.Copy();
                             }
                         }
                         else
                         {
                             dt = null;
                         }
                     }
                 }
             });
            }
            catch (Exception ex)
            {
                lblMsg.Text = dtDate.Count.ToString() + ":GetPreDate:" + ex.ToString();
                dt = null;
            }

            return dt;
        }

        /// <summary>
        /// 根据Caml查询返回数据表
        /// </summary>
        /// <param name="spic">Caml结果集</param>
        /// <returns></returns>
        public DataTable GetCamlDataListRetTable(SPListItemCollection spic)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < spic.List.ContentTypes[0].Fields.Count; i++)
            {
                if (dt.Columns == null || dt.Columns.Count < 1)
                    dt.Columns.Add(spic.List.ContentTypes[0].Fields[i].Title);
                else
                {
                    string colName = spic.List.ContentTypes[0].Fields[i].Title;
                    if (!dt.Columns.Contains(colName))
                    {
                        if (colName.Contains("数量") || colName.Contains("时长"))
                            dt.Columns.Add(colName, typeof(double ));
                        else
                            dt.Columns.Add(colName );
                    }
                   
                }
            }
            dt.Columns.Add("ID");
            dt.Columns.Add("创建时间",typeof(DateTime ));
            if (spic.List.ItemCount > 0)
            {
                for (int i = 0; i < spic.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (spic[i][dt.Columns[j].Caption] != null)
                            dr[dt.Columns[j].ColumnName] = spic[i][dt.Columns[j].ColumnName];// Convert.ToString(spic[i][spic[i].Fields[dt.Columns[j].ColumnName.ToString()].InternalName]);
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        private DataTable CreateData()
        {
            DataTable rTable = new DataTable();
            rTable.Columns.Add("ID",typeof(string));
            rTable.Columns.Add("操作");
            rTable.Columns.Add("频次");
            rTable.Columns.Add("时长", typeof(double));
            rTable.Columns.Add("数量", typeof(double));
            rTable.Columns.Add("说明");
            rTable.Columns.Add("TaskID", typeof(int));
            rTable.Columns.Add("Flag", typeof(int)).DefaultValue = 0;
            rTable.Columns.Add("ItemID", typeof(int));
            rTable.Columns.Add("ActivityDate", typeof(DateTime));//同一天的按时间降序
            rTable.Columns.Add("Date", typeof(DateTime));//日期，
            return rTable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="distField"></param>
        /// <param name="TotalCount">需要再获取的行数,</param>
        /// <returns></returns>
        private DataTable GetDistTable(DataTable dtSource, string distField,int TotalCount,List<string> actions,DateTime dtTime)
        {
            DataTable rTable = CreateData();
            //rTable.Columns.Add("地点");
                int iStart = webObj.ActionCount - TotalCount + 1;//为了获取序号，操作允许有重复
            if (dtSource != null)
            {
                DataTable dt1 = dtSource.DefaultView.ToTable(true, distField);
                int rows = dt1.Rows.Count <= TotalCount ? dt1.Rows.Count : TotalCount;//行数不够指定数，以行数为准
                SPFieldLookupValue tValue;
                int taskID = 0;
                string taskTitle;
                if (dtSource.Columns.Contains("TaskID"))
                    taskTitle = "TaskID";
                else
                    taskTitle = "任务文档";
                //找所有的，之后按频次排序，推出的会超过给出的条数
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    tValue = new SPFieldLookupValue(dt1.Rows[i][distField].ToString());
                    if (actions.Contains(tValue.LookupValue))//以有的操作不加
                        continue;
                    else
                        actions.Add(tValue.LookupValue);
                    DataRow dr = rTable.NewRow();
                    dr["ID"] = iStart.ToString() + "_" + tValue.LookupId;
                    dr["操作"] = dt1.Rows[i][distField];
                    dr["频次"] = dtSource.Compute("count(" + distField + ")", distField + "='" + dt1.Rows[i][distField] + "'");
                    object result = DBNull.Value;
                    result = dtSource.Compute("avg(During)", distField + "='" + dt1.Rows[i][distField] + "'");
                    if (result != DBNull.Value)
                    {
                        decimal during = decimal.Round(decimal.Parse(result.ToString()));
                        during = during - during % 10;
                        dr["时长"] = during;

                    }

                    dr["Flag"] = 3;
                    DataRow[] drs = dtSource.Select(distField + "='" + dt1.Rows[i][distField] + "'", "Created desc");
                    tValue = new SPFieldLookupValue(drs[0][taskTitle].ToString());
                    dr["TaskID"] = tValue.LookupId;
                    dr["Date"] = drs[0]["Date"];
                    taskID = GetActivityTaskIDByDate(tValue.LookupValue, dtTime);//推导的任务
                    if (taskID >= 0)
                        dr["TaskID"] = taskID;
                    dr["ItemID"] = 0;
                    dr["ActivityDate"] = drs[0]["Created"];
                    rTable.Rows.Add(dr);
                    iStart = iStart + 1;
                }
                rTable.DefaultView.Sort = "频次 desc";
                rTable = rTable.DefaultView.ToTable();
                if (rTable.Rows.Count > TotalCount)
                {
                    for (int i = rTable.Rows.Count - 1; i >= TotalCount; i--)
                        rTable.Rows.RemoveAt(i);
                    rTable.AcceptChanges();
                }
            }
            //通过操作推算
            if (rTable.Rows.Count < TotalCount)
            {
                DataTable dtActions = GetBindData(webObj.MyActionList, "");
                int iCount = 0;

                for (int i = rTable.Rows.Count; i < TotalCount; i++)
                {
                    while (dtActions.Rows.Count > iCount && actions.Contains(dtActions.Rows[iCount]["Title"].ToString()))
                        iCount = iCount + 1;
                    if (iCount == dtActions.Rows.Count)
                        break;
                    DataRow dr = rTable.NewRow();
                    dr["ID"] = iStart.ToString() + "_" + dtActions.Rows[iCount]["ID"];

                    dr["操作"] = dtActions.Rows[iCount]["ID"] + ";#" + dtActions.Rows[iCount]["Title"];//[distField];
                    //DataRow[] drs = dtSource.Select(distField + "='" + dt1.Rows[0][distField] + "'");

                    dr["频次"] = 0;
                    dr["时长"] = DBNull.Value;
                    dr["Flag"] = 4;//空的

                    dr["ItemID"] = 0;
                    dr["TaskID"] = GetTaskIDByAction((int)dtActions.Rows[iCount]["ID"], dtTime);
                    rTable.Rows.Add(dr);
                    iCount = iCount + 1;
                    iStart = iStart + 1;
                }
            }
             
            return rTable;
        }
        
        /// <summary>
        /// 将推导出的任务ID变成当前与当前日期相关的，生活类和工作，学习
        /// 如果新用户录入数据，则开头的用户名不是当前用户
        /// </summary>
        /// <param name="taskName"></param>
        /// <returns></returns>
        private int GetActivityTaskIDByDate(string taskName, DateTime currDate)
        {
            int taskID = 0;
            //判断是Routine任务
            bool isRoutine = TaskIsRoutine(taskName);
            if (isRoutine )//taskName.StartsWith(Account))//只处理Routine任务
            {
                //string typeName = taskName.Substring(taskName.Length - 3);
                //if (typeName != "生活类")
                //    typeName = typeName.Substring(1);
                //string taskCurr = GetRoutineDocName(Account, typeName, currDate);
                //taskID = GetTaskID(taskCurr);
                taskID = 0;
            }
            return taskID;
        }
        /// <summary>
        /// 将任务名称转为当前日期的任务名称
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="currDate"></param>
        /// <returns></returns>
        private string GetActivityTaskByDate(string taskName, DateTime currDate)
        {
           string taskCurr="";

            if (taskName.StartsWith(Account))//只处理Routine任务
            {
                string typeName = taskName.Substring(taskName.Length - 3);
                if (typeName != "生活类")
                    typeName = typeName.Substring(1);
                 taskCurr = GetRoutineDocName(Account, typeName, currDate);
            }
            return taskCurr;
        }
        #endregion
        #region 单条数据的录入和编辑方法
        /// <summary>
        /// 写入活动与附件的关系
        /// </summary>
        /// <param name="ids">选中的附件ID</param>
        /// <param name="assistantID">活动ID</param>
        /// <returns></returns>
        private void WriteResultToList(string[] ids, int assistantID, SPWeb sWeb)
        {
            SPUser user = SPContext.Current.Web.CurrentUser;
            // SPSecurity.RunWithElevatedPrivileges(delegate ()
            //{
            //    using (SPSite spSite = new SPSite(SPContext.Current.Site.ID))
            //    {
            //        using (SPWeb sWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
            //        {
            SPList sList = sWeb.Lists.TryGetList(webObj.ListMediaRel);
            if (sList == null)
            {
                lblMsg.Text = webObj.ListMediaRel + " 列表不存在！";
            }
            else
            {
                lblMsg.Text = "";
                SPQuery qry = new SPQuery();
                SPListItemCollection docITems = null;
                SPListItem item;
                //当前个人学习记录的文档
                //sWeb.AllowUnsafeUpdates = true;
                foreach (string id in ids)
                {
                    qry = new SPQuery();
                    qry.ViewFields = "<FieldRef Name='ID' /><FieldRef Name='Author' /><FieldRef Name='ResultID' /><FieldRef Name='AssistantID' /><FieldRef Name='Title' />";
                    qry.Query = @"<Where><And><Eq><FieldRef Name='AssistantID'/><Value Type='Number'>" + assistantID + "</Value></Eq><Eq><FieldRef Name='ResultID'/><Value Type='Number'>" + id + "</Value></Eq></And></Where>";

                    docITems = sList.GetItems(qry);
                    if (docITems.Count == 0)
                    {
                        item = sList.Items.Add();
                        item["AssistantID"] = assistantID;
                        item["ResultID"] = id;
                        item["修改者"] = user.ID;
                        item["创建者"] = user.ID;
                        item.Update();
                    }
                }
                //            sWeb.AllowUnsafeUpdates = false;
            }
            //    }
            //}
            //});
        }
        /// <summary>
        /// 获取操作的活动类型
        /// </summary>
        private string GetActionType(int actionID)
        {
            SPListItemCollection items = GetAllActions;
            SPListItem item = items.GetItemById(actionID);
            string typeName = "";
            if (item["TypeID"] != null)
            {
                SPFieldLookupValueCollection types = (SPFieldLookupValueCollection)item["TypeID"];
                foreach (SPFieldLookupValue type in types)
                    typeName =typeName+ type.LookupValue + "；";
            }
            if (typeName.Length > 0)
                typeName = typeName.TrimEnd('；');
            return typeName;
        }
        /// <summary>
        /// 初始化字段的说明，从列表中读取
        /// </summary>
        private void InitFieldDesc()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(webObj.ActivityList);
            spanHours.InnerText = list.Fields.GetField("During").Description;
            spanNormal.InnerText = list.Fields.GetField("IsNormal").Description;
            spanQuantity.InnerText =list.Fields.GetField("Quantity").Description;
            spanTasks.InnerText =list.Fields.GetField("TaskID").Description;
        }
        /// <summary>
        /// Routine去掉，只有项目任务
        /// </summary>
        private void InitTaskList()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(webObj.ActivityList);

            SPList taskList = GetLookupLists(web, list, "TaskID");//获取活动关联的任务文档列表
            //CheckSubTask(Account, taskList);//创建当前的所有任务
        }
        /// <summary>
        /// 初始化控件,新建或编辑时
        /// </summary>
        private void InitDdlControl(int userID = 0)
        {
            DataTable dtSource = GetBindData(webObj.MyActionList, "",userID );

            BindDdlActions(ddlActions, dtSource.Copy());
            //BindDropDownList(ddlActions, "ID", "Title", dtSource.Copy());

            dtSource = GetBindDataOfTask(ViewState["TaskListName"].ToString(),dtCurrentDate.SelectedDate );
           
            //任务绑定
            BindDropDownList(ddlTypes, "ID", "Title", dtSource);
        }
        private void InitSingleActivity()
        {
            InitFieldDesc();
            SPWeb web = SPContext.Current.Web;
            int id = 0;
            int userID = 0;
            bool hasRight=false;
            if (Request.QueryString["ID"] != null)
            {
                id = int.Parse(Request.QueryString["ID"].ToString());
                if (id > 0)
                {
                    hasRight = UserHasAttachRight(SPContext.Current.Web.CurrentUser, id, ref userID);
                    if (userID == 0)//当前用户
                        ViewState["CurrentUser"] = 1;//用户编辑自己的活动 
                }
            }
            else
                ViewState["CurrentUser"] = 1;//用户新建 自己的活动 

            InitTaskList();//检查任务

            SPList list = web.Lists.TryGetList(webObj.ActivityList);
            ViewState["listUrl"] = list.DefaultViewUrl;
            if (id > 0)//编辑 
            {
                SPListItem item = list.GetItemById(id);
                if (item != null)
                {
                    dtCurrentDate.SelectedDate = DateTime.Parse(item["Date"].ToString());
                    InitDdlControl(userID );//需要获取日期
                    if (item["Desc"] != null)
                        txtDesc.Text = item["Desc"].ToString();
                    if (item["During"] != null)
                        txtHours.Text = item["During"].ToString();
                    if (item["Quantity"] != null)
                        txtQuantity.Text = item["Quantity"].ToString();
                    int actionID = new SPFieldLookupValue(item["ActionID"].ToString()).LookupId;
                    SelectDropDownList(ddlActions, actionID.ToString());
                     
                    if (item["TaskID"] != null)
                        SelectDropDownList(ddlTypes, (new SPFieldLookupValue(item["TaskID"].ToString())).LookupId.ToString());
                    else
                         SelectDropDownList(ddlTypes, "0");

                    //ddlActions.SelectedItem.Value = actionID.ToString();
                    //ddlTypes.SelectedItem.Value = (new SPFieldLookupValue(item["TaskID"].ToString())).LookupId.ToString();
                    cbNormal.Checked = (bool)item["IsNormal"];
                    lblTypeShow.Text = "操作类型：" + GetActionType(actionID);
                }
                if (!hasRight)
                {
                    btnSave.Enabled = false;
                    lblMsg.Text = "您无权编辑！";
                }
            }
            else//新建
            {
                DateTime defDt = GetDefaultDate();
                dtCurrentDate.SelectedDate = defDt;

                InitDdlControl(userID );
                /// 更改任务文档,根据操作类型找任务
                int tastID = GetTaskIDByAction(int.Parse(ddlActions.SelectedItem.Value), dtCurrentDate.SelectedDate);
                // GetActivityTaskID(SPContext.Current.Web.CurrentUser.ID, int.Parse(ddlActions.SelectedItem.Value));
                SelectDropDownList(ddlTypes, tastID.ToString());
                lblTypeShow.Text = "操作类型：" + GetActionType(int.Parse(ddlActions .SelectedItem.Value)   );
            }
        }
        /// <summary>
        /// //用户是否有权限,编辑的时候，创建者等于当前用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemID"></param>
        /// <param name="userID">创建者和当前用户不是同一人时，返回创建者iD</param>
        /// <returns></returns>
        private bool UserHasAttachRight(SPUser user, int itemID,ref int userID)
        {
            bool isRight = false;
            if (itemID == 0) return true;//新建
            using (SPSite spSite = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb sWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
                {
                    SPList sList = sWeb.Lists.TryGetList(webObj.ActivityList);
                    if (sList != null)
                    {
                        SPListItem item = sList.GetItemById(itemID);
                        SPFieldUserValue author = new SPFieldUserValue(sWeb, (item["创建者"].ToString()));
                        if (author.LookupId == user.ID)
                            isRight = true;
                        else
                        {//当前用户和创建者不是同一个用户
                            string lgName = author.User.LoginName.Substring(author.User.LoginName.IndexOf("\\") + 1);
                            if (lgName == "system") lgName = "xueqingxia";
                            ViewState["account"] = lgName;
                            userID = author.LookupId;
                            isRight = sList.DoesUserHavePermissions(user, SPBasePermissions.ApproveItems);
                        }

                    }
                }
            }
            return isRight;
        }
        #endregion
        #region 日历相关
        /// <summary>
        /// 读取日常任务中当天的活动
        /// </summary>
        /// <param name="calendarList"></param>
        /// <param name="userId"></param>
        /// <param name="dtCurrDate"></param>
        private List<SPListItem> GetCalendarActivity(string calendarList,int userId,DateTime dtCurrDate)
        {
            string strNow = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtCurrDate.AddHours(12));
            List<SPListItem> retItems = new List<SPListItem>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
             {
                 using (SPSite mySite = new SPSite(SPContext.Current.Site.ID))
                 {
                     using (SPWeb myWeb = mySite.OpenWeb(SPContext.Current.Web.ID))
                     {
                         SPList taskList = myWeb.Lists.TryGetList(calendarList);
                         SPQuery qry = new SPQuery();
                         qry.Query = @"<Where>
                                    <And>
                                     <Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>" + userId + @"</Value>
                                     </Eq>
                                        <DateRangesOverlap>
                                            <FieldRef Name='EventDate' />;
                                            <FieldRef Name='EndDate' />
                                            <FieldRef Name='RecurrenceID' />
                                            <Value Type='DateTime'><Today /></Value>
                                        </DateRangesOverlap>
                                    </And>
                                </Where>";

                         qry.ExpandRecurrence = true;
                         qry.CalendarDate = dtCurrDate;//Now
                         SPListItemCollection items = taskList.GetItems(qry);
                         List<string> types = new List<string>() { "工作日",webObj.NonWorkday };
                         if (items.Count > 0 && items[0]["Category"] != null)
                         {
                             foreach (SPListItem item in items)
                             {
                                 int t1 = DateTime.Compare((DateTime)item["EndDate"], dtCurrDate);
                                 if (t1>=0  && !types.Contains(item["Title"].ToString()))// && item["Category"]!= null
                                     retItems.Add ( item);
                             }
                         }

                     }
                 }
             });
            return retItems;
        }
        /// <summary>
        /// 判断给定日期，是否是工作日，1为工作日；0为节假日
        /// </summary>
        /// <param name="calendarList">日历列表的名称</param>
        /// <param name="dts">要遍历的日期范围</param>
        /// <param name="retDayType">要遍历的日期类型1-工作日；0-节假日</param>
        /// <param name="retDt">返回的日期列表，工作日或节假日</param>
        /// <returns></returns>
        private int GetDaysType (string calendarList,List<DateTime> dts,int retDayType,ref List<DateTime> retDt)
        {
            int dayType = -1;
            List<DateTime> retDates = new List<DateTime>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
             {
                 using (SPSite mySite = new SPSite(SPContext.Current.Site.ID))
                 {
                     using (SPWeb myWeb = mySite.OpenWeb(SPContext.Current.Web.ID))
                     {
                         SPList taskList = myWeb.Lists.TryGetList(calendarList);
                         SPQuery qry = new SPQuery();
                         for (DateTime dt1 = dts[dts.Count - 1]; dt1 >= dts[0]; dt1=dt1.AddDays(-1))
                         {
                             dayType = -1;
                             //strDt = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dt1.AddHours(12));
                             qry = new SPQuery();
                             qry.Query = @"<Where>
                                    <And>
                                     <Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>" + UserID + @"</Value>
                                     </Eq>
                                        <DateRangesOverlap>
                                            <FieldRef Name='EventDate' />;
                                            <FieldRef Name='EndDate' />
                                            <FieldRef Name='RecurrenceID' />
                                            <Value Type='DateTime'><Today /></Value>
                                        </DateRangesOverlap>
                                       </And>
                                     </Where>";

                             qry.ExpandRecurrence = true;
                             qry.CalendarDate = dt1;//Now
                             SPListItemCollection items = taskList.GetItems(qry);
                             foreach (SPListItem item in items)
                             {
                                 int t1 = DateTime.Compare((DateTime)item["EndDate"], dt1);
                                 if (t1 >= 0 && items[0]["Title"] != null)
                                 {
                                     if (items[0]["Title"].ToString() == webObj.NonWorkday )
                                     { dayType = 0; break; }
                                     else if (items[0]["Title"].ToString() == "工作日")
                                     { dayType = 1; break; }
                                 }
                             }
                             if (dayType == -1)//日历中没有标记
                             {
                                 if (dt1.DayOfWeek == DayOfWeek.Saturday || dt1.DayOfWeek == DayOfWeek.Sunday)//6,0
                                     dayType = 0;
                                 else
                                     dayType = 1;
                             }
                             if (dts.Count >1 && dayType == retDayType)
                             {
                                 retDates.Add(dt1);
                                 if (retDates.Count == webObj.BeforeDays)
                                     break;
                             }

                         }
                     }
                 }
             });
            retDt = retDates;
            return dayType ;
        }
        #endregion

    }
}
