using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace VAKPI.SelfQuantified
{
    [ToolboxItemAttribute(false)]
    public class SelfQuantified : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/VAKPI/SelfQuantified/SelfQuantifiedUserControl.ascx";

        protected override void CreateChildControls()
        {
            SelfQuantifiedUserControl control = Page.LoadControl(_ascxPath) as SelfQuantifiedUserControl;
            if (control != null)
            {
                control.webObj = this;
            }
            Controls.Add(control);
        }

        #region 自定义设置


        private string _planList = "个人学习助手";
        [Personalizable, WebDisplayName("助手列表名称"), WebDescription("个人学习助手列表名称，如：个人学习助手"), WebBrowsable, Category("自定义设置")]
        public string PlanList
        {
            get
            {
                return this._planList;
            }
            set
            {
                this._planList = value;
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

        private string _resultList = "活动结果";
        [Personalizable, WebDisplayName("活动结果列表名称"), WebDescription("活动结果列表名称，如：活动结果"), WebBrowsable, Category("自定义设置")]
        public string ResultList
        {
            get
            {
                return this._resultList;
            }
            set
            {
                this._resultList = value;
            }
        }
        private string _actionList = "操作";
        [Personalizable, WebDisplayName("操作列表名称"), WebDescription("操作列表名称，如：操作"), WebBrowsable, Category("自定义设置")]
        public string ActionList
        {
            get
            {
                return this._actionList;
            }
            set
            {
                this._actionList = value;
            }
        }

        private string _actionTypeList = "操作类别";
        [Personalizable, WebDisplayName("操作类别列表名称"), WebDescription("操作类别列表名称，如：操作类别"), WebBrowsable, Category("自定义设置")]
        public string ActionTypeList
        {
            get
            {
                return this._actionTypeList;
            }
            set
            {
                this._actionTypeList = value;
            }
        }

        private string _siteUrl = "http://ws2018:19568/";
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

        #endregion
    }
}
