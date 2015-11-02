using Lisa.Breakpoint.WebApi.Models;
using Raven.Abstractions.Data;
using Raven.Client;
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

        public Project GetProject(string organizationSlug, string projectSlug, string userName)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                var filter = false;
                var project = session.Query<Project>()
                    .Where(p => p.Organization == organizationSlug && p.Slug == projectSlug)
                    .SingleOrDefault();

                if (project == null)
                {
                    return null;
                }

                // check the role (level) of the userName
                // manager = 3; developer = 2; tester = 1;
                // adds [n/a] to roles yo are not supposed to edit
                // you can search for [n/a] in your javascript
                // and disble those options
                foreach (Member member in project.Members)
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

                            //project.Groups.Where(g => g.Level > level)
                            //    .ToList()
                            //    .ForEach(gg => gg.Name += "[n/a]");

                            project.Groups.Where(g => g.Level <= level)
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

        public Project PostProject(Project project)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                if (session.Query<Project>().Where(p => p.Organization == project.Organization && p.Slug == project.Slug).ToList().Count == 0)
                {
                    session.Store(project);
                    session.SaveChanges();

                    return project;
                } else
                {
                    return null;
                }
            }
        }

        public Project PatchProject(int id, Project patchedProject)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Project project = session.Load<Project>(id);

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
        }

        public Project PatchProjectMembers(string projectSlug, Patch patch)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Project project = session.Query<Project>()
                    .Where(p => p.Slug == projectSlug)
                    .SingleOrDefault();

                IList<Member> members = project.Members;

                if (patch.Type == "add")
                {
                    Member newMember = new Member();

                    newMember.UserName = patch.Member;
                    newMember.Role = patch.Role;

                    members.Add(newMember);
                }
                else if (patch.Type == "remove")
                {
                    Member newMember = new Member();

                    for (int i = 0; i < members.Count; i++)
                    {
                        if (members[i].UserName == patch.Member)
                        {
                            members.RemoveAt(i);
                            break;
                        }
                    }
                }
                else if (patch.Type == "update")
                {
                    foreach (var m in members)
                    {
                        if (m.UserName == patch.Member)
                        {
                            m.Role = patch.Role;
                            break;
                        }
                    }
                }

                project.Members = members;

                session.SaveChanges();
                return project;
            }
        }

        public void DeleteProject(string projectSlug)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Project organization = session.Query<Project>().Where(p => p.Slug == projectSlug).SingleOrDefault();
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}