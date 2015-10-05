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
        public IList<Organization> GetAllOrganizations()
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Query<Organization>().ToList();
            }
        }

        public Organization GetOrganization(int id)
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Load<Organization>(id);
            }
        }

        public void PostOrganization(Organization organization)
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                session.Store(organization);
                session.SaveChanges();
            }
        }

        public Organization PatchOrganization(int id, Organization patchedOrganization)
        {
            IDocumentStore store = CreateDocumentStore();
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

        public void DeleteOrganization(int id)
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                Organization organization = session.Load<Organization>(id);
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}