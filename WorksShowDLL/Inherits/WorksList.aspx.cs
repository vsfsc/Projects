using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace WorksShowDll.Inherits
{
    public class WorksList : LayoutsPageBase
    {
        #region 控件
        protected HtmlGenericControl worksListDiv;
        protected LinkButton lnkPre;
        protected LinkButton lnkNext;
        protected LinkButton lnkFirst;
        protected LinkButton lnkLast;
        protected Label lbPages;

        protected Label lbPageTitle;
        protected HiddenField hfCurrentPage;
        protected LinkButton lnkNews;
        protected LinkButton lnkMy;
        protected LinkButton lnkAll;
        protected LinkButton lnkHots;
        protected DropDownList ddlWebSites;
        protected DropDownList ddlWorksTypes;

        protected HiddenField hfItemCount;

        #endregion

        #region 变量


        #endregion

        #region 事件

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<CSMyWorks> worksList = GetData(0,0, 0);
                GenericList(worksList, 12, 1);
                lbPageTitle.Text = "所有作品";
                hfCurrentPage.Value = "1";
                long pageCount = (long)Math.Ceiling((double)worksList.Count / 12);
                hfItemCount.Value = pageCount.ToString();
                lbPages.Text = "1 /" + pageCount;



                //SPUser spUser = SPContext.Current.Web.CurrentUser;
                //if (spUser != null)
                //{
                //    string username = spUser.LoginName;
                //    username = username.Substring(username.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                //    using (ChannelFactory<IServiceUser> cfUser = new ChannelFactory<IServiceUser>("ServiceUser"))
                //    {
                //        IServiceUser user = cfUser.CreateChannel();
                //        if (user == null) throw new ArgumentNullException(nameof(user));
                //        long userId = user.GetUserIdByLoginName(username);

                //    }
                //}
                BindTypes();
                BindWebSites();
            }
            
            //分页按钮点击事件
            lnkFirst.Click +=lnkFirst_Click;
            lnkLast.Click += LnkLast_Click;
            lnkNext.Click += LnkNext_Click;
            lnkPre.Click += LnkPre_Click;

            //筛选按钮点击事件
            lnkAll.Click += LnkAll_Click;
            lnkMy.Click += LnkMy_Click;
            lnkHots.Click += LnkHots_Click;
            lnkNews.Click += LnkNews_Click;

            //作品来源网站选择事件绑定
            ddlWebSites.SelectedIndexChanged += DdlWebSites_SelectedIndexChanged;
            //作品类别选择事件绑定
            ddlWorksTypes.SelectedIndexChanged += DdlWorksTypes_SelectedIndexChanged;
        }

        
        private void BindWebSites()
        {
            ddlWebSites.DataSource = GetWebSites();
            ddlWebSites.DataTextField = "CourseName";
            ddlWebSites.DataValueField = "CourseID";
            ddlWebSites.DataBind();
        }

        /// <summary>
        /// 获取作品来源网站列表
        /// </summary>
        /// <returns></returns>
        private List<Course> GetWebSites()
        {
            List<Course> wCurses=Bll.WorksBll.GetCourses();
            return wCurses; 
        }

        /// <summary>
        /// 作品类别下拉列表控件数据绑定
        /// </summary>
        private void BindTypes()
        {
            ddlWorksTypes.DataSource = GetWorksTypes();
            ddlWorksTypes.DataTextField = "WorksTypeName";
            ddlWorksTypes.DataValueField = "WorksTypeID";
            ddlWorksTypes.DataBind();
        }

        /// <summary>
        /// 获取作品类别
        /// </summary>
        /// <returns></returns>
        private List<WorksType> GetWorksTypes()
        {
            List<WorksType> wTypes= Bll.WorksBll.GetWorksTypes();            
            return wTypes;
        }

        /// <summary>
        /// 我的作品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkMy_Click(object sender, EventArgs e)
        {
            SPUser spUser = SPContext.Current.Web.CurrentUser;
            if (spUser!=null)
            {
                string username = spUser.LoginName;
                username = username.Substring(username.LastIndexOf("\\", StringComparison.Ordinal) + 1);

                    //Bll.UserBll user=new Bll.UserBll();
                    //if (user == null) throw new ArgumentNullException("user");
                long userId = Bll.UserBll.GetUserIdByLoginName(username);
                    List<CSMyWorks> worksList = GetData(userId,0,0);
                    GenericList(worksList, 12, 1);
                    hfCurrentPage.Value = "1";

                    long pageCount = (long)Math.Ceiling((double)worksList.Count / 12);
                    hfItemCount.Value = pageCount.ToString();
                    lbPages.Text = "1 /" + pageCount;

                    lbPageTitle.Text = "我的作品";//网页标题
                }
        }

        /// <summary>
        /// 课程选择筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DdlWebSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pgTitle = "";
            int courseId = 0;
            if (ddlWebSites.SelectedValue != null)
            {
                courseId = int.Parse(ddlWebSites.SelectedValue);
                ViewState["Course"] = courseId+"，作品网站："+ ddlWebSites.SelectedItem+" ";
                pgTitle += "作品网站：" + ddlWebSites.SelectedItem + "";
            }
            int typeId = 0;
            if (ViewState["Type"] != null)
            {
                string[] tp = ViewState["Type"].ToString().Split('，');
                typeId = int.Parse(tp[0]);
                pgTitle += tp[1];
            }
            lbPageTitle.Text = pgTitle;
            List<CSMyWorks> worksList = GetData(0, typeId, courseId);
            GenericList(worksList, 12, 1);
            hfCurrentPage.Value = "1";

            long pageCount = (long)Math.Ceiling((double)worksList.Count / 12);
            hfItemCount.Value = pageCount.ToString();
            lbPages.Text = "1 /" + pageCount;
        }


        /// <summary>
        /// 作品类别筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DdlWorksTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pgTitle = "";

            int courseId = 0;
            if (ViewState["Course"] != null)
            {
                string[] cs = ViewState["Course"].ToString().Split('，');
                courseId = int.Parse(cs[0]);
                pgTitle += cs[1];
            }
            int worksTypeId = 0;
            if (ddlWorksTypes.SelectedValue != null)
            {
                worksTypeId = int.Parse(ddlWorksTypes.SelectedValue);
                ViewState["Type"] = worksTypeId+ "，作品类别：" + ddlWorksTypes.SelectedItem;
                 pgTitle+="作品类别：" + ddlWorksTypes.SelectedItem;
            }
            lbPageTitle.Text = pgTitle;

            List<CSMyWorks> worksList = GetData(0, worksTypeId, courseId);
            GenericList(worksList, 12, 1);
            hfCurrentPage.Value = "1";

            long pageCount = (long)Math.Ceiling((double)worksList.Count / 12);
            hfItemCount.Value = pageCount.ToString();
            lbPages.Text = "1 /" + pageCount;
        }

        /// <summary>
        /// 最新作品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkNews_Click(object sender, EventArgs e)
        {
            List<CSMyWorks> worksList = GetData(0, 0,0).OrderByDescending(w=>w.Created).ToList();
            GenericList(worksList, 12, 1);
            hfCurrentPage.Value = "1";

            long pageCount = (long)Math.Ceiling((double)worksList.Count / 12);
            hfItemCount.Value = pageCount.ToString();
            lbPages.Text = "1 /" + pageCount;

            lbPageTitle.Text = "最新作品";//网页标题
        }

        /// <summary>
        /// 热门作品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkHots_Click(object sender, EventArgs e)
        {
            List<CSMyWorks> worksList = GetData(0, 0,0).OrderByDescending(w=>w.Score).ToList();
            GenericList(worksList, 12, 1);
            hfCurrentPage.Value = "1";
            long pageCount = (long)Math.Ceiling((double)worksList.Count / 12);
            hfItemCount.Value = pageCount.ToString();
            lbPages.Text = "1 /" + pageCount;

            lbPageTitle.Text = "热门作品";//网页标题
        }

        /// <summary>
        /// 所有作品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LnkAll_Click(object sender, EventArgs e)
        {
            List<CSMyWorks> worksList = GetData(0, 0,0);
            GenericList(worksList, 12, 1);
            hfCurrentPage.Value = "1";

            long pageCount = (long)Math.Ceiling((double)worksList.Count / 12);
            hfItemCount.Value = pageCount.ToString();
            lbPages.Text = "1 /" + pageCount;

            lbPageTitle.Text = "所有作品";//网页标题
        }
        
        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LnkPre_Click(object sender, EventArgs e)
        {
            long currentPage = 1;
            if (hfCurrentPage.Value!="")
            {
                currentPage = long.Parse(hfCurrentPage.Value);
            }
            
            long pageCount = 1;
            if (hfCurrentPage.Value != "")
            {
                pageCount = long.Parse(hfItemCount.Value);
            }
            if (currentPage-1>0)
            {
                List<CSMyWorks> worksList = GetData(0, 0,0);
                GenericList(worksList, 12, currentPage - 1);
                lbPages.Text = (currentPage - 1) + " / " + pageCount;
                hfCurrentPage.Value = (currentPage - 1).ToString();
            }
            else
            {
                Common.Common.ShowMessage(Page,GetType(),"已经是第一页了！");
            }

        }
        
        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LnkNext_Click(object sender, EventArgs e)
        {
            long currentPage = 1;
            if (hfCurrentPage.Value != "")
            {
                currentPage = long.Parse(hfCurrentPage.Value);
            }

            long pageCount = 1;
            if (hfCurrentPage.Value != "")
            {
                pageCount = long.Parse(hfItemCount.Value);
            }
            if (currentPage + 1 <= pageCount)
            {
                List<CSMyWorks> worksList = GetData(0, 0,0);
                GenericList(worksList, 12, currentPage + 1);
                lbPages.Text = (currentPage + 1) + " / " + pageCount;
                hfCurrentPage.Value = (currentPage + 1).ToString();
            }
            else
            {
                Common.Common.ShowMessage(Page, GetType(), "已经是最后一页了！");
                hfCurrentPage.Value = currentPage.ToString();
            }
        }
        
        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LnkLast_Click(object sender, EventArgs e)
        {
           
            long pageCount = 1;
            if (hfCurrentPage.Value != "")
            {
                pageCount = long.Parse(hfItemCount.Value);
            }
            List<CSMyWorks> worksList = GetData(0, 0,0);
            GenericList(worksList, 12, pageCount);
            lbPages.Text = pageCount+ " / " + pageCount;
            hfCurrentPage.Value = pageCount.ToString();
        }
        
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkFirst_Click(object sender, EventArgs e)
        {
            long pageCount = 1;
            if (hfCurrentPage.Value != "")
            {
                pageCount = long.Parse(hfItemCount.Value);
            }
            List<CSMyWorks> worksList = GetData(0, 0,0);
            GenericList(worksList, 12, 1);
            lbPages.Text = "1 / " + pageCount;
            hfCurrentPage.Value = "1";
        }
        #endregion

        #region 方法

        /// <summary>
        /// 获取作品数据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="worksTypeId">作品类别ID</param>
        /// <param name="courseId">课程ID</param>
        /// <returns></returns>
        private List<CSMyWorks> GetData(long userId,int worksTypeId,long courseId)
        {

                List<CSMyWorks> works = Bll.WorksBll.GetMyWorks();

                if (userId!=0)
                {
                    works = works.Where(w => w.UserID == userId).ToList();
                }
                if (worksTypeId!=0)
                {
                    works = works.Where(w => w.WorksTypeID == worksTypeId).ToList();
                }
                if (courseId!=0)
                {
                    works = works.Where(w => w.CourseID == courseId).ToList();
                }
                return works;

            
        }

        /// <summary>
        /// 生成列表页面
        /// </summary>
        /// <param name="worksList">作品列表数据源</param>
        /// <param name="pageSize">每页条目数</param>
        /// <param name="pageNum">当前页数</param>
        private void GenericList(List<CSMyWorks> worksList,long pageSize,long pageNum)
        {
            
            long worksCount = worksList.Count;
            if (worksCount>pageSize)
            {
                if (0 == pageNum)//最后一页
                {
                    pageSize = worksCount - pageSize*(pageNum - 1);
                }
                worksList = worksList.Skip((int)(pageSize*(pageNum-1))).Take((int)pageSize).ToList();
            }
            
            
            if (worksList.Count>0)
            {
                HtmlContainerControl ulControl = new HtmlGenericControl("ul");
                
                foreach (var works in worksList)
                {
                    HtmlContainerControl liControl = new HtmlGenericControl("li");
                    StringBuilder liBuilder = new StringBuilder();
                    
                    //填充作品信息
                    liBuilder.AppendLine("<a class='cbp-vm-image' href='WorksShow.aspx?wid=" + works.WorksID + "' target='_blank' title='" + works.WorksName + "'><img src='images/works.png'></a>");
                    liBuilder.AppendLine("<h3 class='cbp-vm-title'><a href='WorksShow.aspx?wid=" + works.WorksID + "' target='_blank'>" + works.WorksName + "</a></h3>");                        
                        if (works.PeriodID != null)
                        {
                            Periods period = Bll.WorksBll.GetPeriodsById(works.PeriodID.ToString()).FirstOrDefault();
                            if (period?.CourseID != null)
                            {
                                liBuilder.AppendLine("<div class='cbp-vm-price'>" + period.PeriodTitle + "</div>");//作品所属期次
                                string courseId = period.CourseID.ToString();
                                Course course = Bll.WorksBll.GetCourseById(courseId).FirstOrDefault();
                                if (course != null)
                                {
                                    string courseUrl = SPContext.Current.Site.RootWeb.Url  + course.Url;
                                    liBuilder.AppendLine("<div class='cbp-vm-price'><a href='" + courseUrl + "' target='_blank'>" + course.CourseName + "</a></div>");//作品所属网站
                                }
                            }
                        }

                        if (works.WorksTypeID != null)
                        {
                            WorksType worksType = Bll.WorksBll.GetWorksTypeById(works.WorksTypeID.ToString());
                            liBuilder.AppendLine("<div class='cbp-vm-price'>" + worksType.WorksTypeName + "</div>");//作品类别
                        }
                    
                    liBuilder.AppendLine("<div class='cbp-vm-price'>" + $"{works.Created:d}" + "</div>");
                    string desc = Common.Common.NoHtml(works.DesignIdeas);
                    if (desc.Length > 45)
                    {
                        desc = desc.Substring(0, 45) + "...";
                    }
                    liBuilder.AppendLine("<br/><div class='cbp-vm-details'><a href='WorksShow.aspx?wid=" + works.WorksID + "' target='_blank'>" + desc + "</a></div> ");//作品介绍

                    liControl.Controls.Add(new LiteralControl(liBuilder.ToString()));
                    //liBuilder.AppendLine("</li>");
                    ulControl.Controls.Add(liControl);
                }

                //liBuilder.AppendLine("</ul>");
                worksListDiv.Controls.Clear();
                worksListDiv.Controls.Add(ulControl);
            }
            
            //worksListDiv.InnerHtml = liBuilder.ToString();
        }

        /// <summary>
        /// 删除指定作品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IBtnDel_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            string worksId = btn.CommandArgument;
            Bll.WorksBll.Delete(worksId);

        }

        /// <summary>
        /// 生成分页控件代码
        /// </summary>
        /// <param name="itemCount">数据条目数</param>
        /// <param name="pageSize">每页条目数</param>
        /// <returns></returns>
        private StringBuilder GenericPageCtrl(long itemCount, int pageSize)
        {
            StringBuilder pageCtrl = new StringBuilder();
            if (itemCount>pageSize)//超过一页才分页
            {
                long pageCount = (long)Math.Ceiling((double)itemCount / pageSize);
                pageCtrl.AppendLine("首页");
                pageCtrl.AppendLine("上一页");
                pageCtrl.AppendLine("下一页");
                pageCtrl.AppendLine("尾页");
                pageCtrl.AppendLine("当前第"+ hfCurrentPage.Value + "页");
                pageCtrl.AppendLine("共"+ pageCount + "页");
            }
            return pageCtrl;
        }
        #endregion 
    }
}
