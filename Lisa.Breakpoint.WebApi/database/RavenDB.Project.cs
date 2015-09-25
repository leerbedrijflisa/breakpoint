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
        public IList<Project> GetAllProjects()
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Query<Project>().ToList();
            }
        }

        public Project GetProject(int id)
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Load<Project>(id);
            }
        }

        public Project PostProject(Project project)
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                session.Store(project);
                int projectId = project.Id;

                session.SaveChanges();

                return session.Load<Project>(projectId);
            }
        }

        public Project PatchProject(int id, Project patchedProject)
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
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
                            store.DatabaseCommands.Patch("Projects/" + id, new[] { patchRequest });
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
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                Project organization = session.Load<Project>(id);
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}