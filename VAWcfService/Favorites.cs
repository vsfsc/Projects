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
    
    public partial class Favorites
    {
    	[DataMember]
        public long UserID { get; set; }
    	[DataMember]
        public long ItemID { get; set; }
    	[DataMember]
        public long DomainID { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> FavDate { get; set; }
    	[DataMember]
        public Nullable<int> Flag { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> ModifyDate { get; set; }
    }
}
