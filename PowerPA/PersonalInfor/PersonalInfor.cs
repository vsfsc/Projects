using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PowerPA.PersonalInfor
{
    [ToolboxItemAttribute(false)]
    public class PersonalInfor : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PowerPA/PersonalInfor/PersonalInforUserControl.ascx";

        protected override void CreateChildControls()
        {
            PersonalInforUserControl control = Page.LoadControl(_ascxPath) as PersonalInforUserControl;
            if (control != null)
            {
                control.webObj = this;
            }
            Controls.Add(control);
        }

        #region 参数设置

        #endregion
    }
}
