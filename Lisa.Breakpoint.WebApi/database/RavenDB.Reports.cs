﻿using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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
            int number = GenerateAvailableNumber(report.Project[0].Name);

            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {

                report.Number = number;
                report.Reported = DateTime.Now;

                session.Store(report);
                session.SaveChanges();
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

        // gets highest number+1 of a bug report (per project)
        public int GenerateAvailableNumber(string projectName)
        {
            IDocumentStore store = CreateDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                // foreach report
                // get alle 'report.Number'
                // sorteer
                // verkrijg hoogste
                // return hoogste + 1

                var reports = session.Query<Report>()
                    .Where(r => r.Project.Any(project => project.Name == projectName)).ToList();

                IList<int> numberList = new List<int> { };

                foreach (Report report in reports)
                {
                    numberList.Add(report.Number);
                }

                var sortedList = numberList.OrderByDescending(i => i).ToList();
                int number = sortedList[0] + 1;

                return number;
            }
        }
    }
}