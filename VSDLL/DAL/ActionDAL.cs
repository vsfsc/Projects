using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.DAL
{
    public class ActionDAL
    {
        #region 读取活动相关的内容
        /// <summary>
        /// 获取操作关系
        /// </summary>
        /// <returns></returns>
        public static DataSet GetActionRelation()
        {
            DataSet ds = SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetActionRelation");
            return ds;
        }
        /// <summary>
        /// 按频次降序获取用户操作
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static DataSet GetUserActionByUserID(long userID)
        {
            DataSet ds = SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetUserActionByUserID", userID);
            return ds;
        }
        /// <summary>
        /// 按频次降序获取操作
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllActions(int itemTypeID=1 )
        {
            DataSet ds = SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetAllActions",itemTypeID  );
            return ds;
        }
        /// <summary>
        /// 获取操作的概率，包含的内容ActionID,UserID,TotalNums
        /// 在每天活动中，按操作的概率由高到低排序
        /// </summary>
        /// <returns></returns>
        public static DataSet GetActionProbability()
        {
            DataSet ds = SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetActionProbability");
            return ds;
        }
        #endregion
        #region 插入
        public static long InsertUserAction(SqlTransaction trans, DataRow dr)
        {
            return ((long)SqlHelper.ExecuteNonQueryTypedParamsOutput(trans, "InsertUserAction", dr)[0].Value);
        }
        #endregion
        #region 更新
        //更新用户操作
        public static int UpdateUserAction(SqlTransaction trans, DataRow dr)
        {
            if (trans == null)
                return (SqlHelper.ExecuteAppointedParameters(DataProvider.ConnectionString, "UpdateUserAction", dr));
            else
                return (SqlHelper.ExecuteAppointedParameters(trans, "UpdateUserAction", dr));
        }
        /// <summary>
        /// 更新操作
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static int UpdateAction(SqlTransaction trans, DataRow dr)
        {
            if (trans == null)
                return (SqlHelper.ExecuteAppointedParameters(DataProvider.ConnectionString, "UpdateAction", dr));
            else
                return (SqlHelper.ExecuteAppointedParameters(trans, "UpdateAction", dr));
        }
        #endregion
    }
}
