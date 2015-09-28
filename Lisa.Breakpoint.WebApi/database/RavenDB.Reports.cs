using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Data.Entity;

namespace Lisa.Breakpoint.WebApi
{
    public partial class RavenDB
    {
        public IDocumentStore CreateDocumentStore()
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
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                session.Store(project);
                int projectId = project.Id;

                session.SaveChanges();

                return session.Load<Project>(projectId);
            }
        }

        public IList<Report> GetAllReports()
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Query<Report>().ToList();
            }
        }

        public Report GetReport(int id)
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Load<Report>(id);
            }
        }

        public void PostReport(Report report)
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                session.Store(report);
                //int reportId = report.Id;

                //report.Number = 0;
                //report.Reported = DateTime.Now;
                session.SaveChanges();

                //return session.Load<Project>(reportId);
            }
        }

        public void PatchReport(int id, Report patchedReport)
        {
            IDocumentStore store = CreateDocumentStore();
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
                }
                catch (Exception)
                {
                    //return null;
                }
            }
        }

        public void DeleteReport(int id)
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                Report report = session.Load<Report>(id);
                session.Delete(report);
                session.SaveChanges();
            }
        }

        public int GenerateAvailableNumber(string projectName)
        {
            //IDocumentStore store = CreateDocumentStore();
            //using (IDocumentSession session = store.Initialize().OpenSession())
            //{
            //    var proj = session.Load<Report>("davinci");

            //    var reports = session.Query<Report>()
            //        .Select(report => report.Project[0])
            //        .Where(project => project.Name == projectName)
            //        .ToList();

            //    var reports2 = session.Query<Report>()
            //        .ToList();


            //    var report = session.Include<Report>(x => x.Id).Load("orders/1234");
            //    var project = session.Load<Project>(report.Id);

            //    Debug.WriteLine(reports);

                return 0;
            
        }
    }
}