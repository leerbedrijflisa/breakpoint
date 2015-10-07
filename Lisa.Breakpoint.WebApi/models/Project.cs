using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.models
{
    public class Project
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Organization  { get; set; }
        public IList<Member> Members { get; set; }
    }
}