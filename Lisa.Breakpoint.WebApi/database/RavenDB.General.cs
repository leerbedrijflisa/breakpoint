using Raven.Client;

namespace Lisa.Breakpoint.WebApi.database
{
    public partial class RavenDB 
    {
        private readonly IDocumentStore documentStore;

        public RavenDB(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }
    }
}