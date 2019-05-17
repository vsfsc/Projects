using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using System.Web;

namespace SPCustomNav.Layouts.SPCustomNav
{
    public partial class TopNav : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.txtNavXml.Text = Config.Load(base.Web, Config.NavType.Top);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Config.Save(base.Web, this.txtNavXml.Text, Config.NavType.Top);
                SPUtility.Redirect("settings.aspx", SPRedirectFlags.RelativeToLayoutsPage, HttpContext.Current);
            }
            catch (Exception exception)
            {
                throw;//SPMIPTrace.WriteError("SPMIPNavigation", exception);
            }
        }
    }
}
