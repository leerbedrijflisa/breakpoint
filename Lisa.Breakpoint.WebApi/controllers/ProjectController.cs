using Lisa.Breakpoint.WebApi.database;
using Lisa.Breakpoint.WebApi.models;
using Lisa.Breakpoint.WebApi.Models;
using Microsoft.AspNet.Mvc;

namespace Lisa.Breakpoint.WebApi
{
    [Route("projects")]
    public class ProjectController : Controller
    {
        public ProjectController(RavenDB db)
        {
            _db = db;
        }

        [HttpGet("{organization}/{username}")]
        public IActionResult GetAll(string organization, string userName)
        {
            if (_db.GetOrganization(organization) == null)
            {
                return new HttpNotFoundResult();
            }

            if (_db.GetUser(userName) == null)
            {
                return new HttpNotFoundResult();
            }

            var organizations = _db.GetAllProjects(organization, userName);
            if (organizations == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(organizations);
        }

        [HttpGet("get/{project}/{userName}", Name = "project")]
        public IActionResult Get(string project, string userName)
        {
            if (project == null || userName == null)
            {
                return new HttpNotFoundResult();
            }

            var organization = _db.GetProject(project, userName);

            if (organization == null)
            {
                return new HttpNotFoundResult();
            }
            return new HttpOkObjectResult(organization);
        }

        [HttpPost("{userName}")]
        public IActionResult Post([FromBody]Project project, string userName)
        {
            if (project == null)
            {
                return new BadRequestResult();
            }

            var postedProject = _db.PostProject(project);

            string location = Url.RouteUrl("project", new { project = project.Slug, userName = userName }, Request.Scheme);
            return new CreatedResult(location, postedProject);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Project project)
        {
            var patchedProject = _db.PatchProject(id, project);

            return new HttpOkObjectResult(patchedProject);
        }

        [HttpPatch("{projectSlug}/members")]
        public IActionResult PatchMembers(string projectSlug, [FromBody] Patch patch)
        {
            var patchedProjectMembers = _db.PatchProjectMembers(projectSlug, patch);

            return new HttpOkObjectResult(patchedProjectMembers);
        }

        [HttpDelete("{project}/{userName}")]
        public IActionResult Delete(string project, string userName)
        {
            if (_db.GetProject(project, userName) == null)
            {
                return new HttpNotFoundResult();
            }

            _db.DeleteProject(project);

            return new HttpStatusCodeResult(204);
        }

        private readonly RavenDB _db;
    }
}