using Lisa.Breakpoint.WebApi.Models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.controllers
{
    [Route("Project")]
    public class ProjectController
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        [Route("insert")]
        public Project insert()
        {
            Member member = new Member
            {
                Role = "Slave",
                UserName = "Tafinny69",
                FullName = "Tafinny T"
            };

            Project project = new Project
            {
                Slug = "projectBreekPunt",
                Name = "BreekPunt",
                Member = new List<Member> { member },
            };

            return _db.insert(project);
        }
    }
}
