using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace VAWcfService
{
    // 实现作品文件接口的类
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceWorksFile : IServiceWorksFile
    {
        public void DoWork()
        {
        }
        VAExtensionWorks db = new VAExtensionWorks();

        #region 方法
        /// <summary>
        /// 通过ID获取作品文件
        /// </summary>
        /// <param name="fileID">文件ID</param>
        /// <returns></returns>
        public WorksFile GetWorksFileByID(string fileID)
        {
            long lfileID=long.Parse (fileID );
            WorksFile file = db.WorksFile.FirstOrDefault(w => w.WorksFileID == lfileID);
            return file;
        }
        /// <summary>
        /// 通过ID获取文件类别
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public FileType GetFileTypeByID(string typeID)
        {
            long ltypeID = long.Parse(typeID);
            FileType type = db.FileType.FirstOrDefault(w => w.FileTypeID == ltypeID );
            return type;
        }
        /// <summary>
        /// 添加作品文件
        /// </summary>
        /// <param name="worksFile"></param>
        /// <returns></returns>
        public long  Create(WorksFile worksFile)
        {
            db.WorksFile.Add (worksFile);
            db.SaveChanges();
            long fileID=worksFile.WorksFileID;
            return fileID  ;
        }
        /// <summary>
        /// 更新作品文件
        /// </summary>
        /// <param name="worksFile"></param>
        public void Update(WorksFile worksFile)
        {
           WorksFile file1=db.WorksFile.FirstOrDefault (w=>w.WorksFileID ==worksFile.WorksFileID);
            file1.FileName=worksFile.FileName;
            file1.FilePath =worksFile.FilePath;
            file1.FileSize =worksFile.FileSize;
            db.SaveChanges ();

        }
        /// <summary>
        /// 删除作品文件
        /// </summary>
        /// <param name="worksFileID"></param>
        public void Delete(string worksFileID)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
