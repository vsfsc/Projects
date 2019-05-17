using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALibrary
{
    public class FileType
    {
        /// <remarks>类别ID</remarks>
        private int typeId;
        /// <remarks>文件类型名称</remarks>
        private string typeName;
        /// <remarks>父级类别ID</remarks>
        private int parentId;

        /// <summary>
        /// 文件类型ID
        /// </summary>
        /// <remarks>类别ID</remarks>
        public int TypeID
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
            }
        }

        /// <remarks>文件类型名称</remarks>
        public string TypeName
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /// <remarks>父级类别ID</remarks>
        public int ParentID
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /// <summary>
        /// 根据ID查询文件类型
        /// </summary>
        /// <remarks>根据ID获取类型信息</remarks>
        /// <returns>文件类型</returns>
        public static FileType GetTypeById(int typeId)
        {
            throw new NotImplementedException();
        }

        /// <remarks>获取父级作品类别</remarks>
        public static FileType GetParentTypeBYId(int typeId)
        {
            throw new System.NotImplementedException();
        }
    }
}