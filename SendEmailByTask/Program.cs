using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Data;
using System.IO;
using System.Collections;
using System.DirectoryServices;

namespace SendEmailByTask
{
    class Program
    {
        static string siteUrl;
        static ClientContext clientContext;
        static void Main(string[] args)
        {
            try
            {
                siteUrl = ConfigurationManager.AppSettings["siteUrl"];
                CheckTaskList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
       static string emailDespName;
       static  string taskDocField;
        /// <summary>
        /// 遍历任务,
        /// </summary>
       private static void CheckTaskList()
       {
           string[] taskNames = System.Configuration.ConfigurationManager.AppSettings["TaskName"].Split(';');
           emailDespName = ConfigurationManager.AppSettings["emailDisplayName"];
           string emailTitle = ConfigurationManager.AppSettings["emailTitle"];
           string emailBody = ConfigurationManager.AppSettings["emailBody"].Replace(";", "<br>");
           string emailTitle1 = ConfigurationManager.AppSettings["emailTitle1"];
           string emailBody1 = ConfigurationManager.AppSettings["emailBody1"].Replace(";", "<br>");
           //任务对应的文档
           taskDocField = ConfigurationManager.AppSettings["taskDocField"];
           clientContext = new ClientContext(siteUrl);
           Site site = clientContext.Site;
           Web web = clientContext.Web;
           clientContext.Load(site, s => s.Url);
           clientContext.Load(web, w => w.Title);

           foreach (string task in taskNames)
           {
                try
                {
                    List oList = web.Lists.GetByTitle(task);

                    clientContext.Load(oList, list => list.Title, list => list.DefaultDisplayFormUrl);
                    Field f1 = oList.Fields.GetByInternalNameOrTitle(taskDocField);
                    FieldLookup field = clientContext.CastTo<FieldLookup>(f1);
                    //clientContext.Load(f1,f=>f.InternalName,f=>f.Id,f=>f.Title );
                    clientContext.Load(field);
                    clientContext.Load(oList.Fields);
                    clientContext.ExecuteQuery();
                    taskDocField = f1.InternalName;
                    CamlQuery oQuery = new CamlQuery();
                    DateTime today = DateTime.Today;
                    //任务开始通知
                    string xmlQuery = "<Eq><FieldRef Name='StartDate'/><Value Type='DateTime'>" + today.AddDays(1).ToString("yyyy-MM-dd") + "</Value></Eq>";
                    oQuery.ViewXml = "<View><Query><Where>" + xmlQuery + "</Where></Query></View>";
                    DealEmail(oList, oQuery, emailTitle, emailBody, site.Url + oList.DefaultDisplayFormUrl);

                    oQuery = new CamlQuery();
                    //任务结束通知 
                    xmlQuery = "<And> <Lt><FieldRef Name='StartDate' /> <Value Type='DateTime'>" + today.ToString("yyyy-MM-dd") + "</Value></Lt> <And><Gt><FieldRef Name='DueDate' /><Value Type='DateTime'>" + today.ToString("yyyy-MM-dd") + "</Value></Gt><Lt> <FieldRef Name='DueDate' /> <Value Type='DateTime'>" + today.AddDays(7).ToString("yyyy-MM-dd") + "</Value></Lt></And></And></Eq>";
                    oQuery.ViewXml = "<View><Query><Where>" + xmlQuery + "</Where></Query></View>";
                    DealEmail(oList, oQuery, emailTitle1, emailBody1, site.Url + oList.DefaultDisplayFormUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.ReadLine();
                }

            }
       }

       private static void DealEmail(List oList, CamlQuery oQuery, string emailTitle, string emailBody, string taskUrl)
       {
           ListItemCollection lstItems = oList.GetItems(oQuery);
           clientContext.Load(lstItems, items => items.Include(item => item["ID"], item => item["Title"], item => item["StartDate"], item => item["DueDate"], item => item["AssignedTo"], item => item[taskDocField], item => item.Id, item => item["FileRef"]));
           clientContext.ExecuteQuery();
           string itemUrl;
           if (lstItems.Count > 0)
           {
               foreach (ListItem item in lstItems)
               {
                   try
                   {
                       List<string> toEmails = new List<string>();
                       if (item["AssignedTo"] == null) return;
                       FieldUserValue[] authors = (FieldUserValue[])item["AssignedTo"];
                       foreach (var author in authors)
                       {
                           var authorUser = clientContext.Web.SiteUsers.GetById(author.LookupId);
                           clientContext.Load(authorUser, u => u.Email, u => u.LoginName);
                           clientContext.ExecuteQuery();
                           DirectoryEntry dUser = ADHelper.GetDirectoryEntryByAccount(authorUser.LoginName.Substring(authorUser.LoginName.IndexOf("\\") + 1));
                           var email = dUser.Properties["mail"].Value.ToString();
                           toEmails.Add(email);
                       }
                       FieldLookupValue docField = item[taskDocField] as FieldLookupValue;
                       string docName = docField != null ? docField.LookupValue : "";
                       itemUrl = taskUrl + "?ID=" + item["ID"].ToString();
                       emailBody = string.Format(emailBody, "<a href=\"" + itemUrl + "\">" + item["Title"].ToString() + "</a>", item["StartDate"] != null ? ((DateTime)item["StartDate"]).ToShortDateString() : "", item["DueDate"]!=null?((DateTime)item["DueDate"]).ToShortDateString():"", docName);

                       SendMail("training@mail.neu.edu.cn", emailDespName, "110004cc", toEmails.ToArray(), emailTitle, emailBody);
                       Console.WriteLine("ok");

                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ex.ToString());
                       Console.ReadLine();
                   }
               }
           }
       }
        public static bool SendMail(string fromEmail, string fromDisplayName, string pwd, string[] toMail, string toSubject, string toBody)
        {
            ////设置发件人信箱,及显示名字
            MailAddress from = new MailAddress(fromEmail, fromDisplayName);
            //设置收件人信箱,及显示名字 
            //MailAddress to = new MailAddress(TextBox1.Text, "");
            //创建一个MailMessage对象
            MailMessage oMail = new MailMessage();

            oMail.From = from;
            for (int i = 0; i < toMail.Length; i++)
            {
                oMail.To.Add(toMail[i].ToString());
            }


            oMail.Subject = toSubject; //邮件标题 
            oMail.Body = toBody; //邮件内容

            oMail.IsBodyHtml = true; //指定邮件格式,支持HTML格式 
            oMail.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");//邮件采用的编码 
            //oMail.Priority = MailPriority.High;//设置邮件的优先级为高
            //Attachment oAttach = new Attachment("");//上传附件
            //oMail.Attachments.Add(oAttach); 

            //发送邮件服务器 +
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.neu.edu.cn";// fromEmail.Substring(fromEmail.IndexOf("@") + 1); //163.com指定邮件服务器smtp.sina.com
            client.Credentials = new NetworkCredential(fromEmail, pwd);//指定服务器邮件,及密码

            //发送
            try
            {
                client.Send(oMail); //发送邮件
                oMail.Dispose(); //释放资源
                return true;// "恭喜你！邮件发送成功。";
            }
            catch
            {
                oMail.Dispose(); //释放资源
                return false;// "邮件发送失败，检查网络及信箱是否可用。" + e.Message;
            }


        }
    }
}
