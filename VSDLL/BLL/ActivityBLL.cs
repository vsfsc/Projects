using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.BLL
{
    /// <summary>
    /// 活动相关的业绩处理
    /// </summary>
    public class ActivityBLL
    {
        /// <summary>
        /// 批量保存活动，同时保存用户信息
        /// </summary>
        /// <param name="drsActivity"></param>
        /// <param name="drUserRiqi"></param>
        /// <returns></returns>
        public static bool InsertActivities(DataTable dtActivity, DataRow drUserRiqi,ref string errMsg)
        {
            using (SqlTransaction trans = DAL.DataProvider.CurrentTransactionEx)
            {
                try
                {
                    if (drUserRiqi != null)
                    {
                        if (drUserRiqi["Created"] == DBNull.Value)
                        {
                            drUserRiqi["Created"] = DateTime.Now;
                            drUserRiqi["Modified"] = DateTime.Now;
                            DAL.UserDAL.InsertUserRiqi(trans, drUserRiqi);
                        }

                        else
                        {
                            DAL.UserDAL.UpdateUserRiqi(trans, drUserRiqi);

                        }
                    }
                    foreach (DataRow dr in dtActivity.Rows)
                    {
                        if (dr["Created"] == DBNull.Value)
                        {
                            dr["Created"] = DateTime.Now;
                            dr["Modified"] = DateTime.Now;
                            DAL.ActivityDAL.InsertActivity(trans, dr);
                        }
                        else if (dr["Modified"] == DBNull.Value)
                        {
                            dr["Modified"] = DateTime.Now;
                            DAL.ActivityDAL.UpdateActivity(trans, dr);
                        }

                    }


                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    errMsg = ex.Message ;
                    return false;
                }
            }
        }
        /// <summary>
        /// 用户的当天活动是否已经存在相应的活动
        /// </summary>
        /// <param name="dtActivities">包含当日活动的数据集</param>
        /// <param name="action"></param>
        /// <param name="taskName"></param>
        /// <returns></returns>
        public static  bool CheckActivity(DataTable dtActivities,int actionID,long taskID)
        {
            DataRow[] drs = dtActivities.Select("ActionID=" + actionID );
            string task;
            foreach (DataRow dr in drs)
            {
                task = VSDLL.DAL.SystemDataExtension.GetString(dr, "TaskID");
                if (task == (taskID ==0?string.Empty :taskID.ToString ()))
                    return true; 
            }
            return false;
        }
        /// <summary>
        /// 获取用户活动日期
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="typeID">typeID=1最后一次活动日期；typeID=0首次录入活动时间</param>
        /// <returns></returns>
        public static DateTime?  GetLastDateActivityUserID(long userID,int typeID)
        {
            DataSet ds = DAL.ActivityDAL.GetActivityLastDayByUserID(userID,typeID );
            DateTime? lastDay=null;
            if (ds.Tables[0].Rows.Count > 0)
                lastDay = (DateTime)ds.Tables[0].Rows[0]["Riqi"];
            return lastDay;

        }
    }
}
