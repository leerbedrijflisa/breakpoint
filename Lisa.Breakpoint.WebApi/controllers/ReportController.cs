using Lisa.Breakpoint.WebApi.database;
using Microsoft.AspNet.Mvc;
using System.Diagnostics;
using Lisa.Breakpoint.WebApi.Models;

namespace Lisa.Breakpoint.WebApi
{
    [Route("reports")]
    public class ReportController : Controller
    {
        private readonly RavenDB _db;

        public ReportController(RavenDB db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("{project}/{username}")]
        public IActionResult Get(string project, string userName)
        {
            string group = _db.GetGroupFromUser(userName);
            var reports  = _db.GetAllReports(project, userName, group);

            if (reports == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(reports);
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult Get(string id)
        {
            var report = _db.GetReport("reports/"+id);

            if (report == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(report);
        }

        [HttpPost]
        [Route("", Name = "report")]
        public IActionResult Post([FromBody] Report report)
        {
            if (report == null)
            {
                return new BadRequestResult();
            }

            //if (_db.GetProject(project, "") == null)
            //{
            //    return new HttpNotFoundResult();
            //}

            Debug.WriteLine(report.Version);

            //Project project = _db.GetProject(report.Project);
            //if (!project.Version.Contains(report.Version))
            //{
            //    Project patchedProject = project;
            //    patchedProject.Version.Add(report.Version);
            //    //TODO: finish patch project function
            //    _db.PatchProject(project.Id, patchedProject);
            //}

            _db.PostReport(report);

            string location = Url.RouteUrl("report", new {  }, Request.Scheme);
            return new CreatedResult(location, report);
        }

        [HttpPost]
        [Route("patch/{id}")]
        public IActionResult Patch(int id, [FromBody]Report report)
        {
            Report patchedReport = _db.PatchReport(id, report);

            return new HttpOkObjectResult(patchedReport);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            _db.DeleteReport(id);

            return new HttpOkResult();
        }
    }
}