using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace VAWebParts.ListData
{
    public partial class ListDataUserControl : UserControl
    {
        public ListData WebPartObj { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            string listTitle = "新闻公告";
            if (!string.IsNullOrEmpty(WebPartObj.ListTitle))
            {
                listTitle = WebPartObj.ListTitle;
            }

            string groupField = WebPartObj.GroupField;
            int displayType = WebPartObj.DisplayType;
            lbTset.Text = listTitle + groupField + displayType.ToString();
        }
    }
}
