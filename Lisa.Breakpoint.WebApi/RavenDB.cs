using Raven.Client;
using Raven.Client.Document;

namespace Lisa.Breakpoint.WebApi
{
    public class RavenDB
    {

        public RavenDB()
        {
            using (IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080/",
                DefaultDatabase = "breakpoint"
            }.Initialize())
            {
                using (IDocumentSession session = store.OpenSession())
                {
                    Bug bug = new Bug
                    {
                        Title = "een bug",
                        Description = "fix de bug",
                        Status = "Fixed"
                    };

                    session.Store(bug);
                    int bugId = bug.Id;

                    session.SaveChanges();

                    Bug loadedBug = session.Load<Bug>(bugId);
                }
            }
        }
    }
}