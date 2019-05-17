using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SpWebpart.ChangeRibbonSelection
{
    [ToolboxItemAttribute(false)]
    public class ChangeRibbonSelection : WebPart
    {
        protected override void CreateChildControls()
        {
            //SPSecurity.RunWithElevatedPrivileges(delegate()
            //{
                SPRibbon current = SPRibbon.GetCurrent(this.Page);
                if (current != null)
                {
                    current.MakeTabAvailable("Ribbon.Read");
                    current.InitialTabId = "Ribbon.Read";
                    current.Minimized = true;
                }
            //});
            //current.TrimById("Ribbon.ListForm.Display");
            //SPRibbonScriptManager manager = new SPRibbonScriptManager();
            //List<IRibbonCommand> commands = new List<IRibbonCommand>();
            //bool admin = base.GlobalAdmin.IsCurrentUserMachineAdmin();
            //commands.Add(new SPRibbonCommand("WebAppTab"));
            //        <script type='txt/javascript'>
            //_spBodyOnLoadFunctionNames.push("InitTab");
            //function InitTab()
            //{
            //InitializeTab("Ribbon.Document");
            //}
            //</script>
        }

        private int GetUserID()
        {
            SPUser user = SPContext.Current.Web.CurrentUser;
            if (user!=null)
            {
                return user.ID;
            }
            else
            {
                return 0;
            }
        }

        public static bool IsCurrentUserInGroup(string groupName)
        {
            SPGroup group = null;
            bool result = false;
            SPWeb web = SPContext.Current.Web;
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                try
                {
                    group = web.Groups[groupName];
                }
                catch (Exception ex)
                {
                    //Helper.WriteException(ex, "IsCurrentUserInGroup");
                    group = null;
                    try
                    {
                        group = web.SiteGroups[groupName];
                    }
                    catch (Exception innerEx)
                    {
                        //Helper.WriteException(innerEx, "IsCurrentUserInGroup");
                        group = null;
                    }
                }
            }

            if (null == group)
            {
                result = false;
            }
            else
            {
                result = web.IsCurrentUserMemberOfGroup(group.ID) || group.ContainsCurrentUser;
            }

            return result;
        }

        #region 属性
        /// <summary>
        /// 网站的总访问次数
        /// </summary>
        private string _groupName = "位成员";
        [Personalizable, WebDisplayName("组名"), WebDescription("SharePoint用户组名，如：网站成员"), WebBrowsable, Category("自定义设置")]
        private string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
            }
        }
        #endregion
    }
}
