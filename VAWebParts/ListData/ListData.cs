using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace VAWebParts.ListData
{
    [ToolboxItemAttribute(false)]
    public class ListData : WebPart
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/VAWebParts/ListData/ListDataUserControl.ascx";

        protected override void CreateChildControls()
        {
            //Control control = Page.LoadControl(_ascxPath);
            //Controls.Add(control);
            ListDataUserControl control = Page.LoadControl(_ascxPath) as ListDataUserControl;
            if (control != null)
                control.WebPartObj= this;
            Controls.Add(control);
        }
        /// <summary>
        /// 列表标题
        /// </summary>
        private string _listTitle;
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("列表标题")]
        [WebDescription("指定要统计的列表的标题")]
        public string ListTitle
        {
            set { _listTitle = value; }
            get { return _listTitle; }
        }

        /// <summary>
        /// 列表统计字段
        /// </summary>
        private string _groupField;
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
    }
}
