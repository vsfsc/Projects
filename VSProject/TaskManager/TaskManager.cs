using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace VSProject.TaskManager
{
    [ToolboxItemAttribute(false)]
    public class TaskManager : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/VSProject/TaskManager/TaskManagerUserControl.ascx";

        protected override void CreateChildControls()
        {
            TaskManagerUserControl control = Page.LoadControl(_ascxPath) as TaskManagerUserControl;
            if (control != null)
            {
                control.webObj = this;
            }
            Controls.Add(control);
        }

        #region 自定义设置
        private int _titleLength =10;
        [Personalizable, WebDisplayName("标题字数"), WebDescription("超长标题最多显示的字数"), WebBrowsable, Category("自定义设置")]
        public int TitleLength
        {
            get
            {
                return this._titleLength;
            }
            set
            {
                this._titleLength = value;
            }
        }
        #endregion
    }
}
