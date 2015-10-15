﻿using Lisa.Breakpoint.WebApi.database;
using Lisa.Breakpoint.WebApi.Models;
using Microsoft.AspNet.Mvc;

namespace Lisa.Breakpoint.WebApi
{
    [Route("organizations")]
    public class OrganizationController : Controller
    {
        public OrganizationController(RavenDB db)
        {
            _db = db;
        }

        [HttpGet("{username}")]
        public IActionResult GetAll(string userName)
        {
            if (_db.GetUser(userName) == null)
            {
                return new HttpNotFoundResult();
            }

            var organizations = _db.GetAllOrganizations(userName);

            if (organizations == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(organizations);
        }

        [HttpGet("members/{organization}")]
        public IActionResult GetOrganizationMembers(string organization)
        {
            if (_db.GetOrganization(organization) == null)
            {
                return new HttpNotFoundResult();
            }

            var members = _db.GetOrganizationMembers(organization);

            return new HttpOkObjectResult(organization);
        }

        [HttpGet("get/{organization}", Name = "organization")]
        public IActionResult Get(string organizationSlug)
        {
            var organization = _db.GetOrganization(organizationSlug);

            if (organization == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(organization);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Organization organization)
        {
            if (organization == null)
            {
                return new BadRequestResult();
            }

            var postedOrganization = _db.PostOrganization(organization);

            string location = Url.RouteUrl("organization", new { organization = organization }, Request.Scheme);
            return new CreatedResult(location, postedOrganization);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Organization organization)
        {
            var patchedOrganization = _db.PatchOrganization(id, organization);

            return new HttpOkObjectResult(patchedOrganization);
        }

        [HttpDelete("{organization}")]
        public IActionResult Delete(string organization)
        {
            if (_db.GetOrganization(organization) == null)
            {
                return new HttpNotFoundResult();
            }

            _db.DeleteOrganization(organization);

            return new HttpStatusCodeResult(204);
        }

        private readonly RavenDB _db;
    }
}