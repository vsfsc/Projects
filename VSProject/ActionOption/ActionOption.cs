using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace VSProject.ActionOption
{
    [ToolboxItemAttribute(false)]
    public class ActionOption : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/VSProject/ActionOption/ActionOptionUserControl.ascx";



        protected override void CreateChildControls()
        {
            ActionOptionUserControl control = Page.LoadControl(_ascxPath) as ActionOptionUserControl;
            if (control != null)
            {
                control.webObj = this;
            }
            Controls.Add(control);
        }

        #region 设置
        private string _actionDetailsUrl = "";
        [Personalizable, WebDisplayName("操作编辑Url"), WebDescription("编辑操作详情的Url"), WebBrowsable, Category("自定义设置")]
        public string ActionDetailsUrl
        {
            get
            {
                return this._actionDetailsUrl;
            }
            set
            {
                this._actionDetailsUrl = value;
            }
        }

        private string _userDesc = "1. 表格中，除操作外均可自定义填写数值、设置频度、可能使用时段及计量方式;2. 点击已设置并保存后的操作，可以查看操作详细设置;3. 关闭或离开本页前，请确保您已保存所有修改项！";
        [Personalizable, WebDisplayName("使用说明"), WebDescription("使用说明信息，多条请用;隔开"), WebBrowsable, Category("自定义设置")]
        public string UserDesc
        {
            get
            {
                return this._userDesc;
            }
            set
            {
                this._userDesc = value;
            }
        }
        #endregion
    }
}
