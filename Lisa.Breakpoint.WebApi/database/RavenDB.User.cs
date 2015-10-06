using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lisa.Breakpoint.WebApi.database
{
    public partial class RavenDB : DocumentStore
    {
        public IList<User> GetAllUsers()
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Query<User>().ToList();
            }
        }

        public User GetUser(int id)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                return session.Load<User>(id);
            }
        }

        public string GetGroupFromUser(string userName)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                var user = session.Query<User>()
                    .Where(u => u.Username == userName)
                    .ToList();

                return user[0].Role + "s";

            }
        }

        public User PostUser(User user)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                session.Store(user);
                string userId = user.Id;

                session.SaveChanges();

                return session.Load<User>(userId);
            }
        }

        public User PatchUser(int id, User patchedUser)
        {
            using (IDocumentSession session = documentStore.Initialize().OpenSession())
            {
                User user = session.Load<User>(id);

                try
                {
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
                catch (Exception)
                {
                    return null;
                }
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