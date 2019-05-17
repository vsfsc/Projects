using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Linq;
using Button = System.Web.UI.WebControls.Button;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;

namespace WorksShowDll.Inherits
{
    public class WorksShow : LayoutsPageBase
    {
        #region 控件定义
        protected Label LbWorksName;
        protected Label LbWorksCode;
        protected Label LbWorksType;
        protected Label LbSubmitProfile;
        protected Label LbInstallationGuide;
        protected Label lbCount;

        protected Label LbComment;
        protected GridView GvComments;
        protected HtmlGenericControl DivWorksFile;
        protected Label LbPersons;
        protected HtmlGenericControl DivScore;

        protected ImageButton BtnDelete;
        protected ImageButton BtnFav;
        protected Button BtnUnFav;


        protected ImageButton BtnDel;
        protected HtmlGenericControl DocsDiv;
        protected HtmlGenericControl ImagesDiv;
        protected HtmlGenericControl MediaDiv;
        protected HtmlGenericControl MediaList;
        protected HtmlGenericControl OthersDiv;

        protected Button BtnSubmit;
        protected TextBox TxtComments;
        protected HiddenField HiddenField1;
        protected Label LblDemoUrl;
        protected HtmlGenericControl DivDesignIdeas;
        protected HtmlGenericControl DivViewShow;
        protected HtmlGenericControl DivKeyPoints;

        protected HtmlGenericControl WorksNavDiv;
        protected HyperLink lnkPre;
        protected HyperLink lnkNext;

        protected HtmlGenericControl LeftLayer;
        protected HtmlGenericControl RightLayer;

        //protected Button btnSet;
        protected TextBox txtWorksId;

        protected HtmlGenericControl SrDiv;
        #endregion

        #region 属性

        /// <summary>
        /// 获取外部传入URL的参数：作品ID
        /// </summary>
        private long WorksId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["wid"]))
                {
                    return long.Parse(Request.QueryString["wid"]);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取当前登录用户ID
        /// </summary>
        private long UserId
        {
            get
            {
                long userId = 0;
                SPUser spUser = SPContext.Current.Web.CurrentUser;
                if (spUser != null)
                {
                    string username = spUser.LoginName;
                    username = username.Substring(username.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                    userId = Bll.UserBll.GetUserIdByLoginName(username);
                }
                else
                {
                    userId = 0;
                }
                return userId;
            }
        }

        //private List<CSWorksExpertUser> GetDSWorksComments
        //{
        //    get
        //    {
        //        if (ViewState["dsWorksComments"] == null)
        //        {

        //            List<CSWorksExpertUser> ds = DAL.Works.GetWorksCommentsByWorksID(WorksID);
        //            ViewState["dsWorksComments"] = ds;
        //        }
        //        return (List<CSWorksExpertUser>)ViewState["dsWorksComments"];
        //    }
        //}
        //private List<Works> GetDSWorksSubmit
        //{
        //    get
        //    {
        //        if (ViewState["dsWorksSubmit"] == null)
        //        {

        //            List<Works> ds = DAL.Works.GetWorksSubmitByID(WorksID);
        //            ViewState["dsWorksSubmit"] = ds;
        //        }
        //        return (List<Works>)ViewState["dsWorksSubmit"];
        //    }
        //}
        ////类型2为图片
        //private List<WorksFile> GetDSWorksFile
        //{
        //    get
        //    {
        //        if (ViewState["dsWorksFile"] == null)
        //        {
        //            List<WorksFile> ds = DAL.Works.GetWorksFile(WorksID, 2);
        //            ViewState["dsWorksFile"] = ds;
        //        }
        //        return (List<WorksFile>)ViewState["dsWorksFile"];
        //    }
        //}
        //private long WorksID
        //{
        //    get
        //    {
        //        if (ViewState["worksID"] == null)
        //        {
        //            long worksID = 0;
        //            string id = Request.QueryString["WorksID"];
        //            if (string.IsNullOrEmpty(id))
        //                worksID = 0;
        //            else
        //                worksID = long.Parse(id);
        //            ViewState["worksID"] = worksID;
        //        }
        //        return (long)ViewState["worksID"];
        //    }
        //}
        //private long ContestID
        //{
        //    get
        //    {
        //        if (ViewState["contestID"] == null)
        //        {
        //            long worksID = 0;
        //            string id = Request.QueryString["ContestID"];
        //            if (string.IsNullOrEmpty(id))
        //                worksID = 0;
        //            else
        //                worksID = long.Parse(id);
        //            ViewState["contestID"] = worksID;
        //        }
        //        return (long)ViewState["contestID"];
        //    }
        //}
        #endregion

        #region 事件

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //string strTxt = "";
            //ShowWorks oForm = (ShowWorks)this.Context.Handler;
            //strTxt += "Value of Textbox1:" + oForm.Name + "<br>";
            //strTxt += "Time Property:" + oForm.Time + "<br>";
            //strTxt += "Wss_PageUrlPath:" + Context.Items["Wss_PageUrlPath"].ToString() + "<br>";
            //srDiv.InnerHtml = strTxt;

            //点评信息
            //Response.Cache.SetNoStore();
            //if (Convert.ToBoolean(Session["IsSubmit"]))
            //{

            //    //如果表单数据提交成功，就设“Session["IsSubmit"]”为false

            //    Session["IsSubmit"] = false;

            //    //显示提交成功信息

            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('点评成功');YiComments();</script>");

            //}

            if (!Page.IsPostBack)
            {
                InitControl(UserId, WorksId);

            }
            BtnUnFav.CommandArgument = UserId.ToString();
            BtnUnFav.Click += BtnUnFav_Click;
            BtnFav.CommandArgument = UserId.ToString();
            BtnFav.Click += BtnFav_Click;

            BtnDelete.Click += BtnDelete_Click;
            //btnSet.Click+=new EventHandler(btnSet_Click);
            //btnSubmit.Click += new EventHandler(btnSubmit_Click);
        }

        /// <summary>
        /// 删除作品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            string worksId = WorksId.ToString();
            Works wks = Bll.WorksBll.GetWorksSubmitById(worksId);
            wks.Flag = 0;
            Bll.WorksBll.Update(wks);
        }

        /// <summary>
        /// 取消收藏作品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUnFav_Click(object sender, EventArgs e)
        {
            if (BtnUnFav.CommandArgument != null)
            {
                long userId = long.Parse(BtnUnFav.CommandArgument);
                Favorites favWorks = Bll.UserBll.GetUserFavorites(userId.ToString()).FirstOrDefault(f => f.DomainID == 5 & f.ItemID == WorksId);
                if (favWorks!=null)
                {
                    favWorks.Flag = 0;
                    favWorks.ModifyDate = DateTime.Now;
                }
                Bll.UserBll.ModifyFav(favWorks);
                Common.Common.ShowMessage(this, Page.GetType(), "取消收藏成功");
                BtnUnFav.Visible = false;
                BtnFav.Visible = true;
            }
            else
            {
                Common.Common.ShowMessage(this, Page.GetType(), "请先登录");
            }
        }

        /// <summary>
        /// 收藏作品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFav_Click(object sender, EventArgs e)
        {
            if (BtnUnFav.CommandArgument != null)
            {
                long userId = long.Parse(BtnUnFav.CommandArgument);
                Favorites favWorks = Bll.UserBll.GetUserFavorites(userId.ToString()).FirstOrDefault(f => f.DomainID == 5 & f.ItemID == WorksId);
                if (favWorks!=null)
                {
                    favWorks.Flag = 1;
                    favWorks.ModifyDate = DateTime.Now;
                    Bll.UserBll.ModifyFav(favWorks);
                    Common.Common.ShowMessage(this, Page.GetType(), "重新收藏成功");
                }
                else
                {
                    Favorites favorites = new Favorites
                    {
                        UserID = userId,
                        ItemID = WorksId,
                        DomainID = 5,
                        FavDate = DateTime.Now,
                        Flag = 1
                    };
                    Bll.UserBll.AddFav(favorites);
                    Common.Common.ShowMessage(this, Page.GetType(), "收藏成功");
                }
                BtnFav.Visible = false;
                BtnUnFav.Visible = true;
            }
            else
            {
                Common.Common.ShowMessage(this, Page.GetType(), "请先登录");
            }
        }

        private void getReadandFavData(long worksId)
        {

            List<Favorites> favWorks = Bll.UserBll.GetWorksFavorites(worksId,5);
            string favCount = "0";
            string readCount = "0";
            if (favWorks!=null)
            {
                favCount = favWorks.Count.ToString();
            }
            Works work = Bll.WorksBll.GetWorks().FirstOrDefault(w=>w.WorksID==worksId);
            if (work!=null)
            {
                work.Hits = (int.Parse(work.Hits.ToString()) + 1).ToString();
                //Bll.WorksBll.AddWorksRead(work);
                readCount = work.Hits==null?"0":work.Hits;
            }
            lbCount.Text = "阅读："+ readCount + " 次 | 收藏："+favCount+" 次";
        }

        private void addRead()
        {

        }
        //void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    int score = int.Parse(HiddenField1.Value);
        //    List<CSWorksExpertUser> dt = GetDSWorksComments;
        //    CSWorksExpertUser dr = new CSWorksExpertUser();
        //    dr.WorksID = WorksID;
        //    dr.UserID = DAL.Common.LoginID;
        //    dr.Flag = 1;
        //    dr.Score = score;
        //    dr.Comments = txtComments.Text;
        //    dr.Created = DateTime.Now;
        //    try
        //    {
        //        long resultID = DAL.Works.InsertWorksComments(dr);
        //        if (resultID > 0)
        //        {
        //            ViewState["dsWorksComments"] = null;
        //            FillComments();
        //            Session["IsSubmit"] = true;
        //            Response.Redirect("OnshowWorksSubmit.aspx");
        //        }
        //    }
        //    catch
        //    {
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('点评失败');</script>");

        //    }
        //}
        #endregion

        #region 方法

        /// <summary>
        /// 根据作品类别ID获取类别名称
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        private string GetWorksTypeName(int typeId)
        {
            //WorksType dr = Bll.WorksTypeBll.GetTypeById(typeId);
            //if (dr != null)
            //{
            //    return dr.TypeName;
            //}
            //else
            //{
            //    return "";
            //}
            return "";
        }

        /// <summary>
        /// 初始化作品导航
        /// </summary>
        /// <param name="worksId"></param>
        private void GenericNav(long worksId)
        {

        }

        /// <summary>
        /// 初始化作品信息显示控件
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="worksId">当前作品ID</param>
        private void InitControl(long userId,long worksId)
        {
            if (worksId!=0)
            {
                //getall
                Works works = Bll.WorksBll.GetWorksSubmitById(worksId.ToString());
                LbWorksName.Text = works.WorksName;
                LbWorksCode.Text = works.WorksCode;
                LbInstallationGuide.Text = works.InstallationGuide;
                LbComment.Text = works.Comment;
                LbSubmitProfile.Text = works.SubmitProfile;


                //LblDemoUrl.Text = works.DemoURL;
                //WorksType worksType=proxy.

                DivDesignIdeas.Controls.Clear();
                DivDesignIdeas.InnerText = works.DesignIdeas;

                DivKeyPoints.InnerText = works.KeyPoints;
                DivScore.InnerText = works.Score.ToString();

                if (works.WorksTypeID != null)
                {
                    WorksType worksType = Bll.WorksBll.GetWorksTypeById(works.WorksTypeID.ToString());
                    LbWorksType.Text = worksType.WorksTypeName.ToString();
                }

                //作品附件

                ShowDocuments(worksId);
                //ShowMedias(proxy, worksId, 3);
                ShowMedias(worksId, 4);
                ShowImages(worksId);
                ShowOthers(worksId);

                getReadandFavData(worksId);
                //上下文导航
                List<Works> worksList = Bll.WorksBll.GetWorks();
                int currentIndex = worksList.FindIndex(w => w.WorksID == worksId);
                //string worksNavhtml = "";
                if (currentIndex==0)//第一条
                {
                    LeftLayer.InnerHtml = "<a class='nvWorksLnk' href='WorksShow.aspx?wid=" + worksList[worksList.Count - 1].WorksID + "' title='上一作品：" + worksList[worksList.Count - 1].WorksName + "'><img src='images//Prev.png' alt=''/></a>";
                    RightLayer.InnerHtml = "<a class='nvWorksLnk' href='WorksShow.aspx?wid=" + worksList[currentIndex + 1].WorksID + "'  title='下一作品："+ worksList[currentIndex + 1].WorksName + "'><img src='images/Next.png' alt=''/></a>";

                    lnkPre.Text = "上一作品：" + worksList[worksList.Count - 1].WorksName;
                    lnkPre.NavigateUrl = "WorksShow.aspx?wid=" + worksList[worksList.Count - 1].WorksID;
                    lnkNext.Text = "下一作品：" + worksList[currentIndex + 1].WorksName;
                    lnkNext.NavigateUrl = "WorksShow.aspx?wid=" + worksList[currentIndex + 1].WorksID;
                }
                else if (currentIndex == worksList.Count-1)//
                {
                    LeftLayer.InnerHtml = "<a class='nvWorksLnk' href='WorksShow.aspx?wid=" + worksList[currentIndex - 1].WorksID + "' title='上一作品：" + worksList[currentIndex - 1].WorksName + "'><img src='images/Prev.png' alt=''/></a>";
                    RightLayer.InnerHtml = "<a class='nvWorksLnk' href='WorksShow.aspx?wid=" + worksList[0].WorksID + "' title='下一作品：" + worksList[0].WorksName + "'><img src='images/Next.png' alt=''/></a>";

                    lnkPre.Text = "上一作品：" + worksList[currentIndex - 1].WorksName;
                    lnkPre.NavigateUrl = "WorksShow.aspx?wid=" + worksList[currentIndex - 1].WorksID;
                    lnkNext.Text = "下一作品：" + worksList[0].WorksName ;
                    lnkNext.NavigateUrl = "WorksShow.aspx?wid=" + worksList[0].WorksID;
                }
                else//
                {
                    LeftLayer.InnerHtml = "<a class='nvWorksLnk' href='WorksShow.aspx?wid=" + worksList[currentIndex - 1].WorksID + "' title='上一作品：" + worksList[currentIndex - 1].WorksName + "'><img src='images/Prev.png' alt=''/></a>";
                    RightLayer.InnerHtml = "<a class='nvWorksLnk' href='WorksShow.aspx?wid=" + worksList[currentIndex + 1].WorksID + "' title='下一作品：" + worksList[currentIndex + 1].WorksName + "'><img src='images/Next.png' alt=''/></a>";

                    lnkPre.Text = "上一作品：" + worksList[currentIndex - 1].WorksName;
                    lnkPre.NavigateUrl = "WorksShow.aspx?wid=" + worksList[currentIndex - 1].WorksID;
                    lnkNext.Text = "下一作品：" + worksList[currentIndex + 1].WorksName;
                    lnkNext.NavigateUrl = "WorksShow.aspx?wid=" + worksList[currentIndex + 1].WorksID;
                }
                //WorksNavDiv.InnerHtml = worksNavhtml;
                //if (em.WorksTypeID != null)
                //{
                //    LbWorksType.Text = GetWorksTypeName((int)em.WorksTypeID);
                //}
            }


            if (userId != 0)
            {
                Favorites favWorks =Bll.UserBll.GetUserFavorites(userId.ToString()).FirstOrDefault(f => f.DomainID == 5 & f.ItemID == worksId);
                if (favWorks != null)
                {
                    BtnFav.Visible = false;
                    BtnUnFav.Visible = true;
                }
                else
                {
                    BtnFav.Visible = true;
                    BtnUnFav.Visible = false;
                }

            }
            else
            {
                LeftLayer.Visible = false;
                RightLayer.Visible = false;
            }

        }

        /// <summary>
        /// 多媒体文件（包括音视频）的显示：播放窗口
        /// </summary>
        /// <param name="serviceWorks">数据服务层接口</param>
        /// <param name="worksId">作品ID</param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        private void ShowMedias(long worksId, int typeId)
        {
            StringBuilder htmlContent = new StringBuilder();
            List<WorksFile> mediaFiles = Bll.WorksBll.GetWorksFile(worksId.ToString(), typeId.ToString());
            if (mediaFiles.Count > 0)
            {

                WorksFile mediaFile = mediaFiles.OrderByDescending(wf => wf.WorksFileID).FirstOrDefault();//因为可能有多个文档重复上传，所以此处先处理成该作品最后一次上传的文档为作品文档。
                Works works = Bll.WorksBll.GetWorksSubmitById(worksId.ToString());
                string periodId = works.PeriodID.ToString();
                Periods period = Bll.WorksBll.GetPeriodsById(periodId).FirstOrDefault();
                string siteUrl = "http://va.neu.edu.cn";
                if (period != null)
                {
                    string courseId = period.CourseID.ToString();
                    List<Course> cs = Bll.WorksBll.GetCourseById(courseId);
                    Course course = cs.FirstOrDefault();
                    if (course != null)
                    {
                        siteUrl += course.Url;
                    }
                }
                if (mediaFile != null)
                {
                    MediaDiv.Visible = true;
                    //string mediaHtml = "<iframe type='text/html' width='640' height='360'";
                    //mediaHtml += "src='"+ siteUrl + "/_layouts/15/videoembedplayer.aspx?site=" + mediaFile.FilePath + "title=1'></iframe>";
                    htmlContent.AppendLine("<div class='plist'>");
                    htmlContent.AppendLine("<a href='" + siteUrl + mediaFile.FilePath + "' name='" + mediaFile.FileName + "'");
                    htmlContent.AppendLine("<img class='listpic' src='images/start.jpg' alt='" + mediaFile.FileName + "' width='200'  height='150'>");
                    string txtExtend = mediaFile.FileName.Substring(mediaFile.FileName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower();
                    if (txtExtend == "mp3")
                    {
                        htmlContent.AppendLine("<span class='v_bq_hong'>讲解音频</span><span class='v_bg'></span>");
                    }
                    else
                    {
                        htmlContent.AppendLine("<span class='v_bq_lan'>讲解视频</span><span class='v_bg'></span>");
                    }
                    htmlContent.AppendLine("<span class='v_bq_vico'></span></a>");
                    htmlContent.AppendLine("<h2><a href='" + siteUrl + mediaFile.FilePath + "' name='" + mediaFile.FileName + "'>" + mediaFile.FileName + "</a></h2><p>" + mediaFile.Created + "</p>");
                    htmlContent.AppendLine("</div>");
                }

                //}
                MediaList.InnerHtml =  htmlContent.ToString();
            }
            else
            {
                MediaDiv.Visible = false;
            }
            //return htmlContent;
        }

        /// <summary>
        /// 文档文件的显示：在线浏览
        /// </summary>
        /// <param name="serviceWorks"></param>
        /// <param name="worksId"></param>
        /// <returns></returns>
        private void ShowDocuments(long worksId)
        {
            StringBuilder htmlContent = new StringBuilder();
            List<WorksFile> docFiles = Bll.WorksBll.GetWorksFile(worksId.ToString(), "1");
            if (docFiles.Count > 0)
            {
                SPSite spsite = SPContext.Current.Site;
                Works works = Bll.WorksBll.GetWorksSubmitById(worksId.ToString());
                string periodId = works.PeriodID.ToString();
                Periods period = Bll.WorksBll.GetPeriodsById(periodId).FirstOrDefault();
                if (period != null)
                {
                    string courseId = period.CourseID.ToString();
                    Course course = Bll.WorksBll.GetCourseById(courseId).FirstOrDefault();
                    string siteUrl = "http://va.neu.edu.cn";
                    string cUrl = "";
                    if (course != null)
                    {
                        cUrl = course.Url.Substring(0, course.Url.Length - 1);
                    }
                    siteUrl += cUrl;
                    var docFile = docFiles.OrderByDescending(f => f.WorksFileID).FirstOrDefault();//因为可能有多个文档重复上传，所以此处先处理成该作品最后一次上传的文档为作品文档。
                    //if (docFiles.Count ==1)
                    //{
                        //var file = docFiles[0];
                    if (docFile != null)
                    {
                        htmlContent.AppendLine("<fieldset><legend>作品文档:<a href='" + siteUrl + docFile.FilePath + "'>" + docFile.FileName + "</a></legend>");
                        htmlContent.AppendLine("<div style='width:100%;text-align:center'><iframe src='" + siteUrl + "/_layouts/15/WopiFrame.aspx?sourcedoc=" + cUrl + HttpUtility.UrlEncode(docFile.FilePath) + "&action=embedview' width='600px' height='400px' frameborder='0'></iframe></div>");
                        htmlContent.AppendLine("</fieldset>");
                    }

                    //}
                    //else
                    //{
                    //    htmlContent.AppendLine("<div>作品文档:</div>");
                    //    htmlContent.AppendLine("<table style='border:1px solid #666666'>");
                    //    htmlContent.AppendLine("<tr><td valign='top'>");
                    //    htmlContent.AppendLine("<fieldset><legend>文档列表</legend>");
                    //    foreach (var file in docFiles)
                    //    {
                    //        string url = siteUrl + "/_layouts/15/WopiFrame.aspx?sourcedoc= " + cUrl + HttpUtility.UrlEncode(docFiles[0].FilePath) + "&action=embedview";
                    //        htmlContent.AppendLine("<dt onClick='return showframe('" + url + "')' style='cursor: pointer;'>"+ file.FileName+ "</dt>");
                    //    }
                    //    htmlContent.AppendLine("</fieldset>");
                    //    htmlContent.AppendLine("</td><td>");
                    //    htmlContent.AppendLine("<fieldset style='border:1px solid #444444'><legend>文档预览</legend>");
                    //    htmlContent.AppendLine("<iframe id='myIframe' src='"+ siteUrl + "/_layouts/15/WopiFrame.aspx?sourcedoc=" + cUrl + HttpUtility.UrlEncode(docFiles[0].FilePath) + "&action=embedview' width='400px' height='300px' frameborder='0'></iframe>");
                    //    htmlContent.AppendLine("</fieldset>");
                    //    htmlContent.AppendLine("</td></tr>");
                    //    htmlContent.AppendLine("</table>");
                    //}
                }
                DocsDiv.InnerHtml =  htmlContent.ToString();
            }
            else
            {
                DocsDiv.Visible = false;
            }
        }

        /// <summary>
        /// 图片文件的显示：相册
        /// </summary>
        /// <param name="serviceWorks"></param>
        /// <param name="worksId"></param>
        /// <returns></returns>
        private void ShowImages(long worksId)
        {
            StringBuilder htmlContent = new StringBuilder();
            List<WorksFile> imagesList = Bll.WorksBll.GetWorksFile(worksId.ToString(), "2");
            if (imagesList.Count > 0)
            {
                Works works = Bll.WorksBll.GetWorksSubmitById(worksId.ToString());
                string periodId = works.PeriodID.ToString();
                Periods period = Bll.WorksBll.GetPeriodsById(periodId).FirstOrDefault();
                string courseId = period.CourseID.ToString();
                Course course = Bll.WorksBll.GetCourseById(courseId).FirstOrDefault();
                string siteUrl = "http://va.neu.edu.cn";

                if (course != null)
                {
                    siteUrl += course.Url;
                }
                htmlContent.AppendLine("<fieldset><legend>作品图片</legend>");
                htmlContent.AppendLine("<div class='box'>");
                htmlContent.AppendLine("<div id ='imgs' class='imgs'>");
                foreach (var file in imagesList)
                {
                    htmlContent.AppendLine("<img src='" + siteUrl + file.FilePath + "' onClick='$(this).ImgZoomIn();' height='200'>");
                }
                htmlContent.AppendLine("</div>");
                htmlContent.AppendLine("</div>");
                htmlContent.AppendLine("</fieldset>");
                ImagesDiv.InnerHtml = htmlContent.ToString();
            }
            else
            {
                ImagesDiv.Visible = false;
            }

        }

        /// <summary>
        /// 其它文件显示：下载列表
        /// </summary>
        /// <param name="serviceWorks"></param>
        /// <param name="worksId"></param>
        /// <returns></returns>
        private void ShowOthers(long worksId)
        {
            StringBuilder htmlContent = new StringBuilder();
            List<WorksFile> otherFiles = Bll.WorksBll.GetWorksFile(worksId.ToString(), "5");
            if (otherFiles.Count > 0)
            {
                htmlContent.AppendLine("<fieldset><le其它附件：  （点击文件名可下载）</span><br/>");
                foreach (var file in otherFiles)
                {
                    htmlContent.AppendLine("<a href='" + file.FilePath + "'>" + file.FileName + "</a>");
                    htmlContent.AppendLine("<br/>");
                }
                OthersDiv.InnerHtml =htmlContent.ToString();
            }
            else
            {
                OthersDiv.Visible=false;
            }
        }

        /// <summary>
        /// 作品视频
        /// </summary>
        /// <param name="demoUrl"></param>
        private void ShowVideo(string demoUrl)
        {
            string txtVideo = "<embed src=\"http://player.youku.com/player.php/sid/XMzQ4NTI0MzQ0/v.swf\" allowFullScreen=\"true\" quality=\"high\" width=\"480\" height=\"400\" align=\"middle\" allowScriptAccess=\"always\" type=\"application/x-shockwave-flash\"></embed>";
            string txtVideo1 = "视频无法显示";
            DivViewShow.Controls.Clear();
            if (demoUrl.Length > 0)
            {
                string tmpdemoUrl = demoUrl.ToLower();
                int start;
                int end;
                int s1 = 0;
                int s2 = 0;
                string txtType = "tudou";
                try
                {
                    if (tmpdemoUrl.Contains("tudou") || tmpdemoUrl.Contains("sohu") || tmpdemoUrl.Contains("youku"))
                    {
                        if (tmpdemoUrl.Contains("tudou"))
                        {
                            txtType = "tudou";
                            //http://www.tudou.com/programs/view/JccfI6enRlM/
                            txtVideo = "<embed src=\"http://www.tudou.com/v/XMzQ4NTI0MzQ0/&resourceId=0_05_05_99/v.swf\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" allowfullscreen=\"true\" wmode=\"opaque\" width=\"480\" height=\"400\"></embed>";
                            txtVideo1 = GetDemoUrl(tmpdemoUrl, demoUrl, txtType);
                            if (txtVideo1.Contains("/?"))
                            {
                                txtVideo1 = txtVideo1.Substring(0, txtVideo1.IndexOf("/?") + 1);
                            }
                            if (txtVideo1.EndsWith(".html"))
                            {
                                txtVideo = "<embed src=\"http://www.tudou.com/l/XMzQ4NTI0MzQ0/&rpid=111905378&resourceId=111905378_05_05_99&iid=140062282/v.swf\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" allowfullscreen=\"true\" wmode=\"opaque\" width=\"480\" height=\"400\"></embed>";
                                txtVideo1 = txtVideo1.Substring(0, txtVideo1.LastIndexOf("/"));
                            }

                            if (!txtVideo1.EndsWith("/"))
                                txtVideo1 = txtVideo1 + "/";
                            s1 = txtVideo1.LastIndexOf("/", txtVideo1.Length - 2);
                            s2 = txtVideo1.Length - 2;
                        }
                        else if (tmpdemoUrl.Contains("sohu"))
                        {
                            txtType = "sohu";
                            //http://my.tv.sohu.com/u/vw/20027857
                            txtVideo = "<object width= height=506><param name=\"movie\" value=\"http://share.vrs.sohu.com/my/v.swf&topBar=1&id=XMzQ4NTI0MzQ0&autoplay=false\"></param><param name=\"allowFullScreen\" value=\"true\"></param><param name=\"allowscriptaccess\" value=\"always\"></param><param name=\"wmode\" value=\"Transparent\"></param><embed width=640 height=506 wmode=\"Transparent\" allowfullscreen=\"true\" allowscriptaccess=\"always\" quality=\"high\" src=\"http://share.vrs.sohu.com/my/v.swf&topBar=1&id=XMzQ4NTI0MzQ0&autoplay=false\" type=\"application/x-shockwave-flash\"/></embed></object>";
                            txtVideo1 = GetDemoUrl(tmpdemoUrl, demoUrl, txtType);
                            s1 = txtVideo1.LastIndexOf("/");
                            s2 = txtVideo1.Length - 1;
                        }
                        else
                        {
                            txtType = "youku";
                            //http://v.youku.com/v_show/id_XMzkzODUzODA0.html
                            txtVideo1 = GetDemoUrl(tmpdemoUrl, demoUrl, txtType);
                            s1 = txtVideo1.IndexOf("id_") + 2;
                            s2 = txtVideo1.LastIndexOf(".html") - 1;
                        }

                        txtVideo1 = txtVideo1.Substring(s1 + 1, s2 - s1);
                        DivViewShow.Controls.Add(new LiteralControl(txtVideo.Replace("XMzQ4NTI0MzQ0", txtVideo1)));
                        return;
                    }
                }
                catch
                {

                }
                DivViewShow.Controls.Add(new LiteralControl(txtVideo1));
            }

        }

        /// <summary>
        /// 作品演示地址
        /// </summary>
        /// <param name="tmpdemoUrl"></param>
        /// <param name="demoUrl"></param>
        /// <param name="txtType"></param>
        /// <returns></returns>
        private string GetDemoUrl(string tmpdemoUrl, string demoUrl, string txtType)
        {
            int start = tmpdemoUrl.LastIndexOf("http:", tmpdemoUrl.IndexOf(txtType, StringComparison.Ordinal), StringComparison.Ordinal);
            var end = tmpdemoUrl.IndexOf(";", start + 1, StringComparison.Ordinal);
            if (end < 0)
                end = tmpdemoUrl.IndexOf(" ", start + 1, StringComparison.Ordinal);
            if (end < 0)
                end = tmpdemoUrl.Length;
            string txtVideo1 = demoUrl.Substring(start, end - start).Trim();
            return txtVideo1;

        }

        /// <summary>
        /// 作品附件
        /// </summary>
        private void GetWorksFile(long worksId,int typeId)
        {
            SPSite site = SPContext.Current.Site;
            string siteUrl = site.Url;
            siteUrl = siteUrl.Substring(siteUrl.IndexOf("/", 8));
            List<WorksFile> worksFileList =Bll.WorksFileBll.GetFilesByWorksID(worksId);
            StringBuilder txtContent = new StringBuilder();
            foreach (WorksFile wFile in worksFileList)
            {
                int? typeID = wFile.Type;
                //int tid = 5;
                //if (typeID!=null)
                //{
                //    tid = typeID;
                //}

                FileType ft = Bll.WorksFileBll.GetFileTypeByID(typeID);
                string typeName = ft.TypeName;
                if (ft.ParentID!=0)
                {
                    typeName =Bll.WorksFileBll.GetFileTypeByID(ft.ParentID).TypeName;
                }
                typeId = 0;//1:文档；2：图片；3：音频；4：视频；5：其它；其中3和3统归为媒体类；0表示所有
                switch (typeId)
                {

                    case 1://图片附件，显示为图片
                        txtContent.AppendLine("<img src='" + wFile.FilePath + "' width='500px'/><br />" + wFile.FileName);
                        txtContent.AppendLine("<br/>");
                        break;
                    case 3:case 4://音视频附件：显示为播放窗口
                        txtContent.AppendLine("<iframe type='text/html' width='400' height='300' src='" + siteUrl + "/_layouts/15/videoembedplayer.aspx?site=e6bc1a4ae9eb4d22a35ac16c80eedb79&amp;web=0764a19a1b224278861c15141c7a91a8&amp;folder=57bfb4d317814fd49078be8cd9da899f&amp;img=''&amp;title=1&amp;lHome=1&amp;lOwner=1'></iframe>");
                        break;
                    case 2://文档附件：在线浏览
                        txtContent.Append("<iframe src='" + siteUrl + "/_layouts/15/WopiFrame.aspx?sourcedoc=" + wFile.FilePath + "&action=embedview' width='379px' height='252px' frameborder='0'></iframe>");
                        txtContent.AppendLine("<br/>");
                        break;
                    default://其它附件：提供下载
                        txtContent.AppendLine("<a href='" + wFile.FilePath + "'>" + wFile.FileName + "<br />");
                        txtContent.AppendLine("<br/>");
                        break;

                }

            }
            if (txtContent.ToString().Length > 0)
            {
                txtContent.Remove(txtContent.Length - 5, 5);
            }
            DivWorksFile.Controls.Clear();
            DivWorksFile.Controls.Add(new LiteralControl(txtContent.ToString()));
        }


        /// <summary>
        /// 初始化作品视频显示
        /// </summary>
        private void FillVideoShow()
        {
            //DataSet ds = GetWorksDocVideo;//3
            //string txtVideo1 = "";
            //int i = 1;
            //divDovVideo.Controls.Clear();
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    txtVideo1 = FillVideoControl("文档视频：" + (string)dr["FileName"], (string)dr["FilePath"], i.ToString(), "true");
            //    divDovVideo.Controls.Add(new LiteralControl(txtVideo1));
            //    i += 1;
            //}
            //ds = GetWorksViewVideo;//4
            //divViewShow.Controls.Clear();
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    txtVideo1 = FillVideoControl("讲解视频：" + (string)dr["FileName"], (string)dr["FilePath"], i.ToString(), "false");
            //    divViewShow.Controls.Add(new LiteralControl(txtVideo1));
            //    i += 1;
            //}
        }

        /// <summary>
        /// 生成一个媒体文件播放控件
        /// </summary>
        /// <param name="mediaTitle">媒体文件名称</param>
        /// <param name="mediaUrl">媒体文件地址</param>
        /// <param name="i">播放器序号</param>
        /// <param name="autoPlay">自动播放设置</param>
        /// <returns>一个包含播放器的div的Html代码</returns>
        private string FillVideoControl(string mediaTitle, string mediaUrl, string i, string autoPlay)
        {
            StringBuilder txtContent = new StringBuilder();
            txtContent.AppendLine("<div id=\"MediaPlayerHost" + i + "\">");
            txtContent.AppendLine("<script type=\"text/javascript\" >");
            txtContent.AppendLine("{");
            txtContent.AppendLine("mediaPlayer.createMediaPlayer(");
            txtContent.AppendLine("document.getElementById('MediaPlayerHost" + i + "'),");
            txtContent.AppendLine("'MediaPlayerHost',");
            txtContent.AppendLine("'600px', '470px',");
            txtContent.AppendLine("{");
            txtContent.AppendLine("displayMode: 'Inline',");
            txtContent.AppendLine("mediaTitle: '" + DealWorkfileName(mediaTitle) + "',");
            txtContent.AppendLine("mediaSource: '" + Common.Common.SPWeb.Url + mediaUrl + "',");
            string txtExtend = mediaTitle.Substring(mediaTitle.LastIndexOf(".") + 1);
            string previewFile = Common.Common.SPWeb.Url + "/_layouts/15/WorkEvaluate/images/";
            if (txtExtend.ToLower() == "mp3")
            {
                previewFile += "yinpin.jpg";
            }
            else
            {
                previewFile += "shipin.jpg";
            }
            txtContent.AppendLine("previewImageSource: '" + previewFile + "',");
            txtContent.AppendLine("autoPlay: " + autoPlay + ",");
            txtContent.AppendLine("loop: false,");
            txtContent.AppendLine("mediaFileExtensions: 'wmv;wma;avi;mpg;mp3;',");
            txtContent.AppendLine(" silverlightMediaExtensions: 'wmv;wma;mp3;'");
            txtContent.AppendLine("});");
            txtContent.AppendLine("}");
            txtContent.AppendLine("</script>");
            txtContent.AppendLine("</div>");
            return txtContent.ToString();
        }

        /// <summary>
        /// 文件名处理（取出文件名中的初始文件名）
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private static string DealWorkfileName(string fileName)
        {
            string txtFile = fileName.Substring(0, fileName.IndexOf("-")) + fileName.Substring(fileName.LastIndexOf("-") + 1);
            return txtFile;
        }

        ////师生点评
        //private void GetComments(long worksId)
        //{
        //    divScore.Controls.Clear();
        //    List<CSWorksExpertUser> ds = GetDSWorksComments;
        //    List<CSWorksExpertUser> dv = ds.Where(p => p.UserID == DAL.Common.LoginID).ToList();
        //    if (dv.Count > 0)
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>YiComments();</script>");

        //    lblPersons.Text = ds.Count.ToString();
        //    gvComments.DataSource = dv;
        //    gvComments.DataBind();
        //    float avg = 0;
        //    int tCount = 0;
        //    foreach (CSWorksExpertUser dr in ds)
        //    {
        //        if (dr.Score.HasValue && dr.Score > 0)
        //        {
        //            avg = avg + (float)dr.Score;
        //            tCount = tCount + 1;
        //        }
        //    }
        //    if (tCount > 0)
        //        avg = avg / tCount;
        //    int score = (int)avg;
        //    string txt = "";
        //    for (int i = 0; i < score; i++)
        //    {
        //        txt += "<img src='images/star_red.gif'/>";
        //    }
        //    int j = score;
        //    if (avg > score)
        //    {
        //        j = j + 1;
        //        txt += "<img src='images/star_red2.gif'/>";

        //    }
        //    for (int i = j; i < 5; i++)
        //    {
        //        txt += "<img src='images/star_gray.gif'/>";

        //    }
        //    divScore.Controls.Add(new LiteralControl(txt));
        //}

        /// <summary>
        /// 初始化作品图片显示
        /// </summary>
        /// <param name="worksId"></param>
        /// <returns></returns>
        public static string ImagesHtml(long worksId)//
        {
            string imageshtml = "";
            List<WorksFile> imageFiles = Bll.WorksBll.GetWorksFile(worksId.ToString(),"2");
            foreach (var file in imageFiles)
            {

            }
            return imageshtml;
        }

        /// <summary>
        /// 指定作品的附件显示
        /// </summary>
        /// <param name="worksId">作品ID</param>
        /// <param name="typeId">文件类型ID</param>
        /// <returns></returns>
        public static StringBuilder GenerateFileHtml(long worksId,int typeId)
        {
            StringBuilder htmlContent = new StringBuilder();

                //获取指定作品指定文件类型的附件列表

                //switch (typeId)
                //{
                //    case 1://文档
                //        var files = proxy.GetWorksFile(worksId.ToString(), "1");
                //        htmlContent.AppendLine(proxy,worksId);
                //        break;
                //    case 2:
                //        var files = proxy.GetWorksFile(worksId.ToString(), "2");
                //        htmlContent.AppendLine(ShowImages(files));
                //        break;
                //    case 3: case 4:
                //        htmlContent.AppendLine(ShowMedias(files));
                //        break;
                //    default:
                //        htmlContent.AppendLine(ShowOthers(files));
                //        break;
                //}
            return htmlContent;
        }
        #endregion
    }
}
