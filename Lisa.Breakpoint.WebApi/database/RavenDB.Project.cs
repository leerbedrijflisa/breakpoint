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

        public Project GetProject(string organizationSlug, string projectSlug, string userName, string includeAllGroups = "false")
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
                // remove the roles you may not use
                if (includeAllGroups == "false")
                {
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

                                var groups = project.Groups.ToList();

                                groups.RemoveAll(g => g.Level > level);

                                project.Groups = groups;

                                filter = true;
                            }
                        }
                    }
                    if (!filter)
                    {
                        project.Groups = null;
                    }
                }

                return project;
            }
        }

        public IList<Group> GetGroupsFromProject(string organization, string projectSlug)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                var project = session.Query<Project>()
                    .Where(p => p.Organization == organization && p.Slug == projectSlug)
                    .SingleOrDefault();

                if (project.Groups != null)
                {
                    return project.Groups;
                }

                return null;
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

        public Project PatchProjectMembers(string organizationSlug, string projectSlug, Patch patch)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                Project project = session.Query<Project>()
                    .Where(p => p.Organization == organizationSlug && p.Slug == projectSlug)
                    .SingleOrDefault();

                bool roleExist = false;

                string sender      = patch.Sender;
                string senderRole  = GetGroupFromUser(project.Organization, project.Slug, sender);
                int    senderLevel = 0;

                string type      = patch.Type;
                string role      = patch.Role;
                string member    = patch.Member;
                int    roleLevel = 0;

                IList<Group> groups = GetGroupsFromProject(project.Organization, project.Slug);

                foreach (Group group in groups)
                {
                    if (group.Name == senderRole)
                    {
                        senderLevel = group.Level;
                    }
                    if (group.Name == role)
                    {
                        roleLevel = group.Level;
                        roleExist = true;
                    }
                }

                if (!roleExist)
                {
                    return null;
                }

                if (senderLevel >= roleLevel)
                {
                    IList<Member> members = project.Members;
                    if (type == "add")
                    {
                        bool isInProject = false;
                        Member newMember = new Member();

                        newMember.UserName = member;
                        newMember.Role = role;

                        foreach (var m in members)
                        {
                            if (m.UserName == newMember.UserName)
                            {
                                isInProject = true;
                                break;
                            }
                        }

                        if (!isInProject)
                        {
                            members.Add(newMember);
                        }
                    }
                    else if (type == "remove")
                    {
                        Member newMember = new Member();

                        for (int i = 0; i < members.Count; i++)
                        {
                            if (members[i].UserName == member)
                            {
                                members.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    else if (type == "update")
                    {
                        foreach (var m in members)
                        {
                            if (m.UserName == member)
                            {
                                m.Role = role;
                                break;
                            }
                        }
                    }
                    project.Members = members;

                    session.SaveChanges();
                    return project;
                } else
                {
                    return null;
                }
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