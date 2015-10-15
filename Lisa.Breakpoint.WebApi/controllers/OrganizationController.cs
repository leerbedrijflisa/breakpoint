using Lisa.Breakpoint.WebApi.database;
using Lisa.Breakpoint.WebApi.Models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("organizations")]
    public class OrganizationController
    {
        private readonly RavenDB _db;

        public OrganizationController(RavenDB db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("{username}")]
        public IList<Organization> GetAll(string userName)
        {
            return _db.GetAllOrganizations(userName);
        }

        [HttpGet]
        [Route("members/{organization}")]
        public IList<string> GetOrganizationMembers(string organization)
        {
            return _db.GetOrganizationMembers(organization);
        }

        [HttpGet]
        [Route("get/{organization}")]
        public Organization Get(string organization)
        {
            return _db.GetOrganization(organization);
        }

        [HttpPost]
        [Route("post")]
        public void Post([FromBody]Organization organization)
        {
            _db.PostOrganization(organization);
        }

        [HttpPost]
        [Route("patch/{id}")]
        public void Patch(int id)
        {
            Organization patchedOrganization = new Organization
            {
                Slug = "patched-slug"
            };

            _db.PatchOrganization(id, patchedOrganization);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public void Delete(int id)
        {
            _db.DeleteOrganization(id);
        }
    }
}