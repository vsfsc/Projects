using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Reflection;

namespace PowerPA.ActivityStatistics
{
    [ToolboxItemAttribute(false)]
    public class ActivityStatistics : WebPart,IWebPartTable
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PowerPA/ActivityStatistics/ActivityStatisticsUserControl.ascx";

        protected override void CreateChildControls()
        {
            ActivityStatisticsUserControl control = Page.LoadControl(_ascxPath) as ActivityStatisticsUserControl;
            if (control != null)
                control.webObj  = this;
            Controls.Add(control);
        }
        #region 属性
        /// <summary>
        /// 活动所在的列表名称
        /// </summary>
        private string _listTitle = "活动";
        /// <summary>
        /// 活动列表名称
        /// </summary>
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("列表标题")]
        [WebDescription("指定要统计的列表的标题")]
        public string ListTitle
        {
            set { _listTitle = value; }
            get { return _listTitle; }
        }
        private int _totalDays = 7;
        /// <summary>
        ///  默认统计的天数
        /// </summary>
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("默认统计的天数")]
        [WebDescription("默认统计的天数")]
        public int StatisDays
        {
            set { _totalDays = value; }
            get { return _totalDays; }
        }
        private string _typeList = "用户-操作";
        /// <summary>
        /// 我的操作列表
        /// </summary>
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
        
        private string _actionList = "操作";
        /// <summary>
        /// 操作列表名称
        /// </summary>
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
        private string _groupField="";
        /// <summary>
        /// 列表统计字段
        /// </summary>
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("分组字段")]
        [WebDescription("按指定的字段对数据进行分组")]
        public string GroupField
        {
            set { _groupField = value; }
            get { return _groupField; }
        }

        /// <summary>
        /// 显示类型：-1、仅显示最新五条；0、仅显示数据统计；1、两者均显示；
        /// </summary>
        private int _displayType;
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("显示项（整数）")]
        [WebDescription("指定要显示的数据项：-1、仅显示最新五条；0、仅显示数据统计；1、两者均显示")]
        public int DisplayType
        {
            set { _displayType = value; }
            get { return _displayType; }
        }
        private DateTime _dtFrom = DateTime.Now.Date .AddDays (1);
        /// <summary>
        /// 统计的日期的开始
        /// </summary>
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("统计的日期的开始")]
        [WebDescription("统计的日期的开始")]
        public DateTime DateFrom
        {
            get { return _dtFrom; }
            set { _dtFrom = value; }
        }
        private DateTime _dtTo=DateTime.Today.Date ;
        /// <summary>
        /// 统计的日期的结束
        /// </summary>
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("统计的日期的结束")]
        [WebDescription("统计的日期的结束")]
        public DateTime DateTo
        {
            get { return _dtTo; }
            set { _dtTo = value; }
        }
        #endregion
        #region 获取活动操作的二维表格值
        /// <summary>
        /// 获取我的活动操作数据
        /// </summary>
        /// <returns></returns>
        private void ChartTableData()
        {
            if (DateFrom == DateTime.Today.AddDays(1))
                DateFrom = DateTime.Today.AddDays(-StatisDays);
            _table = ActivityStatisticsUserControl.GetActivities(0, ListTitle, DateFrom, DateTo);
        }
        #endregion
        #region 传值
        DataTable _table;
        public PropertyDescriptorCollection Schema
        {
            get
            {
                if (_table.DefaultView.Count > 0)
                    return TypeDescriptor.GetProperties(_table.DefaultView[0]);
                else
                    return TypeDescriptor.GetProperties(_table.DefaultView);
            }
        }


        public void GetTableData(TableCallback callback)
        {
            ChartTableData();
            callback(_table.Rows);
        }

        public bool ConnectionPointEnabled
        {
            get
            {
                object o = ViewState["ConnectionPointEnabled"];
                return (o != null) ? (bool)o : true;
            }
            set
            {
                ViewState["ConnectionPointEnabled"] = value;
            }
        }

        [ConnectionProvider("Table", typeof(TableProviderConnectionPoint),
      AllowsMultipleConnections = true)]
        public IWebPartTable GetConnectionInterface()
        {
            return this;// new PersonalStastics();
        }

        public class TableProviderConnectionPoint : ProviderConnectionPoint
        {
            public TableProviderConnectionPoint(MethodInfo callbackMethod,
        Type interfaceType, Type controlType, string name, string id,
        bool allowsMultipleConnections)
                : base(callbackMethod, interfaceType, controlType, name, id,
                  allowsMultipleConnections)
            {
            }

            public override bool GetEnabled(Control control)
            {
                return ((ActivityStatistics)control).ConnectionPointEnabled;
            }

        }
        #endregion
    }
}
