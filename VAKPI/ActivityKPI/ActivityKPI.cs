using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace VAKPI.ActivityKPI
{
    [ToolboxItemAttribute(false)]
    public class ActivityKPI : WebPart
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/VAKPI/ActivityKPI/ActivityKPIUserControl.ascx";

        protected override void CreateChildControls()
        {
            //Control control = Page.LoadControl(_ascxPath);
            ActivityKPIUserControl control = Page.LoadControl(_ascxPath) as ActivityKPIUserControl;
            if (control != null)
                control.WebPartObj = this;
            Controls.Add(control);
        }
        #region 属性
        string subWebUrl = "Blogs";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("用来统计博客的内容，参数为空则统计当前网站下面的")]
        [WebDescription("")]
        public string SubWebUrl
        {
            get
            {
                return subWebUrl;
            }
            set
            {
                subWebUrl = value;
            }
        }
        string listName = "讨论列表;文档;Posts;新闻公告";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("对当前网站的列表统计，各列表之间用分号隔开")]
        [WebDescription("")]
        public string ListName
        {
            get
            {
                return listName;
            }
            set
            {
                listName = value;
            }
        }
        #endregion
    }
}
