using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PowerPA.ShowBlogByCategory
{
    [ToolboxItemAttribute(false)]
    public class ShowBlogByCategory : WebPart
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PowerPA/ShowBlogByCategory/ShowBlogByCategoryUserControl.ascx";

        protected override void CreateChildControls()
        {
            ShowBlogByCategoryUserControl  control = Page.LoadControl(_ascxPath) as ShowBlogByCategoryUserControl ;
            if (control != null)
                control.webObj = this;
            Controls.Add(control);
        }
        #region 自定义设置
        /// <summary>
        /// 
        /// </summary>
        private string _blogList = "项目任务";
        [Personalizable, WebDisplayName("博客列表名称"), WebDescription("显示分类下的博文"), WebBrowsable, Category("自定义设置")]
        public string BlogList
        {
            get
            {
                return this._blogList;
            }
            set
            {
                this._blogList = value;
            }
        }
        #endregion
    }
}
