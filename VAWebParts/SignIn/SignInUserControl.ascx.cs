using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using System;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace VAWebParts.SignIn
{
    
    public partial class SignInUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            SPWeb web = SPContext.Current.Web;
            if (isLogIn(web))
            {
                ss.Visible = true;
                string[] userName = GetUserInfo(web);
                lbDisplayName.Text = userName[1];//签到用户姓名
                SPList list = web.Lists.TryGetList("SignIn");
                if (list == null)//首先判断当前网站是否已经创建签到列表
                {
                    NewList(web, "SignIn");
                    //list = CreateList("1");
                }

                
                imghp.ImageUrl = GetPhotoUrlByUserProfile(userName[0]);
                imgPhoto.ImageUrl= GetPhotoUrlByUserProfile(userName[0]);
                SPListItem listItem = GetListData(web, list, userName[0]);
                if (listItem != null)
                {
                    DateTime LastTime = DateTime.Parse(listItem["LastTime"].ToString());

                    if (LastTime.Date.Equals(DateTime.Now.Date))//当日已签到
                    {                        
                        ImgSigned.Visible = true;
                        SigninBtn.Visible = false;
                    }
                    else
                    {
                        ImgSigned.Visible = false;
                        SigninBtn.Visible = true;
                    }
                    //上次签到记录
                    lbDays.Text = listItem["LastDays"].ToString();//上次签到持续天数
                    lbLastTime.Text = LastTime.ToString();//上次签到时间
                    lbLastOrder.Text = listItem["LastOrder"].ToString();//上次签到排行
                    lbLastScore.Text = listItem["LastScore"].ToString();//上次签到积分
                    lbIP.Text = listItem["LastIP"].ToString();//上次签到IP
                    //签到统计
                    
                    lbDegree.Text = listItem["Degree"].ToString();//签到等级
                    lbAllDays.Text = listItem["AllDays"].ToString();//签到累计天数
                    GetStars(int.Parse(listItem["AllDays"].ToString()));//签到等级
                    lbAllScores.Text = listItem["AllScore"].ToString();//签到累计积分
                    lbRank.Text = GetRank(web,list,userName[0],1, LastTime).ToString();//签到排行
                }
                else
                {

                }
            }
            else//未登录
            {
                ss.Visible = false;
            }          
            
        }

        public void NewList(SPWeb web, string listTitle)
        {            
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (web)
                {
                    web.AllowUnsafeUpdates = true;
                    SPList list = web.Lists.TryGetList(listTitle);
                    if (list==null)
                    {
                        Guid newListGuid = web.Lists.Add(listTitle, "签到列表", SPListTemplateType.GenericList);
                        list = web.Lists[newListGuid]; //取得刚才添加的List.Guid
                        //给这个新创建的List添加字段.注意，缺省Title作为签到人用户名，即UName
                        //1、DName：签到人姓名 Text
                        SPField fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "DisplayName");
                        fldName.Description = "签到人姓名";
                        fldName.Required = false; //在新建项目时,此字段是否是必填的.
                        list.Fields.Add(fldName);
                        //2、LastTime：上次签到时间 DateTime
                        fldName = (SPFieldDateTime)list.Fields.CreateNewField(SPFieldType.DateTime.ToString(), "LastTime");
                        fldName.Description = "上次签到时间";
                        fldName.Required = false; //在新建项目时,此字段是否是必填的.
                        list.Fields.Add(fldName);
                        //3、LastScore：上次签到积分 Text
                        fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "LastScore");
                        fldName.Description = "上次签到积分";
                        fldName.Required = false; //在新建项目时,此字段是否是必填的.
                        list.Fields.Add(fldName);
                        //4、LastOrder：上次签到排行
                        fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "LastOrder");
                        fldName.Description = "上次签到排行";
                        fldName.Required = false; //在新建项目时,此字段是否是必填的.
                        list.Fields.Add(fldName);
                        //5、LastIP：上次签到IP
                        fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "LastIP");
                        fldName.Description = "上次签到IP";
                        fldName.Required = false; //在新建项目时,此字段是否是必填的.
                        list.Fields.Add(fldName);
                        //6、LastDays：上次签到累计天数
                        fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "LastDays");
                        fldName.Description = "上次签到累计天数";
                        fldName.Required = false; //在新建项目时,此字段是否是必填的.
                        list.Fields.Add(fldName);
                        //7、AllDays：签到总天数
                        fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "AllDays");
                        fldName.Description = "签到总天数";
                        fldName.Required = false; //在新建项目时,此字段是否是必填的.
                        list.Fields.Add(fldName);
                        //8、AllOrder：签到总排行
                        fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "AllOrder");
                        fldName.Description = "签到总排行";
                        fldName.Required = false; //在新建项目时,此字段是否是必填的.
                        list.Fields.Add(fldName);
                        //9、AllScore：签到总积分
                        fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "AllScore");
                        fldName.Description = "签到总积分";
                        fldName.Required = false; //在新建项目时,此字段是否是必填的.
                        list.Fields.Add(fldName);
                        //10、Degree：签到等级
                        fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "Degree");
                        fldName.Description = "签到等级";
                        fldName.Required = false; //在新建项目时,此字段是否是必填的.
                        list.Fields.Add(fldName);

                        list.Update();
                        web.AllowUnsafeUpdates = false;
                    }                    
                }
            });
            
            
        }


        /// <summary>
        /// 获取指定用户的签到数据记录
        /// </summary>
        /// <param name="web">签到网站</param>
        /// <param name="listName">签到列表名称</param>
        /// <param name="userName">用户名</param>
        public static SPListItem GetListData(SPWeb web, SPList list, string userName)
        {
            SPQuery query = new SPQuery();
            query.Query = "<Where><Eq><FieldRef Name='Title'/><Value Type='Text'>"+ userName + "</Value></Eq></Where>";//查询指定用户名的用户签到记录
            SPListItemCollection listItems = list.GetItems(query);
            if (listItems.Count>0)//当前用户已有签到记录
            {
                SPListItem listItem = listItems[0];
                return listItem;
            }
            else//当前用户从未签到过
            {
                return null;
            }            
        }

        private void GetStars(int days)
        {
            if (days > 0 & days <= 7)
            {
                imgStar.ImageUrl= "/_layouts/15/images/star01.png";
            }
            else if (days > 7 & days <= 30)
            {
                imgStar.ImageUrl = "/_layouts/15/images/star02.png";
            }
            else if (days > 30 & days <= 90)
            {
                imgStar.ImageUrl = "/_layouts/15/images/star03.png";
            }
            else if (days > 90 & days <= 365)
            {
                imgStar.ImageUrl = "/_layouts/15/images/star04.png";
            }
            else
            {
                imgStar.ImageUrl = "/_layouts/15/images/star05.png";
            }
        }

        private string GetPhotoUrlByUserProfile(string loginName)
        {
            string photoUrl = "";
            if (SPContext.Current.Site.OpenWeb().CurrentUser == null)
            {
                photoUrl = "/_layouts/15/images/PersonPlaceholder.200x150x32.png";
                return photoUrl;
            }
            string accountName = loginName.Substring(loginName.LastIndexOf("|") + 1);
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                SPServiceContext serviceContext = SPServiceContext.GetContext(site);
                UserProfileManager upm = new UserProfileManager(serviceContext);
                if (upm.UserExists(accountName))
                {
                    UserProfile u = upm.GetUserProfile(accountName);
                    if (u[PropertyConstants.PictureUrl].Value == null)
                        photoUrl = "/_layouts/15/images/PersonPlaceholder.200x150x32.png";
                    else
                        photoUrl = u[PropertyConstants.PictureUrl].Value.ToString();
                }
            }
            return photoUrl;
        }

        public static void getDataFromSPList(string siteURL, string listName,string userName)
        {
            ClientContext clientContext = new ClientContext(siteURL);
            Web web = clientContext.Web;
            ListCollection collList = web.Lists;

            
            //CamlQuery
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View></View>";

            List planlist = collList.GetByTitle(listName);
            Microsoft.SharePoint.Client.ListItemCollection collListItem = planlist.GetItems(camlQuery);
            clientContext.Load(collListItem,
                items => items.Include(
                    item => item.Id,
                    item => item.DisplayName,
                    item => item.HasUniqueRoleAssignments)
                );
            clientContext.ExecuteQuery();

            foreach (Microsoft.SharePoint.Client.ListItem olistItem in collListItem)
            {
                Console.WriteLine("ID: {0} \nDisplay name: {1} \nUnique role assignments: {2}",
                    olistItem.Id, olistItem.DisplayName, olistItem.HasUniqueRoleAssignments);
            }


            clientContext.Load(planlist);
            clientContext.ExecuteQuery();

            Console.WriteLine(planlist.Title.ToString());
            Console.WriteLine(planlist.ItemCount.ToString());
            Console.ReadKey();
        }

        /// <summary>
        /// 根据签到天数计算用户签到等级
        /// </summary>
        /// <returns></returns>
        public string GetDegreeByDays(int days)
        {
            string ranksImageUrl = "";
            if (days>0&days<=7)
            {
                ranksImageUrl = "/_layouts/15/images/star01.png";
            }
            else if (days > 7 & days <= 30)
            {
                ranksImageUrl = "/_layouts/15/images/star02.png";
            }
            else if(days > 30 & days <= 90)
            {
                ranksImageUrl = "/_layouts/15/images/star03.png";
            }
            else if (days > 90 & days <= 365)
            {
                ranksImageUrl = "/_layouts/15/images/star04.png";
            }
            else
            {
                ranksImageUrl = "/_layouts/15/images/star05.png";
            }
            return ranksImageUrl;
        }

        /// <summary>
        /// 获取当前登录用户名及头像信息
        /// </summary>
        /// <returns></returns>
        public string[] GetUserInfo(SPWeb web)
        {
            string[] userName =new string[2];
            userName[0]= web.CurrentUser.LoginName.ToString();
            userName[1] = web.CurrentUser.Name.ToString();
            return userName;
        }
        public static string GetLoginName(string userSid)
        {
            string loginName = string.Empty;
            string domainName = string.Empty;
            try
            {
                domainName = Domain.GetCurrentDomain().Name; //获取domain name
                DirectoryEntry entry = new DirectoryEntry(String.Format("LDAP://{0}/<SID={1}>", domainName, userSid)); //根据sid获得AD用户
                loginName = entry.Properties["sAMAccountName"][0].ToString(); //获得sAMAccountName
            }
            catch { }

            if (!string.IsNullOrEmpty(loginName) && !string.IsNullOrEmpty(domainName))
            {
                loginName = string.Format("{0}\\{1}", domainName, loginName); //使用domain name和sAMAccountName拼接login name
            }

            return loginName;
        }
        public static string GetDisplayName(string userSid)
        {
            string displayName = string.Empty;
            string domainName = string.Empty;
            try
            {
                domainName = Domain.GetCurrentDomain().Name; //获取domain name
                DirectoryEntry entry = new DirectoryEntry(String.Format("LDAP://{0}/<SID={1}>", domainName, userSid)); //根据sid获得AD用户
                displayName = entry.Properties["displayName"][0].ToString(); //获得displayName，显示名
            }
            catch { }

            return displayName;
        }
        public string GetIp()
        {
            string userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (userIP == null || userIP == "")
            {
                userIP = Request.ServerVariables["REMOTE_ADDR"];
            }
            return userIP;
        }
        public static string GetIP()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    var temp = webClient.DownloadString("http://iframe.ip138.com/ic.asp");
                    var ip = Regex.Match(temp, @"\[(?<ip>\d+\.\d+\.\d+\.\d+)]").Groups["ip"].Value;
                    return !string.IsNullOrEmpty(ip) ? ip : null;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        /// <summary>
        /// 获取IP归属地
        /// </summary>
        /// <returns></returns>
        public string GetIpAddress()
        {
            string userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (userIP == null || userIP == "")
            {
                userIP = Request.ServerVariables["REMOTE_ADDR"];
            }
            WebRequest request = WebRequest.Create("http://www.ip138.com/ips138.asp?ip=" + userIP);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
            string read = reader.ReadToEnd();
            Regex regex = new Regex("<td align=\"center\"><ul class=\"ul1\"><li>本站数据：(?<title>.*?)</li>");
            if (regex.IsMatch(read))
            {
                read = regex.Match(read).Groups["title"].Value;
            }
            return read+"("+ userIP + ")";
        }
        public bool isLogIn(SPWeb web)
        {
            bool isOwner=false;
            if (web.CurrentUser != null)
            {                
                isOwner = web.IsCurrentUserMemberOfGroup(web.AssociatedOwnerGroup.ID);
            }
            return isOwner;
        }
        /// <summary>
        /// 签到按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SigninBtn_Click(object sender, ImageClickEventArgs e)
        {
            Random rd = new Random();
            int rdScore = rd.Next(1, 5);//生成1~5之间的一个随机数，作为签到获得随机积分

            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists["SignIn"];
            string url = SPContext.Current.Site.Url;
            string[] userName = GetUserInfo(web);
            DateTime LastTime;
            int LastDays = 0;
            int AllDays = 0;
            int AllScore = 0;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                SPListItem listItem = GetListData(web,list,userName[0]);
                if (listItem!=null)//已有签到记录
                {
                    AllScore = int.Parse(listItem["AllScore"].ToString());
                    LastTime = DateTime.Parse(listItem["LastTime"].ToString());
                    LastDays = int.Parse(listItem["LastDays"].ToString());
                    if (LastTime.Date.Equals(DateTime.Now.Date))//当日已签到
                    {
                        return;
                    }
                    else if (LastTime.Date.Equals(DateTime.Now.AddDays(-1)))//昨日有签到
                    {
                        listItem["LastDays"] = LastDays + 1;//连续签到天数+1
                    }
                    else//昨日无签到，漏签，持续签到从第1天重新开始
                    {
                        listItem["LastDays"] = 1;
                    }
                    listItem["LastScore"] = rdScore.ToString();//本次签到随机积分

                    AllDays =int.Parse(listItem["AllDays"].ToString());
                    listItem["AllDays"] = AllDays + 1;//总计签到天数
                    listItem["Degree"] = GetDegreeByDays(AllDays + 1);//签到等级                   
                    listItem["AllScore"] = (AllScore+rdScore).ToString();//签到总积分
                    listItem["LastTime"] = DateTime.Now;//上次签到时间
                    listItem["LastOrder"] = GetRank(web, list, userName[0],0, DateTime.Now);
                    listItem["LastIP"] = GetIP();//本次签到IP地址
                    listItem.Update();
                }
                else//未有任何签到记录，即本次为第一次签到
                {
                    web.AllowUnsafeUpdates = true;
                    listItem = list.Items.Add();//list.AddItem();
                    listItem["Title"] = userName[0];//签到账号
                    listItem["DisplayName"]= userName[1];//签到姓名
                    listItem["Degree"] = "★";//签到等级1
                    listItem["LastScore"] = rdScore.ToString();//本次签到随机积分
                    listItem["AllScore"] = rdScore.ToString();//签到总积分
                    listItem["LastTime"] = DateTime.Now;//上次签到时间
                    listItem["LastOrder"] = GetRank(web, list, userName[0], 0,DateTime.Now);
                    listItem["LastDays"] = "1";//连续签到天数
                    listItem["AllDays"] = "1";//总计签到天数
                    listItem["LastIP"] = GetIP();//本次签到IP地址
                    listItem.Update();
                }
            }
            );
        }
        public int DateDiff(DateTime dt1,DateTime dt2)
        {
            TimeSpan ts = dt2.Subtract(dt1);
            return ts.Days;
        }

        /// <summary>
        /// 获取两种排行：k=0，当日签到排行；k=1,签到总排行，第一次签到前总排行标记为“-”；
        /// </summary>
        /// <param name="web"></param>
        /// <param name="list"></param>
        /// <param name="userName"></param>
        /// <param name="k"></param>
        /// <param name="lastTime"></param>
        /// <returns></returns>
        public int GetRank(SPWeb web, SPList list, string userName,int k,DateTime lastTime)
        {
            int rank = 0;
            SPQuery query = new SPQuery();
            if (k==0)//本次签到排行
            {
                SPTimeZone timeZone = web.RegionalSettings.TimeZone;
                DateTime dt1 = lastTime.Date;
                DateTime dt2= lastTime.Date.AddDays(1);
                lastTime = timeZone.LocalTimeToUTC(lastTime);
                //dt1= timeZone.LocalTimeToUTC(dt1);
                //dt2= timeZone.LocalTimeToUTC(dt2);
                query.Query = "<Where><And><Geq><FieldRef Name='LastTime'/><Value Type='DateTime' IncludeTimeValue='TRUE'>"+dt1+"</Value></Geq><Lt><FieldRef Name='LastTime'/><Value Type='DateTime' IncludeTimeValue='TRUE'>"+dt2+"</Value></Lt></And></Where><OrderBy><FieldRef Name ='LastTime'/></OrderBy>";//查询所有今天的用户签到记录
            }
            else//签到总排行
            {
                query.Query = "<OrderBy><FieldRef Name ='AllScore' Ascending='FALSE'/></OrderBy>";//查询所有今天的用户签到记录
            }
            int i = 0;
            SPListItemCollection listItems = list.GetItems(query);
            if (listItems.Count>0)
            {
                foreach (SPListItem lItem in listItems)
                {                    
                    if (lItem["Title"].ToString() == userName)
                    {
                        break;
                    }
                    i++;
                }                
            }
            if (i != listItems.Count)
            {
                rank =i;
            }            
            return rank;
        }
    }
}
