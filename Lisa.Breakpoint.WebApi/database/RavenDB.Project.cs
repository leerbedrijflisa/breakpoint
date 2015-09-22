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
        public IList<Project> getAllProjects()
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Query<Project>().ToList();
            }
        }

        public Project getProject(int id)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Load<Project>(id);
            }
        }

        public Project insertProject(Project project)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                session.Store(project);
                string projectId = project.Id;

                session.SaveChanges();

                return session.Load<Project>(projectId);
            }
        }

        public Project patchProject(int id, Project patchedProject)
        {
            IDocumentStore store = createDocumentStore();
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

        public void deleteProject(int id)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                Project organization = session.Load<Project>(id);
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}