using System;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.Models
{
    public class Report
    {
        public string   Id { get; set; }
        public string   Title { get; set; }
        public string   Number { get; set; }
        public string   ProjectName { get; set; }

        //public ProjectReference Project { get; set; }
        public Project Project { get; set; }

        public string   StepByStep { get; set; }
        public string   Expectation { get; set; }
        public string   WhatHappened { get; set; }
        public string   Reporter { get; set; }
        public DateTime Reported { get; set; }
        public string   Status { get; set; }
        public string   Priority { get; set; }
        public string   AssignedTo { get; set; }
        public string   AssignedToPerson { get; set; }
        public string   AssignedToGroup { get; set; }
        public IList<Comment>   Comments { get; set; }
        public string Version { get; set; }
    }

    public class Comment
    {
        public DateTime Posted { get; set; }
        public string   Author { get; set; }
        public string   Text { get; set; }
    }

    public class Reporter
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

    public class Group
    {
        public string Name { get; set; }
        public IList<string> Members { get; set; }
    }

    public class ProjectReference
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
    }

}