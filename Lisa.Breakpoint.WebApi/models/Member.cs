using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.Models
{
    public class Member
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

    public class Group
    {
        public string Name { get; set; }
        public IList<Member> Members { get; set; }
    }
}