using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace VADocumentOnline.DocDownload
{
    [ToolboxItemAttribute(false)]
    public class DocDownload : WebPart
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/VADocumentOnline/DocDownload/DocDownloadUserControl.ascx";

        protected override void CreateChildControls()
        {
            DocDownloadUserControl control = Page.LoadControl(_ascxPath) as DocDownloadUserControl ;
            if (control != null)
                control.webObj = this;
            Controls.Add(control);
        }
        #region "参数"
        private string _ListName = "图像";
        [WebDisplayName("文档库名称")]
        [WebDescription("要下载的文档库名称")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public string ListName
        {
            set { _ListName = value; }
            get { return _ListName; }
        }
        private string subWebName = "blog";
        [WebDisplayName("导入哪个子网站的列表")]
        [WebDescription("子网站Url,空为要网站")]
        [Personalizable(true)]
        [WebBrowsable(true)]
        [Category("设置")]
        public string WebUrl
        {
            set { subWebName = value; }
            get { return subWebName; }
        }

        #endregion
    }
}
