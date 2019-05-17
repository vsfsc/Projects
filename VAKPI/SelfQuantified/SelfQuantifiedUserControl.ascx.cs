using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace VAKPI.SelfQuantified
{
    public partial class SelfQuantifiedUserControl : UserControl
    {
        public SelfQuantified webObj;

        protected void Page_Load(object sender, EventArgs e)
        {
            gvWeekAnalysis.RowDataBound += gvWeekAnalysis_RowDataBound;
            if (!IsPostBack)
            {
                ShowAnalysis(7);
            }

        }


        private void CheckShow()
        {
            if (divErr.InnerHtml == "")
            {
                divErr.Visible = false;
            }
            else
            {
                divErr.Visible = true;
            }
            if (divWaring.InnerHtml=="")
            {
                divWaring.Visible = false;
            }
            else
            {
                divWaring.Visible = true;
            }
        }
        private void ShowAnalysis(int days)
        {
            SPUser spuser = SPContext.Current.Web.CurrentUser;
            if (spuser != null)
            {
                rbPeriods.Visible = true;
                DataTable dtSource = GetAnalysisedTable(spuser,days);
                if (dtSource != null)
                {
                    if (dtSource.Rows.Count>0)
                    {
                        DrawAnalysisImage(dtSource);
                        gvWeekAnalysis.DataSource = dtSource;
                        gvWeekAnalysis.DataBind();
                        divAnaContent.Visible = true;
                        divErr.Visible = false;
                        divWaring.Visible = false;
                    }
                    else
                    {
                        divWaring.InnerHtml = "近 <b>"+days+"</b> 天无活动记录";
                        divAnaContent.Visible = false;
                        divWaring.Visible = true;
                        divErr.Visible = true;
                    }
                }
                else
                {
                    divWaring.InnerHtml = "近 <b>"+days+"</b> 天无活动记录";
                    divAnaContent.Visible = false;
                    divErr.Visible = true;
                    divWaring.Visible = true;
                }
            }
            else
            {
                divAnaContent.Visible = false;
                rbPeriods.Visible = false;
                divWaring.InnerHtml= "当前您尚未登录，无法查看数据！";
                divErr.Visible = true;
                divWaring.Visible = true;
            }
        }

        private void DrawAnalysisImage(DataTable dtSource)
        {
            try
            {
                chartAnalysis.Titles.Clear();
                string title = rbPeriods.SelectedItem.Text + "内个人活动日均时长达标图（单位：分钟）";
                chartAnalysis.Titles.Add(title);
                chartAnalysis.Titles[0].ForeColor = Color.Blue;
                chartAnalysis.Titles[0].Font = new Font("微软雅黑", 12f, FontStyle.Regular); ;
                //背景色设置

                chartAnalysis.ChartAreas[0].BorderColor = Color.Transparent;
                chartAnalysis.ChartAreas[0].ShadowColor = Color.Transparent;
                chartAnalysis.ChartAreas[0].BackColor = Color.FromArgb(60, 209, 237, 254);
                chartAnalysis.ChartAreas[0].BackGradientStyle = GradientStyle.None;
                chartAnalysis.ChartAreas[0].BackSecondaryColor = Color.White;
                chartAnalysis.AntiAliasing = AntiAliasingStyles.All;
                //中间X,Y线条的颜色设置
                chartAnalysis.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(60, 128, 128, 128);
                chartAnalysis.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(60, 128, 128, 128);

                // Show as 3D
                chartAnalysis.ChartAreas[0].Area3DStyle.Enable3D = true;
                //binding data

                chartAnalysis.DataSource = dtSource;
                chartAnalysis.Series.Clear();
                Series sr0 = new Series
                {
                    Name = "我的",//蓝色表示

                    ChartType = SeriesChartType.Radar,
                    IsValueShownAsLabel = true,
                    Color = Color.FromArgb(60, 0, 0, 255),
                    BorderColor = Color.Blue,
                    BorderWidth = 2,
                    LabelForeColor = Color.Blue
                };
                sr0.Points.DataBind(dtSource.DefaultView, "活动类别", "活动时长", "LegendText=活动类别,YValues=活动时长,ToolTip=活动类别");
                chartAnalysis.Series.Add(sr0);


                Series sr1 = new Series//系统目标图像
                {
                    Name = "系统达标线",
                    ChartType = SeriesChartType.Radar,
                    IsValueShownAsLabel = true,
                    Color = Color.FromArgb(60, 255, 0, 0),
                    BorderColor = Color.Red,
                    BorderWidth = 2,
                    LabelForeColor = Color.Red
                };
                sr1.Points.DataBind(dtSource.DefaultView, "活动类别", "系统目标", "LegendText=活动类别,YValues=系统目标,ToolTip=活动类别");
                chartAnalysis.Series.Add(sr1);

                if (dtSource.Columns.Contains("个人目标"))
                {
                    //ChartArea ca1=new ChartArea
                    //{
                    //    Name="",
                    //    BorderColor = Color.Transparent
                    //};
                    //chartAnalysis.ChartAreas.Add(ca1);


                    Series sr2 = new Series
                    {
                        Name = "个人设定达标线",
                        ChartType = SeriesChartType.Radar,
                        IsValueShownAsLabel = true,
                        Color = Color.FromArgb(60, 0, 255, 0),//绿色表示
                        BorderColor = Color.Green,
                        BorderWidth = 2,
                        LabelForeColor = Color.Green,
                    };
                    sr2.Points.DataBind(dtSource.DefaultView, "活动类别", "个人目标", "LegendText=活动类别,YValues=个人目标,ToolTip=活动类别");
                    chartAnalysis.Series.Add(sr2);
                }

                chartAnalysis.DataBind();
            }
            catch (Exception ex)
            {

                divErr.InnerHtml=ex.ToString();
            }
        }

        /// <summary>
        /// 读取指定用户制定天数内的个人活动记录数据
        /// </summary>
        /// <param name="spuser">指定用户</param>
        /// <param name="days">指定天数</param>
        /// <returns></returns>
        private DataTable GetPersonalizedData(SPUser spuser,int days)
        {
            DataTable dt = new DataTable("个人记录表");
            SPSecurity.RunWithElevatedPrivileges(delegate ()
           {
               try
               {
                   string siteUrl = webObj.SiteUrl;
                   using (SPSite spSite = new SPSite(siteUrl)) //找到网站集
                   {
                       using (SPWeb spWeb = spSite.OpenWeb())
                       {
                           string planList = webObj.PlanList;
                           SPList spList = spWeb.Lists.TryGetList(planList);
                           if (spList != null)
                           {
                               DateTime dtStart = DateTime.Now.AddDays(-days).Date;
                               string startStr = SPUtility.CreateISO8601DateTimeFromSystemDateTime(dtStart);
                               string endStr = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.AddDays(1).Date);
                               SPQuery qry = new SPQuery();
                               int userId = spuser.ID;
                               qry.Query = @" <Where><And><And><Lt><FieldRef Name='ActualDate' /><Value Type='DateTime'>" + endStr + "</Value></Lt><Geq><FieldRef Name='ActualDate' /><Value Type='DateTime'>" + startStr + "</Value></Geq></And><Eq><FieldRef Name = 'Author' LookupId = 'TRUE'></FieldRef><Value Type = 'User'>"+userId+"</Value></Eq></And></Where>";//获取当前用户近七天的数据
                               dt = spList.GetItems(qry).GetDataTable();
                           }
                       }
                   }
               }
               catch (Exception ex)
               {

                   divErr.InnerHtml = ex.ToString();
               }
           });
            return dt;
        }

        /// <summary>
        /// 读取所有正在使用的约束条件
        /// </summary>
        /// <returns></returns>
        private DataTable GetConstraintList()
        {
            DataTable dt = new DataTable("约束条件表");
            SPSecurity.RunWithElevatedPrivileges(delegate ()
           {
               try
               {
                   string siteUrl = webObj.SiteUrl;
                   using (SPSite spSite = new SPSite(siteUrl)) //找到网站集
                   {
                       using (SPWeb spWeb = spSite.OpenWeb())
                       {
                           SPList spList = spWeb.Lists.TryGetList(webObj.ConstraintList);
                           if (spList != null)
                           {
                               SPQuery qry = new SPQuery()
                               {
                                   Query = @"<Where><Neq><FieldRef Name='Flag' /><Value Type='Number'>10</Value></Neq></Where>"
                               };
                               dt = spList.GetItems(qry).GetDataTable();
                           }
                       }
                   }
               }
               catch (Exception ex)
               {

                   divErr.InnerHtml = ex.ToString();
               }
           });
            return dt;
        }

        private DataTable GetAnalysisedTable(SPUser spuser,int days)
        {
            DataTable dt = new DataTable("分析表");

            //DateTime fromDate = DateTime.Now.AddDays(-days).Date;
            //string selectExp="ActualDate>='"+fromDate+"'";//筛选近days天数的数据
            try
            {
                dt.Columns.Add("活动类别");//0
                //dt.Columns.Add("日期");
                //dt.Columns.Add("次数");
                dt.Columns.Add("活动时长");//1
                dt.Columns.Add("系统目标");//2
                dt.Columns.Add("系统差距");//3

                DataTable dtConstraint = GetConstraintList();
                DataView dvSys = new DataView(dtConstraint, "Flag='0'", "ActivityType", DataViewRowState.CurrentRows);
                DataTable dtSys = dvSys.ToTable();//系统标准
                DataView dvMy = new DataView(dtConstraint, "Flag<>'0' and Author='" + spuser.Name+"'", "ActivityType", DataViewRowState.CurrentRows);
                DataTable dtMy = dvMy.ToTable();//我的标准
                if (dtMy.Rows.Count > 0)
                {
                    dt.Columns.Add("个人目标");//4
                    dt.Columns.Add("个人差距");//5
                }
                else
                {
                    dt.Columns.Add("达标状态");//4
                }
                DataTable sourceDt = GetPersonalizedData(spuser,days);
                if (sourceDt!=null)
                {

                    DataTable dtTypes = sourceDt.DefaultView.ToTable(true, "ActivityType");//筛选不重复的ActiveType值，形成一个单列数据表
                    for (int i = 0; i < dtTypes.Rows.Count; i++)
                    {
                        string aType = dtTypes.Rows[i][0].ToString();
                        if (aType != "就医")
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = aType;//类别
                            //dr["次数"] = sourceDt.Compute("count(ID)","ActivityType='"+aType+"'");
                            double during = Math.Round((double.Parse(sourceDt.Compute("sum(ActualDuring)", "ActivityType='" + aType + "'").ToString()))/Convert.ToDouble(days),MidpointRounding.AwayFromZero);
                            dr[1] = during;//日均时长
                            DataRow[] drsSys = dtSys.Select("ActivityType='" + aType + "'");
                            if (drsSys.Length>0)
                            {
                                double goalDuring = (double)drsSys[0]["During"];
                                dr[2] = goalDuring;//系统目标
                                dr[3] = during - goalDuring;//系统目标差距
                            }
                            else
                            {
                                dr[2] = "";//系统目标
                                dr[3] = "";//系统目标差距
                            }
                            if (dt.Columns.Contains("个人目标"))
                            {
                                DataRow[] drsMy = dtMy.Select("ActivityType='" + aType + "'");
                                if (drsMy.Length > 0)
                                {
                                    double goalDuring = (double)drsMy[0]["During"];
                                    dr[4] = goalDuring;//系统目标
                                    dr[5] = during - goalDuring;//系统目标差距
                                }
                                else
                                {
                                    dr[4] = "";//系统目标
                                    dr[5] = "";//系统目标差距
                                }
                            }
                            else
                            {
                                if (dr[3].ToString()=="")
                                {
                                    dr[4] = "目标不定";//达标状态
                                }
                                else
                                {
                                    dr[4] =double.Parse(dr[3].ToString())>=0?"达标":"不达标";//达标状态
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                divErr.InnerHtml=ex.ToString();
            }
            return dt;
        }

        /// <summary>
        /// 筛选数据并做格式化（将时间字段转化成仅日期，便于以后按日期做统计分析）
        /// </summary>
        /// <param name="dtTitle">表名称</param>
        /// <param name="sourceDt">源数据表</param>
        /// <param name="selectExp">筛选语句</param>
        /// <param name="spuser">指定用户</param>
        /// <param name="days">统计的历时天数</param>
        /// <returns></returns>
        private DataTable GetgroupDt(string selectExp,SPUser spuser,int days)
        {
            DataTable dtReturn = new DataTable("筛选表");
            dtReturn.Columns.Add("ID", typeof(string));//ID 0
            dtReturn.Columns.Add("Title", typeof(string));//标题1
            dtReturn.Columns.Add("Types", typeof(string));//类别2
            dtReturn.Columns.Add("Start", typeof(DateTime));//实际开始3
            dtReturn.Columns.Add("During", typeof(float));//实际时长4
            dtReturn.Columns.Add("Results", typeof(int));//结果数量5


            DataTable sourceDt = GetPersonalizedData(spuser,days);
            try
            {
                DataRow[] drs = sourceDt.Select(selectExp);

                for (int i = 0; i < drs.Length; i++)
                {
                    DataRow dr = dtReturn.NewRow();
                    dr[0] = drs[i]["ID"];//ID
                    dr[1] = drs[i]["Title"];//标题
                    dr[2] = drs[i]["ActivityType"];//类别
                    if (!Convert.IsDBNull(drs[i]["ActualDate"]))
                    {
                        dr[3] = ((DateTime)drs[i]["ActualDate"]).Date;//实际开始的日期，即活动发生的日期
                    }
                    else
                    {
                        dr[3] = null;
                    }
                    dr[4] = Convert.IsDBNull(drs[i]["ActualDuring"]) ? 0 : drs[i]["ActualDuring"];//实际时长
                    //if (!Convert.IsDBNull(drs[i]["ID"]))
                    //{
                    //    string Id = drs[i]["ID"].ToString();
                    //    int activiId = int.Parse(Id);
                    //    int result = GetActivityResults(activiId);
                    //    dr[5] = result;//活动结果数
                    //}
                    dr[5] = 0;//活动结果数,先不做计算
                    dtReturn.Rows.Add(dr);
                }

            }
            catch (Exception ex)
            {

                divErr.InnerHtml=ex.ToString();
            }
            return dtReturn;
        }

        /// <summary>
        /// 获取对应活动的结果数
        /// </summary>
        /// <param name="activiId">活动ID</param>
        /// <returns></returns>
        private int GetActivityResults(int activiId)
        {
            int result = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    string siteUrl = webObj.SiteUrl;
                    string resultList = webObj.ResultList;
                    using (SPSite spSite = new SPSite(siteUrl)) //找到网站集
                    {
                        using (SPWeb spWeb = spSite.OpenWeb())
                        {
                            SPList spList = spWeb.Lists.TryGetList(resultList);
                            if (spList != null)
                            {
                                SPQuery qry = new SPQuery();
                                qry.Query = "<Where><Eq><FieldRef Name='AssistantID' /><Value Type='Number'>" + activiId + "</Value></Eq></Where>";
                                SPListItemCollection listItems = spList.GetItems(qry);
                                if (listItems.Count > 0)
                                {
                                    result = listItems.Count;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    divErr.InnerHtml = ex.ToString();
                }
            });
            return result;
        }


        protected void gvWeekAnalysis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[4].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[3].Text!="&nbsp;")
                {
                    double diff = double.Parse(e.Row.Cells[3].Text);//系统目标差距
                    if (diff < -100)//极度不达标
                    {
                        e.Row.Cells[3].ForeColor = Color.Red;
                        //e.Row.Attributes.Add("style", "color:red");
                    }
                    else if (diff >= -100 && diff < 0)//不达标
                    {
                        e.Row.Cells[3].ForeColor = Color.DarkOrange;
                        //e.Row.Attributes.Add("style", "color:yellow");
                    }
                    else if (diff >= 0 && diff < 100)//达标
                    {

                        e.Row.Cells[3].ForeColor = Color.Blue;
                        //e.Row.Attributes.Add("style", "color:blue");
                    }
                    else if (diff >= 100)//遥遥领先
                    {
                        e.Row.Cells[3].ForeColor =Color.ForestGreen;
                        //e.Row.Attributes.Add("style", "color:green");
                    }
                }
                if (e.Row.Cells.Count==6)
                {
                    if (e.Row.Cells[5].Text!="&nbsp;")
                    {
                        double diff = double.Parse(e.Row.Cells[5].Text);//个人目标差距
                        if (diff < -100)//极度不达标
                        {
                            e.Row.Cells[5].ForeColor = Color.Red;
                        }
                        else if (diff >= -100 && diff < 0)//不达标
                        {
                            e.Row.Cells[5].ForeColor = Color.DarkOrange;
                        }
                        else if (diff >= 0 && diff < 100)//达标
                        {
                            e.Row.Cells[5].ForeColor = Color.Blue;
                        }
                        else if (diff >= 100)//遥遥领先
                        {
                            e.Row.Cells[5].ForeColor = Color.ForestGreen;
                        }
                    }
                }
            }
        }

        protected void rbPeriods_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectindex =int.Parse(rbPeriods.SelectedItem.Value);

            switch (selectindex)
            {
                case 0:
                    ShowAnalysis(7);
                    break;
                case 1:
                    ShowAnalysis(30);
                    break;
                case 2:
                    ShowAnalysis(90);
                    break;
                case 3:
                    ShowAnalysis(180);
                    break;
                case 4:
                    ShowAnalysis(365);
                    break;
                default:
                    ShowAnalysis(7);
                    break;
            }
        }
    }
}
