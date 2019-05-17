using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using VSDLL.BLL;
using VSDLL.DAL;

namespace VSProject.PlanManager
{
    public partial class PlanManagerUserControl : UserControl
    {
        #region Web部件
        public PlanManager webObj;
        #endregion

        #region 页面事件

        /// <summary>
        /// 页面加载与控件事件绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //btnSetAction.Click += BtnSetActions_Click;//设计计划的操作
            btnSubmit.Click += BtnSubmit_Click;//提交计划保存
            btnCancel.Click += BtnCancel_Click;
            btnAddSingle.Click += BtnAddSingle_Click;
            btnAddMulti.Click += BtnAddMulti_Click;
            ddlPlanAction.SelectedIndexChanged += DdlPlanAction_SelectedIndexChanged;
            //btnSaveToPlan.Click += BtnSaveToPlan_Click;
            btnSaveToList.Click += BtnSaveToList_Click;
            btnCancelToList.Click += BtnCancelToList_Click;
            gvPlans.RowDataBound += GvPlans_RowDataBound;
            gvActions.RowDataBound += GvActions_RowDataBound;
            gvPlans.RowCommand += GvPlans_RowCommand;
            if (!IsPostBack)
            {
                int userId = VSDLL.Common.Users.UserID;
                if (userId==0)
                {
                    divList.Visible = false;
                    divPlan.Visible = false;
                    divAction.Visible = false;
                    lbPTitle.Visible = false;
                    lbErr.Text = "你尚未登录，无法管理个人计划！";
                }
                else
                {
                    lbPTitle.Visible = true;
                    lbErr.Text = "";
                    try
                    {
                        //获取计划列表
                        DataSet dsPlans = TaskDAL.GetDailyTask(userId);
                        DataTable dtPlans = dsPlans.Tables[0];
                        ViewState["dtPlans"] = dtPlans;
                        //获取操作列表
                        DataSet dsActions = ActionDAL.GetAllActions(0);
                        DataTable dtActions = dsActions.Tables[0];
                        ViewState["dtActions"] = dtActions;
                        //获取操作关系列表
                        DataSet dsRelations = ActionDAL.GetActionRelation();
                        DataTable dtRelations = dsRelations.Tables[0];
                        ViewState["dtRelations"] = dtRelations;

                        //绑定计划列表
                        BindgvPlans(dtPlans);
                    }
                    catch (Exception ex)
                    {
                        lbErr.Text = ex.ToString();
                    }
                }
            }
        }



        #endregion

        #region 计划设置

        #region 计划设置事件

        /// <summary>
        /// 计划列表行命令事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GvPlans_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DelPlan")
                {
                    long planId = long.Parse(e.CommandArgument.ToString());
                    DeletePlanByPlanID(planId);
                }
                else if (e.CommandName == "EditPlan")
                {
                    lbErr.Text = "";
                    #region 修改计划
                    lbPTitle.Text = "修改计划";
                    string planId = e.CommandArgument.ToString();
                    hdfID.Value = planId;
                    hdfParentID.Value = "0";
                    DataTable dt = (DataTable)ViewState["dtPlans"];
                    DataTable dtPlans = dt.Copy();
                    DataRow dr = dtPlans.Select("PlanID=" + long.Parse(planId))[0];
                    long actionId = SystemDataExtension.GetInt32(dr, "ActionID");

                    //开始日期
                    tbStart.Text = SystemDataExtension.GetDateTime(dr, "StartDate").ToShortDateString();
                    //截止日期
                    tbDue.Text = SystemDataExtension.GetDateTime(dr, "DueDate").ToShortDateString();
                    //时长目标
                    tbDuring.Text = SystemDataExtension.GetString(dr, "During");
                    //数量目标
                    tbQuantity.Text = SystemDataExtension.GetString(dr, "Quantity");
                    //描述
                    tbDesc.Text = SystemDataExtension.GetString(dr, "Description");

                    DataTable dtActions = (DataTable)ViewState["dtActions"];
                    string planType = GetPlanTypeByPlanID(long.Parse(planId), dtPlans, dtActions);
                    hdfPlanType.Value = planType;
                    BindActions(planType, actionId.ToString());
                    if (planType == "计划")//是包含多个操作的计划
                    {
                        lbPlanActions.Visible = true;
                        cblPlanActions.Visible = true;
                        //btnSetAction.Visible = true;
                        BindPlanActions(actionId, long.Parse(planId), dtPlans, dtActions);
                    }
                    else
                    {
                        lbPlanActions.Visible = false;
                        cblPlanActions.Visible = false;
                        //btnSetAction.Visible = false;
                    }
                    //计划标题,不可更改
                    ddlPlanAction.Enabled = false;

                    divList.Visible = false;
                    divPlan.Visible = true;
                    divAction.Visible = false;

                    #endregion
                }
            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
        }

        /// <summary>
        /// 计划列表的行绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GvPlans_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((HiddenField)e.Row.FindControl("hdfPlanID")) != null)
                    {
                        HiddenField hdfPlanID = (HiddenField)e.Row.FindControl("hdfPlanID");

                        if (hdfPlanID.Value != "")
                        {
                            long planId = long.Parse(hdfPlanID.Value);
                            LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                            LinkButton lnkDel = (LinkButton)e.Row.FindControl("lnkDel");

                            DataTable dtPlans = (DataTable)ViewState["dtPlans"];
                            DataTable dtActions = (DataTable)ViewState["dtActions"];
                            string planType = GetPlanTypeByPlanID(planId,dtPlans,dtActions);

                            if (planType == "计划")
                            {
                                lnkEdit.Visible = true;
                                lnkDel.Visible = false;
                            }
                            else if(planType=="操作")
                            {
                                lnkEdit.Visible = true;
                                lnkDel.Visible = true;
                            }
                            else
                            {
                                lnkEdit.Visible = false;
                                lnkDel.Visible = true;
                            }
                        }
                    }
                    e.Row.Attributes.Add("onmouseover", "if(this!=prevselitem){this.style.backgroundColor='#E6F2FB'}");//当鼠标停留时更改背景色
                    e.Row.Attributes.Add("onmouseout", "if(this!=prevselitem){this.style.backgroundColor='#ffffff'}");//当鼠标移开时还原背景色
                    e.Row.Attributes.Add("onclick", e.Row.ClientID + ".checked=true;selectx(this)");
                }
            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
        }

        /// <summary>
        /// 计划选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DdlPlanAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = ddlPlanAction.SelectedValue;
            if (selectedValue != "0"&&hdfPlanType.Value=="计划")
            {
                lbPlanActions.Visible = true;
                cblPlanActions.Visible = true;
                DataTable dtPlans = (DataTable)ViewState["dtPlans"];
                DataTable dtActions = (DataTable)ViewState["dtActions"];
                BindPlanActions(long.Parse(selectedValue), 0, dtPlans, dtActions);
            }
        }

        /// <summary>
        /// 跳转到添加多操作计划的界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddMulti_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            InitPlanForm("计划","0");
            ddlPlanAction.AutoPostBack = true;
            lbPTitle.Text = "添加计划";
        }

        /// <summary>
        /// 跳转到添加单操作计划的界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddSingle_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            InitPlanForm("操作","0");
            ddlPlanAction.AutoPostBack = false;
            lbPTitle.Text = "添加计划";
        }

        private void InitPlanForm(string planType, string initValue)
        {
            ddlPlanAction.Enabled = true;
            BindActions(planType, initValue);
            hdfID.Value = "0";
            tbStart.Text = "";
            tbDue.Text = "";
            tbDuring.Text = "";
            tbQuantity.Text = "";
            tbDesc.Text = "";
            //控件显隐控制
            lbPlanActions.Visible = false;
            cblPlanActions.Visible = false;
            divList.Visible = false;
            divPlan.Visible = true;
            divAction.Visible = false;
        }

        /// <summary>
        /// 保存计划，并跳转到多操作计划的操作设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetActions_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            lbPTitle.Text = "设置计划操作";
            string planID = hdfID.Value;
            if (planID != "")
            {
                long planId = long.Parse(planID);
                DataTable dtPlans = (DataTable)ViewState["dtPlans"];
                DataRow[] drPlanChildren = dtPlans.Select("ParentID=" + planId);
                if (drPlanChildren.Length > 0)
                {
                    DataTable dtChildrenPlan = Common.RowsToTable(drPlanChildren);
                    gvActions.DataSource = dtChildrenPlan;
                    gvActions.DataBind();
                }
                divList.Visible = false;
                divPlan.Visible = false;
                divAction.Visible = true;
            }
        }

        /// <summary>
        /// 提交计划的编辑或新建计划的表单，并返回修改后的计划列表界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            if (ddlPlanAction.SelectedValue == "0")//没选择操作，不能保存
            {
                lbErr.Text = "你尚选择计划！";
                ddlPlanAction.Focus();
                return;
            }

            DataTable dtPlans = (DataTable)ViewState["dtPlans"];
            int userId = VSDLL.Common.Users.UserID;
            string planId = hdfID.Value;
            string actionId = ddlPlanAction.SelectedValue;

            if (string.IsNullOrEmpty(tbStart.Text.Trim()))
            {
                lbErr.Text = "计划开始日期不可为空！";
                tbStart.Focus();
                return;
            }
            DateTime dtStart = Convert.ToDateTime(tbStart.Text.Trim());
            DateTime dtDue = dtStart.AddDays(-1);
            if (!string.IsNullOrEmpty(tbDue.Text.Trim()))
            {
                dtDue = Convert.ToDateTime(tbDue.Text.Trim());
                if (dtDue < dtStart)
                {
                    lbErr.Text = "计划截止日期不可早于开始日期！";
                    tbDue.Focus();
                    return;
                }
            }

            if (string.IsNullOrEmpty(tbDuring.Text.Trim()) && string.IsNullOrEmpty(tbQuantity.Text.Trim()))
            {
                lbErr.Text = "计划目标中时长和数量至少要填写一项！";
                tbDuring.Focus();
                return;
            }
            try
            {
                long newPlanId = 0;
                if (planId == "0")//本次是新建计划
                {
                    if (IsHadSamePlaninTheseDays(long.Parse(actionId), 0, dtStart, dtDue, dtPlans))
                    {
                        lbErr.Text = "计划设定的日期范围与已有计划有重叠，请重新设置计划周期或选择其他计划！";
                        return;
                    }
                    newPlanId = AddPlan(actionId, dtPlans, userId);
                }
                else//修改计划
                {
                    newPlanId = EditPlan(planId, dtPlans, userId);
                }
                if (newPlanId != 0)
                {
                    hdfID.Value = newPlanId.ToString();
                    divPlan.Visible = false;
                    dtPlans = (DataTable)ViewState["dtPlans"];
                    if (hdfPlanType.Value == "计划")
                    {
                        lbPTitle.Text = "设置计划操作";
                        lbErr.Text = "计划保存成功，请继续设置计划操作！";
                        DataRow[] drPlanChildren = dtPlans.Select("ParentID=" + newPlanId);
                        if (drPlanChildren.Length > 0)
                        {
                            DataTable dtChildrenPlan = Common.RowsToTable(drPlanChildren);
                            gvActions.DataSource = dtChildrenPlan;
                            gvActions.DataBind();
                            btnCancelToList.Text = "取 消";
                            btnCancelToList.CommandArgument = "0";
                        }
                        else
                        {
                            btnCancelToList.CommandArgument = "1";
                            btnCancelToList.Text = "返 回";
                            lbErr.Text = "计划保存成功，你可能未为该计划选择子项操作，请点击返回重新选择，！";
                        }
                        divList.Visible = false;
                        divAction.Visible = true;
                    }
                    else
                    {
                        lbPTitle.Text = "设置计划操作";
                        lbErr.Text = "计划保存成功！";
                        BindgvPlans(dtPlans);
                        divList.Visible = true;
                        divAction.Visible = false;
                    }
                }
                else
                {
                    lbErr.Text = "计划保存失败！";
                }
            }
            catch (Exception ex)
            {
                lbErr.Text = ex.ToString();
                return;
            }

        }
        /// <summary>
        /// 取消本次编辑或新建计划的保单数据，并返回到计划列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            lbPTitle.Text = "计划列表";
            DataTable dtPlans = (DataTable)ViewState["dtPlans"];
            BindgvPlans(dtPlans);
            divList.Visible = true;
            divPlan.Visible = false;
            divAction.Visible = false;
        }

        #endregion

        #region 计划设置方法

        /// <summary>
        /// 新增计划
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="dtPlans"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private long AddPlan(string actionId, DataTable dtPlans, int userId)
        {
            long newPlanId = 0;
            try
            {
                #region 新建计划

                DataRow dr = dtPlans.NewRow();
                dr["ActionID"] = long.Parse(actionId);
                dr["UserID"] = userId;
                //计划开始日期
                if (!string.IsNullOrEmpty(tbStart.Text.Trim()))
                {
                    dr["StartDate"] = Convert.ToDateTime(tbStart.Text.Trim());
                }
                //计划截止日期
                if (!string.IsNullOrEmpty(tbDue.Text.Trim()))
                {
                    dr["DueDate"] = Convert.ToDateTime(tbDue.Text.Trim());
                }
                //时长目标
                if (!string.IsNullOrEmpty(tbDuring.Text.Trim()))
                {
                    int during = int.Parse(tbDuring.Text.Trim());
                    dr["During"] = during;
                }
                //数量目标
                if (!string.IsNullOrEmpty(tbQuantity.Text.Trim()))
                {
                    float quantity = float.Parse(tbQuantity.Text.Trim());
                    dr["Quantity"] = quantity;
                }
                //计划简介
                if (!string.IsNullOrEmpty(tbDesc.Text.Trim()))
                {
                    dr["Description"] = tbDesc.Text.Trim();
                }

                dr["Percent"] = 100;
                dr["Created"] = DateTime.Now;
                dr["CreatedBy"] = userId;
                dr["Modified"] = DateTime.Now;
                dr["ModifiedBy"] = userId;
                dr["ParentID"] = 0;
                dr["FLag"] = 1;
                newPlanId = TaskDAL.InsertPlans(null, dr);
                dtPlans.Rows.Add(dr);
                dtPlans.AcceptChanges();
                //计划中的子操作，仅针对多操作计划
                if (hdfPlanType.Value == "计划")
                {
                    for (int i = 0; i < cblPlanActions.Items.Count; i++)
                    {
                        if (cblPlanActions.Items[i].Selected)
                        {
                            string selectedAId = cblPlanActions.Items[i].Value;
                            DataRow drNew = dtPlans.NewRow();
                            drNew["UserID"] = userId;
                            drNew["ActionID"] = int.Parse(selectedAId);
                            drNew["Created"] = DateTime.Now;
                            drNew["CreatedBy"] = userId;
                            drNew["Modified"] = DateTime.Now;
                            drNew["ModifiedBy"] = userId;
                            drNew["ParentID"] = newPlanId;
                            drNew["FLag"] = 1;
                            long newActionId = TaskDAL.InsertPlans(null, drNew);
                            dtPlans.Rows.Add(drNew);
                            dtPlans.AcceptChanges();
                        }
                    }
                }
                ViewState["dtPlans"] = dtPlans;

                #endregion

            }
            catch (Exception ex)
            {
                newPlanId = 0;
                lbErr.Text = ex.ToString();
            }
            return newPlanId;
        }

        /// <summary>
        /// 修改指定ID的计划
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="dtPlans">计划表</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        private long EditPlan(string planId, DataTable dtPlans, int userId)
        {
            long newPlanId = 0;
            try
            {
                #region 修改计划
                DataRow dr = dtPlans.Select("PlanID=" + long.Parse(planId))[0];
                dr.BeginEdit();
                //计划开始日期
                if (!string.IsNullOrEmpty(tbStart.Text.Trim()))
                {
                    dr["StartDate"] = Convert.ToDateTime(tbStart.Text.Trim());
                }
                //计划截止日期
                if (!string.IsNullOrEmpty(tbDue.Text.Trim()))
                {
                    dr["DueDate"] = Convert.ToDateTime(tbDue.Text.Trim());
                }
                //时长目标
                if (!string.IsNullOrEmpty(tbDuring.Text.Trim()))
                {
                    int during = int.Parse(tbDuring.Text.Trim());
                    dr["During"] = during;
                }
                //数量目标
                if (!string.IsNullOrEmpty(tbQuantity.Text.Trim()))
                {
                    float quantity = float.Parse(tbQuantity.Text.Trim());
                    dr["Quantity"] = quantity;
                }
                //计划简介
                if (!string.IsNullOrEmpty(tbDesc.Text.Trim()))
                {
                    dr["Description"] = tbDesc.Text.Trim();
                }

                dr["Modified"] = DateTime.Now;
                dr["ModifiedBy"] = userId;
                dr.EndEdit();
                TaskDAL.UpdatePlans(null, dr);
                dtPlans.AcceptChanges();


                //对多操作的处理
                DataTable dtActions = (DataTable)ViewState["dtActions"];
                string planType = GetPlanTypeByPlanID(long.Parse(planId), dtPlans, dtActions);
                if (planType == "计划")
                {
                    List<string> aIds = new List<string>();
                    DataRow[] drsPlanActions = dtPlans.Select("ParentID=" + long.Parse(planId));
                    for (int i = 0; i < drsPlanActions.Length; i++)
                    {
                        aIds.Add(SystemDataExtension.GetInt16(drsPlanActions[i], "ActionID").ToString());
                    }
                    for (int i = 0; i < cblPlanActions.Items.Count; i++)
                    {
                        if (cblPlanActions.Items[i].Selected && !aIds.Contains(cblPlanActions.Items[i].Value))//此次选中的是原来没有的，则新增
                        {
                            string selectedAId = cblPlanActions.Items[i].Value;
                            DataRow drNew = dtPlans.NewRow();
                            drNew["UserID"] = userId;
                            drNew["ActionID"] = int.Parse(selectedAId);
                            drNew["Created"] = DateTime.Now;
                            drNew["CreatedBy"] = userId;
                            drNew["Modified"] = DateTime.Now;
                            drNew["ModifiedBy"] = userId;
                            drNew["ParentID"] = long.Parse(planId);
                            drNew["FLag"] = 1;
                            long newActionId = TaskDAL.InsertPlans(null, drNew);
                            dtPlans.Rows.Add(drNew);
                            dtPlans.AcceptChanges();
                        }
                        else if (!cblPlanActions.Items[i].Selected && aIds.Contains(cblPlanActions.Items[i].Value))//此次未选中，但原来已选择，则删除
                        {
                            string selectedAId = cblPlanActions.Items[i].Value;

                            DataRow drOld = dtPlans.Select(string.Format("ActionID={0} and ParentID={1}", int.Parse(selectedAId), long.Parse(planId)))[0];
                            dtPlans.Rows.Remove(drOld);
                            dtPlans.AcceptChanges();
                            drOld.BeginEdit();
                            drOld["Flag"] = 0;
                            drOld["Modified"] = DateTime.Now;
                            drOld["ModifiedBy"] = userId;
                            drOld.EndEdit();
                            TaskDAL.UpdatePlans(null, drOld);
                        }
                    }
                }
                ViewState["dtPlans"] = dtPlans;
                newPlanId = long.Parse(planId);
                #endregion

            }
            catch (Exception ex)
            {
                newPlanId = 0;
                lbErr.Text=ex.ToString();
            }
            return newPlanId;
        }

        /// <summary>
        /// 筛选哪些未在计划中的操作
        /// </summary>
        /// <param name="dtPlans"></param>
        /// <param name="dtActions"></param>
        /// <returns></returns>
        private DataTable GetActionsNotInPlan(DataTable dtPlans,DataTable dtActions)
        {
            DataTable dtMayPlanActions = dtActions.Copy();
            foreach (DataRow drPlan in dtPlans.Rows)
            {
                int actionId = SystemDataExtension.GetInt32(drPlan,"ActionID");
                if (dtMayPlanActions.Select("ActionID=" + actionId).Length > 0)
                {
                    DataRow drAction = dtMayPlanActions.Select("ActionID=" + actionId)[0];
                    dtMayPlanActions.Rows.Remove(drAction);
                    dtMayPlanActions.AcceptChanges();
                }
            }
            return dtMayPlanActions;
        }

        /// <summary>
        /// 是否已有相同的计划：同一用户、同一操作
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="actionId">操作ID</param>
        /// <param name="dtPlans"></param>
        /// <returns></returns>
        private bool IsHadSamePlan(int userId, long actionId, DataTable dtPlans)
        {
            string filterExpression = string.Format("ActionID ={0} and UserID ={1}", actionId, userId);
            DataRow[] drs = dtPlans.Select(filterExpression);
            if (drs.Length > 0)
                return true;
            else
                return false;
        }



        /// <summary>
        /// 在设定的时间周期内是否已有相同操作的计划
        /// </summary>
        /// <param name="actionId">操作ID</param>
        /// <param name="dtStart">开始日期</param>
        /// <param name="dtDue">结束日期</param>
        /// <param name="dtPlans">计划表</param>
        /// <param name="parentId">父级计划ID（仅针对多操作计划）</param>
        /// <returns></returns>
        private bool IsHadSamePlaninTheseDays(long actionId,long parentId, DateTime dtStart,DateTime dtDue,DataTable dtPlans)
        {
            bool isHadSame = false;
            string filterExpression = string.Format("ActionID ={0} and ParentID={1}", actionId, parentId);
            DataRow[] drs1 = dtPlans.Select(filterExpression);
            if (drs1.Length>0)//相同操作和父级
            {
                DataTable dtTemp = Common.RowsToTable(drs1);
                filterExpression = string.Format("StartDate>='{0}' and StartDate<='{1}' or DueDate >='{0}' and DueDate <='{1}'", dtStart, dtDue);
                if (dtDue < dtStart)
                {
                    filterExpression = string.Format("StartDate>='{0}' or DueDate >='{0}'", dtStart);
                }
                DataRow[] drs2 = dtTemp.Select(filterExpression);
                if (drs2.Length > 0)//相同日期
                    isHadSame= true;
            }
            return isHadSame;
        }

        /// <summary>
        /// 根据计划类型设置计划的备选数据列表
        /// </summary>
        /// <param name="planType">计划类型</param>
        /// <param name="initValue">选择初始值</param>
        private void BindActions(string planType,string initValue)
        {
            try
            {
                hdfPlanType.Value = planType;

                DataSet dsMetaData = (DataSet)ViewState["dsMetaData"];
                DataTable dtActionType = MetaDataBLL.GetMetaDataByGroup("Action", dsMetaData);
                DataRow[] drsActionType = dtActionType.Select(string.Format("Title='{0}'", planType));
                ddlPlanAction.Items.Clear();
                ddlPlanAction.Items.Add(new ListItem("--请选择计划--","0"));
                if (drsActionType.Length > 0)
                {
                    int typeId = SystemDataExtension.GetInt16(drsActionType[0], "ItemID");
                    DataTable dtActions = (DataTable)ViewState["dtActions"];
                    DataRow[] drActions = dtActions.Select("ItemTypeID=" + typeId);
                    if (drActions.Length > 0)
                    {
                        foreach (DataRow dr in drActions)
                        {
                            ListItem item = new ListItem
                            {
                                Value = SystemDataExtension.GetInt32(dr, "ActionID").ToString(),
                                Text = SystemDataExtension.GetString(dr, "Title")
                            };
                            ddlPlanAction.Items.Add(item);
                        }
                        ddlPlanAction.SelectedValue = initValue;
                    }
                }

            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
        }

        /// <summary>
        /// 根据计划ID获取计划的类型：(操作)单操作计划、（计划）多操作计划、（已删除）已删除计划、（计划操作）计划的操作
        /// </summary>
        /// <param name="planId">计划ID</param>
        /// <param name="dtPlans">计划表</param>
        /// <param name="dtActions">操作表</param>
        /// <returns></returns>
        private string GetPlanTypeByPlanID(long planId,DataTable dtPlans,DataTable dtActions)
        {
            string planType = "";
            DataRow[] drsPlan = dtPlans.Select("PlanID="+planId);
            if (drsPlan.Length<=0)
            {
                planType= "已删除";
            }
            else
            {
                DataRow drPlan = drsPlan[0];
                long actionId = SystemDataExtension.GetInt32(drPlan, "ActionID");//计划对应的操作ID
                long parentId = SystemDataExtension.GetInt32(drPlan,"ParentID");//计划的父级ID，若为0，则为计划，否则为多操作计划关联的某一个操作
                if (parentId==0)
                {
                    DataRow[] drsActions = dtActions.Select("ActionID=" + actionId);
                    if (drsActions.Length > 0)
                    {
                        DataRow drAction = drsActions[0];
                        int typeId = SystemDataExtension.GetInt16(drAction, "ItemTypeID");
                        if (typeId != 0)
                        {
                            //获取元数据集合
                            DataSet dsMetaData = MetaDataDAL.GetGroupMetaData();
                            ViewState["dsMetaData"] = dsMetaData;
                            DataTable dtActionType = MetaDataBLL.GetMetaDataByGroup("Action", dsMetaData);
                            DataRow[] drsActionType = dtActionType.Select("ItemID="+typeId);
                            if (drsActionType.Length > 0)
                            {
                                planType = SystemDataExtension.GetString(drsActionType[0], "Title");
                            }
                        }
                    }
                }
                else
                {
                    planType = "计划操作";
                }
            }
            return planType;
        }

        /// <summary>
        /// 删除对应ID的计划
        /// </summary>
        /// <param name="planId">计划ID</param>
        private void DeletePlanByPlanID(long planId)
        {
            try
            {
                DataTable dtPlans = (DataTable)ViewState["dtPlans"];
                DataRow[] drs = dtPlans.Select("PlanID=" + planId);
                if (drs.Length > 0)
                {
                    DataRow dr = drs[0];//计划

                    dr.BeginEdit();
                    dr["Flag"] = 0;//数据库逻辑删除，将标记置为0
                    TaskDAL.UpdatePlans(null, dr);
                    lbErr.Text = "计划删除成功！";
                    dr.EndEdit();
                    dtPlans.Rows.Remove(dr);//数据集中物理删除，将行删除
                    ViewState["dtPlans"] = dtPlans;
                    BindgvPlans(dtPlans);
                }
                else
                    lbErr.Text = "你删除的计划已不存在！";
            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
        }

        #endregion

        #endregion

        #region 多操作计划的操作设置

        /// <summary>
        /// 不保存操作设置，返回到计划列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelToList_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            divAction.Visible = false;
            if (btnCancelToList.CommandArgument=="0")//返回列表
            {
                //控件显隐控制
                lbPTitle.Text = "计划列表";
                divList.Visible = true;
                divPlan.Visible = false;
                DataTable dtPlans = (DataTable)ViewState["dtPlans"];
                BindgvPlans(dtPlans);
                hdfID.Value = "";
                hdfParentID.Value = "";
            }
            else//返回重新修改计划
            {
                //控件显隐控制
                lbPTitle.Text = "修改计划";
                divList.Visible = false;
                divPlan.Visible = true;
            }
        }

        /// <summary>
        /// 保存操作设置，并返回到所有计划的列表界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveToList_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            //保存事件
            lbPTitle.Text = "计划列表";
            DataTable dtPlans = (DataTable)ViewState["dtPlans"];
            SavePlanActions(ref dtPlans);
            BindgvPlans(dtPlans);
            //控件显隐控制
            divList.Visible = true;
            divPlan.Visible = false;
            divAction.Visible = false;
            hdfID.Value = "";
            hdfParentID.Value = "";
        }


        /// <summary>
        /// 保存操作设置，并返回到当前计划的编辑界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveToPlan_Click(object sender, EventArgs e)
        {
            lbErr.Text = "";
            //保存事件
            lbPTitle.Text = "编辑计划";
            DataTable dtPlans = (DataTable)ViewState["dtPlans"];
            SavePlanActions(ref dtPlans);
            BindgvPlans(dtPlans);
            //控件显隐控制
            divList.Visible = false;
            divPlan.Visible = true;
            divAction.Visible = false;
            lbPlanActions.Visible = true;
            cblPlanActions.Visible = true;
            btnSubmit.Text = "仅保存计划";
        }


        /// <summary>
        /// 多操作计划的已选择操作的列表的行绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GvActions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((HiddenField)e.Row.FindControl("hdfActionID")) != null)
                    {
                        //确定计划名称
                        HiddenField hdfActionID = (HiddenField)e.Row.FindControl("hdfActionID");

                        if (hdfActionID.Value != "")
                        {
                            string actionId = hdfActionID.Value;
                            Label lbAction = (Label)e.Row.FindControl("lbAction");
                            DataTable dtActions = (DataTable)ViewState["dtActions"];
                            DataRow[] drs = dtActions.Select("ActionID =" + int.Parse(actionId));
                            if (drs.Length > 0)
                            {
                                lbAction.Text = SystemDataExtension.GetString(drs[0], "Title");
                            }
                            else
                            {
                                lbAction.Text = "未找到操作！";
                            }
                        }
                    }
                    if (((Label)e.Row.FindControl("lbPercent"))!=null)
                    {
                        Label lbPercent = (Label)e.Row.FindControl("lbPercent");
                        lbPercent.Text = lbPercent.Text + "%";
                    }
                    e.Row.Attributes.Add("onmouseover", "if(this!=prevselitem){this.style.backgroundColor='#E6F2FB'}");//当鼠标停留时更改背景色
                    e.Row.Attributes.Add("onmouseout", "if(this!=prevselitem){this.style.backgroundColor='#ffffff'}");//当鼠标移开时还原背景色
                    e.Row.Attributes.Add("onclick", e.Row.ClientID + ".checked=true;selectx(this)");
                }

            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
        }


        /// <summary>
        /// 绑定多操作计划的多选框，并将计划已选择的操作条目设置选中
        /// </summary>
        /// <param name="actionId">操作ID</param>
        /// <param name="planId">计划ID</param>
        /// <param name="dtPlans">计划表</param>
        /// <param name="dtActions">操作表</param>
        private void BindPlanActions(long actionId,long planId,DataTable dtPlans,DataTable dtActions)
        {
            DataTable dtRelations = (DataTable)ViewState["dtRelations"];
            int relationId = 2;
            DataRow[] drSysActions = dtRelations.Select(string.Format("ItemID={0} and RelationID ={1}", actionId, relationId));
            if (drSysActions.Length > 0)
            {
                cblPlanActions.Items.Clear();
                foreach (DataRow dr in drSysActions)
                {
                    long sysactionId = SystemDataExtension.GetInt32(dr, "RelateditemID");
                    ListItem item = new ListItem
                    {
                        Value = sysactionId.ToString(),
                        Text = SystemDataExtension.GetString(dtActions.Select("ActionID=" + sysactionId)[0], "Title")
                    };
                    cblPlanActions.Items.Add(item);
                }
                if (planId!=0)//有计划ID传入，表示这是编辑多操作计划，此时需要查询该计划对应的操作哪些已选中
                {
                    DataRow[] drMyActions = dtPlans.Select("ParentID=" + planId);
                    if (drMyActions.Length > 0)
                    {
                        string selectedValue = "";
                        var builder = new StringBuilder();
                        builder.Append(selectedValue);
                        foreach (DataRow dr in drMyActions)
                        {
                            builder.Append(SystemDataExtension.GetInt32(dr, "ActionID") + ";");
                        }
                        selectedValue = builder.ToString();
                        SetCBListChecked(cblPlanActions, selectedValue, ";");
                    }
                }
            }
        }

        /// <summary>
        /// 保存计划操作设置
        /// </summary>
        /// <param name="dtPlans">计划表</param>
        private void SavePlanActions(ref DataTable dtPlans)
        {
            int userId = VSDLL.Common.Users.UserID;
            string parentId = hdfID.Value;
            DataRow drPlan = dtPlans.Select("PlanID=" + long.Parse(parentId))[0];
            foreach (GridViewRow gvr in gvActions.Rows)
            {
                if (gvr.RowType== DataControlRowType.DataRow)
                {
                    HiddenField hdfPlanID = (HiddenField)gvr.FindControl("hdfPlanID");
                    long planId = long.Parse(hdfPlanID.Value);
                    DataRow dr = dtPlans.Select("PlanID=" + planId)[0];
                    dr.BeginEdit();
                    TextBox tbPlanDuring = (TextBox)gvr.FindControl("tbDuring");
                    if (!string.IsNullOrWhiteSpace(tbPlanDuring.Text.Trim()))
                        dr["During"] = int.Parse(tbPlanDuring.Text.Trim());

                    TextBox tbPlanQuantity = (TextBox)gvr.FindControl("tbQuantity");
                    if (!string.IsNullOrWhiteSpace(tbPlanQuantity.Text.Trim()))
                        dr["Quantity"] = float.Parse(tbPlanQuantity.Text.Trim());

                    TextBox tbPlanPercent = (TextBox)gvr.FindControl("tbPercent");
                    if (!string.IsNullOrWhiteSpace(tbPlanPercent.Text.Trim()))
                        dr["Percent"] = int.Parse(tbPlanPercent.Text.Trim());
                    dr["StartDate"] = drPlan["StartDate"];
                    dr["DueDate"] = drPlan["DueDate"];
                    dr["Modified"] = DateTime.Now;
                    dr["ModifiedBy"] = userId;
                    dr.EndEdit();
                    TaskDAL.UpdatePlans(null,dr);
                    dtPlans.AcceptChanges();
                }
            }
        }

        #endregion

        #region 公用方法

        /// <summary>
        /// 将计划表绑定到计划列表GridView控件
        /// </summary>
        /// <param name="dtPlans">数据源</param>
        private void BindgvPlans(DataTable dtPlans)
        {
            try
            {
                DataTable dtActions= (DataTable)ViewState["dtActions"];
                //扩展计划表，增加一个字段"Title"，保存操作的名称，也是计划的名称
                DataTable dtTemp = dtPlans.Copy();
                dtTemp.Columns.Add("Title");
                foreach (DataRow drTemp in dtTemp.Rows)
                {
                    int actionId = SystemDataExtension.GetInt16(drTemp, "ActionID");
                    DataRow drAction = ActionBLL.GetActionByID(actionId, dtActions);
                    drTemp.BeginEdit();
                    drTemp["Title"] = drAction["Title"];
                    drTemp.EndEdit();
                }
                dtTemp.AcceptChanges();
                //绑定分级计划列表
                DataTable dtLPlans = TaskBLL.GetTasksWithLevel(dtTemp, "Title", "ParentID", false);
                gvPlans.DataSource = dtLPlans;
                gvPlans.DataBind();
            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
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

        #region 临时

        #endregion
    }
}
