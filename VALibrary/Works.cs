using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace VALibrary
{
    public class Works
    {
        #region 变量

        

        /// <summary>
        /// 作品ID
        /// </summary>
        /// <remarks>作品ID</remarks>
        private long worksId;
        /// <summary>
        /// 作品名称
        /// </summary>
        private string worksName;
        /// <summary>
        /// 作品描述
        /// </summary>
        /// <remarks>作品说明</remarks>
        private string worksDesc;
        /// <summary>
        /// 作品类别ID
        /// </summary>
        private int worksTypeId;
        /// <remarks>作品编号</remarks>
        private string worksCode;
        #endregion

        #region 事件        
        /// <summary>
        /// 作品状态改变
        /// </summary>
        /// <remarks>作品状态改变事件，包含作品的删除，修改</remarks>
        public static event EventHandler StatusChanged;
        /// <summary>
        /// 新增作品
        /// </summary>
        /// <remarks>添加新的作品事件</remarks>
        public event EventHandler NewWorks;

        #endregion

        #region 属性
        /// <remarks>作品ID</remarks>
        public long WorksID
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public WorksType WorksType
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public WorksFile WorksFile
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /// <remarks>作品名称</remarks>
        public string WorksName
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /// <remarks>作品说明</remarks>
        public string WorksDesc
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /// <remarks>作品编号</remarks>
        public string WorksCode
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /// <remarks>作品类别ID</remarks>
        public int WorksTypeID
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
        #endregion
                
        #region 方法
        
        /// <summary>
        /// 新增作品
        /// </summary>
        /// <remarks>创建一个新的作品</remarks>
        /// <param name="works">要添加的作品记录</param>
        /// <returns>作品ID</returns>
        public static long AddWorks(Works works)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 删除作品
        /// </summary>
        /// <remarks>删除一个作品（逻辑删除，将作品标记为不再显示Flag=0）</remarks>
        /// <param name="works">要删除的作品记录</param>
        /// <returns>被删除作品的ID</returns>
        public static long DeleteWorks(Works works)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 更新作品
        /// </summary>
        /// <remarks>更新一个作品的信息，返回被更新作品的作品ID</remarks>
        /// <param name="works">要更新的作品记录</param>
        /// <returns>作品ID</returns>
        public static long UpdateWorks(Works works)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 查询作品
        /// </summary>
        /// <remarks>根据作品ID查询一个具体的作品信息</remarks>
        /// <returns>作品记录信息</returns>
        public static List<Works> GetWorks()
        {
            throw new System.NotImplementedException();
        }

        /// <remarks>获取指定ID的作品</remarks>
        /// <returns>作品</returns>
        public static Works GetWorksByID(long worksId)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}