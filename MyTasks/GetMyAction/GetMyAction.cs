using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MyTasks.GetMyAction
{
    [ToolboxItemAttribute(false)]
    public class GetMyAction : WebPart
    {
        // 更改可视 Web 部件项目项后，Visual Studio 可能会自动更新此路径。
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/MyTasks/GetMyAction/GetMyActionUserControl.ascx";

        protected override void CreateChildControls()
        {
            GetMyActionUserControl control = Page.LoadControl(_ascxPath) as GetMyActionUserControl;
            if (control!=null)
            {
                control.wpObj = this;
            }
            Controls.Add(control);
        }

        #region 参数
        /// <summary>
        /// 来源列表名称
        /// </summary>
        string fromList = "我的操作";
        [Personalizable(true)]
        [WebBrowsable(true)]
        [WebDisplayName("来源列表名称")]
        [WebDescription("选项来源列表")]
        [Category("设置")]
        public string FromList
        {
            get { return fromList; }
            set { fromList = value; }
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
        public string ToList
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
        #endregion
    }
}
