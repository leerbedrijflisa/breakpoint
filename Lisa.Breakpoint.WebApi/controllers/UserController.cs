using Lisa.Breakpoint.WebApi.database;
using Lisa.Breakpoint.WebApi.Models;
using Microsoft.AspNet.Mvc;

namespace Lisa.Breakpoint.WebApi.controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        public UserController(RavenDB db)
        {
            _db = db;
        }

        [HttpGet("", Name = "users")]
        public IActionResult Get()
        {
            var users = _db.GetAllUsers();

            if (users == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(users);
        }

        [HttpGet("{organization}/{project}/{userName}")]
        public IActionResult GetGroupFromUser(string organization, string project, string userName)
        {
            var role = _db.GetGroupFromUser(organization, project, userName);

            if (role == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(role);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if (user == null)
            {
                return new BadRequestResult();

            }

            var postedUser = _db.PostUser(user);

            string location = Url.RouteUrl("users", new {  }, Request.Scheme);
            return new CreatedResult(location, postedUser);
        }

        [HttpGet("groups", Name = "groups")]
        public IActionResult GetGroups()
        {
            var groups = _db.GetAllGroups();

            if (groups == null)
            {
                return new HttpNotFoundResult();
            }

            return new HttpOkObjectResult(groups);
        }

        [HttpPost("groups")]
        public IActionResult PostGroup([FromBody] Group group)
        {
            if (group == null)
            {
                return new BadRequestResult();
            }

            var postedGroup = _db.PostGroup(group);

            string location = Url.RouteUrl("groups", new {  }, Request.Scheme);
            return new CreatedResult(location, postedGroup);
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

        private readonly RavenDB _db;
    }
}
