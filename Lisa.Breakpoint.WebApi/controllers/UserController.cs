using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.controllers
{
    [Route("users")]
    public class UserController
    {
        static RavenDB _db = new RavenDB();

        [HttpPost]
        [Route("insert")]
        public User Insert([FromBody] User user)
        {
            return _db.InsertUser(user);
        }

        [HttpGet]
        [Route("get")]
        public IList<User> Get()
        {
            return _db.GetAllUsers();
        }

        [HttpGet]
        [Route("{id}")]
        public User Get(int id)
        {
            return _db.GetUser(id);
        }
    }
}
