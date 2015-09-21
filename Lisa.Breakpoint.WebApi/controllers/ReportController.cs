using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("bug-reports")]
    public class ReportController
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        public IList<Report> Get()
        {
            return _db.getAll();
        }

        [HttpGet]
        [Route("{id}")]
        public Report get(int id)
        {
            return _db.get(id);
        }

        [Route("insert")]
        public Report insert(int id)
        {
            Project project = new Project
            {
                Slug = "lisa",
                Name = "Leerbedrijf Lisa"
            };

            Reporter reporter1 = new Reporter
            {
                UserName = "blablaname",
                FullName = "Bas Eenhoorn"
            };

            Reporter reporter2 = new Reporter
            {
                UserName = "otheruser",
                FullName = "Sab Tweehoorn"
            };

            Reporter assignedTo = new Reporter
            {
                UserName = "otheruser",
                FullName = "Sab Tweehoorn"
            };

            Comment comment1 = new Comment
            {
                Posted = new DateTime(2013, 6, 1, 12, 32, 30),
                Author = "Bas eenhoorn",
                Text = "This is a comment with some text.."
            };

            Comment comment2 = new Comment
            {
                Posted = new DateTime(2014, 5, 1, 12, 32, 30),
                Author = "Cas Tweehoorn",
                Text = "This is another comment with more text.."
            };

            Comment comment3 = new Comment
            {
                Posted = new DateTime(2015, 6, 1, 12, 32, 30),
                Author = "Sab driehoorn",
                Text = "This is a comment with less text.. well.. now its more..."
            };

            Report report = new Report
            {   
                Project = new List<Project> { project },
                StepByStep = "step by step",
                Expectation = "it works",
                WhatHappend = "it did not work",
                Reporters = new List<Reporter> { reporter1, reporter2 },
                Reported = "yesterday",
                Status = "not fixed",
                Priority = "High",
                AssignedTo = new List<Reporter> { assignedTo },
                Comments = new List<Comment> { comment1, comment2, comment3 }
            };

            return _db.insert(report);
        }

        [Route("patch/{id}")]
        public Report Patch(int id)
        {
            Report patchedReport = new Report
            {
                Expectation = "it should work again",
                WhatHappend = "it did not work again",
                Reported = "3 days ago",
            };

            return _db.patch(id, patchedReport);
        }

        [Route("delete/{id}")]
        public void delete(int id)
        {
            _db.delete(id);
        }
    }
}