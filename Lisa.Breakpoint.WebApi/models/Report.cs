using System;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.Models
{
    public class Report
    {
        public string   Title { get; set; }
        public string   Number { get; set; }
        public string   Project { get; set; }
        public string   StepByStep { get; set; }
        public string   Expectation { get; set; }
        public string   WhatHappened { get; set; }
        public string   Reporter { get; set; }
        public DateTime Reported { get; set; }
        public string   Status { get; set; }
        public string   Priority { get; set; }
        public string   Version { get; set; }
        public AssignedTo AssignedTo { get; set; }
        public IList<Comment> Comments { get; set; }
        public IList<string> Browsers { get; set; }

    }

    // type = person || group; value = username || groupname;
    public class AssignedTo
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class Comment
    {
        public DateTime Posted { get; set; }
        public string   Author { get; set; }
        public string   Text { get; set; }
    }
}