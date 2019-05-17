using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MyTasks.NewActivity
{
    [ToolboxItemAttribute(false)]
    public class NewActivity : WebPart
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/MyTasks/NewActivity/NewActivityUserControl.ascx";

        protected override void CreateChildControls()
        {
            NewActivityUserControl control = Page.LoadControl(_ascxPath) as NewActivityUserControl;
            if (control != null)
                control.objWeb = this;
            Controls.Add(control);
        }
        #region 参数
        double _days = 7;
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("时间前后延长的天数")]
        [WebDescription("")]
        [Category("设置")]
        public double Days
        {
            get { return _days; }
            set { _days = value; }
        }
        double _hours = 0.5 ;
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("时间前后延长的小时数")]
        [WebDescription("")]
        [Category("设置")]
        public double  Hours
        {
            get { return _hours; }
            set { _hours = value; }
        }
        /// <summary>
        /// 活动列表名称
        /// </summary>
        string lstName = "活动";
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("写入活动列表名称")]
        [WebDescription("写入活动所在列表")]
        [Category("设置")]
        public string ListName
        {
            get { return lstName; }
            set { lstName = value; }
        }
        /// <summary>
        /// 活动列表名称
        /// </summary>
        string actionlstName = "操作";
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("写入操作列表名称")]
        [WebDescription("写入操作所在列表")]
        [Category("设置")]
        public string ActionListName
        {
            get { return actionlstName; }
            set { actionlstName = value; }
        }
        #endregion
    }
}
