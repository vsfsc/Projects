using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace MyTasks.NewActivity
{
    public partial class NewActivityUserControl : UserControl
    {
        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                dtStart.SelectedDate = DateTime.Today;
                InitControl();

            }
            ViewState["dtTime"] = dtStart.SelectedDate; 
            dtStart.DateOnly = false;
            dtStart.DateChanged += DtStart_DateChanged;
            dtStart.AutoPostBack = true;
            btnCancel.Click += BtnCancel_Click;
            btnSave.Click += BtnSave_Click;
            ddlAction.SelectedIndexChanged += DdlAction_SelectedIndexChanged;
        }

        private void DdlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["result"] == null) return;
            if (ddlAction.SelectedItem == null) return;
            dtStart.SelectedDate =(DateTime ) ViewState["dtTime"]; 
            DataTable  items = ViewState["result"] as DataTable ;
            if (!ddlAction.SelectedValue.Contains("-")) return;
            string[] ids = ddlAction.SelectedValue.Split('-');
            DataRow   item= items.Select (string.Format ("id={0}",int.Parse(ids[1])))[0];
            txtActitualDuring.Text = item["ActualDuring"].ToString();

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            int id = 0;
            double during = double.Parse(txtActitualDuring.Text == "" ? "0" : txtActitualDuring.Text);
            bool exits = AssistantExits(dtStart.SelectedDate,during  , ref id);
            if (exits)
            {
                lblMsg.Text = "此时间段时已经存在其他活动，指重新指定活动时间或活动时长！";
                return;
            }
            int actionID;
            if (txtAction.Text != "")
            {
                actionID = AddAction(txtAction.Text);
            }
            else
                actionID = int.Parse (ddlAction.SelectedValue.Split('-')[0]); 
             
            exits = AssistantExits(dtStart.SelectedDate,during , ref id, "Plan");
            AddAssistant(exits ? id : 0, dtStart.SelectedDate, int.Parse(txtActitualDuring.Text),actionID);
            RetPage(1);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            RetPage();
        }

        private void DtStart_DateChanged(object sender, EventArgs e)
        {
           InitControl();
            DdlAction_SelectedIndexChanged(null,null);
        }
        protected TextBox txtStartDate;
        protected DropDownList ddlStartDateHours;
        protected DropDownList ddlStartDateMinutes;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            txtStartDate = (TextBox)dtStart.FindControl("dtStartDate");
            ddlStartDateHours = (DropDownList)dtStart.FindControl(dtStart.ID + "DateHours");
            ddlStartDateMinutes = (DropDownList)dtStart.FindControl(dtStart.ID + "DateMinutes");
            this.txtStartDate.TextChanged += new EventHandler(DtStart_DateChanged);
            this.ddlStartDateHours.AutoPostBack = true;
            this.ddlStartDateHours.SelectedIndexChanged += new EventHandler(DtStart_DateChanged);
            this.ddlStartDateMinutes.AutoPostBack = true;
            this.ddlStartDateMinutes.SelectedIndexChanged += new EventHandler(DtStart_DateChanged);
        }
        #endregion
        #region 属性
        public NewActivity objWeb { get; set; }
        #endregion
        #region 方法
        private void RetPage(int flag=0)
        {
            if (Request.QueryString["Source"] != null)
            {
                string url = Request.QueryString["Source"];
                Response.Redirect(url);
                return;
            }
            if (flag==1 )
            {
                lblMsg.Text = "保存成功！";
            }
        }
        private DataTable GetAllActions()
        {
            DataTable dt = null;
            SPUser user = SPContext.Current.Web.CurrentUser;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
         {
             try
             {
                 using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
                 {
                     using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
                     {

                         SPList spList = spWeb.Lists.TryGetList(objWeb.ActionListName);
                         if (spList == null)
                         {
                             lblMsg.Text = objWeb.ActionListName + " 列表不存在！";
                         }
                         SPQuery qry = new SPQuery();
                         qry.ViewFields = "<FieldRef Name='Title' /><FieldRef Name='ID' />";
                         qry.Query = @"<Where><Or><And><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Lookup'>" + user.ID + "</Value></Eq><Eq><FieldRef Name='Flag' /><Value Type='Number'>11</Value></Eq></And><Eq><FieldRef Name='Flag' /><Value Type='Number'>0</Value></Eq></Or></Where>";
                         SPListItemCollection items = spList.GetItems(qry);
                         dt = items.GetDataTable();

                     }
                 }
             }
             catch
             {
             }
         });
            return dt;
        }
        /// <summary>
        /// 添加新操作
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private int AddAction(string action)
        {
            int id=0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
         {
             try
             {
                  using (SPSite spSite = new SPSite(SPContext.Current.Site.ID )) //找到网站集
                  {
                     using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID ))
                     {
                        
                             SPList spList = spWeb.Lists.TryGetList(objWeb.ActionListName);
                             if (spList == null)
                             {
                                 lblMsg.Text = objWeb.ActionListName + " 列表不存在！";
                             }
                         SPQuery qry = new SPQuery();
                         qry.ViewFields = "<FieldRef Name='Title' /><FieldRef Name='ID' />";
                         qry.Query = @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + action  +
                             "</Value></Eq></Where>";
                         SPListItemCollection items = spList.GetItems(qry);
                         if (items.Count > 0)
                             id = (int)items[0]["ID"]; 
                         else
                         {
                             SPListItem item = spList.Items.Add();
                             item[spList.Fields.GetFieldByInternalName("Title").Title] = action;
                             item[spList.Fields.GetFieldByInternalName("Flag").Title] = "11";
                             item["创建者"] = SPContext.Current.Web.CurrentUser.ID;
                             item.Update();
                             id = item.ID;
                         }

                     }
                 }
             }
             catch
             {

             }
         });
            return id;
        }
        private void InitControl()
        {
            DataTable dt = GetActionsByWeek();
            ddlAction.Items.Clear();
            if (dt == null)
            {
                dt = GetAllActions();
                if (dt == null) return;
                foreach (DataRow dr in dt.Rows)
                {
                    ddlAction.Items.Add(new ListItem(dr["Title"].ToString(), dr["ID"].ToString()));
                }
            }
            else
            {
                SPFieldLookupValue fld;
                foreach (DataRow dr in dt.Rows)
                {
                    fld = new SPFieldLookupValue(dr["CustAction"].ToString());
                    if (fld.LookupId > 0)
                    {
                        ddlAction.Items.Add(new ListItem(fld.LookupValue, fld.LookupId.ToString() + "-" + dr["ID"]));
                    }

                }
            }
        }
        //获取当前用户最近一周的操作
        private DataTable  GetActionsByWeek(string dtType="Actual")
        {
            SPWeb web = SPContext.Current.Web;
            int userID = web.CurrentUser.ID;
            SPList lstAction = web.Lists.TryGetList(objWeb.ListName);
            if (lstAction ==null)
            {
                lblMsg.Text = objWeb.ListName +" 列表不存在！";
                return null;
            }
            //判断当前时间是否存在数据
            string txtDate = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtStart.SelectedDate);
            SPQuery qry = new SPQuery();
            qry.ViewFields = "<FieldRef Name='CustAction' /><FieldRef Name='ID' /><FieldRef Name='" + dtType + "Date' /><FieldRef Name='" + dtType + "During' />";
            qry.Query = @"<Where><And><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Integer'>" + userID +
                "</Value></Eq><Or><Eq><FieldRef Name='" + dtType + "Date' /><Value Type='DateTime' IncludeTimeValue='TRUE'>" + txtDate + "</Value></Eq><Eq><FieldRef Name='PlanDate' /><Value Type='DateTime' IncludeTimeValue='TRUE'>" + txtDate + "</Value></Eq></Or></And></Where>";
            SPListItemCollection items = lstAction.GetItems(qry);
            if (items.Count >0)
            {
                lblMsg.Text ="此时间段时已经存在其他操作，指重新指定活动时间！";
                return null;
            }
            //输入的时间点不存在数据
            txtDate = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtStart.SelectedDate.AddDays(-objWeb.Days).AddHours(-objWeb.Hours));
           qry = new SPQuery();
            qry.ViewFields = "<FieldRef Name='CustAction' /><FieldRef Name='ID' /><FieldRef Name='" + dtType + "Date' /><FieldRef Name='" + dtType + "During' />";
            qry.Query = @"<Where><And><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Integer'>" + userID +
                "</Value></Eq><Geq><FieldRef Name='" + dtType + "Date' /><Value Type='DateTime' IncludeTimeValue='TRUE'>" + txtDate + "</Value></Geq></And></Where>";
            items = lstAction.GetItems(qry);
            if (items.Count ==0)
            {
                qry = new SPQuery();
                qry.ViewFields = "<FieldRef Name='CustAction' /><FieldRef Name='ID' /><FieldRef Name='" + dtType + "Date' /><FieldRef Name='" + dtType + "During' />";
                qry.Query = @"<Where><Geq><FieldRef Name='" + dtType + "Date' /><Value Type='" + dtType + "Date' IncludeTimeValue='TRUE'>" + txtDate + "</Value></Geq></Where>";
                items = lstAction.GetItems(qry);
            }
             
            DataTable myItems = items.GetDataTable();
            if (myItems != null)
            {
                DataRow[] drs;
                DataSet ds = new DataSet();
                DataTable dtResult = myItems.Clone();
                ds.Tables.Add(dtResult);
                for (int d=7; d>0;d--)
                {
                    string strFilter =  dtType + "Date>='{0}' and "+dtType + "Date<='{1}'";
                    strFilter = string.Format(strFilter, dtStart.SelectedDate.AddDays(-d).AddHours(-objWeb.Hours ).ToString (), dtStart.SelectedDate.AddDays(-d).AddHours( objWeb.Hours ).ToString ());

                    drs = myItems.Select(strFilter);
                    if (drs.Length > 0)
                        ds.Merge(drs);
                }
                myItems = ds.Tables[0] .Copy();
                ViewState["result"] = myItems.Copy ();

                DataView dataView = myItems.DefaultView;
                DataTable dataTableDistinct = dataView.ToTable(true, "CustAction","ID");
                myItems = dataTableDistinct.Copy();

           }
            return myItems;
        }
        private void AddAssistant(int id, DateTime dt, int during, int actionID)
        {
            SPWeb web = SPContext.Current.Web;
            int userID = web.CurrentUser.ID;
            SPList lstAction = web.Lists.TryGetList(objWeb.ListName);
            SPListItem item;
            string custID;
            if (id > 0)
            {
                item = lstAction.GetItemById(id);
                custID = item[lstAction.Fields.GetFieldByInternalName("CustAction").Title].ToString();
                if (actionID != int.Parse(Regex.Split(custID, ";#")[0]))
                    item = lstAction.AddItem();
            }
            else
                item = lstAction.AddItem();
            item[lstAction.Fields.GetFieldByInternalName("CustAction").Title] = actionID ;

            item[lstAction.Fields.GetFieldByInternalName("ActualDate").Title] = dt;
            item[lstAction.Fields.GetFieldByInternalName("ActualDuring").Title] = during;
            item.Update();
        }
        /// <summary>
        /// 计划时间和实际时间是否重复
        /// </summary>
        /// <param name="dtCurrent"></param>
        /// <param name="dtType">Plan/Actual</param>
        /// <returns></returns>
        private bool AssistantExits(DateTime dtCurrent,double during ,ref int id,string dtType="Actual")
        {
            DateTime dt = dtCurrent;
            SPWeb web = SPContext.Current.Web;
            int userID = web.CurrentUser.ID;
            SPList lstAction = web.Lists.TryGetList(objWeb.ListName);
            string txtDate = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dt.AddDays(-1));

            SPQuery qry = new SPQuery();
            qry.ViewFields = "<FieldRef Name='ID' /><FieldRef Name='"+dtType+"Date' /><FieldRef Name='"+dtType+"During' />";
            qry.Query = @"<Where><And><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Integer'>" + userID +
                "</Value></Eq><Geq><FieldRef Name='"+dtType+"Date' /><Value Type='DateTime' IncludeTimeValue='TRUE'>" + txtDate + "</Value></Geq></And></Where>";

            DataTable myItems = lstAction.GetItems(qry).GetDataTable();
            if (myItems == null) return false;
            DateTime dtStart;
            DateTime dtEnd;
            DateTime dt1 = dt.AddMinutes(during);
            foreach (DataRow dr in myItems.Rows)
            {
                //if (properties.ListItem != null && (int)dr["ID"] == properties.ListItemId)
                //    continue;
                 id = (int)dr["ID"];
                dtStart = (DateTime)dr[dtType+"Date"];
                dtEnd = dtStart.AddMinutes(dr[dtType+"During"] == null || dr[dtType+"During"].ToString() == "" ? 0 : (double)dr[dtType+"During"]);
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
                        if (dt1 >= dtStart && dt1 < dtEnd)
                            return true;
                    }
                }
               
            }
            return false;
        }
        #endregion
    }
}
