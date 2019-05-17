using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorksShowDll.Bll
{
    public class WorksBll
    {
        static VAExtensionEntities db = new VAExtensionEntities();
        #region 方法
        /// <summary>
        /// 获取作品文件
        /// </summary>
        /// <param name="worksID">作品ID</param>
        /// <param name="typeId">typeID=0为所有的</param>
        /// <returns></returns>
        public static List<WorksFile> GetWorksFile(string worksID, string typeId)
        {
            List<WorksFile> files;
            long worksId = long.Parse(worksID);
            int iTypeId = int.Parse(typeId);

            if (typeId == "0")
            {
                files = db.WorksFile.Where(p => p.WorksID == worksId).ToList();
            }
            else
            {
                files = db.WorksFile.Where(p => p.WorksID == worksId && p.Type == iTypeId).ToList();

            }
            return files;
        }

        /// <summary>
        /// 获取作品类别
        /// </summary>
        /// <returns></returns>
        public static List<WorksType> GetWorksTypes()
        {
            List<WorksType> worksTypes = db.WorksType.ToList();
            return worksTypes;
        }

        /// <summary>
        /// 通过ID获取作品信息
        /// </summary>
        /// <param name="worksId">作品ID</param>
        /// <returns></returns>
        public static Works GetWorksSubmitById(string worksId)
        {
            long workId = long.Parse(worksId);
            Works ds = db.Works.SingleOrDefault(p => p.WorksID == workId);
            return ds;
        }

        /// <summary>
        /// 添加作品
        /// </summary>
        /// <param name="worksEntity"></param>
        /// <returns></returns>
        public long Create(Works worksEntity)
        {
            db.Works.Add(worksEntity);
            db.SaveChanges();
            return worksEntity.WorksID;
        }

        /// <summary>
        /// 更新作品
        /// </summary>
        /// <param name="works"></param>
        public static void Update(Works works)
        {
            Works worksEntity = db.Works.SingleOrDefault(w => w.WorksID == works.WorksID);
            if (worksEntity != null) worksEntity.WorksCode = works.WorksCode;
            db.SaveChanges();
        }

        /// <summary>
        /// 删除作品
        /// </summary>
        /// <param name="worksId"></param>
        public static void Delete(string worksId)
        {
            long lWorksId = long.Parse(worksId);
            Works worksEntity = db.Works.SingleOrDefault(w => w.WorksID == lWorksId);
            if (worksEntity != null) worksEntity.Flag = 0;
            db.SaveChanges();
        }

        /// <summary>
        /// 通过作品类别ID获取作品类别
        /// </summary>
        /// <param name="worksTypeId">作品类别ID</param>
        /// <returns></returns>
        public static WorksType GetWorksTypeById(string worksTypeId)
        {
            long lWorksTypeId = long.Parse(worksTypeId);
            WorksType worksTypeEntity = db.WorksType.SingleOrDefault(w => w.WorksTypeID == lWorksTypeId);
            return worksTypeEntity;
        }

        /// <summary>
        /// 获取所有作品信息
        /// </summary>
        /// <returns></returns>
        public static List<Works> GetWorks()
        {
            List<Works> allWorks = db.Works.ToList();
            return allWorks;
        }

        /// <summary>
        /// 通过期次ID获取期次信息
        /// </summary>
        /// <param name="periodsId"></param>
        /// <returns></returns>
        public static List<Periods> GetPeriodsById(string periodsId)
        {
            long pID = long.Parse(periodsId);
            List<Periods> retPeriods;
            if (pID == 0)

                retPeriods = db.Periods.ToList();
            else
                retPeriods = db.Periods.Where(p => p.PeriodID == pID).ToList();

            return retPeriods;
        }

        /// <summary>
        /// 通过课程ID获取课程信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public static List<Course> GetCourseById(string courseId)
        {
            long cID = long.Parse(courseId);
            List<Course> retCourse;
            if (cID == 0)
                retCourse = db.Course.ToList();
            else
                retCourse = db.Course.Where(p => p.CourseID == cID).ToList();
            return retCourse;
        }

        /// <summary>
        /// 获取所有课程列表
        /// </summary>
        /// <returns></returns>
        public static List<Course> GetCourses()
        {
            List<Course> retCourse = db.Course.ToList();
            return retCourse;
        }

        /// <summary>
        /// 获取用户的作品
        /// </summary>
        /// <returns></returns>
        public static List<CSMyWorks> GetMyWorks()
        {
            List<CSMyWorks> myWorks = db.CSMyWorks.ToList();
            return myWorks;
        }

        internal static void AddWorksRead(Works work)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
