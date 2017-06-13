using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SynergySolution.ModelHelper
{
    public class AddTeamMemberList
    {
        public List<AgileTeamMemberList> agileTeamMemberList { get; set; }
        public List<LineMemberList> lineManagerList { get; set; }
        public List<int> selectedAgileTeamMemberId { get; set; }
        public List<string> selectedAgileTeamMemberName { get; set; }
        public List<int> selectedLineManagerId { get; set; }
        public List<string> selectedLineManagerName { get; set; }
        public int teamId{get;set;}
    }
}