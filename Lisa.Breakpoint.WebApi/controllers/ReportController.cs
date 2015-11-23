using Lisa.Breakpoint.WebApi.database;
using Microsoft.AspNet.Mvc;
using Lisa.Breakpoint.WebApi.Models;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("reports")]
    public class ReportController : Controller
    {
        public ReportController(RavenDB db)
        {
            _db = db;
        }

        [HttpGet("{organization}/{project}/{username}/{filter?}/{value?}")]
        public IActionResult Get(string organization, string project, string userName, string filter = "", string value = "")
        {
            if (_db.GetProject(organization, project, userName) == null)
            {
                return new HttpNotFoundResult();
            }

            if (_db.GetUser(userName) == null)
            {
                return new HttpNotFoundResult();
            }

            IList<Report> reports;
            if (filter != "")
            {
                Filter f = new Filter(filter, value);

                reports = _db.GetAllReports(organization, project, userName, f);
            } else { 
                reports = _db.GetAllReports(organization, project, userName);
            }

            if (reports == null)
            {
                return new HttpNotFoundResult();
            }
            return new HttpOkObjectResult(reports);
        }

        [HttpGet("{id}", Name = "report")]
        public IActionResult Get(int id)
        {
            Report report = _db.GetReport(id);

            if (report == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(report);
        }

        [HttpPost("{organization}/{project}")]
        public IActionResult Post([FromBody] Report report, string organization, string project)
        {

            if (report == null)
            {
                return new BadRequestResult();
            }

            if (report.Platform == "" || report.Platform == null)
            {
                report.Platform = "not specified";
            }

            _db.PostReport(report);

            string location = Url.RouteUrl("report", new { id = report.Number }, Request.Scheme);
            return new CreatedResult(location, report);
        }

        [HttpPatch("{id}/{userName}")]
        public IActionResult Patch(int id, string userName, [FromBody] Report report)
        {

            if (!statusCheck.Contains(report.Status))
            {
                return new BadRequestResult();
            }

            Report checkReport = _db.GetReport(id);

            Project checkProject = _db.GetProject(checkReport.Organization, checkReport.Project, userName);
            
            //If the status is Won't fix (approved) than it will check if the user is a manager, if that is not the case then return badrequestresult.
            if (report.Status == statusCheck[3])
            {
                foreach (var members in checkProject.Members)
                {
                    if (members.UserName == userName && members.Role != "manager")
                    {
                        return new BadRequestResult();
                    }
                    if (members.UserName == userName && members.Role == "manager")
                    {
                        break;
                    }
                }
            }
            if (report.Status == statusCheck[4])
            {
                foreach (var members in checkProject.Members)
                {
                    if (members.UserName == userName && members.Role == "developer" && report.Reporter == userName)
                    {
                        break;
                    }
                    else if (members.UserName == userName && members.Role == "manager" || members.Role == "tester")
                    {
                        break;
                    }
                    else if (members.UserName != userName && members.Role == "developer")
                    {
                        return new BadRequestResult();
                    }
                }
            }

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

        private readonly IList<string> statusCheck = new string[] { "Open", "Fixed", "Won't Fix", "Won't Fix (Approved)", "Closed" };

    }
}