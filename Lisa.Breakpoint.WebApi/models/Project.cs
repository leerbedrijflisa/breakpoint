using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public IList<Member> Member { get; set; }
        public IList<string> Version { get; set; }
    }

    public class Member
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}