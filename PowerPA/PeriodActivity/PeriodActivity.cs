using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PowerPA.PeriodActivity
{
    [ToolboxItemAttribute(false)]
    public class PeriodActivity : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PowerPA/PeriodActivity/PeriodActivityUserControl.ascx";

        protected override void CreateChildControls()
        {
            PeriodActivityUserControl control = Page.LoadControl(_ascxPath) as PeriodActivityUserControl;
            if (control!=null)
            {
                control.webObj = this;
            }
            Controls.Add(control);
        }

        #region 自定义设置
        /// <summary>
        /// 日历列表名称
        /// </summary>
        private string _listName = "周期活动日历";
        [Personalizable, WebDisplayName("日历名称"), WebDescription("列表名称，如：日历"), WebBrowsable, Category("自定义设置")]
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

        private string _webUrl = "http://ws2108：19568";
        [Personalizable, WebDisplayName("列表所在网站地址"), WebDescription("列表名称，如：http://ws2018"), WebBrowsable, Category("自定义设置")]
        public string WebUrl
        {
            get
            {
                return this._webUrl;
            }
            set
            {
                this._webUrl = value;
            }
        }

        #endregion
    }
}
