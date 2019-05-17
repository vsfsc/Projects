using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MyTasks.MyTasksUserControl
{
    [ToolboxItemAttribute(false)]
    public class MyTasksUserControl : WebPart
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/MyTasks/MyTasksUserControl/MyTasksUserControlUserControl.ascx";

        protected override void CreateChildControls()
        {
            //Control control = Page.LoadControl(_ascxPath);
            MyTasksUserControlUserControl control = Page.LoadControl(_ascxPath) as MyTasksUserControlUserControl;
            if (control != null)
                control.WebPartObj = this;
            Controls.Add(control);
        }
        #region 属性

        /// <summary>
        /// 任务列表名称
        /// </summary>
        string listName = "Tasks;Book2;Book3;Book4";
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("列表名称")]
        [WebDescription("项目网站中遍历所有任务列表名称，以“;”隔开")]
        public string ListName
        {
            get { return listName; }
            set { listName = value; }
        }
        #endregion
    }
}
