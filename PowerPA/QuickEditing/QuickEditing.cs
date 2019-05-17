using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PowerPA.QuickEditing
{
    [ToolboxItemAttribute(false)]
    public class QuickEditing : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PowerPA/QuickEditing/QuickEditingUserControl.ascx";

        protected override void CreateChildControls()
        {
            QuickEditingUserControl control = Page.LoadControl(_ascxPath) as QuickEditingUserControl;
            if (control != null)
            {
                control.webObj = this;
            }
            Controls.Add(control);
        }


        #region 自定义设置
        /// <summary>
        /// 活动列表名称
        /// </summary>
        private string _activityList = "活动";
        [Personalizable, WebDisplayName("活动列表名称"), WebDescription("录入数据的列表名称，如：活动"), WebBrowsable, Category("自定义设置")]
        public string ActivityList
        {
            get
            {
                return this._activityList;
            }
            set
            {
                this._activityList = value;
            }
        }
        private string _ListMediaRel = "活动媒体";
        /// <summary>
        /// 活动附件关系库
        /// </summary>
        [WebDisplayName("活动附件关系库")]
        [WebDescription("活动附件关系库")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public string ListMediaRel
        {
            set { _ListMediaRel = value; }
            get { return _ListMediaRel; }
        }
        private string _ListMediaLib = "活动媒体库";
        /// <summary>
        /// 活动附件库
        /// </summary>
        [WebDisplayName("活动图片所在的列表")]
        [WebDescription("活动图片所在的列表")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public string ListMediaLib
        {
            set { _ListMediaLib = value; }
            get { return _ListMediaLib; }
        }
        /// <summary>
        /// 我的操作列表
        /// </summary>
        private string _typeList = "用户-操作";
        [Personalizable, WebDisplayName("我的操作"), WebDescription("我的个性化操作"), WebBrowsable, Category("自定义设置")]
        public string MyActionList
        {
            get
            {
                return this._typeList;
            }
            set
            {
                this._typeList = value;
            }
        }

        /// <summary>
        /// 操作列表名称
        /// </summary>
        private string _actionList = "操作";
        [Personalizable, WebDisplayName("操作列表名称"), WebDescription("活动关联的操作列表的名称，如：操作"), WebBrowsable, Category("自定义设置")]
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
        private string _actionType = "操作-类型";
        /// <summary>
        /// 操作类型关系表的名称
        /// </summary>
        [Personalizable, WebDisplayName("操作列表名称"), WebDescription("活动关联的操作列表的名称，如：操作"), WebBrowsable, Category("自定义设置")]
        public string ActionType
        {
            get
            {
                return this._actionType;
            }
            set
            {
                this._actionType = value;
            }
        }
        private int _quickEdit = 1;
        /// <summary>
        /// 1：快速批量编辑；0：单条录入或编辑
        /// </summary>
        [Personalizable, WebDisplayName("编辑类型"), WebDescription("1：快速批量编辑；0：单条录入或编辑"), WebBrowsable, Category("自定义设置")]
        public int QuickEdit
        {
            get
            {
                return this._quickEdit;
            }
            set
            {
                this._quickEdit = value;
            }
        }


        private string _calendarList = "日历";
        [Personalizable, WebDisplayName("日历列表的名称"), WebDescription(""), WebBrowsable, Category("自定义设置")]
        public string  CalendarList
        {
            get
            {
                return this._calendarList;
            }
            set
            {
                this._calendarList = value;
            }
        }


        private int _actionCount =15;
        /// <summary>
        /// 预读操作数目,缺省为：15
        /// </summary>
        [Personalizable, WebDisplayName("预读操作数"), WebDescription("预读操作数目,缺省为：15"), WebBrowsable, Category("自定义设置")]
        public int ActionCount
        {
            get
            {
                return this._actionCount;
            }
            set
            {
                this._actionCount = value;
            }
        }
        private int _subDays = 7;
        /// <summary>
        /// 活动前推的天数,缺省为：一周
        /// </summary>
        [Personalizable, WebDisplayName("活动前推的天数"), WebDescription("活动前推的天数,缺省为：7"), WebBrowsable, Category("自定义设置")]
        public int BeforeDays
        {
            get
            {
                return this._subDays;
            }
            set
            {
                this._subDays = value;
            }
        }
        private string _userDesp = "点击保存，保存修改或新建的活动。";
        /// <summary>
        /// 批量录入使用说明信息
        /// </summary>
        [Personalizable, WebDisplayName("说明"), WebDescription("使用说明信息"), WebBrowsable, Category("自定义设置")]
        public string UserDesp
        {
            get
            {
                return this._userDesp;
            }
            set
            {
                this._userDesp = value;
            }
        }
        private string _responseUrl = "#";
        /// <summary>
        /// 如果日期改变后当日是明天时，则进入活动分析界面所在的url
        /// </summary>
        [Personalizable, WebDisplayName("说明"), WebDescription("使用说明信息"), WebBrowsable, Category("自定义设置")]
        public string responseUrl
        {
            get
            {
                return this._responseUrl;
            }
            set
            {
                this._responseUrl = value;
            }
        }
        private string _title = "活动录入";
        /// <summary>
        /// 标题信息
        /// </summary>
        [Personalizable, WebDisplayName("标题信息"), WebDescription(" "), WebBrowsable, Category("自定义设置")]
        public string ShowTitle
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }
        private string _nonWorkday = "休息日";
        /// <summary>
        /// 标题信息
        /// </summary>
        [Personalizable, WebDisplayName("非工作日标题"), WebDescription(" "), WebBrowsable, Category("自定义设置")]
        public string NonWorkday
        {
            get
            {
                return this._nonWorkday;
            }
            set
            {
                this._nonWorkday = value;
            }
        }
        #endregion
    }
}
