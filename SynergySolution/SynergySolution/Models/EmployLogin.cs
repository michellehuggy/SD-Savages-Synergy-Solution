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
    
    public partial class EmployLogin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployLogin()
        {
            this.EmployInfoes = new HashSet<EmployInfo>();
        }
    
        public int loginId { get; set; }
        public string email { get; set; }
        public string pasword { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployInfo> EmployInfoes { get; set; }
    }
}
