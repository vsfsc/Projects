using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PowerPA.CurrentActivity
{
    [ToolboxItemAttribute(false)]
    public class CurrentActivity : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PowerPA/CurrentActivity/CurrentActivityUserControl.ascx";

        protected override void CreateChildControls()
        {
            CurrentActivityUserControl control = Page.LoadControl(_ascxPath) as CurrentActivityUserControl;
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
        private string _listName = "个人学习助手";
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
        /// <summary>
        /// 网站地址
        /// </summary>
        private string _siteUrl = "http://localhost";
        [Personalizable, WebDisplayName("网站地址"), WebDescription("网站地址，如：http://localhost"), WebBrowsable, Category("自定义设置")]
        public string SiteUrl
        {
            get
            {
                return this._siteUrl;
            }
            set
            {
                this._siteUrl = value;
            }
        }

        /// <summary>
        /// 调整时间小时数
        /// </summary>
        private int _seconds = 10;
        [Personalizable, WebDisplayName("计时器秒数"), WebDescription("计时器秒数，如：1"), WebBrowsable, Category("自定义设置")]
        public int Seconds
        {
            get
            {
                return this._seconds;
            }
            set
            {
                this._seconds = value;
            }
        }

        /// <summary>
        /// 调整时间小时数
        /// </summary>
        private string _isScroll ="是" ;
        [Personalizable, WebDisplayName("是否滚动"), WebDescription("是否滚动：是/否"), WebBrowsable, Category("自定义设置")]
        public string IsScroll
        {
            get
            {
                return this._isScroll;
            }
            set
            {
                this._isScroll = value;
            }
        }
        #endregion
    }
}
