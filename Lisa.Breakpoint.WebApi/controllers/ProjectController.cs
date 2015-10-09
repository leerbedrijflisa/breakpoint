using Lisa.Breakpoint.WebApi.database;
using Lisa.Breakpoint.WebApi.Models;
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
        [Route("{organization}/{username}")]
        public IList<Project> Get(string organization, string userName)
        {
            return _db.GetAllProjects(organization, userName);
        }

        [HttpGet]
        [Route("/get/{id}")]
        public Project Get(int id)
        {
            return _db.GetProject(id);
        }

        [HttpGet]
        [Route("members/{project}")]
        public IList<string> GetProjectMembers(string project)
        {
            return _db.GetProjectMembers(project);
        }

        [HttpPost]
        public void insert([FromBody]Project project)
        {
            _db.PostProject(project);
        }

        [HttpPost]
        [Route("patch/{id}")]
        public void Patch(int id, Project patchedProject)
        {
            //Project patchedProject = new Project
            //{
            //    Slug = "patched-slug"
            //};

            _db.PatchProject(id, patchedProject);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public void Delete(int id)
        {
            _db.DeleteProject(id);
        }

        //public void CheckVersion() { }
    }
}