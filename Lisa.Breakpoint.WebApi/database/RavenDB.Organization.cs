﻿using Lisa.Breakpoint.WebApi.Models;
using Raven.Abstractions.Data;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lisa.Breakpoint.WebApi.database
{
    public partial class RavenDB
    {
        public IList<Organization> GetAllOrganizations(string userName)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                if (userName != null || userName != "")
                {
                    return session.Query<Organization>()
                        .Where(o => o.Members.Any(m => m == userName))
                        .ToList();
                }
                return null;
            }
        }

        public Organization GetOrganization(string organization)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Query<Organization>()
                    .Where(o => o.Slug == organization)
                    .SingleOrDefault();
            }
        }

        public IList<string> GetOrganizationMembers(string organization)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Query<Organization>()
                    .Where(o => o.Slug == organization)
                    .SingleOrDefault().Members;
            }
        }

        public IList<string> GetMembersNotInProject(string organization, string project)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                var projectMembers = session.Query<Project>()
                    .Where(p => p.Organization == organization && p.Slug == project)
                    .SingleOrDefault().Members;

                var newMembers = session.Query<Organization>()
                    .Where(o => o.Slug == organization)
                    .SingleOrDefault().Members;

                foreach (var newMember in newMembers)
                {
                    foreach (var member in projectMembers)
                    {
                        newMembers = newMembers.Where(m => m != member.UserName).ToArray();
                    }
                }
                
                return newMembers;
            }
        }

        public Organization PostOrganization(Organization organization)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                session.Store(organization);
                session.SaveChanges();

                return organization;
            }
        }

        public Organization PatchOrganization(int id, Organization patchedOrganization)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Organization Organization = session.Load<Organization>(id);

                try
                {
                    foreach (PropertyInfo propertyInfo in Organization.GetType().GetProperties())
                    {
                        var newVal = patchedOrganization.GetType().GetProperty(propertyInfo.Name).GetValue(patchedOrganization, null);

                        if (newVal != null)
                        {
                            var patchRequest = new PatchRequest()
                            {
                                Name = propertyInfo.Name,
                                Type = PatchCommandType.Set,
                                Value = newVal.ToString()
                            };
                            documentStore.DatabaseCommands.Patch("organizations/" + id, new[] { patchRequest });
                        }
                    }

                    return Organization;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public void DeleteOrganization(string organizationSlug)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Organization organization = session.Query<Organization>().Where(o => o.Slug == organizationSlug).SingleOrDefault();
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}