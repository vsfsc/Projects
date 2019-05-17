using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.IO;
using System.Collections.Specialized;
using System.Web;
using System.Text;
using System.DirectoryServices;

namespace VAWcfService
{
    /// <summary/>
    /// 关于实现用户接口的类
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceUser : IServiceUser
    {
        public void DoWork()
        {
        }
        VAExtensionWorks db = new VAExtensionWorks();

        /// <summary>
        /// 验证域用户名和密码是否正确
        /// </summary>
        /// <param name="loginName">登录帐户名</param>
        /// <param name="passWord">登录密码</param>
        /// <returns></returns>
        public bool ValidateLoginUser(string loginName, string passWord)
        {
            try
            {
                string domainName = ADHelper.Domain;
                string strLDAP = "LDAP://" + domainName;
                using (DirectoryEntry objDE = new DirectoryEntry("", loginName , passWord ))
                {
                    DirectorySearcher deSearcher = new DirectorySearcher(objDE);
                    deSearcher.Filter = "(&(objectClass=user)(sAMAccountName=" + loginName  + "))";
                    DirectoryEntry usr = deSearcher.FindOne().GetDirectoryEntry();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 通过登录名获取用户ID,未登录返回0
        /// </summary>
        /// <param name="loginName">用户登录名</param>
        /// <returns></returns>
        public long GetUserIdByLoginName(string loginName)
        {
            User loginUser = db.User.SingleOrDefault(p=>p.Account ==loginName );
            if (loginUser != null)
            {
                return loginUser.UserID;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取用户作品关系列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="worksId">作品ID</param>
        /// <returns></returns>
        public List<UserWorks> GetUserWorks(string userId,string worksId)
        {
            List<UserWorks> userWorks=db.UserWorks.ToList();
            if (userId!="0")
            {
                long uid = long.Parse(userId);
                userWorks = userWorks.Where(uw => uw.UserID == uid).ToList();
            }
            if (worksId!="0")
            {
                long wid = long.Parse(worksId);
                userWorks = userWorks.Where(uw => uw.WorksID == wid).ToList();
            }
            return userWorks;
        }

        /// <summary>
        /// 获取作品的作者信息
        /// </summary>
        /// <param name="worksId">作品ID</param>
        /// <returns></returns>
        public List<User> GetWorksUserInfo(string worksId)
        {
            long lWorksId=long.Parse (worksId);
            List<UserWorks> userWorks = db.UserWorks.Where(p=>p.WorksID == lWorksId).ToList();
            List<User> users=new List<User>();
            foreach (var uw in userWorks)
            {
                User user = db.User.Find(uw.UserID);
                users.Add(user);
            }
            return users;
        }

        /// <summary>
        /// 获取用户的作品信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<Works> GetUserWorksInfo(string userId)
        {
            long lUserId = long.Parse(userId);
            List<UserWorks> userWorks = db.UserWorks.Where(p => p.UserID == lUserId).ToList();
            return userWorks.Select(uw => db.Works.Find(uw.WorksID)).ToList();
        }

        /// <summary>
        /// 获取用户收藏的作品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public List<FavoriteWorks> GetFavoritesByUserIdandDomainId(string userId, string domainId)
        {
            long uId = long.Parse(userId);
            long dId = long.Parse(domainId);
            List<FavoriteWorks> fWorks = db.FavoriteWorks.Where(fw => fw.UserID == uId & fw.DomainID == dId).ToList();
            return fWorks;
        }
        #region 收藏
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="favorites"></param>
        /// <returns></returns>
        public long AddFav(Favorites favorites)
        {
            db.Favorites.Add(favorites);
            db.SaveChanges();
            return favorites.DomainID;
        }

        /// <summary>
        /// 变更收藏
        /// </summary>
        /// <param name="favorites"></param>
        public void ModifyFav(Favorites favorites)
        {
            Favorites fav = db.Favorites.SingleOrDefault(f => f.UserID == favorites.UserID&f.ItemID==favorites.ItemID&f.DomainID==favorites.DomainID);
            if (fav != null)
            {
                fav.Flag = favorites.Flag;
                fav.ModifyDate = favorites.ModifyDate;
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 获取用户的收藏
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<Favorites> GetUserFavorites(string userId)
        {
            long uid = long.Parse(userId);
            List<Favorites> favs = db.Favorites.Where(f => f.UserID == uid).ToList();
            return favs;
        }
        #endregion
    }
    /// <summary>
    /// AD类
    /// </summary>
    public class ADHelper
    {
        #region 属性
        /// <summary>
        /// 返回域ldapServiceName:ccc.neu.edu.cn:ccc-global$@CCC.NEU.EDU.CN
        /// </summary>
        public static string Domain
        {
            get
            {
                DirectoryEntry root = new DirectoryEntry("LDAP://rootDSE");

                string domain = (string)root.Properties["ldapServiceName"].Value;
                domain = domain.Substring(0, domain.IndexOf("."));
                return domain;

            }
        }

        #endregion
    }

}