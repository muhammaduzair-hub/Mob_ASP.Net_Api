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

    public partial class Ariel_shots
    {
        public int pid { get; set; }
        public string Pname { get; set; }
        public string Paddress { get; set; }
        public Nullable<double> Platitude { get; set; }
        public Nullable<double> Plongitude { get; set; }
        public Nullable<System.DateTime> Ptime { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<int> pmobquantity { get; set; }
        public Nullable<int> mpic_no { get; set; }
        public string pspeed { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual MOB MOB { get; set; }
    }
}
