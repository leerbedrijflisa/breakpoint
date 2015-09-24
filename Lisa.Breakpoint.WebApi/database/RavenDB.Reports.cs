using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lisa.Breakpoint.WebApi
{
    public partial class RavenDB
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

        public IList<Report> getAllReports()
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Query<Report>().ToList();
            }
        }

        public Report getReport(int id)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Load<Report>(id);
            }
        }

        public Report insertReport(Report report)
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

        public Report patchReport(int id, Report patchedReport)
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

        public void deleteReport(int id)
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