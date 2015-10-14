﻿using Lisa.Breakpoint.WebApi.database;
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

        [HttpGet("{organization}/{username}")]
        public IList<Project> Get(string organization, string userName)
        {
            return _db.GetAllProjects(organization, userName);
        }

        [HttpGet("{project}")]
        public Project Get(string project)
        {
            return _db.GetProject(project);
        }

        [HttpGet("members/{project}")]
        public IList<Member> GetProjectMembers(string project)
        {
            var projectObject = _db.GetProject(project);
            return projectObject.Members;
        }

        [HttpPost]
        public void insert([FromBody]Project project)
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
    }
}