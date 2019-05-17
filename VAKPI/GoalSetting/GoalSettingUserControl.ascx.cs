using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace VAKPI.GoalSetting
{
    /// <summary>
    /// 个性化规则，约束条件
    /// </summary>
    public partial class GoalSettingUserControl : UserControl
    {
        public GoalSetting webObj;
        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            gvGoalSetting.RowDataBound += GvGoalSetting_RowDataBound; 
            //gvGoalSetting.RowCancelingEdit += GvGoalSetting_RowCancelingEdit;
            //gvGoalSetting.RowUpdating += GvGoalSetting_RowUpdating;
            //gvGoalSetting.RowCreated += GvGoalSetting_RowCreated;
            if (!Page.IsPostBack)
            {
                bool isCheck = CheckPars();
                if (isCheck)
                {
                    lbErr.Text = "";
                    lbDes.Text = webObj.ChangeDes;
                    bind();
                }
            }
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Source"] != null)
                Response.Redirect(Request.QueryString["Source"].ToString());
            //else if (ViewState["listUrl"] != null)
            //    Response.Redirect(ViewState["listUrl"].ToString());
        }

        private void GvGoalSetting_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             if (e.Row.RowType ==DataControlRowType.Header && webObj.ConstraintList=="用户-指标"  )
            {
                e.Row.Cells[0].Text = "指标名称";
            }
        }

        //把asp:BoundField的Visible设置为false，肯定取不到值。
        //两种方法：1。设置width＝0	2。创建Row后立即隐藏。
        private void GvGoalSetting_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[0].Visible = false;
                //e.Row.Cells[1].Visible = false;
                
            }
        }

        //保存当前用户的指标值
        private void BtnSave_Click(object sender, EventArgs e)
        {
            string myDuring;//当前修改的值
            string myDuring0;//修改前的值
            string activType;
            string id;
            List<string> lstGoals = new List<string>();
            for (int i = 0; i < gvGoalSetting.Rows.Count; i++)
            {
                activType = this.gvGoalSetting.Rows[i].Cells[0].Text.Trim();
                id = gvGoalSetting.DataKeys[i].Value.ToString();
                myDuring = ((TextBox)(this.gvGoalSetting.Rows[i].Cells[3].FindControl("txtMyDuring"))).Text.Trim();//获取 模板列的值
                myDuring0 = ((Label)(this.gvGoalSetting.Rows[i].Cells[4].FindControl("lblMyDuring"))).Text.Trim();//

                if (string.Compare(myDuring, myDuring0) != 0)//值不同可以保存
                {
                    lstGoals.Add(activType + ";" + myDuring + ";" + id);
                }
            }
            if (lstGoals.Count ==0)
            {
                lbErr.Text = "没有要保存的变更值！";
                return;
            }
            bool results;
            results = SaveChangeValue(lstGoals);

            if (results)
            {
                BtnCancel_Click(null,null);
                lbErr.Text = "保存成功！";
            }
            else
                lbErr.Text = "保存失败！ ";

        }

        private void GvGoalSetting_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            gvGoalSetting.EditIndex = -1;
            bind();
        }

        private void GvGoalSetting_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvGoalSetting.EditIndex = -1;
            bind();
        }

        private void GvGoalSetting_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvGoalSetting.EditIndex = e.NewEditIndex;
            bind();
        }
        #endregion
        #region 方法
        private void BindGridColumn(string colHeader,string colField)
        {
            BoundField column = new BoundField();
            column.HeaderText = "第二列";
            column.DataField = "date1";
            gvGoalSetting.Columns.Insert(1,column);
            

        }
        /// <summary>
        /// 保存编辑的约束值,活动类型和值
        /// </summary>
        /// <returns></returns>
        private bool  SaveChangeValue(List<string> lstGoals)
        {
            bool result = true;
            string[] txtGoals;
            DataTable dt = (DataTable)ViewState["dtResult"];
            DataRow[] drs;
            DateTime dtNow=DateTime.Now ;
            List<string> gValues = new List<string>();
            foreach (string txtGoal in lstGoals)
            {
                txtGoals = txtGoal.Split(';');
                drs = dt.Select("Flag=11 and ActivityType='" + txtGoals[0] + "'");//获取活动类型ID
                if (drs.Length > 0)
                {
                    //if (((DateTime)drs[0]["StartDate"]).AddMonths(webObj.monthsInternal ).CompareTo(dtNow) >= 0)//起用日期
                    //{
                    //    result = false;
                    //}
                    //else//更新并添加
                    //{
                    drs[0]["EndDate"] = dtNow;
                    drs[0]["Flag"] = "10";
                    gValues.Add(drs[0]["ID"] + ";" + txtGoal);
                    //}
                }
                else
                    gValues.Add("0;" + txtGoal);

            }
            result= SaveMyContraints(gValues);
            return result ;
        }
        private bool CheckPars()
        {
            bool isCheck = true;
            SPUser user = SPContext.Current.Web.CurrentUser;
            if (user == null)
            {
                lbErr.Text = "请先登录！";
                btnSave.Visible = false;
                return false;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate ()
          {
              try
              {
                  //string siteUrl = webObj.SiteUrl;
                  using (SPSite spSite = new SPSite(webObj.SiteUrl)) //找到网站集
                  {
                      using (SPWeb spWeb = spSite.OpenWeb())
                      {
                          if (!spWeb.Exists)
                          {
                              isCheck = false;
                              lbErr.Text = webObj.Site + " 子网站不存在！";
                          }
                          else
                          {
                              SPList spList = spWeb.Lists.TryGetList(webObj.ConstraintList);
                              if (spList == null)
                              {
                                  isCheck = false;
                                  lbErr.Text = webObj.ConstraintList + " 列表不存在！";
                              }
                          }
                      }
                  }
              }
              catch
              {

              }
          });
            return isCheck;
        }
        public bool SaveMyContraints(List<string> gValues)
        {
            bool isTrue = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
           {
               try
               {
                   string siteUrl = webObj.SiteUrl;
                   string listName = webObj.ConstraintList;
                   int userID = SPContext.Current.Web.CurrentUser.ID;
                   SPQuery qry;
                   using (SPSite spSite = new SPSite(siteUrl)) //找到网站集
                   {
                       using (SPWeb spWeb = spSite.OpenWeb())
                       {
                           SPList spList = spWeb.Lists.TryGetList(listName);
                           SPListItem item;
                           SPListItem newItem;
                           if (spList != null)
                           {
                               DateTime dtDate = DateTime.Now;
                               int id;
                               spWeb.AllowUnsafeUpdates = true;
                               foreach (string gvalue in gValues)
                               {
                                   string[] sValues = gvalue.Split(';');
                                   id = int.Parse(sValues[0]);//当前值
                                   if (id > 0)
                                   {
                                       item = spList.Items.GetItemById(id);

                                       item["标记"] = "10";
                                       item["终止日期"] = dtDate;
                                       item["修改者"] = userID;
                                       item.Update();
                                   }
                                   else
                                   {
                                       id = int.Parse(sValues[3]);//通过系统默认的值得到活动类型
                                       item = spList.Items.GetItemById(id);
                                   }
                                   bool isAdd = false;
                                   string lookupFld;
                                   int lookupValue;
                                   if (webObj.ConstraintList == "用户-指标")
                                   {
                                       lookupFld = "IndexName";
                                       lookupValue = new SPFieldLookupValue(item["指标名称"].ToString()).LookupId;
                                   }
                                   else
                                   {
                                       lookupFld = "ActivityType";
                                       lookupValue = new SPFieldLookupValue(item["活动类型"].ToString()).LookupId;

                                   }
                                   isAdd = IsItemAdd(spList, dtDate, userID, lookupFld, lookupValue);
                                   if (!isAdd)
                                   {
                                       newItem = spList.Items.Add();
                                       newItem["标记"] = "11";
                                       newItem["StartDate"] = dtDate;
                                       newItem["创建者"] = userID;
                                       newItem["周期"] = item["周期"];
                                       if (webObj.ConstraintList == "用户-指标")
                                       {
                                           newItem["指标名称"] = item["指标名称"];
                                           newItem["目标值"] = sValues[2];
                                       }
                                       else
                                       {
                                           newItem["活动类型"] = item["活动类型"];
                                           newItem["约定时长"] = sValues[2];
                                       }
                                       newItem.Update();
                                   }
                               }
                               spWeb.AllowUnsafeUpdates = false ;
                               isTrue = true;
                           }
                       }
                   }
               }
               catch (Exception ex)
               {
                   lbErr.Text = ex.ToString();
                   isTrue = false;
               }
           });
            return isTrue;
        }
        private bool IsItemAdd(SPList spList,DateTime dtDate,int userID,string lookupField,int lookupValue)
        {
            SPQuery qry = new SPQuery();
            qry.Query = @"<Where>
                                  <And><And>
                                     <Eq>
                                        <FieldRef Name='"+lookupField +@"' LookupId='True' />
                                        <Value Type='Integer'>" + lookupValue + @"</Value>
                                     </Eq>
                                     <Eq>
                                        <FieldRef Name='StartDate' />
                                        <Value Type='DateTime'>" + dtDate.ToString("yyyy-MM-dd") + @"</Value>
                                     </Eq>
                                  </And><Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>" + userID + @"</Value>
                                     </Eq></And>
                               </Where>";
            SPListItemCollection items = spList.GetItems(qry);
            if (items.Count > 0)
                return true;
            else
                return false;
        }
        private void bind()
        {
            DataTable dt = GetResults();
            ViewState["dtResult"] = dt; 
            gvGoalSetting.DataSource = dt;
            gvGoalSetting.DataBind();
        }
        //系统的和个人的目标,每项指标值一个人可以修改一次,系统的和个人的
        private DataTable GetResults()
        {
            SPUser spuser = SPContext.Current.Web.CurrentUser;
            int userID = spuser.ID;
            DataTable dt = common.GetConstraintList(webObj.SiteUrl, webObj.ConstraintList, userID);
            if (dt == null)
                return null;
            if (webObj.ConstraintList == "用户-指标")
            {
                dt.Columns.Add("ActivityType", typeof(string));
                dt.Columns.Add("During", typeof(float));
                foreach (DataRow dr in dt.Rows)
                {
                    dr["ActivityType"] = dr["IndexName"];
                    dr["During"] = dr["TargetValue"];
                }

            }
            dt.Columns.Add(new DataColumn("MyDuring", typeof(float)));

            DataRow[] drs = dt.Select("Flag=0", "Created Desc");//系统
            List<int> ids = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                drs = dt.Select("Flag=11 and ActivityType='" + dr["ActivityType"] + "'");
                if (drs.Length > 0)
                {
                    if (drs[0]["ID"].ToString() != dr["ID"].ToString())//同一个活动类型系统和用户的
                        dr["MyDuring"] = drs[0]["During"];
                    else
                    {
                        drs = dt.Select("Flag=0 and ActivityType='" + dr["ActivityType"] + "'");
                        if (drs.Length > 0)
                        {
                            dr["MyDuring"] = dr["During"];
                            dr["During"] = drs[0]["During"];
                            ids.Add((int)drs[0]["ID"]);
                        }
                        else
                            dr["During"] = "";
                    }
                }
                else
                    dr["MyDuring"] = dr["During"];
            }
            foreach (int id in ids)//移除系统的
            {
                drs = dt.Select("ID=" + id);
                dt.Rows.Remove(drs[0]);
            }
            return dt;

        }
        #endregion
    }
}
