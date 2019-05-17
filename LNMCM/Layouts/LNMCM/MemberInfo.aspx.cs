using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;

namespace LNMCM.Layouts.LNMCM
{
    public partial class MemberInfo : LayoutsPageBase
    {
        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["ID"] == null) Return();
            int id = int.Parse(Page.Request.QueryString["ID"]);
            string code = Page.Request.QueryString["Code"];//报名ID
            MemberID = id;
            EnrollID = code;
            SchoolCode = code.Substring(0, 5);
            if (!Page.IsPostBack)
            {

                if (id > 0)//编辑
                {
                    DataTable dtMems = SchoolMembers;
                    DataRow[] drs = dtMems.Select("ID=" + id);
                    if (drs.Length > 0)
                    {
                        DataRow dr = drs[0];
                        txtName.Text = dr["Name"].ToString();

                        rblSex.SelectedIndex  = dr["Sex"].ToString()=="男"?0:1;
                        txtNum.Text = dr["Number"].ToString();
                        txtTelephone.Text = dr["Mobile"].ToString();
                        txtProf.Text = dr["Major"].ToString();
                    }


                }
            }
            btnSave.Click += BtnSave_Click;
            btnUnSave.Click += BtnUnSave_Click;
        }

        private void BtnUnSave_Click(object sender, EventArgs e)
        {
           
            Return();
        }
        private void Return()
        {
            string url = SPContext.Current.Web.Url;
            if (Page.Request.QueryString["Source"] != null)
                url = Page.Request.QueryString["Source"];
            if (Page.Request.QueryString["Code"] != null)
            {
                url += "?Code="+Page.Request.QueryString["Code"];
            }
            Response.Redirect(url);
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable dtMems = SchoolMembers;
            string xueHao = txtNum.Text.Trim();
            if (MemberID == 0)
            {
                bool chk = CheckMember(dtMems, xueHao);
                if (chk)
                {
                    lblMsg.Text = "该成员已经存在！";
                    return;
                }
            }

            DataRow dr = dtMems.NewRow();//成员

            if (MemberID > 0)
            {
                dr = dtMems.Select("ID=" + MemberID)[0];
            }
            else
            {
                dr["Flag"] = 1;
                dr["Created"] = DateTime.Now;
                dr["EnrollCode"] = EnrollID;
                dr["IsCaptain"] = 0;
            }
            dr["Name"] = txtName.Text.Trim();
            dr["Major"] = txtProf.Text.Trim();
            dr["Sex"] = rblSex.SelectedItem.Text;// int.Parse(rblSex.SelectedValue);
            dr["Number"] = xueHao;
            dr["Mobile"] = txtTelephone.Text.Trim();

            if (MemberID > 0)
                DAL.User.UpdateMember(dr);
            else
                DAL.User.InsertMember(dr);
            Return();
        }
        #endregion
        #region 方法
        /// <summary>
        /// 加新成员时，在同一学校的报名中不能重复
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="xueHao"></param>
        /// <returns></returns>
        private bool CheckMember(DataTable dt, string xueHao)
        {
            DataRow[] drs = dt.Select("Number='" + xueHao + "'");
            if (drs.Length > 0)
                return true;//报名存在
            else
                return false;
        }
        #endregion
        #region Property
        private int memberID;
        private int MemberID
        {
            get { return memberID; }
            set { memberID = value; }
        }
        private string enrollID;
        private string EnrollID
        {
            get
            {
                return enrollID;
            }
            set
            {
                enrollID = value;
            }
        }
        private string schoolCode;
        private string SchoolCode
        {
            get {
                return schoolCode;
            }
            set
            {
                schoolCode = value;
            }
        }

        private DataTable dtMembers;
        private DataTable SchoolMembers
        {
            get 
            {
                if (dtMembers ==null)
                {
                    dtMembers = DAL.User.GetMembersByCode(SchoolCode ).Tables[0] ;
                }
                return dtMembers;

            }

        }
        #endregion
    }
}
