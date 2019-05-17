using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace VAKPI.GoalSetting
{
    [ToolboxItemAttribute(false)]
    public class GoalSetting : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/VAKPI/GoalSetting/GoalSettingUserControl.ascx";

        protected override void CreateChildControls()
        {
            GoalSettingUserControl control = Page.LoadControl(_ascxPath) as GoalSettingUserControl;
            if (control != null)
            {
                control.webObj = this;
            }
            Controls.Add(control);
        }
        #region 属性
        /// <summary>
        /// 修改说明
        /// </summary>
        private string  _des = "目标值修改时间间隔为1个月";
        public string  ChangeDes
        {
            get
            {
                return this._des;
            }
            set
            {
                this._des = value;
            }
        }
        /// <summary>
        /// 修改指标值的时间间隔
        /// </summary>
        private int _months=1;
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("修改指标值的时间间隔，以月为单位")]
        [WebDescription("")]
        [Category("自定义设置")]
        public int monthsInternal
        {
            get
            {
                return this._months;
            }
            set
            {
                this._months = value;
            }
        }
        private string _siteUrl = "http://xqx2012/blog";
        [Personalizable, WebDisplayName("网址"), WebDescription("网站地址，如：http://va.neu.edu.cn"), WebBrowsable, Category("自定义设置")]
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
        private string _constraintlist = "约束条件";
        [Personalizable, WebDisplayName("约束条件列表名称"), WebDescription("约束条件列表名称，如：约束条件"), WebBrowsable, Category("自定义设置")]
        public string ConstraintList
        {
            get
            {
                return this._constraintlist;
            }
            set
            {
                this._constraintlist = value;
            }
        }
        #endregion
    }
}
