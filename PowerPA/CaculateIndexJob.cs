using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace PowerPA
{
    public class CaculateIndexJob:SPJobDefinition
    {
        public CaculateIndexJob() : base() { }

        public CaculateIndexJob(string jobName, SPWebApplication webApp) : base(jobName, webApp, null, SPJobLockType.Job) { this.Title = jobName; }
        public override void Execute(Guid targetInstanceId)
        {
            base.Execute(targetInstanceId);
            List<string> emailTo = new List<string>();
            SPWebApplication webApp = this.Parent as SPWebApplication;
            SPContentDatabase contentDB = webApp.ContentDatabases[targetInstanceId];
            if (contentDB == null)
                contentDB = webApp.ContentDatabases[0];
            SPWeb web = contentDB.Sites[0].RootWeb;


            // 取参数，通过Properties可以从外部传递参数，但要求参数可以被序列化和反列化
            string siteUrl = this.Properties["SiteUrl"].ToString();
            //上面的内容没有用到

            string userName = "系统帐户";
            string showUrl = ConfigurationManager.AppSettings["retUrl"];
            int delayDays = int.Parse(System.Configuration.ConfigurationManager.AppSettings["delayDays"]);
            SPList oList = web.Lists.TryGetList("讨论列表");
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><And><Eq><FieldRef Name='Author' /><Value Type='User'>" + userName + "</Value></Eq><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + DateTime.Today.AddDays(-delayDays).ToString("yyyy-MM-dd") + "</Value></Geq></And></Where>";
            SPListItemCollection lstItems = oList.GetItems(oQuery);
            if (lstItems.Count == 0)
            {

            }
        }
        
    }
}
