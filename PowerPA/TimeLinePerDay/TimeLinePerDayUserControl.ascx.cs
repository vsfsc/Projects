using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PowerPA.TimeLinePerDay
{
    public partial class TimeLinePerDayUserControl : UserControl
    {
        public TimeLinePerDay webObj;

        protected void Page_Load(object sender, EventArgs e)
        {
            string taskID = GetQueryString("ID");
            try
            {
                if (taskID != "")
                {
                    int Id = int.Parse(taskID);
                    GetTaskDetail(Id);
                }
            }
            catch (Exception ex)
            {

                divContainter.InnerHtml=ex.ToString();
            }
        }

        /// <summary>
        /// 获取当前用户ID，若未登录，则返回0
        /// </summary>
        /// <returns>用户ID，0表示未登录</returns>
        private int GetUserId()
        {
            int userId = 0;
            SPUser cUser = SPContext.Current.Web.CurrentUser;
            if (cUser != null)
            {
                userId = cUser.ID;
            }
            return userId;
        }

        /// <summary>
        /// 获取url字符串参数，返回参数值字符串
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="url">url字符串</param>
        /// <returns></returns>
        public string GetQueryString(string name)
        {
            string url = Request.Url.ToString();
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(url);
            foreach (Match m in mc)
            {
                if (m.Result("$2").Equals(name))
                {
                    return m.Result("$3");
                }
            }
            return "";
        }

        private void GetTaskDetail(int taskId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
          {
              using (SPSite spSite=SPContext.Current.Site)
              {
                  using (SPWeb spWeb=spSite.OpenWeb())
                  {
                      string taskList = webObj.ListName;
                      SPList spList = spWeb.Lists.TryGetList(taskList);
                      if (spList!=null)
                      {
                          SPListItem task = spList.GetItemById(taskId);
                          if (task!=null)
                          {
                              string title=task["Title"].ToString();
                              if (task["StartDate"]!=null&&task["DueDate"]!=null)
                              {
                                  StringBuilder sb = new StringBuilder();
                                  sb.AppendLine("<div id='skill'>");

                                  DateTime startDate = DateTime.Parse(task["StartDate"].ToString()).Date;
                                  DateTime dueDate = DateTime.Parse(task["DueDate"].ToString()).Date;
                                  if (dueDate<startDate)
                                  {
                                      divContainter.InnerHtml = "截止日期不能早于开始日期，请修改！";
                                  }
                                  else
                                  {
                                      int spanMins = 0;
                                      if (dueDate==startDate)
                                      {
                                          spanMins = 8 * 60;
                                      }
                                      else
                                      {
                                          spanMins = TimeDiff(startDate, dueDate).Days * 8 * 60;
                                      }
                                      int durings = 0;
                                      DataTable dt = GetAllActivitiesByTaskId(taskId);
                                      if (dt != null)
                                      {
                                          object result = DBNull.Value;
                                          result = dt.Compute("sum(During)", "1=1");
                                          if (result != DBNull.Value)
                                          {
                                              durings = int.Parse(result.ToString());
                                          }
                                      }
                                      float percent = 0;
                                      if (durings <= 0)//尚未执行过该任务
                                      {
                                          percent = 0;
                                          sb.AppendLine("<div><span style='font-size:14px;font-weight:bold;'>任务状态(0%)</span>：<span style='color:#177fcb;'>该任务尚未开始执行!</span></div>");
                                          sb.AppendLine("<div class='skillbar css' title='剩余工作量：100%'>");
                                      }
                                      else if (durings > 0 && durings <= spanMins)
                                      {
                                          percent = durings * 100 / spanMins;
                                          sb.AppendLine("<div><span style='font-size:14px;font-weight:bold;'>任务状态(" + percent + "%)</span>：<span style='color:#83bf51;'>任务进行中，尚未完成!</span></div>");
                                          sb.AppendLine("<div class='skillbar nodejs' title='剩余工作量：" + (100 - percent).ToString() + "%'>");
                                      }
                                      else
                                      {
                                          percent = 99;
                                          sb.AppendLine("<div><span style='font-size:14px;font-weight:bold;'>任务状态(99%)</span>：<span style='color:#cc2b2c;'>任务未能按预期完成!</span></div>");
                                          sb.AppendLine("<div class='skillbar meteor' title='剩余工作量：1%'>");
                                      }
                                      spWeb.AllowUnsafeUpdates = true;
                                      task["PercentComplete"] = percent * 0.01;
                                      task.Update();
                                      spWeb.AllowUnsafeUpdates = false;
                                      sb.AppendLine("<div class='filled' data-width='" + percent + "%' title='已完成进度：" + percent.ToString() + "%' style='width:" + percent + "%'></div>");
                                      string temptitle = title.Length > 5 ? title.Substring(0, 5) + "…" : title;
                                      sb.AppendLine("<span class='title' title='任务名称：" + title + "'>" + temptitle + "</span>");
                                      sb.AppendLine("<span class='percent' title='进度百分比'>" + percent + "%</span>");
                                      sb.AppendLine("</div>");
                                      sb.AppendLine("</div>");
                                      divContainter.InnerHtml = sb.ToString();
                                  }
                              }
                              else
                              {
                                  divContainter.InnerHtml = "";
                              }
                          }
                      }
                  }
              }
          });
        }

        private DataTable GetAllActivitiesByTaskId(int taskId)
        {
            using (SPSite spSite = SPContext.Current.Site)
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    string activityList = webObj.ChildList;
                    SPList spList = spWeb.Lists.TryGetList(activityList);
                    if (spList != null)
                    {
                        SPQuery spq = new SPQuery
                        {
                            Query = @" <Where>
                                          <Eq>
                                             <FieldRef Name='TaskID' LookupId='True' />
                                             <Value Type='Lookup'>"+taskId+@"</Value>
                                          </Eq>
                                       </Where>"
                        };
                        return spList.GetItems(spq).GetDataTable();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        /// <summary>
        /// 计算两个日期时差（TimeSpan）
        /// </summary>
        /// <param name="dateStart">开始日期</param>
        /// <param name="dateEnd">截止日期</param>
        /// <returns></returns>
        private TimeSpan TimeDiff(DateTime dateStart, DateTime dateEnd)
        {
            DateTime start = Convert.ToDateTime(dateStart.ToShortDateString());
            DateTime end = Convert.ToDateTime(dateEnd.ToShortDateString());
            TimeSpan sp = end.Subtract(start);
            return sp;
        }
    }
}
