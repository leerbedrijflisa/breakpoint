using Lisa.Breakpoint.WebApi.Models;
using Raven.Abstractions.Data;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lisa.Breakpoint.WebApi.database
{
    // TODO: convert all IDs to int, so we don't have to prefix report/ anymore.
    public partial class RavenDB
    {
        public IList<Report> GetAllReports(string project, string userName, string group)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Query<Report>()
                    .Where(r => r.Project == project && (r.AssignedToPerson == userName || r.AssignedToGroup == group))
                    .ToList();
            }
        }

        public Report GetReport(string id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                var report = session.Load<Report>(int.Parse(id));
                return report;
            }
        }

        public IList<string> GetOrganizationMembers(string organization)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                var list = session.Query<Organization>()
                    .Where(o => o.Slug == organization)
                    .ToList();
                if(list == null)
                {
                    return null;
                }
                return list[0].Members;
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
                Report report = session.Load<Report>("reports/" + id);

                foreach (PropertyInfo propertyInfo in report.GetType().GetProperties())
                {
                    var newVal = patchedReport.GetType().GetProperty(propertyInfo.Name).GetValue(patchedReport, null);
                    if (propertyInfo.Name != "Reported" )
                    {
                        if (newVal != null)
                        {
                            var patchRequest = new PatchRequest()
                            {
                                Name = propertyInfo.Name,
                                Type = PatchCommandType.Set,
                                Value = newVal.ToString()
                            };
                            documentStore.DatabaseCommands.Patch("reports/"+id, new[] { patchRequest });
                        }
                    }
                }
                return session.Load<Report>("reports/" + id);
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