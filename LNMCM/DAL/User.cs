using System;
using System.Data;
using System.Data.SqlClient;

namespace LNMCM.DAL
{
    public class User
    {
        /// <summary>
        /// 重新导入删除的数据，AD和数据库
        /// </summary>
        /// <returns></returns>
        public static DataSet GetEnrollDel( )
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "lnmcm_GetEnrollDel" );
            return ds;
        }
        public static DataSet GetSchoolByCity(string cityName)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "lnmcm_GetSchoolByCity", cityName );
            return ds;
        }
        public static DataSet GetOrg(int parentID)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetOrg", parentID);
            return ds;
        }

        /// <summary>
        /// 根据报名序号获取报名信息
        /// </summary>
        /// <param name="enrollCode">报名序号</param>
        /// <returns></returns>
        internal static DataSet GetEnrollByEnrollCode(string enrollCode)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "lnmcm_GetEnrollByEnrollCode", enrollCode);
            return ds;
        }

        /// <summary>
        /// 获取指定代码学校下的报名成员
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <returns></returns>
        public static DataSet GetMembersByCode(string schoolCode)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "lnmcm_GetMembersBySchoolCode", schoolCode);
            return ds;
        }

        internal static DataSet GetMemberById(string id)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "lnmcm_GetMemberById", long.Parse(id));
            return ds;
        }

        internal static DataSet GetMemberByEnrollCode(string enrollCode)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "lnmcm_GetMembersByEnrollCode", enrollCode);
            return ds;
        }


        /// <summary>
        /// 根据单位编码获取单位信息
        /// </summary>
        /// <param name="orgCode">单位编码</param>
        /// <returns></returns>
        internal static DataSet GetOrgByOrgCode(string orgCode)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "lnmcm_GetOrgByOrgCode", orgCode);
            return ds;
        }
        /// <summary>
        /// 获取所有的报名信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllEnrolls( )
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "lnmcm_GetAllEnrolls");
            return ds;
        }
        /// <summary>
        /// 获取所有的包括成员的报名信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllEnrollsIncludeMembers()
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "lnmcm_GetAllEnrollInfo");
            return ds;
        }
        /// <summary>
        /// 获取指定代码学校下的报名信息
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <returns></returns>
        public static DataSet GetEntrollByCode(string schoolCode)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "lnmcm_GetEnrollByOrgCode", schoolCode);
            return ds;
        }
        public static DataSet GetRole()
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetRole");
            return ds;
        }
        public static DataSet GetUserByAccount(string account)
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetUserByAccount", account);
            return ds;
        }
        public static string InsertEntroll(DataRow dr)
        {
            return ((string)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(DAL.DataProvider.ConnectionString, "lnmcm_InsertEnroll", dr)[0].Value);
        }
        public static string InsertMember(DataRow dr)
        {
            return ((string)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(DAL.DataProvider.ConnectionString, "lnmcm_AddMember", dr)[0].Value);
        }
        public static string InsertEntroll(DataRow dr,SqlTransaction trans)
        {
            return ((string)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(trans, "lnmcm_InsertEnroll", dr)[0].Value);
        }
        public static int UpateEnroll(DataRow dr)
        {
            return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "lnmcm_UpdateEnroll", dr));
        }
        //删除报名，置标记位
        public static int DeleteEnroll(SqlTransaction trans, string enrollCode)
        {
            return (DAL.SqlHelper.ExecuteNonQuery(trans, "lnmcm_DeleteEnroll", enrollCode));
        }
        public static int DeleteEnroll(  string enrollCode)
        {
            return (DAL.SqlHelper.ExecuteNonQuery(DAL.DataProvider.ConnectionString, "lnmcm_DeleteEnroll", enrollCode));
        }
        public static int DeleteEnrollMembers(SqlTransaction trans, string enrollCode)
        {
            return (DAL.SqlHelper.ExecuteNonQuery(trans, "lnmcm_DelEnrollMembers", enrollCode));
        }
        public static string InsertMember(DataRow dr, SqlTransaction trans)
        {
            return ((string)DAL.SqlHelper.ExecuteNonQueryTypedParamsOutput(trans, "lnmcm_AddMember", dr)[0].Value);
        }
        public static int UpdateMember(DataRow dr)
        {
            return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "lnmcm_UpdateMember", dr));
        }
        public static int UpdateUser(DataRow dr)
        {
            return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "UpdateUser", dr));
        }

        public static int DelMember(DataRow dr)
        {
            return (DAL.SqlHelper.ExecuteAppointedParameters(DAL.DataProvider.ConnectionString, "lnmcm_DelMemberByID", dr));
        }
    }
}
