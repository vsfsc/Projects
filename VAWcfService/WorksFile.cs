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
    [KnownType(typeof(Works))]
    
    public partial class WorksFile
    {
    	[DataMember]
        public long WorksFileID { get; set; }
    	[DataMember]
        public long WorksID { get; set; }
    	[DataMember]
        public Nullable<int> Type { get; set; }
    	[DataMember]
        public string FileName { get; set; }
    	[DataMember]
        public string FilePath { get; set; }
    	[DataMember]
        public Nullable<int> FileSize { get; set; }
    	[DataMember]
        public Nullable<long> CreatedBy { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> Created { get; set; }
    	[DataMember]
        public Nullable<long> ModifiedBy { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> Modified { get; set; }
    	[DataMember]
        public Nullable<long> Flag { get; set; }
    
    	[DataMember]
        public virtual Works Works { get; set; }
    }
}
