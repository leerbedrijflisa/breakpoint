using Lisa.Breakpoint.WebApi.database;
using Lisa.Breakpoint.WebApi.models;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;

namespace Lisa.Breakpoint.WebApi.controllers
{
    [Route("users")]
    public class UserController
    {
        private readonly RavenDB _db;

        public UserController(RavenDB db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("insert")]
        public User Insert([FromBody] User user)
        {
            return _db.PostUser(user);
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

            //return _db.PostUser(user);

        }
    }
}
