using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;

namespace MyTasks.Layouts.CustomErrorPage
{
    public partial class AssistantError : Page
    {
         //protected System.Web.UI.WebControls.Button btnReturn;
        protected void Page_Load(object sender, EventArgs e)
        {
            //btnReturn.Click += BtnReturn_Click;
            if (!IsPostBack)
                ViewState["retu"] = Request.UrlReferrer.ToString();
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(ViewState["retu"].ToString());
            //或Server.Transfer   (ViewState["retu"].ToString());   
        }
    }
}
