using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.BLL
{
    public class TablesBLL
    {
        /// <summary>
        /// 获取表的定义
        /// </summary>
        /// <param name="tableName">表的名称</param>
        /// <param name="ds">包含所有表结构的数据集</param>
        /// <returns></returns>
        public static DataSet GetTableFields(string tableName,  DataSet ds)
        {
            if (ds==null)
              ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetAllTableStructure");
            DataRow[] drs = ds.Tables[0].Select(string.Format("表名='{0}'", tableName));
            DataSet dsResults = ds.Clone();
            dsResults.Merge(drs);
            return dsResults;
        }
    }
}
