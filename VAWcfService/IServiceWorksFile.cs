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
    // 作品文件接口
    [ServiceContract]
    public interface IServiceWorksFile
    {
        [OperationContract]
        void DoWork();
        [OperationContract]
        [WebGet(UriTemplate = "GetWorksFileByID/{fileID}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]
        //通过作品文件ID获取作品文件
        WorksFile GetWorksFileByID(string fileID);

        [OperationContract]
        [WebGet(UriTemplate = "GetFileTypeByID/{typeID}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
          ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json)]
        //通过文件类型ID获取文件类型
        FileType  GetFileTypeByID(string typeID);
        long  Create(WorksFile  worksFile);
        [WebInvoke(UriTemplate = "/", Method = "PUT")]
        //更新作品文件
        void Update(WorksFile  worksFile);

        [WebInvoke(UriTemplate = "{worksFileID}", Method = "DELETE")]
        //删除作品文件
        void Delete(string worksFileID);
    }
}
