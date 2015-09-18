using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
                return session.Query<Report>()
                    .ToList<Report>();
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

        public Report update(int id)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                Report report = session.Load<Report>(id);

                //report.Title = "Updated report";
                //report.Description = "The bug is almost fixed";
                report.Status = "In progress";

                session.SaveChanges();

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