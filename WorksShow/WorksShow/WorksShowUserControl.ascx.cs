using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WorksShow.WorksShow
{
	public partial class WorksShowUserControl : UserControl
	{
		#region 属性
		public WorksShow webObj;

		#endregion

		#region 事件


		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				try
				{
					int worksId = GetWorksID();
					if (worksId != 0)
					{
						divWorks.Visible = true;
						ShowWorks(worksId);
						divErr.Visible = false;
					}
					else
					{
						divWorks.Visible = false;
						divErr.Visible = true;
						divErr.InnerHtml = "未指定作品ID，无法显示！";
					}
				}
				catch (Exception ex)
				{

					divErr.InnerHtml=ex.ToString();
				}
			}
		}
		#endregion

		#region 方法
		/// <summary>
		/// 展示指定ID的作品信息
		/// </summary>
		/// <param name="worksId"></param>
		private void ShowWorks(int worksId)
		{
			SPListItem works = GetWorksByID(worksId);
			if (works!=null)
			{
				LbWorksName.Text = works["Title"].ToString();
				string author = works["Author"].ToString();
				lbAuthor.Text= Regex.Split(author,";#",RegexOptions.IgnoreCase)[1];
				lbPub.Text=string.Format("{0:d}",(DateTime)works["Created"]);
				lbReadCount.Text =  works["HitCount"]!=null?works["HitCount"].ToString():"0";
				LbWorksStatus.Text =works["Status"]!=null? works["Status"].ToString():"";
				string worksType=works["WorksType"] != null? works["WorksType"].ToString():"";
				if (worksType != "" && worksType.Contains(";#"))
				{
					worksType=Regex.Split(worksType,";#",RegexOptions.IgnoreCase)[1];
				}
				LbWorksType.Text = worksType;
				divIntro.InnerHtml=works["introduction"] != null? works["introduction"].ToString():"";
				divIdeas.InnerHtml=works["DesignIdeas"] != null? works["DesignIdeas"].ToString():"";
				divFeatures.InnerHtml=works["CreativeFeatures"] != null? works["CreativeFeatures"].ToString():"";
				divDesc.InnerHtml=works["Description"] != null? works["Description"].ToString():"";
                SPListItemCollection myworks = GetUserWorksByWorksID(worksId);
				//int hitcount = int.Parse(GetUserWorks(dtUserWorks, "View").ToString());
				//EditUserWorks(webObj.UserWorks, worksId, "View", hitcount + 1);
				GetMyStatus(worksId);
				//lbFavCount.Text = CalculateItems("Fav", "求和", items);;
                //lbLike.Text=CalculateItems("Praise", "求和", items);

				string task = works["TaskID"] != null ? works["TaskID"].ToString() : "";
				if (task != "")
				{
					int taskId = int.Parse(task.Split(';')[0]);
                    ShowTaskDocsByTaskID(taskId);
                    DataTable AttachmentsTable = GenAttachmentsTable();
                    //AttachmentsTable = GetDocsByTaskId(taskId,AttachmentsTable);
                    AttachmentsTable = GetMediasByTaskID(taskId,AttachmentsTable);
					ShowAttachments(AttachmentsTable);
				}
			}
		}


        /// <summary>
        /// 计算SharePoint列表项集合中某个字段
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="caltype">计算类型</param>
        /// <param name="items">列表项集合</param>
        /// <returns></returns>
        private string CalculateItems(string field,string caltype,SPListItemCollection items)
        {
            string result = "0";
            if (items!=null)
            {
                if (items.Count>0)
                {
                    double rvalue = 0;
                    foreach (SPListItem item in items)
                    {
                        if (item[field]!=null)
                        {
                            if (item[field].ToString()!="-1")
                            {
                                rvalue += double.Parse(item[field].ToString());
                            }
                        }
                    }

                    if (caltype=="平均值")
                    {
                        result = (rvalue / items.Count).ToString("0.00");
                    }
                    else if (caltype=="计数")
                    {
                        result = items.Count.ToString();
                    }
                    else
                    {
                        result = rvalue.ToString();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 展示媒体附件
        /// </summary>
        /// <param name="AttachmentsTable">媒体附件列表</param>
        private void ShowAttachments(DataTable AttachmentsTable)
		{
			if (AttachmentsTable != null)
			{
				for (int i = 0; i < AttachmentsTable.Rows.Count; i++)
				{
                    DataRow dr = AttachmentsTable.Rows[i];
					if (dr["Type"].ToString()=="音频"|| dr["Type"].ToString()=="视频")
					{
                        fsmedias.Visible = true;
                        GenMediaFileControl(dr);
					}
					else if(dr["Type"].ToString()=="图片"|| dr["Type"].ToString()=="图像")
                    {
                        if (!fsdocs.Visible)
                        {
                            fsdocs.Visible = true;
                        }
                        HtmlGenericControl div1 = new HtmlGenericControl("div")
                        {
                            InnerHtml = "<img src='" + dr["Url"].ToString() + "' class='ez' alt='" + dr["Title"].ToString() + "'/><h4 style='text-align: center;'>" + dr["Title"].ToString() + "</h4>"
                        };
                        div1.Attributes.Add("class", "box");
                        DocsList.Controls.Add(div1);
                    }
                    else
					{
                        HtmlGenericControl div1 = new HtmlGenericControl("div")
						{
							InnerHtml = "<a href='" + dr["Url"].ToString() + "'>" + dr["Title"].ToString() + "</a>"
						};
                        OthersList.Controls.Add(div1);
					}
				}
			}
			else
			{
                fsmedias.Visible = false;
			}

		}


        #region 作品排行筛选

        private void GetAllWorks()
        {

        }

        /// <summary>
        /// 收藏最多
        /// </summary>
        private void ShowMostFavs()
        {

        }

        /// <summary>
        /// 点赞最多
        /// </summary>
        private void ShowMostLikes()
        {

        }

        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <param name="userId"></param>
        private void ShowAllMyFavs(int userId)
        {

        }

        #endregion

        /// <summary>
        /// 获取对应文件类型的图标
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="iconList"></param>
        /// <returns></returns>
        private string GetFileIconUrl(string fileType,string iconList)
        {
            string iconUrl = "../../../../_layouts/15/WorksShow/Icons/";
            string query = @"<Where>
                              <Eq>
                                 <FieldRef Name='Title' />
                                 <Value Type='Text'>"+fileType+@"</Value>
                              </Eq>
                            </Where>";
            SPListItemCollection items = GetDataFromList(iconList, query);
            if (items!=null)//有该文件类型对应的图标
            {
                if (items.Count>0)
                {
                    SPListItem item = items[0];
                    iconUrl += item["IconUrl"].ToString();
                }
                else
                {
                    iconUrl += "256_ICGEN.PNG";
                }
            }
            else
            {
                iconUrl += "256_ICGEN.PNG";
            }
            return iconUrl;
        }

        /// <summary>
        /// 生成单个视频（音频）的播放控件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
		private void GenMediaList(string title,string url)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<a href='"+url+"' name='"+title+"'>");
			sb.AppendLine("<img='' class='listpic' src='../../../../_layouts/15/WorksShow/images/start.jpg' alt='"+title+"' width='200' height='150'>");
			sb.AppendLine("<span class='v_bq_hong'>作品音频</span><span class='v_bg'></span>");
			sb.AppendLine("<span class='v_bq_vico'></span>");
			sb.AppendLine("</a>");
			sb.AppendLine("<h2><a href='"+url+"' name='"+title+"'>"+title+"</a></h2>");
			HtmlGenericControl div1 = new HtmlGenericControl("div");
			div1.Attributes.Add("class", "plist");
			div1.InnerHtml = sb.ToString();
			MediaList.Controls.Add(div1);
		}

        /// <summary>
        /// 音视频附件的显示：播放窗口
        /// </summary>
        /// <param name="serviceWorks">数据服务层接口</param>
        /// <param name="mediaFile">作品ID</param>
        /// <returns></returns>
        private void GenMediaFileControl(DataRow mediaFile)
        {
            using (HtmlGenericControl div1 = new HtmlGenericControl("div"))
            {
                div1.Attributes.Add("class", "plist");
                StringBuilder htmlContent = new StringBuilder();
                string mediaTitle = mediaFile["Title"].ToString();
                htmlContent.AppendLine("<a href='" + mediaFile["Url"] + "' name='" + mediaTitle+ "'");

                string txtExtend = mediaTitle.Substring(mediaTitle.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower();
                if (txtExtend == "mp3")
                {
                    htmlContent.AppendLine("<img class='listpic' src='../../../../_layouts/15/WorksShow/Icons/256_ICAUDIO.png' alt='" +mediaTitle + "' width='200'  height='150'>");
                    htmlContent.AppendLine("<span class='v_bq_hong'>音频</span><span class='v_bg'></span>");
                }
                else
                {
                    htmlContent.AppendLine("<img class='listpic' src='../../../../_layouts/15/WorksShow/Icons/256_ICVIDEO.png' alt='" +mediaTitle + "' width='200'  height='150'>");
                    htmlContent.AppendLine("<span class='v_bq_lan'>视频</span><span class='v_bg'></span>");
                }
                htmlContent.AppendLine("<span class='v_bq_vico'></span></a>");
                htmlContent.AppendLine("<h2><a href='" + mediaFile["Url"]  + "' name='" + mediaTitle + "'>" +mediaTitle+ "</a></h2>");
                htmlContent.AppendLine("</div>");
                div1.InnerHtml = htmlContent.ToString();
                MediaList.Controls.Add(div1);
            }
            //return htmlContent;
        }

        /// <summary>
        /// 根据作品ID查询作品
        /// </summary>
        /// <param name="worksId"></param>
        private SPListItem GetWorksByID(int worksId)
		{
			string query = @"<Where>
							  <Eq>
								 <FieldRef Name='ID' />
								 <Value Type='Counter'>"+worksId+@"</Value>
							  </Eq>
						   </Where>";
			SPListItemCollection items= GetDataFromList(webObj.WorksList, query);
			if (items.Count>0)
			{
				return items[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获取URL中传递的作品ID
		/// </summary>
		/// <returns></returns>
		private int GetWorksID()
		{

			if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
			{
				return int.Parse(Request.QueryString["ID"]);
			}
			else
			{
				return 0;
			}
		}


		/// <summary>
		/// 获取当前登录用户ID,0表示未登录
		/// </summary>
		private int GetUserId()
		{
			int userId = 0;
			SPUser spUser = SPContext.Current.Web.CurrentUser;
			if (spUser != null)
			{
				userId = spUser.ID;
			}
			else
			{
				userId = 0;
			}
			return userId;
		}

        /// <summary>
		/// 根据作品ID查找作品的关联操作信息：发布、收藏、点赞、评论、评分
		/// </summary>
		/// <param name="worksId"></param>
		private SPListItemCollection GetUserWorksByWorksID(int worksId)
		{
			string query = @" <Where>
							  <Eq>
								 <FieldRef Name='WorksID' LookupId='True' />
								 <Value Type='Lookup'>"+worksId+@"</Value>
							  </Eq>
						   </Where>";
			SPListItemCollection items = GetDataFromList(webObj.UserWorks, query);
			return items;
		}

        /// <summary>
        /// 获取当前登录用户对本作品的操作状态
        /// </summary>
        /// <param name="dtUserWorks"></param>
		private void GetMyStatus(int worksId)
		{
			SPUser user = SPContext.Current.Web.CurrentUser;
			if (user==null)//未登录
			{
				ibtnFav.ImageUrl = "../../../../_layouts/15/WorksShow/images/unfav.png";
				ibtnLike.ImageUrl = "../../../../_layouts/15/WorksShow/images/unlike.png";
				ibtnFav.Enabled = false;
				ibtnLike.Enabled = false;
			}
			else
			{
				int userId = user.ID;
				ibtnFav.Enabled = true;
				ibtnLike.Enabled = true;
				try
				{
                    string query = @"<Where>
                                      <And>
                                         <Eq>
                                            <FieldRef Name='WorksID' LookupId='True' />
                                            <Value Type='Lookup'>"+worksId+@"</Value>
                                         </Eq>
                                         <Eq>
                                            <FieldRef Name='Author' LookupId='True' />
                                            <Value Type='Integer'>"+userId+@"</Value>
                                         </Eq>
                                      </And>
                                   </Where>";
                    SPListItemCollection items= GetDataFromList(webObj.UserWorks, query);
					if (items==null)
					{
						ibtnFav.ImageUrl = "../../../../_layouts/15/WorksShow/images/unfav.png";
						ibtnLike.ImageUrl = "../../../../_layouts/15/WorksShow/images/unlike.png";
					}
					else
					{
						if (items.Count > 0)//已有当前用户对该作品的操作历史
						{
                            SPListItem item = items[0];
							if (item["Fav"].ToString() == "1")
							{
								ibtnFav.ImageUrl = "../../../../_layouts/15/WorksShow/images/fav.png";
							}
							else
							{
								ibtnFav.ImageUrl = "../../../../_layouts/15/WorksShow/images/unfav.png";
							}
							if (item["Praise"].ToString() =="1")
							{
								ibtnLike.ImageUrl = "../../../../_layouts/15/WorksShow/images/like.png";
							}
							else
							{
								ibtnLike.ImageUrl = "../../../../_layouts/15/WorksShow/images/unlike.png";
							}
						}
						else
						{
							ibtnFav.ImageUrl = "../../../../_layouts/15/WorksShow/images/unfav.png";
							ibtnLike.ImageUrl = "../../../../_layouts/15/WorksShow/images/unlike.png";
						}
					}
				}
				catch (Exception ex)
				{

					divErr.InnerHtml = ex.ToString();
				}

			}

		}

		/// <summary>
		/// 获取作品指定操作的统计值
		/// 比如收藏，则统计收藏的次数，评分则统计该作品的平均分
		/// </summary>
		/// <param name="dtUserWorks"></param>
		/// <param name="action">操作</param>
		private string GetUserWorks(DataTable dtUserWorks ,string action)
		{
			string calculated = "0";
			if (dtUserWorks!=null)
			{
				if (action == "Score")//评分求平均值
				{
					calculated = dtUserWorks.Compute("avg(" + action + ")", action + "<>-1").ToString();
				}
				else//其他计数
				{
					calculated = dtUserWorks.Compute("count(ID)", action + "=1").ToString();
				}
			}
			return calculated;
		}


		/// <summary>
		/// 改变用户-作品的关系
		/// </summary>
		/// <param name="listName">用户-作品关系列表名称</param>
		/// <param name="worksId">作品ID</param>
		/// <param name="action">用户对作品的操作</param>
		/// <param name="result">用户对作品的操作结果</param>
		private void EditUserWorks(string listName,int worksId,string action,int result)
		{
			SPSecurity.RunWithElevatedPrivileges(delegate ()
			{
				try
				{
					using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
					{
						using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
						{
							SPList spList = spWeb.Lists.TryGetList(listName);
							int uId = spWeb.CurrentUser.ID;
							if (spList != null)
							{
								SPQuery qry = new SPQuery
								{
									Query = @"<Where>
												  <And>
													<Eq>
														<FieldRef Name='WorksID' LookupId='True' />
														<Value Type='Lookup'>"+worksId+@"</Value>
													</Eq>
													 <Eq>
														<FieldRef Name='Author' LookupId='True' />
														<Value Type='Integer'>"+uId+@"</Value>
													 </Eq>
												  </And>
											   </Where>"
								};
								SPListItemCollection items = spList.GetItems(qry);
								spWeb.AllowUnsafeUpdates = true;
								if (items.Count > 0)//已有操作记录
								{
									SPListItem item = items[0];
									item[action] = result;
									//if (action != "View")
									//{
										item[action + "Date"] = DateTime.Now;
									//}
									item.Update();
								}
								else//尚未有对此作品执行该操作的记录
								{
									SPListItem item = spList.Items.Add();
									item["Author"] = uId;
									item["WorksID"] = worksId;
									item[action] = result;
									//if (action!="View")
									//{
										item[action + "Date"] = DateTime.Now;
									//}
									item.Update();
								}
								spWeb.AllowUnsafeUpdates = false;
							}
							else
							{
								lbErr.Text = "指定的列表“" + listName + "”不存在！";
							}
						}
					}
				}
				catch (Exception ex)
				{

					lbErr.Text = ex.ToString();
				}
			});
		}


        /// <summary>
        /// 获取指定任务ID对应的所有作品
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        private SPListItemCollection GetWorksByTaskId(int taskId)
        {
            string query = @"<Where>
							  <Eq>
								 <FieldRef Name='TaskID'  LookupId='True'/>
								 <Value Type='Lookup'>" + taskId + @"</Value>
							  </Eq>
						   </Where>";
            SPListItemCollection works = GetDataFromList(webObj.WorksList, query);
            return works;
        }

		/// <summary>
		/// 根据任务文档ID查找任务对应的文档的标题和地址
		/// 一个任务对应一个文档，文档总数为1，直接显示到图片文档控件区域
		/// </summary>
		/// <param name="taskId"></param>
		private void ShowTaskDocsByTaskID(int taskId)
		{
			SPListItem task = GetItemByItemID(taskId, webObj.TaskDoc);
			if (task!=null)
			{
				if (task["TaskDoc"]!=null)
                {
                    fsdocs.Visible = true;
                    SPFieldUrlValue tdoc = new SPFieldUrlValue(task["TaskDoc"].ToString());
                    string tdocTitle = tdoc.Description;
                    string tdocLink = ConvertUrl(tdoc.Url);
                    tdocLink = webObj.OOServer + "op/embed.aspx?src=" + tdocLink;
                    string fileType = tdocLink.Substring(tdocLink.LastIndexOf('.')+1);
                    string tdocIcon = GetFileIconUrl(fileType, webObj.FileIcon);
                    HtmlGenericControl div1 = new HtmlGenericControl("div")
                    {
                        InnerHtml = "<img src='" + tdocIcon + "' class='ez' href='" + tdocLink + "' alt='" + tdocTitle + "'><h4 style='text-align:center;'>" + tdocTitle + "</h4>"
                    };
                    div1.Attributes.Add("class", "box");
                    DocsList.Controls.Add(div1);
                }
                else
				{
					fsdocs.Visible = false;
				}
			}
			else
			{
				fsdocs.Visible = false;
			}
		}


        /// <summary>
        /// 初始化作品附件表
        /// </summary>
        /// <returns></returns>
        private DataTable GenAttachmentsTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("Url");
            dt.Columns.Add("Type");
            return dt;
        }

        /// <summary>
        /// 获取任务对应的文档
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="attachmentsTable"></param>
        /// <returns></returns>
        private DataTable GetDocsByTaskId(int taskId, DataTable attachmentsTable)
        {
            SPListItem task = GetItemByItemID(taskId, webObj.TaskDoc);
            if (task != null)
            {
                if (task["TaskDoc"] != null)
                {
                    SPFieldUrlValue tdoc = new SPFieldUrlValue(task["TaskDoc"].ToString());
                    string tdocTitle = tdoc.Description;
                    string tdocLink = ConvertUrl(tdoc.Url);//转化地址为远程服务器地址
                    tdocLink = webObj.OOServer + "op/embed.aspx?src=" + tdocLink;
                    string fileType = tdocTitle.Substring(tdocTitle.LastIndexOf('.')+1);
                    string tdocIcon = GetFileIconUrl(fileType, webObj.FileIcon);
                    DataRow dr = attachmentsTable.NewRow();
                    dr["Title"] = tdocTitle;

                    dr["url"]= "<div class='box'><img src='" + tdocIcon + "' class='ez' href='" + tdocLink + "' alt='" + tdocTitle + "'><h4 style='text-align:center;'>" + tdocTitle + "</h4></div>";
                    dr["Type"] = "文档";
                }
            }
            return attachmentsTable;
        }

        /// <summary>
        /// 将本地服务器Url转化为远程服务器url
        /// </summary>
        /// <param name="url">原地址</param>
        /// <returns></returns>
        private string ConvertUrl(string url)
        {
            string siteUrl = SPContext.Current.Site.Url;
            if (url.Contains("http"))
            {
                int k = url.IndexOf('/', 7);
                if (url.Contains("https"))
                {
                    k = url.IndexOf('/', 8);
                }
                string hostUrl = url.Substring(0, k);
                url = url.Replace(hostUrl, siteUrl);
            }
            else
            {
                url = siteUrl + url;
            }
            return url;
        }

        /// <summary>
        /// 根据任务ID，查询该任务对应的活动的所有附件媒体
        /// 一个任务对应m个活动，一个活动对应n个媒体,媒体总数m*n
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="AttachmentsTable">附件表</param>
        private DataTable GetMediasByTaskID(int taskId,DataTable AttachmentsTable)
		{
			string query = @"<Where>
							  <Eq>
								 <FieldRef Name='TaskID'  LookupId='True'/>
								 <Value Type='Lookup'>"+taskId+@"</Value>
							  </Eq>
						   </Where>";
			SPListItemCollection activities = GetDataFromList(webObj.Activity, query);

			if (activities != null)
			{

				StringBuilder sb = new StringBuilder();
				for(int i = 0; i < activities.Count; i++)
				{
					int aId = activities[i].ID;
					query = @"<Where>
							  <Eq>
								 <FieldRef Name='AssistantID' />
								 <Value Type='Number'>" + aId + @"</Value>
							  </Eq>
						   </Where>";
					SPListItemCollection aMedias = GetDataFromList(webObj.ActivityMedia, query); //指定活动ID为aId的活动媒体关系
					if(aMedias!=null)
					{
						SPListItemCollection medias = GetDataFromList(webObj.MediaLibrary, "");
						//DataTable dtMedias = medias.GetDataTable();
						for (int j = 0; j< aMedias.Count; j++)
						{
							DataRow dr = AttachmentsTable.NewRow();
							int mediaId =int.Parse(aMedias[j]["MediaID"].ToString());
							SPListItem media = medias.GetItemById(mediaId);
                            dr["Title"] = media["FileLeafRef"];
                            string url = media["FileRef"].ToString();
                            //if (media["内容类型"].ToString()=="视频")
                            //{
                            //    url = "<div class='box'>";
                            //}
							dr["Url"]=ConvertUrl(media["FileRef"].ToString());
							dr["Type"]=media["内容类型"];
							AttachmentsTable.Rows.Add(dr);
						}
					}
				}

			}
			return AttachmentsTable;
		}

		/// <summary>
		/// 查询SharePoint列表中指定ID的列表项
		/// </summary>
		/// <param name="itemId">列表项ID</param>
		/// <param name="listName">列表名称</param>
		/// <returns></returns>
		private SPListItem GetItemByItemID(int itemId,string listName)
		{
			SPListItem item = null;
			SPSecurity.RunWithElevatedPrivileges(delegate ()
			{
				try
				{
					using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
					{
						using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
						{
							SPList spList = spWeb.Lists.TryGetList(listName);

							if (spList != null)
							{
								item = spList.GetItemById(itemId);
							}
							else
							{
								lbErr.Text = "指定的列表“" + listName + "”不存在！";
							}
						}
					}
				}
				catch (Exception ex)
				{

					lbErr.Text = ex.ToString();
				}
			});
			return item;
		}

		/// <summary>
		/// 从SharePoint列表获取数据
		/// </summary>
		/// <param name="listName">列表名称</param>
		/// <param name="query">Caml查询语句</param>
		/// <returns></returns>
		private SPListItemCollection GetDataFromList(string listName, string query)
		{
			SPListItemCollection items = null;
			SPSecurity.RunWithElevatedPrivileges(delegate ()
			{
				try
				{
					using (SPSite spSite = new SPSite(SPContext.Current.Site.ID)) //找到网站集
					{
						using (SPWeb spWeb = spSite.OpenWeb(SPContext.Current.Web.ID))
						{
							SPList spList = spWeb.Lists.TryGetList(listName);

							if (spList != null)
							{
								if (query != "")
								{
									SPQuery qry = new SPQuery();
									qry.Query = query;
									items = spList.GetItems(qry);
								}
								else
								{
									items = spList.GetItems();
								}
							}
							else
							{
								lbErr.Text = "指定的列表“" + listName + "”不存在！";
							}
						}
					}
				}
				catch (Exception ex)
				{

					lbErr.Text = ex.ToString();
				}
			});
			return items;
		}

		#endregion

		protected void ibtnFav_Click(object sender, ImageClickEventArgs e)
		{
			string listName = webObj.UserWorks;
			int worksId = GetWorksID();
			if (ibtnFav.ImageUrl == "../../../../_layouts/15/WorksShow/images/fav.png")//已收藏状态
			{
				EditUserWorks(listName, worksId,"Fav", -1);
				ibtnFav.ImageUrl = "../../../../_layouts/15/WorksShow/images/unfav.png";
				lbFavCount.Text = (int.Parse(lbFavCount.Text) - 1).ToString();
			}
			else
			{
				EditUserWorks(listName, worksId,"Fav", 1);
				lbFavCount.Text = (int.Parse(lbFavCount.Text) + 1).ToString();
				ibtnFav.ImageUrl = "../../../../_layouts/15/WorksShow/images/fav.png";
			}
		}

		protected void ibtnLike_Click(object sender, ImageClickEventArgs e)
		{
			string listName = webObj.UserWorks;
			int worksId = GetWorksID();
			if (ibtnLike.ImageUrl == "../../../../_layouts/15/WorksShow/images/like.png")//已点赞状态
			{
				EditUserWorks(listName, worksId, "Praise", -1);
				ibtnLike.ImageUrl = "../../../../_layouts/15/WorksShow/images/unlike.png";
				lbLike.Text = (int.Parse(lbLike.Text) - 1).ToString();
			}
			else
			{
				EditUserWorks(listName, worksId, "Praise", 1);
				lbLike.Text = (int.Parse(lbLike.Text) + 1).ToString();
				ibtnLike.ImageUrl = "../../../../_layouts/15/WorksShow/images/like.png";
			}
		}
	}
}
