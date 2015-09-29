using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

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