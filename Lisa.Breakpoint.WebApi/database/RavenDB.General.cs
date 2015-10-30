using System;
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

        internal object GetProject(object project, string v)
        {
            throw new NotImplementedException();
        }
    }
}