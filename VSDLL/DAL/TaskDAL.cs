using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.DAL
{
    public class TaskDAL
    {
        #region 读取计划
       
        /// <summary>
        /// 获取用户的周期计划，目标计划和日程
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="dtStart">开始日期</param>
        /// <param name="dtEnd">结束</param>
        /// <returns></returns>
        public static DataSet GetDailyTask(long userID)
        {
            //
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetDailyTasks", userID);
            return ds;
        }
        #endregion
        #region 读取任务相关
        /// <summary>
        /// 获取项目任务
        /// </summary>
        /// <returns></returns>
        public static DataSet GetProjectTask()
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetProjectTask");
            return ds;
        }
        
        /// <summary>
        /// 获取任务关联的文档
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public static DataSet GetDocsByTaskID(long taskID)
        {
            //
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetDocsByTaskID",taskID );
            return ds;
        }
        /// <summary>
        /// 通过链接名称查找文档
        /// </summary>
        /// <param name="docLink"></param>
        /// <returns></returns>
        public static DataSet GetDocsByDocLink(string docLink)
        {
            //
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetDocsByDocLink", docLink);
            return ds;
        }
        #endregion
        #region 新建
        /// <summary>
        /// 新建项目计划 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static long InsertProjectTask(DataRow dr, SqlTransaction trans = null)
        {
            if (trans == null)
                return ((long)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(DAL.DataProvider.ConnectionString, "InsertProjectTask", dr)[0].Value);
            else
                return ((long)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(trans, "InsertProjectTask", dr)[0].Value);
        }
        /// <summary>
        /// 新建计划我们文档的关系表
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static long InsertTask_Docs(SqlTransaction trans, DataRow dr)
        {
            if (trans == null)
                return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "InsertTask_Docs", dr));
            else
                return (DAL.SqlHelper.ExecuteAppointedParameters(trans, "InsertTask_Docs", dr));
        }
        /// <summary>
        /// 新建文档
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static long InsertDocs(SqlTransaction trans, DataRow dr)
        {

            if (trans == null)
                return ((long)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(DAL.DataProvider.ConnectionString, "InsertDocs", dr)[0].Value);
            else
                return ((long)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(trans, "InsertDocs", dr)[0].Value);
        }
        /// <summary>
        /// 新建计划
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static long InsertPlans(SqlTransaction trans, DataRow dr)
        {

            if (trans == null)
                return ((long)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(DAL.DataProvider.ConnectionString, "InsertPlans", dr)[0].Value);
            else
                return ((long)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(trans, "InsertPlans", dr)[0].Value);
        }
        #endregion
        #region 更新
        /// <summary>
        /// 更新项目任务
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static int UpdateProjectTask(DataRow dr)
        {
            return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "UpdateProjectTask", dr));
        }
        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static int UpdateDocs(SqlTransaction trans, DataRow dr)
        {
            if (trans == null)
                return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "UpdateDocs", dr));
            else
                return (DAL.SqlHelper.ExecuteAppointedParameters(trans, "UpdateDocs", dr));
        }
        /// <summary>
        /// 更新计划
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static int UpdatePlans(SqlTransaction trans, DataRow dr)
        {
            if (trans == null)
                return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "UpdatePlans", dr));
            else
                return (DAL.SqlHelper.ExecuteAppointedParameters(trans, "UpdatePlans", dr));
        }
        #endregion
    }
}
