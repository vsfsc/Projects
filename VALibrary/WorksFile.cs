using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALibrary
{
    public class WorksFile
    {
        #region 变量
        /// <summary>
        /// 文件名称
        /// </summary>
        /// <remarks>文件名称</remarks>
        private string _fileName;
        /// <summary>
        /// 文件类型ID
        /// </summary>
        /// <remarks>文件类型ID</remarks>
        private int _fileTypeId;
        /// <summary>
        /// 文件ID
        /// </summary>
        /// <remarks>文件ID</remarks>
        private long _fileId;

        /// <remarks>文件大小</remarks>
        private string _fileSize;
        /// <remarks>文件路径</remarks>
        private string _filePath;


        private FileType _fileType;


        public WorksFile(long fileId,string fileName, int fileTypeId, string fileSize,  string filepath)
        {
            this._fileId = fileId;
            this._fileName = fileName;
            this._fileTypeId = fileTypeId;
            this._fileSize = fileSize;
            this._filePath = filepath;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 文件ID
        /// </summary>
        /// <remarks>文件ID</remarks>
        public long FileId
        {
            get { return _fileId; }
            set { _fileId = value; }
        }

        /// <summary>
        /// 文件类型
        /// </summary>
        public FileType FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        /// <remarks>文件路径</remarks>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        /// <remarks>文件名称</remarks>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public int FileTypeId
        {
            get { return _fileTypeId; }
            set { _fileTypeId = value; }
        }

        /// <remarks>文件大小</remarks>
        public string FileSize
        {
            get { return _fileSize; }
            set { _fileSize = value; }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 查询指定ID的作品文件
        /// </summary>
        /// <remarks>查询指定ID的文件</remarks>
        /// <param name="fileId">文件ID</param>
        /// <returns>作品文件</returns>
        public static WorksFile GetFileByID(long fileId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 查询指定作品ID的所有作品记录
        /// </summary>
        /// <remarks>获取一个作品的所有文件</remarks>
        /// <param name="worksId">作品ID</param>
        /// <returns>作品文件集合</returns>
        public static List<WorksFile> GetFilesByWorksID(long worksId)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}