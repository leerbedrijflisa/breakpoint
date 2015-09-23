using Lisa.Breakpoint.WebApi.models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("projects")]
    public class ProjectController
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        public IList<Project> Get()
        {
            return _db.GetAllProjects();
        }

        [HttpGet]
        [Route("{id}")]
        public Project Get(int id)
        {
            return _db.GetProject(id);
        }

        [HttpPost]
        [Route("post")]
        public void Post([FromBody]Project project)
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