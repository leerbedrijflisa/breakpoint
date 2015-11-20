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
                dynamic platforms = session.Load<object>("singles/platforms");

                return platforms != null ? platforms.value : new string[] { };
            }
        }

        public void PatchPlatforms(string[] platforms)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                dynamic platformsDbObject = session.Load<object>("singles/platforms");

				if (platformsDbObject == null)
                {
                    var platformsList = platforms;
                    session.Store(new { value = platformsList }, "singles/platforms");
                }
				else
                {
                    string[] platformsList = platformsDbObject.value;

                    platformsList = platforms.Where(p => !platformsList.Contains(p)).ToArray();

                    var patchRequests = new List<PatchRequest>();

                    Array.ForEach(platformsList, p => patchRequests.AddRange(new[] { new PatchRequest()
                        {
                            Name = "value",
                            Type = PatchCommandType.Add,
                            Value = p
                        }
                    }));

                    documentStore.DatabaseCommands.Patch("singles/platforms", patchRequests.ToArray());
                }

                session.SaveChanges();
            }
        }
    }
}
