using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace VAListTree.TreeStructureUserControl
{
    [ToolboxItemAttribute(false)]
    public class TreeStructureUserControl : WebPart
    {

        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/VAListTree/TreeStructureUserControl/TreeStructureUserControlUserControl.ascx";

        protected override void CreateChildControls()
        {
            //Control control = Page.LoadControl(_ascxPath);
            TreeStructureUserControlUserControl control = Page.LoadControl(_ascxPath) as TreeStructureUserControlUserControl;
            if (control != null)
                control.WebPartObj = this;
            Controls.Add(control);
        }
        #region 属性
        string listName = "Mytask";
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("列表名称")]
        [WebDescription("")]
        public string ListName
        {
            get { return listName; }
            set { listName = value; }
        }
        string parentField = "ParentTask";
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("父级字段名称")]
        [WebDescription("")]
        public string ParentField
        {
            get { return parentField; }
            set { parentField = value; }
        }
        string subField = "Title";
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("子字段名称")]
        [WebDescription("")]
        public string SubField
        {
            get { return subField; }
            set { subField = value; }
        }
        long listID=0;
        public long ListID
        {
            get { return listID; }
            set { listID  = value ; }
        }
       [ConnectionProvider("return the list ID")]
        public long ReturnTreeNodeID()
        {
            return ListID;
        }
        #endregion
      
    }
}
