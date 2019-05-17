using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using VSDLL;
using VSDLL.DAL;
using VSDLL.BLL;

namespace VSProject.ActionOption
{
    public class ActionOptionUserControl:UserControl
    {
        #region 控件定义
        public ActionOption webObj;
        protected Label lbErr;
        protected GridView gvActions;
        protected Button btnSave;
        protected Button btnClose;
        #endregion
        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int userId = GetUserId;
                if (userId != 0)
                {
                    DataSet dsActionStruct = TablesBLL.GetTableFields("Action", null);//操作表结构数据集
                    ViewState["dsActionStruct"] = dsActionStruct;
                    DataSet dsMyActionStruct = TablesBLL.GetTableFields("User_Action", null);//用户-操作表结构数据集
                    ViewState["dsMyActionStruct"] = dsMyActionStruct;
                    DataSet dsMetaData = MetaDataDAL.GetGroupMetaData();
                    ViewState["dsMetaData"] = dsMetaData;
                    DataSet myActions = ActionDAL.GetUserActionByUserID(userId);
                    ViewState["myActions"] = myActions;
                    DataSet sysActions = ActionDAL.GetAllActions();
                    ViewState["sysActions"] = sysActions;
                    DataTable dt = GetBindData(userId, myActions, sysActions, dsMetaData, dsActionStruct, dsMyActionStruct);
                    BindGV(gvActions, dt);
                }
                else
                {
                    lbErr.Text = "请先登录";
                }
                //btnClose.Click += btnClose_Click;
            }
            btnSave.Click += btnSave_Click;
        }
        
          
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string errmsg = SaveGVToDB();
            if (Request.QueryString["Source"] != null)
                Response.Redirect(Request.QueryString["Source"].ToString());
            else
            {
                if (errmsg.Substring(0, errmsg.IndexOf(';')) == "")
                {
                    errmsg = errmsg.Substring(1);
                }
                lbErr.Text = errmsg;
            }
        }
       
        protected void gvActions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((Label)e.Row.FindControl("lbNo") != null)
            {
                Label lbNo = (Label)e.Row.FindControl("lbNo");
                lbNo.Text = (e.Row.RowIndex + 1).ToString();
            }

            //行绑定频度下拉框 ddlFrequency
            if (((DropDownList)e.Row.FindControl("ddlFrequency")) != null)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlFrequency");
                string initValue = ((HiddenField)e.Row.FindControl("hdfFrequency")).Value;

                DDLBind(ddl, "Frequency",initValue);
            }
            //行绑定使用时间段下拉框 ddlperiod
            if (((DropDownList)e.Row.FindControl("ddlShiDuan")) != null)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlShiDuan");
                string initValue = ((HiddenField)e.Row.FindControl("hdfShiDuan")).Value;
                DDLBind(ddl, "ShiDuan",initValue);
            }

            //if ((HiddenField)e.Row.FindControl("hdfUrl") != null && (HyperLink)e.Row.FindControl("lnkaction") != null)
            //{
            //    HiddenField hdfurl = (HiddenField)e.Row.FindControl("hdfUrl");
            //    HyperLink lnkAction = (HyperLink)e.Row.FindControl("lnkaction");
            //    if (string.IsNullOrEmpty(hdfurl.Value))
            //    {
            //        lnkAction.Attributes.Add("Class", "nonegvlink");
            //    }
            //    else
            //    {
            //        lnkAction.Attributes.Add("Class", "gvlink");
            //    }
            //}


            if (((Label)e.Row.FindControl("lbID")) != null && ((HyperLink)e.Row.FindControl("lnkaction")) != null)
            {
                HyperLink lnkAction = (HyperLink)e.Row.FindControl("lnkaction");
                Label lbId = (Label)e.Row.FindControl("lbID");
                string Id = lbId.Text;
                if (Id != "0")//新的操作
                {
                    e.Row.Attributes.CssStyle.Add("background-color", "#ebebeb");
                    //CheckBox cbSel = (CheckBox)e.Row.FindControl("cbSel");
                    //cbSel.Checked = true;
                    lnkAction.ToolTip = "已个性化，可修改设置";
                }
                else
                {
                    lnkAction.ToolTip = "未个性化，请设置";
                }
            }
        }
        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;
            string initValue = ddl.SelectedValue;
            ddl.Attributes.Add("style", GetStyle(initValue));
        }
        #endregion
        #region 方法
        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="gv">表格控件</param>
        /// <param name="dt">数据表</param>
        private void BindGV(GridView gv, DataTable dt)
        {
            gv.DataSource = dt;
            gv.DataBind();
        }
        /// <summary>
        /// 绑定表格中的下拉框（时段、频次）
        /// </summary>
        /// <param name="ddl">下拉框</param>
        /// <param name="gpTitle">标题</param>
        /// <param name="initValue">值</param>
        private void DDLBind(DropDownList ddl, string gpTitle, string initValue)
        {
            //先清空DropDownList
            ddl.Items.Clear();
            //再绑定DropDownList
            string cnTitle = gpTitle == "Frequency" ? "--请选择--" : "--请选择--";
            ddl.Items.Add(new ListItem(cnTitle, "0"));
            DataSet dsMetaData = (DataSet)ViewState["dsMetaData"];
            DataTable dtMetas = MetaDataBLL.GetMetaDataByGroup(gpTitle, dsMetaData);//以gpTitle为父级的常量表
            dtMetas.DefaultView.Sort = "ItemID Desc";
            dtMetas = dtMetas.DefaultView.ToTable();
            for (int i = 0; i <dtMetas.Rows.Count; i++)
            {
                DataRow dr = dtMetas.Rows[i];
                ddl.Items.Add(new ListItem(dr["Title"].ToString(), dr["ItemID"].ToString()));
                ddl.Items[i].Attributes.Add("style", GetStyle(dr["ItemID"].ToString()));
            }

            //设置DropDownList初始项
            if (string.IsNullOrEmpty(initValue))
                initValue = "0";
            ddl.SelectedValue = initValue;
            ddl.Attributes.Add("style", GetStyle(initValue));
            //设置DropDownList事件属性
            //ddl.AutoPostBack = true;
            //ddl.SelectedIndexChanged += ddl_SelectedIndexChanged;
        }

        


        /// <summary>
        /// 初始化一个数据表，保存界面数据集
        /// </summary>
        /// <returns></returns>
        private DataTable InitSouceTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IsMy");//0
            dt.Columns.Add("Title");//1
            dt.Columns.Add("ActionID");//2

            dt.Columns.Add("MinDuring");//3
            dt.Columns.Add("MaxDuring");//4
            dt.Columns.Add("NormalDuring");//5
            dt.Columns.Add("FrequencyID");//6
            dt.Columns.Add("ShiDuanID");//7
            dt.Columns.Add("Description");//8
            dt.Columns.Add("Healthy");//9
            dt.Columns.Add("Interaction");//10
            dt.Columns.Add("Probability");//11


            dt.Columns.Add("Url");//12
            //系统设置项

            dt.Columns.Add("Measurement");//13
            dt.Columns.Add("MUnits");//14
            dt.Columns.Add("SysFrequencyID");//15
            dt.Columns.Add("SysShiduanID");//16
            dt.Columns.Add("SysHealthy");//17
            dt.Columns.Add("SysInteraction");//18
            dt.Columns.Add("SysDescription");//19
            dt.Columns.Add("SysProbability");//20
            dt.Columns.Add("SysMinDuring");//21
            dt.Columns.Add("SysMaxDuring");//22
            dt.Columns.Add("SysNormalDuring");//23
            dt.Columns.Add("UserID");
            return dt;
        }


        /// <summary>
        /// 根据当前用户ID，获取操作设置数据
        /// </summary>
        /// <param name="myActions">我的操作设置表</param>
        /// <param name="sysActions">系统操作表</param>
        /// <param name="dsMetaData">元数据常量表数据集</param>
        /// <param name="dsActionStruct">Action表结构数据集</param>
        /// <param name="userId">当前用户ID</param>
        /// <param name="dsMyActionStruct">User_Action表结构数据集</param>
        /// <returns></returns>
        private DataTable GetBindData(int userId,DataSet myActions, DataSet sysActions, DataSet dsMetaData, DataSet dsActionStruct,DataSet dsMyActionStruct)
        {
            DataTable dt = InitSouceTable();
            try
            {
                DataTable dtMyActions = myActions.Tables[0];
                //int myCount = dtMyActions.Rows.Count;
                DataTable dtSysActions = sysActions.Tables[0];
                int sysCount = dtSysActions.Rows.Count;
                for (int i = 0; i < sysCount; i++)
                {
                    DataRow dr = dt.NewRow();
                    DataRow drSys = dtSysActions.Rows[i];
                    int actionId = SystemDataExtension.GetInt32(drSys, "ActionID");
                    DataRow[] drsMy = dtMyActions.Select("ActionID="+actionId);
                    dr["ActionID"] = actionId;
                    dr["Title"] = drSys["Title"];
                    string[] myflds = new string[] { "MinDuring","MaxDuring","NormalDuring","FrequencyID","ShiDuanID","Healthy","Interaction","Description","Probability"};
                    if (drsMy.Length > 0)//该操作已有个性化设置
                    {
                        DataRow drMy = drsMy[0];
                        dr["IsMy"] = 1;
                        for (int k = 0; k < myflds.Length; k++)
                        {
                            string fldName = myflds[k];
                            dr[fldName] = drMy[fldName];

                        }
                    }
                    else
                    {
                        dr["IsMy"] = 0;
                        for (int k = 0; k < myflds.Length; k++)
                        {
                            string fldName = myflds[k];
                            dr[fldName] = null;
                        }
                    }
                    dr["Url"] = string.Format("{0}?uid={1}&aid={2}", webObj.ActionDetailsUrl, userId, actionId);

                    //系统设置

                    //系统的操作频度设置
                    string fldValue = SystemDataExtension.GetString(drSys, "FrequencyID");
                    dr["SysFrequencyID"] = GetToolTip(dsMetaData, dsActionStruct, "Frequency", fldValue, 1);
                    //系统的操作时段设置
                    fldValue = SystemDataExtension.GetString(drSys, "ShiDuanID");
                    dr["SysShiduanID"] = GetToolTip(dsMetaData, dsActionStruct, "ShiDuan", fldValue, 1);
                    //系统健康相关度设置
                    fldValue = SystemDataExtension.GetString(drSys, "Healthy");
                    dr["SysHealthy"] = GetToolTip(dsMetaData, dsActionStruct, "Healthy", fldValue, 0);
                    //系统的互动值相关度设置
                    fldValue = SystemDataExtension.GetString(drSys, "Interaction");
                    dr["SysInteraction"] = GetToolTip(dsMetaData, dsActionStruct, "Interaction", fldValue, 0);
                    //系统的使用概率的设置
                    fldValue = SystemDataExtension.GetString(drSys, "Probability");
                    dr["SysProbability"] = GetToolTip(dsMetaData, dsActionStruct, "Probability", fldValue, 0);
                    //系统的操作说明的设置
                    fldValue = SystemDataExtension.GetString(drSys, "Description");
                    dr["SysDescription"] = GetToolTip(dsMetaData, dsActionStruct, "Description", fldValue, 0);

                    string jiliang ="时长计量单位：分钟"+ Environment.NewLine;
                    fldValue = SystemDataExtension.GetString(drSys, "Measurement");
                    if (fldValue != string.Empty)
                        jiliang += string.Format("数量计量方式：{0}{1}", fldValue, Environment.NewLine);
                    fldValue = SystemDataExtension.GetString(drSys, "MUnits");
                    if (fldValue != string.Empty)
                        jiliang += string.Format("数量计量单位：{0}{1}", fldValue, Environment.NewLine);

                    DataRow[] drs = dsMyActionStruct.Tables[0].Select("字段名='MinDuring'");
                    if (drs.Length > 0)
                        dr["SysMinDuring"] = SystemDataExtension.GetString(drs[0], "说明")+Environment.NewLine+jiliang;
                    drs = dsMyActionStruct.Tables[0].Select("字段名='MaxDuring'");
                    if (drs.Length > 0)
                        dr["SysMaxDuring"] = SystemDataExtension.GetString(drs[0], "说明")+Environment.NewLine+jiliang;
                    drs = dsMyActionStruct.Tables[0].Select("字段名='NormalDuring'");
                    if (drs.Length > 0)
                        dr["SysNormalDuring"] = SystemDataExtension.GetString(drs[0], "说明")+Environment.NewLine + jiliang;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                dt = null;
                throw;
            }
            return dt;
        }

        /// <summary>
        /// 读取GridView数据到数据表
        /// </summary>
        /// <param name="gv">GridView控件ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public string SaveGVToDB()
        {
            int userId = GetUserId ;
            string errMsg = "";
            DataSet dsMyAction = (DataSet)ViewState["myActions"];
            DataTable dtMyAction = dsMyAction.Tables[0].Clone();
            int m = 0, n = 0;//m修改个数，n新建个数
            for (int rCount =0; rCount < gvActions.Rows.Count; rCount++)
            {
                 GridViewRow gvr = gvActions.Rows[rCount];
               //IsMy 是插入01，更新11，还是未设置0或1
                HiddenField hdfisMy = (HiddenField)gvr.FindControl("hdfIsMy");
                string isMy = hdfisMy.Value;
                //没有修改则不进行遍历
                if (isMy == "0" || isMy == "1") continue;
                DataRow dr = dtMyAction.NewRow();
                //用户ID UserID
                dr["UserID"] = userId;
                //操作ID ActionID
                HiddenField hdfacId = (HiddenField)gvr.FindControl("hdfacId");
                dr["ActionID"] = hdfacId.Value;

                DropDownList ddlFrequency = (DropDownList)gvr.FindControl("ddlFrequency");
                if (ddlFrequency.SelectedItem.Value != "0")
                    dr["FrequencyID"] = ddlFrequency.SelectedItem.Value;

                DropDownList ddlShiDuan = (DropDownList)gvr.FindControl("ddlShiDuan");
                if (ddlShiDuan.SelectedItem.Value != "0")
                    dr["ShiduanID"] = ddlShiDuan.SelectedItem.Value;

                TextBox tbmin = (TextBox)gvr.FindControl("tbMin");
                if (tbmin.Text!="")
                    dr["MinDuring"] = tbmin.Text;
                TextBox tbmax = (TextBox)gvr.FindControl("tbMax");
                if (tbmax.Text!="")
                    dr["MaxDuring"] = tbmax.Text;
                TextBox tbduring = (TextBox)gvr.FindControl("tbNormal");
                if (tbduring.Text!="")
                    dr["NormalDuring"] = tbduring.Text;
                TextBox tbHealthy = (TextBox)gvr.FindControl("tbHealthy");
                if (tbHealthy.Text!="")
                    dr["Healthy"] = tbHealthy.Text;
                TextBox tbInteraction = (TextBox)gvr.FindControl("tbInteraction");
                if (tbInteraction.Text!="")
                    dr["Interaction"] = tbInteraction.Text;
                TextBox tbProbability = (TextBox)gvr.FindControl("tbProbability");
                if (tbProbability.Text!="")
                    dr["Probability"] = tbProbability.Text;
                TextBox tbDesc = (TextBox)gvr.FindControl("tbDesc");
                if (tbDesc.Text!="")
                    dr["Description"] = tbDesc.Text;
                if (isMy == "01")//新设置，做插入操作
                {
                    dr["Created"] = DateTime.Now;
                    dr["CreatedBy"] = userId;
                    dr["Modified"] = DateTime.Now;
                    dr["ModifiedBy"] = userId;
                    ActionBLL.OptionUserAction(isMy, dr, ref errMsg);
                    if (errMsg == "")//没有错误
                    {
                        hdfisMy.Value = "1";
                        m++;
                    }
                }
                else if (isMy == "11")//非新设置，做更新操作
                {
                    dr["Modified"] = DateTime.Now;
                    dr["ModifiedBy"] = userId;
                    ActionBLL.OptionUserAction(isMy, dr, ref errMsg);
                    if (errMsg =="")
                    n++;
                }
                if (errMsg != "")
                {
                    break;
                }
            }
            if (m != 0 || n != 0)
            {
                errMsg += string.Format(";新增操作设置{0}个,修改操作设置{1}个", m, n);
            }
            else
            {
                errMsg += ";本次执行未设置任何操作";
            }
            return errMsg;
        }


        /// <summary>
        /// 根据标记号Flag设置背景颜色
        /// </summary>
        /// <param name="flag">标记号</param>
        /// <returns></returns>
        private string GetStyle(string flag)
        {
            string style = "background-color:RGB(209,209,209)";
            switch (flag)
            {
                case "6":
                    style = "background-color:RGB(255,144,144)";
                    break;
                case "5":
                    style = "background-color:RGB(246,242,144)";
                    break;
                case "4":
                    style = "background-color:RGB(191,238,125)";
                    break;
                case "3":
                    style = "background-color:RGB(171,248,250)";
                    break;
                case "2":
                    style = "background-color:RGB(255,209,255)";
                    break;
                case "1":
                    style = "background-color:RGB(245 245 220)";
                    break;
                default:
                    style = "background-color:RGB(209,209,209)";
                    break;
            }
            return style;
        }


        /// <summary>
        /// 获取指定字段的说明文字
        /// </summary>
        /// <param name="dsMetaData">元数据集</param>
        /// <param name="dsActionStruct">Action表结构数据集</param>
        /// <param name="gpTitle">元数据组名</param>
        /// <param name="sysfldValue">系统值</param>
        /// <param name="isfk">是否外键</param>
        /// <returns></returns>
        private string GetToolTip(DataSet dsMetaData,DataSet dsActionStruct, string gpTitle,string sysfldValue,int isfk)
        {
            string tips ="";
            DataRow[] drs = dsActionStruct.Tables[0].Select(string.Format("字段名='{0}'", gpTitle));
            if (drs.Length > 0)
                tips = drs[0]["说明"] +Environment.NewLine+ GetMetadata(dsMetaData, gpTitle,sysfldValue,isfk);
            return tips;
        }

        /// <summary>
        /// 获取指定组名的元数据
        /// </summary>
        /// <param name="dsMetaData">元数据集</param>
        /// <param name="gpTitle">元数据组名</param>
        /// <param name="sysfldValue">系统给定值</param>
        /// <param name="isfk">是否外键</param>
        /// <returns></returns>
        private string GetMetadata(DataSet dsMetaData, string gpTitle, string sysfldValue, int isfk)
        {
            string mdata = "无推荐值";
            if (sysfldValue != string.Empty)
            {
                if (isfk == 0)
                {
                    mdata ="推荐值："+ sysfldValue;
                }
                else
                {
                    DataTable dtMetas = MetaDataBLL.GetMetaDataByGroup(gpTitle, dsMetaData);//以gpTitle为父级的常量表
                    DataRow[] drs = dtMetas.Select("ItemID=" + sysfldValue);
                    if (drs.Length > 0)
                        mdata = "推荐值：" + drs[0]["Title"];
                }
            }
            return mdata;
        }

        /// <summary>
        /// 获取当前登录用户的ID，未登录时返回0
        /// </summary>
        /// <returns></returns>
        private int GetUserId 
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
    }
}
