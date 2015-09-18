using System;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    public class Report
    {
        public string   Id { get; set; }
        public int      Number { get; set; }
        public IList<Project> Project { get; set; }
        public string   StepByStep { get; set; }
        public string   Expectation { get; set; }
        public string   WhatHappend { get; set; }
        public IList<Reporter> Reporters { get; set; }
        public string   Reported { get; set; }
        public string   Status { get; set; }
        public string   Priority { get; set; }
        public IList<Reporter> AssignedTo { get; set; }
        public IList<Comment> Comments { get; set; }
    }

    public class Comment
    {
        public DateTime Posted { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
    }

    public class Reporter
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

    public class Project
    {
        public string Slug { get; set; }
        public string Name { get; set; }
    }
}
