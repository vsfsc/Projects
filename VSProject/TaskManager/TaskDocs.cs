using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
namespace VSProject.TaskManager
{
    //任务相关的文档，创建新的任务时，创建相关的文档，并写入数据库的关联表中
    //创建的相关文档在文档库和博客文档中
    public class TaskDocs
    {
        #region 方法
        public void  SaveTask_Doc(long taskID, DataTable dtTasks,long userID)
        {
            SPFieldUrlValue docUrl = WriteTaskDoc(taskID, dtTasks);
            if (docUrl != null)
            {
                DataSet dsDoc = VSDLL.DAL.TaskDAL.GetDocsByDocLink(docUrl.Url);
                long docID;
                if (dsDoc.Tables[0].Rows.Count > 0)
                    docID = (long)dsDoc.Tables[0].Rows[0]["DocID"];
                else
                {
                    DataRow drDoc = dsDoc.Tables[0].NewRow();
                    drDoc["Title"] = docUrl.Description;
                    drDoc["DocLink"] = docUrl.Url;
                    drDoc["Created"] = DateTime.Now;
                    drDoc["CreatedBy"] = userID;
                    drDoc["Flag"] = 1;
                    docID = VSDLL.DAL.TaskDAL.InsertDocs(null, drDoc);
                }
                DataSet ds = VSDLL.DAL.TaskDAL.GetDocsByTaskID(taskID);
                DataTable dtTask_Doc = ds.Tables[0];
                if (docID >0)//存在文档
                {
                    DataRow[] drsTask_Doc = dtTask_Doc.Select("TaskID=" + taskID + " and DocID=" + docID);
                    if (drsTask_Doc.Length ==0)//不存在任务和文档的关系
                    {
                        DataRow drTask_Doc = dtTask_Doc.NewRow();
                        drTask_Doc["TaskID"] = taskID;
                        drTask_Doc["DocID"] = docID;
                        drTask_Doc["Created"] = DateTime.Now;
                        drTask_Doc["CreatedBy"] = userID;
                        drTask_Doc["Flag"] = 1;
                        VSDLL.DAL.TaskDAL.InsertTask_Docs(null, drTask_Doc);
                    }
                }
            }
        }
        #endregion
        #region 文档库及博客文档
        private void CreateProjectDoc(string projectName,string projectEngName)
        {
            string taskDocLibName = projectName + "任务文档";// 二级任务所对应的文档库
            string blogName = projectName + "项目任务";//三级及以上对应的博客
            SPSecurity.RunWithElevatedPrivileges(delegate ()
           {
               using (SPSite  site = new SPSite(SPContext.Current.Site.ID))
               {
                   using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                   {
                       //添加文档库
                      Guid docLibID= web.Lists.Add(taskDocLibName, projectName + "的最终文档",SPListTemplateType.DocumentLibrary );
                      
                       //添加博客网站
                       SPWeb blogWeb = web.Webs.Add(projectEngName + "Blogs",blogName,projectName +"过程文档",2052,SPWebTemplate.WebTemplateBLOG,false,false );
                   }
               }
           });
        }
        //创建任务时创建文档对象，如果文档对象不为空，则不创建
        public    SPFieldUrlValue  WriteTaskDoc(long taskID ,DataTable dtTasks)
        {
            int level = 0;
            List<string> account = new List<string>();
            string[] projectNames = GetRootType(taskID, dtTasks, ref level, ref account);
            string projectName =projectNames [0];
            string taskDocLibName =projectName+"任务文档" ;// 二级任务所对应的文档库
            string blogName =projectName+ "项目任务";//三级及以上对应的博客
            try
            {
                SPWeb web = SPContext.Current.Web;
                SPSite site = SPContext.Current.Site;
                string documentName = dtTasks.Select("TaskID=" + taskID)[0]["Title"].ToString();// 去掉用户的显示名称
                SPFieldUrlValue urlValue = new SPFieldUrlValue();
                web.AllowUnsafeUpdates = true;
                if (level < 2)
                {
                    SPDocumentLibrary library = web.Lists.TryGetList(taskDocLibName) as SPDocumentLibrary;
                    if (library == null)
                    {
                        CreateProjectDoc(projectName, projectNames[1]);
                        library = web.Lists.TryGetList(taskDocLibName) as SPDocumentLibrary;
                    }
                    urlValue = new SPFieldUrlValue();
                    urlValue.Description = taskDocLibName;
                    urlValue.Url = library.DefaultViewUrl;// documentPath;
                }
                else//三级以上直接打开编辑
                {
                    int blogCateID = 0;
                    SPWeb blogWeb = GetBlogWeb(web, blogName);//列表名为任务文章 或Posts
                    if (blogWeb == null) return null;
                    string cateListName = "类别";//类别网站
                    SPList cateList = blogWeb.Lists.TryGetList(cateListName);
                    if (level == 2)//按类别打开博客
                    {
                        blogCateID = GetBlogCategoryID(cateList, documentName);
                        urlValue = new SPFieldUrlValue();
                        urlValue.Description = documentName;
                        string itemUrl = cateList.DefaultViewUrl.Replace("AllCategories.aspx", "Category.aspx");//由显示改为编辑
                        urlValue.Url = itemUrl + "?CategoryId=" + blogCateID;
                    }
                    else
                    {
                        SPList blogList = blogWeb.Lists.TryGetList(blogName.Replace(projectName, ""));

                        if (blogList == null)
                            blogList = blogWeb.Lists.TryGetList("Posts");

                        SPQuery qry = new SPQuery();
                        qry.Query = @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + documentName + "</Value></Eq></Where>";
                        SPListItemCollection items = blogList.GetItems(qry);
                        SPListItem item;
                        if (items.Count > 0)
                        {
                            item = items[0];
                        }
                        else
                        {
                            item = blogList.AddItem();
                            item["Title"] = documentName;
                            item["PublishedDate"] = DateTime.Now;
                            string category = account[account.Count -2];
                            //当前任务的上级任务，第二级任务带分类,最后一个节点是项目
                            blogCateID = GetBlogCategoryID(cateList, category);
                            if (blogCateID > 0)
                            {
                                SPFieldLookupValue cateValue = new SPFieldLookupValue();
                                cateValue.LookupId = blogCateID;
                                item["PostCategory"] = cateValue;

                            }
                            item.Update();
                        }

                        urlValue = new SPFieldUrlValue();
                        urlValue.Description = documentName;
                        string itemUrl = blogList.DefaultViewUrl.Replace("AllPosts.aspx", "EditPost.aspx");//由显示改为编辑
                        urlValue.Url = itemUrl + "?ID=" + item.ID;
                    }
                }
                if (urlValue != null)
                    return urlValue;

            }
            catch (Exception ex)
            {
                //w.WriteLine(ex.ToString());
            }
            return null ;
        }
        /// <summary>
        /// 根据博客类别名称返回类别ID
        /// </summary>
        /// <param name="blogWeb"></param>
        /// <param name="cateName"></param>
        /// <returns></returns>
        private int GetBlogCategoryID(SPList cateList, string cateName)
        {
            int cateID = 0;
            SPQuery qry = new SPQuery();
            qry.Query = @"<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + cateName + "</Value></Eq></Where>";
            SPListItemCollection items = cateList.GetItems(qry);
            if (items.Count > 0)
                cateID = items[0].ID;
            else//如果类别不存在，则添加
            {
                SPListItem item = cateList.AddItem();
                item["Title"] = cateName;
                item.Update();
                cateID = item.ID;
            }
            return cateID;

        }
        private SPWeb GetBlogWeb(SPWeb web, string title)
        {
            foreach (SPWeb subWeb in web.Webs)
                if (subWeb.Title == title)
                    return subWeb;
            return null;
        }
        /// <summary>
        /// 根结节为项目名称
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskList"></param>
        /// <param name="level"></param>
        /// <param name="account">向当前列表项自下而上遍历的标题</param>
        /// <returns></returns>
        private string[] GetRootType(long taskId, DataTable taskList,ref int level, ref List<string> account)
        {
            DataRow[] pItems= taskList.Select("TaskID=" + taskId );
            DataRow pItem;
            if (pItems.Length > 0)
                pItem = pItems[0];
            else
                return new string[] { "" };
            level = 1;
            string parentIDFieldName="PID";
            if (pItem[parentIDFieldName] == null)
                return new string[] { pItem["Title"].ToString(), pItem["Title_en"].ToString() };
            else
            {

                while (!pItem.IsNull(parentIDFieldName) &&  pItem[parentIDFieldName].ToString()!=""&&  pItem[parentIDFieldName].ToString()!="0")
                {
                    account.Add(pItem["Title"].ToString());
                    pItems = taskList.Select("TaskID=" + pItem[parentIDFieldName]);
                    if (pItems.Length == 0) break;//到根节点，直接退出
                    pItem = pItems[0];
                    level += 1;
                }
                account.Add(pItem["Title"].ToString());
                return new string[] { pItem["Title"].ToString(), pItem["Title_en"].ToString() };
            }
        }
        #endregion
    }
}
