using Lisa.Breakpoint.WebApi.models;
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
                return session.Query<Organization>()
                    .Where(o => o.Members.Any(m => m.UserName == userName))
                    .ToList();
            }
        }

        public Organization GetOrganization(int id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Load<Organization>(id);
            }
        }

        public void PostOrganization(Organization organization)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                session.Store(organization);
                session.SaveChanges();
            }
        }

        public Group PostGroup(Group group)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                session.Store(group);
                session.SaveChanges();

                return group;
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

        public void DeleteOrganization(int id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Organization organization = session.Load<Organization>(id);
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}