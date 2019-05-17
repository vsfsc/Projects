using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSDLL.DAL;

namespace VSDLL.BLL
{
    public class TaskBLL
    {
        /// <summary>
        /// 分级显示项目任务
        /// </summary>
       /// <param name="addBlankLine">首行是否加空行</param>
        /// <returns></returns>
        public static DataTable GetProjectTaskByLevel(bool addBlankLine=true )
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetProjectTask");
            DataTable dtLevel = Common.GetDataTableByLevel(ds.Tables[0], "Title", "PID",addBlankLine );
            return dtLevel;
        }


        public static DataTable GetTasksWithLevel(DataTable dtTasks, string dispCol, string parentCol, bool addBlankLine = true)
        {
            DataTable dtTemp = dtTasks.Copy();
            DataTable dtLevel = Common.GetDataTableByLevel(dtTemp, dispCol, parentCol, addBlankLine);
            return dtLevel;
        }

        public static DataTable GetTasks()
        {
            DataSet ds= SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetProjectTask");
            return ds.Tables[0];
        }

        private DataRow GetTaskByID(int taskId,DataTable dtTasks)
        {
            if (taskId == 0)
                return dtTasks.NewRow();
            else
            {
                DataRow[] drTasks= dtTasks.Select("TaskID=" + taskId);
                if (drTasks.Length > 0)
                    return drTasks[0];
                else
                    return null;
            }
        }

        public static DataTable GetProjects(DataTable dtTasks)
        {
            DataRow[] drs = dtTasks.Select("PID=0");
            if (drs.Length > 0)
                return Common.RowsToTable(drs);
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 返回说明中包含的字段中文名称，与Excel中的标题对应
        /// </summary>
        /// <param name="fldDes">字段描述信息,字段名称和说明之间用冒号分开</param>
        /// <returns></returns>
        public static string GetTitleByDescption(string fldDes)
        {
            string fldName = fldDes;
            string fldSep = ":";
            fldName = fldName.Replace("：", fldSep);//中文冒号替换成英文冒号
            if (fldName.IndexOf(fldSep) > 0)
                fldName = fldName.Substring(0, fldName.IndexOf(fldSep));
            return fldName;
        }
        /// <summary>
        /// 根据描述信息，将列的名称改写成中文列名
        /// </summary>
        /// <param name="dtChanged">需要更改的表</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static DataTable ChangeColumnCaption(DataTable dtChanged, string tableName)
        {
            DataTable dtResult = dtChanged.Copy();
            DataTable dtTableDes = TablesBLL.GetTableFields(tableName, null).Tables[0];
            string titleCN;
            foreach (DataRow drDes in dtTableDes.Rows)
            {
                if (dtResult.Columns.Contains(drDes["字段名"].ToString()))
                {
                    titleCN = GetTitleByDescption(drDes["说明"].ToString());
                    dtResult.Columns[drDes["字段名"].ToString()].Caption = titleCN;
                    dtResult.Columns[drDes["字段名"].ToString()].ColumnName =titleCN;
                }
            }
            dtResult.AcceptChanges();
            return dtResult;
        }

        /// <summary>
        /// 逻辑删除指定ID的任务
        /// 以后可能的逻辑有：用户对其子任务（用户确认是否删除）进行处理
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns></returns>
        public static int DeleteTask(long taskId)
        {
            int rows = 0;
            DataTable dt =GetTasks();
            DataRow[] drs = dt.Select("TaskID=" + taskId);
            if (drs.Length > 0)
            {
                DataRow[] drChilds = dt.Select("PID=" + taskId);
                if (drChilds.Length <= 0)//无子任务，可以删除
                {
                    DataRow dr = drs[0];
                    dr["Flag"] = 0;
                    TaskDAL.UpdateProjectTask(dr);
                    rows = 1;
                    ////用户确认是否删除子任务
                    //if (isDelchildTask == 0)//不删除子任务，则将其升级到其上一级任务
                    //{
                    //    int pId = DAL.SystemDataExtension.GetInt16(dr, "PID");
                    //    foreach (DataRow drChild in drChilds)
                    //    {
                    //        drChild["PID"] = pId;
                    //        TaskDAL.UpdateProjectTask(drChild);
                    //    }
                    //}
                    //else//子任务一起删除
                    //{
                    //    foreach (DataRow drChild in drChilds)
                    //    {
                    //        drChild["Flag"] = 0;
                    //        TaskDAL.UpdateProjectTask(drChild);
                    //    }
                    //}
                }
                else
                {
                    rows= -1;//有子任务，不可删除
                }
            }
            return rows;
        }

        /// <summary>
        /// 获取指定任务的所属项目ID
        /// </summary>
        /// <param name="taskId">指定任务ID</param>
        /// <param name="dtTasks">所有任务</param>
        /// <param name="drProject">返回的项目行</param>
        /// <param name="keyCol">表的筛选字段</param>
        /// <param name="parentCol">表的父子关联字段</param>
        /// <returns></returns>
        public static void GetProjectByTaskID(long taskId,DataTable dtTasks, ref DataRow drProject)
        {
            DataRow[] drsTasks = dtTasks.Select(string.Format("{0}={1}", "TaskID", taskId));
            if (drsTasks.Length <= 0) return;
            DataRow drTasks = drsTasks[0];
            drProject = drTasks;
            taskId = SystemDataExtension.GetInt32(drTasks, "PID");
            if (taskId == 0) return;
            else
            {
                GetProjectByTaskID(taskId, dtTasks, ref drProject);
            }
        }

        /// <summary>
        /// 获取指定项目的所有任务集合
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="dtTasks">所有任务</param>
        /// <param name="parentCol">表的父子关联字段</param>
        /// <param name="dtReturn">子任务集合表</param>
        /// <returns></returns>
        public static void GetTasksByProjectID(long projectId, DataTable dtTasks, ref DataTable dtReturn)
        {
            DataRow[] drsTasks = dtTasks.Select(string.Format("{0}={1}", "PID", projectId));
            if (drsTasks.Length <= 0) return;
            for (int i = 0; i < drsTasks.Length; i++)
            {
                DataRow drTasks = drsTasks[i];
                dtReturn.Rows.Add(drTasks.ItemArray);
                projectId = SystemDataExtension.GetInt32(drTasks, "TaskID");
                GetTasksByProjectID(projectId, dtTasks, ref dtReturn);
            }

        }

        #region 添加或更新新记录
        /// <summary>
        /// 添加任务计划的同时，写入关联的文档
        /// </summary>
        /// <param name="drProjectTask"></param>
        /// <param name="drsDoc"></param>
        /// <returns></returns>
        public static bool SaveProjectTask(DataRow drProjectTask, DataRow[] drsDoc)
        {
            using (SqlTransaction trans = DAL.DataProvider.CurrentTransactionEx)
            {
                try
                {
                    long taskID = DAL.TaskDAL.InsertProjectTask(drProjectTask, trans);
                    foreach (DataRow drDoc in drsDoc)
                    {
                        drDoc["TaskID"] = taskID;
                        DAL.TaskDAL.InsertTask_Docs(trans, drDoc);
                    }

                    trans.Commit();
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    return false;
                }
            }

        }
        #endregion
    }
}
