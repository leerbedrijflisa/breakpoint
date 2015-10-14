using Lisa.Breakpoint.WebApi.database;
using Lisa.Breakpoint.WebApi.Models;
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

        [HttpGet]
        public IList<User> Get()
        {
            return _db.GetAllUsers();
        }

        [HttpPost]
        public User Post([FromBody] User user)
        {
            return _db.PostUser(user);
        }

        [HttpGet("groups")]
        public IList<Group> GetGroups()
        {
            return _db.GetAllGroups();
        }

        [HttpPost("groups")]
        public Group PostGroup([FromBody] Group group)
        {
            return _db.PostGroup(group);
        }

        [HttpGet("login/{userName}")]
        public IActionResult LogIn(string userName)
        {
            var user = _db.UserExists(userName);
            if (user != null)
            {
                return new HttpOkObjectResult(_db.UserExists(userName));
            }
            return new HttpNotFoundObjectResult(_db.UserExists(userName));
        }
    }
}
