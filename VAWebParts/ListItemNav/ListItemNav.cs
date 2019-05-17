using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace VAWebParts.ListItemNav
{
    [ToolboxItemAttribute(false)]
    public class ListItemNav : WebPart
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/VAWebParts/ListItemNav/ListItemNavUserControl.ascx";

        protected override void CreateChildControls()
        {
            ListItemNavUserControl control = Page.LoadControl(_ascxPath) as ListItemNavUserControl;
            if (control != null)
                control.WebPartObj = this;
            Controls.Add(control);
        }
        string listName = "新闻公告";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("列表标题")]
        [WebDescription("要操作的列表的标题")]
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
    }
}
