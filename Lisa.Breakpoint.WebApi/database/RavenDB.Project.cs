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
        public IList<Project> GetAllProjects(string organizationName, string userName)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Query<Project>()
                    .Where(p => p.Members.Any(m => m == userName) && p.Organization == organizationName)
                    .ToList();
            }
        }

        public Project GetProject(int id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Load<Project>(id);
            }
        }

        public void PostProject(Project project)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                session.Store(project);
                session.SaveChanges();
            }
        }

        public Project PatchProject(int id, Project patchedProject)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Project project = session.Load<Project>(id);

                try
                {
                    foreach (PropertyInfo propertyInfo in project.GetType().GetProperties())
                    {
                        var newVal = patchedProject.GetType().GetProperty(propertyInfo.Name).GetValue(patchedProject, null);

                        if (newVal != null)
                        {
                            var patchRequest = new PatchRequest()
                            {
                                Name = propertyInfo.Name,
                                Type = PatchCommandType.Set,
                                Value = newVal.ToString()
                            };
                            documentStore.DatabaseCommands.Patch("Projects/" + id, new[] { patchRequest });
                        }
                    }

                    return project;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public void DeleteProject(int id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Project organization = session.Load<Project>(id);
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}