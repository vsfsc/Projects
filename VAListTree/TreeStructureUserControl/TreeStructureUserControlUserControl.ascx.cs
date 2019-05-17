using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Text;
using System.Collections.Generic;
using System.Web;

namespace VAListTree.TreeStructureUserControl
{
    public partial class TreeStructureUserControlUserControl : UserControl
    {
        public const string DYNAMIC_CAML_QUERY = "<Where><IsNull><FieldRef Name='{0}' /></IsNull></Where>";
        public const string DYNAMIC_CAML_QUERY_GET_CHILD_NODE = "<Where><Eq><FieldRef Name='{0}' /><Value Type='LookupMulti'>{1}</Value></Eq></Where>";
        public TreeStructureUserControl WebPartObj { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                BuildTree();


            }
        }
        protected void BuildTree()
        {
            SPList TasksList;
            SPQuery objSPQuery;
            StringBuilder Query = new StringBuilder();
            SPListItemCollection objItems;
            string DisplayColumn = string.Empty;
            string Title = string.Empty;
            string[] valueArray = null;
            string itemUrl = string.Empty;
            treeViewCategories.Nodes.Clear();
            //treeViewCategories.Font.Size = 10;
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {

                        TasksList = SPContext.Current.Web.Lists[WebPartObj.ListName ];
                        if (TasksList != null)
                        {

                            objSPQuery = new SPQuery();
                            Query.Append(String.Format(DYNAMIC_CAML_QUERY,WebPartObj.ParentField ));
                            objSPQuery.Query = Query.ToString();
                            objItems = TasksList.GetItems(objSPQuery);
                            if (objItems != null && objItems.Count > 0)
                            {
                                foreach (SPListItem objItem in objItems)
                                {
                                    DisplayColumn = Convert.ToString(objItem[WebPartObj.SubField ]);
                                    Title = Convert.ToString(objItem[WebPartObj.SubField]);
                                    itemUrl = site.Url + TasksList.DefaultDisplayFormUrl + "?ID=" + objItem["ID"].ToString();
                                    
                                    CreateTree(Title, valueArray, null, DisplayColumn, objItem["ID"].ToString(),itemUrl );
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
        private string GetNodeStyle(string TaskTitle,string TaskStatus,string TaskOwner)
        {
            string nodeText = "";
            if (TaskStatus=="未启动")
            {
                nodeText = "<span style='color:yellow'>"+ TaskOwner+"——" + TaskTitle + "("+TaskStatus+")</span>";
            }
            else if (TaskStatus == "进行中")
            {
                nodeText = "<span style='color:blue'>" + TaskOwner + "——" + TaskTitle + "(" + TaskStatus + ")</span>";
            }
            else if (TaskStatus == "已完成")
            {
                nodeText = "<span style='color:green'>" + TaskOwner + "——"+ TaskTitle + "(" + TaskStatus + ")</span>";
            }
            else if (TaskStatus == "已推迟")
            {
                nodeText = "<span style='color:yellow'>" + TaskOwner + "——" + TaskTitle + "(" + TaskStatus + ")</span>";
            }
            else
            {
                nodeText = "<span style='color:black'>" + TaskOwner + "——" + TaskTitle + "(" + TaskStatus + ")</span>";
            }
            return nodeText;
        }
        private void CreateTree(string RootNode, string[] valueArray, List<SPListItem> objNodeCollection, string DisplayValue, string KeyValue,string itemUrl)
        {
            string objExpandValue = string.Empty;
            TreeNode objTreeNode;
            TreeNodeCollection objChildNodeColn;
            try
            {
                objTreeNode = new TreeNode(DisplayValue, KeyValue);
                objTreeNode.NavigateUrl = itemUrl;
                treeViewCategories.Nodes.Add(objTreeNode);
                objTreeNode.CollapseAll();
                objChildNodeColn = GetChildNode(RootNode, valueArray, objNodeCollection);
                foreach (TreeNode childnode in objChildNodeColn)
                {
                    objTreeNode.ChildNodes.Add(childnode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private TreeNodeCollection GetChildNode(string RootNode, string[] valueArray, List<SPListItem> objListItemColn)
        {
            TreeNodeCollection childtreenodes = new TreeNodeCollection();
            SPQuery objSPQuery;
            SPListItemCollection objItems = null;
            List<SPListItem> objNodeListItems = new List<SPListItem>();
            SiteMapNodeCollection objNode = new SiteMapNodeCollection();
            objSPQuery = new SPQuery();
            string objNodeTitle = string.Empty;
            string objNodeStatus = string.Empty;
            string objNodeOwner = string.Empty;
            string objLookupColumn = string.Empty;
            StringBuilder Query = new StringBuilder();
            SPList objTaskList;
            SPField spField;
            string objKeyColumn;

            try
            {
                objTaskList = SPContext.Current.Web.Lists[WebPartObj.ListName ];
                objLookupColumn = WebPartObj.ParentField;//"Parent_x0020_Category";//objTreeViewControlField.ParentLookup;  

                spField = SPContext.Current.Web.Lists[WebPartObj.ListName ].Fields[WebPartObj.ParentField ];

                Query.Append(String.Format(DYNAMIC_CAML_QUERY_GET_CHILD_NODE, spField.InternalName, RootNode));
                objSPQuery.Query = Query.ToString();


                objItems = objTaskList.GetItems(objSPQuery);
                foreach (SPListItem objItem in objItems)
                {
                    objNodeListItems.Add(objItem);
                }

                if (objNodeListItems != null && objNodeListItems.Count > 0)
                {
                    foreach (SPListItem objItem in objNodeListItems)
                    {
                        RootNode = Convert.ToString(objItem[WebPartObj.SubField ]);
                        objKeyColumn = Convert.ToString(objItem["ID"]);
                        
                        objNodeTitle = Convert.ToString(objItem[WebPartObj.SubField]);
                        objNodeStatus = Convert.ToString(objItem["Status"]);
                        objNodeOwner = Convert.ToString(objItem["ParentID"]);
                        if (!String.IsNullOrEmpty(objNodeTitle))
                        {
                            TreeNode childNode = new TreeNode();
                            string nodeStyleText = GetNodeStyle(objNodeTitle, objNodeStatus,objNodeOwner);
                            childNode.Text = nodeStyleText;
                            childNode.Value = objKeyColumn;
                            childNode.NavigateUrl = SPContext.Current.Site .Url + objTaskList.DefaultDisplayFormUrl+"?ID="+objKeyColumn ;
                            
                            childNode.CollapseAll();
                            foreach (TreeNode cnode in GetChildNode(RootNode, valueArray, objListItemColn))
                            {
                                childNode.ChildNodes.Add(cnode);
                            }
                            childtreenodes.Add(childNode);
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return childtreenodes;
            // Call method again (recursion) to get the child items  
        }

        //protected void treeViewCategories_SelectedNodeChanged(object sender, EventArgs e)
        //{
        //    WebPartObj.ListID =long.Parse ( treeViewCategories.SelectedNode.Value);
        //}  
    }
}
