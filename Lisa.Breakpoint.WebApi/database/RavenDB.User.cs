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
        public IList<User> GetAllUsers()
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Query<User>().ToList();
            }
        }

        public IList<Group> GetAllGroups()
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Query<Group>().ToList();
            }
        }

        public Group PostGroup(Group group)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                if (session.Query<Group>().Where(g => g.Name == group.Name).ToList().Count == 0)
                {
                    session.Store(group);
                    session.SaveChanges();

                    return group;
                }
                else
                {
                    return null;
                }
            }
        }

        public User GetUser(string userName)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Query<User>()
                    .Where(u => u.Username == userName)
                    .SingleOrDefault();
            }
        }

        public string GetGroupFromUser(string organization, string projectslug, string userName)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                var project = session.Query<Project>()
                    .Where(p => p.Organization == organization && p.Slug == projectslug && p.Members.Any(m => m.UserName == userName))
                    .SingleOrDefault();

                if (project != null)
                {
                    return project.Members
                        .Where(m => m.UserName == userName)
                        .SingleOrDefault().Role;
                } else
                {
                    return "no group";
                }
            }
        }

        public User UserExists(string userName)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                var user = session.Query<User>()
                    .Where(u => u.Username == userName)
                    .ToList();

                if (user.Count != 0)
                {
                    return user.First();
                }
                return null;
            }
        }

        public User PostUser(User user)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                if (session.Query<User>().Where(u => u.Username == user.Username).ToList().Count == 0)
                {
                    session.Store(user);
                    session.SaveChanges();

                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public User PatchUser(int id, User patchedUser)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                User user = session.Load<User>(id);
                foreach (PropertyInfo propertyInfo in user.GetType().GetProperties())
                {
                    var newVal = patchedUser.GetType().GetProperty(propertyInfo.Name).GetValue(patchedUser, null);
                    if (newVal != null)
                    {
                        var patchRequest = new PatchRequest()
                        {
                            Name = propertyInfo.Name,
                            Type = PatchCommandType.Set,
                            Value = newVal.ToString()
                        };
                        documentStore.DatabaseCommands.Patch("users/" + id, new[] { patchRequest });
                    }
                }
                return user;
            }
        }

        public void DeleteUser(int id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                User organization = session.Load<User>(id);
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}