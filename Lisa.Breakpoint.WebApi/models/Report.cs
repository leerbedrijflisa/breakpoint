using System;
using System.Collections.Generic;
using Lisa.Breakpoint.WebApi.Models;

namespace Lisa.Breakpoint.WebApi
{
    public class Report
    {
        public string   Id { get; set; }
        public int      Number { get; set; }
        public Project  Project { get; set; }
        public string   StepByStep { get; set; }
        public string   Expectation { get; set; }
        public string   WhatHappened { get; set; }
        public Reporter Reporter { get; set; }
        public DateTime Reported { get; set; }
        public string   Status { get; set; }
        public string   Priority { get; set; }
        public Reporter AssignedTo { get; set; }
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

    public class Project
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public List<Member> Member { get; internal set; }
        //public int Id { get; internal set; }
    }
}
