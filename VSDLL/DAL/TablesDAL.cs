using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.DAL
{
    public class TablesDAL
    {
        /// <summary>
        /// 获取所有的表描述定义
        /// </summary>
        /// <returns></returns>
        public static DataSet GetTablesStructure()
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetAllTableStructure");
            return ds;
        }
    }
}
