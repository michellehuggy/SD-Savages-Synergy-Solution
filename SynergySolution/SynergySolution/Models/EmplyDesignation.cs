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
    
    public partial class EmplyDesignation
    {
        public int id { get; set; }
        public int employId { get; set; }
        public int designationId { get; set; }
        public Nullable<bool> isPartOfTeam { get; set; }
    
        public virtual Designation Designation { get; set; }
        public virtual EmployInfo EmployInfo { get; set; }
    }
}