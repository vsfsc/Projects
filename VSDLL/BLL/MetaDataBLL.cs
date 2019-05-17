using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.BLL
{
    public class MetaDataBLL
    {
        /// <summary>
        /// 获取系统设置的指定标题的元数据集合
        /// </summary>
        /// <param name="title">要筛选的元数据集合名称</param>
        /// <param name="dtMetaDatas">所有元数据集合</param>
        /// <returns>对应元数据集合的所有元数据</returns>
        public static DataTable GetMetaDatasByTitle(string title,DataTable dtMetaDatas)
        {
            int tableId = GetMetaDataTableIDByTitle(title, dtMetaDatas);
            DataRow[] dtMetas = dtMetaDatas.Select("TableID="+tableId,"ItemID Asc");
            return dtMetas.CopyToDataTable();

        }

        /// <summary>
        /// 获取系统设置的指定标题的元数据集合对应ItemID
        /// </summary>
        /// <param name="title">要筛选的元数据集合名称</param>
        /// <param name="dtMetaDatas">所有元数据集合</param>
        /// <returns>元数据集合项对应ItemID</returns>
        public static int GetMetaDataTableIDByTitle(string title,DataTable dtMetaDatas)
        {
            int tableId =0;
            DataRow[] dtMetas = dtMetaDatas.Select(string.Format("Title='{0}' and TableID=0", title), "");
            if (dtMetas.Length > 0)
            {
                tableId =DAL.SystemDataExtension.GetInt16(dtMetas[0],"ItemID");
            }
            return tableId;
        }

        public static DataTable GetMetaDataByGroup(string gpTitle,DataSet dsMetaData)
        {
            DataRow[] drs = dsMetaData.Tables[0].Select("GroupTitle='"+gpTitle+"'","ItemID Asc");
            return drs.CopyToDataTable();
        }
    }
}
