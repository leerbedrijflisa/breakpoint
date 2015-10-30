using Lisa.Breakpoint.WebApi.database;
<<<<<<< HEAD
using Microsoft.AspNet.Mvc;
using System.Diagnostics;
=======
>>>>>>> develop
using Lisa.Breakpoint.WebApi.Models;
using Microsoft.AspNet.Mvc;

namespace Lisa.Breakpoint.WebApi
{
    [Route("reports")]
    public class ReportController : Controller
    {
        public ReportController(RavenDB db)
        {
            _db = db;
        }

        [HttpGet("{project}/{username}")]
        public IActionResult Get(string project, string userName)
        {
            if (_db.GetProject(project, userName) == null)
            {
                return new HttpNotFoundResult();
            }

            if (_db.GetUser(userName) == null)
            {
                return new HttpNotFoundResult();
            }

            var reports  = _db.GetAllReports(project, userName);

            if (reports == null)
            {
                return new HttpNotFoundResult();
            }
            return new HttpOkObjectResult(reports);
        }

        [HttpGet("{id}", Name = "report")]
        public IActionResult Get(int id)
        {
            var report = _db.GetReport(id);

            if (report == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(report);
        }

        [HttpPost("{project}")]
        public IActionResult Post([FromBody] Report report, string project)
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

            string location = Url.RouteUrl("report", new { id = report.Number }, Request.Scheme);
            return new CreatedResult(location, report);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] Report report)
        {
            Report patchedReport = _db.PatchReport(id, report);

            return new HttpOkObjectResult(patchedReport);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_db.GetReport(id) == null)
            {
                return new HttpNotFoundResult();
            }

            _db.DeleteReport(id);

            return new HttpStatusCodeResult(204);
        }

        private readonly RavenDB _db;
    }
}