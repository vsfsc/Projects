using System;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MyTasks.TotalMyActions
{
    [ToolboxItemAttribute(false)]
    public class TotalMyActions : WebPart,IWebPartTable
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/MyTasks/TotalMyActions/TotalMyActionsUserControl.ascx";

        protected override void CreateChildControls()
        {
            TotalMyActionsUserControl  control = Page.LoadControl(_ascxPath) as TotalMyActionsUserControl;
            if (control != null)
                control.webObj = this;
            Controls.Add(control);
        }

        #region 参数
        /// <summary>
        /// 进行汇总的天数，从当前日期开始
        /// </summary>
        int _totalDays = 7;
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("进行汇总的天数")]
        [WebDescription("进行汇总的天数")]
        [Category("设置")]
        public int  TotalDays
        {
            get { return _totalDays; }
            set { _totalDays = value; }
        }

        /// <summary>
        /// 写入列表名称
        /// </summary>
        string toList = "个人学习助手";
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("写入列表名称")]
        [WebDescription("写入选项所在列表")]
        [Category("设置")]
        public string ListName
        {
            get { return toList; }
            set { toList = value; }
        }

        /// <summary>
        /// 子网站
        /// </summary>
        string subWebUrl = "";
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("子网站")]
        [WebDescription("子网站地址")]
        [Category("设置")]
        public string SubWebUrl
        {
            get { return subWebUrl; }
            set { subWebUrl = value; }
        }
        public DataTable ChartData
        {
            get { return _table; }
            set { _table = value; }
        }
        #endregion
        #region 传值 
        DataTable _table;
        private void ChartTableData()
        {
            //TotalMyActionsUserControl my = new TotalMyActionsUserControl();
            //DateTime dtFrom = my.dtFrom;
            //DateTime dtTo = my.dtTo;
            //int byDate = my.SearchByDate;
            //_table = TotalMyActionsUserControl.GetDataTable(0 , ListName,DateTime.Today.AddDays(-TotalDays)  ,DateTime.Today   );
        }
        #endregion
        #region 传值
        public PropertyDescriptorCollection Schema
        {
            get
            {
                return TypeDescriptor.GetProperties(_table.DefaultView[0]);
            }
        }

        public void GetTableData(TableCallback callback)
        {
            ChartTableData();
            if (_table != null)
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
                return ((TotalMyActions)control).ConnectionPointEnabled;
            }

        }
        #endregion
    }
    
}
