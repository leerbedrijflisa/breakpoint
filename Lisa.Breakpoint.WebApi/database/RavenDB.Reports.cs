using Lisa.Breakpoint.WebApi.Models;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lisa.Breakpoint.WebApi.database
{
    public partial class RavenDB
    {
        public IList<Report> GetAllReports(string organizationSlug, string projectSlug, string userName)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {

                IList<Report> reports;

                var group = session.Query<Project>()
                    .Where(p => p.Organization == organizationSlug && p.Slug == projectSlug && p.Members.Any(m => m.UserName == userName))
                    .SingleOrDefault().Members
                    .Where(m => m.UserName == userName)
                    .SingleOrDefault();

                var role = group.Role;

                if (role == "manager")
                {
                    reports = session.Query<Report>()
                        .Where(r => r.Organization == organizationSlug && r.Project == projectSlug)
                        .ToList();
                } else
                {
                    reports = session.Query<Report>()
                        .Where(r => r.Organization == organizationSlug && r.Project == projectSlug && (r.AssignedTo.Type == "person" && r.AssignedTo.Value == userName || r.AssignedTo.Type == "group" && r.AssignedTo.Value == role))
                        .ToList();
                }
                return reports;
            }
        }

        public Report GetReport(int id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Load<Report>(id);
            }
        }

        public Report PostReport(Report report)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                session.Store(report);

                string reportId = session.Advanced.GetDocumentId(report);
                report.Number = reportId.Split('/').Last();
                report.Reported = DateTime.Now;

                session.SaveChanges();

                return report;
            }
        }

        public Report PatchReport(int id, Report patchedReport)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Report report = session.Load<Report>(id);

                foreach (PropertyInfo propertyInfo in report.GetType().GetProperties())
                {
                    var newVal = patchedReport.GetType().GetProperty(propertyInfo.Name).GetValue(patchedReport, null);

                    if (propertyInfo.Name != "Reported")
                    {
                        if (newVal != null)
                        {
                            if (newVal is string)
                            {
                                var patchRequest = new PatchRequest()
                                {
                                    Name = propertyInfo.Name,
                                    Type = PatchCommandType.Set,
                                    Value = newVal.ToString()
                                };
                                documentStore.DatabaseCommands.Patch("reports/" + id, new[] { patchRequest });
                            }
                            else
                            {
                                var patchRequest = new PatchRequest()
                                {
                                    Name = propertyInfo.Name,
                                    Type = PatchCommandType.Set,
                                    Value = RavenJObject.FromObject((AssignedTo) newVal)
                                };
                                documentStore.DatabaseCommands.Patch("reports/" + id, new[] { patchRequest });
                            }
                         }
                    }
                }
                return session.Load<Report>(id);
            }
        }

        public void DeleteReport(int id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Report report = session.Load<Report>(id);
                session.Delete(report);
                session.SaveChanges();
            }
        }
    }
}