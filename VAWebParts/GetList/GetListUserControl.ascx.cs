using System;
using System.Data;
using System.Web.UI;
using VAWebParts.ListsWebReference;
using System.Xml;
using System.Net;
using Microsoft.SharePoint;

namespace VAWebParts.GetList
{
    public partial class GetListUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt= GetUsersDataTable();
            if (dt.Rows.Count>0)
            {
                gvGetList.DataSource = dt;
                gvGetList.DataBind();
            }
            
            
        }
        public void GetListsBind()
        {
            DataSet dataset;
            DataTable dataTable;
            DataColumn dataColumn;
            DataRow dataRow;
            Lists listReference = new Lists();
            listReference.Url = "http://neufarm/_vti_bin/lists.asmx";
            listReference.Credentials = CredentialCache.DefaultCredentials;
            XmlNode node = listReference.GetListCollection();
            dataTable = new DataTable();
            dataColumn = new DataColumn("Title");
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("Descrlption");
            dataTable.Columns.Add(dataColumn);
            dataset = new DataSet();
            dataset.Tables.Add(dataTable);
            foreach (XmlNode xmlnode in node)
            {
                dataRow = dataTable.NewRow();
                dataRow["Title"] = xmlnode.Attributes["Title"].Value;
                dataRow["Description"] = xmlnode.Attributes["Description"].Value;
                dataTable.Rows.Add(dataRow);
            }
            gvGetList.DataSource = dataset;
            gvGetList.DataBind();
        }
        public DataTable GetUsersDataTable()
        {
            DataTable dt = newDataTable();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPWeb web = SPContext.Current.Web)
                {
                    SPUserCollection users1 = web.AllUsers;
                    SPUserCollection users2 = web.SiteUsers;
                    SPUserCollection users3 = web.Users;

                    SPList list = web.SiteUserInfoList;
                    // web.Lists["User Information List"]; 
                    foreach (SPListItem item in list.Items)
                    {
                        
                        if (!item["ContentType"].ToString().Equals("Person"))
                            continue;
                        DataRow dr = dt.NewRow();
                        dr["name"] = item["Title"];
                        dr["loginName"] = item["Name"];
                        //dr["Email"] = item["Email"];
                        dr["picture"] = item["Picture"];
                        dt.Rows.Add(dr);
                        //string name = item["Title"] + "";
                        //string loginName = item["Name"] + "";
                        //string Email = item["Email"] + "";
                        //string picture = item["Picture"] + "";
                        //... 其他属性
                    }
                }
            });
            return dt;
        }
        private DataTable newDataTable()
        {
            DataTable dataTable = new DataTable();
            DataColumn dataColumn;
            dataColumn = new DataColumn("name");
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("loginName");
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("Email");
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("picture");
            dataTable.Columns.Add(dataColumn);
            return dataTable;
        }
    }
}
