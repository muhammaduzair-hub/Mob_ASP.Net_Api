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
    
    public partial class zone_detail
    {
        public int zdid { get; set; }
        public Nullable<double> mlatitude { get; set; }
        public Nullable<double> mlongitude { get; set; }
        public Nullable<System.DateTime> mtime { get; set; }
        public string mstatus { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<int> zid { get; set; }
        public Nullable<int> pmobquantity { get; set; }
        public Nullable<int> reachtime { get; set; }
    
        public virtual MOB MOB { get; set; }
        public virtual zone zone { get; set; }
    }
}
