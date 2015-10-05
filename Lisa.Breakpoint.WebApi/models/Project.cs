using Lisa.Breakpoint.WebApi.models;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Organization  { get; set; }
        public IList<Member> Member { get; set; }
    }
}