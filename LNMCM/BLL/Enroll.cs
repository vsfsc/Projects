using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LNMCM.BLL
{
    public class Enroll
    {
        public static int AddEnroll(DataRow drEnroll, DataRow drMember)
        {
            using (SqlTransaction trans = DAL.DataProvider.CurrentTransactionEx)
                try
                {
                    DAL.User.InsertEntroll(drEnroll, trans);
                    DAL.User.InsertMember(drMember, trans);
                    trans.Commit();
                    return 1;
                }

                catch(Exception ex)
                {
                    trans.Rollback();
                    return 0;
                }

        }
        public static int DeleteEnroll(string enrollCode)
        {
            using (SqlTransaction trans = DAL.DataProvider.CurrentTransactionEx)
                try
                {
                    DAL.User.DeleteEnroll( trans,enrollCode );
                    DAL.User.DeleteEnrollMembers( trans,enrollCode );
                    trans.Commit();
                    return 1;
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    return 0;
                }

        }
        public static DataTable GetEnrollByEnrollCode(string enrollCode)
        {
            DataSet ds = DAL.User.GetEnrollByEnrollCode(enrollCode);
            DataTable dt = ds.Tables[0];
            return dt;
        }

        internal static string GetOrgNameByOrgCode(string orgCode)
        {
            string orgName = "";
            DataTable dt = DAL.User.GetOrgByOrgCode(orgCode).Tables[0];
            if (dt.Rows.Count>0)
            {
                orgName = dt.Rows[0]["Name"].ToString();
            }
            return orgName;
        }

        internal static DataTable GetMemberById(string id)
        {
            DataSet ds = DAL.User.GetMemberById(id);
            DataTable dt = ds.Tables[0];
            return dt;
        }

        internal static DataTable GetMemberByEnrollCode(string enrollCode)
        {
            DataSet ds = DAL.User.GetMemberByEnrollCode(enrollCode);
            DataTable dt = ds.Tables[0];
            return dt;
        }
    }
}
