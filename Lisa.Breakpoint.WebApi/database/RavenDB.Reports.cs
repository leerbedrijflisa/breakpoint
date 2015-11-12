using Lisa.Breakpoint.WebApi.Models;
using Raven.Abstractions.Data;
using Raven.Abstractions.Extensions;
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
        public IList<Report> GetAllReports(string organizationSlug, string projectSlug, string userName, Filter filter = null)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                IQueryable<Report> rList = session.Query<Report>().Where(r => r.Organization == organizationSlug && r.Project == projectSlug);
                IQueryable<Report> _rList = rList;
                IList<Report> reports;

                if (filter != null)
                {
                    string[] types = { };
                    string[] values = { };
                    bool multipleFilters = false;

                    if (filter.Type.IndexOf('&') != -1 && filter.Value.IndexOf('&') != -1)
                    {
                        types = filter.Type.Split('&');
                        values = filter.Value.Split('&');

                        multipleFilters = true;
                    }

                    if (!multipleFilters)
                    {
                        rList = _rList.ApplyFilter(filter);
                    }
                    else if (multipleFilters)
                    {
                        for (int i = 0; i < types.Length; i++)
                        {
                            Filter tempFilter = new Filter(types[i], values[i]);

                            rList = _rList.ApplyFilter(tempFilter);
                        }
                    }
                }

                reports = rList.OrderBy(r => r.Priority)
                        .ThenByDescending(r => r.Reported.Date)
                        .ThenBy(r => r.Reported.TimeOfDay)
                        .ToList();

                reports.ForEach(r => r.PriorityString = r.Priority.ToString());

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
                            else if (newVal is Enum)
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

    public static class FilterHandler
    {
        public static IQueryable<Report> ApplyFilter(this IQueryable<Report> reports, Filter filter)
        {
            filter.Type = filter.Type.Replace("Filter", "").ToLower();

            if (filter.Type == "version")
            {
                if (filter.Value != "all")
                {
                    if (filter.Value == "empty")
                    {
                        filter.Value = "";
                    }
                    return reports.Where(r => r.Version == filter.Value);
                }
            }
            else if (filter.Type == "group")
            {
                if (filter.Value != "all")
                {
                    return reports.Where(r => r.AssignedTo.Type == "group" && r.AssignedTo.Value == filter.Value);
                }
            }
            else if (filter.Type == "member")
            {
                if (filter.Value != "all")
                {
                    return reports.Where(r => r.AssignedTo.Type == "person" && r.AssignedTo.Value == filter.Value);
                }
            }

            return reports;
        }
    }
}