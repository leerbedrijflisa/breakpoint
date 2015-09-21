using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Lisa.Breakpoint.WebApi
{
    public class RavenDB
    {
        public IDocumentStore createDocumentStore()
        {
            IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080/",
                DefaultDatabase = "breakpoint"
            };
            
            return store;
        }

        internal Project insert(Project project)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                session.Store(project);
                int projectId = project.Id;

                session.SaveChanges();

                return session.Load<Project>(projectId);
            }
        }

        internal User insert(User user)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                session.Store(user);
                int userId = user.Id;

                session.SaveChanges();

                return session.Load<User>(userId);
            }
        }

        public IList<Report> getAll()
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Query<Report>().ToList<Report>();
            }
        }

        public Report get(int id)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Load<Report>(id);
            }
        }

        public Report insert(Report report)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                session.Store(report);
                string reportId = report.Id;

                session.SaveChanges();

                return session.Load<Report>(reportId);
            }
        }

        public Report patch(int id, Report patchedReport)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                Report report = session.Load<Report>(id);

                try
                {
                    foreach (PropertyInfo propertyInfo in report.GetType().GetProperties())
                    {
                        var newVal = patchedReport.GetType().GetProperty(propertyInfo.Name).GetValue(patchedReport, null);

                        if (newVal != null)
                        {
                            var patchRequest = new PatchRequest()
                            {
                                Name = propertyInfo.Name,
                                Type = PatchCommandType.Set,
                                Value = newVal.ToString()
                            };
                            store.DatabaseCommands.Patch("reports/" + id, new[] { patchRequest });
                        }
                    }

                    return report;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public void delete(int id)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                Report report = session.Load<Report>(id);
                session.Delete(report);
                session.SaveChanges();
            }
        }
    }
}