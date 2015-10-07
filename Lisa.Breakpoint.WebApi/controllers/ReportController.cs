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

            //foreach (var version in project.Version)
            //{
            //    Debug.WriteLine("versions -> " + version);
            //    if (version == report.Version)
            //    {
            //        report.Version = version;
            //        Debug.WriteLine("bestaat al");
            //    }
            //    else
            //    {
            //        Debug.WriteLine("bestaat nog niet");
            //    }
            //}

            // TODO: finish patch project function

            if (!project.Version.Contains(report.Version))
            {
                Debug.WriteLine("bestaat nog niet");
                //Project patchedProject = new Project();
                //patchedProject.Add(report.Version);

                Project patchedProject = project;

                patchedProject.Version.Add(report.Version);

                foreach (var version in patchedProject.Version)
                {
                    Debug.WriteLine(version);
                }
                //_db.PatchProject(project.Id, patchedProject);

            }


            //_db.PatchProject(project.Id, patchedProject);


            //debug.WriteLine("Version that is going in the database " + report.Version);
            _db.PostReport(report);


            string location = Url.RouteUrl("report", new { id = report.Number }, Request.Scheme);
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