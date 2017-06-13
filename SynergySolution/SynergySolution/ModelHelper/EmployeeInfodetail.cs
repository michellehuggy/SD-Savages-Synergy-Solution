using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SynergySolution.ModelHelper
{
    public class EmployeeInfodetail
    {
        public int employId { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string phone { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public int loginId { get; set; }
        public string email { get; set; }
        public string pasword { get; set; }
        public int designationId { get; set; }
       public string designation { get; set; }
    }
}