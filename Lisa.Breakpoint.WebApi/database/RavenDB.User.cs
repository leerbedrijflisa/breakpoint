﻿using Raven.Abstractions.Data;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lisa.Breakpoint.WebApi
{
    public partial class RavenDB
    {
        public IList<User> getAllUsers()
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Query<User>().ToList();
            }
        }

        public User getUser(int id)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                return session.Load<User>(id);
            }
        }

        public User insertUser(User user)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                session.Store(user);
                string userId = user.Id;

                session.SaveChanges();

                return session.Load<User>(userId);
            }
        }

        public User patchUser(int id, User patchedUser)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
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
                            store.DatabaseCommands.Patch("users/" + id, new[] { patchRequest });
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

        public void deleteUser(int id)
        {
            IDocumentStore store = createDocumentStore();
            using (IDocumentSession session = store.Initialize().OpenSession())
            {
                User organization = session.Load<User>(id);
                session.Delete(organization);
                session.SaveChanges();
            }
        }
    }
}