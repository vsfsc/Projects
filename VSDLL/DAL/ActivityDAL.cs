using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.DAL
{
    public class ActivityDAL
    {
        #region 读取活动相关的视图
        /// <summary>
        /// 获取用户指定时间的活动视图
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public static DataSet GetActivityViewByUserID(long userID,DateTime dtStart,DateTime dtEnd)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetActivityViewByUserID", userID,dtStart.ToString("yyyy-MM-dd"),dtEnd.ToString("yyyy-MM-dd"));
            return ds;
        }
        /// <summary>
        /// 获取用户指定时间的活动数据
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <returns></returns>
        public static DataSet GetActivityByUserID(long userID, DateTime dtStart, DateTime dtEnd)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetActivityByUserID", userID, dtStart.ToString("yyyy-MM-dd"), dtEnd.ToString("yyyy-MM-dd"));
            return ds;
        }
        /// <summary>
        /// 获取用户最后一次/第一次录入活动的日期
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="typeID">typeID=1:最后一次活动日期；typeID=0:第一次活动日期</param>
        /// <returns></returns>
        public static DataSet GetActivityLastDayByUserID(long userID,int typeID )
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetActivityLastDayByUserID", userID,typeID );
            return ds;
        }
        #endregion
        #region 添加活动相关的数据
        /// <summary>
        /// 添加新活动
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static long InsertActivity(DataRow dr)
        {
            return ((long)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(DAL.DataProvider.ConnectionString, "InsertActivity", dr)[0].Value);
        }
        public static long InsertActivity(SqlTransaction trans,DataRow dr)
        {
            return ((long)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(trans , "InsertActivity", dr)[0].Value);
        }
        public static int UpdateActivity(SqlTransaction trans, DataRow dr)
        {
            if (trans == null)
                return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "UpdateActivity", dr));
            else
                return (DAL.SqlHelper.ExecuteAppointedParameters(trans, "UpdateActivity", dr));
        }
        #endregion
    }
}
