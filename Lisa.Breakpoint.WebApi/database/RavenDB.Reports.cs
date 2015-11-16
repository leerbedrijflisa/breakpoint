using Lisa.Breakpoint.WebApi.Models;
using Raven.Abstractions.Data;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Linq;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                        rList = rList.ApplyFilters(filter);
                    }
                    else if (multipleFilters)
                    {
                        int filterCount = types.Count();
                        Filter[] tempFilters = new Filter[filterCount];
                        for (int i = 0; i < types.Length; i++)
                        {
                            tempFilters[i] = new Filter(types[i], values[i]);
                        }
                        rList = rList.ApplyFilters(tempFilters);
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
        private static Expression<Func<Report, bool>> WhereVersion(string term)
        {
            return r => r.Version == term;
        }
        private static Expression<Func<Report, bool>> WhereTitleStartsWith(string term)
        {
            return r => r.Title.StartsWith(term.ToString());
        }
        private static Expression<Func<Report, bool>> WhereGroup(string term)
        {
            return r => r.AssignedTo.Value == term && r.AssignedTo.Type == "group";
        }
        private static Expression<Func<Report, bool>> WhereMember(string term)
        {
            return r => r.AssignedTo.Value == term && r.AssignedTo.Type == "person";
        }
        private static Expression<Func<Report, bool>> WhereReporter(string term)
        {
            return r => r.Reporter == term;
        }
        private static Expression<Func<Report, bool>> WhereStatus(string term)
        {
            return r => r.Status == term.Replace("%20", " ");
        }
        private static Expression<Func<Report, bool>> WhereNoGroups()
        {
            return r => r.AssignedTo.Type != "group";
        }
        private static Expression<Func<Report, bool>> WhereNoMembers()
        {
            return r => r.AssignedTo.Type != "person";
        }
        private static Expression<Func<Report, bool>> WhereAllGroups()
        {
            return r => r.AssignedTo.Type == "group";
        }
        private static Expression<Func<Report, bool>> WhereAllMembers()
        {
            return r => r.AssignedTo.Type == "person";
        }
        private static Expression<Func<Report, bool>> WherePriority(Priority priority)
        {
            return r => r.Priority == priority;
        }
        private static Expression<Func<Report, bool>> WhereReportedAfter(DateTime dateTime)
        {
            return r => r.Reported > dateTime;
        }
        private static Expression<Func<Report, bool>> WhereReportedBefore(DateTime dateTime)
        {
            return r => r.Reported < dateTime;
        }
        private static Expression<Func<Report, bool>> WhereReportedOn(DateTime dateTime)
        {
            return r => r.Reported == dateTime;
        }

        public static IQueryable<Report> ApplyFilters(this IQueryable<Report> reports, params Filter[] filters)
        {
            Expression<Func<Report, bool>> outerPredicate = r => r.Number != string.Empty;
            Expression<Func<Report, bool>> innerPredicate = r => r.Number == "-1"; 

            foreach (Filter filter in filters)
            {
                filter.Type = filter.Type.Replace("Filter", "").ToLower();

                if (filter.Type == "version")
                {
                    if (filter.Value != "all")
                    {
                        if (filter.Value == "none")
                        {
                            filter.Value = "";
                        }
                        outerPredicate = outerPredicate.And(WhereVersion(filter.Value));
                    }
                }
                else if (filter.Type == "title")
                {
                    if (filter.Value != "")
                    {
                        outerPredicate = outerPredicate.And(WhereTitleStartsWith(filter.Value));
                    }
                }
                else if (filter.Type == "priority")
                {
                    if (filter.Value != "all")
                    {
                        outerPredicate = outerPredicate.And(WherePriority((Priority)Enum.Parse(typeof(Priority), filter.Value)));
                    }
                }
                else if (filter.Type == "status")
                {
                    if (filter.Value != "all")
                    {
                        outerPredicate = outerPredicate.And(WhereStatus(filter.Value));
                    }
                }
                else if (filter.Type == "group")
                {
                    if (filter.Value == "none")
                    {
                        outerPredicate = outerPredicate.And(WhereNoGroups());
                    }
                    else if (filter.Value == "all")
                    {
                        innerPredicate = innerPredicate.Or(WhereAllGroups());
                    }
                    else
                    {
                        innerPredicate = innerPredicate.Or(WhereGroup(filter.Value));
                    }
                }
                else if (filter.Type == "member")
                {
                    if (filter.Value == "none")
                    {
                        outerPredicate = outerPredicate.And(WhereNoMembers());
                    }
                    else if (filter.Value == "all")
                    {
                        innerPredicate = innerPredicate.Or(WhereAllMembers());
                    }
                    else
                    {
                        innerPredicate = innerPredicate.Or(WhereMember(filter.Value));
                    }
                }
                else if (filter.Type == "reporter")
                {
                    if (filter.Value != "all")
                    {
                        outerPredicate = outerPredicate.And(WhereReporter(filter.Value));
                    }
                }
            }

            reports = reports.Where(outerPredicate.And(innerPredicate));

            return reports;
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, expr2.Body), expr1.Parameters);
        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, expr2.Body), expr1.Parameters);
        }
    }
}