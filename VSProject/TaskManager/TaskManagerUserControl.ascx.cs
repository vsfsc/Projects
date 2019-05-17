using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;
using VSDLL.BLL;
using VSDLL.DAL;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;

namespace VSProject.TaskManager
{
    public partial class TaskManagerUserControl : UserControl
    {
        #region 控件
        protected GridView gvTasks;
        protected LinkButton lnkAddFoot;
        protected DropDownList ddlProjects;
        protected RadioButtonList rblProjects;
        protected CheckBoxList cblProjects;
        protected Label lbErr;
        protected HtmlContainerControl divList;
        protected HtmlContainerControl divModify;

        protected HiddenField hdfPID;
        protected HiddenField hdfID;
        protected Label lbPTitle;
        protected Label lbParentTitle;
        protected Label lbParent;
        protected DropDownList ddlParent;
        protected Label lbTitle;
        protected TextBox tbTitle;
        protected Label lbTitle_EN;
        protected TextBox tbTitle_EN;
        protected Label lbTitle_ENValid;
        protected Label lbStart;
        protected TextBox tbStart;
        protected Label lbDue;
        protected TextBox tbDue;
        protected Label lbDuring;
        protected TextBox tbDuring;
        protected Label lbPercent;
        protected TextBox tbPercent;
        protected Label lbStatus;
        protected DropDownList ddlStatus;
        protected Label lbDesc;
        protected TextBox tbDesc;

        protected Button btnSubmit;
        protected Button btnCancel;
        #endregion

        #region 属性
        public TaskManager webObj { get; set; }
        #endregion

        #region 事件
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkAddFoot.Click += LnkAddFoot_Click;
            btnSubmit.Click += BtnSubmit_Click;
            btnCancel.Click += BtnCancel_Click;
            ddlProjects.SelectedIndexChanged += DdlProjects_SelectedIndexChanged;
            rblProjects.SelectedIndexChanged += RblProjects_SelectedIndexChanged;
            gvTasks.RowDataBound += gvTasks_RowDataBound;
            gvTasks.RowCommand += gvTasks_RowCommand;
            if (!Page.IsPostBack)
            {
                //获取任务列表
                DataSet dsTasks = TaskDAL.GetProjectTask();
                DataTable dtTasks = dsTasks.Tables[0];
                dtTasks.TableName = "所有任务";
                ViewState["dtTasks"] = dtTasks;

                //绑定项目下拉列表
                DataTable dtProjects = TaskBLL.GetProjects(dtTasks);
                dtProjects.TableName = "项目";
                ViewState["dtProjects"] = dtProjects;
                BindProjects(dtProjects);
                //DDLBind(ddlProjects, dtProjects, "0", "TaskID", "Title");

                //绑定任务表
                DataTable dtLTasks= TaskBLL.GetTasksWithLevel(dtTasks,"Title","PID", false);
                dtLTasks.TableName = "分级任务";
                ViewState["dtLTasks"] = dtLTasks;
                BindTasks(dtLTasks);

                BindFormData();

                int userId = VSDLL.Common.Users.UserID;
                lnkAddFoot.Visible = true;
                if (userId == 0)
                    lnkAddFoot.Visible = false;
            }
        }

        /// <summary>
        /// 单选框选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            string projectId = rblProjects.SelectedValue;
            ChangeProject(projectId);
        }

        /// <summary>
        /// 添加项目（即顶级任务：ParentID=0）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkAddFoot_Click(object sender, EventArgs e)
        {
            try
            {
                lbPTitle.Text = "添加新项目";
                hdfID.Value = "0";
                hdfPID.Value = "0";
                lbTitle_EN.Visible = true;
                lbTitle_ENValid.Visible = true;
                tbTitle_EN.Visible = true;
                lbParent.Visible = false;
                ddlParent.Visible = false;
                lbTitle.Text = "项目名称：";
                tbTitle.Text = "";
                tbStart.Text = "";
                tbDue.Text = "";
                tbDuring.Text = "";
                tbPercent.Text = "";
                lbStatus.Text = "项目状态：";
                ddlStatus.SelectedValue = "0";
                lbDesc.Text = "项目描述：";
                tbDesc.Text = "";
                tbDesc.Rows =5;
                divList.Visible = false;
                divModify.Visible = true;
                lbParentTitle.Visible = false;
            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
        }

        /// <summary>
        /// 关闭编辑/添加任务页面事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            //DataTable dtLTasks=(DataTable)ViewState["dtLTasks"];
            //BindTasks(dtLTasks);
            divList.Visible = true;
            divModify.Visible = false;
            lbErr.Text = "";
            hdfID.Value = "";
            hdfPID.Value = "";
        }

        /// <summary>
        /// 选择不同项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DdlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            string projectId = ddlProjects.SelectedValue;
            ChangeProject(projectId);
        }

        private void ChangeProject(string projectId)
        {
            try
            {
                DataTable dtTasks = (DataTable)ViewState["dtTasks"];

                if (projectId != "0")
                {
                    DataTable dtReturn = dtTasks.Clone();
                    DataRow[] drs = dtTasks.Select("TaskID=" + projectId);
                    if (drs.Length > 0)
                    {
                        dtReturn.Rows.Add(drs[0].ItemArray);
                    }
                    TaskBLL.GetTasksByProjectID(long.Parse(projectId), dtTasks, ref dtReturn);
                    DataTable dtLTasks = TaskBLL.GetTasksWithLevel(dtReturn, "Title", "PID", false);
                    dtLTasks.TableName = "分级任务";
                    dtReturn.TableName = "项目任务";
                    BindTasks(dtLTasks);
                    ViewState["dtLTasks"] = dtLTasks;
                }
                else
                {
                    DataTable dtLTasks = TaskBLL.GetTasksWithLevel(dtTasks, "Title", "PID", false);
                    dtLTasks.TableName = "分级任务";
                    BindTasks(dtLTasks);
                    ViewState["dtLTasks"] = dtLTasks;
                }
            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
        }

        /// <summary>
        /// 提交任务修改或新建任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbTitle.Text.Trim()))
                {
                    int userId = VSDLL.Common.Users.UserID;
                    DataTable dtTasks = (DataTable)ViewState["dtTasks"];
                    dtTasks.TableName = "所有任务";
                    long taskId = 0;
                    if (hdfID.Value != "0"&& hdfID.Value != "")//编辑任务或项目
                    {
                        taskId = long.Parse(hdfID.Value);
                        DataRow[] drs = dtTasks.Select("TaskID=" + taskId);
                        if (drs.Length > 0)
                        {
                            DataRow dr = drs[0];
                            dr.BeginEdit();
                            dr["Title"] = tbTitle.Text.Trim();
                            if (hdfPID.Value != "0")//编辑任务
                            {
                                string title = tbTitle.Text.Trim();
                                DataRow[] drSameTitleTask = dtTasks.Select(string.Format("Title='{0}' and PID=0", title));//查询英文名称
                                if (drSameTitleTask.Length > 0)//已有相同英文名称的项目
                                {
                                    tbTitle.Focus();
                                    lbErr.Text = string.Format("已存在名称为 {0} 的项目，请重新填写任务名称!", title);
                                    return;
                                }
                                else
                                {
                                    dr["Title"] = tbTitle.Text.Trim();
                                }
                                dr["PID"] = long.Parse(ddlParent.SelectedValue);
                            }
                            else//编辑项目
                            {
                                tbTitle_EN.Enabled = false;//项目的英文名称不可更改
                                //if (string.IsNullOrEmpty(tbTitle_EN.Text.Trim()))//
                                //{
                                //    lbErr.Text = "项目英文名称必须填写！";
                                //    tbTitle_EN.Focus();
                                //    return;
                                //}
                                //else
                                //{
                                //    dr["Title_en"] = tbTitle_EN.Text.Trim();
                                //}
                            }
                            if (!string.IsNullOrEmpty(tbTitle_EN.Text.Trim()))
                                dr["Title_en"] = tbTitle_EN.Text.Trim();
                            if (!string.IsNullOrEmpty(tbStart.Text.Trim()))
                                dr["StartDate"] = tbStart.Text.Trim();
                            if (!string.IsNullOrEmpty(tbDue.Text.Trim()))
                                dr["DueDate"] = tbDue.Text.Trim();
                            if (!string.IsNullOrEmpty(tbDuring.Text.Trim()))
                                dr["During"] = int.Parse(tbDuring.Text.Trim());
                            if (!string.IsNullOrEmpty(tbPercent.Text.Trim()))
                                dr["PercentComplete"] = decimal.Parse(tbPercent.Text.Trim());
                            if (!string.IsNullOrEmpty(tbDesc.Text.Trim()))
                                dr["Description"] = tbDesc.Text.Trim();
                            if (ddlStatus.SelectedValue != "0")
                                dr["Status"] = int.Parse(ddlStatus.SelectedValue);
                            if (dr["Created"] == DBNull.Value || dr["Created"] == null)
                                dr["Created"] = DateTime.Now;
                            if (dr["CreatedBy"] == DBNull.Value || dr["CreatedBy"] == null)
                                dr["CreatedBy"] = userId;
                            dr["Modified"] = DateTime.Now;
                            dr["ModifiedBy"] = userId;
                            dr["Flag"] = 1;
                            dr.EndEdit();
                            TaskDAL.UpdateProjectTask(dr);
                            dtTasks.AcceptChanges();

                        }
                        else
                        {
                            lbErr.Text = "你修改的任务已不存在！";
                        }
                    }
                    else//添加项目或任务
                    {
                        DataRow dr = dtTasks.NewRow();
                        if (hdfPID.Value != "0")//添加任务
                        {
                            string title = tbTitle.Text.Trim();
                            DataRow[] drSameTitleTask = dtTasks.Select(string.Format("Title='{0}' and PID=0", title));//查询英文名称
                            if (drSameTitleTask.Length > 0)//已有相同英文名称的项目
                            {
                                tbTitle.Focus();
                                lbErr.Text = string.Format("已存在名称为 {0} 的项目，请重新填写任务名称!", title);
                                return;
                            }
                            else
                            {
                                dr["Title"] = tbTitle.Text.Trim();
                            }
                            dr["PID"] = long.Parse(ddlParent.SelectedValue);
                        }
                        else//添加项目
                        {
                            //若该数据项是项目，则英文名称必须填写，且英文名称必须字母开头，由英文、数字或者下划线组成，不能有汉字等其他字符，长度1-50之间
                            Regex regENTitle = new Regex(@"^[a-zA-Z][a-zA-Z0-9_]{0,49}$");
                            if (string.IsNullOrEmpty(tbTitle_EN.Text.Trim())|| !regENTitle.IsMatch(tbTitle_EN.Text.Trim()))//
                            {
                                lbErr.Text = "项目英文名称必须填写，且英文名称必须字母开头，由英文、数字或者下划线组成，不能有汉字、空格等其他字符，长度1-50之间！";
                                tbTitle_EN.Focus();
                                return;
                            }
                            else
                            {
                                string enTitle= tbTitle_EN.Text.Trim();
                                DataRow[] drSameENTitleTask = dtTasks.Select(string.Format("Title_en='{0}' and PID=0", enTitle));//查询英文名称
                                if (drSameENTitleTask.Length > 0)//已有相同英文名称的项目
                                {
                                    tbTitle_EN.Focus();
                                    lbErr.Text = string.Format("已存在英文名称为 {0} 的项目，请重新填写项目的英文名称", enTitle);
                                    return;
                                }
                                else
                                {
                                    dr["Title_en"] = tbTitle_EN.Text.Trim();
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(tbTitle_EN.Text.Trim()))
                            dr["Title_en"] = tbTitle_EN.Text.Trim();
                        if (!string.IsNullOrEmpty(tbStart.Text.Trim()))
                            dr["StartDate"] = tbStart.Text.Trim();
                        if (!string.IsNullOrEmpty(tbDue.Text.Trim()))
                            dr["DueDate"] = tbDue.Text.Trim();
                        if (!string.IsNullOrEmpty(tbDuring.Text.Trim()))
                            dr["During"] = int.Parse(tbDuring.Text.Trim());
                        if (!string.IsNullOrEmpty(tbPercent.Text.Trim()))
                            dr["PercentComplete"] = decimal.Parse(tbPercent.Text.Trim());
                        if (!string.IsNullOrEmpty(tbDesc.Text.Trim()))
                            dr["Description"] = tbDesc.Text.Trim();
                        if (ddlStatus.SelectedValue != "0")
                            dr["Status"] = int.Parse(ddlStatus.SelectedValue);
                        dr["PID"] = 0;
                        dr["Created"] = DateTime.Now;
                        dr["CreatedBy"] = userId;
                        dr["Modified"] = DateTime.Now;
                        dr["ModifiedBy"] = userId;
                        dr["Flag"] = 1;
                        taskId = TaskDAL.InsertProjectTask(dr);
                        dr["TaskID"] = taskId;
                        dtTasks.Rows.Add(dr);
                    }
                    new TaskDocs().SaveTask_Doc(taskId, dtTasks, userId);
                    ViewState["dtTasks"] = dtTasks;
                    DataTable dtLTasks = TaskBLL.GetTasksWithLevel(dtTasks, "Title", "PID", false);
                    ViewState["dtLTasks"] = dtLTasks;
                    BindTasks(dtLTasks);
                    hdfID.Value = "";
                    hdfPID.Value = "";
                    divList.Visible = true;
                    divModify.Visible = false;
                    tbTitle_EN.Enabled = true;
                }
                else
                {
                    lbErr.Text = "必须填写名称";
                    tbTitle.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                lbErr.Text=ex.ToString();
            }
        }


        /// <summary>
        /// 绑定所有项目（即ParentID=0的顶级任务）
        /// </summary>
        /// <param name="dtProjects">任务列表</param>
        private void BindProjects(DataTable dtProjects)
        {
            try
            {
                int rowsCount = dtProjects.Rows.Count;
                if (rowsCount>0&rowsCount<=5)//项目数不超过5，用多选框展示
                {
                    ddlProjects.Visible = false;
                    rblProjects.Visible = true;
                    rblProjects.Items.Clear();
                    rblProjects.Items.Add(new ListItem("所有项目", "0"));
                    foreach (DataRow dr in dtProjects.Rows)
                    {
                        ListItem item = new ListItem()
                        {
                            Text = SystemDataExtension.GetString(dr, "Title"),
                            Value = SystemDataExtension.GetString(dr, "TaskID")
                        };
                        item.Attributes.Add("title", SystemDataExtension.GetString(dr, "Description"));
                        rblProjects.Items.Add(item);
                    }

                }
                else//项目数超过5，用下拉列表展示
                {
                    ddlProjects.Visible = true;
                    rblProjects.Visible = false;
                    ddlProjects.Items.Clear();
                    ListItem item0 = new ListItem
                    {
                        Text = "所有项目",
                        Value = "0"
                    };
                    item0.Attributes.Add("title","筛选所有项目的任务！");
                    ddlProjects.Items.Add(new ListItem("所有项目", "0"));
                    foreach (DataRow dr in dtProjects.Rows)
                    {
                        ListItem item = new ListItem()
                        {
                            Text = SystemDataExtension.GetString(dr, "Title"),
                            Value = SystemDataExtension.GetString(dr, "TaskID")
                        };
                        item.Attributes.Add("title", SystemDataExtension.GetString(dr, "Description"));
                        ddlProjects.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

                lbErr.Text=ex.ToString();
            }
        }

        /// <summary>
        /// 绑定指定项目的任务数据
        /// </summary>
        /// <param name="dtTasks">任务数据表</param>
        private void BindTasks(DataTable dtTasks)
        {
            try
            {
                gvTasks.DataSource = dtTasks;
                gvTasks.DataBind();
            }
            catch (Exception ex)
            {

                lbErr.Text=ex.ToString();
            }
        }

        /// <summary>
        /// 删除指定ID的任务
        /// </summary>
        /// <param name="taskId"></param>
        private void DeleteTaskByTaskID(long taskId)
        {
            try
            {
                DataTable dtTasks =(DataTable) ViewState["dtTasks"];
                DataRow[] drs = dtTasks.Select("TaskID=" + taskId);
                if (drs.Length > 0)
                {
                    DataRow[] drChilds = dtTasks.Select("PID=" + taskId);
                    if (drChilds.Length <= 0)//无子任务，可以删除
                    {
                        DataRow dr = drs[0];
                        dr.BeginEdit();
                        dr["Flag"] = 0;//数据库逻辑删除，将标记置为0
                        TaskDAL.UpdateProjectTask(dr);
                        lbErr.Text = "任务删除成功！";
                        dr.EndEdit();
                        dtTasks.Rows.Remove(dr);//数据集中物理删除，将行删除
                        ViewState["dtTasks"] = dtTasks;
                        DataTable dtLTasks = TaskBLL.GetTasksWithLevel(dtTasks, "Title", "PID", false);
                        ViewState["dtLTasks"] = dtLTasks;
                        BindTasks(dtLTasks);
                        ////用户确认是否删除子任务
                        //if (isDelchildTask == 0)//不删除子任务，则将其升级到其上一级任务
                        //{
                        //    int pId = DAL.SystemDataExtension.GetInt16(dr, "PID");
                        //    foreach (DataRow drChild in drChilds)
                        //    {
                        //        drChild["PID"] = pId;
                        //        TaskDAL.UpdateProjectTask(drChild);
                        //    }
                        //}
                        //else//子任务一起删除
                        //{
                        //    foreach (DataRow drChild in drChilds)
                        //    {
                        //        drChild["Flag"] = 0;
                        //        TaskDAL.UpdateProjectTask(drChild);
                        //    }
                        //}
                    }
                    else
                    {
                        lbErr.Text = "该任务有低级的子任务，无法删除！";//有子任务，不可删除
                    }
                }
                else
                    lbErr.Text = "你删除的任务已不存在！";
            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
        }

        /// <summary>
        /// 任务列表行命令：删除、修改、添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DelTask")
                {
                    long taskId = long.Parse(e.CommandArgument.ToString());
                    DeleteTaskByTaskID(taskId);
                }
                else if (e.CommandName == "EditTask")
                {
                    #region 编辑任务
                    string taskId = e.CommandArgument.ToString();
                    hdfID.Value = taskId;

                    DataTable dt = (DataTable)ViewState["dtTasks"];
                    DataTable dtTasks = dt.Copy();
                    DataRow dr = dtTasks.Select("TaskID=" + long.Parse(taskId))[0];
                    string tTitle = SystemDataExtension.GetString(dr, "Title");
                    //标题
                    tbTitle.Text = tTitle;
                    //英文标题
                    tbTitle_EN.Text = SystemDataExtension.GetString(dr, "Title_en");
                    //开始日期
                    tbStart.Text = SystemDataExtension.GetString(dr, "StartDate");
                    //截止日期
                    tbDue.Text = SystemDataExtension.GetString(dr, "DueDate");
                    //预估工作量
                    tbDuring.Text = SystemDataExtension.GetString(dr, "During");
                    //百分比
                    tbPercent.Text = SystemDataExtension.GetString(dr, "PercentComplete");
                    //描述
                    tbDesc.Text = SystemDataExtension.GetString(dr, "Description");
                    //状态
                    int tStatus = SystemDataExtension.GetInt16(dr, "Status");
                    ddlStatus.SelectedValue = tStatus.ToString();
                    long parentId = SystemDataExtension.GetInt32(dr, "PID");
                    hdfPID.Value =parentId.ToString();
                    if (parentId == 0)//当前任务就是一个项目
                    {
                        lbPTitle.Text = "编辑项目";
                        lbTitle.Text = "项目名称：";
                        lbTitle_EN.Visible = true;
                        lbTitle_ENValid.Visible = true;
                        tbTitle_EN.Visible = true;
                        lbDesc.Text = "项目描述：";
                        lbParentTitle.Visible = false;
                        lbParent.Visible = false;
                        ddlParent.Visible = false;
                        lbStatus.Text = "项目状态：";
                        tbDesc.Rows = 5;
                    }
                    else
                    {
                        lbTitle_EN.Visible = false;
                        lbTitle_ENValid.Visible = false;
                        tbTitle_EN.Visible = false;
                        lbTitle.Text = "任务名称：";
                        DataRow drProject = dr;
                        TaskBLL.GetProjectByTaskID(parentId, dtTasks, ref drProject);
                        lbPTitle.Text ="编辑任务";
                        lbDesc.Text = "任务描述：";
                        lbStatus.Text = "任务状态：";
                        lbParent.Visible = true;
                        ddlParent.Visible = true;
                        DataRow drParentTask = dtTasks.Select("TaskID="+parentId)[0];
                        lbParentTitle.Visible = true;
                        lbParentTitle.Text = string.Format("所属项目：{0}；父级任务：{1}", SystemDataExtension.GetString(drProject, "Title"), SystemDataExtension.GetString(drParentTask, "Title"));
                        //父级任务列表：dtTasks除去当前任务dr以及其下级任务，其他任务均可作为父级任务使用
                        dtTasks.Rows.Remove(dr);
                        dtTasks.AcceptChanges();
                        dtTasks.TableName = "父级任务";
                        long projectId = SystemDataExtension.GetInt32(drProject, "TaskID");
                        DataTable dtReturn = dtTasks.Clone();
                        DataRow[] drPrarents = dtTasks.Select("TaskID=" + projectId);
                        if (drPrarents.Length > 0)
                        {
                            dtReturn.Rows.Add(drPrarents[0].ItemArray);
                        }
                        TaskBLL.GetTasksByProjectID(projectId, dtTasks, ref dtReturn);
                        DataTable dtLTasks = TaskBLL.GetTasksWithLevel(dtReturn, "Title", "PID", false);
                        dtLTasks.TableName = "父级任务";
                        DDLBind(ddlParent, dtLTasks, parentId.ToString(), "ID", "Title");
                        ddlParent.Enabled = true;
                        tbDesc.Rows = 3;
                    }


                    divList.Visible = false;
                    divModify.Visible = true;
                    #endregion
                }
                else//添加子任务
                {
                    lbTitle_EN.Visible = false;
                    lbTitle_ENValid.Visible = false;
                    tbTitle_EN.Visible = false;
                    #region 添加任务
                    string taskId = e.CommandArgument.ToString();
                    DataTable dt = (DataTable)ViewState["dtTasks"];
                    DataTable dtTasks = dt.Copy();
                    hdfID.Value = "0";
                    lbTitle.Text = "任务名称：";
                    tbTitle.Text = "";
                    tbTitle_EN.Text = "";
                    tbStart.Text = "";
                    tbDue.Text = "";
                    tbDuring.Text = "";
                    tbPercent.Text = "";
                    lbDesc.Text = "任务描述：";
                    tbDesc.Text = "";
                    tbDesc.Rows = 3;
                    lbPTitle.Text = "添加任务";

                    //父级任务处理，在新增子任务时，确认了父级任务就不可更改了
                    hdfPID.Value = taskId;
                    DataRow drProject = dtTasks.Select("TaskID=" + taskId)[0];
                    TaskBLL.GetProjectByTaskID(long.Parse(taskId), dtTasks, ref drProject);
                    DataRow drParentTask = dtTasks.Select("TaskID=" + taskId)[0];
                    lbParentTitle.Visible = true;
                    lbParentTitle.Text = string.Format("所属项目：{0}；父级任务：{1}", SystemDataExtension.GetString(drProject, "Title"), SystemDataExtension.GetString(drParentTask, "Title"));
                    lbParent.Visible = false;
                    ddlParent.Visible = false;

                    //ddlParent.Enabled = true;
                    //dtTasks.TableName = "父级任务";
                    //DataTable dtLTasks = TaskBLL.GetTasksWithLevel(dtTasks,"Title","PID", false);
                    //dtLTasks.TableName = "分级任务";
                    //DDLBind(ddlParent, dtLTasks, taskId, "ID", "Title");
                    //ddlParent.Enabled = false;

                    lbStatus.Text = "任务状态：";
                    ddlStatus.SelectedValue = "0";
                    divList.Visible = false;
                    divModify.Visible = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
        }


        /// <summary>
        /// 任务列表行绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (((Label)e.Row.FindControl("lbStatus")) != null)
            {
                DataSet dsMetaData = MetaDataDAL.GetGroupMetaData();
                DataTable dtStatus = MetaDataBLL.GetMetaDataByGroup("ProjectTask_Status", dsMetaData);
                dtStatus.TableName = "任务状态表";
                Label lbStatus = (Label)e.Row.FindControl("lbStatus");
                string initValue = ((HiddenField)e.Row.FindControl("hdfStatus")).Value;
                if (string.IsNullOrEmpty(initValue))
                    initValue = "0";
                DataRow[] drs = dtStatus.Select("ItemID=" + int.Parse(initValue));
                if (drs.Length>0)
                {
                    DataRow dr = drs[0];
                    lbStatus.Text =SystemDataExtension.GetString(dr,"Title");
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((Label)e.Row.FindControl("lbPercent"))!=null)
                {
                    Label lbPercent = (Label)e.Row.FindControl("lbPercent");
                    string value = lbPercent.Text;
                    if (!value.Contains("%"))
                        lbPercent.Text = value + "%";
                    value = value.TrimEnd('%');
                    int percent = 0;
                    if (!string.IsNullOrEmpty(value))
                    {
                        percent = int.TryParse(value, out percent) ? percent : 0; ;
                    }
                    string htmlColor = GetColorByPercent(percent);
                    lbPercent.ForeColor= ColorTranslator.FromHtml(htmlColor);
                    //e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, GetColorByPercent(percent));
                }
                if (((Label)e.Row.FindControl("lbTitle")) != null)//标题标签
                {
                    Label lbTitle = (Label)e.Row.FindControl("lbTitle");
                    string tTtitle = lbTitle.Text;
                    int titleLength = webObj.TitleLength;
                    int blankCount = tTtitle.Length - tTtitle.TrimStart().Length;
                    if (tTtitle.Length > titleLength+blankCount) //标题内容大于20个
                    {
                        lbTitle.Text = tTtitle.Substring(0, titleLength+blankCount) + "..."; //截取20个显示，其他用“...”号代替
                    }
                    lbTitle.ToolTip = tTtitle;//鼠标放上去显示全部信息
                }
                if (((Label)e.Row.FindControl("lbDescription")) != null)//标题标签
                {
                    Label lbDescription = (Label)e.Row.FindControl("lbDescription");
                    string tDesc = lbDescription.Text;
                    int titleLength = webObj.TitleLength;
                    if (tDesc.Length > titleLength*2) //标题内容大于20个
                    {
                        lbDescription.Text = tDesc.Substring(0, titleLength * 2) + "..."; //截取2*webObj.TitleLength个字符，其他用“...”号代替
                    }
                    lbDescription.ToolTip = tDesc;//鼠标放上去显示全部信息
                }

                string taskId = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                DataTable dtDoc= TaskDAL.GetDocsByTaskID(long.Parse(taskId)).Tables[0];
                if (dtDoc.Rows.Count > 0)
                {
                    DataRow dr=dtDoc.Rows[0];
                    string docLink=SystemDataExtension.GetString(dr, "DocLink");
                    HyperLink lnkDoc = (HyperLink)e.Row.FindControl("lnkDoc");
                    if (!string.IsNullOrEmpty(docLink))
                    {
                        lnkDoc.NavigateUrl = docLink;
                        lnkDoc.Target = "_blank";
                        lnkDoc.ToolTip = "点击查看并编辑项目/任务文档";
                        lnkDoc.ImageUrl = "../../../../_layouts/15/VSProject/images/Docs.png";
                        lnkDoc.ImageHeight = 20;
                    }
                    else
                    {
                        lnkDoc.Enabled = false;
                    }

                }

                e.Row.Attributes.Add("onmouseover", "if(this!=prevselitem){this.style.backgroundColor='#E6F2FB'}");//当鼠标停留时更改背景色
                e.Row.Attributes.Add("onmouseout", "if(this!=prevselitem){this.style.backgroundColor='#ffffff'}");//当鼠标移开时还原背景色
                e.Row.Attributes.Add("onclick", e.Row.ClientID + ".checked=true;selectx(this)");
            }
            int userId = VSDLL.Common.Users.UserID;

            if (userId == 0)//未登录，隐藏任务管理按钮列
            {
                e.Row.Cells[8].Visible = false;
            }
            else
            {
                e.Row.Cells[8].Visible = true;
                //HiddenField hdfParentID = (HiddenField)e.Row.FindControl("hdfParentID");

                //if (string.IsNullOrEmpty(hdfParentID.Value)|| hdfParentID.Value=="0")
                //{
                //    LinkButton lnkDel = (LinkButton)e.Row.FindControl("lnkDel");
                //    lnkDel.OnClientClick = "return confirm('这是一个项目，删除后将同时删除项目下级的所有的任务，确定仍要删除吗？')";
                //}
            }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 绑定具有数据源的DropDownList
        /// </summary>
        /// <param name="ddl">DropDownList的ID</param>
        /// <param name="dtSource">数据源集合</param>
        /// <param name="initValue">初始值</param>
        /// <param name="colValue">值字段名</param>
        /// <param name="colText">显示字段名</param>
        private void DDLBind(DropDownList ddl, DataTable dtSource, string initValue,string colValue,string colText)
        {
            try
            {
                //先清空DropDownList
                ddl.Items.Clear();
                //再绑定DropDownList
                string tbName = dtSource.TableName;
                ddl.Items.Add(new ListItem(string.Format("--请选择{0}--", tbName), "0"));

                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    DataRow dr = dtSource.Rows[i];
                    string ivalue = SystemDataExtension.GetInt16(dr, colValue).ToString();
                    string itext = SystemDataExtension.GetString(dr, colText);
                    ddl.Items.Add(new ListItem(itext, ivalue));
                    //ddl.Items[i].Attributes.Add("style", "background-color:" + GetColor(ivalue));
                }

                //设置DropDownList初始项
                if (string.IsNullOrEmpty(initValue))
                    initValue = "0";
                ddl.SelectedValue = initValue;
                //ddl.Attributes.Add("style", "background-color:"+GetColor(initValue));
            }
            catch (Exception ex)
            {

                lbErr.Text=ex.ToString();
            }
        }

        /// <summary>
        /// 根据完成百分比获取要设置的完成百分比文本显示的颜色代码
        /// </summary>
        /// <param name="percent">完成百分比</param>
        /// <returns></returns>
        private string GetColorByPercent(int percent)
        {
            string mycolor = "#000";
            if (percent >= 0 && percent < 5)
                mycolor = "#EE2C2C";
            if (percent >= 5 && percent < 25)
                mycolor = "#FF4500";
            if (percent >= 25 && percent < 50)
                mycolor = "#FF8C00";
            if (percent >= 50 && percent < 75)
                mycolor = "#32CD32";
            if (percent >= 75 && percent < 100)
                mycolor = "#228B22";
            return mycolor;
        }

        /// <summary>
        /// 根据标记号Flag设置一种颜色
        /// </summary>
        /// <param name="flag">标记号</param>
        /// <returns></returns>
        private string GetColor(string flag)
        {
            string style = "RGB(209,209,209)";
            switch (flag)
            {
                case "1":
                    style = "RGB(255,144,144)";
                    break;
                case "2":
                    style = "RGB(246,242,144)";
                    break;
                case "3":
                    style = "RGB(191,238,125)";
                    break;
                case "4":
                    style = "RGB(171,248,250)";
                    break;
                case "5":
                    style = "RGB(255,209,255)";
                    break;
                case "6":
                    style = "RGB(245 245 220)";
                    break;
                default:
                    style = "RGB(209,209,209)";
                    break;
            }
            return style;
        }

        /// <summary>
        /// 获取当前登录用户的ID，未登录时返回0
        /// </summary>
        /// <returns></returns>
        private long GetUserId()
        {
            long userId = 0;
            SPUser cUser = SPContext.Current.Web.CurrentUser;
            if (cUser != null)
            {
                userId = cUser.ID;
            }
            return userId;
        }

        /// <summary>
        /// 绑定表单中的常量数据
        /// </summary>
        private void BindFormData()
        {
            DataSet dsMetaData = MetaDataDAL.GetGroupMetaData();
            DataTable dtStatus = MetaDataBLL.GetMetaDataByGroup("ProjectTask_Status", dsMetaData);
            dtStatus.TableName = "状态";
            DDLBind(ddlStatus, dtStatus, "0", "ItemID", "Title");
        }
        #endregion
    }
}