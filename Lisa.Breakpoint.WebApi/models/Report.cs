namespace Lisa.Breakpoint.WebApi
{
    public class Report
    {
        public int      Id { get; set; }
        public int      Number { get; set; }
        public string   Project { get; set; }
        public string   StepByStep { get; set; }
        public string   Expectation { get; set; }
        public string   WhatHappend { get; set; }
        public string   Reporter { get; set; }
        public string   Reported { get; set; }
        public string   Status { get; set; }
        public string   Priority { get; set; }
        public string   AssignedTo { get; set; }
        public string   Comments { get; set; }
    }

    public class Project
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Members { get; set; }
    }
}
