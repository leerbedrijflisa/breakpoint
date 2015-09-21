using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.models
{
    public class Organization
    {
        public string Id { get; set; }
        public string Slug { get; set; }
        public IList<Member> Member { get; set; }
        public IList<Project> Project { get; set; }
    }

    public class Member
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
