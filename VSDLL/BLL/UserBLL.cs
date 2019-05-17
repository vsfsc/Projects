using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.BLL
{
    public class UserBLL
    {
        #region 相关信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static int GetUser_RiqiType(DataSet dsUser_Riqi,DateTime dt)
        {
           DataRow[] drs=dsUser_Riqi.Tables[0].Select ("Riqi='"+dt.ToString("yyyy-MM-dd")+"'") ;
            if (drs.Length > 0)
                return (int)drs[0]["TypeID"];
            return -1;

        }

        public static DataRow GetUser_RiqiByRiqi(DataSet dsUser_Riqi,DateTime dt)
        {
            DataRow[] drs = dsUser_Riqi.Tables[0].Select("Riqi='" + dt.ToString("yyyy-MM-dd") + "'");
            if (drs.Length > 0)
                return drs[0];
            return null;
        }

        #endregion
        #region 地址相关
        /// <summary>
        /// 从所有地区表中查找与LocationID同一父级的地区集合，并返回父ID
        /// </summary>
        /// <param name="dtLocations">包含地区的数据表</param>
        /// <param name="LocationID">地区ID</param>
        /// <param name="parentID">返回当前地区ID的父ID</param>
        /// <returns></returns>
        public static DataSet GetParentLocationsByID( DataTable dtLocations,  int LocationID ,ref int parentID )
        {

            DataRow[] drs = dtLocations.Select("LocationID=" + LocationID);
            DataSet ds = new DataSet();
            int deep = 0;
            int pid = 0;
            if (drs.Length >0)
            {
                deep = (int)drs[0]["deep"];
                pid = (int)drs[0]["PID"];
            }
            DataRow[] retDrs = dtLocations.Select("PID=" + pid);
            ds.Merge(retDrs);
            parentID = pid;
            return ds;
        }
        /// <summary>
        /// 返回当前地区ID下的所有子地区
        /// </summary>
        /// <param name="dtLocations">包含地区的数据表</param>
        /// <param name="LocationID">地区ID</param>
        /// <returns></returns>
        public static DataSet GetLocationsByParentID(DataTable dtLocations, int LocationID)
        {

            DataRow[] drs = dtLocations.Select("PID=" + LocationID);
            DataSet ds = new DataSet();
            ds.Merge(drs);
            return ds;
        }
        #endregion
    }
}
