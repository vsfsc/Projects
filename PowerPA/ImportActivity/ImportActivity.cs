using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PowerPA.ImportActivity
{
    [ToolboxItemAttribute(false)]
    public class ImportActivity : WebPart
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PowerPA/ImportActivity/ImportActivityUserControl.ascx";

        protected override void CreateChildControls()
        {
            ImportActivityUserControl control = Page.LoadControl(_ascxPath) as ImportActivityUserControl;
            if (control != null)
                control.webObj = this;
            Controls.Add(control);
        }
        #region "参数"
        private string _TempfileName = "VS导出模板.xlsm";
        [WebDisplayName("模版文件名")]
        [WebDescription(" ")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public string ExcelTempfile
        {
            set { _TempfileName = value; }
            get { return _TempfileName; }
        }
        private string _fileName = "VS.xlsm";
        [WebDisplayName("导出到Excel默认的文件名")]
        [WebDescription("默认的文件名")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public string ExcelFilename
        {
            set { _fileName = value; }
            get { return _fileName; }
        }
        private string exportLists = "项目计划；活动";
        [WebDisplayName("导入导出的工作表名称")]
        [WebDescription(" ")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public string ExportLists
        {
            set { exportLists = value; }
            get { return exportLists; }
        }
        private int repeartColumn = 3;
        [WebDisplayName("重复几列")]
        [WebDescription(" ")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public int RepeatCol
        {
            set { repeartColumn = value; }
            get { return repeartColumn; }
        }
        private int beforeYears = 3;
        [WebDisplayName("几年前的业绩")]
        [WebDescription(" ")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public int BeforeYears
        {
            set { beforeYears = value; }
            get { return beforeYears; }
        }
        private short fontsize = 10;
        [WebDisplayName("导出的字体大小")]
        [WebDescription(" ")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public short FontSize
        {
            set { fontsize = value; }
            get { return fontsize; }
        }
        private string noDataMsg = "没有要导出的数据！";
        [WebDisplayName("没有数据提示文本")]
        [WebDescription(" ")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public string NoDataMsg
        {
            set { noDataMsg = value; }
            get { return noDataMsg; }
        }
        #endregion
    }
}
