using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.Models
{
    public class Project
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Organization  { get; set; }
        public string ProjectManager  { get; set; }
        public IList<string> Version  { get; set; }
        public IList<string> Browsers { get; set; }
        public IList<Group>  Groups   { get; set; }
        public IList<Member> Members  { get; set; }
    }

    public class Group
    {
        public Group()
        {
            Disabled = false;
        }

        public int    Level { get; set; }
        public string Name  { get; set; }
        public bool   Disabled { get; set; }
    }
}