using Raven.Abstractions.Data;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lisa.Breakpoint.WebApi.database
{
    public partial class RavenDB
    {
        public string[] GetPlatforms()
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                dynamic platformsDbObject = session.Load<object>("singles/platforms");
                
                return platformsDbObject != null && platformsDbObject.value != null ? platformsDbObject.value : new string[] { };
            }
        }

        public void PostPlatforms(IEnumerable<string> platforms)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                dynamic platformsDbObject = session.Load<object>("singles/platforms");

                // Test if the document already exists in the database
				if (platformsDbObject != null && platformsDbObject.value != null)
                {
                    string[] platformsList = platformsDbObject.value;

                    var patchValuesList = platforms.Where(p => !platformsList.Contains(p) && p != "").ToArray();

                    var patchRequests = new List<PatchRequest>();

                    Array.ForEach(patchValuesList, value => patchRequests.Add(new PatchRequest()
                    {
                        Name = "value",
                        Type = PatchCommandType.Add,
                        Value = value
                    }
                    ));

                    documentStore.DatabaseCommands.Patch("singles/platforms", patchRequests.ToArray());
                }
				else
                {
                    session.Store(new { value = platforms.ToArray() }, "singles/platforms");
                }

                session.SaveChanges();
            }
        }
    }
}
