using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
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

        public IList<Report> getAll()
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
<<<<<<< HEAD
                return session.Query<Report>().ToList<Report>();
=======
                return session.Query<Report>()
                    .ToList<Report>();
>>>>>>> 8ffd6a9523627e99188b84c861c50dffe9da2a9d
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
                int reportId = report.Id;

                session.SaveChanges();

                return session.Load<Report>(reportId);
            }
        }

        public Report update(int id, Report updatedReport)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                Report report = session.Load<Report>(id);

                foreach (PropertyInfo propertyInfo in report.GetType().GetProperties())
                {
                    var newVal = updatedReport.GetType().GetProperty(propertyInfo.Name).GetValue(updatedReport, null);

                    if (newVal != null)
                    {
                        var patchRequest = new PatchRequest()
                        {
                            Name = propertyInfo.Name,
                            Type = PatchCommandType.Set,
                            Value = newVal.ToString()
                        };
                        //Debug.WriteLine("updating field: ");
                        store.DatabaseCommands.Patch("reports/" + id, new[] { patchRequest });
                    }
                    //Debug.WriteLine(propertyInfo.Name + ": " + propertyInfo.GetValue(report, null));
                    //Debug.WriteLine(propertyInfo.Name + ": " + newVal);
                }

                return report;
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