using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace VAWcfService
{
    //用户服务接口类
    [ServiceContract]
    public interface IServiceUser
    {
        [OperationContract]
        void DoWork();

        /// <summary>
        /// 验证登录用户的用户名和密码是否正确
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        [OperationContract]//方法名标识
        [WebGet(UriTemplate = "ValidateLoginUser/{loginName}/{passWord}",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped
           )]
       bool ValidateLoginUser(string loginName, string passWord);

        /// <summary>
        /// 根据用户名获取用户ID
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        [OperationContract]//方法名标识
        [WebGet(UriTemplate = "GetUserIdByLoginName/{loginName}",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped
           )]
        long GetUserIdByLoginName(string loginName);

        [OperationContract]//方法名标识
        [WebGet(UriTemplate = "GetUserWorks/{userID}/{worksID}",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped
           )]
        //获取用户的作品
        List<UserWorks> GetUserWorks(string userId, string worksId);


        /// <summary>
        ///  获取用户的作品信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>一个用户所有作品列表</returns>
        [OperationContract]//方法名标识
        [WebGet(UriTemplate = "GetUserWorksInfo/{userId}",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped
           )]
        List<Works> GetUserWorksInfo(string userId);

        /// <summary>
        ///  获取作品的作者信息
        /// </summary>
        /// <param name="worksId">作品ID</param>
        /// <returns>一个作品所有作者列表</returns>
        [OperationContract]//方法名标识
        [WebGet(UriTemplate = "GetWorksUserInfo/{worksId}",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped
           )]
        List<User> GetWorksUserInfo(string worksId);

        /// <summary>
        /// 获取用户收藏的作品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="domainId"></param>
        /// <returns></returns>
        [OperationContract] //方法名标识
        [WebGet(UriTemplate = "GetFavoritesByUserIdandDomainId/{userId}/{domainId}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        List<FavoriteWorks> GetFavoritesByUserIdandDomainId(string userId, string domainId);

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="favorites"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/", Method = "POST",
            //RequestFormat = WebMessageFormat.Json,
            //ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        long AddFav(Favorites favorites);

        /// <summary>
        /// 变更收藏
        /// </summary>
        /// <param name="favorites"></param>
        [WebInvoke(UriTemplate = "/", Method = "PUT")]
        void ModifyFav(Favorites favorites);

        /// <summary>
        /// 获取用户收藏
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [OperationContract] //方法名标识
        [WebGet(UriTemplate = "GetUserFavorites/{userId}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped
            )]
        List<Favorites> GetUserFavorites(string userId);

    }
}
