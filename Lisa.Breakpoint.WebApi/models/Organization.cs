using Lisa.Breakpoint.WebApi.Models;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.models
{
    public class Organization
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public IList<Member> Members { get; set; }
        public IList<Project> Projects { get; set; }
    }
}
