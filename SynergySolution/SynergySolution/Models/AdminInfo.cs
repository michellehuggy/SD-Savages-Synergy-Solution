//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SynergySolution.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AdminInfo
    {
        public int adminId { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string phone { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public int loginId { get; set; }
        public string email { get; set; }
    
        public virtual AdminLogin AdminLogin { get; set; }
    }
}