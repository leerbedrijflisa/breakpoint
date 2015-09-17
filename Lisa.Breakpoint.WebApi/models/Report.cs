namespace Lisa.Breakpoint.WebApi
{
    public class Report
    {
        public int      Id { get; set; }
        public int      Number { get; set; }
        public string   Project { get; set; }
        public string   StepByStep { get; set; }
        public string   Expectaton { get; set; }
        public string   WhatHappend { get; set; }
        public string   Reporter { get; set; }
        public string   Reported { get; set; }
        public string   Status { get; set; }
        public string   Priority { get; set; }
        public string   AssignedTo { get; set; }
        public string   Comments { get; set; }
    }
}
