using System;
using System.Collections.Generic;
using Lisa.Breakpoint.WebApi.Models;
using Raven.Client;
using System.Linq;

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