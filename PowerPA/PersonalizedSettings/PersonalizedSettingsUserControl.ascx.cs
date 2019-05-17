using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PowerPA.PersonalizedSettings
{
    public partial class PersonalizedSettingsUserControl : UserControl
    {
        public PersonalizedSettings webObj;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    int userId=GetUserId();
                    if (userId!=0)
                    {
                        actionSetting.Visible = true;
                        ShowUserDesc();
                        lbErr.Text = "";
                        DataTable dt = GetBindData(userId);
                        BindGV(gvActions, dt);

                    }
                    else
                    {
                        lbErr.Text = "您尚未登录，无法执行个性化操作设置！";
                        actionSetting.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {

                lbErr.Text=ex.ToString();
            }

        }

        #region 方法

        private void ShowUserDesc()
        {
            string userDesc = webObj.UserDesc;
            if (!string.IsNullOrWhiteSpace(userDesc))
            {
                if (userDesc.Contains(";"))
                {
                    string[] descs = userDesc.Split(';');
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<dl>");
                    for (int i = 0; i < descs.Length; i++)
                    {
                        builder.Append("<dt>" + descs[i] + "</dt>");
                    }
                    builder.Append("</dl>");
                    divdesc.InnerHtml = builder.ToString();
                }
                else
                {
                    divdesc.InnerHtml = userDesc;
                }
            }
        }


        private void BindGV(GridView gv,DataTable dt)
        {
            gv.DataSource= dt;
            gv.DataBind();


        }

        private DataTable InitSouceTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");//0
            dt.Columns.Add("ActionID");//1
            dt.Columns.Add("Frequency");//2
            dt.Columns.Add("MinDuring");//3
            dt.Columns.Add("MaxDuring");//4
            dt.Columns.Add("NormalDuring");//5
            dt.Columns.Add("Desc");//6
            dt.Columns.Add("Title");//7
            dt.Columns.Add("Period");//8
            dt.Columns.Add("DorM");//9
            dt.Columns.Add("Healthy");//10
            dt.Columns.Add("Interaction");//11
            dt.Columns.Add("Url");//12
            //系统设置项
            dt.Columns.Add("SysFrequency");//13
            dt.Columns.Add("SysPeriod");//14
            dt.Columns.Add("SysDorM");//15
            dt.Columns.Add("SysHealthy");//16
            dt.Columns.Add("SysInteraction");//17
            dt.Columns.Add("SysDesc");//18
            dt.Columns.Add("SysJiLiang");//19
            dt.Columns.Add("SysMinDuring");//20
            dt.Columns.Add("SysMaxDuring");//21
            dt.Columns.Add("SysNormalDuring");//22
            return dt;
        }

        /// <summary>
        /// 根据当前用户ID，获取操作设置数据
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <returns></returns>
        private DataTable GetBindData(int userId)
        {
            DataTable dt = InitSouceTable();
            string url=Request.Path;
            try
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
                {
                    using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        //我的操作
                        SPList myactionlist = spWeb.Lists.TryGetList(webObj.MyActions);

                        if (myactionlist != null)
                        {

                                SPQuery myqry = new SPQuery
                                {
                                    Query = @"<Where>
                              <Eq>
                                 <FieldRef Name='Author' LookupId='True' />
                                 <Value Type='Integer'>" + userId + @"</Value>
                              </Eq>
                             </Where>
                             <OrderBy>
                                 <FieldRef Name='Frequency' Ascending='FALSE' />
                                 <FieldRef Name='Frequency' Ascending='FALSE' />
                             </OrderBy>"
                            };
                            SPListItemCollection myActions = myactionlist.GetItems(myqry);

                            //所有操作
                            SPList actionList = spWeb.Lists.TryGetList(webObj.ActionList);
                            SPQuery allQuery = new SPQuery
                            {
                                Query = @" <OrderBy>
                                  <FieldRef Name='Frequency' Ascending='FALSE' />
                                  <FieldRef Name='Frequency' Ascending='FALSE' />
                               </OrderBy>"
                            };
                            SPListItemCollection actions = actionList.GetItems(allQuery);
                            if (myActions.Count > 0)//已有我的操作
                            {
                                url = myactionlist.DefaultDisplayFormUrl+"?Source="+url+"&ID=";
                                if (myActions.Count==actions.Count)//所有系统操作均已被我个性化设置
                                {
                                    #region 所有系统操作均已被我个性化设置，则只绑定我的操作
                                    for (int i = 0; i < myActions.Count; i++)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr[0] = myActions[i]["ID"];
                                        dr[1] = myActions[i]["ActionID"];
                                        dr[2] = myActions[i]["Frequency"];
                                        dr[3] = myActions[i]["MinDuring"];
                                        dr[4] = myActions[i]["MaxDuring"];
                                        dr[5] = myActions[i]["NormalDuring"];
                                        dr[6] = myActions[i]["Desc"];
                                        int actionId=int.Parse(myActions[i]["ActionID"].ToString().Split(';')[0]);
                                        SPListItem thisaction = actions.GetItemById(actionId);
                                        dr[7] =thisaction["Title"];// myActions[i]["Title"];
                                        dr[8] = myActions[i]["Period"];
                                        dr[9]=myActions[i]["DorM"];
                                        dr[10]=myActions[i]["Healthy"];
                                        dr[11]=myActions[i]["Interaction"];
                                        dr[12]=url+myActions[i]["ID"];

                                        //系统设置
                                        dr[13] = actionList.Fields.GetField("Frequency").Description + GetToolTip(webObj.Frequencies, thisaction["Frequency"]);
                                        dr[14] = actionList.Fields.GetField("Period").Description + GetToolTip(webObj.Periods, thisaction["Period"]);
                                        dr[15] = actionList.Fields.GetField("DorM").Description + GetToolTip(webObj.DoMs, thisaction["DorM"]);
                                        dr[16] = actionList.Fields.GetField("Healthy").Description + GetToolTip("", thisaction["Healthy"]);
                                        dr[17] = actionList.Fields.GetField("Interaction").Description + GetToolTip("", thisaction["Interaction"]);
                                        dr[18] = actionList.Fields.GetField("Desc").Description + GetToolTip("", thisaction["Desc"]);
                                        string jiliang = Environment.NewLine;
                                        if (thisaction["Measurement"] != null)
                                        {
                                            jiliang += actionList.Fields.GetField("Measurement").Title + "：" + thisaction["Measurement"];
                                        }
                                        if (thisaction["MUnits"] != null)
                                        {
                                            if (jiliang!=Environment.NewLine)
                                            {
                                                jiliang += Environment.NewLine;
                                            }
                                            jiliang += actionList.Fields.GetField("MUnits").Title + "：" + thisaction["MUnits"];
                                        }
                                        //dr[19] = "时长或数量"+Environment.NewLine+ jiliang;
                                        dr[20] = myactionlist.Fields.GetField("MinDuring").Description + jiliang;
                                        dr[21] = myactionlist.Fields.GetField("MaxDuring").Description + jiliang;
                                        dr[22] = myactionlist.Fields.GetField("NormalDuring").Description + jiliang;
                                        dt.Rows.Add(dr);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 部分操作已经个性化，则首先绑定已个性化的部分
                                    for (int i = 0; i < myActions.Count; i++)//先绑定我已设置的操作
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr[0] = myActions[i]["ID"];
                                        dr[1] = myActions[i]["ActionID"];
                                        dr[2] = myActions[i]["Frequency"];
                                        dr[3] = myActions[i]["MinDuring"];
                                        dr[4] = myActions[i]["MaxDuring"];
                                        dr[5] = myActions[i]["NormalDuring"];
                                        dr[6] = myActions[i]["Desc"];
                                        int actionId = int.Parse(myActions[i]["ActionID"].ToString().Split(';')[0]);
                                        SPListItem thisaction = actions.GetItemById(actionId);
                                        dr[7] = thisaction["Title"];
                                        dr[8] = myActions[i]["Period"];
                                        dr[9]=myActions[i]["DorM"];
                                        dr[10] = myActions[i]["Healthy"];
                                        dr[11] = myActions[i]["Interaction"];
                                        dr[12]=url+myActions[i]["ID"];

                                        //系统设置
                                        dr[13] = actionList.Fields.GetField("Frequency").Description + GetToolTip(webObj.Frequencies, thisaction["Frequency"]);
                                        dr[14] = actionList.Fields.GetField("Period").Description + GetToolTip(webObj.Periods, thisaction["Period"]);
                                        dr[15] = actionList.Fields.GetField("DorM").Description + GetToolTip(webObj.DoMs, thisaction["DorM"]);
                                        dr[16] = actionList.Fields.GetField("Healthy").Description + GetToolTip("", thisaction["Healthy"]);
                                        dr[17] = actionList.Fields.GetField("Interaction").Description + GetToolTip("", thisaction["Interaction"]);
                                        dr[18] = actionList.Fields.GetField("Desc").Description+ GetToolTip("", thisaction["Desc"]);
                                        string jiliang = Environment.NewLine;
                                        if (thisaction["Measurement"] != null)
                                        {
                                            jiliang += actionList.Fields.GetField("Measurement").Title + "：" + thisaction["Measurement"];
                                        }
                                        if (thisaction["MUnits"] != null)
                                        {
                                            if (jiliang != Environment.NewLine)
                                            {
                                                jiliang += Environment.NewLine;
                                            }
                                            jiliang += actionList.Fields.GetField("MUnits").Title + "：" + thisaction["MUnits"];
                                        }
                                        //dr[19] = "时长或数量"+Environment.NewLine+ jiliang;
                                        dr[20] = myactionlist.Fields.GetField("MinDuring").Description+jiliang;
                                        dr[21] = myactionlist.Fields.GetField("MaxDuring").Description+jiliang;
                                        dr[22] = myactionlist.Fields.GetField("NormalDuring").Description+jiliang;
                                        dt.Rows.Add(dr);
                                    }
                                    #endregion

                                    #region 部分未经过个性化设置，继续绑定该部分系统操作
                                    for (int i = 0; i < actions.Count; i++)
                                    {
                                        string actionId=actions[i]["ID"].ToString();
                                        //DataTable dtMy = myActions.GetDataTable();
                                        SPListItem item = GetItemByField(myActions, "ActionID", actionId);
                                        //DataRow[] drs = dtMy.Select("ActionID="+actionId);
                                        if (item==null)//找到那些尚未设置的操作
                                        {
                                            DataRow dr = dt.NewRow();
                                            SPListItem thisaction = actions[i];
                                            dr[0] = 0;
                                            dr[1] = thisaction["ID"];
                                            dr[2] =thisaction["Frequency"];
                                            dr[3] = null;
                                            dr[4] = null;
                                            dr[5] = null;
                                            dr[6] =thisaction["Desc"];
                                            dr[7] =thisaction["Title"];
                                            dr[8] = thisaction["Period"];
                                            dr[9]=thisaction["DorM"];;
                                            dr[10] =thisaction["Healthy"];
                                            dr[11] = thisaction["Interaction"];
                                            dr[12]=null;

                                            //系统设置
                                            dr[13] = actionList.Fields.GetField("Frequency").Description + GetToolTip(webObj.Frequencies, thisaction["Frequency"]);
                                            dr[14] = actionList.Fields.GetField("Period").Description + GetToolTip(webObj.Periods, thisaction["Period"]);
                                            dr[15] = actionList.Fields.GetField("DorM").Description + GetToolTip(webObj.DoMs, thisaction["DorM"]);
                                            dr[16] = actionList.Fields.GetField("Healthy").Description + GetToolTip("", thisaction["Healthy"]);
                                            dr[17] = actionList.Fields.GetField("Interaction").Description + GetToolTip("", thisaction["Interaction"]);
                                            dr[18] = actionList.Fields.GetField("Desc").Description + GetToolTip("", thisaction["Desc"]);
                                            string jiliang = Environment.NewLine;
                                            if (thisaction["Measurement"] != null)
                                            {
                                                jiliang += actionList.Fields.GetField("Measurement").Title + "：" + thisaction["Measurement"];
                                            }
                                            if (thisaction["MUnits"] != null)
                                            {
                                                if (jiliang != Environment.NewLine)
                                                {
                                                    jiliang += Environment.NewLine;
                                                }
                                                jiliang += actionList.Fields.GetField("MUnits").Title + "：" + thisaction["MUnits"];
                                            }
                                            //dr[19] = "时长或数量"+Environment.NewLine+ jiliang;
                                            dr[20] = myactionlist.Fields.GetField("MinDuring").Description + jiliang;
                                            dr[21] = myactionlist.Fields.GetField("MaxDuring").Description + jiliang;
                                            dr[22] = myactionlist.Fields.GetField("NormalDuring").Description + jiliang;
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                    #endregion
                                }
                                //lbckCount.Text =myActions.Count.ToString();
                            }
                            else//尚无我的操作,则绑定系统操作
                            {
                                for (int i = 0; i < actions.Count; i++)
                                {
                                    DataRow dr = dt.NewRow();
                                    SPListItem thisaction = actions[i];
                                    dr[0] = 0;
                                    dr[1] = actions[i]["ID"];
                                    dr[2] = actions[i]["Frequency"];
                                    dr[3] = null;
                                    dr[4] = null;
                                    dr[5] = null;
                                    dr[6] = actions[i]["Desc"];
                                    dr[7] = actions[i]["Title"];
                                    dr[8]=actions[i]["Period"];
                                    dr[9] = actions[i]["DorM"]; ;
                                    dr[10] = actions[i]["Healthy"];
                                    dr[11] = actions[i]["Interaction"];
                                    dr[12]=null;

                                    //系统设置
                                    dr[13] = actionList.Fields.GetField("Frequency").Description + GetToolTip(webObj.Frequencies, thisaction["Frequency"]);
                                    dr[14] =actionList.Fields.GetField("Period").Description + GetToolTip(webObj.Periods, thisaction["Period"]);
                                    dr[15] =actionList.Fields.GetField("DorM").Description+ GetToolTip(webObj.DoMs, thisaction["DorM"]);
                                    dr[16] =actionList.Fields.GetField("Healthy").Description + GetToolTip("", thisaction["Healthy"]);
                                    dr[17] = actionList.Fields.GetField("Interaction").Description + GetToolTip("", thisaction["Interaction"]);
                                    dr[18] = actionList.Fields.GetField("Desc").Description + GetToolTip("", thisaction["Desc"]);
                                    string jiliang = Environment.NewLine;
                                    if (thisaction["Measurement"] != null)
                                    {
                                        jiliang += actionList.Fields.GetField("Measurement").Title + "：" + thisaction["Measurement"];
                                    }
                                    if (thisaction["MUnits"] != null)
                                    {
                                        if (jiliang != Environment.NewLine)
                                        {
                                            jiliang += Environment.NewLine;
                                        }
                                        jiliang += actionList.Fields.GetField("MUnits").Title + "：" + thisaction["MUnits"];
                                    }
                                    //dr[19] = "时长或数量"+Environment.NewLine+ jiliang;
                                    dr[20] = myactionlist.Fields.GetField("MinDuring").Description +jiliang;
                                    dr[21] = myactionlist.Fields.GetField("MaxDuring").Description + jiliang;
                                    dr[22] = myactionlist.Fields.GetField("NormalDuring").Description + jiliang;
                                    dt.Rows.Add(dr);

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
            return dt;
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

        private SPListItem GetItemByField(SPListItemCollection items,string field,string value)
        {
            SPListItem spitem = null;
            foreach (SPListItem item in items)
            {
                if (item[field].ToString().Split(';')[0]==value)
                {
                    spitem = item;
                    break;
                }
            }
            return spitem;
        }

        private string[] GetActionByID(long Id)
        {
            string[] action = new string[2] { "", "" };
            string query = @"<Where>
                              <Eq>
                                 <FieldRef Name='ID' />
                                 <Value Type='Counter'>"+Id+@"</Value>
                              </Eq>
                           </Where>";
            SPListItemCollection items = GetDataFromList(webObj.ActionList, query);
            if (items.Count > 0)
            {
                action[0] = items[0]["Title"]==null?"":items[0]["Title"].ToString();

                action[1] = items[0]["Desc"]==null?"":items[0]["Desc"].ToString();
            }
            return action;
        }


        private SPListItemCollection GetMyAction(string listName)
        {
            SPUser user = SPContext.Current.Web.CurrentUser;
            int uId = user.ID;
            string query = @"<Where>
                              <Eq>
                                 <FieldRef Name='Author' LookupId='True' />
                                 <Value Type='Integer'>"+uId+ @"</Value>
                              </Eq>
                             </Where>
                             <OrderBy>
                                 <FieldRef Name='Frequency' Ascending='FALSE' />
                                 <FieldRef Name='Frequency' Ascending='FALSE' />
                             </OrderBy>";
            SPListItemCollection items = GetDataFromList(listName, query);
            return items;
        }

        /// <summary>
        /// 从SharePoint列表获取数据
        /// </summary>
        /// <param name="listName">列表名称</param>
        /// <param name="query">Caml查询语句</param>
        /// <returns></returns>
        private SPListItemCollection GetDataFromList(string listName, string query)
        {
            SPListItemCollection items=null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
                    {
                        using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
                        {
                            SPList spList = spWeb.Lists.TryGetList(listName);

                            if (spList != null)
                            {
                                if (query != "")
                                {
                                    SPQuery qry = new SPQuery();
                                    qry.Query = query;
                                    items = spList.GetItems(qry);
                                }
                                else
                                {
                                    items = spList.GetItems();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    lbErr.Text=ex.ToString();
                }
            });
            return items;
        }


        private string GetToolTip(string valueStr,object fieldValue)
        {
            string value = "";
            if (fieldValue != null)
            {
                if (valueStr!="")
                {
                    int index = int.Parse(fieldValue.ToString());
                    string[] values = valueStr.Split('、');
                    index = values.Length - index;
                    value = Environment.NewLine +"推荐值：" + values.GetValue(index).ToString();
                }
                else
                {
                    value =Environment.NewLine + "推荐值：" +fieldValue.ToString();
                }
            }
            else
            {
                value =Environment.NewLine + "无推荐值";
            }

            return value;
        }

        /// <summary>
        /// 遍历DataTable数据写入SharePoint列表中
        /// </summary>
        /// <param name="listName">列表名称</param>
        /// <param name="dt">源数据</param>
        /// <returns></returns>
        public int WriteDataToList()
        {
            string listName = webObj.MyActions;
            DataTable dt = GetGVToTable(gvActions);
            int icount = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
                    {
                        using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
                        {
                            SPList spList = spWeb.Lists.TryGetList(listName);

                            if (spList != null)
                            {
                                spWeb.AllowUnsafeUpdates = true;
                                SPUser cUser = SPContext.Current.Web.CurrentUser;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow dr = dt.Rows[i];
                                    if (dr[0].ToString()=="0")//插入新行
                                    {
                                        SPListItem item = spList.Items.Add();
                                        item["ActionID"] = dr[1];
                                        item["Frequency"] = dr[2];
                                        item["MinDuring"] = dr[3];
                                        item["MaxDuring"] = dr[4];
                                        item["NormalDuring"] = dr[5];
                                        item["Desc"] = dr[6];
                                        item["Title"] = dr[7];
                                        item["Period"] = dr[8];
                                        item["DorM"] = dr[9];
                                        item["Healthy"] = dr[10];
                                        item["Interaction"] = dr[11];
                                        item["Author"] = cUser;
                                        item["Editor"] = cUser;
                                        item.Update();
                                    }
                                    else
                                    {
                                        SPListItem item = spList.GetItemById(int.Parse(dr[0].ToString()));
                                        item["ActionID"] = dr[1];
                                        item["Frequency"] = dr[2];
                                        item["MinDuring"] = dr[3];
                                        item["MaxDuring"] = dr[4];
                                        item["NormalDuring"] = dr[5];
                                        item["Desc"] = dr[6];
                                        item["Title"] = dr[7];
                                        item["Period"] = dr[8];
                                        item["DorM"] = dr[9];
                                        item["Healthy"] = dr[10];
                                        item["Interaction"] = dr[11];
                                        item["Author"] = cUser;
                                        item["Editor"] = cUser;
                                        item.Update();
                                    }
                                    icount++;
                                }
                                spWeb.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    lbErr.Text=ex.ToString();
                }
            });
            return icount;
        }

        /// <summary>
        /// 创建GridView列
        /// </summary>
        /// <param name="dataField">数据列</param>
        /// <param name="headerText">列头标题</param>
        /// <param name="footerText">列脚标题</param>
        /// <param name="width">宽度</param>
        /// <param name="headerStyle">列头样式</param>
        /// <param name="itemStyle">数据行样式</param>
        /// <param name="gv">GridView控件ID</param>
        private void CreateGridColumn(string dataField, string headerText, string footerText, int width, string headerStyle, string itemStyle, GridView gv)
        {
            BoundField bc = new BoundField
            {
                FooterText = footerText,
                DataField = dataField,
                HeaderText = headerText
            };
            if (!string.IsNullOrEmpty(headerStyle))
                bc.HeaderStyle.CssClass = headerStyle;  //若有默认样式，此行代码及对应的参数可以移除
            if (!string.IsNullOrEmpty(itemStyle))
                bc.ItemStyle.CssClass = itemStyle;   //若有默认样式，此行代码及对应的参数可以移除
            gv.Columns.Add(bc);  //把动态创建的列，添加到GridView中
            if (width > 0)
                gv.Width = new Unit(gv.Width.Value + width); //每添加一列后，要增加GridView的总体宽度
        }

        /// <summary>
        /// 读取GridView数据到数据表
        /// </summary>
        /// <param name="gv">GridView控件ID</param>
        /// <returns></returns>
        public DataTable GetGVToTable(GridView gv)
        {
            DataTable dt = InitSouceTable();

            for (int rCount = 0; rCount < gv.Rows.Count; rCount++)
            {
                //CheckBox cbSel = (CheckBox)gv.Rows[rCount].FindControl("cbSel");
                //if (cbSel.Checked)
                //{
                    DataRow dr = dt.NewRow();
                    Label lbID = (Label)gv.Rows[rCount].FindControl("lbID");
                    dr[0] = lbID.Text;
                    HiddenField hdfacId = (HiddenField)gv.Rows[rCount].FindControl("hdfacId");
                    dr[1] = hdfacId.Value;
                    DropDownList ddlpindu = (DropDownList)gv.Rows[rCount].FindControl("ddlpindu");
                    dr[2] = ddlpindu.SelectedItem.Value;
                    TextBox tbmin = (TextBox)gv.Rows[rCount].FindControl("tbmin");
                    dr[3] = tbmin.Text;
                    TextBox tbmax = (TextBox)gv.Rows[rCount].FindControl("tbmax");
                    dr[4] = tbmax.Text;
                    TextBox tbduring = (TextBox)gv.Rows[rCount].FindControl("tbduring");
                    dr[5] = tbduring.Text;
                    TextBox tbDesc = (TextBox)gv.Rows[rCount].FindControl("tbDesc");
                    dr[6] = tbDesc.Text;
                    HyperLink lnkaction = (HyperLink)gv.Rows[rCount].FindControl("lnkaction");
                    dr[7] = lnkaction.Text;
                    DropDownList ddlperiod = (DropDownList)gv.Rows[rCount].FindControl("ddlperiod");
                    dr[8] = ddlperiod.SelectedItem.Value;
                    DropDownList ddlDorM = (DropDownList)gv.Rows[rCount].FindControl("ddlDorM");
                    dr[9] = ddlDorM.SelectedItem.Value;
                    TextBox tbHealthy = (TextBox)gv.Rows[rCount].FindControl("tbHealthy");
                    dr[10] = tbHealthy.Text;
                    TextBox tbInteraction = (TextBox)gv.Rows[rCount].FindControl("tbInteraction");
                    dr[11] = tbInteraction.Text;
                    dt.Rows.Add(dr);
                //}
            }
            return dt;
        }

        private void AcceptOrReject(DataTable table)
        {
            // If there are errors, try to reconcile.
            if (table.HasErrors)
            {
                if (Reconcile(table))
                {
                    // Fixed all errors.
                    table.AcceptChanges();
                }
                else
                {
                    // Couldn't table fix all errors.
                    table.RejectChanges();
                }
            }
            else
                // If no errors, AcceptChanges.
                table.AcceptChanges();
        }

        private bool Reconcile(DataTable thisTable)
        {
            foreach (DataRow row in thisTable.Rows)
            {
                //Insert code to try to reconcile error.

                // If there are still errors return immediately
                // since the caller rejects all changes upon error.
                if (row.HasErrors)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取指定列名在GridView中的列序号
        /// </summary>
        /// <param name="gv">GridView控件ID</param>
        /// <param name="colName">列名</param>
        /// <returns></returns>
        private int GetGridViewColumnIndex(GridView gv, string colName)
        {
            int ndx = 0;
            foreach (DataControlField oCol in gv.Columns)
            {
                if (oCol.GetType() == typeof(BoundField))
                {
                    BoundField bfield = (BoundField)oCol;
                    if (bfield.DataField.ToUpper() == colName.ToUpper())
                    {
                        return ndx;
                    }
                }
                ndx++;
            }
            return ndx;
        }
        #endregion

        protected void cbSelAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbAll = (CheckBox)sender;
            if (cbAll.ToolTip == "全选")
            {
                foreach (GridViewRow gvr in gvActions.Rows)
                {
                    CheckBox cbSel = (CheckBox)gvr.Cells[7].FindControl("cbSel");
                    cbSel.Checked = cbAll.Checked;
                }
                if (cbAll.Checked)
                {
                    //lbckCount.Text = gvActions.Rows.Count.ToString();
                }
                else
                {
                    //lbckCount.Text = "0";
                }
            }
        }



        protected void gvActions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((Label)e.Row.FindControl("lbNo")!=null)
            {
                Label lbNo = (Label)e.Row.FindControl("lbNo");
                lbNo.Text = (e.Row.RowIndex+1).ToString();
            }

            //行绑定频度下拉框 ddlpindu
            if (((DropDownList)e.Row.FindControl("ddlpindu")) != null)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlpindu");
                string initValue = ((HiddenField)e.Row.FindControl("hdfpindu")).Value;

                DDLBind(ddl,webObj.Frequencies,initValue);
            }
            //行绑定使用时间段下拉框 ddlperiod
            if (((DropDownList)e.Row.FindControl("ddlperiod")) != null)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlperiod");
                string initValue = ((HiddenField)e.Row.FindControl("hdfperiod")).Value;
                DDLBind(ddl, webObj.Periods, initValue);
            }

            if ((HiddenField)e.Row.FindControl("hdfUrl")!=null&&(HyperLink)e.Row.FindControl("lnkaction")!=null)
            {
                HiddenField hdfurl = (HiddenField)e.Row.FindControl("hdfUrl");
                HyperLink lnkAction = (HyperLink)e.Row.FindControl("lnkaction");
                if (string.IsNullOrEmpty(hdfurl.Value))
                {
                    lnkAction.Attributes.Add("Class", "nonegvlink");
                }
                else
                {
                    lnkAction.Attributes.Add("Class", "gvlink");
                }
            }

            //行绑定时长数量设置选择下拉框 ddlDorM
            if (((DropDownList)e.Row.FindControl("ddlDorM")) != null)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlDorM");
                string initValue = ((HiddenField)e.Row.FindControl("hdfDorM")).Value;
                string[] values = new string[3] { "不限", "时长", "数量" };
                DDLBind(ddl, webObj.DoMs, initValue);
            }
            if (((Label)e.Row.FindControl("lbID"))!=null&&((HyperLink)e.Row.FindControl("lnkaction"))!=null)
            {
                HyperLink lnkAction = (HyperLink)e.Row.FindControl("lnkaction");
                Label lbId = (Label)e.Row.FindControl("lbID");
                string Id=lbId.Text;
                if (Id!="0")//新的操作
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


        private void DDLBind(DropDownList ddl, string valueStr, string initValue)
        {
            //先清空DropDownList
            ddl.Items.Clear();
            //再绑定DropDownList
            string[] values =valueStr.Split('、');
            for (int i =0;i< values.Length;  i++)
            {
                ddl.Items.Add(new ListItem(values[i], (values.Length-i).ToString()));
                ddl.Items[i].Attributes.Add("style", GetStyle((values.Length-i).ToString()));
            }
            //设置DropDownList初始项
            ddl.SelectedValue = initValue;
            ddl.Attributes.Add("style", GetStyle(initValue));
            //设置DropDownList事件属性
            //ddl.AutoPostBack = true;
            //ddl.SelectedIndexChanged += ddl_SelectedIndexChanged;
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList Drp = (DropDownList)sender;
            //string region = Drp.SelectedItem.Value;
            DropDownList ddl = sender as DropDownList;
            //TableCell cell = (TableCell)ddl.Parent;
            //GridViewRow item = (GridViewRow)cell.Parent;
            //GridViewRow gvr = (GridViewRow)ddl.NamingContainer;
            //int num = int.Parse(ddl.Text);
            string initValue = ddl.SelectedValue;
            ddl.Attributes.Add("style", GetStyle(initValue));
        }


        private void SetStyle()
        {
            int temp=0;
            for (int i = 0; i < gvActions.Columns.Count; i++)
            {
                temp+=(int)gvActions.Columns[i].HeaderStyle.Width.Value;
            }
            divBtn.Style.Value = "width:" + (temp - 60).ToString() + "px;text-align:right;margin-bottom:10px;";

            foreach (TableRow tr in gvActions.Rows)
            {
                temp+= (int)tr.Height.Value;
            }
            if (temp < 400)
            {
                //divGv.Style.Value = "height:" + (temp + 30).ToString() + "px;";
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int icount=WriteDataToList();
            if (icount>0)
            {
                if (Request.QueryString["Source"] != null)
                {
                    Response.Redirect(Request.QueryString["Source"].ToString());
                }
                else
                {
                    lbErr.Text = "保存成功";
                    int userId = GetUserId();
                    DataTable dtSource = GetBindData(userId);
                    BindGV(gvActions, dtSource);
                }
            }
            else
            {
                lbErr.Text = "未更改任何操作的设置！";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string backUrl = SPContext.Current.Web.Url;
            if (Request.QueryString["Source"] != null)
            {
                backUrl = Request.QueryString["Source"].ToString();
            }
            Response.Redirect(backUrl);
        }

        private int GetUserId()
        {
            int userId = 0;
            SPUser cUser = SPContext.Current.Web.CurrentUser;
            if (cUser!=null)
            {
                userId = cUser.ID;
            }
            return userId;
        }
    }
}
