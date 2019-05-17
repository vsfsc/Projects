using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PowerPA.CustNewForm
{
    [ToolboxItemAttribute(false)]
    public class CustNewForm : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PowerPA/CustNewForm/CustNewFormUserControl.ascx";

        protected override void CreateChildControls()
        {
            CustNewFormUserControl control = Page.LoadControl(_ascxPath) as CustNewFormUserControl;
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
        /// 列表字段
        /// </summary>
        private string _listFields = "对象;计划开始;计划时长;实际开始;实际时长";
        [Personalizable, WebDisplayName("列表字段"), WebDescription("表单中的列表字段，如：标题；..."), WebBrowsable, Category("自定义设置")]
        public string ListFields
        {
            get
            {
                return this._listFields;
            }
            set
            {
                this._listFields = value;
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

        private string _logList = "例行计划生成历史";
        [Personalizable, WebDisplayName("例行计划生成历史"), WebDescription("例行计划生成历史列表名称，如：个人学习助手"), WebBrowsable, Category("自定义设置")]
        public string LogList
        {
            get
            {
                return this._logList;
            }
            set
            {
                this._logList = value;
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

        private int _minValue = 1;
        [Personalizable, WebDisplayName("最小生成天数"), WebDescription("一次生成数据的最小天数，如：1"), WebBrowsable, Category("自定义设置")]
        public int MinValue
        {
            get
            {
                return this._minValue;
            }
            set
            {
                this._minValue = value;
            }
        }


        private int _maxValue = 7;
        [Personalizable, WebDisplayName("最大生成天数"), WebDescription("一次生成数据的最大天数，如：7"), WebBrowsable, Category("自定义设置")]
        public int MaxValue
        {
            get
            {
                return this._maxValue;
            }
            set
            {
                this._maxValue = value;
            }
        }

        private int _colCount = 4;
        [Personalizable, WebDisplayName("列数"), WebDescription("多选框选项分布列数，如：4"), WebBrowsable, Category("自定义设置")]
        public int ColCount
        {
            get
            {
                return this._colCount;
            }
            set
            {
                this._colCount = value;
            }
        }

        #endregion
    }
}
