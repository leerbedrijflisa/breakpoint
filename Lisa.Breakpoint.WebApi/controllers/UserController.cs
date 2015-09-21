using Microsoft.AspNet.Mvc;

namespace Lisa.Breakpoint.WebApi.controllers
{
    [Route("user")]
    public class UserController
    {
        static RavenDB _db = new RavenDB();

        [HttpGet]
        [Route("insert")]
        public User insert()
        {
            User user = new User
            {
                Username = "Henk1",
                FullName = "HenkHenksen",
                Role = "User"
            };

            return _db.insertUser(user);
        }
    }
}
