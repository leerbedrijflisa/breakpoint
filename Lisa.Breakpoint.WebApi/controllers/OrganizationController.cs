using Lisa.Breakpoint.WebApi.models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi
{
    [Route("organizations")]
    public class OrganizationController
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        public IList<Organization> Get()
        {
            return _db.GetAllOrganizations();
        }

        [HttpGet]
        [Route("{id}")]
        public Organization Get(int id)
        {
            return _db.GetOrganization(id);
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