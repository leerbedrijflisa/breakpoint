﻿using Lisa.Breakpoint.WebApi.models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("project")]
    public class ProjectController
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        public IList<Project> Get()
        {
            return _db.getAllProjects();
        }

        [HttpGet]
        [Route("{id}")]
        public Project get(int id)
        {
            return _db.getProject(id);
        }

        [HttpGet]
        [Route("insert")]
        public Project insert(int id)
        {
            Member member = new Member
            {
                Role     = "admin",
                UserName = "blablaname",
                FullName = "Bas Eenhoorn"
            };

            Project project = new Project
            {   
                Slug = "eerste-project",
                Name = "Eerste project",
            };

            return _db.insertProject(project);
        }

        [HttpGet]
        [Route("patch/{id}")]
        public Project Patch(int id)
        {
            Project patchedProject = new Project
            {
                Slug = "patched-slug"
            };

            return _db.patchProject(id, patchedProject);
        }

        [HttpGet]
        [Route("delete/{id}")]
        public void delete(int id)
        {
            _db.deleteProject(id);
        }
    }
}