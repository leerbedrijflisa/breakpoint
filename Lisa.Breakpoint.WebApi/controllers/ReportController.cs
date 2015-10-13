using Lisa.Breakpoint.WebApi.database;
using Microsoft.AspNet.Mvc;
using Lisa.Breakpoint.WebApi.Models;

namespace Lisa.Breakpoint.WebApi
{
    [Route("reports")]
    public class ReportController : Controller
    {
        public ReportController(RavenDB db)
        {
            _db = db;
        }

        // TODO: add 404 if project doesn't exist
        // TODO: add 404 if username doesn't exist
        [HttpGet("{project}/{username}")]
        public IActionResult Get(string project, string userName)
        {
            // TODO: put these two queries in one function
            string group = _db.GetGroupFromUser(userName);
            var reports  = _db.GetAllReports(project, userName, group);

            if (reports == null)
            {
                return new HttpNotFoundResult();
            }
            return new HttpOkObjectResult(reports);
        }

        [HttpGet("{id}", Name = "report")]
        public IActionResult Get(string id)
        {
            var report = _db.GetReport(id);

            if (report == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(report);
        }

        // TODO: project should be specified in URL
        // TODO: add 404 if project doesn't exist
        [HttpPost("")]
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

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] Report report)
        {
            Report patchedReport = _db.PatchReport(id, report);

            return new HttpOkObjectResult(patchedReport);
        }

        // TODO: add 404 if report doesn't exist
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _db.DeleteReport(id);

            return new HttpStatusCodeResult(204);
        }

        private readonly RavenDB _db;
    }
}