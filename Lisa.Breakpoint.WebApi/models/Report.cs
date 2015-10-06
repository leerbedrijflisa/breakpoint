using System;
using System.Collections.Generic;
using Lisa.Breakpoint.WebApi.models;

namespace Lisa.Breakpoint.WebApi.models
{
    public class Report
    {
        public string   Title { get; set; }
        public string   Id { get; set; }
        public int      Number { get; set; }
        public string   Project { get; set; }
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
        public List<Member> Members { get; set; }
    }
}
