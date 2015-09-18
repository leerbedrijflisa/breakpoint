using System;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    public class Report
    {
        public string      Id { get; set; }
        public int      Number { get; set; }
        public IEnumerable<Project> Project { get; set; }
        public string   StepByStep { get; set; }
        public string   Expectation { get; set; }
        public string   WhatHappend { get; set; }
        public IEnumerable<UserRef> Reporter { get; set; }
        public string   Reported { get; set; }
        public string   Status { get; set; }
        public string   Priority { get; set; }
        public IEnumerable<UserRef> AssignedTo { get; set; }
        public IEnumerable<Comments> Comments { get; set; }
    }

    public class Comments
    {
        public DateTime Posted { get; set; }
        public IEnumerable<UserRef> Author { get; set; }
        public string Text { get; set; }
    }

    public class UserRef
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
