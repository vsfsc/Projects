using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PowerPA.TimeLinePerDay
{
    [ToolboxItemAttribute(false)]
    public class TimeLinePerDay : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PowerPA/TimeLinePerDay/TimeLinePerDayUserControl.ascx";

        protected override void CreateChildControls()
        {
            TimeLinePerDayUserControl control = Page.LoadControl(_ascxPath) as TimeLinePerDayUserControl;
            if (control!=null)
            {
                control.webObj = this;
            }
            Controls.Add(control);
        }

        #region 自定义设置
        /// <summary>
        /// 列表名称
        /// </summary>
        private string _listName = "任务文档";
        [Personalizable, WebDisplayName("列表名称"), WebDescription("列表名称，如：个人学习助手"), WebBrowsable, Category("自定义设置")]
        public string ListName
        {
            get
            {
                return this._listName;
            }
            set
            {
                this._listName = value;
            }
        }

        private string _childList = "活动";
        [Personalizable, WebDisplayName("关联子表名称"), WebDescription("列表名称，如：活动"), WebBrowsable, Category("自定义设置")]
        public string ChildList
        {
            get
            {
                return this._childList;
            }
            set
            {
                this._childList = value;
            }
        }

        #endregion
    }
}
