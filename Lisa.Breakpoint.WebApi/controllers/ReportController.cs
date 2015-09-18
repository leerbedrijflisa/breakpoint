using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("reports")]
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

        [HttpGet]
        [Route("insert")]
        public Report insert(int id)
        {
            Report report = new Report
            {
                Project = "projectnaam",
                StepByStep = "step by step",
                Expectation = "it works",
                WhatHappend = "it did not work",
                Reporter = "bas",
                Reported = "yesterday",
                Status = "not fixed",
                Priority = "High",
                AssignedTo = "bas2",
                Comments = "-"
            };

            return _db.insert(report);
        }

        [HttpGet]
        [Route("update/{id}")]
        public Report update(int id)
        {
            Report updatedReport = new Report
            {
                Expectation = "it should work again",
                WhatHappend = "it did not work again",
                Reported = "3 days ago",
            };

            return _db.update(id, updatedReport);
        }

        [HttpGet]
        [Route("delete/{id}")]
        public void delete(int id)
        {
            _db.delete(id);
        }
    }
}