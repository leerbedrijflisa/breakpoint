using Lisa.Breakpoint.WebApi.Models;
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
        public IList<Report> GetAllReports(string project, string userName, string group)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Query<Report>()
                    .Where(r => r.ProjectName == project && r.AssignedToPerson == userName || r.ProjectName == project && r.AssignedToGroup == group)
                    .ToList();
            }
        }

        public Report GetReport(string id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Load<Report>(id);
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

//<<<<<<< HEAD
//                string reportId = session.Advanced.GetDocumentId(report);
//                report.Number = reportId.Split('/').Last();
//=======
//>>>>>>> feature/test-version
                report.Reported = DateTime.Now;

                session.SaveChanges();

                return report;
            }
        }

        public Report PatchReport(int id, Report patchedReport)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Report report = session.Load<Report>("reports/"+id);

                try
                {
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
                catch (Exception)
                {
                    return null;
                }
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