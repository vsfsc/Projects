using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSDLL.DAL
{
    public class MetaDataDAL
    {
        public static DataTable GetMetaDatas()
        {
            throw new NotImplementedException();
        }

        public static DataSet GetGroupMetaData()
        {
            DataSet ds = DAL.SqlHelper.ExecuteDataset(DAL.DataProvider.ConnectionString, "GetGroupMetaData");
            return ds;

        }
    }
}
