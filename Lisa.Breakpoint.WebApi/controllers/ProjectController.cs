using Lisa.Breakpoint.WebApi.Models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lisa.Breakpoint.WebApi
{
    [Route("projects")]
    public class ProjectController
    {
        static RavenDB _db = new RavenDB();

        public IList<Project> Get()
        {
            return _db.GetAllProjects();
        }

        [Route("{id}")]
        public Project Get(int id)
        {
            return _db.GetProject(id);
        }

        [HttpPost]
        [Route("insert")]
        public Project insert([FromBody]Project project)
        {
            //Member member = new Member
            //{
            //    Role = "admin",
            //    UserName = "blablaname",
            //    FullName = "Bas Eenhoorn"
            //};

            //Project project = new Project
            //{
            //    Slug = "eerste-project",
            //    Name = "Eerste project",
            //};

            return _db.PostProject(project);
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