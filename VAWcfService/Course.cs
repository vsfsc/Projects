//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace VAWcfService
{
    using System;
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    
    [Serializable]
    [DataContract]
    [KnownType(typeof(Periods))]
    
    public partial class Course
    {
        public Course()
        {
            this.Periods = new HashSet<Periods>();
        }
    
    	[DataMember]
        public long CourseID { get; set; }
    	[DataMember]
        public string CourseName { get; set; }
    	[DataMember]
        public Nullable<System.Guid> Guid { get; set; }
    	[DataMember]
        public Nullable<long> SubjiectID { get; set; }
    	[DataMember]
        public string Url { get; set; }
    	[DataMember]
        public Nullable<int> Flag { get; set; }
    
    	[DataMember]
        public virtual ICollection<Periods> Periods { get; set; }
    }
}