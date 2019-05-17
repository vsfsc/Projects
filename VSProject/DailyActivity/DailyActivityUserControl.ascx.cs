using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSDLL;


namespace VSProject.DailyActivity
{
    public class DailyActivityUserControl:UserControl
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
        /// dtCurrentDate 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::Microsoft.SharePoint.WebControls.DateTimeControl dtCurrentDate;

        /// <summary>
        /// btnNext 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnNext;

        /// <summary>
        /// spShowInfo 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl spShowInfo;

        /// <summary>
        /// cblistLocal 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.CheckBoxList cblistLocal;

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
        /// btn7Days 控件。
        /// </summary>
        /// <remarks>
        /// 自动生成的字段。
        /// 若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btn7Days;

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


        protected Button btnNew;
        #endregion
        #region 属性
        public DailyActivity webObj { get; set; }
        //获取用户登录名称
        private string Account
        {
            get
            {
                if (ViewState["account"] == null)
                {
                    SPUser user = SPContext.Current.Web.CurrentUser;
                    string lngAccount = user.LoginName.Substring(user.LoginName.IndexOf("\\") + 1);
                    //if (lngAccount == "system") lngAccount = "xueqingxia";
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
                    //SPUser user = SPContext.Current.Web.CurrentUser;
                    int userID = VSDLL.Common.Users.UserID;
                    ViewState["userID"] = userID;

                }
                return (int)ViewState["userID"];
            }
        }
        #endregion
        #region 事件
        /// <summary>
        /// 页面加载事件，设置控件的事件连接，用户的权限判断，是批量录入还是单条录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            gvActivities.RowDataBound += gvActivities_RowDataBound;
            btnPre.Click += BtnPre_Click;
            btnNext.Click += BtnNext_Click;
            btnCancel.Click += BtnCancel_Click;
            btnNew.Click += btnNew_Click;
            ddlActions.SelectedIndexChanged += ddlActions_SelectedIndexChanged;
            dtCurrentDate.DateChanged += dtCurrentDate_DateChanged;
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
                if (webObj.QuickEdit == 1)
                {
                    tbQuick.Visible = true;
                    tbSingle.Visible = false;
                    InitMultiActivity();
                    udesp.Visible = true;
                }
                else
                {
                    //chkDayType.Visible = false;
                    tbQuick.Visible = false;
                    tbSingle.Visible = true;
                    udesp.Visible = false;
                    InitSingleActivity();
                }
                BindLocals();
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
            BindDdlActionsStyle(ddl );
            GridViewRow row = ddl.Parent.Parent as GridViewRow;
            DropDownList ddlTask = row.FindControl("ddlTasks") as DropDownList;
            //更改任务文档
            int tastID = GetActivityTaskID(SPContext.Current.Web.CurrentUser.ID, int.Parse(ddl.SelectedItem.Value));
            SelectDropDownList(ddlTask, tastID.ToString());
            Caculate();
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
        //前一天按钮事件
        private void BtnPre_Click(object sender, EventArgs e)
        {
            DateTime dt = dtCurrentDate.SelectedDate.AddDays(-1);
            CheckDateValue(ref dt);
            dtCurrentDate.SelectedDate = dt;
            if (webObj.QuickEdit == 1)
                InitGridView();
        }
        //后一天按钮事件
        private void BtnNext_Click(object sender, EventArgs e)
        {
            DateTime dt = dtCurrentDate.SelectedDate.AddDays(1);
            CheckDateValue(ref dt);
            dtCurrentDate.SelectedDate = dt;
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
            DateTime dt = dtCurrentDate.SelectedDate;
            CheckDateValue(ref dt);
            if (dtCurrentDate.SelectedDate.CompareTo(dt) != 0)
            {
                lblMsg.Text = "日期不能小于首次录入活动日期（" + dt.ToString("yyyy-MM-dd") + "）";
                dtCurrentDate.SelectedDate = dt;
                return;
            }
            else
                lblMsg.Text = "";
            if (webObj.QuickEdit == 1)
                InitGridView();
        }

        /// <summary>
        /// 新增更多活动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, EventArgs e)
        {
            //gvActivities.ShowFooter = true;
            DataTable dtBind = GetChangedActivities(dtCurrentDate.SelectedDate);
            DataRow drAdd = dtBind.Rows.Add();
            drAdd["ActionID"] = 0;
            drAdd["ActivityID"] =-1;
            DataBind(dtBind);
            ShowChangedDurings();//重新计算时长

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
            DataTable dtChangedActivity=GetChangedActivities (dtCurrentDate.SelectedDate );
            DataRow drRiqi=GetChangedUserRiqi( dtCurrentDate.SelectedDate);
            string errMsg="";
            bool result = VSDLL.BLL.ActivityBLL.InsertActivities(dtChangedActivity,  drRiqi,ref errMsg );
            if (result)
                ViewState["dtActivity"] = dtChangedActivity;

            //跳转，一般进入“后一日”，如果日期改变后当日是明天时，则进入活动分析界面
            if (result)
            {
                if (dtDate == DateTime.Today.Date)
                {
                    if (Request.QueryString["Source"] != null)
                        Response.Redirect(Request.QueryString["Source"].ToString());
                    else
                    {
                        ShowChangedDurings();
                        lblMsg.Text = "保存成功！";
                    }

                }
                else
                {
                    dtCurrentDate.SelectedDate = dtDate.AddDays(1);
                    if (webObj.QuickEdit == 1)
                        InitGridView();
                }
            }
            else
                lblMsg.Text = "保存失败:"+errMsg ;

        }
        /// <summary>
        /// 更改的数据写到列表中
        /// </summary>
        /// <param name="results">有脏数据的行</param>
        /// <param name="dtDate">当前日期</param>
        /// <param name="dayType">日期类型</param>
    /// <summary>
        /// 活动列表主体区数据绑定事件，绑定操作（用户操作，如果没有则所有操作）和任务（按层次显示与用户相关的任务）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTask = e.Row.FindControl("lblTaskID") as Label;
                //操作绑定lblTaskID
                DropDownList ddlActions = e.Row.FindControl("ddlActions") as DropDownList;
                DataTable dtSource = GetBindDataOfAction();

                BindDdlActions(ddlActions, dtSource.Copy());
                Label  lblAction =e.Row.FindControl("lblActionID") as Label;;// gvActivities.DataKeys[e.Row.RowIndex]["ID"].ToString();
                SelectDropDownList(ddlActions, lblAction.Text );

                dtSource = GetBindDataOfTask();
                //任务绑定
                DropDownList ddlTypes = e.Row.FindControl("ddlTasks") as DropDownList;
                BindDropDownList(ddlTypes, "ID", "Title", dtSource);
                SelectDropDownList(ddlTypes, lblTask.Text);
            }
        }

        /// <summary>
        /// 计算时长和选中的条数
        /// </summary>
        private void Caculate()
        {
           ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "null", "cacul();", true);
            //int durings = 0;
            //int count = 0;
            //foreach (GridViewRow gvr in gvActivities.Rows)
            //{
            //    TextBox txtHours = ((TextBox)gvr.FindControl("tbDuring"));
            //    count += 1;
            //    if (txtHours.Text != "")
            //        durings += int.Parse(txtHours.Text);
            //}
            //spShowInfo.InnerText = "共输入时长 " + durings.ToString() + " 分";
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
        /// <summary>
        /// 返回带有级别的任务
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="currDt"></param>
        /// <returns></returns>
        private DataTable GetBindDataOfTask( )
        {
            DataTable dt=VSDLL.BLL.TaskBLL.GetProjectTaskByLevel ();
            return dt;
        }
        #endregion
        #region 2019-2 新加的方法，默认日期
        /// <summary>
        /// 获取用户地点的更改
        /// </summary>
        /// <param name="dtDate"></param>
        /// <returns></returns>
        private DataRow GetChangedUserRiqi(  DateTime dtDate)
        {
            string locus = GetCBListChecked(this.cblistLocal, ";");
            DataRow drRiqi = null;
            if (locus.Length > 0)
            {
                DataTable dtUserRiqi = (DataTable)ViewState["dtMyRiqi"];//保存我的数据
                if (dtUserRiqi.Rows.Count > 0)
                {
                    drRiqi = dtUserRiqi.Rows[0];
                    if (drRiqi["Locus"].ToString() == locus)//选项没有改变则不保存
                        drRiqi = null;
                    else
                        drRiqi["Locus"] = locus;
                }
                else
                {
                    drRiqi = dtUserRiqi.NewRow();
                    drRiqi["UserID"] = UserID;
                    drRiqi["Riqi"] = dtDate;
                    drRiqi["Created"] = DBNull.Value;//用来判断新建
                    drRiqi["Flag"] = 1;
                    drRiqi["Locus"] = locus;
                }
            }
            return drRiqi;
        }
        private DataTable GetChangedActivities(DateTime dtDate )
        {
            DataTable dtActivity = (DataTable)ViewState["dtActivity"];
            DataTable dtChangedActivity = dtActivity.Clone();
            DataRow drActivity;
            DataRow[] drsActivity;
            int totalDurings = 0;
            if (webObj.QuickEdit == 1)
            {
                #region enumerae gridview
                foreach (GridViewRow gvr in gvActivities.Rows)
                {

                    DropDownList ddlActions = ((DropDownList)gvr.FindControl("ddlActions"));
                    int actionId = int.Parse(ddlActions.SelectedItem.Value);
                    TextBox txtHours = ((TextBox)gvr.FindControl("tbDuring"));
                    TextBox txtQuantity = ((TextBox)gvr.FindControl("tbQuantity"));
                    TextBox txtStartTime = ((TextBox)gvr.FindControl("tbStartTime"));
                    TextBox txtDesc = ((TextBox)gvr.FindControl("tbDesc"));
                    DropDownList ddlTasks = gvr.FindControl("ddlTasks") as DropDownList ;//任务文档
                    CheckBox cbNormal = (CheckBox)gvr.FindControl("cbSelNormal");
                    Label lblFlag = ((Label)gvr.FindControl("lblFlag")) as Label;
                    int taskID = int.Parse(ddlTasks.SelectedItem.Value);

                    if (txtHours.Text != "" && txtHours.Text != "0" || txtQuantity.Text != "" && txtQuantity.Text != "0")
                    {
                        drActivity = dtChangedActivity.Rows.Add();
                        drActivity["ActionID"] = actionId;
                        if (txtHours.Text != "")
                        {
                            drActivity["During"] = int.Parse(txtHours.Text);
                            totalDurings =totalDurings +int.Parse(txtHours.Text);
                        }
                        if (txtQuantity.Text != "")
                            drActivity["Quantity"] = float.Parse(txtQuantity.Text);

                        drActivity["Description"] = txtDesc.Text;
                        if (txtStartTime.Text.Length > 0)
                        {
                            string[] tspans = txtStartTime.Text.Split(':');
                            int hour = 0;
                            int minute = 0;
                            int second = 0;
                            hour = int.Parse(tspans[0]);
                            minute = int.Parse(tspans[1]);
                            if (tspans.Length > 2)
                                second = int.Parse(tspans[2]);
                            TimeSpan startTime = new TimeSpan(hour, minute, second);
                            drActivity["StartTime"] = startTime;
                        }
                        drActivity["IsNormal"] = cbNormal.Checked;
                        drActivity["TaskID"] = taskID;
                        drActivity["UserID"] = UserID;
                        drActivity["Riqi"] = dtDate;
                        drActivity["Flag"] = 1;
                        drsActivity = dtActivity.Select("ActionID=" + actionId + " and TaskID=" + taskID);
                        if (drsActivity.Length == 0)
                            drActivity["Created"] = DBNull.Value;//用来判断新建
                        else
                        {
                            bool isEqual = VSDLL.BLL.Common.DataRowCompare(drActivity, drsActivity[0], new string[] { "StartTime", "Description", "ActionID", "During", "Quantity", "TaskID" });
                            drActivity["Created"] = drsActivity[0]["Created"];

                            if (!isEqual)
                            {
                                drActivity["Modified"] = DBNull.Value;
                                drActivity["ActivityID"] = drsActivity[0]["ActivityID"];
                            }
                            else
                                drActivity["Modified"] = drsActivity[0]["Modified"] == DBNull.Value ? drsActivity[0]["Created"] : drsActivity[0]["Modified"];
                        }
                    }

                }
                #endregion
            }
            ViewState["ChangedDurings"] = totalDurings;
            return dtChangedActivity;
        }
        /// <summary>
        /// 重新显示改变后的时长
        /// </summary>
        private void ShowChangedDurings()
        {
            if (ViewState["ChangedDurings"] != null)
            {
                int durings = (int)ViewState["ChangedDurings"];
                spShowInfo.InnerText = "共输入时长 " + durings.ToString() + " 分";
            }
        }
        /// <summary>
        /// 判断选择或输入的日期值范围
        /// </summary>
        /// <param name="inputDate">日期值</param>
        private void CheckDateValue(ref DateTime inputDate)
        {
            if (ViewState["ActionFirstDay"] == null)
            {
                DataSet ds = VSDLL.DAL.ActivityDAL.GetActivityLastDayByUserID(UserID,0);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["ActionFirstDay"] = ds.Tables[0].Rows[0]["Riqi"];
                }
            }
            if (ViewState["ActionFirstDay"] != null)//活动开始录入的时间
            {
                DateTime firstDate = (DateTime)ViewState["ActionFirstDay"];
                if (inputDate < firstDate)
                {
                    inputDate = firstDate;
                    return;
                }
            }

            DateTime lstDate = (DateTime)DateTime.Now.Date ;
            if (inputDate > lstDate)
                inputDate = lstDate;
        }

        /// <summary>
        /// 如果老用户近期有几天没有录入活动数据了，则缺省日期选为第一个没有保存数据的日期
        /// 当前时间如果为12点前则为昨天，午后则为今天；
        /// </summary>
        /// <returns></returns>
        private DateTime GetDefaultDate()
        {
            DateTime dtDef;
            DateTime? dt = VSDLL.BLL.ActivityBLL.GetLastDateActivityUserID(UserID,1);
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
        #region 处理场所选择
        /// <summary>
        /// 初始化场所多选控件绑定数据
        /// </summary>
        /// <param name="cbl"></param>
        /// <param name="selectedValue"></param>
        /// <param name="splitStr"></param>
        private void BindLocals()
        {

            DateTime dtStart = dtCurrentDate.SelectedDate;
            int userId = VSDLL.Common.Users.UserID;
            DataSet dsMetaData = VSDLL.DAL.MetaDataDAL.GetGroupMetaData();
            //ViewState["dsMetaData"] = dsMetaData;
            DataSet dsUser_Riqi = VSDLL.DAL.UserDAL.GetUser_RiqiByUserID(userId);
            DataTable dt = VSDLL.BLL.MetaDataBLL.GetMetaDataByGroup("locale", dsMetaData);
            cblistLocal.Items.Clear();
            cblistLocal.DataSource = dt;
            cblistLocal.DataValueField = "ItemID";
            cblistLocal.DataTextField = "Title";
            cblistLocal.DataBind();

            DataTable dtMyRiqi = dsUser_Riqi.Tables[0].Clone();
            DataRow dr = VSDLL.BLL.UserBLL.GetUser_RiqiByRiqi(dsUser_Riqi, dtStart.Date);
            if (dr != null)
            {
                string strL = VSDLL.DAL.SystemDataExtension.GetString(dr, "Locus");
                SetCBListChecked(cblistLocal, strL, ";");
                //在保存时使用
                dtMyRiqi.Rows.Add(dr.ItemArray);

            }
            ViewState["dtMyRiqi"] = dtMyRiqi;
        }

        /// <summary>
        /// 设置CheckBoxList中选中项
        /// </summary>
        /// <param name="cbl">CheckBoxList</param>
        /// <param name="selectedValue">选中了的值串,例如："1;2;4;"</param>
        /// <param name="splitStr">值串中使用的分割符,例如"1;2;4;"中的分号</param>
        public static void SetCBListChecked(CheckBoxList cbl, string selectedValue, string splitStr)
        {
            selectedValue = splitStr + selectedValue;//例如："1;2;4;"->";1;2;4;"
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                cbl.Items[i].Selected = false;//先让所有选项处于未选中状态

                string val = splitStr + cbl.Items[i].Value + splitStr;
                if (selectedValue.IndexOf(val) != -1)//存在如“;1;”这样的子串
                {
                    cbl.Items[i].Selected = true;
                    selectedValue = selectedValue.Replace(val, splitStr);//然后从原来的值串中用分隔符替换已经选中了的
                    if (selectedValue == splitStr)//selectedValue的最后一项也被选中，此时selectedValue仅剩下一个分隔符
                    {
                        return;//跳出循环，不再执行
                    }
                }
            }
        }

        /// <summary>
        /// 获取CheckBoxList中选中了的值
        /// </summary>
        /// <param name="cbl">CheckBoxList</param>
        /// <param name="splitStr">分割符号,例如"1;2;4"中的分号</param>
        /// <returns></returns>
        public static string GetCBListChecked(CheckBoxList cbl, string splitStr)
        {
            string selval = "";
            var builder = new StringBuilder();
            builder.Append(selval);
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected)
                {
                    builder.Append(cbl.Items[i].Value + splitStr);
                }
            }
            selval = builder.ToString();
            return selval;
        }
        #endregion
        /// <summary>
        /// 初始化活动列表
        /// </summary>
        private void InitMultiActivity()
        {
            DateTime defDt = GetDefaultDate();//最近一天没有录入的日期
            dtCurrentDate.SelectedDate = defDt;// DateTime.Now.Date;
            InitGridView();
        }
        /// <summary>
        /// 先检查并创建任务,来源（1、日历2、操作3、活动）
        /// </summary>
        ///<param name="ischkDateType">1=用户自行切换；0=根据日期判断</param>

        private void InitGridView(int ischkDateType = 0)
        {
            DateTime currDate = dtCurrentDate.SelectedDate;
            BindLocals();
            SPUser cuser = SPContext.Current.Web.CurrentUser;
            if (cuser != null)//已登录
            {
                int userId = cuser.ID;
                DataTable dtActivity = GetActivities(userId, currDate, currDate);//获取当日活动
                ViewState["dtActivity"] = dtActivity;
                if (dtActivity.Rows.Count == 0)//当前日期没有活动
                {

                    CalculationActivityByAction(currDate, webObj.ActionCount, ref dtActivity);
                    btnNew.Enabled = false;
                }
                else
                    btnNew.Enabled = true;
                DataBind(dtActivity);
                //ViewState["dtResult"] = dtResult;
                //计算时间和条数
                Caculate();
            }
            else
            {
                lblMsg.Text = "您尚未登录，无法快速录入活动！";
            }
        }

        /// <summary>
        /// 活动数据绑定显示
        /// </summary>
        /// <param name="dt"></param>
        private void DataBind(DataTable dt)
        {
            gvActivities.DataSource = dt;
            gvActivities.DataBind();
            if (dt.Rows.Count == 0)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
        }

        #endregion
        #region 操作加背景颜色及相关
        /// <summary>
        /// 获取我的操作数据，如果我的操作设置不全，则读取操作中剩余的部分数据
        /// </summary>
        /// <param name="listName">我的操作列表名称</param>
        /// <param name="userID">当前用户iD</param>
        /// <returns></returns>
        private DataTable GetBindDataOfAction()
        {
            DataTable dtSource;
            if (ViewState["dtBindAction"] != null)
                dtSource = (DataTable)ViewState["dtBindAction"];
            else
            {
                dtSource = GetActions().Copy ();
                DataRow drNew = dtSource.NewRow();//首行加空数据
                drNew["ID"] = 0;
                dtSource.Rows.InsertAt(drNew, 0);
                ViewState["dtBindAction"] = dtSource;
            }
            return dtSource;
        }

        private DataTable GetActions()
        {
            int userID = UserID;
            DataTable dt = null;
            if (ViewState["dtAction"] != null)
                dt = (DataTable)ViewState["dtAction"];
            else
            {
                dt = VSDLL.BLL.ActionBLL.GetAcivityActions(userID);
                ViewState["dtAction"] = dt;
            }
            return dt;
        }

        /// <summary>
        /// 获取最近一条活动，操作对应的任务文档
        /// </summary>
        /// <param name="dsActivity">最近日期的活动</param>
        /// <param name="actionID">操作ID</param>
        /// <returns></returns>
        private int GetTaskIDByAction(DataTable  dtActivity, int actionID)
        {
            DataRow[] drs = dtActivity.Select("ActionID=" + actionID, "Created desc");
            int taskID = 0;
            if (drs.Length > 0)
                taskID = VSDLL.DAL.SystemDataExtension.GetInt32(drs[0], "TaskID");
            return 0;
        }
        /// <summary>
        /// 刷新后重新绑定样式
        /// </summary>
        private void BindDdlActionsStyle(DropDownList ddl )
        {
            DataTable     dt = GetBindDataOfAction();
            if (webObj.QuickEdit == 1)
            {
                #region enumerae gridview
                 foreach (GridViewRow gvr in gvActivities.Rows)
                {

                    DropDownList ddlActions = ((DropDownList)gvr.FindControl("ddlActions"));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        string flag = dr["FrequencyID"].ToString();
                        ddlActions.Items[i].Attributes.Add("style", GetStyle(flag));
                    }
                }
                #endregion
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string flag = dr["FrequencyID"].ToString();
                    ddl.Items[i].Attributes.Add("style", GetStyle(flag));
                }
            }
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
                    string flag = dr["FrequencyID"].ToString();
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
        #region 方法
        /// <summary>
        /// 获取用户活动的最后一条带任务的数据，
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="actionID"></param>
        /// <returns></returns>
        private int GetActivityTaskID(int userId, int actionID)
        {
            DataSet ds = VSDLL.DAL.ActivityDAL.GetActivityLastDayByUserID(userId,1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ActionLastDay"] = ds.Tables[0].Rows[0]["Riqi"];
                return int.Parse (ds.Tables[0].Rows[0]["TaskID"].ToString ());
            }

            else
                return 0;
        }
        /// <summary>
        /// 获取时间范围内的活动
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        private DataTable   GetActivities(int userId, DateTime startDate,DateTime endDate)
        {
            DataSet ds = VSDLL.DAL.ActivityDAL.GetActivityByUserID(userId, startDate, endDate);
            return ds.Tables[0]  ;
        }
        /// <summary>
        /// 如果当天没有录入过活动，则推算活动
        /// </summary>
        /// <returns></returns>
        private DataTable CalculationActivity(DateTime currDate)
        {
            int userID = UserID;
            DataTable dtResult = null;//CreateData();
            List<string> actions = new List<string>();
            List<DateTime> retdts = new List<DateTime>();
            //获取指定天数的日期
            //GetDaysTypeBySql(dtCurrentDate.SelectedDate.AddDays(-1), chkDayType.Checked ? 1 : 0, ref retdts);
            int icount = 1;
            DataTable dtRetAct = null;
            if (retdts.Count > 0)
            {
                DataTable dtActivities = GetActivities(UserID, retdts[retdts.Count - 1], retdts[0]);
                dtRetAct = GetPreData(dtActivities, userID, retdts);//操作不应该重复
            }

            //读取日常任务,此处需要更改
            DataSet ds = VSDLL.DAL.TaskDAL.GetDailyTask(userID);
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                DataRow dr = dtResult.Rows.Add();
                dr["操作"] = item["ActionID"];
                actions.Add(item["ActionID"].ToString());
                dr["频次"] = 0;
                dr["说明"] = item["Desc"];
                if (item["Quantity"] != null)
                    dr["数量"] = item["Quantity"];
                if (item["During"] != null)
                    dr["时长"] = item["During"];
                int taskID = 0;
                if (dtRetAct != null)
                {
                    taskID = GetTaskIDByAction(dtRetAct, (int)item["ActionID"]);
                }
                dr["TaskID"] = taskID;
                dr["ID"] = icount.ToString() + "_" + item["ActionID"] + "_" + taskID;
                dr["Flag"] = 1;
                dr["ItemID"] = 0;
                dr["ActivityDate"] = currDate;
                icount = icount + 1;
            }


            DataTable dt = GetDistTable(dtRetAct, webObj.ActionCount, actions, currDate);
            return dt;
        }
        /// <summary>
        /// 通过操作获取活动
        /// </summary>
        /// <param name="currDate"></param>
        /// <returns></returns>
        private DataTable CalculationActivityByAction(DateTime currDate,int TotalCount,ref DataTable rTable)
        {
            DataTable dtActions = GetActions();
            int iCount = 0;
            int iStart =-1;
            for (int i = rTable.Rows.Count; i < TotalCount; i++)
            {

                DataRow dr = rTable.NewRow();
                dr["ActivityID"] = iStart ;
                dr["ActionID"] = dtActions.Rows[iCount]["ID"];
                dr["TaskID"] = 0;
                rTable.Rows.Add(dr);
                iCount = iCount + 1;
                iStart = iStart -1;
            }
            return rTable ;
        }
        /// <summary>
        /// 当录入新的活动时，根据以前的活动进行推算
        /// 查找当前用户一周内的所有活动操作，去重并按照频率由高到低排序，选取前25个操作，不够25个选择全部
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="dtDate">推导出的日期</param>
        /// <returns></returns>
        private DataTable GetPreData(DataTable dt, int userId, List<DateTime> dtDate)
        {
            //获取最小的日期
            DateTime maxTime = dtDate[dtDate.Count - 1];//将参数的日期改过日期列表，去除掉休息日
            DataTable dtAll = dt.Clone();
            DataRow[] drs = dt.Select("Riqi>='" + maxTime.ToString("yyyy-MM-dd") + "'");

            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtTmp = dt.Clone();

                string sql;
                foreach (DateTime dateTmp in dtDate)
                {
                    sql = "Riqi='" + dateTmp.ToString("yyyy-MM-dd") + "'";
                    drs = dt.Select(sql, "Created");
                    foreach (DataRow dr in drs)
                    {
                        dtTmp.ImportRow(dr);
                    }
                }
                dt = dtTmp.Copy();
            }
            else
            {
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// 获取不重复的数据
        /// </summary>
        /// <param name="dtSource"></param>
         /// <param name="TotalCount">需要再获取的行数,</param>
        /// <returns></returns>
        private DataTable GetDistTable(DataTable dtSource,   int TotalCount, List<string> actions, DateTime dtTime)
        {
            DataTable rTable = null;// CreateData();
            int iStart = webObj.ActionCount - TotalCount + 1;//为了获取序号，操作允许有重复
            if (dtSource != null)
            {
                DataTable dt1 = dtSource.DefaultView.ToTable(true, "ActionID","TaskID");
                int rows = dt1.Rows.Count <= TotalCount ? dt1.Rows.Count : TotalCount;//行数不够指定数，以行数为准
                //找所有的，之后按频次排序，推出的会超过给出的条数
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow dr = rTable.NewRow();
                    dr["ID"] = iStart.ToString() + "_" + dt1.Rows[i]["ActionID"]+"_"+dt1.Rows[i]["TaskID"];
                    dr["操作"] = dt1.Rows[i]["ActionID"];
                    dr["频次"] = dtSource.Compute("count(ActionID)","ActionID=" + dt1.Rows[i]["ActionID"] + " and TaskID="+dt1.Rows[i]["TaskID"]);
                    object result = DBNull.Value;
                    result = dtSource.Compute("avg(During)", "ActionID=" + dt1.Rows[i]["ActionID"] + " and TaskID="+dt1.Rows[i]["TaskID"]);
                    if (result != DBNull.Value)
                    {
                        decimal during = decimal.Round(decimal.Parse(result.ToString()));
                        during = during - during % 10;
                        dr["时长"] = during;

                    }
                    dr["Flag"] = 3;
                    DataRow[] drs;
                    int taskID = GetTaskIDByAction(dtSource, (int)dt1.Rows[i]["ActionID"]);
                    drs = dtSource.Select("ActionID=" + dt1.Rows[i]["ActionID"] + " and TaskID="+dt1.Rows[i]["TaskID"],"Created desc");
                    dr["TaskID"] =taskID ;
                    dr["Date"] = dtTime;
                    dr["ItemID"] = 0;
                    dr["StartTime"] = drs[0]["StartTime"];
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

            }

            return rTable;
        }
        #endregion
        #region 单条数据的录入和编辑方法
        /// <summary>
        /// 写入活动与附件的关系
        /// </summary>
        /// <param name="ids">选中的附件ID</param>
        /// <param name="assistantID">活动ID</param>
        /// <returns></returns>
        private void WriteResultToList(string[] ids, int assistantID, SPWeb subWeb)
        {
            SPUser user = SPContext.Current.Web.CurrentUser;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
           {
           using (SPSite spSite = new SPSite(SPContext.Current.Site.ID))
           {
               using (SPWeb sWeb = spSite.OpenWeb(subWeb .ID))
               {
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
                           sWeb.AllowUnsafeUpdates = false;
                       }
                   }
               }
           });
        }
        /// <summary>
        /// 获取操作的活动类型,在操作表中找，待做
        /// </summary>
        private string GetActionType(int actionID)
        {
            string typeName = "";
            return typeName;
        }
        /// <summary>
        /// 初始化字段的说明，从表结构定义中读取
        /// </summary>
        private void InitFieldDesc()
        {
            DataSet ds = VSDLL.BLL.TablesBLL.GetTableFields("Activity", null);
            DataRow[] drs = ds.Tables[0].Select("字段名='During'");
            if (drs.Length > 0)
                spanHours.InnerText = drs[0]["说明"].ToString();
            drs = ds.Tables[0].Select("字段名='IsNormal'");
            if (drs.Length > 0)
                spanNormal.InnerText = drs[0]["说明"].ToString();
            drs = ds.Tables[0].Select("字段名='Quantity'");
            if (drs.Length > 0)
                spanQuantity.InnerText = drs[0]["说明"].ToString();
            drs = ds.Tables[0].Select("字段名='TaskID'");
            if (drs.Length > 0)
                spanTasks.InnerText = drs[0]["说明"].ToString();
        }


        /// <summary>
        /// 初始化控件,新建或编辑时
        /// </summary>
        private void InitDdlControl(int userID = 0)
        {
            DataTable dtSource = GetBindDataOfAction();
            BindDdlActions(ddlActions, dtSource.Copy());
            dtSource = GetBindDataOfTask( );
            //任务绑定
            BindDropDownList(ddlTypes, "ID", "Title", dtSource);
        }
        /// <summary>
        /// 初始化单条活动录入或编辑
        /// </summary>
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


            if (id > 0)//编辑
            {
                //SPListItem item = list.GetItemById(id);
                //if (item != null)
                //{
                //    dtCurrentDate.SelectedDate = DateTime.Parse(item["Date"].ToString());
                //    InitDdlControl(userID );//需要获取日期
                //    if (item["Desc"] != null)
                //        txtDesc.Text = item["Desc"].ToString();
                //    if (item["During"] != null)
                //        txtHours.Text = item["During"].ToString();
                //    if (item["Quantity"] != null)
                //        txtQuantity.Text = item["Quantity"].ToString();
                //    int actionID = new SPFieldLookupValue(item["ActionID"].ToString()).LookupId;
                //    SelectDropDownList(ddlActions, actionID.ToString());

                //    if (item["TaskID"] != null)
                //        SelectDropDownList(ddlTypes, (new SPFieldLookupValue(item["TaskID"].ToString())).LookupId.ToString());
                //    else
                //         SelectDropDownList(ddlTypes, "0");

                //    cbNormal.Checked = (bool)item["IsNormal"];
                //    lblTypeShow.Text = "操作类型：" + GetActionType(actionID);
                //}
                //if (!hasRight)
                //{
                //    btnSave.Enabled = false;
                //    lblMsg.Text = "您无权编辑！";
                //}
            }
            else//新建
            {
                DateTime defDt = GetDefaultDate();
                dtCurrentDate.SelectedDate = defDt;

                InitDdlControl(userID );
                /// 更改任务文档,根据操作类型找任务
                int tastID = 0;// GetTaskIDByAction(int.Parse(ddlActions.SelectedItem.Value));
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

            return isRight;
        }
        #endregion
        #region 获取用户与活动相关
        /// <summary>
        /// 获取日期类型
        /// </summary>
        /// <param name="currDate">当前日期</param>
        /// <param name="retDayType">判断日期类型</param>
        /// <param name="retDt">返回指定日期类型的日期</param>
        /// <returns>1是休息日，0为非休息日</returns>
        private int GetDaysTypeBySql(DateTime currDate, int retDayType, ref List<DateTime> retDt)
        {
            int dayType = -1;
            DataSet dsUser_Riqi = VSDLL.DAL.UserDAL.GetUser_RiqiByUserID(UserID);
            List<DateTime> retDates = new List<DateTime>();
            int i = 0;
            DateTime dt1 = currDate;
            while (i < dsUser_Riqi.Tables[0].Rows.Count)
            {
                dayType = -1;
                //读取数据库
                dayType = VSDLL.BLL.UserBLL.GetUser_RiqiType(dsUser_Riqi, dt1);
                if (dayType == -1)
                {
                    if (dt1.DayOfWeek == DayOfWeek.Saturday || dt1.DayOfWeek == DayOfWeek.Sunday)//6,0
                        dayType = 1;
                    else
                        dayType = 0;
                }
                if (retDayType == -1)//只返回当前日期的类型
                    break;
                else if (dayType == retDayType)
                {
                    retDates.Add(dt1);
                    if (retDates.Count == webObj.BeforeDays)
                        break;
                }
                dt1 = dt1.AddDays(-1);
                i = i + 1;
            }
            retDt = retDates;
            return dayType;
        }
        #endregion
    }
}
