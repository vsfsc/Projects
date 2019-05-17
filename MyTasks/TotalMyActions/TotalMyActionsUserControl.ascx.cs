using System;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace MyTasks.TotalMyActions
{
    public partial class TotalMyActionsUserControl : UserControl
    {
        #region 事件
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SPContext.Current.Web.CurrentUser == null)
            {
                lblMsg.Text = "请选登录";
                divUpload.Visible = false;
                return;
            }
            bool isExits = ListExits();
            if (!isExits)
            {
                lblMsg.Text = webObj.ListName + "  列表不存在！";
                divUpload.Visible = false;
                return;
            }
            lblMsg.Text = "";
            divUpload.Visible = true;
            if (!Page.IsPostBack)
            {
                dtStart.SelectedDate = DateTime.Today.AddDays(-webObj.TotalDays );
                dtEnd.SelectedDate = DateTime.Today;
                FillData();
            }


            rbOpt.SelectedIndexChanged += rbOpt_SelectedIndexChanged;
            btnImport.Click += btnImport_Click;
 
        }
        /// <summary>
        /// 汇总数据并填充 
        /// </summary>
        private void FillData()
        {
            try
            {
                int days = rbOpt.SelectedItem.Value == "1"?1:0;
                string lstName = webObj.ListName;
                DataTable dt = GetDataTable(days, lstName,dtStart.SelectedDate ,dtEnd.SelectedDate );
                if (dt == null)
                {
                    lblMsg.Text = "此段时间内无数据！";
                    return;
                }
                webObj.ChartData = dt;
                Table tbl = new Table();
                tbl.Width = 500;
                tbl.BorderWidth = 1;
                 
                TableRow tRow;
                TableCell tCell;
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (i % 10 == 0)
                    {
                        tRow = new TableRow();
                        tRow.BorderWidth = 1;
                        tbl.Rows.Add(tRow);
                        tRow.BorderWidth = 1;
                        tRow = new TableRow();
                        tbl.Rows.Add(tRow);//两行
                    }
                    tCell = new TableCell();
                    tCell.HorizontalAlign = HorizontalAlign.Center;
                    tCell.Font.Bold = true;
                    tCell.BorderWidth = 1;
                    tCell.Text = dr["Title"].ToString();
                    tbl.Rows[tbl.Rows.Count - 2].Cells.Add(tCell);
                    tCell = new TableCell();
                    tCell.BorderWidth = 1;
                    tCell.HorizontalAlign = HorizontalAlign.Center;
                    tCell.Text = dr["value"].ToString();
                    tbl.Rows[tbl.Rows.Count - 1].Cells.Add(tCell);


                    i = i + 1;
                }
                this.divContent.Controls.Add(tbl);
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }
        void rbOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbOpt.SelectedItem.Value == "1")//datetime
            {
                spanDate.Visible = true;
            }
            else//all
            {
                spanDate.Visible = false;
            }
        }

        void btnImport_Click(object sender, EventArgs e)
        {
            FillData();
        }
        #endregion
        #region 方法
        private bool ListExits()
        {
            bool isExits = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
                {
                    using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList parentList = spWeb.Lists.TryGetList(webObj.ListName);
                        if (parentList == null)
                        {
                            isExits = false;

                        }
                        else
                            isExits = true;
                    }
                }
            });
            return isExits;
        }
        #endregion
        #region 属性
        //
        public DateTime dtFrom
        {
            get { return dtStart.SelectedDate; }
        }
        public DateTime dtTo
        {
            get { return dtEnd.SelectedDate; }
        }
        public int SearchByDate
        {
            get { return rbOpt.SelectedItem.Value == "1" ? 1 : 0; }
        }
        public TotalMyActions webObj { get; set; }
        #endregion
        #region 方法
        /// <summary>
        /// 获取操作和时长
        /// </summary>
        /// <param name="tDays"></param>
        /// <param name="listName"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(int tDays, string listName,DateTime dtFrom,DateTime dtTo)
        {
            DataTable _table = new DataTable();
            //获取用户ID
            int userID = SPContext.Current.Web.CurrentUser.ID;
            DataColumn col = new DataColumn();
            col.DataType = typeof(string);
            col.ColumnName = "Title";
            _table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = typeof(int);
            col.ColumnName = "Value";
            _table.Columns.Add(col);
            DataRow row;
            DataTable dt = GetAssitants(userID, tDays, listName,dtFrom,dtTo);
            if (dt == null)
                return null;
            //需要添加System.Data.DataSetExtensiona的引用，
            var query = from t in dt.AsEnumerable()
                        group t by new { t1 = t.Field<string>("Action") } into m
                        select new
                        {
                            action = m.Key.t1,
                            ActualDuring = m.Sum(n => n.Field<double?>("ActualDuring"))
                        };
            if (query.ToList().Count > 0)
            {
                query.ToList().ForEach(q =>
                {
                    row = _table.NewRow();
                    row["Title"] = q.action;
                    row["Value"] = q.ActualDuring;
                    _table.Rows.Add(row);
                });
            }
            
            return _table;
        }

        /// <summary>
        /// 某个人的一段时间内,助手数据ActualDuring
        /// tDays,按日期还是所有的操作
        /// </summary>
        /// <returns></returns>
        private static  DataTable GetAssitants(int userID, int tDays,string listName,DateTime dtStart,DateTime dtTo)
        {
            DataTable retDataTable = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
                {
                    using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList spList = spWeb.Lists.TryGetList(listName );
                        if (spList != null)
                        {
                            SPQuery qry = new SPQuery();

                            qry.ViewFields = "<FieldRef Name='Author' /><FieldRef Name='ActualDuring' /><FieldRef Name='Action' /><FieldRef Name='ActualDate' />";
                            if (tDays>0)//按日期
                            {
                                string strFrom = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtStart);
                                string strTo = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtTo.AddDays(1));

                                qry.Query = @"<Where><And><And><Geq><FieldRef Name='ActualDate' /><Value Type='DateTime'>" + strFrom
                                + "</Value></Geq><Lt><FieldRef Name = 'ActualDate' /><Value Type = 'DateTime'> " + strTo
                                + "</Value></Lt></And><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Lookup'>" + userID + "</Value></Eq></And></Where><OrderBy><FieldRef Name='ActualDate' /></OrderBy>";
                            }
                            else//等于0为所有的数据
                            {
                                qry.Query = @"<Where><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Lookup'>" + userID + "</Value></Eq></Where><OrderBy><FieldRef Name='ActualDate' /></OrderBy>";
                            }

                            SPListItemCollectionPosition lstPosi;
                            retDataTable = spList.GetDataTable(qry, SPListGetDataTableOptions.RetrieveLookupIdsOnly, out lstPosi);
                            //SPListItemCollection listItems = spList.GetItems(qry);
                            //if (listItems.Count > 0)
                            //    retDataTable = listItems.GetDataTable();
                        }
                    }
                }
            });
            return retDataTable;
        }
        #endregion
    }
}
