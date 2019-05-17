using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace PowerPA.ActivityStatistics
{
    public partial class ActivityStatisticsUserControl : UserControl
    {
        public ActivityStatistics webObj { get; set; }
        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SPUser user = SPContext.Current.Web.CurrentUser;
                if (user ==null)
                {
                    lblMsg.Text = "请先登录！";
                    tbDate.Visible = false;
                    return;
                }
                InitControl();
                ReadLists(int.Parse (ddlActions.SelectedItem.Value) );
            }

        }
        protected void ddlActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadLists( int.Parse(ddlActions.SelectedItem.Value));
        }

        protected void dtStart_DateChanged(object sender, EventArgs e)
        {
            ReadLists( int.Parse(ddlActions.SelectedItem.Value));
            webObj.DateFrom = dtStart.SelectedDate;
            webObj.DateTo = dtEnd.SelectedDate;

        }
        #endregion
        #region 方法
        private void InitControl()
        {
            dtStart.SelectedDate = DateTime.Now.Date.AddDays(-webObj.StatisDays);
            dtEnd.SelectedDate = DateTime.Now.Date;
            webObj.DateFrom = dtStart.SelectedDate;
            webObj.DateTo = dtEnd.SelectedDate;
            DataTable dtSource = GetBindData(webObj.MyActionList, "");
            DataTable dtTmp = dtSource.Clone();
            DataRow dr = dtTmp.Rows.Add();
            dr["ID"] = 0;
            dr["Title"] = "全部";
            dtTmp.Merge(dtSource);

            BindDropDownList(ddlActions, "ID", "Title", dtTmp.Copy());
        }
        //返回操作列表
        private DataTable GetBindData(string listName, string parentId)
        {
            DataTable dt = null;
            if (ViewState["dtAction"] != null)
                dt = (DataTable)ViewState["dtAction"];
            else
            {
                SPWeb web = SPContext.Current.Web;
                SPList list = web.Lists.TryGetList(listName);//先到我的操作表中去找
                SPQuery qry = new SPQuery
                {
                    DatesInUtc = false
                };
                if (list != null)
                {

                    qry.Query = "<Where><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Integer'>" + SPContext.Current.Web.CurrentUser.ID + @"</Value></Eq></Where><OrderBy><FieldRef Name='Period' Ascending='TRUE' /><FieldRef Name='Frequency' Ascending='FALSE' /></OrderBy>";
                    dt = list.GetItems(qry).GetDataTable();
                    SPListItemCollection items = list.GetItems(qry);
                    SPListItem item;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            item = items.GetItemById((int)dr["ID"]);
                            dr["ID"] = new SPFieldLookupValue(item["ActionID"].ToString()).LookupId;
                        }
                        dt.AcceptChanges();
                    }
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    list = web.Lists.TryGetList(webObj.ActionList);
                    qry = new SPQuery();
                    qry.Query = "<OrderBy><FieldRef Name='Period' Ascending='TRUE' /><FieldRef Name='Frequency' Ascending='FALSE' /></OrderBy>";
                    dt = list.GetItems(qry).GetDataTable();
                }
                ViewState["dtAction"] = dt;
            }
            return dt;
        }
        private void BindDropDownList(DropDownList objDropDownList, string vField, string tField, DataTable dtSource)
        {
            objDropDownList.DataSource = dtSource;
            objDropDownList.DataValueField = vField;
            objDropDownList.DataTextField = tField;
            objDropDownList.DataBind();
        }

        public static DataTable GetActivities(int actionID, string ListName, DateTime dtFrom, DateTime dtTo)
        {
            //string ListName = webObj.ListTitle;
            DataTable dt = null;
            int userId = SPContext.Current.Web.CurrentUser.ID;
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists.TryGetList(ListName);
            SPQuery query = new SPQuery();
            //设置需要显示的字段
            query.ViewFields ="<FieldRef Name='ActionID'/>" +
                                "<FieldRef Name='Date'/>" +
                                "<FieldRef Name='During'/>" +
                                "<FieldRef Name='Quantity'/>";
            if (actionID == 0)
                query.Query = @"<Where>
                                  <And>
                                     <Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>" + userId + @"</Value>
                                     </Eq>
                                     <And>
                                     <Geq>
                                        <FieldRef Name='Date' />
                                        <Value Type='DateTime'>" + dtFrom.ToString("yyyy-MM-dd") + @"</Value>
                                     </Geq>
                                     <Leq>
                                        <FieldRef Name='Date' />
                                        <Value Type='DateTime'>" + dtTo.ToString("yyyy-MM-dd") + @"</Value>
                                     </Leq>
                                   </And>
                                  </And>
                               </Where>";
            else

                query.Query = @"<Where>
                                  <And>
                                     <Eq>
                                        <FieldRef Name='Author' LookupId='True' />
                                        <Value Type='Integer'>" + userId + @"</Value>
                                     </Eq>
                                    <And>
                                     <Eq>
                                        <FieldRef Name='ActionID' LookupId='True' />
                                        <Value Type='Lookup'>" + actionID + @"</Value>
                                     </Eq>
                                       <And>
                                         <Geq>
                                            <FieldRef Name='Date' />
                                            <Value Type='DateTime'>" + dtFrom.ToString("yyyy-MM-dd") + @"</Value>
                                         </Geq>
                                         <Leq>
                                            <FieldRef Name='Date' />
                                            <Value Type='DateTime'>" + dtTo.ToString("yyyy-MM-dd") + @"</Value>
                                         </Leq>
                                        </And>
                                    </And>
                                  </And>
                               </Where>";

            SPListItemCollection itemcoll = list.GetItems(query);
            DataTable dTable = itemcoll.GetDataTable();
            if (dTable == null)
                dTable = list.Items.GetDataTable().Clone();
            //dTable.Columns[1].ColumnName = "ActionID";
            //dTable.Columns[1].Caption = "操作";
            //dTable.Columns[2].ColumnName = "Date";
            //dTable.Columns[2].Caption = "日期";
            //dTable.Columns[3].ColumnName = "During";
            //dTable.Columns[3].Caption = "时长";
            //dTable.Columns[4].ColumnName = "Quantity";
            //dTable.Columns[4].Caption = "数量";

            int tFields = dTable.Columns.Count - 1;
            while (tFields > 4)
            {

                dTable.Columns.RemoveAt(tFields);
                tFields -= 1;
            }
            dTable.Columns.Add("DuringMax", typeof(double)).Caption = "最大时长";
            dTable.Columns.Add("DuringMin", typeof(double)).Caption = "最小时长";
            dTable.Columns.Add("DuringAvg", typeof(double)).Caption = "平均时长";
            dTable.Columns.Add("QuantityMax", typeof(double)).Caption = "最大数量";
            dTable.Columns.Add("QuantityMin", typeof(double)).Caption = "最小数量";
            dTable.Columns.Add("QuantityAvg", typeof(double)).Caption = "平均数量";
            dTable.Columns.Add("Attachments", typeof(int)).Caption = "附件个数";
            //SUM类型不能出错，原来的decimal改成double
            dt = dTable.Clone();
            var lQuery = from t in dTable.AsEnumerable()
                         group t by new { t1 = t.Field<string>("ActionID"), t2 = t.Field<int?>("Attachments") } into m
                         select new
                         {
                             ActionID = m.Key.t1,

                             DuringAvg = m.Average(n => n.Field<double?>("During")).HasValue ? decimal.Round(decimal.Parse(m.Average(n => n.Field<double?>("During")).ToString())) : 0,
                             DuringMax = m.Max(n => n.Field<double?>("During")),
                             DuringMin = m.Min(n => n.Field<double?>("During")),

                             QuantityAvg = m.Average(n => n.Field<double?>("Quantity")).HasValue ? decimal.Round(decimal.Parse(m.Average(n => n.Field<double?>("Quantity")).ToString()), 2) : 0,
                             QuantityMax = m.Max(n => n.Field<double?>("Quantity"))<0?m.Min(n => n.Field<double?>("Quantity")):m.Max(n => n.Field<double?>("Quantity")) ,
                             QuantityMin = m.Min(n => n.Field<double?>("Quantity"))<0?m.Max(n => n.Field<double?>("Quantity")):m.Min(n => n.Field<double?>("Quantity")),

                         };
            //if (lQuery.ToList().Count > 0)
            //{
            //    lQuery.ToList().ForEach(q =>
            //    {
            //        Console.WriteLine(q.QuantityAvg  + " , " + q.DuringAvg + " , " + q.ActionID );
            //        dt.Rows.Add ()
            //    });
            //}
            dt = ToDataTable(lQuery.ToList(), ref dt);
            return dt;

        }
         private static  DataTable ToDataTable<T>(IEnumerable<T> collection,ref DataTable dt)
        {
            var props = typeof(T).GetProperties();
            
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    DataRow dr = dt.NewRow();
                    foreach (PropertyInfo pi in props)
                    {
                        dr[pi.Name] = pi.GetValue(collection.ElementAt(i), null) == null ? DBNull.Value : pi.GetValue(collection.ElementAt(i), null);
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        private void ReadLists( int actionID)
        {
            try
            {
                DataTable dt = GetActivities(actionID,webObj.ListTitle ,dtStart.SelectedDate ,dtEnd.SelectedDate );
                this.gvShowData.DataSource = dt.DefaultView;// lQuery.ToList();// dataTableDistinct;
                this.gvShowData.DataBind();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }

        }
        #endregion
    }
}
