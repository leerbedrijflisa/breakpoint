using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using Lisa.Breakpoint.WebApi.Models;
using Lisa.Breakpoint.WebApi;
using System;
using System.Linq;
using Microsoft.Framework.DependencyInjection.Extensions;

namespace Lisa.Breakpoint.WebApi
{
    [Route("reports")]
    public class ReportController : Controller
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        public IList<Report> Get()
        {
            return _db.GetAllReports();
        }

        [HttpGet]
        [Route("{id}", Name = "report")]
        public Report Get(int id)
        {
            return _db.GetReport(id);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Report report)
        {
            if (report == null)
            {
                return new BadRequestResult();
            }

            Debug.WriteLine(report.Version);

            var project = _db.GetProject(report.Project.Id);

            if (!project.Version.Contains(report.Version))
            {
                Debug.WriteLine("bestaat nog niet");

                Project patchedProject = project;

                patchedProject.Version.Add(report.Version);

                foreach (var version in patchedProject.Version)
                {
                    Debug.WriteLine(version);
                }

                // TODO: finish patch project function
                //_db.PatchProject(project.Id, patchedProject);

            }

            _db.PostReport(report);

            string location = Url.RouteUrl("report", new { id = report.Id }, Request.Scheme);
            return new CreatedResult(location, report);
        }

        [HttpPost]
        [Route("patch/{id}")]
        public void Patch(int id)
        {
            Report patchedReport = new Report
            {
                Expectation = "it should work again",
            };

            _db.PatchReport(id, patchedReport);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public void Delete(int id)
        {
            _db.DeleteReport(id);
        }
    }
}