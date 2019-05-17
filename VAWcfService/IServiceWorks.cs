using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace VAWcfService
{
    //作品接口
    [ServiceContract]
    public interface IServiceWorks
    {
        [OperationContract]
        void DoWork();

        /// <summary>
        /// 获取作品文件
        /// </summary>
        /// <param name="worksId">作品ID</param>
        /// <param name="typeId">类别ID</param>
        /// <returns></returns>
        [OperationContract]//方法名标识
        [WebGet(UriTemplate = "GetWorksFile/{worksId}/{typeId}",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped
           )]
        List<WorksFile> GetWorksFile(string worksId, string typeId);

        /// <summary>
        /// 获取作品列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]//方法名标识
        [WebGet(UriTemplate = "GetWorks",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped
           )]
        List<Works> GetWorks();

        /// <summary>
        /// 获取作品类别
        /// </summary>
        /// <returns></returns>
        [OperationContract]//方法名标识
        [WebGet(UriTemplate = "GetWorksTypes",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped
           )]
        List<WorksType> GetWorksTypes();

        /// <summary>
        /// 根据作品ID获取作品信息
        /// </summary>
        /// <param name="worksId">作品ID</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetWorksSubmitByID/{worksID}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]
        Works GetWorksSubmitById(string worksId);

        /// <summary>
        /// 通过类别ID获取作品类别
        /// </summary>
        /// <param name="worksTypeId">作品类别ID</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetWorksTypeById/{worksTypeId}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]
        WorksType  GetWorksTypeById(string worksTypeId);

        /// <summary>
        /// 添加新的作品
        /// </summary>
        /// <param name="works">作品实体</param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/", Method = "POST",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Wrapped)]
        long Create(Works works);

        /// <summary>
        /// 更新作品信息
        /// </summary>
        /// <param name="works">作品实体</param>
        [WebInvoke(UriTemplate = "/", Method = "PUT")]
        void Update(Works works);

        /// <summary>
        /// 删除作品
        /// </summary>
        /// <param name="worksId">作品ID</param>
        [WebInvoke(UriTemplate = "{worksId}", Method = "DELETE")]
        void Delete(string worksId);

        /// <summary>
        /// 获取所有课程列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetCourses",
            BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]
        List<Course> GetCourses();

        /// <summary>
        /// 获取我的作品
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetMyWorks",
            BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]
        List<CSMyWorks> GetMyWorks();

        /// <summary>
        /// 通过期次ID获取期次信息
        /// </summary>
        /// <param name="periodsId"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetPeriodsById/{periodsId}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]
        List<Periods>  GetPeriodsById(string periodsId);

        /// <summary>
        /// 通过课程ID获取课程信息
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetCourseById/{courseId}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]
        List<Course>  GetCourseById(string courseId);
    }
}
