using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Web.UI.WebControls;

namespace LNMCM.Layouts.LNMCM
{
    public partial class MyEnroll : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.gvMembers.RowDataBound += gvMembers_RowDataBound;
                //SPUser user = SPContext.Current.Web.CurrentUser;
                string enrollCode = GetUserName();

                if (enrollCode != "")
                {
                    int result = BindEnroll(enrollCode);
                    if (result > 0)
                    {
                        BindMembers(enrollCode);
                        //BindWorks(enrollCode);
                        HiddenColByDate();

                    }
                    else
                    {
                        divInfo.InnerHtml = "<h3 style='color:red'>报名序号为“" + enrollCode + "”的报名信息不存在！</h3>";
                    }
                }
                else
                {
                    divInfo.InnerHtml = "<h3 style='color:red'>您尚未登录，无法查看报名信息！</h3>";
                }
            }
            catch (Exception ex)
            {

                divInfo.InnerHtml = ex.ToString();
            }
        }

        private void HiddenColByDate()
        {
            DateTime endDate = DAL.Common.getEnrollEndDate();
            if (endDate < DateTime.Now)
            {
                gvMembers.Columns[gvMembers.Columns.Count - 1].Visible = false;
                gvMembers.Columns[gvMembers.Columns.Count - 2].Visible = false;
                lnkNewMember.Visible = false;
            }
        }

        /// <summary>
        /// 第一行数据为队长，可编辑不可删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMembers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    e.Row.Cells[e.Row.Cells.Count - 2].Controls[1].Visible = false;//不可删除
                    //e.Row.Cells[e.Row.Cells.Count - 1].Controls[0].Visible = false;//修改
                    //e.Row.Enabled = false;//不可修改

                }

                if (e.Row.RowIndex != -1)
                {
                    int indexID = 0;
                    if (gvMembers.AllowPaging)
                    {
                        indexID = gvMembers.PageIndex * gvMembers.PageSize + e.Row.RowIndex + 1;
                    }
                    else
                    {
                        indexID=e.Row.RowIndex + 1;
                    }
                    e.Row.Cells[1].Text = indexID.ToString();
                }

            }

        }



        private string GetUserName()
        {
            string userName = "";

            SPWeb web = SPContext.Current.Web;
            SPUser user = web.CurrentUser;
            SPRoleDefinitionBindingCollection usersRoles = web.AllRolesForCurrentUser;
            SPRoleDefinitionCollection siteRoleCollection = web.RoleDefinitions;
            SPRoleDefinition roleDefinition = siteRoleCollection["完全控制"];

            SPRoleDefinition roleDefinition2 = siteRoleCollection["参与讨论"];

            if (user != null)//当前有用户登录
            {
                //if (usersRoles.Contains(roleDefinition) && Page.Request.QueryString["Code"] != null)
                //{
                //    //具有完全控制权限
                //    userName = Page.Request.QueryString["Code"];
                //}
                if ((GetGroupofUser("辽宁省数学建模竞赛组委会")||usersRoles.Contains(roleDefinition)||usersRoles.Contains(roleDefinition2))&& Page.Request.QueryString["Code"] != null)
                    //已登录的用户是组委会成员且url中传递了EnrollCode,则查看制定EnrollCode的报名信息
                {
                    userName = Page.Request.QueryString["Code"];
                }
                else
                {
                    userName = user.LoginName;
                    if (userName.Contains("|"))
                    {
                        userName = userName.Substring(userName.IndexOf("|") + 1);
                    }
                    if (userName.Contains("\\"))
                    {
                        userName = userName.Substring(userName.IndexOf("\\") + 1);
                    }
                }
            }
            return userName;
        }


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


        private void BindWorks(string enrollCode)
        {

        }

        private int BindEnroll(string enrollCode)
        {
            int result = 0;
            DataTable dt= BLL.Enroll.GetEnrollByEnrollCode(enrollCode);
            if (dt.Rows.Count>0)
            {
                string eCode = dt.Rows[0]["EnrollCode"].ToString();
                string orgCode=dt.Rows[0]["Org"].ToString();
                orgCode =BLL.Enroll.GetOrgNameByOrgCode(orgCode);
                string email=dt.Rows[0]["Email"].ToString();
                string eDate=dt.Rows[0]["Created"].ToString();
                divEInfo.InnerHtml = @"<h2>报名序号：" + eCode+"</h2><h3>培养单位："+orgCode+"</h3><h3>报名邮箱："+email+"</h3><h3>报名时间："+eDate+"</h3>";
                result = (int)dt.Rows[0]["Flag"];
            }
            return result;
        }

        private void BindMembers(string enrollCode)
        {
            DataTable dt = BLL.Enroll.GetMemberByEnrollCode(enrollCode);
            gvMembers.DataSource = dt;

            gvMembers.DataBind();
            if (dt.Rows.Count >= 3)
            {
                lnkNewMember.Visible = false;
            }
            else
            {
                lnkNewMember.Visible = true;
                lnkNewMember.PostBackUrl = "MemberInfo.aspx?ID=0&Code=" + enrollCode + "&Source=/LNMCM/_layouts/15/lnmcm/MyEnroll.aspx";
            }
        }

        /// <summary>
        /// 团队成员行操作事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMembers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                String Id = e.CommandArgument.ToString();
                DataTable dt = BLL.Enroll.GetMemberById(Id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    dr["Flag"] = 0;
                    DAL.User.DelMember(dr);
                }
                //SPUser user = SPContext.Current.Web.CurrentUser;
                string enrollCode = GetUserName();
                BindMembers(enrollCode);
            }
        }
    }
}
