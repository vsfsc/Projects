using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PowerPA.ImportActivity
{
    public partial class ImportActivityUserControl : UserControl
    {
        #region 常量
        //模板文件和导出临时文件所在的服务器路径
        private const string _exportPath = @"/_layouts/15/excel/";
        #endregion
        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
                InitControl();
            btnImport.Click += BtnImport_Click;
            btnExport.Click += BtnExport_Click;
            btnImport.Attributes.Add("onclick", "javascript:document.getElementById('" + divSaveAs.ClientID + "').innerHTML ='正在导入，请稍等……';document.getElementById('" + lblMsg.ClientID + "').innerText='';");
            btnExport.Attributes.Add("onclick", "javascript:document.getElementById('" + divSaveAs.ClientID + "').innerHTML ='正在导出，请稍等……';document.getElementById('" + lblMsg.ClientID + "').innerText='';");

        }
        /// <summary>
        /// 数据导出到excel中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            int userID = UserID ;
            ExportToExcel(userID , dtStart.SelectedDate, dtEnd.SelectedDate);
        }
        /// <summary>
        /// 从Excel导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImport_Click(object sender, EventArgs e)
        {
            int userID = UserID ;
            ImportFromExcel(userID,dtStart.SelectedDate,dtEnd.SelectedDate );
        }
        #endregion
        #region 初始化控件
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            dtStart.SelectedDate = DateTime.Now.AddDays(-3);
            dtEnd.SelectedDate = DateTime.Now.Date; 
            WriteChkItems();
        }
        //要导出的Excel工作表
        private void WriteChkItems()
        {
            string items = webObj.ExportLists.Replace("；", ";").Trim();
            //chkLists.Items.Clear();
            //if (items.Length >0)//
            //{
            //    string[] chkItems = Regex.Split(items, ";");
            //    chkLists.RepeatColumns = webObj.RepeatCol;
            //    foreach (string chkItem in chkItems)
            //    {
            //        chkLists.Items.Add(new ListItem(chkItem));
            //        chkLists.Items[chkLists.Items.Count - 1].Selected = true;
            //    }
            //}
        }
        #endregion
        private int UserID
        {
            get
            {
                if (ViewState["userID"] == null)
                {
                    int userID = VSDLL.Common.Users.UserID;
                    ViewState["userID"] = userID;
                }
                return (int)ViewState["userID"];
            }
        }
        #region 调用 的方法
        /// <summary>
        /// 根据excel的中文列名返回字段对应的英文名
        /// 读取视图的结构
        /// </summary>
        /// <param name="title">excel标题的中文名称</param>
        /// <returns></returns>
        private string GetFieldName(string title)
        {
            string fName = "";
            DataSet ds;
            if (ViewState["dsActivityFields"] == null)
            {
                ds = VSDLL.BLL.TablesBLL.GetTableFields("Activity", null);
                ViewState["dsActivityFields"] = ds;
            }
            else
                ds = (DataSet)ViewState["dsActivityFields"];

            DataRow[] drs = ds.Tables[0].Select("说明='" + title + "'");
            if (drs.Length > 0)
                fName = drs[0]["字段名"].ToString();
            return fName;
        }
        /// <summary>
        /// 根据excel的工作表项目计划中文列名返回字段对应的英文名
        /// 读取视图的结构
        /// </summary>
        /// <param name="title">excel标题的中文名称</param>
        /// <returns></returns>
        private string GetFieldNameOfTask(string title)
        {
            string fName = "";
            DataSet ds;
            if (ViewState["dsTaskFields"] == null)
            {
                ds = VSDLL.BLL.TablesBLL.GetTableFields("ProjectTask", null);
                ViewState["dsTaskFields"] = ds;
            }
            else
                ds = (DataSet)ViewState["dsTaskFields"];

            DataRow[] drs = ds.Tables[0].Select("说明='" + title + "'");
            if (drs.Length > 0)
                fName = drs[0]["字段名"].ToString();
            return fName;
        }
        /// <summary>
        /// 通过任务名称，获取ID
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <returns></returns>
        private int GetTaskID(string taskName)
        {
            int taskID=0;
            DataSet dsTasks;
            if (ViewState["dsTasks"] == null)
            {
                dsTasks = VSDLL.DAL.TaskDAL.GetProjectTask();
                ViewState["dsTasks"] = dsTasks;
            }
            else
                dsTasks = (DataSet)ViewState["dsTasks"];
            DataRow[] drs = dsTasks.Tables[0].Select("Title='"+taskName.Trim ()+"'");
            if (drs.Length > 0)
                taskID = int.Parse (drs[0]["TaskID"].ToString ());
            return taskID;
        }
        /// <summary>
        /// 通过操作名称获取操作iD
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <returns></returns>
        private int GetActionID(string actionName)
        {
            int actionID=0;
            DataSet dsActions;
            if (ViewState["dsActions"] == null)
            {
                dsActions = VSDLL.DAL.ActionDAL.GetAllActions ();
                ViewState["dsActions"] = dsActions;
            }
            else
                dsActions = (DataSet)ViewState["dsActions"];
            DataRow[] drs = dsActions.Tables[0].Select("Title='" + actionName.Trim() + "'");
            if (drs.Length > 0)
                actionID = int.Parse(drs[0]["ActionID"].ToString());
            return actionID;
        }
        #endregion
        #region 导入
        /// <summary>
        /// 从Excel导入数据,主要导入活动
        /// </summary>
        private void ImportFromExcel(int userID,DateTime dtStart,DateTime dtEnd)
        {
            if (FileUpload1.FileName != "")
            {
                if (userID == 0)
                    divSaveAs.InnerHtml  = "请先登录！";
                else
                {
                    DataTable dt = ReadToTable( userID,dtStart,dtEnd );
                    WriteDataToSql(dt);
                }
            }
            else
                divSaveAs.InnerHtml = "先选择要导入活动的Excel文件";
        }
        private ISheet ReadExcel()
        {
            Stream fs = FileUpload1.PostedFile.InputStream;
            IWorkbook book = WorkbookFactory.Create(fs);

            ISheet wSheet;//define worksheet
            //此外需要打开活动所在的工作表
            int shtIndex = book.GetSheetIndex("活动");
            wSheet = book.GetSheetAt(shtIndex);//打开指定的工作表
            return wSheet;
        }
        //读取Excel中的活动，根据是否更改，写入到Sql数据库
        private DataTable ReadToTable(int userID, DateTime dtStart, DateTime dtEnd)
        {
            ISheet wSheet = ReadExcel();
            DataTable dtActivity = VSDLL.DAL.ActivityDAL.GetActivityByUserID(userID, dtStart, dtEnd).Tables[0];
            DataTable dtChangedActivity = dtActivity.Clone();
            DataRow[] drsActivity;
            DataRow drActivity;
            int lStart = 2;//导入的开始行，标题行除外
            int lEnd = wSheet.LastRowNum;//导入的结束行
            #region 读取标题以外的数据
            IRow rowTitle = wSheet.GetRow(0);
            IRow rowtemp;
            int lastCell = rowTitle.LastCellNum - 1;
            int importLastCell = lastCell;
            if (rowTitle.GetCell(lastCell).ToString() == "是否导出")//最后一列为是否导出列
            {
                importLastCell = lastCell - 1;

            }
            for (int i = lStart - 1; i < lEnd; i++)
            {
                rowtemp = wSheet.GetRow(i);//先获取现有的行， 
                if (rowtemp == null || rowtemp.GetCell(0)==null)
                    break;

                if (importLastCell < lastCell && rowtemp.GetCell(lastCell) != null && rowtemp.GetCell(lastCell).ToString() == "否")
                    continue;


                DateTime dtRiqi;
                try
                {
                    dtRiqi = DateTime.Parse(rowtemp.GetCell(0).DateCellValue.ToShortDateString());
                }
                catch//excel中设置常规格式，而非日期格式
                {
                    dtRiqi = DateTime.Parse(rowtemp.GetCell(0).ToString());
                }
                if (DateTime.Compare(dtRiqi, dtStart) < 0 || DateTime.Compare(dtRiqi, dtEnd) > 0)
                    continue;

                int actionID = 0;
                drActivity = dtChangedActivity.NewRow();
                bool allowAdd = true;
                string headTitle;
                string fieldName;
                string cellValue;
                for (int j = 0; j <= importLastCell; j++)//最后一列为是否导出列，即私有的不导出
                {
                    if (rowtemp.GetCell(j) != null)
                        cellValue = rowtemp.GetCell(j).ToString().Trim();
                    else
                        cellValue = "";
                    if (cellValue.Length == 0)//值为空略过
                        continue;
                    headTitle = rowTitle.GetCell(j).ToString();
                    fieldName = GetFieldName(headTitle);//如果数据库中不存在对应的列，则此列忽略
                    if (fieldName == "") continue;
                    if (fieldName == "Riqi")
                        drActivity[fieldName] = dtRiqi;
                    //保存时间，是否正常，是否
                    else if (fieldName == "StartTime")
                    {
                        DateTime StartTime;
                        try
                        {
                            StartTime = DateTime.Parse(rowtemp.GetCell(j).DateCellValue.ToShortTimeString());
                        }
                        catch//非日期时间格式
                        {
                            StartTime = DateTime.Parse(cellValue);
                        }

                        int hour = StartTime.Hour;
                        int minute = StartTime.Minute;
                        int second = StartTime.Second;
                        TimeSpan startTime = new TimeSpan(hour, minute, second);
                        drActivity[fieldName] = startTime;
                    }
                    else if (fieldName == "IsNormal")
                    {
                        if (cellValue == "否")
                            drActivity[fieldName] = 0;
                        else
                            drActivity[fieldName] = 1;
                    }
                    else if (fieldName.EndsWith("ID"))//ActionID TaskID
                    {
                        if (fieldName == "ActionID")
                        {
                            actionID = GetActionID(cellValue);
                            if (actionID == 0) break;//找不到对应的操作则不导入
                            drActivity[fieldName] = actionID;

                        }
                        else
                            drActivity[fieldName] = GetTaskID(cellValue);
                    }
                    else if (drActivity.Table.Columns[fieldName].DataType.Name == "Decimal")
                    {
                        try
                        {
                            drActivity[fieldName] = decimal.Parse(cellValue);
                        }
                        catch
                        { }
                    }
                    else if (drActivity.Table.Columns[fieldName].DataType.Name == "Int32")
                    {
                        try
                        {
                            drActivity[fieldName] = int.Parse(cellValue);
                        }
                        catch
                        {

                        }
                    }
                    else
                        drActivity[fieldName] = cellValue;
                }
                if (actionID == 0) continue;
                if (drActivity.IsNull("TaskID"))
                    drActivity["TaskID"] = 0;
                if (drActivity.IsNull("IsNormal"))//是否正常默认为正常
                    drActivity["IsNormal"] = 1;
                drsActivity = dtActivity.Select("Riqi='" + ((DateTime)drActivity["Riqi"]).ToString("yyyy-MM-dd") + "' and ActionID=" + drActivity["ActionID"] + " and TaskID=" + drActivity["TaskID"]);//判断同一天是否存在同一活动

                if (drsActivity.Length == 0)
                {
                    drActivity["Created"] = DBNull.Value;//用来判断新建
                }
                else
                {
                    bool isEqual = VSDLL.BLL.Common.DataRowCompare(drActivity, drsActivity[0], new string[] { "StartTime", "Description", "ActionID", "During", "Quantity", "TaskID", "IsNormal" });
                    drActivity["Created"] = drsActivity[0]["Created"];

                    if (!isEqual)
                    {
                        drActivity["ActivityID"] = drsActivity[0]["ActivityID"];
                        drActivity["Modified"] = DBNull.Value;//编辑
                    }
                    else
                        allowAdd = false;//有相同的活动则不添加
                }
                if (allowAdd)
                {
                    drActivity["Flag"] = 1;
                    drActivity["UserID"] = userID;
                    dtChangedActivity.Rows.Add(drActivity);
                }
            }
            #endregion
            return dtChangedActivity;
        }
        /// <summary>
        /// 将读到的Excel中的数据写入sql数据库中
        /// </summary>
        /// <param name="dtChangedActivity"></param>
        /// <param name="userID"></param>
        private void WriteDataToSql(DataTable dtChangedActivity)
        {
            if (dtChangedActivity.Rows.Count ==0)
            {
                divSaveAs.InnerHtml = "没有要导的数据！";
                return;
            }
            string errMsg = "";
            bool result = VSDLL.BLL.ActivityBLL.InsertActivities(dtChangedActivity, null,ref errMsg );
            if (result)
                divSaveAs.InnerHtml = "导入完成！本次导入活动 :"+dtChangedActivity.Rows.Count+"条" ;
            else
                divSaveAs.InnerHtml= "导入失败:"+errMsg ;
        }
        #endregion
        #region 数据导出
        /// <summary>
        /// 数据导出到excel
        /// </summary>
        private void ExportToExcel(int userID, DateTime dtStart, DateTime dtEnd)
        {
            string fileName = webObj.ExcelFilename;
            string tempName = webObj.ExcelTempfile;
            string allNames = webObj.ExportLists.Replace("；", ";").Trim();
            string[] sheetNames = Regex.Split(allNames, ";");
            DataSet ds = new DataSet();
            DataTable dt;
            DataTable dtResult=null;
            foreach (string sheetName in sheetNames)
            {
                if (sheetName == "活动")
                {
                    dt = GetDataFromSql(userID, dtStart, dtEnd);
                    dtResult = dt.Copy();

                }
                else if (sheetName.Substring(0, 2) == "项目")
                {
                    dt = VSDLL.BLL.TaskBLL.GetProjectTaskByLevel(false );
                    dt = VSDLL.BLL.TaskBLL.ChangeColumnCaption(dt, "ProjectTask");
                    dtResult = dt.Copy();
                }
                if (dtResult !=null && dtResult.Rows.Count > 0)
                {
                    dtResult.TableName = sheetName;
                    ds.Tables.Add(dtResult);
                }

            }
             
            if (ds.Tables.Count > 0)
                WriteToExcel(tempName, fileName, ds);
            else
            {
                divSaveAs.InnerHtml = "";
                lblMsg.Text = webObj.NoDataMsg;
            }
        }
        /// <summary>
        /// 读取要导出到excel中的活动的数据
        /// </summary>
        /// <param name="sheetName">工作表名</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        private DataTable GetDataFromSql( int userID,DateTime dtStart, DateTime dtEnd)
        {
            DataTable dtActivities = VSDLL.DAL.ActivityDAL.GetActivityViewByUserID(userID, dtStart, dtEnd).Tables[0] ;
            return dtActivities;
        }
        /// <summary>
        /// 数据导出到excel
        /// </summary>
        /// <param name="tempName">模板名称</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="dsSheet">包含工作表数据的数据集</param>
        private void WriteToExcel(string tempName, string fileName, DataSet dsSheet  )
        {
            IWorkbook book = null;
            string _filePath = Server.MapPath(_exportPath) + tempName;
            using (FileStream fs = File.Open(_filePath, FileMode.Open, FileAccess.ReadWrite)) 
            {
                book = WorkbookFactory.Create(fs);
                ISheet wSheet;//define worksheet
                ICellStyle cStyle = book.CreateCellStyle();//define style
                ////HSSFDataFormat.GetBuiltinFormat("###0.00");//(short)CellType.NUMERIC;
                IDataFormat dataformat = book.CreateDataFormat();
                cStyle.DataFormat = 194;//数字格式

                ICellStyle cStyleDate = book.CreateCellStyle(); 
                cStyleDate.DataFormat = dataformat.GetFormat("yyyy-MM-dd"); //
                ICellStyle cStyleTime = book.CreateCellStyle();
                cStyleTime.DataFormat = dataformat.GetFormat("hh:mm"); //

                foreach (DataTable dt in dsSheet.Tables)
                {
                    wSheet = book.GetSheet(dt.TableName);//打开指定的工作表
                    #region f填充数据
                    NPOI.SS.UserModel.IRow rowTitle = wSheet.GetRow(0);
                    NPOI.SS.UserModel.IRow rowtemp;
                    DataColumn dc;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        rowtemp = wSheet.GetRow(i + 1);//先获取现有的行，如果为空，则创建
                        if (rowtemp == null)
                            rowtemp = wSheet.CreateRow(i + 1);
                        ICell rowCell;
                        string headTitle;
                        for (int j = 0; j < rowTitle.LastCellNum; j++)
                        {
                            if (rowTitle.GetCell(j) == null) break;
                            headTitle = rowTitle.GetCell(j).ToString();
                            if (!dt.Columns.Contains(headTitle)) continue;
                            dc = dt.Columns[headTitle];

                            rowCell = rowtemp.GetCell(j);
                            if (rowCell == null)
                                rowCell = rowtemp.CreateCell(j);
                            if ((dc.DataType.Name == "Decimal" || dc.DataType.Name.Substring(0, 3) == "Int") && !dt.Rows[i].IsNull(dc.ColumnName))
                            {
                                double during = double.Parse(dt.Rows[i][dc.ColumnName].ToString());
                                rowCell.SetCellType(CellType.Numeric);
                                rowCell.SetCellValue(during);
                                rowCell.CellStyle = cStyle;
                            }
                            else if (dc.DataType.Name == "DateTime" && !dt.Rows[i].IsNull(dc.ColumnName))
                            {
                               
                                rowCell.SetCellValue( (DateTime)dt.Rows[i][dc.ColumnName] );
                                rowCell.CellStyle = cStyleDate;
                            }
                            else if (dc.DataType.Name == "TimeSpan" && !dt.Rows[i].IsNull(dc.ColumnName))
                            {
                                rowCell.SetCellValue(DateTime.Today.Date+ (TimeSpan)dt.Rows[i][dc.ColumnName]);
                                rowCell.CellStyle = cStyleTime;
                            }
                            else if (!dt.Rows[i].IsNull(dc.ColumnName))
                                rowCell.SetCellValue(dt.Rows[i][dc.ColumnName].ToString());
                        }
                    }
                    //调整列宽
                    for (int j = 0; j < rowTitle.LastCellNum; j++)
                    {
                        wSheet.AutoSizeColumn(j);
                    }
                    #endregion
                }
            }
            //保存到本地文件
            _filePath = Server.MapPath(_exportPath) + fileName;
            using (FileStream fs1 = File.Open(_filePath, FileMode.Create, FileAccess.ReadWrite)) 
            {
                book.Write(fs1);
            }
            book = null;
            divSaveAs.InnerHtml = "<font color='black'>Excel文件已生成，请</font> <a href='" + _exportPath + fileName + "'>单击下载</a> ";
        }
        #endregion
        #region 属性
        public ImportActivity  webObj {get ;set ;}
        #endregion
    }
}
