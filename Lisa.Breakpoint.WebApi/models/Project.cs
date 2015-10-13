using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Organization  { get; set; }
        public IList<string> Members { get; set; }
        public IList<string> Browsers { get; set; }
        public IList<string> Version { get; set; }
    }
}