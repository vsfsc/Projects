using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.DAL
{
    /// <summary>
    /// 处理SQL数据库中与用户有关的底层数据，读，写，更新
    /// </summary>
    public class UserDAL
    {
        #region 用户相关
        /// <summary>
        /// 获取用户信息2019-3-11
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static DataSet GetUser_RiqiByUserID(long userID)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetUser_RiqiByUserID", userID);
            return ds;
        }
        /// <summary>
        /// 通过用户帐户获取用户信息
        /// </summary>
        /// <param name="account">用户帐号，即登录名</param>
        /// <returns></returns>
        public static DataSet GetUserByAccount(string account)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetUserByAccount", account);
            return ds;
        }
        /// <summary>
        /// 写入用户信息到数据库
        /// </summary>
        /// <param name="dr">包含用户信息的数据行</param>
        /// <returns></returns>
        public static long InsertUser(DataRow dr)
        {
            return ((long)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(DAL.DataProvider.ConnectionString, "InsertUser", dr)[0].Value);
        }
        /// <summary>
        /// 写入用户信息到数据库
        /// </summary>
        /// <param name="dr">包含用户信息的数据行</param>
        /// <returns></returns>
        public static long InsertUserRiqi(SqlTransaction trans, DataRow dr)
        {
            if (trans ==null)
              return  (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "InsertUserRiqi", dr));
            else
                return (DAL.SqlHelper.ExecuteAppointedParameters(trans, "InsertUserRiqi", dr));
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="dr">包含用户信息的数据行</param>
        /// <returns></returns>
        public static int UpdateUser(DataRow dr)
        {
            return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "UpdateUser", dr));
        }
        /// <summary>
        /// 更新用户日期
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static int UpdateUserRiqi(SqlTransaction trans, DataRow dr)
        {
            if (trans == null)
                return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "UpdateUserRiqi", dr));
            else
                return (DAL.SqlHelper.ExecuteAppointedParameters(trans, "UpdateUserRiqi", dr));
        }
        #endregion
        #region 读取用户相关
        /// <summary>
        /// 获取包括省市县的地区信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetLocation( )
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetLocation" );
            return ds;
        }
        /// <summary>
        /// 获取学历信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetDegree()
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetDegree");
            return ds;
        }
        /// <summary>
        /// 获取职业信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetProfession()
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetProfession");
            return ds;
        }
        /// <summary>
        /// 获取行业信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetIndustry()
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetIndustry");
            return ds;
        }
        /// <summary>
        /// 获取爱好信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetHobby()
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetHobby");
            return ds;
        }
        #endregion
        #region 读取关系表
        /// <summary>
        /// 获取用户的爱好
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataSet GetUserHobby(long userID)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetUserHobby",userID);
            return ds;
        }
        #endregion
    }
}
