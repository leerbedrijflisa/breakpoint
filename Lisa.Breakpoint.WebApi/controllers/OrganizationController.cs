using Lisa.Breakpoint.WebApi.models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("organization")]
    public class OrganizationController
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        public IList<Organization> Get()
        {
            return _db.getAllOrganizations();
        }

        [HttpGet]
        [Route("{id}")]
        public Organization get(int id)
        {
            return _db.getOrganization(id);
        }

        [HttpGet]
        [Route("insert")]
        public Organization insert(int id)
        {
            Project project = new Project
            {
                Slug = "eerste-project",
                Name = "Eerste Project"
            };

            Member user = new Member
            {
                Role     = "admin",
                UserName = "blablaname",
                FullName = "Bas Eenhoorn"
            };

            Organization organization = new Organization
            {   
                Slug = "OrganizationSlug",
                Member = new List<Member> { user },
                Project = new List<Project> { project }
            };

            return _db.insertOrganization(organization);
        }

        [HttpGet]
        [Route("patch/{id}")]
        public Organization Patch(int id)
        {
            Organization patchedOrganization = new Organization
            {
                Slug = "patched-slug"
            };

            return _db.patchOrganization(id, patchedOrganization);
        }

        [HttpGet]
        [Route("delete/{id}")]
        public void delete(int id)
        {
            _db.deleteOrganization(id);
        }
    }
}