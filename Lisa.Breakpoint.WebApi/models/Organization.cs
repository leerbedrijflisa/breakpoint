using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.Models
{
    public class Organization
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public IList<string> Members { get; set; }
        public IList<string> Projects { get; set; }
    }
}