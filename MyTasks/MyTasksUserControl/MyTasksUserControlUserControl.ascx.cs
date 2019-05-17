using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace MyTasks.MyTasksUserControl
{
    public partial class MyTasksUserControlUserControl : UserControl
    {
        string siteUrl;
        ClientContext clientContext;
        public const string DYNAMIC_TASKS_QUERY = "<Where><IsNull><FieldRef Name='{0}' /></IsNull></Where>";
        public const string DYNAMIC_CAML_QUERY_GET_MYTASKS = @"<Where><And><Eq><FieldRef Name='AssignedTo' /><Value Type='Integer'><UserID /></Value></Eq><Eq><FieldRef Name='Status' /><Value Type='Choice'>"+"'{0}'"+"</Value></Eq></And></Where>";
        public MyTasksUserControl WebPartObj { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                GetMyTasks();
            }
        }
        protected void GetMyTasks()
        {
            SPList TasksList;
            SPQuery spQuery;
            StringBuilder Query = new StringBuilder();
            SPListItemCollection objItems;
            string DisplayColumn = string.Empty;
            string Title = string.Empty;
            string[] taskLists = WebPartObj.ListName.Split(';');
            string itemUrl = string.Empty;
            //treeViewCategories.Font.Size = 10;
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        foreach (string taskList in taskLists)
                        {
                            TasksList = SPContext.Current.Web.Lists[taskList];
                            if (TasksList != null)
                            {
                                spQuery = new SPQuery();
                                Query.Append(String.Format(DYNAMIC_TASKS_QUERY, taskList));
                                spQuery.Query = Query.ToString();
                                objItems = TasksList.GetItems(spQuery);
                                if (objItems != null && objItems.Count > 0)
                                {
                                    foreach (SPListItem objItem in objItems)
                                    {
                                        if (objItem["AssignedTo"] == null) return;
                                        FieldUserValue[] authors = (FieldUserValue[])objItem["AssignedTo"];
                                        foreach (var author in authors)
                                        {
                                            var authorUser = clientContext.Web.SiteUsers.GetById(author.LookupId);
                                        }
                                            //DisplayColumn = Convert.ToString(objItem[WebPartObj.SubField]);
                                            //Title = Convert.ToString(objItem[WebPartObj.SubField]);
                                            //itemUrl = site.Url + TasksList.DefaultDisplayFormUrl + "?ID=" + objItem["ID"].ToString();
                                        }
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
    }
}
