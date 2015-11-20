using System;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.Models
{
    public class Report
    {
        public string   Title { get; set; }
        public string   Number { get; set; }
        public string   Project { get; set; }
        public string   Organization { get; set; }
        public string   StepByStep { get; set; }
        public string   Expectation { get; set; }
        public string   WhatHappened { get; set; }
        public string   Reporter { get; set; }
        public DateTime Reported { get; set; }
        public string   Status { get; set; }
        public Priority Priority { get; set; }
        public string   PriorityString { get; set; }
        public string   Version { get; set; }
        public AssignedTo AssignedTo { get; set; }
        public IList<Comment> Comments { get; set; }
        public IList<string> Platforms { get; set; }
    }

    public enum Priority
    {
        FixImmediately,
        FixBeforeRelease,
        FixForNextRelease,
        FixWhenever
    }

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

    public class Filter
    {
        public Filter (string type, string value)
        {
            Type = type;
            Value = value;
        }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}