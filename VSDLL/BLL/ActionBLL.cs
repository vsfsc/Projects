using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.BLL
{
    public class ActionBLL
    {

        /// <summary>
        /// 获取活动录入中的操作数据集
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static DataTable  GetAcivityActions(long userID)
        {
            DataSet dsMyActions = DAL.ActionDAL.GetUserActionByUserID(userID );
            DataSet dsActions = DAL.ActionDAL.GetAllActions();
            DataTable dtActions = dsActions.Tables[0];
            DataRow[] drs;
            foreach  (DataRow dr in  dsMyActions.Tables[0].Rows  )//通过我的操作来遍历操作，更新时段、频次、描述
            {
                drs = dtActions.Select("ActionID=" + dr["ActionID"]);
                if (drs.Length > 0)
                {
                    drs[0]["ShiDuanID"] = dr["ShiDuanID"];
                    drs[0]["FrequencyID"] = dr["FrequencyID"];
                    drs[0]["Description"] = dr["Description"];
                    drs[0]["Probability"] = dr["Probability"];
                }

            }
            dtActions.Columns[0].ColumnName = "ID";
            dtActions.AcceptChanges();
            dtActions.DefaultView.Sort = "Probability desc";
            dtActions=dtActions.DefaultView.Table;
            return dtActions ;
        }

        /// <summary>
        /// 批量保存活动，同时保存用户信息
        /// </summary>
        /// <param name="dr">用户操作设置数据行</param>
        /// <param name="errMsg">返回的错误信息</param>
        /// <param name="isMy">判断用户操作的修改状态：01新建设置（InsertUserAction），11修改设置（UpdateUserAction）</param>
        /// <returns></returns>
        public static bool OptionUserAction(string isMy,DataRow dr, ref string errMsg)
        {
            using (SqlTransaction trans = DAL.DataProvider.CurrentTransactionEx)
            {
                try
                {
                    if (isMy=="01")
                    {
                        dr["Created"] = DateTime.Now;
                        dr["Modified"] = DateTime.Now;
                        DAL.ActionDAL.InsertUserAction(trans, dr);
                    }
                    else if (isMy=="11")
                    {
                        dr["Modified"] = DateTime.Now;
                        DAL.ActionDAL.UpdateUserAction(trans, dr);
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    errMsg = ex.Message;
                    return false;
                }
            }
        }

        public static DataRow GetActionByID(int actionId, DataTable sysActions)
        {
            DataRow[] drs = sysActions.Select("ActionID=" + actionId);
            if (drs.Length > 0)
            {
                return drs[0];
            }
            else
            {
                return null;
            }
        }
        #region 更新操作
        /// <summary>
        /// 根据汇总的操作次数更新操作和我的操作
        /// </summary>
        /// <param name="dtActions"></param>
        /// <param name="dtUserActions"></param>
        /// <returns></returns>
        public static bool UdateActionProbability(DataTable dtActions, DataTable dtUserActions)
        {
            using (SqlTransaction trans = DAL.DataProvider.CurrentTransactionEx)
            {
                try
                {
                    foreach (DataRow dr in dtActions.Rows)
                        DAL.ActionDAL.UpdateAction(trans, dr);
                    foreach (DataRow dr in dtUserActions.Rows)
                        DAL.ActionDAL.UpdateUserAction(trans, dr);
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
