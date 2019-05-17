using System;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace LNMCM.Layouts.LNMCM
{
    //
    public partial class AllEnrolls : LayoutsPageBase
    {
        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            bool right = JudgeRight();
            if (!right )
            {
                Response.Redirect(SPContext.Current.Web.Url);
                //lblMsg.Text = "报歉，您无权查看！";
                return;
            }
            if (!Page.IsPostBack)
                BindMembers();
            btnQuery.Click += BtnQuery_Click;
            gvMembers.RowDataBound += GvMembers_RowDataBound;
            //gvMembers.RowCreated += gvMembers_RowCreated;
            gvMembers.PageIndexChanging += GvMembers_PageIndexChanging;
            btnImport.Click += BtnImport_Click;

        }

        private void GvMembers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //设置鼠标滑过，行变色的效果
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //当鼠标放上去的时候 先保存当前行的背景颜色 并设置新的背景色
                e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='LightSteelBlue'; this.style.cursor='pointer';");
                //当鼠标离开的时候 将背景颜色恢复成之前的颜色
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor;");
            }

            //设置鼠标点击，行变色、鼠标指针变成手状的效果
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onclick", "this.style.backgroundColor='#99cc00'; this.style.cursor='pointer';");
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetAllEnrollsIncludeMems;
            WriteToExcel("报名信息.xlsx", dt);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GvMembers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMembers.DataSource = EnrollsQuery;
            gvMembers.PageIndex = e.NewPageIndex;
            gvMembers.DataBind();
        }


        //查询不同的单位
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            DataTable dt = GetAllEnrollsIncludeMems;
            if (ddlSchool.SelectedIndex > 0)
            {
                DataRow[] drs = dt.Select("培养单位='" + ddlSchool.SelectedItem.Value  + "'");//Text改Value

                DataTable tmpDt = dt.Clone();
                DataSet ds = new DataSet();
                ds.Tables.Add(tmpDt);
                ds.Merge(drs);

                dt = ds.Tables[0];
            }
            ViewState["dtEnrollsQuery"] = dt.Copy();
            gvMembers.DataSource = dt.Copy();

            gvMembers.DataBind();

            ShowTotal(dt.Rows.Count );
        }

        //删除报名同时要删除成员，以及AD中的数据
        protected void gvMembers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                String Id = e.CommandArgument.ToString();
                DataTable dt = DAL.User.GetEnrollByEnrollCode(Id).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    int result = BLL.Enroll.DeleteEnroll(Id);
                    if (result == 1)
                    {
                        Enroll dal = new Enroll();
                        dal.DisableUser(Id,HiddenField1.Value);
                        dt = EnrollsQuery;
                        DataRow[] drs = dt.Select("报名序号='" + Id + "'");
                        dt.Rows.Remove(drs[0]);
                        EnrollsQuery = dt.Copy();

                        dt = GetAllEnrollsIncludeMems;
                        drs = dt.Select("报名序号='" + Id + "'");
                        dt.Rows.Remove(drs[0]);
                        GetAllEnrollsIncludeMems = dt.Copy();
                        BindMembers();
                    }
                }

            }
        }

        protected void gvMembers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            if (e.Row.FindControl("btnviewpwd") != null)
            {
                Button CtlButton = (Button)e.Row.FindControl("btnviewpwd");
                CtlButton.Click += new EventHandler(CtlButton_Click);
            }
        }

        private void CtlButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            GridViewRow gvr = (GridViewRow)button.Parent.Parent;
            string pk ="报名序号："+ gvr.Cells[3].Text.ToString()+";  密码:"+gvr.Cells[4].Text.ToString();
            DAL.Common.ShowMessage(Page, GetType(), pk);
        }
        #endregion

        #region 方法
        private bool JudgeRight()
        {
            string grpName = "辽宁省数学建模竞赛组委会";
            bool result = GetGroupofUser(grpName);
            SPWeb web = SPContext.Current.Web;
            SPRoleDefinitionBindingCollection usersRoles = web.AllRolesForCurrentUser;
            SPRoleDefinitionCollection siteRoleCollection = web.RoleDefinitions;
            SPRoleDefinition roleDefinition = siteRoleCollection["完全控制"];

            SPRoleDefinition roleDefinition2 = siteRoleCollection["参与讨论"];
            return result||usersRoles.Contains(roleDefinition)||usersRoles.Contains(roleDefinition2);
        }
        //用户有审核权限
        private bool GetGroupofUser(string groupName)
        {
            bool flag = false;
            using (SPSite siteCollection = SPContext.Current.Site)
            {
                using (SPWeb site = siteCollection.OpenWeb())
                {
                    //string groupName = "TestGroup";
                    //获取当前登录的用户
                    SPUser currentUser = site.CurrentUser;

                    //获取该用户在site/web中所有的组
                    SPGroupCollection userGroups = currentUser.Groups;
                    //循环判断当前用户所在的组有没有给定的组
                    foreach (SPGroup group in userGroups)
                    {
                        //Checking the group
                        if (group.Name.Contains(groupName))
                            flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        private DataTable _allEnrollsIncludeMems;
        private DataTable GetAllEnrollsIncludeMems
        {
            get
            {
                if (ViewState["dtEnrollsIncludeMems"] == null)
                {
                    _allEnrollsIncludeMems = DAL.User.GetAllEnrollsIncludeMembers().Tables[0];
                    ViewState["dtEnrollsIncludeMems"] = _allEnrollsIncludeMems;

                }
                return (DataTable)ViewState["dtEnrollsIncludeMems"];
            }
            set
            {
                ViewState["dtEnrollsIncludeMems"] = value;
            }
        }
        private DataTable EnrollsQuery
        {
            get
            {
                if (ViewState["dtEnrollsQuery"] == null)
                {

                    ViewState["dtEnrollsQuery"] = GetAllEnrollsIncludeMems;

                }
                return (DataTable)ViewState["dtEnrollsQuery"];
            }
            set
            {
                ViewState["dtEnrollsQuery"] = value;

            }

        }
        private DataTable _allEnrolls;
        private DataTable GetAllEnrolls
        {
            get
            {
                if (ViewState["dtEnrolls"] == null)
                {
                    _allEnrolls = DAL.User.GetAllEnrolls().Tables[0];
                    ViewState["dtEnrolls"] = _allEnrolls;

                }
                return (DataTable)ViewState["dtEnrolls"];
            }
        }
        private void BindMembers()
        {
            //所有培养单位
            //DataSet dsOrgType = DAL.User.GetSchoolByCity("");
            //ddlSchool.DataTextField = "Name";
            //ddlSchool.DataValueField = "Code";
            //DataRow dr = dsOrgType.Tables[0].NewRow();
            //dr["Name"] = "全部";
            //dr["Code"] = "00000";
            //dsOrgType.Tables[0].Rows.InsertAt(dr, 0);
            //ddlSchool.DataSource = dsOrgType;
            //ddlSchool.DataBind();


            //所有报名
            DataTable dt = EnrollsQuery;

            DataView dv = dt.DefaultView;
            DataTable dtSchoolDistinct = dv.ToTable(true, "培养单位");

            dtSchoolDistinct.Columns.Add("Name", typeof(string));
            dtSchoolDistinct.Columns.Add("Num", typeof(float));
            for (int i = 0; i < dtSchoolDistinct.Rows.Count; i++)
            {
                var icount = dt.Compute("count(培养单位)","培养单位='"+dtSchoolDistinct.Rows[i]["培养单位"]+"'");
                dtSchoolDistinct.Rows[i]["Name"] = dtSchoolDistinct.Rows[i]["培养单位"] + "(" +icount+ ")";
                dtSchoolDistinct.Rows[i]["Num"] = icount;
            }
            dtSchoolDistinct.DefaultView.Sort = "Num Desc";
            dtSchoolDistinct = dtSchoolDistinct.DefaultView.ToTable();

            ddlSchool.DataTextField = "Name";
            ddlSchool.DataValueField = "培养单位";
            DataRow dr = dtSchoolDistinct.NewRow();
            var allcount = dt.Rows.Count;
            dr["Name"] = "全部("+allcount+")";
            dr["培养单位"] = "全部";
            dtSchoolDistinct.Rows.InsertAt(dr, 0);
            ddlSchool.DataSource = dtSchoolDistinct;
            ddlSchool.DataBind();

            gvMembers.DataSource = dt.Copy();
            gvMembers.DataBind();
            if (dt.Rows.Count==0)
            {
                gvMembers.Width=800;
            }
            else
            {
                gvMembers.Width = 1800;
            }
            ShowTotal(dt.Rows.Count );

        }
        private void ShowTotal(int totals)
        {
            if (totals > 0)
            {
                gvMembers.Width = 1800;
                lblMsg.Text = "当前报名总数为：" + totals.ToString();
            }
            else
            {
                gvMembers.Width = 800;
                lblMsg.Text = "";
            }

        }
        private const string _exportPath = @"/_layouts/15/excel/";
        //基于空白的Excel文件写入，没有模板
        private void WriteToExcel(string fileName, DataTable dt)
        {
            IWorkbook book = null;

            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                book = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                book = new XSSFWorkbook();

            ISheet wSheet;//define worksheet

            wSheet = book.CreateSheet(dt.TableName);//

            #region 列标题及样式
            ICellStyle headStyle = book.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;
            IFont font = book.CreateFont();
            font.FontHeightInPoints = 14;
            font.FontName = "宋体";
            font.Boldweight = 700;
            headStyle.SetFont(font);

            NPOI.SS.UserModel.IRow row1 = wSheet.CreateRow(0);
            NPOI.SS.UserModel.IRow rowtemp;


            #endregion
            #region 正文样式
            ICellStyle itemStyle = book.CreateCellStyle();
            itemStyle.Alignment = HorizontalAlignment.Left;
            font = book.CreateFont();
            font.FontHeightInPoints = 14;
            font.FontName = "宋体";
            itemStyle.SetFont(font);
            #endregion
            #region f填充数据
            string[] colsOrder = new string[] {"培养单位","报名序号","密码","邮箱" ,"报名时间","队员1学号","队员1姓名","队员1电话","队员2学号","队员2姓名","队员2电话","队员3学号","队员3姓名","队员3电话"};
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rowtemp = wSheet.CreateRow(i + 1);
                for (int j = 0; j < colsOrder.Length ; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(dt.Rows[i][colsOrder[j]].ToString ());//[dt.Columns[j].ColumnName].ToString());
                    rowtemp.GetCell(j).CellStyle = itemStyle;
                }
            }
            #endregion
            for (int i = 0; i < colsOrder.Length ; i++)
            {
                row1.CreateCell(i).SetCellValue(colsOrder[i]);// dt.Columns[i].Caption);
                row1.GetCell(i).CellStyle = headStyle;
                //设置列宽
                wSheet.AutoSizeColumn(i);
            }

            //保存到本地文件
            string _filePath = Server.MapPath(_exportPath) + fileName;
            using (FileStream fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write))
            {
                book.Write(fs);
            }
            book = null;
            divSaveAs.InnerHtml = "<a href='/LNMCM" + _exportPath + fileName + "'>单击下载</a>";
        }
        #endregion
    }
}
