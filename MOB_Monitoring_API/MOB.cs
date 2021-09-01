//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MOB_Monitoring_API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    public partial class MOB
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MOB()
        {
            this.Ariel_shots = new HashSet<Ariel_shots>();
            this.zone_detail = new HashSet<zone_detail>();
        }
    
        public int mid { get; set; }
        public string mname { get; set; }
        public Nullable<System.DateTime> ms_time { get; set; }
        public Nullable<System.DateTime> me_time { get; set; }
        public string mdesc { get; set; }
        public Nullable<int> mflag { get; set; }
        public Nullable<int> mdevice { get; set; }
        public Nullable<int> threshhold { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Ariel_shots> Ariel_shots { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Device Device { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<zone_detail> zone_detail { get; set; }
    }
}