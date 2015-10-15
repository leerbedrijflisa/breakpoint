using Lisa.Breakpoint.WebApi.database;
using Lisa.Breakpoint.WebApi.Models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("projects")]
    public class ProjectController
    {
        public ProjectController(RavenDB db)
        {
            _db = db;
        }

        [HttpGet("{organization}/{username}")]
        public IList<Project> GetAll(string organization, string userName)
        {
            return _db.GetAllProjects(organization, userName);
        }

        [HttpGet("get/{project}/{userName}")]
        public Project Get(string project, string userName)
        {
            return _db.GetProject(project, userName);
        }

        [HttpPost]
        public void Insert([FromBody]Project project)
        {
            _db.PostProject(project);
        }

        [HttpPatch("{id}")]
        public void Patch(int id, Project patchedProject)
        {
            _db.PatchProject(id, patchedProject);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _db.DeleteProject(id);
        }

        private readonly RavenDB _db;
    }
}