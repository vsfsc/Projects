using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;
using VSDLL.DAL;
using VSDLL.BLL;

namespace PowerPA.PersonalInfor
{
    public partial class PersonalInforUserControl : UserControl
    {
        public PersonalInfor webObj;
        #region 事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fvBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ViewState["dtUser"] != null)
            {
                DataTable dt = (DataTable)ViewState["dtUser"];
                DataRow dr = dt.Rows[0];
                dr["Name"] = tbName.Text;

                dr["IDCard"] = tbIDCard.Text;
                if (rblistSex.SelectedValue != null)
                {
                    dr["Sex"] = int.Parse(rblistSex.SelectedValue);
                }
                try
                {
                    dr["Birthday"] = tbBirthday.Text;
                }
                catch
                {
                    string birth = tbBirthday.Text;
                    if (Common.isNumberic(birth) && birth.Length == 8)
                        dr["Birthday"] = ChangeDateFormat(birth);
                }
                dr["Telephone"] = tbTelephone.Text;
                dr["Email"] = tbEmail.Text;
                dr["Description"] = tbDescription.Text;
                if (ddlDegree.SelectedValue != "0")
                {
                    dr["DegreeID"] = int.Parse(ddlDegree.SelectedValue);
                }
                if (ddlIndustry.SelectedValue != "0")
                {
                    dr["IndustryID"] = int.Parse(ddlIndustry.SelectedValue);
                }
                if (ddlProfession.SelectedValue != "0")
                {
                    dr["ProfessionID"] = int.Parse(ddlProfession.SelectedValue);
                }
                if (ddlXian.SelectedValue != "0" && ddlXian.SelectedValue!="")
                {
                    dr["LocationID"] = int.Parse(ddlXian.SelectedValue);
                }
                try
                {
                    if (dr["UserID"].ToString() != "")//新的用户
                    {
                        int result = UserDAL.UpdateUser(dr);
                    }

                    else
                    {
                        long userID = UserDAL.InsertUser(dr);
                        dr["UserID"] = userID;
                        dt.AcceptChanges();
                        ViewState["dtUser"] = dt;
                    }

                    if (Request.QueryString["Source"] != null)
                        Response.Redirect(Request.QueryString["Source"].ToString());
                    else
                        lbErr.Text = "保存成功";
                }
                catch (Exception ex)
                {
                    lbErr.Text = ex.ToString();
                }
                
            }
            else
            {
                lbErr.Text = "尚未登陆，无法操作！";
            }
        }
       /// <summary>
       /// 地区中省下拉框选项更改事件
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        protected void ddlSheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ddlSheng.SelectedValue;
            if (value != "0")
            {
                ddlShi.Visible = true;
                int intvalue = int.Parse(value);
                DataTable dtLocations = (DataTable)ViewState["dtLocations"];
                DataSet ds = UserBLL.GetLocationsByParentID(dtLocations, intvalue);
                BindDDL(ddlShi, ds, 0);
            }
        }
        /// <summary>
        /// 地区中市下拉框选项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlShi_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ddlShi.SelectedValue;
            if (value != "0")
            {
                ddlXian.Visible = true;
                int intvalue = int.Parse(value);
                DataTable dtLocations = (DataTable)ViewState["dtLocations"];
                DataSet ds = UserBLL.GetLocationsByParentID(dtLocations, intvalue);
                BindDDL(ddlXian, ds, 0);
            }
        }
        #endregion
        #region 方法
        private string ChangeDateFormat(string txtNum)
        {
            string retString;
            retString = DateTime.ParseExact(txtNum, "yyyyMMdd", null).ToShortDateString();
            return retString;
        }
        /// <summary>
        /// 控件初始化方法
        /// </summary>
        private void fvBind()
        {
            try
            {
                string account =VSDLL.Common.Users. GetLoginAccount;
                if (account != "")
                {
                    DataTable dtLocations = UserDAL.GetLocation().Tables[0];
                    ViewState["dtLocations"] = dtLocations;
                    DataSet ds = UserDAL.GetUserByAccount(account);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        DataTable dtTmp = ds.Tables[0].Copy();
                        dtTmp.Rows.Add(dtTmp.NewRow());
                        dtTmp.Rows[0]["Account"] = account;
                        ViewState["dtUser"] = dtTmp;
                        lbAccount.Text = account;
                        BindDDL(ddlDegree, UserDAL.GetDegree(), 0);
                        BindDDL(ddlProfession, UserDAL.GetProfession(), 0);
                        BindDDL(ddlIndustry, UserDAL.GetIndustry(), 0);
                        DataSet dsLocation = UserBLL.GetLocationsByParentID(dtLocations, 0);
                        BindDDL(ddlSheng, dsLocation, 0);
                    }
                    else
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        ViewState["dtUser"] = ds.Tables[0].Copy();
                        lbAccount.Text = SystemDataExtension.GetString(dr, "Account");
                        tbName.Text = SystemDataExtension.GetString(dr, "Name");
                        tbIDCard.Text = SystemDataExtension.GetString(dr, "IDCard");
                        tbTelephone.Text = SystemDataExtension.GetString(dr, "Telephone");
                        tbEmail.Text = SystemDataExtension.GetString(dr, "Email");
                        tbDescription.Text = SystemDataExtension.GetString(dr, "Description");
                        rblistSex.SelectedValue = SystemDataExtension.GetBool(dr, "Sex")?"1":"0";
                        DateTime? birth = SystemDataExtension.GetNullDateTime(dr, "Birthday") ;
                        tbBirthday.Text = birth !=null?birth.Value.ToShortDateString():"";
                        int initValue = SystemDataExtension.GetInt16(dr, "DegreeID");
                        BindDDL(ddlDegree, UserDAL.GetDegree(), initValue);

                        initValue = SystemDataExtension.GetInt16(dr, "ProfessionID");
                        BindDDL(ddlProfession, UserDAL.GetProfession(), initValue);

                        initValue = SystemDataExtension.GetInt16(dr, "IndustryID");
                        BindDDL(ddlIndustry, UserDAL.GetIndustry(), initValue);

                        initValue = SystemDataExtension.GetInt16(dr, "LocationID");//县级ID

                        int parentId = 0;
                        if (initValue == 0)
                        {
                            DataSet dsLocation = UserBLL.GetLocationsByParentID(dtLocations, 0);
                            BindDDL(ddlSheng, dsLocation, 0);
                        }
                        else
                        {
                            DataSet dsXian = UserBLL.GetParentLocationsByID(dtLocations, initValue, ref parentId);
                            BindDDL(ddlXian, dsXian, initValue);
                            initValue = parentId;
                            DataSet dsShi = UserBLL.GetParentLocationsByID(dtLocations, initValue, ref parentId);
                            BindDDL(ddlShi, dsShi, initValue);
                            initValue = parentId;
                            DataSet dsSheng = UserBLL.GetParentLocationsByID(dtLocations, initValue, ref parentId);
                            BindDDL(ddlSheng, dsSheng, initValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                lbErr.Text = ex.ToString();
            }
        }
        /// <summary>
        /// 绑定下拉列表框的方法
        /// </summary>
        /// <param name="ddl">下拉列表框</param>
        /// <param name="ds">要绑定的数据集</param>
        /// <param name="initValue">选中的初始值</param>
        private void BindDDL(DropDownList ddl,DataSet ds,int initValue)
        {
            //先清空DropDownList
            ddl.Items.Clear();
            //再绑定DropDownList
            ddl.DataSource = ds.Tables[0].DefaultView;
            ddl.DataTextField = "Title";
            ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName  ;
            ddl.DataBind();
            ListItem li = new ListItem("--请选择--", "0");
            ddl.Items.Insert(0, li);
            ddl.SelectedValue = initValue.ToString();

        }

        
        #endregion
    
    }
}
