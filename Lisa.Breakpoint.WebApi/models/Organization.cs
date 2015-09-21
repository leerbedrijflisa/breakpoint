using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.models
{
    public class Organization
    {
        public string Id { get; set; }
        public string Slug { get; set; }
        public IList<Members> Members{ get; set; }
        public IList<Projects> Projects { get; set; }
    }

    public class Members
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

    public class Projects
    {
        public string Slug { get; set; }
        public string Name { get; set; }
    }
}
