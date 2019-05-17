using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VALibrary
{
    public class WorksType
    {
        /// <summary>
        /// 作品类型ID
        /// </summary>
        private int typeId;
        private string typeName;
        /// <remarks>父级作品类型ID</remarks>
        private int parentId;

        /// <summary>
        /// 作品类型ID
        /// </summary>
        public int TypeID
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

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

        /// <remarks>父级作品类型ID</remarks>
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
        /// 获取作品类别
        /// </summary>
        /// <remarks>根据类别ID查询作品类别信息</remarks>
        /// <returns>作品类别信息</returns>
        public static WorksType GetTypeById(long typeId)
        {
            throw new System.NotImplementedException();
        }
    }
}