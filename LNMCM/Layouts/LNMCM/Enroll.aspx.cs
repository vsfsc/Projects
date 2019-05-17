using System;
using System.Web;
using System.Web.Mail;
using System.Runtime.InteropServices;
using System.DirectoryServices;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LNMCM.DAL;
using LNMCM.BLL;
using System.Security.Principal;
using System.DirectoryServices.ActiveDirectory;
using System.Web.Configuration;
using System.Web.UI;
using System.Text.RegularExpressions;
namespace LNMCM.Layouts.LNMCM
{
    public partial class Enroll : LayoutsPageBase
    {

        #region 事件
        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                InitControl();
            }
            btnSave.Click += btnSave_Click;
            btnClose.Click += BtnClose_Click;
            txtNum.TextChanged += txtAccount_TextChanged;
            //ddlCity.SelectedIndexChanged += ddlProvince_SelectedIndexChanged;
            txtPwd.Attributes["value"] = txtPwd.Text;
            txtPwd1.Attributes["value"] = txtPwd1.Text;
        }
        #region 未使用
        void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (!Common.IsEmail(txtEmail.Text))
                lblEmailMsg.Text = "E-mail地址格式错误!";
            else
                lblEmailMsg.Text = "";

        }

        void txtTelephone_TextChanged(object sender, EventArgs e)
        {
            if (!Common.IsTelephone(txtTelephone.Text))
                lblTelMsg.Text = "电话输入格式错误";
            else
                lblTelMsg.Text = "";

        }

        void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!Common.IsChinese(txtName.Text))
                lblNameMsg.Text = "姓名只能输入中文";
            else
                lblNameMsg.Text = "";

        }

        void txtPwd1_TextChanged(object sender, EventArgs e)
        {
            if (txtPwd.Text.Length !=txtPwd1.Text.Length )
                lblPwd1Msg.Text = "密码与确认密码不一致！";
            else
                lblPwd1Msg.Text = "";

        }

        void txtPwd_TextChanged(object sender, EventArgs e)
        {
            if (txtPwd.Text.Length < 6)
                lblPwdMsg.Text = "密码长度不能小于6！";
            else
                lblPwdMsg.Text = "";

        }




        public static void ShowConfirm(string strMsg, string strUrl_Yes, string strUrl_No)
        {
            System.Web.HttpContext.Current.Response.Write("<Script Language='JavaScript'>if ( window.confirm('" + strMsg + "')) { window.location.href='" + strUrl_Yes +
            "' } else {window.location.href='" + strUrl_No + "' };</script>");
        }
        private bool IsMatching(string accString)
        {
            bool ismatch = true;
            ismatch = Common.isNumberic(accString);
            if (ismatch)
            {
                if (accString.Length >= 5 && accString.Length <= 8)
                    ismatch = true;
                else
                    ismatch = false;
            }

            return ismatch;
        }
        /// <summary>
        /// 报名号为学校代码+三位序号
        /// </summary>
        /// <param name="account"></param>
        private void UserAddToGroup(string account)
        {
            //string account = txtAccount.Text.Trim().Replace(" ", "");
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                string domain = ADHelper.Domain;
                string strConst = HiddenField1.Value;
            if (impersonateValidUser("administrator", domain, strConst))//.Substring(strConst.IndexOf(" ") + 1)))
                {
                    ADHelper.AddUserToSafeGroup(account, account.Length == 5 ? "SmartNEUTeacher" : "SmartNEUStudent");
                    undoImpersonation();
                }
                else
                {
                    //Your impersonation failed. Therefore, include a fail-safe mechanism here.
                }

            });
        }
        //判断指定的帐户是否存在
        private bool UserExits(string account,ref int stateid)
        {
            int stateint = 0;
            //string account = txtAccount.Text.Trim().Replace(" ", "");
            bool retValue = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   string domain = ADHelper.Domain;
                   string strConst = HiddenField1.Value;
                   if (impersonateValidUser("administrator", domain, strConst.Substring(strConst.IndexOf(" ") + 1)))
                   {

                       retValue = Common.UserExits(account, ref stateint);//stateint

                       undoImpersonation();
                   }
                   else
                   {
                       //Your impersonation failed. Therefore, include a fail-safe mechanism here.
                   }

               });
            stateid = stateint;
            return retValue;
        }
        #endregion
        private void BtnClose_Click(object sender, EventArgs e)
        {
            //AllEnrolls();
            //ImportMembersDel();
            string currentwebUrl = SPContext.Current.Web.Url;
            Page.Response.Redirect(currentwebUrl);
        }
        //学号
        void txtAccount_TextChanged(object sender, EventArgs e)
        {
            string schoolCode = ddlSchool.SelectedValue;
            DataTable dtMems = DAL.User.GetMembersByCode(schoolCode).Tables[0];
            lblMsg.Text = "";
            if (CheckMember(dtMems, txtNum.Text))
            {
                lblNumMsg.Text = "你填写的学籍信息已经存在，不能重复报名！";
                return;
            }
            else
                lblNumMsg.Text = "";
        }
        #region 恢复数据
        private void  ImportMembersDel ()
        {
            DataSet ds = DAL.User.GetEnrollDel();

            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                string schoolCode = "10145";// dr1["SchoolCode"].ToString();
                string account = dr1["EnrollCode"].ToString();
                DataTable dtMems = DAL.User.GetMembersByCode(schoolCode).Tables[0];
                DataRow dr;
                if (dr1["Name2"] != null && dr1["Name2"].ToString() != "")
                {
                    dr = dtMems.NewRow();//成员
                    dr["Name"] = dr1["Name2"];
                    //dr["Major"] = dr1["Major"];
                    //dr["Sex"] = dr1["Sex2"];// int.Parse(rblSex.SelectedValue);
                    dr["Number"] = dr1["Number2"];
                    dr["Mobile"] = dr1["Mobile2"];
                    dr["Flag"] = 1;
                    dr["Created"] = dr1["Created"];
                    dr["EnrollCode"] = account;
                    dr["IsCaptain"] = 0;
                     DAL.User.InsertMember(dr);
                }

                if (dr1["Name3"] != null && dr1["Name3"].ToString() != "")
                {
                    dr = dtMems.NewRow();//成员
                    dr["Name"] = dr1["Name3"];
                    //dr["Major"] = dr1["Major"];
                    //dr["Sex"] = dr1["Sex2"];// int.Parse(rblSex.SelectedValue);
                    dr["Number"] = dr1["Number3"];
                    dr["Mobile"] = dr1["Mobile3"];
                    dr["Flag"] = 1;
                    dr["Created"] = dr1["Created"];
                    dr["EnrollCode"] = account;
                    dr["IsCaptain"] = 0;
                     DAL.User.InsertMember(dr);
                }
            }
        }
        /// <summary>
        /// 导入误删除的数据
        /// </summary>
        private void AllEnrolls()
        {
            DataSet ds = DAL.User.GetEnrollDel();

            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                string schoolCode = "10145";// dr1["SchoolCode"].ToString();
                DataTable dtMems = DAL.User.GetMembersByCode(schoolCode).Tables[0];
                //获取学校下的报名信息，删除和不删除的充号不重复
                DataTable dt = DAL.User.GetEntrollByCode(schoolCode).Tables[0];
                string account = dr1["EnrollCode"].ToString ();// GetEntrollID(dt.Copy(), schoolCode);//生成的报名号

                string ouName = GetOUNameFromWebUrl();//子网站url链接

                bool succeed = SaveAD(account,dr1["Name"].ToString ().Trim(), dr1["Email"].ToString ().Trim(), dr1["Mobile"].ToString().Trim(), dr1["Pwd"].ToString ().Trim(), ouName, true, dr1["OrgName"].ToString ().Trim());
                if (succeed)
                {
                    DataRow dr = dtMems.NewRow();//成员
                    dr["Name"] = dr1["Name"];
                    //dr["Major"] = dr1["Major"];
                    dr["Sex"] = dr1["Sex"];// int.Parse(rblSex.SelectedValue);
                    dr["Number"] = dr1["Number"];
                    dr["Mobile"] = dr1["Mobile"];
                    dr["Flag"] = 1;
                    dr["Created"] = dr1["Created"] ;
                    dr["EnrollCode"] = account;
                    dr["IsCaptain"] = 1;
                    DataRow drEnroll = dt.NewRow();//报名
                    drEnroll["EnrollCode"] = account;
                    drEnroll["Org"] = account.Substring(0, 5) ;
                    drEnroll["Email"] = dr1["Email"] ;
                    drEnroll["Flag"] = 1;
                    drEnroll["Created"] = dr1["Created"] ;
                    drEnroll["Pwd"] = dr1["Pwd"] ;
                    BLL.Enroll.AddEnroll(drEnroll, dr);
                }
            }
            lblMsg.Text = "成功导入！";
        }
        #endregion
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string schoolCode = ddlSchool.SelectedValue;
                DataTable dtMems = DAL.User.GetMembersByCode(schoolCode).Tables[0];
                if (CheckMember(dtMems, txtNum.Text))
                {
                    lblMsg.Text = "你已经在其它团队报名本届比赛，不能重复报名！";
                    return;
                }
                else
                    lblMsg.Text = "";
                //获取学校下的报名信息，删除和不删除的充号不重复
                DataTable dt = DAL.User.GetEntrollByCode(schoolCode).Tables[0];
                string account = GetEntrollID(dt.Copy(), schoolCode);//生成的报名号

                string ouName = GetOUNameFromWebUrl();//子网站url链接

                bool succeed = SaveAD(account, txtName.Text.Trim(), txtEmail.Text.Trim(), txtTelephone.Text.Trim(), txtPwd.Text.Trim(), ouName, true, ddlSchool.SelectedItem.Text);
                if (succeed)
                {
                    DataRow dr = dtMems.NewRow();//成员
                    dr["Name"] = txtName.Text.Trim();
                    dr["Major"] = txtProf.Text.Trim();
                    dr["Sex"] = rblSex.SelectedItem.Text;// int.Parse(rblSex.SelectedValue);
                    dr["Number"] = txtNum.Text.Trim();
                    dr["Mobile"] = txtTelephone.Text.Trim();
                    dr["Flag"] = 1;
                    dr["Created"] = DateTime.Now;
                    dr["EnrollCode"] = account;
                    dr["IsCaptain"] = 1;
                    DataRow drEnroll = dt.NewRow();//报名
                    drEnroll["EnrollCode"] = account;
                    drEnroll["Org"] = ddlSchool.SelectedValue;
                    drEnroll["Email"] = txtEmail.Text.Trim();
                    drEnroll["Flag"] = 1;
                    drEnroll["Created"] = DateTime.Now;
                    drEnroll["Pwd"] = txtPwd.Text;
                    BLL.Enroll.AddEnroll(drEnroll, dr);
                    bool flag = SendEmail(account);
                    //if (flag)
                    //    lblMsg.Text = "报名成功！报名信息已经发送邮件，请注意查收！";
                    string sexString = rblSex.SelectedItem.Text=="男"?"先生":"女士" ;

                    lbuserName.Text = txtName.Text.Trim() + sexString;
                    lbuseAcc.Text = account;
                    lbuserADAcc.Text = "ccc\\" + account;
                    lbDateNow.Text = DateTime.Now.ToString("f");
                    lbEmail.Text= txtEmail.Text.Trim();
                    divregInfo.Visible = false;
                    divregSuccess.Visible = true;
                }
                else
                {
                    lblMsg.Text = "很抱歉，报名失败，请检查填报信息后重试！";
                }

            }
            catch (Exception ex)
            {
                lblMsg.Text = "很抱歉，你本次报名失败，请检查填报信息后重试！" + ex.ToString();
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('报名失败！" + ex.ToString() + "')</script>");
            }
        }
        #endregion
        #region 方法新加
        /// <summary>
        /// 将当前子网站的URL名称作为ouName
        /// </summary>
        /// <returns></returns>
        private string GetOUNameFromWebUrl()
        {
            string ouName = SPContext.Current.Web.Url;
            int index = ouName.LastIndexOf("/");
            if (index > -1)
                ouName = ouName.Substring(ouName.LastIndexOf("/") + 1);
            return ouName;

        }
        private bool  CheckMember(DataTable  dt ,string xueHao)
        {
            DataRow[] drs = dt.Select("Number='" + xueHao + "'");
            if (drs.Length >0)
                return true;//报名存在
            else
                return false;
        }
        /// <summary>
        /// 获取报名号，学校代号加三位编号
        /// 学校代号和学号为唯一报名
        /// 先判断报名的唯一性，如果报名存在，则返回空
        /// </summary>
        /// <param name="dt">todo: describe dt parameter on GetEntrollID</param>
        /// <param name="schoolCode">todo: describe schoolCode parameter on GetEntrollID</param>
        /// <returns></returns>
        private string GetEntrollID( DataTable  dt ,string schoolCode)
        {

            int id = 1;
            string enrollID;

            if (dt.Rows.Count >0)
            {
                enrollID = dt.Rows[0]["EnrollCode"].ToString();
                id = int.Parse(enrollID.Substring(5));
                id = id + 1;
            }
            enrollID = schoolCode + id.ToString().PadLeft(3,'0');
            return enrollID;
        }
        #endregion
        //绑定控件
        #region 数据加载
        private void InitControl()
        {
            DataSet dsOrgType = DAL.User.GetSchoolByCity("");
            ddlSchool.DataTextField = "Name";
            ddlSchool.DataValueField = "Code";
            ddlSchool.DataSource = dsOrgType;
            ddlSchool.DataBind();

            DateTime endEnrollDate = DAL.Common.getEnrollEndDate();
            if (endEnrollDate<DateTime.Now)
            {
                divregInfo.Visible = false;
                divregSuccess.Visible = false;
                lbnoEnroll.Text = "当前已过报名期限！";
                lbnoEnroll.ForeColor = System.Drawing.Color.Red;
                lbnoEnroll.Font.Size =15;
            }
            //院系分类
            //foreach (DataRow dr in dsOrgType.Tables[0].Rows  )
            //{
            //    ddlCity.Items.Add(dr["City"].ToString());
            //}

            //ddlProvince_SelectedIndexChanged(null, null);

        }
        //城市改动，显示下面的学校
        void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            DataSet dsCity = DAL.User.GetSchoolByCity("");// ddlCity.SelectedItem.Text );// int.Parse(ddlCity.SelectedValue));
            ddlSchool.DataTextField = "Name";
            ddlSchool.DataValueField = "Code";
            ddlSchool.DataSource = dsCity;
            ddlSchool.DataBind();
            ddlSchool.SelectedIndex = 0;

        }

        #endregion
        #region 方法
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);
        private bool impersonateValidUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }

        private void undoImpersonation()
        {
            impersonationContext.Undo();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="txtName"></param>
        /// <param name="txtEmail"></param>
        /// <param name="txtTelephone"></param>
        /// <param name="txtPwd"></param>
        /// <param name="ouName">ouName为子网站的Url地址</param>
        /// <param name="schoolName"></param>
        /// <param name="userEnabled"></param>
        /// <param name="userDept"></param>
        /// <returns></returns>
        private bool SaveAD(string userAccount, string txtName, string txtEmail, string txtTelephone, string txtPwd, string ouName,  bool userEnabled,string userDept="")
        {
            bool retValue = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    string domain = ADHelper.Domain;
                    string strConst = HiddenField1.Value;
                    if (impersonateValidUser("administrator", domain, strConst))//.Substring(strConst.IndexOf(" ") + 1)))
                    {
                        //string ouName = strConst.Substring(0, strConst.IndexOf(" "));// "iSmart";// System.Configuration.ConfigurationManager.AppSettings["adPath"];
                        string ouPath = ADHelper.GetDirectoryEntryOfOU("", ouName);
                        retValue = ADHelper.AddUser(userAccount, txtName, txtEmail, txtTelephone, txtPwd, ouPath ,ouName , userEnabled,userDept  );
                        undoImpersonation();
                    }
                    else
                    {
                        //Your impersonation failed. Therefore, include a fail-safe mechanism here.
                    }

                });
                return retValue;
            }
            catch
            {
                return false;
            }
        }
        public bool  DisableUser(string userAccount,string strConst )
        {
            bool retValue = false;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    string domain = ADHelper.Domain;
                    //string strConst = HiddenField1.Value;
                    if (impersonateValidUser("administrator", domain, strConst))//.Substring(strConst.IndexOf(" ") + 1)))
                    {
                        ADHelper.EnabledUser(userAccount, false);;// DeleteAdUser(userAccount)
                        undoImpersonation();
                    }
                    else
                    {
                        //Your impersonation failed. Therefore, include a fail-safe mechanism here.
                    }

                });
                return retValue;
            }
            catch
            {
                return false;
            }
        }
        private bool SendEmail(string entrollID)
        {
            bool flag = false;
            string sexString = "女士";
            if (int.Parse(rblSex.SelectedValue) == 1)
            {
                sexString = "先生";
            }
            string dtNow=DateTime.Now.ToString("f");
            string cts = "<div style='font-size:14px;background-color:#F8F8F8;font-family:微软雅黑;width:580px;padding:5px;'><p><b>尊敬的" + txtName.Text.Trim() + sexString + "</b></p><p>您好，欢迎报名参加辽宁省研究生数学建模竞赛。</p>";
            cts+= "<p>您的注册信息如下：</p><p align='center'><i>账号："+entrollID  +" 密码："+txtPwd.Text.Trim() +"</i></p>";
            cts+= "<div style='border:#999 solid 1px;'>请注意以下事项：<ul><li>登录时输入的账号格式为：ccc\\" + entrollID + "</li><li>请您一定保管好自己的账号和密码，以防丢失。</ul></div><p><p align='right'>辽宁省研究生数学建模竞赛组委会</p><p align='right'>" + dtNow + "</p><hr/><p style='font-size:20px;font-weight:600;text-align:center;'><i>辽宁省第二届研究生数学建模竞赛</i></p></div>";
            string email = WebConfigurationManager.AppSettings["emailFromlnmcm"];
            if (email != "")
            {
                string[] mails = Common.getEmailFrom(email);//配置文件信息:邮件地址,邮件密码,邮件smtp服务器地址
                //flag = Common.SendWebMail(mails[0], mails[2], mails[3], txtEmail.Text.Trim(), "智慧东大 - 账户注册成功", content, "1");
                flag = Common.SendMail(mails[0], mails[1], mails[2], new string[] { txtEmail.Text.Trim() }, "辽宁省研究生数学建模竞赛 - 报名成功", cts, mails[3]);
            }
            return flag;
        }
        /// <summary>
        /// 生成验证码的随机数
        /// </summary>
        /// <returns>返回五位随机数</returns>
        private string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            Random random = new Random();

            for (int i = 0; i < 5; i++)//可以任意设定生成验证码的位数
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }

            return checkCode;
        }

        #endregion
    }
}
