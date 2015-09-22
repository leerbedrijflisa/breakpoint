using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lisa.Breakpoint.WebApi
{
    [Route("reports")]
    public class ReportController
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        public IList<Report> Get()
        {
            return _db.getAllReports();
        }

        [HttpGet]
        [Route("{id}")]
        public Report get(int id)
        {
            return _db.getReport(id);
        }

        [HttpPost]
        [Route("insert")]
        public void insert([FromBody]Report report)
        {
            Debug.WriteLine(report);
            _db.insertReport(report);
        }

        [HttpGet]
        [Route("patch/{id}")]
        public void Patch(int id)
        {
            Report patchedReport = new Report
            {
                Expectation = "it should work again",
                WhatHappened = "it did not work again",
                Reported = "3 days ago",
            };

            _db.patchReport(id, patchedReport);
        }

        [HttpGet]
        [Route("delete/{id}")]
        public void delete(int id)
        {
            _db.deleteReport(id);
        }
    }
}