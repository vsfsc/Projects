using Microsoft.SharePoint;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace VAKPI
{
    public class common
    {
        
        /// <summary>
        /// ///获取约束数据
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="listName"></param>
        /// <param name="userID"></param>
        /// <param name="flag">0:系统目标；11：用户的当前目标</param>
        /// <returns></returns>
        public static DataTable GetConstraintList(string siteUrl,string listName,int userID)
        {
            DataTable dt = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
           {
               try
               {
                   //string siteUrl = webObj.SiteUrl;
                   using (SPSite spSite = new SPSite(siteUrl)) //找到网站集
                   {
                       using (SPWeb spWeb = spSite.OpenWeb())
                       {
                           SPList spList = spWeb.Lists.TryGetList(listName);
                           if (spList != null)
                           {
                               SPQuery qry = new SPQuery();

                               qry.Query = @"<Where><Or><Eq><FieldRef Name='Flag' /><Value Type='Number'>0</Value></Eq><And><Eq><FieldRef Name='Flag' /><Value Type='Number'>11</Value>
      </Eq><Eq><FieldRef Name = 'Author' LookupId = 'TRUE'></FieldRef><Value Type = 'User'>" + userID + "</Value></Eq></And></Or></Where>";

                               dt = spList.GetItems(qry).GetDataTable();
                               if (dt == null)
                                   dt = spList.GetItems().GetDataTable().Clone();
                           }
                       }
                   }
               }
               catch (Exception ex)
               {
                   dt = null;//lbErr.Text = ex.ToString();
               }
           });
            return dt;
        }

    }
}
