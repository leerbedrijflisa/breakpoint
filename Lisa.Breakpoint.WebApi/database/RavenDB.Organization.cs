using Lisa.Breakpoint.WebApi.models;
using Raven.Abstractions.Data;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lisa.Breakpoint.WebApi
{
    public partial class RavenDB
    {
        public IList<Organization> getAllOrganizations()
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Query<Organization>().ToList();
            }
        }

        public Organization getOrganization(int id)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Load<Organization>(id);
            }
        }

        public Organization insertOrganization(Organization organization)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                session.Store(organization);
                string organizationId = organization.Id;

                session.SaveChanges();

                return session.Load<Organization>(organizationId);
            }
        }

        public Organization patchOrganization(int id, Organization patchedOrganization)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
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
                            store.DatabaseCommands.Patch("organizations/" + id, new[] { patchRequest });
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

        public void deleteOrganization(int id)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                Organization organization = session.Load<Organization>(id);
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}