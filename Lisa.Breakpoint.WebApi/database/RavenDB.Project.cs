using Lisa.Breakpoint.WebApi.Models;
using Raven.Abstractions.Data;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lisa.Breakpoint.WebApi.database
{
    public partial class RavenDB
    {
        public IList<Project> GetAllProjects(string organizationName, string userName)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Query<Project>()
                    .Where(p => p.Members.Any(m => m.UserName == userName) && p.Organization == organizationName)
                    .ToList();
            }
        }

        public Project GetProject(string projectSlug, string userName)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                var filter = false;
                var project = session.Query<Project>()
                    .Where(p => p.Slug == projectSlug)
                    .SingleOrDefault();

                if (project == null)
                {
                    return null;
                }

                foreach (var member in project.Members)
                {
                    if (member.UserName == userName)
                    {
                        var role  = member.Role;
                        if (role != "")
                        {
                            int level = project.Groups
                                .Where(g => g.Name == role)
                                .Select(g => g.Level)
                                .SingleOrDefault();

                            project.Groups = project.Groups
                                .Where(g => g.Level <= level)
                                .ToList();

                            filter = true;
                        }
                    }
                }

                if (!filter)
                {
                    project.Groups = null;
                }

                return project;
            }
        }

        public void PostProject(Project project)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                session.Store(project);
                session.SaveChanges();
            }
        }

        public Project PatchProject(int id, Project patchedProject)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Project project = session.Load<Project>(id);

                try
                {
                    foreach (PropertyInfo propertyInfo in project.GetType().GetProperties())
                    {
                        var newVal = patchedProject.GetType().GetProperty(propertyInfo.Name).GetValue(patchedProject, null);

                        if (newVal != null)
                        {
                            var patchRequest = new PatchRequest()
                            {
                                Name = propertyInfo.Name,
                                Type = PatchCommandType.Add,
                                Value = newVal.ToString()
                            };
                            documentStore.DatabaseCommands.Patch("Projects/" + id, new[] { patchRequest });
                        }
                    }

                    return project;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Project PatchMember(string projectName, Member patchedMember)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Project project = session.Query<Project>()
                    .Where(p => p.Name == projectName)
                    .SingleOrDefault();

                return project;
            }
        }

        public void DeleteProject(int id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Project organization = session.Load<Project>(id);
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}