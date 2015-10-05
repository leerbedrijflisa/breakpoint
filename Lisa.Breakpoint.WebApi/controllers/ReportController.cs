using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("reports")]
    public class ReportController : Controller
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        [Route("{project}/{username}")]
        public IList<Report> Get(string project, string userName)
        {
            string group = _db.GetGroupFromUser(userName);
            return _db.GetAllReports(project, userName, group);
        }

        [HttpGet]
        [Route("get/{id}")]
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

            string location = "report/{id}";
            //string location = Url.RouteUrl("report", new { id = report.Number }, Request.Scheme);
            return new CreatedResult(location, report);
        }

        [HttpPost]
        [Route("patch/{id}")]
        public void Patch(int id, [FromBody] Report report)
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