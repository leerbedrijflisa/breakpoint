using Lisa.Breakpoint.WebApi.database;
using Lisa.Breakpoint.WebApi.models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("projects")]
    public class ProjectController
    {
        private readonly RavenDB _db;

        public ProjectController(RavenDB db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("{organization}")]
        public IList<Project> Get(string organization)
        {
            return _db.GetAllProjects(organization);
        }

        [HttpGet]
        [Route("/get/{id}")]
        public Project Get(int id)
        {
            return _db.GetProject(id);
        }

        [HttpPost]
        public void insert([FromBody]Project project)
        {
            _db.PostProject(project);
        }

        [HttpPost]
        [Route("patch/{id}")]
        public void Patch(int id)
        {
            Project patchedProject = new Project
            {
                Slug = "patched-slug"
            };

            _db.PatchProject(id, patchedProject);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public void Delete(int id)
        {
            _db.DeleteProject(id);
        }
    }
}